using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Unified Standing Record (Expansion 03) state envelope. Wraps the
    /// per-system states so the save codec carries one envelope instead
    /// of three. Engine-agnostic.
    /// </summary>
    [Serializable]
    public sealed class StandingRecordState
    {
        public string systemId = StandingRecordEngine.SystemId;
        public bool expansionUnlocked;
        public int currentDay;
        public bool overlayAccess = true;
        public LocationLayoutState layout = new LocationLayoutState();
        public LocationMemoryState memory = new LocationMemoryState();
        public SiteEncounterState encounters = new SiteEncounterState();
    }

    /// <summary>
    /// Standing Record (Expansion 03) engine. Coordinates the existing
    /// read-only catalog systems — LocationLayout, LocationMemory, and
    /// SiteEncounter — adding a unified tick + expedition hook +
    /// CaptureState / RestoreState. Engine-agnostic; mirrors the Phase-18
    /// Skill Progression port shape.
    /// </summary>
    public sealed class StandingRecordEngine
    {
        public const string SystemId = "standing_record_system";
        public const string FlagExpUnlocked = "exp_standing_record_unlocked";

        public StandingRecordState State { get; private set; }

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public LocationLayoutSystem Layouts { get; }
        public LocationMemorySystem Memory { get; }
        public SiteEncounterSystem Encounters { get; }

        public StandingRecordEngine(
            IFileIO files, IJsonSerializer json,
            ISeededRng rng, ILog log = null,
            StandingRecordState state = null)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            _files = files;
            _json = json;
            _rng = rng;
            _log = log ?? NullLog.Instance;
            State = state ?? new StandingRecordState();
            Layouts = new LocationLayoutSystem(_files, _json, _log);
            Memory = new LocationMemorySystem(_files, _json, _log);
            Encounters = new SiteEncounterSystem();
            Layouts.RestoreState(State.layout);
            Memory.RestoreState(State.memory);
            Encounters.RestoreState(State.encounters);
            State.overlayAccess = Encounters.OverlayAccess;
        }

        public void Load(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir))
            {
                _log.Warn("[StandingRecord] Load called with empty dataDir — engine will run on catalog-less state");
            }
            Layouts.Load(dataDir);
            Memory.Load(dataDir);
            // SiteEncounterSystem has no catalog file — it operates on
            // room-keyed encounter records emitted by expedition entry.
        }

        public bool IsUnlocked => State.expansionUnlocked;
        public int CurrentDay => State.currentDay;
        public bool HasOverlayAccess => State.overlayAccess;

        public void UnlockExpansion(int currentDay)
        {
            if (State.expansionUnlocked) return;
            State.expansionUnlocked = true;
            State.currentDay = currentDay;
            Layouts.Unlock();
            Memory.Unlock();
            Encounters.Unlock();
            Memory.ApplyMutation(FlagExpUnlocked);
            _log.Info("[StandingRecord] unlocked @ day " + currentDay);
        }

        /// <summary>
        /// Day-step hook. Drives overlay-access bookkeeping through
        /// SiteEncounterSystem.ScrapePlate if the engine has been asked
        /// to lose overlay access on a particular day; otherwise mirrors
        /// day progress so the facing dashboard sees a current day.
        /// </summary>
        public void Tick(int newDay)
        {
            if (!State.expansionUnlocked) return;
            State.currentDay = newDay;
            State.overlayAccess = Encounters.OverlayAccess;
        }

        /// <summary>
        /// Expedition-based mutation surfaces a site-level flag. Triggers
        /// a memory stratum swap for the named site.
        /// </summary>
        public bool ApplySiteMutation(string siteId, string mutation)
        {
            if (!State.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(mutation)) return false;
            Memory.ApplyMutation(mutation);
            Layouts.MutateLayout(siteId, mutation);
            return true;
        }

        /// <summary>
        /// Read-only passthrough: return the active "now"-text stratum for
        /// a site id, or null if no `'after'` stratum is selected.
        /// </summary>
        public string? GetActiveRecast(string siteId)
        {
            if (Memory == null) return null;
            return Memory.GetActiveRecast(siteId);
        }

        /// <summary>
        /// Capture full engine state — engine-agnostic; serializes through
        /// the host IJsonSerializer when the save codec fires.
        /// </summary>
        public StandingRecordState CaptureState()
        {
            State.layout = Layouts.CaptureState();
            State.memory = Memory.CaptureState();
            State.encounters = Encounters.CaptureState();
            State.overlayAccess = Encounters.OverlayAccess;
            return State;
        }

        /// <summary>
        /// Restore from a previously captured state. SiteEncounterSystem /
        /// MemorySystem / LayoutSystem each receive their slice.
        /// </summary>
        public void RestoreState(StandingRecordState saved)
        {
            if (saved == null) return;
            State = saved;
            Layouts.RestoreState(State.layout);
            Memory.RestoreState(State.memory);
            Encounters.RestoreState(State.encounters);
            State.overlayAccess = Encounters.OverlayAccess;
        }
    }
}
