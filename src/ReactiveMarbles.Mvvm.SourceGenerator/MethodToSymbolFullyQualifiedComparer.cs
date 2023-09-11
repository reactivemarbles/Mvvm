// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace ReactiveMarbles.Mvvm.SourceGenerator.Shared;

/// <summary>
/// Compare a symbol to the fully qualified name.
/// </summary>
public class MethodToSymbolFullyQualifiedComparer : IEqualityComparer<ISymbol>
{
    /// <inheritdoc />
    public bool Equals(ISymbol x, ISymbol y) => string.Equals(x.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), y.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), StringComparison.InvariantCulture);

    /// <inheritdoc/>
    public int GetHashCode(ISymbol obj)
    {
        return obj.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).GetHashCode();
    }
}
