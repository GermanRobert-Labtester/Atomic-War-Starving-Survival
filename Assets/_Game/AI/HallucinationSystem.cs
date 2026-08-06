using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    [Serializable]
    public class PhantomItem
    {
        public string Id;
        public string DisplayName;
        public string TargetSurvivorId;
        public float LifetimeHours;

        public PhantomItem() { }

        public PhantomItem(string id, string displayName, string survivorId, float lifetimeHours = 24f)
        {
            Id = id;
            DisplayName = displayName;
            TargetSurvivorId = survivorId;
            LifetimeHours = lifetimeHours;
        }
    }

    public class HallucinationSystem
    {
        private readonly List<PhantomItem> _activePhantoms = new List<PhantomItem>();

        public event Action<Survivor, PhantomItem> OnPhantomSpawned;
        public event Action<Survivor, PhantomItem> OnPhantomConsumed; // 0 benefit

        public IReadOnlyList<PhantomItem> ActivePhantoms => _activePhantoms;

        public List<PhantomItem> GetPhantomsForSurvivor(string survivorId)
        {
            var list = new List<PhantomItem>();
            if (string.IsNullOrEmpty(survivorId)) return list;
            for (int i = 0; i < _activePhantoms.Count; i++)
            {
                if (_activePhantoms[i].TargetSurvivorId == survivorId)
                {
                    list.Add(_activePhantoms[i]);
                }
            }
            return list;
        }

        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors, System.Random rng)
        {
            if (gameHours <= 0f || survivors == null) return;
            if (rng == null) rng = new System.Random();

            // Decay phantom lifetimes
            for (int i = _activePhantoms.Count - 1; i >= 0; i--)
            {
                _activePhantoms[i].LifetimeHours -= gameHours;
                if (_activePhantoms[i].LifetimeHours <= 0f)
                {
                    _activePhantoms.RemoveAt(i);
                }
            }

            // Spawn phantoms for low health or high anxiety
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                bool triggers = (sv.Needs != null && sv.Needs.Health < 30f) || sv.RadiationAnxiety > 0.7f;
                if (triggers && rng.NextDouble() < 0.15f * (gameHours / 24f))
                {
                    SpawnPhantomForSurvivor(sv, rng);
                }
            }
        }

        public PhantomItem SpawnPhantomForSurvivor(Survivor sv, System.Random rng)
        {
            if (sv == null) return null;
            if (rng == null) rng = new System.Random();

            string[] phantomNames = { "Clean Water", "Canned Beef", "Fresh Pill Bottle", "Gas Mask Filter" };
            string name = phantomNames[rng.Next(phantomNames.Length)];
            string id = "phantom_" + sv.Id + "_" + rng.Next(1000, 9999);

            var item = new PhantomItem(id, name, sv.Id);
            _activePhantoms.Add(item);

            OnPhantomSpawned?.Invoke(sv, item);
            return item;
        }

        public bool ConsumePhantomItem(Survivor sv, string phantomId)
        {
            if (sv == null || string.IsNullOrEmpty(phantomId)) return false;

            for (int i = 0; i < _activePhantoms.Count; i++)
            {
                if (_activePhantoms[i].Id == phantomId && _activePhantoms[i].TargetSurvivorId == sv.Id)
                {
                    var item = _activePhantoms[i];
                    _activePhantoms.RemoveAt(i);

                    // Zero benefit applied to survivor (hydration/morale unchanged)
                    OnPhantomConsumed?.Invoke(sv, item);
                    return true;
                }
            }
            return false;
        }

        // -----------------------------------------------------------------
        // Save / Load (audit wiring fix)
        // -----------------------------------------------------------------
        public HallucinationSave CaptureState()
        {
            var items = new PhantomItemSave[_activePhantoms.Count];
            for (int i = 0; i < _activePhantoms.Count; i++)
            {
                var p = _activePhantoms[i];
                items[i] = new PhantomItemSave
                {
                    Id = p.Id,
                    DisplayName = p.DisplayName,
                    TargetSurvivorId = p.TargetSurvivorId,
                    LifetimeHours = p.LifetimeHours
                };
            }
            return new HallucinationSave { ActivePhantoms = items };
        }

        public void RestoreState(HallucinationSave save)
        {
            _activePhantoms.Clear();
            if (save?.ActivePhantoms == null) return;
            for (int i = 0; i < save.ActivePhantoms.Length; i++)
            {
                var s = save.ActivePhantoms[i];
                if (s == null || string.IsNullOrEmpty(s.Id)) continue;
                _activePhantoms.Add(new PhantomItem(s.Id, s.DisplayName, s.TargetSurvivorId, s.LifetimeHours));
            }
        }
    }

    [Serializable]
    public class HallucinationSave
    {
        public PhantomItemSave[] ActivePhantoms;
    }

    [Serializable]
    public class PhantomItemSave
    {
        public string Id;
        public string DisplayName;
        public string TargetSurvivorId;
        public float LifetimeHours;
    }
}
