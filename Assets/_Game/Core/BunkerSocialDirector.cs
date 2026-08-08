using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Bunker Social Director (Prompts #469-#478) — the single host-facing front
    /// for the interpersonal &amp; leadership systems. It owns one shared
    /// <see cref="InterpersonalAffinity"/> matrix and every relationship
    /// sub-system, drives their per-day + per-tick behaviour, exposes the player
    /// action API GameBootstrap / UI call into, and persists the whole family
    /// through one <see cref="ISaveable"/> slot ("bunker_social").
    ///
    /// Sub-systems (all in AtomicWar._Game.Survivors, pure C#):
    ///   Romance       #469 Lovers + #470 Breakup
    ///   Feuds         #475 Feud + passive sabotage
    ///   Mutiny        #471 Leadership challenge
    ///   Brig          #472 Imprisonment
    ///   Banishment    #473 Banishment + #474 return
    ///   Pregnancy     #476 Child-bearing
    ///   Tribunal      #477 trial / judging
    ///   BlackMarket   #478 secret smuggling alliances
    ///
    /// Inventory / shelter / hatch side-effects run through injected hooks only,
    /// keeping Core thin and the Survivors assembly leaf-level.
    /// </summary>
    public class BunkerSocialDirector : ISaveable
    {
        // -----------------------------------------------------------------
        // Sub-systems
        // -----------------------------------------------------------------
        public RomanceSystem Romance { get; }
        public FeudSystem Feuds { get; }
        public MutinySystem Mutiny { get; }
        public ImprisonmentSystem Brig { get; }
        public BanishmentSystem Banishment { get; }
        public PregnancySystem Pregnancy { get; }
        public TribunalSystem Tribunal { get; }
        public BlackMarketSystem BlackMarket { get; }

        /// <summary>The single affinity matrix shared by all relationship systems.</summary>
        public InterpersonalAffinity Affinity { get; }

        private readonly Dictionary<string, float> _lastHealth = new Dictionary<string, float>();
        private int _lastTickDay = -1;
        private System.Random _cachedRng;

        public event Action<Survivor, string> OnGriefMentalBreakApplied; // (bereaved, breakId)

        // -----------------------------------------------------------------
        // Host hooks (wired by GameBootstrap / tests)
        // -----------------------------------------------------------------

        /// <summary>Which survivors are a severe threat (SerialKiller/Saboteur) → no banish morale penalty.</summary>
        public Func<Survivor, bool> IsSevereThreat;

        /// <summary>Pristine MedicalSupplies check for a safe childbirth (#476).</summary>
        public Func<Survivor, bool> HasPristineMedicalSupplies;

        /// <summary>Leadership rank = Charisma/Strength proxy (#471).</summary>
        public Func<Survivor, float> LeadershipScore;

        /// <summary>Yield <paramref name="units"/> of ownership/resources to end a mutiny (#471).</summary>
        public Func<int, bool> YieldBunkerControl;

        /// <summary>Drain one resource of the given id from the bunker; returns a comfort item (or null) (#478).</summary>
        public Func<string, string> SmuggleDrain;

        /// <summary>Available smuggled-out resource ids for a perpetrator (#478).</summary>
        public Func<string, IReadOnlyList<string>> AvailableSmuggleResources;

        /// <summary>Apply a sabotage side-effect to the victim's recent work (#475).</summary>
        public Func<Survivor, Survivor, string, bool> SabotageWorkHandler;

        /// <summary>Trigger a Hatch Breach / Raider Boss raid led by a returned banished survivor (#474).</summary>
        public Action<string, int> TriggerBanishedRaid;

        /// <summary>
        /// DEATH-001 hardened: the death chain that runs when Tribunal.Execution
        /// kills a survivor via <see cref="SurvivorNeedWrite.SetHealth"/>. The
        /// bootstrap wires this to its <see cref="NeedsSystem.OnDied"/> handler
        /// (which fires NotifySurvivorDied, EmpathSystem, ChildSystem, GriefKeepsakes,
        /// IronMan). Without this, an execution is a silent death — the survivor
        /// is dead on disk but no other system reacts. Default is a no-op.
        /// </summary>
        public System.Action<Survivor> OnKilled;

        // -----------------------------------------------------------------
        // Construction
        // -----------------------------------------------------------------

        /// <param name="affinity">
        /// The matrix every relationship sub-system reads. Callers must pass the
        /// same instance the rest of the game mutates — in GameBootstrap that is
        /// <c>MentalBreakSystem.Affinity</c>, which is also the only matrix
        /// SaveSystem persists. Allocating a private one here (the old behaviour)
        /// left Romance / Feuds / BlackMarket reading a matrix that EventRunner
        /// choices, mental-break drain and gossip rot never touched, and dropped
        /// every bond the director did build on load. Null allocates a fresh
        /// matrix, which keeps standalone tests working.
        /// </param>
        public BunkerSocialDirector(InterpersonalAffinity affinity = null)
        {
            Affinity = affinity ?? new InterpersonalAffinity();
            Romance = new RomanceSystem(Affinity);
            Feuds = new FeudSystem(Affinity);
            Mutiny = new MutinySystem();
            Brig = new ImprisonmentSystem();
            Banishment = new BanishmentSystem();
            Pregnancy = new PregnancySystem();
            Tribunal = new TribunalSystem();
            BlackMarket = new BlackMarketSystem(Affinity);

            // Default refusals / spaces.
            Romance.ShareSleepingSpace = (a, b) => ShareBedSpace(a, b);
            Romance.RefuseCooperationCheck = (a, b) => Romance.BreakupAuraActive(a, b);
            Banishment.IsSevereThreat = sv => IsSevereThreat != null && IsSevereThreat(sv);
            Pregnancy.HasPristineMedicalSupplies = sv => HasPristineMedicalSupplies != null && HasPristineMedicalSupplies(sv);
            Mutiny.LeadershipScore = sv => LeadershipScore != null ? LeadershipScore(sv) : DefaultLeadership(sv);
            // Default: a pure sabotage event lands even with no host side-effect; the
            // host hook only supplies the concrete consequence (contamination, hiding).
            Feuds.SabotageWorkHandler = (a, b, kind) => SabotageWorkHandler == null || SabotageWorkHandler(a, b, kind);
            BlackMarket.DrainingSmuggleHandler = id => SmuggleDrain != null ? SmuggleDrain(id) : null;
            BlackMarket.AvailableResourceIds = id => AvailableSmuggleResources != null ? AvailableSmuggleResources(id) : null;

            Brig.GetSurvivors = () => Survivors;
            Tribunal.GetSurvivors = () => Survivors;
        }

        public IReadOnlyList<Survivor> Survivors { get; set; }

        /// <summary>Default leadership = morale + modest base so the system works sans wiring.</summary>
        private static float DefaultLeadership(Survivor sv) =>
            sv == null ? 0f : 0.4f + Mathf.Clamp01(sv.Needs.Morale / 100f);

        private bool ShareBedSpace(Survivor a, Survivor b)
        {
            if (a == null || b == null) return false;
            // Both unassigned → common quarters; both in same assigned room → shared.
            return string.IsNullOrEmpty(a.CurrentRoomId)
                && string.IsNullOrEmpty(b.CurrentRoomId)
                || (!string.IsNullOrEmpty(a.CurrentRoomId)
                    && string.Equals(a.CurrentRoomId, b.CurrentRoomId, StringComparison.Ordinal));
        }

        // -----------------------------------------------------------------
        // Main tick (per-tick auras + daily transitions)
        // -----------------------------------------------------------------

        public void Tick(float gameHours, int day, IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            if (survivors == null) return;
            Survivors = survivors;
            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("bunkersocialdirector");
            _cachedRng = rng;

            // Per-tick continuous auras.
            Romance.ApplyAuras(gameHours, survivors);
            Pregnancy.ApplyChildHopeBuff(survivors);

            // Damage→anxiety monitor for lovers (#469).
            TickLoverDamageMonitor(survivors);

            // Daily-gated transitions & event logic.
            if (day != _lastTickDay)
            {
                _lastTickDay = day;
                TickDaily(day, survivors, rng);
            }
        }

        private void TickDaily(int day, IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            Romance.UpdateBondStates(survivors);
            Feuds.UpdateFeuds(survivors);
            Feuds.TickSabotage(1f, survivors, rng);
            BlackMarket.TickFormAlliances(survivors, rng);
            BlackMarket.TickSmuggle(survivors, rng);
            Banishment.TickBanishedReturns(day, rng);
            Pregnancy.TickPregnancy(day, survivors, rng);
            Mutiny.TickWeekly(day, survivors, rng);

            // #476 rare conception between lover pairs.
            TryAutoConception(survivors, rng);
        }

        private void TryAutoConception(IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            var pairs = Romance.GetLoverPairs();
            if (pairs == null || pairs.Count == 0) return;
            for (int i = 0; i < pairs.Count; i++)
            {
                var a = Find(survivors, pairs[i].A);
                var b = Find(survivors, pairs[i].B);
                if (a == null || b == null) continue;
                // Patient: the more rested lover. Roll conception in PregnancySystem.
                Survivor patient = a.Needs.Fatigue <= b.Needs.Fatigue ? a : b;
                Survivor partner = patient == a ? b : a;
                if (Pregnancy.TryStartPregnancy(patient, partner, rng)) return; // one per day is enough magic
            }
        }

        private void TickLoverDamageMonitor(IReadOnlyList<Survivor> survivors)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                bool had = _lastHealth.TryGetValue(sv.Id, out float prev);
                if (had && prev - sv.Needs.Health >= 1f)
                {
                    Romance.ApplyLoverDamageAnxiety(sv, survivors);
                }
                _lastHealth[sv.Id] = sv.Needs.Health;
            }
        }

        // -----------------------------------------------------------------
        // Death handling (#469 grief break, cleanup)
        // -----------------------------------------------------------------

        public void NotifySurvivorDied(Survivor deceased, System.Random rng)
        {
            _lastHealth.Remove(deceased.Id);
            if (Romance.GetLoverOf(deceased.Id) != null)
            {
                Romance.NotifyLoverDied(deceased, rng);
                var bereaved = Find(Survivors, Romance.PendingGriefBereavedId);
                string breakId = Romance.PendingGriefBreakId;
                Romance.ClearPendingGrief();
                if (bereaved != null && bereaved.IsAlive && !string.IsNullOrEmpty(breakId))
                {
                    bereaved.currentMentalBreakId = breakId;
                    bereaved.mentalBreakCureProgress = 0f;
                    bereaved.lowMoraleHours = 0f;
                    OnGriefMentalBreakApplied?.Invoke(bereaved, breakId);
                }
            }
            // A mutiny leader who dies ends the mutiny (control returns).
            if (Mutiny.MutinyActive && string.Equals(Mutiny.LeaderId, deceased.Id, StringComparison.Ordinal))
                Mutiny.ResolveExecute(Survivors);
            // Imprisoned dead survivors are removed from the brig.
            if (Brig.IsImprisoned(deceased.Id)) Brig.Release(deceased.Id);
        }

        // -----------------------------------------------------------------
        // Player actions (UI / GameBootstrap)
        // -----------------------------------------------------------------

        public bool ConvertRoomToCell(string roomId) => Brig.ConvertRoomToCell(roomId);
        public bool Imprison(string survivorId) => Brig.Imprison(survivorId);
        public bool Release(string survivorId) => Brig.Release(survivorId);
        public int ImprisonedCount => Brig.ImprisonedIds.Count;

        public bool Banish(Survivor shunned, int day) => Banishment.Banish(shunned, day);

        public bool RegisterCrime(Survivor suspect, string crimeId, BunkerCrimeSeverity severity) =>
            Tribunal.RegisterCrime(suspect, crimeId, severity);

        public bool JudgeNext(BunkerPunishment punishment)
        {
            return Tribunal.JudgeNext(punishment, (sv, p) =>
            {
                if (p == BunkerPunishment.Banishment && sv != null) Banish(sv, _currentDay);
                else if (p == BunkerPunishment.Execution && sv != null)
                {
                    // DEATH-001: pass OnKilled so the same death chain
                    // (NeedsSystem.OnDied + all the bootstrapped hooks) runs
                    // that a natural death would. Pre-fix the survivor became
                    // State=Dead but nothing else in the game world knew.
                    SurvivorNeedWrite.SetHealth(sv, 0f, -1f, OnKilled);
                }
            });
        }

        public int CurrentDay { get { return _currentDay; } set { _currentDay = value; } }
        private int _currentDay;

        public bool IsRebel(string id) => Mutiny.IsRebel(id);
        public bool ResolveMutinyNegotiate() => Mutiny.ResolveNegotiate();
        public bool ResolveMutinyYield(int units) => Mutiny.ResolveYieldResources(units, yieldResources: YieldLocalResources);
        public bool ResolveMutinyExecute() => Mutiny.ResolveExecute(Survivors);

        private bool YieldLocalResources(int units)
        {
            return YieldBunkerControl != null && YieldBunkerControl(units);
        }

        public bool ExposeAlliance(string a, string b) => BlackMarket.ExposeAlliance(a, b);
        public bool TryStartPregnancy(Survivor patient, Survivor partner, System.Random rng) =>
            Pregnancy.TryStartPregnancy(patient, partner, rng);

        public bool RefusesCooperation(string a, string b) => Romance.RefusesCooperativeTask(a, b);
        public float GetFatigueRecoveryMultiplier(Survivor sv) => Romance.GetFatigueRecoveryMultiplier(sv);
        public bool BreakupAuraActive(string a, string b) => Romance.BreakupAuraActive(a, b);

        // -----------------------------------------------------------------
        // ISaveable
        // -----------------------------------------------------------------

        public string SaveId => "bunker_social";

        public object CaptureState()
        {
            return new BunkerSocialSave
            {
                Romance = Romance.CaptureState(),
                Feuds = Feuds.CaptureState(),
                Mutiny = Mutiny.CaptureState(),
                Brig = Brig.CaptureState(),
                Banishment = Banishment.CaptureState(),
                Pregnancy = Pregnancy.CaptureState(),
                Tribunal = Tribunal.CaptureState(),
                BlackMarket = BlackMarket.CaptureState()
            };
        }

        public void RestoreState(object state)
        {
            var save = state as BunkerSocialSave;
            Romance.RestoreState(save?.Romance);
            Feuds.RestoreState(save?.Feuds);
            Mutiny.RestoreState(save?.Mutiny);
            Brig.RestoreState(save?.Brig);
            Banishment.RestoreState(save?.Banishment);
            Pregnancy.RestoreState(save?.Pregnancy);
            Tribunal.RestoreState(save?.Tribunal);
            BlackMarket.RestoreState(save?.BlackMarket);
            _lastHealth.Clear();
            _lastTickDay = -1;
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

    /// <summary>Flat, [Serializable], list-based snapshot of the whole social family.</summary>
    [Serializable]
    public class BunkerSocialSave
    {
        public RomanceSave Romance;
        public FeudSave Feuds;
        public MutinySave Mutiny;
        public ImprisonmentSave Brig;
        public BanishmentSave Banishment;
        public PregnancySave Pregnancy;
        public TribunalSave Tribunal;
        public BlackMarketSave BlackMarket;
    }
}
