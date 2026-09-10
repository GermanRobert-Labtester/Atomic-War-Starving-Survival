using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ashfall.Core.Content;
using Ashfall.Core.Factions;
using Ashfall.Core.IO;

namespace Ashfall.Core.Narrative
{
    /// <summary>Typed effect kinds authored by narrative_arc_events.json.</summary>
    public enum NarrativeArcEffectKind
    {
        Unsupported = 0,
        AdvanceNarrativeArc = 1,
        NarrativeArcBranch = 2,
        GainFactionIntel = 3,
        StartExpedition = 4,
        FactionStanding = 5
    }

    /// <summary>Base class for the deliberately small Plan 143 effect whitelist.</summary>
    public abstract class NarrativeArcEffect
    {
        public NarrativeArcEffectKind Kind { get; }
        public string AuthoredType { get; }

        protected NarrativeArcEffect(NarrativeArcEffectKind kind, string authoredType)
        {
            Kind = kind;
            AuthoredType = authoredType ?? string.Empty;
        }
    }

    public sealed class AdvanceNarrativeArcEffect : NarrativeArcEffect
    {
        public string SurvivorId { get; }

        public AdvanceNarrativeArcEffect(string survivorId)
            : base(NarrativeArcEffectKind.AdvanceNarrativeArc, "advance_narrative_arc")
        {
            SurvivorId = survivorId ?? string.Empty;
        }
    }

    public sealed class NarrativeArcBranchEffect : NarrativeArcEffect
    {
        public string SurvivorId { get; }
        public string BranchId { get; }

        public NarrativeArcBranchEffect(string survivorId, string branchId)
            : base(NarrativeArcEffectKind.NarrativeArcBranch, "narrative_arc_branch")
        {
            SurvivorId = survivorId ?? string.Empty;
            BranchId = branchId ?? string.Empty;
        }
    }

    public sealed class GainFactionIntelEffect : NarrativeArcEffect
    {
        public string FactionId { get; }

        public GainFactionIntelEffect(string factionId)
            : base(NarrativeArcEffectKind.GainFactionIntel, "gain_faction_intel")
        {
            FactionId = factionId ?? string.Empty;
        }
    }

    public sealed class StartExpeditionEffect : NarrativeArcEffect
    {
        public string LocationId { get; }

        public StartExpeditionEffect(string locationId)
            : base(NarrativeArcEffectKind.StartExpedition, "start_expedition")
        {
            LocationId = locationId ?? string.Empty;
        }
    }

    public sealed class FactionStandingEffect : NarrativeArcEffect
    {
        public string FactionId { get; }
        public int Delta { get; }

        public FactionStandingEffect(string factionId, int delta)
            : base(NarrativeArcEffectKind.FactionStanding, "faction_standing")
        {
            FactionId = factionId ?? string.Empty;
            Delta = delta;
        }
    }

    /// <summary>Represents an effect the runtime intentionally refuses to execute.</summary>
    public sealed class UnsupportedNarrativeArcEffect : NarrativeArcEffect
    {
        public string Reason { get; }

        public UnsupportedNarrativeArcEffect(string authoredType, string reason)
            : base(NarrativeArcEffectKind.Unsupported, authoredType)
        {
            Reason = reason ?? "unsupported effect";
        }
    }

    [Serializable]
    public sealed class NarrativeArcChoiceDefinition
    {
        public string ChoiceId { get; internal set; } = string.Empty;
        public string Text { get; internal set; } = string.Empty;
        public int MoraleDelta { get; internal set; }
        public IReadOnlyList<NarrativeArcEffect> Effects => _effects;
        public bool IsExecutable { get; internal set; } = true;
        public string ValidationError { get; internal set; } = string.Empty;

        private readonly List<NarrativeArcEffect> _effects = new List<NarrativeArcEffect>();

        internal void AddEffect(NarrativeArcEffect effect)
        {
            _effects.Add(effect);
            if (effect.Kind == NarrativeArcEffectKind.Unsupported)
            {
                IsExecutable = false;
                var unsupported = (UnsupportedNarrativeArcEffect)effect;
                ValidationError = string.IsNullOrEmpty(ValidationError)
                    ? unsupported.Reason
                    : ValidationError + "; " + unsupported.Reason;
            }
        }
    }

    [Serializable]
    public sealed class NarrativeArcEventDefinition
    {
        public string Id { get; internal set; } = string.Empty;
        public string Title { get; internal set; } = string.Empty;
        public string BodyText { get; internal set; } = string.Empty;
        public float Weight { get; internal set; }
        public int MinDay { get; internal set; }
        public string ArcId { get; internal set; } = string.Empty;
        public string RequiredSurvivorId { get; internal set; } = string.Empty;
        public int Stage { get; internal set; }
        public bool IsTerminal => Stage == 3;
        public IReadOnlyList<NarrativeArcChoiceDefinition> Choices => _choices;

        private readonly List<NarrativeArcChoiceDefinition> _choices =
            new List<NarrativeArcChoiceDefinition>();

        internal void AddChoice(NarrativeArcChoiceDefinition choice) => _choices.Add(choice);
    }

    [Serializable]
    public sealed class NarrativeArcProgressState
    {
        public string arcId = string.Empty;
        public string survivorId = string.Empty;
        public int currentStage;
        public string branchId = string.Empty;
        public bool branchCommitted;
        public bool complete;
    }

    [Serializable]
    public sealed class NarrativeArcResolutionRecord
    {
        public string eventId = string.Empty;
        public string choiceId = string.Empty;
        public int day;
    }

    [Serializable]
    public sealed class NarrativeArcEventState
    {
        public string systemId = NarrativeArcEventSystem.SystemId;
        public string pendingEventId = string.Empty;
        public int pendingDay;
        public List<string> completedEventIds = new List<string>();
        public List<NarrativeArcProgressState> arcs = new List<NarrativeArcProgressState>();
        public List<string> offeredExpeditionLocationIds = new List<string>();
        public List<NarrativeArcResolutionRecord> resolutions = new List<NarrativeArcResolutionRecord>();
    }

    /// <summary>Typed boundary from Core narrative state to existing host authorities.</summary>
    public interface INarrativeArcConsequencePort
    {
        bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason);
        void ApplyMorale(string survivorId, int delta, bool shelterWide);

        bool CanGrantFactionIntel(string canonicalFactionId, out string reason);
        void GrantFactionIntel(string canonicalFactionId);

        bool CanOfferExpedition(string locationId, out string reason);
        void OfferExpedition(string locationId);

        bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason);
        void ApplyFactionStanding(string canonicalFactionId, int delta);
    }

    /// <summary>Closed default port used by focused Core callers with no host authorities bound.</summary>
    public sealed class NullNarrativeArcConsequencePort : INarrativeArcConsequencePort
    {
        public static readonly NullNarrativeArcConsequencePort Instance = new NullNarrativeArcConsequencePort();

        private NullNarrativeArcConsequencePort() { }

        public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
        {
            reason = "morale authority is not bound";
            return false;
        }

        public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }

        public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
        {
            reason = "journal knowledge authority is not bound";
            return false;
        }

        public void GrantFactionIntel(string canonicalFactionId) { }

        public bool CanOfferExpedition(string locationId, out string reason)
        {
            reason = "expedition authority is not bound";
            return false;
        }

        public void OfferExpedition(string locationId) { }

        public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
        {
            reason = "faction standing authority is not bound";
            return false;
        }

        public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
    }

    public enum NarrativeArcChoiceStatus
    {
        Committed,
        AlreadyCommitted,
        Rejected
    }

    [Serializable]
    public sealed class NarrativeArcChoiceResult
    {
        public NarrativeArcChoiceStatus Status { get; internal set; }
        public string EventId { get; internal set; } = string.Empty;
        public string ChoiceId { get; internal set; } = string.Empty;
        public string Reason { get; internal set; } = string.Empty;
        public int MoraleDelta { get; internal set; }

        public bool Succeeded => Status == NarrativeArcChoiceStatus.Committed ||
                                 Status == NarrativeArcChoiceStatus.AlreadyCommitted;
    }

    /// <summary>Result of the typed optional narrative arc catalog load.</summary>
    public sealed class NarrativeArcEventCatalogLoadResult
    {
        public int SchemaVersion { get; internal set; }
        public List<NarrativeArcEventDefinition> Events { get; } = new List<NarrativeArcEventDefinition>();
        public List<string> Warnings { get; } = new List<string>();
        public List<string> Errors { get; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
    }

    /// <summary>
    /// Strict typed loader for the one Plan 143 catalog. The DTOs are only a
    /// parsing boundary; execution receives the discriminated effect classes.
    /// </summary>
    public static class NarrativeArcEventCatalogLoader
    {
        public const string FileName = "narrative_arc_events.json";

        private sealed class RootDto
        {
            public int schema_version { get; set; }
            public List<EventDto>? events { get; set; }
        }

        private sealed class EventDto
        {
            public string? id { get; set; }
            public string? title { get; set; }
            public string? bodyText { get; set; }
            public float weight { get; set; }
            public int minDay { get; set; }
            public List<ChoiceDto>? choices { get; set; }
        }

        private sealed class ChoiceDto
        {
            public string? choiceId { get; set; }
            public string? text { get; set; }
            public int moraleDelta { get; set; }
            public List<EffectDto>? effects { get; set; }
        }

        private sealed class EffectDto
        {
            public string? type { get; set; }
            public string? survivorId { get; set; }
            public string? branchId { get; set; }
            public string? factionId { get; set; }
            public string? locationId { get; set; }
            public int delta { get; set; }
        }

        private sealed class ArcSpec
        {
            public string ArcId = string.Empty;
            public string SurvivorId = string.Empty;
            public int Stage;
        }

        private static readonly Dictionary<string, ArcSpec> Specs = BuildSpecs();

        private static Dictionary<string, ArcSpec> BuildSpecs()
        {
            var specs = new Dictionary<string, ArcSpec>(StringComparer.Ordinal);
            AddArc(specs, "aris_thorne", "aris_thorne",
                "narrative_aris_thorne_stage_1", "narrative_aris_thorne_stage_2", "narrative_aris_thorne_stage_3");
            AddArc(specs, "maya_lin", "maya_lin",
                "narrative_maya_lin_stage_1", "narrative_maya_lin_stage_2", "narrative_maya_lin_stage_3");
            AddArc(specs, "victor_vance", "victor_vance",
                "narrative_victor_vance_stage_1", "narrative_victor_vance_stage_2", "narrative_victor_vance_stage_3");
            AddArc(specs, "elena_rostov", "elena_rostov",
                "narrative_elena_rostov_stage_1", "narrative_elena_rostov_stage_2", "narrative_elena_rostov_stage_3");
            specs["narrative_garrison_defector_intel"] = new ArcSpec();
            specs["narrative_cult_prophet_rumor"] = new ArcSpec();
            specs["narrative_militia_council_invitation"] = new ArcSpec();
            return specs;
        }

        private static void AddArc(
            Dictionary<string, ArcSpec> specs,
            string arcId,
            string survivorId,
            string stage1,
            string stage2,
            string stage3)
        {
            specs[stage1] = new ArcSpec { ArcId = arcId, SurvivorId = survivorId, Stage = 1 };
            specs[stage2] = new ArcSpec { ArcId = arcId, SurvivorId = survivorId, Stage = 2 };
            specs[stage3] = new ArcSpec { ArcId = arcId, SurvivorId = survivorId, Stage = 3 };
        }

        public static List<NarrativeArcEventDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var loaded = LoadDetailed(dataDir, fileIO, json, null);
            return loaded.IsSuccess ? loaded.Events : new List<NarrativeArcEventDefinition>();
        }

        public static NarrativeArcEventCatalogLoadResult LoadDetailed(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json,
            ContentUtilizationInstrumentation? instrumentation = null)
        {
            var result = new NarrativeArcEventCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("narrative arc catalog requires a data directory, file IO, and JSON serializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Warnings.Add("optional catalog file not found: " + FileName);
                return result;
            }

            instrumentation?.RecordCatalogOpened(FileName, nameof(NarrativeArcEventCatalogLoader));

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

            instrumentation?.RecordCatalogDeserialized(FileName, root.events.Count);
            var seenEvents = new HashSet<string>(StringComparer.Ordinal);
            var seenSpecs = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.events.Count; i++)
            {
                var dto = root.events[i];
                if (dto == null)
                {
                    result.Errors.Add($"event[{i}] is null");
                    continue;
                }

                string eventId = dto.id?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(eventId))
                {
                    result.Errors.Add($"event[{i}] has no id");
                    continue;
                }
                if (!seenEvents.Add(eventId))
                {
                    result.Errors.Add("duplicate event id: " + eventId);
                    continue;
                }
                if (!Specs.TryGetValue(eventId, out var spec))
                {
                    result.Errors.Add("event id is outside the Plan 143 catalog contract: " + eventId);
                    continue;
                }
                seenSpecs.Add(eventId);

                if (string.IsNullOrWhiteSpace(dto.title)) result.Errors.Add(eventId + " has no title");
                if (float.IsNaN(dto.weight) || float.IsInfinity(dto.weight) || dto.weight <= 0f)
                    result.Errors.Add(eventId + " must have a finite positive weight");
                if (dto.minDay < 0) result.Errors.Add(eventId + " has a negative minDay");

                var def = new NarrativeArcEventDefinition
                {
                    Id = eventId,
                    Title = dto.title?.Trim() ?? string.Empty,
                    BodyText = dto.bodyText ?? string.Empty,
                    Weight = dto.weight,
                    MinDay = dto.minDay,
                    ArcId = spec.ArcId,
                    RequiredSurvivorId = spec.SurvivorId,
                    Stage = spec.Stage
                };

                ParseChoices(dto, def, result);
                result.Events.Add(def);
            }

            foreach (var kv in Specs)
            {
                if (!seenSpecs.Contains(kv.Key))
                    result.Errors.Add("required Plan 143 event is missing: " + kv.Key);
            }

            ValidateArcShape(result);
            if (result.Errors.Count > 0)
            {
                result.Events.Clear();
                return result;
            }

            result.Events.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            instrumentation?.RecordDefinitionsRegistered(
                FileName, nameof(NarrativeArcEventSystem) + ".Catalog", result.Events.Count);
            return result;
        }

        private static void ParseChoices(
            EventDto dto,
            NarrativeArcEventDefinition def,
            NarrativeArcEventCatalogLoadResult result)
        {
            if (dto.choices == null)
            {
                result.Errors.Add(def.Id + " must contain a choices array");
                return;
            }

            var seenChoices = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < dto.choices.Count; i++)
            {
                var dtoChoice = dto.choices[i];
                if (dtoChoice == null)
                {
                    result.Errors.Add(def.Id + " choice[" + i + "] is null");
                    continue;
                }

                string choiceId = dtoChoice.choiceId?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(choiceId))
                {
                    result.Errors.Add(def.Id + " choice[" + i + "] has no choiceId");
                    continue;
                }
                if (!seenChoices.Add(choiceId))
                {
                    result.Errors.Add("duplicate choice id in " + def.Id + ": " + choiceId);
                    continue;
                }

                var choice = new NarrativeArcChoiceDefinition
                {
                    ChoiceId = choiceId,
                    Text = dtoChoice.text ?? string.Empty,
                    MoraleDelta = dtoChoice.moraleDelta
                };
                if (string.IsNullOrWhiteSpace(choice.Text))
                    result.Errors.Add(def.Id + "/" + choiceId + " has no text");

                if (dtoChoice.effects == null)
                {
                    result.Errors.Add(def.Id + "/" + choiceId + " must contain an effects array");
                }
                else
                {
                    for (int effectIndex = 0; effectIndex < dtoChoice.effects.Count; effectIndex++)
                    {
                        var effectDto = dtoChoice.effects[effectIndex];
                        choice.AddEffect(ParseEffect(effectDto));
                    }
                }
                def.AddChoice(choice);
            }
        }

        private static NarrativeArcEffect ParseEffect(EffectDto? dto)
        {
            if (dto == null)
                return new UnsupportedNarrativeArcEffect(string.Empty, "null effect record");

            string type = dto.type?.Trim() ?? string.Empty;
            switch (type)
            {
                case "advance_narrative_arc":
                    return string.IsNullOrWhiteSpace(dto.survivorId)
                        ? new UnsupportedNarrativeArcEffect(type, "advance_narrative_arc requires survivorId")
                        : new AdvanceNarrativeArcEffect(dto.survivorId.Trim());
                case "narrative_arc_branch":
                    return string.IsNullOrWhiteSpace(dto.survivorId) || string.IsNullOrWhiteSpace(dto.branchId)
                        ? new UnsupportedNarrativeArcEffect(type, "narrative_arc_branch requires survivorId and branchId")
                        : new NarrativeArcBranchEffect(dto.survivorId.Trim(), dto.branchId.Trim());
                case "gain_faction_intel":
                    if (string.IsNullOrWhiteSpace(dto.factionId))
                        return new UnsupportedNarrativeArcEffect(type, "gain_faction_intel requires factionId");
                    if (!FactionStandingIdResolver.TryToSystemsId(dto.factionId.Trim(), out var intelFaction))
                        return new UnsupportedNarrativeArcEffect(type, "unknown factionId: " + dto.factionId.Trim());
                    return new GainFactionIntelEffect(intelFaction);
                case "start_expedition":
                    return string.IsNullOrWhiteSpace(dto.locationId)
                        ? new UnsupportedNarrativeArcEffect(type, "start_expedition requires locationId")
                        : new StartExpeditionEffect(dto.locationId.Trim());
                case "faction_standing":
                    if (string.IsNullOrWhiteSpace(dto.factionId))
                        return new UnsupportedNarrativeArcEffect(type, "faction_standing requires factionId");
                    if (!FactionStandingIdResolver.TryToSystemsId(dto.factionId.Trim(), out var standingFaction))
                        return new UnsupportedNarrativeArcEffect(type, "unknown factionId: " + dto.factionId.Trim());
                    if (dto.delta < -100 || dto.delta > 100)
                        return new UnsupportedNarrativeArcEffect(type, "faction_standing delta must be between -100 and 100");
                    return new FactionStandingEffect(standingFaction, dto.delta);
                default:
                    return new UnsupportedNarrativeArcEffect(
                        type,
                        string.IsNullOrEmpty(type) ? "effect type is missing" : "unknown effect type: " + type);
            }
        }

        private static void ValidateArcShape(NarrativeArcEventCatalogLoadResult result)
        {
            foreach (string arcId in new[] { "aris_thorne", "maya_lin", "victor_vance", "elena_rostov" })
            {
                var arcEvents = result.Events.Where(e => e.ArcId == arcId).OrderBy(e => e.Stage).ToList();
                if (arcEvents.Count != 3 || arcEvents[0].Stage != 1 || arcEvents[1].Stage != 2 || arcEvents[2].Stage != 3)
                {
                    result.Errors.Add("arc " + arcId + " must contain contiguous stages 1, 2, and 3");
                    continue;
                }

                var stage1 = arcEvents[0];
                if (!stage1.Choices.SelectMany(c => c.Effects)
                    .OfType<AdvanceNarrativeArcEffect>()
                    .Any(effect => effect.SurvivorId == stage1.RequiredSurvivorId))
                    result.Errors.Add(stage1.Id + " must advance its arc");

                var stage2 = arcEvents[1];
                var branches = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < stage2.Choices.Count; i++)
                {
                    var branchEffects = stage2.Choices[i].Effects.OfType<NarrativeArcBranchEffect>().ToList();
                    var branch = branchEffects.FirstOrDefault();
                    if (branchEffects.Count != 1 || branch == null || branch.SurvivorId != stage2.RequiredSurvivorId ||
                        (branch.BranchId != "a" && branch.BranchId != "b"))
                    {
                        result.Errors.Add(stage2.Id + "/" + stage2.Choices[i].ChoiceId + " must carry one branch a or b for its survivor");
                    }
                    else if (!branches.Add(branch.BranchId))
                    {
                        result.Errors.Add(stage2.Id + " repeats branch " + branch.BranchId);
                    }
                }
                if (stage2.Choices.Count != 2 || branches.Count != 2)
                    result.Errors.Add(stage2.Id + " must expose mutually exclusive branches a and b");

                if (arcEvents[2].Choices.Count != 0)
                    result.Errors.Add(arcEvents[2].Id + " must be a terminal acknowledgement event");
            }
        }

    }

    /// <summary>
    /// Deterministic Plan 143 runtime. It owns only candidate selection,
    /// bounded arc progress, pending presentation, and choice completion.
    /// All cross-system consequences pass through typed preflight/commit ports.
    /// </summary>
    public sealed class NarrativeArcEventSystem
    {
        public const string SystemId = "narrative_arc_event_system";
        public const string CatalogFileName = NarrativeArcEventCatalogLoader.FileName;

        private NarrativeArcEventState _state;
        private readonly List<NarrativeArcEventDefinition> _catalog = new List<NarrativeArcEventDefinition>();

        public event Action<NarrativeArcEventDefinition>? OnEventSelected;
        public event Action<NarrativeArcChoiceResult>? OnChoiceCommitted;
        public event Action<NarrativeArcEventState>? OnStateChanged;

        public ContentUtilizationInstrumentation? Instrumentation { get; set; }
        public Func<string, bool> SurvivorIsPresent { get; set; } = _ => false;
        public INarrativeArcConsequencePort Consequences { get; set; } = NullNarrativeArcConsequencePort.Instance;

        public NarrativeArcEventSystem(NarrativeArcEventState? state = null)
        {
            _state = state ?? new NarrativeArcEventState();
            NormalizeState(_state);
        }

        public NarrativeArcEventSystem(
            IEnumerable<NarrativeArcEventDefinition> catalog,
            NarrativeArcEventState? state = null,
            ContentUtilizationInstrumentation? instrumentation = null)
            : this(state)
        {
            Instrumentation = instrumentation;
            RegisterRange(catalog);
        }

        public NarrativeArcEventState State => _state;
        public IReadOnlyList<NarrativeArcEventDefinition> Catalog => _catalog;
        public NarrativeArcEventDefinition? PendingEvent => Find(_state.pendingEventId);
        public bool HasPendingEvent => !string.IsNullOrEmpty(_state.pendingEventId) && PendingEvent != null;
        public bool HasPersistedState =>
            !string.IsNullOrEmpty(_state.pendingEventId) ||
            _state.completedEventIds.Count > 0 ||
            _state.arcs.Count > 0 ||
            _state.offeredExpeditionLocationIds.Count > 0 ||
            _state.resolutions.Count > 0;

        public void RegisterRange(IEnumerable<NarrativeArcEventDefinition>? definitions)
        {
            if (definitions == null) return;
            foreach (var definition in definitions)
            {
                if (definition == null || string.IsNullOrEmpty(definition.Id)) continue;
                if (_catalog.Any(e => e.Id == definition.Id)) continue;
                _catalog.Add(definition);
            }
            _catalog.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
        }

        public NarrativeArcEventDefinition? Find(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return null;
            for (int i = 0; i < _catalog.Count; i++)
            {
                if (_catalog[i].Id == eventId)
                {
                    Instrumentation?.RecordDefinitionQueried(
                        CatalogFileName, eventId, nameof(Find), nameof(NarrativeArcEventSystem));
                    return _catalog[i];
                }
            }
            return null;
        }

        public IReadOnlyList<(NarrativeArcEventDefinition Event, double Weight)> GetEligibleCandidates(int day)
        {
            var result = new List<(NarrativeArcEventDefinition, double)>();
            for (int i = 0; i < _catalog.Count; i++)
            {
                var definition = _catalog[i];
                Instrumentation?.RecordDefinitionQueried(
                    CatalogFileName, definition.Id, "GetEligibleCandidates", nameof(NarrativeArcEventSystem), day);
                if (IsEligible(definition, day))
                    result.Add((definition, definition.Weight));
            }
            return result;
        }

        public NarrativeArcEventDefinition? SelectForDay(int day, ISeededRng rng)
        {
            if (rng == null) return null;
            ReconcilePending(day);
            if (HasPendingEvent) return PendingEvent;

            var candidates = GetEligibleCandidates(day);
            if (candidates.Count == 0) return null;

            double total = 0d;
            for (int i = 0; i < candidates.Count; i++) total += candidates[i].Weight;
            if (total <= 0d || double.IsNaN(total) || double.IsInfinity(total)) return null;

            double roll = rng.NextDouble() * total;
            double accumulated = 0d;
            for (int i = 0; i < candidates.Count; i++)
            {
                accumulated += candidates[i].Weight;
                if (roll < accumulated || i == candidates.Count - 1)
                {
                    var selected = candidates[i].Event;
                    _state.pendingEventId = selected.Id;
                    _state.pendingDay = day;
                    OnEventSelected?.Invoke(selected);
                    Instrumentation?.RecordDefinitionSelected(
                        CatalogFileName, selected.Id, nameof(NarrativeArcEventSystem), day);
                    RaiseChanged();
                    return selected;
                }
            }
            return null;
        }

        public NarrativeArcChoiceResult CanApplyChoice(string eventId, string choiceId, int day)
        {
            var result = new NarrativeArcChoiceResult
            {
                Status = NarrativeArcChoiceStatus.Rejected,
                EventId = eventId ?? string.Empty,
                ChoiceId = choiceId ?? string.Empty
            };

            var definition = Find(eventId ?? string.Empty);
            if (definition == null) return Reject(result, "unknown narrative arc event");
            if (IsCompleted(definition.Id)) return AlreadyCommitted(result, "event already resolved");
            if (!string.Equals(_state.pendingEventId, definition.Id, StringComparison.Ordinal))
                return Reject(result, "event is not the pending narrative choice");
            if (day < definition.MinDay) return Reject(result, "event is not available yet");
            if (!IsEligible(definition, day)) return Reject(result, "event prerequisites are no longer satisfied");

            NarrativeArcChoiceDefinition? choice = null;
            for (int i = 0; i < definition.Choices.Count; i++)
                if (definition.Choices[i].ChoiceId == choiceId) { choice = definition.Choices[i]; break; }
            if (choice == null) return Reject(result, "unknown choice");
            result.MoraleDelta = choice.MoraleDelta;
            if (!choice.IsExecutable) return Reject(result, choice.ValidationError);
            if (!ValidateArcTransition(definition, choice, out var transitionReason))
                return Reject(result, transitionReason);

            bool shelterWideMorale = string.IsNullOrEmpty(definition.RequiredSurvivorId);
            if (choice.MoraleDelta != 0 && !Consequences.CanApplyMorale(
                    definition.RequiredSurvivorId, choice.MoraleDelta, shelterWideMorale, out var moraleReason))
                return Reject(result, moraleReason);

            for (int i = 0; i < choice.Effects.Count; i++)
            {
                if (!CanApplyEffect(definition, choice.Effects[i], out var effectReason))
                    return Reject(result, effectReason);
            }
            result.Status = NarrativeArcChoiceStatus.Committed;
            return result;
        }

        public NarrativeArcChoiceResult CommitChoice(string eventId, string choiceId, int day)
        {
            var preflight = CanApplyChoice(eventId, choiceId, day);
            if (preflight.Status != NarrativeArcChoiceStatus.Committed)
                return preflight;

            var definition = Find(eventId ?? string.Empty)!;
            NarrativeArcChoiceDefinition choice = definition.Choices.First(c => c.ChoiceId == choiceId);
            var next = CloneState(_state);
            ApplyInternalTransition(next, definition, choice, day);

            // Every call below has passed its preflight. Ports are deliberately
            // void on commit: a host adapter must not reject after the barrier.
            if (choice.MoraleDelta != 0)
                Consequences.ApplyMorale(
                    definition.RequiredSurvivorId,
                    choice.MoraleDelta,
                    string.IsNullOrEmpty(definition.RequiredSurvivorId));
            for (int i = 0; i < choice.Effects.Count; i++)
                CommitEffect(choice.Effects[i], next);

            _state = next;
            var committed = new NarrativeArcChoiceResult
            {
                Status = NarrativeArcChoiceStatus.Committed,
                EventId = definition.Id,
                ChoiceId = choice.ChoiceId,
                MoraleDelta = choice.MoraleDelta
            };
            OnChoiceCommitted?.Invoke(committed);
            Instrumentation?.RecordDefinitionConsumed(
                CatalogFileName,
                definition.Id,
                nameof(NarrativeArcEventSystem),
                "choice=" + choice.ChoiceId + "; morale=" + choice.MoraleDelta.ToString(CultureInfo.InvariantCulture),
                day);
            RaiseChanged();
            return committed;
        }

        public NarrativeArcChoiceResult AcknowledgeEvent(string eventId, int day)
        {
            var definition = Find(eventId ?? string.Empty);
            var result = new NarrativeArcChoiceResult
            {
                Status = NarrativeArcChoiceStatus.Rejected,
                EventId = eventId ?? string.Empty
            };
            if (definition == null) return Reject(result, "unknown narrative arc event");
            if (IsCompleted(definition.Id)) return AlreadyCommitted(result, "event already acknowledged");
            if (definition.Choices.Count != 0) return Reject(result, "event requires a choice");
            if (!string.Equals(_state.pendingEventId, definition.Id, StringComparison.Ordinal))
                return Reject(result, "event is not the pending narrative event");
            if (!IsEligible(definition, day)) return Reject(result, "event prerequisites are no longer satisfied");

            var next = CloneState(_state);
            MarkCompleted(next, definition.Id);
            if (!string.IsNullOrEmpty(definition.ArcId))
            {
                var progress = GetOrCreateProgress(next, definition.ArcId, definition.RequiredSurvivorId);
                progress.currentStage = 3;
                progress.complete = true;
            }
            next.pendingEventId = string.Empty;
            next.pendingDay = 0;
            _state = next;

            result.Status = NarrativeArcChoiceStatus.Committed;
            result.ChoiceId = string.Empty;
            OnChoiceCommitted?.Invoke(result);
            Instrumentation?.RecordDefinitionConsumed(
                CatalogFileName, definition.Id, nameof(NarrativeArcEventSystem), "acknowledged", day);
            RaiseChanged();
            return result;
        }

        public void ReconcilePending(int day)
        {
            if (!HasPendingEvent) return;
            var pending = PendingEvent;
            if (pending == null || !IsEligible(pending, day))
            {
                _state.pendingEventId = string.Empty;
                _state.pendingDay = 0;
                RaiseChanged();
            }
        }

        public bool IsCompleted(string eventId)
        {
            return !string.IsNullOrEmpty(eventId) && _state.completedEventIds.Contains(eventId);
        }

        public bool HasExpeditionOffer(string locationId)
        {
            return !string.IsNullOrEmpty(locationId) && _state.offeredExpeditionLocationIds.Contains(locationId);
        }

        public NarrativeArcEventState CaptureState() => CloneState(_state);

        public void RestoreState(NarrativeArcEventState? saved)
        {
            _state = saved == null ? new NarrativeArcEventState() : CloneState(saved);
            NormalizeState(_state);
            RaiseChanged();
        }

        private bool IsEligible(NarrativeArcEventDefinition definition, int day)
        {
            if (definition == null || day < definition.MinDay || IsCompleted(definition.Id)) return false;
            if (!string.IsNullOrEmpty(definition.RequiredSurvivorId) &&
                !SurvivorIsPresent(definition.RequiredSurvivorId)) return false;

            if (string.IsNullOrEmpty(definition.ArcId)) return true;
            var progress = FindProgress(definition.ArcId);
            int expectedStage = progress == null ? 1 : progress.currentStage;
            return expectedStage == definition.Stage && (progress == null || !progress.complete);
        }

        private bool ValidateArcTransition(
            NarrativeArcEventDefinition definition,
            NarrativeArcChoiceDefinition choice,
            out string reason)
        {
            reason = string.Empty;
            if (definition.Stage == 1)
            {
                if (!choice.Effects.OfType<AdvanceNarrativeArcEffect>().Any(e => e.SurvivorId == definition.RequiredSurvivorId))
                {
                    reason = "stage 1 choice does not advance its bound survivor arc";
                    return false;
                }
            }
            else if (definition.Stage == 2)
            {
                var branchEffects = choice.Effects.OfType<NarrativeArcBranchEffect>().ToList();
                var branch = branchEffects.FirstOrDefault();
                if (branchEffects.Count != 1 || branch == null || branch.SurvivorId != definition.RequiredSurvivorId ||
                    (branch.BranchId != "a" && branch.BranchId != "b"))
                {
                    reason = "stage 2 choice has no valid branch";
                    return false;
                }
                var progress = FindProgress(definition.ArcId);
                if (progress != null && progress.branchCommitted)
                {
                    reason = "arc branch is already committed";
                    return false;
                }
            }
            else if (definition.Stage == 3 && definition.Choices.Count == 0)
            {
                reason = "terminal event must be acknowledged without a choice";
                return false;
            }
            return true;
        }

        private bool CanApplyEffect(
            NarrativeArcEventDefinition definition,
            NarrativeArcEffect effect,
            out string reason)
        {
            switch (effect)
            {
                case AdvanceNarrativeArcEffect advance:
                    if (definition.Stage != 1 ||
                        !string.Equals(advance.SurvivorId, definition.RequiredSurvivorId, StringComparison.Ordinal))
                    {
                        reason = "advance_narrative_arc is outside its bound stage or survivor";
                        return false;
                    }
                    reason = string.Empty;
                    return true;
                case NarrativeArcBranchEffect branch:
                    if (definition.Stage != 2 ||
                        !string.Equals(branch.SurvivorId, definition.RequiredSurvivorId, StringComparison.Ordinal) ||
                        (branch.BranchId != "a" && branch.BranchId != "b"))
                    {
                        reason = "narrative_arc_branch is outside its bound stage, survivor, or branch set";
                        return false;
                    }
                    reason = string.Empty;
                    return true;
                case GainFactionIntelEffect intel:
                    if (!string.IsNullOrEmpty(definition.ArcId) ||
                        !IsCanonicalFactionId(intel.FactionId))
                    {
                        reason = "gain_faction_intel requires an independent event and canonical faction";
                        return false;
                    }
                    return Consequences.CanGrantFactionIntel(intel.FactionId, out reason);
                case StartExpeditionEffect expedition:
                    if (!string.IsNullOrEmpty(definition.ArcId) || string.IsNullOrEmpty(expedition.LocationId))
                    {
                        reason = "start_expedition requires an independent event and a location";
                        return false;
                    }
                    return Consequences.CanOfferExpedition(expedition.LocationId, out reason);
                case FactionStandingEffect standing:
                    if (!string.IsNullOrEmpty(definition.ArcId) ||
                        standing.Delta < -100 || standing.Delta > 100 ||
                        !IsCanonicalFactionId(standing.FactionId))
                    {
                        reason = "faction_standing requires an independent event, canonical faction, and delta between -100 and 100";
                        return false;
                    }
                    return Consequences.CanApplyFactionStanding(standing.FactionId, standing.Delta, out reason);
                default:
                    reason = effect is UnsupportedNarrativeArcEffect unsupported
                        ? unsupported.Reason
                        : "effect is outside the Plan 143 whitelist";
                    return false;
            }
        }

        private static bool IsCanonicalFactionId(string factionId)
        {
            return FactionStandingIdResolver.TryToSystemsId(factionId, out var canonical) &&
                string.Equals(factionId, canonical, StringComparison.Ordinal);
        }

        private void CommitEffect(NarrativeArcEffect effect, NarrativeArcEventState next)
        {
            switch (effect)
            {
                case AdvanceNarrativeArcEffect _:
                    break;
                case NarrativeArcBranchEffect _:
                    break;
                case GainFactionIntelEffect intel:
                    Consequences.GrantFactionIntel(intel.FactionId);
                    break;
                case StartExpeditionEffect expedition:
                    Consequences.OfferExpedition(expedition.LocationId);
                    if (!next.offeredExpeditionLocationIds.Contains(expedition.LocationId))
                        next.offeredExpeditionLocationIds.Add(expedition.LocationId);
                    break;
                case FactionStandingEffect standing:
                    Consequences.ApplyFactionStanding(standing.FactionId, standing.Delta);
                    break;
            }
        }

        private static void ApplyInternalTransition(
            NarrativeArcEventState next,
            NarrativeArcEventDefinition definition,
            NarrativeArcChoiceDefinition choice,
            int day)
        {
            MarkCompleted(next, definition.Id);
            next.pendingEventId = string.Empty;
            next.pendingDay = 0;
            next.resolutions.Add(new NarrativeArcResolutionRecord
            {
                eventId = definition.Id,
                choiceId = choice.ChoiceId,
                day = day
            });

            if (string.IsNullOrEmpty(definition.ArcId)) return;
            var progress = GetOrCreateProgress(next, definition.ArcId, definition.RequiredSurvivorId);
            if (definition.Stage == 1)
            {
                progress.currentStage = 2;
            }
            else if (definition.Stage == 2)
            {
                var branch = choice.Effects.OfType<NarrativeArcBranchEffect>().Single();
                progress.currentStage = 3;
                progress.branchId = branch.BranchId;
                progress.branchCommitted = true;
            }
        }

        private static void MarkCompleted(NarrativeArcEventState state, string eventId)
        {
            if (!state.completedEventIds.Contains(eventId)) state.completedEventIds.Add(eventId);
            state.completedEventIds.Sort(StringComparer.Ordinal);
        }

        private NarrativeArcProgressState? FindProgress(string arcId)
        {
            for (int i = 0; i < _state.arcs.Count; i++)
                if (_state.arcs[i].arcId == arcId) return _state.arcs[i];
            return null;
        }

        private static NarrativeArcProgressState GetOrCreateProgress(
            NarrativeArcEventState state,
            string arcId,
            string survivorId)
        {
            for (int i = 0; i < state.arcs.Count; i++)
                if (state.arcs[i].arcId == arcId) return state.arcs[i];
            var created = new NarrativeArcProgressState
            {
                arcId = arcId,
                survivorId = survivorId,
                currentStage = 1
            };
            state.arcs.Add(created);
            state.arcs.Sort((a, b) => string.CompareOrdinal(a.arcId, b.arcId));
            return created;
        }

        private static void NormalizeState(NarrativeArcEventState state)
        {
            state.systemId = SystemId;
            state.completedEventIds ??= new List<string>();
            state.arcs ??= new List<NarrativeArcProgressState>();
            state.offeredExpeditionLocationIds ??= new List<string>();
            state.resolutions ??= new List<NarrativeArcResolutionRecord>();
            state.completedEventIds = state.completedEventIds
                .Where(id => !string.IsNullOrEmpty(id)).Distinct(StringComparer.Ordinal).OrderBy(id => id, StringComparer.Ordinal).ToList();
            state.offeredExpeditionLocationIds = state.offeredExpeditionLocationIds
                .Where(id => !string.IsNullOrEmpty(id)).Distinct(StringComparer.Ordinal).OrderBy(id => id, StringComparer.Ordinal).ToList();
            state.arcs = state.arcs.Where(a => a != null && !string.IsNullOrEmpty(a.arcId))
                .OrderBy(a => a.arcId, StringComparer.Ordinal).ToList();
            state.resolutions = state.resolutions.Where(r => r != null && !string.IsNullOrEmpty(r.eventId))
                .OrderBy(r => r.day).ThenBy(r => r.eventId, StringComparer.Ordinal).ThenBy(r => r.choiceId, StringComparer.Ordinal).ToList();
        }

        private static NarrativeArcEventState CloneState(NarrativeArcEventState source)
        {
            var copy = new NarrativeArcEventState
            {
                systemId = source.systemId,
                pendingEventId = source.pendingEventId ?? string.Empty,
                pendingDay = source.pendingDay,
                completedEventIds = source.completedEventIds != null ? new List<string>(source.completedEventIds) : new List<string>(),
                arcs = new List<NarrativeArcProgressState>(),
                offeredExpeditionLocationIds = source.offeredExpeditionLocationIds != null
                    ? new List<string>(source.offeredExpeditionLocationIds)
                    : new List<string>(),
                resolutions = new List<NarrativeArcResolutionRecord>()
            };
            if (source.arcs != null)
                foreach (var arc in source.arcs)
                    if (arc != null) copy.arcs.Add(new NarrativeArcProgressState
                    {
                        arcId = arc.arcId,
                        survivorId = arc.survivorId,
                        currentStage = arc.currentStage,
                        branchId = arc.branchId,
                        branchCommitted = arc.branchCommitted,
                        complete = arc.complete
                    });
            if (source.resolutions != null)
                foreach (var resolution in source.resolutions)
                    if (resolution != null) copy.resolutions.Add(new NarrativeArcResolutionRecord
                    {
                        eventId = resolution.eventId,
                        choiceId = resolution.choiceId,
                        day = resolution.day
                    });
            NormalizeState(copy);
            return copy;
        }

        private static NarrativeArcChoiceResult Reject(NarrativeArcChoiceResult result, string reason)
        {
            result.Status = NarrativeArcChoiceStatus.Rejected;
            result.Reason = string.IsNullOrEmpty(reason) ? "choice rejected" : reason;
            return result;
        }

        private static NarrativeArcChoiceResult AlreadyCommitted(NarrativeArcChoiceResult result, string reason)
        {
            result.Status = NarrativeArcChoiceStatus.AlreadyCommitted;
            result.Reason = reason;
            return result;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
