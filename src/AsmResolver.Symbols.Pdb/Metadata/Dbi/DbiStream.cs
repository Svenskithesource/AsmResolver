using System.Collections.Generic;
using System.Threading;
using AsmResolver.IO;
using AsmResolver.PE.File.Headers;

namespace AsmResolver.Symbols.Pdb.Metadata.Dbi;

/// <summary>
/// Represents the DBI Stream (also known as the Debug Information stream).
/// </summary>
public class DbiStream : SegmentBase
{
    /// <summary>
    /// Gets the default fixed MSF stream index for the DBI stream.
    /// </summary>
    public const int StreamIndex = 3;

    private IList<ModuleDescriptor>? _modules;

    /// <summary>
    /// Gets or sets the version signature assigned to the DBI stream.
    /// </summary>
    /// <remarks>
    /// This value should always be -1 for valid PDB files.
    /// </remarks>
    public int VersionSignature
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// Gets or sets the version number of the DBI header.
    /// </summary>
    /// <remarks>
    /// Modern tooling only recognize the VC7.0 file format.
    /// </remarks>
    public DbiStreamVersion VersionHeader
    {
        get;
        set;
    } = DbiStreamVersion.V70;

    /// <summary>
    /// Gets or sets the number of times the DBI stream has been written.
    /// </summary>
    public uint Age
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// Gets or sets the MSF stream index of the Global Symbol Stream.
    /// </summary>
    public ushort GlobalStreamIndex
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a bitfield containing the major and minor version of the toolchain that was used to build the program.
    /// </summary>
    public ushort BuildNumber
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the MSF stream index of the Public Symbol Stream.
    /// </summary>
    public ushort PublicStreamIndex
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the version number of mspdbXXXX.dll that was used to produce this PDB file.
    /// </summary>
    public ushort PdbDllVersion
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the MSF stream index of the Symbol Record Stream.
    /// </summary>
    public ushort SymbolRecordStreamIndex
    {
        get;
        set;
    }

    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort PdbDllRbld
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the MSF stream index of the MFC type server.
    /// </summary>
    public uint MfcTypeServerIndex
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets attributes associated to the DBI stream.
    /// </summary>
    public DbiAttributes Attributes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the machine type the program was compiled for.
    /// </summary>
    public MachineType Machine
    {
        get;
        set;
    }

    /// <summary>
    /// Gets a collection of modules (object files) that were linked together into the program.
    /// </summary>
    public IList<ModuleDescriptor> Modules
    {
        get
        {
            if (_modules is null)
                Interlocked.CompareExchange(ref _modules, GetModules(), null);
            return _modules;
        }
    }

    /// <summary>
    /// Obtains the list of modules
    /// </summary>
    /// <returns></returns>
    protected virtual IList<ModuleDescriptor> GetModules() => new List<ModuleDescriptor>();

    /// <summary>
    /// Reads a single DBI stream from the provided input stream.
    /// </summary>
    /// <param name="reader">The input stream.</param>
    /// <returns>The parsed DBI stream.</returns>
    public static DbiStream FromReader(BinaryStreamReader reader) => new SerializedDbiStream(reader);

    /// <inheritdoc />
    public override uint GetPhysicalSize()
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public override void Write(IBinaryStreamWriter writer)
    {
        throw new System.NotImplementedException();
    }
}
