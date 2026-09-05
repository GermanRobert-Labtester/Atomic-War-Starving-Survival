using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
#pragma warning disable CS8618
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class LibraryStudyState
    {
        public string systemId = LibraryStudySystem.SystemId;
        public List<StudyJob> activeJobs = new List<StudyJob>();
        public List<string> completedManualIds = new List<string>();
        public int totalStudyHours;
    }

    [Serializable]
    public sealed class ManualDefinition
    {
        [JsonPropertyName("manual_id")]
        public string manual_id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string display_name { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string category { get; set; } = string.Empty;       // "technical", "medical", "military", etc.

        [JsonPropertyName("study_hours_required")]
        public int studyHoursRequired { get; set; } = 10;

        [JsonPropertyName("fatigue_per_hour")]
        public float fatiguePerHour { get; set; } = 0.3f;

        [JsonPropertyName("morale_effect")]
        public float moraleEffect { get; set; } = -0.5f;           // studying is draining

        [JsonPropertyName("skill_xp_grants")]
        public List<string> skillXpGrants { get; set; } = new List<string>(); // skill_id, xp_amount pairs

        [JsonPropertyName("research_unlocks")]
        public List<string> researchUnlocks { get; set; } = new List<string>();

        [JsonPropertyName("knowledge_unlocks")]
        public List<string> knowledgeUnlocks { get; set; } = new List<string>();

        [JsonPropertyName("prerequisites")]
        public List<string> prerequisites { get; set; } = new List<string>();

        [JsonPropertyName("requires_power")]
        public bool requiresPower { get; set; } = true;

        [JsonPropertyName("loot_table_ids")]
        public List<string> lootTableIds { get; set; } = new List<string>();

        [JsonPropertyName("expedition_reward_ids")]
        public List<string> expeditionRewardIds { get; set; } = new List<string>();

        [JsonPropertyName("trader_pool_ids")]
        public List<string> traderPoolIds { get; set; } = new List<string>();

        [JsonPropertyName("archive_scribing_recipe_id")]
        public string archiveScribingRecipeId { get; set; } = string.Empty;

        [JsonPropertyName("starting_origin_ids")]
        public List<string> startingOriginIds { get; set; } = new List<string>();

        [JsonPropertyName("origin_facility")]
        public string originFacility { get; set; } = string.Empty;

        [JsonPropertyName("technical_complexity_tier")]
        public int technicalComplexityTier { get; set; } = 1;

        [JsonPropertyName("schematic_summary")]
        public string schematicSummary { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class StudyJob
    {
        public string jobId = string.Empty;
        public string manualId = string.Empty;
        public string readerId = string.Empty;
        public int dayStarted = -1;
        public float progressHours;
        public bool isComplete;
        public bool isCancelled;
    }

    public sealed class LibraryStudySystem
    {
        public const string SystemId = "library_study";
        private LibraryStudyState _state = new LibraryStudyState();
        private readonly Dictionary<string, ManualDefinition> _catalog = new Dictionary<string, ManualDefinition>(StringComparer.Ordinal);
        private readonly ILog _log;
        private readonly SkillProgressionSystem _skills;
        private readonly ResearchSystem _research;
        private readonly JournalSystem _journal;
        private readonly DutyRosterSystem _roster;
        private int _currentDay;

        public LibraryStudyState State => _state;
        public IReadOnlyDictionary<string, ManualDefinition> Catalog => _catalog;
        public event Action<StudyJob> OnJobCompleted;
        public event Action OnLibraryChanged;

        public LibraryStudySystem(
            SkillProgressionSystem skills,
            ResearchSystem research,
            JournalSystem journal,
            DutyRosterSystem roster,
            ILog? log = null)
        {
            _skills = skills ?? throw new ArgumentNullException(nameof(skills));
            _research = research ?? throw new ArgumentNullException(nameof(research));
            _journal = journal ?? throw new ArgumentNullException(nameof(journal));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _log = log ?? NullLog.Instance;

            // Bidirectional availability reservation with DutyRoster (B2-009)
            var prevReservation = _roster.IsSurvivorReservedExternally;
            _roster.IsSurvivorReservedExternally = id =>
                (prevReservation != null && prevReservation(id)) || IsReaderStudying(id);
        }

        public bool IsReaderStudying(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            return _state.activeJobs.Exists(j => !j.isComplete && !j.isCancelled && j.readerId == survivorId);
        }

        public static string NormalizeDiscipline(string category)
        {
            if (string.IsNullOrEmpty(category)) return "survival";
            string lower = category.Trim().ToLowerInvariant();
            switch (lower)
            {
                case "technical":
                case "engineering":
                case "crafting":
                    return "crafting";
                case "military":
                case "combat":
                    return "combat";
                case "medical":
                    return "medical";
                case "science":
                    return "science";
                case "scavenging":
                    return "scavenging";
                case "survival":
                default:
                    return "survival";
            }
        }

        public float GetComprehensionRate(string readerId, string manualId)
        {
            if (!_catalog.TryGetValue(manualId, out var manual)) return 1.0f;
            string disc = NormalizeDiscipline(manual.category);
            float progress01 = _skills.GetDisciplineProgress01(readerId, disc);
            float bonus = _skills.GetCachedBonus(readerId, disc);
            // Monotonic: rate = 1.0 + 0.6 * progress01 + 0.4 * bonus
            float rate = 1.0f + 0.6f * progress01 + 0.4f * bonus;
            // Strict bounds: [0.75f, 2.0f] (B2-006)
            return Math.Clamp(rate, 0.75f, 2.0f);
        }

        public float GetEffectiveStudyHours(string readerId, string manualId)
        {
            if (!_catalog.TryGetValue(manualId, out var manual)) return 0f;
            float rate = GetComprehensionRate(readerId, manualId);
            return (float)Math.Round(manual.studyHoursRequired / rate, 1);
        }

        public float GetEstimatedDays(string readerId, string manualId)
        {
            float effHours = GetEffectiveStudyHours(readerId, manualId);
            if (effHours <= 0f) return 0f;
            return (float)Math.Ceiling(effHours / 8.0f);
        }

        public void LoadCatalog(List<ManualDefinition> manuals)
        {
            if (manuals == null) return;
            _catalog.Clear();
            foreach (var m in manuals)
            {
                if (string.IsNullOrEmpty(m.manual_id)) continue;
                // Bug-10: skillXpGrants is documented as (skillId, xpAmount) pairs;
                // an odd-length list would IndexOutOfRange on TickDay when the loop
                // reads grants[i+1]. Reject malformed manuals at load time so the
                // bad data never reaches the tick path.
                if (m.skillXpGrants != null && m.skillXpGrants.Count % 2 != 0)
                    throw new System.IO.InvalidDataException(
                        $"manual '{m.manual_id}' has {m.skillXpGrants.Count} skillXpGrants entries — expected pairs (skillId, xpAmount)");
                _catalog[m.manual_id] = m;
            }
        }

        public ActionResult StartStudy(string manualId, string readerId)
        {
            if (!_catalog.TryGetValue(manualId, out var manual))
                return ActionResult.Failed("unknown_manual", "library.unknown_manual");

            // Bug-15b: a manual with studyHoursRequired <= 0 would complete
            // instantly on TickDay (8h >= 0 is trivially satisfied), granting
            // all XP / research / knowledge unlocks in zero time. Reject such
            // manuals at the start path so they never reach the tick loop.
            // Validated at StartStudy, not LoadCatalog, so existing catalogs
            // (with manually-curated 0-hour entries) still load.
            if (manual.studyHoursRequired <= 0)
                return ActionResult.Blocked("invalid_hours", "library.invalid_hours");

            if (_state.completedManualIds.Contains(manualId))
                return ActionResult.Blocked("already_completed", "library.already_completed");

            // Check prerequisites
            foreach (var prereq in manual.prerequisites)
            {
                if (!_state.completedManualIds.Contains(prereq))
                    return ActionResult.Blocked("missing_prerequisite", "library.missing_prerequisite");
            }

            // Check duty roster availability (B2-008: GetRoleOf checks if reader is on duty)
            if (!string.IsNullOrEmpty(_roster.GetRoleOf(readerId)))
                return ActionResult.Blocked("busy", "library.busy");

            // Reader cannot study two manuals simultaneously
            if (IsReaderStudying(readerId))
                return ActionResult.Blocked("busy", "library.busy");

            var job = new StudyJob
            {
                jobId = $"study_{_currentDay}_{manualId}_{readerId}",
                manualId = manualId, readerId = readerId, dayStarted = _currentDay
            };
            _state.activeJobs.Add(job);
            OnLibraryChanged?.Invoke();
            return ActionResult.Success("library.study_started");
        }

        public ActionResult CancelStudy(string jobId)
        {
            var job = _state.activeJobs.Find(j => j.jobId == jobId);
            if (job == null || job.isComplete || job.isCancelled)
                return ActionResult.Blocked("no_job", "library.no_job");

            job.isCancelled = true;
            OnLibraryChanged?.Invoke();
            return ActionResult.Success("library.study_cancelled");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var job in _state.activeJobs)
            {
                if (job.isComplete || job.isCancelled) continue;
                if (!_catalog.TryGetValue(job.manualId, out var manual))
                {
                    _log.Warn($"[Library] active job '{job.jobId}' references unknown manual '{job.manualId}'");
                    continue;
                }

                float rate = GetComprehensionRate(job.readerId, job.manualId);
                job.progressHours += 8f * rate;

                if (job.progressHours >= manual.studyHoursRequired)
                {
                    job.isComplete = true;
                    if (!_state.completedManualIds.Contains(job.manualId))
                        _state.completedManualIds.Add(job.manualId);
                    _state.totalStudyHours += (int)job.progressHours;

                    // Grant skill XP
                    for (int i = 0; i < manual.skillXpGrants.Count; i += 2)
                    {
                        string skillId = manual.skillXpGrants[i];
                        if (float.TryParse(manual.skillXpGrants[i + 1], out float xp))
                            _skills.RecordAction(new SimpleSkillActor(job.readerId), skillId, xp, _currentDay);
                    }

                    // Unlock research (reveal / discover only — NEVER CompleteResearch!)
                    foreach (var unlock in manual.researchUnlocks)
                        _research.UnlockManual(unlock);

                    // Add knowledge evidence (idempotent and deduped in JournalSystem)
                    foreach (var knowledge in manual.knowledgeUnlocks)
                    {
                        _journal.AddKnowledgeEvidence(job.readerId, knowledge);
                    }

                    _log.Info($"[Library] {job.readerId} completed {manual.display_name}");
                    OnJobCompleted?.Invoke(job);
                }
            }

            _state.activeJobs.RemoveAll(j => j.isCancelled);
            OnLibraryChanged?.Invoke();
        }

        public List<StudyJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);

        public bool IsManualCompleted(string manualId) => _state.completedManualIds.Contains(manualId);

        public LibraryStudyState CaptureState() => CloneState(_state);

        public void RestoreState(LibraryStudyState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static LibraryStudyState CloneState(LibraryStudyState src)
        {
            if (src == null) return new LibraryStudyState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<LibraryStudyState>(json) ?? new LibraryStudyState();
        }
    }
}
