// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Defense
{
    public enum RadarOperationalMode
    {
        Off = 0,
        LowPowerStandby = 1,
        ActiveScan = 2,
        HighFrequencySweep = 3
    }

    public enum RadarTargetClassification
    {
        Unknown = 0,
        EnvironmentalNoise = 1,
        WildlifeCluster = 2,
        HostileIncursion = 3
    }

    [Serializable]
    public sealed class RadarContactSaveState
    {
        public string ContactId { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public int DistanceMeters { get; set; }
        public RadarTargetClassification Classification { get; set; }
        public int ConfidencePermille { get; set; }
        public int EstimatedArrivalMinutes { get; set; }
        public int DetectedTick { get; set; }
    }

    [Serializable]
    public sealed class PerimeterEarlyWarningSaveState
    {
        public int schema_version { get; set; } = 1;
        public RadarOperationalMode Mode { get; set; }
        public int CalibrationPermille { get; set; }
        public List<RadarContactSaveState> Contacts { get; set; } = new();
    }

    public sealed class RadarContact
    {
        public string ContactId { get; }
        public string Sector { get; }
        public int DistanceMeters { get; }
        public RadarTargetClassification Classification { get; }
        public int ConfidencePermille { get; }
        public int EstimatedArrivalMinutes { get; }
        public int DetectedTick { get; }

        public RadarContact(
            string contactId,
            string sector,
            int distanceMeters,
            RadarTargetClassification classification,
            int confidencePermille,
            int estimatedArrivalMinutes,
            int detectedTick)
        {
            ContactId = contactId ?? throw new ArgumentNullException(nameof(contactId));
            Sector = sector ?? PerimeterSector.Gate;
            DistanceMeters = Math.Max(0, distanceMeters);
            Classification = classification;
            ConfidencePermille = Math.Clamp(confidencePermille, 0, 1000);
            EstimatedArrivalMinutes = Math.Max(0, estimatedArrivalMinutes);
            DetectedTick = detectedTick;
        }

        internal RadarContact(RadarContactSaveState state)
        {
            ContactId = state.ContactId;
            Sector = state.Sector;
            DistanceMeters = state.DistanceMeters;
            Classification = state.Classification;
            ConfidencePermille = state.ConfidencePermille;
            EstimatedArrivalMinutes = state.EstimatedArrivalMinutes;
            DetectedTick = state.DetectedTick;
        }

        public RadarContactSaveState CaptureState()
        {
            return new RadarContactSaveState
            {
                ContactId = ContactId,
                Sector = Sector,
                DistanceMeters = DistanceMeters,
                Classification = Classification,
                ConfidencePermille = ConfidencePermille,
                EstimatedArrivalMinutes = EstimatedArrivalMinutes,
                DetectedTick = DetectedTick
            };
        }
    }

    /// <summary>
    /// Expansion 19 / UNBLOCK-05 §3.1 / §18.9:
    /// The Silent Watch — Automated Perimeter Early Warning Radar Engine.
    /// Models automated radar sensor sweeps, false-alarm calibration against storms/wildlife,
    /// power load consumption (watts draw), and threat lead-time warnings for shelter defenses.
    /// </summary>
    public sealed class PerimeterEarlyWarningEngine
    {
        public const int DefaultCalibrationPermille = 500; // 50% base calibration
        public const int ApproachingRaiderSpeedMetersPerMinute = 50; // ~3 km/h over rough wasteland terrain

        private readonly List<RadarContact> _activeContacts = new();

        public RadarOperationalMode Mode { get; private set; } = RadarOperationalMode.ActiveScan;
        public int CalibrationPermille { get; private set; } = DefaultCalibrationPermille;

        public IReadOnlyList<RadarContact> ActiveContacts => _activeContacts;

        public int PowerDrawWatts => Mode switch
        {
            RadarOperationalMode.Off => 0,
            RadarOperationalMode.LowPowerStandby => 50,
            RadarOperationalMode.ActiveScan => 250,
            RadarOperationalMode.HighFrequencySweep => 600,
            _ => 0
        };

        public int DetectionRangeMeters => Mode switch
        {
            RadarOperationalMode.Off => 0,
            RadarOperationalMode.LowPowerStandby => 1000,
            RadarOperationalMode.ActiveScan => 3000,
            RadarOperationalMode.HighFrequencySweep => 6000,
            _ => 0
        };

        public int BaseLeadTimeMinutes => Mode switch
        {
            RadarOperationalMode.Off => 0,
            RadarOperationalMode.LowPowerStandby => 20,
            RadarOperationalMode.ActiveScan => 60,
            RadarOperationalMode.HighFrequencySweep => 120,
            _ => 0
        };

        public event Action<RadarContact>? OnThreatDetected;
        public event Action<RadarContact>? OnFalseAlarmTriggered;
        public event Action<RadarOperationalMode, int>? OnModeChanged;

        public void SetMode(RadarOperationalMode mode)
        {
            if (Mode != mode)
            {
                Mode = mode;
                OnModeChanged?.Invoke(mode, PowerDrawWatts);
            }
        }

        public void CalibrateSensors(int deltaPermille)
        {
            CalibrationPermille = Math.Clamp(CalibrationPermille + deltaPermille, 100, 950);
        }

        public RadarContact? ProcessScanSweep(
            string sector,
            int actualDistanceMeters,
            bool isHostile,
            bool isStormOrDust,
            int currentTick,
            int seededRollPermille)
        {
            if (Mode == RadarOperationalMode.Off)
                return null;

            if (actualDistanceMeters > DetectionRangeMeters)
                return null; // Beyond current radar range

            string canonicalSector = PerimeterSector.IsValid(sector) ? sector : PerimeterSector.Gate;
            RadarTargetClassification classification;
            int confidence = CalibrationPermille;

            if (isStormOrDust)
            {
                // Poorly calibrated sensors confuse dust/atmospheric disturbance with hostiles
                if (seededRollPermille >= CalibrationPermille)
                {
                    // False alarm!
                    classification = RadarTargetClassification.HostileIncursion;
                    confidence = 400; // Low confidence false alarm
                }
                else
                {
                    classification = RadarTargetClassification.EnvironmentalNoise;
                    confidence = Math.Min(1000, CalibrationPermille + 200);
                }
            }
            else if (isHostile)
            {
                classification = RadarTargetClassification.HostileIncursion;
                confidence = Math.Min(1000, CalibrationPermille + 150);
            }
            else
            {
                classification = RadarTargetClassification.WildlifeCluster;
                confidence = Math.Min(1000, CalibrationPermille + 100);
            }

            int estimatedMinutes = actualDistanceMeters / Math.Max(1, ApproachingRaiderSpeedMetersPerMinute);
            string contactId = $"contact_{canonicalSector}_{currentTick}_{_activeContacts.Count + 1}";

            var contact = new RadarContact(
                contactId,
                canonicalSector,
                actualDistanceMeters,
                classification,
                confidence,
                estimatedMinutes,
                currentTick);

            _activeContacts.Add(contact);

            if (classification == RadarTargetClassification.HostileIncursion)
            {
                if (isStormOrDust)
                {
                    OnFalseAlarmTriggered?.Invoke(contact);
                }
                else
                {
                    OnThreatDetected?.Invoke(contact);
                }
            }

            return contact;
        }

        public void ClearExpiredContacts(int currentTick, int maxAgeTicks = 100)
        {
            _activeContacts.RemoveAll(c => currentTick - c.DetectedTick > maxAgeTicks);
        }

        public PerimeterEarlyWarningSaveState CaptureState()
        {
            var save = new PerimeterEarlyWarningSaveState
            {
                schema_version = 1,
                Mode = Mode,
                CalibrationPermille = CalibrationPermille,
                Contacts = new List<RadarContactSaveState>(_activeContacts.Count)
            };

            for (int i = 0; i < _activeContacts.Count; i++)
            {
                save.Contacts.Add(_activeContacts[i].CaptureState());
            }

            return save;
        }

        public void RestoreState(PerimeterEarlyWarningSaveState? state)
        {
            _activeContacts.Clear();
            if (state == null) return;

            Mode = state.Mode;
            CalibrationPermille = Math.Clamp(state.CalibrationPermille, 100, 950);

            if (state.Contacts != null)
            {
                for (int i = 0; i < state.Contacts.Count; i++)
                {
                    var cState = state.Contacts[i];
                    if (cState != null)
                    {
                        _activeContacts.Add(new RadarContact(cState));
                    }
                }
            }
        }
    }
}
