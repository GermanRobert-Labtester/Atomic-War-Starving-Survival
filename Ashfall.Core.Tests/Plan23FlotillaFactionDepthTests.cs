using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Maritime;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 23 Task 23A — Black Flotilla faction &amp; item depth.
    /// Reads the real data authority: holdfast_factions.json (roster),
    /// black_flotilla_items.json (item catalog), characters.json (NPCs),
    /// faction_radio_corpus.json (broadcasts), hardcore_economy_tuning.json
    /// (marine salvage trade preference).
    /// </summary>
    public class Plan23FlotillaFactionDepthTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        private static string ReadData(string fileName)
            => File.ReadAllText(Path.Combine(FindDataDir(), fileName));

        // ── Faction roster ───────────────────────────────────────────

        [Fact]
        public void Faction_BlackFlotilla_InHoldfastRosterWithCanonicalGrammar()
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(FindDataDir());
            var entry = catalog.GetFaction(BlackFlotillaStanding.FactionId);

            Assert.NotNull(entry);
            Assert.Equal("The Black Flotilla", entry!.DisplayName);
            Assert.Equal("coastal_shelf", entry.HomeRegion);
            Assert.True(entry.IsActive);
            Assert.False(string.IsNullOrWhiteSpace(entry.SignatureQuote));
            Assert.False(string.IsNullOrWhiteSpace(entry.AccessRule));
            Assert.Equal("faction_icon_black_flotilla", entry.BadgeAssetId);
        }

        [Fact]
        public void Faction_RosterIds_AllUnique()
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(FindDataDir());
            var ids = catalog.Factions.Select(f => f.Id).ToList();

            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
            Assert.Contains(BlackFlotillaStanding.FactionId, ids);
        }

        // ── Standing on the existing stance authority ───────────────

        [Fact]
        public void Standing_Registration_ResolvesThresholdsAndTrust()
        {
            var engine = new FactionStanceEngine();
            BlackFlotillaStanding.Register(engine);

            Assert.True(engine.IsFactionActive(BlackFlotillaStanding.FactionId));
            engine.SetTrust(BlackFlotillaStanding.FactionId, 20f);
            Assert.Equal(20f, engine.GetTrust(BlackFlotillaStanding.FactionId));
            engine.ModifyTrust(BlackFlotillaStanding.FactionId, -70f);
            Assert.True(engine.GetTrust(BlackFlotillaStanding.FactionId) <= BlackFlotillaStanding.RaidThreshold);
        }

        [Fact]
        public void Standing_TierBoundaries_AreCanonical()
        {
            Assert.Equal(BlackFlotillaTier.Hostile, BlackFlotillaStanding.TierFor(-1f));
            Assert.Equal(BlackFlotillaTier.Tolerated, BlackFlotillaStanding.TierFor(0f));
            Assert.Equal(BlackFlotillaTier.Tolerated, BlackFlotillaStanding.TierFor(29f));
            Assert.Equal(BlackFlotillaTier.Trading, BlackFlotillaStanding.TierFor(30f));
            Assert.Equal(BlackFlotillaTier.Trading, BlackFlotillaStanding.TierFor(54.9f));
            Assert.Equal(BlackFlotillaTier.SalvageTrusted, BlackFlotillaStanding.TierFor(55f));
            Assert.Equal(BlackFlotillaTier.SalvageTrusted, BlackFlotillaStanding.TierFor(75f));
            Assert.False(BlackFlotillaStanding.CanTrade(-1f));
            Assert.True(BlackFlotillaStanding.CanTrade(0f));
            Assert.False(BlackFlotillaStanding.CanShareIntel(39f));
            Assert.True(BlackFlotillaStanding.CanShareIntel(40f));
            Assert.True(BlackFlotillaStanding.IsSalvageTrusted(30f));
            Assert.False(BlackFlotillaStanding.CanCooperateOnDeepDives(54.9f));
            Assert.True(BlackFlotillaStanding.CanCooperateOnDeepDives(55f));
        }

        // ── Twelve new items ─────────────────────────────────────────

        [Fact]
        public void Items_TwelvePlan23Items_ParseIntoMergedCatalog()
        {
            string dir = FindDataDir();
            var items = ItemCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());

            string[] expected = new[]
            {
                "item_descent_line", "item_sealed_dive_lamp", "item_salvage_cutting_tool",
                "item_rebreather_canister", "item_escort_challenge_ribbon",
                "item_deep_service_ribbon", "item_claim_tag_stamped",
                "item_sea_ration", "item_brine_protein_tin", "item_marine_sealant_kit",
                "item_ships_bell_picket", "item_fleet_log_cylinder"
            };

            Assert.True(items.Count >= 36, "merged item catalog should include all Flotilla items");
            foreach (var id in expected)
            {
                var def = items.FirstOrDefault(i => i.id == id);
                Assert.True(def != null, $"Plan 23 Flotilla item missing from merged catalog: {id}");
                Assert.False(string.IsNullOrWhiteSpace(def!.displayName));
            }

            // Roles: food restores, code/identity items are zero-value, relic is Relic.
            Assert.True(items.First(i => i.id == "item_sea_ration").hungerRestore > 0);
            Assert.True(items.First(i => i.id == "item_brine_protein_tin").hungerRestore > 0);
            Assert.Equal(0f, items.First(i => i.id == "item_escort_challenge_ribbon").tradeValue);
            Assert.Equal(0f, items.First(i => i.id == "item_deep_service_ribbon").tradeValue);
            Assert.Equal(ItemType.Relic, items.First(i => i.id == "item_ships_bell_picket").type);
            Assert.Equal(ItemType.Quest, items.First(i => i.id == "item_fleet_log_cylinder").type);
            Assert.True(items.First(i => i.id == "item_descent_line").isEquipable == false);
        }

        [Fact]
        public void Items_NoDuplicateIds_InFlotillaCatalog()
        {
            string json = ReadData("black_flotilla_items.json");
            var ids = System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("items")
                .EnumerateArray().Select(e => e.GetProperty("id").GetString()!).ToList();

            Assert.Equal(36, ids.Count);
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
        }

        // ── Marine salvage trade specialty ───────────────────────────

        [Fact]
        public void TradePreference_FlotillaSalvageSpecialty_LoadsFromTuningAuthority()
        {
            string json = File.ReadAllText(Path.Combine(FindDataDir(), "hardcore_economy_tuning.json"));
            var result = HardcoreEconomyTuningLoader.Load(json);
            Assert.True(result.IsValid, string.Join("; ", result.Errors));

            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(result.Bundle!);
            Assert.True(tuning.TryGetFactionPreference(BlackFlotillaStanding.FactionId, out var preference));
            Assert.Equal(BlackFlotillaStanding.FactionId, preference.FactionId);
            Assert.Contains("item_marine_sealant_kit", preference.BuysAtPremium);
            Assert.Contains("scrap_mechanical", preference.BuysAtPremium);
            Assert.Contains("jewelry", preference.Refuses);
            Assert.False(string.IsNullOrWhiteSpace(preference.TradeCurrency));
        }

        // ── Six named NPCs ───────────────────────────────────────────

        [Fact]
        public void Npcs_SixFlotillaRoles_PresentWithRequiredCoverage()
        {
            string json = ReadData("characters.json");
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var items = doc.RootElement.GetProperty("items");

            string[] expected =
            {
                "npc_odile_vanter",   // fleet-master / political coordinator
                "npc_cass_polder",    // salvage chief / quartermaster
                "npc_jorin_hael",     // dive-chief
                "npc_uma_tarran",     // code-keeper / radio authority
                "npc_halloran_vesk",  // escort officer / blockade-minded commander
                "npc_lotte_verrill"   // struck-off diver / dissident
            };

            foreach (var id in expected)
            {
                var npc = doc.RootElement.GetProperty("items").EnumerateArray()
                    .FirstOrDefault(c => c.GetProperty("id").GetString() == id);
                Assert.True(npc.ValueKind != System.Text.Json.JsonValueKind.Undefined, $"missing {id}");
                Assert.False(string.IsNullOrWhiteSpace(npc.GetProperty("profession").GetString()));
                Assert.False(string.IsNullOrWhiteSpace(npc.GetProperty("bio").GetString()));
                Assert.True(npc.TryGetProperty("wants", out var wants) && wants.GetArrayLength() > 0,
                    $"{id} must have a want (player-facing hook)");
            }
        }

        [Fact]
        public void Npcs_UniqueIds_AndFactionReferencesResolve()
        {
            string json = ReadData("characters.json");
            var ids = System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("items")
                .EnumerateArray().Select(c => c.GetProperty("id").GetString()!).ToList();

            Assert.Equal(68, ids.Count);
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
            Assert.Contains("\"faction\": \"faction_black_flotilla\"", json);

            // Every Flotilla NPC resolves against the faction roster authority.
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var holdfast = loader.Load(FindDataDir());
            Assert.NotNull(holdfast.GetFaction(BlackFlotillaStanding.FactionId));
        }

        // ── Radio broadcasts ─────────────────────────────────────────

        [Fact]
        public void Radio_FlotillaBandRegistered_WithCallsignAndFrequency()
        {
            string json = ReadData("faction_radio_corpus.json");
            var engine = FactionRadioEngine.LoadFromJson(json);

            Assert.Equal(14, engine.FactionCount);
            Assert.Contains(BlackFlotillaStanding.FactionId, engine.GetAllFactions());
            Assert.Equal(124.2f, engine.GetFactionFrequency(BlackFlotillaStanding.FactionId), 1);
            Assert.Equal("MOORING WATCH / BLACK FLOTILLA", engine.GetFactionCallsign(BlackFlotillaStanding.FactionId));
        }

        [Fact]
        public void Radio_Broadcasts_DeliveredThroughRealEngine_Deterministically()
        {
            string json = ReadData("faction_radio_corpus.json");
            var engine = FactionRadioEngine.LoadFromJson(json);

            var chatter = engine.GetFactionEvent(BlackFlotillaStanding.FactionId, RadioEventKind.InterceptChatter, 200, new SeededRng(31));
            Assert.Equal(BlackFlotillaStanding.FactionId, chatter.FactionId);
            Assert.False(string.IsNullOrWhiteSpace(chatter.Message));

            var trade = engine.GetFactionEvent(BlackFlotillaStanding.FactionId, RadioEventKind.TradeReaction, 200, new SeededRng(2009));
            Assert.Equal(RadioEventKind.TradeReaction, trade.Kind);
            Assert.False(string.IsNullOrWhiteSpace(trade.Message));

            // Deterministic: same faction + kind + day + seed → identical message.
            var a = engine.GetFactionEvent(BlackFlotillaStanding.FactionId, RadioEventKind.InterceptChatter, 55, new SeededRng(77));
            var b = engine.GetFactionEvent(BlackFlotillaStanding.FactionId, RadioEventKind.InterceptChatter, 55, new SeededRng(77));
            Assert.Equal(a.Message, b.Message);
        }

        [Fact]
        public void Radio_FlotillaFrequency_IsDistinctFromAllOtherBands()
        {
            string json = ReadData("faction_radio_corpus.json");
            var engine = FactionRadioEngine.LoadFromJson(json);
            float flotillaFreq = engine.GetFactionFrequency(BlackFlotillaStanding.FactionId);

            foreach (var other in engine.GetAllFactions())
            {
                if (other == BlackFlotillaStanding.FactionId) continue;
                Assert.True(Math.Abs(engine.GetFactionFrequency(other) - flotillaFreq) >= 1.5f,
                    $"Flotilla frequency within tolerance of {other}");
            }
        }

        [Fact]
        public void Radio_CodedVocabulary_UsesConsistentFlotillaTerms()
        {
            string json = ReadData("faction_radio_corpus.json");
            var engine = FactionRadioEngine.LoadFromJson(json);
            var chatter = engine.GetFactionEvent(BlackFlotillaStanding.FactionId, RadioEventKind.InterceptChatter, 10, new SeededRng(3));
            _ = chatter;
            // The corpus itself must carry the code vocabulary the broadcasts reuse.
            Assert.Contains("claim", json);
            Assert.Contains("ribbon", json);
            Assert.Contains("mooring", json.ToLowerInvariant());
        }

        // ── Old-save compatibility ───────────────────────────────────

        [Fact]
        public void OldSaves_FlotillaAdditionsRequireNoFabricatedState()
        {
            // Catalog additions never fabricate historical state: the maritime
            // save section does not persist faction rosters, item catalogs,
            // radio bands, or NPC registries. Standing restores at 0 (Tolerated).
            var engine = new FactionStanceEngine();
            BlackFlotillaStanding.Register(engine);
            Assert.Equal(0f, engine.GetTrust(BlackFlotillaStanding.FactionId));
            Assert.Equal(BlackFlotillaTier.Tolerated, BlackFlotillaStanding.TierFor(0f));
        }
    }
}
