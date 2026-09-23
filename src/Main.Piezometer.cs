// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 189 — Piezometer intake advisory host wire
// Subsystems   : aquifer monitoring network construction, daily advisory
//                handoff into the water-treatment intake gate, dedicated save.
// Contract     : the piezometer forecasts; WaterTreatmentSystem owns liters and
//                admits/refuses intake. No new liter ledger is created here.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PiezometerHostSession? _piezometer;

        /// <summary>Panel/CLI-facing session.</summary>
        public PiezometerHostSession EnsurePiezometerSession()
        {
            SetupPiezometer();
            return _piezometer!;
        }

        private void SetupPiezometer()
        {
            if (_piezometer != null) return;

            var engine = new AquiferPiezometerEngine(log: new GodotLog())
            {
                DayProvider = () => _simDay,
                // Plan 115 host bindings: aquifer drawdown reads the canonical
                // fluid network's reservoir draw, recharge reads the authored
                // seasonal rain share, and specialist skills read the shared
                // skill-progression authority. Unbound systems keep the
                // engine's documented neutral defaults.
                PumpDemandProvider = () =>
                {
                    var fluid = _fluidLogistics168?.System;
                    var nodes = fluid?.State?.nodes;
                    var reservoir = nodes?.Find(n => n != null && n.nodeId == FluidLogisticsSystem.DefaultReservoirId);
                    float capacity = reservoir?.capacity ?? 0f;
                    if (fluid == null || capacity <= 0.01f) return 0.4f;
                    return Math.Clamp(fluid.LastReport?.deliveredVolume / capacity ?? 0f, 0f, 1f);
                },
                SeasonalRechargeModifierProvider = () =>
                {
                    var weather = _world?.Weather;
                    var season = weather?.GetSeasonForDay(_simDay);
                    var seasons = weather?.Profile?.seasons;
                    if (season == null || seasons == null || seasons.Count == 0) return 0.5f;

                    float wettest = 0f;
                    for (int i = 0; i < seasons.Count; i++)
                        if (seasons[i] != null) wettest = Math.Max(wettest, SeasonRainShare(seasons[i]));
                    if (wettest <= 0.001f) return 0.5f;
                    return Math.Clamp(SeasonRainShare(season) / wettest, 0f, 1f);
                },
                HydrogeologistSkillProvider = () => BestDisciplineProgress("science"),
                WellDrillerSkillProvider = () => BestDisciplineProgress("crafting")
            };

            var inv = _inventory?.Inventory;
            if (inv != null)
            {
                engine.BindInventory(
                    itemId => inv.CountById(itemId),
                    (itemId, count) => inv.TryConsumeById(itemId, count));
            }

            _piezometer = new PiezometerHostSession(engine);
            _piezometer.LoadCatalog(_dataDir);

            var saved = PiezometerSaveStore.TryLoad();
            if (saved != null)
            {
                _piezometer.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Aquifer piezometer network restored.");
            }
        }

        /// <summary>Share of a season's weather rolls that is rain (including black rain).</summary>
        private static float SeasonRainShare(SeasonWindowDef season)
        {
            float total = season.clearWeight + season.rainWeight + season.overcastWeight + season.ashfallWeight
                + season.falloutStormWeight + season.blizzardWeight + season.blackRainWeight;
            if (total <= 0.001f) return 0f;
            return (season.rainWeight + season.blackRainWeight) / total;
        }

        /// <summary>Best living survivor's progress (0..1) in one skill discipline.</summary>
        private float BestDisciplineProgress(string disciplineId)
        {
            var roster = _survivors?.RosterState;
            if (roster == null || roster.Count == 0 || string.IsNullOrEmpty(disciplineId)) return 0f;

            var skills = EnsureSharedSkillProgression();
            float best = 0f;
            for (int i = 0; i < roster.Count; i++)
            {
                var survivor = roster[i];
                if (survivor == null || !survivor.IsAliveState || string.IsNullOrEmpty(survivor.Id)) continue;
                float progress = skills.GetDisciplineProgress01(survivor.Id, disciplineId);
                if (progress > best) best = progress;
            }
            return Math.Clamp(best, 0f, 1f);
        }

        /// <summary>Install the monitoring network (consumes catalog install materials).</summary>
        public string ConstructPiezometerNetwork()
        {
            SetupPiezometer();
            return _piezometer?.ConstructNetwork() ?? "Piezometer network unavailable.";
        }

        /// <summary>
        /// Plan 189 bridge: piezometer <c>BuildAdvisory</c> → water-treatment
        /// <c>RegisterContaminationAdvisory</c>. Runs before the water tick so a
        /// blocked source refuses intake in the same day.
        /// </summary>
        private void TickPiezometerAdvisoryBridge(int day)
        {
            SetupPiezometer();
            if (_piezometer == null || _waterTreatment?.System == null) return;
            _piezometer.PublishAdvisory(_waterTreatment.System, day);
        }

        private void SavePiezometer()
        {
            if (_piezometer == null) return;
            CaptureSection(
                PiezometerSaveStore.SectionName,
                PiezometerSaveStore.TryCapturePersisted(_piezometer.CaptureSave()));
        }
    }
}
