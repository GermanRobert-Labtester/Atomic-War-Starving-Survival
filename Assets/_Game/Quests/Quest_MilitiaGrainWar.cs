using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_militia_grain_war — The Grain War (Faction: Upland Militia).
    /// Stage 1: Runner arrives. The Militia's grain cache is hidden in a
    ///          sealed barn; the Garrison has spotted it.
    /// Stage 2: The Militia asks for a diversion: send two survivors to
    ///          the surface to trigger a noise event near the Garrison
    ///          patrol route.
    /// Stage 3: The diversion works, but one of your survivors is spotted
    ///          and taken by the Garrison.
    /// Stage 4: Negotiate. The Militia will not trade grain for a person.
    ///          The Garrison wants medical supplies. Three days before
    ///          the prisoner is "reassigned."
    /// </summary>
    public class Quest_MilitiaGrainWar : QuestRuntime
    {
        public const string Id = "quest_militia_grain_war";
        public const string SealedBarn = "loc_sealed_barn";
        public const string PrisonerKey = "prisoner_id";

        public Quest_MilitiaGrainWar()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Grain War",
                Description = "The Upland Militia's grain cache is in a sealed barn. The Garrison has spotted it. The Militia wants a diversion. Someone is going to be seen.",
                FactionId = "faction_upland_militia",
                MaxStages = 4
            };
        }

        protected override void OnBegin()
        {
            RecordMoralEntry?.Invoke("a runner from the Upland Militia arrived. they need a diversion.");
        }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 2:
                    OfferChoice("diversion_send_two");
                    OfferChoice("diversion_refuse");
                    break;
                case 3:
                    // host invokes CaptureSurvivor for the spotted survivor
                    RecordMoralEntry?.Invoke("the diversion worked. one of ours was taken.");
                    break;
                case 4:
                    OfferChoice("negotiate_with_garrison_medical");
                    OfferChoice("negotiate_with_militia_grain");
                    OfferChoice("abandon_prisoner");
                    break;
            }
        }

        protected override void OnSuccess() { }
        protected override void OnFailure() { }

        // ── Stage 2 resolver ─────────────────────────────────────────────
        public void ResolveDiversionSendTwo(string survivorASeen, string survivorBTaken)
        {
            SetProgress("sent", 2);
            SetProgress(PrisonerKey, Random.Range(0, 2) == 0 ? 1 : 2); // pseudo; host injects
            RecordMoralEntry?.Invoke("two went up. one was seen.");
            Advance();
        }

        public void ResolveDiversionRefuse()
        {
            SetProgress("sent", 0);
            SubtractFactionTrust?.Invoke("faction_upland_militia", -25f);
            RecordMoralEntry?.Invoke("refused the diversion. the Militia remembers.");
            Fail();
        }

        // ── Stage 4 resolvers ────────────────────────────────────────────
        public void ResolveNegotiateMedicalForPrisoner(string prisonerId, int medicalSupplyCost)
        {
            // host validates: must have antibiotics * medicalSupplyCost
            GiveItem?.Invoke(null, "grain", 1); // grain comes from Militia side, this represents the player receiving it
            AddFactionTrust?.Invoke("faction_upland_militia", +20f);
            SubtractFactionTrust?.Invoke("faction_garrison", -5f);
            ReleaseSurvivor?.Invoke(null); // host maps to prisoner
            RecordMoralEntry?.Invoke("traded medicine for a life. the Militia noticed.");
            Complete();
        }

        public void ResolveNegotiateGrainForPrisoner()
        {
            // The Militia will not trade grain for a person. This is the
            // documented refusal: the player asked, the answer is no.
            RecordMoralEntry?.Invoke("asked the Militia to trade grain for a person. they said no.");
            OfferChoice("negotiate_with_garrison_medical");
            OfferChoice("abandon_prisoner");
        }

        public void ResolveAbandonPrisoner()
        {
            RecordMoralEntry?.Invoke("abandoned the prisoner. three days. reassigned.");
            SubtractFactionTrust?.Invoke("faction_garrison", -5f);
            SubtractFactionTrust?.Invoke("faction_upland_militia", -10f);
            KillSurvivor?.Invoke(null); // host maps via PrisonerKey
            Complete();
        }
    }
}
