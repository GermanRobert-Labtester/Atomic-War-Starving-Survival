using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    // =====================================================================
    // BUNKER SOCIAL SYSTEMS (Prompts #469-#478)
    //
    // A cohesive family of interpersonal / leadership systems that all read
    // and write the SAME <see cref="InterpersonalAffinity"/> matrix so the
    // relationships stay in one decision space:
    //
    //   #469  RomanceSystem         Lovers bond + Hope aura + grief mechanic
    //   #470  RomanceSystem.Breakup Lovers < 50 affinity break apart
    //   #475  FeudSystem            Affinity < -50 → passive sabotage
    //   #471  MutinySystem          avg morale < 20 a week → leadership challenge
    //   #472  ImprisonmentSystem    convert room to a Cell, lock survivors up
    //   #473  BanishmentSystem      kick a survivor out the airlock
    //   #474  BanishedReturnSystem  banished survivor returns as a Raider Boss
    //   #476  PregnancySystem       Lovers → child after a draining term
    //   #477  TribunalSystem        crime → trial; punishment must fit crime
    //   #478  BlackMarketSystem     high-affinity/low-morale pairs smuggle
    //
    // Design constraints honored here:
    //   * Plain C# — no MonoBehaviour, no UI, no direct Inventory/Shelter/Core
    //     references (the Survivors asmdef is the leaf here). Everything that
    //     touches inventory, shelter modules, or hatch raids goes through an
    //     injected hook that Core wires up (the same pattern MentalBreakSystem
    //     uses for BingeEatHandler / SabotageHandler).
    //   * Save/load safe — every mutable relationship is a dictionary owned
    //     by a system; CaptureState() flattens it to a List-based [Serializable]
    //     DTO and RestoreState() rebuilds the dictionary. The aggregator
    //     (BunkerSocialDirector in Core) implements ISaveable so the whole
    //     family persists in one SaveId.
    //   * Event-driven — each system raises Action events on state change so
    //     the UI / journal can subscribe without holding a direct reference.
    // =====================================================================

    /// <summary>Shared severity scale for Tribunal crimes + matching punishments.</summary>
    public enum BunkerCrimeSeverity { Minor, Moderate, Severe }

    /// <summary>Juridical punishments the player (judge) can hand down.</summary>
    public enum BunkerPunishment { RationCut, Banishment, Execution }

    /// <summary>Whether a punishment "matches" the crime severity (Tribunal #477).</summary>
    public enum PunishmentMatch { Appropriate, Lenient, Excessive }

    /// <summary>An unordered pair of survivor ids (a,b) == (b,a). Shared by the
    /// bond/smuggling systems so they keep one equality contract.</summary>
    public struct SocialPairKey : IEquatable<SocialPairKey>
    {
        public readonly string A;
        public readonly string B;
        public SocialPairKey(string a, string b)
        {
            if (string.CompareOrdinal(a, b) <= 0) { A = a; B = b; }
            else { A = b; B = a; }
        }
        public bool Equals(SocialPairKey other) =>
            string.Equals(A, other.A, StringComparison.Ordinal)
            && string.Equals(B, other.B, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is SocialPairKey k && Equals(k);
        public override int GetHashCode() =>
            ((A ?? "").GetHashCode() * 397) ^ (B ?? "").GetHashCode();
    }


    /// <summary>Modes the player can use to end a mutiny (#471).</summary>
    public enum MutinyResolution { Negotiate, YieldResources, Execute }

    // =====================================================================
    // #469 LOVERS + #470 BREAKUP
    // =====================================================================

    /// <summary>
    /// #469/#470 — Romance bond on the affinity matrix.
    ///
    /// Two living survivors who sit above <see cref="LoversAffinityThreshold"/>
    /// AND share a sleeping space become Lovers. While lovers they broadcast a
    /// shared Hope aura (mutual morale bump) and regenerate Fatigue faster.
    /// When one lover takes damage the other takes an instant Anxiety hit; when
    /// one dies the other instantly suffers a Catatonic or Suicide mental break.
    ///
    /// If the pair's affinity ever drops below <see cref="BreakupAffinityThreshold"/>,
    /// they break up (#470). The breakup leaves a permanent Awkward/Hostile aura
    /// when they share a room and makes them refuse cooperative tasks together.
    /// </summary>
    public class RomanceSystem
    {
        public const float LoversAffinityThreshold = 90f;
        public const float BreakupAffinityThreshold = 50f;

        /// <summary>Hope aura per hour while both lovers are home (mutual morale).</summary>
        public const float LoversHopeAuraPerHour = 3f;

        /// <summary>Fractional bonus to fatigue recovery while lovers rest nearby.</summary>
        public const float LoversFatigueRegenBonus = 0.5f;

        /// <summary>Instant Anxiety morale hit when a lover takes damage.</summary>
        public const float LoverDamageAnxietyMoraleHit = 25f;

        /// <summary>Per-hour morale drain while a broken-up pair shares a room.</summary>
        public const float BreakupAuraDrainPerHour = 8f;

        public const string GriefCatatonicBreakId = "catatonic";
        public const string GriefSuicideBreakId = "suicide";

        public event Action<Survivor, Survivor> OnBecomeLovers;
        public event Action<Survivor, Survivor> OnBreakup;
        public event Action<Survivor, Survivor> OnLoverAnxietyHit;
        public event Action<Survivor, string> OnLoverGriefBreak;

        private readonly InterpersonalAffinity _affinity;
        private readonly Dictionary<SocialPairKey, LoverEntry> _lovers = new Dictionary<SocialPairKey, LoverEntry>();
        private readonly HashSet<SocialPairKey> _brokenUp = new HashSet<SocialPairKey>();

        // #470 cooperative-task refusal after a breakup.
        public Func<string, string, bool> RefuseCooperationCheck;

        /// <summary>
        /// Whether two survivors currently share a sleeping space. Injected by Core
        /// to inspect bed/bunk module assignment; defaults to "same room or both
        /// unassigned (common quarters)". Returns true when they may bond.
        /// </summary>
        public Func<Survivor, Survivor, bool> ShareSleepingSpace;

        public RomanceSystem(InterpersonalAffinity affinity)
        {
            _affinity = affinity ?? new InterpersonalAffinity();
            ShareSleepingSpace = (a, b) => string.IsNullOrEmpty(a.CurrentRoomId)
                && string.IsNullOrEmpty(b.CurrentRoomId);
        }

        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        // -----------------------------------------------------------------
        // State transitions (call once per day)
        // -----------------------------------------------------------------

        /// <summary>
        /// Scan all living pairs and form / dissolve lover bonds based on the
        /// affinity matrix. New lovers need affinity &gt; threshold and a shared
        /// sleeping space; existing lovers break up below the threshold.
        /// </summary>
        public void UpdateBondStates(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            List<Survivor> alive = new List<Survivor>(survivors.Count);
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && survivors[i].IsAlive) alive.Add(survivors[i]);

            for (int i = 0; i < alive.Count; i++)
            {
                for (int j = i + 1; j < alive.Count; j++)
                {
                    var a = alive[i];
                    var b = alive[j];
                    var key = new SocialPairKey(a.Id, b.Id);
                    float aff = _affinity.Get(a.Id, b.Id);
                    bool isLover = _lovers.ContainsKey(key);
                    bool wasBroken = _brokenUp.Contains(key);

                    if (isLover)
                    {
                        if (aff < BreakupAffinityThreshold)
                        {
                            _lovers.Remove(key);
                            _brokenUp.Add(key); // permanent aura
                            OnBreakup?.Invoke(a, b);
                        }
                    }
                    else if (!wasBroken && aff > LoversAffinityThreshold)
                    {
                        if (ShareSleepingSpace != null && ShareSleepingSpace(a, b))
                        {
                            _lovers[key] = new LoverEntry { A = a.Id, B = b.Id };
                            OnBecomeLovers?.Invoke(a, b);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply the persistent lover effects: mutual Hope aura (both gain morale)
        /// and faster fatigue recovery. Breakup pairs instead take the Awkward/Hostile
        /// aura drain when sharing a room. Fatigue bonus is surfaced via
        /// <see cref="GetFatigueRecoveryMultiplier(Survivor)"/>.
        /// </summary>
        public void ApplyAuras(float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null) return;

            foreach (var pair in _lovers.Values)
            {
                var a = Find(survivors, pair.A);
                var b = Find(survivors, pair.B);
                if (a == null || b == null || !a.IsAlive || !b.IsAlive) continue;
                if (_needsSystem != null)

                    _needsSystem.Modify(a, NeedKind.Morale, LoversHopeAuraPerHour * gameHours);

                else

                    a.Needs.Morale = Mathf.Clamp(a.Needs.Morale + LoversHopeAuraPerHour * gameHours, 0f, 100f);
                if (_needsSystem != null)

                    _needsSystem.Modify(b, NeedKind.Morale, LoversHopeAuraPerHour * gameHours);

                else

                    b.Needs.Morale = Mathf.Clamp(b.Needs.Morale + LoversHopeAuraPerHour * gameHours, 0f, 100f);
            }

            // Broken-up pairs suffer the aura when sharing a room.
            foreach (var key in _brokenUp)
            {
                var a = Find(survivors, key.A);
                var b = Find(survivors, key.B);
                if (a == null || b == null || !a.IsAlive || !b.IsAlive) continue;
                if (string.Equals(a.CurrentRoomId, b.CurrentRoomId, StringComparison.Ordinal))
                {
                    if (_needsSystem != null)

                        _needsSystem.Modify(a, NeedKind.Morale, -(BreakupAuraDrainPerHour * gameHours));

                    else

                        a.Needs.Morale = Mathf.Max(0f, a.Needs.Morale - BreakupAuraDrainPerHour * gameHours);
                    if (_needsSystem != null)

                        _needsSystem.Modify(b, NeedKind.Morale, -(BreakupAuraDrainPerHour * gameHours));

                    else

                        b.Needs.Morale = Mathf.Max(0f, b.Needs.Morale - BreakupAuraDrainPerHour * gameHours);
                }
            }
        }

        /// <summary>Fatigue recovery multiplier: 1.5 while the lover is home &amp; alive.</summary>
        public float GetFatigueRecoveryMultiplier(Survivor sv)
        {
            if (sv == null) return 1f;
            var partnerId = GetLoverOf(sv.Id);
            if (partnerId == null) return 1f;
            return 1f + LoversFatigueRegenBonus;
        }

        /// <summary>The other lover of <paramref name="damaged"/> in the given roster.</summary>
        public Survivor GetAnxietyTarget(Survivor damaged, IReadOnlyList<Survivor> survivors)
        {
            if (damaged == null || survivors == null) return null;
            var partnerId = GetLoverOf(damaged.Id);
            if (partnerId == null) return null;
            return Find(survivors, partnerId);
        }

        /// <summary>
        /// #469 — one lover took damage; the other instantly suffers Anxiety.
        /// Resolves the partner and applies the morale hit. Returns the partner
        /// who was hit, or null.
        /// </summary>
        public Survivor ApplyLoverDamageAnxiety(Survivor damaged, IReadOnlyList<Survivor> survivors)
        {
            var partner = GetAnxietyTarget(damaged, survivors);
            if (partner == null) return null;
            if (_needsSystem != null)

                _needsSystem.Modify(partner, NeedKind.Morale, -(LoverDamageAnxietyMoraleHit));

            else

                partner.Needs.Morale = Mathf.Max(0f, partner.Needs.Morale - LoverDamageAnxietyMoraleHit);
            OnLoverAnxietyHit?.Invoke(damaged, partner);
            return partner;
        }

        /// <summary>
        /// #469 — a lover died. Bereaved partner instantly suffers a Catatonic or
        /// Suicide mental break and their morale collapses.
        /// </summary>
        public void NotifyLoverDied(Survivor dead, System.Random rng)
        {
            if (dead == null) return;
            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.Stream("bunkersocialsystems");
            string bereavedId = null;
            SocialPairKey? deadKey = null;
            foreach (var kv in _lovers)
            {
                if (string.Equals(kv.Value.A, dead.Id, StringComparison.Ordinal)
                    || string.Equals(kv.Value.B, dead.Id, StringComparison.Ordinal))
                {
                    bereavedId = string.Equals(kv.Value.A, dead.Id, StringComparison.Ordinal) ? kv.Value.B : kv.Value.A;
                    deadKey = kv.Key;
                    break;
                }
            }
            if (bereavedId == null) return;

            _lovers.Remove(deadKey.Value);
            // The bereaved is expected in `survivors`; we raise the event and let
            // Core apply the break (it owns the survivor lookup). We keep the id
            // exposed for tests/host.
            _pendingGriefTarget = bereavedId;
            string breakId = rng.NextDouble() < 0.5 ? GriefCatatonicBreakId : GriefSuicideBreakId;
            _pendingGriefBreakId = breakId;
            OnLoverGriefBreak?.Invoke(null, breakId); // host resolves the survivor by id
        }
        private string _pendingGriefTarget;
        private string _pendingGriefBreakId;

        /// <summary>Id of the survivor awaiting a grief mental break (post-fatal call).</summary>
        public string PendingGriefBereavedId => _pendingGriefTarget;

        /// <summary>The grief break id chosen for the pending bereaved survivor (or null).</summary>
        public string PendingGriefBreakId => _pendingGriefBreakId;

        public void ClearPendingGrief()
        {
            _pendingGriefTarget = null;
            _pendingGriefBreakId = null;
        }

        // -----------------------------------------------------------------
        // Queries
        // -----------------------------------------------------------------

        public string GetLoverOf(string survivorId)
        {
            foreach (var kv in _lovers)
            {
                if (string.Equals(kv.Value.A, survivorId, StringComparison.Ordinal)) return kv.Value.B;
                if (string.Equals(kv.Value.B, survivorId, StringComparison.Ordinal)) return kv.Value.A;
            }
            return null;
        }

        public bool AreLovers(string a, string b) =>
            !string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b)
            && _lovers.ContainsKey(new SocialPairKey(a, b));

        /// <summary>Enumerate all active lover pairs as (a,b) id tuples.</summary>
        public List<(string A, string B)> GetLoverPairs()
        {
            var list = new List<(string A, string B)>();
            foreach (var entry in _lovers.Values)
                list.Add((entry.A, entry.B));
            return list;
        }

        public bool BreakupAuraActive(string a, string b) =>
            !string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b)
            && _brokenUp.Contains(new SocialPairKey(a, b));

        /// <summary>#470 — will two survivor/craft partners refuse to cooperate together?</summary>
        public bool RefusesCooperativeTask(string a, string b) => BreakupAuraActive(a, b);

        public int ActiveLoverCount => _lovers.Count;
        public int BrokenUpCount => _brokenUp.Count;

        // -----------------------------------------------------------------
        // Capture / Restore
        // -----------------------------------------------------------------

        public RomanceSave CaptureState()
        {
            var save = new RomanceSave();
            foreach (var kv in _lovers)
                save.Lovers.Add(new LoverSave { A = kv.Value.A, B = kv.Value.B });
            foreach (var key in _brokenUp)
                save.BrokenUp.Add(new LoverSave { A = key.A, B = key.B });
            save.PendingGriefBereavedId = _pendingGriefTarget;
            return save;
        }

        public void RestoreState(RomanceSave save)
        {
            _lovers.Clear();
            _brokenUp.Clear();
            _pendingGriefTarget = null;
            if (save == null) return;
            if (save.Lovers != null)
                foreach (var l in save.Lovers)
                    if (l != null && !string.IsNullOrEmpty(l.A) && !string.IsNullOrEmpty(l.B))
                        _lovers[new SocialPairKey(l.A, l.B)] = new LoverEntry { A = l.A, B = l.B };
            if (save.BrokenUp != null)
                foreach (var l in save.BrokenUp)
                    if (l != null && !string.IsNullOrEmpty(l.A) && !string.IsNullOrEmpty(l.B))
                        _brokenUp.Add(new SocialPairKey(l.A, l.B));
            _pendingGriefTarget = save.PendingGriefBereavedId;
        }

        private static Survivor Find(IReadOnlyList<Survivor> survivors, string id)
        {
            if (survivors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && string.Equals(survivors[i].Id, id, StringComparison.Ordinal))
                    return survivors[i];
            return null;
        }


        private sealed class LoverEntry
        {
            public string A;
            public string B;
        }
    }

    [Serializable]
    public class RomanceSave
    {
        public List<LoverSave> Lovers = new List<LoverSave>();
        public List<LoverSave> BrokenUp = new List<LoverSave>();
        public string PendingGriefBereavedId;
    }

    [Serializable]
    public class LoverSave
    {
        public string A;
        public string B;
    }

    // =====================================================================
    // #475 FEUDS (passive sabotage)
    // =====================================================================

    /// <summary>
    /// #475 — two survivors at affinity &lt; -50 enter a Feud. They will actively
    /// try to ruin each other's work: poisoning a meal the other cooked, hiding
    /// their tools, etc. Sabotage is a per-day chance; the actual inventory /
    /// module effect goes through <see cref="SabotageWorkHandler"/> so this
    /// assembly stays leaf-level.
    /// </summary>
    public class FeudSystem
    {
        public const float FeudAffinityThreshold = -50f;

        /// <summary>Base per-day chance a feud pair attempts to sabotage each other.</summary>
        public const float SabotageChancePerDay = 0.35f;

        public event Action<Survivor, Survivor> OnFeudStarted;
        public event Action<Survivor, Survivor, string> OnSabotageOccurred;

        private readonly InterpersonalAffinity _affinity;
        private readonly HashSet<SocialPairKey> _feuds = new HashSet<SocialPairKey>();

        /// <summary>
        /// Host hook: apply a concrete sabotaging side effect to the victim's
        /// recent work (e.g. add Contamination to a meal or hide a tool). The
        /// string kind is "meal_contamination" / "tool_hiding" / "toolbreak".
        /// Returns true if the sabotage landed.
        /// </summary>
        public Func<Survivor, Survivor, string, bool> SabotageWorkHandler;

        public FeudSystem(InterpersonalAffinity affinity)
        {
            _affinity = affinity ?? new InterpersonalAffinity();
        }

        /// <summary>Detect and open feuds from the affinity matrix (call once/day).</summary>
        public void UpdateFeuds(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            List<Survivor> alive = new List<Survivor>(survivors.Count);
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && survivors[i].IsAlive) alive.Add(survivors[i]);

            for (int i = 0; i < alive.Count; i++)
            {
                for (int j = i + 1; j < alive.Count; j++)
                {
                    var a = alive[i];
                    var b = alive[j];
                    var key = new SocialPairKey(a.Id, b.Id);
                    if (_feuds.Contains(key)) continue;
                    if (_affinity.Get(a.Id, b.Id) < FeudAffinityThreshold)
                    {
                        _feuds.Add(key);
                        OnFeudStarted?.Invoke(a, b);
                    }
                }
            }
        }

        /// <summary>
        /// Daily sabotage attempts between feuding, co-located pairs. Returns how
        /// many saboteur->victim hits actually landed.
        /// </summary>
        public int TickSabotage(float gameHours, IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            if (gameHours <= 0f || survivors == null || rng == null) return 0;
            int landed = 0;
            foreach (var feh in _feuds)
            {
                var a = Find(survivors, feh.A);
                var b = Find(survivors, feh.B);
                if (a == null || b == null || !a.IsAlive || !b.IsAlive) continue;
                if (!string.Equals(a.CurrentRoomId, b.CurrentRoomId, StringComparison.Ordinal)) continue;

                double roll = rng.NextDouble();
                if (roll < SabotageChancePerDay * gameHours)
                {
                    string kind = (rng.NextDouble() < 0.5) ? "meal_contamination" : "tool_hiding";
                    bool ok = SabotageWorkHandler == null || SabotageWorkHandler(a, b, kind);
                    if (ok)
                    {
                        landed++;
                        OnSabotageOccurred?.Invoke(a, b, kind);
                    }
                }
            }
            return landed;
        }

        public bool AreFeuding(string a, string b) =>
            !string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b)
            && _feuds.Contains(new SocialPairKey(a, b));

        public int ActiveFeudCount => _feuds.Count;

        public FeudSave CaptureState()
        {
            var save = new FeudSave();
            foreach (var f in _feuds)
                save.Feuds.Add(new LoverSave { A = f.A, B = f.B });
            return save;
        }

        public void RestoreState(FeudSave save)
        {
            _feuds.Clear();
            if (save?.Feuds == null) return;
            foreach (var f in save.Feuds)
                if (f != null && !string.IsNullOrEmpty(f.A) && !string.IsNullOrEmpty(f.B))
                    _feuds.Add(new SocialPairKey(f.A, f.B));
        }

        private static Survivor Find(IReadOnlyList<Survivor> survivors, string id)
        {
            if (survivors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && string.Equals(survivors[i].Id, id, StringComparison.Ordinal))
                    return survivors[i];
            return null;
        }
    }

    [Serializable]
    public class FeudSave
    {
        public List<LoverSave> Feuds = new List<LoverSave>();
    }

    // =====================================================================
    // #471 MUTINY & LEADERSHIP CHALLENGES
    // =====================================================================

    /// <summary>
    /// #471 — if the bunker's average Morale sits below 20 for a full week, the
    /// survivor with the highest leadership score (Charisma/Strength — provided
    /// by Core via <see cref="LeadershipScore"/>) challenges the player's control.
    /// The player loses direct UI control over the challenger and their followers
    /// until they negotiate, yield resources, or execute the leader.
    /// </summary>
    public class MutinySystem
    {
        public const float MutinyAverageMoraleThreshold = 20f;
        public const int MutinyWindowDays = 7;

        /// <summary>Morale hit applied to everyone when a mutiny resolves (divided for leader execution).</summary>
        public const float MutinyFalloutMoraleHit = 8f;

        public event Action<Survivor> OnMutinyStarted;
        public event Action<MutinyResolution> OnMutinyResolved;

        /// <summary>
        /// DEATH-001 hardened: fired when ResolveExecute kills the leader.
        /// Bootstrap wires this to the NeedsSystem death chain so the same
        /// NotifySurvivorDied / EmpathSystem / ChildSystem / GriefKeepsakes /
        /// <summary>How many days the bunker average has been below the threshold.</summary>
        public int LowMoraleStreakDays;

        public bool MutinyActive;
        public string LeaderId;
        public List<string> FollowerIds = new List<string>();

        private int _lastCheckedDay = -1;

        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        /// <summary>Leadership rank (Charisma/Strength proxy). Wired by Core; defaults to morale.</summary>
        public Func<Survivor, float> LeadershipScore;

        /// <summary>Publicly reachable set of rebel ids (leader + followers).</summary>
        public bool IsRebel(string id) => MutinyActive && !string.IsNullOrEmpty(id)
            && (string.Equals(id, LeaderId, StringComparison.Ordinal) || FollowerIds.Contains(id));

        public void TickWeekly(int day, IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            if (survivors == null) return;
            if (day == _lastCheckedDay) return;
            _lastCheckedDay = day;

            if (MutinyActive)
            {
                LowMoraleStreakDays = 0; // frozen while unresolved
                return;
            }

            // Live average morale.
            float sum = 0f; int n = 0;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || sv.IsChild) continue;
                sum += sv.Needs.Morale; n++;
            }
            float avg = n > 0 ? sum / n : 100f;

            if (avg < MutinyAverageMoraleThreshold)
            {
                LowMoraleStreakDays++;
                if (LowMoraleStreakDays >= MutinyWindowDays)
                    TriggerMutiny(survivors, rng);
            }
            else
            {
                LowMoraleStreakDays = 0;
            }
        }

        private void TriggerMutiny(IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            Survivor leader = null;
            float best = float.NegativeInfinity;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || sv.IsChild) continue;
                if (IsRebel(sv.Id)) continue;
                float score = (LeadershipScore != null ? LeadershipScore(sv) : Mathf.Clamp01(sv.Needs.Morale / 100f));
                if (score > best) { best = score; leader = sv; }
            }
            if (leader == null) { LowMoraleStreakDays = 0; return; }

            MutinyActive = true;
            LeaderId = leader.Id;
            FollowerIds.Clear();

            // Followers: alive survivors with high affinity to the leader (or just the most demoralised).
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || sv == leader || !sv.IsAlive || sv.IsChild) continue;
                if (FollowerIds.Count >= 2) break;
                FollowerIds.Add(sv.Id);
            }

            OnMutinyStarted?.Invoke(leader);
        }

        /// <summary>Player negotiates — regains control with a small morale cost.</summary>
        public bool ResolveNegotiate()
        {
            if (!MutinyActive) return false;
            MutinyActive = false;
            LeaderId = null;
            FollowerIds.Clear();
            LowMoraleStreakDays = 0;
            OnMutinyResolved?.Invoke(MutinyResolution.Negotiate);
            return true;
        }

        /// <summary>
        /// Player yields hostage resources to the faction to restore control.
        /// The resource units are taken by <paramref name="yieldResources"/> (Core
        /// hook) before control is restored. Returns false if the resource cannot
        /// be afforded (mutiny continues).
        /// </summary>
        public bool ResolveYieldResources(int unitsRequested, Func<int, bool> yieldResources)
        {
            if (!MutinyActive) return false;
            bool paid = yieldResources != null ? yieldResources(unitsRequested) : false;
            if (!paid) return false;
            MutinyActive = false;
            LeaderId = null;
            FollowerIds.Clear();
            LowMoraleStreakDays = 0;
            OnMutinyResolved?.Invoke(MutinyResolution.YieldResources);
            return true;
        }

        /// <summary>Player executes the leader — control is restored but morale collapses.</summary>
        public bool ResolveExecute(IReadOnlyList<Survivor> survivors)
        {
            if (!MutinyActive) return false;
            var leader = SurvivorById(survivors, LeaderId);
            if (leader != null && leader.IsAlive)
            {
                // DEATH-001/006: the centralized SurvivorNeedWrite.OnKilled event
                // (wired to the bootstrap death chain) fires automatically, so a
                // mutiny execution produces the same world reactions as a
                // natural death.
                SurvivorNeedWrite.SetHealth(leader, 0f);
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive || sv.Id == LeaderId) continue;
                    if (_needsSystem != null)

                        _needsSystem.Modify(sv, NeedKind.Morale, -(MutinyFalloutMoraleHit));

                    else

                        sv.Needs.Morale = Mathf.Max(0f, sv.Needs.Morale - MutinyFalloutMoraleHit);
                }
            }
            MutinyActive = false;
            LeaderId = null;
            FollowerIds.Clear();
            LowMoraleStreakDays = 0;
            OnMutinyResolved?.Invoke(MutinyResolution.Execute);
            return true;
        }

        public MutinySave CaptureState()
        {
            return new MutinySave
            {
                LowMoraleStreakDays = LowMoraleStreakDays,
                MutinyActive = MutinyActive,
                LeaderId = LeaderId,
                FollowerIds = new List<string>(FollowerIds),
                LastCheckedDay = _lastCheckedDay
            };
        }

        public void RestoreState(MutinySave save)
        {
            if (save == null) return;
            LowMoraleStreakDays = save.LowMoraleStreakDays;
            MutinyActive = save.MutinyActive;
            LeaderId = save.LeaderId;
            FollowerIds = save.FollowerIds ?? new List<string>();
            _lastCheckedDay = save.LastCheckedDay;
        }

        private static Survivor SurvivorById(IReadOnlyList<Survivor> survivors, string id)
        {
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && string.Equals(survivors[i].Id, id, StringComparison.Ordinal))
                    return survivors[i];
            return null;
        }
    }

    [Serializable]
    public class MutinySave
    {
        public int LowMoraleStreakDays;
        public bool MutinyActive;
        public string LeaderId;
        public List<string> FollowerIds = new List<string>();
        public int LastCheckedDay;
    }

    // =====================================================================
    // #472 IMPRISONMENT (THE BRIG)
    // =====================================================================

    /// <summary>
    /// #472 — the player can convert a room into a Cell (removing the door and
    /// building IronBars via <see cref="TryConvertRoomToCell"/>). Mutinous,
    /// violent or infected survivors can be locked inside. Imprisoned survivors
    /// consume food but provide zero labor — a grim alternative to execution or
    /// banishment.
    /// </summary>
    public class ImprisonmentSystem
    {
        public event Action<Survivor> OnImprisoned;
        public event Action<Survivor> OnReleased;
        public event Action<string> OnRoomConverted;

        private readonly HashSet<string> _imprisoned = new HashSet<string>();
        private readonly List<string> _cells = new List<string>();

        /// <summary>Host index of living survivors (wired by Core).</summary>
        public Func<IReadOnlyList<Survivor>> GetSurvivors;

        public bool ConvertRoomToCell(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            if (_cells.Contains(roomId)) return false;
            _cells.Add(roomId);
            OnRoomConverted?.Invoke(roomId);
            return true;
        }

        public bool HasCell => _cells.Count > 0;

        public bool Imprison(string survivorId)
        {
            // Require a converted cell: you cannot lock someone up with no brig yet.
            if (!HasCell || string.IsNullOrEmpty(survivorId) || _imprisoned.Contains(survivorId)) return false;
            _imprisoned.Add(survivorId);
            var sv = SurvivorById(survivorId);
            OnImprisoned?.Invoke(sv);
            return true;
        }

        public bool Release(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId) || !_imprisoned.Remove(survivorId)) return false;
            OnReleased?.Invoke(SurvivorById(survivorId));
            return true;
        }

        public bool IsImprisoned(string survivorId) =>
            !string.IsNullOrEmpty(survivorId) && _imprisoned.Contains(survivorId);

        /// <summary>Imprisoned survivors contribute no labor.</summary>
        public bool ProvidesLabor(string survivorId) => !IsImprisoned(survivorId);

        /// <summary>Imprisoned survivors still consume food (they are fed).</summary>
        public bool ConsumesFood(string survivorId) => IsImprisoned(survivorId);

        public IReadOnlyCollection<string> ImprisonedIds => _imprisoned;

        private Survivor SurvivorById(string id)
        {
            if (string.IsNullOrEmpty(id) || GetSurvivors == null) return null;
            var all = GetSurvivors();
            if (all == null) return null;
            for (int i = 0; i < all.Count; i++)
                if (all[i] != null && string.Equals(all[i].Id, id, StringComparison.Ordinal))
                    return all[i];
            return null;
        }

        public ImprisonmentSave CaptureState()
        {
            return new ImprisonmentSave
            {
                Imprisoned = new List<string>(_imprisoned),
                Cells = new List<string>(_cells)
            };
        }

        public void RestoreState(ImprisonmentSave save)
        {
            _imprisoned.Clear();
            _cells.Clear();
            if (save == null) return;
            if (save.Imprisoned != null) foreach (var s in save.Imprisoned) if (!string.IsNullOrEmpty(s)) _imprisoned.Add(s);
            if (save.Cells != null) foreach (var c in save.Cells) if (!string.IsNullOrEmpty(c)) _cells.Add(c);
        }

        public void Clear() => _imprisoned.Clear();
    }

    [Serializable]
    public class ImprisonmentSave
    {
        public List<string> Imprisoned = new List<string>();
        public List<string> Cells = new List<string>();
    }

    // =====================================================================
    // #473 BANISHMENT + #474 THE RETURN
    // =====================================================================

    /// <summary>
    /// #473/#474 — the player can kick a survivor out the airlock. This lowers
    /// bunker morale unless the banished was a Serial Killer or Saboteur. The
    /// banished survivor's inventory is permanently lost. 30 days later the
    /// banished has a 50% chance to return as a Raider Boss during a Hatch
    /// Breach, bypassing the exterior PerimeterTraps.
    /// </summary>
    public class BanishmentSystem
    {
        public const int ReturnCooldownDays = 30;
        public const double ReturnChance = 0.5;
        public const float BanishMoraleHit = 8f;

        public event Action<Survivor, bool> OnBanish;              // (shunned, penalizedMorale)
        public event Action<BanishedRecord> OnBanishedReturned;

        public readonly List<BanishedRecord> Banished = new List<BanishedRecord>();
        private readonly HashSet<string> _returned = new HashSet<string>();

        /// <summary>True if the survivor is a SerialKiller/Saboteur (no morale penalty). Wired by Core.</summary>
        public Func<Survivor, bool> IsSevereThreat;

        /// <summary>Total morale penalty to apply to the remaining survivors after a banish.</summary>
        public float CurrentBanishMoraleHit => BanishMoraleHit;

        /// <summary>
        /// Banish a survivor. Records them for the potential return; applies a
        /// morale penalty to everyone else UNLESS the banished was a severe threat.
        /// Returns true on a fresh banish.
        /// </summary>
        public bool Banish(Survivor shunned, int day)
        {
            if (shunned == null) return false;
            bool penalize = !(IsSevereThreat != null && IsSevereThreat(shunned));
            Banished.Add(new BanishedRecord { Id = shunned.Id, Day = day, Penalized = penalize });
            OnBanish?.Invoke(shunned, penalize);
            return true;
        }

        /// <summary>
        /// Roll for banished survivors whose cooldown has elapsed to return as a
        /// raider boss. Wired Core listens for <see cref="OnBanishedReturned"/> to
        /// spawn the breach. Returns the number that returned today.
        /// </summary>
        public int TickBanishedReturns(int day, System.Random rng)
        {
            if (rng == null) return 0;
            int returned = 0;
            for (int i = 0; i < Banished.Count; i++)
            {
                var rec = Banished[i];
                if (_returned.Contains(rec.Id)) continue;
                if (day - rec.Day < ReturnCooldownDays) continue;
                if (rng.NextDouble() < ReturnChance)
                {
                    _returned.Add(rec.Id);
                    OnBanishedReturned?.Invoke(rec);
                    returned++;
                }
            }
            return returned;
        }

        public bool HasReturnedAsRaider(string id) => _returned.Contains(id);

        public BanishmentSave CaptureState()
        {
            return new BanishmentSave
            {
                Banished = Banished.Select(b => new BanishedRecordSave { Id = b.Id, Day = b.Day }).ToList(),
                Returned = new List<string>(_returned)
            };
        }

        public void RestoreState(BanishmentSave save)
        {
            Banished.Clear();
            _returned.Clear();
            if (save == null) return;
            if (save.Banished != null)
                foreach (var b in save.Banished)
                    if (b != null && !string.IsNullOrEmpty(b.Id))
                        Banished.Add(new BanishedRecord { Id = b.Id, Day = b.Day, Penalized = true });
            if (save.Returned != null) foreach (var r in save.Returned) if (!string.IsNullOrEmpty(r)) _returned.Add(r);
        }
    }

    public class BanishedRecord
    {
        public string Id;
        public int Day;
        public bool Penalized;
    }

    [Serializable]
    public class BanishmentSave
    {
        public List<BanishedRecordSave> Banished = new List<BanishedRecordSave>();
        public List<string> Returned = new List<string>();
    }

    [Serializable]
    public class BanishedRecordSave
    {
        public string Id;
        public int Day;
    }

    // =====================================================================
    // #476 PREGNANCY
    // =====================================================================

    /// <summary>
    /// #476 — a rare event for Lovers. The pregnant survivor suffers escalating
    /// fatigue and caloric needs across a 9-month term. If she is successfully
    /// brought to term — which requires pristine MedicalSupplies at birth — a
    /// Child is added to the bunker, granting a permanent, massive Hope buff to
    /// every occupant.
    /// </summary>
    public class PregnancySystem
    {
        public const int PregnancyDurationDays = 270; // ~9 months
        public const double ConceptionChancePerDay = 0.02;

        /// <summary>Pristine-supply failure → risk of a tragic outcome each overdue day.</summary>
        public const double NoSuppliesFailureChancePerDay = 0.35;

        /// <summary>Daily fatigue escalation once pregnant (grows with trimester).</summary>
        public const float PregnancyFatigueBasePerDay = 3f;

        /// <summary>Additional daily hunger pressure (caloric needs).</summary>
        public const float PregnancyHungerPerDay = 4f;

        /// <summary>Permanent massive Hope buff once a child is born.</summary>
        public const float ChildBornHopeBuff = 30f;

        public event Action<Survivor, Survivor> OnPregnancyStarted;
        public event Action<Survivor> OnChildBorn;
        public event Action<Survivor, string> OnPregnancyFailed;
        public event Action<bool> OnChildHopeBuffChanged;

        private readonly Dictionary<string, PregnancyRecord> _active = new Dictionary<string, PregnancyRecord>();
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;
        private bool _childHopeActive;
        private bool _childBorn;

        public Func<Survivor, bool> HasPristineMedicalSupplies;
        public Func<RomanceSystem, string, string> LoverLookup; // (romance, patientId) -> partner

        public bool ChildHopeBuffActive => _childHopeActive;
        public bool ChildBorn => _childBorn;

        /// <summary>
        /// Roll for conception. Requires the patient to be a Lover this day and
        /// the pair to be resting (low fatigue) recently. Returns true if a new
        /// pregnancy began.
        /// </summary>
        public bool TryStartPregnancy(Survivor patient, Survivor partner, System.Random rng)
        {
            if (patient == null || partner == null || rng == null) return false;
            if (!patient.IsAlive || !partner.IsAlive) return false;
            if (patient.IsChild) return false;
            if (_active.ContainsKey(patient.Id)) return false;
            if (rng.NextDouble() >= ConceptionChancePerDay) return false;
            // Requires the pair to be lovers (guaranteed by the director) + rested.
            if (patient.Needs.Fatigue > 60f) return false;

            _active[patient.Id] = new PregnancyRecord
            {
                PatientId = patient.Id,
                PartnerId = partner.Id,
                StartedDay = 0,
                ProgressDays = 0
            };
            OnPregnancyStarted?.Invoke(patient, partner);
            return true;
        }

        /// <summary>
        /// Advance every active pregnancy by one day. Applies escalating fatigue /
        /// hunger burdens. Temps a birth at term and rolls a failure each overdue
        /// day when pristine supplies are missing.
        /// </summary>
        public void TickPregnancy(int currentDay, IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            if (survivors == null) return;
            var toRemove = new List<string>();
            foreach (var kv in _active)
            {
                var rec = kv.Value;
                var patient = Find(survivors, rec.PatientId);
                if (patient == null || !patient.IsAlive)
                {
                    toRemove.Add(rec.PatientId); // tragic loss with the parent
                    continue;
                }
                rec.ProgressDays++;
                int trimester = 1 + (rec.ProgressDays / (PregnancyDurationDays / 3));
                if (_needsSystem != null)

                    _needsSystem.Modify(patient, NeedKind.Fatigue, PregnancyFatigueBasePerDay * trimester);

                else

                    patient.Needs.Fatigue = Mathf.Min(100f, patient.Needs.Fatigue + PregnancyFatigueBasePerDay * trimester);
                if (_needsSystem != null)

                    _needsSystem.Modify(patient, NeedKind.Hunger, PregnancyHungerPerDay);

                else

                    patient.Needs.Hunger = Mathf.Min(100f, patient.Needs.Hunger + PregnancyHungerPerDay);

                if (rec.ProgressDays >= PregnancyDurationDays)
                {
                    bool supplies = HasPristineMedicalSupplies != null && HasPristineMedicalSupplies(patient);
                    if (supplies)
                    {
                        toRemove.Add(rec.PatientId);
                        _childBorn = true;
                        _childHopeActive = true;
                        OnChildBorn?.Invoke(patient);
                        OnChildHopeBuffChanged?.Invoke(true);
                    }
                    else if (rng.NextDouble() < NoSuppliesFailureChancePerDay)
                    {
                        toRemove.Add(rec.PatientId);
                        OnPregnancyFailed?.Invoke(patient, "no_pristine_supplies");
                    }
                    // else: overdue — keep burdening until supplies arrive or tragedy.
                }
            }
            foreach (var id in toRemove) _active.Remove(id);
        }

        /// <summary>Apply the permanent massive Hope buff to all occupants while it is active.</summary>
        public void ApplyChildHopeBuff(IReadOnlyList<Survivor> survivors)
        {
            if (!_childHopeActive || survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (_needsSystem != null)

                    _needsSystem.Modify(sv, NeedKind.Morale, ChildBornHopeBuff);

                else

                    sv.Needs.Morale = Mathf.Min(100f, sv.Needs.Morale + ChildBornHopeBuff);
            }
        }

        public bool IsPregnant(string id) => _active.ContainsKey(id);
        public int ActivePregnancyCount => _active.Count;

        public PregnancySave CaptureState()
        {
            var save = new PregnancySave { ChildHopeActive = _childHopeActive, ChildBorn = _childBorn };
            foreach (var kv in _active)
                save.Pregnancies.Add(new PregnancyRecordSave
                {
                    PatientId = kv.Value.PatientId,
                    PartnerId = kv.Value.PartnerId,
                    ProgressDays = kv.Value.ProgressDays
                });
            return save;
        }

        public void RestoreState(PregnancySave save)
        {
            _active.Clear();
            _childHopeActive = false;
            _childBorn = false;
            if (save == null) return;
            _childHopeActive = save.ChildHopeActive;
            _childBorn = save.ChildBorn;
            if (save.Pregnancies != null)
                foreach (var pr in save.Pregnancies)
                    if (pr != null && !string.IsNullOrEmpty(pr.PatientId))
                        _active[pr.PatientId] = new PregnancyRecord
                        {
                            PatientId = pr.PatientId,
                            PartnerId = pr.PartnerId,
                            ProgressDays = pr.ProgressDays
                        };
        }

        private static Survivor Find(IReadOnlyList<Survivor> survivors, string id)
        {
            if (survivors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && string.Equals(survivors[i].Id, id, StringComparison.Ordinal))
                    return survivors[i];
            return null;
        }

        private sealed class PregnancyRecord
        {
            public string PatientId;
            public string PartnerId;
            public int StartedDay;
            public int ProgressDays;
        }
    }

    [Serializable]
    public class PregnancySave
    {
        public List<PregnancyRecordSave> Pregnancies = new List<PregnancyRecordSave>();
        public bool ChildHopeActive;
        public bool ChildBorn;
    }

    [Serializable]
    public class PregnancyRecordSave
    {
        public string PatientId;
        public string PartnerId;
        public int ProgressDays;
    }

    // =====================================================================
    // #477 THE BUNKER TRIBUNAL
    // =====================================================================

    /// <summary>
    /// #477 — if a survivor commits a crime (stealing rations, murder), the bunker
    /// demands a trial; the player acts as judge. The punishment must match the
    /// crime — a lenient OR excessive verdict is mismatched and costs the player
    /// Trust with the whole crew.
    /// </summary>
    public class TribunalSystem
    {
        public event Action<Survivor, string, BunkerCrimeSeverity> OnTribunalStarted;
        public event Action<Survivor, BunkerPunishment, PunishmentMatch, bool> OnVerdict;
        public event Action<int> OnTrustChanged;

        private readonly List<TribunalCase> _pending = new List<TribunalCase>();

        public bool RegisterCrime(Survivor survivor, string crimeId, BunkerCrimeSeverity severity)
        {
            if (survivor == null) return false;
            _pending.Add(new TribunalCase { SurvivorId = survivor.Id, CrimeId = crimeId, Severity = severity });
            OnTribunalStarted?.Invoke(survivor, crimeId, severity);
            return true;
        }

        public bool HasPending => _pending.Count > 0;
        public int PendingCount => _pending.Count;

        /// <summary>
        /// Judge the current case. <paramref name="onPunish"/> lets Core apply the
        /// consequence (ration cut / banish / execute). Returns whether a verdict
        /// was handed down; Trust is lost on a mismatched verdict.
        /// </summary>
        public bool JudgeNext(BunkerPunishment punishment, Action<Survivor, BunkerPunishment> onPunish)
        {
            if (_pending.Count == 0) return false;
            var c = _pending[0];
            _pending.RemoveAt(0);
            var sv = SurvivorById(c.SurvivorId);
            var match = MatchPunishment(c.Severity, punishment);
            bool mismatched = match != PunishmentMatch.Appropriate;
            int trustDelta = mismatched ? -10 : 5;
            OnVerdict?.Invoke(sv, punishment, match, mismatched);
            OnTrustChanged?.Invoke(trustDelta);
            onPunish?.Invoke(sv, punishment);
            return true;
        }

        public static PunishmentMatch MatchPunishment(BunkerCrimeSeverity severity, BunkerPunishment punishment)
        {
            int s = (int)severity;   // 0..2
            int p = (int)punishment; // 0..2
            if (p == s) return PunishmentMatch.Appropriate;
            return p < s ? PunishmentMatch.Lenient : PunishmentMatch.Excessive;
        }

        public TribunalSave CaptureState()
        {
            var save = new TribunalSave();
            foreach (var c in _pending)
                save.Pending.Add(new TribunalCaseSave { SurvivorId = c.SurvivorId, CrimeId = c.CrimeId, Severity = (int)c.Severity });
            return save;
        }

        public void RestoreState(TribunalSave save)
        {
            _pending.Clear();
            if (save?.Pending == null) return;
            foreach (var c in save.Pending)
                if (c != null && !string.IsNullOrEmpty(c.SurvivorId))
                    _pending.Add(new TribunalCase
                    {
                        SurvivorId = c.SurvivorId,
                        CrimeId = c.CrimeId,
                        Severity = (BunkerCrimeSeverity)Mathf.Clamp(c.Severity, 0, 2)
                    });
        }

        private Survivor SurvivorById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            if (GetSurvivors == null) return null;
            var all = GetSurvivors();
            if (all == null) return null;
            for (int i = 0; i < all.Count; i++)
                if (all[i] != null && string.Equals(all[i].Id, id, StringComparison.Ordinal))
                    return all[i];
            return null;
        }

        public Func<IReadOnlyList<Survivor>> GetSurvivors;

        private sealed class TribunalCase
        {
            public string SurvivorId;
            public string CrimeId;
            public BunkerCrimeSeverity Severity;
        }
    }

    [Serializable]
    public class TribunalSave
    {
        public List<TribunalCaseSave> Pending = new List<TribunalCaseSave>();
    }

    [Serializable]
    public class TribunalCaseSave
    {
        public string SurvivorId;
        public string CrimeId;
        public int Severity;
    }

    // =====================================================================
    // #478 SECRET ALLIANCES (THE BLACK MARKET)
    // =====================================================================

    /// <summary>
    /// #478 — two survivors with high affinity but low morale may start trading
    /// bunker resources to hostile factions behind the player's back in exchange
    /// for comfort items (drugs, alcohol). The smuggling is tracked so an
    /// attentive player (or a sabotage investigation) can expose it.
    /// </summary>
    public class BlackMarketSystem
    {
        public const float AllianceAffinityThreshold = 70f;
        public const float AllianceMoraleCeiling = 40f;

        /// <summary>Chance per day that an eligible pair forms an alliance.</summary>
        public const double FormChancePerDay = 0.03;

        public event Action<Survivor, Survivor> OnAllianceFormed;
        public event Action<Survivor, string, string> OnSmuggleOccurred; // (perpetrator, resourceId, comfortItemId)
        public event Action<string, string> OnAllianceExposed;

        private readonly InterpersonalAffinity _affinity;
        private readonly Dictionary<SocialPairKey, AllianceEntry> _alliances = new Dictionary<SocialPairKey, AllianceEntry>();
        private readonly List<SmuggleRecord> _ledger = new List<SmuggleRecord>();

        /// <summary>Core hook: drain a bunker resource; returns a comfort item id or null.</summary>
        public Func<string, string> DrainingSmuggleHandler; // (resourceId) -> comfortItemId
        public Func<string, IReadOnlyList<string>> AvailableResourceIds; // (perpetratorId) -> resource list

        public BlackMarketSystem(InterpersonalAffinity affinity)
        {
            _affinity = affinity ?? new InterpersonalAffinity();
        }

        /// <summary>Form alliances among eligible pairs (call once/day).</summary>
        public void TickFormAlliances(IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            if (survivors == null || rng == null) return;
            List<Survivor> alive = new List<Survivor>(survivors.Count);
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && survivors[i].IsAlive) alive.Add(survivors[i]);

            for (int i = 0; i < alive.Count; i++)
            {
                for (int j = i + 1; j < alive.Count; j++)
                {
                    var a = alive[i];
                    var b = alive[j];
                    var key = new SocialPairKey(a.Id, b.Id);
                    if (_alliances.ContainsKey(key)) continue;
                    if (_affinity.Get(a.Id, b.Id) < AllianceAffinityThreshold) continue;
                    if (a.Needs.Morale > AllianceMoraleCeiling && b.Needs.Morale > AllianceMoraleCeiling) continue;
                    if (rng.NextDouble() >= FormChancePerDay) continue;
                    _alliances[key] = new AllianceEntry { A = a.Id, B = b.Id };
                    OnAllianceFormed?.Invoke(a, b);
                }
            }
        }

        /// <summary>Attempt one smuggling act per active alliance (call once/day).</summary>
        public int TickSmuggle(IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            if (survivors == null || rng == null) return 0;
            int smuggled = 0;
            var drop = new List<SocialPairKey>();
            foreach (var kv in _alliances)
            {
                var a = Find(survivors, kv.Value.A);
                var b = Find(survivors, kv.Value.B);
                if (a == null || b == null || !a.IsAlive || !b.IsAlive) { drop.Add(kv.Key); continue; }

                var perp = rng.NextDouble() < 0.5 ? a : b;
                var resources = AvailableResourceIds != null ? AvailableResourceIds(perp.Id) : null;
                if (resources == null || resources.Count == 0) continue;
                string resource = resources[rng.Next(resources.Count)];
                string comfort = DrainingSmuggleHandler != null ? DrainingSmuggleHandler(resource) : "comfort_alcohol";
                if (!string.IsNullOrEmpty(comfort))
                {
                    smuggled++;
                    _ledger.Add(new SmuggleRecord { PerpetratorId = perp.Id, ResourceId = resource, Day = 0, ComfortItemId = comfort });
                    OnSmuggleOccurred?.Invoke(perp, resource, comfort);
                }
            }
            foreach (var k in drop) _alliances.Remove(k);
            return smuggled;
        }

        /// <summary>Player discovers &amp; breaks an alliance; the smugglers lose standing.</summary>
        public bool ExposeAlliance(string a, string b)
        {
            var key = new SocialPairKey(a, b);
            if (!_alliances.Remove(key)) return false;
            _affinity.Adjust(a, b, -20f);
            OnAllianceExposed?.Invoke(a, b);
            return true;
        }

        public bool HasAlliance(string a, string b) =>
            !string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b) && _alliances.ContainsKey(new SocialPairKey(a, b));

        public IReadOnlyList<SmuggleRecord> Ledger => _ledger;
        public int ActiveAllianceCount => _alliances.Count;
        public int TotalSmuggled => _ledger.Count;

        public BlackMarketSave CaptureState()
        {
            var save = new BlackMarketSave();
            foreach (var kv in _alliances)
                save.Alliances.Add(new LoverSave { A = kv.Value.A, B = kv.Value.B });
            foreach (var r in _ledger)
                save.Ledger.Add(new SmuggleRecordSave { PerpetratorId = r.PerpetratorId, ResourceId = r.ResourceId, ComfortItemId = r.ComfortItemId });
            return save;
        }

        public void RestoreState(BlackMarketSave save)
        {
            _alliances.Clear();
            _ledger.Clear();
            if (save == null) return;
            if (save.Alliances != null)
                foreach (var al in save.Alliances)
                    if (al != null && !string.IsNullOrEmpty(al.A) && !string.IsNullOrEmpty(al.B))
                        _alliances[new SocialPairKey(al.A, al.B)] = new AllianceEntry { A = al.A, B = al.B };
            if (save.Ledger != null)
                foreach (var r in save.Ledger)
                    if (r != null && !string.IsNullOrEmpty(r.PerpetratorId))
                        _ledger.Add(new SmuggleRecord { PerpetratorId = r.PerpetratorId, ResourceId = r.ResourceId, ComfortItemId = r.ComfortItemId });
        }

        private static Survivor Find(IReadOnlyList<Survivor> survivors, string id)
        {
            if (survivors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < survivors.Count; i++)
                if (survivors[i] != null && string.Equals(survivors[i].Id, id, StringComparison.Ordinal))
                    return survivors[i];
            return null;
        }


        private sealed class AllianceEntry
        {
            public string A;
            public string B;
        }
    }

    [Serializable]
    public class BlackMarketSave
    {
        public List<LoverSave> Alliances = new List<LoverSave>();
        public List<SmuggleRecordSave> Ledger = new List<SmuggleRecordSave>();
    }

    [Serializable]
    public class SmuggleRecordSave
    {
        public string PerpetratorId;
        public string ResourceId;
        public string ComfortItemId;
    }

    /// <summary>A single black-market smuggling incident (id form, for the UI ledger).</summary>
    public class SmuggleRecord
    {
        public string PerpetratorId;
        public string ResourceId;
        public string ComfortItemId;
        public int Day;
    }
}
