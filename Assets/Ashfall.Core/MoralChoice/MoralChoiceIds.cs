using System;
using System.Linq;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Canonical quest ids and flags for the moral choice system
    /// ("The Weight of Survival"), pinned 1:1 against the data files.
    /// Base catalog: moral_choice_quests.json (65 quests).
    /// Chain quests: moral_choice_quests_branching.json (100 quests, 4 × 25).
    /// Expansion quests: moral_choice_quests_expansion.json (50 quests).
    /// Threshold event ids live on MoralChoiceSystem; this class owns quest
    /// and flag ids.
    /// </summary>
    public static class MoralChoiceIds
    {
        public const int BaseQuestCount = 65;
        public const int ChainQuestCount = 100;
        public const int ExpansionQuestCount = 50;
        public const int TotalQuestCount = BaseQuestCount + ChainQuestCount + ExpansionQuestCount;

        // ── Base: Sharing Supplies (13) ─────────────────────────────────
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
        public const string ShareScavengerChild = "quest_moral_env_scavenger_child";

        // ── Base: Listening to Stories (13) ─────────────────────────────
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
        public const string ListenBuriedLetters = "quest_moral_env_buried_letters";

        // ── Base: Offering Comfort (13) ─────────────────────────────────
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
        public const string ComfortWoundedScavenger = "quest_moral_env_wounded_scavenger";

        // ── Base: Respecting the Dead (13) ──────────────────────────────
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
        public const string DeadExplorer = "quest_moral_env_dead_explorer";

        // ── Base: Trusting Strangers (13) ───────────────────────────────
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
        public const string TrustShelterRefugee = "quest_moral_env_shelter_refugee";

        // ── Chain: Mercy Road (25) ──────────────────────────────────────
        public static readonly string[] ChainMercy = Enumerable.Range(1, 25)
            .Select(i => $"quest_moral_chain_mercy_{i:D2}").ToArray();

        // ── Chain: Iron Way (25) ────────────────────────────────────────
        public static readonly string[] ChainIron = Enumerable.Range(1, 25)
            .Select(i => $"quest_moral_chain_iron_{i:D2}").ToArray();

        // ── Chain: Listener's Thread (25) ───────────────────────────────
        public static readonly string[] ChainListen = Enumerable.Range(1, 25)
            .Select(i => $"quest_moral_chain_listen_{i:D2}").ToArray();

        // ── Chain: Broken Compact (25) ──────────────────────────────────
        public static readonly string[] ChainBetray = Enumerable.Range(1, 25)
            .Select(i => $"quest_moral_chain_betray_{i:D2}").ToArray();

        /// <summary>All 100 chain quest ids (mercy + iron + listen + betray).</summary>
        public static readonly string[] AllChain =
            ChainMercy.Concat(ChainIron).Concat(ChainListen).Concat(ChainBetray).ToArray();

        // ── Expansion (50) ──────────────────────────────────────────────
        public static readonly string[] AllExpansion =
        {
            "quest_moral_share_medicine",
            "quest_moral_share_coat",
            "quest_moral_share_fire",
            "quest_moral_trust_scout",
            "quest_moral_trust_stranger_water",
            "quest_moral_comfort_dying_stranger",
            "quest_moral_comfort_grieving",
            "quest_moral_dead_unburied",
            "quest_moral_dead_name",
            "quest_moral_listen_confession",
            "quest_moral_listen_old_story",
            "quest_moral_share_seed",
            "quest_moral_trust_coded_message",
            "quest_moral_comfort_broken",
            "quest_moral_dead_mass_grave",
            "quest_moral_listen_child_question",
            "quest_moral_share_shelter",
            "quest_moral_trust_traitor",
            "quest_moral_comfort_letter",
            "quest_moral_dead_dog",
            "quest_moral_listen_singer",
            "quest_moral_share_skill",
            "quest_moral_trust_child_message",
            "quest_moral_comfort_despairing",
            "quest_moral_dead_last_request",
            "quest_moral_listen_veteran",
            "quest_moral_share_last_match",
            "quest_moral_trust_map",
            "quest_moral_comfort_dying_alone",
            "quest_moral_dead_unknown",
            "quest_moral_listen_prophecy",
            "quest_moral_share_blanket",
            "quest_moral_trust_returned",
            "quest_moral_comfort_reminder",
            "quest_moral_dead_pet",
            "quest_moral_listen_drunkard",
            "quest_moral_share_rope",
            "quest_moral_trust_camp_invite",
            "quest_moral_comfort_old_fear",
            "quest_moral_dead_letter_unsent",
            "quest_moral_listen_silence",
            "quest_moral_share_skill_medical",
            "quest_moral_trust_returned_thief",
            "quest_moral_comfort_burned",
            "quest_moral_dead_mass_burial_help",
            "quest_moral_listen_rumor",
            "quest_moral_share_lamp_oil",
            "quest_moral_trust_orphan",
            "quest_moral_comfort_remorse",
            "quest_moral_dead_ceremony",
        };

        /// <summary>All 65 base quest ids in catalog order.</summary>
        public static readonly string[] All =
        {
            ShareChild, ShareFamily, ShareInjured, ShareWater, ShareElder, SharePregnant,
            ShareRaider, SharePeacekeeper, ShareKeeper, ShareBanditLeader, ShareScientist, ShareFarmer,
            ShareScavengerChild,
            ListenOldMan, ListenMother, ListenSoldier, ListenChild, ListenDoctor, ListenPreacher,
            ListenEngineer, ListenWarning, ListenLover, ListenTeacher, ListenThief, ListenProphet,
            ListenBuriedLetters,
            ComfortWidow, ComfortChild, ComfortInjured, ComfortFear, ComfortAddict, ComfortGuilt,
            ComfortElder, ComfortNightmare, ComfortLoneliness, ComfortAnger, ComfortHope, ComfortDespair,
            ComfortWoundedScavenger,
            DeadUnmarked, DeadBurned, DeadBloated, DeadChild, DeadMass, DeadHanged,
            DeadCloset, DeadWater, DeadCremated, DeadExecuted, DeadSuicide, DeadMassacre,
            DeadExplorer,
            TrustFire, TrustWounded, TrustMerchant, TrustChild, TrustDeserter, TrustWoman,
            TrustSoldier, TrustRunaway, TrustSilent, TrustSignal, TrustBorrower, TrustMessenger,
            TrustShelterRefugee,
        };

        // ── Branch flags ────────────────────────────────────────────────
        public const string FlagMercyRoadLocked = "flag_branch_mercy_road_locked";
        public const string FlagIronWayLocked = "flag_branch_iron_way_locked";
        public const string FlagListenerLocked = "flag_branch_listener_locked";
        public const string FlagBrokenCompactLocked = "flag_branch_broken_compact_locked";

        // ── Narrative flags ─────────────────────────────────────────────
        public const string FlagBetrayedAlly = "flag_betrayed_ally";
        public const string FlagBetrayedFaction = "flag_betrayed_faction";
        public const string FlagBetrayedTrust = "flag_betrayed_trust";
        public const string FlagBrokenPact = "flag_broken_pact";
        public const string FlagBecomeWarlord = "flag_become_warlord";
        public const string FlagThroneOfAsh = "flag_throne_of_ash";

        /// <summary>Set when the Dying Messenger's packet is delivered unopened — a Storykeeper key.</summary>
        public const string FlagMessengerKept = "flag_moral_messenger_kept";

        /// <summary>All 11 moral flag ids.</summary>
        public static readonly string[] AllFlags =
        {
            FlagMercyRoadLocked, FlagIronWayLocked, FlagListenerLocked, FlagBrokenCompactLocked,
            FlagBetrayedAlly, FlagBetrayedFaction, FlagBetrayedTrust, FlagBrokenPact,
            FlagBecomeWarlord, FlagThroneOfAsh, FlagMessengerKept
        };

        // ── Branch ids ──────────────────────────────────────────────────
        public const string BranchMercyRoad = "branch_mercy_road";
        public const string BranchIronWay = "branch_iron_way";
        public const string BranchListenerThread = "branch_listener_thread";
        public const string BranchBrokenCompact = "branch_broken_compact";

        public static readonly string[] AllBranches =
        {
            BranchMercyRoad, BranchIronWay, BranchListenerThread, BranchBrokenCompact
        };
    }
}
