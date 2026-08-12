using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IV — The War of the Airwaves. The radio is not just for listening.
    /// It is a weapon. Using the ShelterModule_RadioArray and generator power,
    /// the_radio_host or the_tech_bro can broadcast localized spoofed signals.
    /// This costs massive fuel and risks triangulation by Siege_Artillery.
    /// Extends PropagandaSystem with active signal hijacking capabilities.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class SignalHijackSystem
    {
        // ── Broadcast types ───────────────────────────────────────────
        public enum HijackType
        {
            DecoyBroadcast,      // Lure factions to fake coordinates
            GhostTape,           // Psychological warfare with dead survivor's voice
            NumbersInjection     // Inject false cipher into Numbers Station
        }

        // ── Constants ─────────────────────────────────────────────────
        public const string RadioArrayModuleId = "radio_array";
        public const float DecoyFuelCost = 15f;           // liters
        public const float GhostTapeFuelCost = 5f;
        public const float NumbersInjectionFuelCost = 10f;
        public const float TriangulationRisk = 0.25f;     // 25% chance per broadcast
        public const float DecoySuccessRate = 0.70f;      // 70% chance to divert raid
        public const float GhostTapeFalterChance = 0.60f; // 60% chance raiders falter
        public const string CassetteTapeItemId = "cassette_tape";
        public const string SignalSplicerItemId = "signal_splicer";
        public const string VacuumTubeItemId = "vacuum_tube";
        public const string CopperWireItemId = "copper_wire_10m_of_10m";

        // ── Triangulation consequences ────────────────────────────────
        public const string SiegeArtilleryEvent = "siege_artillery";

        // ── Events ────────────────────────────────────────────────────
        public event Action<HijackType, string> OnHijackBroadcast;   // type, broadcasterId
        public event Action<string> OnTriangulationDetected;         // broadcasterId
        public event Action<string> OnRadioArrayDestroyed;
        public event Action<string, string> OnGhostTapePlayed;       // tapeId, deadSurvivorName
        public event Action<string> OnFamilyGriefCascade;            // familyMemberId
        public event Action<string> OnNumbersStationInjected;        // injectorId
        public event Action<string, bool> OnDecoyResult;             // factionId, success

        private readonly System.Random _rng;
        private bool _radioArrayDestroyed;
        private readonly List<HijackRecord> _broadcastHistory = new List<HijackRecord>();
        private int _totalDecoysSent;
        private int _totalRaidsDiverted;
        private int _estimatedCasualties;

        public bool IsRadioArrayDestroyed => _radioArrayDestroyed;
        public int TotalDecoysSent => _totalDecoysSent;
        public int TotalRaidsDiverted => _totalRaidsDiverted;
        public int EstimatedCasualties => _estimatedCasualties;
        public IReadOnlyList<HijackRecord> BroadcastHistory => _broadcastHistory;

        public SignalHijackSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(5555);
        }

        // ── Decoy Broadcast ───────────────────────────────────────────

        /// <summary>
        /// Broadcast a looping "Supply Cache Confirmed" signal on a faction frequency,
        /// shifted 2km east into a hazard zone. Next raid is diverted.
        /// </summary>
        public DecoyResult SendDecoyBroadcast(
            string broadcasterId,
            string targetFactionId,
            string hazardZoneId,
            Func<float, bool> consumeFuel,
            Func<string, bool> hasRadioArray)
        {
            if (_radioArrayDestroyed) return new DecoyResult { Success = false, ArrayDestroyed = true };
            if (hasRadioArray != null && !hasRadioArray(RadioArrayModuleId))
                return new DecoyResult { Success = false };

            if (consumeFuel != null && !consumeFuel(DecoyFuelCost))
                return new DecoyResult { Success = false, InsufficientFuel = true };

            bool triangulated = _rng.NextDouble() < TriangulationRisk;
            if (triangulated)
            {
                _radioArrayDestroyed = true;
                OnTriangulationDetected?.Invoke(broadcasterId);
                OnRadioArrayDestroyed?.Invoke(RadioArrayModuleId);
                RecordBroadcast(HijackType.DecoyBroadcast, broadcasterId, targetFactionId, true);
                return new DecoyResult
                {
                    Success = false,
                    ArrayDestroyed = true,
                    Message = "The antenna array is gone. They found you."
                };
            }

            bool diverted = _rng.NextDouble() < DecoySuccessRate;
            if (diverted)
            {
                _totalDecoysSent++;
                _totalRaidsDiverted++;
                _estimatedCasualties += _rng.Next(5, 25);
            }

            OnDecoyResult?.Invoke(targetFactionId, diverted);
            RecordBroadcast(HijackType.DecoyBroadcast, broadcasterId, targetFactionId, false);

            return new DecoyResult
            {
                Success = diverted,
                FactionDiverted = diverted,
                HazardZoneId = hazardZoneId,
                Message = diverted
                    ? "The patrol changed course. They're walking into the ash."
                    : "The frequency was ignored. They're still coming."
            };
        }

        // ── Ghost Tape ────────────────────────────────────────────────

        /// <summary>
        /// Record a dying survivor's final breaths on cassette_tape.
        /// Later broadcast during a night raid to break raider morale.
        /// </summary>
        public bool RecordGhostTape(string deadSurvivorName, string deadSurvivorId,
            Func<string, bool> hasItem)
        {
            if (hasItem != null && !hasItem(CassetteTapeItemId)) return false;
            OnGhostTapePlayed?.Invoke(CassetteTapeItemId, deadSurvivorName);
            return true;
        }

        /// <summary>
        /// Broadcast the ghost tape during a raid. Raiders falter.
        /// Family members suffer GriefCascade.
        /// </summary>
        public GhostTapeResult BroadcastGhostTape(
            string broadcasterId,
            string deadSurvivorName,
            string familyMemberId,
            Func<float, bool> consumeFuel)
        {
            if (consumeFuel != null && !consumeFuel(GhostTapeFuelCost))
                return new GhostTapeResult { Success = false };

            bool falter = _rng.NextDouble() < GhostTapeFalterChance;

            // Family always suffers
            if (!string.IsNullOrEmpty(familyMemberId))
                OnFamilyGriefCascade?.Invoke(familyMemberId);

            RecordBroadcast(HijackType.GhostTape, broadcasterId, "local", false);

            return new GhostTapeResult
            {
                Success = true,
                RaidersFaltered = falter,
                FamilyMemberAffected = familyMemberId,
                Message = falter
                    ? "The raiders heard the dead. They are running."
                    : "The tape played. The raiders didn't care. The family did."
            };
        }

        // ── Numbers Station Injection ─────────────────────────────────

        /// <summary>
        /// Inject a false cipher block into the automated Numbers Station (99.0 FM).
        /// Can trick cultists or garrison remnants.
        /// </summary>
        public NumbersInjectionResult InjectNumbersStation(
            string injectorId,
            string targetGroupId,
            string falseCommand,
            Func<float, bool> consumeFuel,
            Func<string, bool> hasItem)
        {
            if (_radioArrayDestroyed)
                return new NumbersInjectionResult { Success = false, ArrayDestroyed = true };

            if (hasItem != null && !hasItem(SignalSplicerItemId))
                return new NumbersInjectionResult { Success = false, MissingItem = SignalSplicerItemId };

            if (consumeFuel != null && !consumeFuel(NumbersInjectionFuelCost))
                return new NumbersInjectionResult { Success = false, InsufficientFuel = true };

            bool triangulated = _rng.NextDouble() < TriangulationRisk * 0.5f; // Lower risk
            if (triangulated)
            {
                _radioArrayDestroyed = true;
                OnTriangulationDetected?.Invoke(injectorId);
                OnRadioArrayDestroyed?.Invoke(RadioArrayModuleId);
                return new NumbersInjectionResult { Success = false, ArrayDestroyed = true };
            }

            OnNumbersStationInjected?.Invoke(injectorId);
            RecordBroadcast(HijackType.NumbersInjection, injectorId, targetGroupId, false);

            return new NumbersInjectionResult
            {
                Success = true,
                TargetGroupId = targetGroupId,
                FalseCommand = falseCommand,
                Message = "The cipher block was accepted. They believe the numbers."
            };
        }

        // ── Record keeping ────────────────────────────────────────────

        private void RecordBroadcast(HijackType type, string broadcasterId,
            string targetId, bool arrayDestroyed)
        {
            _broadcastHistory.Add(new HijackRecord
            {
                Type = type,
                BroadcasterId = broadcasterId,
                TargetId = targetId,
                ArrayDestroyed = arrayDestroyed,
                DayRecorded = 0 // Host provides current day
            });
        }

        public void MarkRadioArrayDestroyed()
        {
            _radioArrayDestroyed = true;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public SignalHijackSave CaptureState()
        {
            var history = new HijackRecordSave[_broadcastHistory.Count];
            for (int i = 0; i < _broadcastHistory.Count; i++)
            {
                var r = _broadcastHistory[i];
                history[i] = new HijackRecordSave
                {
                    Type = r.Type,
                    BroadcasterId = r.BroadcasterId,
                    TargetId = r.TargetId,
                    ArrayDestroyed = r.ArrayDestroyed,
                    DayRecorded = r.DayRecorded
                };
            }
            return new SignalHijackSave
            {
                RadioArrayDestroyed = _radioArrayDestroyed,
                TotalDecoysSent = _totalDecoysSent,
                TotalRaidsDiverted = _totalRaidsDiverted,
                EstimatedCasualties = _estimatedCasualties,
                BroadcastHistory = history
            };
        }

        public void RestoreState(SignalHijackSave save)
        {
            _broadcastHistory.Clear();
            _radioArrayDestroyed = false;
            _totalDecoysSent = 0;
            _totalRaidsDiverted = 0;
            _estimatedCasualties = 0;
            if (save == null) return;
            _radioArrayDestroyed = save.RadioArrayDestroyed;
            _totalDecoysSent = save.TotalDecoysSent;
            _totalRaidsDiverted = save.TotalRaidsDiverted;
            _estimatedCasualties = save.EstimatedCasualties;
            if (save.BroadcastHistory != null)
            {
                for (int i = 0; i < save.BroadcastHistory.Length; i++)
                {
                    var r = save.BroadcastHistory[i];
                    if (r == null) continue;
                    _broadcastHistory.Add(new HijackRecord
                    {
                        Type = r.Type,
                        BroadcasterId = r.BroadcasterId,
                        TargetId = r.TargetId,
                        ArrayDestroyed = r.ArrayDestroyed,
                        DayRecorded = r.DayRecorded
                    });
                }
            }
        }
    }

    // ── Result types ──────────────────────────────────────────────────

    [Serializable]
    public class DecoyResult
    {
        public bool Success;
        public bool FactionDiverted;
        public bool ArrayDestroyed;
        public bool InsufficientFuel;
        public string HazardZoneId;
        public string Message;
    }

    [Serializable]
    public class GhostTapeResult
    {
        public bool Success;
        public bool RaidersFaltered;
        public string FamilyMemberAffected;
        public string Message;
    }

    [Serializable]
    public class NumbersInjectionResult
    {
        public bool Success;
        public bool ArrayDestroyed;
        public bool InsufficientFuel;
        public string MissingItem;
        public string TargetGroupId;
        public string FalseCommand;
        public string Message;
    }

    public class HijackRecord
    {
        public SignalHijackSystem.HijackType Type;
        public string BroadcasterId;
        public string TargetId;
        public bool ArrayDestroyed;
        public int DayRecorded;
    }

    [Serializable]
    public class SignalHijackSave
    {
        public bool RadioArrayDestroyed;
        public int TotalDecoysSent;
        public int TotalRaidsDiverted;
        public int EstimatedCasualties;
        public HijackRecordSave[] BroadcastHistory;
    }

    [Serializable]
    public class HijackRecordSave
    {
        public SignalHijackSystem.HijackType Type;
        public string BroadcasterId;
        public string TargetId;
        public bool ArrayDestroyed;
        public int DayRecorded;
    }
}
