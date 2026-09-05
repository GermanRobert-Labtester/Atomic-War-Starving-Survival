using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    public static class GeothermalCatalogLoader
    {
        public const string CatalogFileName = "geothermal_drilling_depths.json";

        public static GeothermalDrillingCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(dataDir)) throw new ArgumentNullException(nameof(dataDir));
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            string path = Path.Combine(dataDir, CatalogFileName);
            if (!fileIO.FileExists(path)) return null;

            try
            {
                string json = fileIO.ReadAllText(path);
                return serializer.Deserialize<GeothermalDrillingCatalog>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load {CatalogFileName}: {ex.Message}", ex);
            }
        }
    }
}
