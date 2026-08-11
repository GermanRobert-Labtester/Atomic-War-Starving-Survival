using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_mechanic_highway_heart — The Last Engine
    /// The Mechanic's personal quest. There is a working engine in the
    /// highway_pileup (20 mSv/h zone with a leaking fuel tanker).
    /// Stage 1: Scout, identify engine, assess fuel leak.
    /// Stage 2: Gather tools: multitool, wrench, rubber_hose, fuel_1l x 5.
    /// Stage 3: Extract. Mechanical 70 % check. Failure: Durability -40.
    /// Stage 4: Rebuild in the bunker via craft_engine. Unlocks
    ///          VehicleSystem (Migration victory path).
    /// </summary>
    public class Quest_MechanicHighwayHeart : QuestRuntime
    {
        public const string Id = "quest_mechanic_highway_heart";
        public const string Owner = "survivor_the_mechanic";
        public const string HighwayPileup = "loc_highway_pileup";
        public const string EngineItem = "engine_block_intact";
        public const string DurabilityKey = "engine_durability";
        public const string ExtractedKey = "extracted";
        public const string RebuiltKey = "rebuilt";
        public const float RequiredMechanical = 0.70f;

        public Quest_MechanicHighwayHeart()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Last Engine",
                Description = "A working engine in the highway_pileup. The Mechanic wants it. So does the leaking fuel tanker.",
                Personal = true,
                OwnerSurvivorId = Owner,
                MaxStages = 4
            };
        }

        protected override void OnBegin() { SetProgress(DurabilityKey, 100); }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 2:
                    // host must verify items present
                    OfferChoice("have_all_tools");
                    OfferChoice("missing_tools");
                    break;
                case 3:
                    OfferChoice("attempt_extraction");
                    break;
                case 4:
                    OfferChoice("craft_engine");
                    break;
            }
        }

        protected override void OnSuccess()
        {
            // VehicleSystem unlock: host listens to OnStatusChanged and
            // activates the migration path when this quest completes.
            RecordMoralEntry?.Invoke("the engine is in. the shelter can move.");
        }

        protected override void OnFailure() { }

        public void ResolveAttemptExtraction(float mechanicalSkill, System.Random rng)
        {
            float roll = (float)(rng?.NextDouble() ?? 0.5);
            if (roll <= mechanicalSkill)
            {
                GiveItem?.Invoke(null, EngineItem, 1);
                SetProgress(ExtractedKey, 1);
                Advance();
            }
            else
            {
                float dur = GetProgress(DurabilityKey) - 40f;
                SetProgress(DurabilityKey, Mathf.Max(0, dur));
                RecordMoralEntry?.Invoke("the engine came out damaged. durability -40.");
                // still advance so quest can complete via repair path
                SetProgress(ExtractedKey, 1);
                Advance();
            }
        }

        public void ResolveCraftEngine()
        {
            // host invokes craft_engine recipe; on success, Advance.
            SetProgress(RebuiltKey, 1);
            Advance();
        }
    }
}
