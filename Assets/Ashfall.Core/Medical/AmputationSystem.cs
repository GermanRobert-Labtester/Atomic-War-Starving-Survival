// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Medical
{
    public enum LimbId
    {
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }

    public enum LimbCondition
    {
        Intact,
        Wounded,
        Infected,
        Gangrenous,
        Amputated,
        Prosthetic,
        Bionic
    }

    [Serializable]
    public sealed class LimbState
    {
        public LimbId limb { get; set; }
        public LimbCondition condition { get; set; } = LimbCondition.Intact;
        public float infectionSeverity { get; set; } = 0f;
        public int daysUntreated { get; set; } = 0;
        public bool hasDeepWound { get; set; } = false;
        public string? prostheticId { get; set; } = null;
        public int recoveryDaysLeft { get; set; } = 0;
        public bool hasPhantomPain { get; set; } = false;
    }

    [Serializable]
    public sealed class SurgicalProcedureDef
    {
        public string procedure_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string limb_group { get; set; } = "any";
        public string required_tool_id { get; set; } = "surgical_saw";
        public List<SurgicalItemCost> required_items { get; set; } = new List<SurgicalItemCost>();
        public float base_shock_risk { get; set; } = 0.35f;
        public float base_bleeding { get; set; } = 50.0f;
        public float infection_reduction { get; set; } = 1.0f;
        public int recovery_days_min { get; set; } = 10;
        public int recovery_days_max { get; set; } = 20;
        public float phantom_pain_chance { get; set; } = 0.40f;
    }

    [Serializable]
    public sealed class SurgicalItemCost
    {
        public string item_id { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    [Serializable]
    public sealed class SurgicalProcedureCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<SurgicalProcedureDef> procedures { get; set; } = new List<SurgicalProcedureDef>();
    }

    public sealed class AmputationResult
    {
        public bool Success { get; set; }
        public bool SurvivorDied { get; set; }
        public LimbId Limb { get; set; }
        public float BloodLoss { get; set; }
        public float ShockSeverity { get; set; }
        public int RecoveryDays { get; set; }
        public bool PhantomPainTriggered { get; set; }
        public string OutcomeId { get; set; } = string.Empty;
        public string FailureCode { get; set; } = string.Empty;

        public static AmputationResult Fail(string failureCode, LimbId limb) =>
            new AmputationResult { Success = false, FailureCode = failureCode, Limb = limb };
    }

    [Serializable]
    public sealed class AmputationSystemState
    {
        public int schema_version { get; set; } = 1;
        public Dictionary<string, List<LimbState>> survivorLimbs { get; set; } = new Dictionary<string, List<LimbState>>();
        public List<string> phantomPainSurvivorIds { get; set; } = new List<string>();
        public int totalAmputationsPerformed { get; set; } = 0;
    }

    public sealed class AmputationSystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly NeedsSystem? _needs;
        private readonly ILog _log;
        private readonly Dictionary<string, SurgicalProcedureDef> _procedures = new Dictionary<string, SurgicalProcedureDef>(StringComparer.Ordinal);
        private AmputationSystemState _state = new AmputationSystemState();

        public event Action<string, LimbId, LimbCondition>? OnAmputationComplete;
        public event Action<string, LimbId>? OnGangreneDeclared;
        public event Action<string, string>? OnProstheticFitted;

        public AmputationSystemState State => _state;

        public AmputationSystem(
            ISeededRng? rng = null,
            Inventory.Inventory? inventory = null,
            NeedsSystem? needs = null,
            ILog? log = null)
        {
            _rng = rng ?? new SeededRng(190);
            _inventory = inventory ?? new Inventory.Inventory();
            _needs = needs;
            _log = log ?? NullLog.Instance;
        }

        public void RegisterProcedure(SurgicalProcedureDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.procedure_id)) return;
            _procedures[def.procedure_id] = def;
        }

        public List<LimbState> EnsureSurvivorLimbs(string survivorId)
        {
            if (!_state.survivorLimbs.TryGetValue(survivorId, out var limbs))
            {
                limbs = new List<LimbState>
                {
                    new LimbState { limb = LimbId.LeftArm, condition = LimbCondition.Intact },
                    new LimbState { limb = LimbId.RightArm, condition = LimbCondition.Intact },
                    new LimbState { limb = LimbId.LeftLeg, condition = LimbCondition.Intact },
                    new LimbState { limb = LimbId.RightLeg, condition = LimbCondition.Intact }
                };
                _state.survivorLimbs[survivorId] = limbs;
            }
            return limbs;
        }

        public LimbState? GetLimb(string survivorId, LimbId limb)
        {
            var limbs = EnsureSurvivorLimbs(survivorId);
            return limbs.Find(l => l.limb == limb);
        }

        public void InflictDeepWound(string survivorId, LimbId limb)
        {
            var l = GetLimb(survivorId, limb);
            if (l == null || l.condition == LimbCondition.Amputated || l.condition == LimbCondition.Bionic) return;

            l.condition = LimbCondition.Wounded;
            l.hasDeepWound = true;
            l.infectionSeverity = 0.15f;
            l.daysUntreated = 0;
        }

        public void TreatWound(string survivorId, LimbId limb, float cleaningEfficacy)
        {
            var l = GetLimb(survivorId, limb);
            if (l == null || l.condition == LimbCondition.Amputated) return;

            l.infectionSeverity = Math.Max(0f, l.infectionSeverity - cleaningEfficacy);
            if (l.infectionSeverity <= 0.05f && l.condition != LimbCondition.Gangrenous)
            {
                l.condition = LimbCondition.Intact;
                l.hasDeepWound = false;
                l.daysUntreated = 0;
            }
        }

        public void TickDay(int currentDay)
        {
            foreach (var kvp in _state.survivorLimbs)
            {
                string survivorId = kvp.Key;
                var limbs = kvp.Value;

                for (int i = 0; i < limbs.Count; i++)
                {
                    var l = limbs[i];
                    if (l.recoveryDaysLeft > 0)
                    {
                        l.recoveryDaysLeft--;
                    }

                    if (l.condition == LimbCondition.Amputated || l.condition == LimbCondition.Bionic || l.condition == LimbCondition.Intact)
                        continue;

                    l.daysUntreated++;

                    if (l.condition == LimbCondition.Wounded && l.daysUntreated >= 2)
                    {
                        l.condition = LimbCondition.Infected;
                        l.infectionSeverity += 0.25f;
                    }
                    else if (l.condition == LimbCondition.Infected)
                    {
                        l.infectionSeverity += 0.20f;
                        if (l.infectionSeverity >= 0.75f || l.daysUntreated >= 5)
                        {
                            l.condition = LimbCondition.Gangrenous;
                            OnGangreneDeclared?.Invoke(survivorId, l.limb);
                        }
                    }
                    else if (l.condition == LimbCondition.Gangrenous)
                    {
                        l.infectionSeverity = Math.Min(1.0f, l.infectionSeverity + 0.15f);
                        // Systemic sepsis damage to survivor
                        if (_needs != null)
                        {
                            var s = _needs.Get(survivorId);
                            if (s != null && s.IsAliveState)
                            {
                                _needs.Modify(s, NeedKind.Health, -15f);
                                _needs.Modify(s, NeedKind.Morale, -10f);
                            }
                        }
                    }
                }
            }
        }

        public AmputationResult PerformAmputation(string survivorId, LimbId limb, string procedureId)
        {
            var l = GetLimb(survivorId, limb);
            if (l == null) return AmputationResult.Fail("invalid_survivor", limb);
            if (l.condition == LimbCondition.Amputated || l.condition == LimbCondition.Bionic)
                return AmputationResult.Fail("already_amputated", limb);

            if (!_procedures.TryGetValue(procedureId, out var proc))
            {
                proc = new SurgicalProcedureDef
                {
                    procedure_id = procedureId,
                    required_tool_id = "surgical_saw",
                    base_shock_risk = 0.35f,
                    base_bleeding = 50f
                };
            }

            // 1. Tool check
            if (!string.IsNullOrEmpty(proc.required_tool_id) && _inventory.CountById(proc.required_tool_id) <= 0)
            {
                return AmputationResult.Fail("missing_surgical_tool", limb);
            }

            // 2. Consumable check
            for (int i = 0; i < proc.required_items.Count; i++)
            {
                var req = proc.required_items[i];
                if (_inventory.CountById(req.item_id) < req.amount)
                    return AmputationResult.Fail($"missing_item_{req.item_id}", limb);
            }

            // 3. Atomically consume supplies
            for (int i = 0; i < proc.required_items.Count; i++)
            {
                var req = proc.required_items[i];
                _inventory.RemoveById(req.item_id, req.amount);
            }

            // 4. Resolve surgery outcome
            double roll = _rng.NextDouble();
            bool survived = roll >= proc.base_shock_risk;
            int recoveryDays = _rng.Next(proc.recovery_days_min, proc.recovery_days_max + 1);
            bool phantomPain = _rng.NextDouble() < proc.phantom_pain_chance;

            l.condition = LimbCondition.Amputated;
            l.infectionSeverity = 0f;
            l.daysUntreated = 0;
            l.hasDeepWound = false;
            l.recoveryDaysLeft = recoveryDays;
            l.hasPhantomPain = phantomPain;

            if (phantomPain && !_state.phantomPainSurvivorIds.Contains(survivorId))
            {
                _state.phantomPainSurvivorIds.Add(survivorId);
            }

            _state.totalAmputationsPerformed++;

            if (_needs != null)
            {
                var s = _needs.Get(survivorId);
                if (s != null && s.IsAliveState)
                {
                    if (!survived)
                    {
                        _needs.Modify(s, NeedKind.Health, -100f);
                    }
                    else
                    {
                        _needs.Modify(s, NeedKind.Health, -proc.base_bleeding);
                        _needs.Modify(s, NeedKind.Morale, -25f);
                    }
                }
            }

            OnAmputationComplete?.Invoke(survivorId, limb, l.condition);

            return new AmputationResult
            {
                Success = true,
                SurvivorDied = !survived,
                Limb = limb,
                BloodLoss = proc.base_bleeding,
                ShockSeverity = proc.base_shock_risk,
                RecoveryDays = recoveryDays,
                PhantomPainTriggered = phantomPain,
                OutcomeId = survived ? "amputation_survived" : "amputation_fatal"
            };
        }

        public ActionResult FitProsthetic(string survivorId, LimbId limb, string prostheticItemId)
        {
            var l = GetLimb(survivorId, limb);
            if (l == null) return ActionResult.Blocked("invalid_survivor", "amputation.invalid_survivor");
            if (l.condition != LimbCondition.Amputated)
                return ActionResult.Blocked("not_amputated", "amputation.not_amputated");
            if (l.recoveryDaysLeft > 0)
                return ActionResult.Blocked("in_recovery", "amputation.in_recovery");

            if (_inventory.CountById(prostheticItemId) <= 0)
                return ActionResult.Blocked("missing_prosthetic_item", "amputation.missing_prosthetic_item");

            _inventory.RemoveById(prostheticItemId, 1);

            l.condition = LimbCondition.Prosthetic;
            l.prostheticId = prostheticItemId;

            OnProstheticFitted?.Invoke(survivorId, prostheticItemId);
            return ActionResult.Success("amputation.prosthetic_fitted");
        }

        public ActionResult UpgradeToBionic(string survivorId, LimbId limb, string bionicItemId)
        {
            var l = GetLimb(survivorId, limb);
            if (l == null) return ActionResult.Blocked("invalid_survivor", "amputation.invalid_survivor");
            if (l.condition != LimbCondition.Amputated && l.condition != LimbCondition.Prosthetic)
                return ActionResult.Blocked("ineligible_socket", "amputation.ineligible_socket");

            if (_inventory.CountById(bionicItemId) <= 0)
                return ActionResult.Blocked("missing_bionic_item", "amputation.missing_bionic_item");

            _inventory.RemoveById(bionicItemId, 1);

            l.condition = LimbCondition.Bionic;
            l.prostheticId = bionicItemId;
            l.hasPhantomPain = false;

            return ActionResult.Success("amputation.bionic_integrated");
        }

        public float GetWorkSpeedMultiplier(string survivorId)
        {
            var limbs = EnsureSurvivorLimbs(survivorId);
            float mult = 1.0f;

            for (int i = 0; i < limbs.Count; i++)
            {
                var l = limbs[i];
                if (l.limb == LimbId.LeftArm || l.limb == LimbId.RightArm)
                {
                    if (l.condition == LimbCondition.Amputated) mult -= 0.35f;
                    else if (l.condition == LimbCondition.Prosthetic) mult -= 0.15f;
                    else if (l.condition == LimbCondition.Bionic) mult += 0.10f;
                    else if (l.condition == LimbCondition.Gangrenous) mult -= 0.30f;
                    else if (l.condition == LimbCondition.Infected) mult -= 0.15f;
                }
            }

            return Math.Max(0.20f, mult);
        }

        public float GetMovementSpeedMultiplier(string survivorId)
        {
            var limbs = EnsureSurvivorLimbs(survivorId);
            float mult = 1.0f;

            for (int i = 0; i < limbs.Count; i++)
            {
                var l = limbs[i];
                if (l.limb == LimbId.LeftLeg || l.limb == LimbId.RightLeg)
                {
                    if (l.condition == LimbCondition.Amputated) mult -= 0.45f;
                    else if (l.condition == LimbCondition.Prosthetic) mult -= 0.20f;
                    else if (l.condition == LimbCondition.Bionic) mult += 0.10f;
                    else if (l.condition == LimbCondition.Gangrenous) mult -= 0.35f;
                    else if (l.condition == LimbCondition.Infected) mult -= 0.15f;
                }
            }

            return Math.Max(0.15f, mult);
        }

        public void RestoreState(AmputationSystemState state)
        {
            if (state == null) return;
            _state = state;
        }
    }
}
