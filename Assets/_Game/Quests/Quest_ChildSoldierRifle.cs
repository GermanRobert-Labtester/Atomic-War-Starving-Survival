using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_child_soldier_rifle — The Rifle Will Not Leave
    /// The Child Soldier will not give up their weapon. Any attempt to
    /// confiscate it triggers a Panic Action and Morale collapse.
    /// Stage 1: Therapist or Empath must spend 3 consecutive days
    ///          talking. No other survivor may intervene.
    /// Stage 2: The child puts the rifle down for one hour. If any
    ///          survivor touches it, the quest resets.
    /// Stage 3: After 5 days of voluntary disarmament, the child asks
    ///          to learn something else. Assign to a non-combat skill.
    /// Reward: perk_medic_apprentice or perk_garden_tender. Rifle goes
    ///          into storage.
    /// Failure: If the bunker is raided during the quest, the child
    ///          picks up the rifle permanently. The quest is locked.
    /// </summary>
    public class Quest_ChildSoldierRifle : QuestRuntime
    {
        public const string Id = "quest_child_soldier_rifle";
        public const string Owner = "survivor_the_child_soldier";
        public const string TalkDaysKey = "consecutive_talk_days";
        public const string DisarmamentDaysKey = "voluntary_disarmament_days";
        public const string LearnedSkillKey = "learned_skill";
        public const string RifleItem = "rifle_field";

        public Quest_ChildSoldierRifle()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Rifle Will Not Leave",
                Description = "The child will not give up the rifle. Touching it is a panic action. This will be a long conversation.",
                Personal = true,
                OwnerSurvivorId = Owner,
                MaxStages = 3
            };
        }

        protected override void OnBegin() { SetProgress(TalkDaysKey, 0); }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 2: SetProgress(DisarmamentDaysKey, 0); break;
                case 3:
                    OfferChoice("learn_medical");
                    OfferChoice("learn_gardening");
                    break;
            }
        }

        protected override void OnSuccess()
        {
            string skill = GetProgress(LearnedSkillKey) > 0.5f ? "perk_garden_tender" : "perk_medic_apprentice";
            GrantPerk?.Invoke(Owner, skill, 1);
            TakeItem?.Invoke(Owner, RifleItem, 1);
            RecordMoralEntry?.Invoke("the rifle is in storage. the child is learning something else.");
        }

        protected override void OnFailure()
        {
            RecordMoralEntry?.Invoke("the child picked up the rifle during the raid. it will not leave now.");
            Lock();
        }

        public void RecordTalkDay(string therapistId)
        {
            float d = GetProgress(TalkDaysKey) + 1;
            SetProgress(TalkDaysKey, d);
            if (d >= 3) Advance();
        }

        public void RecordDisarmamentDay()
        {
            float d = GetProgress(DisarmamentDaysKey) + 1;
            SetProgress(DisarmamentDaysKey, d);
            if (d >= 5) Advance();
        }

        public void ResolveChooseMedical()
        {
            SetProgress(LearnedSkillKey, 0f);
            Complete();
        }

        public void ResolveChooseGardening()
        {
            SetProgress(LearnedSkillKey, 1f);
            Complete();
        }

        public void ResetOnTouch()
        {
            SetProgress(TalkDaysKey, 0);
            SetProgress(DisarmamentDaysKey, 0);
            State.Stage = 1;
            OnStageEnter(1);
        }

        public void OnRaidDuringQuest()
        {
            // Per spec: rifle picked up permanently. Quest locked.
            Fail();
        }
    }
}
