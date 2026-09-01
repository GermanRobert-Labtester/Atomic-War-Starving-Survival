using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --wasteland-inhabitants-selftest / --plan20-selftest:
        /// Verifies Plan 20 Wasteland Inhabitants:
        /// 1. Field Guide (32 entries: 20 fauna + 12 flora, actionable intel, unlock tracking)
        /// 2. Wasteland Settlements (6 settlements across 6 archetypes, 18 named NPCs, standing greetings, trade tells)
        /// 3. Repeatable Side-Work (6 quest templates, cooldowns, completion persistence)
        /// 4. Route-Aware Travel Encounters (24 encounters + 4 multi-stage chains, stance weighting, deterministic selection)
        /// </summary>
        public static int RunWastelandInhabitantsSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int failures = 0;
            int totalAssertions = 0;

            void Check(bool ok, string label)
            {
                totalAssertions++;
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            GD.Print("[WastelandInhabitantsHeadlessDemo] begin Plan 20 verification...");

            var fileIO = new FileSystemIO();

            // ── 1. Field Guide Catalog & Intel ──────────────────────────────
            var fieldGuide = FieldGuideCatalog.LoadFromDirectory(dataDirectory, fileIO);
            Check(fieldGuide.Count == 32, $"Field guide entry count == 32 (got {fieldGuide.Count})");

            var fauna = fieldGuide.GetEntriesByCategory("Fauna");
            var flora = fieldGuide.GetEntriesByCategory("Flora");
            Check(fauna.Count == 20, $"Field guide fauna count == 20 (got {fauna.Count})");
            Check(flora.Count == 12, $"Field guide flora count == 12 (got {flora.Count})");

            Check(fieldGuide.TryGetEntry("field_fauna_two_headed_wolf", out var wolf) && wolf.ThreatLevel == 3, "Two-Headed Wolf threat level == 3");
            Check(fieldGuide.TryGetEntry("field_fauna_cave_bear", out var bear) && bear.ThreatLevel == 5, "Cave Bear threat level == 5");
            Check(fieldGuide.TryGetEntry("field_flora_nitrogen_mushroom", out var shroom) && !string.IsNullOrEmpty(shroom.Edibility), "Nitrogen Mushroom edibility documented");
            Check(fieldGuide.TryGetEntry("field_flora_glowing_rad_rye", out var rye) && rye.Tags.Contains("grain"), "Glowing Rad Rye tagged with 'grain'");

            // Unlock tracking & roundtrip
            fieldGuide.UnlockEntry("field_fauna_two_headed_wolf");
            fieldGuide.UnlockEntry("field_flora_nitrogen_mushroom");
            Check(fieldGuide.UnlockedCount == 2, "Field guide unlocked count == 2");
            Check(fieldGuide.IsUnlocked("field_fauna_two_headed_wolf"), "Wolf unlocked in guide");

            var fgState = fieldGuide.CaptureState();
            var fgNew = FieldGuideCatalog.LoadFromDirectory(dataDirectory, fileIO);
            fgNew.RestoreState(fgState);
            Check(fgNew.UnlockedCount == 2 && fgNew.IsUnlocked("field_flora_nitrogen_mushroom"), "Field guide unlock state restored cleanly");

            // ── 2. Wasteland Settlements & 18 NPCs ──────────────────────────
            var settlements = SettlementCatalog.LoadFromDirectory(dataDirectory, fileIO);
            Check(settlements.SettlementCount == 6, $"Settlement count == 6 (got {settlements.SettlementCount})");
            Check(settlements.NpcCount == 18, $"Settlement NPC count == 18 (got {settlements.NpcCount})");
            Check(settlements.QuestCount == 6, $"Repeatable quest count == 6 (got {settlements.QuestCount})");

            // Check each settlement archetype
            Check(settlements.TryGetSettlement("settlement_brine_pans", out var brine) && brine.Archetype == "Salt Camp", "Brine-Pan Hollow is Salt Camp");
            Check(settlements.TryGetSettlement("settlement_iron_siding", out var siding) && siding.Archetype == "Rail Siding Town", "Iron Siding is Rail Siding Town");
            Check(settlements.TryGetSettlement("settlement_cape_beacon", out var cape) && cape.Archetype == "Coastal Lighthouse Commune", "Cape Beacon is Coastal Lighthouse Commune");
            Check(settlements.TryGetSettlement("settlement_slate_hollow", out var slate) && slate.Archetype == "Quarry Enclave", "Slate Hollow is Quarry Enclave");
            Check(settlements.TryGetSettlement("settlement_pilgrim_hearth", out var pilgrim) && pilgrim.Archetype == "Religious / Monastic Sanctuary", "Pilgrim's Hearth is Monastic Sanctuary");
            Check(settlements.TryGetSettlement("settlement_tinkers_notch", out var tinker) && tinker.Archetype == "Free Trader Scrap Market", "Tinker's Notch is Free Trader Scrap Market");

            // Check NPC roles & standing greetings
            Check(settlements.TryGetNpc("npc_salt_marshal_varn", out var varn) && varn.Role == "Keeper", "Marshal Varn is Keeper at Brine-Pan");
            Check(settlements.TryGetNpc("npc_salt_trader_elena", out var elena) && elena.Role == "Trader", "Elena Kosh is Trader at Brine-Pan");
            Check(settlements.TryGetNpc("npc_salt_boiler_petyr", out var petyr) && petyr.Role == "Fixture", "Petyr is Fixture at Brine-Pan");

            string lowGreeting = settlements.GetNpcGreeting("npc_salt_marshal_varn", -30f);
            string neutralGreeting = settlements.GetNpcGreeting("npc_salt_marshal_varn", 0f);
            string highGreeting = settlements.GetNpcGreeting("npc_salt_marshal_varn", 50f);
            Check(!string.IsNullOrEmpty(lowGreeting) && !string.IsNullOrEmpty(neutralGreeting) && !string.IsNullOrEmpty(highGreeting), "Varn has 3 standing-reactive greetings");
            Check(lowGreeting != neutralGreeting && neutralGreeting != highGreeting, "Greetings differ across standing tiers");

            // ── 3. Repeatable Side-Work Quests ──────────────────────────────
            string questId = "quest_repeat_salt_boiler_scum";
            Check(settlements.IsQuestAvailable(questId, 1), "Side-work quest initially available on Day 1");
            settlements.CompleteQuest(questId, 1);
            Check(!settlements.IsQuestAvailable(questId, 2), "Side-work quest in cooldown on Day 2");
            Check(!settlements.IsQuestAvailable(questId, 7), "Side-work quest in cooldown on Day 7");
            Check(settlements.IsQuestAvailable(questId, 8), "Side-work quest available again on Day 8");
            Check(settlements.GetCompletedQuestCount(questId) == 1, "Completed quest count == 1");

            var stState = settlements.CaptureState();
            var stNew = SettlementCatalog.LoadFromDirectory(dataDirectory, fileIO);
            stNew.RestoreState(stState);
            Check(!stNew.IsQuestAvailable(questId, 5) && stNew.IsQuestAvailable(questId, 10), "Settlement quest state roundtrips accurately");

            // ── 4. Travel Encounters & Chained Events ───────────────────────
            var encCatalog = TravelEncounterCatalog.LoadFromDirectory(dataDirectory, fileIO);
            Check(encCatalog.Count >= 28, $"Travel encounter count >= 28 (got {encCatalog.Count})");

            var encSystem = new TravelEncounterSystem(encCatalog);
            var rng = new SeededRng(42);

            // Deterministic encounter selection
            var selectedEnc = encSystem.SelectEncounter("the_toll", 2.0f, "Balanced", "all", 1, rng);
            Check(selectedEnc != null, "Deterministic encounter selection returned an encounter");

            // Stance weight differentiation
            if (encCatalog.TryGetEncounter("enc_travel_bristleback_charge", out var boarEnc))
            {
                float cautiousWeight = encSystem.GetEffectiveWeight(boarEnc, "Cautious");
                float aggressiveWeight = encSystem.GetEffectiveWeight(boarEnc, "Aggressive");
                Check(aggressiveWeight > cautiousWeight, "Aggressive stance increases boar encounter weight");
            }

            // Chain progression
            string chainId = "chain_wandering_pilgrim";
            Check(encSystem.GetChainStage(chainId) == 0, "Chain initial stage == 0");
            encSystem.ResolveChoice("enc_chain_pilgrim_stage1", "choice_give_wood_and_water", 1, out int mDelta, out int gDelta, out string unlockedId);
            Check(encSystem.GetChainStage(chainId) == 2, "Chain stage advanced to 2 after resolving choice");
            Check(mDelta == 4, "Morale delta == +4 on kind pilgrim choice");

            // Field guide unlock from encounter choice
            encSystem.ResolveChoice("enc_travel_wolf_pack_crossing", "choice_throw_flare", 1, out _, out _, out string fgUnlock);
            Check(fgUnlock == "field_fauna_two_headed_wolf", "Encounter choice unlocked two-headed wolf field guide entry");

            // Save/load state of encounter system
            var encState = encSystem.CaptureState();
            var encSysNew = new TravelEncounterSystem(encCatalog);
            encSysNew.RestoreState(encState);
            Check(encSysNew.GetChainStage(chainId) == 2, "Encounter chain state restored accurately");

            GD.Print($"[Plan20Summary] total assertions: {totalAssertions}, failures: {failures}");
            return failures == 0 ? 0 : 1;
        }
    }
}
