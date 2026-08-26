using System;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE DUTY ROSTER — canonical ID and constant bucket.
    /// Extracted from DutyRosterSystem so the system class no longer doubles
    /// as the namespace for every string/int literal in the expansion.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.1.
    /// </summary>
    public static class DutyRosterIds
    {
        // ── Expansion / system ────────────────────────────────────────
        public const string SystemId = "duty_roster_system";
        public const string ExpansionId = "expansion_the_duty_roster";
        public const string FlagExpUnlocked = "exp_duty_roster_unlocked";

        // ── Stack locations ───────────────────────────────────────────
        public const string LocStackRosterWall = "loc_stack_roster_wall";
        public const string LocStackSleeping = "loc_stack_sleeping";
        public const string LocStackMess = "loc_stack_mess";
        public const string LocStackFiltration = "loc_stack_filtration";
        public const string LocStackAirlock = "loc_stack_airlock";
        public const string LocStackClinicAlcove = "loc_stack_clinic_alcove";

        // ── Quests ────────────────────────────────────────────────────
        public const string QuestTheChart = "quest_roster_the_chart";
        public const string QuestWhoEats = "quest_roster_who_eats";
        public const string QuestFourteenth = "quest_roster_fourteenth";
        public const string QuestCaretaker = "quest_roster_caretaker";
        public const string QuestTheColumn = "quest_roster_the_column";
        public const string QuestTheTin = "quest_roster_the_tin";
        public const string QuestQuiet = "quest_roster_quiet";
        public const string QuestSole = "quest_roster_sole";
        public const string QuestWindow = "quest_roster_window";
        public const string QuestInk = "quest_roster_ink";

        // ── NPCs ──────────────────────────────────────────────────────
        public const string NpcKessAdler = "npc_kess_adler";
        public const string NpcAnselDuth = "npc_ansel_duth";
        public const string NpcHadiMorrow = "npc_hadi_morrow";
        public const string NpcTamsinRook = "npc_tamsin_rook";
        public const string NpcLenQuill = "npc_len_quill";
        public const string NpcNilaBrant = "npc_nila_brant";

        // ── Player choices ────────────────────────────────────────────
        public const string ChoiceWritePencil = "roster_write_pencil";
        public const string ChoiceLeaveBlank = "roster_leave_blank";
        public const string ChoiceWaitInk = "roster_wait_ink";
        public const string ChoiceLadleChild = "roster_ladle_child";
        public const string ChoiceLadleHatch = "roster_ladle_hatch";
        public const string ChoiceLadleLeave = "roster_ladle_leave";
        public const string ChoiceLadleProtocol = "roster_ladle_protocol";

        // ── Chart scripts ─────────────────────────────────────────────
        public const string ScriptBlank = "blank";
        public const string ScriptPencil = "pencil";
        public const string ScriptInk = "ink";
        public const string ScriptBurned = "burned";

        // ── Survivor status ───────────────────────────────────────────
        public const string StatusHome = "home";
        public const string StatusLevy = "levy";
        public const string StatusWaystation = "waystation";
        public const string StatusQuiet = "quiet";
        public const string StatusMissing = "missing";
        public const string StatusDead = "dead";

        // ── Assignment roles ──────────────────────────────────────────
        public const string RoleNightWatch = "night_watch";
        public const string RoleMess = "mess";
        public const string RoleHatchOpener = "hatch_opener";
        public const string RoleIntakeSleeper = "intake_sleeper";
        public const string RoleExpedition = "expedition";

        // ── Authored mutations / flags ────────────────────────────────
        public const string MutationRosterInUse = "mutation_roster_in_use";
        public const string MutationRosterStillBlank = "mutation_roster_still_blank";
        public const string MutationRationProtocol = "mutation_ration_protocol";
        public const string MutationRosterBurned = "mutation_roster_burned";
        public const string MutationRosterInk = "mutation_roster_ink";
        public const string MutationRosterBlank = "mutation_roster_blank";
        public const string MutationFactionBlankRowsAccess = "faction_blank_rows_access";
        public const string FlagWaitInk = "flag_wait_ink";

        // ── Endings ───────────────────────────────────────────────────
        public const string EndingInk = "ending_roster_ink";
        public const string EndingPencil = "ending_roster_pencil";
        public const string EndingBlank = "ending_roster_blank";
        public const string EndingBurned = "ending_roster_burned";
        public const string EndingSecondWinter = "ending_roster_second_winter";

        // ── Second Winter ─────────────────────────────────────────────
        public const string SeasonSecondWinter = "season_second_winter";
        public const int SecondWinterWindowMinDays = 8;
        public const int SecondWinterWindowMaxDays = 12;
        public const float SecondWinterEncounterWeight = 1.6f;

        // ── Tuning constants ──────────────────────────────────────────
        public const int ManifestCap = 14;
        public const int SoftGateDay = 60;
        public const int StillBlankDays = 40;
        public const int SeedUtilityOffset = 1208;

        // ── Static arrays ─────────────────────────────────────────────
        public static readonly string[] StackWingIds =
        {
            LocStackRosterWall, LocStackSleeping, LocStackMess, LocStackFiltration, LocStackAirlock, LocStackClinicAlcove
        };

        public const string LocOverflowAlloc11 = "loc_overflow_alloc_11";
        public const string LocOverflowAlloc13 = "loc_overflow_alloc_13";
        public const string LocOverflowPumpHatch = "loc_overflow_pump_hatch";
        public const string LocOverflowBlankCellar = "loc_overflow_blank_cellar";

        public static readonly string[] OverflowNodeIds =
        {
            LocOverflowAlloc11, LocOverflowAlloc13, LocOverflowPumpHatch, LocOverflowBlankCellar
        };

        public static readonly string[] AssignmentRoles =
        {
            RoleNightWatch, RoleMess, RoleHatchOpener, RoleIntakeSleeper, RoleExpedition
        };
    }
}
