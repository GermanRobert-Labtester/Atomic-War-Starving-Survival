using System.Collections.Generic;
using Ashfall.Core;
using AtomicWar.GodotApp.Host;
using Godot;

namespace AtomicWar.GodotApp.Host
{
    /// <summary>
    /// ASHFALL — Events Host Session.
    /// Manages event history, incidents, and narrative progression for the Events Log panel.
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
            string eventsJsonPath = "res://Assets/StreamingAssets/Data/events.json";
            string eventsJson = _fileIO.ReadAllText(eventsJsonPath);
            var eventsData = _jsonSerializer.Deserialize<EventsRoot>(eventsJson);
            _events = eventsData.Events;
        }

        private void LoadIncidents()
        {
            string incidentsJsonPath = "res://Assets/StreamingAssets/Data/incidents.json";
            if (_fileIO.FileExists(incidentsJsonPath))
            {
                string incidentsJson = _fileIO.ReadAllText(incidentsJsonPath);
                var incidentsData = _jsonSerializer.Deserialize<IncidentsRoot>(incidentsJson);
                _incidents = incidentsData.Incidents;
            }
        }

        private void LoadNarrativeProgression()
        {
            string narrativeJsonPath = "res://Assets/StreamingAssets/Data/narrative_progression.json";
            if (_fileIO.FileExists(narrativeJsonPath))
            {
                string narrativeJson = _fileIO.ReadAllText(narrativeJsonPath);
                var narrativeData = _jsonSerializer.Deserialize<NarrativeRoot>(narrativeJson);
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