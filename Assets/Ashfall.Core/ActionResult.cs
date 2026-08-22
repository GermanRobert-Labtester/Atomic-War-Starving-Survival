using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Small engine-agnostic result envelope for player/simulation actions.
    /// Distinguishes a rule-gated action (Blocked) from invalid/missing input
    /// (Failed) while keeping presentation text behind a localization key.
    /// </summary>
    [Serializable]
    public sealed class ActionResult
    {
        public enum StatusKind
        {
            Success = 0,
            Blocked = 1,
            Failed = 2
        }

        public StatusKind Status { get; private set; }
        public string FailureCode { get; private set; } = string.Empty;
        public string MessageKey { get; private set; } = string.Empty;
        public IReadOnlyDictionary<string, double> Values => _values;
        public bool IsSuccess => Status == StatusKind.Success;

        private readonly Dictionary<string, double> _values;

        private ActionResult(
            StatusKind status,
            string failureCode,
            string messageKey,
            IDictionary<string, double> values = null)
        {
            Status = status;
            FailureCode = failureCode ?? string.Empty;
            MessageKey = messageKey ?? string.Empty;
            _values = values == null
                ? new Dictionary<string, double>(StringComparer.Ordinal)
                : new Dictionary<string, double>(values, StringComparer.Ordinal);
        }

        public static ActionResult Success(string messageKey = "", IDictionary<string, double> values = null) =>
            new ActionResult(StatusKind.Success, string.Empty, messageKey, values);

        public static ActionResult Blocked(string failureCode, string messageKey = "", IDictionary<string, double> values = null) =>
            new ActionResult(StatusKind.Blocked, failureCode, messageKey, values);

        public static ActionResult Failed(string failureCode, string messageKey = "", IDictionary<string, double> values = null) =>
            new ActionResult(StatusKind.Failed, failureCode, messageKey, values);

        public bool TryGetValue(string key, out double value) => _values.TryGetValue(key, out value);
    }
}
