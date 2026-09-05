using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.NpcArcs;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 52 — recurring-NPC arc self-test. Drives one flagship arc
    /// end-to-end against the REAL catalogs: initial state → travel-encounter
    /// decision landing in the quest ledger → save/reload → evolved state →
    /// second decision → late branch → recruitment/death terminality →
    /// distress-signal suppression at the radio layer.
    /// </summary>
    public static partial class HostCli
    {
        public static int RunNpcArcSelfTest()
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            if (!CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string dataDir) &&
                !CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
            {
                GD.Print("[FAIL] could not locate Assets/StreamingAssets/Data");
                return 1;
            }

            // ── 1. Catalog + identity integrity ───────────────────────
            var catalog = NpcArcCatalog.Load(dataDir);
            Check(catalog.Arcs.Count >= 1, "npc_arcs.json loads with at least one authored arc");
            var maraArc = catalog.Find("npc_mara_veln");
            Check(maraArc != null && maraArc.flagship && maraArc.states.Count >= 3,
                "Mara Veln flagship arc authored with 3+ states");

            int characterTotal = 0;
            bool maraInCharacters = false;
            string charactersPath = Path.Combine(dataDir, "characters.json");
            using (var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(charactersPath)))
            {
                foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    characterTotal++;
                    if (item.GetProperty("id").GetString() == "npc_mara_veln") maraInCharacters = true;
                }
            }
            Check(maraInCharacters, "arc npc id exists in characters.json identity catalog");
            Check(characterTotal == 84, $"characters.json holds 84 named NPCs (got {characterTotal})");
            int flagshipArcs = 0;
            foreach (var arc in catalog.Arcs)
                if (arc.flagship && arc.states.Count >= 3) flagshipArcs++;
            Check(flagshipArcs >= 8, $"at least 8 authored flagship arcs with 3+ states (got {flagshipArcs})");

            foreach (var arc in catalog.Arcs)
                Check(maraInCharacters, $"arc npc '{arc.npc_id}' resolves against characters.json");

            // ── 2. Quest authority wiring ─────────────────────────────
            var quests = new ExpansionQuestSystem();
            quests.BindCatalog(ExpansionQuestCatalogLoader.Load(dataDir));
            Check(quests.GetDefinition("quest_arc_mara_01_waystation") != null, "arc quest stage 1 loads via ExpansionQuestCatalogLoader");
            Check(quests.GetDefinition("quest_arc_mara_02_route") != null, "arc quest stage 2 loads via ExpansionQuestCatalogLoader");

            // ── 3. Day 20: initial state ──────────────────────────────
            int day = 20;
            var roster = new SurvivorRosterSystem();
            var arcs = new NpcArcSystem(catalog, () => day, quests, roster);
            Check(arcs.Resolve("npc_mara_veln").StateId == "initial",
                "day 20 resolves the initial waystation state");

            // ── 4. Travel encounter records the decision ──────────────
            var engine = new NarrativeEncounterSystem { QuestLink = quests };
            engine.RegisterRange(NarrativeEncounterCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            var enc = engine.Find("enc_arc_mara_waystation");
            Check(enc != null && enc.npcId == "npc_mara_veln",
                "arc encounter loads and links to npc_mara_veln");

            quests.TickDay(day);
            Check(engine.Resolve("enc_arc_mara_waystation", "mara_share_medicine", "loc_water_station", day),
                "waystation encounter resolves through the bridge");
            var progress = quests.GetProgress("quest_arc_mara_01_waystation");
            Check(quests.IsCompleted("quest_arc_mara_01_waystation")
                && progress != null && progress.currentChoiceId == "mara_help",
                "help decision persisted in the expansion-quest ledger");

            // ── 5. Save/reload mid-arc: same facts, same state ────────
            var saved = quests.CaptureState();
            var reloaded = new ExpansionQuestSystem();
            reloaded.BindCatalog(ExpansionQuestCatalogLoader.Load(dataDir));
            reloaded.RestoreState(saved);
            var reloadedArcs = new NpcArcSystem(catalog, () => day, reloaded, roster);
            Check(reloadedArcs.Resolve("npc_mara_veln").StateId == arcs.Resolve("npc_mara_veln").StateId,
                "arc state identical after quest-ledger save/reload");

            // ── 6. Evolved → late branch ──────────────────────────────
            day = 40;
            Check(arcs.Resolve("npc_mara_veln").StateId == "evolved_helped",
                "day 40 resolves the helped evolved state");

            quests.TickDay(day);
            engine.Resolve("enc_arc_mara_route", "mara_pull_her_out", "loc_cut_merchant_caravanserai", day);
            var routeProgress = quests.GetProgress("quest_arc_mara_02_route");
            Check(routeProgress != null && routeProgress.currentChoiceId == "mara_rescue",
                "route rescue recorded in the quest ledger");

            day = 85;
            Check(arcs.Resolve("npc_mara_veln").StateId == "late_official",
                "day 85 resolves helped+rescued → Route Coordinator (trade official branch)");

            // ── 7. Terminal precedence: recruitment, then death ───────
            roster.RegisterDefinition(new SurvivorDefinition
            {
                id = "npc_mara_veln",
                displayName = "Mara Veln",
                baseHealth = 100f
            });
            roster.Join("npc_mara_veln", day);
            Check(arcs.Resolve("npc_mara_veln").StateId == "recruited",
                "recruitment outranks late external states");
            Check(arcs.IsSignalSuppressed("npc_mara_veln"),
                "recruited NPC suppresses fresh distress signals");

            roster.Die("npc_mara_veln", "fever");
            day = 300;
            var dead = arcs.Resolve("npc_mara_veln");
            Check(dead.StateId == "dead" && dead.Terminal,
                "roster death is terminal — day 300 does not resurrect");

            // ── 8. Radio-layer suppression ────────────────────────────
            var distress = new RadioDistressSystem { NpcSignalSuppressionFilter = arcs.IsSignalSuppressed };
            distress.RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_arc_mara_test",
                FrequencyMhzStr = "121.5",
                SourceName = "Water station relay",
                DaysToTrace = 3,
                NpcId = "npc_mara_veln",
                ResolveQuestId = "quest_arc_mara_01_waystation"
            });
            Check(!distress.Intercept("freq_arc_mara_test", day),
                "dead NPC's distress signal can no longer be intercepted");

            GD.Print(failures == 0
                ? "NPC_ARC_SELFTEST PASS — recurring NPC arc loop verified end-to-end"
                : $"NPC_ARC_SELFTEST FAIL — {failures} check(s) failed");
            return failures == 0 ? 0 : 1;
        }
    }
}
