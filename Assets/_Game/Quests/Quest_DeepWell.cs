using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_deep_well — Water From Below (Shelter quest).
    /// Stage 1: Water supply failing; depletion projected in 12 days.
    /// Stage 2: The Architect identifies a possible aquifer 40 m below
    ///          the sub-pen. Drilling requires Project_DeepWell.
    /// Stage 3: Gather concrete_patch_mix x 8, copper_tubing_1m x 6,
    ///          bearing_set_industrial x 2, generator_parts x 1.
    /// Stage 4: Excavation 8 days. Structural integrity drops 15 %.
    ///          Noise attracts attention. One raid guaranteed.
    /// Stage 5: Hits water. Clean. Cold. Someone fills a glass and
    ///          drinks without boiling, filtering, or praying.
    /// </summary>
    public class Quest_DeepWell : QuestRuntime
    {
        public const string Id = "quest_deep_well";
        public const string ProjectId = "Project_DeepWell";
        public const string DaysKey = "excavation_days";
        public const float DaysRequired = 8f;
        public const string RaidGuaranteedKey = "raid_guaranteed";
        public const string IntegrityKey = "structural_integrity_loss";

        public Quest_DeepWell()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "Water From Below",
                Description = "The water is failing. Below the sub-pen, somewhere, an aquifer is waiting. If the noise doesn't kill you, the cave-in might.",
                FactionId = "", // shelter, not faction
                MaxStages = 5
            };
        }

        protected override void OnBegin()
        {
            SetProgress(DaysKey, 0);
            SetProgress(IntegrityKey, 0);
            SetProgress(RaidGuaranteedKey, 0);
        }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 4:
                    RecordMoralEntry?.Invoke("excavation began. the bunker shakes. something will hear.");
                    break;
            }
        }

        protected override void OnSuccess()
        {
            RecordMoralEntry?.Invoke("the well hit water. clean. cold. someone filled a glass without boiling, filtering, or praying.");
        }

        protected override void OnFailure() { }

        public void ResolveProjectApproved()
        {
            // Host validates materials and starts excavation.
            Advance();
        }

        public void RecordExcavationDay()
        {
            float d = GetProgress(DaysKey) + 1f;
            SetProgress(DaysKey, d);
            SetProgress(IntegrityKey, 15f * (d / DaysRequired));
            if (d >= DaysRequired)
            {
                if (GetProgress(RaidGuaranteedKey) < 0.5f) SetProgress(RaidGuaranteedKey, 1f);
                Advance();
            }
        }
    }
}
