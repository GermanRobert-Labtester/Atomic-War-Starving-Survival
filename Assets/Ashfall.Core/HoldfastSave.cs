using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Cross-host save envelope for ASHFALL: THE HOLDFAST Sprint 1 ("Ice &amp; paper")
    /// and Sprint 2 ("Salt &amp; steam").
    /// Carries the S1 systems (IceRoadSystem + CensusClaimSystem), the S2
    /// BrineWaterSystem, and the sim day. Both hosts write this shape through the
    /// IJsonSerializer port, so a save written by Godot must load in the other host
    /// and vice versa. Spec: docs/expansions/expansion_the_holdfast_plan.md §8.3.
    /// </summary>
    [Serializable]
    public class HoldfastSave
    {
        public const int CurrentSaveVersion = 2;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public IceRoadSystemState iceRoad = new IceRoadSystemState();
        public CensusClaimSystemState census = new CensusClaimSystemState();
        public BrineWaterSystemState brineWater = new BrineWaterSystemState();

        /// <summary>
        /// Integrity hash over the other fields (SaveChecksum skips this slot by name).
        /// Empty until <see cref="HoldfastSaveCodec.Encode"/> or
        /// <see cref="HoldfastSaveCodec.Capture"/> stamps it.
        /// </summary>
        public string Checksum = "";
    }

    /// <summary>
    /// Frozen v1 envelope shape (Sprint 1 only, no brineWater). Kept so a v1 save
    /// written before the S2 upgrade can be validated and migrated forward. Do not
    /// add fields here — it must match what v1 wrote byte-for-byte in field set.
    /// </summary>
    [Serializable]
    public class HoldfastSaveV1
    {
        public int saveVersion = 1;
        public int simDay;
        public IceRoadSystemState iceRoad = new IceRoadSystemState();
        public CensusClaimSystemState census = new CensusClaimSystemState();
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
            IClock clock)
        {
            var save = new HoldfastSave
            {
                simDay = clock.Day,
                iceRoad = iceRoad.CaptureState(),
                census = census.CaptureState(),
                brineWater = brine != null ? brine.CaptureState() : new BrineWaterSystemState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static HoldfastSave Capture(
            IceRoadSystem iceRoad,
            CensusClaimSystem census,
            IClock clock)
        {
            return Capture(iceRoad, census, new BrineWaterSystem(), clock);
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
        /// foreign-format file), or a saveVersion this build cannot read. A v1 save
        /// (Sprint 1, no brine) is validated against the frozen v1 shape and migrated
        /// forward to v2: brine starts fresh, the version bumps, the checksum recomputes.
        /// </summary>
        public static HoldfastSave Decode(string jsonText, IJsonSerializer json)
        {
            HoldfastSave save;
            try
            {
                save = json.Deserialize<HoldfastSave>(jsonText);
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

            if (save == null)
                throw new InvalidOperationException("HoldfastSave: empty save payload.");

            // A checksumless envelope is not a valid save: the tamper guarantee is
            // void if deleting the field is enough to bypass validation.
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "HoldfastSave: save carries no checksum (truncated or tampered file).");

            if (save.saveVersion > HoldfastSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "HoldfastSave: saveVersion " + save.saveVersion
                    + " is newer than this build supports (" + HoldfastSave.CurrentSaveVersion + ").");
            if (save.saveVersion < 1)
                throw new InvalidOperationException(
                    "HoldfastSave: saveVersion " + save.saveVersion + " is not a valid version.");

            if (save.saveVersion == 1)
            {
                // v1 checksum was computed over the v1 field set (no brineWater).
                // Deserializing a v1 file leaves brineWater at default-init; validate
                // against the frozen v1 shape before trusting the rest.
                var legacy = new HoldfastSaveV1
                {
                    saveVersion = save.saveVersion,
                    simDay = save.simDay,
                    iceRoad = save.iceRoad,
                    census = save.census
                };
                string expectedV1 = SaveChecksum.Compute(legacy);
                if (!string.Equals(save.Checksum, expectedV1, StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        "HoldfastSave: v1 checksum mismatch (corrupt or foreign save).");

                save.saveVersion = HoldfastSave.CurrentSaveVersion;
                save.Checksum = SaveChecksum.Compute(save);
                return save;
            }

            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "HoldfastSave: checksum mismatch (corrupt or foreign save).");

            return save;
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
