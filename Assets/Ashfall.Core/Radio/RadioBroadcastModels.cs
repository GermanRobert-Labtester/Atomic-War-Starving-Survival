// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Content genre / classification for unified radio broadcasts.
    /// </summary>
    public enum BroadcastGenre
    {
        CivilianNews = 0,
        MilitaryEdict = 1,
        EmergencyAlert = 2,
        NumbersStation = 3,
        SurvivorTestimony = 4,
        ReligiousLiturgy = 5,
        TradeMarket = 6,
        Educational = 7,
        InfrastructureLogistics = 8,
        VerdictCensus = 9,
        FactionWar = 10,
        AutomatedLoop = 11,
        DistressSignal = 12,
        AtmosphericMystery = 13
    }

    /// <summary>
    /// Source reliability tier for diegetic broadcasts.
    /// </summary>
    public enum SourceReliability
    {
        Official = 0,
        Partisan = 1,
        Anonymous = 2,
        Automated = 3,
        Unknown = 4
    }

    /// <summary>
    /// Alert / priority tier for radio transmission interruption.
    /// </summary>
    public enum BroadcastPriority
    {
        Routine = 0,
        Important = 1,
        Urgent = 2,
        Emergency = 3
    }

    /// <summary>
    /// Operational state of a radio station broadcaster.
    /// </summary>
    public enum RadioStationState
    {
        Normal = 0,
        Degraded = 1,
        Jammed = 2,
        Captured = 3,
        Silent = 4,
        EmergencyOnly = 5
    }

    /// <summary>
    /// Canonical station definition in ASHFALL airwaves.
    /// </summary>
    [Serializable]
    public sealed class RadioStationDefinition
    {
        public string StationId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float FrequencyMhz { get; set; } = 88.5f;
        public string OwnerFactionId { get; set; } = string.Empty;
        public string PersonaVoice { get; set; } = string.Empty;
        public SourceReliability Reliability { get; set; } = SourceReliability.Official;
        public RadioStationState DefaultState { get; set; } = RadioStationState.Normal;
        public string SilenceText { get; set; } = "STATIC... [ Carrier hum steady. No voice detected. ]";
        public string JammedText { get; set; } = "STATIC... [ Severe RF interference / heterodyne squeal. ]";
    }

    /// <summary>
    /// Unified broadcast record combining data from radio.json, year_of_ash_radio.json,
    /// verdict_radio.json, and faction_war_radio.json.
    /// </summary>
    [Serializable]
    public sealed class UnifiedRadioBroadcast
    {
        public string BroadcastId { get; set; } = string.Empty;
        public float FrequencyMhz { get; set; } = 88.5f;
        public int DayMin { get; set; } = 1;
        public int DayMax { get; set; } = 9999;
        public int DayTrigger { get; set; } = 1;
        public string StationId { get; set; } = string.Empty;
        public string SourceName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
        public SourceReliability Reliability { get; set; } = SourceReliability.Official;
        public BroadcastPriority Priority { get; set; } = BroadcastPriority.Routine;
        public int SignalStrength { get; set; } = 5; // S-units 1..9
        public bool IsEmergency { get; set; }
        public bool IsOneShot { get; set; }
        public string AudioCue { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public string DownstreamConsequence { get; set; } = string.Empty;
    }

    /// <summary>
    /// Appointment program format definition.
    /// </summary>
    [Serializable]
    public sealed class AppointmentProgramDefinition
    {
        public string ProgramId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string StationId { get; set; } = string.Empty;
        public float FrequencyMhz { get; set; } = 88.5f;
        public int CadenceDays { get; set; } = 1;
        public int DayOffset { get; set; } = 0;
        public int CadenceWindow { get; set; } = 0;
        public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
        public BroadcastPriority Priority { get; set; } = BroadcastPriority.Important;
    }
}
