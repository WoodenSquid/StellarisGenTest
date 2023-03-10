using System.Collections.Generic;

namespace CWToolsHelpers.FileParsing
{
    public interface ICWParserHelper
    {
        /// <summary>
        /// Convenience method to parse multiple paradox files using <see cref="ParseParadoxFile(System.String)"/>
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="continueOnFailure">Whether to log and continue if an exception is thrown in an individual file.  Defaults to false</param>
        /// <returns>A <see cref="IDictionary{TKey,TValue}"/> of file path -> CWNode representing the contents of the file</returns>
        IDictionary<string, CWNode> ParseParadoxFiles(IEnumerable<string> filePaths, bool continueOnFailure = false);

        /// <summary>
        /// Main method for using the CWTools library to parse an individual paradox file into an easier to use data structure.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>A CWNode representing the contents of the file.</returns>
        CWNode ParseParadoxFile(string filePath);
    }
}