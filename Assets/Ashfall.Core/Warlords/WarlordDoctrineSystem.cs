using System;
using System.Collections.Generic;

namespace Ashfall.Core.Warlords
{
    /// <summary>Territory state ladder: unclaimed → claimed → contested → controlled.</summary>
    public enum WarlordTerritoryState
    {
        None = 0,
        Claimed = 1,
        Contested = 2,
        Controlled = 3
    }

    /// <summary>Strategic actions the warlord may take on an operation day.</summary>
    public enum WarlordStrategicAction
    {
        None = 0,
        DemandTribute = 1,
        Raid = 2,
        Defend = 3,
        Contest = 4,
        Annex = 5,
        Withdraw = 6
    }

    /// <summary>
    /// Per-location territory record. locationId references an existing catalog
    /// location (validated at load); state follows the ladder above.
    /// </summary>
    [Serializable]
    public class WarlordTerritoryRecord
    {
        public string locationId = string.Empty;
        public int state = (int)WarlordTerritoryState.None;
        public int sinceDay = -1;
        public int lastOutcomeDay = -1;
        public bool lastOutcomeSuccess;
        public int claimAttempts;
    }

    /// <summary>
    /// Explicit intelligence: what the warlord believes about a location, with
    /// the day the report arrived. Decisions read this table, never the true
    /// world state directly — the host feeds observations through Observe().
    /// </summary>
    [Serializable]
    public class WarlordReport
    {
        public string locationId = string.Empty;
        public int state = (int)WarlordTerritoryState.None;
        public int reportDay = -1;
        public float confidence = 0.5f;
    }

    [Serializable]
    public class WarlordDoctrineHistoryEntry
    {
        public string doctrineId = string.Empty;
        public int day = -1;
        public string reason = string.Empty;
    }

    /// <summary>
    /// Serialized warlord state. Owned by the YearOfAshSave envelope (v3).
    /// RestoreState(null) yields a fresh toll-doctrine warlord with the home
    /// location controlled and no knowledge — older saves load safely.
    /// </summary>
    [Serializable]
    public class WarlordDoctrineState
    {
        public string factionId = "warlords_sector_4";
        public string doctrineId = "warlord_doctrine_toll";
        public List<WarlordDoctrineHistoryEntry> doctrineHistory = new List<WarlordDoctrineHistoryEntry>();
        public int doctrineChangedDay = -1;
        public int doctrineCooldownUntilDay = -1;

        // Supply ledger.
        public int supply;
        public int supplyNeed = 10;

        // Tribute ledger (Unity Warlord Code semantics, proposed canon).
        public float tributeMultiplier = 1f;
        public int consecutiveShortWeeks;
        public int totalWeeksPaid;
        public int totalWeeksAsked;
        public int lastAskDay = -1;

        // Operation ledger.
        public int successStreak;
        public int failureStreak;
        public int totalOperations;
        public int casualties;
        public int lastActionDay = -1;

        public List<WarlordTerritoryRecord> territory = new List<WarlordTerritoryRecord>();
        public List<WarlordReport> reports = new List<WarlordReport>();
        public List<string> narrativeMarkers = new List<string>();

        public int seedSalt;
        public int lastTickDay = -1;

        public WarlordTerritoryRecord? Territory(string locationId)
        {
            if (territory == null) return null;
            for (int i = 0; i < territory.Count; i++)
                if (territory[i] != null && territory[i].locationId == locationId)
                    return territory[i];
            return null;
        }
    }

    /// <summary>Per-tick host-provided world view (no omniscience — only what the host feeds).</summary>
    public sealed class WarlordContext
    {
        public float EnvironmentHazard; // 0..1 (weather, radiation, deep-freeze severity)
        public float RivalPressure;     // 0..1 (garrison/rebuilders pressure on warlord ground)
        public int PlayerStanding;      // -100..100 (warlord's standing with the player)
    }

    public sealed class WarlordActionResult
    {
        public WarlordStrategicAction Action;
        public string TargetLocationId = string.Empty;
        public bool Success;
        public string Detail = string.Empty;
    }

    /// <summary>
    /// PROPOSED MODEL (the Unity stub, System_AdaptiveWarlords, is a meta-
    /// progression combat-counter learner with no usable faction-AI semantics;
    /// see docs/ASHFALL_DEEP_CODE_AUDIT_2026-08-07.txt). This is a new,
    /// conservative, data-driven faction-level warlord decision system:
    /// doctrine state machine with hysteresis + cooldown, explicit intelligence
    /// (reports, not omniscience), seeded deterministic action selection, and
    /// territory claims/contests/control over existing catalog locations.
    /// Binds only to the canonical faction id warlords_sector_4; alias pairs
    /// (raiders / iron_garrison / ash_militia) are reported, never merged.
    /// Engine-agnostic; the host owns the rng, the world context and the
    /// journal/radio/inventory/standing plumbing.
    /// </summary>
    public sealed class WarlordDoctrineSystem
    {
        public const string SystemId = "warlord_doctrine_system";
        public const string CanonicalFactionId = "warlords_sector_4";

        public const int BaseSupplyNeed = 10;
        public const int SupplyPerControlledNode = 2;
        public const float AnnexBaseChance = 0.55f;
        public const float MinResolutionChance = 0.10f;
        public const float MaxResolutionChance = 0.90f;

        private readonly WarlordDoctrineCatalog _catalog;
        private WarlordDoctrineState _state = new WarlordDoctrineState();

        public event Action<string, string, string, int> OnDoctrineChanged;   // from, to, reason, day
        public event Action<WarlordActionResult> OnActionExecuted;
        public event Action<string, int, int, int> OnTerritoryChanged;       // locationId, from, to, day
        public event Action<int, string, int> OnTributeDemanded;              // amount, itemId, day
        public event Action<bool, int> OnTributeSettled;                      // paidFull, day
        public event Action<string, string> OnNarrativeRequested;             // journalKey / radioKey
        public event Action OnStateChanged;

        public WarlordDoctrineSystem(WarlordDoctrineCatalog catalog = null!, int seedSalt = 808)
        {
            _catalog = catalog ?? new WarlordDoctrineCatalog();
            _state.seedSalt = seedSalt;
            ApplyCatalogDefaults();
        }

        public WarlordDoctrineState State => _state;
        public string DoctrineId => _state.doctrineId;
        public WarlordDoctrineDef? Doctrine => _catalog.GetDoctrine(_state.doctrineId);
        public int Supply => _state.supply;
        public int SupplyNeed => _state.supplyNeed;
        public float TributeMultiplier => _state.tributeMultiplier;
        public int TotalOperations => _state.totalOperations;
        public WarlordDoctrineCatalog Catalog => _catalog;

        // ── Territory queries (reported/true blend, player-visible) ─────

        public WarlordTerritoryState TerritoryState(string locationId)
        {
            var rec = _state.Territory(locationId);
            return rec != null ? (WarlordTerritoryState)rec.state : WarlordTerritoryState.None;
        }

        public int ControlledCount()
        {
            int n = 0;
            if (_state.territory == null) return n;
            for (int i = 0; i < _state.territory.Count; i++)
                if (_state.territory[i] != null && _state.territory[i].state == (int)WarlordTerritoryState.Controlled)
                    n++;
            return n;
        }

        public int ContestedCount()
        {
            int n = 0;
            if (_state.territory == null) return n;
            for (int i = 0; i < _state.territory.Count; i++)
                if (_state.territory[i] != null && _state.territory[i].state == (int)WarlordTerritoryState.Contested)
                    n++;
            return n;
        }

        /// <summary>
        /// Travel danger multiplier the player's columns face at a location the
        /// warlord holds. Controlled → +0.35, Contested → +0.20, Claimed → +0.10.
        /// Hosts apply this to expedition encounter chance / danger level.
        /// </summary>
        public float TravelDangerModifier(string locationId)
        {
            switch (TerritoryState(locationId))
            {
                case WarlordTerritoryState.Controlled: return 0.35f;
                case WarlordTerritoryState.Contested: return 0.20f;
                case WarlordTerritoryState.Claimed: return 0.10f;
                default: return 0f;
            }
        }

        /// <summary>True when the warlord actively holds the location (checkpoints up).</summary>
        public bool IsHostileAccess(string locationId) =>
            TerritoryState(locationId) == WarlordTerritoryState.Controlled;

        // ── Intelligence (explicit, non-omniscient) ────────────────────

        /// <summary>Feed the warlord what it could plausibly learn (scouts, radio, records).</summary>
        public void Observe(string locationId, WarlordTerritoryState state, int day, float confidence = 1f)
        {
            if (string.IsNullOrEmpty(locationId)) return;
            if (_state.reports == null) _state.reports = new List<WarlordReport>();
            for (int i = 0; i < _state.reports.Count; i++)
            {
                if (_state.reports[i] != null && _state.reports[i].locationId == locationId)
                {
                    _state.reports[i].state = (int)state;
                    _state.reports[i].reportDay = day;
                    _state.reports[i].confidence = confidence;
                    return;
                }
            }
            _state.reports.Add(new WarlordReport
            {
                locationId = locationId,
                state = (int)state,
                reportDay = day,
                confidence = confidence
            });
            RaiseChanged();
        }

        /// <summary>What the warlord believes about a location (None = unknown).</summary>
        public WarlordTerritoryState ReportedState(string locationId)
        {
            if (_state.reports != null)
            {
                for (int i = 0; i < _state.reports.Count; i++)
                {
                    var r = _state.reports[i];
                    if (r != null && r.locationId == locationId)
                        return (WarlordTerritoryState)r.state;
                }
            }
            return WarlordTerritoryState.None;
        }

        // ── Daily operation boundary ───────────────────────────────────

        /// <summary>
        /// One deliberate operation tick (not per-frame): signals → doctrine
        /// transition → action selection/resolution → tribute cadence. Guarded
        /// by lastTickDay so a host that ticks twice in a day cannot double-run.
        /// All randomness flows through the caller's ISeededRng.
        /// </summary>
        public void TickDaily(int day, ISeededRng rng, WarlordContext context)
        {
            if (_state.lastTickDay == day) return;
            _state.lastTickDay = day;
            if (rng == null) return;
            context = context ?? new WarlordContext();

            // 1. Harvest from controlled territory (supply ledger).
            HarvestSupply();

            // 2. Doctrine transition (hysteresis + cooldown, reported signals).
            MaybeTransitionDoctrine(day, context);

            // 3. Strategic action on the operation cadence.
            if (day - _state.lastActionDay >= Math.Max(1, _catalog.Warlord.action_interval_days))
            {
                var result = SelectAndResolveAction(day, rng, context);
                _state.lastActionDay = day;
                if (result != null)
                {
                    _state.totalOperations++;
                    OnActionExecuted?.Invoke(result);
                }
            }

            // 4. Tribute cadence.
            if (DoctrineEligible(WarlordStrategicAction.DemandTribute)
                && day - _state.lastAskDay >= Math.Max(1, _catalog.Warlord.tribute_interval_days))
            {
                int amount = Math.Max(1, (int)(_catalog.Warlord.tribute_base_amount * _state.tributeMultiplier));
                _state.lastAskDay = day;
                _state.totalWeeksAsked++;
                OnTributeDemanded?.Invoke(amount, _catalog.Warlord.tribute_currency_item, day);
            }

            RaiseChanged();
        }

        // ── Player-facing tribute settlement (host routes inventory) ────

        /// <summary>
        /// Settle the latest tribute ask. Full payment (≥ short-payment
        /// threshold) resets the short-week counter; short payment or refusal
        /// escalates the ask per the Warlord Code (×1.5, capped) and worsens
        /// standing. Returns true when the ask was met in full.
        /// </summary>
        public bool SettleTribute(int amountPaid, int day, out int nextAsk)
        {
            float threshold = _catalog.Warlord.short_payment_threshold;
            bool full = amountPaid > 0 && amountPaid >= _catalog.Warlord.tribute_base_amount * threshold;
            if (full)
            {
                _state.totalWeeksPaid++;
                _state.consecutiveShortWeeks = 0;
                OnTributeSettled?.Invoke(true, day);
            }
            else
            {
                _state.consecutiveShortWeeks++;
                float factor = _catalog.Warlord.tribute_escalation_factor;
                float cap = _catalog.Warlord.tribute_max_multiplier;
                _state.tributeMultiplier = Math.Min(cap, _state.tributeMultiplier * factor);
                OnTributeSettled?.Invoke(false, day);
            }
            nextAsk = Math.Max(1, (int)(_catalog.Warlord.tribute_base_amount * _state.tributeMultiplier));
            RaiseChanged();
            return full;
        }

        // ── Doctrine machinery ─────────────────────────────────────────

        private void MaybeTransitionDoctrine(int day, WarlordContext context)
        {
            if (day < _state.doctrineCooldownUntilDay) return;
            var def = Doctrine;
            if (def == null || def.transitions == null) return;

            var signals = ComputeSignals(context);
            for (int i = 0; i < def.transitions.Count; i++)
            {
                var t = def.transitions[i];
                if (t == null || string.IsNullOrEmpty(t.to)) continue;
                var target = _catalog.GetDoctrine(t.to);
                if (target == null) continue;
                if (!SignalMet(t, signals)) continue;

                string reason = t.signal + " " + t.condition + " " + t.threshold.ToString("0.##");
                SwitchDoctrine(t.to, day, reason);
                return; // one transition per operation day
            }
        }

        private static bool SignalMet(WarlordDoctrineTransitionDef t, WarlordSignals s)
        {
            float v = ValueOf(s, t.signal);
            float threshold = t.threshold;
            if (t.condition == "gte") return v >= threshold;
            return v <= threshold;
        }

        private static float ValueOf(WarlordSignals s, string signal)
        {
            switch (signal)
            {
                case "supply_ratio": return s.SupplyRatio;
                case "failure_streak": return s.FailureStreak;
                case "success_streak": return s.SuccessStreak;
                case "contested_count": return s.ContestedCount;
                case "player_tribute_reliability": return s.PlayerTributeReliability;
                case "environment_hazard": return s.EnvironmentHazard;
                case "rival_pressure": return s.RivalPressure;
                default: return 0f;
            }
        }

        private sealed class WarlordSignals
        {
            public float SupplyRatio;
            public float FailureStreak;
            public float SuccessStreak;
            public float ContestedCount;
            public float PlayerTributeReliability;
            public float EnvironmentHazard;
            public float RivalPressure;
        }

        private WarlordSignals ComputeSignals(WarlordContext context)
        {
            int controlled = ControlledCount();
            _state.supplyNeed = BaseSupplyNeed + SupplyPerControlledNode * controlled;
            int contested = ContestedCount();

            float reliability = _state.totalWeeksAsked > 0
                ? (float)_state.totalWeeksPaid / _state.totalWeeksAsked
                : 1f; // nothing asked yet: no complaint

            return new WarlordSignals
            {
                SupplyRatio = _state.supplyNeed > 0 ? (float)_state.supply / _state.supplyNeed : 0f,
                FailureStreak = _state.failureStreak,
                SuccessStreak = _state.successStreak,
                ContestedCount = contested,
                PlayerTributeReliability = reliability,
                EnvironmentHazard = Math.Clamp(context.EnvironmentHazard, 0f, 1f),
                RivalPressure = Math.Clamp(context.RivalPressure, 0f, 1f)
            };
        }

        private void SwitchDoctrine(string doctrineId, int day, string reason)
        {
            string from = _state.doctrineId;
            if (from == doctrineId) return;
            _state.doctrineId = doctrineId;
            _state.doctrineChangedDay = day;
            _state.doctrineCooldownUntilDay = day + Math.Max(1, _catalog.Warlord.doctrine_cooldown_days);
            if (_state.doctrineHistory == null) _state.doctrineHistory = new List<WarlordDoctrineHistoryEntry>();
            _state.doctrineHistory.Add(new WarlordDoctrineHistoryEntry { doctrineId = doctrineId, day = day, reason = reason });
            // Doctrine change resets streaks: the road starts over.
            _state.successStreak = 0;
            _state.failureStreak = 0;
            EmitNarrative(_catalog.GetDoctrine(doctrineId)!);
            OnDoctrineChanged?.Invoke(from, doctrineId, reason, day);
            RaiseChanged();
        }

        // ── Action machinery (seeded, ordinal-ordered) ─────────────────

        private bool DoctrineEligible(WarlordStrategicAction action)
        {
            var def = Doctrine;
            if (def == null || def.eligible_actions == null) return false;
            string name = action.ToString();
            for (int i = 0; i < def.eligible_actions.Count; i++)
                if (MatchesActionName(def.eligible_actions[i], action))
                    return true;
            return false;
        }

        private WarlordActionResult? SelectAndResolveAction(int day, ISeededRng rng, WarlordContext context)
        {
            var def = Doctrine;
            if (def == null || def.eligible_actions == null || def.eligible_actions.Count == 0) return null;

            // Weighted selection over the doctrine's eligible actions (ordinal
            // order by action name for stable iteration). Accepts either the
            // enum name (DemandTribute) or its snake_case form (demand_tribute).
            var eligible = new List<string>(def.eligible_actions);
            eligible.Sort(string.CompareOrdinal);
            int totalWeight = 0;
            for (int i = 0; i < eligible.Count; i++)
                totalWeight += WeightOf(def, eligible[i]);

            int roll = totalWeight > 0 ? rng.Next(0, totalWeight) : 0;
            string chosen = eligible[0];
            int acc = 0;
            for (int i = 0; i < eligible.Count; i++)
            {
                acc += WeightOf(def, eligible[i]);
                if (roll < acc) { chosen = eligible[i]; break; }
            }

            // Normalize the catalog name to the enum name for resolution.
            return ResolveAction(NormalizeActionName(chosen), day, rng, context);
        }

        /// <summary>demand_tribute → DemandTribute; DemandTribute stays as-is.</summary>
        private static string NormalizeActionName(string catalogName)
        {
            if (string.IsNullOrEmpty(catalogName)) return catalogName;
            foreach (WarlordStrategicAction a in Enum.GetValues(typeof(WarlordStrategicAction)))
            {
                if (a == WarlordStrategicAction.None) continue;
                if (a.ToString() == catalogName) return catalogName;
                if (ToSnake(a.ToString()) == catalogName) return a.ToString();
            }
            return catalogName;
        }

        private static bool MatchesActionName(string catalogName, WarlordStrategicAction action)
        {
            if (string.IsNullOrEmpty(catalogName)) return false;
            if (catalogName == action.ToString()) return true;
            return ToSnake(action.ToString()) == catalogName;
        }

        private static string ToSnake(string pascal)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < pascal.Length; i++)
            {
                char c = pascal[i];
                if (i > 0 && char.IsUpper(c))
                    sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        private static int WeightOf(WarlordDoctrineDef def, string actionName)
        {
            if (def.action_weights != null && def.action_weights.TryGetValue(actionName, out int w))
                return Math.Max(0, w);
            return 1;
        }

        private WarlordActionResult? ResolveAction(string actionName, int day, ISeededRng rng, WarlordContext context)
        {
            switch (actionName)
            {
                case nameof(WarlordStrategicAction.DemandTribute): return DemandTributeAction(day);
                case nameof(WarlordStrategicAction.Raid): return RaidAction(day, rng, context);
                case nameof(WarlordStrategicAction.Defend): return DefendAction(day);
                case nameof(WarlordStrategicAction.Contest): return ContestAction(day, rng, context);
                case nameof(WarlordStrategicAction.Annex): return AnnexAction(day, rng, context);
                case nameof(WarlordStrategicAction.Withdraw): return WithdrawAction(day);
                default: return null;
            }
        }

        private WarlordActionResult DemandTributeAction(int day)
        {
            // The tribute cadence is handled in TickDaily; this is the fallback
            // ask so a tribute doctrine never starves the ledger.
            int amount = Math.Max(1, (int)(_catalog.Warlord.tribute_base_amount * _state.tributeMultiplier));
            return new WarlordActionResult
            {
                Action = WarlordStrategicAction.DemandTribute,
                Success = true,
                Detail = "Collector calls. Ask: " + amount + "× " + _catalog.Warlord.tribute_currency_item + "."
            };
        }

        private WarlordActionResult RaidAction(int day, ISeededRng rng, WarlordContext context)
        {
            // Target: highest-supply non-controlled node adjacent to warlord
            // ground (ordinal tie-break), per the doctrine target rule.
            var target = PickTarget(adjacentOnly: true, requireControlled: false, rng);
            if (target == null)
            {
                return new WarlordActionResult
                {
                    Action = WarlordStrategicAction.Raid,
                    Success = false,
                    Detail = "No raidable target within reach."
                };
            }

            float chance = ResolutionChance(rng, context, target);
            bool success = rng.NextDouble() < chance;
            if (success)
            {
                _state.successStreak++;
                _state.supply += target.supply_value;
                AdvanceTerritory(target.location_id, WarlordTerritoryState.Contested, day, success: true);
                EmitNarrative();
            }
            else
            {
                _state.failureStreak++;
                _state.casualties++;
                _state.supply = Math.Max(0, _state.supply - 1);
            }
            return new WarlordActionResult
            {
                Action = WarlordStrategicAction.Raid,
                TargetLocationId = target.location_id,
                Success = success,
                Detail = success
                    ? "Column hit " + target.location_id + " and took what the road owed."
                    : "Column came back short at " + target.location_id + "."
            };
        }

        private WarlordActionResult ContestAction(int day, ISeededRng rng, WarlordContext context)
        {
            // Push a claim on an unclaimed/claimed node adjacent to control.
            var target = PickTarget(adjacentOnly: true, requireControlled: false, rng);
            if (target == null)
            {
                return new WarlordActionResult
                {
                    Action = WarlordStrategicAction.Contest,
                    Success = false,
                    Detail = "Nothing left to claim within reach."
                };
            }

            var rec = EnsureTerritory(target.location_id);
            WarlordTerritoryState next = rec.state >= (int)WarlordTerritoryState.Contested
                ? WarlordTerritoryState.Contested
                : WarlordTerritoryState.Claimed;
            float chance = ResolutionChance(rng, context, target) * 0.9f;
            bool success = rng.NextDouble() < chance;
            if (success)
            {
                AdvanceTerritory(target.location_id, next, day, success: true);
                _state.successStreak++;
            }
            else
            {
                _state.failureStreak++;
                _state.casualties++;
            }
            return new WarlordActionResult
            {
                Action = WarlordStrategicAction.Contest,
                TargetLocationId = target.location_id,
                Success = success,
                Detail = success
                    ? "Claim advanced at " + target.location_id + " (" + next + ")."
                    : "Claim at " + target.location_id + " did not take."
            };
        }

        private WarlordActionResult AnnexAction(int day, ISeededRng rng, WarlordContext context)
        {
            // Annex: claimed/contested → controlled. Legal only when the node is
            // adjacent to a controlled node (or already claimed/contested) and
            // not the home; failed attempts cool down the target.
            var target = PickAnnexTarget(rng);
            if (target == null)
            {
                return new WarlordActionResult
                {
                    Action = WarlordStrategicAction.Annex,
                    Success = false,
                    Detail = "No annexable ground; the map has no legal claim."
                };
            }

            float chance = ResolutionChance(rng, context, target);
            bool success = rng.NextDouble() < chance;
            if (success)
            {
                AdvanceTerritory(target.location_id, WarlordTerritoryState.Controlled, day, success: true);
                _state.successStreak++;
                _state.supply += target.supply_value;
                EmitNarrative();
            }
            else
            {
                _state.failureStreak++;
                _state.casualties++;
                var rec = _state.Territory(target.location_id);
                if (rec != null) rec.lastOutcomeDay = day;
            }
            return new WarlordActionResult
            {
                Action = WarlordStrategicAction.Annex,
                TargetLocationId = target.location_id,
                Success = success,
                Detail = success
                    ? "The Warlords take " + target.location_id + ": checkpoints up, boom across the road."
                    : "Annexation at " + target.location_id + " failed; the column fell back."
            };
        }

        private WarlordActionResult DefendAction(int day)
        {
            if (_state.supply <= 0)
            {
                _state.failureStreak++;
                return new WarlordActionResult
                {
                    Action = WarlordStrategicAction.Defend,
                    Success = false,
                    Detail = "No supplies to hold the ground."
                };
            }
            _state.supply = Math.Max(0, _state.supply - 1);
            _state.successStreak++;
            return new WarlordActionResult
            {
                Action = WarlordStrategicAction.Defend,
                Success = true,
                Detail = "Checkpoints held. The road stays priced."
            };
        }

        private WarlordActionResult WithdrawAction(int day)
        {
            int dropped = 0;
            if (_state.territory != null)
            {
                for (int i = 0; i < _state.territory.Count; i++)
                {
                    var rec = _state.territory[i];
                    if (rec == null) continue;
                    if (rec.state == (int)WarlordTerritoryState.Contested
                        || rec.state == (int)WarlordTerritoryState.Claimed)
                    {
                        rec.state = (int)WarlordTerritoryState.None;
                        rec.sinceDay = -1;
                        dropped++;
                    }
                }
            }
            _state.successStreak++;
            EmitNarrative();
            return new WarlordActionResult
            {
                Action = WarlordStrategicAction.Withdraw,
                Success = dropped > 0,
                Detail = dropped > 0
                    ? "Claims dropped: " + dropped + " contested/claimed node(s) released."
                    : "Nothing to withdraw; the Warlords stay quiet."
            };
        }

        // ── Target selection (deterministic ordering) ──────────────────

        private WarlordTerritoryNodeDef? PickTarget(bool adjacentOnly, bool requireControlled, ISeededRng rng)
        {
            var candidates = new List<WarlordTerritoryNodeDef>();
            for (int i = 0; i < _catalog.Territory.Count; i++)
            {
                var node = _catalog.Territory[i];
                if (node == null || node.home) continue;
                var rec = _state.Territory(node.location_id);
                int st = rec != null ? rec.state : (int)WarlordTerritoryState.None;
                if (st == (int)WarlordTerritoryState.Controlled && requireControlled) continue;
                if (st == (int)WarlordTerritoryState.Controlled) continue; // no raiding your own ground
                if (adjacentOnly && !IsAdjacentToControlled(node.location_id)) continue;
                candidates.Add(node);
            }
            if (candidates.Count == 0) return null;

            // Target rule: nearest_undefended → lowest defense, then highest
            // supply; highest_supply → highest supply. Ties break by ordinal id.
            var rule = Doctrine != null ? Doctrine.target_rule : "nearest_undefended";
            candidates.Sort((a, b) => CompareTargets(a, b, rule));
            return candidates[0];
        }

        private WarlordTerritoryNodeDef? PickAnnexTarget(ISeededRng rng)
        {
            var candidates = new List<WarlordTerritoryNodeDef>();
            for (int i = 0; i < _catalog.Territory.Count; i++)
            {
                var node = _catalog.Territory[i];
                if (node == null || node.home) continue;
                var rec = _state.Territory(node.location_id);
                int st = rec != null ? rec.state : (int)WarlordTerritoryState.None;
                if (st == (int)WarlordTerritoryState.Controlled) continue;
                // Legal annexation: adjacent to warlord control, or already
                // claimed/contested by the warlord, and not cooling down.
                bool adjacent = IsAdjacentToControlled(node.location_id);
                bool alreadyPushed = st == (int)WarlordTerritoryState.Claimed || st == (int)WarlordTerritoryState.Contested;
                if (!adjacent && !alreadyPushed) continue;
                if (rec != null && rec.lastOutcomeDay > 0
                    && _state.lastActionDay - rec.lastOutcomeDay < Math.Max(1, _catalog.Warlord.action_cooldown_days))
                    continue;
                candidates.Add(node);
            }
            if (candidates.Count == 0) return null;
            candidates.Sort((a, b) => CompareTargets(a, b, "highest_supply"));
            return candidates[0];
        }

        private static int CompareTargets(WarlordTerritoryNodeDef a, WarlordTerritoryNodeDef b, string rule)
        {
            int c;
            if (rule == "highest_supply")
            {
                c = b.supply_value.CompareTo(a.supply_value); // higher first
                if (c != 0) return c;
                c = b.defense_value.CompareTo(a.defense_value); // prefer weaker defenses
                if (c != 0) return c;
            }
            else
            {
                c = a.defense_value.CompareTo(b.defense_value); // lower defense first
                if (c != 0) return c;
                c = b.supply_value.CompareTo(a.supply_value);
                if (c != 0) return c;
            }
            return string.CompareOrdinal(a.location_id, b.location_id);
        }

        private bool IsAdjacentToControlled(string locationId)
        {
            var neighbors = _catalog.Neighbors(locationId);
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (TerritoryState(neighbors[i]) == WarlordTerritoryState.Controlled)
                    return true;
            }
            return false;
        }

        private float ResolutionChance(ISeededRng rng, WarlordContext context, WarlordTerritoryNodeDef target)
        {
            float def = Doctrine != null ? Doctrine.risk_tolerance : 0.5f;
            float supplyEdge = _state.supplyNeed > 0
                ? Math.Clamp(((float)_state.supply / _state.supplyNeed) - 1f, -0.3f, 0.3f) * 0.3f
                : 0f;
            float hazardPenalty = context.EnvironmentHazard * 0.35f;
            float rivalPenalty = context.RivalPressure * 0.25f;
            float defense = target.defense_value * 0.03f;
            float chance = AnnexBaseChance + def * 0.25f + supplyEdge - hazardPenalty - rivalPenalty - defense;
            return Math.Clamp(chance, MinResolutionChance, MaxResolutionChance);
        }

        private void AdvanceTerritory(string locationId, WarlordTerritoryState next, int day, bool success)
        {
            var rec = EnsureTerritory(locationId);
            int from = rec.state;
            if (from == (int)next) return;
            rec.state = (int)next;
            rec.sinceDay = day;
            rec.lastOutcomeDay = day;
            rec.lastOutcomeSuccess = success;
            rec.claimAttempts++;
            OnTerritoryChanged?.Invoke(locationId, from, (int)next, day);
        }

        private WarlordTerritoryRecord EnsureTerritory(string locationId)
        {
            var rec = _state.Territory(locationId);
            if (rec != null) return rec;
            rec = new WarlordTerritoryRecord { locationId = locationId };
            if (_state.territory == null) _state.territory = new List<WarlordTerritoryRecord>();
            _state.territory.Add(rec);
            _state.territory.Sort((a, b) => string.CompareOrdinal(a.locationId, b.locationId));
            return rec;
        }

        private void HarvestSupply()
        {
            int gain = 0;
            if (_state.territory != null)
            {
                for (int i = 0; i < _state.territory.Count; i++)
                {
                    var rec = _state.territory[i];
                    if (rec == null || rec.state != (int)WarlordTerritoryState.Controlled) continue;
                    var node = _catalog.GetNode(rec.locationId);
                    if (node != null) gain += node.supply_value;
                }
            }
            _state.supply += gain;
        }

        private void EmitNarrative(WarlordDoctrineDef doctrine = null!)
        {
            var d = doctrine ?? Doctrine;
            if (d == null) return;
            if (_state.narrativeMarkers == null) _state.narrativeMarkers = new List<string>();
            if (!string.IsNullOrEmpty(d.journal_key) && !_state.narrativeMarkers.Contains(d.journal_key))
            {
                _state.narrativeMarkers.Add(d.journal_key);
                OnNarrativeRequested?.Invoke(d.journal_key, d.radio_key);
            }
        }

        // ── Initialisation / save ──────────────────────────────────────

        private void ApplyCatalogDefaults()
        {
            var w = _catalog.Warlord ?? new WarlordDef();
            _state.factionId = w.faction_id;
            _state.doctrineId = w.starting_doctrine_id;
            if (_catalog.GetDoctrine(_state.doctrineId) == null)
                _state.doctrineId = _catalog.Doctrines.Count > 0 ? _catalog.Doctrines[0].id : "warlord_doctrine_toll";
            _state.supplyNeed = BaseSupplyNeed;

            // Home is controlled from the start; all other nodes unclaimed.
            _state.territory = new List<WarlordTerritoryRecord>();
            for (int i = 0; i < _catalog.Territory.Count; i++)
            {
                var node = _catalog.Territory[i];
                if (node == null || string.IsNullOrEmpty(node.location_id)) continue;
                var rec = new WarlordTerritoryRecord
                {
                    locationId = node.location_id,
                    state = node.home ? (int)WarlordTerritoryState.Controlled : (int)WarlordTerritoryState.None,
                    sinceDay = node.home ? 1 : -1
                };
                _state.territory.Add(rec);
            }
            _state.territory.Sort((a, b) => string.CompareOrdinal(a.locationId, b.locationId));
        }

        public WarlordDoctrineState CaptureState()
        {
            var copy = new WarlordDoctrineState
            {
                factionId = _state.factionId,
                doctrineId = _state.doctrineId,
                doctrineChangedDay = _state.doctrineChangedDay,
                doctrineCooldownUntilDay = _state.doctrineCooldownUntilDay,
                supply = _state.supply,
                supplyNeed = _state.supplyNeed,
                tributeMultiplier = _state.tributeMultiplier,
                consecutiveShortWeeks = _state.consecutiveShortWeeks,
                totalWeeksPaid = _state.totalWeeksPaid,
                totalWeeksAsked = _state.totalWeeksAsked,
                lastAskDay = _state.lastAskDay,
                successStreak = _state.successStreak,
                failureStreak = _state.failureStreak,
                totalOperations = _state.totalOperations,
                casualties = _state.casualties,
                lastActionDay = _state.lastActionDay,
                seedSalt = _state.seedSalt,
                lastTickDay = _state.lastTickDay
            };

            copy.doctrineHistory = new List<WarlordDoctrineHistoryEntry>();
            if (_state.doctrineHistory != null)
                for (int i = 0; i < _state.doctrineHistory.Count; i++)
                    if (_state.doctrineHistory[i] != null)
                        copy.doctrineHistory.Add(new WarlordDoctrineHistoryEntry
                        {
                            doctrineId = _state.doctrineHistory[i].doctrineId,
                            day = _state.doctrineHistory[i].day,
                            reason = _state.doctrineHistory[i].reason
                        });

            copy.territory = new List<WarlordTerritoryRecord>();
            if (_state.territory != null)
                for (int i = 0; i < _state.territory.Count; i++)
                    if (_state.territory[i] != null)
                        copy.territory.Add(CloneTerritory(_state.territory[i]));

            copy.reports = new List<WarlordReport>();
            if (_state.reports != null)
                for (int i = 0; i < _state.reports.Count; i++)
                    if (_state.reports[i] != null)
                        copy.reports.Add(new WarlordReport
                        {
                            locationId = _state.reports[i].locationId,
                            state = _state.reports[i].state,
                            reportDay = _state.reports[i].reportDay,
                            confidence = _state.reports[i].confidence
                        });

            copy.narrativeMarkers = new List<string>();
            if (_state.narrativeMarkers != null)
                for (int i = 0; i < _state.narrativeMarkers.Count; i++)
                    copy.narrativeMarkers.Add(_state.narrativeMarkers[i]);

            return copy;
        }

        public void RestoreState(WarlordDoctrineState saved)
        {
            _state = new WarlordDoctrineState { seedSalt = saved != null ? saved.seedSalt : _state.seedSalt };
            if (saved != null)
            {
                _state.factionId = string.IsNullOrEmpty(saved.factionId) ? CanonicalFactionId : saved.factionId;
                _state.doctrineId = saved.doctrineId;
                if (_catalog.GetDoctrine(_state.doctrineId) == null)
                    _state.doctrineId = _catalog.Doctrines.Count > 0 ? _catalog.Doctrines[0].id : "warlord_doctrine_toll";
                _state.doctrineChangedDay = saved.doctrineChangedDay;
                _state.doctrineCooldownUntilDay = saved.doctrineCooldownUntilDay;
                _state.supply = Math.Max(0, saved.supply);
                _state.supplyNeed = Math.Max(1, saved.supplyNeed);
                _state.tributeMultiplier = Math.Max(1f, Math.Min(8f, saved.tributeMultiplier));
                _state.consecutiveShortWeeks = Math.Max(0, saved.consecutiveShortWeeks);
                _state.totalWeeksPaid = Math.Max(0, saved.totalWeeksPaid);
                _state.totalWeeksAsked = Math.Max(0, saved.totalWeeksAsked);
                _state.lastAskDay = saved.lastAskDay;
                _state.successStreak = Math.Max(0, saved.successStreak);
                _state.failureStreak = Math.Max(0, saved.failureStreak);
                _state.totalOperations = Math.Max(0, saved.totalOperations);
                _state.casualties = Math.Max(0, saved.casualties);
                _state.lastActionDay = saved.lastActionDay;
                _state.lastTickDay = saved.lastTickDay;

                if (saved.doctrineHistory != null)
                {
                    _state.doctrineHistory = new List<WarlordDoctrineHistoryEntry>();
                    for (int i = 0; i < saved.doctrineHistory.Count; i++)
                        if (saved.doctrineHistory[i] != null)
                            _state.doctrineHistory.Add(new WarlordDoctrineHistoryEntry
                            {
                                doctrineId = saved.doctrineHistory[i].doctrineId,
                                day = saved.doctrineHistory[i].day,
                                reason = saved.doctrineHistory[i].reason
                            });
                }

                if (saved.territory != null)
                {
                    _state.territory = new List<WarlordTerritoryRecord>();
                    for (int i = 0; i < saved.territory.Count; i++)
                        if (saved.territory[i] != null)
                            _state.territory.Add(CloneTerritory(saved.territory[i]));
                }

                if (saved.reports != null)
                {
                    _state.reports = new List<WarlordReport>();
                    for (int i = 0; i < saved.reports.Count; i++)
                        if (saved.reports[i] != null)
                            _state.reports.Add(new WarlordReport
                            {
                                locationId = saved.reports[i].locationId,
                                state = saved.reports[i].state,
                                reportDay = saved.reports[i].reportDay,
                                confidence = saved.reports[i].confidence
                            });
                }

                if (saved.narrativeMarkers != null)
                    _state.narrativeMarkers = new List<string>(saved.narrativeMarkers);
            }

            // Missing-state defaults: a save without a territory list gets the
            // catalog default (home controlled, rest unclaimed).
            if (_state.territory == null || _state.territory.Count == 0)
            {
                ApplyCatalogDefaults();
            }
            RaiseChanged();
        }

        private static WarlordTerritoryRecord CloneTerritory(WarlordTerritoryRecord src)
        {
            return new WarlordTerritoryRecord
            {
                locationId = src.locationId,
                state = src.state,
                sinceDay = src.sinceDay,
                lastOutcomeDay = src.lastOutcomeDay,
                lastOutcomeSuccess = src.lastOutcomeSuccess,
                claimAttempts = src.claimAttempts
            };
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
