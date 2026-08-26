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
    : HostSessionBase{
        public WaterTreatmentSystem System { get; }
        public InventoryHostSession? InventoryHost { get; set; }
        public string LastEvent { get; private set; } = string.Empty;
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
                RaiseStateChanged();
            };
        }

        public ActionResult StartFiltration(TreatmentMode mode, float amount)
        {
            var res = System.StartTreatment(mode, amount);
            if (res.IsSuccess)
            {
                LastEvent = $"Started {mode} processing {amount:F1}L water.";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult ReplaceFilter()
        {
            var res = System.ReplaceFilter();
            if (res.IsSuccess)
            {
                LastEvent = "Replaced sediment/charcoal filter membrane.";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult AddWater(WaterType type, float amount)
        {
            var res = System.AddWater(type, amount);
            if (res.IsSuccess)
            {
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
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
