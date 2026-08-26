using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Journal;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ArchiveDeskState
    {
        public string systemId = ArchiveDeskSystem.SystemId;
        public List<TranscriptionJob> queue = new List<TranscriptionJob>();
        public List<string> unlockedEvidenceIds = new List<string>();
        public int totalTranscriptions;
    }

    [Serializable]
    public sealed class InkMaterialDefinition
    {
        public string ink_id = string.Empty;
        public string display_name = string.Empty;
        public float legibilityScore = 1f;      // 0-1
        public float archivalLongevityDays = 365f;
        public float fadeRatePerDay = 0.001f;
        public string requiredItemId = string.Empty;
        public int requiredAmount = 1;
    }

    [Serializable]
    public sealed class TranscriptionJob
    {
        public string jobId = string.Empty;
        public string evidenceId = string.Empty;
        public string archivistId = string.Empty;
        public string inkId = string.Empty;
        public int dayStarted = -1;
        public float progressHours;
        public float totalHoursRequired = 4f;
        public bool isComplete;
        public bool isCancelled;
        public float legibilityScore = 1f;
        public string journalEntryId = string.Empty;
    }

    public sealed class ArchiveDeskSystem
    {
        private sealed class SimpleAuthor : ISurvivorAuthor
        {
            public string Id { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public RiskBiasTrait RiskBias { get; set; } = RiskBiasTrait.Realist;
        }
        public const string SystemId = "archive_desk";
        private ArchiveDeskState _state = new ArchiveDeskState();
        private readonly Dictionary<string, InkMaterialDefinition> _inkCatalog = new Dictionary<string, InkMaterialDefinition>(StringComparer.Ordinal);
        private readonly ILog _log;
        private readonly JournalSystem _journal;
        private readonly KnowledgeBase _knowledge;
        private readonly Inventory.Inventory _inventory;
        private readonly DutyRosterSystem _roster;
        private int _currentDay;

        public ArchiveDeskState State => _state;
        public IReadOnlyDictionary<string, InkMaterialDefinition> Catalog => _inkCatalog;
        public event Action<TranscriptionJob> OnJobCompleted;
        public event Action OnArchiveChanged;

        public ArchiveDeskSystem(
            JournalSystem journal,
            KnowledgeBase knowledge,
            Inventory.Inventory inventory,
            DutyRosterSystem roster,
ILog? log = null)
        {
            _journal = journal ?? throw new ArgumentNullException(nameof(journal));
            _knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _log = log ?? NullLog.Instance;
        }

        public void LoadInkCatalog(List<InkMaterialDefinition> inks)
        {
            if (inks == null) return;
            _inkCatalog.Clear();
            foreach (var ink in inks)
                if (!string.IsNullOrEmpty(ink.ink_id))
                    _inkCatalog[ink.ink_id] = ink;
        }

        public ActionResult QueueTranscription(string evidenceId, string archivistId, string inkId)
        {
            if (_state.unlockedEvidenceIds.Contains(evidenceId))
                return ActionResult.Blocked("already_unlocked", "archive.already_unlocked");

            if (!_inkCatalog.TryGetValue(inkId, out var ink))
                return ActionResult.Failed("unknown_ink", "archive.unknown_ink");

            // CR3-04 atomicity: pre-check ALL gating predicates (ink availability
            // AND roster state) BEFORE any inventory mutation. Pre-fix, ink was
            // consumed before the roster check, leaving a busy archivist with
            // their inventory drained and no transcription queued. Twin to
            // CR3-02 (Kitchen) and CR3-03 (Equipment) atomicity pattern.
            if (_inventory.CountById(ink.requiredItemId) < ink.requiredAmount)
                return ActionResult.Blocked("insufficient_ink", "archive.insufficient_ink");

            // Bug-14 follow-on: GetAssignment takes a ROLE, not a survivorId.
            // Use GetRoleOf so an archivist currently held on a duty shift
            // actually triggers the busy block.
            if (_roster.GetRoleOf(archivistId) != null)
                return ActionResult.Blocked("busy", "archive.busy");

            _inventory.RemoveById(ink.requiredItemId, ink.requiredAmount);

            var job = new TranscriptionJob
            {
                jobId = $"trans_{_currentDay}_{evidenceId}_{archivistId}",
                evidenceId = evidenceId, archivistId = archivistId, inkId = inkId,
                dayStarted = _currentDay,
                legibilityScore = ink.legibilityScore,
                totalHoursRequired = 4f
            };
            _state.queue.Add(job);
            OnArchiveChanged?.Invoke();
            return ActionResult.Success("archive.queued");
        }

        public ActionResult CancelJob(string jobId)
        {
            var job = _state.queue.Find(j => j.jobId == jobId);
            if (job == null || job.isComplete || job.isCancelled)
                return ActionResult.Blocked("no_job", "archive.no_job");

            job.isCancelled = true;
            // Refund ink
            if (_inkCatalog.TryGetValue(job.inkId, out var ink))
                _inventory.AddById(ink.requiredItemId, ink.requiredAmount);

            OnArchiveChanged?.Invoke();
            return ActionResult.Success("archive.job_cancelled");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var job in _state.queue)
            {
                if (job.isComplete || job.isCancelled) continue;

                job.progressHours += 8f; // standard work day

                if (job.progressHours >= job.totalHoursRequired)
                {
                    job.isComplete = true;
                    _state.totalTranscriptions++;

                    // Create journal entry via discovery
                    var author = new SimpleAuthor { Id = job.archivistId, DisplayName = job.archivistId };
                    var entry = _journal.TryDiscover(job.evidenceId, author, day);
                    job.journalEntryId = entry?.KnowledgeKey ?? string.Empty;

                    // Unlock knowledge
                    _knowledge.Discover(job.evidenceId);

                    _state.unlockedEvidenceIds.Add(job.evidenceId);
                    _log.Info($"[Archive] {job.archivistId} transcribed {job.evidenceId} (legibility={job.legibilityScore:F2})");
                    OnJobCompleted?.Invoke(job);
                }
            }

            _state.queue.RemoveAll(j => j.isCancelled);
            OnArchiveChanged?.Invoke();
        }

        public List<TranscriptionJob> GetActiveJobs() => _state.queue.FindAll(j => !j.isComplete && !j.isCancelled);

        public bool IsEvidenceUnlocked(string evidenceId) => _state.unlockedEvidenceIds.Contains(evidenceId);

        public ArchiveDeskState CaptureState() => CloneState(_state);

        public void RestoreState(ArchiveDeskState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ArchiveDeskState CloneState(ArchiveDeskState src)
        {
            if (src == null) return new ArchiveDeskState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ArchiveDeskState>(json) ?? new ArchiveDeskState();
        }
    }
}
