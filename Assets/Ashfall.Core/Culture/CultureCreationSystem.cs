// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Culture
{
    public enum ArtMedium
    {
        Painting = 0,
        Sculpture = 1,
        MusicComposition = 2,
        Poetry = 3,
        Storytelling = 4,
        Craftwork = 5
    }

    public enum ArtTheme
    {
        Hope = 0,
        Loss = 1,
        Resistance = 2,
        Nature = 3,
        Community = 4,
        Memorial = 5
    }

    [Serializable]
    public sealed class ArtworkRecord
    {
        public string ArtworkId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CreatorSurvivorId { get; set; } = string.Empty;
        public ArtMedium Medium { get; set; } = ArtMedium.Painting;
        public ArtTheme Theme { get; set; } = ArtTheme.Hope;
        public int CreationDay { get; set; } = 1;
        public float QualityScore { get; set; } = 50f;
        public float CulturalImpact { get; set; } = 5f;
        public string DisplayLocation { get; set; } = string.Empty;
        public bool IsMasterwork { get; set; } = false;
    }

    [Serializable]
    public sealed class CultureCreationState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public string ShelterCulturalIdentity { get; set; } = "Pioneers";
        public float TotalCulturalValue { get; set; } = 0f;
        public List<ArtworkRecord> Artworks { get; set; } = new List<ArtworkRecord>();
    }

    /// <summary>
    /// Plan 178 — Art & Culture Creation System.
    /// Tracks survivor artistic expressions, compositions, sculptures, and literature,
    /// fostering communal morale, shelter cultural identity, and memory preservation.
    /// </summary>
    public sealed class CultureCreationSystem
    {
        private readonly CultureCreationState _state;

        public event Action<ArtworkRecord>? OnArtworkCreated;
        public event Action<ArtworkRecord>? OnMasterworkCreated;

        public int ArtworkCount => _state.Artworks.Count;
        public float TotalCulturalValue => _state.TotalCulturalValue;
        public string CulturalIdentity => _state.ShelterCulturalIdentity;

        public CultureCreationSystem(CultureCreationState? state = null)
        {
            _state = state ?? new CultureCreationState();
        }

        public ArtworkRecord CreateArtwork(
            string creatorSurvivorId,
            string title,
            ArtMedium medium,
            ArtTheme theme,
            int day,
            float artistSkill = 50f,
            ISeededRng? rng = null)
        {
            if (string.IsNullOrEmpty(creatorSurvivorId)) throw new ArgumentNullException(nameof(creatorSurvivorId));

            float baseQuality = 30f + (artistSkill * 0.5f);
            float variance = 0f;
            if (rng != null)
            {
                variance = (float)((rng.NextDouble() * 30.0) - 10.0);
            }
            float finalQuality = Math.Clamp(baseQuality + variance, 10f, 100f);
            bool isMasterwork = finalQuality >= 85f;

            float impact = (finalQuality * 0.1f) * (isMasterwork ? 2.0f : 1.0f);

            var artwork = new ArtworkRecord
            {
                ArtworkId = $"art_{_state.NextSequence++}",
                Title = string.IsNullOrWhiteSpace(title) ? $"{medium} #{_state.NextSequence}" : title.Trim(),
                CreatorSurvivorId = creatorSurvivorId,
                Medium = medium,
                Theme = theme,
                CreationDay = Math.Max(1, day),
                QualityScore = finalQuality,
                CulturalImpact = impact,
                DisplayLocation = string.Empty,
                IsMasterwork = isMasterwork
            };

            _state.Artworks.Add(artwork);
            _state.TotalCulturalValue += impact;

            UpdateCulturalIdentity();

            OnArtworkCreated?.Invoke(artwork);
            if (isMasterwork)
            {
                OnMasterworkCreated?.Invoke(artwork);
            }

            return artwork;
        }

        public bool DisplayArtwork(string artworkId, string locationId)
        {
            if (string.IsNullOrEmpty(artworkId)) return false;

            var art = _state.Artworks.FirstOrDefault(a => string.Equals(a.ArtworkId, artworkId, StringComparison.OrdinalIgnoreCase));
            if (art == null) return false;

            art.DisplayLocation = locationId ?? string.Empty;
            return true;
        }

        public float GetShelterCultureMoraleBonus()
        {
            float bonus = 0f;
            for (int i = 0; i < _state.Artworks.Count; i++)
            {
                var art = _state.Artworks[i];
                if (!string.IsNullOrEmpty(art.DisplayLocation))
                {
                    bonus += art.CulturalImpact * 0.5f;
                }
            }
            return Math.Clamp(bonus, 0f, 15f);
        }

        private void UpdateCulturalIdentity()
        {
            if (_state.Artworks.Count < 3) return;

            var mostFrequentTheme = _state.Artworks
                .GroupBy(a => a.Theme)
                .OrderByDescending(g => g.Count())
                .First().Key;

            _state.ShelterCulturalIdentity = mostFrequentTheme switch
            {
                ArtTheme.Hope => "The Beacon",
                ArtTheme.Loss => "The Mourners",
                ArtTheme.Resistance => "The Iron-Willed",
                ArtTheme.Nature => "The Naturalists",
                ArtTheme.Community => "The Collective",
                ArtTheme.Memorial => "The Archivists",
                _ => "Pioneers"
            };
        }

        public CultureCreationState CaptureState()
        {
            var captured = new CultureCreationState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                ShelterCulturalIdentity = _state.ShelterCulturalIdentity,
                TotalCulturalValue = _state.TotalCulturalValue,
                Artworks = new List<ArtworkRecord>(_state.Artworks.Count)
            };

            for (int i = 0; i < _state.Artworks.Count; i++)
            {
                var a = _state.Artworks[i];
                captured.Artworks.Add(new ArtworkRecord
                {
                    ArtworkId = a.ArtworkId,
                    Title = a.Title,
                    CreatorSurvivorId = a.CreatorSurvivorId,
                    Medium = a.Medium,
                    Theme = a.Theme,
                    CreationDay = a.CreationDay,
                    QualityScore = a.QualityScore,
                    CulturalImpact = a.CulturalImpact,
                    DisplayLocation = a.DisplayLocation,
                    IsMasterwork = a.IsMasterwork
                });
            }

            return captured;
        }

        public void RestoreState(CultureCreationState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.ShelterCulturalIdentity = state.ShelterCulturalIdentity ?? "Pioneers";
            _state.TotalCulturalValue = state.TotalCulturalValue;
            _state.Artworks.Clear();

            if (state.Artworks != null)
            {
                for (int i = 0; i < state.Artworks.Count; i++)
                {
                    var a = state.Artworks[i];
                    _state.Artworks.Add(new ArtworkRecord
                    {
                        ArtworkId = a.ArtworkId,
                        Title = a.Title,
                        CreatorSurvivorId = a.CreatorSurvivorId,
                        Medium = a.Medium,
                        Theme = a.Theme,
                        CreationDay = a.CreationDay,
                        QualityScore = a.QualityScore,
                        CulturalImpact = a.CulturalImpact,
                        DisplayLocation = a.DisplayLocation,
                        IsMasterwork = a.IsMasterwork
                    });
                }
            }
        }
    }
}
