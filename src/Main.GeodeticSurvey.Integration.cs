// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private GeodeticSurveyHostSession? _geodeticSurvey;

        // ─── Setup ───

        private void SetupGeodeticSurvey()
        {
            if (_geodeticSurvey != null) return;
            SetupCampaignDay();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = GeodeticSurveyCatalogLoader.Load(_dataDir, fileIO, json);

            var gsState = GeodeticSurveySaveStore.TryLoad() ?? new GeodeticSurveyState();
            var gsSys = new GeodeticSurveyEngine(
                catalog,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, 0, 15),
                new GodotLog());
            gsSys.RestoreState(gsState);
            _geodeticSurvey = new GeodeticSurveyHostSession(gsSys);
        }

        // ─── Save (triad) ───

        private void SaveGeodeticSurvey()
        {
            if (_geodeticSurvey != null)
                CaptureSection("geodetic_survey", GeodeticSurveySaveStore.TryCapturePersisted(_geodeticSurvey.System.CaptureState()));
        }

    }
}
