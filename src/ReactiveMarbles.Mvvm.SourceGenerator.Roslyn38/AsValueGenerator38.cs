// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using static ReactiveMarbles.Mvvm.SourceGenerator.Shared.GeneratorConstants;
using static ReactiveMarbles.Mvvm.SourceGenerator.Shared.GeneratorHelper;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Roslyn38
{
    /// <summary>
    /// Generate the AsValue bits.
    /// </summary>
    [Generator]
    public class AsValueGenerator38 : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            // add the attribute text.
            context.AddSource("AsValueExtensions.SourceGenerated.cs", SourceText.From(AsValueExtensionsSource, Encoding.UTF8));

            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
            {
                return;
            }

            var compilation = context.Compilation;
            var options = (compilation as CSharpCompilation)?.SyntaxTrees[0].Options as CSharpParseOptions;
            compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(AsValueExtensionsSource, Encoding.UTF8), options));

            void AddText(string fileName, string text) => context.AddSource(fileName, SourceText.From(text, Encoding.UTF8));
            void ReportDiagnostic(Diagnostic diagnostic) => context.ReportDiagnostic(diagnostic);

            GenerateAsValueProperty(AddText, ReportDiagnostic, compilation, receiver.AsValueInvocations);
        }
    }
}
