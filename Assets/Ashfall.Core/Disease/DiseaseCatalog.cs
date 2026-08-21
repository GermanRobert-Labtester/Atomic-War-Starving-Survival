using System;
using System.Collections.Generic;

namespace Ashfall.Core.Disease
{
    /// <summary>
    /// Transmission vectors for the Disease Expansion. Matches the vector set
    /// the legacy Unity system used (water / air / blood) and adds the authored
    /// spore vector for contagious spore plumes (spore lung / blight strains).
    /// </summary>
    public enum DiseaseVector
    {
        Water = 0,
        Air = 1,
        Blood = 2,
        Spore = 3
    }

    /// <summary>Canonical vector strings — the exact values authored in disease_catalog.json.</summary>
    public static class DiseaseVectorNames
    {
        public const string Water = "water";
        public const string Air = "air";
        public const string Blood = "blood";
        public const string Spore = "spore";

        public static DiseaseVector Parse(string vector)
        {
            if (string.Equals(vector, Air, StringComparison.Ordinal)) return DiseaseVector.Air;
            if (string.Equals(vector, Blood, StringComparison.Ordinal)) return DiseaseVector.Blood;
            if (string.Equals(vector, Spore, StringComparison.Ordinal)) return DiseaseVector.Spore;
            return DiseaseVector.Water;
        }

        public static string Name(DiseaseVector vector)
        {
            switch (vector)
            {
                case DiseaseVector.Air: return Air;
                case DiseaseVector.Blood: return Blood;
                case DiseaseVector.Spore: return Spore;
                default: return Water;
            }
        }
    }

    /// <summary>
    /// One authored disease. All rules are typed fields so the runtime enforces
    /// them deterministically — no rules hidden in notes/tags. Data authority:
    /// Assets/StreamingAssets/Data/disease_catalog.json.
    /// </summary>
    [Serializable]
    public sealed class DiseaseDefinition
    {
        public string id = string.Empty;                 // disease_*
        public string display_name = string.Empty;       // human-readable
        public string vector = DiseaseVectorNames.Water; // water | air | blood | spore

        /// <summary>Chance (0..1) that an outcome roll resolves as death.</summary>
        public float lethality = 0f;

        /// <summary>Days a carrier is infected but not yet contagious.</summary>
        public int incubation_days = 0;

        /// <summary>Days a patient is sick before the outcome roll.</summary>
        public int illness_days = 1;

        /// <summary>Chance (0..1) per contagious patient per spread attempt to
        /// expose a candidate.</summary>
        public float infectivity = 0f;

        /// <summary>In-game days between spread attempts for the disease.</summary>
        public int spread_interval_days = 1;

        /// <summary>Maximum number of candidates exposed per spread attempt.</summary>
        public int spread_radius = 1;

        /// <summary>
        /// Exact item id whose possession/use neutralises the vector (the host
        /// consumes it when the player applies the protocol). water → clean_water,
        /// air/spore → gas_mask / hazmat_suit, blood → antibiotics.
        /// </summary>
        public string countermeasure_item_id = string.Empty;

        /// <summary>Player-facing protocol text (short, restrained).</summary>
        public string guidance = string.Empty;

        public string source_note = string.Empty;
    }

    /// <summary>Root shape of disease_catalog.json.</summary>
    [Serializable]
    public sealed class DiseaseCollectionFile
    {
        public int schema_version = DiseaseCatalog.SchemaVersion;
        public string collection_id = DiseaseCatalog.CollectionId;
        public List<DiseaseDefinition> diseases = new List<DiseaseDefinition>();
    }

    /// <summary>
    /// Static authored disease catalog. Mutable during load only; the runtime
    /// disease system reads it and never mutates it. Engine-agnostic; loaded via
    /// IFileIO + IJsonSerializer so both hosts read the same bytes.
    /// </summary>
    public sealed class DiseaseCatalog
    {
        public const string FileName = "disease_catalog.json";
        public const string CollectionId = "disease_catalog";
        public const int SchemaVersion = 1;

        public readonly List<string> Errors = new List<string>();
        public readonly List<DiseaseDefinition> Diseases = new List<DiseaseDefinition>();

        public int Count => Diseases.Count;
        public bool HasErrors => Errors.Count > 0;

        public void Add(DiseaseDefinition disease)
        {
            if (disease == null || string.IsNullOrEmpty(disease.id)) return;
            if (GetById(disease.id) != null) return; // duplicates rejected
            Diseases.Add(disease);
        }

        public DiseaseDefinition? GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Diseases.Count; i++)
            {
                var d = Diseases[i];
                if (d != null && string.Equals(d.id, id, StringComparison.Ordinal))
                    return d;
            }
            return null;
        }
    }

    /// <summary>
    /// Engine-agnostic loader for disease_catalog.json. Reads the exact
    /// snake_case schema authored in StreamingAssets/Data and reports schema /
    /// range errors on the catalog instead of throwing (hosts decide how to
    /// surface them).
    /// </summary>
    public static class DiseaseCatalogLoader
    {
        public static DiseaseCatalog Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            var catalog = new DiseaseCatalog();
            string path = files.Combine(dataDirectory, DiseaseCatalog.FileName);
            if (!files.FileExists(path))
            {
                catalog.Errors.Add("missing " + DiseaseCatalog.FileName + " in " + dataDirectory);
                return catalog;
            }

            DiseaseCollectionFile file;
            try
            {
                file = json.Deserialize<DiseaseCollectionFile>(files.ReadAllText(path)!);
            }
            catch (Exception e)
            {
                catalog.Errors.Add("disease_catalog.json parse failed: " + e.Message);
                return catalog;
            }

            if (file == null || file.diseases == null || file.diseases.Count == 0)
            {
                catalog.Errors.Add("disease_catalog.json carries no diseases");
                return catalog;
            }

            for (int i = 0; i < file.diseases.Count; i++)
            {
                var d = file.diseases[i];
                if (d == null) continue;

                if (string.IsNullOrEmpty(d.id))
                {
                    catalog.Errors.Add("disease_catalog.json[" + i + "]: missing id");
                    continue;
                }
                if (catalog.GetById(d.id) != null)
                {
                    catalog.Errors.Add("disease_catalog.json: duplicate disease id '" + d.id + "'");
                    continue;
                }
                if (d.lethality < 0f || d.lethality > 1f)
                {
                    catalog.Errors.Add("disease_catalog.json: '" + d.id + "' lethality outside 0..1");
                    continue;
                }
                if (d.infectivity < 0f || d.infectivity > 1f)
                {
                    catalog.Errors.Add("disease_catalog.json: '" + d.id + "' infectivity outside 0..1");
                    continue;
                }
                if (d.illness_days < 1)
                {
                    catalog.Errors.Add("disease_catalog.json: '" + d.id + "' illness_days must be >= 1");
                    continue;
                }
                if (d.spread_interval_days < 1)
                {
                    catalog.Errors.Add("disease_catalog.json: '" + d.id + "' spread_interval_days must be >= 1");
                    continue;
                }
                if (d.spread_radius < 1)
                {
                    catalog.Errors.Add("disease_catalog.json: '" + d.id + "' spread_radius must be >= 1");
                    continue;
                }

                // Normalise the vector enum; unknown/empty defaults to water so a
                // typo degrades safe (vector blocked only when its protocol is set).
                if (string.IsNullOrEmpty(d.vector))
                    d.vector = DiseaseVectorNames.Water;
                DiseaseVectorNames.Parse(d.vector);

                catalog.Add(d);
            }
            return catalog;
        }
    }
}