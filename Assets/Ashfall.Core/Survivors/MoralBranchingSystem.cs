using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Direction of moral branching after accumulating enough moral choices.
    /// Mirror of the Unity-side enum; kept here so the Core stays engine-agnostic.
    /// </summary>
    public enum MoralBranchDirection
    {
        Neutral,
        NumbedResilience,
        BurdenedCompassion
    }

    /// <summary>
    /// Engine-agnostic per-survivor moral-branching state. Hosts map this onto
    /// their own survivor objects (Unity Survivor, Godot Survivor, etc.).
    /// </summary>
    public class MoralBranchState
    {
        public string SurvivorId = string.Empty;
        public bool IsAlive = true;

        /// <summary>Total moral choices made. At ChoicesToBranch, branching is decided.</summary>
        public int MoralChoiceCount;

        /// <summary>Which branch the survivor is on (Neutral until decided).</summary>
        public MoralBranchDirection BranchDirection = MoralBranchDirection.Neutral;

        /// <summary>0..1 — higher = more immune to death morale loss.</summary>
        public float NumbedResilienceLevel;

        /// <summary>0..1 — higher = stronger shelter buff when helping others.</summary>
        public float BurdenedCompassionLevel;

        /// <summary>True once a branch has been decided (direction != Neutral).</summary>
        public bool HasMoralBranch => BranchDirection != MoralBranchDirection.Neutral;
    }

    /// <summary>
    /// Serializable snapshot of the entire MoralBranchingSystem for save/load.
    /// </summary>
    public class MoralBranchingSaveState
    {
        public List<MoralBranchState> Survivors = new List<MoralBranchState>();
    }

    /// <summary>
    /// Moral Branching System — as moral choices pile up across a campaign,
    /// survivors lean toward Numbed Resilience (immune to death morale loss,
    /// unable to boost others) or Burdened Compassion (boosts bunker morale
    /// when helping others, takes severe stress penalties on tragedy).
    ///
    /// Branch is decided at 5+ moral choices and locked in for the campaign.
    ///
    /// Engine-agnostic port of the Unity-side MoralBranchingSystem. Hosts
    /// inject morale/shelter callbacks; the core owns only the branching rules.
    /// </summary>
    public class MoralBranchingSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const int ChoicesToBranch = 5;
        public const float NumbedBaseLevel = 0.6f;
        public const float CompassionBaseLevel = 0.6f;
        public const float BranchStrengthPerChoice = 0.08f;
        public const float BurdenedCompassionShelterMoraleBuff = 5f;
        public const float BurdenedCompassionTragedyPenalty = -15f;
        public const float NumbedResilienceDeathImmuneChance = 1f;
        public const float NumbedResilienceComfortDisabled = 1f;

        // ── Events ─────────────────────────────────────────────────────
        /// <summary>Fired when a survivor's branch direction is decided.</summary>
        public event Action<MoralBranchState, MoralBranchDirection> OnBranchDecided;

        /// <summary>Fired when burdened compassion shelter buff is applied. Args: state, shelterMoraleDelta.</summary>
        public event Action<MoralBranchState, float> OnBurdenedCompassionActivated;

        /// <summary>Fired when a numbed survivor blocks a comfort action.</summary>
        public event Action<MoralBranchState> OnNumbedComfortBlocked;

        /// <summary>Generic state-changed event for save/UI.</summary>
        public event Action OnStateChanged;

        // ── Host hooks ─────────────────────────────────────────────────
        /// <summary>Host callback: apply a morale delta to a specific survivor.</summary>
        public Action<MoralBranchState, float> ApplyMoraleDelta;

        /// <summary>Host callback: apply a shelter-wide morale delta.</summary>
        public Action<float> ApplyShelterMoraleDelta;

        // ── Tracked survivors ──────────────────────────────────────────
        private readonly List<MoralBranchState> _tracked = new List<MoralBranchState>();

        /// <summary>Register a survivor for moral-branching tracking.</summary>
        public void Register(MoralBranchState state)
        {
            if (state == null) return;
            if (!_tracked.Contains(state))
                _tracked.Add(state);
        }

        /// <summary>Unregister a survivor.</summary>
        public void Unregister(MoralBranchState state)
        {
            _tracked.Remove(state);
        }

        /// <summary>Get tracked moral branch state by survivor ID.</summary>
        public MoralBranchState? GetState(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            return _tracked.Find(s => string.Equals(s.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Register a moral choice made by the player that affects a survivor.
        /// Choices are categorized as empathy-driven or pragmatism-driven.
        /// </summary>
        public void RegisterMoralChoice(MoralBranchState state, bool isEmpathyChoice)
        {
            if (state == null || !state.IsAlive) return;
            if (state.HasMoralBranch) return; // already branched, locked in

            state.MoralChoiceCount++;

            if (state.MoralChoiceCount >= ChoicesToBranch)
                DecideBranch(state, isEmpathyChoice);
        }

        private void DecideBranch(MoralBranchState state, bool lastChoiceIsEmpathy)
        {
            if (lastChoiceIsEmpathy)
            {
                state.BranchDirection = MoralBranchDirection.BurdenedCompassion;
                state.BurdenedCompassionLevel = CompassionBaseLevel;
            }
            else
            {
                state.BranchDirection = MoralBranchDirection.NumbedResilience;
                state.NumbedResilienceLevel = NumbedBaseLevel;
            }
            OnBranchDecided?.Invoke(state, state.BranchDirection);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Apply the Burdened Compassion shelter-wide morale buff.
        /// Call when a burdened-compassion survivor helps another.
        /// </summary>
        public void OnHelpedOthers(MoralBranchState state)
        {
            if (state == null || state.BranchDirection != MoralBranchDirection.BurdenedCompassion)
                return;
            ApplyShelterMoraleDelta?.Invoke(BurdenedCompassionShelterMoraleBuff);
            OnBurdenedCompassionActivated?.Invoke(state, BurdenedCompassionShelterMoraleBuff);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Apply the Burdened Compassion tragedy penalty.
        /// Call when a tragedy occurs (death, raid loss, etc.).
        /// </summary>
        public void OnTragedyWitnessed(MoralBranchState state)
        {
            if (state == null || !state.IsAlive) return;
            if (state.BranchDirection == MoralBranchDirection.BurdenedCompassion)
            {
                ApplyMoraleDelta?.Invoke(state, BurdenedCompassionTragedyPenalty);
                OnStateChanged?.Invoke();
            }
            // Numbed: no effect — immune
        }

        /// <summary>
        /// Check if a numbed survivor blocks a comfort action.
        /// </summary>
        public bool IsComfortBlocked(MoralBranchState state)
        {
            if (state == null) return false;
            if (state.BranchDirection == MoralBranchDirection.NumbedResilience)
            {
                OnNumbedComfortBlocked?.Invoke(state);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if a numbed survivor is immune to death morale loss.
        /// </summary>
        public bool IsDeathMoraleImmune(MoralBranchState state)
        {
            return state != null &&
                state.BranchDirection == MoralBranchDirection.NumbedResilience;
        }

        /// <summary>
        /// Get the effective shelter morale modifier from all burdened-compassion
        /// survivors currently alive.
        /// </summary>
        public float GetShelterMoraleBuff(IReadOnlyList<MoralBranchState> survivors)
        {
            if (survivors == null) return 0f;
            float buff = 0f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive &&
                    sv.BranchDirection == MoralBranchDirection.BurdenedCompassion)
                {
                    buff += BurdenedCompassionShelterMoraleBuff *
                        sv.BurdenedCompassionLevel;
                }
            }
            return buff;
        }

        /// <summary>Overload that uses the internally tracked survivor list.</summary>
        public float GetShelterMoraleBuff()
        {
            return GetShelterMoraleBuff(_tracked);
        }

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>Capture a deep-copy snapshot of all tracked survivor states.</summary>
        public MoralBranchingSaveState CaptureState()
        {
            var save = new MoralBranchingSaveState();
            for (int i = 0; i < _tracked.Count; i++)
                save.Survivors.Add(CopyState(_tracked[i]));
            return save;
        }

        /// <summary>Restore from a saved snapshot. Clears and re-registers all survivors.</summary>
        public void RestoreState(MoralBranchingSaveState saved)
        {
            _tracked.Clear();
            if (saved == null || saved.Survivors == null)
            {
                OnStateChanged?.Invoke();
                return;
            }
            for (int i = 0; i < saved.Survivors.Count; i++)
            {
                var copy = CopyState(saved.Survivors[i]);
                _tracked.Add(copy);
            }
            OnStateChanged?.Invoke();
        }

        /// <summary>Restore into externally-owned state objects matched by SurvivorId.
        /// Useful when the host already has its survivor objects and wants to patch them
        /// from a save rather than replacing them with the system's copies.</summary>
        public void RestoreStateInto(IList<MoralBranchState> targets, MoralBranchingSaveState saved)
        {
            if (targets == null || saved == null || saved.Survivors == null) return;
            for (int i = 0; i < saved.Survivors.Count; i++)
            {
                var src = saved.Survivors[i];
                for (int j = 0; j < targets.Count; j++)
                {
                    if (string.Equals(targets[j].SurvivorId, src.SurvivorId, StringComparison.Ordinal))
                    {
                        targets[j].MoralChoiceCount = src.MoralChoiceCount;
                        targets[j].BranchDirection = src.BranchDirection;
                        targets[j].NumbedResilienceLevel = src.NumbedResilienceLevel;
                        targets[j].BurdenedCompassionLevel = src.BurdenedCompassionLevel;
                        break;
                    }
                }
            }
            OnStateChanged?.Invoke();
        }

        private static MoralBranchState CopyState(MoralBranchState s)
        {
            if (s == null) return new MoralBranchState();
            return new MoralBranchState
            {
                SurvivorId = s.SurvivorId,
                IsAlive = s.IsAlive,
                MoralChoiceCount = s.MoralChoiceCount,
                BranchDirection = s.BranchDirection,
                NumbedResilienceLevel = s.NumbedResilienceLevel,
                BurdenedCompassionLevel = s.BurdenedCompassionLevel
            };
        }
    }
}
