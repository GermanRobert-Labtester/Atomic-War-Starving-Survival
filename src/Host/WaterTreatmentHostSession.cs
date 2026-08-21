using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot host session for WaterTreatmentSystem.
    /// Manages water purification batches, filter maintenance, charcoal/fuel supplies,
    /// and routes exposure events to Disease, Needs, and Dose systems.
    /// </summary>
    public sealed class WaterTreatmentHostSession
    {
        public WaterTreatmentSystem System { get; }
        public InventoryHostSession? InventoryHost { get; set; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public WaterTreatmentHostSession(WaterTreatmentSystem system, InventoryHostSession? inventoryHost = null)
        {
            System = system ?? new WaterTreatmentSystem(new GodotLog());
            InventoryHost = inventoryHost;

            System.OnTreatmentCompleted += result =>
            {
                LastEvent = result.IsSuccess
                    ? $"[WaterTreatment] Batch complete: {result.MessageKey}"
                    : $"[WaterTreatment] Batch failed: {result.MessageKey}";
                StateChanged?.Invoke();
            };

            System.OnWaterStateChanged += () =>
            {
                StateChanged?.Invoke();
            };

            System.OnHeavyMetalExposure += dose =>
            {
                LastEvent = $"[WaterTreatment] WARNING: Heavy metal exposure ({dose:F1} ppm) detected in water output!";
                StateChanged?.Invoke();
            };

            System.OnPathogenExposure += dose =>
            {
                LastEvent = $"[WaterTreatment] WARNING: Pathogen contamination ({dose:F1} CFU) detected in water output!";
                StateChanged?.Invoke();
            };
        }

        public ActionResult StartFiltration(TreatmentMode mode, float amount)
        {
            var res = System.StartTreatment(mode, amount);
            if (res.IsSuccess)
            {
                LastEvent = $"Started {mode} processing {amount:F1}L water.";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult ReplaceFilter()
        {
            var res = System.ReplaceFilter();
            if (res.IsSuccess)
            {
                LastEvent = "Replaced sediment/charcoal filter membrane.";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult AddWater(WaterType type, float amount)
        {
            var res = System.AddWater(type, amount);
            if (res.IsSuccess)
            {
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            StateChanged?.Invoke();
        }
    }
}
