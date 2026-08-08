using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #124 — Lateral tunneling into adjacent basements (brownstone layout).</summary>
    public class TunnelingSystem
    {
        public const string TunnelingToolItemId = "pickaxe";
        public const float TunnelHoursPerTile = 8f;
        public const float TunnelFatiguePerHour = 10f;

        private bool _neighborBreached;
        private bool _neighborLooted;
        private bool _neighborHasHostiles;
        private bool _hostilesCleared;
        private float _tunnelProgress;
        private ShelterPerkSystem _shelterPerks;
        private System.Random _rng = new System.Random(124);

        public bool CanTunnel => true; // Gated by layout trait
        public bool NeighborBreached => _neighborBreached;
        public bool NeighborLooted => _neighborLooted;
        public bool HasHostiles => _neighborHasHostiles && !_hostilesCleared;
        public float TunnelProgress => _tunnelProgress;

        public event Action OnNeighborBreached;
        public event Action OnHostilesCleared;
        public event Action OnNeighborLooted;
        public event Action OnCaveInWhileDigging;

        public TunnelingSystem() { }

        /// <summary>Prompt #199 — Sandhog fatigue / cave-in immunity while tunneling.</summary>
        public void BindShelterPerks(ShelterPerkSystem perks, System.Random rng = null)
        {
            _shelterPerks = perks;
            if (rng != null) _rng = rng;
        }

        /// <summary>Seed neighbor state based on layout.</summary>
        public void SeedNeighbor(System.Random rng)
        {
            _neighborHasHostiles = (rng?.NextDouble() ?? 0.5) < 0.4f;
        }

        public float Tunnel(float workHours, Survivors.Survivor worker, bool hasPickaxe)
        {
            if (_neighborBreached) return 0f;
            if (worker == null || !worker.IsAlive) return 0f;
            float mult = hasPickaxe ? 1.5f : 0.5f;
            _tunnelProgress += workHours * mult / TunnelHoursPerTile;
            float fatMult = _shelterPerks != null
                ? _shelterPerks.GetExcavationFatigueMultiplier(worker)
                : 1f;
            worker.Needs.Fatigue = Mathf.Clamp(
                worker.Needs.Fatigue + TunnelFatiguePerHour * workHours * fatMult, 0f, 100f);

            // Cave-in risk while tunneling (Sandhog never triggers).
            if (_shelterPerks != null)
            {
                if (_shelterPerks.RollDigCaveIn(worker, _rng))
                    OnCaveInWhileDigging?.Invoke();
            }
            else if (_rng.NextDouble() < ShelterPerkSystem.BaseExcavationCaveInChance)
            {
                OnCaveInWhileDigging?.Invoke();
            }

            if (_tunnelProgress >= 1f && !_neighborBreached)
            { _neighborBreached = true; OnNeighborBreached?.Invoke(); }
            return _tunnelProgress;
        }

        public void ClearHostiles(Survivors.Survivor fighter)
        {
            if (!_neighborHasHostiles || _hostilesCleared) return;
            _hostilesCleared = true;
            if (fighter != null) SurvivorNeedWrite.AdjustHealth(fighter, -15f);
            OnHostilesCleared?.Invoke();
        }

        public void LootNeighbor()
        {
            if (_neighborLooted || (_neighborHasHostiles && !_hostilesCleared)) return;
            _neighborLooted = true;
            OnNeighborLooted?.Invoke();
        }

        public TunnelingSave CaptureState() => new TunnelingSave
        {
            NeighborBreached = _neighborBreached, NeighborLooted = _neighborLooted,
            NeighborHasHostiles = _neighborHasHostiles, HostilesCleared = _hostilesCleared, TunnelProgress = _tunnelProgress
        };
        public void RestoreState(TunnelingSave s)
        {
            if (s == null) return;
            _neighborBreached = s.NeighborBreached; _neighborLooted = s.NeighborLooted;
            _neighborHasHostiles = s.NeighborHasHostiles; _hostilesCleared = s.HostilesCleared; _tunnelProgress = s.TunnelProgress;
        }
    }
    [Serializable] public class TunnelingSave
    {
        public bool NeighborBreached, NeighborLooted, NeighborHasHostiles, HostilesCleared;
        public float TunnelProgress;
    }
}
