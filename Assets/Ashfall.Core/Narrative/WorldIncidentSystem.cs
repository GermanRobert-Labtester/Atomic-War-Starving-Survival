// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Narrative
{
    public sealed class WorldIncidentState
    {
        public string PendingEventId = string.Empty;
        public int PendingDay;
        public List<string> ResolvedEventIds = new List<string>();
        public List<string> EventFlags = new List<string>();
        public List<WorldIncidentScheduleState> Scheduled = new List<WorldIncidentScheduleState>();
        public List<WorldIncidentResolutionState> Resolutions = new List<WorldIncidentResolutionState>();
    }

    public sealed class WorldIncidentScheduleState
    {
        public string EventId = string.Empty;
        public int DueDay;
    }

    public sealed class WorldIncidentResolutionState
    {
        public string EventId = string.Empty;
        public string ChoiceId = string.Empty;
        public int Day;
    }

    public enum WorldIncidentResolutionStatus
    {
        Rejected = 0,
        Committed = 1,
        AlreadyResolved = 2
    }

    public sealed class WorldIncidentResolutionResult
    {
        public WorldIncidentResolutionStatus Status { get; internal set; }
        public string EventId { get; internal set; } = string.Empty;
        public string ChoiceId { get; internal set; } = string.Empty;
        public string Reason { get; internal set; } = string.Empty;
        public WorldIncidentDefinition? Incident { get; internal set; }
        public WorldIncidentChoice? Choice { get; internal set; }
        public double MoraleDelta { get; internal set; }
        public bool Succeeded =>
            Status == WorldIncidentResolutionStatus.Committed ||
            Status == WorldIncidentResolutionStatus.AlreadyResolved;
    }

    public sealed class WorldIncidentConditions
    {
        public int MinDay { get; internal set; }
        public string RequiredFlagId { get; internal set; } = string.Empty;
        public string RequireWorldFlag { get; internal set; } = string.Empty;
        public bool WorldFlagValue { get; internal set; } = true;
        public List<string> RequiredEventFlags { get; } = new List<string>();
        public string RequireWeather { get; internal set; } = string.Empty;
        public bool RequireFalloutStorm { get; internal set; }
        /// <summary>
        /// True when the row carries a condition key no runtime owner binds yet
        /// (child cohorts, ration states, roommate friction, ...). Such rows
        /// stay ineligible fail-closed instead of firing on an unevaluated gate.
        /// </summary>
        public bool HasUnsupportedConditions { get; internal set; }
    }

    public sealed class WorldIncidentEffect
    {
        public string TargetNeed { get; internal set; } = string.Empty;
        public double NeedDelta { get; internal set; }
        public string SetWorldFlag { get; internal set; } = string.Empty;
        public bool WorldFlagValue { get; internal set; } = true;
        public string ItemId { get; internal set; } = string.Empty;
        public int ItemAmount { get; internal set; }
        public string ScheduleEventId { get; internal set; } = string.Empty;
        public int ScheduleOnDay { get; internal set; }
        public int ScheduleDelayDays { get; internal set; }
        public string Type { get; internal set; } = string.Empty;
        public string FactionId { get; internal set; } = string.Empty;
        public int Delta { get; internal set; }

        public bool IsSchedule =>
            !string.IsNullOrEmpty(ScheduleEventId) &&
            (ScheduleOnDay > 0 || ScheduleDelayDays > 0);
    }

    public sealed class WorldIncidentChoice
    {
        public string ChoiceId { get; internal set; } = string.Empty;
        public string Text { get; internal set; } = string.Empty;
        public double MoraleDelta { get; internal set; }
        public List<WorldIncidentEffect> Effects { get; } = new List<WorldIncidentEffect>();
        public List<string> SetEventFlags { get; } = new List<string>();
        public bool IsExecutable { get; internal set; } = true;
        public string ValidationError { get; internal set; } = string.Empty;
    }

    public sealed class WorldIncidentDefinition
    {
        public string Id { get; internal set; } = string.Empty;
        public string Title { get; internal set; } = string.Empty;
        public string BodyText { get; internal set; } = string.Empty;
        public double Weight { get; internal set; }
        public int MinDay { get; internal set; }
        public int MaxDay { get; internal set; }
        public WorldIncidentConditions? Conditions { get; internal set; }
        public IReadOnlyList<WorldIncidentChoice> Choices => _choices;
        private readonly List<WorldIncidentChoice> _choices = new List<WorldIncidentChoice>();

        public bool HasExecutableChoices => _choices.Any(c => c.IsExecutable);

        internal void AddChoice(WorldIncidentChoice choice) => _choices.Add(choice);
    }

    /// <summary>
    /// Typed boundary from the incident engine to the host's existing effect
    /// owners (world flags, shared inventory, needs, morale, weather, faction
    /// standing). The closed default fails every Can* so an unbound host can
    /// never silently drop a consequence.
    /// </summary>
    public interface IWorldIncidentConsequencePort
    {
        bool CanApplyMorale(double delta, out string reason);
        void ApplyMorale(double delta);
        bool CanGrantItem(string itemId, int amount, out string reason);
        void GrantItem(string itemId, int amount);
        bool CanApplyNeedDelta(string needId, double delta, out string reason);
        void ApplyNeedDelta(string needId, double delta);
        bool CanSetWorldFlag(string flagId, bool value, out string reason);
        void SetWorldFlag(string flagId, bool value);
        bool IsWorldFlagSet(string flagId);
        bool WeatherIs(string weatherKindName);
        bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason);
        void ApplyFactionStanding(string canonicalFactionId, int delta);
    }

    public sealed class NullWorldIncidentConsequencePort : IWorldIncidentConsequencePort
    {
        public static readonly NullWorldIncidentConsequencePort Instance = new NullWorldIncidentConsequencePort();
        private NullWorldIncidentConsequencePort() { }

        public bool CanApplyMorale(double delta, out string reason)
        { reason = "morale authority is not bound"; return false; }
        public void ApplyMorale(double delta) { }
        public bool CanGrantItem(string itemId, int amount, out string reason)
        { reason = "inventory authority is not bound"; return false; }
        public void GrantItem(string itemId, int amount) { }
        public bool CanApplyNeedDelta(string needId, double delta, out string reason)
        { reason = "needs authority is not bound"; return false; }
        public void ApplyNeedDelta(string needId, double delta) { }
        public bool CanSetWorldFlag(string flagId, bool value, out string reason)
        { reason = "world flag authority is not bound"; return false; }
        public void SetWorldFlag(string flagId, bool value) { }
        public bool IsWorldFlagSet(string flagId) => false;
        public bool WeatherIs(string weatherKindName) => false;
        public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
        { reason = "faction standing authority is not bound"; return false; }
        public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
    }

    /// <summary>
    /// Engine-free authority for the authored world-incident stream
    /// (events.json): day-window and condition gating, deterministic weighted
    /// selection, scheduled follow-up events, once-only resolution, and
    /// persistence. It is the third fallback of the per-day decision stream
    /// (narrative arc, then field echo, then world incident) and applies no
    /// consequence itself — every effect routes through the consequence port.
    /// </summary>
    public sealed class WorldIncidentSystem
    {
        public const string SystemId = "world_incident_system";
        public const string CatalogFileName = WorldIncidentCatalogLoader.FileName;

        private WorldIncidentState _state;
        private readonly List<WorldIncidentDefinition> _catalog = new List<WorldIncidentDefinition>();

        public event Action<WorldIncidentDefinition>? OnIncidentSurfaced;
        public event Action<WorldIncidentResolutionResult>? OnIncidentResolved;
        public event Action? StateChanged;

        public IWorldIncidentConsequencePort Consequences { get; set; } =
            NullWorldIncidentConsequencePort.Instance;

        public WorldIncidentSystem(WorldIncidentState? state = null)
        {
            _state = state ?? new WorldIncidentState();
            NormalizeState(_state);
        }

        public WorldIncidentSystem(
            IEnumerable<WorldIncidentDefinition> catalog,
            WorldIncidentState? state = null)
            : this(state)
        {
            RegisterRange(catalog);
        }

        public WorldIncidentState State => _state;
        public IReadOnlyList<WorldIncidentDefinition> Catalog => _catalog;
        public string LastEvent { get; private set; } = string.Empty;

        public WorldIncidentDefinition? PendingIncident => Find(_state.PendingEventId);
        public bool HasPendingIncident => PendingIncident != null;

        public void RegisterRange(IEnumerable<WorldIncidentDefinition>? definitions)
        {
            if (definitions == null) return;
            foreach (var definition in definitions)
            {
                if (definition == null) continue;
                if (_catalog.Any(d => string.Equals(d.Id, definition.Id, StringComparison.Ordinal)))
                    continue;
                _catalog.Add(definition);
            }
        }

        public WorldIncidentDefinition? Find(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return null;
            for (int i = 0; i < _catalog.Count; i++)
                if (string.Equals(_catalog[i].Id, eventId, StringComparison.Ordinal))
                    return _catalog[i];
            return null;
        }

        public bool IsResolved(string eventId) => _state.ResolvedEventIds.Contains(eventId);
        public bool HasEventFlag(string flagId) => _state.EventFlags.Contains(flagId);

        public IReadOnlyList<WorldIncidentDefinition> GetEligibleCandidates(int day)
        {
            var result = new List<WorldIncidentDefinition>();
            for (int i = 0; i < _catalog.Count; i++)
                if (IsEligible(_catalog[i], day))
                    result.Add(_catalog[i]);
            return result;
        }

        public bool IsEligible(WorldIncidentDefinition definition, int day)
        {
            if (definition == null || day < definition.MinDay || IsResolved(definition.Id))
                return false;
            if (definition.MaxDay > 0 && (day > definition.MaxDay || definition.MaxDay < definition.MinDay))
                return false;
            var conditions = definition.Conditions;
            if (conditions == null) return true;

            if (day < conditions.MinDay) return false;
            if (conditions.HasUnsupportedConditions) return false;
            if (!string.IsNullOrWhiteSpace(conditions.RequiredFlagId) &&
                !Consequences.IsWorldFlagSet(conditions.RequiredFlagId))
                return false;
            if (!string.IsNullOrWhiteSpace(conditions.RequireWorldFlag) &&
                Consequences.IsWorldFlagSet(conditions.RequireWorldFlag) != conditions.WorldFlagValue)
                return false;
            for (int i = 0; i < conditions.RequiredEventFlags.Count; i++)
                if (!HasEventFlag(conditions.RequiredEventFlags[i])) return false;
            if (conditions.RequireFalloutStorm && !Consequences.WeatherIs("FalloutStorm"))
                return false;
            if (!string.IsNullOrWhiteSpace(conditions.RequireWeather) &&
                !Consequences.WeatherIs(conditions.RequireWeather))
                return false;
            return true;
        }

        /// <summary>
        /// One deterministic draw for the day: a due scheduled follow-up wins,
        /// then a weighted roll over the eligible pool. A selected incident
        /// with at least one executable choice becomes the pending decision;
        /// one without choices resolves immediately as an informational
        /// broadcast (journal/HUD only, never a modal).
        /// </summary>
        public WorldIncidentDefinition? SelectForDay(int day, ISeededRng rng)
        {
            if (rng == null) return null;
            if (HasPendingIncident) return PendingIncident;

            var scheduled = TakeDueSchedule(day);
            if (scheduled != null)
            {
                Surface(scheduled, day);
                return scheduled;
            }

            var candidates = GetEligibleCandidates(day)
                .Where(c => c.Weight > 0d)
                .ToList();
            if (candidates.Count == 0) return null;
            double total = candidates.Sum(c => c.Weight);
            if (total <= 0d || double.IsNaN(total) || double.IsInfinity(total)) return null;

            double roll = rng.NextDouble() * total;
            double accumulated = 0d;
            WorldIncidentDefinition? selected = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                accumulated += candidates[i].Weight;
                if (roll < accumulated || i == candidates.Count - 1)
                {
                    selected = candidates[i];
                    break;
                }
            }
            if (selected == null) return null;

            Surface(selected, day);
            return selected;
        }

        private WorldIncidentDefinition? TakeDueSchedule(int day)
        {
            if (_state.Scheduled.Count == 0) return null;
            WorldIncidentScheduleState? due = null;
            foreach (var entry in _state.Scheduled)
            {
                if (entry.DueDay > day) continue;
                if (due == null || entry.DueDay < due.DueDay) due = entry;
            }
            if (due == null) return null;

            _state.Scheduled.Remove(due);
            var definition = Find(due.EventId);
            if (definition == null || !IsEligible(definition, day)) return null;
            return definition;
        }

        private void Surface(WorldIncidentDefinition definition, int day)
        {
            if (definition.HasExecutableChoices)
            {
                _state.PendingEventId = definition.Id;
                _state.PendingDay = day;
            }
            else
            {
                // Informational incident: no decision exists, so it resolves
                // the moment it surfaces instead of blocking the pending slot.
                _state.ResolvedEventIds.Add(definition.Id);
                _state.Resolutions.Add(new WorldIncidentResolutionState
                {
                    EventId = definition.Id,
                    ChoiceId = string.Empty,
                    Day = day
                });
            }
            LastEvent = "World incident surfaced: " + definition.Id;
            OnIncidentSurfaced?.Invoke(definition);
            RaiseChanged();
        }

        public WorldIncidentResolutionResult CanResolve(string eventId, string choiceId, int day)
        {
            var result = new WorldIncidentResolutionResult
            {
                Status = WorldIncidentResolutionStatus.Rejected,
                EventId = eventId ?? string.Empty,
                ChoiceId = choiceId ?? string.Empty
            };

            var definition = Find(eventId ?? string.Empty);
            if (definition == null) return Reject(result, "unknown world incident");
            if (IsResolved(definition.Id))
            {
                result.Status = WorldIncidentResolutionStatus.AlreadyResolved;
                result.Incident = definition;
                return result;
            }
            if (!string.Equals(_state.PendingEventId, definition.Id, StringComparison.Ordinal))
                return Reject(result, "incident is not the pending world decision");
            if (!IsEligible(definition, day)) return Reject(result, "incident is no longer eligible");

            WorldIncidentChoice? choice = null;
            for (int i = 0; i < definition.Choices.Count; i++)
                if (string.Equals(definition.Choices[i].ChoiceId, choiceId, StringComparison.Ordinal))
                { choice = definition.Choices[i]; break; }
            if (choice == null) return Reject(result, "unknown choice");
            if (!choice.IsExecutable) return Reject(result, choice.ValidationError);

            result.Incident = definition;
            result.Choice = choice;
            result.MoraleDelta = choice.MoraleDelta;
            if (choice.MoraleDelta != 0d &&
                !Consequences.CanApplyMorale(choice.MoraleDelta, out var moraleReason))
                return Reject(result, moraleReason);

            for (int i = 0; i < choice.Effects.Count; i++)
            {
                if (!CanApplyEffect(choice.Effects[i], out var effectReason))
                    return Reject(result, effectReason);
            }
            result.Status = WorldIncidentResolutionStatus.Committed;
            return result;
        }

        public WorldIncidentResolutionResult Resolve(string eventId, string choiceId, int day)
        {
            var preflight = CanResolve(eventId, choiceId, day);
            if (preflight.Status != WorldIncidentResolutionStatus.Committed)
                return preflight;

            var definition = preflight.Incident!;
            var choice = preflight.Choice!;

            if (choice.MoraleDelta != 0d)
                Consequences.ApplyMorale(choice.MoraleDelta);
            for (int i = 0; i < choice.Effects.Count; i++)
                ApplyEffect(choice.Effects[i], day);
            for (int i = 0; i < choice.SetEventFlags.Count; i++)
                if (!_state.EventFlags.Contains(choice.SetEventFlags[i]))
                    _state.EventFlags.Add(choice.SetEventFlags[i]);

            _state.ResolvedEventIds.Add(definition.Id);
            _state.Resolutions.Add(new WorldIncidentResolutionState
            {
                EventId = definition.Id,
                ChoiceId = choice.ChoiceId,
                Day = day
            });
            _state.PendingEventId = string.Empty;
            _state.PendingDay = 0;

            LastEvent = "World incident resolved: " + definition.Id + " (" + choice.ChoiceId + ")";
            preflight.Status = WorldIncidentResolutionStatus.Committed;
            OnIncidentResolved?.Invoke(preflight);
            RaiseChanged();
            return preflight;
        }

        private bool CanApplyEffect(WorldIncidentEffect effect, out string reason)
        {
            if (effect.IsSchedule)
            {
                if (Find(effect.ScheduleEventId) == null)
                { reason = "scheduled follow-up '" + effect.ScheduleEventId + "' is not in the catalog"; return false; }
                reason = string.Empty;
                return true;
            }
            if (!string.IsNullOrWhiteSpace(effect.SetWorldFlag))
                return Consequences.CanSetWorldFlag(effect.SetWorldFlag, effect.WorldFlagValue, out reason);
            if (!string.IsNullOrWhiteSpace(effect.ItemId) && effect.ItemAmount != 0)
                return Consequences.CanGrantItem(effect.ItemId, effect.ItemAmount, out reason);
            if (!string.IsNullOrWhiteSpace(effect.TargetNeed) && effect.NeedDelta != 0d)
                return Consequences.CanApplyNeedDelta(effect.TargetNeed, effect.NeedDelta, out reason);
            if (!string.IsNullOrWhiteSpace(effect.Type) &&
                string.Equals(effect.Type, "faction_standing", StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(effect.FactionId))
                { reason = "faction_standing effect has no faction"; return false; }
                return Consequences.CanApplyFactionStanding(effect.FactionId, effect.Delta, out reason);
            }
            reason = string.Empty;
            return true;
        }

        private void ApplyEffect(WorldIncidentEffect effect, int day)
        {
            if (effect.IsSchedule)
            {
                var dueDay = effect.ScheduleOnDay > 0
                    ? effect.ScheduleOnDay
                    : day + Math.Max(1, effect.ScheduleDelayDays);
                _state.Scheduled.Add(new WorldIncidentScheduleState
                {
                    EventId = effect.ScheduleEventId,
                    DueDay = dueDay
                });
                return;
            }
            if (!string.IsNullOrWhiteSpace(effect.SetWorldFlag))
                Consequences.SetWorldFlag(effect.SetWorldFlag, effect.WorldFlagValue);
            else if (!string.IsNullOrWhiteSpace(effect.ItemId) && effect.ItemAmount != 0)
                Consequences.GrantItem(effect.ItemId, effect.ItemAmount);
            else if (!string.IsNullOrWhiteSpace(effect.TargetNeed) && effect.NeedDelta != 0d)
                Consequences.ApplyNeedDelta(effect.TargetNeed, effect.NeedDelta);
            else if (!string.IsNullOrWhiteSpace(effect.Type) &&
                     string.Equals(effect.Type, "faction_standing", StringComparison.Ordinal) &&
                     !string.IsNullOrWhiteSpace(effect.FactionId))
                Consequences.ApplyFactionStanding(effect.FactionId, effect.Delta);
        }

        // ── Persistence ──────────────────────────────────────────────

        public WorldIncidentState CaptureState() => CloneState(_state);

        public void RestoreState(WorldIncidentState? saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            NormalizeState(_state);
            RaiseChanged();
        }

        private static WorldIncidentState CloneState(WorldIncidentState state)
        {
            return new WorldIncidentState
            {
                PendingEventId = state.PendingEventId,
                PendingDay = state.PendingDay,
                ResolvedEventIds = new List<string>(state.ResolvedEventIds),
                EventFlags = new List<string>(state.EventFlags),
                Scheduled = state.Scheduled
                    .Select(s => new WorldIncidentScheduleState { EventId = s.EventId, DueDay = s.DueDay })
                    .ToList(),
                Resolutions = state.Resolutions
                    .Select(r => new WorldIncidentResolutionState
                    {
                        EventId = r.EventId, ChoiceId = r.ChoiceId, Day = r.Day
                    })
                    .ToList()
            };
        }

        private static void NormalizeState(WorldIncidentState state)
        {
            state.PendingEventId ??= string.Empty;
            state.ResolvedEventIds ??= new List<string>();
            state.EventFlags ??= new List<string>();
            state.Scheduled ??= new List<WorldIncidentScheduleState>();
            state.Resolutions ??= new List<WorldIncidentResolutionState>();
        }

        private void RaiseChanged()
        {
            StateChanged?.Invoke();
            LastEventChanged?.Invoke();
        }

        public event Action? LastEventChanged;

        private static WorldIncidentResolutionResult Reject(
            WorldIncidentResolutionResult result, string reason)
        {
            result.Status = WorldIncidentResolutionStatus.Rejected;
            result.Reason = reason;
            return result;
        }
    }

    public sealed class WorldIncidentCatalogLoadResult
    {
        public int SchemaVersion { get; set; }
        public List<WorldIncidentDefinition> Incidents { get; } = new List<WorldIncidentDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
    }

    public static class WorldIncidentCatalogLoader
    {
        public const string FileName = "events.json";

        private sealed class RootDto
        {
            public int schema_version { get; set; }
            public List<IncidentDto>? events { get; set; }
        }

        private sealed class IncidentDto
        {
            public string? id { get; set; }
            public string? title { get; set; }
            public string? bodyText { get; set; }
            public double weight { get; set; }
            public int minDay { get; set; }
            public int maxDay { get; set; }
            public ConditionsDto? conditions { get; set; }
            public List<ChoiceDto>? choices { get; set; }
        }

        private sealed class ConditionsDto
        {
            public int MinDay { get; set; }
            public string? RequiredFlagId { get; set; }
            public string? RequireWorldFlag { get; set; }
            // Authored as either a bool or a string flag value; the bool-only
            // campaign ledger cannot evaluate string values.
            public object? WorldFlagValue { get; set; }
            public List<string>? RequiredEventFlags { get; set; }
            public string? RequireWeather { get; set; }
            public bool RequireFalloutStorm { get; set; }
            public bool RequireChildCohort { get; set; }
            public bool RequireMaturationEligible { get; set; }
            public bool RequireCasualtyOrphan { get; set; }
            public bool RequireRoommateFriction { get; set; }
            public bool RequireCohortAdoptionPending { get; set; }
            public bool RequireRationUnevenAllocation { get; set; }
            public bool RequireRationTheft { get; set; }
            public bool RequireResentmentBuffer { get; set; }
        }

        private sealed class ChoiceDto
        {
            public string? choiceId { get; set; }
            public string? text { get; set; }
            public double moraleDelta { get; set; }
            public List<EffectDto>? effects { get; set; }
            public string? factionId { get; set; }
            public double trustDelta { get; set; }
            public List<string>? setEventFlags { get; set; }
            public string? requiredTrait { get; set; }
            public string? requiredTrustFactionId { get; set; }
            public double requiredTrustMin { get; set; }
            public double requiredTrustMaxExclusive { get; set; }
            public bool hideIfGatesFail { get; set; }
        }

        private sealed class EffectDto
        {
            public string? type { get; set; }
            public string? targetNeed { get; set; }
            public double needDelta { get; set; }
            public string? setWorldFlag { get; set; }
            public object? worldFlagValue { get; set; }
            public string? scheduleEventId { get; set; }
            public int scheduleOnDay { get; set; }
            public int scheduleDelayDays { get; set; }
            public string? itemId { get; set; }
            public int itemAmount { get; set; }
            public string? factionId { get; set; }
            // Authored typed effects carry fractional deltas/amounts and
            // survivor target lists; they are parsed so the row loads, then
            // fail closed as unbound consequence owners.
            public double delta { get; set; }
            public double amount { get; set; }
            public List<string>? targetIds { get; set; }
        }

        /// <summary>
        /// Reads an authored world-flag value. Bools bind to the bool-only
        /// campaign ledger; absent defaults to true. Any other shape (the
        /// schooling rows carry string values like 'letters') returns false so
        /// the row fails closed instead of firing on an unevaluated value.
        /// </summary>
        private static bool TryParseFlagValue(object? raw, out bool value)
        {
            value = true;
            if (raw == null) return true;
            if (raw is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.True) { value = true; return true; }
                if (element.ValueKind == JsonValueKind.False) { value = false; return true; }
            }
            return false;
        }

        public static List<WorldIncidentDefinition> Load(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = LoadDetailed(dataDir, fileIO, json);
            return result.IsSuccess ? result.Incidents : new List<WorldIncidentDefinition>();
        }

        public static WorldIncidentCatalogLoadResult LoadDetailed(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new WorldIncidentCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrWhiteSpace(dataDir))
            {
                result.Errors.Add("world incident catalog requires a data directory, file IO, and JSON serializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("world incident catalog file not found: " + FileName);
                return result;
            }

            string raw;
            try
            {
                raw = fileIO.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw))
                {
                    result.Errors.Add("catalog file is empty: " + FileName);
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add("catalog read failed: " + ex.Message);
                return result;
            }

            RootDto? root;
            try
            {
                root = json.Deserialize<RootDto>(raw);
            }
            catch (Exception ex)
            {
                result.Errors.Add("catalog JSON parse failed: " + ex.Message);
                return result;
            }

            if (root == null)
            {
                result.Errors.Add("catalog root is null");
                return result;
            }
            result.SchemaVersion = root.schema_version;
            if (root.schema_version != 1)
                result.Errors.Add("unsupported schema_version " + root.schema_version);
            if (root.events == null)
            {
                result.Errors.Add("catalog root must contain an events array");
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.events.Count; i++)
            {
                var dto = root.events[i];
                if (dto == null)
                {
                    result.Errors.Add($"events[{i}] is null");
                    continue;
                }
                string id = dto.id?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(id))
                {
                    result.Errors.Add($"events[{i}] has no id");
                    continue;
                }
                if (!seen.Add(id))
                {
                    result.Errors.Add("duplicate event id: " + id);
                    continue;
                }
                // Weight 0 is authored intent: schedule-only follow-ups that
                // are never part of the random pool but still fire when a
                // choice schedules them.
                if (dto.weight < 0d)
                {
                    result.Errors.Add("event '" + id + "' has negative weight");
                    continue;
                }
                if (dto.minDay < 0)
                {
                    result.Errors.Add("event '" + id + "' has negative minDay");
                    continue;
                }
                if (dto.maxDay > 0 && dto.maxDay < dto.minDay)
                {
                    result.Errors.Add("event '" + id + "' has maxDay before minDay");
                    continue;
                }

                var definition = new WorldIncidentDefinition
                {
                    Id = id,
                    Title = dto.title ?? string.Empty,
                    BodyText = dto.bodyText ?? string.Empty,
                    Weight = dto.weight,
                    MinDay = dto.minDay,
                    MaxDay = dto.maxDay
                };

                if (dto.conditions != null)
                {
                    var c = dto.conditions;
                    var worldFlagValueBound = TryParseFlagValue(c.WorldFlagValue, out var worldFlagValue);
                    var conditions = new WorldIncidentConditions
                    {
                        MinDay = c.MinDay,
                        RequiredFlagId = c.RequiredFlagId?.Trim() ?? string.Empty,
                        RequireWorldFlag = c.RequireWorldFlag?.Trim() ?? string.Empty,
                        WorldFlagValue = worldFlagValue,
                        RequireWeather = c.RequireWeather?.Trim() ?? string.Empty,
                        RequireFalloutStorm = c.RequireFalloutStorm,
                        HasUnsupportedConditions =
                            !worldFlagValueBound ||
                            c.RequireChildCohort || c.RequireMaturationEligible ||
                            c.RequireCasualtyOrphan || c.RequireRoommateFriction ||
                            c.RequireCohortAdoptionPending || c.RequireRationUnevenAllocation ||
                            c.RequireRationTheft || c.RequireResentmentBuffer
                    };
                    foreach (var flag in c.RequiredEventFlags ?? new List<string>())
                        if (!string.IsNullOrWhiteSpace(flag))
                            conditions.RequiredEventFlags.Add(flag.Trim());
                    definition.Conditions = conditions;
                }

                foreach (var choiceDto in dto.choices ?? new List<ChoiceDto>())
                {
                    if (choiceDto == null) continue;
                    var choice = new WorldIncidentChoice
                    {
                        ChoiceId = choiceDto.choiceId?.Trim() ?? string.Empty,
                        Text = choiceDto.text ?? string.Empty,
                        MoraleDelta = choiceDto.moraleDelta
                    };
                    if (string.IsNullOrEmpty(choice.ChoiceId))
                    {
                        result.Errors.Add("event '" + id + "' has a choice with no choiceId");
                        continue;
                    }
                    foreach (var flag in choiceDto.setEventFlags ?? new List<string>())
                        if (!string.IsNullOrWhiteSpace(flag))
                            choice.SetEventFlags.Add(flag.Trim());

                    string? unsupported = null;
                    if (!string.IsNullOrWhiteSpace(choiceDto.requiredTrait))
                        unsupported = "trait gate '" + choiceDto.requiredTrait + "' has no bound owner";
                    else if (!string.IsNullOrWhiteSpace(choiceDto.requiredTrustFactionId) ||
                             choiceDto.trustDelta != 0d || choiceDto.requiredTrustMin != 0d ||
                             choiceDto.requiredTrustMaxExclusive != 0d)
                        unsupported = "faction trust gate has no bound owner";
                    else if (choiceDto.hideIfGatesFail)
                        unsupported = "hideIfGatesFail choice has no bound gate owner";

                    foreach (var effectDto in choiceDto.effects ?? new List<EffectDto>())
                    {
                        if (effectDto == null) continue;
                        var flagValueBound = TryParseFlagValue(effectDto.worldFlagValue, out var flagValue);
                        var effect = new WorldIncidentEffect
                        {
                            TargetNeed = effectDto.targetNeed?.Trim() ?? string.Empty,
                            NeedDelta = effectDto.needDelta,
                            SetWorldFlag = effectDto.setWorldFlag?.Trim() ?? string.Empty,
                            WorldFlagValue = flagValue,
                            ItemId = effectDto.itemId?.Trim() ?? string.Empty,
                            ItemAmount = effectDto.itemAmount,
                            ScheduleEventId = effectDto.scheduleEventId?.Trim() ?? string.Empty,
                            ScheduleOnDay = effectDto.scheduleOnDay,
                            ScheduleDelayDays = effectDto.scheduleDelayDays,
                            Type = effectDto.type?.Trim() ?? string.Empty,
                            FactionId = effectDto.factionId?.Trim() ?? string.Empty,
                            Delta = (int)effectDto.delta
                        };
                        choice.Effects.Add(effect);

                        if (!flagValueBound)
                            unsupported = "string-valued world flag has no bound owner";
                        else if (!string.IsNullOrEmpty(effect.Type) &&
                            !string.Equals(effect.Type, "faction_standing", StringComparison.Ordinal))
                        {
                            unsupported = "effect type '" + effect.Type + "' has no bound consequence owner";
                        }
                    }

                    if (unsupported != null)
                    {
                        choice.IsExecutable = false;
                        choice.ValidationError = unsupported;
                    }
                    definition.AddChoice(choice);
                }

                result.Incidents.Add(definition);
            }

            // Scheduled follow-ups must point at real catalog rows.
            foreach (var definition in result.Incidents)
                foreach (var choice in definition.Choices)
                    foreach (var effect in choice.Effects)
                        if (effect.IsSchedule &&
                            !result.Incidents.Any(d => string.Equals(d.Id, effect.ScheduleEventId, StringComparison.Ordinal)))
                            result.Errors.Add("event '" + definition.Id + "' schedules unknown event '" +
                                effect.ScheduleEventId + "'");

            return result;
        }
    }
}
