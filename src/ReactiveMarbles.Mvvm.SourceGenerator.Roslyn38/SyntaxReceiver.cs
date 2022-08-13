// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using static ReactiveMarbles.Mvvm.SourceGenerator.Shared.GeneratorHelper;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Roslyn38;

/// <summary>
/// Receives the syntax.
/// </summary>
public class SyntaxReceiver : ISyntaxReceiver
{
    /// <summary>
    /// Gets the list of invocations.
    /// </summary>
    public List<InvocationExpressionSyntax> AsValueInvocations { get; } = new List<InvocationExpressionSyntax>();

    /// <inheritdoc/>
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (IsAsValueNode(syntaxNode) && syntaxNode is InvocationExpressionSyntax invocation)
        {
            AsValueInvocations.Add(invocation);
        }
    }
}
