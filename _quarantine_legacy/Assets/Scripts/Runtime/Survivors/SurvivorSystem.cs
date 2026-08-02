using System.Collections.Generic;
using System.Linq;
using AtomicWar.Core.Events;
using AtomicWar.Data;
using UnityEngine;

namespace AtomicWar.Runtime.Survivors
{
    public struct SurvivorStateChangedEvent
    {
        public SurvivorModel Survivor;
    }

    public struct SurvivorDiedEvent
    {
        public SurvivorModel Survivor;
        public string CauseOfDeath;
    }

    /// <summary>
    /// Pure C# system managing active survivors in the shelter.
    /// </summary>
    public class SurvivorSystem
    {
        private readonly List<SurvivorModel> _survivors = new List<SurvivorModel>();
        public IReadOnlyList<SurvivorModel> ActiveSurvivors => _survivors;

        public void AddSurvivor(SurvivorData data)
        {
            var survivor = new SurvivorModel(data);
            _survivors.Add(survivor);
            Debug.Log($"[SurvivorSystem] Added survivor: {data.CharacterName}");
            EventBus.Raise(new SurvivorStateChangedEvent { Survivor = survivor });
        }

        public void RemoveSurvivor(SurvivorModel survivor, string cause)
        {
            if (_survivors.Remove(survivor))
            {
                survivor.CurrentState = SurvivorState.Dead;
                Debug.LogWarning($"[SurvivorSystem] Survivor died ({cause}): {survivor.Data.CharacterName}");
                EventBus.Raise(new SurvivorDiedEvent { Survivor = survivor, CauseOfDeath = cause });
            }
        }

        public SurvivorModel GetSurvivorById(string instanceId)
        {
            return _survivors.FirstOrDefault(s => s.InstanceId == instanceId);
        }

        public IEnumerable<SurvivorModel> GetLivingSurvivors()
        {
            return _survivors.Where(s => s.IsAlive);
        }
    }
}
