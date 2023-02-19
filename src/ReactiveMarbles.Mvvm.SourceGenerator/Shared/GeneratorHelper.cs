// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Shared
{
    /// <summary>
    /// Helps source generation.
    /// </summary>
    public static class GeneratorHelper
    {
        /// <summary>
        /// Generate as value property.
        /// </summary>
        /// <param name="addFileNameAndSourceText">A callback to add source text.</param>
        /// <param name="addDiagnostic">A callback to add a diagnostic message.</param>
        /// <param name="compilation">The compilation.</param>
        /// <param name="invocations">The invocation expressions.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static void GenerateAsValueProperty(
            Action<string, string> addFileNameAndSourceText,
            Action<Diagnostic> addDiagnostic,
            Compilation compilation,
            IEnumerable<InvocationExpressionSyntax> invocations,
            CancellationToken cancellationToken = default)
        {
            var validInvocations = new List<(IMethodSymbol MethodSymbol, IFieldSymbol FieldSymbol)>();
            foreach (var invocation in invocations)
            {
                var semanticModel = compilation.GetSemanticModel(invocation.SyntaxTree);
                if (semanticModel.GetSymbolInfo(invocation).Symbol is not
                    IMethodSymbol methodSymbol)
                {
                    continue;
                }

                if (methodSymbol.Parameters.Length >= 3)
                {
                    continue;
                }

                if (!methodSymbol.Parameters[1].Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                        .StartsWith("global::System.IObservable"))
                {
                    continue;
                }

                var methodSymbolParameter = methodSymbol.Parameters[2];
                if (methodSymbolParameter.Type.SpecialType != SpecialType.System_String)
                {
                    continue;
                }

                var fieldName = GetFieldNameFromInvocation(invocation, semanticModel) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(fieldName))
                {
                    continue;
                }

                var fieldSymbol = GetFieldSymbol(invocation, semanticModel, fieldName);

                if (fieldSymbol is null)
                {
                    continue;
                }

                validInvocations.Add((methodSymbol, fieldSymbol));
            }

            foreach (var validInvocation in validInvocations
                         .GroupBy(
                             methodSymbol => methodSymbol.Parameters[0].Type,
                             new MethodToSymbolFullyQualifiedComparer()))
            {
                var generated = GenerateSourceCode(validInvocation, cancellationToken);
                addFileNameAndSourceText(validInvocation.Key.Name + ".g.cs", generated);
            }
        }

        /// <summary>
        /// Checks if the provided <see cref="SyntaxNode"/> is an AsValue node.
        /// </summary>
        /// <param name="syntax">The syntax node.</param>
        /// <returns>A value indicating whether the node is an as value node.</returns>
        public static bool IsAsValueNode(SyntaxNode syntax) => syntax is InvocationExpressionSyntax
        {
            Expression:
            MemberAccessExpressionSyntax { Name.Identifier.Text: "AsValue" } or
            MemberBindingExpressionSyntax { Name.Identifier.Text: "AsValue" }
        };

        private static string GenerateSourceCode(
            IEnumerable<(IMethodSymbol MethodSymbol, IFieldSymbol FieldSymbol)> validInvocations,
            CancellationToken cancellationToken)
        {
            foreach (var observableGrouping in validInvocations)
            {
                var propertyName = GetGeneratedPropertyName(observableGrouping.FieldSymbol);
                // TODO: [rlittlesii: February 18, 2023] Generate Code.
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the generated property name for an input field.
        /// </summary>
        /// <param name="fieldSymbol">The input <see cref="IFieldSymbol"/> instance to process.</param>
        /// <returns>The generated property name for <paramref name="fieldSymbol"/>.</returns>
        private static string GetGeneratedPropertyName(IFieldSymbol fieldSymbol)
        {
            string propertyName = fieldSymbol.Name;

            if (propertyName.StartsWith("m_"))
            {
                propertyName = propertyName.Substring(2);
            }
            else if (propertyName.StartsWith("_"))
            {
                propertyName = propertyName.TrimStart('_');
            }

            return $"{char.ToUpper(propertyName[0], CultureInfo.InvariantCulture)}{propertyName.Substring(1)}";
        }

        private static IFieldSymbol? GetFieldSymbol(
            InvocationExpressionSyntax invocationExpression,
            SemanticModel semanticModel,
            string fieldName)
        {
            var parent = invocationExpression.FirstAncestorOrSelf<MethodDeclarationSyntax>();
            if (parent == null)
            {
                return null;
            }

            var parentType = parent.FirstAncestorOrSelf<TypeDeclarationSyntax>();
            if (parentType == null)
            {
                return null;
            }

            var parentSymbol = semanticModel.GetDeclaredSymbol(parentType) as ITypeSymbol;

            if (parentSymbol is null)
            {
                return null;
            }

            if (parentSymbol.Name == fieldName)
            {
                return parentSymbol.GetMembers()
                                   .OfType<IFieldSymbol>()
                                   .FirstOrDefault(x => x.Name.Equals(fieldName, StringComparison.InvariantCulture));
            }

            return null;
        }

        private static string? GetFieldNameFromInvocation(
            InvocationExpressionSyntax invocationExpression,
            SemanticModel semanticModel)
        {
            return invocationExpression
                    .ArgumentList
                    .Arguments
                    .Where(
                        argumentSyntax => argumentSyntax.Expression is LiteralExpressionSyntax literal
                          && literal.IsKind(SyntaxKind.StringLiteralExpression))
                    .Select(argumentSyntax => semanticModel.GetConstantValue(argumentSyntax.Expression).Value?.ToString())
                    .FirstOrDefault();
        }
    }
}
