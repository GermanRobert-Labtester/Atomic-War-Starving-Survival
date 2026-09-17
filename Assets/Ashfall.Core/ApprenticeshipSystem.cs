// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class MentorshipDef
    {
        public string mentorship_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string discipline { get; set; } = string.Empty;
        public string required_room_tag { get; set; } = "workshop";
        public float daily_xp_rate { get; set; } = 15f;
        public string manual_item_id { get; set; } = string.Empty;
        public int manual_transcription_days { get; set; } = 4;
        public string legacy_trait_id { get; set; } = "trait_stoic_craftsman";
        public float legacy_xp_grant { get; set; } = 150f;
    }

    [Serializable]
    public sealed class MentorshipCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<MentorshipDef> mentorships { get; set; } = new List<MentorshipDef>();
    }

    [Serializable]
    public sealed class TranscriptionTask
    {
        public string taskId = string.Empty;
        public string survivorId = string.Empty;
        public string mentorshipId = string.Empty;
        public int daysWorked;
        public int daysRequired = 4;
        public bool isComplete;
    }

    [Serializable]
    public sealed class SurvivorWill
    {
        public string willId = string.Empty;
        public string testatorSurvivorId = string.Empty;
        public string primaryBeneficiaryId = string.Empty;
        public string fallbackBeneficiaryId = string.Empty;
        public List<string> bequeathedItemIds = new List<string>();
        public string finalWords = string.Empty;
        public bool isExecuted;
        public int executionDay = -1;
    }

    [Serializable]
    public sealed class ApprenticeshipState
    {
        public string systemId = ApprenticeshipSystem.SystemId;
        public List<Apprenticeship> activePairs = new List<Apprenticeship>();
        public List<string> completedSkillIds = new List<string>();
        public List<TranscriptionTask> transcriptionTasks = new List<TranscriptionTask>();
        public List<SurvivorWill> registeredWills = new List<SurvivorWill>();
        public List<SurvivorWill> executedWills = new List<SurvivorWill>();
        public Dictionary<string, List<string>> legacyTraitsGranted = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        public SkillProgressionSaveState skillProgression = new SkillProgressionSaveState();
    }

    [Serializable]
    public sealed class Apprenticeship
    {
        public string pairId = string.Empty;
        public string mentorId = string.Empty;
        public string apprenticeId = string.Empty;
        public string targetSkillId = string.Empty;
        public string mentorshipId = string.Empty;
        public string requiredRoomTag = string.Empty;
        public float progressXp;
        public float targetXp = 100f;
        public int dayStarted = -1;
        public bool isComplete;
        public bool isCancelled;
        public bool isLegacyInherited;
        public string milestonePerkId = string.Empty;
    }

    public sealed class ApprenticeshipSystem
    {
        public const string SystemId = "apprenticeship";
        public const string CatalogPath = "apprenticeship_catalog.json";
        private ApprenticeshipState _state = new ApprenticeshipState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly SkillProgressionSystem _skills;
        private readonly DutyRosterSystem _roster;
        private readonly SurvivorRelationsSystem _relations;
        private readonly ShelterAssignmentSystem? _assignments;
        private readonly InventoryContainer? _inventory;
        private readonly Dictionary<string, MentorshipDef> _catalog = new Dictionary<string, MentorshipDef>(StringComparer.Ordinal);
        private int _currentDay;

        public ApprenticeshipState State => _state;
        public IReadOnlyDictionary<string, MentorshipDef> Catalog => _catalog;

        public int MaxConcurrentPairs { get; set; } = 3;
        public Func<string, bool>? IsApprenticeEligible { get; set; }

        public event Action<Apprenticeship>? OnApprenticeshipCompleted;
        public event Action? OnApprenticeshipChanged;
        public event Action<string, string>? OnManualTranscribed; // survivorId, manualItemId
        public event Action<SurvivorWill, string>? OnWillExecuted; // will, recipientSurvivorId
        public event Action<string, string, float>? OnMentorLegacyInherited; // apprenticeId, traitId, xpGranted

        public ApprenticeshipSystem(
            ISeededRng rng,
            SkillProgressionSystem skills,
            DutyRosterSystem roster,
            SurvivorRelationsSystem relations,
            ILog? log = null)
            : this(rng, skills, roster, relations, null, null, log)
        {
        }

        public ApprenticeshipSystem(
            ISeededRng rng,
            SkillProgressionSystem skills,
            DutyRosterSystem roster,
            SurvivorRelationsSystem relations,
            ShelterAssignmentSystem? assignments,
            InventoryContainer? inventory = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _skills = skills ?? throw new ArgumentNullException(nameof(skills));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _relations = relations ?? throw new ArgumentNullException(nameof(relations));
            _assignments = assignments;
            _inventory = inventory;
            _log = log ?? NullLog.Instance;

            RegisterDefaultMentorships();
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                var cat = serializer.Deserialize<MentorshipCatalog>(jsonContent);
                if (cat?.mentorships != null)
                {
                    foreach (var m in cat.mentorships)
                        RegisterMentorship(m);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[Apprentice] Failed to load mentorship catalog: {ex.Message}");
            }
        }

        public void RegisterMentorship(MentorshipDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.mentorship_id)) return;
            _catalog[def.mentorship_id] = def;
        }

        private void RegisterDefaultMentorships()
        {
            RegisterMentorship(new MentorshipDef
            {
                mentorship_id = "mentorship_generator_maintenance",
                name = "Generator & Radiator Systems Engineering",
                discipline = "engineering",
                required_room_tag = "workshop",
                daily_xp_rate = 15f,
                manual_item_id = "item_manual_generator_maintenance",
                manual_transcription_days = 4,
                legacy_trait_id = "trait_stoic_craftsman",
                legacy_xp_grant = 150f
            });
            RegisterMentorship(new MentorshipDef
            {
                mentorship_id = "mentorship_field_medicine",
                name = "Wasteland Trauma & Surgery Triage",
                discipline = "medicine",
                required_room_tag = "medical",
                daily_xp_rate = 14f,
                manual_item_id = "item_manual_field_medicine",
                manual_transcription_days = 5,
                legacy_trait_id = "trait_hardened_disciple",
                legacy_xp_grant = 160f
            });
            RegisterMentorship(new MentorshipDef
            {
                mentorship_id = "mentorship_rough_repairs",
                name = "Emergency Shoring & Pneumatic Tooling",
                discipline = "crafting",
                required_room_tag = "workshop",
                daily_xp_rate = 16f,
                manual_item_id = "item_manual_rough_repairs",
                manual_transcription_days = 3,
                legacy_trait_id = "trait_stoic_craftsman",
                legacy_xp_grant = 140f
            });
            RegisterMentorship(new MentorshipDef
            {
                mentorship_id = "mentorship_seismology",
                name = "Sub-Strata Geomechanics & Fault Sensing",
                discipline = "seismology",
                required_room_tag = "laboratory",
                daily_xp_rate = 12f,
                manual_item_id = "item_manual_seismology",
                manual_transcription_days = 6,
                legacy_trait_id = "trait_hardened_disciple",
                legacy_xp_grant = 180f
            });
        }

        public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f, string? mentorshipId = null)
        {
            int activeCount = _state.activePairs.FindAll(p => !p.isComplete && !p.isCancelled).Count;
            if (activeCount >= MaxConcurrentPairs)
                return ActionResult.Blocked("capacity_full", "apprentice.capacity_full");

            if (IsApprenticeEligible != null && !IsApprenticeEligible(apprenticeId))
                return ActionResult.Blocked("apprentice_ineligible", "apprentice.apprentice_ineligible");

            if (_roster.GetRoleOf(mentorId) != null)
                return ActionResult.Blocked("mentor_busy", "apprentice.mentor_busy");
            if (_roster.GetRoleOf(apprenticeId) != null)
                return ActionResult.Blocked("apprentice_busy", "apprentice.apprentice_busy");

            // Check eligibility
            float mentorSkill = _skills.GetXp(mentorId, targetSkillId);
            if (mentorSkill < 30f)
                return ActionResult.Blocked("mentor_unqualified", "apprentice.mentor_unqualified");

            if (_state.activePairs.Exists(p => p.mentorId == mentorId && p.apprenticeId == apprenticeId))
                return ActionResult.Blocked("pair_exists", "apprentice.pair_exists");

            string reqTag = string.Empty;
            if (!string.IsNullOrEmpty(mentorshipId) && _catalog.TryGetValue(mentorshipId, out var mDef))
            {
                reqTag = mDef.required_room_tag;
            }

            var pair = new Apprenticeship
            {
                pairId = $"appr_{_currentDay}_{mentorId}_{apprenticeId}",
                mentorId = mentorId,
                apprenticeId = apprenticeId,
                targetSkillId = targetSkillId,
                mentorshipId = mentorshipId ?? string.Empty,
                requiredRoomTag = reqTag,
                targetXp = targetXp,
                dayStarted = _currentDay
            };
            _state.activePairs.Add(pair);
            OnApprenticeshipChanged?.Invoke();
            return ActionResult.Success("apprentice.pair_started");
        }

        public ActionResult CancelPair(string pairId)
        {
            var pair = _state.activePairs.Find(p => p.pairId == pairId);
            if (pair == null || pair.isComplete || pair.isCancelled)
                return ActionResult.Blocked("no_pair", "apprentice.no_pair");

            _state.activePairs.Remove(pair);
            OnApprenticeshipChanged?.Invoke();
            return ActionResult.Success("apprentice.pair_cancelled");
        }

        public ActionResult StartTranscription(string survivorId, string mentorshipId, InventoryContainer? inv = null)
        {
            if (!_catalog.TryGetValue(mentorshipId, out var def))
                return ActionResult.Failed("unknown_mentorship", "apprentice.unknown_mentorship");

            if (_state.transcriptionTasks.Exists(t => t.survivorId == survivorId && !t.isComplete))
                return ActionResult.Blocked("already_transcribing", "apprentice.already_transcribing");

            // Verify survivor skill in discipline
            float currentXp = _skills.GetXp(survivorId, def.discipline);
            if (currentXp < 25f)
                return ActionResult.Blocked("survivor_unqualified", "apprentice.survivor_unqualified");

            var invToUse = inv ?? _inventory;
            if (invToUse != null)
            {
                // Optional paper/supplies check
                if (invToUse.HasSufficient("item_scrap_metal", 2))
                    invToUse.TryConsume("item_scrap_metal", 2);
            }

            var task = new TranscriptionTask
            {
                taskId = $"task_transcribe_{_currentDay}_{survivorId}_{mentorshipId}",
                survivorId = survivorId,
                mentorshipId = mentorshipId,
                daysWorked = 0,
                daysRequired = def.manual_transcription_days
            };
            _state.transcriptionTasks.Add(task);
            OnApprenticeshipChanged?.Invoke();

            return ActionResult.Success("apprentice.transcription_started",
                new Dictionary<string, double> { { "days_required", def.manual_transcription_days } });
        }

        public ActionResult RegisterWill(SurvivorWill will)
        {
            if (will == null || string.IsNullOrEmpty(will.testatorSurvivorId))
                return ActionResult.Failed("invalid_will", "apprentice.invalid_will");

            if (string.IsNullOrEmpty(will.primaryBeneficiaryId))
                return ActionResult.Failed("missing_beneficiary", "apprentice.missing_beneficiary");

            if (string.IsNullOrEmpty(will.willId))
                will.willId = $"will_{will.testatorSurvivorId}";

            _state.registeredWills.RemoveAll(w => w.testatorSurvivorId == will.testatorSurvivorId);
            _state.registeredWills.Add(will);
            OnApprenticeshipChanged?.Invoke();

            return ActionResult.Success("apprentice.will_registered");
        }

        public ActionResult ExecuteWill(string deceasedSurvivorId, InventoryContainer targetInventory, HashSet<string>? livingSurvivors = null)
        {
            var will = _state.registeredWills.Find(w => w.testatorSurvivorId == deceasedSurvivorId && !w.isExecuted);
            if (will == null)
                return ActionResult.Blocked("no_will_found", "apprentice.no_will_found");

            if (targetInventory == null)
                return ActionResult.Failed("missing_inventory", "apprentice.missing_inventory");

            // Select living recipient
            string recipientId = will.primaryBeneficiaryId;
            if (livingSurvivors != null && !livingSurvivors.Contains(recipientId))
            {
                if (!string.IsNullOrEmpty(will.fallbackBeneficiaryId) && livingSurvivors.Contains(will.fallbackBeneficiaryId))
                    recipientId = will.fallbackBeneficiaryId;
                else
                    recipientId = "shelter_common_store";
            }

            // Atomically grant bequeathed items into inventory
            int transferredCount = 0;
            if (will.bequeathedItemIds != null)
            {
                foreach (var itemId in will.bequeathedItemIds)
                {
                    if (!string.IsNullOrEmpty(itemId))
                    {
                        targetInventory.TryProduce(itemId, 1);
                        transferredCount++;
                    }
                }
            }

            will.isExecuted = true;
            will.executionDay = _currentDay;
            _state.executedWills.Add(will);
            _state.registeredWills.Remove(will);

            _log.Info($"[Apprentice] Will of {deceasedSurvivorId} executed for recipient {recipientId} ({transferredCount} items transferred)");
            OnWillExecuted?.Invoke(will, recipientId);
            OnApprenticeshipChanged?.Invoke();

            return ActionResult.Success("apprentice.will_executed",
                new Dictionary<string, double> { { "transferred_items", transferredCount } });
        }

        public void NotifyMentorDeath(string deceasedMentorId)
        {
            var activePairs = _state.activePairs.FindAll(p => p.mentorId == deceasedMentorId && !p.isComplete && !p.isCancelled);
            foreach (var pair in activePairs)
            {
                float grantXp = 150f;
                string traitId = "trait_hardened_disciple";

                if (!string.IsNullOrEmpty(pair.mentorshipId) && _catalog.TryGetValue(pair.mentorshipId, out var def))
                {
                    grantXp = def.legacy_xp_grant;
                    traitId = def.legacy_trait_id;
                }

                pair.isComplete = true;
                pair.isLegacyInherited = true;

                // Grant legacy XP
                _skills.RecordAction(new SimpleSkillActor(pair.apprenticeId), pair.targetSkillId, grantXp, _currentDay);

                // Record legacy trait
                if (!_state.legacyTraitsGranted.TryGetValue(pair.apprenticeId, out var traitList))
                {
                    traitList = new List<string>();
                    _state.legacyTraitsGranted[pair.apprenticeId] = traitList;
                }
                if (!traitList.Contains(traitId))
                    traitList.Add(traitId);

                _log.Info($"[Apprentice] Mentor {deceasedMentorId} died. Apprentice {pair.apprenticeId} inherited legacy trait {traitId} and {grantXp} XP.");
                OnMentorLegacyInherited?.Invoke(pair.apprenticeId, traitId, grantXp);
                OnApprenticeshipCompleted?.Invoke(pair);
            }

            OnApprenticeshipChanged?.Invoke();
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // 1. Advance Apprenticeships
            foreach (var pair in _state.activePairs)
            {
                if (pair.isComplete || pair.isCancelled) continue;

                // Co-occupancy check
                float dailyRate = 10f;
                if (!string.IsNullOrEmpty(pair.mentorshipId) && _catalog.TryGetValue(pair.mentorshipId, out var mDef))
                {
                    dailyRate = mDef.daily_xp_rate;
                }

                if (_assignments != null)
                {
                    if (!_assignments.AreInSameRoom(pair.mentorId, pair.apprenticeId))
                    {
                        dailyRate *= 0.5f; // reduced rate when not working in same room
                    }
                }

                pair.progressXp += dailyRate;

                if (pair.progressXp >= pair.targetXp)
                {
                    pair.isComplete = true;
                    _state.completedSkillIds.Add(pair.targetSkillId);

                    _skills.RecordAction(new SimpleSkillActor(pair.apprenticeId), pair.targetSkillId, pair.targetXp, _currentDay);
                    _relations.ModifyAffinity(pair.mentorId, pair.apprenticeId, 10f);

                    _log.Info($"[Apprentice] {pair.apprenticeId} completed {pair.targetSkillId} under {pair.mentorId}");
                    OnApprenticeshipCompleted?.Invoke(pair);
                }
            }

            _state.activePairs.RemoveAll(p => p.isCancelled);

            // 2. Advance Transcription Tasks
            foreach (var task in _state.transcriptionTasks)
            {
                if (task.isComplete) continue;

                task.daysWorked++;
                if (task.daysWorked >= task.daysRequired)
                {
                    task.isComplete = true;
                    if (_catalog.TryGetValue(task.mentorshipId, out var def) && !string.IsNullOrEmpty(def.manual_item_id))
                    {
                        _inventory?.TryProduce(def.manual_item_id, 1);
                        _log.Info($"[Apprentice] Survivor {task.survivorId} finished transcribing manual {def.manual_item_id}");
                        OnManualTranscribed?.Invoke(task.survivorId, def.manual_item_id);
                    }
                }
            }

            OnApprenticeshipChanged?.Invoke();
        }

        public List<Apprenticeship> GetActivePairs() => _state.activePairs.FindAll(p => !p.isComplete && !p.isCancelled);

        public ApprenticeshipState CaptureState() => CloneState(_state);

        public void RestoreState(ApprenticeshipState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ApprenticeshipState CloneState(ApprenticeshipState src)
        {
            if (src == null) return new ApprenticeshipState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ApprenticeshipState>(json) ?? new ApprenticeshipState();
        }
    }
}
