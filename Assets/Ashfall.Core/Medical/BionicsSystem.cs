// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 177 — Bionics & Cybernetic Prosthetics (Core authority).
// Authority split (one authority per concern):
//   • AmputationSystem — THE limb/socket truth (LimbCondition.Bionic,
//     prostheticId, recovery). This system mutates limbs ONLY through its
//     public methods (UpgradeToBionic / RevertBionicToAmputated). No second
//     body model (Trap I).
//   • Inventory — surgical kits, prototype parts, maintenance consumables.
//   • PowerGridSystem — shelter power truth. Core tracks charge STATE; the
//     host reports charger availability (no free energy, §6.10).
//   • CombatTraumaSystem — combat damage handoff only (§6.12); no bypass.
//   • MedicalPipelineCoordinator — complication treatment is a typed event
//     for the medical host; never self-treated (§6.6).
//   • NeedsSystem/RadiationSystem — NEVER touched. Electrical disruption is
//     TYPED component failure, never blanket health damage (§6.11, Trap J).
// Determinism: install/complication rolls use the host-forked ISeededRng;
// decay/maintenance/power are pure functions of state + authored rates.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.IO;

namespace Ashfall.Core.Medical
{
    /// <summary>Bounded design constants (§1.5: strong but capped, explicit costs).</summary>
    public static class BionicsCaps
    {
        /// <summary>Max capability bonus beyond baseline (bp of 1000-scale).</summary>
        public const float CapabilityBonusCapBp = 200f;
        /// <summary>Implant condition below this contributes zero function.</summary>
        public const float ConditionFunctionFloor = 25f;
        public const float DestroyedCondition = 0f;
        /// <summary>Day-1 rehab performance factor (§6.8: never instant full).</summary>
        public const float RehabStartFactor = 0.5f;
        /// <summary>Maintenance grace days before malfunction rolls start.</summary>
        public const int OverdueMalfunctionGraceDays = 2;
    }

    public enum ImplantMalfunction
    {
        None = 0,
        ActuatorLock = 1,     // temporary leg function loss
        SensorBlackout = 2,   // temporary arm/perception function loss
        Stunned = 3           // brief full lockup (high-draw class only)
    }

    public enum ImplantIntegrationStatus
    {
        Integrating = 0,   // rehab period: reduced performance (§6.8)
        Integrated = 1
    }

    public enum ImplantComplication
    {
        None = 0,
        Inflammation = 1,             // treatable with antiseptic-class kits
        ChronicPain = 2,              // long horizon; host morale route
        NeuralAdaptationFailure = 3   // needs specialist treatment
    }

    public enum ImplantElectricalVulnerability
    {
        None = 0,
        Low = 1,
        High = 2
    }

    /// <summary>One authored implant definition (bionics.json).</summary>
    [Serializable]
    public sealed class ImplantDefinition
    {
        public string implant_id { get; set; } = string.Empty;     // implant_*
        public string display_name { get; set; } = string.Empty;
        public string body_slot { get; set; } = "arm";             // arm | leg (v1 limb slots)
        public string implant_class { get; set; } = "mechanical";  // mechanical|powered|rechargeable|neuro_linked
        public int functional_restore_bp { get; set; }             // 500..1200 capability restoration
        public int skill_modifier_bp { get; set; }                 // 0..500
        public string power_profile { get; set; } = "passive";     // passive|rechargeable|high_draw
        public int daily_power_draw_watts { get; set; }            // 0..60
        public int battery_days { get; set; }                      // 0..14 expedition endurance
        public int maintenance_interval_days { get; set; }         // 3..30
        public int daily_condition_decay_bp { get; set; }          // 0..200 bp/day
        public int condition_max { get; set; } = 100;
        public int integration_risk_bp { get; set; }               // 0..3000 complication chance
        public int integration_recovery_days { get; set; }         // 3..30 rehab
        public int malfunction_risk_bp { get; set; }               // 0..1000 overdue-maintenance roll
        public string electrical_vulnerability { get; set; } = "none"; // none|low|high
        public string required_surgery_tool_id { get; set; } = "surgical_saw";
        public List<string> required_item_ids { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class BionicsCatalogRoot
    {
        public int schema_version { get; set; } = 1;
        public List<ImplantDefinition> implants { get; set; } = new List<ImplantDefinition>();
    }

    /// <summary>Load outcome: rows plus validation errors (domain result, no exceptions).</summary>
    public sealed class BionicsCatalogLoadResult
    {
        public List<ImplantDefinition> Implants { get; } = new List<ImplantDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    [Serializable]
    public sealed class ImplantInstanceState
    {
        public string instance_id { get; set; } = string.Empty;    // implant_inst_<n>
        public string implant_id { get; set; } = string.Empty;     // authored definition
        public string survivor_id { get; set; } = string.Empty;
        public int body_slot_limb { get; set; }                    // (int) LimbId
        public float condition { get; set; } = 100f;
        public int installed_day { get; set; }
        public int integration_status { get; set; } = (int)ImplantIntegrationStatus.Integrating;
        public int integration_days_left { get; set; }
        public int integration_days_total { get; set; }
        public float battery_days_remaining { get; set; }
        public bool is_charging { get; set; }
        public int last_maintenance_day { get; set; }
        public int complication { get; set; } = 0;                 // (int) ImplantComplication
        public int complication_days_left { get; set; }            // -1 = chronic (persistent until treated)
        public int malfunction { get; set; } = 0;                  // (int) ImplantMalfunction
        public int malfunction_days_left { get; set; }
        public bool destroyed { get; set; }
    }

    [Serializable]
    public sealed class BionicsSystemState
    {
        public string system_id { get; set; } = "bionics";
        public int schema_version { get; set; } = 1;
        public int instance_counter { get; set; }
        public int last_tick_day { get; set; }
        public List<ImplantInstanceState> implants { get; set; } = new List<ImplantInstanceState>();
    }

    /// <summary>Install outcome (domain result, no exceptions).</summary>
    public sealed class ImplantInstallResult
    {
        public bool Success;
        public string ReasonCode = string.Empty;                   // unknown_implant | body_slot_mismatch |
                                                                   // socket_ineligible | limb_in_recovery |
                                                                   // missing_surgery_tool | missing_item_<id> |
                                                                   // already_implanted | installed
        public ImplantInstanceState? Instance;
        public ImplantComplication Complication = ImplantComplication.None;

        public static ImplantInstallResult Fail(string reason) => new ImplantInstallResult { Success = false, ReasonCode = reason };
    }

    /// <summary>Typed disruption/combat-damage outcome per implant (§6.11).</summary>
    public sealed class ImplantDisruptionOutcome
    {
        public string InstanceId = string.Empty;
        public string Effect = string.Empty;    // stunned | actuator_lock | sensor_blackout | battery_drain | condition_damage | destroyed
        public int Days;
        public float BatteryDaysDrained;
    }

    /// <summary>
    /// Bionic implant authority: eligibility, surgical installation THROUGH the
    /// canonical limb authority, integration/rehab, maintenance decay, power
    /// state, typed electrical vulnerability, and bounded capability queries.
    /// </summary>
    public sealed class BionicsSystem
    {
        public const string SystemId = "bionics";

        private readonly Dictionary<string, ImplantDefinition> _defs =
            new Dictionary<string, ImplantDefinition>(StringComparer.Ordinal);
        private BionicsSystemState _state = new BionicsSystemState();

        /// <summary>The limb/socket authority (single body model, §6.1).</summary>
        private readonly AmputationSystem _amputation;
        private Func<string, int>? _itemCount;
        private Action<string, int>? _itemConsume;
        /// <summary>Host-forked malfunction roll (day-keyed); Core stores no RNG state.</summary>
        public Func<double>? MalfunctionRoll;
        /// <summary>Host-provided charger availability (real power truth, §6.10).</summary>
        public Func<bool>? ChargerAvailable;

        public event Action<ImplantInstanceState, ImplantDefinition>? OnImplantInstalled;
        public event Action<ImplantInstanceState>? OnImplantConditionChanged;
        public event Action<ImplantInstanceState, ImplantMalfunction>? OnImplantMalfunctioned;
        public event Action<ImplantInstanceState, ImplantComplication>? OnImplantComplication;
        public event Action<ImplantInstanceState>? OnImplantRemoved;
        public event Action<ImplantInstanceState>? OnImplantDestroyed;
        public event Action<ImplantInstanceState>? OnIntegrationCompleted;

        public BionicsSystemState State => _state;

        public BionicsSystem(AmputationSystem amputation, IEnumerable<ImplantDefinition>? defs = null)
        {
            _amputation = amputation ?? throw new ArgumentNullException(nameof(amputation));
            if (defs != null)
                foreach (var d in defs)
                    if (d != null && !string.IsNullOrEmpty(d.implant_id)) _defs[d.implant_id] = d;
        }

        public void BindInventory(Func<string, int> count, Action<string, int> consume)
        {
            _itemCount = count;
            _itemConsume = consume;
        }

        public ImplantDefinition? Definition(string implantId) =>
            !string.IsNullOrEmpty(implantId) && _defs.TryGetValue(implantId, out var d) ? d : null;

        public IReadOnlyList<ImplantInstanceState> ImplantsFor(string survivorId) =>
            _state.implants.Where(i => i != null && string.Equals(i.survivor_id, survivorId, StringComparison.Ordinal)).ToList();

        public ImplantInstanceState? InstanceFor(string survivorId, LimbId limb) =>
            _state.implants.FirstOrDefault(i => i != null && !i.destroyed
                && string.Equals(i.survivor_id, survivorId, StringComparison.Ordinal)
                && i.body_slot_limb == (int)limb);

        private static bool SlotMatches(string defSlot, LimbId limb) =>
            defSlot switch
            {
                "arm" => limb is LimbId.LeftArm or LimbId.RightArm,
                "leg" => limb is LimbId.LeftLeg or LimbId.RightLeg,
                _ => false
            };

        // ── Eligibility + surgical installation (§6.4-6.5) ─────────────

        /// <summary>
        /// Install an implant through the canonical limb authority. The limb must
        /// be an amputated/prosthetic socket with recovery elapsed; the surgery
        /// consumes the authored tool + items via the bound inventory. Outcome
        /// rolls (integration success / complication) use the host-forked rng.
        /// </summary>
        public ImplantInstallResult TryInstall(string survivorId, LimbId limb, string implantId, int day, ISeededRng? rng)
        {
            var def = Definition(implantId);
            if (def == null) return ImplantInstallResult.Fail("unknown_implant");
            if (InstanceFor(survivorId, limb) != null) return ImplantInstallResult.Fail("already_implanted");
            if (!SlotMatches(def.body_slot, limb))
                return ImplantInstallResult.Fail("body_slot_mismatch");

            var limbState = _amputation.GetLimb(survivorId, limb);
            if (limbState == null) return ImplantInstallResult.Fail("socket_ineligible");
            if (limbState.condition != LimbCondition.Amputated && limbState.condition != LimbCondition.Prosthetic)
                return ImplantInstallResult.Fail("socket_ineligible");
            if (limbState.recoveryDaysLeft > 0)
                return ImplantInstallResult.Fail("limb_in_recovery");

            if (_itemCount == null || _itemConsume == null) return ImplantInstallResult.Fail("inventory_unbound");
            if (!string.IsNullOrEmpty(def.required_surgery_tool_id)
                && _itemCount(def.required_surgery_tool_id) <= 0)
                return ImplantInstallResult.Fail("missing_surgery_tool");
            foreach (var req in def.required_item_ids)
                if (_itemCount(req) <= 0)
                    return ImplantInstallResult.Fail("missing_item_" + req);

            // Atomically consume the surgery bill, then mutate the limb THROUGH
            // its authority (socket rules + phantom-pain clearing stay there).
            if (!string.IsNullOrEmpty(def.required_surgery_tool_id))
                _itemConsume(def.required_surgery_tool_id, 1);
            foreach (var req in def.required_item_ids)
                _itemConsume(req, 1);

            var upgrade = _amputation.UpgradeToBionic(survivorId, limb, implantId, chargeInventoryItem: false);
            if (upgrade.Status != global::Ashfall.Core.ActionResult.StatusKind.Success)
                return ImplantInstallResult.Fail(string.IsNullOrEmpty(upgrade.FailureCode) ? "socket_ineligible" : upgrade.FailureCode);

            _state.instance_counter++;
            var instance = new ImplantInstanceState
            {
                instance_id = $"implant_inst_{_state.instance_counter}",
                implant_id = implantId,
                survivor_id = survivorId,
                body_slot_limb = (int)limb,
                condition = def.condition_max,
                installed_day = day,
                integration_status = (int)ImplantIntegrationStatus.Integrating,
                integration_days_left = def.integration_recovery_days,
                integration_days_total = def.integration_recovery_days,
                battery_days_remaining = def.battery_days,
                last_maintenance_day = day
            };
            _state.implants.Add(instance);

            // Integration complication roll — deterministic from the forked rng.
            var complication = ImplantComplication.None;
            if (rng != null && def.integration_risk_bp > 0
                && rng.NextDouble() < def.integration_risk_bp / 10000.0)
            {
                complication = RollComplication(rng, def);
                instance.complication = (int)complication;
                instance.complication_days_left = complication == ImplantComplication.ChronicPain ? -1 : 3;
                OnImplantComplication?.Invoke(instance, complication);
            }

            OnImplantInstalled?.Invoke(instance, def);
            return new ImplantInstallResult { Success = true, ReasonCode = "installed", Instance = instance, Complication = complication };
        }

        private static ImplantComplication RollComplication(ISeededRng rng, ImplantDefinition def)
        {
            double roll = rng.NextDouble();
            if (def.implant_class == "neuro_linked" && roll < 0.4) return ImplantComplication.NeuralAdaptationFailure;
            if (roll < 0.3) return ImplantComplication.ChronicPain;
            return ImplantComplication.Inflammation;
        }

        /// <summary>Planned surgical removal: limb reverts through its authority.</summary>
        public bool RemoveImplant(string survivorId, LimbId limb)
        {
            var instance = InstanceFor(survivorId, limb);
            if (instance == null) return false;
            var revert = _amputation.RevertBionicToAmputated(survivorId, limb);
            if (revert.Status != global::Ashfall.Core.ActionResult.StatusKind.Success) return false;
            _state.implants.Remove(instance);
            OnImplantRemoved?.Invoke(instance);
            return true;
        }

        // ── Daily tick (§6.8-6.10) ─────────────────────────────────────

        /// <summary>
        /// One bionics day: integration countdown → power state → condition
        /// decay → maintenance-overdue malfunction roll → complication
        /// resolution → destruction. Deterministic except the forked roll.
        /// </summary>
        public void TickDay(int day)
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;

            var ordered = _state.implants.Where(i => i != null)
                .OrderBy(i => i.instance_id, StringComparer.Ordinal).ToList();
            foreach (var instance in ordered)
            {
                if (instance.destroyed) continue;
                var def = Definition(instance.implant_id);
                if (def == null) continue;

                // 1. Integration countdown (rehab, §6.8).
                if (instance.integration_status == (int)ImplantIntegrationStatus.Integrating
                    && instance.integration_days_left > 0)
                {
                    instance.integration_days_left--;
                    if (instance.integration_days_left == 0)
                    {
                        instance.integration_status = (int)ImplantIntegrationStatus.Integrated;
                        OnIntegrationCompleted?.Invoke(instance);
                    }
                }

                // 2. Power state (§6.10): no free energy — charge ONLY when the
                // host reports a real charger; otherwise the battery drains.
                if (def.power_profile != "passive")
                {
                    bool charging = ChargerAvailable != null && ChargerAvailable();
                    instance.is_charging = charging;
                    if (charging)
                        instance.battery_days_remaining = def.battery_days;
                    else
                        instance.battery_days_remaining = Math.Max(0f, instance.battery_days_remaining - 1f);
                }

                // 3. Condition decay (authored bp/day, §6.9).
                float decay = def.daily_condition_decay_bp / 100f;
                float newCondition = Math.Max(BionicsCaps.DestroyedCondition, instance.condition - decay);
                if (Math.Abs(newCondition - instance.condition) > 0.0001f)
                {
                    instance.condition = newCondition;
                    OnImplantConditionChanged?.Invoke(instance);
                }

                // 4. Maintenance overdue → typed malfunction roll (§6.9).
                if (instance.malfunction == (int)ImplantMalfunction.None
                    && day - instance.last_maintenance_day > def.maintenance_interval_days + BionicsCaps.OverdueMalfunctionGraceDays
                    && MalfunctionRoll != null
                    && MalfunctionRoll() < def.malfunction_risk_bp / 10000.0)
                {
                    var malfunction = RollMalfunction(def);
                    instance.malfunction = (int)malfunction.kind;
                    instance.malfunction_days_left = malfunction.days;
                    OnImplantMalfunctioned?.Invoke(instance, malfunction.kind);
                }
                else if (instance.malfunction != (int)ImplantMalfunction.None)
                {
                    instance.malfunction_days_left--;
                    if (instance.malfunction_days_left <= 0)
                        instance.malfunction = (int)ImplantMalfunction.None;
                }

                // 5. Complication resolution (medical host treats, §6.6).
                if (instance.complication != (int)ImplantComplication.None
                    && instance.complication_days_left > 0)
                {
                    instance.complication_days_left--;
                    if (instance.complication_days_left <= 0)
                        instance.complication = (int)ImplantComplication.None;
                }

                // 6. Destruction — the limb authority reverts the socket (§6.18).
                if (instance.condition <= BionicsCaps.DestroyedCondition)
                {
                    instance.destroyed = true;
                    _amputation.RevertBionicToAmputated(instance.survivor_id, (LimbId)instance.body_slot_limb);
                    OnImplantDestroyed?.Invoke(instance);
                }
            }
        }

        private static (ImplantMalfunction kind, int days) RollMalfunction(ImplantDefinition def)
        {
            // Typed by class and slot — never generic damage (§6.11).
            if (def.power_profile == "high_draw") return (ImplantMalfunction.Stunned, 1);
            if (def.body_slot == "leg") return (ImplantMalfunction.ActuatorLock, 2);
            return (ImplantMalfunction.SensorBlackout, 2);
        }

        // ── Electrical disruption (§6.11, Trap J) ─────────────────────

        /// <summary>
        /// Typed EMP/electrical response by implant class — NEVER health damage.
        /// Passive-mechanical implants are immune; battery classes drain;
        /// high-draw classes suffer a brief stun; condition damage scales with
        /// severity (0..1 abstract band supplied by the caller).
        /// </summary>
        public List<ImplantDisruptionOutcome> ApplyElectricalDisruption(string survivorId, float severity)
        {
            severity = Math.Clamp(severity, 0f, 1f);
            var outcomes = new List<ImplantDisruptionOutcome>();
            foreach (var instance in ImplantsFor(survivorId))
            {
                if (instance.destroyed) continue;
                var def = Definition(instance.implant_id);
                if (def == null) continue;

                var vuln = ParseVulnerability(def.electrical_vulnerability);
                if (vuln == ImplantElectricalVulnerability.None) continue;

                var outcome = new ImplantDisruptionOutcome { InstanceId = instance.instance_id };

                if (def.power_profile != "passive")
                {
                    float drain = 1f + severity * (vuln == ImplantElectricalVulnerability.High ? 2f : 1f);
                    instance.battery_days_remaining = Math.Max(0f, instance.battery_days_remaining - drain);
                    outcome.BatteryDaysDrained = drain;
                }

                if (def.power_profile == "high_draw" && severity > 0.3f)
                {
                    instance.malfunction = (int)ImplantMalfunction.Stunned;
                    instance.malfunction_days_left = 1;
                    outcome.Effect = "stunned";
                    outcome.Days = 1;
                }
                else if (severity > 0.6f)
                {
                    var malfunction = def.body_slot == "leg" ? ImplantMalfunction.ActuatorLock : ImplantMalfunction.SensorBlackout;
                    instance.malfunction = (int)malfunction;
                    instance.malfunction_days_left = 2;
                    outcome.Effect = malfunction == ImplantMalfunction.ActuatorLock ? "actuator_lock" : "sensor_blackout";
                    outcome.Days = 2;
                }

                float conditionDamage = severity * (vuln == ImplantElectricalVulnerability.High ? 20f : 8f);
                if (conditionDamage > 0f)
                {
                    instance.condition = Math.Max(BionicsCaps.DestroyedCondition, instance.condition - conditionDamage);
                    outcome.Effect = string.IsNullOrEmpty(outcome.Effect) ? "condition_damage" : outcome.Effect + "+condition_damage";
                    OnImplantConditionChanged?.Invoke(instance);
                }
                outcomes.Add(outcome);
            }
            return outcomes;
        }

        private static ImplantElectricalVulnerability ParseVulnerability(string label) =>
            label switch
            {
                "low" => ImplantElectricalVulnerability.Low,
                "high" => ImplantElectricalVulnerability.High,
                _ => ImplantElectricalVulnerability.None
            };

        // ── Maintenance & repair (§6.9) ────────────────────────────────

        /// <summary>Scheduled maintenance: consumes a canonical kit, resets the
        /// interval clock, and restores bounded condition.</summary>
        public bool PerformMaintenance(string survivorId, LimbId limb, string kitItemId, int day)
        {
            var instance = InstanceFor(survivorId, limb);
            if (instance == null || instance.destroyed) return false;
            var def = Definition(instance.implant_id);
            if (def == null) return false;
            if (_itemCount == null || _itemConsume == null) return false;
            if (_itemCount(kitItemId) <= 0) return false;

            _itemConsume(kitItemId, 1);
            instance.condition = Math.Min(def.condition_max, instance.condition + 40f);
            instance.last_maintenance_day = day;
            OnImplantConditionChanged?.Invoke(instance);
            return true;
        }

        /// <summary>Repair a damaged (not destroyed) implant with a repair kit.</summary>
        public bool Repair(string survivorId, LimbId limb, string kitItemId)
        {
            var instance = InstanceFor(survivorId, limb);
            if (instance == null || instance.destroyed) return false;
            var def = Definition(instance.implant_id);
            if (def == null) return false;
            if (_itemCount == null || _itemConsume == null) return false;
            if (_itemCount(kitItemId) <= 0) return false;

            _itemConsume(kitItemId, 1);
            instance.condition = Math.Min(def.condition_max, instance.condition + 60f);
            instance.last_maintenance_day = _state.last_tick_day;
            OnImplantConditionChanged?.Invoke(instance);
            return true;
        }

        // ── Bounded capability queries (§6.7) ─────────────────────────

        /// <summary>
        /// Extra capability bonus (bp) for one limb, ON TOP of the
        /// AmputationSystem base condition multiplier (which already grants
        /// Bionic +0.10/+0.15). Bounded by <see cref="BionicsCaps.CapabilityBonusCapBp"/>;
        /// gated by power, malfunction, condition, integration, and complications.
        /// Consumers apply it — this system never writes survivor stats.
        /// </summary>
        public float GetLimbCapabilityBonusBp(string survivorId, LimbId limb)
        {
            var instance = InstanceFor(survivorId, limb);
            if (instance == null || instance.destroyed) return 0f;
            var def = Definition(instance.implant_id);
            if (def == null) return 0f;

            // Hard gate: unpowered battery implant contributes nothing (§6.18).
            if (def.power_profile != "passive"
                && instance.battery_days_remaining <= 0f && !instance.is_charging)
                return 0f;

            // Malfunction → zero function while active.
            if (instance.malfunction != (int)ImplantMalfunction.None) return 0f;

            // Condition gate: zero below floor; smooth recovery up to 50%.
            float conditionFactor = instance.condition <= BionicsCaps.ConditionFunctionFloor
                ? 0f
                : Math.Clamp((instance.condition - BionicsCaps.ConditionFunctionFloor)
                    / (50f - BionicsCaps.ConditionFunctionFloor), 0f, 1f);

            // Rehab factor: RehabStartFactor at install → 1.0 when integrated.
            float integrationFactor = 1f;
            if (instance.integration_status == (int)ImplantIntegrationStatus.Integrating
                && instance.integration_days_total > 0)
            {
                float progress = 1f - (float)instance.integration_days_left / instance.integration_days_total;
                integrationFactor = BionicsCaps.RehabStartFactor + (1f - BionicsCaps.RehabStartFactor) * progress;
            }

            float complicationFactor = instance.complication != (int)ImplantComplication.None ? 0.5f : 1f;

            // Restoration above full capability + a damped skill modifier,
            // bounded by the hard cap.
            float restorationBp = Math.Max(0f, def.functional_restore_bp - 1000f);
            float skillContribution = Math.Clamp(def.skill_modifier_bp * 0.3f, 0f, BionicsCaps.CapabilityBonusCapBp);
            float total = (restorationBp + skillContribution) * conditionFactor * integrationFactor * complicationFactor;
            return Math.Min(total, BionicsCaps.CapabilityBonusCapBp);
        }

        // ── Combat damage handoff (§6.12) ─────────────────────────────

        /// <summary>Trauma-system handoff: combat may damage or destroy an
        /// implant. This system never calls it on its own.</summary>
        public ImplantDisruptionOutcome ApplyCombatDamage(string survivorId, LimbId limb, float severity)
        {
            var instance = InstanceFor(survivorId, limb)
                ?? throw new InvalidOperationException("no implant on limb");
            severity = Math.Clamp(severity, 0f, 1f);
            instance.condition = Math.Max(BionicsCaps.DestroyedCondition, instance.condition - 60f * severity);
            OnImplantConditionChanged?.Invoke(instance);
            if (instance.condition <= BionicsCaps.DestroyedCondition)
            {
                instance.destroyed = true;
                _amputation.RevertBionicToAmputated(survivorId, limb);
                OnImplantDestroyed?.Invoke(instance);
            }
            return new ImplantDisruptionOutcome
            {
                InstanceId = instance.instance_id,
                Effect = instance.destroyed ? "destroyed" : "condition_damage"
            };
        }

        // ── Save (§6.17) ───────────────────────────────────────────────

        public BionicsSystemState CaptureState()
        {
            var copy = new BionicsSystemState
            {
                instance_counter = _state.instance_counter,
                last_tick_day = _state.last_tick_day,
                implants = new List<ImplantInstanceState>(_state.implants.Count)
            };
            foreach (var i in _state.implants)
            {
                if (i == null) continue;
                copy.implants.Add(CloneInstance(i));
            }
            return copy;
        }

        public void RestoreState(BionicsSystemState? state)
        {
            if (state == null) return;
            _state = new BionicsSystemState
            {
                instance_counter = state.instance_counter,
                last_tick_day = state.last_tick_day,
                implants = new List<ImplantInstanceState>(state.implants?.Count ?? 0)
            };
            if (state.implants != null)
                foreach (var i in state.implants)
                    if (i != null) _state.implants.Add(CloneInstance(i));
        }

        private static ImplantInstanceState CloneInstance(ImplantInstanceState i) => new ImplantInstanceState
        {
            instance_id = i.instance_id, implant_id = i.implant_id,
            survivor_id = i.survivor_id, body_slot_limb = i.body_slot_limb,
            condition = i.condition, installed_day = i.installed_day,
            integration_status = i.integration_status,
            integration_days_left = i.integration_days_left,
            integration_days_total = i.integration_days_total,
            battery_days_remaining = i.battery_days_remaining,
            is_charging = i.is_charging,
            last_maintenance_day = i.last_maintenance_day,
            complication = i.complication, complication_days_left = i.complication_days_left,
            malfunction = i.malfunction, malfunction_days_left = i.malfunction_days_left,
            destroyed = i.destroyed
        };
    }

    // ── Strict catalog loader (repo pattern: schema envelope, snake_case,
    //    collected errors, never throws) ──────────────────────────────────

    public static class BionicsCatalogLoader
    {
        public const string FileName = "bionics.json";
        public const int CurrentSchemaVersion = 1;

        public static readonly IReadOnlyList<string> AcceptedBodySlots = new[] { "arm", "leg" };
        public static readonly IReadOnlyList<string> AcceptedClasses = new[] { "mechanical", "powered", "rechargeable", "neuro_linked" };
        public static readonly IReadOnlyList<string> AcceptedPowerProfiles = new[] { "passive", "rechargeable", "high_draw" };
        public static readonly IReadOnlyList<string> AcceptedVulnerabilities = new[] { "none", "low", "high" };

        public const int MaxFunctionalRestoreBp = 1200;
        public const int MaxSkillModifierBp = 500;
        public const int MaxDailyDrawWatts = 60;
        public const int MaxBatteryDays = 14;
        public const int MaxMaintenanceInterval = 30;
        public const int MaxDecayBp = 200;
        public const int MaxIntegrationRiskBp = 3000;
        public const int MaxRecoveryDays = 30;
        public const int MaxMalfunctionRiskBp = 1000;

        private static readonly Regex SnakeCase = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);

        public static BionicsCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new BionicsCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            BionicsCatalogRoot root;
            try
            {
                root = json.Deserialize<BionicsCatalogRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null)
            {
                result.Errors.Add("catalog parsed to null");
                return result;
            }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"schema_version {root.schema_version} newer than supported {CurrentSchemaVersion}");
                return result;
            }
            if (root.schema_version < 1)
            {
                result.Errors.Add($"schema_version {root.schema_version} invalid");
                return result;
            }
            if (root.implants == null || root.implants.Count == 0)
            {
                result.Errors.Add("catalog has no implant rows");
                return result;
            }

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.implants.Count; i++)
            {
                var def = root.implants[i];
                string at = $"implants[{i}]";
                if (def == null)
                {
                    result.Errors.Add(at + ": null row");
                    continue;
                }

                if (string.IsNullOrEmpty(def.implant_id) || !SnakeCase.IsMatch(def.implant_id)
                    || !def.implant_id.StartsWith("implant_", StringComparison.Ordinal))
                {
                    result.Errors.Add(at + ": implant_id must be snake_case with implant_ prefix");
                    continue;
                }
                if (!seenIds.Add(def.implant_id))
                {
                    result.Errors.Add(at + ": duplicate implant_id " + def.implant_id);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(def.display_name))
                    result.Errors.Add(at + " (" + def.implant_id + "): display_name required");
                if (!AcceptedBodySlots.Contains(def.body_slot))
                    result.Errors.Add(at + " (" + def.implant_id + "): body_slot must be arm|leg");
                if (!AcceptedClasses.Contains(def.implant_class))
                    result.Errors.Add(at + " (" + def.implant_id + "): implant_class vocabulary violation");
                if (!AcceptedPowerProfiles.Contains(def.power_profile))
                    result.Errors.Add(at + " (" + def.implant_id + "): power_profile vocabulary violation");

                bool passive = def.power_profile == "passive";
                if (passive && (def.daily_power_draw_watts != 0 || def.battery_days != 0))
                    result.Errors.Add(at + " (" + def.implant_id + "): passive profile requires zero draw and battery");
                if (!passive && (def.battery_days <= 0 || def.daily_power_draw_watts <= 0))
                    result.Errors.Add(at + " (" + def.implant_id + "): battery profile requires draw and endurance");

                if (def.functional_restore_bp < 500 || def.functional_restore_bp > MaxFunctionalRestoreBp)
                    result.Errors.Add(at + " (" + def.implant_id + "): functional_restore_bp out of range [500," + MaxFunctionalRestoreBp + "]");
                if (def.skill_modifier_bp < 0 || def.skill_modifier_bp > MaxSkillModifierBp)
                    result.Errors.Add(at + " (" + def.implant_id + "): skill_modifier_bp out of range [0," + MaxSkillModifierBp + "]");
                if (def.daily_power_draw_watts < 0 || def.daily_power_draw_watts > MaxDailyDrawWatts)
                    result.Errors.Add(at + " (" + def.implant_id + "): daily_power_draw_watts out of range");
                if (def.battery_days < 0 || def.battery_days > MaxBatteryDays)
                    result.Errors.Add(at + " (" + def.implant_id + "): battery_days out of range");
                if (def.maintenance_interval_days < 1 || def.maintenance_interval_days > MaxMaintenanceInterval)
                    result.Errors.Add(at + " (" + def.implant_id + "): maintenance_interval_days out of range [1," + MaxMaintenanceInterval + "]");
                if (def.daily_condition_decay_bp < 0 || def.daily_condition_decay_bp > MaxDecayBp)
                    result.Errors.Add(at + " (" + def.implant_id + "): daily_condition_decay_bp out of range");
                if (def.integration_risk_bp < 0 || def.integration_risk_bp > MaxIntegrationRiskBp)
                    result.Errors.Add(at + " (" + def.implant_id + "): integration_risk_bp out of range");
                if (def.integration_recovery_days < 1 || def.integration_recovery_days > MaxRecoveryDays)
                    result.Errors.Add(at + " (" + def.implant_id + "): integration_recovery_days out of range");
                if (def.malfunction_risk_bp < 0 || def.malfunction_risk_bp > MaxMalfunctionRiskBp)
                    result.Errors.Add(at + " (" + def.implant_id + "): malfunction_risk_bp out of range");
                if (!new[] { "none", "low", "high" }.Contains(def.electrical_vulnerability))
                    result.Errors.Add(at + " (" + def.implant_id + "): electrical_vulnerability must be none|low|high");

                if (def.required_item_ids == null || def.required_item_ids.Count == 0)
                    result.Errors.Add(at + " (" + def.implant_id + "): required_item_ids required");

                result.Implants.Add(def);
            }

            return result;
        }

        /// <summary>Index a successful load result into runtime definitions.</summary>
        public static List<ImplantDefinition> ToDefinitions(BionicsCatalogLoadResult result)
        {
            var list = new List<ImplantDefinition>();
            if (result == null) return list;
            foreach (var def in result.Implants)
                if (def != null && !string.IsNullOrEmpty(def.implant_id)) list.Add(def);
            return list;
        }
    }
}
