// SPDX-License-Identifier: MIT
using Godot;
using System;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Canonical overlay-panel catalog. Single authority shared by
        /// CloseAllOverlayPanels and AnyOverlayPanelOpen so global dismissal and
        /// global close-detection can never drift apart again
        /// (UI/UX audit 2026-09-25 — previously two hand-maintained lists, 68
        /// panels invisible to Esc detection).
        /// </summary>
        private Control[] OverlayPanelCatalog()
        {
            return new Control[]
            {
                _settingsPanel,
                _inventoryOverlay,
                _survivorsOverlay,
                _craftingPanel,
                _startingCohortSetupPanel,
                _radioPanel,
                _medicalPanel,
                _dutyRosterPanel,
                _expeditionPanel,
                _weatherPanel,
                _questsPanel,
                _journalPanel,
                _factionsPanel,
                _musterPanel,
                _expansionsHubPanel,
                _standingRecordPanel,
                _maritimePanel,
                _centurySeedPanel,
                _epiloguePanel,
                _chroniclePanel,
                _verdictPanel,
                _researchPanel,
                _shelterPanel,
                _greenhousePanel,
                _combatPanel,
                _mapPanel,
                _silentFoundryPanel,
                _tradePanel,
                _survivorDetailPanel,
                _inventoryDetailPanel,
                _questDetailPanel,
                _moralChoiceModal,
                _narrativeArcModal,
                _achievementsPanel,
                _weatherDetailPanel,
                _radiationDetailPanel,
                _eventsLogPanel,
                _dutyRosterDetailPanel,
                _economyDetailPanel,
                _combatDetailPanel,
                _factionDetailPanel,
                _factionCultureCodexPanel,
                _crossingQuestPanel,
                _saveLoadPanel,
                _tutorialPanel,
                _afflictionsPanel,
                _statusPanel,
                _survivalDetailPanel,
                _weatherForecastPanel,
                _radiationHistoryPanel,
                _journalDetailPanel,
                _combatHistoryPanel,
                _mapDetailPanel,
                _eventDetailPanel,
                _openingProtocolModal,
                _holdfastTerminal,
                _onboardingHintPanel,
                _brineExtractionPanel,
                _expeditionCampPanel,
                _fireIncidentPanel,
                _geigerCalibrationPanel,
                _triangulationPanel,
                _weatherSondePanel,
                _powerGridPanel,
                _expeditionRadarPanel,
                _doseLedgerPanel,
                _doseGeographyPanel,
                _geothermalOrcPanel,
                _ballisticsWorkbenchPanel,
                _aeroponicsPanel,
                _pneumaticDispatchPanel,
                _caravanBarterLedgerPanel,
                _factionMatrixPanel,
                _factionsNarrativePanel,
                _communiqueBoardPanel,
                _skillMatrixPanel,
                _survivalWorkstationPanel,
                _verdictDashboardPanel,
                _mapAtlasPanel,
                _maritimeAtlasPanel,
                _musterAtlasPanel,
                _questsAtlasPanel,
                _researchAtlasPanel,
                _standingRecordAtlasPanel,
                _combatHudOverlay,
                _biogasDigesterPanel,
                _cartographyGisPanel,
                _printingPressPanel,
                _siliconSlicingPanel,
                _geothermalTurbinePanel,
                _warDogKennelPanel,
                _isotopeSeparatorPanel,
                _plasmaSmeltingPanel,
                _boreholeSeismographPanel,
                _logisticsAirlockPanel,
                _cryoPermafrostCorePanel,
                _basalRadonMigrationPanel,
                _traumaBondingCohortPanel,
                _clandestineInsurgencyPanel,
                _subterraneanDebtLedgerPanel,
                _surfaceShrapnelAegisPanel,
                _longWalkExpeditionPanel,
                _sonicRuptureDrillPanel,
                _vaultDoorBreachingPanel,
                _ironCenotaphMemorialPanel,
                _aquiferTreatyConcessionPanel,
                _crossingSafeConductVouchPanel,
                _mechanicalProstheticsLathePanel,
                _fungalProteinFermenterPanel,
                _ultrasonicDecontamAirlockPanel,
                _troposphericRadioRelayPanel,
                _inductionCupolaFurnacePanel,
                _heavyMarineDieselGenPanel,
                _slurryDewateringSumpPanel,
                _magneticDrumArchivePanel,
                _plans130To133Panel,
                _blackProjectsArchivePanel,
                _chemWarfareDefensePanel,
                _commsArrayTransceiverPanel,
                _ceremonyFestivalPanel,
                _roboticsWorkshopPanel,
                _survivorDowntimePanel,
                _winterFreezePanel,
                _amputationTriagePanel,
                _justiceTribunalPanel,
                _railwayTerminalPanel,
                _archaeologyExcavationPanel,
                _desperationCrisisPanel,
                _mercenaryBountyBoardPanel,
                _falloutPlumePanel,
                _dailyBriefingModal,
                _romanceFamilyBoard,
                _colonyOperationsBoard,
                _ideologicalMediationDesk,
                // ── Expanded shelter panels (WHOLEGAME-P1B) — previously bypassed
                // the catalog, so Esc fell through to ReturnToMenu with them open.
                _waterTreatmentPanel,
                _airlockSecurityPanel,
                _survivorRelationsPanel,
                _regionalTreatyPanel,
                _vinylMoralePanel,
                _lowBackgroundPanel,
                _inSarPanel,
                _hydraulicExtrusionPanel,
                _runFlatTirePanel,
                _wildlifeTrappingPanel,
                _excavationPanel,
                _apprenticeshipPanel,
                _caregivingPanel,
                _shelterThermalPanel,
                _shelterSchedulePanel,
                _autopsyReportPanel,
                _waystationPanel,
                _chemicalDependencyPanel,
                _sumpFloodingPanel,
                _decontaminationPanel,
                _kitchenNutritionPanel,
                _equipmentConditionPanel,
                _libraryStudyPanel,
                _archiveDeskPanel,
                _contractorRosterPanel,
                _mentalHealthCrisisPanel,
                _phantomMemoryPanel,
                _travelingCaravanPanel,
                _shelterDecorPanel,
                _medicalWardPanel,
                _plans94To97Panel,
                // ── Additional overlay panels not previously catalogued ──
                _aviationPanel,
                _bestiaryPanel,
                _bioFermentationPanel,
                _cargoAirdropPanel,
                _chemicalReconPanel,
                _chemPanel,
                _deconAirlockPanel,
                _deepCoastPanel,
                _defenseGridPanel,
                _economyPanel,
                _electrostaticScrubberPanel,
                _farmingPanel,
                _fungiCultivationBedPanel,
                _geodeticSurveyPanel,
                _geothermalAquiferPanel,
                _inventoryPanel,
                _kineticStoragePanel,
                _laborPanel,
                _mutationTreePanel,
                _nurseryPanel,
                _pharmaLabPanel,
                _phase0Panel,
                _plasticPyrolysisPanel,
                _politicsPanel,
                _prisonerPanel,
                _psychologyArcPanel,
                _radioIntelligencePanel,
                _reconTelemetryPanel,
                _shelterSocialPanel,
                _stealthReadoutPanel,
                _subterraneanOperationsPanel,
                _utilityAiPanel,
                _weatherHistoryPanel,
                _workshopPanel,
            };
        }

        /// <summary>
        /// Opens a panel with full lifecycle: visibility, animation, and focus.
        /// Use this instead of bare <c>panel.Visible = true</c> for all panel opens.
        /// For panels whose VisibilityChanged hook was registered by
        /// RegisterOpenMotionRecursive, the animation and focus fire automatically.
        /// For lazy panels added after registration, this method provides the fallback.
        /// </summary>
        private void ShowPanelLifecycle(Control panel)
        {
            if (panel == null || !GodotObject.IsInstanceValid(panel)) return;
            panel.Visible = true;
            AtomicWar.GodotApp.UI.UiMotion.AnimateOpen(panel);
            EnsureInitialFocus(panel);
        }

        private void CloseAllOverlayPanels()
        {
            foreach (Control panel in OverlayPanelCatalog())
            {
                if (panel == null || !panel.Visible || AtomicWar.GodotApp.UI.UiMotion.IsClosing(panel))
                    continue;

                AtomicWar.GodotApp.UI.AshfallFocusPolicy.RestoreFocusFromRoot(panel);
                // Central exit transition (UI/UX audit 2026-09-25): every global
                // dismissal and panel switch fades the old shell out. Falls back
                // to an immediate hide when motion is unavailable (headless,
                // captures, ReducedMotion).
                if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(panel))
                    panel.Visible = false;
            }

            if (_journalBook != null && _journalBook.IsOpen)
            {
                AtomicWar.GodotApp.UI.AshfallFocusPolicy.RestoreFocusFromRoot(_journalBook);
                _journalBook.Close();
            }
        }

        private void CloseSettingsPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_settingsPanel))
                _settingsPanel.Visible = false;
        }

        private void CloseQuestsPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_questsPanel))
                _questsPanel.Visible = false;
        }

        private void CloseFactionsPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_factionsPanel))
                _factionsPanel.Visible = false;
        }

        private void CloseResearchPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_researchPanel))
                _researchPanel.Visible = false;
        }

        private void CloseShelterPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_shelterPanel))
                _shelterPanel.Visible = false;
        }

        private void CloseQuestDetailPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_questDetailPanel))
                _questDetailPanel.Visible = false;
        }

        private void CloseMoralChoiceModal()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_moralChoiceModal))
                _moralChoiceModal.Visible = false;
        }

        private void CloseNarrativeArcModal()
        {
            if (_narrativeArcModal != null) _narrativeArcModal.Visible = false;
        }

        private void CloseFactionDetailPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_factionDetailPanel))
                _factionDetailPanel.Visible = false;
        }

        private void CloseFactionCultureCodexPanel()
        {
            if (_factionCultureCodexPanel != null && !AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_factionCultureCodexPanel))
                _factionCultureCodexPanel.Visible = false;
        }

        private void CloseCrossingQuestPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_crossingQuestPanel))
                _crossingQuestPanel.Visible = false;
        }

        private void CloseAchievementsPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_achievementsPanel))
                _achievementsPanel.Visible = false;
        }

        private void CloseRadiationDetailPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_radiationDetailPanel))
                _radiationDetailPanel.Visible = false;
        }

        private void CloseEventsLogPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_eventsLogPanel))
                _eventsLogPanel.Visible = false;
        }

        private void CloseSaveLoadPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_saveLoadPanel))
                _saveLoadPanel.Visible = false;
        }

        private void CloseTutorialPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_tutorialPanel))
                _tutorialPanel.Visible = false;
        }

        private void CloseAfflictionsPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_afflictionsPanel))
                _afflictionsPanel.Visible = false;
        }

        private void CloseStatusPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_statusPanel))
                _statusPanel.Visible = false;
        }

        private void CloseSurvivalDetailPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_survivalDetailPanel))
                _survivalDetailPanel.Visible = false;
        }

        private void CloseRadiationHistoryPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_radiationHistoryPanel))
                _radiationHistoryPanel.Visible = false;
        }

        private void CloseEventDetailPanel()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_eventDetailPanel))
                _eventDetailPanel.Visible = false;
        }
    }
}
