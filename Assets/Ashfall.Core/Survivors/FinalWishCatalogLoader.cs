using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Engine-agnostic loader for authored final wishes from the JSON data authority
    /// (<c>final_wishes.json</c>). Mirrors the <see cref="FeedbackMessageCatalogLoader"/>
    /// shape: deserialize the <c>{ schema_version, items }</c> envelope, fall back to
    /// <see cref="CatalogLocator.LoadWrappedList{T}"/> for shape-tolerant parsing, and
    /// degrade to an empty catalog on any failure (the runtime system stays functional
    /// without authored text).
    /// </summary>
    public static class FinalWishCatalogLoader
    {
        public const string FileName = "final_wishes.json";

        public static FinalWishCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new FinalWishCatalog();

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return new FinalWishCatalog();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new FinalWishCatalog();

            List<FinalWishEntry> entries = new List<FinalWishEntry>();

            try
            {
                var container = json.Deserialize<FinalWishContainer>(raw);
                if (container != null && container.items != null && container.items.Count > 0)
                    entries = container.items;
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "<root>", ex);
            }

            // Shape-tolerant fallback: parse a bare array or a differently-wrapped object.
            if (entries.Count == 0)
            {
                try
                {
                    var list = CatalogLocator.LoadWrappedList<FinalWishEntry>(raw, SystemTextJsonSerializer.Options);
                    if (list != null && list.Count > 0)
                        entries = list;
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(FileName, "<root>", ex);
                }
            }

            var catalog = new FinalWishCatalog();
            foreach (var entry in entries)
                catalog.Add(entry);
            return catalog;
        }
    }
}
