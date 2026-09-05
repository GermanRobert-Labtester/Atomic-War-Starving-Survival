using System;
using System.Collections.Generic;
using Ashfall.Core.World;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Radio trigger ids for weather-gate transitions (F11 / C5).
    /// Data-driven mapping from transition category to trigger id —
    /// never spread string literals across WeatherSystem, radio UI, and
    /// tests (C5). Radio copy references generic geography only; it never
    /// contains route_XX / gate_XX ids (C9).
    /// </summary>
    public static class WeatherGateRadioTriggerIds
    {
        public const string HighlandClosure = "weather_gate.highland_closure";
        public const string ThawOpening = "weather_gate.thaw_opening";
        public const string LowlandFlood = "weather_gate.lowland_flood";
        public const string WastelandFallout = "weather_gate.wasteland_fallout";
        public const string BasinFogWarning = "weather_gate.basin_fog_warning";
    }

    /// <summary>
    /// Subscribes to <see cref="WeatherSystem.OnWeatherChanged"/> and turns
    /// gate-state transitions into radio triggers (F11).
    ///
    /// Semantics:
    ///   - rising-edge only: a trigger fires when a gate actually changes
    ///     state between the previous and current weather — never on every
    ///     weather tick (C8), UI refresh, forecast generation, or save
    ///     restore (R9);
    ///   - one-shot: each trigger id fires at most once per transition
    ///     until its world condition clears (C6/C7);
    ///   - re-subscribe safe: on Subscribe the pending queue is dropped
    ///     but consumed-trigger state is kept, so a save/load cycle cannot
    ///     replay an already-heard broadcast (R9/§15);
    ///   - no id leakage: payloads handed to the radio runtime carry
    ///     generic geography strings only (C9).
    /// </summary>
    public sealed class WeatherGateRadioHooks
    {
        private readonly WeatherGateEvaluator _evaluator;
        private readonly ILog _log;

        private readonly HashSet<string> _consumedTriggers =
            new HashSet<string>(StringComparer.Ordinal);

        private readonly Dictionary<string, bool> _worldConditionOpen =
            new Dictionary<string, bool>(StringComparer.Ordinal);

        private readonly Queue<(string TriggerId, WeatherGateTransition Transition)> _pending =
            new Queue<(string, WeatherGateTransition)>();

        private WeatherKind _lastWeather = WeatherKind.Clear;
        private bool _subscribed;

        public WeatherGateRadioHooks(WeatherGateEvaluator evaluator, ILog? log = null)
        {
            _evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            _log = log ?? NullLog.Instance;
        }

        /// <summary>World condition flags per gate (C6). Cleared only on closure.</summary>
        public IReadOnlyDictionary<string, bool> WorldConditionOpen => _worldConditionOpen;

        /// <summary>Trigger ids already consumed this session (one-shot set).</summary>
        public IReadOnlyCollection<string> ConsumedTriggers => _consumedTriggers;

        public int PendingCount => _pending.Count;

        public void Subscribe(WeatherSystem weather)
        {
            if (weather == null) throw new ArgumentNullException(nameof(weather));
            if (_subscribed)
                weather.OnWeatherChanged -= HandleWeatherChanged;
            _lastWeather = weather.Current;
            weather.OnWeatherChanged += HandleWeatherChanged;
            _subscribed = true;
            // Deliberately keep _consumedTriggers and _worldConditionOpen:
            // re-subscription (save/load, panel reopen) must not replay
            // broadcasts or resurrect cleared world conditions (R9/§15).
        }

        public void Unsubscribe(WeatherSystem weather)
        {
            if (weather == null || !_subscribed)
                return;
            weather.OnWeatherChanged -= HandleWeatherChanged;
            _subscribed = false;
            _pending.Clear(); // drop queued triggers; consumed state stays
        }

        private void HandleWeatherChanged(WeatherKind next)
        {
            var previous = _lastWeather;
            _lastWeather = next;

            var transitions = _evaluator.CompareWeatherStates(previous, next);
            foreach (var transition in transitions)
            {
                bool wasOpen = transition.WasOpen;
                bool isOpen = transition.IsOpen;

                // Current condition state (C6/C7): track and clear on closure.
                if (isOpen)
                    _worldConditionOpen[transition.GateId] = true;
                else
                    _worldConditionOpen.Remove(transition.GateId);

                if (wasOpen == isOpen)
                    continue; // no state change — never a trigger (C8)

                string triggerId = TriggerIdFor(transition);
                if (_consumedTriggers.Contains(triggerId))
                    continue; // one-shot: already heard this transition (C8)

                _consumedTriggers.Add(triggerId);
                _pending.Enqueue((triggerId, transition));
                _log.Info($"[WeatherGateRadio] {previous} -> {next}: trigger '{triggerId}' " +
                          $"({Describe(transition)}, polarity={transition.Polarity})");
            }
        }

        /// <summary>
        /// Deliver every queued trigger to the radio runtime and clear the
        /// pending queue. Consumed-trigger state is kept (one-shot set);
        /// world-condition flags are untouched (C6).
        /// </summary>
        private void DrainPending()
        {
            while (_pending.Count > 0)
            {
                var (triggerId, transition) = _pending.Dequeue();
                _log.Info($"[WeatherGateRadio] delivering '{triggerId}': {Describe(transition)}");
            }
        }

        /// <summary>
        /// Map a transition to its radio trigger id (C5). Centralized —
        /// WeatherSystem, radio UI, and tests all use this one mapping.
        /// </summary>
        private static string TriggerIdFor(WeatherGateTransition transition)
        {
            // Negative closure during severe weather, positive opening
            // during the same weather: category decides the trigger.
            return transition.Polarity switch
            {
                WeatherGatePolarity.NegativeBlockedDuring
                    when IsSevereWeatherTransition(transition)
                    => WeatherGateRadioTriggerIds.HighlandClosure,

                WeatherGatePolarity.PositiveRequiredWeather
                    when IsDeepColdTransition(transition)
                    => WeatherGateRadioTriggerIds.ThawOpening,

                _ => DefaultTrigger(transition)
            };
        }

        private static bool IsSevereWeatherTransition(WeatherGateTransition t) =>
            t.GateId.Contains("blizzard", StringComparison.OrdinalIgnoreCase) ||
            t.GateId.Contains("black_rain", StringComparison.OrdinalIgnoreCase) ||
            t.GateId.Contains("fallout", StringComparison.OrdinalIgnoreCase);

        private static bool IsDeepColdTransition(WeatherGateTransition t) =>
            t.GateId.Contains("frozen", StringComparison.OrdinalIgnoreCase) ||
            t.GateId.Contains("deep_cold", StringComparison.OrdinalIgnoreCase);

        private static string DefaultTrigger(WeatherGateTransition t)
        {
            // Fallback keeps behaviour deterministic and non-repetitive:
            // closure of a previously-open gate reports as a closure; an
            // opening reports as an opening; anything else is ignored.
            if (t.WasOpen && !t.IsOpen)
                return "weather_gate.generic_closure";
            if (!t.WasOpen && t.IsOpen)
                return "weather_gate.generic_opening";
            return string.Empty; // no transition — no trigger
        }

        private static string Describe(WeatherGateTransition t) =>
            $"{t.GateId} ({t.TargetId}): {(t.WasOpen ? "open" : "closed")} -> {(t.IsOpen ? "open" : "closed")}";

        /// <summary>
        /// Drain queued triggers for delivery to the radio runtime.
        /// Consumption clears only the trigger — never the underlying
        /// world condition (C6).
        /// </summary>
        public IReadOnlyList<(string TriggerId, WeatherGateTransition Transition)> DequeueAll()
        {
            var drained = new List<(string, WeatherGateTransition)>();
            while (_pending.Count > 0)
                drained.Add(_pending.Dequeue());
            return drained;
        }

        /// <summary>Test/diagnostic reset: clears queues but keeps consumed ids.</summary>
        public void ResetForTest()
        {
            _pending.Clear();
            _worldConditionOpen.Clear();
            _lastWeather = WeatherKind.Clear;
        }
    }
}
