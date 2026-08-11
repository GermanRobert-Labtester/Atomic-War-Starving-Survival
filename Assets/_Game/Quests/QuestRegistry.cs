using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// Registry of all new content quests (Faction, Personal, Shelter).
    /// Owns the runtime map and save state. Host wires it up at boot.
    /// </summary>
    public class QuestRegistry
    {
        public const string IdGarrisonLastOrder = Quest_GarrisonLastOrder.Id;
        public const string IdMilitiaGrainWar = Quest_MilitiaGrainWar.Id;
        public const string IdCultGlowCommunion = Quest_CultGlowCommunion.Id;
        public const string IdElenaTriage = Quest_ElenaTriage.Id;
        public const string IdMechanicHighwayHeart = Quest_MechanicHighwayHeart.Id;
        public const string IdChildSoldierRifle = Quest_ChildSoldierRifle.Id;
        public const string IdDeepWell = Quest_DeepWell.Id;

        [Serializable]
        public class State
        {
            public List<QuestRuntimeState> States = new List<QuestRuntimeState>();
        }

        private State _state = new State();
        public State Current => _state;

        private readonly Dictionary<string, QuestRuntime> _byId = new Dictionary<string, QuestRuntime>();

        public event Action<QuestRuntimeState> OnQuestStateChanged;

        public QuestRegistry()
        {
            _byId[IdGarrisonLastOrder] = new Quest_GarrisonLastOrder();
            _byId[IdMilitiaGrainWar] = new Quest_MilitiaGrainWar();
            _byId[IdCultGlowCommunion] = new Quest_CultGlowCommunion();
            _byId[IdElenaTriage] = new Quest_ElenaTriage();
            _byId[IdMechanicHighwayHeart] = new Quest_MechanicHighwayHeart();
            _byId[IdChildSoldierRifle] = new Quest_ChildSoldierRifle();
            _byId[IdDeepWell] = new Quest_DeepWell();

            foreach (var kvp in _byId)
            {
                var rt = kvp.Value;
                rt.OnStageChanged += (state, stage) => OnQuestStateChanged?.Invoke(state);
                rt.OnStatusChanged += (state, status) => OnQuestStateChanged?.Invoke(state);
            }
        }

        public QuestRuntime Get(string id) => string.IsNullOrEmpty(id) || !_byId.TryGetValue(id, out var q) ? null : q;
        public T Get<T>(string id) where T : QuestRuntime => Get(id) as T;
        public IEnumerable<QuestRuntime> All => _byId.Values;

        public QuestRuntimeState GetOrCreateState(string id)
        {
            var q = Get(id);
            if (q == null) return null;
            if (q.State == null)
            {
                q.State = new QuestRuntimeState { QuestId = id };
                _state.States.Add(q.State);
            }
            return q.State;
        }

        public bool Start(string id, int currentDay)
        {
            var q = Get(id);
            if (q == null) return false;
            if (q.State != null && q.State.Status != QuestStatus.NotStarted) return false;
            GetOrCreateState(id);
            q.Start(currentDay);
            return true;
        }

        public State CaptureState() => _state;

        public void RestoreState(State s)
        {
            _state = s ?? new State();
            // Reattach states to live runtimes.
            for (int i = 0; i < _state.States.Count; i++)
            {
                var qs = _state.States[i];
                if (qs == null) continue;
                var rt = Get(qs.QuestId);
                if (rt == null) continue;
                rt.State = qs;
            }
        }
    }
}
