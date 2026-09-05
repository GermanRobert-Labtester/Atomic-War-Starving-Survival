// SPDX-License-Identifier: MIT
// ASHFALL Epilogue Chronicle Catalog & Loader (Plan 96).

using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Endgame
{
    /// <summary>
    /// Schema definition for a single slide entry in epilogue_chronicle.json.
    /// </summary>
    [Serializable]
    public sealed class EpilogueSlideDefinition
    {
        public int order { get; set; }
        public string title { get; set; } = string.Empty;
        public string art_asset_id { get; set; } = string.Empty;

        public EpilogueSlideDefinition() { }

        public EpilogueSlideDefinition(int order, string title, string artAssetId)
        {
            this.order = order;
            this.title = title ?? string.Empty;
            this.art_asset_id = artAssetId ?? string.Empty;
        }

        public EpilogueSlide ToSlide(string prose = "")
        {
            return new EpilogueSlide(order, title, prose, art_asset_id);
        }
    }

    /// <summary>
    /// Root envelope for epilogue_chronicle.json.
    /// </summary>
    [Serializable]
    public sealed class EpilogueChronicleCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<EpilogueSlideDefinition> default_slides { get; set; } = new List<EpilogueSlideDefinition>();
    }

    /// <summary>
    /// Engine-agnostic loader for the epilogue chronicle catalog.
    /// </summary>
    public static class EpilogueChronicleLoader
    {
        public const string DefaultFileName = "epilogue_chronicle.json";

        public static EpilogueChronicleCatalogData? Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(dataDir) || fileIO == null || serializer == null)
                return null;

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return null;

            try
            {
                string raw = fileIO.ReadAllText(path);
                return serializer.Deserialize<EpilogueChronicleCatalogData>(raw);
            }
            catch (Exception ex)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "EpilogueChronicleCatalogData", ex);
                return null;
            }
        }

        public static List<EpilogueSlide> LoadDefaultSlides(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var catalog = Load(dataDir, fileIO, serializer);
            var result = new List<EpilogueSlide>();
            if (catalog?.default_slides == null) return result;

            foreach (var def in catalog.default_slides)
            {
                if (def != null)
                {
                    result.Add(def.ToSlide());
                }
            }
            return result;
        }
    }
}
