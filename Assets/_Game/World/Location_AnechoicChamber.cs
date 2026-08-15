using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion VIII — Location: The Anechoic Chamber. A pre-war research facility
    /// designed to absorb 99.99% of all acoustic reflections. The quietest place
    /// in the wasteland. You can hear your own blood pumping.
    /// Cures NightTerrors permanently. But the silence forces confrontation with
    /// your own thoughts — low Morale survivors suffer Catatonia.
    /// </summary>
    public class Location_AnechoicChamber
    {
        public const string LocationId = "location_anechoic_chamber";
        public const string DisplayName = "The Anechoic Chamber";
        public const int TravelHours = 2;
        public const int DangerLevel = 4;
        public const float BaseRads = 8f;

        public const string Item_AcousticFoam = "acoustic_foam_panel";
        public const int FoamYieldPerVisit = 4;
        public const float CatatoniaThresholdHours = 2f;
        public const float MoraleThresholdForCatatonia = 40f;

        public event Action<string> OnNightTerrorCured;
        public event Action<string> OnCatatoniaTriggered;
        public event Action<string, int> OnFoamCollected;

        private readonly System.Random _rng;
        private int _foamCollected;
        private int _visitsCount;
        private readonly HashSet<string> _nightTerrorCured = new HashSet<string>();

        public int FoamCollected => _foamCollected;

        public Location_AnechoicChamber(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(13000);
        }

        public AnechoicResult EnterChamber(string survivorId, float survivorMorale,
            bool hasPTSD, bool hasHyperacusis, float hoursSpent)
        {
            _visitsCount++;
            var result = new AnechoicResult { Success = true };

            // Cure NightTerrors for PTSD/Hyperacusis survivors
            if ((hasPTSD || hasHyperacusis) && !_nightTerrorCured.Contains(survivorId))
            {
                _nightTerrorCured.Add(survivorId);
                result.NightTerrorCured = true;
                OnNightTerrorCured?.Invoke(survivorId);
            }

            // Catatonia check for low-morale survivors
            if (survivorMorale < MoraleThresholdForCatatonia && hoursSpent >= CatatoniaThresholdHours)
            {
                result.CatatoniaTriggered = true;
                OnCatatoniaTriggered?.Invoke(survivorId);
            }

            return result;
        }

        public int CollectFoam(string survivorId)
        {
            int yield = FoamYieldPerVisit;
            _foamCollected += yield;
            OnFoamCollected?.Invoke(survivorId, yield);
            return yield;
        }

        public AnechoicChamberSave CaptureState()
        {
            var cured = new string[_nightTerrorCured.Count];
            _nightTerrorCured.CopyTo(cured);
            return new AnechoicChamberSave
            {
                FoamCollected = _foamCollected,
                VisitsCount = _visitsCount,
                NightTerrorCured = cured
            };
        }

        public void RestoreState(AnechoicChamberSave save)
        {
            _foamCollected = 0;
            _visitsCount = 0;
            _nightTerrorCured.Clear();
            if (save == null) return;
            _foamCollected = save.FoamCollected;
            _visitsCount = save.VisitsCount;
            if (save.NightTerrorCured != null)
                for (int i = 0; i < save.NightTerrorCured.Length; i++)
                    if (!string.IsNullOrEmpty(save.NightTerrorCured[i]))
                        _nightTerrorCured.Add(save.NightTerrorCured[i]);
        }
    }

    [Serializable]
    public class AnechoicResult
    {
        public bool Success;
        public bool NightTerrorCured;
        public bool CatatoniaTriggered;
    }

    [Serializable]
    public class AnechoicChamberSave
    {
        public int FoamCollected;
        public int VisitsCount;
        public string[] NightTerrorCured;
    }
}
