using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>
    /// Boolean trigger-condition grammar for FactionWarEventStage.triggerCondition,
    /// per NARRATIVE_NEEDS.md §2's explicit requirement ("Needs a real boolean
    /// grammar ... before this can drive anything other than a human reading
    /// the JSON"). The 45 stages in faction_war_events.json are authored prose
    /// ("Fires automatically once the player has visited X", "Fires N days
    /// after stage Y", etc.) that were deliberately kept close to what such a
    /// grammar would need to express. This is a closed set of condition node
    /// types evaluated by a runner, not a free-text parser — the source data
    /// is static, authored JSON, not user input, so a real parser would be
    /// over-engineering; each stage is annotated with its condition once,
    /// here, in a lookup keyed by stageId.
    /// </summary>
    public abstract class FactionWarTrigger
    {
        public abstract bool IsSatisfied(FactionWarTriggerContext ctx);
    }

    /// <summary>Everything a trigger needs to evaluate itself, supplied by the runner.</summary>
    public sealed class FactionWarTriggerContext
    {
        public int CurrentDay;
        public Func<string, bool> IsChainResolved = _ => false;
        public Func<string, bool> HasVisitedLocation = _ => false;
        public Func<string, int> StageResolvedDay = _ => -1; // -1 = not yet resolved

        /// <summary>Plan 25: campaign flag probe (runner-produced flags plus the
        /// host's external flag store). Defaults to never-set.</summary>
        public Func<string, bool> IsFlagSet = _ => false;
    }

    /// <summary>
    /// Plan 25: fires while the named campaign flag is set. Escalation chains
    /// gate their opening stages on grievance flags authored by peacetime
    /// faction actions and earlier war events. Part of the closed trigger
    /// grammar — still one explicit FactionWarTriggerTable entry per stage.
    /// </summary>
    public sealed class FlagTrigger : FactionWarTrigger
    {
        private readonly string _flagId;
        public FlagTrigger(string flagId) => _flagId = flagId;
        public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsFlagSet(_flagId);
    }

    /// <summary>Fires once the player has visited the given location (any day).</summary>
    public sealed class PlayerVisitedTrigger : FactionWarTrigger
    {
        private readonly string _locationId;
        public PlayerVisitedTrigger(string locationId) => _locationId = locationId;
        public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.HasVisitedLocation(_locationId);
    }

    /// <summary>Fires once the named chain has fully resolved (its terminal stage reached).</summary>
    public sealed class ChainResolvedTrigger : FactionWarTrigger
    {
        private readonly string _chainId;
        public ChainResolvedTrigger(string chainId) => _chainId = chainId;
        public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsChainResolved(_chainId);
    }

    /// <summary>
    /// Fires offsetDays after the earliest-resolving of the given source
    /// stages resolves. A single source stage is the common linear-progression
    /// case (pattern 2 in the design inventory); multiple sources cover the
    /// one "regardless of which s2 variant the player reached" case (pattern
    /// 7, evt_d545_ration_plaza_strike_s1) where any one of several fan-out
    /// terminal stages starts the same countdown.
    /// </summary>
    public sealed class DayOffsetTrigger : FactionWarTrigger
    {
        private readonly string[] _fromStageIds;
        private readonly int _offsetDays;

        public DayOffsetTrigger(int offsetDays, params string[] fromStageIds)
        {
            _offsetDays = offsetDays;
            _fromStageIds = fromStageIds ?? Array.Empty<string>();
        }

        public override bool IsSatisfied(FactionWarTriggerContext ctx)
        {
            int earliest = -1;
            for (int i = 0; i < _fromStageIds.Length; i++)
            {
                int resolvedDay = ctx.StageResolvedDay(_fromStageIds[i]);
                if (resolvedDay < 0) continue;
                if (earliest < 0 || resolvedDay < earliest) earliest = resolvedDay;
            }
            if (earliest < 0) return false;
            return ctx.CurrentDay >= earliest + _offsetDays;
        }
    }

    /// <summary>All sub-conditions must be satisfied. Covers the two authored
    /// AND-of-two-conditions stages (chain-resolved+visited; chain-resolved+chain-resolved).</summary>
    public sealed class AndTrigger : FactionWarTrigger
    {
        private readonly FactionWarTrigger[] _conditions;
        public AndTrigger(params FactionWarTrigger[] conditions) => _conditions = conditions ?? Array.Empty<FactionWarTrigger>();

        public override bool IsSatisfied(FactionWarTriggerContext ctx)
        {
            for (int i = 0; i < _conditions.Length; i++)
                if (!_conditions[i].IsSatisfied(ctx)) return false;
            return true;
        }
    }

    /// <summary>
    /// Always-true trigger for a chain's very first stage when its own
    /// minDay is the only real gate (the runner still enforces minDay
    /// separately — this exists so every stage has SOME trigger object,
    /// keeping the lookup total rather than partial).
    /// </summary>
    public sealed class AlwaysTrigger : FactionWarTrigger
    {
        public static readonly AlwaysTrigger Instance = new AlwaysTrigger();
        public override bool IsSatisfied(FactionWarTriggerContext ctx) => true;
    }

    /// <summary>
    /// Maps every one of the 45 authored triggerCondition prose strings in
    /// faction_war_events.json to its real FactionWarTrigger, by stageId.
    /// This is a content-shaped lookup (one entry per stage), not a parser —
    /// per this file's class doc, the prose was authored to be translatable
    /// 1:1 into these five node types, and this table IS that translation.
    /// Extend when NEW stages are authored; do not attempt to auto-derive
    /// this from the prose string at runtime.
    /// </summary>
    public static class FactionWarTriggerTable
    {
        public static readonly Dictionary<string, FactionWarTrigger> ByStageId = Build();

        public static FactionWarTrigger For(string stageId) =>
            ByStageId.TryGetValue(stageId, out var t) ? t : AlwaysTrigger.Instance;

        private static Dictionary<string, FactionWarTrigger> Build()
        {
            var t = new Dictionary<string, FactionWarTrigger>(StringComparer.Ordinal);

            // Pattern 1: chain-opening stages gated on visiting the chain's location.
            t["evt_d480_grain_tally_dispute_s1"] = new PlayerVisitedTrigger("loc_grain_silo");
            t["evt_d485_checkpoint_notice_war_s1"] = new PlayerVisitedTrigger("loc_garrison_checkpoint_gamma");
            t["evt_d488_manifest_holdup_s1"] = new PlayerVisitedTrigger("loc_garrison_checkpoint_gamma");
            t["evt_d491_toll_hike_s1"] = new PlayerVisitedTrigger("loc_weighbridge");
            t["evt_d495_the_clean_strike_s1"] = new PlayerVisitedTrigger("loc_railway_span_44_alpha");
            t["evt_d503_conscription_lists_s1"] = new PlayerVisitedTrigger("loc_conscription_office");
            t["evt_d517_almshouse_shelling_s1"] = new PlayerVisitedTrigger("loc_st_brigids_almshouse");
            t["evt_d522_switchback_toll_s1"] = new PlayerVisitedTrigger("loc_shrine_switchback_waystation");
            t["evt_d565_hydro_leverage_break_s1"] = new PlayerVisitedTrigger("loc_terrace_pumphouse");

            // Pattern 2: plain "N days after {stageId}" linear progression.
            t["evt_d480_grain_tally_dispute_s2"] = new DayOffsetTrigger(3, "evt_d480_grain_tally_dispute_s1");
            t["evt_d485_checkpoint_notice_war_s2"] = new DayOffsetTrigger(4, "evt_d485_checkpoint_notice_war_s1");
            t["evt_d488_manifest_holdup_s2"] = new DayOffsetTrigger(2, "evt_d488_manifest_holdup_s1");
            t["evt_d491_toll_hike_s2"] = new DayOffsetTrigger(3, "evt_d491_toll_hike_s1");
            t["evt_d495_the_clean_strike_s2"] = new DayOffsetTrigger(3, "evt_d495_the_clean_strike_s1");
            t["evt_d503_conscription_lists_s2"] = new DayOffsetTrigger(5, "evt_d503_conscription_lists_s1");
            t["evt_d509_border_clash_span44_s2"] = new DayOffsetTrigger(3, "evt_d509_border_clash_span44_s1");
            t["evt_d517_almshouse_shelling_s2"] = new DayOffsetTrigger(2, "evt_d517_almshouse_shelling_s1");
            t["evt_d517_almshouse_shelling_s3"] = new DayOffsetTrigger(2, "evt_d517_almshouse_shelling_s2");
            t["evt_d522_switchback_toll_s2"] = new DayOffsetTrigger(3, "evt_d522_switchback_toll_s1");
            t["evt_d524_market_price_spike_s2"] = new DayOffsetTrigger(4, "evt_d524_market_price_spike_s1");
            t["evt_d533_garrison_offensive_grain_silo_s2"] = new DayOffsetTrigger(3, "evt_d533_garrison_offensive_grain_silo_s1");
            t["evt_d541_evacuation_window_plaza_s2_warned"] = new DayOffsetTrigger(0, "evt_d541_evacuation_window_plaza_s1");
            t["evt_d541_evacuation_window_plaza_s2_looted"] = new DayOffsetTrigger(0, "evt_d541_evacuation_window_plaza_s1");
            t["evt_d541_evacuation_window_plaza_s2_silent"] = new DayOffsetTrigger(0, "evt_d541_evacuation_window_plaza_s1");
            t["evt_d545_ration_plaza_strike_s2"] = new DayOffsetTrigger(3, "evt_d545_ration_plaza_strike_s1");
            t["evt_d552_rebuilders_fracture_s2"] = new DayOffsetTrigger(3, "evt_d552_rebuilders_fracture_s1");
            t["evt_d558_ln74_signal_intercept_s2"] = new DayOffsetTrigger(2, "evt_d558_ln74_signal_intercept_s1");
            t["evt_d565_hydro_leverage_break_s2"] = new DayOffsetTrigger(4, "evt_d565_hydro_leverage_break_s1");
            t["evt_d570_forward_roster_first_action_s2"] = new DayOffsetTrigger(2, "evt_d570_forward_roster_first_action_s1");
            t["evt_d578_shrine_strike_anomaly_s2"] = new DayOffsetTrigger(2, "evt_d578_shrine_strike_anomaly_s1");
            t["evt_d578_shrine_strike_anomaly_s3"] = new DayOffsetTrigger(4, "evt_d578_shrine_strike_anomaly_s2");
            t["evt_d588_ceasefire_by_exhaustion_s2"] = new DayOffsetTrigger(4, "evt_d588_ceasefire_by_exhaustion_s1");

            // Pattern 7: OR-of-three-stages day offset (any of the three
            // evacuation-window variants starts the same two-day countdown to
            // the strike itself — the strike is not preventable by that choice).
            t["evt_d545_ration_plaza_strike_s1"] = new DayOffsetTrigger(2,
                "evt_d541_evacuation_window_plaza_s2_warned",
                "evt_d541_evacuation_window_plaza_s2_looted",
                "evt_d541_evacuation_window_plaza_s2_silent");

            // Pattern 3: plain cross-chain "once {chainId} has resolved" dependency.
            t["evt_d509_border_clash_span44_s1"] = new ChainResolvedTrigger("evt_d495_the_clean_strike");
            t["evt_d524_market_price_spike_s1"] = new ChainResolvedTrigger("evt_d517_almshouse_shelling");
            t["evt_d533_garrison_offensive_grain_silo_s1"] = new ChainResolvedTrigger("evt_d524_market_price_spike");
            t["evt_d541_evacuation_window_plaza_s1"] = new ChainResolvedTrigger("evt_d533_garrison_offensive_grain_silo");
            t["evt_d552_rebuilders_fracture_s1"] = new ChainResolvedTrigger("evt_d545_ration_plaza_strike");
            t["evt_d558_ln74_signal_intercept_s1"] = new ChainResolvedTrigger("evt_d509_border_clash_span44");
            t["evt_d578_shrine_strike_anomaly_s1"] = new ChainResolvedTrigger("evt_d558_ln74_signal_intercept");
            t["evt_d583_d9_reassessment_s1"] = new ChainResolvedTrigger("evt_d570_forward_roster_first_action");
            t["evt_d588_ceasefire_by_exhaustion_s1"] = new ChainResolvedTrigger("evt_d578_shrine_strike_anomaly");
            t["evt_d600_theory_surfaces_s1"] = new ChainResolvedTrigger("evt_d588_ceasefire_by_exhaustion");

            // Pattern 4: chain-resolved AND player-visited.
            t["evt_d570_forward_roster_first_action_s1"] = new AndTrigger(
                new ChainResolvedTrigger("evt_d552_rebuilders_fracture"),
                new PlayerVisitedTrigger("loc_forward_roster_camp"));

            // Pattern 5: chain-resolved AND chain-resolved.
            t["evt_d605_post_ceasefire_forward_roster_s1"] = new AndTrigger(
                new ChainResolvedTrigger("evt_d588_ceasefire_by_exhaustion"),
                new ChainResolvedTrigger("evt_d570_forward_roster_first_action"));

            return t;
        }
    }

    /// <summary>Per-chain progress: which stage is current (empty = chain fully
    /// resolved / chain not yet started is distinguished by absence from the
    /// dictionary), and the day each stage actually resolved on (for
    /// DayOffsetTrigger's fromStageId lookups and for "chain resolved" checks).</summary>
    [Serializable]
    public sealed class FactionWarChainProgress
    {
        public string chainId = string.Empty;
        public string currentStageId = string.Empty;
        public bool resolved;

        /// <summary>stageId -> day it resolved on (a choice was made, or the
        /// stage had no choices and simply advanced). Needed by DayOffsetTrigger
        /// even after currentStageId has moved past that stage.</summary>
        public List<StageResolution> stageResolutions = new List<StageResolution>();
    }

    [Serializable]
    public sealed class StageResolution
    {
        public string stageId = string.Empty;
        public int day;
    }

    [Serializable]
    public sealed class FactionWarChainRunnerState
    {
        public string systemId = FactionWarChainRunner.SystemId;
        public int schemaVersion = 1;
        public List<FactionWarChainProgress> chains = new List<FactionWarChainProgress>();
        public List<string> visitedLocations = new List<string>();
        public int cumulativeMoraleDelta;

        /// <summary>Plan 25 additive: flags produced by stage/choice producesFlag
        /// fields. Old saves deserialize with null; the runner re-initializes.</summary>
        public List<string> producedFlags = new List<string>();
    }

    /// <summary>
    /// Advances every chain in a FactionWarContentCatalog day by day: tracks
    /// which stage is current per chain, evaluates that stage's
    /// FactionWarTrigger (via FactionWarTriggerTable), surfaces the stage to
    /// the host when its trigger fires, and — once the host reports the
    /// player's choice — applies moraleDelta and advances to leadsToStageId
    /// (or marks the chain resolved when leadsToStageId is empty). Stages
    /// with zero choices auto-advance the instant their trigger fires (no
    /// player input required — matches the many "Fires N days after X" plain
    /// narration-only stages in the authored data).
    ///
    /// Zero engine dependencies; deterministic (no randomness at all — pure
    /// day/state advancement, matching the authored content's own lack of
    /// dice rolls).
    /// </summary>
    public sealed class FactionWarChainRunner
    {
        public const string SystemId = "faction_war_chain_runner";

        private readonly FactionWarContentCatalog _catalog;
        private FactionWarChainRunnerState _state;

        public event Action<FactionWarEventChain, FactionWarEventStage>? OnStageSurfaced;
        public event Action<FactionWarEventChain, FactionWarEventStage, FactionWarEventChoice>? OnStageResolved;
        public event Action<FactionWarEventChain>? OnChainResolved;

        public FactionWarChainRunner(FactionWarContentCatalog catalog, FactionWarChainRunnerState? state = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _state = state ?? new FactionWarChainRunnerState();
            if (_state.chains == null) _state.chains = new List<FactionWarChainProgress>();
            if (_state.visitedLocations == null) _state.visitedLocations = new List<string>();
            if (_state.producedFlags == null) _state.producedFlags = new List<string>();
        }

        public FactionWarChainRunnerState State => _state;
        public int CumulativeMoraleDelta => _state.cumulativeMoraleDelta;

        // ── Plan 25 injection points (host-owned effects, no Core coupling) ──

        /// <summary>Optional probe into the host's campaign flag store, consulted
        /// in addition to the runner's own produced flags (e.g. Plan 25 grievance
        /// flags authored by the FactionActionBoard).</summary>
        public Func<string, bool>? ExternalFlagProbe;

        /// <summary>Optional sink for choice standing adjustments — the host binds
        /// FactionWarSystem.ModifyStanding here. Core never touches war standing
        /// on its own.</summary>
        public Action<string, int>? StandingDeltaApplier;

        /// <summary>Records that the player has visited a location — feeds
        /// PlayerVisitedTrigger for every chain, present and future.</summary>
        public void RecordLocationVisited(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return;
            if (!_state.visitedLocations.Contains(locationId))
                _state.visitedLocations.Add(locationId);
        }

        public bool HasVisited(string locationId) => _state.visitedLocations.Contains(locationId);

        public bool IsChainResolved(string chainId)
        {
            var progress = FindProgress(chainId);
            return progress != null && progress.resolved;
        }

        /// <summary>The stage currently awaiting the player's choice for a chain,
        /// or null if the chain hasn't started, is resolved, or its current
        /// stage's trigger hasn't fired yet for the given day.</summary>
        public FactionWarEventStage? GetSurfacedStage(string chainId, int currentDay)
        {
            var chain = FindChain(chainId);
            if (chain == null) return null;

            var progress = FindProgress(chainId);
            string stageId = progress?.currentStageId ?? string.Empty;
            if (string.IsNullOrEmpty(stageId))
                stageId = chain.stages.Count > 0 ? chain.stages[0].stageId : string.Empty;
            if (string.IsNullOrEmpty(stageId)) return null;
            if (progress != null && progress.resolved) return null;

            var stage = chain.stages.FirstOrDefault(s => s.stageId == stageId);
            if (stage == null) return null;
            if (currentDay < stage.minDay) return null;

            var trigger = FactionWarTriggerTable.For(stageId);
            var ctx = BuildContext(currentDay);
            if (!trigger.IsSatisfied(ctx)) return null;

            // Plan 25: authored flag gate on the stage itself.
            if (!string.IsNullOrEmpty(stage.requiresFlag) && !ctx.IsFlagSet(stage.requiresFlag))
                return null;

            return stage;
        }

        /// <summary>
        /// Plan 25: whether a choice may be offered for the given stage — a
        /// choice carrying requiresFlag is hidden until that flag is set. Hosts
        /// must not render (or speculatively resolve) unavailable choices.
        /// </summary>
        public bool IsChoiceAvailable(FactionWarEventStage stage, FactionWarEventChoice choice, int currentDay)
        {
            if (stage == null || choice == null) return false;
            if (string.IsNullOrEmpty(choice.requiresFlag)) return true;
            return IsFlagSet(choice.requiresFlag);
        }

        /// <summary>True when the flag was produced by this runner or reported by
        /// the host's external probe.</summary>
        public bool IsFlagSet(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return false;
            if (_state.producedFlags.Contains(flagId)) return true;
            return ExternalFlagProbe?.Invoke(flagId) ?? false;
        }

        /// <summary>
        /// Advances every registered chain for the given day: for each chain
        /// whose current stage's trigger just became satisfied, surfaces it
        /// (OnStageSurfaced) and, if that stage has zero choices, immediately
        /// auto-resolves it (no player input needed) and advances. Call once
        /// per simulated day, same cadence as YearOfAshHostSession.TickDay.
        /// </summary>
        public void TickDay(int currentDay)
        {
            foreach (var chain in _catalog.EventChains)
            {
                if (chain?.stages == null || chain.stages.Count == 0) continue;
                if (IsChainResolved(chain.chainId)) continue;

                var stage = GetSurfacedStage(chain.chainId, currentDay);
                if (stage == null) continue;

                OnStageSurfaced?.Invoke(chain, stage);

                if (stage.choices == null || stage.choices.Count == 0)
                {
                    // Zero-choice stages narrate and advance automatically —
                    // there is nothing for the player to decide.
                    AdvancePastStage(chain, stage, currentDay, choice: null);
                }
            }
        }

        /// <summary>
        /// The host calls this once the player has picked a choice for the
        /// currently-surfaced stage of the given chain. Applies moraleDelta
        /// and advances to the choice's leadsToStageId (empty = chain
        /// resolved). Throws if the chain/stage/choice don't match what is
        /// actually surfaced — the host must not call this speculatively.
        /// </summary>
        public void ResolveChoice(string chainId, string stageId, string choiceId, int currentDay)
        {
            var chain = FindChain(chainId) ?? throw new ArgumentException($"Unknown chain '{chainId}'.", nameof(chainId));
            var stage = chain.stages.FirstOrDefault(s => s.stageId == stageId)
                ?? throw new ArgumentException($"Unknown stage '{stageId}' in chain '{chainId}'.", nameof(stageId));
            var choice = stage.choices.FirstOrDefault(c => c.choiceId == choiceId)
                ?? throw new ArgumentException($"Unknown choice '{choiceId}' in stage '{stageId}'.", nameof(choiceId));
            if (!IsChoiceAvailable(stage, choice, currentDay))
                throw new InvalidOperationException(
                    $"Choice '{choiceId}' in stage '{stageId}' is gated by flag '{choice.requiresFlag}', which is not set.");

            var progress = FindProgress(chainId);
            string surfacedStageId = progress?.currentStageId;
            if (string.IsNullOrEmpty(surfacedStageId)) surfacedStageId = chain.stages[0].stageId;
            if (!string.Equals(surfacedStageId, stageId, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Chain '{chainId}' is currently at stage '{surfacedStageId}', not '{stageId}'.");
            }

            AdvancePastStage(chain, stage, currentDay, choice);
        }

        private void AdvancePastStage(FactionWarEventChain chain, FactionWarEventStage stage, int currentDay, FactionWarEventChoice? choice)
        {
            var progress = FindOrCreateProgress(chain.chainId);
            RecordStageResolution(progress, stage.stageId, currentDay);

            // Plan 25: a resolving stage may author a flag regardless of how it resolves.
            if (!string.IsNullOrEmpty(stage.producesFlag))
                ProduceFlag(stage.producesFlag);

            if (choice != null)
            {
                _state.cumulativeMoraleDelta += choice.moraleDelta;
                if (!string.IsNullOrEmpty(choice.producesFlag))
                    ProduceFlag(choice.producesFlag);
                if (choice.standingDelta != 0 && !string.IsNullOrEmpty(choice.standingFactionId))
                    StandingDeltaApplier?.Invoke(choice.standingFactionId, choice.standingDelta);
                OnStageResolved?.Invoke(chain, stage, choice);
            }

            string next = choice?.leadsToStageId ?? string.Empty;
            if (string.IsNullOrEmpty(next))
            {
                progress.resolved = true;
                progress.currentStageId = string.Empty;
                OnChainResolved?.Invoke(chain);
            }
            else
            {
                progress.currentStageId = next;
            }
        }

        private void ProduceFlag(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            if (!_state.producedFlags.Contains(flagId))
                _state.producedFlags.Add(flagId);
        }

        private static void RecordStageResolution(FactionWarChainProgress progress, string stageId, int day)
        {
            var existing = progress.stageResolutions.FirstOrDefault(r => r.stageId == stageId);
            if (existing != null) { existing.day = day; return; }
            progress.stageResolutions.Add(new StageResolution { stageId = stageId, day = day });
        }

        private FactionWarTriggerContext BuildContext(int currentDay) => new FactionWarTriggerContext
        {
            CurrentDay = currentDay,
            IsChainResolved = IsChainResolved,
            HasVisitedLocation = HasVisited,
            IsFlagSet = IsFlagSet,
            StageResolvedDay = stageId =>
            {
                foreach (var progress in _state.chains)
                {
                    var r = progress.stageResolutions.FirstOrDefault(x => x.stageId == stageId);
                    if (r != null) return r.day;
                }
                return -1;
            }
        };

        private FactionWarEventChain? FindChain(string chainId) =>
            _catalog.EventChains.FirstOrDefault(c => c.chainId == chainId);

        private FactionWarChainProgress? FindProgress(string chainId) =>
            _state.chains.FirstOrDefault(p => p.chainId == chainId);

        private FactionWarChainProgress FindOrCreateProgress(string chainId)
        {
            var existing = FindProgress(chainId);
            if (existing != null) return existing;
            var created = new FactionWarChainProgress { chainId = chainId };
            _state.chains.Add(created);
            return created;
        }

        public FactionWarChainRunnerState CaptureState() => Clone(_state);

        public void RestoreState(FactionWarChainRunnerState state)
        {
            if (state == null) return;
            if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
                throw new ArgumentException($"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
            if (state.schemaVersion > 1)
                throw new NotSupportedException($"Future FactionWarChainRunner save schema {state.schemaVersion}; supported schema is 1.");
            _state = Clone(state);
        }

        private static FactionWarChainRunnerState Clone(FactionWarChainRunnerState source)
        {
            var copy = new FactionWarChainRunnerState
            {
                systemId = source.systemId,
                schemaVersion = source.schemaVersion,
                cumulativeMoraleDelta = source.cumulativeMoraleDelta,
                visitedLocations = new List<string>(source.visitedLocations ?? new List<string>()),
                producedFlags = new List<string>(source.producedFlags ?? new List<string>()),
                chains = new List<FactionWarChainProgress>()
            };
            if (source.chains != null)
            {
                foreach (var p in source.chains)
                {
                    if (p == null) continue;
                    var pCopy = new FactionWarChainProgress
                    {
                        chainId = p.chainId,
                        currentStageId = p.currentStageId,
                        resolved = p.resolved,
                        stageResolutions = new List<StageResolution>()
                    };
                    if (p.stageResolutions != null)
                    {
                        foreach (var r in p.stageResolutions)
                            if (r != null) pCopy.stageResolutions.Add(new StageResolution { stageId = r.stageId, day = r.day });
                    }
                    copy.chains.Add(pCopy);
                }
            }
            return copy;
        }
    }
}
