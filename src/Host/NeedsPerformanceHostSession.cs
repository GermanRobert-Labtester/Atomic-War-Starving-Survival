// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Survivors;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session managing the Plan 137 Needs -> Performance Cascade.
    /// Pure projection over live survivor needs states; zero parallel mutable state
    /// or duplicate save stores per Rule 5 and Invariant 4.
    /// </summary>
    public sealed class NeedsPerformanceHostSession : HostSessionBase,
        ICombatPerformanceModifier,
        IWorkEfficiencyModifier,
        IExpeditionPerformanceModifier
    {
        private readonly Main _main;
        private bool _isDisposed;

        public NeedsPerformanceHostSession(Main main)
        {
            _main = main ?? throw new ArgumentNullException(nameof(main));
        }

        public void Initialize(string? dataDir = null)
        {
            string baseDir = string.IsNullOrWhiteSpace(dataDir)
                ? CatalogPath.ResolveDataDir()
                : dataDir;

            string configPath = Path.Combine(baseDir, "needs_performance.json");
            if (File.Exists(configPath))
            {
                try
                {
                    string json = File.ReadAllText(configPath);
                    NeedsPerformanceBridge.LoadFromJson(json);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[NeedsPerformanceHostSession] Failed to load needs_performance.json: {ex.Message}");
                }
            }
        }

        public NeedsPerformanceModifiers GetModifiers(SurvivorNeedsState? state)
        {
            return NeedsPerformanceBridge.Project(state);
        }

        public NeedsPerformanceModifiers GetModifiers(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId) || _main.Survivors == null)
                return NeedsPerformanceModifiers.Neutral;

            var state = _main.Survivors.Find(survivorId);
            return NeedsPerformanceBridge.Project(state);
        }

        public (float accuracy, float damage) GetCombatModifiers(string survivorId)
        {
            var m = GetModifiers(survivorId);
            return (m.CombatAccuracyMultiplier, m.CombatDamageMultiplier);
        }

        public float GetWorkSpeedModifier(string survivorId)
        {
            return GetModifiers(survivorId).WorkSpeedMultiplier;
        }

        public float GetExpeditionSpeedModifier(string survivorId)
        {
            return GetModifiers(survivorId).ExpeditionSpeedMultiplier;
        }

        public float GetExpeditionStaminaDrainModifier(string survivorId)
        {
            return GetModifiers(survivorId).ExpeditionStaminaDrainMultiplier;
        }

        public NeedsPerformanceCensus GetCensus(IEnumerable<SurvivorNeedsState>? states = null)
        {
            int optimal = 0;
            int impaired = 0;
            int severe = 0;
            int critical = 0;

            var targetStates = states;
            if (targetStates == null && _main.Survivors != null)
            {
                var list = new List<SurvivorNeedsState>();
                foreach (var r in _main.Survivors.RosterState)
                {
                    if (r != null && r.IsAlive)
                    {
                        list.Add(r);
                    }
                }
                targetStates = list;
            }

            if (targetStates != null)
            {
                foreach (var s in targetStates)
                {
                    var mods = NeedsPerformanceBridge.Project(s);
                    switch (mods.OverallBand)
                    {
                        case PerformanceBand.Optimal: optimal++; break;
                        case PerformanceBand.Impaired: impaired++; break;
                        case PerformanceBand.Severe: severe++; break;
                        case PerformanceBand.Critical: critical++; break;
                    }
                }
            }

            return new NeedsPerformanceCensus(optimal, impaired, severe, critical);
        }

        public void TickDay(int day, List<Ashfall.Core.Campaign.DayStateChangeEvent>? events = null)
        {
            events?.Add(new Ashfall.Core.Campaign.DayStateChangeEvent(
                kind: "needs_performance_ticked",
                sourceOwnerId: "needs_performance",
                primaryId: null,
                secondaryId: null,
                numeric: 0f));
        }

        public override void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            base.Dispose();
        }
    }
}
