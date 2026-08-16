using System;
using System.Collections.Generic;
using System.Text;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.Warlords
{
    /// <summary>
    /// Vertical-slice smoke for the adaptive warlord AI (PROPOSED model — the
    /// Unity stub is a meta-progression combat-counter learner with no usable
    /// faction semantics). Proves: catalog load + validation, initial doctrine,
    /// doctrine transitions with cooldown/hysteresis, seeded deterministic
    /// action selection, territory claims/contests/annexation over existing
    /// location ids, tribute escalation (Warlord Code), travel-danger
    /// consequences, alias-conflict reporting, and the YearOfAshSave v3
    /// round-trip with v2 migration + future-version rejection.
    /// Invoked by `dotnet test` and by Godot `-- --warlord-selftest`.
    /// </summary>
    public static class WarlordHeadlessDemo
    {
        public const int DefaultSeed = 7719;

        public static HeadlessReport Run(string dataDirectory = null, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) report.PassedCount++;
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
                if (condition) log.Info("[PASS] " + name);
            }

            log.Info("[WarlordHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // ── Catalog load + loud validation ─────────────────────────
            WarlordDoctrineCatalog catalog;
            if (!string.IsNullOrEmpty(dataDirectory))
            {
                catalog = WarlordDoctrineCatalogLoader.Load(dataDirectory, files, json);
                var validation = WarlordCatalogValidator.Validate(catalog, dataDirectory, files);
                Check(validation.Clean, "warlord catalog cross-references resolve (locations/factions/items/doctrines)");
                for (int i = 0; i < validation.Errors.Count; i++)
                    log.Error("  validation error: " + validation.Errors[i]);
                Check(validation.AliasWarnings.Count >= 1, "faction alias conflicts are reported, not merged");
                for (int i = 0; i < validation.AliasWarnings.Count; i++)
                    log.Info("  " + validation.AliasWarnings[i]);
            }
            else
            {
                catalog = BuildInlineCatalog();
            }

            Check(catalog.Warlord.faction_id == WarlordDoctrineSystem.CanonicalFactionId,
                "warlord binds only to warlords_sector_4 (canonical id)");
            Check(catalog.Doctrines.Count >= 3, "at least three materially distinct doctrines");
            Check(catalog.GetNode(catalog.Warlord.home_location_id) != null,
                "home location is a territory node");

            // ── Initial state ──────────────────────────────────────────
            var warlord = new WarlordDoctrineSystem(catalog, DefaultSeed);
            Check(warlord.DoctrineId == catalog.Warlord.starting_doctrine_id, "initial doctrine from catalog");
            Check(warlord.TerritoryState(catalog.Warlord.home_location_id) == WarlordTerritoryState.Controlled,
                "home location controlled from the start");
            Check(warlord.ControlledCount() == 1, "exactly one controlled node at start");
            Check(warlord.TravelDangerModifier(catalog.Warlord.home_location_id) == 0.35f,
                "controlled home raises travel danger by 0.35");
            Check(warlord.ReportedState("loc_weighbridge") == WarlordTerritoryState.None,
                "warlord has no knowledge until observed (non-omniscient)");

            // ── Seeded action selection + territory ────────────────────
            var rngA = new SeededRng(DefaultSeed);
            var actionsA = new List<WarlordActionResult>();
            var territoryEventsA = new List<string>();
            warlord.OnActionExecuted += r => actionsA.Add(r);
            warlord.OnTerritoryChanged += (loc, from, to, day) => territoryEventsA.Add(loc + ":" + from + "->" + to);

            // Feed scouts: adjacent chokepoints are observable.
            warlord.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            warlord.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);

            var ctx = new WarlordContext { EnvironmentHazard = 0.2f, RivalPressure = 0.3f, PlayerStanding = 0 };
            for (int day = 210; day <= 340; day++)
                warlord.TickDaily(day, rngA, ctx);

            Check(actionsA.Count > 0, "strategic actions executed on the operation cadence");
            Check(warlord.TotalOperations > 0, "operation ledger advances");
            bool someGround = false;
            foreach (var a in actionsA)
                if (a.Action == WarlordStrategicAction.Contest || a.Action == WarlordStrategicAction.Annex)
                    if (a.Success && !string.IsNullOrEmpty(a.TargetLocationId)) someGround = true;
            Check(someGround || warlord.ContestedCount() + (warlord.ControlledCount() - 1) > 0,
                "territory claims/contests/annexation produced ground change");

            // ── Determinism: same seed + same actions ⇒ same trace ─────
            var warlordB = new WarlordDoctrineSystem(catalog, DefaultSeed);
            var actionsB = new List<WarlordActionResult>();
            warlordB.OnActionExecuted += r => actionsB.Add(r);
            warlordB.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            warlordB.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            var rngB = new SeededRng(DefaultSeed);
            for (int day = 210; day <= 340; day++)
                warlordB.TickDaily(day, rngB, ctx);
            Check(actionsB.Count == actionsA.Count, "same-seed action count identical");
            for (int i = 0; i < actionsA.Count && i < actionsB.Count; i++)
            {
                bool same = actionsA[i].Action == actionsB[i].Action
                    && actionsA[i].TargetLocationId == actionsB[i].TargetLocationId
                    && actionsA[i].Success == actionsB[i].Success;
                if (!same)
                {
                    Check(false, "same-seed action #" + i + " identical");
                    break;
                }
                if (i == actionsA.Count - 1)
                    Check(true, "same-seed action trace identical (action/target/outcome)");
            }
            Check(SameTerritory(warlord, warlordB), "same-seed territory state identical");

            // ── Cooldown / hysteresis: no thrash ───────────────────────
            int maxChanges = 1 + (340 - 210) / Math.Max(1, catalog.Warlord.doctrine_cooldown_days);
            Check(warlord.State.doctrineHistory.Count <= maxChanges,
                "doctrine changes stay bounded (cooldown + margin prevent thrash)");
            for (int i = 1; i < warlord.State.doctrineHistory.Count; i++)
            {
                int gap = warlord.State.doctrineHistory[i].day - warlord.State.doctrineHistory[i - 1].day;
                if (gap < catalog.Warlord.doctrine_cooldown_days)
                {
                    Check(false, "doctrine changes respect the cooldown gap");
                    break;
                }
                if (i == warlord.State.doctrineHistory.Count - 1)
                    Check(true, "doctrine changes respect the cooldown gap");
            }

            // ── Tribute escalation (Warlord Code) ──────────────────────
            var tribute = new WarlordDoctrineSystem(catalog, DefaultSeed + 1);
            int firstAsk = 0;
            tribute.OnTributeDemanded += (amount, item, day) => firstAsk = amount;
            var trng = new SeededRng(DefaultSeed + 1);
            for (int day = 210; day < 217; day++)
                tribute.TickDaily(day, trng, new WarlordContext());
            Check(firstAsk == catalog.Warlord.tribute_base_amount, "first tribute ask is the base amount");
            int next;
            tribute.SettleTribute(0, 217, out next);
            tribute.SettleTribute(0, 224, out next);
            Check(next > firstAsk, "short payment escalates the ask (×1.5, capped at 8×)");
            Check(tribute.TributeMultiplier <= catalog.Warlord.tribute_max_multiplier,
                "tribute multiplier respects the cap");

            // ── Invalid action guards ──────────────────────────────────
            var withdrawn = new WarlordDoctrineSystem(catalog, DefaultSeed + 2);
            Check(!withdrawn.IsHostileAccess("loc_grain_silo"), "uncontrolled location is not hostile access");
            Check(withdrawn.TravelDangerModifier("loc_grain_silo") == 0f, "unclaimed location has no travel danger");

            // ── YearOfAshSave v3 round-trip + v2 migration ────────────
            var timeline = new YearOfAsh.YearOfAshTimelineSystem();
            var encounters = new YearOfAsh.DoorEncounterSystem();
            var factionWar = new YearOfAsh.FactionWarSystem();
            var save = YearOfAsh.YearOfAshSaveCodec.Capture(timeline, encounters, factionWar, null, null, null, null, warlord);
            Check(save.saveVersion == YearOfAsh.YearOfAshSave.CurrentSaveVersion, "saveVersion is " + YearOfAsh.YearOfAshSave.CurrentSaveVersion);
            Check(save.warlord != null && save.warlord.doctrineId == warlord.DoctrineId, "warlord state captured");
            string encoded = YearOfAsh.YearOfAshSaveCodec.Encode(save, json);
            var loaded = YearOfAsh.YearOfAshSaveCodec.Decode(encoded, json);
            Check(loaded.warlord.doctrineId == warlord.DoctrineId, "warlord doctrine round-trips");
            Check(loaded.warlord.territory.Count == warlord.State.territory.Count, "warlord territory round-trips");
            Check(loaded.warlord.supply == warlord.Supply, "warlord supply ledger round-trips");

            // v2 migration → fresh toll-doctrine warlord (missing-state default).
            var v2 = new YearOfAsh.YearOfAshSaveV2
            {
                saveVersion = 2,
                simDay = 220,
                timeline = timeline.CaptureState(),
                encounters = encounters.CaptureState(),
                factionWar = factionWar.CaptureState()
            };
            v2.Checksum = SaveChecksum.Compute(v2);
            var migrated = YearOfAsh.YearOfAshSaveCodec.Decode(json.Serialize(v2), json);
            Check(migrated.saveVersion == YearOfAsh.YearOfAshSave.CurrentSaveVersion, "v2 migrates to v3");
            Check(migrated.warlord.doctrineId == catalog.Warlord.starting_doctrine_id,
                "v2 migration yields the default toll-doctrine warlord");
            Check(migrated.warlord.territory == null || migrated.warlord.territory.Count == 0,
                "migrated warlord has no territory list (system applies catalog defaults)");

            // Future-version rejection + tamper.
            var future = json.Deserialize<YearOfAsh.YearOfAshSave>(encoded);
            future.saveVersion = YearOfAsh.YearOfAshSave.CurrentSaveVersion + 1;
            future.Checksum = SaveChecksum.Compute(future);
            bool futureRejected = false;
            try { YearOfAsh.YearOfAshSaveCodec.Decode(json.Serialize(future), json); }
            catch (InvalidOperationException) { futureRejected = true; }
            Check(futureRejected, "future saveVersion rejected");

            string tampered = encoded.Replace("\"simDay\":180", "\"simDay\":181");
            bool tamperRejected = false;
            try { YearOfAsh.YearOfAshSaveCodec.Decode(tampered, json); }
            catch (InvalidOperationException) { tamperRejected = true; }
            Check(tamperRejected, "tampered v3 save rejected (checksum)");

            report.Summary = $"[WarlordHeadlessDemo] {report.PassedCount}/{report.PassedCount + report.FailedCount} PASSED";
            log.Info(report.Summary);
            return report;
        }

        private static bool SameTerritory(WarlordDoctrineSystem a, WarlordDoctrineSystem b)
        {
            var ta = a.State.territory;
            var tb = b.State.territory;
            if (ta == null || tb == null || ta.Count != tb.Count) return false;
            for (int i = 0; i < ta.Count; i++)
            {
                if (ta[i] == null || tb[i] == null) return false;
                if (ta[i].locationId != tb[i].locationId || ta[i].state != tb[i].state) return false;
            }
            return true;
        }

        private static WarlordDoctrineCatalog BuildInlineCatalog()
        {
            var catalog = new WarlordDoctrineCatalog();
            catalog.Warlord.faction_id = "warlords_sector_4";
            catalog.Warlord.home_location_id = "loc_toll_house";
            catalog.Warlord.starting_doctrine_id = "warlord_doctrine_toll";
            catalog.Warlord.tribute_currency_item = "canned_food";
            catalog.Territory.Add(new WarlordTerritoryNodeDef
            {
                location_id = "loc_toll_house", home = true, supply_value = 2, defense_value = 4,
                neighbors = new List<string> { "loc_weighbridge", "loc_denial_cut_substation" }
            });
            catalog.Territory.Add(new WarlordTerritoryNodeDef
            {
                location_id = "loc_weighbridge", supply_value = 2, defense_value = 2,
                neighbors = new List<string> { "loc_toll_house", "loc_denial_cut_substation", "loc_continental_convoy_staging_area" }
            });
            catalog.Territory.Add(new WarlordTerritoryNodeDef
            {
                location_id = "loc_denial_cut_substation", supply_value = 3, defense_value = 3,
                neighbors = new List<string> { "loc_toll_house", "loc_weighbridge", "loc_grain_silo" }
            });
            catalog.Territory.Add(new WarlordTerritoryNodeDef
            {
                location_id = "loc_continental_convoy_staging_area", supply_value = 4, defense_value = 2,
                neighbors = new List<string> { "loc_weighbridge" }
            });
            catalog.Territory.Add(new WarlordTerritoryNodeDef
            {
                location_id = "loc_grain_silo", supply_value = 5, defense_value = 3,
                neighbors = new List<string> { "loc_denial_cut_substation" }
            });
            catalog.Doctrines.Add(new WarlordDoctrineDef
            {
                id = "warlord_doctrine_toll", display_name = "The Toll", risk_tolerance = 0.6f,
                eligible_actions = new List<string> { "DemandTribute", "Raid", "Defend", "Contest" },
                action_weights = new Dictionary<string, int> { { "DemandTribute", 4 }, { "Raid", 3 }, { "Defend", 2 }, { "Contest", 1 } },
                journal_key = "journal_warlord_toll_doctrine", radio_key = "radio_warlord_toll_standing",
                transitions = new List<WarlordDoctrineTransitionDef>
                {
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_annexation", signal = "success_streak", condition = "gte", threshold = 2 },
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_consolidation", signal = "failure_streak", condition = "gte", threshold = 2 },
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_consolidation", signal = "supply_ratio", condition = "lt", threshold = 0.6f }
                }
            });
            catalog.Doctrines.Add(new WarlordDoctrineDef
            {
                id = "warlord_doctrine_consolidation", display_name = "Holding the Line", risk_tolerance = 0.3f,
                eligible_actions = new List<string> { "Defend", "DemandTribute", "Contest" },
                action_weights = new Dictionary<string, int> { { "Defend", 5 }, { "DemandTribute", 2 }, { "Contest", 1 } },
                journal_key = "journal_warlord_consolidation_doctrine", radio_key = "radio_warlord_consolidation",
                transitions = new List<WarlordDoctrineTransitionDef>
                {
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_toll", signal = "supply_ratio", condition = "gte", threshold = 1.2f },
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_toll", signal = "player_tribute_reliability", condition = "lt", threshold = 0.5f },
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_withdrawal", signal = "environment_hazard", condition = "gte", threshold = 0.7f }
                }
            });
            catalog.Doctrines.Add(new WarlordDoctrineDef
            {
                id = "warlord_doctrine_annexation", display_name = "The Long Reach", risk_tolerance = 0.8f,
                eligible_actions = new List<string> { "Annex", "Contest", "Raid", "DemandTribute" },
                action_weights = new Dictionary<string, int> { { "Annex", 4 }, { "Contest", 3 }, { "Raid", 2 }, { "DemandTribute", 1 } },
                journal_key = "journal_warlord_annexation_doctrine", radio_key = "radio_warlord_annexation",
                transitions = new List<WarlordDoctrineTransitionDef>
                {
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_consolidation", signal = "contested_count", condition = "gte", threshold = 2 },
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_consolidation", signal = "failure_streak", condition = "gte", threshold = 2 },
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_withdrawal", signal = "environment_hazard", condition = "gte", threshold = 0.7f }
                }
            });
            catalog.Doctrines.Add(new WarlordDoctrineDef
            {
                id = "warlord_doctrine_withdrawal", display_name = "Gone to Ground", risk_tolerance = 0.15f,
                eligible_actions = new List<string> { "Withdraw", "Defend" },
                action_weights = new Dictionary<string, int> { { "Withdraw", 5 }, { "Defend", 1 } },
                journal_key = "journal_warlord_withdrawal_doctrine", radio_key = "radio_warlord_withdrawal",
                transitions = new List<WarlordDoctrineTransitionDef>
                {
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_toll", signal = "environment_hazard", condition = "lt", threshold = 0.4f },
                    new WarlordDoctrineTransitionDef { to = "warlord_doctrine_toll", signal = "supply_ratio", condition = "gte", threshold = 0.9f }
                }
            });
            return catalog;
        }
    }
}
