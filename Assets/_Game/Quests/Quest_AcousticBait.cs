using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_acoustic_bait — The Acoustic Bait (Expansion III questline).
    ///
    /// Stage 1: Scavengers trapped in location_uxo_highway_choke.
    ///          A rusted sentry blocks the only safe path.
    /// Stage 2: Manufacture item_acoustic_decoy (hand_crank_radio +
    ///          battery + scrap_metal).
    /// Stage 3 (The Choice):
    ///   A) Throw the Decoy: Sentry fires 200 rounds, barrel melts.
    ///      Safe passage, but noise attracts Warlord patrol to shelter.
    ///   B) The Human Bait: Send a survivor running across the open.
    ///      80% chance of TraumaticAmputation or BleedOut.
    ///   C) Wait it Out: Camp 48h until pneumatic pressure fails.
    ///      Massive ration/water consumption, Weather_AshLightning risk.
    /// Stage 4: Consequence resolution.
    /// Stage 5: Epilogue.
    /// </summary>
    public class Quest_AcousticBait : QuestRuntime
    {
        public const string Id = "quest_acoustic_bait";

        public const string DecoyThrownKey = "decoy_thrown";
        public const string HumanBaitUsedKey = "human_bait_used";
        public const string WaitedOutKey = "waited_out";
        public const string SentryDisabledKey = "sentry_disabled";
        public const string WarlordAttractedKey = "warlord_attracted";
        public const string BaitSurvivorIdKey = "bait_survivor_id";

        public const string ChoiceThrowDecoy = "throw_decoy";
        public const string ChoiceHumanBait = "human_bait";
        public const string ChoiceWaitOut = "wait_out";

        public Quest_AcousticBait()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Acoustic Bait",
                Description = "A sentry blocks the only path through the UXO choke. Its ears work better than its eyes. Give it something to listen to.",
                FactionId = "",
                MaxStages = 5
            };
        }

        protected override void OnBegin()
        {
            SetProgress(DecoyThrownKey, 0);
            SetProgress(HumanBaitUsedKey, 0);
            SetProgress(WaitedOutKey, 0);
            SetProgress(SentryDisabledKey, 0);
            SetProgress(WarlordAttractedKey, 0);
        }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 1:
                    RecordMoralEntry?.Invoke("trapped in the UXO choke. a sentry watches the only dry path. it listens.");
                    break;
                case 2:
                    RecordMoralEntry?.Invoke("we need bait. something loud. something that ticks.");
                    break;
                case 3:
                    OfferChoice(ChoiceThrowDecoy);
                    OfferChoice(ChoiceHumanBait);
                    OfferChoice(ChoiceWaitOut);
                    break;
            }
        }

        protected override void OnSuccess()
        {
            if (GetProgress(DecoyThrownKey) > 0.5f)
                RecordMoralEntry?.Invoke("the decoy ticked. the sentry screamed. the barrel melted. something else heard the noise.");
            else if (GetProgress(HumanBaitUsedKey) > 0.5f)
                RecordMoralEntry?.Invoke("someone ran. the sentry followed. we made it through. not everyone did.");
            else if (GetProgress(WaitedOutKey) > 0.5f)
                RecordMoralEntry?.Invoke("we waited. the pneumatic pressure failed. the sentry sagged. patience is a kind of courage.");
        }

        protected override void OnFailure() { }

        /// <summary>Player threw the acoustic decoy. Sentry melts barrel, but attracts warlords.</summary>
        public void ResolveThrowDecoy()
        {
            SetProgress(DecoyThrownKey, 1f);
            SetProgress(SentryDisabledKey, 1f);
            SetProgress(WarlordAttractedKey, 1f);
            Advance();
        }

        /// <summary>Player sent a survivor as human bait. High casualty risk.</summary>
        public void ResolveHumanBait(string baitSurvivorId)
        {
            SetProgress(HumanBaitUsedKey, 1f);
            SetProgress(SentryDisabledKey, 1f);
            SetProgress(BaitSurvivorIdKey, baitSurvivorId?.GetHashCode() ?? 0);
            Advance();
        }

        /// <summary>Player chose to wait 48 hours for pneumatic failure.</summary>
        public void ResolveWaitOut()
        {
            SetProgress(WaitedOutKey, 1f);
            SetProgress(SentryDisabledKey, 1f);
            Advance();
        }
    }
}
