using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Final Wish System — when a survivor contracts terminal radiation
    /// poisoning, a personal "Final Request" questline opens. Completing
    /// it before their demise grants the bunker permanent morale bonuses.
    ///
    /// Each survivor archetype has a unique wish: retrieve family heirloom,
    /// deliver final letter, build memorial, teach final lesson.
    ///
    /// Owns: Survivor.HasTerminalPrognosis, Survivor.TerminalPrognosisDaysRemaining,
    /// Survivor.FinalWishCompleted.
    ///
    /// Plain C#, leaf assembly.
    /// </summary>
    public class FinalWishSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float WishCompletedMoraleBuff = 15f;
        public const float WishFailedMoralePenalty = -10f;
        public const string BuffId = "their_memory_lives_on";
        public const float DefaultPrognosisDaysMin = 3f;
        public const float DefaultPrognosisDaysMax = 7f;

        // ── Wish archetypes ────────────────────────────────────────────
        public const string WishRetrieveHeirloom = "retrieve_heirloom";
        public const string WishDeliverLetter = "deliver_letter";
        public const string WishBuildMemorial = "build_memorial";
        public const string WishTeachLesson = "teach_lesson";
        public const string WishReconcile = "reconcile";
        public const string WishSeeTheSky = "see_the_sky";

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, string, float> OnTerminalPrognosisDeclared;
        // sv, wishId, daysRemaining
        public event Action<Survivor, string> OnFinalWishStepCompleted;
        // sv, stepId
        public event Action<Survivor> OnFinalWishCompleted;
        public event Action<Survivor> OnFinalWishFailed;
        public event Action<float> OnPermanentMoraleBuffApplied;

        // ── State ──────────────────────────────────────────────────────
        /// <summary>ArchetypeId → wish type.</summary>
        private readonly Dictionary<string, string> _archetypeWishes =
            new Dictionary<string, string>();
        /// <summary>Wish progress per survivor.</summary>
        private readonly Dictionary<string, FinalWishState> _wishStates =
            new Dictionary<string, FinalWishState>();

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<float> ApplyPermanentShelterMoraleBuff;
        public Func<string, string> GetWishNarrativeText;
        // wishId → localized text
        public System.Random Rng;

        /// <summary>
        /// Register a wish type for an archetype.
        /// </summary>
        public void RegisterWish(string archetypeId, string wishType)
        {
            _archetypeWishes[archetypeId] = wishType;
        }

        /// <summary>
        /// Declare a terminal prognosis for a survivor — activates their
        /// final wish questline. Called by PrognosisPipeline when Manifest
        /// resolves with fatal outcome.
        /// </summary>
        public void DeclareTerminalPrognosis(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return;
            if (sv.HasTerminalPrognosis) return; // already declared

            sv.HasTerminalPrognosis = true;
            sv.TerminalPrognosisDaysRemaining = DefaultPrognosisDaysMin +
                (float)((Rng?.NextDouble() ?? 0.5) *
                (DefaultPrognosisDaysMax - DefaultPrognosisDaysMin));

            string wishType = GetWishForArchetype(sv.ArchetypeId);
            var state = new FinalWishState
            {
                SurvivorId = sv.Id,
                WishType = wishType,
                DaysRemaining = sv.TerminalPrognosisDaysRemaining,
                StepsCompleted = 0,
                IsActive = true
            };
            _wishStates[sv.Id] = state;

            OnTerminalPrognosisDeclared?.Invoke(sv, wishType,
                sv.TerminalPrognosisDaysRemaining);
        }

        /// <summary>
        /// Advance a step in the final wish questline.
        /// Returns true if this completed the wish.
        /// </summary>
        public bool AdvanceWishStep(Survivor sv, string stepId)
        {
            if (sv == null || !sv.IsAlive || !sv.HasTerminalPrognosis) return false;
            if (!_wishStates.TryGetValue(sv.Id, out var state) || !state.IsActive)
                return false;

            state.StepsCompleted++;
            _wishStates[sv.Id] = state;
            OnFinalWishStepCompleted?.Invoke(sv, stepId);

            // Different wish types have different step counts
            int requiredSteps = state.WishType switch
            {
                WishRetrieveHeirloom => 2,
                WishDeliverLetter => 2,
                WishBuildMemorial => 3,
                WishTeachLesson => 2,
                WishReconcile => 2,
                WishSeeTheSky => 1,
                _ => 2
            };

            if (state.StepsCompleted >= requiredSteps)
            {
                CompleteWish(sv);
                return true;
            }
            return false;
        }

        private void CompleteWish(Survivor sv)
        {
            sv.FinalWishCompleted = true;
            ApplyPermanentShelterMoraleBuff?.Invoke(WishCompletedMoraleBuff);
            OnFinalWishCompleted?.Invoke(sv);
            OnPermanentMoraleBuffApplied?.Invoke(WishCompletedMoraleBuff);
        }

        /// <summary>
        /// Called when the terminal prognosis timer expires without completion.
        /// </summary>
        public void OnPrognosisExpired(Survivor sv)
        {
            if (sv == null || !sv.HasTerminalPrognosis) return;
            if (sv.FinalWishCompleted) return; // already completed in time

            if (_wishStates.TryGetValue(sv.Id, out var state))
            {
                state.IsActive = false;
                _wishStates[sv.Id] = state;
            }

            // Apply the bunker-wide grief penalty for a failed wish.
            ApplyPermanentShelterMoraleBuff?.Invoke(WishFailedMoralePenalty);
            OnFinalWishFailed?.Invoke(sv);
        }

        /// <summary>
        /// Tick — count down prognosis timer.
        /// </summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || !sv.IsAlive || !sv.HasTerminalPrognosis) return;
            if (sv.FinalWishCompleted) return;

            sv.TerminalPrognosisDaysRemaining -= gameHours / 24f;
            if (sv.TerminalPrognosisDaysRemaining <= 0f)
            {
                OnPrognosisExpired(sv);
            }
        }

        private string GetWishForArchetype(string archetypeId)
        {
            if (!string.IsNullOrEmpty(archetypeId) &&
                _archetypeWishes.TryGetValue(archetypeId, out var wish))
                return wish;

            // Default wishes by archetype prefix
            if (archetypeId != null)
            {
                if (archetypeId.Contains("surgeon") || archetypeId.Contains("nurse"))
                    return WishTeachLesson;
                if (archetypeId.Contains("soldier") || archetypeId.Contains("guard"))
                    return WishBuildMemorial;
                if (archetypeId.Contains("parent") || archetypeId.Contains("mother"))
                    return WishReconcile;
            }
            return WishDeliverLetter; // default
        }

        /// <summary>
        /// Returns true if a final wish is currently active for this survivor.
        /// </summary>
        public bool HasActiveWish(Survivor sv)
        {
            return sv != null && sv.HasTerminalPrognosis &&
                !sv.FinalWishCompleted &&
                _wishStates.TryGetValue(sv.Id, out var state) && state.IsActive;
        }
    }

    [System.Serializable]
    internal class FinalWishState
    {
        public string SurvivorId;
        public string WishType;
        public float DaysRemaining;
        public int StepsCompleted;
        public bool IsActive;
    }
}
