// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Research
{
    public enum ArchiveDecryptionStatus
    {
        Discovered = 0,
        Stabilizing = 1,
        Stabilized = 2,
        Decrypting = 3,
        Completed = 4,
        Corrupted = 5
    }

    [Serializable]
    public sealed class PrewarArchiveProject
    {
        public string ArchiveId { get; set; } = string.Empty;
        public ArchiveDecryptionStatus Status { get; set; }
        public float Progress { get; set; }
        public float TargetProgress { get; set; }
        public string AssignedResearcherId { get; set; } = string.Empty;
        public int DayStarted { get; set; }
        public int DayCompleted { get; set; }
        public bool HasSolventApplied { get; set; }
    }

    [Serializable]
    public sealed class PrewarArchiveDecryptionState
    {
        public string SystemId { get; set; } = PrewarArchiveDecryptionSystem.SystemId;
        public List<PrewarArchiveProject> Projects { get; set; } = new List<PrewarArchiveProject>();
        public List<string> DiscoveredArchiveIds { get; set; } = new List<string>();
        public List<string> CompletedArchiveIds { get; set; } = new List<string>();
        public bool IsPowerOnline { get; set; } = true;
        public int TotalDecrypted { get; set; }
        public int TotalBreakthroughs { get; set; }
    }

    public sealed class PrewarArchiveDecryptionSystem
    {
        public const string SystemId = "prewar_archive_decryption";

        private PrewarArchiveDecryptionState _state = new PrewarArchiveDecryptionState();
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly PrewarArchiveCatalog _catalog;
        private readonly ILog _log;
        private int _currentDay;

        public PrewarArchiveDecryptionState State => _state;
        public bool IsPowerOnline => _state.IsPowerOnline;
        public int TotalDecrypted => _state.TotalDecrypted;

        public event Action<string>? OnArchiveDiscovered;
        public event Action<PrewarArchiveProject>? OnArchiveStabilized;
        public event Action<PrewarArchiveProject, PrewarArchiveDef>? OnArchiveDecrypted;
        public event Action<PrewarArchiveProject, float>? OnDecryptionBreakthrough;

        public PrewarArchiveDecryptionSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            PrewarArchiveCatalog catalog,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public void SetPowerStatus(bool isOnline)
        {
            _state.IsPowerOnline = isOnline;
        }

        public ActionResult DiscoverArchive(string archiveId, int currentDay)
        {
            var def = _catalog.GetArchive(archiveId);
            if (def == null)
                return ActionResult.Blocked("unknown_archive", "archive.unknown");

            if (_state.DiscoveredArchiveIds.Contains(archiveId))
                return ActionResult.Blocked("already_discovered", "archive.already_discovered");

            _state.DiscoveredArchiveIds.Add(archiveId);

            var project = new PrewarArchiveProject
            {
                ArchiveId = archiveId,
                Status = ArchiveDecryptionStatus.Discovered,
                Progress = 0f,
                TargetProgress = def.base_effort_points,
                AssignedResearcherId = string.Empty,
                DayStarted = currentDay,
                DayCompleted = -1,
                HasSolventApplied = false
            };

            _state.Projects.Add(project);
            _log.Info($"[PrewarArchive] Discovered archive: {def.display_name} ({archiveId})");
            OnArchiveDiscovered?.Invoke(archiveId);
            return ActionResult.Success("archive.discovered");
        }

        public ActionResult StabilizeArchive(string archiveId, string researcherId, int currentDay)
        {
            var project = _state.Projects.Find(p => p.ArchiveId == archiveId);
            if (project == null)
                return ActionResult.Blocked("not_found", "archive.not_found");

            if (project.Status != ArchiveDecryptionStatus.Discovered)
                return ActionResult.Blocked("invalid_status", "archive.not_in_discovered_state");

            var def = _catalog.GetArchive(archiveId);
            if (def == null)
                return ActionResult.Blocked("unknown_archive", "archive.unknown");

            // Consume solvent chemicals atomically
            var bill = new Dictionary<string, int>
            {
                { def.cleaning_solvent_id, def.cleaning_solvent_count }
            };

            if (!_inventory.TryConsumeBill(bill))
                return ActionResult.Blocked("insufficient_solvent", "archive.insufficient_solvent");

            project.HasSolventApplied = true;
            project.Status = ArchiveDecryptionStatus.Stabilized;
            project.AssignedResearcherId = researcherId;
            project.DayStarted = currentDay;

            _log.Info($"[PrewarArchive] Stabilized archive: {def.display_name}");
            OnArchiveStabilized?.Invoke(project);
            return ActionResult.Success("archive.stabilized");
        }

        public ActionResult StartDecryption(string archiveId, string researcherId, int currentDay)
        {
            var project = _state.Projects.Find(p => p.ArchiveId == archiveId);
            if (project == null)
                return ActionResult.Blocked("not_found", "archive.not_found");

            if (project.Status != ArchiveDecryptionStatus.Stabilized)
                return ActionResult.Blocked("not_stabilized", "archive.must_be_stabilized_first");

            project.Status = ArchiveDecryptionStatus.Decrypting;
            project.AssignedResearcherId = researcherId;

            _log.Info($"[PrewarArchive] Started decrypting: {archiveId} with researcher {researcherId}");
            return ActionResult.Success("archive.decryption_started");
        }

        public ActionResult AssignResearcher(string archiveId, string researcherId)
        {
            var project = _state.Projects.Find(p => p.ArchiveId == archiveId);
            if (project == null)
                return ActionResult.Blocked("not_found", "archive.not_found");

            if (project.Status == ArchiveDecryptionStatus.Completed || project.Status == ArchiveDecryptionStatus.Corrupted)
                return ActionResult.Blocked("finished", "archive.already_finished");

            project.AssignedResearcherId = researcherId;
            return ActionResult.Success("archive.researcher_assigned");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            if (!_state.IsPowerOnline)
            {
                // Laboratory computers offline without power
                return;
            }

            foreach (var project in _state.Projects)
            {
                if (project.Status != ArchiveDecryptionStatus.Decrypting) continue;
                if (string.IsNullOrEmpty(project.AssignedResearcherId)) continue;

                var def = _catalog.GetArchive(project.ArchiveId);
                if (def == null) continue;

                // Base daily cryptanalysis progress = 15.0f
                float dailyProgress = 15.0f;

                // Deterministic breakthrough roll via ISeededRng
                double roll = _rng.NextDouble();
                if (roll < 0.15) // 15% chance of cryptographic breakthrough
                {
                    float bonus = 10.0f;
                    dailyProgress += bonus;
                    _state.TotalBreakthroughs++;
                    OnDecryptionBreakthrough?.Invoke(project, bonus);
                    _log.Info($"[PrewarArchive] Cryptographic breakthrough on {project.ArchiveId}! +{bonus} progress");
                }

                project.Progress += dailyProgress;

                if (project.Progress >= project.TargetProgress)
                {
                    project.Progress = project.TargetProgress;
                    project.Status = ArchiveDecryptionStatus.Completed;
                    project.DayCompleted = day;

                    if (!_state.CompletedArchiveIds.Contains(project.ArchiveId))
                    {
                        _state.CompletedArchiveIds.Add(project.ArchiveId);
                        _state.TotalDecrypted++;
                    }

                    _log.Info($"[PrewarArchive] Decryption COMPLETED for {def.display_name}!");
                    OnArchiveDecrypted?.Invoke(project, def);
                }
            }
        }

        public PrewarArchiveProject? GetProject(string archiveId)
        {
            return _state.Projects.Find(p => p.ArchiveId == archiveId);
        }

        public PrewarArchiveDecryptionState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<PrewarArchiveDecryptionState>(json) ?? new PrewarArchiveDecryptionState();
        }

        public void RestoreState(PrewarArchiveDecryptionState saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<PrewarArchiveDecryptionState>(json) ?? new PrewarArchiveDecryptionState();
        }
    }
}
