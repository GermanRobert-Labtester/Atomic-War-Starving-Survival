using System;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Canonical quest ids and flags for the moral choice system
    /// ("The Weight of Survival"), pinned 1:1 against
    /// moral_choice_quests.json by MoralChoiceCatalogTests. Threshold event
    /// ids live on MoralChoiceSystem; this class owns quest and flag ids.
    /// </summary>
    public static class MoralChoiceIds
    {
        public const int QuestCount = 60;

        // ── Sharing Supplies (12) ──────────────────────────────────────
        public const string ShareChild = "quest_moral_share_child";
        public const string ShareFamily = "quest_moral_share_family";
        public const string ShareInjured = "quest_moral_share_injured";
        public const string ShareWater = "quest_moral_share_water";
        public const string ShareElder = "quest_moral_share_elder";
        public const string SharePregnant = "quest_moral_share_pregnant";
        public const string ShareRaider = "quest_moral_share_raider";
        public const string SharePeacekeeper = "quest_moral_share_peacekeeper";
        public const string ShareKeeper = "quest_moral_share_keeper";
        public const string ShareBanditLeader = "quest_moral_share_bandit_leader";
        public const string ShareScientist = "quest_moral_share_scientist";
        public const string ShareFarmer = "quest_moral_share_farmer";

        // ── Listening to Stories (12) ──────────────────────────────────
        public const string ListenOldMan = "quest_moral_listen_oldman";
        public const string ListenMother = "quest_moral_listen_mother";
        public const string ListenSoldier = "quest_moral_listen_soldier";
        public const string ListenChild = "quest_moral_listen_child";
        public const string ListenDoctor = "quest_moral_listen_doctor";
        public const string ListenPreacher = "quest_moral_listen_preacher";
        public const string ListenEngineer = "quest_moral_listen_engineer";
        public const string ListenWarning = "quest_moral_listen_warning";
        public const string ListenLover = "quest_moral_listen_lover";
        public const string ListenTeacher = "quest_moral_listen_teacher";
        public const string ListenThief = "quest_moral_listen_thief";
        public const string ListenProphet = "quest_moral_listen_prophet";

        // ── Offering Comfort (12) ──────────────────────────────────────
        public const string ComfortWidow = "quest_moral_comfort_widow";
        public const string ComfortChild = "quest_moral_comfort_child";
        public const string ComfortInjured = "quest_moral_comfort_injured";
        public const string ComfortFear = "quest_moral_comfort_fear";
        public const string ComfortAddict = "quest_moral_comfort_addict";
        public const string ComfortGuilt = "quest_moral_comfort_guilt";
        public const string ComfortElder = "quest_moral_comfort_elder";
        public const string ComfortNightmare = "quest_moral_comfort_nightmare";
        public const string ComfortLoneliness = "quest_moral_comfort_loneliness";
        public const string ComfortAnger = "quest_moral_comfort_anger";
        public const string ComfortHope = "quest_moral_comfort_hope";
        public const string ComfortDespair = "quest_moral_comfort_despair";

        // ── Respecting the Dead (12) ───────────────────────────────────
        public const string DeadUnmarked = "quest_moral_dead_unmarked";
        public const string DeadBurned = "quest_moral_dead_burned";
        public const string DeadBloated = "quest_moral_dead_bloated";
        public const string DeadChild = "quest_moral_dead_child";
        public const string DeadMass = "quest_moral_dead_mass";
        public const string DeadHanged = "quest_moral_dead_hanged";
        public const string DeadCloset = "quest_moral_dead_closet";
        public const string DeadWater = "quest_moral_dead_water";
        public const string DeadCremated = "quest_moral_dead_cremated";
        public const string DeadExecuted = "quest_moral_dead_executed";
        public const string DeadSuicide = "quest_moral_dead_suicide";
        public const string DeadMassacre = "quest_moral_dead_massacre";

        // ── Trusting Strangers (12) ────────────────────────────────────
        public const string TrustFire = "quest_moral_trust_fire";
        public const string TrustWounded = "quest_moral_trust_wounded";
        public const string TrustMerchant = "quest_moral_trust_merchant";
        public const string TrustChild = "quest_moral_trust_child";
        public const string TrustDeserter = "quest_moral_trust_deserter";
        public const string TrustWoman = "quest_moral_trust_woman";
        public const string TrustSoldier = "quest_moral_trust_soldier";
        public const string TrustRunaway = "quest_moral_trust_runaway";
        public const string TrustSilent = "quest_moral_trust_silent";
        public const string TrustSignal = "quest_moral_trust_signal";
        public const string TrustBorrower = "quest_moral_trust_borrower";
        public const string TrustMessenger = "quest_moral_trust_messenger";

        // ── Flags ──────────────────────────────────────────────────────
        /// <summary>Set when the Dying Messenger's packet is delivered unopened — a Storykeeper key.</summary>
        public const string FlagMessengerKept = "flag_moral_messenger_kept";

        /// <summary>All 60 quest ids in catalog order (share, listen, comfort, dead, trust).</summary>
        public static readonly string[] All =
        {
            ShareChild, ShareFamily, ShareInjured, ShareWater, ShareElder, SharePregnant,
            ShareRaider, SharePeacekeeper, ShareKeeper, ShareBanditLeader, ShareScientist, ShareFarmer,
            ListenOldMan, ListenMother, ListenSoldier, ListenChild, ListenDoctor, ListenPreacher,
            ListenEngineer, ListenWarning, ListenLover, ListenTeacher, ListenThief, ListenProphet,
            ComfortWidow, ComfortChild, ComfortInjured, ComfortFear, ComfortAddict, ComfortGuilt,
            ComfortElder, ComfortNightmare, ComfortLoneliness, ComfortAnger, ComfortHope, ComfortDespair,
            DeadUnmarked, DeadBurned, DeadBloated, DeadChild, DeadMass, DeadHanged,
            DeadCloset, DeadWater, DeadCremated, DeadExecuted, DeadSuicide, DeadMassacre,
            TrustFire, TrustWounded, TrustMerchant, TrustChild, TrustDeserter, TrustWoman,
            TrustSoldier, TrustRunaway, TrustSilent, TrustSignal, TrustBorrower, TrustMessenger,
        };
    }
}
