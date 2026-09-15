// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Amphibious draisine crossing state machine (plan §7.4). LandReady is
    /// the resting state; crossings are expensive, risky, and always
    /// recoverable to LandReady.
    /// </summary>
    public enum AmphibiousCrossingPhase
    {
        NoKit,
        LandReady,
        Deploying,
        WaterReady,
        Crossing,
        Landing,
        Recovering,
        Aborted,
        EmergencyRecovery,
        Disabled
    }

    /// <summary>Typed failure codes (plan §12). Host supplies localized prose.</summary>
    public static class AmphibiousFailureCodes
    {
        public const string VehicleIncompatible = "vehicle_incompatible";
        public const string KitNotInstalled = "kit_not_installed";
        public const string FlotationMarginInsufficient = "flotation_margin_insufficient";
        public const string CurrentRiskTooHigh = "current_risk_too_high";
        public const string PumpUnavailable = "pump_unavailable";
        public const string PontoonDamaged = "pontoon_damaged";
        public const string CrossingAlreadyActive = "crossing_already_active";
        public const string RouteNotAmphibiousCapable = "route_not_amphibious_capable";
    }

    /// <summary>Per-vehicle amphibious kit state. Owned by this engine only.</summary>
    public sealed class AmphibiousDraisineState
    {
        public string VehicleId { get; set; } = string.Empty;
        public string KitProfileId { get; set; } = string.Empty;
        public AmphibiousCrossingPhase Phase { get; set; } = AmphibiousCrossingPhase.NoKit;
        public int PhaseTicks { get; set; }
        /// <summary>Kit fabric condition, bp: 10000 = pristine.</summary>
        public int KitConditionBp { get; set; } = 10000;
        /// <summary>Pontoon condition, bp: 10000 = intact. Damage raises ingress risk.</summary>
        public int PontoonConditionBp { get; set; } = 10000;
        /// <summary>Hull ingress level 0..10000 (10000 = swamped).</summary>
        public int IngressBp { get; set; }
        /// <summary>Cargo load fraction 0..1 for the current crossing decision.</summary>
        public float CargoLoadFraction { get; set; }
        /// <summary>Active route class id while deployed/crossing.</summary>
        public string ActiveRouteClassId { get; set; } = string.Empty;
        public int CrossingProgressBp { get; set; }
        public string FaultCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// Amphibious draisine outrigger engine (Plan 125 Core). Fictionalized
    /// vehicle-kit capability: kit installs against vehicle tags, deployment
    /// costs ticks, crossings are gated by typed flotation margins and
    /// current-risk thresholds, and hazards (pontoon damage, ingress, pump
    /// degradation) are seeded. Water routes stay topology-owned — this
    /// engine only declares a typed route capability; open-water naval
    /// travel remains with the naval authority. No real conversion or
    /// flotation engineering is modeled or documented.
    /// </summary>
    public sealed class AmphibiousDraisineEngine
    {
        public const string SystemId = "amphibious_draisine";

        // ── Bounded design constants (plan §7.6, §7.8) ──────────────────
        /// <summary>Minimum flotation margin (bp) for any attempted crossing.</summary>
        public const int MinFlotationMarginBp = 1500;
        /// <summary>Cargo load fraction that counts fully against flotation.</summary>
        public const float CargoFlotationPenaltyFactor = 0.6f;
        /// <summary>Kit mass fraction that counts against flotation when installed.</summary>
        public const float KitMassFlotationPenaltyFactor = 0.15f;
        /// <summary>Vehicle damage (fraction of condition lost) that counts against flotation.</summary>
        public const float VehicleDamageFlotationFactor = 0.3f;
        /// <summary>Ingress above this level forces emergency recovery (bp).</summary>
        public const int IngressEmergencyBp = 8000;
        /// <summary>Pontoon damage below this level cannot attempt crossings (bp).</summary>
        public const int PontoonUsableThresholdBp = 2500;
        /// <summary>Kits can be installed only in a workshop.</summary>
        public const bool RequireWorkshopForInstall = true;

        private readonly Dictionary<string, AmphibiousDraisineState> _vehicles = new(StringComparer.Ordinal);
        private readonly AmphibiousDraisineCatalog _catalog;
        private readonly ILog _log;

        public ISeededRng? Rng { get; set; }

        public AmphibiousDraisineState? FindVehicle(string vehicleId)
            => !string.IsNullOrEmpty(vehicleId) && _vehicles.TryGetValue(vehicleId, out var v) ? v : null;
        public AmphibiousDraisineCatalog Catalog => _catalog;

        public AmphibiousDraisineEngine(AmphibiousDraisineCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        private AmphibiousKitProfile? ProfileOf(AmphibiousDraisineState state)
            => _catalog.GetKit(string.IsNullOrEmpty(state.KitProfileId) ? "" : state.KitProfileId);

        // ── Installation (vehicle upgrade, not a new vehicle authority) ──

        public ActionResult InstallKit(string vehicleId, string vehicleTag, string kitProfileId,
            bool workshopAvailable, bool partsAvailable, int vehicleConditionBp)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                return ActionResult.Blocked(AmphibiousFailureCodes.VehicleIncompatible, "amb.vehicle_id_missing");
            var kit = _catalog.GetKit(kitProfileId ?? string.Empty);
            if (kit == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_profile_unknown");
            if (!kit.compatible_vehicle_tags.Contains(vehicleTag ?? string.Empty, StringComparer.Ordinal))
                return ActionResult.Blocked(AmphibiousFailureCodes.VehicleIncompatible, "amb.vehicle_incompatible");
            if (vehicleConditionBp < kit.min_vehicle_condition_bp)
                return ActionResult.Blocked(AmphibiousFailureCodes.VehicleIncompatible, "amb.vehicle_condition_insufficient");
            if (!partsAvailable)
                return ActionResult.Blocked("amb_install_parts_missing", "amb.install_parts_missing");
            if (RequireWorkshopForInstall && !workshopAvailable)
                return ActionResult.Blocked("amb_workshop_required", "amb.workshop_required");

            var existing = FindVehicle(vehicleId);
            if (existing != null && existing.Phase != AmphibiousCrossingPhase.LandReady
                && existing.Phase != AmphibiousCrossingPhase.NoKit
                && existing.Phase != AmphibiousCrossingPhase.Disabled)
                return ActionResult.Blocked("amb_vehicle_busy", "amb.vehicle_busy");

            _vehicles[vehicleId] = new AmphibiousDraisineState
            {
                VehicleId = vehicleId,
                KitProfileId = kit.id,
                Phase = AmphibiousCrossingPhase.LandReady
            };
            return ActionResult.Success("amb_kit_installed", eventId: "amb.install_kit");
        }

        public ActionResult UninstallKit(string vehicleId, bool workshopAvailable)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            if (state.Phase != AmphibiousCrossingPhase.LandReady && state.Phase != AmphibiousCrossingPhase.Disabled)
                return ActionResult.Blocked("amb_vehicle_busy", "amb.vehicle_busy");
            if (RequireWorkshopForInstall && !workshopAvailable)
                return ActionResult.Blocked("amb_workshop_required", "amb.workshop_required");
            _vehicles.Remove(vehicleId);
            return ActionResult.Success("amb_kit_removed", eventId: "amb.uninstall_kit");
        }

        // ── Deployment (costs ticks; never instant) ─────────────────────

        public ActionResult Deploy(string vehicleId)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            if (state.Phase != AmphibiousCrossingPhase.LandReady)
                return ActionResult.Blocked("amb_not_land_ready", "amb.not_land_ready");
            if (state.PontoonConditionBp < PontoonUsableThresholdBp)
                return ActionResult.Blocked(AmphibiousFailureCodes.PontoonDamaged, "amb.pontoon_damaged");

            var kit = ProfileOf(state)!;
            state.Phase = AmphibiousCrossingPhase.Deploying;
            state.PhaseTicks = kit.deployment_ticks;
            return ActionResult.Success("amb_deploying", eventId: "amb.deploy");
        }

        /// <summary>Advances deployment/recovery ticks. Returns true when a phase boundary was crossed.</summary>
        public bool TickDeployment(string vehicleId)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return false;
            switch (state.Phase)
            {
                case AmphibiousCrossingPhase.Deploying:
                    state.PhaseTicks--;
                    if (state.PhaseTicks <= 0)
                    {
                        state.Phase = AmphibiousCrossingPhase.WaterReady;
                        state.PhaseTicks = 0;
                        return true;
                    }
                    return false;
                case AmphibiousCrossingPhase.Recovering:
                    state.PhaseTicks--;
                    if (state.PhaseTicks <= 0)
                    {
                        state.Phase = AmphibiousCrossingPhase.LandReady;
                        state.PhaseTicks = 0;
                        state.CrossingProgressBp = 0;
                        state.IngressBp = 0;
                        state.ActiveRouteClassId = string.Empty;
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public ActionResult StowKit(string vehicleId)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            if (state.Phase != AmphibiousCrossingPhase.WaterReady)
                return ActionResult.Blocked("amb_not_water_ready", "amb.not_water_ready");
            state.Phase = AmphibiousCrossingPhase.Recovering;
            state.PhaseTicks = 2;
            return ActionResult.Success("amb_recovering", eventId: "amb.stow");
        }

        /// <summary>Adjusts the cargo load for the pending crossing decision.</summary>
        public ActionResult SetCargoLoad(string vehicleId, float cargoLoadFraction)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            state.CargoLoadFraction = Math.Clamp(cargoLoadFraction, 0f, 1f);
            return ActionResult.Success("amb_cargo_set", eventId: "amb.set_cargo");
        }

        // ── Typed route capability (route planner seam; topology stays map-owned)

        /// <summary>
        /// Typed capability check consumed by the route planner. True only
        /// when the kit is installed, deployed or deployable, and the route
        /// class is one the kit unlocks. Never unlocks raw water tiles and
        /// never touches naval authority.
        /// </summary>
        public bool TryGetRouteCapability(string vehicleId, string routeClassId, out string failureCode)
        {
            failureCode = AmphibiousFailureCodes.KitNotInstalled;
            var state = FindVehicle(vehicleId);
            if (state == null)
                return false;
            var kit = ProfileOf(state)!;
            if (!kit.route_class_ids.Contains(routeClassId, StringComparer.Ordinal))
            {
                failureCode = AmphibiousFailureCodes.RouteNotAmphibiousCapable;
                return false;
            }
            failureCode = string.Empty;
            return true;
        }

        /// <summary>
        /// Flotation margin for a crossing attempt (bp). Positive margin is
        /// required; the decision is the player's loadout tradeoff.
        /// </summary>
        public int ComputeFlotationMargin(string vehicleId, string routeClassId,
            float vehicleMassFraction, float cargoLoadFraction, int vehicleConditionBp, out string failureCode)
        {
            failureCode = string.Empty;
            var state = FindVehicle(vehicleId);
            if (state == null)
            {
                failureCode = AmphibiousFailureCodes.KitNotInstalled;
                return 0;
            }
            var kit = ProfileOf(state)!;
            var routeClass = _catalog.GetRouteClass(routeClassId);
            if (routeClass == null || !kit.route_class_ids.Contains(routeClassId, StringComparer.Ordinal))
            {
                failureCode = AmphibiousFailureCodes.RouteNotAmphibiousCapable;
                return 0;
            }

            float vehicleDamage = 1f - Math.Clamp(vehicleConditionBp, 0, 10000) / 10000f;
            float margin = kit.flotation_rating
                - Math.Clamp(vehicleMassFraction, 0f, 1f)
                - Math.Clamp(cargoLoadFraction, 0f, 1f) * CargoFlotationPenaltyFactor
                - KitMassFlotationPenaltyFactor
                - vehicleDamage * VehicleDamageFlotationFactor;
            int marginBp = (int)(margin * 10000f);
            if (marginBp < MinFlotationMarginBp)
                failureCode = AmphibiousFailureCodes.FlotationMarginInsufficient;
            return marginBp;
        }

        // ── Crossings ────────────────────────────────────────────────────

        public ActionResult BeginCrossing(string vehicleId, string routeClassId,
            float vehicleMassFraction, int vehicleConditionBp, bool pumpPowerAvailable)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            if (state.Phase == AmphibiousCrossingPhase.Crossing || state.Phase == AmphibiousCrossingPhase.Deploying)
                return ActionResult.Blocked(AmphibiousFailureCodes.CrossingAlreadyActive, "amb.crossing_already_active");
            if (state.Phase != AmphibiousCrossingPhase.WaterReady)
                return ActionResult.Blocked("amb_not_water_ready", "amb.not_water_ready");
            if (state.PontoonConditionBp < PontoonUsableThresholdBp)
                return ActionResult.Blocked(AmphibiousFailureCodes.PontoonDamaged, "amb.pontoon_damaged");

            var kit = ProfileOf(state)!;
            if (!kit.route_class_ids.Contains(routeClassId, StringComparer.Ordinal))
                return ActionResult.Blocked(AmphibiousFailureCodes.RouteNotAmphibiousCapable, "amb.route_not_capable");

            var routeClass = _catalog.GetRouteClass(routeClassId);
            if (routeClass == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.RouteNotAmphibiousCapable, "amb.route_class_unknown");
            if (state.CargoLoadFraction * 10000f > kit.cargo_capacity_modifier_bp)
                return ActionResult.Blocked(AmphibiousFailureCodes.VehicleIncompatible, "amb.cargo_overloaded");

            var pump = _catalog.GetPump(kit.pump_profile_id);
            if (pump == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.PumpUnavailable, "amb.pump_profile_unknown");
            if (pump.power_source == "battery" && !pumpPowerAvailable)
                return ActionResult.Blocked(AmphibiousFailureCodes.PumpUnavailable, "amb.pump_power_unavailable");

            state.ActiveRouteClassId = routeClassId;
            state.Phase = AmphibiousCrossingPhase.Crossing;
            state.PhaseTicks = 0;
            state.CrossingProgressBp = 0;
            return ActionResult.Success("amb_crossing_started", eventId: "amb.begin_crossing");
        }

        /// <summary>
        /// Advances one crossing tick. Hazards are seeded; ingress above the
        /// emergency level forces recovery; the vehicle base state is never
        /// mutated here (vehicle logistics remains the owner).
        /// </summary>
        public ActionResult AdvanceCrossing(string vehicleId, float currentRisk,
            bool badWeather, float navigatorSkill, int vehicleConditionBp)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            if (state.Phase != AmphibiousCrossingPhase.Crossing)
                return ActionResult.Blocked("amb_not_crossing", "amb.not_crossing");

            var kit = ProfileOf(state)!;
            var routeClass = _catalog.GetRouteClass(state.ActiveRouteClassId);
            if (routeClass == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.RouteNotAmphibiousCapable, "amb.route_class_unknown");

            // ── Current risk gate: bounded by kit tolerance, weather-adjusted
            float effectiveRisk = Math.Clamp(currentRisk, 0f, 1f);
            if (badWeather)
                effectiveRisk = Math.Min(1f, effectiveRisk + 0.15f);
            int riskBp = (int)(effectiveRisk * 10000f);
            // Navigator skill reduces effective risk, bounded (never to zero).
            int skillReductionBp = (int)(Math.Clamp(navigatorSkill, 0f, 100f) * 20f); // max 2000 bp
            riskBp = Math.Max(0, riskBp - skillReductionBp);
            if (riskBp > (int)(kit.current_tolerance * 10000f))
            {
                AbortInternal(state, "current exceeded tolerance");
                return ActionResult.Blocked(AmphibiousFailureCodes.CurrentRiskTooHigh, "amb.current_risk_too_high");
            }

            // ── Progress: water speed modifier (fraction of land pace) ───
            float progressPerTick = kit.water_speed_modifier_bp / 10000f
                * (1f - effectiveRisk * 0.5f)
                * (0.6f + 0.4f * Math.Clamp(navigatorSkill, 0f, 100f) / 100f);
            state.CrossingProgressBp = Math.Min(10000,
                state.CrossingProgressBp + (int)(progressPerTick * 10000f));

            // ── Seeded hazards: pontoon damage + ingress ─────────────────
            var pump = _catalog.GetPump(kit.pump_profile_id)!;
            int pontoonDamageRiskBp = (int)(effectiveRisk * 3000)
                + (badWeather ? 1000 : 0)
                + (int)((1f - Math.Clamp(state.KitConditionBp, 0, 10000) / 10000f) * 2000)
                + (int)(Math.Clamp(state.CargoLoadFraction, 0f, 1f) * 1500)
                + (int)((1f - Math.Clamp(vehicleConditionBp, 0, 10000) / 10000f) * 1500);
            pontoonDamageRiskBp = Math.Max(0, pontoonDamageRiskBp - skillReductionBp / 2);
            if (Rng != null && Rng.Next(0, 10000) < pontoonDamageRiskBp)
            {
                int damageBp = 500 + Rng.Next(0, 1500);
                state.PontoonConditionBp = Math.Clamp(state.PontoonConditionBp - damageBp, 0, 10000);
                state.KitConditionBp = Math.Clamp(state.KitConditionBp - damageBp / 2, 0, 10000);
                if (state.PontoonConditionBp < PontoonUsableThresholdBp)
                {
                    AbortInternal(state, "pontoon crippled");
                    return ActionResult.Blocked(AmphibiousFailureCodes.PontoonDamaged, "amb.pontoon_damaged");
                }
            }

            int ingressRiskBp = (int)(effectiveRisk * 4000)
                + (badWeather ? 1200 : 0)
                + (10000 - state.PontoonConditionBp) / 2;
            ingressRiskBp = Math.Max(0, ingressRiskBp - skillReductionBp / 2);
            if (Rng != null && Rng.Next(0, 10000) < ingressRiskBp)
            {
                int ingressAmount = 800 + Rng.Next(0, 1200);
                // A working pump halves the intake; a failed pump mitigates
                // nothing (plan §7.9 — no real pump-sizing procedure).
                bool pumpFailed = Rng.Next(0, 10000) < 800 + (10000 - state.KitConditionBp) / 5;
                int mitigated = pumpFailed ? ingressAmount : ingressAmount / 2;
                state.IngressBp = Math.Clamp(state.IngressBp + mitigated, 0, 10000);
            }

            if (state.IngressBp >= IngressEmergencyBp)
            {
                EmergencyRecoverInternal(state);
                return ActionResult.Blocked(AmphibiousFailureCodes.PumpUnavailable, "amb.ingress_emergency");
            }

            if (state.CrossingProgressBp >= 10000)
            {
                state.Phase = AmphibiousCrossingPhase.Landing;
                state.PhaseTicks = 1;
                return ActionResult.Success("amb_crossing_landed", eventId: "amb.landing");
            }
            return ActionResult.Success("amb_crossing_progress", eventId: "amb.advance_crossing");
        }

        /// <summary>Completes Landing → Recovering → LandReady handoff.</summary>
        public bool TickLanding(string vehicleId)
        {
            var state = FindVehicle(vehicleId);
            if (state == null || state.Phase != AmphibiousCrossingPhase.Landing)
                return false;
            state.Phase = AmphibiousCrossingPhase.Recovering;
            state.PhaseTicks = 2;
            return true;
        }

        public ActionResult AbortCrossing(string vehicleId)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            if (state.Phase != AmphibiousCrossingPhase.Crossing && state.Phase != AmphibiousCrossingPhase.WaterReady)
                return ActionResult.Blocked("amb_not_crossing", "amb.not_crossing");
            AbortInternal(state, "crew aborted");
            return ActionResult.Success("amb_crossing_aborted", eventId: "amb.abort");
        }

        private void AbortInternal(AmphibiousDraisineState state, string reason)
        {
            state.Phase = AmphibiousCrossingPhase.EmergencyRecovery;
            state.PhaseTicks = 3;
            state.FaultCode = reason;
            state.CrossingProgressBp = 0;
        }

        private void EmergencyRecoverInternal(AmphibiousDraisineState state)
        {
            state.Phase = AmphibiousCrossingPhase.EmergencyRecovery;
            state.PhaseTicks = 4;
            state.FaultCode = "ingress_emergency";
        }

        /// <summary>Advances emergency recovery; returns true on returning to LandReady.</summary>
        public bool TickEmergencyRecovery(string vehicleId)
        {
            var state = FindVehicle(vehicleId);
            if (state == null || state.Phase != AmphibiousCrossingPhase.EmergencyRecovery)
                return false;
            state.PhaseTicks--;
            if (state.PhaseTicks <= 0)
            {
                state.Phase = AmphibiousCrossingPhase.LandReady;
                state.PhaseTicks = 0;
                state.CrossingProgressBp = 0;
                state.IngressBp = 0;
                state.ActiveRouteClassId = string.Empty;
                state.FaultCode = string.Empty;
                return true;
            }
            return false;
        }

        /// <summary>Field repair of pontoons/kit fabric (consumes kit repair items via host).</summary>
        public ActionResult RepairKit(string vehicleId, bool partsAvailable, float mechanicSkill)
        {
            var state = FindVehicle(vehicleId);
            if (state == null)
                return ActionResult.Blocked(AmphibiousFailureCodes.KitNotInstalled, "amb.kit_not_installed");
            if (!partsAvailable)
                return ActionResult.Blocked("amb_repair_parts_missing", "amb.repair_parts_missing");
            int restoreBp = (int)(2000 + Math.Clamp(mechanicSkill, 0f, 100f) * 20f); // up to +4000
            state.PontoonConditionBp = Math.Min(10000, state.PontoonConditionBp + restoreBp);
            state.KitConditionBp = Math.Min(10000, state.KitConditionBp + restoreBp / 2);
            return ActionResult.Success("amb_kit_repaired", eventId: "amb.repair_kit");
        }

    
        // ── Save round-trip (Plan 125 Phase 8): per-vehicle deep clone. ──
        public System.Collections.Generic.Dictionary<string, AmphibiousDraisineState> CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var copy = new System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>(StringComparer.Ordinal);
            foreach (var kv in _vehicles)
                copy[kv.Key] = s.Deserialize<AmphibiousDraisineState>(s.Serialize(kv.Value)) ?? new AmphibiousDraisineState();
            return copy;
        }

        public void RestoreState(System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>? saved)
        {
            if (saved == null) return; // old saves: no section → no kits
            _vehicles.Clear();
            var s = new SystemTextJsonSerializer();
            foreach (var kv in saved)
            {
                var state = s.Deserialize<AmphibiousDraisineState>(s.Serialize(kv.Value)) ?? new AmphibiousDraisineState();
                if (state.Phase < AmphibiousCrossingPhase.NoKit || state.Phase > AmphibiousCrossingPhase.Disabled)
                    state.Phase = AmphibiousCrossingPhase.LandReady;
                state.KitConditionBp = Math.Clamp(state.KitConditionBp, 0, 10000);
                state.PontoonConditionBp = Math.Clamp(state.PontoonConditionBp, 0, 10000);
                state.IngressBp = Math.Clamp(state.IngressBp, 0, 10000);
                _vehicles[kv.Key] = state;
            }
        }
}
}
