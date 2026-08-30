using Godot;
using System;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private void CloseAllOverlayPanels()
        {
            Control[] panels =
            {
                _settingsPanel, _inventoryOverlay, _survivorsOverlay, _craftingPanel,
                _radioPanel, _medicalPanel, _dutyRosterPanel,
                _expeditionPanel, _weatherPanel, _questsPanel, _journalPanel,
                _factionsPanel, _musterPanel, _expansionsHubPanel, _standingRecordPanel,
                _maritimePanel, _centurySeedPanel, _epiloguePanel, _verdictPanel,
                _researchPanel, _shelterPanel, _greenhousePanel, _combatPanel, _mapPanel,
                _silentFoundryPanel,
                _tradePanel,
                _survivorDetailPanel, _inventoryDetailPanel, _questDetailPanel,
                _achievementsPanel, _weatherDetailPanel, _radiationDetailPanel,
                _eventsLogPanel, _dutyRosterDetailPanel, _economyDetailPanel,
                _combatDetailPanel, _factionDetailPanel, _crossingQuestPanel, _saveLoadPanel, _tutorialPanel, _afflictionsPanel,
                _statusPanel, _survivalDetailPanel, _weatherForecastPanel,
                _radiationHistoryPanel, _journalDetailPanel, _combatHistoryPanel,
                _mapDetailPanel, _eventDetailPanel, _openingProtocolModal, _holdfastTerminal,
                _onboardingHintPanel,
                _brineExtractionPanel, _expeditionCampPanel, _fireIncidentPanel,
                _geigerCalibrationPanel, _triangulationPanel, _weatherSondePanel,
                _powerGridPanel, _expeditionRadarPanel, _doseLedgerPanel,
                _caravanBarterLedgerPanel, _factionMatrixPanel, _factionsNarrativePanel,
                _skillMatrixPanel, _survivalWorkstationPanel, _verdictDashboardPanel,
                _mapAtlasPanel, _maritimeAtlasPanel, _musterAtlasPanel,
                _questsAtlasPanel, _researchAtlasPanel, _standingRecordAtlasPanel,
                _combatHudOverlay, _biogasDigesterPanel, _cartographyGisPanel,
                _printingPressPanel, _siliconSlicingPanel, _geothermalTurbinePanel,
                _warDogKennelPanel, _isotopeSeparatorPanel, _plasmaSmeltingPanel,
                _boreholeSeismographPanel, _logisticsAirlockPanel,
                _cryoPermafrostCorePanel, _basalRadonMigrationPanel,
                _traumaBondingCohortPanel, _clandestineInsurgencyPanel,
                _subterraneanDebtLedgerPanel, _surfaceShrapnelAegisPanel,
                _longWalkExpeditionPanel, _sonicRuptureDrillPanel,
                _vaultDoorBreachingPanel, _ironCenotaphMemorialPanel,
                _aquiferTreatyConcessionPanel, _crossingSafeConductVouchPanel,
                _mechanicalProstheticsLathePanel, _fungalProteinFermenterPanel,
                _ultrasonicDecontamAirlockPanel,
                _troposphericRadioRelayPanel, _inductionCupolaFurnacePanel,
                _heavyMarineDieselGenPanel, _slurryDewateringSumpPanel,
                _magneticDrumArchivePanel
            };

            foreach (Control panel in panels)
            {
                if (panel != null)
                    panel.Visible = false;
            }

            if (_journalBook != null && _journalBook.IsOpen)
                _journalBook.Close();
        }

        private void CloseSettingsPanel()
        {
            _settingsPanel.Visible = false;
        }

        private void CloseQuestsPanel()
        {
            _questsPanel.Visible = false;
        }

        private void CloseFactionsPanel()
        {
            _factionsPanel.Visible = false;
        }

        private void CloseResearchPanel()
        {
            _researchPanel.Visible = false;
        }

        private void CloseShelterPanel()
        {
            _shelterPanel.Visible = false;
        }

        private void CloseQuestDetailPanel()
        {
            _questDetailPanel.Visible = false;
        }

        private void CloseFactionDetailPanel()
        {
            _factionDetailPanel.Visible = false;
        }

        private void CloseCrossingQuestPanel()
        {
            _crossingQuestPanel.Visible = false;
        }

        private void CloseAchievementsPanel()
        {
            _achievementsPanel.Visible = false;
        }

        private void CloseRadiationDetailPanel()
        {
            _radiationDetailPanel.Visible = false;
        }

        private void CloseEventsLogPanel()
        {
            _eventsLogPanel.Visible = false;
        }

        private void CloseSaveLoadPanel()
        {
            _saveLoadPanel.Visible = false;
        }

        private void CloseTutorialPanel()
        {
            _tutorialPanel.Visible = false;
        }

        private void CloseAfflictionsPanel()
        {
            _afflictionsPanel.Visible = false;
        }

        private void CloseStatusPanel()
        {
            _statusPanel.Visible = false;
        }

        private void CloseSurvivalDetailPanel()
        {
            _survivalDetailPanel.Visible = false;
        }

        private void CloseRadiationHistoryPanel()
        {
            _radiationHistoryPanel.Visible = false;
        }

        private void CloseEventDetailPanel()
        {
            _eventDetailPanel.Visible = false;
        }
    }
}
