// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Main.Plans162_165 — flagship survival layer wiring
// Plan 162     : AgricultureSystem + NutritionDiversitySystem (this file).
// Plans 163-165: wired in later phases onto the same partial.
// Ownership    : the host composes the environment snapshot (weather, power,
//      water treatment), gates every command on the canonical inventory, and
//      applies nutrition-deficiency consequences through NeedsSystem.Modify.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AgricultureHostSession _agriculture = null!;
        private bool _agricultureDirty;
        private UI.FarmingPanel _farmingPanel = null!;
        private bool _farmingPanelBound;

        // ------------------------------------------------------------------
        // Setup / Save
        // ------------------------------------------------------------------

        private void SetupAgriculture()
        {
            if (_agriculture != null) return;
            SetupGreenhouse();
            SetupInventory();

            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var strainCatalog = CropStrainCatalogLoader.Load(_dataDir, fileIO, json);
            var nutritionCatalog = NutritionProfileCatalogLoader.Load(_dataDir, fileIO, json);

            var agriSystem = new AgricultureSystem(_greenhouse.System);
            agriSystem.LoadCatalog(strainCatalog);
            var nutrition = new NutritionDiversitySystem();
            nutrition.LoadCatalog(nutritionCatalog);

            var saved = AgricultureSaveStore.TryLoad();
            if (saved != null)
            {
                agriSystem.RestoreState(saved.agriculture);
                nutrition.RestoreState(saved.nutrition);
            }

            _agriculture = new AgricultureHostSession(agriSystem, nutrition);
            _agriculture.StateChanged += () => _agricultureDirty = true;

            // The greenhouse owns blight; agriculture only dedupes the narrative
            // episode (one journal entry per outbreak, plan §5.23).
            _greenhouse.System.OnBlightOutbreak += plotIndex => _agriculture.System.NotifyBlightOutbreak(plotIndex);

            _agriculture.System.OnFirstHarvest += plot =>
                _journal?.TryAddRawEntry("agriculture_first_harvest",
                    "First harvest under the glass. The bench holds something we grew ourselves.",
                    null!, _simDay);
            _agriculture.System.OnPlotInfested += (plot, pest) =>
                _journal?.TryAddRawEntry($"agriculture_infestation_{plot}",
                    $"Plot {plot + 1}: {pest.Replace("infestation_", "").Replace('_', ' ')} found on the bench.",
                    null!, _simDay);
            _agriculture.System.OnBlightNarrative += plot =>
                _journal?.TryAddRawEntry($"agriculture_blight_{plot}",
                    $"Plot {plot + 1}: blight is spreading. Treat it or lose the crop.",
                    null!, _simDay);

            // Nutrition: record every consumed food item through the canonical
            // inventory consumption path (plan §5.19 — diet, not crop, drives state).
            _inventory.OnConsumed += (survivorId, itemId) =>
            {
                var def = _inventory.Catalog.Get(itemId);
                if (def != null && (def.type == ItemType.Food || def.hungerRestore > 0f))
                    _agriculture.Nutrition.RecordMeal(survivorId ?? "", itemId, _simDay);
            };
        }

        private void SaveAgriculture()
        {
            if (_agriculture == null) return;
            var payload = AgricultureSaveStore.TryCapturePersisted(new AgricultureCampaignState
            {
                agriculture = _agriculture.System.CaptureState(),
                nutrition = _agriculture.Nutrition.CaptureState()
            });
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(AgricultureSaveStore.SectionName, payload);
                _agricultureDirty = false;
            }
        }

        // ------------------------------------------------------------------
        // Day tick (called from GreenhouseFoundryDayOwner, phase 2)
        // ------------------------------------------------------------------

        private AgricultureEnvironmentSnapshot BuildAgricultureEnvironment(int day)
        {
            var weather = _world?.Weather;
            bool lightsPowered = _powerGrid?.System != null && _powerGrid.System.IsRoomPowered("room_greenhouse");
            string season = weather?.GetSeasonForDay(day)?.id ?? "any";
            return new AgricultureEnvironmentSnapshot
            {
                TemperaturePenaltyC = weather?.GetTemperaturePenaltyCelsius() ?? 0f,
                OutdoorRadModifier = weather?.OutdoorRadModifier ?? 100f,
                LightingAvailabilityPermille = lightsPowered ? 1000f : 0f,
                AshContaminationRate = weather != null && weather.Current == WeatherKind.FalloutStorm ? 0.08f : 0.04f,
                SeasonWindowId = season
            };
        }

        /// <summary>
        /// One agriculture day: environment snapshot → RNG forks → Core tick
        /// (which ticks the greenhouse growth authority exactly once) →
        /// nutrition evaluation → deficiency consequences via NeedsSystem.
        /// </summary>
        private void TickAgricultureDay(int day)
        {
            SetupAgriculture();
            var env = BuildAgricultureEnvironment(day);
            var pestRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.AgriculturePest, day, 0)
                : new SeededRng(1620 + day);
            var mutationRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.AgricultureMutation, day, 0)
                : new SeededRng(1621 + day);
            _agriculture.System.TickDay(day, env, pestRng, mutationRng, env.SeasonWindowId);

            _agriculture.Nutrition.TickDay(day);
            ApplyNutritionDeficiencyModifiers();
            _farmingPanel?.RefreshView();
        }

        /// <summary>Deficiency pressure surfaces through the canonical needs
        /// authority: each deficient category wears on morale (max -3/day).</summary>
        private void ApplyNutritionDeficiencyModifiers()
        {
            if (_survivors?.Needs == null) return;
            foreach (var s in _survivors.Needs.Registered)
            {
                if (s == null) continue;
                float pressure = _agriculture.Nutrition.DeficiencyPressure(s.Id);
                if (pressure > 0f)
                    _survivors.Needs.Modify(s.Id, NeedKind.Morale, Math.Min(3f, pressure * 3f));
            }
        }

        // ------------------------------------------------------------------
        // Player actions (routed from FarmingPanel via Main.UiPanels wiring)
        // ------------------------------------------------------------------

        private void HandleAgricultureAction(string action, string param)
        {
            if (action == "OPEN")
            {
                SetupAgriculture();
                if (!_farmingPanelBound && _agriculture != null)
                {
                    _farmingPanel.Bind(_agriculture);
                    _farmingPanelBound = true;
                }
                _farmingPanel.Open();
                return;
            }
            if (action == "CLOSE")
            {
                _farmingPanel.Close();
                return;
            }

            if (_agriculture == null) return;
            SetupInventory();
            var inv = _inventory.Inventory;
            var sys = _agriculture.System;
            int day = _simDay;
            bool ok = false;
            string? feedback = null;

            int plot = int.TryParse(param, out var p) ? p : -1;

            switch (action)
            {
                case "PLANT": // param: "plotIndex|strainId"
                {
                    int bar = param?.IndexOf('|') ?? -1;
                    if (bar > 0 && int.TryParse(param.Substring(0, bar), out int pi))
                    {
                        string strainId = param.Substring(bar + 1);
                        var strain = sys.Strain(strainId);
                        if (strain == null) { feedback = "Unknown strain."; break; }
                        if (!sys.CanPlantStrain(pi, strainId)) { feedback = "Plot occupied or invalid."; break; }
                        if (!inv.HasSufficient(strain.seed_item_id, 1)) { feedback = $"No {strain.seed_item_id} in inventory."; break; }
                        if (inv.TryConsume(strain.seed_item_id, 1))
                            ok = sys.PlantWithStrain(pi, strainId, day);
                    }
                    break;
                }
                case "WATER_CLEAN":
                case "WATER_TAINTED":
                {
                    bool tainted = action == "WATER_TAINTED";
                    string waterItem = tainted ? "irradiated_water" : "clean_water";
                    if (plot < 0) break;
                    if (!inv.HasSufficient(waterItem, 5)) { feedback = $"Needs 5 × {waterItem}."; break; }
                    if (inv.TryConsume(waterItem, 5))
                    {
                        var band = tainted
                            ? AgriWaterBand.Unsafe
                            : (_waterTreatment != null && _waterTreatment.System.State.incomingContaminationLevel >= 0.5f
                                ? AgriWaterBand.Marginal
                                : AgriWaterBand.Clean);
                        sys.Water(plot, band);
                        ok = true;
                    }
                    break;
                }
                case "TREAT":
                {
                    if (plot < 0) break;
                    if (!sys.CanTreatPest(plot, "item_pest_treatment_dust")) { feedback = "No treatable infestation."; break; }
                    if (!inv.HasSufficient("item_pest_treatment_dust", 1)) { feedback = "No pest treatment dust held."; break; }
                    if (inv.TryConsume("item_pest_treatment_dust", 1))
                        ok = sys.TryTreatPestInfestation(plot, "item_pest_treatment_dust");
                    break;
                }
                case "COMPOST_START": // param: recipeId
                {
                    var recipe = sys.CompostRecipe(param ?? "");
                    if (recipe == null) { feedback = "Unknown compost recipe."; break; }
                    if (!inv.HasSufficient(recipe.input_item_id, recipe.input_count))
                    { feedback = $"Needs {recipe.input_count} × {recipe.input_item_id}."; break; }
                    if (inv.TryConsume(recipe.input_item_id, recipe.input_count))
                        ok = sys.TryStartCompostBatch(recipe.id, day);
                    break;
                }
                case "COMPOST_COLLECT": // param: recipeId
                {
                    int output = sys.TryCollectCompost(param ?? "", day);
                    if (output > 0)
                    {
                        var recipe = sys.CompostRecipe(param!);
                        inv.AddById(recipe!.output_item_id, output);
                        ok = true;
                    }
                    else feedback = "No finished batch.";
                    break;
                }
                case "COMPOST_APPLY":
                {
                    if (plot < 0) break;
                    if (!inv.HasSufficient("item_compost_humus", 1)) { feedback = "No compost humus held."; break; }
                    if (inv.TryConsume("item_compost_humus", 1))
                        ok = sys.TryApplyCompost(plot);
                    break;
                }
                case "HARVEST":
                {
                    if (plot < 0) break;
                    var harvest = sys.Harvest(plot);
                    if (harvest.success)
                    {
                        if (harvest.finalAmount > 0)
                            inv.AddById(harvest.yieldItemId, harvest.finalAmount);
                        ok = true;
                    }
                    else feedback = "Plot is not ready for harvest.";
                    break;
                }
                case "CLEAR":
                {
                    if (plot < 0) break;
                    ok = sys.ClearPlot(plot);
                    break;
                }
            }

            if (!ok && feedback != null)
                _agriculture.MarkDirty(feedback);
            _farmingPanel?.RefreshView();
        }

        // ==================================================================
        // Plan 163 — DefenseSystem (traps + pre-combat raid resolution)
        // ==================================================================

        private DefenseHostSession _defense = null!;
        private bool _defenseDirty;
        private UI.DefenseGridPanel _defenseGridPanel = null!;
        private bool _defenseGridPanelBound;

        private void SetupDefense()
        {
            if (_defense != null) return;
            SetupInventory();

            var traps = Ashfall.Core.Defense.TrapCatalogLoader.Load(_dataDir);
            var system = new Ashfall.Core.Defense.DefenseSystem(traps);
            var saved = DefenseSaveStore.TryLoad();
            if (saved != null)
                system.RestoreState(saved);

            _defense = new DefenseHostSession(system);
            _defense.StateChanged += () => _defenseDirty = true;

            // Capture handoff: DefenseSystem only reports the fact; the
            // PrisonerSystem is the single captive authority (plan §6.13-6.14).
            _defense.OnCaptivesToHandOff += (day, count) =>
            {
                var prisoners = EnsurePrisoners();
                int taken = 0;
                for (int i = 0; i < count; i++)
                {
                    string captiveId = $"captive_raid_{day}_{i + 1}";
                    if (prisoners.TakePrisoner(captiveId, "faction_iron_raiders", day))
                        taken++;
                }
                _journal?.TryAddRawEntry($"raid_captures_{day}",
                    taken > 0
                        ? $"{taken} raider(s) taken alive from the pit and moved to the cells."
                        : "The pit held raiders, but the cells are full — they were turned loose at the wire.",
                    null!, _simDay);
            };

            _defense.System.OnTrapSprung += (installationId, trapId) =>
                _journal?.TryAddRawEntry($"trap_sprung_{installationId}",
                    $"A {trapId.Replace("trap_", "").Replace('_', ' ')} sprang at the wire.",
                    null!, _simDay);
        }

        private void SaveDefense()
        {
            if (_defense == null) return;
            var payload = DefenseSaveStore.TryCapturePersisted(_defense.System.CaptureState());
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(DefenseSaveStore.SectionName, payload);
                _defenseDirty = false;
            }
        }

        /// <summary>
        /// Plan 163 pre-combat phase: static defenses (traps, then perimeter
        /// emplacements) resolve BEFORE direct survivor combat. Returns the
        /// engagement; the caller escalates only what breached.
        /// </summary>
        private Ashfall.Core.Defense.DefenseEngagementResult ResolveRaidDefenses(
            int day, int raiderStrength, bool isNight)
        {
            SetupDefense();
            var targetingRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.DefenseTargeting, day, 0)
                : new SeededRng(1630 + day);
            var captureRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.DefenseCapture, day, 0)
                : new SeededRng(1631 + day);
            // Turrets draw from the grid: emplacements are exterior hardware,
            // so their power query is the grid-level state (no invented room).
            return _defense.System.ResolvePreCombatRaid(
                day, raiderStrength, isNight,
                _perimeterDefense,
                _ => _powerGrid?.System == null || !_powerGrid.System.IsBrownout,
                targetingRng, captureRng);
        }

        private void HandleDefenseAction(string action, string param)
        {
            if (action == "OPEN")
            {
                SetupDefense();
                if (!_defenseGridPanelBound && _defense != null)
                {
                    _defenseGridPanel.Bind(_defense);
                    _defenseGridPanelBound = true;
                }
                _defenseGridPanel.Open();
                return;
            }
            if (action == "CLOSE")
            {
                _defenseGridPanel.Close();
                return;
            }

            if (_defense == null) return;
            SetupInventory();
            var inv = _inventory.Inventory;
            var sys = _defense.System;
            bool ok = false;
            string? feedback = null;

            // param shapes: INSTALL:<trapId>|<placement> or <installationId>
            int colon = param?.IndexOf(':') ?? -1;
            string arg = colon > 0 ? param!.Substring(colon + 1) : param ?? "";

            switch (action)
            {
                case "INSTALL": // param "trapId|placement"
                {
                    int bar = arg.IndexOf('|');
                    if (bar > 0)
                    {
                        string trapId = arg.Substring(0, bar);
                        string placement = arg.Substring(bar + 1);
                        var def = sys.FindTrapDefinition(trapId);
                        if (def == null) { feedback = "Unknown trap."; break; }
                        bool costsOk = true;
                        foreach (var cost in def.build_costs)
                            if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
                        if (!costsOk) break;
                        ok = sys.InstallTrap(trapId, placement, (item, n) => inv.TryConsume(item, n));
                    }
                    break;
                }
                case "RESET":
                {
                    if (!sys.CanResetTrap(arg)) { feedback = "Trap must be sprung and intact."; break; }
                    var inst = sys.FindInstallation(arg);
                    var def = sys.FindTrapDefinition(inst!.trap_id);
                    bool costsOk = true;
                    foreach (var cost in def!.reset_costs)
                        if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
                    if (!costsOk) break;
                    ok = sys.TryResetTrap(arg, (item, n) => inv.TryConsume(item, n));
                    break;
                }
                case "REPAIR":
                {
                    if (!sys.CanRepairTrap(arg)) { feedback = "Nothing to repair."; break; }
                    var inst = sys.FindInstallation(arg);
                    var def = sys.FindTrapDefinition(inst!.trap_id);
                    bool costsOk = true;
                    foreach (var cost in def!.repair_costs)
                        if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
                    if (!costsOk) break;
                    ok = sys.TryRepairTrap(arg, (item, n) => inv.TryConsume(item, n));
                    break;
                }
                case "SIMULATE": // manual drill: resolve against a dummy squad
                {
                    var result = ResolveRaidDefenses(_simDay, int.TryParse(arg, out var s) && s > 0 ? Math.Min(s, 12) : 4, isNight: false);
                    ok = true;
                    _journal?.TryAddRawEntry($"defense_drill_{_simDay}",
                        $"Defense drill against {result.InitialRaiderStrength} simulated raiders: {(result.Repelled ? "repelled." : $"breached with {result.RemainingRaiders} through.")}",
                        null!, _simDay);
                    break;
                }
            }

            if (!ok && feedback != null)
                _defense.MarkDirty(feedback);
            _defenseGridPanel?.RefreshView();
        }
    }
}
