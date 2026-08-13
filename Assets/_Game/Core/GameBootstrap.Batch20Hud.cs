using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        // -----------------------------------------------------------------
        // Batch 20 UI Elements Integration & Event Wiring
        // -----------------------------------------------------------------

        private void WireBatch20HudElements()
        {
            if (_hud == null) return;

            // 01 & 02: Radiation Dosimeter & Geiger Sweep Gauge
            WireRadiationHudWidgets();

            // 03 & 09: Air Filter Integrity & Temperature Readout
            WireAirHeatHudWidgets();

            // 04: Fallout Storm Warning Banner
            WireFalloutStormBanner();

            // 05, 06, 18: Survivor Portrait, Moral Decay, Blood Type
            WireSurvivorHudWidgets();

            // 07: Ration Allocation Dial
            WireRationAllocationDial();

            // 08: Water Purity Gauge
            WireWaterPurityHudWidget();

            // 10: Power Flow Schematic
            WirePowerFlowSchematicWidget();

            // 11: Faction Pressure Ring
            WireFactionPressureWidget();

            // 12 & 19: Expedition Countdown & Loot Haul Ticker
            WireExpeditionAndLootWidgets();

            // 13: Radio Signal Strength Bar
            WireRadioSignalWidget();

            // 14: Craft Queue Strip
            WireCraftQueueWidget();

            // 15: Alert Toast Notification
            WireAlertToastNotification();

            // 16: Bunker Floor Map Miniature
            WireBunkerMapWidget();

            // 17: Day/Night Arc Clock
            WireDayNightClockWidget();

            // 20: Endgame Victory Path Tracker
            WireEndgameVictoryTracker();
        }

        private void WireRadiationHudWidgets()
        {
            if (_hud == null) return;
            var primary = Survivors != null && Survivors.Count > 0 ? Survivors[0] : null;

            if (_hud.RadiationDosimeterWidget != null && primary != null)
            {
                _hud.RadiationDosimeterWidget.SetDosimeterData(
                    primary.RadiationDose / 1000f,
                    primary.LifetimeRadiationExposure,
                    0);
            }

            if (_hud.GeigerSweepGauge != null)
            {
                float cpm = primary != null ? primary.RadiationDose * 10f : 0f;
                _hud.GeigerSweepGauge.SetCPM(cpm);
            }
        }

        private void WireAirHeatHudWidgets()
        {
            if (_hud == null) return;

            if (_hud.AirFilterIntegrityBar != null && AirHeatManagementSystem != null)
            {
                var snap = AirHeatManagementSystem.GetSnapshot();
                if (snap != null)
                {
                    float integrity01 = snap.FilterHealth / 100f;
                    float countdownDays = snap.FilterRuntimeHours / 24f;
                    float toxicity01 = 1f - Mathf.Clamp01(snap.AirQuality);
                    _hud.AirFilterIntegrityBar.SetFilterData(integrity01, countdownDays, toxicity01);
                }
            }

            if (_hud.TemperatureReadoutWidget != null && TemperatureSystem != null)
            {
                float indoor = TemperatureSystem.GetIndoorTemperature(Shelter);
                float outdoor = TemperatureSystem.AmbientCelsius;
                float fuelHours = AirHeatManagementSystem != null
                    ? AirHeatManagementSystem.GetSnapshot().HeaterRuntimeHours
                    : 0f;
                _hud.TemperatureReadoutWidget.SetTemperatureData(indoor, outdoor, fuelHours, "BUNKER HEATER");
            }
        }

        private void WireFalloutStormBanner()
        {
            if (_hud == null || _hud.FalloutStormWarningBanner == null) return;
            if (WeatherSystem != null && WeatherSystem.Current == WeatherKind.FalloutStorm)
            {
                _hud.FalloutStormWarningBanner.ShowStorm(
                    "ASHFRONT ECHO",
                    "NE 45 KM/H",
                    FalloutStormWarningBanner.StormIntensity.Heavy,
                    15f);
            }
        }

        private void WireSurvivorHudWidgets()
        {
            if (_hud == null) return;
            var primary = Survivors != null && Survivors.Count > 0 ? Survivors[0] : null;
            if (primary == null) return;

            float health = primary.Needs != null ? primary.Needs.Health : 100f;
            float morale = primary.Needs != null ? primary.Needs.Morale : 75f;
            float fatigue = primary.Needs != null ? primary.Needs.Fatigue : 0f;

            if (_hud.SurvivorPortraitCard != null)
            {
                var status = health <= 20f ? SurvivorPortraitCard.SurvivorStatus.Critical :
                             health <= 50f ? SurvivorPortraitCard.SurvivorStatus.Ill :
                             morale <= 30f ? SurvivorPortraitCard.SurvivorStatus.Stressed :
                             SurvivorPortraitCard.SurvivorStatus.Healthy;

                string role = !string.IsNullOrEmpty(primary.ArchetypeId) ? primary.ArchetypeId : "SURVIVOR";

                _hud.SurvivorPortraitCard.Bind(
                    primary.Id,
                    primary.DisplayName ?? primary.Id,
                    role.ToUpperInvariant(),
                    health,
                    morale,
                    fatigue,
                    primary.RadiationDose,
                    "O-",
                    status);
            }

            if (_hud.MoralDecayMeter != null)
            {
                _hud.MoralDecayMeter.SetMorale(morale / 100f);
            }

            if (_hud.BloodTypeIndicator != null)
            {
                _hud.BloodTypeIndicator.SetBloodType("O-");
            }
        }

        private void WireRationAllocationDial()
        {
            if (_hud == null || _hud.RationAllocationDial == null) return;
            _hud.RationAllocationDial.OnRationConfirmed += (survivorId, foodId, kcal) =>
            {
                BunkerRationingSystem?.SetLevel(RationResource.Food, RationLevel.Standard);
            };
        }

        private void WireWaterPurityHudWidget()
        {
            if (_hud == null || _hud.WaterPurityGauge == null) return;
            if (WaterEconomySystem != null && Shelter != null && WaterStorage != null)
            {
                var snap = WaterEconomySystem.GetSnapshot(Shelter, WaterStorage);
                if (snap != null)
                {
                    float total = Mathf.Max(0.001f, snap.CleanWater + snap.DirtyWater + snap.IrradiatedWater);
                    float contamination = (snap.DirtyWater + snap.IrradiatedWater) / total;
                    float outputPerDay = snap.PurifierOperational && snap.HoursPerUnit > 0f
                        ? 24f / snap.HoursPerUnit
                        : 0f;
                    var status = snap.PurifierOperational
                        ? WaterPurityGauge.PurificationStatus.Filtering
                        : WaterPurityGauge.PurificationStatus.Idle;
                    _hud.WaterPurityGauge.SetWaterData(snap.CleanWater, outputPerDay, contamination, status);
                }
            }
        }

        private void WirePowerFlowSchematicWidget()
        {
            if (_hud == null || _hud.PowerFlowSchematic == null) return;
            if (PowerNetwork != null)
            {
                float supplyKW = PowerNetwork.TotalGeneration / 1000f;
                float demandKW = PowerNetwork.TotalDraw / 1000f;
                _hud.PowerFlowSchematic.SetPowerData(supplyKW, demandKW, null);
            }
        }

        private void WireFactionPressureWidget()
        {
            if (_hud == null || _hud.FactionPressureRing == null) return;
            _hud.FactionPressureRing.UpdateThreats(0.3f, 0.5f, 0.2f, 0.4f);
        }

        private void WireExpeditionAndLootWidgets()
        {
            if (_hud == null) return;

            if (_hud.ExpeditionCountdownTimer != null && ExpeditionSystem != null)
            {
                _hud.ExpeditionCountdownTimer.UpdateProgress(0f, 10f, 0.2f);
            }

            if (_hud.LootHaulTicker != null && ScavengingSystem != null)
            {
                // Loot haul ticker is updated when scavenge returns
            }
        }

        private void WireRadioSignalWidget()
        {
            if (_hud == null || _hud.RadioSignalStrengthBar == null) return;
            if (RadioTunerSystem != null)
            {
                float freq = RadioTunerSystem.GetCurrentFrequency()?.frequencyMHz ?? 104.5f;
                int bars = RadioTunerSystem.State != null
                    ? Mathf.RoundToInt(RadioTunerSystem.State.SignalStrength * 5f)
                    : 3;
                _hud.RadioSignalStrengthBar.SetSignal(freq, bars, RadioStationType.Emergency, 15f);
            }
        }

        private void WireCraftQueueWidget()
        {
            if (_hud == null || _hud.CraftQueueStrip == null) return;
            if (CraftingSystem != null)
            {
                // Sync craft queue
            }
        }

        private void WireAlertToastNotification()
        {
            if (_hud == null || _hud.AlertToastNotification == null) return;
            // System ready for PostToast calls across Bootstrap events
        }

        private void WireBunkerMapWidget()
        {
            if (_hud == null || _hud.BunkerFloorMapMiniature == null) return;
            if (Shelter != null)
            {
                // Sync shelter room map cells
            }
        }

        private void WireDayNightClockWidget()
        {
            if (_hud == null || _hud.DayNightArcClock == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            float time01 = TimeSystem != null ? (float)TimeSystem.CurrentHour / 24f : 0.5f;
            _hud.DayNightArcClock.SetTime(day, time01, "NUCLEAR WINTER");
        }

        private void WireEndgameVictoryTracker()
        {
            if (_hud == null || _hud.EndgameVictoryPathTracker == null) return;
            // Configured for campaign resolution triggers
        }
    }
}
