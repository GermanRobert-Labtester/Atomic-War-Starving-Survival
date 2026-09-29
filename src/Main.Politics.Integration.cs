// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PoliticsSystem? _politics;

        // ── Plan 185: Elections, Leadership & Settlement Politics ─────────

        public PoliticsSystem EnsurePolitics()
        {
            if (_politics != null) return _politics;

            _politics = new PoliticsSystem();

            string catalogPath = CatalogPath.ResolveCatalog("political_policies.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _politics.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = PoliticsSaveStore.TryLoad();
            if (saved != null)
            {
                _politics.RestoreState(saved);
            }

            // Bind leadership designation to LeadershipSystem
            _politics.OnLeaderDesignated += (newLeaderId) =>
            {
                if (_survivorSocial != null)
                {
                    _survivorSocial.DesignateLeader(newLeaderId);
                }
            };

            _politics.OnCoupTriggered += (msg) =>
            {
                _journal?.TryAddRawEntry("political_coup", $"CRISIS: Armed coup d'etat in progress! {msg}", null!, _simDay);
            };

            return _politics;
        }

        private void SetupPolitics()
        {
            EnsurePolitics();
        }

        private void SavePolitics()
        {
            if (_politics != null)
            {
                CaptureSection("settlement_politics", PoliticsSaveStore.TryCapturePersisted(_politics.CaptureState()));
            }
        }

    }
}
