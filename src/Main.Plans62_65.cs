// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Research;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PrewarArchiveDecryptionSystem? _archiveDecryption62;
        private ShelterPrisonerSystem? _shelterPrisoner63;
        private FoodPreservationSystem? _foodPreservation64;
        private CampaignEpilogueEngine? _epilogueEngine65;

        private bool _archiveDecryption62Dirty;
        private bool _shelterPrisoner63Dirty;
        private bool _foodPreservation64Dirty;

        public PrewarArchiveDecryptionSystem? ArchiveDecryptionSystem => _archiveDecryption62;
        public ShelterPrisonerSystem? ShelterPrisonerSystem => _shelterPrisoner63;
        public FoodPreservationSystem? FoodPreservationSystem => _foodPreservation64;
        public CampaignEpilogueEngine? CampaignEpilogueEngine => _epilogueEngine65;

        /// <summary>
        /// Composes Plans 62–65 late-midgame systems:
        /// 1. Plan 62: Deep-Strata Pre-war Archive Decryption
        /// 2. Plan 63: Raider Captives & Penal Labor
        /// 3. Plan 64: Food Spoilage & Cryogenic Preservation
        /// 4. Plan 65: Grand Epilogue Simulator
        /// </summary>
        private void SetupPlans62To65()
        {
            SetupCampaignDay();
            SetupSurvivors();
            SetupInventory();
            SetupPowerGrid();

            if (_inventory == null) return;

            // 1. Food Preservation (Plan 64)
            if (_foodPreservation64 == null)
            {
                var foodCatalog = FoodPreservationCatalogLoader.Load(_dataDir, new FileSystemIO());
                var rng = _campaignDay != null ? _campaignDay.Rng.Fork("food_preservation") : new SeededRng(64);
                _foodPreservation64 = new FoodPreservationSystem(rng, _inventory.Inventory, foodCatalog, new GodotLog());

                var savedFood = FoodPreservationSaveStore.TryLoad();
                if (savedFood != null)
                {
                    _foodPreservation64.RestoreState(savedFood);
                }

                _foodPreservation64.OnFoodSpoiled += _ => _foodPreservation64Dirty = true;
                _foodPreservation64.OnCuringCompleted += _ => _foodPreservation64Dirty = true;
            }

            // 2. Pre-war Archive Decryption (Plan 62)
            if (_archiveDecryption62 == null)
            {
                var archiveCatalog = PrewarArchiveCatalogLoader.Load(_dataDir, new FileSystemIO());
                var rng = _campaignDay != null ? _campaignDay.Rng.Fork("archive_decrypt") : new SeededRng(62);
                _archiveDecryption62 = new PrewarArchiveDecryptionSystem(rng, _inventory.Inventory, archiveCatalog, new GodotLog());

                var savedArchives = PrewarArchiveSaveStore.TryLoad();
                if (savedArchives != null)
                {
                    _archiveDecryption62.RestoreState(savedArchives);
                }

                _archiveDecryption62.OnArchiveDecrypted += (_, _) => _archiveDecryption62Dirty = true;
                _archiveDecryption62.OnArchiveDiscovered += _ => _archiveDecryption62Dirty = true;
            }

            // 3. Shelter Prisoner System (Plan 63)
            if (_shelterPrisoner63 == null)
            {
                var captiveCatalog = CaptiveInterrogationCatalogLoader.Load(_dataDir, new FileSystemIO());
                var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_prisoners") : new SeededRng(63);
                _shelterPrisoner63 = new ShelterPrisonerSystem(rng, _inventory.Inventory, captiveCatalog, new GodotLog());

                var savedPrisoners = ShelterPrisonerSaveStore.TryLoad();
                if (savedPrisoners != null)
                {
                    _shelterPrisoner63.RestoreState(savedPrisoners);
                }

                _shelterPrisoner63.OnPrisonerCaptured += _ => _shelterPrisoner63Dirty = true;
                _shelterPrisoner63.OnPrisonerParoled += _ => _shelterPrisoner63Dirty = true;
                _shelterPrisoner63.OnTopicExtracted += (_, _) => _shelterPrisoner63Dirty = true;
            }

            // 4. Grand Epilogue Engine (Plan 65)
            if (_epilogueEngine65 == null)
            {
                var epilogueCatalog = CampaignEpilogueCatalogLoader.Load(_dataDir, new FileSystemIO());
                _epilogueEngine65 = new CampaignEpilogueEngine(epilogueCatalog);
            }
        }

        private void SaveFoodPreservation()
        {
            if (_foodPreservation64 != null)
            {
                CaptureSection(FoodPreservationSaveStore.SectionName,
                    FoodPreservationSaveStore.TryCapturePersisted(_foodPreservation64.CaptureState()));
                _foodPreservation64Dirty = false;
            }
        }

        private void SavePrewarArchives()
        {
            if (_archiveDecryption62 != null)
            {
                CaptureSection(PrewarArchiveSaveStore.SectionName,
                    PrewarArchiveSaveStore.TryCapturePersisted(_archiveDecryption62.CaptureState()));
                _archiveDecryption62Dirty = false;
            }
        }

        private void SaveShelterPrisoners()
        {
            if (_shelterPrisoner63 != null)
            {
                CaptureSection(ShelterPrisonerSaveStore.SectionName,
                    ShelterPrisonerSaveStore.TryCapturePersisted(_shelterPrisoner63.CaptureState()));
                _shelterPrisoner63Dirty = false;
            }
        }

        private void FlushFoodPreservationIfDirty()
        {
            if (_foodPreservation64Dirty)
                SaveFoodPreservation();
        }

        private void FlushPrewarArchivesIfDirty()
        {
            if (_archiveDecryption62Dirty)
                SavePrewarArchives();
        }

        private void FlushShelterPrisonersIfDirty()
        {
            if (_shelterPrisoner63Dirty)
                SaveShelterPrisoners();
        }

        public void TickPlans62To65(int day)
        {
            bool isPowerOnline = _powerGrid?.System == null || !_powerGrid.System.IsBrownout;

            if (_foodPreservation64 != null)
            {
                _foodPreservation64.SetPowerStatus(isPowerOnline);
                _foodPreservation64.TickDay(day);
                _foodPreservation64Dirty = true;
            }

            if (_archiveDecryption62 != null)
            {
                _archiveDecryption62.SetPowerStatus(isPowerOnline);
                _archiveDecryption62.TickDay(day);
                _archiveDecryption62Dirty = true;
            }

            if (_shelterPrisoner63 != null)
            {
                _shelterPrisoner63.TickDay(day);
                _shelterPrisoner63Dirty = true;
            }
        }
    }
}
