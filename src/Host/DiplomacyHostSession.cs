// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : DiplomacySaveStore
// Core State : Ashfall.Core.Diplomacy.FactionDiplomacyState
// Host Caller: Main.Diplomacy
// Purpose    : Faction diplomacy host session & persistence. The Core system
//              remains the sole treaty/relation/mission authority. This partial
//              composes the authored treaty catalog and the campaign day tick.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class DiplomacySaveStore
    {
        public const string FileName = "diplomacy_save.json";
        public const string SectionName = "diplomacy";

        private static readonly SaveStore<FactionDiplomacyState> s_store =
            SaveStoreHub.Checksummed<FactionDiplomacyState>(FileName, nameof(DiplomacySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(FactionDiplomacyState state) => s_store.CaptureBare(state);
        public static FactionDiplomacyState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(FactionDiplomacyState state) => s_store.TrySave(state);
        public static FactionDiplomacyState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the Core <see cref="FactionDiplomacySystem"/>.</summary>
    public sealed class DiplomacyHostSession : HostSessionBase
    {
        private readonly FactionDiplomacySystem _system;

        public FactionDiplomacySystem System => _system;
        public bool CatalogReady { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public DiplomacyHostSession(FactionDiplomacyState? state = null)
        {
            _system = new FactionDiplomacySystem();
            if (state != null) _system.RestoreState(state);
        }

        public static DiplomacyHostSession Create(FactionDiplomacyState? state = null) =>
            new DiplomacyHostSession(state);

        public bool LoadCatalog(string dataDirectory)
        {
            if (string.IsNullOrWhiteSpace(dataDirectory)) return false;
            string path = Path.Combine(dataDirectory, "treaty_templates.json");
            if (!File.Exists(path)) return false;
            try
            {
                _system.LoadCatalog(File.ReadAllText(path));
                CatalogReady = _system.GetTemplate("non_aggression") != null
                    || _system.GetTemplate("trade_agreement") != null;
                return CatalogReady;
            }
            catch (Exception)
            {
                CatalogReady = false;
                return false;
            }
        }

        public (bool Success, string Message, ActiveTreatyRecord? Treaty) ProposeTreaty(
            string factionId, string treatyTypeId, int day, int envoySkill = 50)
        {
            var result = _system.ProposeTreaty(factionId, treatyTypeId, day, envoySkill);
            LastEvent = result.Message;
            RaiseStateChanged();
            return result;
        }

        public DiplomaticMissionRecord DispatchMission(
            string missionType, string targetFactionId, string envoyId, int day,
            int durationDays = 3, int envoySkill = 50)
        {
            var mission = _system.DispatchMission(missionType, targetFactionId, envoyId, day, durationDays, envoySkill);
            LastEvent = $"Dispatched {missionType} to {targetFactionId}.";
            RaiseStateChanged();
            return mission;
        }

        public void AssignEnvoy(string factionId, string survivorId)
        {
            _system.AssignEnvoy(factionId, survivorId);
            LastEvent = $"Assigned envoy {survivorId} to {factionId}.";
            RaiseStateChanged();
        }

        public DiplomaticRelationState GetOrCreateRelation(string factionId) => _system.GetOrCreateRelation(factionId);
        public void TickDay(int day) => _system.TickDay(day);

        public int GlobalReputation => _system.GlobalReputation;
        public int ActiveTreatyCount => _system.ActiveTreatyCount;
        public int TotalViolationCount => _system.TotalViolationCount;

        public FactionDiplomacyState CaptureState() => _system.CaptureState();

        public void RestoreState(FactionDiplomacyState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored diplomacy state.";
            RaiseStateChanged();
        }

        public bool TrySave() => DiplomacySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = DiplomacySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
