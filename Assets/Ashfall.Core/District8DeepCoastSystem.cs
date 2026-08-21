using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Reopening stage of the District 8 coastal perimeter — the deep coast
    /// beyond the Shelf roadstead. This is a narrow geographic sibling of
    /// Exp 01 Holdfast: it owns ONLY the sealed→…→operational state machine,
    /// the route graph beyond the Shelf, route-specific decisions, and the
    /// one-time survey/repair/narrative markers. Everything else (seasonal
    /// access, expeditions, dive, scavenge, contamination, inventory, faction
    /// standing, journal, global time) belongs to the existing systems and is
    /// wired by the host.
    ///
    /// Node spine (all beyond the Shelf, season-gated by IceRoadSystem via the
    /// loc_shelf_ prefix):
    ///   loc_shelf_foghorn (existing approach)
    ///     → loc_shelf_perimeter_breakwater (new — coastal perimeter)
    ///     → loc_shelf_service_channel (new — flooded naval service channel)
    ///     → loc_shelf_deep_berth (new — icebreaker maintenance berth)
    ///     → loc_maritime_icebreaker_dock (EXISTING anchor — the deep naval dock)
    ///
    /// Stages: sealed → surveyed → perimeter_open → dock_accessible →
    /// deep_berth_operational. Deterministic: all salvage/hazard rolls go
    /// through the ISeededRng passed per call (the host owns seeding).
    /// </summary>
    public enum DeepCoastStage
    {
        Sealed = 0,
        Surveyed = 1,
        PerimeterOpen = 2,
        DockAccessible = 3,
        DeepBerthOperational = 4
    }

    /// <summary>
    /// The meaningful reopening decision taken once the perimeter is surveyed.
    /// Mirrors the four authored choices; consequences (immediate + delayed)
    /// are applied by the system and surfaced to the host.
    /// </summary>
    public enum DeepCoastAccessDecision
    {
        None = 0,
        StabilizeRepair = 1,
        SalvageImmediate = 2,
        FleetControlled = 3,
        MunicipalControlled = 4
    }

    /// <summary>
    /// Serialized state of the District 8 deep-coast route. Owned by the
    /// HoldfastSave envelope (v5). Missing-state defaults: RestoreState(null)
    /// yields a freshly sealed route; unknown decisions map to None; float
    /// fields clamp to safe ranges. Future-version rejection lives on the
    /// envelope codec (HoldfastSaveCodec), not here.
    /// </summary>
    [Serializable]
    public class District8DeepCoastState
    {
        public string systemId = District8DeepCoastSystem.SystemId;
        public string expansionKey = District8DeepCoastSystem.ExpansionKey;
        public int stage = (int)DeepCoastStage.Sealed;
        public int accessDecision = (int)DeepCoastAccessDecision.None;
        public int decisionDay = -1;

        // Route clearance flags (one-time).
        public bool perimeterSurveyed;
        public bool perimeterCleared;
        public bool channelCleared;
        public bool berthRepaired;

        // Persistent hazard / structural state.
        public float structuralIntegrity = 100f; // 100 = sound; <60 forces the shoring bill
        public float contaminationLevel;         // 0..1 brine/industrial contamination
        public bool fleetLevyActive;             // Fleet-controlled: levy on dock salvage
        public bool officeAccessLimited;         // municipal-controlled: access restricted
        public bool fleetStoodUp;                // fleet-controlled: the Fleet came ashore

        // One-time narrative / journal markers (dedupe keys, never a parallel list).
        public List<string> narrativeMarkers = new List<string>();

        // Active dock-operation reference (the dive itself lives in MaritimeSaveStore).
        public string activeDockOperationId = string.Empty;
        public string activeDockOperationLocationId = string.Empty;
        public string dockOperationDiverId = string.Empty;
        public int dockOperationStartedDay = -1;

        // Daily-tick guard: degradation applies once per calendar day.
        public int lastTickDay = -1;

        /// <summary>True when the route reached the given stage or beyond.</summary>
        public bool AtLeast(DeepCoastStage s) => stage >= (int)s;
    }

    /// <summary>
    /// Route graph edge: one node with its cumulative travel hours from the
    /// Shelf approach (loc_shelf_foghorn) and its hazard profile.
    /// </summary>
    [Serializable]
    public sealed class DeepCoastRouteNode
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public float travelHours;   // cumulative from loc_shelf_foghorn
        public float dangerLevel;   // 1..10
        public float baseRadsPerHour;
        public float segmentHours;  // hours from the previous node
    }

    public sealed class District8DeepCoastSystem
    {
        public const string SystemId = "district8_deep_coast_system";
        public const string ExpansionKey = "expansion_district8_deep_coast";
        public const string RegionId = "region_district8_deep_coast";

        // Route anchors (canonical ids — audit, do not invent aliases).
        public const string RouteStartId = "loc_shelf_foghorn";
        public const string PerimeterBreakwaterId = "loc_shelf_perimeter_breakwater";
        public const string ServiceChannelId = "loc_shelf_service_channel";
        public const string DeepBerthId = "loc_shelf_deep_berth";
        public const string DockId = "loc_maritime_icebreaker_dock"; // existing Year of Ash anchor
        public const string DockDiveSiteId = "site_exp09_naval_patrol"; // existing dive site

        // Canonical factions (holdfast_factions.json / faction_lore.json).
        public const string FactionFleet = "faction_the_fleet";
        public const string FactionOffice = "faction_the_office";

        // Canonical goods (existing catalogs; no new items).
        public const string ItemScrapMetal = "scrap_metal";
        public const string ItemBrassFittings = "brass_fittings";
        public const string ItemFuel = "fuel";
        public const string ItemCleanWater = "clean_water";
        public const string ItemRoResin = "item_ro_resin";
        public const string ItemIodine = "iodine_pills";

        // Journal knowledge keys — once-only via the real JournalSystem.
        public const string JournalSurvey = "dc8_survey_perimeter";
        public const string JournalStabilize = "dc8_decision_stabilize";
        public const string JournalSalvage = "dc8_decision_salvage";
        public const string JournalFleet = "dc8_decision_fleet";
        public const string JournalMunicipal = "dc8_decision_municipal";
        public const string JournalDockOpen = "dc8_dock_open";
        public const string JournalBerthOperational = "dc8_berth_operational";
        public const string JournalDiveLaunched = "dc8_dive_launched";

        // Structural / hazard thresholds.
        public const float BerthRepairMinIntegrity = 60f;
        public const float SalvageImmediateIntegrityHit = 25f;
        public const float SalvageImmediateContaminationBoost = 0.40f;
        public const float FleetLevyFraction = 0.25f; // Fleet takes 25% of dock salvage

        private District8DeepCoastState _state = new District8DeepCoastState();
        private readonly List<DeepCoastRouteNode> _route = new List<DeepCoastRouteNode>();
        private readonly Dictionary<string, DeepCoastRouteNode> _byId = new Dictionary<string, DeepCoastRouteNode>(StringComparer.Ordinal);

        public event Action<DeepCoastStage> OnStageAdvanced;
        public event Action<DeepCoastAccessDecision> OnDecisionMade;
        public event Action<string, string, int> OnSalvageRolled;      // locationId, itemId, qty
        public event Action<string> OnNarrativeMarker;                 // marker/journal key
        public event Action OnStateChanged;

        public District8DeepCoastSystem()
        {
            BuildRoute();
        }

        public District8DeepCoastSystem(int seedSalt) : this()
        {
            _seedSalt = seedSalt;
        }

        private int _seedSalt = 808;

        public District8DeepCoastState State => _state;
        public DeepCoastStage Stage => (DeepCoastStage)_state.stage;
        public DeepCoastAccessDecision AccessDecision => (DeepCoastAccessDecision)_state.accessDecision;
        public bool IsFleetLevyActive => _state.fleetLevyActive;
        public bool IsOfficeAccessLimited => _state.officeAccessLimited;
        /// <summary>True after the fleet-controlled decision: the Fleet stands up and comes ashore.</summary>
        public bool IsFleetStoodUp => _state.fleetStoodUp;
        public float StructuralIntegrity => _state.structuralIntegrity;
        public float ContaminationLevel => _state.contaminationLevel;
        public bool IsDockOperationActive => !string.IsNullOrEmpty(_state.activeDockOperationId);
        public IReadOnlyList<DeepCoastRouteNode> Route => _route;

        /// <summary>
        /// Route spine in travel order. Uses only existing Shelf/Roadstead
        /// anchors plus the three new geographic nodes; never duplicates the
        /// dock (loc_maritime_icebreaker_dock) or the crashed convoy.
        /// </summary>
        private void BuildRoute()
        {
            AddNode(RouteStartId, "Foghorn 8", segmentHours: 0f, total: 10.5f, danger: 6f, rads: 32f);
            AddNode(PerimeterBreakwaterId, "The Perimeter Breakwater", segmentHours: 2.0f, total: 12.5f, danger: 8f, rads: 42f);
            AddNode(ServiceChannelId, "The Flooded Service Channel", segmentHours: 1.5f, total: 14.0f, danger: 8f, rads: 50f);
            AddNode(DeepBerthId, "The Deep Berth", segmentHours: 1.0f, total: 15.0f, danger: 9f, rads: 46f);
            AddNode(DockId, "Northern Sound Icebreaker Dock", segmentHours: 0.5f, total: 15.5f, danger: 9f, rads: 58f);
        }

        private void AddNode(string id, string name, float segmentHours, float total, float danger, float rads)
        {
            var node = new DeepCoastRouteNode
            {
                id = id,
                displayName = name,
                segmentHours = segmentHours,
                travelHours = total,
                dangerLevel = danger,
                baseRadsPerHour = rads
            };
            _route.Add(node);
            _byId[id] = node;
        }

        // ── Stage gating ──────────────────────────────────────────────

        /// <summary>
        /// Geographic gating for the deep-coast nodes. The Shelf approach is
        /// always traversable (the ice road owns seasonal gating). The
        /// breakwater is reachable even while the yard is sealed — the boom is
        /// the gate, and the survey trip is the first expedition that reads it.
        /// Everything beyond it unlocks with its stage.
        /// </summary>
        public bool IsNodeAccessible(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            if (nodeId == RouteStartId) return true;
            if (nodeId == PerimeterBreakwaterId) return true; // reachable to survey
            if (nodeId == ServiceChannelId) return _state.AtLeast(DeepCoastStage.PerimeterOpen);
            if (nodeId == DeepBerthId) return _state.AtLeast(DeepCoastStage.DockAccessible);
            if (nodeId == DockId) return _state.AtLeast(DeepCoastStage.DockAccessible);
            return false;
        }

        /// <summary>True when the node is on the deep-coast spine (route gating applies).</summary>
        public bool IsDeepCoastNode(string nodeId) =>
            !string.IsNullOrEmpty(nodeId) && _byId.ContainsKey(nodeId);

        public bool CanStartDockOperation =>
            _state.AtLeast(DeepCoastStage.DeepBerthOperational) && !IsDockOperationActive;

        public float TravelHours(string nodeId)
        {
            return _byId.TryGetValue(nodeId, out var n) ? n.travelHours : 10.5f;
        }

        public float DangerLevel(string nodeId)
        {
            if (!_byId.TryGetValue(nodeId, out var n)) return 7f;
            // Contamination raises the hazard on the flooded segments.
            if (_state.contaminationLevel > 0.6f && nodeId != RouteStartId)
                return Math.Min(10f, n.dangerLevel + 1f);
            return n.dangerLevel;
        }

        public float RadsPerHour(string nodeId)
        {
            if (!_byId.TryGetValue(nodeId, out var n)) return 32f;
            return n.baseRadsPerHour * (1f + _state.contaminationLevel * 0.25f);
        }

        // ── Stage machine ─────────────────────────────────────────────

        /// <summary>
        /// Sealed → Surveyed. One-time. The survey trip is an ordinary
        /// expedition to the breakwater; this call records the outcome.
        /// </summary>
        public bool SurveyPerimeter(int day)
        {
            if (_state.perimeterSurveyed) return false;
            if (_state.stage != (int)DeepCoastStage.Sealed) return false;
            _state.perimeterSurveyed = true;
            SetStage(DeepCoastStage.Surveyed);
            MarkNarrative(JournalSurvey);
            _state.lastTickDay = day;
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Records the meaningful reopening decision. Immediate consequences
        /// (faction standing deltas, salvage roll, structural damage) are
        /// returned to the host; delayed consequences (integrity repair bill,
        /// fleet levy, contamination persistence) are owned here.
        /// Returns the immediate trust deltas applied, or null when invalid.
        /// </summary>
        public DeepCoastDecisionOutcome? MakeReopeningDecision(DeepCoastAccessDecision decision, int day, ISeededRng rng)
        {
            if (_state.accessDecision != (int)DeepCoastAccessDecision.None) return null;
            if (_state.stage < (int)DeepCoastStage.Surveyed) return null;
            if (decision == DeepCoastAccessDecision.None) return null;
            if (rng == null) return null;

            _state.accessDecision = (int)decision;
            _state.decisionDay = day;

            var outcome = new DeepCoastDecisionOutcome { Decision = decision };
            switch (decision)
            {
                case DeepCoastAccessDecision.StabilizeRepair:
                    outcome.OfficeTrustDelta = +5f;
                    outcome.NarrativeKey = JournalStabilize;
                    break;

                case DeepCoastAccessDecision.SalvageImmediate:
                    // Fast, risky: immediate salvage but a structural and
                    // contamination bill that must be paid before the berth works.
                    outcome.OfficeTrustDelta = -3f;
                    outcome.NarrativeKey = JournalSalvage;
                    _state.structuralIntegrity = Math.Max(0f, _state.structuralIntegrity - SalvageImmediateIntegrityHit);
                    _state.contaminationLevel = Math.Min(1f, _state.contaminationLevel + SalvageImmediateContaminationBoost);
                    RollImmediateSalvage(rng, outcome);
                    break;

                case DeepCoastAccessDecision.FleetControlled:
                    // The Fleet does the heavy work; the levy is the price.
                    outcome.FleetTrustDelta = +12f;
                    outcome.OfficeTrustDelta = -5f;
                    outcome.NarrativeKey = JournalFleet;
                    _state.fleetLevyActive = true;
                    _state.fleetStoodUp = true;
                    break;

                case DeepCoastAccessDecision.MunicipalControlled:
                    outcome.OfficeTrustDelta = +5f;
                    outcome.NarrativeKey = JournalMunicipal;
                    _state.officeAccessLimited = true;
                    break;
            }

            MarkNarrative(outcome.NarrativeKey);
            OnDecisionMade?.Invoke(decision);
            RaiseChanged();
            return outcome;
        }

        /// <summary>
        /// Surveyed → PerimeterOpen. Consumes the material bill through the
        /// host-provided atomic check/consume lambda (the canonical inventory
        /// authority). Fleet-controlled access clears the perimeter for free.
        /// </summary>
        public bool TryClearPerimeter(int day, Func<string, int, bool> tryConsume)
        {
            if (_state.perimeterCleared) return false;
            if (_state.stage < (int)DeepCoastStage.Surveyed) return false;
            if (_state.accessDecision == (int)DeepCoastAccessDecision.None) return false;

            if (_state.accessDecision != (int)DeepCoastAccessDecision.FleetControlled)
            {
                var bill = PerimeterClearBill();
                if (tryConsume == null || !ConsumeBill(bill, tryConsume)) return false;
            }

            _state.perimeterCleared = true;
            SetStage(DeepCoastStage.PerimeterOpen);
            _state.lastTickDay = day;
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// PerimeterOpen → DockAccessible. Requires the service channel
        /// cleared (materials) unless the Fleet already cut it.
        /// </summary>
        public bool TryClearServiceChannel(int day, Func<string, int, bool> tryConsume)
        {
            if (_state.channelCleared) return false;
            if (_state.stage < (int)DeepCoastStage.PerimeterOpen) return false;

            if (!_state.fleetLevyActive)
            {
                var bill = ChannelClearBill();
                if (tryConsume == null || !ConsumeBill(bill, tryConsume)) return false;
            }

            _state.channelCleared = true;
            SetStage(DeepCoastStage.DockAccessible);
            MarkNarrative(JournalDockOpen);
            _state.lastTickDay = day;
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// DockAccessible → DeepBerthOperational. The berth at the icebreaker
        /// dock needs the structural bill paid; a damaged perimeter
        /// (salvage-immediate) forces a heavier scrap-only shoring bill first.
        /// </summary>
        public bool TryRepairDeepBerth(int day, Func<string, int, bool> tryConsume)
        {
            if (_state.berthRepaired) return false;
            if (_state.stage < (int)DeepCoastStage.DockAccessible) return false;

            if (!_state.fleetLevyActive)
            {
                var bill = BerthRepairBill();
                if (tryConsume == null || !ConsumeBill(bill, tryConsume)) return false;
            }

            _state.berthRepaired = true;
            _state.structuralIntegrity = 100f;
            SetStage(DeepCoastStage.DeepBerthOperational);
            MarkNarrative(JournalBerthOperational);
            _state.lastTickDay = day;
            RaiseChanged();
            return true;
        }

        // ── Dock operation (expedition → dive handoff) ───────────────

        /// <summary>
        /// Starts a dock dive operation. Only legal when the berth is
        /// operational. The dive instance itself lives in the maritime host
        /// (MaritimeSaveStore); this system only owns the authoritative
        /// reference and the gate, so reload cannot resurrect a dive that
        /// never started.
        /// </summary>
        public bool TryStartDockOperation(string operationId, string diverId, int day)
        {
            if (!CanStartDockOperation) return false;
            if (string.IsNullOrEmpty(operationId)) return false;

            _state.activeDockOperationId = operationId;
            _state.activeDockOperationLocationId = DockId;
            _state.dockOperationDiverId = diverId;
            _state.dockOperationStartedDay = day;
            if (!_state.narrativeMarkers.Contains(JournalDiveLaunched))
                MarkNarrative(JournalDiveLaunched);
            RaiseChanged();
            return true;
        }

        /// <summary>Ends the dock operation; returns whether the Fleet levy applies to rewards.</summary>
        public bool TryEndDockOperation(bool success, out float levyFraction)
        {
            levyFraction = 0f;
            if (!IsDockOperationActive) return false;
            if (success && _state.fleetLevyActive)
                levyFraction = FleetLevyFraction;
            _state.activeDockOperationId = string.Empty;
            _state.activeDockOperationLocationId = string.Empty;
            _state.dockOperationDiverId = string.Empty;
            _state.dockOperationStartedDay = -1;
            RaiseChanged();
            return true;
        }

        public string ActiveDockOperationId => _state.activeDockOperationId;
        public string ActiveDockOperationDiverId => _state.dockOperationDiverId;

        // ── Material bills (canonical ids; visible to UI) ─────────────

        /// <summary>itemId → qty required for the current next step (empty when free/none).</summary>
        public Dictionary<string, int> NextStepBill()
        {
            var bill = new Dictionary<string, int>(StringComparer.Ordinal);
            if (_state.fleetLevyActive) return bill; // the Fleet pays
            switch (Stage)
            {
                case DeepCoastStage.Surveyed:
                    return PerimeterClearBill();
                case DeepCoastStage.PerimeterOpen:
                    return ChannelClearBill();
                case DeepCoastStage.DockAccessible:
                    return BerthRepairBill();
                default:
                    return bill;
            }
        }

        public Dictionary<string, int> PerimeterClearBill()
        {
            var bill = new Dictionary<string, int>(StringComparer.Ordinal);
            switch ((DeepCoastAccessDecision)_state.accessDecision)
            {
                case DeepCoastAccessDecision.SalvageImmediate:
                    bill[ItemScrapMetal] = 1; // fast cut, high risk
                    break;
                case DeepCoastAccessDecision.MunicipalControlled:
                    bill[ItemScrapMetal] = 4; // full engineered bill
                    break;
                default:
                    bill[ItemScrapMetal] = 3;
                    bill[ItemBrassFittings] = 1;
                    break;
            }
            return bill;
        }

        public Dictionary<string, int> ChannelClearBill()
        {
            var bill = new Dictionary<string, int>(StringComparer.Ordinal);
            bill[ItemFuel] = 2;
            bill[ItemScrapMetal] = 1;
            return bill;
        }

        public Dictionary<string, int> BerthRepairBill()
        {
            var bill = new Dictionary<string, int>(StringComparer.Ordinal);
            if (_state.structuralIntegrity < BerthRepairMinIntegrity)
            {
                // Shored structure first: the salvage-immediate bill, in scrap.
                bill[ItemScrapMetal] = 6;
            }
            else
            {
                bill[ItemBrassFittings] = 3;
                bill[ItemScrapMetal] = 2;
            }
            return bill;
        }

        // ── Daily degradation (one tick per calendar day) ─────────────

        /// <summary>
        /// Idempotent daily tick: contamination decays slowly; an unrepaired
        /// structural breach slowly worsens. Guarded by lastTickDay so a host
        /// that ticks twice on one day cannot double-apply.
        /// </summary>
        public void TickDaily(int day, WeatherKind weather)
        {
            if (_state.lastTickDay == day) return;
            _state.lastTickDay = day;

            // Brine contamination recedes when the road is worked; a damaged
            // perimeter leaks and re-contaminates until repaired.
            float decay = weather == WeatherKind.Rain || weather == WeatherKind.FalseSpring ? 0.05f : 0.03f;
            _state.contaminationLevel = Math.Max(0f, _state.contaminationLevel - decay);

            if (_state.perimeterCleared && _state.structuralIntegrity < BerthRepairMinIntegrity && !_state.berthRepaired)
                _state.contaminationLevel = Math.Min(1f, _state.contaminationLevel + 0.02f);

            RaiseChanged();
        }

        // ── Helpers ───────────────────────────────────────────────────

        private void RollImmediateSalvage(ISeededRng rng, DeepCoastDecisionOutcome outcome)
        {
            // Deterministic per seed: salvage scrap + brass, sometimes resin.
            int scrap = 2 + rng.Next(0, 4);
            int brass = rng.Next(0, 3);
            outcome.Salvage.Add(new SalvageEntry(ItemScrapMetal, scrap));
            OnSalvageRolled?.Invoke(PerimeterBreakwaterId, ItemScrapMetal, scrap);
            if (brass > 0)
            {
                outcome.Salvage.Add(new SalvageEntry(ItemBrassFittings, brass));
                OnSalvageRolled?.Invoke(PerimeterBreakwaterId, ItemBrassFittings, brass);
            }
            if (rng.NextDouble() < 0.35)
            {
                outcome.Salvage.Add(new SalvageEntry(ItemRoResin, 1));
                OnSalvageRolled?.Invoke(PerimeterBreakwaterId, ItemRoResin, 1);
            }
        }

        private static bool ConsumeBill(Dictionary<string, int> bill, Func<string, int, bool> tryConsume)
        {
            if (bill == null || bill.Count == 0) return true;
            if (tryConsume == null) return false;

            // Atomic: verify the full bill first, then consume.
            foreach (var kv in bill)
            {
                if (!tryConsume(kv.Key, kv.Value)) return false;
            }
            return true;
        }

        private void SetStage(DeepCoastStage next)
        {
            if (_state.stage == (int)next) return;
            _state.stage = (int)next;
            OnStageAdvanced?.Invoke(next);
        }

        private void MarkNarrative(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (_state.narrativeMarkers.Contains(key)) return;
            _state.narrativeMarkers.Add(key);
            OnNarrativeMarker?.Invoke(key);
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();

        // ── Save / Load ───────────────────────────────────────────────

        public District8DeepCoastState CaptureState()
        {
            var copy = new District8DeepCoastState();
            CopyState(_state, copy);
            return copy;
        }

        public void RestoreState(District8DeepCoastState saved)
        {
            var fresh = new District8DeepCoastState();
            if (saved != null) CopyState(saved, fresh);
            // Missing-state defaults: any null/empty system id is the sealed route.
            if (string.IsNullOrEmpty(fresh.systemId)) fresh.systemId = SystemId;
            if (string.IsNullOrEmpty(fresh.expansionKey)) fresh.expansionKey = ExpansionKey;
            if (fresh.stage < (int)DeepCoastStage.Sealed || fresh.stage > (int)DeepCoastStage.DeepBerthOperational)
                fresh.stage = (int)DeepCoastStage.Sealed;
            if (fresh.accessDecision < (int)DeepCoastAccessDecision.None || fresh.accessDecision > (int)DeepCoastAccessDecision.MunicipalControlled)
                fresh.accessDecision = (int)DeepCoastAccessDecision.None;
            fresh.structuralIntegrity = Math.Clamp(fresh.structuralIntegrity, 0f, 100f);
            fresh.contaminationLevel = Math.Clamp(fresh.contaminationLevel, 0f, 1f);
            if (fresh.narrativeMarkers == null) fresh.narrativeMarkers = new List<string>();
            _state = fresh;
            RaiseChanged();
        }

        private static void CopyState(District8DeepCoastState from, District8DeepCoastState to)
        {
            to.systemId = from.systemId;
            to.expansionKey = from.expansionKey;
            to.stage = from.stage;
            to.accessDecision = from.accessDecision;
            to.decisionDay = from.decisionDay;
            to.perimeterSurveyed = from.perimeterSurveyed;
            to.perimeterCleared = from.perimeterCleared;
            to.channelCleared = from.channelCleared;
            to.berthRepaired = from.berthRepaired;
            to.structuralIntegrity = from.structuralIntegrity;
            to.contaminationLevel = from.contaminationLevel;
            to.fleetLevyActive = from.fleetLevyActive;
            to.officeAccessLimited = from.officeAccessLimited;
            to.fleetStoodUp = from.fleetStoodUp;
            to.activeDockOperationId = from.activeDockOperationId;
            to.activeDockOperationLocationId = from.activeDockOperationLocationId;
            to.dockOperationDiverId = from.dockOperationDiverId;
            to.dockOperationStartedDay = from.dockOperationStartedDay;
            to.lastTickDay = from.lastTickDay;
            to.narrativeMarkers = new List<string>();
            if (from.narrativeMarkers != null)
                for (int i = 0; i < from.narrativeMarkers.Count; i++)
                    to.narrativeMarkers.Add(from.narrativeMarkers[i]);
        }
    }

    /// <summary>
    /// Result of the reopening decision: immediate trust deltas and any
    /// immediate salvage from the risky path. Delayed consequences are read
    /// from system state (integrity, contamination, levy, access limit).
    /// </summary>
    public sealed class DeepCoastDecisionOutcome
    {
        public DeepCoastAccessDecision Decision;
        public float FleetTrustDelta;
        public float OfficeTrustDelta;
        public string NarrativeKey = string.Empty;
        public List<SalvageEntry> Salvage = new List<SalvageEntry>();
    }

    [Serializable]
    public sealed class SalvageEntry
    {
        public string ItemId = string.Empty;
        public int Quantity;

        public SalvageEntry() { }
        public SalvageEntry(string itemId, int qty) { ItemId = itemId; Quantity = qty; }
    }
}
