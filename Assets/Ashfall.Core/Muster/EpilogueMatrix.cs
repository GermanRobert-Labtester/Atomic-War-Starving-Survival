using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>One Section XII epilogue-matrix outcome (muster_epilogues.json).</summary>
    public class EndingDefinition
    {
        public string endingKey = string.Empty;
        public string title = string.Empty;
        public string prose = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic loader for muster_epilogues.json — the eight Day-360
    /// outcomes (Section XII). MusterSystem resolves ending keys at approach
    /// time; this catalog supplies the prose those keys name.
    /// </summary>
    public static class EpilogueMatrixLoader
    {
        public const string FileName = "muster_epilogues.json";
        public const int CurrentSchemaVersion = 1;

        public static List<EndingDefinition> LoadEpilogues(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<EndingDefinition>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var root = json.Deserialize<EpilogueCatalogRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion)
                    return result;
                var entries = root.epilogues;
                if (entries == null) return result;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.ending_key)) continue;
                    result.Add(new EndingDefinition
                    {
                        endingKey = e.ending_key,
                        title = e.title ?? string.Empty,
                        prose = e.prose ?? string.Empty
                    });
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "EpilogueCatalogRoot", ex_CATDIAG);
                return result;
            }
            return result;
        }

        /// <summary>Schema-envelope root for muster_epilogues.json.</summary>
        private class EpilogueCatalogRoot
        {
            public int schema_version = 1;
            public List<EndingEntry> epilogues = new List<EndingEntry>();
        }

        private class EndingEntry
        {
            public string ending_key;
            public string title;
            public string prose;
        }
    }

    public enum FactionTerminalOutcome
    {
        None = 0,
        GarrisonAbsorbed,
        RebuildersJoined,
        Independent,
        FoundryAnnexed
    }

    public sealed class EpilogueMatrixInput
    {
        public bool ShelterFallen { get; set; }
        public bool WaterPlantHeld { get; set; }
        public bool GrainSiloCaptured { get; set; }
        public bool FuelDepotBurned { get; set; }
        public bool MercyPattern { get; set; }
        public bool IronPattern { get; set; }
        public bool DiplomacyPattern { get; set; }
        public string VerdictEndingKey { get; set; } = string.Empty;
        public string MusterEndingKey { get; set; } = string.Empty;
        public FactionTerminalOutcome FactionOutcome { get; set; } = FactionTerminalOutcome.None;
    }

    public static class EpilogueMatrix
    {
        public const string TheOpenMuster = "the_open_muster";
        public const string TheAmnesty = "the_amnesty";
        public const string TheCorridor = "the_corridor";
        public const string TheBloodPrice = "the_blood_price";
        public const string TheRateCardRevised = "the_rate_card_revised";
        public const string TheAdministrator = "the_administrator";
        public const string TheMeasuredTruthContested = "the_measured_truth_contested";
        public const string TheMeasuredTruth = "the_measured_truth";
        public const string Unwritten = "unwritten";
        public const string VerdictSectorRecounts = "ending_verdict_the_sector_recounts";
        public const string VerdictCountHeld = "ending_verdict_the_count_is_held";
        public const string VerdictOfferLease = "ending_verdict_the_offer_is_a_lease";

        // Faction (4)
        public const string GarrisonAbsorbsCoalition = "ending_garrison_absorbs_coalition";
        public const string RebuildersJoined = "ending_rebuilders_joined";
        public const string CoalitionIndependent = "ending_coalition_independent";
        public const string FoundryAnnexation = "ending_foundry_annexation";

        // Resource (3)
        public const string WaterPlantHeld = "ending_water_plant_held";
        public const string GrainSiloCaptured = "ending_grain_silo_captured";
        public const string FuelDepotBurned = "ending_fuel_depot_burned";

        // Moral (3)
        public const string MercyRoad = "ending_mercy_road";
        public const string IronWay = "ending_iron_way";
        public const string ListenersThread = "ending_listeners_thread";

        // Compound (2)
        public const string MercyWaterHeld = "ending_mercy_water_held";
        public const string IronFuelAsh = "ending_iron_fuel_ash";

        // Failure (1)
        public const string ShelterFalls = "ending_shelter_falls";

        public static readonly string[] AllKeys =
        {
            TheOpenMuster,
            TheAmnesty,
            TheCorridor,
            TheBloodPrice,
            TheRateCardRevised,
            TheAdministrator,
            TheMeasuredTruthContested,
            TheMeasuredTruth,
            Unwritten,
            VerdictSectorRecounts,
            VerdictCountHeld,
            VerdictOfferLease,
            GarrisonAbsorbsCoalition,
            RebuildersJoined,
            CoalitionIndependent,
            FoundryAnnexation,
            WaterPlantHeld,
            GrainSiloCaptured,
            FuelDepotBurned,
            MercyRoad,
            IronWay,
            ListenersThread,
            MercyWaterHeld,
            IronFuelAsh,
            ShelterFalls
        };

        public static string Evaluate(EpilogueMatrixInput? input)
        {
            if (input == null) return Unwritten;

            // 1. Terminal Failure / Collapse
            if (input.ShelterFallen) return ShelterFalls;

            // 2. Specific Compound Endings
            if (input.MercyPattern && input.WaterPlantHeld) return MercyWaterHeld;
            if (input.IronPattern && input.FuelDepotBurned) return IronFuelAsh;

            // 3. Verdict Specific Outcome
            if (!string.IsNullOrEmpty(input.VerdictEndingKey)) return input.VerdictEndingKey;

            // 4. Muster Specific Approach
            if (!string.IsNullOrEmpty(input.MusterEndingKey)) return input.MusterEndingKey;

            // 5. Faction Terminal Outcome
            switch (input.FactionOutcome)
            {
                case FactionTerminalOutcome.GarrisonAbsorbed:
                    return GarrisonAbsorbsCoalition;
                case FactionTerminalOutcome.RebuildersJoined:
                    return RebuildersJoined;
                case FactionTerminalOutcome.Independent:
                    return CoalitionIndependent;
                case FactionTerminalOutcome.FoundryAnnexed:
                    return FoundryAnnexation;
            }

            // 6. Strategic Resource Outcome
            if (input.WaterPlantHeld) return WaterPlantHeld;
            if (input.GrainSiloCaptured) return GrainSiloCaptured;
            if (input.FuelDepotBurned) return FuelDepotBurned;

            // 7. Moral Pattern Outcome
            if (input.MercyPattern) return MercyRoad;
            if (input.IronPattern) return IronWay;
            if (input.DiplomacyPattern) return ListenersThread;

            // 8. Fallback / Uninvestigated
            return Unwritten;
        }
    }
}
