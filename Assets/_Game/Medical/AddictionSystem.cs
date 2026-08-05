using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Addiction & Withdrawal Pipeline (Prompt #7). Morphine, Anti-Rads, and
    /// other tagged chems carry an Addictive tag. Consuming an Addictive chem
    /// three times within a rolling 7-day window grants the Addicted trait.
    /// After 48 hours without a dose while Addicted, the survivor enters
    /// Withdrawal: Utility AI overrides to SearchForChems (destroying other
    /// items in panic), followed by crippling WithdrawalSickness.
    ///
    /// Plain C# system; needs Inventory + AI hooks injected (same pattern as
    /// MentalBreakSystem.BingeEatHandler). Owns Survivor.ConsumptionHistory,
    /// Survivor.HoursSinceLastDose, Survivor.IsInWithdrawal, and the Addicted
    /// trait.
    /// </summary>
    public class AddictionSystem
    {
        private Survivors.PersonalQuestSystem _personalQuests;

        /// <summary>Prompt #247 — Chem-Resistant: never re-addict from stims.</summary>
        public void BindPersonalQuests(Survivors.PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        /// <summary>Snake_case trait id added when the survivor becomes addicted.</summary>
        public const string AddictedTraitId = "addicted";

        /// <summary>Snake_case trauma id applied after a full withdrawal cycle.</summary>
        public const string WithdrawalTraumaId = "withdrawal_crash";

        /// <summary>Number of doses within the rolling window to trigger addiction.</summary>
        public const int DosesToAddict = 3;

        /// <summary>Rolling window in days for counting doses.</summary>
        public const float AddictWindowDays = 7f;

        /// <summary>Hours without a dose (while Addicted) before Withdrawal begins.</summary>
        public const float WithdrawalThresholdHours = 48f;

        /// <summary>Morale drained per hour during active withdrawal.</summary>
        public const float WithdrawalMoraleDrainPerHour = 2f;

        /// <summary>Health drained per hour during active withdrawal.</summary>
        public const float WithdrawalHealthDrainPerHour = 0.5f;

        /// <summary>Fatigue penalty per hour during withdrawal (shaking, insomnia).</summary>
        public const float WithdrawalFatigueDrainPerHour = 1.5f;

        /// <summary>Fired when a survivor gains the Addicted trait.</summary>
        public event Action<Survivors.Survivor> OnAddicted;

        /// <summary>Fired when withdrawal begins.</summary>
        public event Action<Survivors.Survivor> OnWithdrawalStarted;

        /// <summary>Fired when withdrawal ends (survivor takes a dose or recovers naturally).</summary>
        public event Action<Survivors.Survivor> OnWithdrawalEnded;

        /// <summary>
        /// Host hook: panic-destroy items during withdrawal. Injected by
        /// GameBootstrap so Medical stays free of Inventory refs.
        /// Signature: (survivor, rng) => true if items were destroyed.
        /// </summary>
        public Func<Survivors.Survivor, System.Random, bool> PanicDestroyHandler;

        private readonly System.Random _rng;

        /// <summary>Set of item ids that carry the addictive tag.
        /// Populated by GameBootstrap from item catalog data.</summary>
        public readonly HashSet<string> AddictiveItemIds = new HashSet<string>();

        /// <summary>Chance per withdrawal tick that the survivor destroys items.</summary>
        public const float PanicDestroyChancePerHour = 0.25f;

        /// <summary>Prompt #59 — hours of continuous withdrawal required to break addiction.</summary>
        public const float RecoveryHours = 336f; // 14 days

        /// <summary>Fired when a survivor breaks their addiction after surviving full withdrawal.</summary>
        public event Action<Survivors.Survivor> OnAddictionBroken;

        /// <summary>Per-survivor hours spent in continuous withdrawal (toward recovery).</summary>
        private readonly Dictionary<string, float> _recoveryHours = new Dictionary<string, float>();

        public AddictionSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random();
        }

        /// <summary>
        /// Register an item id as addictive (e.g. "morphine", "anti_rad").
        /// Called by GameBootstrap during initialization.
        /// </summary>
        public void RegisterAddictiveItem(string itemId)
        {
            if (!string.IsNullOrEmpty(itemId))
                AddictiveItemIds.Add(itemId);
        }

        /// <summary>True if the given item id is registered as addictive.</summary>
        public bool IsAddictive(string itemId)
        {
            return !string.IsNullOrEmpty(itemId) && AddictiveItemIds.Contains(itemId);
        }

        // -----------------------------------------------------------------
        // Consumption — call when a survivor uses an item
        // -----------------------------------------------------------------

        /// <summary>
        /// Log consumption of an item. If it's addictive, update the survivor's
        /// history and check for addiction/withdrawal state changes. Call from
        /// the item-use pathway (Inventory consume or Medical treatment).
        /// </summary>
        public void OnItemConsumed(Survivors.Survivor sv, string itemId, int currentDay)
        {
            if (sv == null || string.IsNullOrEmpty(itemId)) return;
            if (!IsAddictive(itemId)) return;
            // Prompt #247 — Chem-Resistant never gains addiction again.
            if (_personalQuests != null && _personalQuests.ImmuneToAddiction(sv))
            {
                sv.HoursSinceLastDose = 0f;
                if (sv.IsInWithdrawal)
                {
                    sv.IsInWithdrawal = false;
                    OnWithdrawalEnded?.Invoke(sv);
                }
                return;
            }

            // Ensure list is initialized (JsonUtility leaves it null on empty save)
            if (sv.ConsumptionHistory == null)
                sv.ConsumptionHistory = new System.Collections.Generic.List<Survivors.ConsumptionRecord>();

            // Log the consumption
            sv.ConsumptionHistory.Add(new Survivors.ConsumptionRecord(itemId, currentDay));

            // #267 Cold Turkey: any addictive dose counts as a chem day.
            _personalQuests?.NotifyChemUsed(sv);

            // Prune old records outside the rolling window
            PruneHistory(sv, currentDay);

            // Reset withdrawal state on dose — also resets recovery progress.
            if (sv.IsInWithdrawal)
            {
                sv.IsInWithdrawal = false;
                sv.HoursSinceLastDose = 0f;
                _recoveryHours.Remove(sv.Id);
                OnWithdrawalEnded?.Invoke(sv);
            }
            else
            {
                sv.HoursSinceLastDose = 0f;
            }

            // Check for addiction threshold
            if (!sv.HasTrait(AddictedTraitId))
            {
                int recentDoses = CountRecentDoses(sv, currentDay);
                if (recentDoses >= DosesToAddict)
                {
                    sv.Traits.Add(AddictedTraitId);
                    OnAddicted?.Invoke(sv);
                }
            }
        }

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance the system by <paramref name="gameHours"/>. Drives withdrawal
        /// state, withdrawal needs drain, and panic item destruction.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<Survivors.Survivor> survivors, int currentDay)
        {
            if (gameHours <= 0f || survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                TickSurvivor(sv, gameHours, currentDay);
            }
        }

        private void TickSurvivor(Survivors.Survivor sv, float gameHours, int currentDay)
        {
            // Prune old history entries
            PruneHistory(sv, currentDay);

            if (!sv.HasTrait(AddictedTraitId)) return;

            sv.HoursSinceLastDose += gameHours;

            if (sv.HoursSinceLastDose >= WithdrawalThresholdHours)
            {
                if (!sv.IsInWithdrawal)
                {
                    sv.IsInWithdrawal = true;
                    OnWithdrawalStarted?.Invoke(sv);
                }

                // Active withdrawal drains needs
                if (sv.Needs != null)
                {
                    sv.Needs.Morale = Mathf.Clamp(
                        sv.Needs.Morale - WithdrawalMoraleDrainPerHour * gameHours, 0f, 100f);
                    sv.Needs.Health = Mathf.Clamp(
                        sv.Needs.Health - WithdrawalHealthDrainPerHour * gameHours, 0f, sv.MaxHealthCap);
                    sv.Needs.Fatigue = Mathf.Clamp(
                        sv.Needs.Fatigue + WithdrawalFatigueDrainPerHour * gameHours, 0f, 100f);
                }

                // Accumulate recovery hours toward breaking the addiction.
                float prevRecovery = _recoveryHours.TryGetValue(sv.Id, out float rh) ? rh : 0f;
                float newRecovery = prevRecovery + gameHours;
                _recoveryHours[sv.Id] = newRecovery;

                // After 14 days (336h) of continuous withdrawal, addiction breaks.
                if (prevRecovery < RecoveryHours && newRecovery >= RecoveryHours)
                {
                    sv.Traits.Remove(AddictedTraitId);
                    sv.IsInWithdrawal = false;
                    sv.HoursSinceLastDose = 0f;
                    _recoveryHours.Remove(sv.Id);
                    if (!sv.HasTrauma(WithdrawalTraumaId))
                        sv.Traumas.Add(WithdrawalTraumaId);
                    OnAddictionBroken?.Invoke(sv);
                    OnWithdrawalEnded?.Invoke(sv);
                }

                // Panic item destruction
                if (PanicDestroyHandler != null && _rng.NextDouble() < PanicDestroyChancePerHour * gameHours)
                {
                    PanicDestroyHandler(sv, _rng);
                }
            }
            else
            {
                // Not in withdrawal: decay recovery hours.
                _recoveryHours.Remove(sv.Id);
            }
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private void PruneHistory(Survivors.Survivor sv, int currentDay)
        {
            if (sv.ConsumptionHistory == null) return;
            // Rolling 7-day window: keep entries from (currentDay - 6) through currentDay.
            int cutoffDay = currentDay - (int)AddictWindowDays + 1;
            for (int i = sv.ConsumptionHistory.Count - 1; i >= 0; i--)
            {
                if (sv.ConsumptionHistory[i].DayConsumed < cutoffDay)
                {
                    sv.ConsumptionHistory.RemoveAt(i);
                }
            }
        }

        private int CountRecentDoses(Survivors.Survivor sv, int currentDay)
        {
            if (sv.ConsumptionHistory == null) return 0;
            int cutoffDay = currentDay - (int)AddictWindowDays + 1;
            int count = 0;
            for (int i = 0; i < sv.ConsumptionHistory.Count; i++)
            {
                if (sv.ConsumptionHistory[i].DayConsumed >= cutoffDay)
                    count++;
            }
            return count;
        }
    }
}
