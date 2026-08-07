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
    /// Host supplies module float states and executes action commands.
    /// </summary>
    public class System_LogicGates
    {
        /// <summary>Known action commands the host understands.</summary>
        public const string CmdEnable = "enable";
        public const string CmdDisable = "disable";
        public const string CmdOn = "on";
        public const string CmdOff = "off";
        public const string CmdRequest = "request";
        public const string CmdUnrequest = "unrequest";
        public const string CmdSourceOn = "source_on";
        public const string CmdSourceOff = "source_off";

        public event Action<string> OnRuleTriggered;  // ruleId
        public event Action<string> OnRuleAdded;      // ruleId
        public event Action<string> OnRuleRemoved;    // ruleId

        private LogicGatesState _state;

        public System_LogicGates(LogicGatesState state = null)
        {
            _state = state != null ? CloneState(state) : new LogicGatesState();
            if (string.IsNullOrEmpty(_state.systemId))
                _state.systemId = "system_logic_gates";
            if (_state.rules == null)
                _state.rules = new List<LogicRule>();
        }

        public string SystemId => _state.systemId;
        public IReadOnlyList<LogicRule> Rules => _state.rules;
        public int GetRuleCount() => _state.rules != null ? _state.rules.Count : 0;

        public LogicRule FindRule(string ruleId)
        {
            if (string.IsNullOrEmpty(ruleId) || _state.rules == null) return null;
            for (int i = 0; i < _state.rules.Count; i++)
            {
                var r = _state.rules[i];
                if (r != null && r.ruleId == ruleId) return r;
            }
            return null;
        }

        public void AddRule(string ruleId, LogicRule rule)
        {
            if (string.IsNullOrEmpty(ruleId) || rule == null)
            {
                Debug.LogWarning("[System_LogicGates] AddRule called with null ruleId or rule.");
                return;
            }

            if (_state.rules == null)
                _state.rules = new List<LogicRule>();

            rule.ruleId = ruleId;
            // Remove existing rule with same id if present
            _state.rules.RemoveAll(r => r != null && r.ruleId == ruleId);
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

            if (_state.rules == null) return;
            int removed = _state.rules.RemoveAll(r => r != null && r.ruleId == ruleId);
            if (removed > 0)
                OnRuleRemoved?.Invoke(ruleId);
        }

        /// <summary>
        /// Evaluates all rules against current module states and fires matching events.
        /// moduleStates maps moduleId -&gt; current float value.
        /// Returns list of triggered rule IDs.
        /// </summary>
        public List<string> EvaluateRules(Dictionary<string, float> moduleStates)
        {
            var triggered = new List<string>();

            if (moduleStates == null)
            {
                Debug.LogWarning("[System_LogicGates] EvaluateRules called with null moduleStates.");
                return triggered;
            }

            if (_state.rules == null) return triggered;

            for (int i = 0; i < _state.rules.Count; i++)
            {
                LogicRule rule = _state.rules[i];
                if (rule == null || string.IsNullOrEmpty(rule.conditionModule))
                    continue;

                if (!moduleStates.TryGetValue(rule.conditionModule, out float moduleValue))
                    continue;

                if (!EvaluateCondition(moduleValue, rule.conditionOperator, rule.conditionValue))
                    continue;

                triggered.Add(rule.ruleId);
                OnRuleTriggered?.Invoke(rule.ruleId);
            }

            return triggered;
        }

        private static bool EvaluateCondition(float actual, string op, float threshold)
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

        public LogicGatesState CaptureState()
        {
            return CloneState(_state);
        }

        public void RestoreState(LogicGatesState state)
        {
            _state = state != null ? CloneState(state) : new LogicGatesState();
            if (string.IsNullOrEmpty(_state.systemId))
                _state.systemId = "system_logic_gates";
            if (_state.rules == null)
                _state.rules = new List<LogicRule>();
        }

        private static LogicGatesState CloneState(LogicGatesState src)
        {
            var captured = new LogicGatesState
            {
                systemId = string.IsNullOrEmpty(src.systemId) ? "system_logic_gates" : src.systemId,
                rules = new List<LogicRule>()
            };

            if (src.rules == null) return captured;

            for (int i = 0; i < src.rules.Count; i++)
            {
                var rule = src.rules[i];
                if (rule == null) continue;
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
    }
}
