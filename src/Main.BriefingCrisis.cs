// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : C1.4 deferred consumer — crisis predictions in the daily
// briefing. Assembles read-only CrisisPredictionInputs from the canonical
// campaign owners (roster, inventory, power grid, sanitation, weather
// intelligence) and renders them through DailyBriefingReportBuilder.
// Presentation/aggregation only — CrisisPredictor owns the policy.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>
        /// Build the briefing's crisis predictions from authoritative read-only
        /// state. Inputs with no clear canonical source are intentionally left
        /// at the predictor's neutral defaults (no invented values):
        /// daily burn rates fall back to the living-survivor count, and the
        /// distress/demographic classes stay inert until their owners expose a
        /// matching signal. Deterministic: zero RNG, zero mutation.
        /// </summary>
        private IReadOnlyList<CrisisPredictionRecord> BuildBriefingCrisisPredictions(int day)
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = day,
                DeadlineMultiplier = _difficultyScalars?.CrisisDeadlineMult ?? 1f
            };

            // ── Roster: living count + radiation exposure ────────────────
            SetupSurvivors();
            int living = 0;
            float doseSum = 0f;
            float maxDose = 0f;
            var roster = _survivors?.RosterState;
            if (roster != null)
            {
                foreach (var s in roster)
                {
                    if (s == null || !s.IsAlive || s.IsDead) continue;
                    living++;
                    float dose = _survivors?.RadStateFor(s.Id)?.RadiationDose ?? 0f;
                    doseSum += dose;
                    if (dose > maxDose) maxDose = dose;
                }
            }
            inputs.LivingSurvivorCount = living;
            inputs.MaxRadiationDose = maxDose;
            inputs.AverageRadiationDose = living > 0 ? doseSum / living : 0f;

            // ── Food & water stock (by canonical item type) ─────────────
            SetupInventory();
            var inv = _inventory?.Inventory;
            if (inv != null)
            {
                inputs.FoodStockUnits = inv.CountByType(ItemType.Food)
                    + inv.CountByType(ItemType.ContaminatedFood);
                inputs.WaterStockUnits = inv.CountByType(ItemType.Water)
                    + inv.CountByType(ItemType.IrradiatedWater);
            }

            // ── Power grid runway ───────────────────────────────────────
            SetupPowerGrid();
            var grid = _powerGrid?.System;
            if (grid != null)
            {
                inputs.BatteryReserveWh = grid.BatteryReserveWh;
                inputs.BatteryCapacityWh = grid.BatteryCapacityWh;
                inputs.GenerationWatts = grid.GenerationWatts;
                inputs.TotalDrawWatts = grid.TotalDrawWatts;
                inputs.FuelUnits = grid.FuelUnits;
                inputs.FuelRunwayDays = grid.EstimatedFuelRunwayDays;
            }

            // ── Sanitation spill (pathogen exposure) ────────────────────
            SetupSanitation();
            var sanitation = _sanitation?.System;
            if (sanitation?.ActiveSpill != null)
            {
                inputs.ActiveSanitationSpills = 1;
                inputs.PathogenExposureModifier = 1f + sanitation.ActiveSpill.severity;
            }

            // ── Weather forecast + station quality ──────────────────────
            SetupWorld();
            var intelligence = _world?.WeatherIntelligence?.BuildReadModel();
            if (intelligence != null)
            {
                inputs.Forecast = intelligence.forecast;
                inputs.StationAccuracy = intelligence.stationAccuracy;
                inputs.IsStationCalibrated = intelligence.stationCalibrated;
                inputs.IsStationOperational = intelligence.stationOperational;
            }

            return CrisisPredictor.Evaluate(inputs);
        }
    }
}
