// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Reactive Property Extensions.
/// </summary>
public static class ReactivePropertyMixins
{
    /// <summary>
    /// Set validation logic from DataAnnotations attributes.
    /// </summary>
    /// <typeparam name="T">Property type.</typeparam>
    /// <param name="self">Target ReactiveProperty.</param>
    /// <param name="selfSelector">The self selector.</param>
    /// <returns>
    /// Self.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// selfSelector
    /// or
    /// self.
    /// </exception>
    public static ReactiveProperty<T> AddValidation<T>(this ReactiveProperty<T> self, Expression<Func<ReactiveProperty<T>?>> selfSelector)
    {
        if (selfSelector == null)
        {
            throw new ArgumentNullException(nameof(selfSelector));
        }

        if (self == null)
        {
            throw new ArgumentNullException(nameof(self));
        }

        var memberExpression = (MemberExpression)selfSelector.Body;
        var propertyInfo = (PropertyInfo)memberExpression.Member;
        var display = propertyInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
        var attrs = propertyInfo.GetCustomAttributes<System.ComponentModel.DataAnnotations.ValidationAttribute>().ToArray();
        var context = new System.ComponentModel.DataAnnotations.ValidationContext(self)
        {
            DisplayName = display?.GetName() ?? propertyInfo.Name,
            MemberName = nameof(ReactiveProperty<T>.Value),
        };

        if (attrs.Length != 0)
        {
            self.AddValidationError(x =>
            {
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (System.ComponentModel.DataAnnotations.Validator.TryValidateValue(x!, context, validationResults, attrs))
                {
                    return null;
                }

                return validationResults[0].ErrorMessage;
            });
        }

        return self;
    }

    /// <summary>
    /// Create an IObservable instance to observe validation error messages of ReactiveProperty.
    /// </summary>
    /// <typeparam name="T">Property type.</typeparam>
    /// <param name="self">Target ReactiveProperty.</param>
    /// <returns>A IObservable of string.</returns>
    public static IObservable<string?> ObserveValidationErrors<T>(this ReactiveProperty<T> self)
    {
        if (self == null)
        {
            throw new ArgumentNullException(nameof(self));
        }

        return self.ObserveErrorChanged
            .Select(x => x?.OfType<string>()?.FirstOrDefault());
    }
}
