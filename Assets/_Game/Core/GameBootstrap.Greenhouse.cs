using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Utilities;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: THE GLASS ORCHARD (Expansion XI) — host wiring for the bunker
    /// greenhouse. Called from <see cref="GameBootstrap.InitDeepLore"/> after the
    /// other narrative expansions boot. The system is constructed and tick-wired
    /// here; its <see cref="ISaveable"/> registration lives in
    /// <c>RegisterExpansionSaveables</c> alongside the other expansion saveables.
    ///
    /// The greenhouse is player-reachable through the existing event modal +
    /// journal loop: the system auto-simulates on the daily tick and raises
    /// events the host routes to <c>TriggerEventById</c>; the authored
    /// greenhouse events' choices are resolved here into system calls
    /// (TreatBlight / Clear / UnlockPreWarWheat / SurgeContamination). A
    /// dedicated HUD panel is deliberately out of scope (Phase 11 workstream).
    /// </summary>
    public partial class GameBootstrap
    {
        public GreenhouseSystem Greenhouse { get; private set; }

        // Plot index of the most recent blight outbreak, used by the blight
        // event's choices. -1 = none pending.
        private int _greenhouseBlightPlot = -1;

        /// <summary>Grow-lamp light bonus per owned lamp, in hours/day.</summary>
        private const float GreenhouseGrowLampHoursEach = 6f;
        /// <summary>Baseline nuclear-winter daylight available to a crop, in hours/day.</summary>
        private const float GreenhouseBaseWinterLightHours = 3f;
        /// <summary>Contamination surge when the lead-glass breaks and is left unfixed.</summary>
        private const float GreenhouseGlassBreakContaminationSurge = 15f;

        private void BootGreenhouse()
        {
            Greenhouse = new GreenhouseSystem(_worldSeed + 911);

            MergeGreenhouseItems();
            MergeGreenhouseLocations();

            WireGreenhouseEvents();

            // Daily tick (growth/water/contamination/blight) + diagnostics tracking.
            _registry.RegisterDaily("greenhouse", TickGreenhouseDaily);
            _registry.Register(Greenhouse);

            GameLog.Log("[GameBootstrap] Glass Orchard booted: greenhouse system + items + locations.");
        }

        // ═══════════════════════════════════════════════════════════════
        // Content merge — items + locations into the runtime catalogs
        // ═══════════════════════════════════════════════════════════════

        private void MergeGreenhouseItems()
        {
            if (_itemCatalog == null) return;
            var defs = GreenhouseItemsCatalogLoader.MaterialiseAll();
            int added = 0;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                if (_itemCatalog.GetById(d.id) != null) continue;
                _itemCatalog.items.Add(d);
                added++;
            }
            if (added > 0)
                GameLog.Log("[GameBootstrap] Greenhouse items merged: " + added);
        }

        private void MergeGreenhouseLocations()
        {
            if (_locationCatalog == null) return;
            if (_locationCatalog.locations == null)
                _locationCatalog.locations = new List<LocationDefinitionSO>();

            int added = 0;
            var defs = GreenhouseLocations();
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                if (_locationCatalog.GetById(d.id) != null) continue;
                _locationCatalog.locations.Add(d);
                added++;
            }
            if (added > 0)
                GameLog.Log("[GameBootstrap] Greenhouse locations merged: " + added);
        }

        /// <summary>The four greenhouse scavenge locations, materialised at runtime.</summary>
        private static List<LocationDefinitionSO> GreenhouseLocations()
        {
            return new List<LocationDefinitionSO>
            {
                Loc(GreenhouseExpansionCatalog.Locations.GlasshouseRuins,
                    "Glasshouse Ruins", 4f, 2.0f, 18f,
                    "The municipal glasshouse, its panes long since starred and caved by ashfall. Under the drifted soot, the planter beds are still intact — and so, sometimes, is the salvage left by the people who came to grow and never left."),
                Loc(GreenhouseExpansionCatalog.Locations.SeedVault,
                    "Seed Vault", 7f, 4.0f, 8f,
                    "A deep, cold room behind a door marked with the old civil-defense trefoil. Low radiation, high danger: the vault kept its contents and its locks. Whatever is still sealed in the cold is exactly what someone would need to make the bunker's planter boxes mean something."),
                Loc(GreenhouseExpansionCatalog.Locations.HydroBaronsAquaponics,
                    "Hydro Barons' Aquaponics", 3f, 2.5f, 12f,
                    "The Hydro Barons run their tanks in the flooded lower floor of an old food-processing plant. They trade grow-lamp filaments and sterile grow medium, and they watch the water level of their walls the way other people watch the sky."),
                Loc(GreenhouseExpansionCatalog.Locations.RotFarmersCompostYard,
                    "Rot Farmers' Compost Yard", 5f, 1.5f, 22f,
                    "The Rot Farmers work their compost in the lee of a collapsed overpass, turning the city's dead soil into something that will, eventually, grow. The smell is unforgettable. So is the yield — and the contamination that comes with it.")
            };
        }

        private static LocationDefinitionSO Loc(string id, string name,
            float danger, float travel, float rads, string desc)
        {
            var so = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            so.id = id;
            so.displayName = name;
            so.dangerLevel = danger;
            so.travelHours = travel;
            so.baseRadsPerHour = rads;
            so.description = desc;
            return so;
        }

        // ═══════════════════════════════════════════════════════════════
        // Event wiring — system events → narrative; choices → system calls
        // ═══════════════════════════════════════════════════════════════

        private void WireGreenhouseEvents()
        {
            if (Greenhouse == null) return;

            // First plant → the "First Sprout" vignette (fire-once via world flag).
            Greenhouse.OnCropPlanted += (plotIndex, seedId, day) =>
            {
                if (SaveSystem != null && SaveSystem.GetWorldFlag(GreenhouseExpansionCatalog.Flags.FirstSproutSeen))
                    return;
                AddGreenhouseJournalEntry("greenhouse_first_plant",
                    "Something has been planted in the planter box. A small thing, done with cold hands. It is the first deliberate act of growing since the sky changed.",
                    day);
                TriggerEventById(GreenhouseExpansionCatalog.Events.FirstSprout);
            };

            Greenhouse.OnCropMatured += (plotIndex, seedId) =>
            {
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                AddGreenhouseJournalEntry("greenhouse_crop_matured_" + seedId,
                    "A crop has come ready in the planter box. For a moment, no one mentions how little of it there is — only that it is there at all.",
                    day);
            };

            Greenhouse.OnCropHarvested += harvest =>
            {
                if (!harvest.success) return;
                GrantGreenhouseYield(harvest);
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                if (harvest.contaminated)
                {
                    AddGreenhouseJournalEntry("greenhouse_tainted_yield_" + day,
                        "The harvest is in. It came up dark at the root — the soil remembered. It is food. It is also a debt, to be paid later, slowly.",
                        day);
                    TriggerEventById(GreenhouseExpansionCatalog.Events.TaintedHarvest);
                }
                else
                {
                    AddGreenhouseJournalEntry("greenhouse_clean_yield_" + day,
                        "The harvest is in, clean enough to trust. A small weight off the ration shelf, and a smaller one off the mind.",
                        day);
                }
            };

            Greenhouse.OnBlightOutbreak += plotIndex =>
            {
                _greenhouseBlightPlot = plotIndex;
                GameLog.Log("[Greenhouse] Blight outbreak on plot " + plotIndex + ".");
                TriggerEventById(GreenhouseExpansionCatalog.Events.BlightOutbreak);
            };

            Greenhouse.OnPlotDriedOut += plotIndex =>
                GameLog.LogWarning("[Greenhouse] Plot " + plotIndex + " has run dry — growth stalled, blight risk rising.");

            Greenhouse.OnCropFailed += plotIndex =>
            {
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                AddGreenhouseJournalEntry("greenhouse_crop_failed_" + plotIndex + "_" + day,
                    "A plot has gone black and been lost. The soil will hold the memory of it for a while yet.",
                    day);
            };

            // Authored-event choices → system mutations.
            if (EventRunner != null)
            {
                Action<GameEvent, EventChoice, EventContext> onChoice = HandleGreenhouseEventChoice;
                EventRunner.OnChoiceApplied += onChoice;
                _subscriptions.Track(() => EventRunner.OnChoiceApplied -= onChoice);
            }

            // Defensive seed-ledger hook: the Svalbard Seed Ledger is currently
            // a dormant ghost (not boot-wired), so this is usually a no-op — but
            // if a future task revives it, the wheat unlock flows automatically.
            // The in-game unlock path today is the "dead gardener" event below.
            if (ItemSeedLedger != null)
            {
                Action<SeedLedgerState, string> onCropUnlocked = (state, crop) =>
                {
                    if (Greenhouse != null && !Greenhouse.IsPreWarWheatUnlocked)
                    {
                        Greenhouse.UnlockPreWarWheat();
                        SaveSystem?.SetWorldFlag(GreenhouseExpansionCatalog.Flags.WheatUnlocked, true);
                        GameLog.Log("[Greenhouse] Pre-war wheat unlocked via Seed Ledger.");
                    }
                };
                ItemSeedLedger.OnCropUnlocked += onCropUnlocked;
                _subscriptions.Track(() => ItemSeedLedger.OnCropUnlocked -= onCropUnlocked);
            }

            // On load, restore the wheat unlock from its persisted world flag
            // (covers saves where the ledger/gardener granted it before).
            if (Greenhouse != null && !Greenhouse.IsPreWarWheatUnlocked
                && SaveSystem != null && SaveSystem.GetWorldFlag(GreenhouseExpansionCatalog.Flags.WheatUnlocked))
            {
                Greenhouse.UnlockPreWarWheat();
            }
        }

        /// <summary>
        /// Resolve authored greenhouse events' choices into system calls. Item
        /// grants/consumes that are unconditional are handled by the events'
        /// own <see cref="EventEffect"/>s; this handler only does the parts the
        /// event runner cannot — conditional system mutations.
        /// </summary>
        private void HandleGreenhouseEventChoice(GameEvent evt, EventChoice choice, EventContext ctx)
        {
            if (evt == null || choice == null || string.IsNullOrEmpty(evt.id)) return;
            if (Greenhouse == null) return;
            string id = evt.id;
            string choiceId = choice.ChoiceId;

            if (id == GreenhouseExpansionCatalog.Events.BlightOutbreak)
            {
                HandleBlightChoice(choice);
            }
            else if (id == GreenhouseExpansionCatalog.Events.DeadGardener)
            {
                // Taking the sealed tin is the in-game wheat-unlock path.
                if (choiceId == "take_seed_tin" && !Greenhouse.IsPreWarWheatUnlocked)
                {
                    Greenhouse.UnlockPreWarWheat();
                    SaveSystem?.SetWorldFlag(GreenhouseExpansionCatalog.Flags.WheatUnlocked, true);
                    GameLog.Log("[Greenhouse] Pre-war wheat unlocked via the dead gardener's tin.");
                }
            }
            else if (id == GreenhouseExpansionCatalog.Events.GlassBreaks)
            {
                if (choiceId == "do_nothing")
                    Greenhouse.SurgeContamination(GreenhouseGlassBreakContaminationSurge);
                else if (choiceId == "sacrifice_crop")
                    Greenhouse.Clear(FirstNonFallowPlotIndex());
            }
        }

        private void HandleBlightChoice(EventChoice choice)
        {
            int plot = _greenhouseBlightPlot;
            if (plot < 0) return;

            if (choice.ChoiceId == "treat_it")
            {
                // Consume a treatment only if the system can actually apply it
                // (a Failed crop is dead — treatment cannot revive it).
                if (Greenhouse.TreatBlight(plot, out _))
                    Inventory?.RemoveById(GreenhouseExpansionCatalog.Items.BlightTreatment, 1);
            }
            else if (choice.ChoiceId == "burn_crop")
            {
                Greenhouse.Clear(plot);
            }
            // "leave_it": the blight stays; the next tick will continue to accrue.

            _greenhouseBlightPlot = -1;
        }

        // ═══════════════════════════════════════════════════════════════
        // Daily tick — translate host state into the system's two inputs
        // ═══════════════════════════════════════════════════════════════

        private void TickGreenhouseDaily(int day)
        {
            if (Greenhouse == null) return;

            // One plot per owned planter box (new boxes add fallow plots;
            // growing crops are never destroyed by box removal).
            int boxes = Inventory != null
                ? Inventory.CountById(GreenhouseExpansionCatalog.Items.PlanterBox)
                : 0;
            Greenhouse.EnsurePlots(boxes);

            Greenhouse.TickDay(day, GetDailyGrowLightHours(), GetAshContaminationRate());
        }

        /// <summary>
        /// Photoperiod + owned grow lamps. Nuclear winter daylight is thin; each
        /// grow lamp adds a flat slice of usable spectrum. (A future task can
        /// route this through PhotoperiodSystem instead of this heuristic.)
        /// </summary>
        private float GetDailyGrowLightHours()
        {
            float hours = GreenhouseBaseWinterLightHours;
            if (Inventory != null)
            {
                int lamps = Inventory.CountById(GreenhouseExpansionCatalog.Items.GrowLamp);
                hours += Mathf.Min(lamps, 2) * GreenhouseGrowLampHoursEach;
            }
            return hours;
        }

        /// <summary>
        /// Net ash-contamination drift into the planter beds per day. Ashy
        /// weather raises it; lead-glass panes shield the soil (each owned pane
        /// counts, modelled as a flat reduction in line with how shielding is
        /// tracked elsewhere in the bunker).
        /// </summary>
        private float GetAshContaminationRate()
        {
            var weather = WeatherSystem != null ? WeatherSystem.Current : WeatherKind.Clear;
            float rate = IsAshyWeather(weather) ? 2.0f : 0.3f;
            if (Inventory != null && Inventory.CountById(GreenhouseExpansionCatalog.Items.LeadGlassPane) > 0)
                rate *= 0.4f;
            return rate;
        }

        private static bool IsAshyWeather(WeatherKind w)
        {
            switch (w)
            {
                case WeatherKind.Ashfall:
                case WeatherKind.BlackRain:
                case WeatherKind.BlackSnow:
                case WeatherKind.AshLightning:
                case WeatherKind.ParticulateFog:
                case WeatherKind.FalloutStorm:
                    return true;
                default:
                    return false;
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Helpers — yield grants, journal entries, plot selection
        // ═══════════════════════════════════════════════════════════════

        private void GrantGreenhouseYield(GreenhouseHarvest harvest)
        {
            if (Inventory == null || string.IsNullOrEmpty(harvest.yieldItemId) || harvest.amount <= 0)
                return;
            var def = _itemCatalog != null ? _itemCatalog.GetById(harvest.yieldItemId) : null;
            if (def == null)
            {
                GameLog.LogWarning("[Greenhouse] Yield grant skipped — unknown id " + harvest.yieldItemId);
                return;
            }
            Inventory.Add(def, harvest.amount);
        }

        private void AddGreenhouseJournalEntry(string key, string text, int day)
        {
            if (JournalSystem == null || string.IsNullOrEmpty(key)) return;
            JournalSystem.TryAddRawEntry(key, text, null, day);
        }

        /// <summary>Index of the first planted (non-fallow) plot, or -1 if none.</summary>
        private int FirstNonFallowPlotIndex()
        {
            if (Greenhouse == null) return -1;
            var plots = Greenhouse.Plots;
            for (int i = 0; i < plots.Count; i++)
                if (!GreenhouseSystem.IsFallow(plots[i])) return i;
            return -1;
        }
    }
}
