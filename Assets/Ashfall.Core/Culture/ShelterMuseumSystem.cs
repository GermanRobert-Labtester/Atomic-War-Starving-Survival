// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Culture
{
    public enum ArtifactType
    {
        Document = 0,
        Tool = 1,
        Weapon = 2,
        Clothing = 3,
        PersonalEffect = 4,
        Technological = 5,
        Natural = 6,
        Historical = 7
    }

    public enum ExhibitionTheme
    {
        Founding = 0,
        Medical = 1,
        Military = 2,
        Cultural = 3,
        Technological = 4,
        Personal = 5,
        Memorial = 6
    }

    public enum ExhibitionStatus
    {
        Planned = 0,
        Active = 1,
        Completed = 2,
        Cancelled = 3
    }

    [Serializable]
    public sealed class MuseumArtifact
    {
        public string ArtifactId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string ArtifactName { get; set; } = string.Empty;
        public ArtifactType ArtifactType { get; set; } = ArtifactType.Historical;
        public string OriginStory { get; set; } = string.Empty;
        public float HistoricalSignificance { get; set; } = 50f;
        public float Condition { get; set; } = 100f;
        public int DisplayedSince { get; set; } = 1;
        public string DonorId { get; set; } = string.Empty;
        public bool IsOnDisplay { get; set; } = true;
    }

    [Serializable]
    public sealed class Exhibition
    {
        public string ExhibitionId { get; set; } = string.Empty;
        public string ExhibitionName { get; set; } = string.Empty;
        public ExhibitionTheme Theme { get; set; } = ExhibitionTheme.Founding;
        public List<string> ArtifactIds { get; set; } = new List<string>();
        public int StartDay { get; set; } = 1;
        public int EndDay { get; set; } = -1; // -1 = permanent
        public string Description { get; set; } = string.Empty;
        public int VisitorCount { get; set; } = 0;
        public float MoraleBoost { get; set; } = 5f;
        public ExhibitionStatus Status { get; set; } = ExhibitionStatus.Active;
    }

    [Serializable]
    public sealed class MuseumEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public List<string> Participants { get; set; } = new List<string>();
        public string Significance { get; set; } = "moderate";
    }

    [Serializable]
    public sealed class MuseumArtifactTemplate
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string artifact_type { get; set; } = "historical";
        public float default_significance { get; set; } = 50f;
        public string default_origin { get; set; } = string.Empty;
        public string theme { get; set; } = "founding";
        public string description { get; set; } = string.Empty;

        public ArtifactType ParseArtifactType() => artifact_type?.ToLowerInvariant() switch
        {
            "document" => ArtifactType.Document,
            "tool" => ArtifactType.Tool,
            "weapon" => ArtifactType.Weapon,
            "clothing" => ArtifactType.Clothing,
            "personal_effect" or "personal" => ArtifactType.PersonalEffect,
            "technological" or "tech" => ArtifactType.Technological,
            "natural" => ArtifactType.Natural,
            _ => ArtifactType.Historical
        };

        public ExhibitionTheme ParseTheme() => theme?.ToLowerInvariant() switch
        {
            "medical" => ExhibitionTheme.Medical,
            "military" => ExhibitionTheme.Military,
            "cultural" => ExhibitionTheme.Cultural,
            "technological" => ExhibitionTheme.Technological,
            "personal" => ExhibitionTheme.Personal,
            "memorial" => ExhibitionTheme.Memorial,
            _ => ExhibitionTheme.Founding
        };
    }

    [Serializable]
    public sealed class MuseumCollectionCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<MuseumArtifactTemplate> artifact_templates { get; set; } = new List<MuseumArtifactTemplate>();
        public List<string> exhibition_themes { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class ShelterMuseumState
    {
        public int SchemaVersion { get; set; } = 1;
        public string CuratorId { get; set; } = string.Empty;
        public string MuseumName { get; set; } = "Shelter Historical Archive";
        public int EstablishedDay { get; set; } = 1;
        public int TotalVisitors { get; set; } = 0;
        public List<MuseumArtifact> Artifacts { get; set; } = new List<MuseumArtifact>();
        public List<Exhibition> Exhibitions { get; set; } = new List<Exhibition>();
        public List<MuseumEvent> Events { get; set; } = new List<MuseumEvent>();
    }

    /// <summary>
    /// Plan 218 — Shelter Museum & Historical Archive System.
    /// Manages the collection, preservation, and curation of historical shelter artifacts,
    /// themed exhibitions, curator appointments, and survivor visits.
    /// </summary>
    public sealed class ShelterMuseumSystem
    {
        private readonly ShelterMuseumState _state;
        private readonly Dictionary<string, MuseumArtifactTemplate> _templates =
            new Dictionary<string, MuseumArtifactTemplate>(StringComparer.OrdinalIgnoreCase);

        private int _nextArtifactSeq = 1;
        private int _nextExhibitionSeq = 1;
        private int _nextEventSeq = 1;

        public event Action<MuseumArtifact>? OnArtifactDonated;
        public event Action<Exhibition>? OnExhibitionOpened;
        public event Action<Exhibition>? OnExhibitionClosed;
        public event Action<string, float>? OnMuseumVisited; // visitorId, moraleGain
        public event Action<string>? OnCuratorAppointed;

        public int TotalArtifactCount => _state.Artifacts.Count;
        public int DisplayedArtifactCount => _state.Artifacts.Count(a => a.IsOnDisplay);
        public int ActiveExhibitionCount => _state.Exhibitions.Count(e => e.Status == ExhibitionStatus.Active);
        public int TotalVisitors => _state.TotalVisitors;
        public string CuratorId => _state.CuratorId;

        public ShelterMuseumSystem(ShelterMuseumState? state = null)
        {
            _state = state ?? new ShelterMuseumState();
            _nextArtifactSeq = _state.Artifacts.Count + 1;
            _nextExhibitionSeq = _state.Exhibitions.Count + 1;
            _nextEventSeq = _state.Events.Count + 1;
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var catalog = System.Text.Json.JsonSerializer.Deserialize<MuseumCollectionCatalogData>(json);
                if (catalog?.artifact_templates != null)
                {
                    foreach (var t in catalog.artifact_templates)
                    {
                        if (!string.IsNullOrWhiteSpace(t.id))
                        {
                            _templates[t.id] = t;
                        }
                    }
                }
            }
            catch
            {
                // Fallback
            }
        }

        public MuseumArtifactTemplate? GetTemplate(string templateId)
        {
            if (string.IsNullOrWhiteSpace(templateId)) return null;
            return _templates.TryGetValue(templateId, out var t) ? t : null;
        }

        public void AppointCurator(string survivorId, int currentDay)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return;
            _state.CuratorId = survivorId.Trim();

            RecordEvent("curator_appointed", currentDay, $"Survivor {survivorId} appointed as Chief Museum Curator.",
                new[] { survivorId }, "major");

            OnCuratorAppointed?.Invoke(_state.CuratorId);
        }

        public MuseumArtifact DonateArtifact(
            string itemId,
            string name,
            ArtifactType type,
            string originStory,
            float significance,
            string donorId,
            int currentDay)
        {
            var artifact = new MuseumArtifact
            {
                ArtifactId = $"art_{_nextArtifactSeq++}",
                ItemId = itemId ?? string.Empty,
                ArtifactName = !string.IsNullOrWhiteSpace(name) ? name : "Historical Relic",
                ArtifactType = type,
                OriginStory = originStory ?? string.Empty,
                HistoricalSignificance = Math.Clamp(significance, 1f, 100f),
                Condition = 100f,
                DisplayedSince = currentDay,
                DonorId = donorId ?? string.Empty,
                IsOnDisplay = true
            };

            _state.Artifacts.Add(artifact);

            RecordEvent("artifact_donated", currentDay,
                $"Artifact '{artifact.ArtifactName}' donated by {donorId}.",
                new[] { donorId },
                artifact.HistoricalSignificance >= 80f ? "major" : "moderate");

            OnArtifactDonated?.Invoke(artifact);
            return artifact;
        }

        public MuseumArtifact? DonateFromTemplate(
            string templateId,
            string donorId,
            int currentDay,
            string? customOrigin = null)
        {
            if (string.IsNullOrWhiteSpace(templateId) || !_templates.TryGetValue(templateId, out var t))
                return null;

            return DonateArtifact(
                itemId: t.id,
                name: t.name,
                type: t.ParseArtifactType(),
                originStory: !string.IsNullOrWhiteSpace(customOrigin) ? customOrigin : t.default_origin,
                significance: t.default_significance,
                donorId: donorId,
                currentDay: currentDay);
        }

        public Exhibition CurateExhibition(
            string name,
            ExhibitionTheme theme,
            IEnumerable<string> artifactIds,
            int startDay,
            int durationDays,
            string description,
            float moraleBoost = 5f)
        {
            var exhibition = new Exhibition
            {
                ExhibitionId = $"exh_{_nextExhibitionSeq++}",
                ExhibitionName = !string.IsNullOrWhiteSpace(name) ? name : "Special Exhibition",
                Theme = theme,
                ArtifactIds = artifactIds?.ToList() ?? new List<string>(),
                StartDay = startDay,
                EndDay = durationDays > 0 ? startDay + durationDays : -1,
                Description = description ?? string.Empty,
                VisitorCount = 0,
                MoraleBoost = Math.Max(1f, moraleBoost),
                Status = ExhibitionStatus.Active
            };

            // Set all contained artifacts to on display
            foreach (var artId in exhibition.ArtifactIds)
            {
                var artifact = _state.Artifacts.FirstOrDefault(a => string.Equals(a.ArtifactId, artId, StringComparison.OrdinalIgnoreCase));
                if (artifact != null)
                {
                    artifact.IsOnDisplay = true;
                }
            }

            _state.Exhibitions.Add(exhibition);

            RecordEvent("exhibition_opened", startDay,
                $"Exhibition '{exhibition.ExhibitionName}' opened to shelter dwellers.",
                new[] { _state.CuratorId }, "major");

            OnExhibitionOpened?.Invoke(exhibition);
            return exhibition;
        }

        public bool CloseExhibition(string exhibitionId, int currentDay)
        {
            var exhibition = _state.Exhibitions.FirstOrDefault(e => string.Equals(e.ExhibitionId, exhibitionId, StringComparison.OrdinalIgnoreCase));
            if (exhibition == null || exhibition.Status != ExhibitionStatus.Active)
                return false;

            exhibition.Status = ExhibitionStatus.Completed;
            exhibition.EndDay = currentDay;

            RecordEvent("exhibition_closed", currentDay,
                $"Exhibition '{exhibition.ExhibitionName}' concluded with {exhibition.VisitorCount} visitors.",
                new[] { _state.CuratorId }, "moderate");

            OnExhibitionClosed?.Invoke(exhibition);
            return true;
        }

        public float VisitMuseum(string visitorId, int currentDay)
        {
            if (string.IsNullOrWhiteSpace(visitorId)) return 0f;

            _state.TotalVisitors++;

            // Calculate morale based on active exhibitions and displayed artifacts
            float totalMorale = 2.0f; // Base visit comfort

            var activeExhibitions = _state.Exhibitions.Where(e => e.Status == ExhibitionStatus.Active).ToList();
            foreach (var exh in activeExhibitions)
            {
                exh.VisitorCount++;
                totalMorale += exh.MoraleBoost * 0.5f;
            }

            // High significance artifacts add bonus
            float significanceBonus = _state.Artifacts
                .Where(a => a.IsOnDisplay)
                .Select(a => a.HistoricalSignificance)
                .DefaultIfEmpty(0f)
                .Average() * 0.05f;

            totalMorale += significanceBonus;
            totalMorale = Math.Clamp(totalMorale, 1.0f, 25.0f);

            OnMuseumVisited?.Invoke(visitorId, totalMorale);
            return totalMorale;
        }

        public void TickDay(int currentDay)
        {
            // Auto-conclude expired exhibitions
            foreach (var exh in _state.Exhibitions)
            {
                if (exh.Status == ExhibitionStatus.Active && exh.EndDay > 0 && currentDay >= exh.EndDay)
                {
                    CloseExhibition(exh.ExhibitionId, currentDay);
                }
            }
        }

        public float GetHistoricalSignificanceScore()
        {
            if (_state.Artifacts.Count == 0) return 0f;
            return (float)Math.Round(_state.Artifacts.Average(a => a.HistoricalSignificance), 1);
        }

        public IReadOnlyList<Exhibition> GetActiveExhibitions()
        {
            return _state.Exhibitions.Where(e => e.Status == ExhibitionStatus.Active).ToList();
        }

        public IReadOnlyList<MuseumArtifact> GetArtifactsOnDisplay()
        {
            return _state.Artifacts.Where(a => a.IsOnDisplay).ToList();
        }

        public IReadOnlyList<MuseumEvent> GetEvents() => _state.Events;

        public MuseumEvent RecordEvent(string eventType, int day, string description, IEnumerable<string>? participants, string significance = "moderate")
        {
            var ev = new MuseumEvent
            {
                EventId = $"mev_{_nextEventSeq++}",
                EventType = eventType,
                Day = day,
                Description = description ?? string.Empty,
                Participants = participants?.ToList() ?? new List<string>(),
                Significance = significance
            };

            _state.Events.Add(ev);
            return ev;
        }

        public ShelterMuseumState CaptureState()
        {
            var copy = new ShelterMuseumState
            {
                SchemaVersion = _state.SchemaVersion,
                CuratorId = _state.CuratorId,
                MuseumName = _state.MuseumName,
                EstablishedDay = _state.EstablishedDay,
                TotalVisitors = _state.TotalVisitors,
                Artifacts = new List<MuseumArtifact>(_state.Artifacts.Count),
                Exhibitions = new List<Exhibition>(_state.Exhibitions.Count),
                Events = new List<MuseumEvent>(_state.Events.Count)
            };

            foreach (var a in _state.Artifacts)
            {
                copy.Artifacts.Add(new MuseumArtifact
                {
                    ArtifactId = a.ArtifactId,
                    ItemId = a.ItemId,
                    ArtifactName = a.ArtifactName,
                    ArtifactType = a.ArtifactType,
                    OriginStory = a.OriginStory,
                    HistoricalSignificance = a.HistoricalSignificance,
                    Condition = a.Condition,
                    DisplayedSince = a.DisplayedSince,
                    DonorId = a.DonorId,
                    IsOnDisplay = a.IsOnDisplay
                });
            }

            foreach (var e in _state.Exhibitions)
            {
                copy.Exhibitions.Add(new Exhibition
                {
                    ExhibitionId = e.ExhibitionId,
                    ExhibitionName = e.ExhibitionName,
                    Theme = e.Theme,
                    ArtifactIds = new List<string>(e.ArtifactIds),
                    StartDay = e.StartDay,
                    EndDay = e.EndDay,
                    Description = e.Description,
                    VisitorCount = e.VisitorCount,
                    MoraleBoost = e.MoraleBoost,
                    Status = e.Status
                });
            }

            foreach (var ev in _state.Events)
            {
                copy.Events.Add(new MuseumEvent
                {
                    EventId = ev.EventId,
                    EventType = ev.EventType,
                    Day = ev.Day,
                    Description = ev.Description,
                    Participants = new List<string>(ev.Participants),
                    Significance = ev.Significance
                });
            }

            return copy;
        }

        public void RestoreState(ShelterMuseumState state)
        {
            if (state == null) return;

            _state.SchemaVersion = state.SchemaVersion;
            _state.CuratorId = state.CuratorId;
            _state.MuseumName = state.MuseumName;
            _state.EstablishedDay = state.EstablishedDay;
            _state.TotalVisitors = state.TotalVisitors;

            _state.Artifacts.Clear();
            if (state.Artifacts != null)
            {
                foreach (var a in state.Artifacts)
                {
                    _state.Artifacts.Add(new MuseumArtifact
                    {
                        ArtifactId = a.ArtifactId,
                        ItemId = a.ItemId,
                        ArtifactName = a.ArtifactName,
                        ArtifactType = a.ArtifactType,
                        OriginStory = a.OriginStory,
                        HistoricalSignificance = a.HistoricalSignificance,
                        Condition = a.Condition,
                        DisplayedSince = a.DisplayedSince,
                        DonorId = a.DonorId,
                        IsOnDisplay = a.IsOnDisplay
                    });
                }
            }

            _state.Exhibitions.Clear();
            if (state.Exhibitions != null)
            {
                foreach (var e in state.Exhibitions)
                {
                    _state.Exhibitions.Add(new Exhibition
                    {
                        ExhibitionId = e.ExhibitionId,
                        ExhibitionName = e.ExhibitionName,
                        Theme = e.Theme,
                        ArtifactIds = new List<string>(e.ArtifactIds ?? Enumerable.Empty<string>()),
                        StartDay = e.StartDay,
                        EndDay = e.EndDay,
                        Description = e.Description,
                        VisitorCount = e.VisitorCount,
                        MoraleBoost = e.MoraleBoost,
                        Status = e.Status
                    });
                }
            }

            _state.Events.Clear();
            if (state.Events != null)
            {
                foreach (var ev in state.Events)
                {
                    _state.Events.Add(new MuseumEvent
                    {
                        EventId = ev.EventId,
                        EventType = ev.EventType,
                        Day = ev.Day,
                        Description = ev.Description,
                        Participants = new List<string>(ev.Participants ?? Enumerable.Empty<string>()),
                        Significance = ev.Significance
                    });
                }
            }

            _nextArtifactSeq = _state.Artifacts.Count + 1;
            _nextExhibitionSeq = _state.Exhibitions.Count + 1;
            _nextEventSeq = _state.Events.Count + 1;
        }
    }
}
