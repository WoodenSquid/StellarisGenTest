using CWToolsHelpers.FileParsing;
using CWToolsHelpers.Localisation;

namespace StellarisGenTest;

public class SpeciesClass
{
    // public SpeciesClass(CWNode node, LocalisationApiHelper localisationApiHelper)
    // {
    //     
    //     Name = localisationApiHelper.GetName(node.Key);
    //     Archetype = node.GetKeyValue("archetype");
    // }

    public string? Name { get; set; }
    public string? Archetype { get; set; }
    public string? Playable { get; set; }

    // private static string GetAchetype()
    // {
    //     
    //     return "";
    // }
}