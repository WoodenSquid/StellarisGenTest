using CWTools.Common;
using CWToolsHelpers.Directories;
using CWToolsHelpers.FileParsing;
using CWToolsHelpers.Localisation;
using CWToolsHelpers.ScriptedVariables;
using Microsoft.AspNetCore.Mvc;
using NetExtensions.Collection;

namespace StellarisGenTest.Controllers;

[ApiController]
[Route("[controller]")]

public class EthicsClassController : ControllerBase
{
    private readonly ILogger<EthicsClassController> _logger;
    private static List<EthicsClass> _results;
    public EthicsClassController(ILogger<EthicsClassController> logger, DataStructure config)
    {
        _logger = logger;
        _results = config.EthicsClass;
        //_results.ForEach(x => _logger.Log(LogLevel.Information, x.Name));
        //_results.ForEach(x => Console.WriteLine(x.Name));
    }
    
    [HttpGet]
    public IEnumerable<EthicsClass> Get()
    {
        //Program.list
        // var stellarisDirectoryHelper = new StellarisDirectoryHelper(@"C:\Program Files (x86)\Steam\steamapps\common\Stellaris");
        // var localisationApiHelper = new LocalisationApiHelper(stellarisDirectoryHelper, STLLang.English);
        // var scriptedVariableAccessor = new ScriptedVariableAccessor(stellarisDirectoryHelper);
        // var cwParserHelper = new CWParserHelper(scriptedVariableAccessor);
        //
        // List<FileInfo> traitFiles = DirectoryWalker.FindFilesInDirectoryTree(stellarisDirectoryHelper.Traits, StellarisDirectoryHelper.TextMask);
        // IDictionary<string, CWNode> parsedTraitFiles = cwParserHelper.ParseParadoxFiles(traitFiles.Select(x => x.FullName).ToList());
        // var results = new List<CwTest>();
        // foreach (var fileAndContents in parsedTraitFiles)
        // {
        //     foreach (var traitNode in fileAndContents.Value.Nodes)
        //     {
        //         _logger.Log(LogLevel.Information, traitNode.Key);
        //         //Console.WriteLine(traitNode.Key);
        //         var r = new CwTest
        //         {
        //             Name = traitNode.Key,
        //             Test = string.Join(", ", traitNode.KeyValues.Select(x => x.Key).ToList().Concat(traitNode.Nodes.Select(x => x.Key).ToList()).ToList())
        //             //NextLayer = traitNode.KeyValues.Select(x => x.Key).ToList().Concat(traitNode.Nodes.Select(x => x.Key).ToList()).ToList()
        //         };
        //         results.Add(r);
        //     }
        // }
        // return Enumerable.Range(1, 5).Select(index => new CwTest
        //     {
        //         Name = "gggg",
        //         Test = "aa"
        //         //NextLayer = new List<string>().Append("TT").ToList()
        //     })
        //     .ToArray();
        return _results.ToArray();
        
        
    }
}

