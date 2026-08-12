using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class RoomAcousticSnapshot
    {
        public string roomId;
        public float baseNoiseDb;
        public bool hasFoam;
        public bool isConcrete;
        public bool isSecretSafe;
        public bool isIncineratorDetected;
    }

    public class AcousticDecoySnapshot
    {
        public string decoyId;
        public string roomId;
        public float remainingMinutes;
    }

    public class AcousticEcologySnapshot
    {
        public List<RoomAcousticSnapshot> roomProfiles = new List<RoomAcousticSnapshot>();
        public List<AcousticDecoySnapshot> activeDecoys = new List<AcousticDecoySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Acoustic Ecology HUD view-model.
    /// Monitors room noise profiles, concrete reverberation, acoustic foam panel installation
    /// for soundproofing, secret meeting safety isolation, and acoustic decoy deployments.
    /// </summary>
    public class AcousticEcologyHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedRoomIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnAcousticEcologyChanged;
        public event Action<string> OnInstallFoamRequested; // (roomId)
        public event Action<string> OnDeployDecoyRequested; // (roomId)

        private Func<AcousticEcologySnapshot> _getSnapshot;
        private AcousticEcologySnapshot _snapshot;

        public void Bind(Func<AcousticEcologySnapshot> getSnapshot)
        {
            _getSnapshot = getSnapshot;
            Refresh();
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public bool SelectNextRoom()
        {
            if (!IsOpen || _snapshot == null || _snapshot.roomProfiles == null || _snapshot.roomProfiles.Count == 0)
                return false;
            SelectedRoomIndex = (SelectedRoomIndex + 1) % _snapshot.roomProfiles.Count;
            ReportOutcome("Selected room acoustic profile: " + GetSelectedRoomName());
            return true;
        }

        public bool SelectPreviousRoom()
        {
            if (!IsOpen || _snapshot == null || _snapshot.roomProfiles == null || _snapshot.roomProfiles.Count == 0)
                return false;
            SelectedRoomIndex = (SelectedRoomIndex - 1 + _snapshot.roomProfiles.Count) % _snapshot.roomProfiles.Count;
            ReportOutcome("Selected room acoustic profile: " + GetSelectedRoomName());
            return true;
        }

        public bool RequestInstallFoam()
        {
            if (!IsOpen || _snapshot == null || _snapshot.roomProfiles == null || _snapshot.roomProfiles.Count == 0)
            {
                ReportOutcome("No room selected for acoustic foam installation.");
                return false;
            }

            var room = GetSelectedRoom();
            if (room == null) return false;

            if (room.hasFoam)
            {
                ReportOutcome("Room " + room.roomId + " already has soundproofing foam installed.");
                return false;
            }

            if (OnInstallFoamRequested == null)
            {
                ReportOutcome("Acoustic installation service link offline.");
                return false;
            }

            OnInstallFoamRequested.Invoke(room.roomId);
            ReportOutcome("Installing acoustic foam panels in " + room.roomId + "...");
            return true;
        }

        public bool RequestDeployDecoy()
        {
            if (!IsOpen || _snapshot == null || _snapshot.roomProfiles == null || _snapshot.roomProfiles.Count == 0)
            {
                ReportOutcome("No room selected for acoustic decoy deployment.");
                return false;
            }

            var room = GetSelectedRoom();
            if (room == null) return false;

            if (OnDeployDecoyRequested == null)
            {
                ReportOutcome("Decoy deployment launcher link offline.");
                return false;
            }

            OnDeployDecoyRequested.Invoke(room.roomId);
            ReportOutcome("Deploying acoustic wind-up decoy in " + room.roomId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No acoustic action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnAcousticEcologyChanged?.Invoke();
        }

        private RoomAcousticSnapshot GetSelectedRoom()
        {
            if (_snapshot != null && _snapshot.roomProfiles != null && SelectedRoomIndex >= 0 && SelectedRoomIndex < _snapshot.roomProfiles.Count)
            {
                return _snapshot.roomProfiles[SelectedRoomIndex];
            }
            return null;
        }

        private string GetSelectedRoomName()
        {
            var r = GetSelectedRoom();
            return r != null ? r.roomId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("ACOUSTIC ECOLOGY & SOUNDPROOFING  [A] close  ·  [Tab] cycle  ·  [F] foam panel  ·  [D] acoustic decoy");

            if (_snapshot == null)
            {
                sb.Append("\nAcoustic telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nACTIVE ACOUSTIC DECOYS: ").Append(_snapshot.activeDecoys?.Count ?? 0);

            sb.Append("\n\nSHELTER ROOM ACOUSTIC PROFILES:");
            if (_snapshot.roomProfiles == null || _snapshot.roomProfiles.Count == 0)
            {
                sb.Append("\n  No rooms registered in acoustic monitoring grid.");
            }
            else
            {
                for (int i = 0; i < _snapshot.roomProfiles.Count; i++)
                {
                    var room = _snapshot.roomProfiles[i];
                    if (room == null) continue;

                    bool selected = (i == SelectedRoomIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Room ").Append(room.roomId)
                      .Append(" — Base Noise: ").Append(room.baseNoiseDb.ToString("0.#")).Append(" dB")
                      .Append(" | Foam: ").Append(room.hasFoam ? "[INSTALLED -85% NOISE]" : "[NONE]");

                    if (room.isConcrete) sb.Append(" (Concrete Reverb)");
                    if (room.isSecretSafe) sb.Append(" ★ [SECRET SAFE]");
                    if (room.isIncineratorDetected) sb.Append("  [WARNING: INCINERATOR NOISE DETECTED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nACOUSTIC LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
