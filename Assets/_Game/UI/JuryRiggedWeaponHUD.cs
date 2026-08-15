#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class JuryWeaponSnapshot
    {
        public string weaponId;
        public string weaponName;
        public float durabilityPercent; // 0..100
        public float jamProbabilityPercent;
        public bool isJammed;
        public int scrapRepairCost;
    }

    public class JuryRiggedWeaponSnapshot
    {
        public int totalWeaponsMaintained;
        public List<JuryWeaponSnapshot> weapons = new List<JuryWeaponSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Jury-Rigged Weapon Degradation & Jam HUD view-model.
    /// Monitors improvised wasteland firearms (pipe rifles, scrap shotguns),
    /// weapon jam probabilities, scrap metal field repairs, and clearing firing pin jams.
    /// </summary>
    public class JuryRiggedWeaponHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedWeaponIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnJuryRiggedWeaponChanged;
        public event Action<string> OnRepairWeaponRequested; // (weaponId)
        public event Action<string> OnClearJamRequested; // (weaponId)

        private Func<JuryRiggedWeaponSnapshot> _getSnapshot;
        private JuryRiggedWeaponSnapshot _snapshot;

        public void Bind(Func<JuryRiggedWeaponSnapshot> getSnapshot)
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

        public bool SelectNextWeapon()
        {
            if (!IsOpen || _snapshot == null || _snapshot.weapons == null || _snapshot.weapons.Count == 0)
                return false;
            SelectedWeaponIndex = (SelectedWeaponIndex + 1) % _snapshot.weapons.Count;
            ReportOutcome("Selected firearm: " + GetSelectedWeaponName());
            return true;
        }

        public bool SelectPreviousWeapon()
        {
            if (!IsOpen || _snapshot == null || _snapshot.weapons == null || _snapshot.weapons.Count == 0)
                return false;
            SelectedWeaponIndex = (SelectedWeaponIndex - 1 + _snapshot.weapons.Count) % _snapshot.weapons.Count;
            ReportOutcome("Selected firearm: " + GetSelectedWeaponName());
            return true;
        }

        public bool RequestRepairWeapon()
        {
            if (!IsOpen || _snapshot == null || _snapshot.weapons == null || _snapshot.weapons.Count == 0)
            {
                ReportOutcome("No firearm selected for scrap repair.");
                return false;
            }

            var wpn = GetSelectedWeapon();
            if (wpn == null) return false;

            if (OnRepairWeaponRequested == null)
            {
                ReportOutcome("Armory workbench link offline.");
                return false;
            }

            OnRepairWeaponRequested.Invoke(wpn.weaponId);
            ReportOutcome("Repairing firearm [" + wpn.weaponName + "] with " + wpn.scrapRepairCost + " Scrap Metal...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No weapon action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnJuryRiggedWeaponChanged?.Invoke();
        }

        private JuryWeaponSnapshot GetSelectedWeapon()
        {
            if (_snapshot != null && _snapshot.weapons != null && SelectedWeaponIndex >= 0 && SelectedWeaponIndex < _snapshot.weapons.Count)
            {
                return _snapshot.weapons[SelectedWeaponIndex];
            }
            return null;
        }

        private string GetSelectedWeaponName()
        {
            var w = GetSelectedWeapon();
            return w != null ? w.weaponName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("JURY-RIGGED FIREARM DEGRADATION & JAM MONITOR  [W] close  ·  [Tab] cycle  ·  [R] repair with scrap");

            if (_snapshot == null)
            {
                sb.Append("\nArmory weapon telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nARMORY STATS: Total Firearm Maintenance: ").Append(_snapshot.totalWeaponsMaintained);

            sb.Append("\n\nSHELTER IMPROVISED FIREARMS:");
            if (_snapshot.weapons == null || _snapshot.weapons.Count == 0)
            {
                sb.Append("\n  No jury-rigged firearms registered.");
            }
            else
            {
                for (int i = 0; i < _snapshot.weapons.Count; i++)
                {
                    var wpn = _snapshot.weapons[i];
                    if (wpn == null) continue;

                    bool selected = (i == SelectedWeaponIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(wpn.weaponName ?? wpn.weaponId)
                      .Append(" — Condition: ").Append(wpn.durabilityPercent.ToString("0")).Append("%")
                      .Append(" | Jam Chance: ").Append(wpn.jamProbabilityPercent.ToString("0")).Append("%")
                      .Append(" | Scrap Cost: ").Append(wpn.scrapRepairCost);

                    if (wpn.isJammed) sb.Append("  ✖ [JAMMED — NEEDS BOLT CLEARING]");
                    else sb.Append("  ✔ [FUNCTIONAL]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nARMORY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
