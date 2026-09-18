// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using AtomicWar.GodotApp.Host;
using Godot;

namespace AtomicWar.GodotApp.Host
{
    /// <summary>
    /// ASHFALL — Events Host Session.
    /// Catalog/read-model for event history, incidents, and narrative progression displayed
    /// by the Events Log panel. Dynamic trigger progress belongs to HostEventAdapter and is
    /// persisted through HostEventSaveStore; this session intentionally has no save state.
    /// </summary>
    public partial class EventsHostSession : Node
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IFileIO _fileIO;
        private List<EventData> _events;
        private List<IncidentData> _incidents;
        private List<NarrativeEntryData> _narrativeProgression;

        public EventsHostSession(IJsonSerializer jsonSerializer, IFileIO fileIO)
        {
            _jsonSerializer = jsonSerializer;
            _fileIO = fileIO;
            _events = new List<EventData>();
            _incidents = new List<IncidentData>();
            _narrativeProgression = new List<NarrativeEntryData>();
        }

        public override void _Ready()
        {
            LoadEvents();
            LoadIncidents();
            LoadNarrativeProgression();
        }

        private void LoadEvents()
        {
            string eventsJsonPath = CatalogPath.ResolveCatalog("events.json");
            string eventsJson = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir()).ReadAllText(eventsJsonPath);
            var eventsData = _jsonSerializer.Deserialize<EventsRoot>(eventsJson);
            if (eventsData?.Events != null)
                _events = eventsData.Events;
        }

        /// <summary>
        /// Resolve one authored event for a host adapter. The lazy guard keeps
        /// composition-order safe: a trapping event can be delivered before
        /// Godot has run this node's _Ready callback.
        /// </summary>
        public bool TryGetEvent(string eventId, out EventData eventData)
        {
            if (_events.Count == 0)
                LoadEvents();
            for (int i = 0; i < _events.Count; i++)
            {
                var candidate = _events[i];
                if (candidate != null && string.Equals(candidate.Id, eventId, System.StringComparison.Ordinal))
                {
                    eventData = candidate;
                    return true;
                }
            }
            eventData = null!;
            return false;
        }

        private void LoadIncidents()
        {
            string incidentsJsonPath = CatalogPath.ResolveCatalog("incidents.json");
            var io = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (io.FileExists(incidentsJsonPath))
            {
                string incidentsJson = io.ReadAllText(incidentsJsonPath);
                var incidentsData = _jsonSerializer.Deserialize<IncidentsRoot>(incidentsJson);
                if (incidentsData?.Incidents != null)
                    _incidents = incidentsData.Incidents;
            }
        }

        private void LoadNarrativeProgression()
        {
            string narrativeJsonPath = CatalogPath.ResolveCatalog("narrative_progression.json");
            var io = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (io.FileExists(narrativeJsonPath))
            {
                string narrativeJson = io.ReadAllText(narrativeJsonPath);
                var narrativeData = _jsonSerializer.Deserialize<NarrativeRoot>(narrativeJson);
                if (narrativeData?.Entries != null)
                    _narrativeProgression = narrativeData.Entries;
            }
        }

        /// <summary>
        /// Returns a list of recent events with Day and Description properties.
        /// </summary>
        public List<EventEntry> GetRecentEvents()
        {
            var recentEvents = new List<EventEntry>();
            foreach (var evt in _events)
            {
                recentEvents.Add(new EventEntry
                {
                    Day = evt.MinDay,
                    Description = evt.BodyText
                });
            }
            return recentEvents;
        }

        /// <summary>
        /// Returns a list of incidents with Day and Description properties.
        /// </summary>
        public List<IncidentEntry> GetIncidents()
        {
            var incidents = new List<IncidentEntry>();
            foreach (var incident in _incidents)
            {
                incidents.Add(new IncidentEntry
                {
                    Day = incident.MinDay,
                    Description = incident.BodyText
                });
            }
            return incidents;
        }

        /// <summary>
        /// Returns a list of narrative entries with Description and Order properties.
        /// </summary>
        public List<NarrativeEntry> GetNarrativeProgression()
        {
            var narrativeEntries = new List<NarrativeEntry>();
            foreach (var entry in _narrativeProgression)
            {
                narrativeEntries.Add(new NarrativeEntry
                {
                    Description = entry.Description,
                    Order = entry.Order
                });
            }
            return narrativeEntries;
        }
    }

    // Data Models
    public class EventsRoot
    {
        public int SchemaVersion { get; set; }
        public List<EventData> Events { get; set; }
    }

    public class EventData
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BodyText { get; set; } = string.Empty;
        public float Weight { get; set; }
        public int MinDay { get; set; }
    }

    public class IncidentData
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BodyText { get; set; } = string.Empty;
        public float Weight { get; set; }
        public int MinDay { get; set; }
    }

    public class NarrativeEntryData
    {
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public class IncidentsRoot
    {
        public int SchemaVersion { get; set; }
        public List<IncidentData> Incidents { get; set; } = new List<IncidentData>();
    }

    public class NarrativeRoot
    {
        public int SchemaVersion { get; set; }
        public List<NarrativeEntryData> Entries { get; set; } = new List<NarrativeEntryData>();
    }

    // Return Models
    public class EventEntry
    {
        public int Day { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class IncidentEntry
    {
        public int Day { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class NarrativeEntry
    {
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
