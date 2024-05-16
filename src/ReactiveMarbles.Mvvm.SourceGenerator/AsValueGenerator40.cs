// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveMarbles.Mvvm.SourceGenerator.Shared;
using static ReactiveMarbles.Mvvm.SourceGenerator.Shared.GeneratorHelper;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Roslyn40
{
    /// <summary>
    /// Generates AsValue extensions.
    /// </summary>
    [Generator(LanguageNames.CSharp)]
    public class AsValueGenerator40 : IIncrementalGenerator, ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            initContext.RegisterPostInitializationOutput(ctx =>
                ctx.AddSource("AsValue.g.cs", GeneratorConstants.AsValueExtensionsSource));
            var candidateInvocations =
                initContext.SyntaxProvider.CreateSyntaxProvider(
                    (syntax, _) => IsAsValueNode(syntax),
                    (syntax, _) => (InvocationExpressionSyntax)syntax.Node);
            var inputs = candidateInvocations.Collect()
                .Combine(initContext.CompilationProvider)
                .Select((combined, _) => (Candidates: combined.Left, Compilation: combined.Right));

            initContext.RegisterSourceOutput(
                inputs,
                (context, collectedValues) =>
                    GenerateInstances(context, collectedValues.Compilation, collectedValues.Candidates));
        }

        /// <inheritdoc/>
        void ISourceGenerator.Initialize(GeneratorInitializationContext context)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            throw new NotImplementedException();
        }

        private static void GenerateInstances(
            SourceProductionContext context,
            Compilation compilation,
            ImmutableArray<InvocationExpressionSyntax> candidates) =>
            GenerateAsValueProperty(
                (fileName, sourceText) => context.AddSource(fileName, sourceText),
                diagnostic => context.ReportDiagnostic(diagnostic),
                compilation,
                candidates,
                context.CancellationToken);
    }
}
