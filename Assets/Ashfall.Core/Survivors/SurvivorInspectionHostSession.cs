using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Survivor Inspection host-side query model (item 2).
    ///
    /// Aggregates authoritative Core state into a single snapshot the
    /// <see cref="SurvivorDetailPanel"/> can render, and provides atomic
    /// commands (feeding, rest, bandaging, iodine, anti-rad, speaking) that
    /// validate inputs, mutate Core state, and return stable failure codes
    /// plus resource deltas so the UI cannot partially consume items.
    ///
    /// The host owns the resolver functions that look up survivor state
    /// objects in the existing systems. This keeps the Core query model
    /// engine-agnostic while still routing mutations through the live
    /// Needs / Radiation / Inventory systems.
    /// </summary>
    public sealed class SurvivorInspectionHostSession
    {
        public NeedsSystem Needs { get; }
        public RadiationSystem Radiation { get; }
        public Ashfall.Core.Inventory.Inventory Inventory { get; }

        private readonly Func<string, SurvivorNeedsState?> _resolveNeeds;
        private readonly Func<string, SurvivorRadState?> _resolveRad;

        public event Action<string>? OnCommandApplied;

        public SurvivorInspectionHostSession(
            NeedsSystem needs,
            RadiationSystem radiation,
            Ashfall.Core.Inventory.Inventory inventory,
            Func<string, SurvivorNeedsState?> resolveNeeds,
            Func<string, SurvivorRadState?> resolveRad)
        {
            Needs = needs ?? throw new ArgumentNullException(nameof(needs));
            Radiation = radiation ?? throw new ArgumentNullException(nameof(radiation));
            Inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _resolveNeeds = resolveNeeds ?? throw new ArgumentNullException(nameof(resolveNeeds));
            _resolveRad = resolveRad ?? throw new ArgumentNullException(nameof(resolveRad));
        }

        public SurvivorInspectionSnapshot Inspect(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorInspectionSnapshot.Empty;
            var snap = new SurvivorInspectionSnapshot
            {
                SurvivorId = survivorId,
                IsAlive = true
            };
            var needs = _resolveNeeds(survivorId);
            if (needs == null) snap.IsAlive = false;
            else
            {
                snap.Hunger = needs.Hunger;
                snap.Thirst = needs.Thirst;
                snap.Fatigue = needs.Fatigue;
                snap.Warmth = needs.Warmth;
                snap.Health = needs.Health;
                snap.Morale = needs.Morale;
                snap.Hygiene = needs.Hygiene;
                snap.IsAlive = needs.IsAliveState;
            }
            var rad = _resolveRad(survivorId);
            if (rad != null)
            {
                snap.RadiationDose = rad.RadiationDose;
                snap.IodineProtectionHours = rad.IodineProtectionTimer;
            }
            return snap;
        }

        /// <summary>Feed a survivor food units. Reduces hunger, requires the inventory quantity.</summary>
        public SurvivorCommandResult Feed(string survivorId, int foodUnits)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorCommandResult.Fail("missing_survivor_id");
            if (foodUnits <= 0)
                return SurvivorCommandResult.Fail("invalid_food_units");
            var needs = _resolveNeeds(survivorId);
            if (needs == null || !needs.IsAliveState)
                return SurvivorCommandResult.Fail("survivor_unavailable");
            if (Inventory.CountById("canned_food") < foodUnits)
                return SurvivorCommandResult.Fail("insufficient_food");
            Inventory.RemoveById("canned_food", foodUnits);
            Needs.Modify(needs, NeedKind.Hunger, -foodUnits * 10f);
            OnCommandApplied?.Invoke("feed:" + survivorId);
            return SurvivorCommandResult.Ok(new Dictionary<string, int>
            {
                ["food_consumed"] = foodUnits
            }, "fed");
        }

        public SurvivorCommandResult Drink(string survivorId, int waterUnits)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorCommandResult.Fail("missing_survivor_id");
            if (waterUnits <= 0)
                return SurvivorCommandResult.Fail("invalid_water_units");
            var needs = _resolveNeeds(survivorId);
            if (needs == null || !needs.IsAliveState)
                return SurvivorCommandResult.Fail("survivor_unavailable");
            if (Inventory.CountById("clean_water") < waterUnits)
                return SurvivorCommandResult.Fail("insufficient_water");
            Inventory.RemoveById("clean_water", waterUnits);
            Needs.Modify(needs, NeedKind.Thirst, -waterUnits * 10f);
            OnCommandApplied?.Invoke("drink:" + survivorId);
            return SurvivorCommandResult.Ok(new Dictionary<string, int>
            {
                ["water_consumed"] = waterUnits
            }, "drank");
        }

        public SurvivorCommandResult AssignRest(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorCommandResult.Fail("missing_survivor_id");
            if (hours <= 0 || hours > 16)
                return SurvivorCommandResult.Fail("invalid_rest_hours");
            var needs = _resolveNeeds(survivorId);
            if (needs == null || !needs.IsAliveState)
                return SurvivorCommandResult.Fail("survivor_unavailable");
            Needs.Modify(needs, NeedKind.Fatigue, -hours * 8f);
            OnCommandApplied?.Invoke("rest:" + survivorId);
            return SurvivorCommandResult.Ok(new Dictionary<string, float>
            {
                ["rest_hours"] = hours
            }, "rested");
        }

        public SurvivorCommandResult Bandage(string survivorId, int bandages)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorCommandResult.Fail("missing_survivor_id");
            if (bandages <= 0)
                return SurvivorCommandResult.Fail("invalid_bandage_count");
            var needs = _resolveNeeds(survivorId);
            if (needs == null || !needs.IsAliveState)
                return SurvivorCommandResult.Fail("survivor_unavailable");
            if (Inventory.CountById("bandage") < bandages)
                return SurvivorCommandResult.Fail("insufficient_bandages");
            Inventory.RemoveById("bandage", bandages);
            Needs.Modify(needs, NeedKind.Health, bandages * 4f);
            OnCommandApplied?.Invoke("bandage:" + survivorId);
            return SurvivorCommandResult.Ok(new Dictionary<string, int>
            {
                ["bandages_used"] = bandages
            }, "bandaged");
        }

        public SurvivorCommandResult TakeIodide(string survivorId, int pills)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorCommandResult.Fail("missing_survivor_id");
            if (pills <= 0)
                return SurvivorCommandResult.Fail("invalid_pill_count");
            var rad = _resolveRad(survivorId);
            if (rad == null)
                return SurvivorCommandResult.Fail("survivor_unavailable");
            if (Inventory.CountById("potassium_iodide") < pills)
                return SurvivorCommandResult.Fail("insufficient_iodide");
            Inventory.RemoveById("potassium_iodide", pills);
            // Each pill grants 2h of iodine protection (the system decays via Tick()).
            rad.IodineProtectionTimer = Math.Max(rad.IodineProtectionTimer,
                rad.IodineProtectionTimer + pills * 2f);
            OnCommandApplied?.Invoke("iodide:" + survivorId);
            return SurvivorCommandResult.Ok(new Dictionary<string, int>
            {
                ["pills_consumed"] = pills
            }, "iodide_taken");
        }

        public SurvivorCommandResult TakeAntiRad(string survivorId, int doses)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorCommandResult.Fail("missing_survivor_id");
            if (doses <= 0)
                return SurvivorCommandResult.Fail("invalid_dose_count");
            var rad = _resolveRad(survivorId);
            if (rad == null)
                return SurvivorCommandResult.Fail("survivor_unavailable");
            if (Inventory.CountById("anti_rad") < doses)
                return SurvivorCommandResult.Fail("insufficient_anti_rad");
            Inventory.RemoveById("anti_rad", doses);
            rad.RadiationDose = Math.Max(0f, rad.RadiationDose - doses * 8f);
            OnCommandApplied?.Invoke("anti_rad:" + survivorId);
            return SurvivorCommandResult.Ok(new Dictionary<string, int>
            {
                ["doses_consumed"] = doses
            }, "anti_rad_taken");
        }

        public SurvivorCommandResult Speak(string survivorId, float moraleBoost = 5f)
        {
            if (string.IsNullOrEmpty(survivorId))
                return SurvivorCommandResult.Fail("missing_survivor_id");
            var needs = _resolveNeeds(survivorId);
            if (needs == null || !needs.IsAliveState)
                return SurvivorCommandResult.Fail("survivor_unavailable");
            Needs.Modify(needs, NeedKind.Morale, moraleBoost);
            OnCommandApplied?.Invoke("speak:" + survivorId);
            return SurvivorCommandResult.Ok(new Dictionary<string, float>(), "spoken");
        }
    }

    [Serializable]
    public sealed class SurvivorInspectionSnapshot
    {
        public string SurvivorId;
        public bool IsAlive;
        public float Hunger;
        public float Thirst;
        public float Fatigue;
        public float Warmth;
        public float Health;
        public float Morale;
        public float Hygiene;
        public float RadiationDose;
        public bool HasWornGear;
        public float IodineProtectionHours;

        public static SurvivorInspectionSnapshot Empty => new SurvivorInspectionSnapshot
        {
            SurvivorId = string.Empty,
            IsAlive = false
        };
    }

    [Serializable]
    public sealed class SurvivorCommandResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public string OutcomeLabel;
        public Dictionary<string, int> IntDeltas = new Dictionary<string, int>();
        public Dictionary<string, float> FloatDeltas = new Dictionary<string, float>();

        public static SurvivorCommandResult Ok(Dictionary<string, int> intDeltas, string label)
        {
            var r = new SurvivorCommandResult
            {
                Succeeded = true,
                ReasonCode = "ok",
                OutcomeLabel = label
            };
            if (intDeltas != null)
                foreach (var kv in intDeltas) r.IntDeltas[kv.Key] = kv.Value;
            return r;
        }

        public static SurvivorCommandResult Ok(Dictionary<string, float> floatDeltas, string label)
        {
            var r = new SurvivorCommandResult
            {
                Succeeded = true,
                ReasonCode = "ok",
                OutcomeLabel = label
            };
            if (floatDeltas != null)
                foreach (var kv in floatDeltas) r.FloatDeltas[kv.Key] = kv.Value;
            return r;
        }

        public static SurvivorCommandResult Fail(string reason) => new SurvivorCommandResult
        {
            Succeeded = false,
            ReasonCode = reason ?? "fail",
            OutcomeLabel = string.Empty
        };
    }
}
