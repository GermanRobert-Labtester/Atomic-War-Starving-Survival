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
        public void RoundtripPreservesQuestProgress()
        {
            // v3: the quest snapshot (started/stage/branch) must survive the roundtrip.
            var ice = new IceRoadSystem(808);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var quests = new HoldfastQuestSystem();
            quests.BindCatalog(new[]
            {
                new HoldfastQuestEntry { id = "quest_holdfast_the_levy", display_name = "Levy" }
            });
            quests.State.drawerRead = true; // S3 story gate: the Office has shown the drawer
            Assert.True(quests.TryStart("quest_holdfast_the_levy", 100));
            Assert.True(quests.ChooseBranch("quest_holdfast_the_levy", CensusClaimSystem.FlagLevyRefuse));
            census.Activate12C();
            var clock = new SimClock(101);

            var save = HoldfastSaveCodec.Capture(ice, census, brine, quests, clock);
            var loaded = HoldfastSaveCodec.Decode(HoldfastSaveCodec.Encode(save, Json), Json);
            Assert.True(loaded.quests.quests.Count == 1);
            Assert.True(loaded.quests.quests[0].started);
            Assert.Equal(CensusClaimSystem.FlagLevyRefuse, loaded.quests.quests[0].branchId);

            var fresh = new HoldfastQuestSystem();
            var freshCensus = new CensusClaimSystem();
            HoldfastSaveCodec.Restore(loaded, new IceRoadSystem(808), freshCensus, new BrineWaterSystem(), fresh, new SimClock(1));
            Assert.True(fresh.IsStarted("quest_holdfast_the_levy"));
            Assert.True(fresh.HasRefuseBranch());
            Assert.True(freshCensus.Order12CActive);
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
        public void V1SaveMigratesToCurrent()
        {
            // The frozen v1 envelope shape (Sprint 1, no brineWater). A checksum
            // over the v1 field set must validate, then the save migrates forward
            // with fresh default brine + quest systems.
            var v1 = new HoldfastSaveV1 { saveVersion = 1, simDay = 104 };
            var ice = new IceRoadSystem(808);
            ice.Unlock(90);
            ice.NotifyClerkStarted();
            v1.iceRoad = IceRoadSystemStateV1toV3.From(ice.CaptureState());
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
            Assert.NotNull(loaded.quests);
            Assert.False(loaded.quests.sheetObtained, "migrated save starts quests fresh");
            Assert.Empty(loaded.quests.quests);
            Assert.True(loaded.iceRoad.clerkStarted);
            Assert.True(loaded.census.levyHonour);

            // The migrated envelope is itself valid: re-encode and re-decode.
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

        [Fact]
        public void V2SaveMigratesToCurrent()
        {
            // The frozen v2 envelope shape (Sprints 1-2, no quest snapshot).
            var v2 = new HoldfastSaveV2 { saveVersion = 2, simDay = 112 };
            var ice = new IceRoadSystem(808);
            ice.Unlock(90);
            ice.NotifyClerkStarted();
            v2.iceRoad = IceRoadSystemStateV1toV3.From(ice.CaptureState());
            var census = new CensusClaimSystem();
            census.Activate12C();
            v2.census = census.CaptureState();
            var brine = new BrineWaterSystem();
            brine.UnlockSaltTrade();
            v2.brineWater = brine.CaptureState();
            v2.Checksum = SaveChecksum.Compute(v2);
            string text = Json.Serialize(v2);

            var loaded = HoldfastSaveCodec.Decode(text, Json);
            Assert.Equal(HoldfastSave.CurrentSaveVersion, loaded.saveVersion);
            Assert.Equal(112, loaded.simDay);
            Assert.NotNull(loaded.quests);
            Assert.Empty(loaded.quests.quests);
            Assert.True(loaded.brineWater.saltTradeUnlocked, "v2 brine state kept");
            Assert.True(loaded.census.order12cActive, "v2 census state kept");

            var again = HoldfastSaveCodec.Decode(HoldfastSaveCodec.Encode(loaded, Json), Json);
            Assert.Equal(loaded.Checksum, again.Checksum);
        }

        [Fact]
        public void V2SaveWithTamperedChecksumRejected()
        {
            var v2 = new HoldfastSaveV2 { simDay = 90 };
            v2.Checksum = "deadbeef";
            string text = Json.Serialize(v2);
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(text, Json));
        }

        [Fact]
        public void V3SaveMigratesToCurrent()
        {
            // Frozen v3 shape (Sprints 1-3): quest snapshot present, ending id rides
            // inside it. v4 shares the JSON keys, so the version field discriminates.
            var v3 = new HoldfastSaveV3 { saveVersion = 3, simDay = 200 };
            var ice = new IceRoadSystem(808);
            ice.Unlock(90);
            ice.NotifyClerkStarted();
            v3.iceRoad = IceRoadSystemStateV1toV3.From(ice.CaptureState());
            var census = new CensusClaimSystem();
            census.Activate12C();
            v3.census = census.CaptureState();
            var brine = new BrineWaterSystem();
            brine.UnlockSaltTrade();
            v3.brineWater = brine.CaptureState();
            var quests = new HoldfastQuestSystem();
            quests.SetEnding(HoldfastEndings.White);
            v3.quests = quests.CaptureState();
            v3.Checksum = SaveChecksum.Compute(v3);
            string text = Json.Serialize(v3);

            var loaded = HoldfastSaveCodec.Decode(text, Json);
            Assert.Equal(HoldfastSave.CurrentSaveVersion, loaded.saveVersion);
            Assert.Equal(200, loaded.simDay);
            Assert.True(loaded.quests.endingId == HoldfastEndings.White, "ending survives v3 migration");
            Assert.True(loaded.census.order12cActive);

            var again = HoldfastSaveCodec.Decode(HoldfastSaveCodec.Encode(loaded, Json), Json);
            Assert.Equal(loaded.Checksum, again.Checksum);
        }

        [Fact]
        public void V3SaveWithTamperedChecksumRejected()
        {
            var v3 = new HoldfastSaveV3 { simDay = 90 };
            v3.Checksum = "deadbeef";
            string text = Json.Serialize(v3);
            Assert.Throws<InvalidOperationException>(() => HoldfastSaveCodec.Decode(text, Json));
        }

        [Fact]
        public void LegacySaveWithInjectedSystemStateDropsTheInjection()
        {
            // QA finding: a v1 file with an injected brineWater (a field the declared
            // version never had) must NOT be blessed by the migration. The codec
            // deserializes into the frozen v1 shape, which drops the extra key.
            var v1 = new HoldfastSaveV1 { saveVersion = 1, simDay = 90 };
            var ice = new IceRoadSystem(808);
            ice.Unlock(90);
            v1.iceRoad = IceRoadSystemStateV1toV3.From(ice.CaptureState());
            v1.Checksum = SaveChecksum.Compute(v1);
            string text = Json.Serialize(v1);

            // Inject a fully-populated brineWater node that v1 never had.
            var injected = new BrineWaterSystem();
            injected.Unlock();
            for (int d = 0; d < 20; d++)
                injected.TickDaily(d, WeatherKind.Blizzard, -20f, outfallShifted: false);
            string injectedJson = Json.Serialize(injected.CaptureState());
            text = text.Replace("\"census\":", "\"brineWater\":" + injectedJson + ",\"census\":");

            var loaded = HoldfastSaveCodec.Decode(text, Json);
            Assert.Equal(HoldfastSave.CurrentSaveVersion, loaded.saveVersion);
            Assert.False(loaded.brineWater.unlocked, "injected brine state must not survive migration");
            Assert.True(System.Math.Abs(loaded.brineWater.membraneIntegrity - 72f) < 0.01f, "brine stays at fresh defaults");
            Assert.True(loaded.iceRoad.expansionUnlocked, "legitimate v1 fields still migrate");
        }

        [Fact]
        public void WindowLengthOverrideSurvivesRoundtrip()
        {
            // QA finding: Second Winter's window cap is runtime state and was lost
            // on load. Now part of IceRoadSystemState.
            var ice = new IceRoadSystem(808);
            ice.ShortenWindowLength(5, 8, 42);
            Assert.True(ice.State.windowLengthOverride > 0, "override set on live state");

            var save = HoldfastSaveCodec.Capture(ice, new CensusClaimSystem(),
                new BrineWaterSystem(), new HoldfastQuestSystem(), new SimClock(1));
            var loaded = HoldfastSaveCodec.Decode(HoldfastSaveCodec.Encode(save, Json), Json);
            Assert.Equal(ice.State.windowLengthOverride, loaded.iceRoad.windowLengthOverride);

            var fresh = new IceRoadSystem(808);
            HoldfastSaveCodec.Restore(loaded, fresh, new CensusClaimSystem(),
                new BrineWaterSystem(), new HoldfastQuestSystem(), new SimClock(1));
            Assert.True(ice.State.windowLengthOverride == fresh.State.windowLengthOverride,
                "override restored onto the system");
        }

        [Fact]
        public void CensusRestoreDoesNotAliasTheEnvelope()
        {
            // QA finding: CensusClaimSystem.RestoreState shared the ledger list with
            // the decoded envelope. A mutation after restore must not corrupt the save.
            var save = MakeSave();
            var loaded = HoldfastSaveCodec.Decode(HoldfastSaveCodec.Encode(save, Json), Json);
            var census = new CensusClaimSystem();
            HoldfastSaveCodec.Restore(loaded, new IceRoadSystem(808), census,
                new BrineWaterSystem(), new HoldfastQuestSystem(), new SimClock(1));

            Assert.False(ReferenceEquals(loaded.census.ledger, census.State.ledger),
                "live ledger must not alias the envelope");

            census.UpsertLedger("sv_new_row", "New", "scrounger", false);
            Assert.DoesNotContain(loaded.census.ledger,
                entry => entry != null && entry.survivorId == "sv_new_row");
        }

        [Fact]
        public void FrozenV1ShapeChecksumIsGolden()
        {
            // QA finding: migration tests are self-consistent — they compute the
            // checksum with today's code, so drift in the frozen v1 shape would
            // silently change what "valid v1" means. Pin the canonical bytes:
            // any change to IceRoadSystemStateV1toV3 / HoldfastSaveV1 field set
            // or SaveChecksum formatting must fail here, forcing a deliberate
            // migration decision instead of a silent one.
            var v1 = new HoldfastSaveV1 { saveVersion = 1, simDay = 104 };
            var ice = new IceRoadSystem(808);
            ice.Unlock(90);
            ice.NotifyClerkStarted();
            v1.iceRoad = IceRoadSystemStateV1toV3.From(ice.CaptureState());
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "elena_vasquez", "marcus_olejnik", "suki_tanaka" }, 100);
            census.HonourLevy();
            v1.census = census.CaptureState();

            string golden = SaveChecksum.Compute(v1);
            Assert.Equal("801fd7ffbcde3dc4ff7eb29c2d4764455637b5c17efcfdab2de219b8c8aa844d", golden);
        }
    }
}
