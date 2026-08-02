using System.Collections.Generic;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI
{
    /// <summary>
    /// Pure C# system driving Utility AI decision ticks across all active survivors at set time intervals.
    /// </summary>
    public class UtilityAISystem
    {
        private readonly UtilityAIContext _context;
        private readonly List<UtilityActionSO> _defaultActionPool;
        private readonly Dictionary<string, UtilityAIBrain> _survivorBrains = new Dictionary<string, UtilityAIBrain>();

        private float _evaluationIntervalSeconds = 3.0f;
        private float _timer = 0f;

        public UtilityAISystem(UtilityAIContext context, IEnumerable<UtilityActionSO> defaultActions, float evaluationInterval = 3.0f)
        {
            _context = context;
            _defaultActionPool = new List<UtilityActionSO>(defaultActions);
            _evaluationIntervalSeconds = evaluationInterval;
        }

        public void Tick(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer >= _evaluationIntervalSeconds)
            {
                _timer -= _evaluationIntervalSeconds;
                EvaluateAllSurvivors();
            }
        }

        public void EvaluateAllSurvivors()
        {
            if (_context.SurvivorSystem == null) return;

            foreach (var survivor in _context.SurvivorSystem.GetLivingSurvivors())
            {
                var brain = GetOrCreateBrain(survivor);
                brain.EvaluateAndExecute(survivor, _context);
            }
        }

        private UtilityAIBrain GetOrCreateBrain(SurvivorModel survivor)
        {
            if (!_survivorBrains.TryGetValue(survivor.InstanceId, out var brain))
            {
                brain = new UtilityAIBrain(_defaultActionPool);
                _survivorBrains.Add(survivor.InstanceId, brain);
            }
            return brain;
        }
    }
}
