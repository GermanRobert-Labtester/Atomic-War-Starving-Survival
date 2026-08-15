using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Cross-host save envelope for ASHFALL: THE HOLDFAST Sprints 1-4
    /// ("Ice &amp; paper", "Salt &amp; steam", "Cluster &amp; claim", "Shelf &amp; endings").
    /// Carries the S1 systems (IceRoadSystem + CensusClaimSystem), the S2
    /// BrineWaterSystem, the S3 HoldfastQuestSystem snapshot, the S4 ending id,
    /// and the sim day. Both hosts write this shape through the IJsonSerializer
    /// port, so a save written by Godot must load in the other host and vice versa.
    /// Spec: docs/expansions/expansion_the_holdfast_plan.md §8.3.
    /// </summary>
    [Serializable]
    public class HoldfastSave
    {
        public const int CurrentSaveVersion = 4;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public IceRoadSystemState iceRoad = new IceRoadSystemState();
        public CensusClaimSystemState census = new CensusClaimSystemState();
        public BrineWaterSystemState brineWater = new BrineWaterSystemState();
        public HoldfastQuestSystemState quests = new HoldfastQuestSystemState();

        /// <summary>
        /// Integrity hash over the other fields (SaveChecksum skips this slot by name).
        /// Empty until <see cref="HoldfastSaveCodec.Encode"/> or
        /// <see cref="HoldfastSaveCodec.Capture"/> stamps it.
        /// </summary>
        public string Checksum = "";
    }

    /// <summary>
    /// Frozen v1 envelope shape (Sprint 1 only, no brineWater). Kept so a v1 save
    /// written before the S2 upgrade can be validated and migrated forward.
    /// Uses the frozen v1-v3 ice road DTO: any later drift in IceRoadSystemState
    /// must NOT invalidate older saves (see HoldfastSaveFrozen.cs).
    /// </summary>
    [Serializable]
    public class HoldfastSaveV1
    {
        public int saveVersion = 1;
        public int simDay;
        public IceRoadSystemStateV1toV3 iceRoad = new IceRoadSystemStateV1toV3();
        public CensusClaimSystemState census = new CensusClaimSystemState();
        public string Checksum = "";
    }

    /// <summary>
    /// Frozen v2 envelope shape (Sprints 1-2, no quest snapshot). Do not add
    /// fields here — it must match what v2 wrote byte-for-byte in field set.
    /// </summary>
    [Serializable]
    public class HoldfastSaveV2
    {
        public int saveVersion = 2;
        public int simDay;
        public IceRoadSystemStateV1toV3 iceRoad = new IceRoadSystemStateV1toV3();
        public CensusClaimSystemState census = new CensusClaimSystemState();
        public BrineWaterSystemState brineWater = new BrineWaterSystemState();
        public string Checksum = "";
    }

    /// <summary>
    /// Frozen v3 envelope shape (Sprints 1-3, no ending snapshot — the ending id
    /// rides inside HoldfastQuestSystemState, so v3 and v4 share the same JSON
    /// keys; the saveVersion field discriminates). Do not add fields here.
    /// </summary>
    [Serializable]
    public class HoldfastSaveV3
    {
        public int saveVersion = 3;
        public int simDay;
        public IceRoadSystemStateV1toV3 iceRoad = new IceRoadSystemStateV1toV3();
        public CensusClaimSystemState census = new CensusClaimSystemState();
        public BrineWaterSystemState brineWater = new BrineWaterSystemState();
        public HoldfastQuestSystemState quests = new HoldfastQuestSystemState();
        public string Checksum = "";
    }

    /// <summary>
    /// Serialization bridge between the S1 systems and the cross-host save envelope.
    /// No engine references: hosts provide IJsonSerializer + IClock. Verification
    /// lives in Ashfall.Core.Tests/HoldfastSaveTests.cs (plain dotnet test).
    /// </summary>
    public static class HoldfastSaveCodec
    {
        public static HoldfastSave Capture(
            IceRoadSystem iceRoad,
            CensusClaimSystem census,
            BrineWaterSystem brine,
            HoldfastQuestSystem quests,
            IClock clock)
        {
            var save = new HoldfastSave
            {
                simDay = clock.Day,
                iceRoad = iceRoad.CaptureState(),
                census = census.CaptureState(),
                brineWater = brine.CaptureState(),
                quests = quests.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static HoldfastSave Capture(
            IceRoadSystem iceRoad,
            CensusClaimSystem census,
            BrineWaterSystem brine,
            IClock clock)
        {
            return Capture(iceRoad, census, brine, new HoldfastQuestSystem(), clock);
        }

        public static HoldfastSave Capture(
            IceRoadSystem iceRoad,
            CensusClaimSystem census,
            IClock clock)
        {
            return Capture(iceRoad, census, new BrineWaterSystem(), new HoldfastQuestSystem(), clock);
        }

        public static string Encode(HoldfastSave save, IJsonSerializer json)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            // Always recompute: a caller may have mutated a captured save after
            // Capture() stamped it, and a stale checksum would poison the file.
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        /// <summary>
        /// Parses and validates. Throws <see cref="InvalidOperationException"/> on a
        /// malformed or empty payload, a missing or mismatched checksum (tamper or
        /// foreign-format file), or a saveVersion this build cannot read.
        /// A v1/v2/v3 save is validated against its FROZEN shape (the file is
        /// deserialized into that frozen type, so any field outside the declared
        /// version — e.g. injected brineWater in a v1 file — is dropped, never
        /// blessed) and migrated forward: systems added later start fresh, the
        /// version bumps, the checksum recomputes.
        /// </summary>
        public static HoldfastSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("HoldfastSave: empty save payload.");

            HoldfastSave migrated;
            try
            {
                // Probe by declared version: deserializing a newer file into an older
                // frozen type ignores unknown keys, and the saveVersion field tells us
                // which shape the file was actually written with.
                var v1 = json.Deserialize<HoldfastSaveV1>(jsonText);
                if (v1 != null && v1.saveVersion == 1)
                {
                    ValidateChecksum(v1.Checksum, v1, "v1");
                    return BuildCurrent(v1.simDay, v1.iceRoad, v1.census, null, null);
                }

                var v2 = json.Deserialize<HoldfastSaveV2>(jsonText);
                if (v2 != null && v2.saveVersion == 2)
                {
                    ValidateChecksum(v2.Checksum, v2, "v2");
                    return BuildCurrent(v2.simDay, v2.iceRoad, v2.census, v2.brineWater, null);
                }

                var v3 = json.Deserialize<HoldfastSaveV3>(jsonText);
                if (v3 != null && v3.saveVersion == 3)
                {
                    ValidateChecksum(v3.Checksum, v3, "v3");
                    return BuildCurrent(v3.simDay, v3.iceRoad, v3.census, v3.brineWater, v3.quests);
                }

                migrated = json.Deserialize<HoldfastSave>(jsonText);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "HoldfastSave: malformed save payload: " + e.Message, e);
            }

            if (migrated == null)
                throw new InvalidOperationException("HoldfastSave: empty save payload.");

            if (string.IsNullOrEmpty(migrated.Checksum))
                throw new InvalidOperationException(
                    "HoldfastSave: save carries no checksum (truncated or tampered file).");
            if (migrated.saveVersion > HoldfastSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "HoldfastSave: saveVersion " + migrated.saveVersion
                    + " is newer than this build supports (" + HoldfastSave.CurrentSaveVersion + ").");
            if (migrated.saveVersion < 1)
                throw new InvalidOperationException(
                    "HoldfastSave: saveVersion " + migrated.saveVersion + " is not a valid version.");

            ValidateChecksum(migrated.Checksum, migrated, "v" + HoldfastSave.CurrentSaveVersion);
            return migrated;
        }

        /// <summary>Builds a current-version envelope from frozen legacy fields and stamps it.</summary>
        private static HoldfastSave BuildCurrent(
            int simDay,
            IceRoadSystemStateV1toV3 iceRoad,
            CensusClaimSystemState census,
            BrineWaterSystemState brine,
            HoldfastQuestSystemState quests)
        {
            var save = new HoldfastSave
            {
                saveVersion = HoldfastSave.CurrentSaveVersion,
                simDay = simDay,
                iceRoad = (iceRoad ?? new IceRoadSystemStateV1toV3()).ToCurrent(),
                census = census ?? new CensusClaimSystemState(),
                brineWater = brine ?? new BrineWaterSystemState(),
                quests = quests ?? new HoldfastQuestSystemState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        private static void ValidateChecksum(string stored, object shape, string label)
        {
            string actual = SaveChecksum.Compute(shape);
            if (!string.Equals(stored, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "HoldfastSave: " + label + " checksum mismatch (corrupt or foreign save).");
        }

        /// <summary>Restores all systems and the sim clock. Idempotent.</summary>
        public static void Restore(
            HoldfastSave save,
            IceRoadSystem iceRoad,
            CensusClaimSystem census,
            BrineWaterSystem brine,
            HoldfastQuestSystem quests,
            IClock clock)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            iceRoad.RestoreState(save.iceRoad);
            census.RestoreState(save.census);
            brine?.RestoreState(save.brineWater);
            quests?.RestoreState(save.quests);
            clock.SetDay(save.simDay);
        }

        /// <summary>Restores all systems and the sim clock. Idempotent.</summary>
        public static void Restore(
            HoldfastSave save,
            IceRoadSystem iceRoad,
            CensusClaimSystem census,
            BrineWaterSystem brine,
            IClock clock)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            iceRoad.RestoreState(save.iceRoad);
            census.RestoreState(save.census);
            brine?.RestoreState(save.brineWater);
            clock.SetDay(save.simDay);
        }

        public static void Restore(
            HoldfastSave save,
            IceRoadSystem iceRoad,
            CensusClaimSystem census,
            IClock clock)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            iceRoad.RestoreState(save.iceRoad);
            census.RestoreState(save.census);
            clock.SetDay(save.simDay);
        }
    }
}
