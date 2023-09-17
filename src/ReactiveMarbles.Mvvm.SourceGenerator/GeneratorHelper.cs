// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;
using System.Runtime.CompilerServices;
using ReactiveMarbles.RoslynHelpers;
using static ReactiveMarbles.RoslynHelpers.SyntaxFactoryHelpers;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Shared
{
    /// <summary>
    /// Helps source generation.
    /// </summary>
    public static class GeneratorHelper
    {
        private static readonly ParameterSyntax FieldNameParameter;
        private static readonly ParameterSyntax CallerMemberNameParameter;
        private static readonly ParameterSyntax CallerMemberLineNumberParameter;
        private static readonly ParameterSyntax CallerMemberArgumentExpressionParameter;
        private static readonly SyntaxKind[] MethodModifier;
        private static readonly SyntaxKind[] ClassModifier;

        static GeneratorHelper()
        {
            FieldNameParameter = Parameter(StringType(), "fieldName");
            CallerMemberNameParameter = Parameter(new[] { AttributeList(Attribute("global::System.Runtime.CompilerServices.CallerMemberName")) }, StringType(), "callerMemberName", EqualsValueClause(NullLiteral()));
            CallerMemberLineNumberParameter = Parameter(new[] { AttributeList(Attribute("global::System.Runtime.CompilerServices.CallerLineNumber")) }, IntegerType(), "callerLineNumber", EqualsValueClause(LiteralExpression(0)));
            CallerMemberArgumentExpressionParameter = Parameter(new[] { AttributeList(Attribute("global::System.Runtime.CompilerServices.CallerArgumentExpression", new[] { AttributeArgument(LiteralExpression("fieldName")) })) }, StringType(), "callerArgumentExpression", EqualsValueClause(NullLiteral()));
            MethodModifier = new[]
                             {
                                 SyntaxKind.PublicKeyword,
                                 SyntaxKind.PartialKeyword,
                                 SyntaxKind.StaticKeyword,
                             };
            ClassModifier = new SyntaxKind[]
                            {
                                SyntaxKind.InternalKeyword,
                                SyntaxKind.PartialKeyword,
                                SyntaxKind.StaticKeyword,
                            };
        }

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
                var symbol = semanticModel.GetSymbolInfo(invocation).Symbol;
                if (symbol is not
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
                             methodSymbol => methodSymbol.MethodSymbol.Parameters[0].Type,
                             new MethodToSymbolFullyQualifiedComparer()))
            {
                var generated = "#pragma warning disable \n" + GenerateSourceCode(validInvocation, cancellationToken);
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
            var asValueMethodList = new List<MethodDeclarationSyntax>();
            foreach (var observableGrouping in validInvocations)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var propertyName = GetGeneratedPropertyName(observableGrouping.FieldSymbol);

                //// public partial static global::ReactiveMarbles.Mvvm.ValueBinder<T> AsValue<T, TObject>(this TObject sourceObject, global::System.IObservable<T> observable, string fieldName, [global::System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = null, [global::System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0, [global::System.Runtime.CompilerServices.CallerArgumentExpression(nameof(variableName)] string callerArgumentExpression = null)
                ////     where TObject : global::System.ComponentModel.INotifyPropertyChanged
                //// {
                ////     return default!;
                //// }

                asValueMethodList.Add(GenerateMethod(observableGrouping));

                // TODO: [rlittlesii: February 18, 2023] Generate Code.
            }
            //// internal partial static class AsValueExtensions

            var classDeclaration = ClassDeclaration(
                "AsValueGeneratedExtensions",
                ClassModifier,
                asValueMethodList,
                0);

            return CompilationUnit(
                    null,
                    new[]
                    {
                        classDeclaration,
                    },
                    null)
                .ToFullString();
        }

        private static MethodDeclarationSyntax GenerateMethod(
            (IMethodSymbol MethodSymbol, IFieldSymbol FieldSymbol) observableGrouping)
        {
            var fieldTypeString =
                observableGrouping.FieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sourceParameter = Parameter(
                fieldTypeString,
                "sourceObject",
                new[]
                {
                    SyntaxKind.ThisKeyword,
                });

            var observableType = IdentifierName(fieldTypeString)
                .GenerateObservableType();
            var observableParameter = Parameter(observableType, "observable");
            var returnType = GenericName(
                "global::ReactiveMarbles.Mvvm.ValueBinder",
                new[]
                {
                    IdentifierName(fieldTypeString),
                });

            var block = Block(
                Array.Empty<StatementSyntax>(),
                2);
            return MethodDeclaration(
                null,
                MethodModifier,
                returnType,
                null,
                "AsValue",
                new[]
                {
                    sourceParameter,
                    observableParameter,
                    FieldNameParameter,
                    CallerMemberNameParameter,
                    CallerMemberLineNumberParameter,
                    CallerMemberArgumentExpressionParameter,
                },
                null,
                null,
                block,
                null,
                1);
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

        /// <summary>
        /// Gets the string type.
        /// </summary>
        /// <returns>The void type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PredefinedTypeSyntax StringType() => SyntaxFactory.PredefinedType(Token(SyntaxKind.StringKeyword));

        /// <summary>
        /// Gets the int type.
        /// </summary>
        /// <returns>The void type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PredefinedTypeSyntax IntegerType() => SyntaxFactory.PredefinedType(Token(SyntaxKind.IntKeyword));
    }
}
