using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Campaign;
using Ashfall.Core.Disease;
using Ashfall.Core.Ecology;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 28 Phase 4 — ecological infestation host wire.
    ///
    ///   • trigger   — season window (Plan 19 profile) + authored site
    ///                 preconditions, rolled on the world_evolution day fork;
    ///   • food loss — routed through <c>InventoryHostSession.Remove/Add</c>
    ///                 (bounded: <see cref="EcologicalInfestationSystem.MaxFoodLossPerDay"/>);
    ///   • disease   — Plan 09 <see cref="IDiseaseOutbreakSource"/> contract
    ///                 ("ecological_infestation"), system-rejected otherwise;
    ///   • clears    — item costs consumed through the inventory authority;
    ///   • notices   — journal + briefing projections, once per event (28AX).
    /// </summary>
    public partial class Main
    {
        private EcologicalInfestationSystem? _ecologicalInfestations;
        private bool _ecologicalInfestationsDirty;
        private readonly IDiseaseOutbreakSource _ecologicalDiseaseSource =
            new EcologicalInfestationDiseaseSource();

        // ── Setup / Save / Flush (triad) ────────────────────────────────

        private void SetupEcologicalInfestation()
        {
            if (_ecologicalInfestations != null) return;
            _ecologicalInfestations = new EcologicalInfestationSystem();

            var defs = EcologicalInfestationCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog());
            if (defs != null) _ecologicalInfestations.LoadDefinitions(defs);

            var saved = EcologicalInfestationSaveStore.TryLoad();
            if (saved != null) _ecologicalInfestations.RestoreState(saved);
        }

        private void TickEcologicalInfestations(int day, List<DayStateChangeEvent> events)
        {
            if (_ecologicalInfestations == null) SetupEcologicalInfestation();
            if (_ecologicalInfestations == null) return;

            var season = WildlifeSeasonalCalendar.SeasonWindowForDay(_world.Profile, day);
            var triggerRng = _campaignDay.Rng.Fork(CampaignStreamIds.WorldEvolution, day, 21);

            foreach (var def in _ecologicalInfestations.Definitions)
            {
                if (def == null || !InfestationEligible(def, day, season)) continue;
                if (!_ecologicalInfestations.TryTrigger(def.id, day, triggerRng, out _)) continue;

                _ecologicalInfestationsDirty = true;
                events.Add(new DayStateChangeEvent(
                    def.scope == "shelter" ? "hazard_warning" : "radio_intercept",
                    "world_evolution",
                    def.target_id,
                    def.trigger_summary,
                    def.food_loss_per_day));
                SetupJournal();
                _journal.TryAddRawEntry($"eco_{def.id}_outbreak",
                    $"⚠ {def.trigger_summary}", null!, day);
            }

            _ecologicalInfestations.TickDay(day,
                _campaignDay.Rng.Fork(CampaignStreamIds.WorldEvolution, day, 22),
                foodLoss: (infestationId, units) => ConvertSpoiledRations(units, day, infestationId),
                diseaseRisk: TriggerInfestationDisease);
        }

        private void SaveEcologicalInfestation()
        {
            if (_ecologicalInfestations == null) return;
            if (CaptureSection("ecological_infestation",
                EcologicalInfestationSaveStore.TryCapturePersisted(_ecologicalInfestations.CaptureState())))
            {
                _ecologicalInfestationsDirty = false;
                GD.Print("[Ashfall Godot] Ecological infestation save written.");
            }
        }

        private void FlushEcologicalInfestationIfDirty()
        {
            if (_ecologicalInfestationsDirty) SaveEcologicalInfestation();
        }

        // ── Preconditions (authored, site-driven) ───────────────────────

        private bool InfestationEligible(EcologicalInfestationDefinition def, int day, SeasonWindowDef? season)
        {
            if (_ecologicalInfestations == null) return false;
            if (!_ecologicalInfestations.IsEligibleToTrigger(def.id, day, season)) return false;
            if (!SitePreconditionPasses(def)) return false;
            return def.trigger_chance_per_day > 0f;
        }

        private bool SitePreconditionPasses(EcologicalInfestationDefinition def)
        {
            switch (def.scope)
            {
                case "location":
                    // The site must be known ground (scouted/visited) — the
                    // wilderness does not ambush the untravelled.
                    return _world.LocationEvolution?.TryGetRecord(def.target_id) != null;
                case "shelter" when def.requires_state == "grain_stores":
                    // Weevils ride stored rations: a real pantry to raid.
                    return _inventory != null
                        && (_inventory.Inventory.CountById("canned_food")
                            + _inventory.Inventory.CountById("item_canned_grain_stew")) >= 4;
                case "shelter" when def.requires_state == "low_filtration":
                    // The HEPA stack is the shelter's ventilation authority.
                    return (_startingLevel?.System.State.airFilterHealthPercent ?? 100f) < 55f;
                case "shelter" when def.requires_state == "greenhouse_planted":
                    return _greenhouse?.System.State.plots.Any(p => p != null
                        && !string.IsNullOrEmpty(p.seedItemId)) == true;
                case "shelter" when def.requires_state == "quiet_winter":
                    // Deep cold drives burrowers toward warm walls.
                    return _world.Weather.Current == WeatherKind.Blizzard
                        || WildlifeSeasonalCalendar.SeasonWindowForDay(_world.Profile, _simDay)?.id
                            == WildlifeSeasonalCalendar.SeasonDeepFreeze;
                default:
                    return true;
            }

            SeasonWindowDef? seasonWindow(int day) =>
                WildlifeSeasonalCalendar.SeasonWindowForDay(_world.Profile, day);
        }

        private SeasonWindowDef? seasonWindow(int day) =>
            WildlifeSeasonalCalendar.SeasonWindowForDay(_world.Profile, day);

        // ── Effect routing (owning authorities only) ────────────────────

        private void ConvertSpoiledRations(int units, int day, string infestationId)
        {
            // Bounded: at most the routed units turn per day, only while
            // stock exists. The inventory authority performs the removal —
            // no hidden inventory writes.
            int converted = 0;
            for (int i = 0; i < Math.Min(units, EcologicalInfestationSystem.MaxFoodLossPerDay); i++)
            {
                if ((_inventory?.Inventory.CountById("canned_food") ?? 0) <= 0) break;
                var res = _inventory!.Remove("canned_food", 1);
                if (string.IsNullOrEmpty(res) || res.Contains("Not enough")) break;
                _inventory.Add("spoiled_canned_food", 1);
                converted++;
            }
            if (converted > 0)
            {
                _ecologicalInfestationsDirty = true;
                SetupJournal();
                _journal.TryAddRawEntry($"eco_food_loss_{infestationId}_{day}",
                    $"🐜 Pantry loss ({converted} canned): weevils working the stores.",
                    null!, day);
            }
        }

        private void TriggerInfestationDisease(string diseaseId)
        {
            // Plan 09 authority: TriggerOutbreak rejects ids outside the
            // ecological source's authored contract, so no second infection
            // path can exist by construction.
            var roster = _survivors?.Roster?.Roster ?? new List<SurvivorRosterEntry>();
            var candidates = roster.Where(e => e != null && e.isAlive).Select(e => e.survivorId).ToList();
            if (candidates.Count == 0) return;

            var result = _disease.Engine.TriggerOutbreak(
                _ecologicalDiseaseSource, diseaseId, _simDay, candidates);
            if (result.InfectionsApplied > 0)
            {
                SetupJournal();
                _journal.TryAddRawEntry($"eco_spore_{_simDay}",
                    "The bloom breathes at night. Someone woke with a tight chest.",
                    null!, _simDay);
                _ecologicalInfestationsDirty = true;
            }
        }

        /// <summary>Disease contract for every ecological infestation (Plan 09 port).</summary>
        private sealed class EcologicalInfestationDiseaseSource : IDiseaseOutbreakSource
        {
            public string SourceId => "ecological_infestation";
            public IReadOnlyList<string> AuthoredDiseaseIds { get; } =
                new[] { "disease_spore_blight", "disease_fungal_respiratory" };
        }

        // ── Plan 28 Phase 5: field-guide observation knowledge ─────────

        private FieldGuideCatalog? _fieldGuide;

        /// <summary>
        /// A sighted species unlocks its "reading the land" entry. Session
        /// knowledge: persistence rides Plan 20A's save store (their GAP row).
        /// </summary>
        private void UnlockFieldGuideObservation(string fieldEntryId)
        {
            if (_fieldGuide == null)
            {
                _fieldGuide = FieldGuideCatalog.LoadFromDirectory(
                    _dataDir, new FileSystemIO());
            }
            if (_fieldGuide!.UnlockEntry(fieldEntryId))
            {
                SetupJournal();
                var entry = _fieldGuide.TryGetEntry(fieldEntryId, out var e) ? e : null;
                _journal.TryAddRawEntry($"field_guide_{fieldEntryId}",
                    $"📖 {entry?.CommonName ?? fieldEntryId}: added to the field guide.", null!, _simDay);
            }
        }

        private void SetupFieldGuide()
        {
            if (_fieldGuide != null) return;
            _fieldGuide = FieldGuideCatalog.LoadFromDirectory(_dataDir, new FileSystemIO());

            // Plan 20A GAP closure: unlocked entries persist across save/load.
            var saved = FieldGuideSaveStore.TryLoad();
            if (saved != null) _fieldGuide.RestoreState(saved);
        }

        private void SaveFieldGuide()
        {
            if (_fieldGuide == null) return;
            if (CaptureSection("field_guide",
                FieldGuideSaveStore.TryCapturePersisted(_fieldGuide.CaptureState())))
            {
                GD.Print("[Ashfall Godot] Field guide save written.");
            }
        }

        private void FlushFieldGuideIfDirty()
        {
            // Day-advance flush performs a full silent SaveAll; this keeps the
            // triad shape consistent for the registry gate.
            SaveFieldGuide();
        }
    }
}
