using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Legacy wrapper for air filtration unit.
    /// Delegates to shelter modules and maintains serializable compatibility.
    /// </summary>
    [System.Serializable]
    public class AirFiltration
    {
        private Shelter _shelter;
        private float _fallbackHealth = 100f;
        private bool _fallbackPowered = true;

        public float FilterHealth
        {
            get
            {
                var mod = _shelter?.GetModule("air_filtration");
                return mod != null ? mod.FilterHealth : _fallbackHealth;
            }
            set
            {
                _fallbackHealth = value;
                var mod = _shelter?.GetModule("air_filtration");
                if (mod != null) mod.FilterHealth = value;
            }
        }

        public bool IsPowered
        {
            get
            {
                var mod = _shelter?.GetModule("air_filtration");
                return mod != null ? mod.IsEnabled : _fallbackPowered;
            }
            set
            {
                _fallbackPowered = value;
                var mod = _shelter?.GetModule("air_filtration");
                if (mod != null) mod.IsEnabled = value;
            }
        }

        public float FiltrationEfficiency => Mathf.Clamp01(FilterHealth / 100f);

        public void BindShelter(Shelter shelter)
        {
            _shelter = shelter;
        }

        public void Tick(float gameHours)
        {
            if (_shelter != null)
            {
                var mod = _shelter.GetModule("air_filtration");
                mod?.Tick(gameHours, _shelter);
            }
            else
            {
                FilterHealth = Mathf.Max(0f, FilterHealth - 2f * gameHours);
            }
        }

        public void ReplaceFilter()
        {
            FilterHealth = 100f;
            var mod = _shelter?.GetModule("air_filtration");
            if (mod != null)
            {
                mod.ReplaceFilter();
            }
        }
    }
}
