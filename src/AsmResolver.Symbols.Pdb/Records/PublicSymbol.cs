namespace AsmResolver.Symbols.Pdb.Records;

/// <summary>
/// Represents a public symbol stored in a PDB symbol stream.
/// </summary>
public class PublicSymbol : CodeViewSymbol
{
    private readonly LazyVariable<Utf8String> _name;

    /// <summary>
    /// Initializes a new empty public symbol.
    /// </summary>
    protected PublicSymbol()
    {
        _name = new LazyVariable<Utf8String>(GetName);
    }

    /// <summary>
    /// Creates a new public symbol.
    /// </summary>
    /// <param name="segment">The segment index.</param>
    /// <param name="offset">The offset within the segment the symbol starts at.</param>
    /// <param name="name">The name of the symbol.</param>
    /// <param name="attributes">The attributes associated to the symbol.</param>
    public PublicSymbol(ushort segment, uint offset, Utf8String name, PublicSymbolAttributes attributes)
    {
        Segment = segment;
        Offset = offset;
        _name = new LazyVariable<Utf8String>(name);
        Attributes = attributes;
    }

    /// <inheritdoc />
    public override CodeViewSymbolType CodeViewSymbolType => CodeViewSymbolType.Pub32;

    /// <summary>
    /// Gets or sets the file segment index this symbol is located in.
    /// </summary>
    public ushort Segment
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the offset within the file that this symbol is defined at.
    /// </summary>
    public uint Offset
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets attributes associated to the public symbol.
    /// </summary>
    public PublicSymbolAttributes Attributes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the symbol is a code symbol.
    /// </summary>
    public bool IsCode
    {
        get => (Attributes & PublicSymbolAttributes.Code) != 0;
        set => Attributes = (Attributes & ~PublicSymbolAttributes.Code)
                            | (value ? PublicSymbolAttributes.Code : 0);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the symbol is a function symbol.
    /// </summary>
    public bool IsFunction
    {
        get => (Attributes & PublicSymbolAttributes.Function) != 0;
        set => Attributes = (Attributes & ~PublicSymbolAttributes.Function)
                            | (value ? PublicSymbolAttributes.Function : 0);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the symbol involves managed code.
    /// </summary>
    public bool IsManaged
    {
        get => (Attributes & PublicSymbolAttributes.Managed) != 0;
        set => Attributes = (Attributes & ~PublicSymbolAttributes.Managed)
                            | (value ? PublicSymbolAttributes.Managed : 0);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the symbol involves MSIL code.
    /// </summary>
    public bool IsMsil
    {
        get => (Attributes & PublicSymbolAttributes.Msil) != 0;
        set => Attributes = (Attributes & ~PublicSymbolAttributes.Msil)
                            | (value ? PublicSymbolAttributes.Msil : 0);
    }

    /// <summary>
    /// Gets or sets the name of the symbol.
    /// </summary>
    public Utf8String Name
    {
        get => _name.Value;
        set => _name.Value = value;
    }

    /// <summary>
    /// Obtains the name of the public symbol.
    /// </summary>
    /// <returns>The name.</returns>
    /// <remarks>
    /// This method is called upon initialization of the <see cref="Name"/> property.
    /// </remarks>
    protected virtual Utf8String GetName() => Utf8String.Empty;

    /// <inheritdoc />
    public override string ToString() => $"{CodeViewSymbolType}: [{Segment:X4}:{Offset:X8}] {Name}";
}
