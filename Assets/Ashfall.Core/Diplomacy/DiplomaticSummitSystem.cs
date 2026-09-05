using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Diplomacy
{
    // ---------------------------------------------------------------------
    // Ports (host binds to the live faction / skills authorities)
    // ---------------------------------------------------------------------

    /// <summary>
    /// Faction standing authority port. The diplomacy system NEVER keeps its
    /// own trust totals — standing deltas route through this port to the
    /// canonical faction authority (plan Risk 5).
    /// </summary>
    public interface IFactionStandingPort
    {
        float GetStanding(string factionId);
        void AdjustStanding(string factionId, float delta, string reasonCode);
    }

    /// <summary>Read-only faction facts the summit logic needs (tags, hostility).</summary>
    public interface IFactionContextPort
    {
        /// <summary>Eligibility tag for a faction id (e.g. "militia"); null when unknown.</summary>
        string? GetFactionTag(string factionId);

        /// <summary>True when the faction is currently hostile to the shelter.</summary>
        bool IsHostile(string factionId);
    }

    /// <summary>Survivor skill lookup port (canonical skill ids, e.g. skill_cold_analysis).</summary>
    public interface ISurvivorSkillsPort
    {
        bool HasSkill(string survivorId, string skillId);
    }

    // ---------------------------------------------------------------------
    // Persisted state
    // ---------------------------------------------------------------------

    [Serializable]
    public sealed class DiplomaticSummitState
    {
        public string summit_id = string.Empty;
        public string location_id = string.Empty;
        public string framework_id = string.Empty;
        public int convening_day = -1;
        public List<string> attending_faction_ids = new();
        public List<string> delegate_survivor_ids = new();
        public int agenda_index;                          // next agenda_clauses entry to negotiate
        public int security_tension;                      // 0..100, collapse at 100
        public int negotiation_stability;                 // 0..100, ratifiable at RatificationThreshold
        public int negotiation_round;
        public string status = "scheduled";               // scheduled | negotiating | ratified | collapsed
        public string ratified_treaty_id = string.Empty;
    }

    [Serializable]
    public sealed class ActiveTreatyState
    {
        public string treaty_id = string.Empty;
        public string framework_id = string.Empty;
        public List<string> signatory_faction_ids = new();
        public int start_day = -1;
        public int expiry_day = -1;
        public int stability = 50;                        // 0..100, decays on violations
        public int violation_count;
        public List<string> dmz_zone_ids = new();
        public string status = "active";                  // active | expired | collapsed
        public int violation_penalty_standing;
    }

    [Serializable]
    public sealed class GuaranteeState
    {
        public string guarantee_id = string.Empty;
        public string treaty_id = string.Empty;
        public string survivor_id = string.Empty;
        public string holding_faction_id = string.Empty;
        public int start_day = -1;
        public int release_day = -1;                      // -1 while exchanged
        public string status = "exchanged";               // exchanged | released | forfeited
    }

    [Serializable]
    public sealed class TreatyViolationRecord
    {
        public string violation_id = string.Empty;
        public string treaty_id = string.Empty;
        public string faction_id = string.Empty;
        public int day = -1;
        public string kind = string.Empty;                // dmz_armed_patrol | raid_against_signatory | withheld_share
        public int severity = 1;                          // 1..3
    }

    [Serializable]
    public sealed class DiplomaticSummitSave
    {
        public int schema_version = 1;
        public List<DiplomaticSummitState> summits = new();
        public List<ActiveTreatyState> treaties = new();
        public List<GuaranteeState> guarantees = new();
        public List<TreatyViolationRecord> violations = new();
        public int next_summit_ordinal;
        public int next_treaty_ordinal;
        public int next_guarantee_ordinal;
        public int next_violation_ordinal;
    }

    // ---------------------------------------------------------------------
    // System
    // ---------------------------------------------------------------------

    /// <summary>
    /// Wasteland diplomatic summits & regional non-aggression treaties
    /// (flagship Task 6). Orchestrates summits, treaty lifecycle, guarantees
    /// and DMZ rules; faction standing, war and territory stay owned by the
    /// canonical authorities (accessed through ports).
    ///
    /// Determinism: negotiation uses KEYED RNG STREAMS — a fresh
    /// <see cref="SeededRng"/> per (masterSeed, summit, round) derived by
    /// stable hash, so no RNG continuation state is persisted and
    /// save/restore cannot shift future outcomes (plan §2.4).
    /// </summary>
    public sealed class DiplomaticSummitSystem
    {
        public const string SystemId = "diplomatic_summits";
        public const string InstitutionId = "institution_diplomacy";

        /// <summary>
        /// Authored neutral summit site. The plan's suggested
        /// loc_waystation_crossing does not exist in locations.json; this is
        /// the shipped neutral site (documented divergence).
        /// </summary>
        public const string NeutralSummitSiteId = "loc_neutral_ground";

        public const int RatificationThreshold = 70;
        public const int CollapseTension = 100;
        public const int ConcessionStabilityBonus = 15;
        public const int GuaranteeReleaseDays = 14;
        public const int MaxNegotiationRounds = 12;

        private readonly Inventory.Inventory? _inventory;
        private readonly ILog _log;
        private readonly int _masterSeed;
        private readonly IInstitutionAvailability? _availability;
        private readonly IFactionStandingPort? _standing;
        private readonly IFactionContextPort? _factions;
        private readonly ISurvivorSkillsPort? _skills;

        private readonly Dictionary<string, DiplomaticTreatyDefinition> _frameworks = new(StringComparer.Ordinal);
        private DiplomaticSummitSave _state = new();

        public DiplomaticSummitSystem(
            int masterSeed,
            Inventory.Inventory? inventory = null,
            ILog? log = null,
            IInstitutionAvailability? availability = null,
            IFactionStandingPort? standing = null,
            IFactionContextPort? factions = null,
            ISurvivorSkillsPort? skills = null)
        {
            _masterSeed = masterSeed;
            _inventory = inventory;
            _log = log ?? new ConsoleLog();
            _availability = availability;
            _standing = standing;
            _factions = factions;
            _skills = skills;
        }

        // -----------------------------------------------------------------
        // Events
        // -----------------------------------------------------------------

        public event Action<DiplomaticSummitState>? OnSummitScheduled;
        public event Action<ActiveTreatyState>? OnTreatyRatified;
        public event Action<TreatyViolationRecord>? OnTreatyViolationRecorded;
        public event Action<ActiveTreatyState, string>? OnTreatyEnded;      // treaty, reason (expired|collapsed)
        public event Action<GuaranteeState>? OnGuaranteeExchanged;
        public event Action<GuaranteeState>? OnGuaranteeReleased;

        // -----------------------------------------------------------------
        // Catalog + queries
        // -----------------------------------------------------------------

        public void LoadTreatyCatalog(List<DiplomaticTreatyDefinition> frameworks)
        {
            if (frameworks == null) return;
            _frameworks.Clear();
            foreach (var t in frameworks)
                if (!string.IsNullOrEmpty(t.treaty_id))
                    _frameworks[t.treaty_id] = t;
        }

        public IReadOnlyList<DiplomaticSummitState> Summits => _state.summits.AsReadOnly();
        public IReadOnlyList<ActiveTreatyState> Treaties => _state.treaties.AsReadOnly();
        public IReadOnlyList<GuaranteeState> Guarantees => _state.guarantees.AsReadOnly();
        public IReadOnlyList<TreatyViolationRecord> Violations => _state.violations.AsReadOnly();

        public DiplomaticSummitState? GetSummit(string summitId) =>
            _state.summits.FirstOrDefault(s => s.summit_id == summitId);
        public ActiveTreatyState? GetTreaty(string treatyId) =>
            _state.treaties.FirstOrDefault(t => t.treaty_id == treatyId);

        /// <summary>DMZ query consumed by patrol/raid systems. Armed presence in the zone is prohibited.</summary>
        public bool IsArmedPatrolAllowed(string factionId, string zoneId)
        {
            foreach (var treaty in _state.treaties)
            {
                if (treaty.status != "active") continue;
                if (!treaty.signatory_faction_ids.Contains(factionId)) continue;
                if (treaty.dmz_zone_ids.Contains(zoneId)) return false;
            }
            return true;
        }

        /// <summary>True when a survivor is held as an active diplomatic guarantee.</summary>
        public bool IsGuaranteeHeld(string survivorId) =>
            _state.guarantees.Any(g => g.survivor_id == survivorId && g.status == "exchanged");

        // -----------------------------------------------------------------
        // Summit scheduling + negotiation
        // -----------------------------------------------------------------

        public ActionResult TryScheduleSummit(
            string locationId, IReadOnlyList<string> factionIds, IReadOnlyList<string> delegateIds,
            string frameworkId, int day)
        {
            if (locationId != NeutralSummitSiteId)
                return ActionResult.Blocked("site_not_neutral", "diplomacy.site_not_neutral");
            if (!_frameworks.TryGetValue(frameworkId, out var framework))
                return ActionResult.Blocked("unknown_framework", "diplomacy.unknown_framework");
            if (factionIds.Count < framework.minimum_signatories)
                return ActionResult.Blocked("too_few_factions", "diplomacy.too_few_factions");
            if (delegateIds.Count == 0)
                return ActionResult.Blocked("no_delegates", "diplomacy.no_delegates");

            foreach (var factionId in factionIds)
            {
                string? tag = _factions?.GetFactionTag(factionId);
                if (_factions != null && tag == null)
                    return ActionResult.Blocked("unknown_faction", "diplomacy.unknown_faction");
                if (_factions != null && !framework.EligibleForTag(tag!))
                    return ActionResult.Blocked("faction_ineligible", "diplomacy.faction_ineligible");
                if (_factions != null && _factions.IsHostile(factionId))
                    return ActionResult.Blocked("faction_hostile", "diplomacy.faction_hostile");
            }

            foreach (var delegateId in delegateIds)
            {
                if (_availability != null && !_availability.TryClaim(delegateId, InstitutionId, "delegate"))
                    return ActionResult.Blocked("delegate_unavailable", "diplomacy.delegate_unavailable");
            }

            int ordinal = _state.next_summit_ordinal++;
            var summit = new DiplomaticSummitState
            {
                summit_id = $"summit_{day}_{ordinal}",
                location_id = locationId,
                framework_id = frameworkId,
                convening_day = day,
                attending_faction_ids = factionIds.ToList(),
                delegate_survivor_ids = delegateIds.ToList(),
                status = "negotiating",
                security_tension = 10,
                negotiation_stability = 35,
            };
            _state.summits.Add(summit);
            _log.Info($"[Diplomacy] summit '{summit.summit_id}' convened at {locationId} ({frameworkId})");
            OnSummitScheduled?.Invoke(summit);
            return ActionResult.Success("diplomacy.summit_scheduled",
                new Dictionary<string, double> { { "summit", ordinal } });
        }

        /// <summary>
        /// Resolves one negotiation round over the summit's next agenda clause.
        /// Deterministic given (masterSeed, summit, round). Offering a
        /// concession from the framework's list adds authored stability.
        /// </summary>
        public ActionResult AdvanceNegotiation(string summitId, bool offerConcession)
        {
            var summit = GetSummit(summitId);
            if (summit == null)
                return ActionResult.Blocked("unknown_summit", "diplomacy.unknown_summit");
            if (summit.status != "negotiating")
                return ActionResult.Blocked("not_negotiating", "diplomacy.not_negotiating");
            if (!_frameworks.TryGetValue(summit.framework_id, out var framework))
                return ActionResult.Blocked("unknown_framework", "diplomacy.unknown_framework");

            if (summit.negotiation_round >= MaxNegotiationRounds)
            {
                summit.status = "collapsed";
                ReleaseDelegates(summit);
                OnTreatyEnded?.Invoke(new ActiveTreatyState { framework_id = summit.framework_id, status = "collapsed" }, "agenda_exhausted");
                return ActionResult.Blocked("agenda_exhausted", "diplomacy.agenda_exhausted");
            }

            int delegateSkillBonus = 0;
            foreach (var d in summit.delegate_survivor_ids)
            {
                if (_skills?.HasSkill(d, "skill_cold_analysis") == true) delegateSkillBonus += 8;
                else if (_skills?.HasSkill(d, "skill_watchful") == true) delegateSkillBonus += 5;
            }

            int acceptance = 50
                + summit.negotiation_stability / 4
                - summit.security_tension / 5
                + delegateSkillBonus
                + (offerConcession ? ConcessionStabilityBonus : 0);
            acceptance = Math.Clamp(acceptance, 5, 95);

            var rng = StreamFor(summit.summit_id, summit.negotiation_round);
            int roll = rng.Next(0, 100);
            summit.negotiation_round++;
            var clauses = framework.agenda_clauses ?? new List<string>();
            if (clauses.Count > 0)
                summit.agenda_index = (summit.agenda_index + 1) % clauses.Count;

            if (roll < acceptance)
            {
                summit.negotiation_stability = Math.Min(100, summit.negotiation_stability + 12 + rng.Next(0, 7));
                summit.security_tension = Math.Max(0, summit.security_tension - 5);
            }
            else
            {
                summit.negotiation_stability = Math.Max(0, summit.negotiation_stability - 10);
                summit.security_tension = Math.Min(CollapseTension, summit.security_tension + 12);
                if (summit.security_tension >= CollapseTension)
                {
                    summit.status = "collapsed";
                    ReleaseDelegates(summit);
                    _log.Info($"[Diplomacy] summit '{summitId}' collapsed in round {summit.negotiation_round}");
                    OnTreatyEnded?.Invoke(new ActiveTreatyState { framework_id = summit.framework_id, status = "collapsed" }, "summit_collapsed");
                    return ActionResult.Success("diplomacy.summit_collapsed");
                }
            }

            return ActionResult.Success("diplomacy.round_resolved",
                new Dictionary<string, double>
                {
                    { "stability", summit.negotiation_stability },
                    { "tension", summit.security_tension },
                    { "roll", roll },
                });
        }

        /// <summary>Ratifies the summit's framework into an active treaty. Atomic: concessions commit or nothing does.</summary>
        public ActionResult TryRatifyTreaty(string summitId, int day)
        {
            var summit = GetSummit(summitId);
            if (summit == null)
                return ActionResult.Blocked("unknown_summit", "diplomacy.unknown_summit");
            if (summit.status != "negotiating")
                return ActionResult.Blocked("not_negotiating", "diplomacy.not_negotiating");
            if (summit.negotiation_stability < RatificationThreshold)
                return ActionResult.Blocked("insufficient_stability", "diplomacy.insufficient_stability");
            if (!_frameworks.TryGetValue(summit.framework_id, out var framework))
                return ActionResult.Blocked("unknown_framework", "diplomacy.unknown_framework");
            if (_inventory == null && (framework.required_concessions?.Count ?? 0) > 0)
                return ActionResult.Blocked("no_inventory", "diplomacy.no_inventory");

            int treatyOrdinal = _state.next_treaty_ordinal++;
            var treaty = new ActiveTreatyState
            {
                treaty_id = $"treaty_active_{day}_{treatyOrdinal}",
                framework_id = framework.treaty_id,
                signatory_faction_ids = summit.attending_faction_ids.ToList(),
                start_day = day,
                expiry_day = day + framework.duration_days,
                stability = framework.stability_rating,
                dmz_zone_ids = framework.dmz_zone_ids?.ToList() ?? new List<string>(),
                violation_penalty_standing = framework.violation_penalty_standing,
            };

            // Atomic concession payment.
            if (_inventory != null && framework.required_concessions is { Count: > 0 })
            {
                var bill = new InventoryBill();
                foreach (var c in framework.required_concessions)
                    if (c.concession_kind == "goods")
                        bill.AddCost(c.item_id, c.amount);
                if (bill.GetAggregatedCosts().Count > 0 && !_inventory.TryExecuteTransaction(bill))
                    return ActionResult.Blocked("missing_concessions", "diplomacy.missing_concessions");
            }

            treaty.status = "active";
            _state.treaties.Add(treaty);
            summit.status = "ratified";
            summit.ratified_treaty_id = treaty.treaty_id;
            ReleaseDelegates(summit);

            _log.Info($"[Diplomacy] treaty '{treaty.treaty_id}' ratified ({framework.treaty_id}), expires day {treaty.expiry_day}");
            OnTreatyRatified?.Invoke(treaty);
            return ActionResult.Success("diplomacy.treaty_ratified",
                new Dictionary<string, double> { { "expiry_day", treaty.expiry_day } });
        }

        // -----------------------------------------------------------------
        // Guarantees
        // -----------------------------------------------------------------

        /// <summary>
        /// Exchanges a survivor as a treaty guarantee. The survivor stays on
        /// the roster; only their availability is claimed (plan §6.11).
        /// </summary>
        public ActionResult TryExchangeGuarantee(string treatyId, string survivorId, string holdingFactionId, int day)
        {
            var treaty = GetTreaty(treatyId);
            if (treaty == null || treaty.status != "active")
                return ActionResult.Blocked("treaty_not_active", "diplomacy.treaty_not_active");
            if (!_frameworks.TryGetValue(treaty.framework_id, out var framework) || !framework.guarantee_allowed)
                return ActionResult.Blocked("guarantee_not_allowed", "diplomacy.guarantee_not_allowed");
            if (IsGuaranteeHeld(survivorId))
                return ActionResult.Blocked("already_held", "diplomacy.already_held");
            if (_availability != null && !_availability.TryClaim(survivorId, InstitutionId, "guarantee"))
                return ActionResult.Blocked("survivor_unavailable", "diplomacy.survivor_unavailable");

            int ordinal = _state.next_guarantee_ordinal++;
            var guarantee = new GuaranteeState
            {
                guarantee_id = $"guarantee_{day}_{ordinal}",
                treaty_id = treatyId,
                survivor_id = survivorId,
                holding_faction_id = holdingFactionId,
                start_day = day,
            };
            _state.guarantees.Add(guarantee);
            _log.Info($"[Diplomacy] guarantee '{guarantee.guarantee_id}': {survivorId} held by {holdingFactionId}");
            OnGuaranteeExchanged?.Invoke(guarantee);
            return ActionResult.Success("diplomacy.guarantee_exchanged");
        }

        /// <summary>Releases a held guarantee, restoring the survivor's availability.</summary>
        public ActionResult TryReleaseGuarantee(string guaranteeId, int day, bool forfeited = false)
        {
            var guarantee = _state.guarantees.FirstOrDefault(g => g.guarantee_id == guaranteeId);
            if (guarantee == null || guarantee.status != "exchanged")
                return ActionResult.Blocked("guarantee_not_held", "diplomacy.guarantee_not_held");

            guarantee.status = forfeited ? "forfeited" : "released";
            guarantee.release_day = day;
            _availability?.Release(guarantee.survivor_id, InstitutionId, "guarantee");
            _log.Info($"[Diplomacy] guarantee '{guaranteeId}' {guarantee.status} on day {day}");
            OnGuaranteeReleased?.Invoke(guarantee);

            if (forfeited)
                _standing?.AdjustStanding(guarantee.holding_faction_id, -10, "diplomacy.guarantee_forfeit");
            return ActionResult.Success("diplomacy.guarantee_released");
        }

        // -----------------------------------------------------------------
        // Violations
        // -----------------------------------------------------------------

        /// <summary>
        /// Consumes an observed armed patrol movement. Records at most one
        /// violation per (treaty, faction, zone, day) — event-driven, no world
        /// scanning (plan §6.16).
        /// </summary>
        public ActionResult ReportArmedPatrol(string factionId, string zoneId, int day)
        {
            if (IsArmedPatrolAllowed(factionId, zoneId))
                return ActionResult.Success("diplomacy.no_violation");

            var treaty = _state.treaties.First(t =>
                t.status == "active"
                && t.signatory_faction_ids.Contains(factionId)
                && t.dmz_zone_ids.Contains(zoneId));

            bool duplicate = _state.violations.Any(v =>
                v.treaty_id == treaty.treaty_id && v.faction_id == factionId
                && v.day == day && v.kind == "dmz_armed_patrol");
            if (duplicate)
                return ActionResult.Success("diplomacy.violation_already_recorded");

            int ordinal = _state.next_violation_ordinal++;
            var record = new TreatyViolationRecord
            {
                violation_id = $"violation_{day}_{ordinal}",
                treaty_id = treaty.treaty_id,
                faction_id = factionId,
                day = day,
                kind = "dmz_armed_patrol",
                severity = 1,
            };
            ApplyViolation(treaty, record);
            return ActionResult.Success("diplomacy.violation_recorded");
        }

        /// <summary>Records a raid committed by a signatory against another signatory.</summary>
        public ActionResult ReportRaidAgainstSignatory(string aggressorFactionId, int day)
        {
            var treaty = _state.treaties.FirstOrDefault(t =>
                t.status == "active" && t.signatory_faction_ids.Contains(aggressorFactionId));
            if (treaty == null)
                return ActionResult.Success("diplomacy.no_violation");

            bool duplicate = _state.violations.Any(v =>
                v.treaty_id == treaty.treaty_id && v.faction_id == aggressorFactionId
                && v.day == day && v.kind == "raid_against_signatory");
            if (duplicate)
                return ActionResult.Success("diplomacy.violation_already_recorded");

            int ordinal = _state.next_violation_ordinal++;
            var record = new TreatyViolationRecord
            {
                violation_id = $"violation_{day}_{ordinal}",
                treaty_id = treaty.treaty_id,
                faction_id = aggressorFactionId,
                day = day,
                kind = "raid_against_signatory",
                severity = 2,
            };
            ApplyViolation(treaty, record);
            return ActionResult.Success("diplomacy.violation_recorded");
        }

        private void ApplyViolation(ActiveTreatyState treaty, TreatyViolationRecord record)
        {
            _state.violations.Add(record);
            treaty.violation_count++;
            treaty.stability = Math.Max(0, treaty.stability - 20 * record.severity);
            _standing?.AdjustStanding(record.faction_id, treaty.violation_penalty_standing, $"diplomacy.{record.kind}");

            _log.Info($"[Diplomacy] violation '{record.violation_id}' ({record.kind}) on '{treaty.treaty_id}'");
            OnTreatyViolationRecorded?.Invoke(record);

            if (!_frameworks.TryGetValue(treaty.framework_id, out var framework))
                return;
            if (treaty.violation_count > framework.violation_tolerance)
                CollapseTreaty(treaty, day: record.day, reason: "violation_tolerance_exceeded");
        }

        private void CollapseTreaty(ActiveTreatyState treaty, int day, string reason)
        {
            treaty.status = "collapsed";
            foreach (var guarantee in _state.guarantees)
            {
                if (guarantee.treaty_id == treaty.treaty_id && guarantee.status == "exchanged")
                    TryReleaseGuarantee(guarantee.guarantee_id, day, forfeited: true);
            }
            _log.Info($"[Diplomacy] treaty '{treaty.treaty_id}' collapsed: {reason}");
            OnTreatyEnded?.Invoke(treaty, "collapsed");
        }

        // -----------------------------------------------------------------
        // Daily tick
        // -----------------------------------------------------------------

        private int _currentDay;

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var treaty in _state.treaties)
            {
                if (treaty.status != "active") continue;
                if (day >= treaty.expiry_day)
                {
                    treaty.status = "expired";
                    foreach (var guarantee in _state.guarantees)
                    {
                        if (guarantee.treaty_id == treaty.treaty_id && guarantee.status == "exchanged")
                            TryReleaseGuarantee(guarantee.guarantee_id, day);
                    }
                    _log.Info($"[Diplomacy] treaty '{treaty.treaty_id}' expired on day {day}");
                    OnTreatyEnded?.Invoke(treaty, "expired");
                }
            }

            // Authored slow decay: stability drifts down 1/day while active.
            foreach (var treaty in _state.treaties)
            {
                if (treaty.status == "active")
                    treaty.stability = Math.Max(0, treaty.stability - 1);
            }
        }

        private void ReleaseDelegates(DiplomaticSummitState summit)
        {
            foreach (var d in summit.delegate_survivor_ids)
                _availability?.Release(d, InstitutionId, "delegate");
        }

        // -----------------------------------------------------------------
        // Keyed RNG streams (no persisted continuation)
        // -----------------------------------------------------------------

        private SeededRng StreamFor(string summitId, int round)
        {
            ulong h = 1469598103934665603UL; // FNV-1a basis
            foreach (char c in summitId)
            {
                h ^= c;
                h *= 1099511628211UL;
            }
            h ^= (uint)round;
            h *= 1099511628211UL;
            h ^= (uint)_masterSeed;
            h *= 1099511628211UL;
            int seed = unchecked((int)(h ^ (h >> 32)));
            return new SeededRng(seed);
        }

        // -----------------------------------------------------------------
        // Save / restore
        // -----------------------------------------------------------------

        public DiplomaticSummitSave CaptureState() => Clone(_state);

        public void RestoreState(DiplomaticSummitSave? saved)
        {
            if (saved == null) return;
            _state = Clone(saved);
        }

        private static DiplomaticSummitSave Clone(DiplomaticSummitSave src)
        {
            var json = new SystemTextJsonSerializer();
            return json.Deserialize<DiplomaticSummitSave>(json.Serialize(src)) ?? new DiplomaticSummitSave();
        }
    }
}
