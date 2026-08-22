using System;
using System.Collections.Generic;
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
        public string manual_id = string.Empty;
        public string display_name = string.Empty;
        public string category = string.Empty;       // "technical", "medical", "military", etc.
        public int studyHoursRequired = 10;
        public float fatiguePerHour = 0.3f;
        public float moraleEffect = -0.5f;           // studying is draining
        public List<string> skillXpGrants = new List<string>(); // skill_id, xp_amount pairs
        public List<string> researchUnlocks = new List<string>();
        public List<string> knowledgeUnlocks = new List<string>();
        public List<string> prerequisites = new List<string>();
        public bool requiresPower = true;
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
        public event Action<StudyJob> OnJobCompleted;
        public event Action OnLibraryChanged;

        public LibraryStudySystem(
            SkillProgressionSystem skills,
            ResearchSystem research,
            JournalSystem journal,
            DutyRosterSystem roster,
            ILog log = null)
        {
            _skills = skills ?? throw new ArgumentNullException(nameof(skills));
            _research = research ?? throw new ArgumentNullException(nameof(research));
            _journal = journal ?? throw new ArgumentNullException(nameof(journal));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(List<ManualDefinition> manuals)
        {
            if (manuals == null) return;
            _catalog.Clear();
            foreach (var m in manuals)
                if (!string.IsNullOrEmpty(m.manual_id))
                    _catalog[m.manual_id] = m;
        }

        public ActionResult StartStudy(string manualId, string readerId)
        {
            if (!_catalog.TryGetValue(manualId, out var manual))
                return ActionResult.Failed("unknown_manual", "library.unknown_manual");

            if (_state.completedManualIds.Contains(manualId))
                return ActionResult.Blocked("already_completed", "library.already_completed");

            // Check prerequisites
            foreach (var prereq in manual.prerequisites)
            {
                if (!_state.completedManualIds.Contains(prereq))
                    return ActionResult.Blocked("missing_prerequisite", "library.missing_prerequisite");
            }

            // Check duty roster availability
            if (_roster.GetAssignment(readerId) != null)
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
                if (!_catalog.TryGetValue(job.manualId, out var manual)) continue;

                job.progressHours += 8f; // standard study day

                if (job.progressHours >= manual.studyHoursRequired)
                {
                    job.isComplete = true;
                    _state.completedManualIds.Add(job.manualId);
                    _state.totalStudyHours += (int)job.progressHours;

                    // Grant skill XP
                    for (int i = 0; i < manual.skillXpGrants.Count; i += 2)
                    {
                        string skillId = manual.skillXpGrants[i];
                        if (float.TryParse(manual.skillXpGrants[i + 1], out float xp))
                            _skills.RecordAction(new SimpleSkillActor(job.readerId), skillId, xp, _currentDay);
                    }

                    // Unlock research
                    foreach (var unlock in manual.researchUnlocks)
                        _research.UnlockManual(unlock);

                    // Add knowledge evidence
                    foreach (var knowledge in manual.knowledgeUnlocks)
                        _journal.AddKnowledgeEvidence(job.readerId, knowledge);

                    _log.Info($"[Library] {job.readerId} completed {manual.display_name}");
                    OnJobCompleted?.Invoke(job);
                }
            }

            _state.activeJobs.RemoveAll(j => j.isCancelled);
            OnLibraryChanged?.Invoke();
        }

        public List<StudyJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);

        public bool IsManualCompleted(string manualId) => _state.completedManualIds.Contains(manualId);

        public LibraryStudyState CaptureState() => _state;
        public void RestoreState(LibraryStudyState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnLibraryChanged?.Invoke();
        }
    }
}
