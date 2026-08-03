using System.Text;
using UnityEngine;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Hatch security panel: aggregate ShelterSecurity, weapon power, guards,
    /// and last raid outcome. Data-driven from <see cref="HatchDefenseSystem"/>.
    /// </summary>
    public class HatchDefenseHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }

        public float ShelterSecurity { get; private set; }
        public float WeaponPower { get; private set; }
        public float DefenseScore { get; private set; }
        public int ActiveGuards { get; private set; }
        public bool RaidUnlocked { get; private set; }
        public bool GeneratorOutside { get; private set; }
        public string StatusLine { get; private set; } = "Hatch: —";
        public string UpgradesLine { get; private set; } = string.Empty;
        public string LastRaidLine { get; private set; } = "Last raid: none";
        public string DetailSummary { get; private set; } = string.Empty;

        private HatchDefenseSystem _hatch;
        private int _day;

        public void Bind(HatchDefenseSystem hatch)
        {
            if (_hatch != null)
            {
                _hatch.OnRaidResolved -= OnRaid;
                _hatch.OnSecurityChanged -= Refresh;
            }

            _hatch = hatch;
            if (_hatch != null)
            {
                _hatch.OnRaidResolved += OnRaid;
                _hatch.OnSecurityChanged += Refresh;
            }

            Refresh();
        }

        public void SetDay(int day)
        {
            _day = day;
            Refresh();
        }

        public void Open()
        {
            IsOpen = true;
            Refresh();
        }

        public void Close() => IsOpen = false;

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        private void OnRaid(RaidResolution _)
        {
            Refresh();
        }

        public void Refresh()
        {
            if (_hatch == null)
            {
                ShelterSecurity = WeaponPower = DefenseScore = 0f;
                ActiveGuards = 0;
                RaidUnlocked = false;
                GeneratorOutside = false;
                StatusLine = "Hatch: offline";
                UpgradesLine = string.Empty;
                LastRaidLine = "Last raid: none";
                DetailSummary = "No hatch defense system.";
                return;
            }

            ShelterSecurity = _hatch.GetShelterSecurity();
            WeaponPower = _hatch.GetWeaponPower();
            DefenseScore = ShelterSecurity + WeaponPower;
            ActiveGuards = _hatch.ActiveGuardCount;
            RaidUnlocked = _hatch.IsRaidUnlocked(_day > 0 ? _day : -1);
            GeneratorOutside = _hatch.GeneratorRunningOutside
                || _hatch.ExternalNoise >= HatchDefenseSystem.ExternalGeneratorNoiseThreshold;

            string threat = !RaidUnlocked
                ? "SECURE (pre-Day 30)"
                : GeneratorOutside
                    ? "NOISY — drawn fire risk"
                    : "WATCHING";

            StatusLine = $"HATCH [H]  DEF {DefenseScore:0}  (sec {ShelterSecurity:0} + arms {WeaponPower:0})  [{threat}]";
            if (ActiveGuards > 0)
                StatusLine += $"  guards×{ActiveGuards}";

            LastRaidLine = "Last: " + (_hatch.LastRaidSummary ?? "Hatch quiet.");
            if (_hatch.TotalRaidsResolved > 0)
                LastRaidLine += $"  (raids {_hatch.TotalRaidsResolved}, breaches {_hatch.TotalBreaches})";

            var up = new StringBuilder();
            up.Append("Upgrades: ");
            bool any = false;
            for (int i = 0; i < HatchDefenseSystem.HatchModuleIds.Length; i++)
            {
                string id = HatchDefenseSystem.HatchModuleIds[i];
                float contrib = HatchDefenseSystem.DefaultSecurityForModuleId(id);
                string label = id == HatchDefenseModuleSO.BlastDoorId ? "blast door"
                    : id == HatchDefenseModuleSO.HatchTrapsId ? "traps"
                    : id == HatchDefenseModuleSO.ReinforcedLocksId ? "locks"
                    : id;
                if (any) up.Append(" · ");
                up.Append($"{label}(+{contrib:0}/lvl)");
                any = true;
            }
            UpgradesLine = up.ToString();

            var detail = new StringBuilder();
            detail.AppendLine(StatusLine);
            detail.AppendLine(LastRaidLine);
            detail.AppendLine(UpgradesLine);
            detail.AppendLine("Install / upgrade at workbench [B] (scrap + mechanical parts).");
            if (GeneratorOutside)
                detail.AppendLine("Outdoor generator is drawing attention.");
            if (ActiveGuards > 0)
                detail.AppendLine($"Guard post active (+{HatchDefenseSystem.GuardSecurityBonusPerGuard * ActiveGuards:0} security).");
            DetailSummary = detail.ToString().TrimEnd();
        }

        private void OnDestroy()
        {
            if (_hatch != null)
            {
                _hatch.OnRaidResolved -= OnRaid;
                _hatch.OnSecurityChanged -= Refresh;
            }
        }
    }
}
