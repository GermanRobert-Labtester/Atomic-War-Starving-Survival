using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_elena_triage — The Hands That Don't Shake
    /// Personal quest for Elena Vasquez. Five successful medical
    /// treatments without a patient dying. Death under her care costs
    /// her -30 morale and a 2-day treatment refusal.
    ///
    /// Reward: perk_field_triage — all medical actions take 30 % less time.
    /// Failure (3 patient deaths): affliction_survivors_guilt permanently.
    /// </summary>
    public class Quest_ElenaTriage : QuestRuntime
    {
        public const string Id = "quest_elena_triage";
        public const string SuccessKey = "treatments_successful";
        public const string DeathsKey = "patient_deaths";
        public const string RefusalDaysKey = "refusal_days";
        public const string Owner = "survivor_elena_vasquez";
        public const string RewardPerk = "perk_field_triage";
        public const string FailureAffliction = "affliction_survivors_guilt";

        public Quest_ElenaTriage()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Hands That Don't Shake",
                Description = "Five successful medical treatments. No patient dies on her table.",
                Personal = true,
                OwnerSurvivorId = Owner,
                MaxStages = 1   // single-stage counter quest
            };
        }

        protected override void OnBegin()
        {
            SetProgress(SuccessKey, 0);
            SetProgress(DeathsKey, 0);
            SetProgress(RefusalDaysKey, 0);
        }

        protected override void OnStageEnter(int stage) { }

        protected override void OnSuccess()
        {
            GrantPerk?.Invoke(null, RewardPerk, 1);
            RecordMoralEntry?.Invoke("elena's hands stopped shaking. she earned Field Triage.");
        }

        protected override void OnFailure()
        {
            AddAffliction?.Invoke(null, FailureAffliction);
            RecordMoralEntry?.Invoke("elena lost three. she carries it now, permanently.");
        }

        // ── Host-invoked recorders ───────────────────────────────────────
        public void RecordTreatmentSuccess(string elenaId)
        {
            int s = Mathf.FloorToInt(GetProgress(SuccessKey) + 1);
            SetProgress(SuccessKey, s);
            if (s >= 5) Complete();
        }

        public void RecordPatientDiedUnderCare(string elenaId)
        {
            int d = Mathf.FloorToInt(GetProgress(DeathsKey) + 1);
            SetProgress(DeathsKey, d);
            ApplyMorale?.Invoke(elenaId, -30f);
            SetProgress(RefusalDaysKey, 2);
            if (d >= 3) Fail();
        }

        public bool IsRefusingToday()
        {
            float r = GetProgress(RefusalDaysKey);
            if (r <= 0f) return false;
            SetProgress(RefusalDaysKey, r - 1f);
            return true;
        }
    }
}
