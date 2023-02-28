using CWTools.Common;
using CWToolsHelpers.Directories;
using CWToolsHelpers.FileParsing;
using CWToolsHelpers.Localisation;
using CWToolsHelpers.ScriptedVariables;
using Microsoft.Extensions.Options;
using NetExtensions.Collection;
using StellarisGenTest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//List<CwTest> list = new();

var stellarisDirectoryHelper = new StellarisDirectoryHelper(@"C:\Program Files (x86)\Steam\steamapps\common\Stellaris");
var localisationApiHelper = new LocalisationApiHelper(stellarisDirectoryHelper, STLLang.English);
var scriptedVariableAccessor = new ScriptedVariableAccessor(stellarisDirectoryHelper);
var cwParserHelper = new CWParserHelper(scriptedVariableAccessor);
        
List<FileInfo> traitFiles = DirectoryWalker.FindFilesInDirectoryTree(stellarisDirectoryHelper.Traits, StellarisDirectoryHelper.TextMask);
IDictionary<string, CWNode> parsedTraitFiles = cwParserHelper.ParseParadoxFiles(traitFiles.Select(x => x.FullName).ToList());
var results = new List<CwTest>();
foreach (var fileAndContents in parsedTraitFiles)
{
    foreach (var traitNode in fileAndContents.Value.Nodes)
    {
        //_logger.Log(LogLevel.Information, traitNode.Key);
        //Console.WriteLine(traitNode.Key);
        var r = new CwTest
        {
            Name = traitNode.Key,
            Test = string.Join(", ", traitNode.KeyValues.Select(x => x.Key).ToList().Concat(traitNode.Nodes.Select(x => x.Key).ToList()).ToList())
            //NextLayer = traitNode.KeyValues.Select(x => x.Key).ToList().Concat(traitNode.Nodes.Select(x => x.Key).ToList()).ToList()
        };
        results.Add(r);
    }
}
/*
 * 
 */
List<FileInfo> speciesClassFiles = DirectoryWalker.FindFilesInDirectoryTree(stellarisDirectoryHelper.SpeciesClasses, StellarisDirectoryHelper.TextMask);
IDictionary<string, CWNode> parsedSpeciesClassFiles = cwParserHelper.ParseParadoxFiles(speciesClassFiles.Select(x => x.FullName).ToList());


Action<DataStructure> data = (opt =>
{
    List<SpeciesClass> speciesClass = new();
    foreach (var file in parsedSpeciesClassFiles)
    {
        foreach (var node in file.Value.Nodes)
        {
            string? val = null;
            foreach (var n in node.Nodes)
            {
                
                if (n.Key == "playable")
                {
                    val = n.GetKeyValue("always");
                    //string h = "";
                }
            }
            speciesClass.Add(new SpeciesClass
            {
                Name = node.Key,
                Archetype = node.GetKeyValue("archetype"),
                Playable = val
                //Playable = node.Nodes.FirstOrDefault(x => x.Key == "playable").GetKeyValue("always")
            });
        }
    }
    //work Ethics
    // List<EthicsClass> ethicsClass = new();
    // foreach (var file in parsedEthicsClassFiles)
    // {
    //     foreach (var node in file.Value.Nodes)
    //     {
    //         // string? val = null;
    //         // foreach (var n in node.Nodes)
    //         // {
    //         //     
    //         //     if (n.Key == "playable")
    //         //     {
    //         //         val = n.GetKeyValue("always");
    //         //         //string h = "";
    //         //     }
    //         // }
    //         ethicsClass.Add(new EthicsClass
    //         {
    //             Name = node.Key,
    //             Cost = Convert.ToInt32(node.GetKeyValue("cost")),
    //             Category = node.GetKeyValue("category"),
    //             CategoryValue = Convert.ToInt32(node.GetKeyValue("category_value"))
    //         });
    //     }
    // }
    // opt.EthicsClass = ethicsClass;
    opt.Traits = results;
    opt.SpeciesClasses = speciesClass;
    //opt.SpeciesClasses = parsedSpeciesClassFiles.Select(speciesClassFile => speciesClassFile.Value.Nodes.Select(x => new SpeciesClass(x)));
});
builder.Services.Configure(data);
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<DataStructure>>().Value);
/*
 * 
 */
Action<Config> config = (opt =>
{
    opt.Traits = results;
});

builder.Services.Configure(config);
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<Config>>().Value);
/*
 *
 * 
 */

//Ethics
List<FileInfo> ethicsClassFiles = DirectoryWalker.FindFilesInDirectoryTree(stellarisDirectoryHelper.EthicsClasses, StellarisDirectoryHelper.TextMask);
IDictionary<string, CWNode> parsedEthicsClassFiles = cwParserHelper.ParseParadoxFiles(ethicsClassFiles.Select(x => x.FullName).ToList());
Action<DataStructure> ethicsData = (opt =>
{
    List<EthicsClass> ethicsClass = new();
    foreach (var file in parsedEthicsClassFiles)
    {
        foreach (var node in file.Value.Nodes)
        {
            // string? val = null;
            // foreach (var n in node.Nodes)
            // {
            //     
            //     if (n.Key == "playable")
            //     {
            //         val = n.GetKeyValue("always");
            //         //string h = "";
            //     }
            // }
            ethicsClass.Add(new EthicsClass
            {
                Name = node.Key,
                Cost = Convert.ToInt32(node.GetKeyValue("cost")),
                Category = node.GetKeyValue("category"),
                CategoryValue = Convert.ToInt32(node.GetKeyValue("category_value"))
            });
        }
    }
    opt.EthicsClass = ethicsClass;
    //opt.SpeciesClasses = parsedSpeciesClassFiles.Select(speciesClassFile => speciesClassFile.Value.Nodes.Select(x => new SpeciesClass(x)));
});
builder.Services.Configure(ethicsData);
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<DataStructure>>().Value);




/*
 * 
 */
// var stellarisDirectoryHelper = new StellarisDirectoryHelper(@"C:\Program Files (x86)\Steam\steamapps\common\Stellaris");
// var localisationApiHelper = new LocalisationApiHelper(stellarisDirectoryHelper, STLLang.English);
//
// // Numerous values in the game files make use of variables in the scriptedVariables folder.  e.g. @tier1cost1 for level 1 tech costs
// // The scripted variables accessor handles the converting of these into their actual values
// // If it cannot look up a value for a given request it returns the requesting key
// var scriptedVariableAccessor = new ScriptedVariableAccessor(stellarisDirectoryHelper);
//
//
// // Create the main parser helper
// // You do not need to assign a scripted variables helper if you don't want to resolve variables.
// var cwParserHelper = new CWParserHelper(scriptedVariableAccessor);
//
// // parse over all the tech files and convert each teach into a basic descriptive string
// // am storing each tech in a dictionary to allow the mods (which process after main) to override values in the core game.
// var result = new Dictionary<string, string>();
// // use DirectoryWalker to find all tech files to process.  Do not want to process the tier or category files.
// List<FileInfo> techFiles = DirectoryWalker.FindFilesInDirectoryTree(stellarisDirectoryHelper.Technology, StellarisDirectoryHelper.TextMask, new[] { "00_tier.txt", "00_category.txt" });
// List<FileInfo> traitFiles = DirectoryWalker.FindFilesInDirectoryTree(stellarisDirectoryHelper.Traits, StellarisDirectoryHelper.TextMask);
//
// // each node is the contents of an individual file, keyed by file name
// IDictionary<string, CWNode> parsedTechFiles = cwParserHelper.ParseParadoxFiles(techFiles.Select(x => x.FullName).ToList());
// IDictionary<string, CWNode> parsedTraitFiles = cwParserHelper.ParseParadoxFiles(traitFiles.Select(x => x.FullName).ToList());
//
// foreach (var fileAndContents in parsedTraitFiles)
// {
//     Console.WriteLine("--------------------------------------------------------");
//     Console.WriteLine(fileAndContents.Key);
//     foreach (CWNode traitNode in fileAndContents.Value.Nodes)
//     {
//         Console.WriteLine("****************************");
//         Console.WriteLine($"Trait: {traitNode.Key}");
//         traitNode.KeyValues.ForEach(x => Console.WriteLine(x.Key));
//     }
//     Console.WriteLine("--------------------------------------------------------");
// }
//
// foreach (var fileAndContents in parsedTechFiles)
// {
//     // top level nodes are files, so we process the immediate children of each file, which is the individual techs.
//     foreach (var techNode in fileAndContents.Value.Nodes)
//     {
//         techNode.KeyValues.ForEach(x => Console.WriteLine(x.Key));
//         var test = techNode.GetKeyValue("prerequisites");
//         // extract some info about the tech and put it in a string
//         string techString = "[" + techNode.GetKeyValue("area") + "]" +
//                             "(" + (techNode.GetKeyValue("tier") ?? "0") + ") " +
//                             localisationApiHelper.GetName(techNode.Key) + ": " +
//                             localisationApiHelper.GetDescription(techNode.Key) +
//                             " (from: " + fileAndContents.Key + ")";
//         result[techNode.Key] = techString;
//     }
// }
// // loop over and output
// result.Values.ForEach(Console.WriteLine);






builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

public class Config
{
    public List<CwTest>? Traits { get; set; }
}