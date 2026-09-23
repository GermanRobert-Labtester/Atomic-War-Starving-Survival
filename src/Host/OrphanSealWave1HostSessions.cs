// SPDX-License-Identifier: MIT
// ============================================================================
// ORPHAN-SEAL-PRIORITY-W1 (2026-09-23, user-authorized integrator package)
//
// Thin Godot host adapters for the ten priority orphan authorities. Each
// session owns exactly one Core system, binds it to the canonical catalog
// file, restores its pre-validated save section (Appendix Q), exposes typed
// commands for the CLI probe / journal seam, and persists through
// SaveStoreHub.Checksummed. No gameplay rules live here — hosts present and
// wire only (Appendix C.1/C.2).
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Communications;
using Ashfall.Core.Culture;
using Ashfall.Core.Education;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.Weather;

namespace AtomicWar.GodotApp
{
    // ── Survivor autonomy ───────────────────────────────────────────────────

    public sealed class SurvivorAutonomyHostSession : HostSessionBase
    {
        public SurvivorAutonomySystem System { get; }

        public SurvivorAutonomyHostSession(SurvivorAutonomySystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public AutonomyAction? EvaluateDaily(
            string actorId, int day, float morale, float fatigue,
            string[]? traits = null, string? targetId = null)
            => System.EvaluateDailyAutonomy(actorId, day, morale, fatigue, traits, targetId);

        public bool OverrideRefusal(string actionId, bool enforceWork)
            => System.OverrideRefusal(actionId, enforceWork);

        public void AssignGoal(string survivorId, string goalId, string title, int targetProgress = 3)
            => System.AssignGoal(survivorId, goalId, title, targetProgress);

        public void TickDay(int day) { /* command-driven; no autonomous default */ }

        public override void Save()
        {
            if (!IsDirty) return;
            if (SurvivorAutonomySaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class SurvivorAutonomySaveStore
    {
        public const string FileName = "survivor_autonomy_save.json";
        public const string SectionName = "survivor_autonomy";

        private static readonly SaveStore<SurvivorAutonomySaveState> s_store =
            SaveStoreHub.Checksummed<SurvivorAutonomySaveState>(FileName, nameof(SurvivorAutonomySaveStore));

        public static bool TrySave(SurvivorAutonomySaveState state) => s_store.TrySave(state);
        public static SurvivorAutonomySaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SurvivorAutonomySaveState state) => s_store.CapturePersisted(state);
        public static SurvivorAutonomySaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Nuclear winter progression ──────────────────────────────────────────

    public sealed class NuclearWinterHostSession : HostSessionBase
    {
        public NuclearWinterProgressionSystem System { get; }

        public NuclearWinterHostSession(NuclearWinterProgressionSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public ClimateState AdvanceDay(int day, ISeededRng? rng)
            => System.AdvanceDay(day, rng);

        public WinterPhaseDef GetPhaseForDay(int day) => System.GetPhaseForDay(day);
        public float CalculateHeatingDemand(float baseDemand, int day) => System.CalculateHeatingDemand(baseDemand, day);
        public float CalculateExpeditionRisk(float baseRisk, int day) => System.CalculateExpeditionRisk(baseRisk, day);
        public float CalculateCropYieldMultiplier(int day, bool hasGreenhouse) => System.CalculateCropYieldMultiplier(day, hasGreenhouse);

        public override void Save()
        {
            if (!IsDirty) return;
            if (NuclearWinterSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class NuclearWinterSaveStore
    {
        public const string FileName = "nuclear_winter_progression_save.json";
        public const string SectionName = "nuclear_winter_progression";

        private static readonly SaveStore<NuclearWinterSaveState> s_store =
            SaveStoreHub.Checksummed<NuclearWinterSaveState>(FileName, nameof(NuclearWinterSaveStore));

        public static bool TrySave(NuclearWinterSaveState state) => s_store.TrySave(state);
        public static NuclearWinterSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(NuclearWinterSaveState state) => s_store.CapturePersisted(state);
        public static NuclearWinterSaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Seasonal celebrations ───────────────────────────────────────────────

    public sealed class SeasonalCelebrationHostSession : HostSessionBase
    {
        public SeasonalCelebrationSystem System { get; }

        public SeasonalCelebrationHostSession(SeasonalCelebrationSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public HolidayDef? CheckHoliday(int day) => System.CheckHolidayForDay(day);

        public CelebrationRecord HoldCelebration(string holidayId, string scaleId, int participants, ISeededRng? rng)
            => System.HoldCelebration(holidayId, scaleId, participants, rng);

        public float SkipHoliday(string holidayId) => System.SkipHoliday(holidayId);

        public float CommemorateAnniversary(string typeId, string entityName, int day)
            => System.CommemorateAnniversary(typeId, entityName, day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (SeasonalCelebrationSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class SeasonalCelebrationSaveStore
    {
        public const string FileName = "seasonal_celebration_save.json";
        public const string SectionName = "seasonal_celebration";

        private static readonly SaveStore<CelebrationSaveState> s_store =
            SaveStoreHub.Checksummed<CelebrationSaveState>(FileName, nameof(SeasonalCelebrationSaveStore));

        public static bool TrySave(CelebrationSaveState state) => s_store.TrySave(state);
        public static CelebrationSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CelebrationSaveState state) => s_store.CapturePersisted(state);
        public static CelebrationSaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Disaster response ───────────────────────────────────────────────────

    public sealed class DisasterResponseHostSession : HostSessionBase
    {
        public DisasterResponseSystem System { get; }

        public DisasterResponseHostSession(DisasterResponseSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public DisasterEventDto Trigger(
            DisasterType type, DisasterSeverity severity, List<string> affectedRoomIds, int day)
            => System.TriggerDisaster(type, severity, affectedRoomIds, day);

        public bool TickDisaster(string disasterId, double mitigationLabor, int day, ISeededRng rng)
            => System.TickDisaster(disasterId, mitigationLabor, day, rng);

        public bool ActivateProtocol(EmergencyProtocolType type) => System.ActivateProtocol(type);
        public bool DeactivateProtocol(EmergencyProtocolType type) => System.DeactivateProtocol(type);
        public IReadOnlyList<DisasterEventDto> Disasters => System.GetAllDisasters();

        public override void Save()
        {
            if (!IsDirty) return;
            if (DisasterResponseSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class DisasterResponseSaveStore
    {
        public const string FileName = "disaster_response_save.json";
        public const string SectionName = "disaster_response";

        private static readonly SaveStore<DisasterResponseState> s_store =
            SaveStoreHub.Checksummed<DisasterResponseState>(FileName, nameof(DisasterResponseSaveStore));

        public static bool TrySave(DisasterResponseState state) => s_store.TrySave(state);
        public static DisasterResponseState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(DisasterResponseState state) => s_store.CapturePersisted(state);
        public static DisasterResponseState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Communications (antenna / intercept / broadcast layer) ──────────────

    public sealed class CommunicationsHostSession : HostSessionBase
    {
        public CommunicationsSystem System { get; }

        public CommunicationsHostSession(CommunicationsSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public InterceptedMessageDto? InterceptFactionSignal(string factionId, ISeededRng rng, int day)
            => System.InterceptFactionSignal(factionId, rng, day);

        public OutgoingBroadcastDto? Transmit(double frequencyMhz, string content, int encryptionLevel, int day)
            => System.TransmitBroadcast(frequencyMhz, content, encryptionLevel, day);

        public bool Decode(string messageId, int cryptanalysisSkill, ISeededRng rng)
            => System.DecodeMessage(messageId, cryptanalysisSkill, rng);

        public void DegradeAntennas(double wearAmount) => System.DegradeAntennas(wearAmount);

        public double ReceptionRangeKm => System.GetEffectiveReceptionRangeKm();

        public override void Save()
        {
            if (!IsDirty) return;
            if (CommunicationsSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class CommunicationsSaveStore
    {
        public const string FileName = "communications_save.json";
        public const string SectionName = "communications";

        private static readonly SaveStore<CommunicationsState> s_store =
            SaveStoreHub.Checksummed<CommunicationsState>(FileName, nameof(CommunicationsSaveStore));

        public static bool TrySave(CommunicationsState state) => s_store.TrySave(state);
        public static CommunicationsState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CommunicationsState state) => s_store.CapturePersisted(state);
        public static CommunicationsState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Colonies (player-founded establishments) ────────────────────────────

    public sealed class ColonyHostSession : HostSessionBase
    {
        public ColonySystem System { get; }

        public ColonyHostSession(ColonySystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public void TickDay(int day) => System.TickDay(day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (ColonySaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class ColonySaveStore
    {
        public const string FileName = "colony_save.json";
        public const string SectionName = "colony";

        private static readonly SaveStore<ColonyState> s_store =
            SaveStoreHub.Checksummed<ColonyState>(FileName, nameof(ColonySaveStore));

        public static bool TrySave(ColonyState state) => s_store.TrySave(state);
        public static ColonyState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ColonyState state) => s_store.CapturePersisted(state);
        public static ColonyState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Hobbies ─────────────────────────────────────────────────────────────

    public sealed class HobbyHostSession : HostSessionBase
    {
        public HobbySystem System { get; }

        public HobbyHostSession(HobbySystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public HobbySessionResult ConductSession(
            string survivorId, string hobbyId, int currentDay,
            IEnumerable<string>? coParticipantIds = null, ISeededRng? rng = null)
            => System.ConductSession(survivorId, hobbyId, currentDay, coParticipantIds, rng);

        public void TickDay(int day) { /* session-driven; no daily default */ }

        public override void Save()
        {
            if (!IsDirty) return;
            if (HobbySaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class HobbySaveStore
    {
        public const string FileName = "hobby_save.json";
        public const string SectionName = "hobby";

        private static readonly SaveStore<HobbySystemState> s_store =
            SaveStoreHub.Checksummed<HobbySystemState>(FileName, nameof(HobbySaveStore));

        public static bool TrySave(HobbySystemState state) => s_store.TrySave(state);
        public static HobbySystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(HobbySystemState state) => s_store.CapturePersisted(state);
        public static HobbySystemState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Survivor education ──────────────────────────────────────────────────

    public sealed class SurvivorEducationHostSession : HostSessionBase
    {
        public SurvivorEducationSystem System { get; }

        public SurvivorEducationHostSession(SurvivorEducationSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public SurvivorEducationRecord RegisterLearner(string survivorId, int initialAge)
            => System.RegisterLearner(survivorId, initialAge);

        public bool AssignTeacherAndSubject(
            string studentId, string teacherId, string subjectId, bool isParentChild = false)
            => System.AssignTeacherAndSubject(studentId, teacherId, subjectId, isParentChild);

        public EducationSessionResult ConductDailySession(
            string studentId, ISeededRng rng, bool hasSchoolroom = false, int teacherCompetencyPermille = 1000)
            => System.ConductDailySession(studentId, rng, hasSchoolroom, teacherCompetencyPermille);

        public bool EvaluateGraduation(string studentId, int currentDay)
            => System.EvaluateGraduation(studentId, currentDay);

        public SurvivorEducationRecord? GetRecord(string survivorId) => System.GetRecord(survivorId);

        public void TickDay(int day) { /* session-driven; assignments gate progress */ }

        public override void Save()
        {
            if (!IsDirty) return;
            if (SurvivorEducationSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class SurvivorEducationSaveStore
    {
        public const string FileName = "survivor_education_save.json";
        public const string SectionName = "survivor_education";

        private static readonly SaveStore<EducationSystemState> s_store =
            SaveStoreHub.Checksummed<EducationSystemState>(FileName, nameof(SurvivorEducationSaveStore));

        public static bool TrySave(EducationSystemState state) => s_store.TrySave(state);
        public static EducationSystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(EducationSystemState state) => s_store.CapturePersisted(state);
        public static EducationSystemState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Shelter expansion ───────────────────────────────────────────────────

    public sealed class ShelterExpansionHostSession : HostSessionBase
    {
        public ShelterExpansionSystem System { get; }

        public ShelterExpansionHostSession(ShelterExpansionSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public bool ProgressProject(string projectId, double dailyLabor, int currentDay)
            => System.ProgressProject(projectId, dailyLabor, currentDay);

        public void TickDay(int day) { /* labor is explicit; no free progress */ }

        public override void Save()
        {
            if (!IsDirty) return;
            if (ShelterExpansionSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class ShelterExpansionSaveStore
    {
        public const string FileName = "shelter_expansion_save.json";
        public const string SectionName = "shelter_expansion";

        private static readonly SaveStore<ShelterExpansionState> s_store =
            SaveStoreHub.Checksummed<ShelterExpansionState>(FileName, nameof(ShelterExpansionSaveStore));

        public static bool TrySave(ShelterExpansionState state) => s_store.TrySave(state);
        public static ShelterExpansionState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ShelterExpansionState state) => s_store.CapturePersisted(state);
        public static ShelterExpansionState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Shelter festivals (planned events; complements SeasonalCelebration) ──

    /// <summary>
    /// Boundary (ORPHAN-SEAL-W1): SeasonalCelebrationSystem owns the calendar
    /// (holidays/anniversaries); ShelterFestivalEngine owns player-scheduled
    /// festivals that consume authored commodities. Both are live; neither
    /// duplicates the other's state.
    /// </summary>
    public sealed class ShelterFestivalHostSession : HostSessionBase
    {
        public ShelterFestivalEngine System { get; }

        public ShelterFestivalHostSession(ShelterFestivalEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public FestivalPlan ScheduleFestival(
            FestivalType type, string title, int plannedDay, int durationDays = 1,
            IEnumerable<FestivalCommodityRequirement>? customCommodities = null)
            => System.ScheduleFestival(type, title, plannedDay, durationDays, customCommodities);

        public bool TryCommenceFestival(string festivalId, int currentDay, Func<string, int, bool> tryConsumeCommodity)
            => System.TryCommenceFestival(festivalId, currentDay, tryConsumeCommodity);

        public void TickDay(int day) => System.ProcessDailyTick(day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (ShelterFestivalSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class ShelterFestivalSaveStore
    {
        public const string FileName = "shelter_festival_save.json";
        public const string SectionName = "shelter_festival";

        private static readonly SaveStore<ShelterFestivalSaveState> s_store =
            SaveStoreHub.Checksummed<ShelterFestivalSaveState>(FileName, nameof(ShelterFestivalSaveStore));

        public static bool TrySave(ShelterFestivalSaveState state) => s_store.TrySave(state);
        public static ShelterFestivalSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ShelterFestivalSaveState state) => s_store.CapturePersisted(state);
        public static ShelterFestivalSaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Rival covert operations (complements player-deployed EspionageSystem) ──

    /// <summary>
    /// Boundary (ORPHAN-SEAL-W1): EspionageSystem owns player-deployed agent
    /// missions; FactionCovertOpsCoordinator owns rival-faction operations and
    /// the suspicion ladder. Both are live; they share no save state.
    /// </summary>
    public sealed class FactionCovertOpsHostSession : HostSessionBase
    {
        public FactionCovertOpsCoordinator System { get; }

        public FactionCovertOpsHostSession(FactionCovertOpsCoordinator system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public void AdvanceDay(ISeededRng rng, int day) => System.AdvanceDay(rng, day);

        public bool LaunchOperation(string operationId, string agentId, float agentStealthSkill, int currentDay)
            => System.LaunchOperation(operationId, agentId, agentStealthSkill, currentDay);

        public float GetSuspicion(string factionId) => System.GetSuspicion(factionId);

        public override void Save()
        {
            if (!IsDirty) return;
            var payload = new CovertOpsPersistedPayload { json = System.CaptureState() };
            if (FactionCovertOpsSaveStore.TrySave(payload))
                base.Save();
        }
    }

    [Serializable]
    public sealed class CovertOpsPersistedPayload
    {
        public string json { get; set; } = string.Empty;
    }

    public static class FactionCovertOpsSaveStore
    {
        public const string FileName = "faction_covert_ops_save.json";
        public const string SectionName = "faction_covert_ops";

        private static readonly SaveStore<CovertOpsPersistedPayload> s_store =
            SaveStoreHub.Checksummed<CovertOpsPersistedPayload>(FileName, nameof(FactionCovertOpsSaveStore));

        public static bool TrySave(CovertOpsPersistedPayload state) => s_store.TrySave(state);
        public static CovertOpsPersistedPayload? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CovertOpsPersistedPayload state) => s_store.CapturePersisted(state);
        public static CovertOpsPersistedPayload? TryRestore(string json) => s_store.RestoreBare(json);
    }

    // ── Confession secrets ──────────────────────────────────────────────────

    public sealed class ConfessionSecretHostSession : HostSessionBase
    {
        public ConfessionSecretSystem System { get; }

        public ConfessionSecretHostSession(ConfessionSecretSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public bool DiscoverSecret(string secretId, int currentDay, string sourceId = "")
            => System.DiscoverSecret(secretId, currentDay, sourceId);

        public bool ExposeSecret(string secretId, int currentDay)
            => System.ExposeSecret(secretId, currentDay);

        public bool KeepSecret(string secretId, int currentDay)
            => System.KeepSecret(secretId, currentDay);

        public bool BlackmailSecret(string secretId, int currentDay)
            => System.BlackmailSecret(secretId, currentDay);

        public void TickDay(int day) { /* discovery-driven; no daily default */ }

        public override void Save()
        {
            if (!IsDirty) return;
            if (ConfessionSecretSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class ConfessionSecretSaveStore
    {
        public const string FileName = "confession_secret_save.json";
        public const string SectionName = "confession_secret";

        private static readonly SaveStore<ConfessionSecretState> s_store =
            SaveStoreHub.Checksummed<ConfessionSecretState>(FileName, nameof(ConfessionSecretSaveStore));

        public static bool TrySave(ConfessionSecretState state) => s_store.TrySave(state);
        public static ConfessionSecretState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ConfessionSecretState state) => s_store.CapturePersisted(state);
        public static ConfessionSecretState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
