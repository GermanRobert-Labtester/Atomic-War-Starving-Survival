// FactionPressureHUD.cs — Presentation-only widget for the four Expansion II
// faction-pressure systems. Mirrors BunkerMaintenanceHUD's pattern: a
// serializable FactionPressureSnapshot the host fills in, plus an
// OnFactionPressureChanged event the controller subscribes to. The widget
// never mutates the four systems itself; it just formats the snapshot for
// the diegetic panel.
using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Snapshot of the four Expansion II faction-pressure systems. The host
    /// (FactionPressureWiring + GameBootstrap) builds one of these on every
    /// refresh; the HUD reads it and repaints. Snake_case fields stay
    /// uppercase in display (lore style: "GARRISON", "MILITIA", etc.).
    /// </summary>
    [Serializable]
    public class FactionPressureSnapshot
    {
        // Garrison
        public string GarrisonShelterStatus = "COMPLIANT";   // COMPLIANT | NON-COMPLIANT | REINSTATED
        public int GarrisonStrikes;
        public int GarrisonWeeksUntilReinstated;

        // Militia
        public float MilitiaTaxRate;                          // 0..1
        public bool MilitiaProtectionWithdrawn;
        public int MilitiaRefusalStreak;

        // Cult
        public int CultVisitCount;
        public bool CultBlessed;
        public bool CultUnderProtection;
        public int CultCommunionMisses;

        // Warlord
        public float WarlordTributeRequired;
        public int WarlordShortWeeks;
        public bool WarlordShelterBurned;

        public FactionPressureSnapshot Clone()
        {
            return new FactionPressureSnapshot
            {
                GarrisonShelterStatus = GarrisonShelterStatus,
                GarrisonStrikes = GarrisonStrikes,
                GarrisonWeeksUntilReinstated = GarrisonWeeksUntilReinstated,
                MilitiaTaxRate = MilitiaTaxRate,
                MilitiaProtectionWithdrawn = MilitiaProtectionWithdrawn,
                MilitiaRefusalStreak = MilitiaRefusalStreak,
                CultVisitCount = CultVisitCount,
                CultBlessed = CultBlessed,
                CultUnderProtection = CultUnderProtection,
                CultCommunionMisses = CultCommunionMisses,
                WarlordTributeRequired = WarlordTributeRequired,
                WarlordShortWeeks = WarlordShortWeeks,
                WarlordShelterBurned = WarlordShelterBurned
            };
        }
    }

    /// <summary>
    /// Presentation-only widget for the faction-pressure panel. Subscribes
    /// nothing; the controller calls Refresh() and listens to the changed
    /// event to repaint the panel.
    /// </summary>
    public class FactionPressureHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;

        /// <summary>Fired after every Refresh; the controller repaints on this signal.</summary>
        public event Action OnFactionPressureChanged;

        private Func<FactionPressureSnapshot> _getSnapshot;
        private FactionPressureSnapshot _snapshot = new FactionPressureSnapshot();

        /// <summary>Host calls this once at boot. Optional default to a fresh snapshot.</summary>
        public void Bind(Func<FactionPressureSnapshot> getSnapshot)
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

        /// <summary>Refresh: pull the latest snapshot, format the panel body, raise the event.</summary>
        public void Refresh()
        {
            if (_getSnapshot != null)
            {
                var fresh = _getSnapshot();
                if (fresh != null) _snapshot = fresh;
            }
            PanelSummary = FormatBody(_snapshot);
            OnFactionPressureChanged?.Invoke();
        }

        /// <summary>Capture the current snapshot for save/load.</summary>
        public FactionPressureSnapshot Capture()
        {
            return _snapshot != null ? _snapshot.Clone() : new FactionPressureSnapshot();
        }

        /// <summary>Restore a saved snapshot (does not raise the changed event).</summary>
        public void Restore(FactionPressureSnapshot saved)
        {
            if (saved == null) saved = new FactionPressureSnapshot();
            _snapshot = saved.Clone();
            PanelSummary = FormatBody(_snapshot);
        }

        // Build the 4-line body: GARRISON, MILITIA, CULT, WARLORD. UPPERCASE
        // labels per the Figma HUD spec ("Compliant. Patrolled. Tithed. Fed.
        // Or not.").
        public static string FormatBody(FactionPressureSnapshot s)
        {
            if (s == null) s = new FactionPressureSnapshot();
            var sb = new StringBuilder();
            sb.Append("GARRISON: STRIKES ").Append(s.GarrisonStrikes).Append("/3 ")
              .Append(string.IsNullOrEmpty(s.GarrisonShelterStatus) ? "COMPLIANT" : s.GarrisonShelterStatus.ToUpperInvariant());
            if (s.GarrisonWeeksUntilReinstated > 0)
            {
                sb.Append(" (REINSTATE ").Append(s.GarrisonWeeksUntilReinstated).Append("W)");
            }
            sb.Append("\nMILITIA: ").Append(Mathf.RoundToInt(s.MilitiaTaxRate * 100f)).Append("% TITHE ")
              .Append(s.MilitiaProtectionWithdrawn ? "UNPROTECTED" : "PROTECTED");
            if (s.MilitiaRefusalStreak > 0)
            {
                sb.Append(" REFUSED ").Append(s.MilitiaRefusalStreak).Append("W");
            }
            sb.Append("\nCULT: ").Append(s.CultVisitCount).Append(" VISITS ")
              .Append(s.CultBlessed ? "BLESSED" : (s.CultUnderProtection ? "UNDER PROTECTION" : "UNBLESSED"));
            if (s.CultCommunionMisses > 0)
            {
                sb.Append(" MISSED ").Append(s.CultCommunionMisses).Append("W");
            }
            sb.Append("\nWARLORD: ").Append(s.WarlordTributeRequired.ToString("0.0")).Append(" UNITS");
            if (s.WarlordShortWeeks > 0)
            {
                sb.Append(" ESCALATED ").Append(s.WarlordShortWeeks).Append("x");
            }
            if (s.WarlordShelterBurned)
            {
                sb.Append(" BURNED");
            }
            return sb.ToString();
        }
    }
}
