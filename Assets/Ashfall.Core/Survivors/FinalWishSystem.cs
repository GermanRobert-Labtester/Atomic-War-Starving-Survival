// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    // ── Save-state DTOs ────────────────────────────────────────────────
    [Serializable]
    public sealed class FinalWishSurvivorState
    {
        public string survivorId = string.Empty;
        public string wishType = string.Empty;
        /// <summary>
        /// The specific authored wish id drawn from the archetype's pool (e.g.
        /// <c>wish_surgeon_final_surgery</c>), empty when no catalog is bound or for
        /// legacy saves. Surfaced to the host for authored title/description text.
        /// </summary>
        public string wishId = string.Empty;
        public float daysRemaining;
        public int stepsCompleted;
        public bool isActive;
        public bool hasTerminalPrognosis;
        public bool wishCompleted;
    }

    [Serializable]
    public sealed class FinalWishSaveState
    {
        public List<FinalWishSurvivorState> survivors = new List<FinalWishSurvivorState>();
        public Dictionary<string, string> archetypeWishes = new Dictionary<string, string>();
    }

    /// <summary>
    /// Final Wish System — when a survivor contracts terminal radiation
    /// poisoning, a personal "Final Request" questline opens. Completing
    /// it before their demise grants the bunker permanent morale bonuses.
    ///
    /// Engine-agnostic port: uses string survivor IDs, raises C# events on state
    /// change, and is save/load safe via CaptureState/RestoreState (deep copy).
    /// Host injects morale and RNG callbacks.
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
        public event Action<string, string, float> OnTerminalPrognosisDeclared;
        // survivorId, wishId, daysRemaining
        public event Action<string, string> OnFinalWishStepCompleted;
        // survivorId, stepId
        public event Action<string> OnFinalWishCompleted;
        public event Action<string> OnFinalWishFailed;
        public event Action<float> OnPermanentMoraleBuffApplied;
        public event Action OnStateChanged;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<float> ApplyPermanentShelterMoraleBuff;
        public Func<string, string> GetWishNarrativeText;
        // wishId → localized text
        public ISeededRng Rng;

        /// <summary>
        /// Optional authored-wish catalog. When bound, a terminal prognosis draws a
        /// specific <c>wishId</c> from the archetype's pool (seeded, deterministic) so
        /// each elder death surfaces a distinct authored moment. Null → the system
        /// behaves exactly as before (wishType only, no authored text).
        /// </summary>
        public IFinalWishCatalog? Catalog;

        // ── State ──────────────────────────────────────────────────────
        /// <summary>ArchetypeId → wish type.</summary>
        private readonly Dictionary<string, string> _archetypeWishes =
            new Dictionary<string, string>(StringComparer.Ordinal);
        /// <summary>Wish progress per survivor.</summary>
        private readonly Dictionary<string, FinalWishSurvivorState> _wishStates =
            new Dictionary<string, FinalWishSurvivorState>(StringComparer.Ordinal);

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Register a wish type for an archetype.
        /// </summary>
        public void RegisterWish(string archetypeId, string wishType)
        {
            if (string.IsNullOrEmpty(archetypeId)) return;
            _archetypeWishes[archetypeId] = wishType ?? string.Empty;
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Declare a terminal prognosis for a survivor — activates their
        /// final wish questline. Called by PrognosisPipeline when Manifest
        /// resolves with fatal outcome.
        /// </summary>
        public void DeclareTerminalPrognosis(string survivorId, string archetypeId, bool isAlive)
        {
            if (string.IsNullOrEmpty(survivorId) || !isAlive) return;
            if (_wishStates.TryGetValue(survivorId, out var existing) && existing.hasTerminalPrognosis)
                return; // already declared

            float daysRemaining = DefaultPrognosisDaysMin +
                (float)((Rng?.NextDouble() ?? 0.5) *
                (DefaultPrognosisDaysMax - DefaultPrognosisDaysMin));

            string wishType = GetWishForArchetype(archetypeId);
            string wishId = ResolveWishIdForArchetype(archetypeId);
            var state = new FinalWishSurvivorState
            {
                survivorId = survivorId,
                wishType = wishType,
                wishId = wishId,
                daysRemaining = daysRemaining,
                stepsCompleted = 0,
                isActive = true,
                hasTerminalPrognosis = true,
                wishCompleted = false
            };
            _wishStates[survivorId] = state;

            OnTerminalPrognosisDeclared?.Invoke(survivorId, wishType, daysRemaining);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Advance a step in the final wish questline.
        /// Returns true if this completed the wish.
        /// </summary>
        public bool AdvanceWishStep(string survivorId, string stepId)
        {
            if (!_wishStates.TryGetValue(survivorId, out var state)) return false;
            if (!state.hasTerminalPrognosis || !state.isActive) return false;

            state.stepsCompleted++;
            OnFinalWishStepCompleted?.Invoke(survivorId, stepId);

            // Prefer the authored step count when a catalog entry is bound; fall back
            // to the wish-type switch otherwise (backward compatible, tested).
            int requiredSteps = RequiredStepsFor(state);

            if (state.stepsCompleted >= requiredSteps)
            {
                CompleteWish(survivorId, state);
                return true;
            }

            OnStateChanged?.Invoke();
            return false;
        }

        private void CompleteWish(string survivorId, FinalWishSurvivorState state)
        {
            state.wishCompleted = true;
            state.isActive = false;
            ApplyPermanentShelterMoraleBuff?.Invoke(WishCompletedMoraleBuff);
            OnFinalWishCompleted?.Invoke(survivorId);
            OnPermanentMoraleBuffApplied?.Invoke(WishCompletedMoraleBuff);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Resolve the number of steps required to complete a survivor's wish.
        /// Uses the authored catalog entry's step count when available; otherwise
        /// falls back to the wish-type switch (the historical behavior).
        /// </summary>
        private int RequiredStepsFor(FinalWishSurvivorState state)
        {
            if (Catalog != null && !string.IsNullOrEmpty(state.wishId))
            {
                var entry = Catalog.GetEntry(state.wishId);
                if (entry != null && entry.steps != null && entry.steps.Count > 0)
                    return entry.steps.Count;
            }

            return state.wishType switch
            {
                WishRetrieveHeirloom => 2,
                WishDeliverLetter => 2,
                WishBuildMemorial => 3,
                WishTeachLesson => 2,
                WishReconcile => 2,
                WishSeeTheSky => 1,
                _ => 2
            };
        }

        /// <summary>
        /// Called when the terminal prognosis timer expires without completion.
        /// </summary>
        public void OnPrognosisExpired(string survivorId)
        {
            if (!_wishStates.TryGetValue(survivorId, out var state)) return;
            if (!state.hasTerminalPrognosis) return;
            if (state.wishCompleted) return; // already completed in time

            state.isActive = false;

            // Apply the bunker-wide grief penalty for a failed wish.
            ApplyPermanentShelterMoraleBuff?.Invoke(WishFailedMoralePenalty);
            OnFinalWishFailed?.Invoke(survivorId);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Tick — count down prognosis timer.
        /// </summary>
        public void Tick(string survivorId, float gameHours, bool isAlive)
        {
            if (string.IsNullOrEmpty(survivorId) || !isAlive) return;
            if (!_wishStates.TryGetValue(survivorId, out var state)) return;
            if (!state.hasTerminalPrognosis || state.wishCompleted) return;

            state.daysRemaining -= gameHours / 24f;
            if (state.daysRemaining <= 0f)
            {
                OnPrognosisExpired(survivorId);
            }
            else
            {
                OnStateChanged?.Invoke();
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
        /// Draw a specific authored wish id from the archetype's pool using the seeded
        /// RNG, so the same seed always selects the same wish for a given archetype.
        /// Returns empty when no catalog is bound or the archetype has no pool — the
        /// caller then runs on wishType only (legacy behavior).
        /// </summary>
        private string ResolveWishIdForArchetype(string archetypeId)
        {
            if (Catalog == null || string.IsNullOrEmpty(archetypeId)) return string.Empty;
            var pool = Catalog.GetWishIdsForArchetype(archetypeId);
            if (pool == null || pool.Count == 0) return string.Empty;
            if (Rng == null) return pool[0];
            int idx = Rng.Next(0, pool.Count);
            return pool[idx];
        }

        /// <summary>
        /// Returns true if a final wish is currently active for this survivor.
        /// </summary>
        public bool HasActiveWish(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            return _wishStates.TryGetValue(survivorId, out var state) &&
                state.hasTerminalPrognosis &&
                !state.wishCompleted &&
                state.isActive;
        }

        /// <summary>
        /// Get the wish type for a survivor (empty string if none).
        /// </summary>
        public string GetWishType(string survivorId)
        {
            return _wishStates.TryGetValue(survivorId, out var state)
                ? state.wishType : string.Empty;
        }

        /// <summary>
        /// Get the authored wish id drawn for a survivor (empty if no catalog was
        /// bound or the survivor has no wish). Hosts use this to resolve authored
        /// title/description/completion text from <see cref="IFinalWishCatalog"/>.
        /// </summary>
        public string GetWishId(string survivorId)
        {
            return _wishStates.TryGetValue(survivorId, out var state)
                ? state.wishId : string.Empty;
        }

        /// <summary>
        /// Get the days remaining for a survivor's prognosis.
        /// </summary>
        public float GetDaysRemaining(string survivorId)
        {
            return _wishStates.TryGetValue(survivorId, out var state)
                ? state.daysRemaining : 0f;
        }

        /// <summary>
        /// Get the number of steps completed for a survivor's wish.
        /// </summary>
        public int GetStepsCompleted(string survivorId)
        {
            return _wishStates.TryGetValue(survivorId, out var state)
                ? state.stepsCompleted : 0;
        }

        /// <summary>
        /// Check whether a survivor has a terminal prognosis.
        /// </summary>
        public bool HasTerminalPrognosis(string survivorId)
        {
            return _wishStates.TryGetValue(survivorId, out var state) &&
                state.hasTerminalPrognosis;
        }

        /// <summary>
        /// Check whether a survivor has completed their final wish.
        /// </summary>
        public bool HasCompletedWish(string survivorId)
        {
            return _wishStates.TryGetValue(survivorId, out var state) &&
                state.wishCompleted;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public FinalWishSaveState CaptureState()
        {
            var save = new FinalWishSaveState();
            foreach (var kv in _wishStates)
            {
                var s = kv.Value;
                save.survivors.Add(new FinalWishSurvivorState
                {
                    survivorId = s.survivorId,
                    wishType = s.wishType,
                    wishId = s.wishId,
                    daysRemaining = s.daysRemaining,
                    stepsCompleted = s.stepsCompleted,
                    isActive = s.isActive,
                    hasTerminalPrognosis = s.hasTerminalPrognosis,
                    wishCompleted = s.wishCompleted
                });
            }
            foreach (var kv in _archetypeWishes)
                save.archetypeWishes[kv.Key] = kv.Value;
            return save;
        }

        public void RestoreState(FinalWishSaveState save)
        {
            _wishStates.Clear();
            _archetypeWishes.Clear();
            if (save == null) return;

            if (save.archetypeWishes != null)
            {
                foreach (var kv in save.archetypeWishes)
                    _archetypeWishes[kv.Key] = kv.Value;
            }

            if (save.survivors != null)
            {
                foreach (var s in save.survivors)
                {
                    if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                    _wishStates[s.survivorId] = new FinalWishSurvivorState
                    {
                        survivorId = s.survivorId,
                        wishType = s.wishType,
                        wishId = s.wishId,
                        daysRemaining = s.daysRemaining,
                        stepsCompleted = s.stepsCompleted,
                        isActive = s.isActive,
                        hasTerminalPrognosis = s.hasTerminalPrognosis,
                        wishCompleted = s.wishCompleted
                    };
                }
            }
            OnStateChanged?.Invoke();
        }
    }
}
