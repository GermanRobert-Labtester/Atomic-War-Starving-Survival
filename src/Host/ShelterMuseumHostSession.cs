// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterMuseumSaveStore
// Core State : Ashfall.Core.Culture.ShelterMuseumState
// Host Caller: Main.ShelterMuseum
// Purpose    : Plan 218 — Shelter Museum & Historical Archive host session & persistence.
//              DEC-203 ownership: the museum owns its collection, exhibitions,
//              curator, visits, and event history under its own save key —
//              separate from CulturalArchiveVaultSystem (`cultural_archives`)
//              and external communications. Physical-inventory donation is
//              NOT exposed (no transaction-safe custody bridge yet).
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Culture;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ShelterMuseumSaveStore
    {
        public const string FileName = "shelter_museum_save.json";
        public const string SectionName = "shelter_museum";

        private static readonly SaveStore<ShelterMuseumState> s_store =
            SaveStoreHub.Checksummed<ShelterMuseumState>(FileName, nameof(ShelterMuseumSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ShelterMuseumState state) => s_store.CaptureBare(state);
        public static ShelterMuseumState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ShelterMuseumState state) => s_store.TrySave(state);
        public static ShelterMuseumState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Read-only museum projection consumed by the archive desk surface.</summary>
    public sealed class ShelterMuseumSnapshot
    {
        public string MuseumName { get; set; } = string.Empty;
        public string CuratorId { get; set; } = string.Empty;
        public int ArtifactCount { get; set; }
        public int DisplayedArtifactCount { get; set; }
        public float SignificanceScore { get; set; }
        public int TotalVisitors { get; set; }
        public List<(string ExhibitionId, string Name, string Theme, int Visitors, bool Active)> Exhibitions { get; } = new();
        public List<(string EventType, int Day, string Description)> RecentEvents { get; } = new();
        public int AuthoredTemplateCount { get; set; }
    }

    /// <summary>
    /// Plan 218 host session. Wraps <see cref="ShelterMuseumSystem"/>.
    /// Player surface is inspection + explicit once-per-day visits; the morale
    /// delta is applied by the Main adapter through the canonical Needs owner.
    /// Donation from physical inventory is intentionally not exposed.
    /// </summary>
    public sealed class ShelterMuseumHostSession : HostSessionBase
    {
        private readonly ShelterMuseumSystem _system;
        private int _authoredTemplateCount;

        public ShelterMuseumSystem System => _system;
        public int AuthoredTemplateCount => _authoredTemplateCount;
        public string LastEvent { get; private set; } = string.Empty;

        public ShelterMuseumHostSession(ShelterMuseumState? state = null)
        {
            _system = new ShelterMuseumSystem(state);
            _system.OnArtifactDonated += a =>
            {
                LastEvent = $"Artifact accessioned: {a.ArtifactName} (significance {a.HistoricalSignificance:0}).";
                RaiseStateChanged();
            };
            _system.OnExhibitionOpened += e =>
            {
                LastEvent = $"Exhibition opened: {e.ExhibitionName}.";
                RaiseStateChanged();
            };
            _system.OnExhibitionClosed += e =>
            {
                LastEvent = $"Exhibition concluded: {e.ExhibitionName} ({e.VisitorCount} visitors).";
                RaiseStateChanged();
            };
            _system.OnCuratorAppointed += id =>
            {
                LastEvent = $"Curator appointed: {id}.";
                RaiseStateChanged();
            };
        }

        public static ShelterMuseumHostSession Create(ShelterMuseumState? state = null) =>
            new ShelterMuseumHostSession(state);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            _authoredTemplateCount = _system.GetAllTemplates().Count;
            LastEvent = $"Loaded {_authoredTemplateCount} artifact templates.";
            RaiseStateChanged();
        }

        /// <summary>
        /// Explicit once-per-day visit. Returns the morale delta for the Main
        /// adapter to apply through the canonical Needs owner (exactly once),
        /// or null when the survivor already visited today (no mutation).
        /// </summary>
        public float? Visit(string visitorId, int currentDay)
        {
            if (!_system.TryVisitMuseum(visitorId, currentDay, out float morale))
            {
                LastEvent = $"{visitorId} has already visited the museum today.";
                return null;
            }
            LastEvent = $"{visitorId} visited the museum (morale +{morale:0.#}).";
            return morale;
        }

        public void AppointCurator(string survivorId, int currentDay)
        {
            _system.AppointCurator(survivorId, currentDay);
            RaiseStateChanged();
        }

        /// <summary>
        /// Accessions a nonphysical historical record from an authored template.
        /// This consumes no inventory item; physical donation stays unavailable
        /// until a transaction-safe custody bridge is signed.
        /// </summary>
        public MuseumArtifact? AccessionTemplateRecord(string templateId, string donorId, int currentDay, string? customOrigin = null)
        {
            var artifact = _system.DonateFromTemplate(templateId, donorId, currentDay, customOrigin);
            if (artifact == null)
            {
                LastEvent = $"Accession refused: unknown template '{templateId}'.";
            }
            return artifact;
        }

        public Exhibition? CurateExhibition(string name, ExhibitionTheme theme, IEnumerable<string> artifactIds, int startDay, int durationDays, string description)
        {
            var exhibition = _system.CurateExhibition(name, theme, artifactIds, startDay, durationDays, description);
            return exhibition;
        }

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
        }

        public ShelterMuseumSnapshot GetSnapshot()
        {
            var snap = new ShelterMuseumSnapshot
            {
                MuseumName = _system.MuseumName,
                CuratorId = _system.CuratorId,
                ArtifactCount = _system.TotalArtifactCount,
                DisplayedArtifactCount = _system.DisplayedArtifactCount,
                SignificanceScore = _system.GetHistoricalSignificanceScore(),
                TotalVisitors = _system.TotalVisitors,
                AuthoredTemplateCount = _authoredTemplateCount
            };
            foreach (var e in _system.GetActiveExhibitions())
            {
                snap.Exhibitions.Add((e.ExhibitionId, e.ExhibitionName, e.Theme.ToString(), e.VisitorCount, true));
            }
            var recent = _system.GetEvents();
            foreach (var ev in recent.Skip(Math.Max(0, recent.Count - 5)))
            {
                snap.RecentEvents.Add((ev.EventType, ev.Day, ev.Description));
            }
            return snap;
        }

        public ShelterMuseumState CaptureState() => _system.CaptureState();

        public void RestoreState(ShelterMuseumState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored shelter museum state.";
            RaiseStateChanged();
        }

        public bool TrySave() => ShelterMuseumSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = ShelterMuseumSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
