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
