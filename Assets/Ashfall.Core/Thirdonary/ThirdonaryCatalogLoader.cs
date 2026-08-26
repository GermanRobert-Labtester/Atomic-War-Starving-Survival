using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Thirdonary
{
    /// <summary>
    /// Loads thirdonary_quests.json into a list of ThirdonaryQuestDef.
    /// Follows the same pattern as ExpansionQuestCatalogLoader.
    /// </summary>
    public static class ThirdonaryCatalogLoader
    {
        public const string FileName = "thirdonary_quests.json";

        public static List<ThirdonaryQuestDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();

            var result = new List<ThirdonaryQuestDef>();

            if (string.IsNullOrEmpty(dataDir) || !fileIO.DirectoryExists(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            try
            {
                string json = fileIO.ReadAllText(path);
                if (string.IsNullOrEmpty(json)) return result;

                var root = serializer.Deserialize<ThirdonaryCatalogRoot>(json);
                if (root?.quests != null)
                    result.AddRange(root.quests);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "unknown", ex);
            }

            return result;
        }
    }
}
