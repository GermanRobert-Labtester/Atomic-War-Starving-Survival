// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Content;

namespace Ashfall.Core.Narrative
{
    public sealed class EchoState
    {
        public string PendingEchoId = string.Empty;
        public int PendingDay;
        public List<string> ResolvedEchoIds = new List<string>();
        public List<string> ResolvedChoiceIds = new List<string>();
        public List<EchoPendingConsequenceState> PendingConsequences =
            new List<EchoPendingConsequenceState>();
    }

    public sealed class EchoPendingConsequenceState
    {
        public string EchoId = string.Empty;
        public string ChoiceId = string.Empty;
        public int DueDay;
    }

    public enum EchoResolutionStatus
    {
        Rejected = 0,
        Committed = 1,
        AlreadyResolved = 2
    }

    public sealed class EchoResolutionResult
    {
        public EchoResolutionStatus Status { get; internal set; }
        public string EchoId { get; internal set; } = string.Empty;
        public string ChoiceId { get; internal set; } = string.Empty;
        public string Reason { get; internal set; } = string.Empty;
        public EchoDefinition? Echo { get; internal set; }
        public EchoChoiceDefinition? Choice { get; internal set; }
        public double MoraleDelta { get; internal set; }
        public bool Succeeded =>
            Status == EchoResolutionStatus.Committed ||
            Status == EchoResolutionStatus.AlreadyResolved;
    }

    public sealed class EchoDelayedConsequenceResult
    {
        public string EchoId { get; internal set; } = string.Empty;
        public string ChoiceId { get; internal set; } = string.Empty;
        public EchoDelayedConsequence Consequence { get; internal set; } = new EchoDelayedConsequence();
        public int DueDay { get; internal set; }
    }

    /// <summary>
    /// Engine-free authority for echo availability, deterministic selection,
    /// once-only resolution, and persistence. Consequences are returned as a
    /// typed choice result for the host's existing effect owners to apply.
    /// </summary>
    public sealed class EchoSystem
    {
        public const string SystemId = "echo_system";
        public const string CatalogFileName = EchoCatalogLoader.FileName;

        private EchoState _state;
        private readonly List<EchoDefinition> _catalog = new List<EchoDefinition>();

        public event Action<EchoDefinition>? OnEchoSurfaced;
        public event Action<EchoResolutionResult>? OnEchoResolved;
        public event Action<EchoDelayedConsequenceResult>? OnDelayedConsequenceDue;
        public event Action<EchoState>? OnStateChanged;

        public ContentUtilizationInstrumentation? Instrumentation { get; set; }
        public Func<string, bool> HasWorldFlag { get; set; } = _ => false;

        public EchoSystem(EchoState? state = null)
        {
            _state = state ?? new EchoState();
            NormalizeState(_state);
        }

        public EchoSystem(
            IEnumerable<EchoDefinition> catalog,
            EchoState? state = null,
            ContentUtilizationInstrumentation? instrumentation = null)
            : this(state)
        {
            Instrumentation = instrumentation;
            RegisterRange(catalog);
        }

        public EchoState State => _state;
        public IReadOnlyList<EchoDefinition> Catalog => _catalog;
        public EchoDefinition? PendingEcho => Find(_state.PendingEchoId);
        public bool HasPendingEcho => PendingEcho != null;
        public bool HasPersistedState =>
            !string.IsNullOrEmpty(_state.PendingEchoId) ||
            _state.ResolvedEchoIds.Count > 0 ||
            _state.PendingConsequences.Count > 0;

        public void RegisterRange(IEnumerable<EchoDefinition>? definitions)
        {
            if (definitions == null) return;
            foreach (var definition in definitions)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.Id)) continue;
                if (_catalog.Any(e => string.Equals(e.Id, definition.Id, StringComparison.Ordinal))) continue;
                _catalog.Add(definition);
            }
            _catalog.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
        }

        public EchoDefinition? Find(string echoId)
        {
            if (string.IsNullOrWhiteSpace(echoId)) return null;
            for (int i = 0; i < _catalog.Count; i++)
            {
                if (!string.Equals(_catalog[i].Id, echoId, StringComparison.Ordinal)) continue;
                Instrumentation?.RecordDefinitionQueried(
                    CatalogFileName, echoId, nameof(Find), nameof(EchoSystem));
                return _catalog[i];
            }
            return null;
        }

        public IReadOnlyList<EchoDefinition> GetEligibleCandidates(int day)
        {
            var result = new List<EchoDefinition>();
            for (int i = 0; i < _catalog.Count; i++)
            {
                var definition = _catalog[i];
                Instrumentation?.RecordDefinitionQueried(
                    CatalogFileName, definition.Id, nameof(GetEligibleCandidates), nameof(EchoSystem), day);
                if (IsAvailable(definition, day)) result.Add(definition);
            }
            return result;
        }

        public bool IsAvailable(EchoDefinition definition, int day)
        {
            if (definition == null || day < definition.MinDay || IsResolved(definition.Id))
                return false;
            if (definition.Conditions != null)
            {
                if (day < definition.Conditions.MinDay) return false;
                if (!string.IsNullOrWhiteSpace(definition.Conditions.RequiredFlagId) &&
                    !HasWorldFlag(definition.Conditions.RequiredFlagId))
                    return false;
            }
            return true;
        }

        public EchoDefinition? SelectForDay(int day, ISeededRng rng)
        {
            if (rng == null) return null;
            if (HasPendingEcho) return PendingEcho;

            var candidates = GetEligibleCandidates(day);
            if (candidates.Count == 0) return null;
            double total = candidates.Sum(e => e.Weight);
            if (total <= 0d || double.IsNaN(total) || double.IsInfinity(total)) return null;

            double roll = rng.NextDouble() * total;
            double accumulated = 0d;
            for (int i = 0; i < candidates.Count; i++)
            {
                accumulated += candidates[i].Weight;
                if (roll < accumulated || i == candidates.Count - 1)
                {
                    var selected = candidates[i];
                    _state.PendingEchoId = selected.Id;
                    _state.PendingDay = day;
                    Instrumentation?.RecordDefinitionSelected(
                        CatalogFileName, selected.Id, nameof(EchoSystem), day);
                    OnEchoSurfaced?.Invoke(selected);
                    RaiseChanged();
                    return selected;
                }
            }
            return null;
        }

        public EchoResolutionResult CanResolve(string echoId, string choiceId, int day)
        {
            var result = NewResult(echoId, choiceId);
            var definition = Find(echoId);
            if (definition == null) return Reject(result, "unknown echo");
            if (IsResolved(echoId))
            {
                result.Status = EchoResolutionStatus.AlreadyResolved;
                result.Reason = "echo already resolved";
                return result;
            }
            if (!string.Equals(_state.PendingEchoId, echoId, StringComparison.Ordinal))
                return Reject(result, "echo is not pending");
            if (!IsAvailable(definition, day))
                return Reject(result, "echo prerequisites are no longer satisfied");

            var choice = definition.Choices.FirstOrDefault(c =>
                string.Equals(c.ChoiceId, choiceId, StringComparison.Ordinal));
            if (choice == null) return Reject(result, "unknown choice");
            if (!choice.IsExecutable) return Reject(result, "choice has no real effect");
            result.Echo = definition;
            result.Choice = choice;
            result.MoraleDelta = choice.MoraleDelta;
            result.Status = EchoResolutionStatus.Committed;
            return result;
        }

        public EchoResolutionResult Resolve(string echoId, string choiceId, int day)
        {
            var preflight = CanResolve(echoId, choiceId, day);
            if (preflight.Status != EchoResolutionStatus.Committed)
                return preflight;

            var next = CloneState(_state);
            if (!next.ResolvedEchoIds.Contains(echoId, StringComparer.Ordinal))
                next.ResolvedEchoIds.Add(echoId);
            next.ResolvedChoiceIds.Add(echoId + "/" + choiceId);
            if (preflight.Choice?.DelayedConsequence != null)
            {
                int delayDays = Math.Max(
                    1,
                    (int)Math.Ceiling(preflight.Choice.DelayedConsequence.DelayHours / 24d));
                next.PendingConsequences.Add(new EchoPendingConsequenceState
                {
                    EchoId = echoId,
                    ChoiceId = choiceId,
                    DueDay = day + delayDays
                });
            }
            next.PendingEchoId = string.Empty;
            next.PendingDay = 0;
            _state = next;

            var committed = new EchoResolutionResult
            {
                Status = EchoResolutionStatus.Committed,
                EchoId = echoId,
                ChoiceId = choiceId,
                Echo = preflight.Echo,
                Choice = preflight.Choice,
                MoraleDelta = preflight.MoraleDelta
            };
            Instrumentation?.RecordDefinitionConsumed(
                CatalogFileName,
                echoId,
                nameof(EchoSystem),
                "choice=" + choiceId,
                day);
            OnEchoResolved?.Invoke(committed);
            RaiseChanged();
            return committed;
        }

        /// <summary>
        /// Advances one-time delayed echo payloads through the same authority
        /// that resolved the choice. The catalog is used to reconstruct prose
        /// and effects from persisted IDs; no consequence is replayed twice.
        /// </summary>
        public IReadOnlyList<EchoDelayedConsequenceResult> TickDay(int day)
        {
            var due = _state.PendingConsequences
                .Where(p => p != null && p.DueDay <= day)
                .OrderBy(p => p.DueDay)
                .ThenBy(p => p.EchoId, StringComparer.Ordinal)
                .ThenBy(p => p.ChoiceId, StringComparer.Ordinal)
                .ToList();
            if (due.Count == 0) return Array.Empty<EchoDelayedConsequenceResult>();

            var results = new List<EchoDelayedConsequenceResult>(due.Count);
            var next = CloneState(_state);
            foreach (var pending in due)
            {
                var echo = Find(pending.EchoId);
                var choice = echo?.Choices.FirstOrDefault(c =>
                    string.Equals(c.ChoiceId, pending.ChoiceId, StringComparison.Ordinal));
                if (choice?.DelayedConsequence == null) continue;

                results.Add(new EchoDelayedConsequenceResult
                {
                    EchoId = pending.EchoId,
                    ChoiceId = pending.ChoiceId,
                    Consequence = choice.DelayedConsequence,
                    DueDay = pending.DueDay
                });
                next.PendingConsequences.RemoveAll(p =>
                    p != null &&
                    string.Equals(p.EchoId, pending.EchoId, StringComparison.Ordinal) &&
                    string.Equals(p.ChoiceId, pending.ChoiceId, StringComparison.Ordinal));
            }

            _state = next;
            foreach (var result in results)
                OnDelayedConsequenceDue?.Invoke(result);
            RaiseChanged();
            return results;
        }

        public bool IsResolved(string echoId)
        {
            return !string.IsNullOrWhiteSpace(echoId) &&
                   _state.ResolvedEchoIds.Contains(echoId, StringComparer.Ordinal);
        }

        public EchoState CaptureState() => CloneState(_state);

        public void RestoreState(EchoState? saved)
        {
            _state = saved == null ? new EchoState() : CloneState(saved);
            NormalizeState(_state);
            RaiseChanged();
        }

        private static EchoResolutionResult NewResult(string echoId, string choiceId)
        {
            return new EchoResolutionResult
            {
                Status = EchoResolutionStatus.Rejected,
                EchoId = echoId ?? string.Empty,
                ChoiceId = choiceId ?? string.Empty
            };
        }

        private static EchoResolutionResult Reject(EchoResolutionResult result, string reason)
        {
            result.Reason = reason;
            return result;
        }

        private void RaiseChanged()
        {
            OnStateChanged?.Invoke(CaptureState());
        }

        private static EchoState CloneState(EchoState source)
        {
            return new EchoState
            {
                PendingEchoId = source.PendingEchoId ?? string.Empty,
                PendingDay = source.PendingDay,
                ResolvedEchoIds = source.ResolvedEchoIds == null
                    ? new List<string>()
                    : source.ResolvedEchoIds.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct(StringComparer.Ordinal).ToList(),
                ResolvedChoiceIds = source.ResolvedChoiceIds == null
                    ? new List<string>()
                    : source.ResolvedChoiceIds.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct(StringComparer.Ordinal).ToList()
                ,
                PendingConsequences = source.PendingConsequences == null
                    ? new List<EchoPendingConsequenceState>()
                    : source.PendingConsequences
                        .Where(p => p != null && !string.IsNullOrWhiteSpace(p.EchoId) && !string.IsNullOrWhiteSpace(p.ChoiceId))
                        .Select(p => new EchoPendingConsequenceState
                        {
                            EchoId = p.EchoId,
                            ChoiceId = p.ChoiceId,
                            DueDay = p.DueDay
                        })
                        .OrderBy(p => p.DueDay)
                        .ThenBy(p => p.EchoId, StringComparer.Ordinal)
                        .ThenBy(p => p.ChoiceId, StringComparer.Ordinal)
                        .ToList()
            };
        }

        private static void NormalizeState(EchoState state)
        {
            state.PendingEchoId ??= string.Empty;
            state.ResolvedEchoIds ??= new List<string>();
            state.ResolvedChoiceIds ??= new List<string>();
            state.PendingConsequences ??= new List<EchoPendingConsequenceState>();
            state.ResolvedEchoIds = state.ResolvedEchoIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(id => id, StringComparer.Ordinal)
                .ToList();
            state.PendingConsequences = state.PendingConsequences
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.EchoId) && !string.IsNullOrWhiteSpace(p.ChoiceId))
                .GroupBy(p => p.EchoId + "/" + p.ChoiceId, StringComparer.Ordinal)
                .Select(g => g.OrderBy(p => p.DueDay).First())
                .OrderBy(p => p.DueDay)
                .ThenBy(p => p.EchoId, StringComparer.Ordinal)
                .ThenBy(p => p.ChoiceId, StringComparer.Ordinal)
                .ToList();
            state.ResolvedChoiceIds = state.ResolvedChoiceIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(id => id, StringComparer.Ordinal)
                .ToList();
            if (state.ResolvedEchoIds.Contains(state.PendingEchoId, StringComparer.Ordinal))
            {
                state.PendingEchoId = string.Empty;
                state.PendingDay = 0;
            }
        }
    }
}
