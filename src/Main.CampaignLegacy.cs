// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Legacy;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CampaignLegacyHostSession? _campaignLegacy;
        private bool _campaignLegacyDirty;

        public CampaignLegacyHostSession EnsureCampaignLegacy()
        {
            if (_campaignLegacy == null)
            {
                var legacyRng = _campaignDay?.Rng?.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter)?.Rng;
                _campaignLegacy = new CampaignLegacyHostSession(this, null, legacyRng);
                _campaignLegacy.Initialize(_dataDir);
            }
            return _campaignLegacy;
        }

        public void SetupCampaignLegacy()
        {
            var session = EnsureCampaignLegacy();
            if (session == null) return;

            if (_saveLoadHost != null
                && _saveLoadHost.TryGetSectionPayload(CampaignLegacySaveStore.SectionName, out string payload))
            {
                session.RestoreFromPayload(payload);
                _campaignLegacyDirty = false;
            }
            else
            {
                RestoreCampaignLegacy();
            }
        }

        public void SaveCampaignLegacy()
        {
            if (_campaignLegacy == null) return;
            try
            {
                string payload = CampaignLegacySaveStore.CapturePersisted(_campaignLegacy.System.CaptureState());
                CaptureSection(CampaignLegacySaveStore.SectionName, payload);
                _campaignLegacyDirty = false;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Main.CampaignLegacy] Save failed: {ex.Message}");
            }
        }

        public void RestoreCampaignLegacy()
        {
            if (_campaignLegacy == null) return;
            try
            {
                _campaignLegacy.Restore(out _);
                _campaignLegacyDirty = false;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Main.CampaignLegacy] Restore failed: {ex.Message}");
            }
        }

        public void FlushCampaignLegacyIfDirty()
        {
            if (!_campaignLegacyDirty || _campaignLegacy == null) return;
            SaveCampaignLegacy();
        }

        public void ResetCampaignLegacy()
        {
            _campaignLegacy?.Dispose();
            _campaignLegacy = null;
            _campaignLegacyDirty = false;
        }

        public void TickCampaignLegacy(int day)
        {
            _campaignLegacy?.TickDay(day);
        }

        public void ArchiveCurrentCampaign(CampaignLegacy legacy)
        {
            var legacyRng = _campaignDay?.Rng?.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter)?.Rng;
            EnsureCampaignLegacy().ArchiveCurrentCampaign(legacy, legacyRng);
            _campaignLegacyDirty = true;
        }

        public StartingCampaignContext PrepareStartingCampaignContext()
        {
            return EnsureCampaignLegacy().PrepareNewGameContext();
        }
    }
}
