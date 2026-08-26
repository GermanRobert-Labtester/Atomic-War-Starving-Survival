using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE STANDING RECORD — thin Godot-host session.
    /// Wraps the unified engine-agnostic StandingRecordEngine around the
    /// three existing catalog systems (LocationLayout, LocationMemory,
    /// SiteEncounter). Captures / restores the unified envelope via
    /// StandingRecordSaveStore. No gameplay rules here — hosts only
    /// present the engine's read surface to the dashboard.
    /// Spec: docs/expansions/expansion_03_the_standing_record_plan.md.
    /// </summary>
    public sealed class StandingRecordHostSession
    : HostSessionBase{
        public const int DefaultSeed = 1401; // catalog seed offset for SR rooms/mutations.

        public StandingRecordEngine Engine { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public string DataDir { get; }
        public static StandingRecordHostSession Create(string dataDir)
        {
            return new StandingRecordHostSession(dataDir, seed: DefaultSeed);
        }

        public static StandingRecordHostSession Create(string dataDir, int seed)
        {
            return new StandingRecordHostSession(dataDir, seed);
        }

        private StandingRecordHostSession(string? dataDir, int seed)
        {
            DataDir = dataDir ?? string.Empty;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var hostLog = new ConsoleLog();
            var rng = new SeededRng(seed);
            Engine = new StandingRecordEngine(
                files: fileIO,
                json: serializer,
                rng: rng,
                log: hostLog);

            try
            {
                Engine.Load(DataDir);
            }
            catch (Exception ex)
            {
                hostLog.Error("[StandingRecord] load failed: " + ex.Message);
            }

            if (Engine.State.expansionUnlocked)
            {
                RaiseStateChanged();
            }
        }

        public bool IsUnlocked => Engine != null && Engine.IsUnlocked;
        public int CurrentDay => Engine != null ? Engine.CurrentDay : 0;
        public bool HasOverlayAccess =>
            Engine != null && Engine.HasOverlayAccess;
        public int LayoutCount => Engine != null ? Engine.Layouts.LayoutCount : 0;
        public int StratumCount => Engine != null ? Engine.Memory.StratumCount : 0;
        public IReadOnlyList<LocationLayoutDef> Layouts =>
            Engine != null ? Engine.Layouts.Layouts : new List<LocationLayoutDef>();
        public IReadOnlyList<LocationMemoryStratum> AllStrata =>
            Engine != null && Engine.State?.memory?.strata != null
                ? Engine.State.memory.strata
                : new List<LocationMemoryStratum>();

        public void Unlock(int day)
        {
            if (Engine == null) return;
            Engine.UnlockExpansion(day);
            LastEvent = "Standing Record unlocked @ day " + day;
            RaiseStateChanged();
        }

        public void AdvanceDay(int day)
        {
            if (Engine == null) return;
            Engine.Tick(day);
            LastEvent = "Tick @ day " + day;
            RaiseStateChanged();
        }

        public bool ApplyMutation(string siteId, string mutation)
        {
            if (Engine == null) return false;
            bool applied = Engine.ApplySiteMutation(siteId, mutation);
            if (applied)
            {
                LastEvent = "Mutation applied: " + mutation + " @ " + siteId;
                RaiseStateChanged();
            }
            return applied;
        }

        public string GetActiveRecast(string siteId)
        {
            return Engine == null ? string.Empty : Engine.GetActiveRecast(siteId) ?? string.Empty;
        }

        public StandingRecordSave CaptureSave()
        {
            var state = Engine != null
                ? Engine.CaptureState()
                : new StandingRecordState();
            return new StandingRecordSave
            {
                systemId = StandingRecordEngine.SystemId,
                state = state,
            };
        }

        public void RestoreSave(StandingRecordSave save)
        {
            if (save == null || save.state == null) return;
            Engine.RestoreState(save.state);
            LastEvent = "Standing Record restored";
            RaiseStateChanged();
        }

        private void RaiseStateChanged()
        {
            try { RaiseStateChanged(); }
            catch (Exception ex) { GD.PrintErr($"[StandingRecord] StateChanged event failed: {ex.Message}"); }
        }
    }

    /// <summary>
    /// Save DTO for the unified Standing Record envelope.
    /// Mirrors the unified-state shape required by StandingRecordEngine.
    /// </summary>
    [Serializable]
    public sealed class StandingRecordSave
    {
        public string systemId = StandingRecordEngine.SystemId;
        public StandingRecordState state = new StandingRecordState();
    }
}
