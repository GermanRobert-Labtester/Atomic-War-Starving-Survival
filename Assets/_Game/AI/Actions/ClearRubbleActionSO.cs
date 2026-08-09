using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Clear rubble from a sealed bunker room (Prompt #5 — Previous Tenants).
    /// The survivor spends work-hours clearing a sealed/caved-in room. As
    /// progress advances, diary fragments from the previous tenants are
    /// revealed. When complete, the room transitions to Cleared and becomes
    /// accessible.
    ///
    /// This action only scores when there is a sealed or partially-cleared
    /// room in the shelter. The player can only assign one survivor at a time
    /// per room (the AI respects this).
    /// </summary>
    [CreateAssetMenu(fileName = "NewClearRubbleAction", menuName = "ASHFALL/AI Actions/Clear Rubble")]
    public class ClearRubbleActionSO : SurvivorAction
    {
        [Header("Rubble Clearing")]
        [Tooltip("Work-hours cleared per execution tick (survivor's CraftingSkill modifies this).")]
        [Range(0.1f, 2f)]
        public float baseClearRatePerHour = 0.5f;

        [Tooltip("Base utility score when a sealed room is available.")]
        [Range(0f, 1f)]
        public float sealedRoomScore = 0.6f;

        [Tooltip("Score bonus when the room has undiscovered diary fragments.")]
        [Range(0f, 0.5f)]
        public float diaryDiscoveryBonus = 0.2f;

        /// <summary>
        /// Host hook: reveal a diary fragment from the room being cleared.
        /// Injected by GameBootstrap. Returns the diary text for journal logging.
        /// Signature: (roomId, fragmentIndex) => diaryText or null if none.
        /// </summary>
        public System.Func<string, int, string> OnDiaryRevealed;

        /// <summary>Fired when a room transitions from Clearing to Cleared.
        /// Args: (roomId).</summary>
        public event System.Action<string> OnRoomCleared;

        // -----------------------------------------------------------------
        // Scoring
        // -----------------------------------------------------------------

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            if (!context.Survivor.IsAlive) return 0f;

            // Check if there's a sealable or clearing room
            var room = FindClearingTarget(context);
            if (room == null) return 0f;

            float score = sealedRoomScore;

            // Bonus if there are undiscovered diary fragments in this room
            int undiscovered = room.DiaryFragmentIds.Count - room.RevealedDiaryIndices.Count;
            if (undiscovered > 0)
            {
                score += diaryDiscoveryBonus;
            }

            // Crafting skill makes the work faster but also more appealing
            score += context.Survivor.EffectiveCraftingSkill * 0.1f;

            // Scaled by Morale: low-morale survivors are less motivated to do hard labor
            float moraleFactor = Mathf.Lerp(0.3f, 1f, context.Survivor.Needs.Morale / 100f);
            score *= moraleFactor;

            return Mathf.Clamp01(score);
        }

        // -----------------------------------------------------------------
        // Execution
        // -----------------------------------------------------------------

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;

            var room = FindClearingTarget(context);
            if (room == null) return;

            // Calculate effective clear rate (CraftingSkill boosts it).
            // Prompt #213 — Taskmaster Pacing Aura: +15% work rate nearby.
            float pace = 1f;
            if (context.SocialPerks != null && context.GetSurvivors != null)
            {
                pace = context.SocialPerks.GetPacingAuraMultiplier(
                    context.Survivor,
                    context.GetSurvivors(),
                    context.AreRoomsAdjacent);
            }
            float effectiveRate = baseClearRatePerHour
                                  * (1f + context.Survivor.EffectiveCraftingSkill)
                                  * pace;

            if (room.UnlockState == RoomUnlockState.Sealed)
            {
                room.UnlockState = RoomUnlockState.Clearing;
            }

            room.RubbleClearHoursRemaining = Mathf.Max(0f, room.RubbleClearHoursRemaining - effectiveRate);

            // Fatigue cost for hard labor (Prompt #199 — Sandhog: half fatigue).
            float fatMult = context.ShelterPerks != null
                ? context.ShelterPerks.GetExcavationFatigueMultiplier(context.Survivor)
                : 1f;
            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 1.5f * fatMult);
            else
                context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 1.5f * fatMult, 0f, 100f);

            // Check for diary fragment reveals at progress milestones
            TryRevealDiary(room);

            // Check for completion
            if (room.RubbleClearHoursRemaining <= 0f)
            {
                room.UnlockState = RoomUnlockState.Cleared;
                context.ShelterPerks?.RecordRoomCleared(
                    context.Survivor, context.CurrentDay);
                OnRoomCleared?.Invoke(room.RoomId);
            }
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private ShelterRoom FindClearingTarget(AIContext context)
        {
            if (context.Shelter?.Rooms == null) return null;

            for (int i = 0; i < context.Shelter.Rooms.Count; i++)
            {
                var room = context.Shelter.Rooms[i];
                if (room == null) continue;
                if (room.UnlockState == RoomUnlockState.Sealed ||
                    room.UnlockState == RoomUnlockState.Clearing)
                {
                    return room;
                }
            }
            return null;
        }

        private void TryRevealDiary(ShelterRoom room)
        {
            if (room.DiaryFragmentIds == null || room.DiaryFragmentIds.Count == 0) return;

            // Ensure lists are initialized (JsonUtility leaves them null on empty save)
            if (room.RevealedDiaryIndices == null)
                room.RevealedDiaryIndices = new System.Collections.Generic.List<int>();

            // Guard against uninitialized total (0 means no diaries configured)
            float effectiveTotal = Mathf.Max(1f, room.RubbleClearHoursTotal, room.RubbleClearHoursRemaining + 0.01f);
            float progress = 1f - (room.RubbleClearHoursRemaining / effectiveTotal);
            int expectedReveals = Mathf.FloorToInt(progress * room.DiaryFragmentIds.Count);

            while (room.RevealedDiaryIndices.Count < expectedReveals &&
                   room.RevealedDiaryIndices.Count < room.DiaryFragmentIds.Count)
            {
                int nextIndex = room.RevealedDiaryIndices.Count; // sequential reveal
                room.RevealedDiaryIndices.Add(nextIndex);

                if (nextIndex < room.DiaryFragmentIds.Count)
                {
                    string fragmentId = room.DiaryFragmentIds[nextIndex];
                    OnDiaryRevealed?.Invoke(room.RoomId, nextIndex);
                }
            }
        }
    }
}
