using System;
using System.Collections.Generic;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using UnityEngine;
using Random = System.Random;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #17 — Radio Interception / Wiretapping: latent inter-faction raid
    /// plans. With an operational high-tier radio/antenna the player hears the
    /// traffic and may warn the target, wait to scavenge the battlefield, or
    /// do nothing. Without antenna the plan resolves silently.
    /// </summary>
    public class FactionRaidPlanSystem
    {
        public const int MinDayForPlans = 30;
        public const int PlanLeadDays = 2;
        public const float DailyScheduleChance = 0.22f;

        public const float WarnTargetTrustDelta = 18f;
        public const float AttackerAngerTrustDelta = -25f;

        /// <summary>Ambient rad floor on the battlefield loot node.</summary>
        public const float BattlefieldRadFloor = 90f;

        public const int BattlefieldWeaponLoot = 1;
        public const int BattlefieldScrapLoot = 3;
        public const string BattlefieldWeaponItemId = "service_rifle";
        public const string BattlefieldScrapItemId = "scrap_metal";

        public const string ChoiceDoNothing = "do_nothing";
        public const string ChoiceWarnTarget = "warn_target";
        public const string ChoiceScavenge = "scavenge_battlefield";

        public const string EventIdPrefix = "evt_raid_plan_intercept_";

        private readonly Random _rng;
        private readonly List<FactionRaidPlan> _plans = new List<FactionRaidPlan>();
        private int _seq;

        private DynamicEconomySystem _economy;
        private FactionRadioInterceptSystem _intercepts;
        private Func<int> _getDay;
        private Func<bool> _isAntennaOperational;
        private GeneratedMap _map;
        private RadiationSystem _radiation;

        public IReadOnlyList<FactionRaidPlan> Plans => _plans;

        /// <summary>Newest pending plan awaiting player choice (if any).</summary>
        public FactionRaidPlan ActiveInterceptPlan
        {
            get
            {
                for (int i = 0; i < _plans.Count; i++)
                {
                    var p = _plans[i];
                    if (p != null && p.InterceptPresented && !p.Resolved
                        && string.IsNullOrEmpty(p.ChoiceId))
                        return p;
                }
                return null;
            }
        }

        public event Action<FactionRaidPlan> OnPlanScheduled;
        public event Action<FactionRaidPlan, GameEvent> OnInterceptOffered;
        public event Action<FactionRaidPlan> OnPlanResolved;
        public event Action OnStateChanged;

        public FactionRaidPlanSystem(Random rng = null)
        {
            _rng = rng ?? new Random(17);
        }

        public void Bind(
            DynamicEconomySystem economy,
            FactionRadioInterceptSystem intercepts = null,
            Func<int> getDay = null,
            Func<bool> isAntennaOperational = null,
            GeneratedMap map = null,
            RadiationSystem radiation = null)
        {
            _economy = economy;
            _intercepts = intercepts;
            _getDay = getDay ?? (() => 0);
            _isAntennaOperational = isAntennaOperational;
            _map = map;
            if (radiation != null) _radiation = radiation;
        }

        public void BindRadiation(RadiationSystem radiation) => _radiation = radiation;

        public void SetMap(GeneratedMap map) => _map = map;

        /// <summary>
        /// True when the high-tier radio/antenna can hear inter-faction traffic.
        /// Unset provider = not operational (silent world).
        /// </summary>
        public bool IsAntennaOperational()
        {
            return _isAntennaOperational != null && _isAntennaOperational();
        }

        public int CurrentDay => _getDay != null ? _getDay() : 0;

        /// <summary>
        /// Daily roll: may schedule a new latent A→B raid plan post-Day-30.
        /// Also resolves plans whose fire day has arrived.
        /// </summary>
        public void TickDay(int day)
        {
            TryAutoSchedule(day);
            ResolveDuePlans(day);
        }

        /// <summary>
        /// Force-schedule a plan (tests / scripted beats). Returns null if ids invalid
        /// or attacker == target.
        /// </summary>
        public FactionRaidPlan SchedulePlan(
            string attackerFactionId,
            string targetFactionId,
            int scheduleDay = -1,
            int leadDays = -1)
        {
            if (string.IsNullOrEmpty(attackerFactionId) || string.IsNullOrEmpty(targetFactionId))
                return null;
            if (string.Equals(attackerFactionId, targetFactionId, StringComparison.Ordinal))
                return null;

            int day = scheduleDay >= 0 ? scheduleDay : CurrentDay;
            int lead = leadDays >= 0 ? leadDays : PlanLeadDays;

            var plan = new FactionRaidPlan
            {
                Id = $"raid_plan_{++_seq}",
                AttackerFactionId = attackerFactionId,
                TargetFactionId = targetFactionId,
                ScheduledDay = day,
                FireDay = day + Mathf.Max(0, lead),
                Resolved = false,
                InterceptPresented = false,
                PlanSucceeded = false,
                ChoiceId = string.Empty,
                BattlefieldNodeId = string.Empty,
                BattlefieldLootReady = false,
                BattlefieldLootClaimed = false,
                PreviousNodeRad = -1f
            };
            _plans.Add(plan);
            OnPlanScheduled?.Invoke(plan);
            OnStateChanged?.Invoke();

            TryPresentIntercept(plan);
            return plan;
        }

        /// <summary>
        /// Build the player-facing intercept event (3 choices). Null if plan missing.
        /// </summary>
        public GameEvent CreateInterceptEvent(FactionRaidPlan plan)
        {
            if (plan == null) return null;

            string attacker = DisplayName(plan.AttackerFactionId);
            string target = DisplayName(plan.TargetFactionId);
            string channel = DynamicEconomySystem.GetParleyChannelTag(plan.AttackerFactionId);
            int daysOut = Mathf.Max(0, plan.FireDay - CurrentDay);

            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EventIdPrefix + plan.Id;
            ev.title = "Wiretap — raid traffic";
            ev.bodyText =
                $"Cross-band chatter on {channel}. {attacker} is talking about hitting {target} " +
                $"in {daysOut} day{(daysOut == 1 ? "" : "s")}. " +
                "You can warn them, wait for the smoke and pick the dead, or leave the air alone.";
            ev.weight = 1f;
            ev.conditions = new EventConditions { MinDay = 1 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = ChoiceDoNothing,
                    Text = "Do nothing. Let the static go.",
                    MoraleDelta = -1f,
                    FactionId = plan.AttackerFactionId
                },
                new EventChoice
                {
                    ChoiceId = ChoiceWarnTarget,
                    Text = $"Warn {target}. Burn the channel.",
                    MoraleDelta = 2f,
                    FactionId = plan.TargetFactionId
                    // Trust applied once in ApplyChoice (avoid double with EventRunner).
                },
                new EventChoice
                {
                    ChoiceId = ChoiceScavenge,
                    Text = "Wait. Scavenge the battlefield after.",
                    MoraleDelta = -2f,
                    FactionId = plan.AttackerFactionId
                }
            };
            return ev;
        }

        /// <summary>
        /// Apply a player choice to a plan. Safe to call before FireDay.
        /// Trust effects apply immediately; battlefield spawns on scavenge;
        /// do-nothing waits for <see cref="ResolveDuePlans"/>.
        /// </summary>
        public bool ApplyChoice(string planId, string choiceId)
        {
            var plan = FindPlan(planId);
            if (plan == null || plan.Resolved) return false;
            if (string.IsNullOrEmpty(choiceId)) return false;

            plan.ChoiceId = choiceId;

            if (string.Equals(choiceId, ChoiceWarnTarget, StringComparison.Ordinal))
            {
                if (_economy != null)
                {
                    _economy.ModifyTrust(plan.TargetFactionId, WarnTargetTrustDelta);
                    _economy.ModifyTrust(plan.AttackerFactionId, AttackerAngerTrustDelta);
                }
                plan.Resolved = true;
                plan.PlanSucceeded = false;
                plan.PlayerWarned = true;
                PushOutcomeLog(plan,
                    $"You burn the channel. {DisplayName(plan.TargetFactionId)} gets the warning. " +
                    $"{DisplayName(plan.AttackerFactionId)} goes cold on your frequency.");
                OnPlanResolved?.Invoke(plan);
                OnStateChanged?.Invoke();
                return true;
            }

            if (string.Equals(choiceId, ChoiceScavenge, StringComparison.Ordinal))
            {
                plan.PlayerScavenged = true;
                // Battlefield opens when the fight would happen (or immediately if already due).
                if (CurrentDay >= plan.FireDay)
                    ResolveScavenge(plan);
                else
                {
                    // Mark intent; ResolveDuePlans finishes it.
                    OnStateChanged?.Invoke();
                }
                return true;
            }

            if (string.Equals(choiceId, ChoiceDoNothing, StringComparison.Ordinal))
            {
                if (CurrentDay >= plan.FireDay)
                    ResolveDoNothing(plan);
                else
                    OnStateChanged?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>Apply choice from an EventChoice.ChoiceId.</summary>
        public bool ApplyChoiceFromEvent(GameEvent gameEvent, EventChoice choice)
        {
            if (choice == null || gameEvent == null) return false;
            string planId = ExtractPlanIdFromEventId(gameEvent.id);
            if (string.IsNullOrEmpty(planId)) return false;
            return ApplyChoice(planId, choice.ChoiceId);
        }

        public FactionRaidPlan FindPlan(string planId)
        {
            if (string.IsNullOrEmpty(planId)) return null;
            for (int i = 0; i < _plans.Count; i++)
            {
                if (_plans[i] != null && _plans[i].Id == planId)
                    return _plans[i];
            }
            return null;
        }

        /// <summary>
        /// Claim battlefield loot at a node if a scavenge plan left gear there.
        /// Applies residual rad dose to the scavenger.
        /// </summary>
        public bool TryClaimBattlefieldLoot(
            string nodeId,
            Survivor scavenger,
            Inventory.Inventory inventory,
            ItemDefinition weaponDef = null,
            ItemDefinition scrapDef = null,
            float radDoseOnClaim = 12f)
        {
            if (string.IsNullOrEmpty(nodeId) || inventory == null) return false;

            FactionRaidPlan plan = null;
            for (int i = 0; i < _plans.Count; i++)
            {
                var p = _plans[i];
                if (p == null || !p.BattlefieldLootReady || p.BattlefieldLootClaimed) continue;
                if (string.Equals(p.BattlefieldNodeId, nodeId, StringComparison.Ordinal))
                {
                    plan = p;
                    break;
                }
            }
            if (plan == null) return false;

            weaponDef = weaponDef ?? CreateDefaultWeaponDef();
            scrapDef = scrapDef ?? CreateDefaultScrapDef();

            if (weaponDef != null)
                inventory.Add(weaponDef, BattlefieldWeaponLoot);
            if (scrapDef != null)
                inventory.Add(scrapDef, BattlefieldScrapLoot);

            if (scavenger != null && scavenger.IsAlive && radDoseOnClaim > 0f)
            {
                // MISC-007 — scavenger battlefield spike only through RadiationSystem.
                // Callers (bootstrap Bind + EditMode tests) must BindRadiation first;
                // no direct RadiationDose write when unbound (dose is simply skipped).
                if (_radiation != null)
                    _radiation.Expose(scavenger, radDoseOnClaim, 1f);
            }

            plan.BattlefieldLootClaimed = true;
            plan.BattlefieldLootReady = false;
            OnStateChanged?.Invoke();
            return true;
        }

        public bool HasBattlefieldLootAt(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            for (int i = 0; i < _plans.Count; i++)
            {
                var p = _plans[i];
                if (p != null && p.BattlefieldLootReady && !p.BattlefieldLootClaimed
                    && string.Equals(p.BattlefieldNodeId, nodeId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public FactionRaidPlanSave CaptureState()
        {
            var arr = new FactionRaidPlanSaveEntry[_plans.Count];
            for (int i = 0; i < _plans.Count; i++)
            {
                var p = _plans[i];
                if (p == null) continue;
                arr[i] = new FactionRaidPlanSaveEntry
                {
                    Id = p.Id,
                    AttackerFactionId = p.AttackerFactionId,
                    TargetFactionId = p.TargetFactionId,
                    ScheduledDay = p.ScheduledDay,
                    FireDay = p.FireDay,
                    Resolved = p.Resolved,
                    InterceptPresented = p.InterceptPresented,
                    PlanSucceeded = p.PlanSucceeded,
                    ChoiceId = p.ChoiceId ?? string.Empty,
                    BattlefieldNodeId = p.BattlefieldNodeId ?? string.Empty,
                    BattlefieldLootReady = p.BattlefieldLootReady,
                    BattlefieldLootClaimed = p.BattlefieldLootClaimed,
                    PreviousNodeRad = p.PreviousNodeRad,
                    PlayerWarned = p.PlayerWarned,
                    PlayerScavenged = p.PlayerScavenged
                };
            }
            return new FactionRaidPlanSave { NextSeq = _seq, Plans = arr };
        }

        public void RestoreState(FactionRaidPlanSave save)
        {
            _plans.Clear();
            _seq = 0;
            if (save == null) return;
            _seq = Math.Max(0, save.NextSeq);
            if (save.Plans == null) return;
            for (int i = 0; i < save.Plans.Length; i++)
            {
                var e = save.Plans[i];
                if (e == null || string.IsNullOrEmpty(e.Id)) continue;
                _plans.Add(new FactionRaidPlan
                {
                    Id = e.Id,
                    AttackerFactionId = e.AttackerFactionId ?? string.Empty,
                    TargetFactionId = e.TargetFactionId ?? string.Empty,
                    ScheduledDay = e.ScheduledDay,
                    FireDay = e.FireDay,
                    Resolved = e.Resolved,
                    InterceptPresented = e.InterceptPresented,
                    PlanSucceeded = e.PlanSucceeded,
                    ChoiceId = e.ChoiceId ?? string.Empty,
                    BattlefieldNodeId = e.BattlefieldNodeId ?? string.Empty,
                    BattlefieldLootReady = e.BattlefieldLootReady,
                    BattlefieldLootClaimed = e.BattlefieldLootClaimed,
                    PreviousNodeRad = e.PreviousNodeRad,
                    PlayerWarned = e.PlayerWarned,
                    PlayerScavenged = e.PlayerScavenged
                });
            }
        }

        public void Clear()
        {
            _plans.Clear();
            _seq = 0;
        }

        // -----------------------------------------------------------------
        // Internals
        // -----------------------------------------------------------------

        private void TryAutoSchedule(int day)
        {
            if (day < MinDayForPlans) return;
            if (_economy == null || _economy.Factions == null || _economy.Factions.Count < 2)
                return;

            // Only one open intercept/pending plan at a time.
            for (int i = 0; i < _plans.Count; i++)
            {
                var p = _plans[i];
                if (p != null && !p.Resolved) return;
            }

            if (_rng.NextDouble() > DailyScheduleChance) return;

            var ids = new List<string>();
            foreach (var kv in _economy.Factions)
            {
                if (kv.Value == null || string.IsNullOrEmpty(kv.Key)) continue;
                if (!_economy.IsFactionActive(kv.Key)) continue;
                ids.Add(kv.Key);
            }
            if (ids.Count < 2) return;

            int a = _rng.Next(0, ids.Count);
            int b = _rng.Next(0, ids.Count - 1);
            if (b >= a) b++;
            SchedulePlan(ids[a], ids[b], day, PlanLeadDays);
        }

        private void ResolveDuePlans(int day)
        {
            for (int i = 0; i < _plans.Count; i++)
            {
                var plan = _plans[i];
                if (plan == null || plan.Resolved) continue;
                if (day < plan.FireDay) continue;

                if (plan.PlayerScavenged
                    || string.Equals(plan.ChoiceId, ChoiceScavenge, StringComparison.Ordinal))
                {
                    ResolveScavenge(plan);
                    continue;
                }

                // Default / do_nothing: the raid happens off-screen.
                ResolveDoNothing(plan);
            }
        }

        private void TryPresentIntercept(FactionRaidPlan plan)
        {
            if (plan == null || plan.InterceptPresented || plan.Resolved) return;
            if (!IsAntennaOperational()) return;

            plan.InterceptPresented = true;

            string attacker = DisplayName(plan.AttackerFactionId);
            string target = DisplayName(plan.TargetFactionId);
            string channel = DynamicEconomySystem.GetParleyChannelTag(plan.AttackerFactionId);
            int daysOut = Mathf.Max(0, plan.FireDay - CurrentDay);

            _intercepts?.Push(
                plan.AttackerFactionId,
                FactionRadioInterceptSystem.InterceptKind.RaidPlan,
                $"Wiretap. {channel}. {attacker} traffic about hitting {target} — {daysOut} day{(daysOut == 1 ? "" : "s")} out.",
                CurrentDay);

            var ev = CreateInterceptEvent(plan);
            OnInterceptOffered?.Invoke(plan, ev);
            OnStateChanged?.Invoke();
        }

        private void ResolveDoNothing(FactionRaidPlan plan)
        {
            if (plan == null || plan.Resolved) return;
            plan.Resolved = true;
            plan.PlanSucceeded = true;
            if (string.IsNullOrEmpty(plan.ChoiceId))
                plan.ChoiceId = ChoiceDoNothing;

            // Quiet outcome log only if they had ears on the band.
            if (plan.InterceptPresented)
            {
                PushOutcomeLog(plan,
                    $"Traffic dies. {DisplayName(plan.AttackerFactionId)} hit {DisplayName(plan.TargetFactionId)}. " +
                    "No one on your frequency asks for help.");
            }

            OnPlanResolved?.Invoke(plan);
            OnStateChanged?.Invoke();
        }

        private void ResolveScavenge(FactionRaidPlan plan)
        {
            if (plan == null || plan.Resolved) return;
            plan.Resolved = true;
            plan.PlanSucceeded = true;
            plan.PlayerScavenged = true;
            plan.ChoiceId = ChoiceScavenge;

            SpawnBattlefield(plan);

            PushOutcomeLog(plan,
                $"Smoke on the ring road. {DisplayName(plan.AttackerFactionId)} left bodies and brass. " +
                "The ground still ticks. Loot if you can stand the dose.");

            OnPlanResolved?.Invoke(plan);
            OnStateChanged?.Invoke();
        }

        private void SpawnBattlefield(FactionRaidPlan plan)
        {
            if (plan == null) return;

            string nodeId = PickBattlefieldNode();
            plan.BattlefieldNodeId = nodeId ?? string.Empty;
            plan.BattlefieldLootReady = !string.IsNullOrEmpty(nodeId);
            plan.BattlefieldLootClaimed = false;

            if (_map == null || string.IsNullOrEmpty(nodeId)) return;
            var node = _map.GetNode(nodeId);
            if (node == null) return;

            plan.PreviousNodeRad = node.TrueRad;
            if (node.TrueRad < BattlefieldRadFloor)
                node.TrueRad = BattlefieldRadFloor;
            node.IsRevealed = true;
        }

        private string PickBattlefieldNode()
        {
            if (_map?.Nodes == null || _map.Nodes.Count == 0) return string.Empty;

            // Prefer non-shelter, non-death-zone ring road nodes.
            var candidates = new List<MapNode>();
            for (int i = 0; i < _map.Nodes.Count; i++)
            {
                var n = _map.Nodes[i];
                if (n == null || n.IsShelter || n.IsDeathZone) continue;
                candidates.Add(n);
            }
            if (candidates.Count == 0)
            {
                for (int i = 0; i < _map.Nodes.Count; i++)
                {
                    var n = _map.Nodes[i];
                    if (n != null && !n.IsShelter) candidates.Add(n);
                }
            }
            if (candidates.Count == 0) return string.Empty;
            return candidates[_rng.Next(0, candidates.Count)].NodeId;
        }

        private void PushOutcomeLog(FactionRaidPlan plan, string message)
        {
            if (_intercepts == null || string.IsNullOrEmpty(message)) return;
            _intercepts.Push(
                plan?.AttackerFactionId,
                FactionRadioInterceptSystem.InterceptKind.RaidPlan,
                message,
                CurrentDay);
        }

        private string DisplayName(string factionId)
        {
            if (_economy == null || string.IsNullOrEmpty(factionId)) return "someone";
            var fac = _economy.GetFaction(factionId);
            return fac != null && !string.IsNullOrEmpty(fac.displayName) ? fac.displayName : factionId;
        }

        public static string ExtractPlanIdFromEventId(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return string.Empty;
            if (!eventId.StartsWith(EventIdPrefix, StringComparison.Ordinal)) return string.Empty;
            return eventId.Substring(EventIdPrefix.Length);
        }

        public static ItemDefinition CreateDefaultWeaponDef()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = BattlefieldWeaponItemId;
            item.displayName = "Service Rifle";
            item.description = "Warm barrel. Someone else paid for it.";
            item.type = ItemType.Weapon;
            item.stackMax = 1;
            item.weight = 3.5f;
            item.tradeValue = 45f;
            return item;
        }

        public static ItemDefinition CreateDefaultScrapDef()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = BattlefieldScrapItemId;
            item.displayName = "Scrap Metal";
            item.type = ItemType.Material;
            item.stackMax = 50;
            item.weight = 0.5f;
            item.tradeValue = 2f;
            return item;
        }
    }

    [Serializable]
    public class FactionRaidPlan
    {
        public string Id;
        public string AttackerFactionId;
        public string TargetFactionId;
        public int ScheduledDay;
        public int FireDay;
        public bool Resolved;
        public bool InterceptPresented;
        public bool PlanSucceeded;
        public string ChoiceId;
        public string BattlefieldNodeId;
        public bool BattlefieldLootReady;
        public bool BattlefieldLootClaimed;
        public float PreviousNodeRad;
        public bool PlayerWarned;
        public bool PlayerScavenged;
    }

    [Serializable]
    public class FactionRaidPlanSave
    {
        public int NextSeq;
        public FactionRaidPlanSaveEntry[] Plans;
    }

    [Serializable]
    public class FactionRaidPlanSaveEntry
    {
        public string Id;
        public string AttackerFactionId;
        public string TargetFactionId;
        public int ScheduledDay;
        public int FireDay;
        public bool Resolved;
        public bool InterceptPresented;
        public bool PlanSucceeded;
        public string ChoiceId;
        public string BattlefieldNodeId;
        public bool BattlefieldLootReady;
        public bool BattlefieldLootClaimed;
        public float PreviousNodeRad;
        public bool PlayerWarned;
        public bool PlayerScavenged;
    }
}
