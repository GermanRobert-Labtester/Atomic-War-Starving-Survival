// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Culture;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CultureCreationHostSession? _cultureCreation;
        private bool _cultureCreationDirty;

        public CultureCreationHostSession EnsureCultureCreation()
        {
            if (_cultureCreation != null) return _cultureCreation;
            SetupCultureCreation();
            return _cultureCreation!;
        }

        private void SetupCultureCreation()
        {
            if (_cultureCreation != null) return;

            string dataDir = CatalogPath.ResolveDataDir();
            var saved = CultureCreationSaveStore.TryLoad();
            _cultureCreation = CultureCreationHostSession.Create(dataDir, saved);

            _cultureCreation.StateChanged += () =>
            {
                _cultureCreationDirty = true;
            };
        }

        private void SaveCultureCreation()
        {
            if (_cultureCreation == null) return;

            var state = _cultureCreation.CaptureState();
            CultureCreationSaveStore.TrySave(state);
            string? payload = CultureCreationSaveStore.TryCapturePersisted(state);
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(CultureCreationSaveStore.SectionName, payload);
            }
            _cultureCreationDirty = false;
        }

        public void FlushCultureCreationSave()
        {
            if (_cultureCreationDirty)
            {
                SaveCultureCreation();
            }
        }

        public void ResetCultureCreation()
        {
            _cultureCreation = null;
            _cultureCreationDirty = false;
        }

        public ArtworkRecord CreateArtwork(
            string creatorSurvivorId,
            string title,
            ArtMedium medium,
            ArtTheme theme,
            float artistSkill = 50f)
        {
            var session = EnsureCultureCreation();
            var rng = _campaignDay?.Rng.Fork("culture_creation") ?? new SeededRng(178);
            var record = session.CreateArtwork(creatorSurvivorId, title, medium, theme, _simDay, artistSkill, rng);

            _journal?.TryAddRawEntry(
                "artwork_created",
                $"Artwork '{record.Title}' created by {creatorSurvivorId} (quality: {record.QualityScore:F0}{(record.IsMasterwork ? ", MASTERWORK" : "")}).",
                null!,
                _simDay);

            return record;
        }

        public bool DisplayArtwork(string artworkId, string locationId)
        {
            return EnsureCultureCreation().DisplayArtwork(artworkId, locationId);
        }

        public float GetShelterCultureMoraleBonus()
        {
            return EnsureCultureCreation().GetShelterCultureMoraleBonus();
        }

        public CultureCreationCensus GetCultureCreationCensus()
        {
            return EnsureCultureCreation().GetCensus();
        }
    }
}
