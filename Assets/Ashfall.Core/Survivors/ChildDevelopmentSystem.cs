// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Survivors
{
    public enum DevelopmentStage
    {
        Infant = 0,
        Toddler = 1,
        Child = 2,
        Adolescent = 3,
        YoungAdult = 4
    }

    [Serializable]
    public sealed class ChildProfile
    {
        public string ChildId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int BirthDay { get; set; } = 1;
        public DevelopmentStage Stage { get; set; } = DevelopmentStage.Infant;
        public string AssignedCaregiverId { get; set; } = string.Empty;
        public float EducationScore { get; set; } = 0f;
        public float ChoreEfficiency { get; set; } = 0f;
        public List<string> ParentIds { get; set; } = new List<string>();
        public List<string> Milestones { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class DevelopmentMilestoneEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string ChildId { get; set; } = string.Empty;
        public DevelopmentStage NewStage { get; set; }
        public int Day { get; set; }
        public string MilestoneName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ChildDevelopmentState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<ChildProfile> Profiles { get; set; } = new List<ChildProfile>();
        public List<DevelopmentMilestoneEvent> MilestoneHistory { get; set; } = new List<DevelopmentMilestoneEvent>();
    }

    /// <summary>
    /// Plan 183 — Child Development Stages System.
    /// Tracks developmental progression (infant → toddler → child → adolescent → young adult),
    /// age-appropriate capabilities, education, chore capacity, and milestone transitions.
    /// </summary>
    public sealed class ChildDevelopmentSystem
    {
        private readonly ChildDevelopmentState _state;
        private readonly List<DevelopmentTraitDef> _traits = new List<DevelopmentTraitDef>();

        public event Action<ChildProfile, DevelopmentStage>? OnStageChanged;
        public event Action<DevelopmentMilestoneEvent>? OnMilestoneAchieved;

        public int ChildCount => _state.Profiles.Count;

        public ChildDevelopmentSystem(ChildDevelopmentState? state = null)
        {
            _state = state ?? new ChildDevelopmentState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<DevelopmentTraitsCatalog>(json, options);
                if (catalog?.traits == null) return;

                _traits.Clear();
                foreach (var t in catalog.traits)
                {
                    if (!string.IsNullOrWhiteSpace(t.trait_id))
                        _traits.Add(t);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyList<DevelopmentTraitDef> GetAllTraits() => _traits;

        public DevelopmentTraitDef? GetTrait(string traitId)
        {
            return _traits.FirstOrDefault(t => string.Equals(t.trait_id, traitId, StringComparison.OrdinalIgnoreCase));
        }

        public static DevelopmentStage ResolveStage(int ageDays)
        {
            if (ageDays < 60) return DevelopmentStage.Infant;
            if (ageDays < 180) return DevelopmentStage.Toddler;
            if (ageDays < 500) return DevelopmentStage.Child;
            if (ageDays < 720) return DevelopmentStage.Adolescent;
            return DevelopmentStage.YoungAdult;
        }

        /// <summary>
        /// Canonical age adapter for Plan 183. Birth day and adulthood flags
        /// come from the existing <see cref="GenerationalSystem"/> record;
        /// this method derives a presentation stage without creating a
        /// second age clock or mutating the source record.
        /// </summary>
        public static int ResolveCanonicalAgeDays(int birthDay, int currentDay)
            => Math.Max(0, currentDay - Math.Max(1, birthDay));

        public static DevelopmentStage ResolveCanonicalStage(
            int birthDay,
            int currentDay,
            bool adultTransitionCompleted = false)
            => adultTransitionCompleted
                ? DevelopmentStage.YoungAdult
                : ResolveStage(ResolveCanonicalAgeDays(birthDay, currentDay));

        /// <summary>
        /// Projects the canonical GenerationalSystem child into the legacy
        /// Plan 183 read shape. The returned profile is a detached view: it
        /// must never be restored as an independent authority.
        /// </summary>
        public static ChildProfile ProjectCanonicalChild(ChildDevelopment canonical, int currentDay)
        {
            if (canonical == null) throw new ArgumentNullException(nameof(canonical));

            return new ChildProfile
            {
                ChildId = canonical.survivorId,
                Name = canonical.survivorId,
                BirthDay = Math.Max(1, canonical.birthDay),
                Stage = ResolveCanonicalStage(canonical.birthDay, currentDay, canonical.adulthoodProcessed),
                AssignedCaregiverId = canonical.assignedGuardianId ?? string.Empty,
                EducationScore = Math.Clamp(canonical.educationXp, 0f, 100f),
                // Chore capacity remains derived by this projection only;
                // the duty/roster owners still decide whether work is legal.
                ChoreEfficiency = canonical.developmentPhase >= DevelopmentPhase.OlderChild ?
                    Math.Clamp(canonical.developmentProgress, 0f, 100f) : 0f
            };
        }

        public ChildProfile RegisterChild(
            string childId,
            string name,
            int birthDay,
            IEnumerable<string>? parentIds = null,
            string caregiverId = "")
        {
            if (string.IsNullOrEmpty(childId)) throw new ArgumentNullException(nameof(childId));

            var profile = _state.Profiles.FirstOrDefault(p => string.Equals(p.ChildId, childId, StringComparison.OrdinalIgnoreCase));
            if (profile == null)
            {
                profile = new ChildProfile
                {
                    ChildId = childId,
                    Name = string.IsNullOrWhiteSpace(name) ? childId : name.Trim(),
                    BirthDay = Math.Max(1, birthDay),
                    Stage = ResolveStage(0),
                    AssignedCaregiverId = caregiverId ?? string.Empty,
                    ParentIds = parentIds != null ? new List<string>(parentIds) : new List<string>()
                };
                _state.Profiles.Add(profile);
            }

            return profile;
        }

        public void TickDay(int currentDay)
        {
            for (int i = 0; i < _state.Profiles.Count; i++)
            {
                var child = _state.Profiles[i];
                int age = Math.Max(0, currentDay - child.BirthDay);
                var expectedStage = ResolveStage(age);

                if (expectedStage > child.Stage)
                {
                    var oldStage = child.Stage;
                    child.Stage = expectedStage;

                    string milestoneName = $"Milestone_{expectedStage}";
                    child.Milestones.Add(milestoneName);

                    var ev = new DevelopmentMilestoneEvent
                    {
                        EventId = $"dev_{_state.NextSequence++}",
                        ChildId = child.ChildId,
                        NewStage = expectedStage,
                        Day = currentDay,
                        MilestoneName = milestoneName,
                        Description = $"{child.Name} reached developmental stage {expectedStage}."
                    };

                    _state.MilestoneHistory.Add(ev);
                    OnStageChanged?.Invoke(child, expectedStage);
                    OnMilestoneAchieved?.Invoke(ev);
                }

                // Passive education/chore growth if child/adolescent and has caregiver
                if (!string.IsNullOrEmpty(child.AssignedCaregiverId))
                {
                    if (child.Stage == DevelopmentStage.Child)
                    {
                        child.EducationScore = Math.Clamp(child.EducationScore + 0.5f, 0f, 100f);
                        child.ChoreEfficiency = Math.Clamp(child.ChoreEfficiency + 0.3f, 0f, 100f);
                    }
                    else if (child.Stage == DevelopmentStage.Adolescent)
                    {
                        child.EducationScore = Math.Clamp(child.EducationScore + 0.8f, 0f, 100f);
                        child.ChoreEfficiency = Math.Clamp(child.ChoreEfficiency + 0.6f, 0f, 100f);
                    }
                }
            }
        }

        public bool RecordEducation(string childId, float amount = 5f)
        {
            var child = _state.Profiles.FirstOrDefault(p => string.Equals(p.ChildId, childId, StringComparison.OrdinalIgnoreCase));
            if (child == null) return false;

            child.EducationScore = Math.Clamp(child.EducationScore + amount, 0f, 100f);
            return true;
        }

        public bool AssignCaregiver(string childId, string caregiverId)
        {
            var child = _state.Profiles.FirstOrDefault(p => string.Equals(p.ChildId, childId, StringComparison.OrdinalIgnoreCase));
            if (child == null) return false;

            child.AssignedCaregiverId = caregiverId ?? string.Empty;
            return true;
        }

        public float GetChoreWorkCapacity(string childId)
        {
            var child = _state.Profiles.FirstOrDefault(p => string.Equals(p.ChildId, childId, StringComparison.OrdinalIgnoreCase));
            if (child == null) return 0f;

            return child.Stage switch
            {
                DevelopmentStage.Infant => 0.0f,
                DevelopmentStage.Toddler => 0.0f,
                DevelopmentStage.Child => 0.35f + (child.ChoreEfficiency * 0.0015f),
                DevelopmentStage.Adolescent => 0.75f + (child.ChoreEfficiency * 0.0025f),
                DevelopmentStage.YoungAdult => 1.0f,
                _ => 0f
            };
        }

        public ChildProfile? GetChild(string childId)
        {
            return _state.Profiles.FirstOrDefault(p => string.Equals(p.ChildId, childId, StringComparison.OrdinalIgnoreCase));
        }

        public ChildDevelopmentState CaptureState()
        {
            var captured = new ChildDevelopmentState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Profiles = new List<ChildProfile>(_state.Profiles.Count),
                MilestoneHistory = new List<DevelopmentMilestoneEvent>(_state.MilestoneHistory.Count)
            };

            for (int i = 0; i < _state.Profiles.Count; i++)
            {
                var p = _state.Profiles[i];
                captured.Profiles.Add(new ChildProfile
                {
                    ChildId = p.ChildId,
                    Name = p.Name,
                    BirthDay = p.BirthDay,
                    Stage = p.Stage,
                    AssignedCaregiverId = p.AssignedCaregiverId,
                    EducationScore = p.EducationScore,
                    ChoreEfficiency = p.ChoreEfficiency,
                    ParentIds = new List<string>(p.ParentIds),
                    Milestones = new List<string>(p.Milestones)
                });
            }

            for (int i = 0; i < _state.MilestoneHistory.Count; i++)
            {
                var m = _state.MilestoneHistory[i];
                captured.MilestoneHistory.Add(new DevelopmentMilestoneEvent
                {
                    EventId = m.EventId,
                    ChildId = m.ChildId,
                    NewStage = m.NewStage,
                    Day = m.Day,
                    MilestoneName = m.MilestoneName,
                    Description = m.Description
                });
            }

            return captured;
        }

        public void RestoreState(ChildDevelopmentState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Profiles.Clear();
            _state.MilestoneHistory.Clear();

            if (state.Profiles != null)
            {
                for (int i = 0; i < state.Profiles.Count; i++)
                {
                    var p = state.Profiles[i];
                    _state.Profiles.Add(new ChildProfile
                    {
                        ChildId = p.ChildId,
                        Name = p.Name,
                        BirthDay = p.BirthDay,
                        Stage = p.Stage,
                        AssignedCaregiverId = p.AssignedCaregiverId,
                        EducationScore = p.EducationScore,
                        ChoreEfficiency = p.ChoreEfficiency,
                        ParentIds = new List<string>(p.ParentIds ?? Enumerable.Empty<string>()),
                        Milestones = new List<string>(p.Milestones ?? Enumerable.Empty<string>())
                    });
                }
            }

            if (state.MilestoneHistory != null)
            {
                for (int i = 0; i < state.MilestoneHistory.Count; i++)
                {
                    var m = state.MilestoneHistory[i];
                    _state.MilestoneHistory.Add(new DevelopmentMilestoneEvent
                    {
                        EventId = m.EventId,
                        ChildId = m.ChildId,
                        NewStage = m.NewStage,
                        Day = m.Day,
                        MilestoneName = m.MilestoneName,
                        Description = m.Description
                    });
                }
            }
        }

        public ChildDevelopmentCensus GetCensus()
        {
            int infant = 0, toddler = 0, child = 0, adolescent = 0, youngAdult = 0;
            for (int i = 0; i < _state.Profiles.Count; i++)
            {
                switch (_state.Profiles[i].Stage)
                {
                    case DevelopmentStage.Infant: infant++; break;
                    case DevelopmentStage.Toddler: toddler++; break;
                    case DevelopmentStage.Child: child++; break;
                    case DevelopmentStage.Adolescent: adolescent++; break;
                    case DevelopmentStage.YoungAdult: youngAdult++; break;
                }
            }
            return new ChildDevelopmentCensus(_state.Profiles.Count, infant, toddler, child, adolescent, youngAdult, _state.MilestoneHistory.Count);
        }
    }

    public struct ChildDevelopmentCensus
    {
        public readonly int TotalChildren;
        public readonly int InfantCount;
        public readonly int ToddlerCount;
        public readonly int ChildCount;
        public readonly int AdolescentCount;
        public readonly int YoungAdultCount;
        public readonly int TotalMilestones;

        public ChildDevelopmentCensus(int totalChildren, int infantCount, int toddlerCount, int childCount, int adolescentCount, int youngAdultCount, int totalMilestones)
        {
            TotalChildren = totalChildren;
            InfantCount = infantCount;
            ToddlerCount = toddlerCount;
            ChildCount = childCount;
            AdolescentCount = adolescentCount;
            YoungAdultCount = youngAdultCount;
            TotalMilestones = totalMilestones;
        }
    }
}
