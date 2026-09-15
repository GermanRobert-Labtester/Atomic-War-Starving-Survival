// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.PlayerCommand;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot host session for WaterTreatmentSystem.
    /// Manages water purification batches, filter maintenance, charcoal/fuel supplies,
    /// and routes exposure events to Disease, Needs, and Dose systems.
    /// </summary>
    public sealed class WaterTreatmentHostSession
    : HostSessionBase{
        public WaterTreatmentSystem System { get; }
        public InventoryHostSession? InventoryHost { get; set; }
        public string LastEvent { get; private set; } = string.Empty;
        public event Action? OnTreatmentStarted;

        /// <summary>
        /// B5–B8 expansion (§9.12): unsafe-water exposure sink. Main wires it
        /// to the canonical disease sweep (roster → TryExpose with the pure
        /// WaterborneExposureRules mapping); the session only forwards the
        /// fact. Null = legacy behavior (event surfaces as text only).
        /// </summary>
        public Action<float>? PathogenExposureSink { get; set; }
        public WaterTreatmentHostSession(WaterTreatmentSystem system, InventoryHostSession? inventoryHost = null)
        {
            System = system ?? new WaterTreatmentSystem(new GodotLog());
            InventoryHost = inventoryHost;

            System.OnTreatmentCompleted += result =>
            {
                LastEvent = result.IsSuccess
                    ? $"[WaterTreatment] Batch complete: {result.MessageKey}"
                    : $"[WaterTreatment] Batch failed: {result.MessageKey}";
                RaiseStateChanged();
            };

            System.OnWaterStateChanged += () =>
            {
                RaiseStateChanged();
            };

            System.OnHeavyMetalExposure += dose =>
            {
                LastEvent = $"[WaterTreatment] WARNING: Heavy metal exposure ({dose:F1} ppm) detected in water output!";
                RaiseStateChanged();
            };

            System.OnPathogenExposure += dose =>
            {
                LastEvent = $"[WaterTreatment] WARNING: Pathogen contamination ({dose:F1} CFU) detected in water output!";
                // B5–B8 expansion: route the fact through the canonical disease
                // contract (the sink owns the roster sweep; DiseaseSystem owns
                // the outcome roll).
                PathogenExposureSink?.Invoke(dose);
                RaiseStateChanged();
            };
        }

        public CommandResult StartFiltration(TreatmentMode mode, float amount)
        {
            var result = System.ExecuteStartTreatment(mode, amount, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (result.IsSuccess)
            {
                LastEvent = $"Started {mode} processing {amount:F1}L water.";
                OnTreatmentStarted?.Invoke();
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Water treatment refused: {result.FailureCode}.";
            }
            return result;
        }

        public CommandResult ReplaceFilter()
        {
            // B5–B8 Phase 6 repair (§8.8/§9.16): the replacement now costs the
            // canonical filter item. Core's doc comment always claimed a
            // consumed item; neither layer actually consumed one — a free
            // maintenance loop on the authority that gates treatment quality.
            // Atomic: the item is consumed only when the Core commit succeeds.
            const string filterItemId = "water_filter";
            if (InventoryHost != null)
            {
                if (InventoryHost.Inventory.CountById(filterItemId) < 1)
                {
                    LastEvent = $"Filter replacement blocked: requires {filterItemId}.";
                    return new CommandResult(
                        PlayerCommandCode.TreatmentReplaceFilter,
                        ActionResult.Blocked("missing_filter", "water.filter_missing"),
                        StateVersion,
                        StateVersion);
                }

                var result = System.ReplaceFilter();
                if (result.IsSuccess)
                {
                    InventoryHost.Remove(filterItemId, 1);
                    LastEvent = "Replaced sediment/charcoal filter membrane.";
                    RaiseStateChanged();
                }
                return new CommandResult(
                    PlayerCommandCode.TreatmentReplaceFilter,
                    result,
                    StateVersion,
                    StateVersion);
            }

            var legacy = System.ReplaceFilter();
            if (legacy.IsSuccess)
            {
                LastEvent = "Replaced sediment/charcoal filter membrane.";
                RaiseStateChanged();
            }
            return new CommandResult(
                PlayerCommandCode.TreatmentReplaceFilter,
                legacy,
                StateVersion,
                StateVersion);
        }

        public CommandResult AddWater(WaterType type, float amount)
        {
            var result = System.AddWater(type, amount);
            if (result.IsSuccess)
            {
                RaiseStateChanged();
            }
            return new CommandResult(
                PlayerCommandCode.TreatmentStart,
                result,
                StateVersion,
                StateVersion);
        }

        public void TickDay(int day, float powerAvailability01 = 1f)
        {
            System.TickDay(day, powerAvailability01);
            RaiseStateChanged();
        }

        public void SetIncomingContamination(float level)
        {
            System.SetIncomingContamination(level);
            if (level > 0.5f)
                LastEvent = $"[WaterTreatment] External contamination influx ({level:F2}) — flood source";
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            WaterTreatmentSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
