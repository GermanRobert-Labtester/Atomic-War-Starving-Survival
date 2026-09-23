// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Legacy;
using Ashfall.Core.Save;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Save store for Plan 140 generational legacy and campaign inheritance.
    /// Checksummed envelope targeting campaign_legacy_save.json.
    /// </summary>
    public static class CampaignLegacySaveStore
    {
        public const string SectionName = "campaign_legacy";
        public const string FileName = "campaign_legacy_save.json";

        private static readonly SaveStore<CampaignLegacyState> s_store =
            SaveStoreHub.Checksummed<CampaignLegacyState>(FileName, SaveSectionRegistry.ExpandedShelterLifecycleGroup);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CampaignLegacyState state) => s_store.TrySave(state);
        public static CampaignLegacyState? TryLoad() => s_store.TryLoad();
        public static string CapturePersisted(CampaignLegacyState state) => s_store.CapturePersisted(state);
        public static string CaptureBare(CampaignLegacyState state) => s_store.CaptureBare(state);
        public static CampaignLegacyState? RestoreBare(string json) => s_store.RestoreBare(json);
    }

    /// <summary>
    /// Host session managing Plan 140 Generational Legacy & Campaign Inheritance.
    /// Bridges CampaignLegacySystem to the Godot application and save orchestrator.
    /// </summary>
    public sealed class CampaignLegacyHostSession : HostSessionBase
    {
        private readonly Main _main;
        private readonly CampaignLegacySystem _system;
        private bool _isDisposed;

        public CampaignLegacyHostSession(Main main, string? baseDirectory = null, ISeededRng? rng = null)
        {
            _main = main ?? throw new ArgumentNullException(nameof(main));
            _system = new CampaignLegacySystem(null, rng);
        }

        public CampaignLegacySystem System => _system;

        public void Initialize(string? dataDir = null)
        {
            string baseDir = string.IsNullOrWhiteSpace(dataDir)
                ? CatalogPath.ResolveDataDir()
                : dataDir;

            string catalogPath = Path.Combine(baseDir, CampaignLegacySystem.DefaultCatalogFileName);
            if (File.Exists(catalogPath))
            {
                try
                {
                    string json = File.ReadAllText(catalogPath);
                    _system.LoadCatalog(json);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[CampaignLegacyHostSession] Failed to load legacy catalog: {ex.Message}");
                }
            }
        }

        public void ArchiveCurrentCampaign(CampaignLegacy legacy, ISeededRng? rng = null)
        {
            _system.ArchiveCampaign(legacy, rng);
        }

        public StartingCampaignContext PrepareNewGameContext()
        {
            return _system.PrepareNewGameContext();
        }

        public CampaignLegacyCensus GetCensus()
        {
            return _system.GetCensus();
        }

        public void TickDay(int day, List<DayStateChangeEvent>? events = null)
        {
            events?.Add(new DayStateChangeEvent(
                kind: "campaign_legacy_ticked",
                sourceOwnerId: "campaign_legacy",
                primaryId: null,
                secondaryId: null,
                numeric: _system.State.activeLegacyTraits.Count));
        }

        public void RestoreFromPayload(string json)
        {
            var state = CampaignLegacySaveStore.RestoreBare(json);
            if (state != null)
            {
                _system.RestoreState(state);
            }
        }

        public bool Save(out string error)
        {
            error = string.Empty;
            try
            {
                var state = _system.CaptureState();
                return CampaignLegacySaveStore.TrySave(state);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool Restore(out string error)
        {
            error = string.Empty;
            try
            {
                var state = CampaignLegacySaveStore.TryLoad();
                if (state != null)
                {
                    _system.RestoreState(state);
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public override void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            base.Dispose();
        }
    }
}
