// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Excavation;
using Ashfall.Core.IO;
using Ashfall.Core.Quests;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterSocialDynamicsSystem? _shelterSocialDynamics;
        private bool _shelterSocialDirty;

        // ── Plan 48: Shelter Social Dynamics ────────────────────────────

        public ShelterSocialDynamicsSystem EnsureShelterSocialDynamics()
        {
            if (_shelterSocialDynamics != null)
            {
                // The panel route can call Ensure after the session was first
                // composed. Re-bind the communications seam without rebuilding
                // the social authority or creating a second session.
                SetupInternalCommunication();
                BindInternalCommunicationPanel();
                return _shelterSocialDynamics;
            }

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_social") : new SeededRng(48);
            var relations = _survivorRelations?.System;
            var needs = _survivors?.Needs;
            var memorial = _memorial;

            _shelterSocialDynamics = new ShelterSocialDynamicsSystem(rng, relations, needs, memorial, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("shelter_social_events.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _shelterSocialDynamics.LoadCatalog(json);
                }
            }

            var saved = ShelterSocialSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterSocialDynamics.RestoreState(saved);
            }

            _shelterSocialDynamics.OnIncidentTriggered += inc =>
            {
                _journal?.TryAddRawEntry("shelter_social_incident", $"Social incident occurred in {inc.RoomId} (event: {inc.EventId})", null!, _simDay);
            };

            _shelterSocialDynamics.OnSocialStateChanged += () => _shelterSocialDirty = true;
            SetupInternalCommunication();
            BindInternalCommunicationPanel();
            return _shelterSocialDynamics;
        }

        private void SetupShelterSocial()
        {
            EnsureShelterSocialDynamics();
            SetupInternalCommunication();
            BindInternalCommunicationPanel();
        }

        private void SaveShelterSocial()
        {
            if (_shelterSocialDynamics != null)
            {
                CaptureSection("shelter_social_dynamics", ShelterSocialSaveStore.TryCapturePersisted(_shelterSocialDynamics.CaptureState()));
                _shelterSocialDirty = false;
            }

            // Plan 211 has a distinct authority/section; capture it from the
            // same existing shelter-social save composition seam without
            // teaching the social Core system about communications.
            SaveInternalCommunication();
        }

    }
}
