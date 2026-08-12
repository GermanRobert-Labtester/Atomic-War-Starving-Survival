using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_black_vein — The Black Vein (Expansion II questline).
    ///
    /// Stage 1: DeepWellProject draws water that smells of sulfur and
    ///          almonds. WaterPurifierModuleSO degrades 300% faster.
    /// Stage 2: Dispatch team to location_municipal_sewage. Discover
    ///          collapsed bulkhead leaking the Black Aquifer.
    /// Stage 3: The Choice —
    ///   A) Seal the Bulkhead: Requires 3x cement_mix + 2x shoring_timber.
    ///      Saves the well but seals a trapped Sump-Dredger.
    ///   B) Reroute the Flow: Use high_pressure_hose to vent sludge into
    ///      surface ash-swamps. Saves the Dredger but poisons Biome_AshSwamp,
    ///      permanently destroying location_marta_farmhouse foraging.
    /// Stage 4: Consequence resolution.
    /// Stage 5: Epilogue — the well runs clean (or poisoned).
    /// </summary>
    public class Quest_BlackVein : QuestRuntime
    {
        public const string Id = "quest_black_vein";

        public const string BulkheadSealedKey = "bulkhead_sealed";
        public const string FlowReroutedKey = "flow_rerouted";
        public const string DredgerSavedKey = "dredger_saved";
        public const string PurifierDegradationKey = "purifier_degradation";
        public const string MartaFarmhouseDestroyedKey = "marta_farmhouse_destroyed";

        public const string ChoiceSealBulkhead = "seal_bulkhead";
        public const string ChoiceRerouteFlow = "reroute_flow";

        public Quest_BlackVein()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Black Vein",
                Description = "The water tastes of sulfur and almonds. Something below is bleeding into the clean lens. Find the source, or the well dies.",
                FactionId = "",
                MaxStages = 5
            };
        }

        protected override void OnBegin()
        {
            SetProgress(BulkheadSealedKey, 0);
            SetProgress(FlowReroutedKey, 0);
            SetProgress(DredgerSavedKey, 0);
            SetProgress(PurifierDegradationKey, 0);
            SetProgress(MartaFarmhouseDestroyedKey, 0);
        }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 1:
                    RecordMoralEntry?.Invoke("the water tastes wrong. sulfur and almonds. the purifier is dying.");
                    SetProgress(PurifierDegradationKey, 3f); // 300% faster degradation
                    break;
                case 2:
                    RecordMoralEntry?.Invoke("found the source. a collapsed bulkhead in the sewage treatment. the Black Aquifer is bleeding through.");
                    break;
                case 3:
                    OfferChoice(ChoiceSealBulkhead);
                    OfferChoice(ChoiceRerouteFlow);
                    break;
            }
        }

        protected override void OnSuccess()
        {
            if (GetProgress(BulkheadSealedKey) > 0.5f)
                RecordMoralEntry?.Invoke("sealed the bulkhead. the well runs clean. someone in the dark is still screaming.");
            else if (GetProgress(FlowReroutedKey) > 0.5f)
                RecordMoralEntry?.Invoke("rerouted the flow. the Dredger lived. the ash-swamp is dead, and the militia knows.");
        }

        protected override void OnFailure() { }

        /// <summary>
        /// Player chose to seal the bulkhead. Requires materials.
        /// Saves the well but entombs the Sump-Dredger.
        /// </summary>
        public void ResolveSealBulkhead()
        {
            SetProgress(BulkheadSealedKey, 1f);
            SetProgress(DredgerSavedKey, 0f);
            SetProgress(PurifierDegradationKey, 1f); // back to normal
            Advance();
        }

        /// <summary>
        /// Player chose to reroute the flow. Saves the Dredger but
        /// poisons the ash-swamp biome and angers the Upland Militia.
        /// </summary>
        public void ResolveRerouteFlow()
        {
            SetProgress(FlowReroutedKey, 1f);
            SetProgress(DredgerSavedKey, 1f);
            SetProgress(MartaFarmhouseDestroyedKey, 1f);
            SetProgress(PurifierDegradationKey, 1.5f); // partial improvement
            Advance();
        }
    }
}
