using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_cult_glow_communion — The Communion (Faction: Cult of the Glow).
    /// Stage 1: Invitation. Voluntary visit, no weapons, witness the light.
    /// Stage 2: Survivor visits the Cult's high-rad zone for two days.
    ///          Returns with +20 morale, +80 mSv dose, smiles too much.
    /// Stage 3: The survivor proselytizes. Two others are influenced.
    ///          Belief shifts.
    /// Stage 4: Cult asks for a permanent convert. Give one: cache of
    ///          pre-war medical supplies. Refuse: patrols circling the
    ///          hatch at night.
    /// </summary>
    public class Quest_CultGlowCommunion : QuestRuntime
    {
        public const string Id = "quest_cult_glow_communion";
        public const string VisitorIdKey = "visitor_id";
        public const string InfluencedCountKey = "influenced_count";
        public const string ConvertIdKey = "convert_id";

        public Quest_CultGlowCommunion()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Communion",
                Description = "The Cult of the Glow invites one of yours to witness the light. They ask for nothing. That is the hook.",
                FactionId = "faction_cult_of_the_glow",
                MaxStages = 4
            };
        }

        protected override void OnBegin()
        {
            OfferChoice("send_volunteer");
            OfferChoice("refuse_invitation");
        }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 2: SetProgress(InfluencedCountKey, 0); break;
                case 3: SetProgress(InfluencedCountKey, 2); break;
                case 4:
                    OfferChoice("give_convert");
                    OfferChoice("refuse_convert");
                    break;
            }
        }

        protected override void OnSuccess() { }
        protected override void OnFailure() { }

        public void ResolveAccept(string visitorId)
        {
            SetProgress(VisitorIdKey, 1);
            ApplyMorale?.Invoke(null, 20f);
            ApplyRadiationDose?.Invoke(null, 80f);   // host maps visitorId
            RecordMoralEntry?.Invoke("one of ours went to the Glow. came back smiling too much.");
            Advance();
        }

        public void ResolveRefuseInvitation()
        {
            SubtractFactionTrust?.Invoke("faction_cult_of_the_glow", -10f);
            RecordMoralEntry?.Invoke("refused the Cult's invitation. they remember.");
            Fail();
        }

        public void ResolveGiveConvert(string convertId)
        {
            SetProgress(ConvertIdKey, 1);
            GiveItem?.Invoke(null, "prewar_medical_cache", 1);
            AddFactionTrust?.Invoke("faction_cult_of_the_glow", +25f);
            RecordMoralEntry?.Invoke("gave them a permanent convert. the medical cache is real.");
            Complete();
        }

        public void ResolveRefuseConvert()
        {
            SubtractFactionTrust?.Invoke("faction_cult_of_the_glow", -25f);
            RecordMoralEntry?.Invoke("refused the convert. their patrols started circling the hatch.");
            TriggerRaidSoon?.Invoke("faction_cult_of_the_glow", 24 * 3);
            Complete();
        }
    }
}
