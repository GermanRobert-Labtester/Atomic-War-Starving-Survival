using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LogicRule
    {
        public string ruleId;
        public string conditionModule;
        public string conditionOperator; // "gt", "lt", "eq", "gte", "lte"
        public float conditionValue;
        public string actionModule;
        public string actionCommand;
    }

    [Serializable]
    public class LogicGatesState
    {
        public string systemId = "system_logic_gates";
        public List<LogicRule> rules = new List<LogicRule>();
    }

    /// <summary>
    /// Prompt #799: Logic Gates (Grid Routing).
    /// Wire modules together with IF/THEN rules. Saves power through automation.
    /// </summary>
    public class System_LogicGates
    {
        public event Action<string> OnRuleTriggered;  // ruleId
        public event Action<string> OnRuleAdded;      // ruleId
        public event Action<string> OnRuleRemoved;    // ruleId

        private LogicGatesState _state;

        public System_LogicGates(LogicGatesState state = null)
        {
            _state = state ?? new LogicGatesState();
        }

        public string SystemId => _state.systemId;

        public void AddRule(string ruleId, LogicRule rule)
        {
            if (string.IsNullOrEmpty(ruleId) || rule == null)
            {
                Debug.LogWarning("[System_LogicGates] AddRule called with null ruleId or rule.");
                return;
            }

            rule.ruleId = ruleId;
            // Remove existing rule with same id if present
            _state.rules.RemoveAll(r => r.ruleId == ruleId);
            _state.rules.Add(rule);
            OnRuleAdded?.Invoke(ruleId);
        }

        public void RemoveRule(string ruleId)
        {
            if (string.IsNullOrEmpty(ruleId))
            {
                Debug.LogWarning("[System_LogicGates] RemoveRule called with null/empty ruleId.");
                return;
            }

            int removed = _state.rules.RemoveAll(r => r.ruleId == ruleId);
            if (removed > 0)
            {
                OnRuleRemoved?.Invoke(ruleId);
            }
        }

        /// <summary>
        /// Evaluates all rules against current module states and executes matching actions.
        /// moduleStates maps moduleId -> current float value.
        /// Returns list of triggered rule IDs.
        /// </summary>
        public List<string> EvaluateRules(Dictionary<string, float> moduleStates)
        {
            List<string> triggered = new List<string>();

            if (moduleStates == null)
            {
                Debug.LogWarning("[System_LogicGates] EvaluateRules called with null moduleStates.");
                return triggered;
            }

            foreach (LogicRule rule in _state.rules)
            {
                if (rule == null || string.IsNullOrEmpty(rule.conditionModule))
                    continue;

                float moduleValue;
                if (!moduleStates.TryGetValue(rule.conditionModule, out moduleValue))
                    continue;

                bool conditionMet = EvaluateCondition(moduleValue, rule.conditionOperator, rule.conditionValue);

                if (conditionMet)
                {
                    triggered.Add(rule.ruleId);
                    OnRuleTriggered?.Invoke(rule.ruleId);
                }
            }

            return triggered;
        }

        private bool EvaluateCondition(float actual, string op, float threshold)
        {
            switch (op)
            {
                case "gt":  return actual > threshold;
                case "lt":  return actual < threshold;
                case "eq":  return Mathf.Approximately(actual, threshold);
                case "gte": return actual >= threshold;
                case "lte": return actual <= threshold;
                default:
                    Debug.LogWarning($"[System_LogicGates] Unknown operator: {op}");
                    return false;
            }
        }

        public int GetRuleCount() => _state.rules.Count;

        public LogicGatesState CaptureState()
        {
            var captured = new LogicGatesState
            {
                systemId = _state.systemId,
                rules = new List<LogicRule>()
            };

            foreach (var rule in _state.rules)
            {
                captured.rules.Add(new LogicRule
                {
                    ruleId = rule.ruleId,
                    conditionModule = rule.conditionModule,
                    conditionOperator = rule.conditionOperator,
                    conditionValue = rule.conditionValue,
                    actionModule = rule.actionModule,
                    actionCommand = rule.actionCommand
                });
            }

            return captured;
        }

        public void RestoreState(LogicGatesState state)
        {
            _state = state ?? new LogicGatesState();
        }
    }
}
