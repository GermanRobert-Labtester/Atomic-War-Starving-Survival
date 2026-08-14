using System;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Cross-host save roundtrip for Holdfast Sprint 1 + 2 (IceRoadSystem +
    /// CensusClaimSystem + BrineWaterSystem + sim day). The codec is
    /// engine-agnostic and runs here without Unity or Godot, which is the point:
    /// a save written by either host must load in the other, so the shape is
    /// verified against the core ports.
    /// </summary>
    public class HoldfastSaveTests
    {
        private static readonly IJsonSerializer Json = new SystemTextJsonSerializer();

        private static HoldfastSave MakeSave(int seed = 808)
        {
            var ice = new IceRoadSystem(seed);
            ice.Unlock(90);
            ice.NotifyClerkStarted();
            for (int d = 90; d <= 110; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -22f);

            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "elena_vasquez", "marcus_olejnik", "suki_tanaka" }, 100);
            census.HonourLevy();
            census.TickDaily(104);
            census.AdjustOfficeTrust(3f);

            var brine = new BrineWaterSystem();
            brine.UnlockSaltTrade();
            for (int d = 90; d < 95; d++)
                brine.TickDaily(d, WeatherKind.Clear, -12f, outfallShifted: false);
            brine.RepairWithResin(2);

            var clock = new SimClock(104);
            return HoldfastSaveCodec.Capture(ice, census, brine, clock);
        }

        [Fact]
        public void CaptureStampsChecksumAndVersion()
        {
            var save = MakeSave();
            Assert.Equal(HoldfastSave.CurrentSaveVersion, save.saveVersion);
            Assert.False(string.IsNullOrEmpty(save.Checksum));
            Assert.True(save.iceRoad.clerkStarted);
            Assert.True(save.iceRoad.expansionUnlocked);
            Assert.True(save.census.levyHonour);
            Assert.True(save.brineWater.unlocked);
            Assert.True(save.brineWater.saltTradeUnlocked);
            Assert.Equal(104, save.simDay);
        }

        [Fact]
        public void RoundtripPreservesGateCensusAndBrine()
        {
            var save = MakeSave();
            string text = HoldfastSaveCodec.Encode(save, Json);
            var loaded = HoldfastSaveCodec.Decode(text, Json);

            var ice = new IceRoadSystem(808);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var clock = new SimClock(1);
            HoldfastSaveCodec.Restore(loaded, ice, census, brine, clock);

            Assert.Equal(104, clock.Day);
            Assert.Equal(save.iceRoad.isOpen, ice.IsOpen);
            Assert.Equal(save.iceRoad.iceThicknessM, ice.IceThicknessM);
            Assert.Equal(save.iceRoad.windowDaysRemaining, ice.WindowDaysRemaining);
            Assert.Equal(save.iceRoad.windowsCompleted, ice.State.windowsCompleted);
            Assert.Equal(save.iceRoad.seedSalt, ice.State.seedSalt);
            Assert.Equal(save.census.levyHonour, census.LevyHonour);
            Assert.Equal(save.census.levyRefuse, census.LevyRefuse);
            Assert.Equal(save.census.officeTrust, census.State.officeTrust);
            Assert.Equal(save.census.ledger.Count, census.State.ledger.Count);
            Assert.Equal(save.brineWater.unlocked, brine.Unlocked);
            Assert.Equal(save.brineWater.saltTradeUnlocked, brine.State.saltTradeUnlocked);
            Assert.Equal(save.brineWater.membraneIntegrity, brine.MembraneIntegrity);
            Assert.Equal(save.brineWater.steamTripped, brine.SteamTripped);
        }

        [Fact]
        public void RestoreIsIdempotent()
        {
            var save = MakeSave();
            var ice = new IceRoadSystem(808);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var clock = new SimClock(1);

            HoldfastSaveCodec.Restore(save, ice, census, brine, clock);
            HoldfastSaveCodec.Restore(save, ice, census, brine, clock);

            Assert.Equal(save.simDay, clock.Day);
            Assert.Equal(save.iceRoad.iceThicknessM, ice.IceThicknessM);
            Assert.Equal(save.census.ledger.Count, census.State.ledger.Count);
            Assert.Equal(save.brineWater.membraneIntegrity, brine.MembraneIntegrity);
        }

        [Fact]
        public void TamperedSaveRejectedByChecksum()
        {
            var save = MakeSave();
            string text = HoldfastSaveCodec.Encode(save, Json);
            Assert.Contains("\"clerkStarted\":true", text);
            string tampered = text.Replace("\"clerkStarted\":true", "\"clerkStarted\":false");
            Assert.NotEqual(text, tampered);
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(tampered, Json));
        }

        [Fact]
        public void NewerSaveVersionRejected()
        {
            var save = MakeSave();
            save.saveVersion = HoldfastSave.CurrentSaveVersion + 1;
            string text = HoldfastSaveCodec.Encode(save, Json);
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(text, Json));
        }

        [Fact]
        public void EmptyPayloadRejected()
        {
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode("", Json));
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(null, Json));
        }

        [Fact]
        public void EncodeStampsChecksumWhenMissing()
        {
            var save = MakeSave();
            save.Checksum = "";
            string text = HoldfastSaveCodec.Encode(save, Json);
            Assert.False(string.IsNullOrEmpty(save.Checksum));
            var loaded = HoldfastSaveCodec.Decode(text, Json);
            Assert.Equal(save.Checksum, loaded.Checksum);
        }

        [Fact]
        public void ChecksumlessSaveRejected()
        {
            // Deleting the checksum field must not be a way past validation.
            // Encode always recomputes, so build the checksumless payload with the
            // raw serializer — the same bytes a tampered file would contain.
            var save = MakeSave();
            var stripped = Json.Deserialize<HoldfastSave>(HoldfastSaveCodec.Encode(save, Json));
            stripped.Checksum = "";
            string text = Json.Serialize(stripped);
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(text, Json));
        }

        [Fact]
        public void VersionZeroRejected()
        {
            var save = MakeSave();
            save.saveVersion = 0;
            string text = HoldfastSaveCodec.Encode(save, Json);
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(text, Json));
        }

        [Fact]
        public void MalformedJsonWrappedAsInvalidOperation()
        {
            Assert.Throws<InvalidOperationException>(
                () => HoldfastSaveCodec.Decode("{not json", Json));
        }

        [Fact]
        public void BareObjectPayloadRejected()
        {
            // "{}" deserializes to a version-1 envelope with no checksum: invalid.
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode("{}", Json));
        }

        [Fact]
        public void NullIceRoadStateRestoresFreshDefaults()
        {
            var save = MakeSave();
            save.iceRoad = null;
            string text = HoldfastSaveCodec.Encode(save, Json);
            var loaded = HoldfastSaveCodec.Decode(text, Json);

            var ice = new IceRoadSystem(808);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var clock = new SimClock(1);
            HoldfastSaveCodec.Restore(loaded, ice, census, brine, clock);

            Assert.False(ice.IsUnlocked);
            Assert.False(ice.IsOpen);
            Assert.Equal(0f, ice.IceThicknessM);
            Assert.Equal(IceRoadSystem.SystemId, ice.State.systemId);
            Assert.True(census.LevyHonour, "census half of the envelope still restores");
            Assert.True(brine.State.saltTradeUnlocked, "brine half of the envelope still restores");
        }

        [Fact]
        public void RestoreThenRecaptureProducesSameChecksum()
        {
            var save = MakeSave();
            string text = HoldfastSaveCodec.Encode(save, Json);
            var loaded = HoldfastSaveCodec.Decode(text, Json);

            var ice = new IceRoadSystem(808);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var clock = new SimClock(1);
            HoldfastSaveCodec.Restore(loaded, ice, census, brine, clock);

            var again = HoldfastSaveCodec.Capture(ice, census, brine, clock);
            Assert.Equal(save.Checksum, again.Checksum);
        }

        [Fact]
        public void ForeignFormattingAndNullStringNormalizationAccepted()
        {
            // Simulates the other host's writer: a null string persisted as "" and
            // re-indented text. SaveChecksum normalizes both, so Decode must accept.
            // UpsertLedger coerces nulls, so inject the null directly into the live
            // state (CaptureState clones it verbatim).
            var census = new CensusClaimSystem();
            census.UpsertLedger("sv_test_null", "Test Survivor", "dock hand", true);
            census.State.ledger[0].occupationGuess = null;

            var ice = new IceRoadSystem(808);
            var brine = new BrineWaterSystem();
            var clock = new SimClock(40);
            var save = HoldfastSaveCodec.Capture(ice, census, brine, clock);
            string text = HoldfastSaveCodec.Encode(save, Json);

            Assert.Contains("\"occupationGuess\":null", text);
            string foreign = text.Replace("\"occupationGuess\":null", "\"occupationGuess\":\"\"");
            var loaded = HoldfastSaveCodec.Decode(foreign, Json);
            Assert.Equal(save.Checksum, loaded.Checksum);
            Assert.Equal("", loaded.census.ledger[0].occupationGuess);
        }

        [Fact]
        public void V1SaveMigratesToV2()
        {
            // The frozen v1 envelope shape (Sprint 1, no brineWater). A checksum
            // over the v1 field set must validate, then the save migrates to v2
            // with a fresh default brine system.
            var v1 = new HoldfastSaveV1 { saveVersion = 1, simDay = 104 };
            var ice = new IceRoadSystem(808);
            ice.Unlock(90);
            ice.NotifyClerkStarted();
            v1.iceRoad = ice.CaptureState();
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "elena_vasquez", "marcus_olejnik", "suki_tanaka" }, 100);
            census.HonourLevy();
            v1.census = census.CaptureState();
            v1.Checksum = SaveChecksum.Compute(v1);
            string text = Json.Serialize(v1);

            var loaded = HoldfastSaveCodec.Decode(text, Json);
            Assert.Equal(HoldfastSave.CurrentSaveVersion, loaded.saveVersion);
            Assert.Equal(104, loaded.simDay);
            Assert.NotNull(loaded.brineWater);
            Assert.False(loaded.brineWater.unlocked, "migrated save starts brine fresh");
            Assert.Equal(72f, loaded.brineWater.membraneIntegrity);
            Assert.True(loaded.iceRoad.clerkStarted);
            Assert.True(loaded.census.levyHonour);

            // The migrated envelope is itself valid v2: re-encode and re-decode.
            var again = HoldfastSaveCodec.Decode(HoldfastSaveCodec.Encode(loaded, Json), Json);
            Assert.Equal(loaded.Checksum, again.Checksum);
        }

        [Fact]
        public void V1SaveWithTamperedChecksumRejected()
        {
            var v1 = new HoldfastSaveV1 { simDay = 90 };
            v1.Checksum = "deadbeef";
            string text = Json.Serialize(v1);
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(text, Json));
        }
    }
}
