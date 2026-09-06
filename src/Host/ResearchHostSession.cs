using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Research / R&D host session. Thin Godot wrapper
    /// around the engine-agnostic <see cref="ResearchSystem"/>.
    /// Captures / restores the unified <see cref="ResearchState"/>
    /// envelope. No gameplay rules here — hosts only present the
    /// engine's read surface to the dashboard.
    /// </summary>
    public sealed class ResearchHostSession
    : HostSessionBase{
        public ResearchSystem Engine { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>
        /// Create a research host session bound to <paramref name="engine"/> (or a
        /// fresh one) and load the authoritative research_knowledge.json catalog
        /// (Plan 34: JSON is the sole authored research authority — no hardcoded
        /// fallback; an empty load is surfaced as a diagnostic).
        /// </summary>
        public static ResearchHostSession Create(string dataDir, ResearchSystem? engine = null)
        {
            return new ResearchHostSession(dataDir, engine);
        }

        private ResearchHostSession(string? dataDir, ResearchSystem? engine)
        {
            Engine = engine ?? new ResearchSystem(log: new NullLog());
            Engine.OnResearchCompleted += def =>
            {
                LastEvent = $"Breakthrough achieved: {def.displayName} ({def.breakthroughItem ?? "none"})";
                RaiseStateChanged();
            };
            if (!string.IsNullOrEmpty(dataDir))
                LoadCatalog(dataDir);
        }

        /// <summary>Load the research_knowledge.json catalog into the Core system (the authority).</summary>
        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var fileIO = new Ashfall.Core.FileSystemIO();
            var serializer = new Ashfall.Core.SystemTextJsonSerializer();
            int count = ResearchKnowledgeCatalogLoader.LoadAndRegister(Engine, dataDir, fileIO, serializer);
            if (count > 0)
            {
                LastEvent = $"Research catalog loaded: {count} nodes";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = "Research catalog MISSING or empty — research content unavailable";
                Godot.GD.PrintErr($"[Research] research_knowledge.json missing/empty under {dataDir} — no fallback is provided (Plan 34)");
            }
        }

        public bool IsUnlocked => Engine.State.expansionUnlocked;
        public int CurrentDay => Engine.State.currentDay;
        public int CatalogCount => Engine.CatalogCount;
        public int CompletedCount => Engine.State.completedIds.Count;
        public int UnlockedCount => Engine.State.unlockedIds.Count;
        public string ActiveResearchId => Engine.State.activeResearchId ?? string.Empty;
        public int ActiveResearchDays => Engine.State.activeResearchDays;
        public IReadOnlyDictionary<string, ResearchKnowledgeDef> Catalog => Engine.Catalog;

        public ResearchKnowledgeDef? GetActiveResearch() => Engine.GetActiveResearch();

        public void Unlock(int day)
        {
            if (Engine.State.expansionUnlocked) return;
            Engine.State.expansionUnlocked = true;
            Engine.State.currentDay = day;
            LastEvent = "Research unlocked @ day " + day;
            RaiseStateChanged();
        }

        public bool StartResearch(string id, int day) => TryStart(id, day, out _);

        public bool TryStart(string id) => TryStart(id, CurrentDay, out _);

        public bool TryStart(string id, int day, out ResearchEligibilityCode code)
        {
            var eligibility = Engine.GetEligibility(id);
            code = eligibility.Code;
            if (!eligibility.CanStart)
            {
                LastEvent = $"Cannot start {id}: {FormatFailureCode(eligibility)}";
                RaiseStateChanged();
                return false;
            }

            bool ok = Engine.StartResearch(id, day);
            if (ok)
            {
                LastEvent = $"Research started: {id} @ day {day}";
                RaiseStateChanged();
            }
            return ok;
        }

        public string FormatFailureCode(ResearchEligibility eligibility)
        {
            return eligibility.Code switch
            {
                ResearchEligibilityCode.UnknownNode => "Unknown research node",
                ResearchEligibilityCode.AlreadyCompleted => "Already completed",
                ResearchEligibilityCode.AlreadyActive => "Currently in progress",
                ResearchEligibilityCode.AnotherResearchActive => $"Another research active ({eligibility.ConflictingActiveResearchId})",
                ResearchEligibilityCode.MissingPrerequisites => $"Missing prerequisites ({string.Join(", ", eligibility.MissingPrerequisites)})",
                ResearchEligibilityCode.NotDiscovered => "Not yet discovered in research tree",
                _ => "Eligible"
            };
        }

        public IReadOnlyList<ResearchKnowledgeDef> AvailableNodes() => Engine.GetAvailableNodes();
        public IReadOnlyList<ResearchKnowledgeDef> LockedNodes() => Engine.GetLockedNodes();
        public IReadOnlyList<ResearchKnowledgeDef> CompletedNodes() => Engine.GetCompletedNodes();
        public int DaysRemaining(string id) => Engine.GetDaysRemaining(id);
        public ResearchEligibility GetEligibility(string id) => Engine.GetEligibility(id);
        public IReadOnlyList<string> GetDependents(string id) => Engine.GetDependents(id);

        public void AdvanceDay(int day)
        {
            Engine.Tick(day);
            LastEvent = "Tick @ day " + day;
            RaiseStateChanged();
        }

        public bool CompleteResearch(string id)
        {
            bool ok = Engine.CompleteResearch(id);
            if (ok)
            {
                LastEvent = $"Research completed: {id}";
                RaiseStateChanged();
            }
            return ok;
        }

        public ResearchSave CaptureSave()
        {
            return new ResearchSave
            {
                systemId = ResearchSystem.SystemId,
                state = Engine.CaptureState(),
            };
        }

        public void RestoreSave(ResearchSave save)
        {
            if (save == null || save.state == null) return;
            Engine.RestoreState(save.state);
            LastEvent = "Research state restored.";
            RaiseStateChanged();
        }
    }

    [Serializable]
    public sealed class ResearchSave
    {
        public string systemId = ResearchSystem.SystemId;
        public ResearchState state = new ResearchState();
    }
}
