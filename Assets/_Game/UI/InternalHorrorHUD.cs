using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Player surfaces for Internal Horror bunker mechanics:
    /// corpse dispose (bury / fertilizer), fire fight/seal, coma care urgency,
    /// and contaminated-food stock icon. Data-only view-model — Core wires
    /// callbacks and pushes snapshots. Tone: cold, restrained.
    /// </summary>
    public class InternalHorrorHUD : MonoBehaviour
    {
        // ── Corpse dispose ──────────────────────────────────────────────
        public int CorpseCount { get; private set; }
        public bool CanBury { get; private set; }
        public bool CanProcessFertilizer { get; private set; }
        public string CorpseStatusLine { get; private set; } = string.Empty;
        public string CorpseDetailLine { get; private set; } = string.Empty;
        public bool IsCorpsePanelOpen { get; private set; }

        // ── Fire alert ──────────────────────────────────────────────────
        public bool HasActiveFire { get; private set; }
        public string FireAlertLine { get; private set; } = string.Empty;
        public string FireDetailLine { get; private set; } = string.Empty;
        public bool IsFirePanelOpen { get; private set; }
        public string ActiveFireRoomId { get; private set; }
        public IReadOnlyList<FireRoomView> FireRooms => _fireRooms;

        // ── Status icons ────────────────────────────────────────────────
        public int ComaPatientCount { get; private set; }
        public bool ComaCareUrgent { get; private set; }
        public string ComaStatusLine { get; private set; } = string.Empty;
        public int ContaminatedFoodCount { get; private set; }
        public string ContaminatedFoodLine { get; private set; } = string.Empty;
        /// <summary>Compact strip for the main HUD (icons + short labels).</summary>
        public string StatusIconStrip { get; private set; } = string.Empty;
        /// <summary>Full multi-line status for debug / expanded panel.</summary>
        public string DetailSummary { get; private set; } = string.Empty;

        /// <summary>Hotkey line when corpse dispose panel is open.</summary>
        public string CorpsePanelChoicesLine { get; private set; } = string.Empty;
        /// <summary>Hotkey line when fire panel is open.</summary>
        public string FirePanelChoicesLine { get; private set; } = string.Empty;

        // ── Diegetic pings (journal-style; optional audio hooks listen) ──
        /// <summary>True after a new blaze until the fire panel is opened.</summary>
        public bool FireNotificationPing { get; private set; }
        /// <summary>True when care becomes overdue until urgency clears or ack.</summary>
        public bool CareNotificationPing { get; private set; }
        /// <summary>Last rising-edge ping (for audio / tests).</summary>
        public HorrorPingKind LastPing { get; private set; } = HorrorPingKind.None;
        public int FirePingCount { get; private set; }
        public int CarePingCount { get; private set; }

        private readonly List<FireRoomView> _fireRooms = new List<FireRoomView>();
        private readonly List<ComaPatientView> _comas = new List<ComaPatientView>();
        private int _prevFireRoomCount;
        private bool _prevComaUrgent;

        public event Action OnBuryRequested;
        public event Action OnProcessFertilizerRequested;
        public event Action<string> OnFightFireRequested;
        public event Action<string> OnSealBulkheadRequested;
        public event Action OnStateChanged;
        /// <summary>Rising-edge ping: fire started or care went overdue. Audio may hook this.</summary>
        public event Action<HorrorPingKind> OnHorrorPing;

        // ── Snapshot push ───────────────────────────────────────────────

        /// <summary>
        /// Push a pure data snapshot from the host. Safe to call every game-hour
        /// or on inventory/medical/atmosphere change.
        /// </summary>
        public void ApplySnapshot(InternalHorrorSnapshot snap)
        {
            if (snap == null)
            {
                Clear();
                return;
            }

            CorpseCount = Mathf.Max(0, snap.CorpseCount);
            CanBury = snap.CanBury && CorpseCount > 0;
            CanProcessFertilizer = CorpseCount > 0;
            CorpseStatusLine = InternalHorrorHudFormatter.FormatCorpseStatus(CorpseCount);
            CorpseDetailLine = InternalHorrorHudFormatter.FormatCorpseDetail(
                CorpseCount, CanBury, snap.DaylightHoursAvailable);

            _fireRooms.Clear();
            if (snap.Fires != null)
            {
                for (int i = 0; i < snap.Fires.Length; i++)
                {
                    var f = snap.Fires[i];
                    if (f == null || string.IsNullOrEmpty(f.RoomId) || !f.IsOnFire) continue;
                    _fireRooms.Add(new FireRoomView
                    {
                        RoomId = f.RoomId,
                        Intensity = f.Intensity,
                        OxygenFraction = f.OxygenFraction,
                        LocalCoPpm = f.LocalCoPpm,
                        BulkheadSealed = f.BulkheadSealed,
                        StatusLine = InternalHorrorHudFormatter.FormatFireRoom(
                            f.RoomId, f.Intensity, f.OxygenFraction, f.BulkheadSealed)
                    });
                }
            }
            HasActiveFire = _fireRooms.Count > 0;
            if (HasActiveFire)
            {
                if (string.IsNullOrEmpty(ActiveFireRoomId)
                    || FindFire(ActiveFireRoomId) == null)
                {
                    ActiveFireRoomId = _fireRooms[0].RoomId;
                }
            }
            else
            {
                ActiveFireRoomId = null;
                IsFirePanelOpen = false;
                FireNotificationPing = false;
            }
            FireAlertLine = InternalHorrorHudFormatter.FormatFireAlert(_fireRooms.Count);
            FireDetailLine = InternalHorrorHudFormatter.FormatFireDetail(_fireRooms, ActiveFireRoomId);

            _comas.Clear();
            float worstSinceCare = 0f;
            if (snap.Comas != null)
            {
                for (int i = 0; i < snap.Comas.Length; i++)
                {
                    var c = snap.Comas[i];
                    if (c == null || string.IsNullOrEmpty(c.SurvivorId)) continue;
                    _comas.Add(new ComaPatientView
                    {
                        SurvivorId = c.SurvivorId,
                        DisplayName = c.DisplayName ?? c.SurvivorId,
                        HoursSinceLastCare = c.HoursSinceLastCare,
                        NeedsCare = c.NeedsCare
                    });
                    if (c.HoursSinceLastCare > worstSinceCare)
                        worstSinceCare = c.HoursSinceLastCare;
                }
            }
            ComaPatientCount = _comas.Count;
            ComaCareUrgent = snap.ComaCareUrgent || HasUrgentComa(snap.CareIntervalHours);
            ComaStatusLine = InternalHorrorHudFormatter.FormatComaStatus(
                ComaPatientCount, worstSinceCare, snap.CareIntervalHours, ComaCareUrgent);
            if (!ComaCareUrgent)
                CareNotificationPing = false;

            ContaminatedFoodCount = Mathf.Max(0, snap.ContaminatedFoodCount);
            ContaminatedFoodLine = InternalHorrorHudFormatter.FormatContaminatedFood(
                ContaminatedFoodCount);

            // Rising-edge diegetic pings (fire / overdue care).
            RaisePingsIfNeeded(_fireRooms.Count, ComaCareUrgent);

            StatusIconStrip = InternalHorrorHudFormatter.BuildIconStrip(
                CorpseCount, _fireRooms.Count, ComaPatientCount, ComaCareUrgent,
                ContaminatedFoodCount, FireNotificationPing, CareNotificationPing);
            RebuildPanelChoiceLines();
            DetailSummary = InternalHorrorHudFormatter.BuildDetailSummary(
                CorpseStatusLine, CorpseDetailLine, FireAlertLine, FireDetailLine,
                ComaStatusLine, ContaminatedFoodLine,
                IsCorpsePanelOpen ? CorpsePanelChoicesLine : null,
                IsFirePanelOpen ? FirePanelChoicesLine : null);

            OnStateChanged?.Invoke();
        }

        public void Clear()
        {
            CorpseCount = 0;
            CanBury = false;
            CanProcessFertilizer = false;
            CorpseStatusLine = string.Empty;
            CorpseDetailLine = string.Empty;
            IsCorpsePanelOpen = false;
            CorpsePanelChoicesLine = string.Empty;
            HasActiveFire = false;
            FireAlertLine = string.Empty;
            FireDetailLine = string.Empty;
            IsFirePanelOpen = false;
            FirePanelChoicesLine = string.Empty;
            ActiveFireRoomId = null;
            _fireRooms.Clear();
            ComaPatientCount = 0;
            ComaCareUrgent = false;
            ComaStatusLine = string.Empty;
            _comas.Clear();
            ContaminatedFoodCount = 0;
            ContaminatedFoodLine = string.Empty;
            StatusIconStrip = string.Empty;
            DetailSummary = string.Empty;
            FireNotificationPing = false;
            CareNotificationPing = false;
            LastPing = HorrorPingKind.None;
            _prevFireRoomCount = 0;
            _prevComaUrgent = false;
            OnStateChanged?.Invoke();
        }

        // ── Corpse panel actions ────────────────────────────────────────

        public void OpenCorpsePanel()
        {
            if (CorpseCount <= 0) return;
            IsCorpsePanelOpen = true;
            RebuildPanelChoiceLines();
            OnStateChanged?.Invoke();
        }

        public void CloseCorpsePanel()
        {
            IsCorpsePanelOpen = false;
            CorpsePanelChoicesLine = string.Empty;
            OnStateChanged?.Invoke();
        }

        /// <summary>Player chose bury or fertilizer from the corpse dispose UI.</summary>
        public bool SelectCorpseDispose(CorpseDisposeChoice choice)
        {
            if (CorpseCount <= 0) return false;
            switch (choice)
            {
                case CorpseDisposeChoice.Bury:
                    if (!CanBury) return false;
                    OnBuryRequested?.Invoke();
                    return true;
                case CorpseDisposeChoice.ProcessFertilizer:
                    OnProcessFertilizerRequested?.Invoke();
                    return true;
                default:
                    return false;
            }
        }

        // ── Fire panel actions ──────────────────────────────────────────

        public void OpenFirePanel(string roomId = null)
        {
            if (!HasActiveFire) return;
            if (!string.IsNullOrEmpty(roomId) && FindFire(roomId) != null)
                ActiveFireRoomId = roomId;
            IsFirePanelOpen = true;
            FireNotificationPing = false;
            FireDetailLine = InternalHorrorHudFormatter.FormatFireDetail(_fireRooms, ActiveFireRoomId);
            RebuildPanelChoiceLines();
            OnStateChanged?.Invoke();
        }

        public void CloseFirePanel()
        {
            IsFirePanelOpen = false;
            FirePanelChoicesLine = string.Empty;
            OnStateChanged?.Invoke();
        }

        /// <summary>Clear overdue-care ping (player acknowledged strip).</summary>
        public void AcknowledgeCarePing()
        {
            if (!CareNotificationPing) return;
            CareNotificationPing = false;
            StatusIconStrip = InternalHorrorHudFormatter.BuildIconStrip(
                CorpseCount, _fireRooms.Count, ComaPatientCount, ComaCareUrgent,
                ContaminatedFoodCount, FireNotificationPing, CareNotificationPing);
            OnStateChanged?.Invoke();
        }

        public void SelectFireRoom(string roomId)
        {
            if (FindFire(roomId) == null) return;
            ActiveFireRoomId = roomId;
            FireDetailLine = InternalHorrorHudFormatter.FormatFireDetail(_fireRooms, ActiveFireRoomId);
            OnStateChanged?.Invoke();
        }

        /// <summary>Fight the fire in the active (or specified) room — burn risk.</summary>
        public bool SelectFightFire(string roomId = null)
        {
            string id = !string.IsNullOrEmpty(roomId) ? roomId : ActiveFireRoomId;
            if (FindFire(id) == null) return false;
            OnFightFireRequested?.Invoke(id);
            return true;
        }

        /// <summary>Seal bulkhead — starve fire, sacrifice modules inside.</summary>
        public bool SelectSealBulkhead(string roomId = null)
        {
            string id = !string.IsNullOrEmpty(roomId) ? roomId : ActiveFireRoomId;
            if (FindFire(id) == null) return false;
            OnSealBulkheadRequested?.Invoke(id);
            return true;
        }

        private FireRoomView FindFire(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return null;
            for (int i = 0; i < _fireRooms.Count; i++)
            {
                if (_fireRooms[i] != null && _fireRooms[i].RoomId == roomId)
                    return _fireRooms[i];
            }
            return null;
        }

        private bool HasUrgentComa(float careInterval)
        {
            float threshold = careInterval > 0f ? careInterval * 0.5f : 3f;
            for (int i = 0; i < _comas.Count; i++)
            {
                if (_comas[i] != null && (_comas[i].NeedsCare || _comas[i].HoursSinceLastCare >= threshold))
                    return true;
            }
            return false;
        }

        private void RaisePingsIfNeeded(int fireCount, bool careUrgent)
        {
            if (fireCount > _prevFireRoomCount)
            {
                FireNotificationPing = true;
                FirePingCount++;
                LastPing = HorrorPingKind.Fire;
                OnHorrorPing?.Invoke(HorrorPingKind.Fire);
            }
            if (careUrgent && !_prevComaUrgent)
            {
                CareNotificationPing = true;
                CarePingCount++;
                LastPing = HorrorPingKind.CareUrgent;
                OnHorrorPing?.Invoke(HorrorPingKind.CareUrgent);
            }
            _prevFireRoomCount = fireCount;
            _prevComaUrgent = careUrgent;
        }

        private void RebuildPanelChoiceLines()
        {
            CorpsePanelChoicesLine = IsCorpsePanelOpen
                ? InternalHorrorHudFormatter.FormatCorpsePanelChoices(CanBury, CanProcessFertilizer)
                : string.Empty;
            FirePanelChoicesLine = IsFirePanelOpen
                ? InternalHorrorHudFormatter.FormatFirePanelChoices()
                : string.Empty;
        }
    }

    public enum CorpseDisposeChoice
    {
        Bury,
        ProcessFertilizer
    }

    /// <summary>Diegetic Internal Horror alert kinds (optional audio listens).</summary>
    public enum HorrorPingKind
    {
        None = 0,
        Fire = 1,
        CareUrgent = 2
    }

    /// <summary>Host-built snapshot of Internal Horror state for the HUD.</summary>
    public class InternalHorrorSnapshot
    {
        public int CorpseCount;
        public bool CanBury;
        public float DaylightHoursAvailable;
        public FireRoomSnapshot[] Fires;
        public ComaPatientSnapshot[] Comas;
        public bool ComaCareUrgent;
        public float CareIntervalHours = 6f;
        public int ContaminatedFoodCount;
    }

    public class FireRoomSnapshot
    {
        public string RoomId;
        public bool IsOnFire;
        public float Intensity;
        public float OxygenFraction;
        public float LocalCoPpm;
        public bool BulkheadSealed;
    }

    public class ComaPatientSnapshot
    {
        public string SurvivorId;
        public string DisplayName;
        public float HoursSinceLastCare;
        public bool NeedsCare;
    }

    public class FireRoomView
    {
        public string RoomId;
        public float Intensity;
        public float OxygenFraction;
        public float LocalCoPpm;
        public bool BulkheadSealed;
        public string StatusLine;
    }

    public class ComaPatientView
    {
        public string SurvivorId;
        public string DisplayName;
        public float HoursSinceLastCare;
        public bool NeedsCare;
    }

    /// <summary>
    /// Pure string formatting for Internal Horror HUD. No MonoBehaviour deps —
    /// unit-testable without a scene.
    /// </summary>
    public static class InternalHorrorHudFormatter
    {
        public static string FormatCorpseStatus(int count)
        {
            if (count <= 0) return string.Empty;
            if (count == 1) return "BODY in stores";
            return $"BODIES ×{count}";
        }

        public static string FormatCorpseDetail(int count, bool canBury, float daylightHours)
        {
            if (count <= 0) return string.Empty;
            var sb = new StringBuilder();
            sb.Append("It won't keep. ");
            if (canBury)
                sb.Append("Bury outside (4h light).");
            else if (daylightHours < 4f)
                sb.Append("Not enough light to bury.");
            else
                sb.Append("Cannot bury.");
            sb.Append(" Or process for fertilizer.");
            return sb.ToString();
        }

        public static string FormatFireAlert(int roomCount)
        {
            if (roomCount <= 0) return string.Empty;
            if (roomCount == 1) return "FIRE — air going";
            return $"FIRE — {roomCount} rooms";
        }

        public static string FormatFireRoom(
            string roomId, float intensity, float o2, bool bulkheadSealed)
        {
            string name = string.IsNullOrEmpty(roomId) ? "?" : roomId;
            string seal = bulkheadSealed ? " SEALED" : "";
            return $"{name}: burn {intensity:0%}  O₂ {o2:0.000}{seal}";
        }

        public static string FormatFireDetail(
            IReadOnlyList<FireRoomView> rooms, string activeRoomId)
        {
            if (rooms == null || rooms.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            sb.Append("Fight it (burns) or seal the bulkhead (lose what's inside).");
            for (int i = 0; i < rooms.Count; i++)
            {
                var r = rooms[i];
                if (r == null) continue;
                sb.Append('\n');
                if (r.RoomId == activeRoomId) sb.Append("> ");
                else sb.Append("  ");
                sb.Append(r.StatusLine ?? r.RoomId);
            }
            return sb.ToString();
        }

        public static string FormatComaStatus(
            int count, float worstHoursSinceCare, float careInterval, bool urgent)
        {
            if (count <= 0) return string.Empty;
            string who = count == 1 ? "1 bedridden" : $"{count} bedridden";
            if (urgent)
                return $"{who} — care overdue";
            if (careInterval > 0f && worstHoursSinceCare >= careInterval * 0.5f)
                return $"{who} — needs water";
            return who;
        }

        public static string FormatContaminatedFood(int count)
        {
            if (count <= 0) return string.Empty;
            if (count == 1) return "1 rusted can";
            return $"{count} rusted cans";
        }

        public static string FormatCorpsePanelChoices(bool canBury, bool canFertilizer)
        {
            // Cold, restrained. Numbers match PlayerInputHandler when panel open.
            string bury = canBury ? "[1] Bury outside (4h light)" : "[1] Bury — not enough light";
            string fert = canFertilizer ? "[2] Process for fertilizer" : "[2] —";
            return $"{bury}   {fert}   [Esc] close";
        }

        public static string FormatFirePanelChoices()
        {
            return "[1] Fight it (burns)   [2] Seal bulkhead   [Esc] close";
        }

        public static string BuildIconStrip(
            int corpses, int fires, int comas, bool comaUrgent, int contaminated,
            bool firePing = false, bool carePing = false)
        {
            var parts = new List<string>(5);
            if (fires > 0)
            {
                // Unacked blaze uses [FIRE!] (same urgency mark as care).
                if (firePing)
                    parts.Add(fires == 1 ? "[FIRE!]" : $"[FIRE!×{fires}]");
                else
                    parts.Add(fires == 1 ? "[FIRE]" : $"[FIRE×{fires}]");
            }
            if (corpses > 0) parts.Add(corpses == 1 ? "[BODY]" : $"[BODY×{corpses}]");
            if (comas > 0)
                parts.Add(comaUrgent || carePing ? "[CARE!]" : "[CARE]");
            if (contaminated > 0)
                parts.Add(contaminated == 1 ? "[RUST]" : $"[RUST×{contaminated}]");
            if (parts.Count == 0) return string.Empty;
            return string.Join(" ", parts);
        }

        public static string BuildDetailSummary(
            string corpseStatus, string corpseDetail,
            string fireAlert, string fireDetail,
            string comaStatus, string contamLine,
            string corpseChoices = null, string fireChoices = null)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(fireAlert))
            {
                sb.Append(fireAlert);
                if (!string.IsNullOrEmpty(fireDetail))
                    sb.Append('\n').Append(fireDetail);
                if (!string.IsNullOrEmpty(fireChoices))
                    sb.Append('\n').Append(fireChoices);
            }
            if (!string.IsNullOrEmpty(corpseStatus))
            {
                if (sb.Length > 0) sb.Append('\n');
                sb.Append(corpseStatus);
                if (!string.IsNullOrEmpty(corpseDetail))
                    sb.Append('\n').Append(corpseDetail);
                if (!string.IsNullOrEmpty(corpseChoices))
                    sb.Append('\n').Append(corpseChoices);
            }
            if (!string.IsNullOrEmpty(comaStatus))
            {
                if (sb.Length > 0) sb.Append('\n');
                sb.Append(comaStatus);
            }
            if (!string.IsNullOrEmpty(contamLine))
            {
                if (sb.Length > 0) sb.Append('\n');
                sb.Append(contamLine);
            }
            return sb.ToString();
        }
    }
}
