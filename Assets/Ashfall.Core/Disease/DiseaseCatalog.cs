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
    /// <summary>
    /// Plan 60 / D3 — one authored treatment option for one disease, resolved by
    /// item id. Before this the disease engine had no intervention path at all:
    /// <c>ResolveOutcomes</c> rolled <c>def.lethality</c> regardless of anything the
    /// player did, and <c>countermeasure_item_id</c> only <em>blocks a vector</em>
    /// (prevention), so "treat this patient" was not a question the engine could
    /// answer. The role lives on the treatment entry, never on the caller, so no
    /// host can claim to be curative by asking politely.
    /// </summary>
    public sealed class DiseaseTreatment
    {
        /// <summary>Item id consumed when this treatment is applied.</summary>
        public string item_id = string.Empty;

        /// <summary>
        /// One of <see cref="DiseaseTreatmentRoles"/>: curative, suppressive,
        /// symptomatic, supportive. Unknown roles are rejected by the loader.
        /// </summary>
        public string role = DiseaseTreatmentRoles.Supportive;

        /// <summary>
        /// Latest day of illness on which this treatment still helps. 0 means "any
        /// time before the outcome roll"; a positive value is what makes treatment
        /// windows matter instead of being a save-anytime button.
        /// </summary>
        public int max_days = 0;

        /// <summary>
        /// Amount subtracted from the disease's lethality for this patient when this
        /// treatment is applied, 0..1. Cumulative reduction is capped by
        /// <c>DiseaseSystem.MaxLethalityReduction</c>.
        /// </summary>
        public float lethality_reduction = 0f;
    }

    /// <summary>Canonical treatment role strings, as authored in the catalog.</summary>
    public static class DiseaseTreatmentRoles
    {
        /// <summary>Removes the infection; the only role that can cure.</summary>
        public const string Curative = "curative";

        /// <summary>Delays/blunts progression — buys time, does not cure.</summary>
        public const string Suppressive = "suppressive";

        /// <summary>Eases presentation (fever, pain) without changing the outcome odds.</summary>
        public const string Symptomatic = "symptomatic";

        /// <summary>Comfort care: modest survival benefit, keeps the patient alive to be cared for.</summary>
        public const string Supportive = "supportive";

        public static bool IsKnown(string role) =>
            role == Curative || role == Suppressive || role == Symptomatic || role == Supportive;

        /// <summary>True when the role removes the infection rather than easing it.</summary>
        public static bool IsCurative(string role) =>
            string.Equals(role, Curative, StringComparison.OrdinalIgnoreCase);
    }

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

        /// <summary>
        /// Plan 60 / D2 — the clinical tell: the first thing a medic would notice and
        /// the thing that separates this illness from its neighbours. Authored prose in
        /// the same convention as <c>display_name</c>/<c>guidance</c>, so it renders on a
        /// surface instead of sitting unread in the catalog.
        /// </summary>
        public string tell = string.Empty;

        /// <summary>
        /// A second, corroborating sign. Deliberately allowed to overlap with another
        /// disease's tell: reading medicine is weighing several signs, not matching one
        /// exclusive key.
        /// </summary>
        public string tell_secondary = string.Empty;

        /// <summary>
        /// Timing clue — when the sign appears relative to exposure, which is how a
        /// player distinguishes a waterborne gut illness from a chest illness with a
        /// similar cough.
        /// </summary>
        public string timing_clue = string.Empty;

        public string source_note = string.Empty;

        /// <summary>
        /// Plan 60 / D3 — authored treatment options for this disease. Empty means
        /// "no treatment path is authorised", which is a legitimate clinical fact for
        /// an illness the holdfast can only endure. Schema-1 catalogs load as empty.
        /// </summary>
        public List<DiseaseTreatment> treatments = new List<DiseaseTreatment>();

        /// <summary>Find the authorised treatment entry for an item id, if any.</summary>
        public DiseaseTreatment? TreatmentFor(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || treatments == null) return null;
            for (int i = 0; i < treatments.Count; i++)
            {
                var t = treatments[i];
                if (t != null && string.Equals(t.item_id, itemId, StringComparison.Ordinal))
                    return t;
            }
            return null;
        }
    }

    /// <summary>Root shape of disease_catalog.json.</summary>
    [Serializable]
    public sealed class DiseaseCollectionFile
    {
        public int schema_version = DiseaseCatalog.SchemaVersion;
        public string collection_id = DiseaseCatalog.CollectionId;
        public List<DiseaseDefinition> diseases = new List<DiseaseDefinition>();

        /// <summary>Plan 60 / D4 — how long each vector protocol holds before it
        /// lapses. Absent (or zero) means the protocol holds until manually
        /// disengaged, which is the pre-D4 behaviour.</summary>
        public List<VectorProtocolFile> vector_protocols = new List<VectorProtocolFile>();
    }

    /// <summary>Authored maintenance window for one vector countermeasure.</summary>
    [Serializable]
    public sealed class VectorProtocolFile
    {
        public string vector = string.Empty;
        public int duration_days = 0;
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

        /// <summary>
        /// 1 → 2 (Plan 60 / D3 + D2): added <c>treatments[]</c> per disease and the
        /// clinical tell fields (<c>tell</c>, <c>tell_secondary</c>, <c>timing_clue</c>).
        /// Both are additive and optional — a version-1 file loads unchanged, with no
        /// authorised treatment and no tell, exactly as it behaved before.
        /// </summary>
        public const int SchemaVersion = 2;

        public readonly List<string> Errors = new List<string>();
        public readonly List<DiseaseDefinition> Diseases = new List<DiseaseDefinition>();

        /// <summary>Plan 60 / D4 — authored lapse durations per vector, in days.
        /// A protocol is maintenance, not a switch you flip once: purified water
        /// goes stale, seals get opened by work details, filters clog. Zero means
        /// the protocol holds until manually disengaged.</summary>
        public readonly List<VectorProtocolFile> VectorProtocols = new List<VectorProtocolFile>();

        public IReadOnlyList<DiseaseDefinition> All => Diseases;
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

        /// <summary>
        /// Plan 60 / D4 — authored lapse duration for a vector protocol, in days.
        /// Unknown vector or no authored entry returns 0 (holds until disengaged),
        /// so a version-1 catalog keeps the old never-lapse behaviour.
        /// </summary>
        public int ProtocolDurationDays(string vectorName)
        {
            if (string.IsNullOrEmpty(vectorName)) return 0;
            var canonical = DiseaseVectorNames.Name(DiseaseVectorNames.Parse(vectorName));
            for (int i = 0; i < VectorProtocols.Count; i++)
            {
                var p = VectorProtocols[i];
                if (p == null || string.IsNullOrEmpty(p.vector)) continue;
                if (string.Equals(DiseaseVectorNames.Name(DiseaseVectorNames.Parse(p.vector)), canonical,
                    StringComparison.OrdinalIgnoreCase))
                    return p.duration_days < 0 ? 0 : p.duration_days;
            }
            return 0;
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

                // Plan 60 / D3 — treatment entries are validated, not trusted. An
                // unknown role or an unbounded reduction would silently rewrite
                // lethality, so an unusable entry is dropped and reported instead.
                if (d.treatments != null && d.treatments.Count > 0)
                {
                    var keep = new List<DiseaseTreatment>(d.treatments.Count);
                    for (int t = 0; t < d.treatments.Count; t++)
                    {
                        var entry = d.treatments[t];
                        if (entry == null || string.IsNullOrEmpty(entry.item_id))
                        {
                            catalog.Errors.Add("disease_catalog.json: '" + d.id + "' treatment[" + t + "] missing item_id");
                            continue;
                        }
                        if (string.IsNullOrEmpty(entry.role)) entry.role = DiseaseTreatmentRoles.Supportive;
                        if (!DiseaseTreatmentRoles.IsKnown(entry.role))
                        {
                            catalog.Errors.Add("disease_catalog.json: '" + d.id + "' treatment '" + entry.item_id + "' has unknown role '" + entry.role + "'");
                            continue;
                        }
                        if (entry.lethality_reduction < 0f || entry.lethality_reduction > 1f)
                        {
                            catalog.Errors.Add("disease_catalog.json: '" + d.id + "' treatment '" + entry.item_id + "' lethality_reduction outside 0..1");
                            continue;
                        }
                        if (entry.max_days < 0)
                        {
                            catalog.Errors.Add("disease_catalog.json: '" + d.id + "' treatment '" + entry.item_id + "' max_days must be >= 0");
                            continue;
                        }
                        keep.Add(entry);
                    }
                    d.treatments = keep;
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

            // Plan 60 / D4 — vector protocol durations. Validated, not trusted: a
            // negative duration would mean "never lapses" by accident, so it is
            // rejected; an unknown vector name is rejected outright (a typo would
            // silently arm nothing, and the player would never learn why).
            if (file.vector_protocols != null)
            {
                for (int i = 0; i < file.vector_protocols.Count; i++)
                {
                    var p = file.vector_protocols[i];
                    if (p == null || string.IsNullOrEmpty(p.vector))
                    {
                        catalog.Errors.Add("disease_catalog.json: vector_protocols[" + i + "] missing vector");
                        continue;
                    }
                    if (p.duration_days < 0)
                    {
                        catalog.Errors.Add("disease_catalog.json: vector_protocols['" + p.vector + "'] duration_days must be >= 0");
                        continue;
                    }
                    catalog.VectorProtocols.Add(p);
                }
            }
            return catalog;
        }
    }
}
