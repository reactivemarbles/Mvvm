// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveMarbles.PropertyChanged;

namespace ReactiveMarbles.Mvvm.Tests;

/// <summary>
/// Test the value thing.
/// </summary>
public class AsValueTestObject : RxObject
{
    private readonly ValueBinder<string?> _fullName;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsValueTestObject"/> class.
    /// </summary>
    public AsValueTestObject()
    {
        _fullName =
            this.WhenChanged(
                    x => x.FirstName,
                    x => x.LastName,
                    (first, last) => first + last)
            .AsValue(onChanged: _ => RaisePropertyChanged(nameof(FullName)));
    }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName
    {
        get => _firstName;
        set => RaiseAndSetIfChanged(ref _firstName, value);
    }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName
    {
        get => _lastName;
        set => RaiseAndSetIfChanged(ref _lastName, value);
    }

    /// <summary>
    /// Gets the full name.
    /// </summary>
    public string? FullName => _fullName.Value;
}
