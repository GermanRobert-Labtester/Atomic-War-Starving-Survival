// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Content;

namespace Ashfall.Core.Narrative
{
    /// <summary>One authored, one-time field echo from echoes.json.</summary>
    public sealed class EchoDefinition
    {
        public string Id { get; internal set; } = string.Empty;
        public string Title { get; internal set; } = string.Empty;
        public string BodyText { get; internal set; } = string.Empty;
        public double Weight { get; internal set; }
        public int MinDay { get; internal set; }
        public EchoConditions Conditions { get; internal set; } = new EchoConditions();
        public List<EchoChoiceDefinition> Choices { get; } = new List<EchoChoiceDefinition>();
    }

    public sealed class EchoConditions
    {
        public int MinDay { get; internal set; }
        public string RequiredFlagId { get; internal set; } = string.Empty;
    }

    public sealed class EchoChoiceDefinition
    {
        public string ChoiceId { get; internal set; } = string.Empty;
        public string Text { get; internal set; } = string.Empty;
        public double MoraleDelta { get; internal set; }
        public List<EchoEffectDefinition> Effects { get; } = new List<EchoEffectDefinition>();
        public EchoDelayedConsequence? DelayedConsequence { get; internal set; }

        public bool IsExecutable =>
            !string.IsNullOrWhiteSpace(ChoiceId) &&
            !string.IsNullOrWhiteSpace(Text) &&
            (Math.Abs(MoraleDelta) > double.Epsilon || Effects.Count > 0 || DelayedConsequence != null);
    }

    public sealed class EchoEffectDefinition
    {
        public string ItemId { get; internal set; } = string.Empty;
        public int ItemAmount { get; internal set; }
        public string TargetNeed { get; internal set; } = string.Empty;
        public double NeedDelta { get; internal set; }
        public string SetWorldFlag { get; internal set; } = string.Empty;
        public bool WorldFlagValue { get; internal set; }

        public bool HasRuntimeEffect =>
            (!string.IsNullOrWhiteSpace(ItemId) && ItemAmount != 0) ||
            (!string.IsNullOrWhiteSpace(TargetNeed) && Math.Abs(NeedDelta) > double.Epsilon) ||
            !string.IsNullOrWhiteSpace(SetWorldFlag);
    }

    public sealed class EchoDelayedConsequence
    {
        public double DelayHours { get; internal set; }
        public string Title { get; internal set; } = string.Empty;
        public string Description { get; internal set; } = string.Empty;
        public List<EchoEffectDefinition> Effects { get; } = new List<EchoEffectDefinition>();
    }

    public sealed class EchoCatalogLoadResult
    {
        public int SchemaVersion { get; internal set; }
        public List<EchoDefinition> Echoes { get; } = new List<EchoDefinition>();
        public List<string> Warnings { get; } = new List<string>();
        public List<string> Errors { get; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
    }

    /// <summary>
    /// Strict loader for the authored echoes.json shape. The DTOs stay private
    /// so mutable JSON transport details never leak into runtime authority.
    /// </summary>
    public static class EchoCatalogLoader
    {
        public const string FileName = "echoes.json";

        private sealed class RootDto
        {
            public int schema_version { get; set; }
            public List<EchoDto>? echoes { get; set; }
        }

        private sealed class EchoDto
        {
            public string? id { get; set; }
            public string? title { get; set; }
            public string? bodyText { get; set; }
            public double weight { get; set; }
            public int minDay { get; set; }
            public ConditionsDto? conditions { get; set; }
            public List<ChoiceDto>? choices { get; set; }
        }

        private sealed class ConditionsDto
        {
            public int MinDay { get; set; }
            public string? RequiredFlagId { get; set; }
        }

        private sealed class ChoiceDto
        {
            public string? choiceId { get; set; }
            public string? text { get; set; }
            public double moraleDelta { get; set; }
            public List<EffectDto>? effects { get; set; }
            public DelayedDto? delayedConsequence { get; set; }
        }

        private sealed class EffectDto
        {
            public string? itemId { get; set; }
            public int itemAmount { get; set; }
            public string? targetNeed { get; set; }
            public double needDelta { get; set; }
            public string? setWorldFlag { get; set; }
            public bool worldFlagValue { get; set; }
            public DelayedDto? delayedConsequence { get; set; }
        }

        private sealed class DelayedDto
        {
            public double delayHours { get; set; }
            public string? title { get; set; }
            public string? description { get; set; }
            public List<EffectDto>? effects { get; set; }
        }

        public static List<EchoDefinition> Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            var result = LoadDetailed(dataDir, fileIO, json, null);
            return result.IsSuccess ? result.Echoes : new List<EchoDefinition>();
        }

        public static EchoCatalogLoadResult LoadDetailed(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json,
            ContentUtilizationInstrumentation? instrumentation = null)
        {
            var result = new EchoCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrWhiteSpace(dataDir))
            {
                result.Errors.Add("echo catalog requires a data directory, file IO, and JSON serializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Warnings.Add("optional catalog file not found: " + FileName);
                return result;
            }

            instrumentation?.RecordCatalogOpened(FileName, nameof(EchoCatalogLoader));
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
            if (root.echoes == null)
            {
                result.Errors.Add("catalog root must contain an echoes array");
                return result;
            }

            instrumentation?.RecordCatalogDeserialized(FileName, root.echoes.Count);
            var seenEchoes = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.echoes.Count; i++)
            {
                var dto = root.echoes[i];
                if (dto == null)
                {
                    result.Errors.Add($"echo[{i}] is null");
                    continue;
                }

                string id = dto.id?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id))
                {
                    result.Errors.Add($"echo[{i}] has no id");
                    continue;
                }
                if (!id.StartsWith("echo_", StringComparison.Ordinal))
                    result.Errors.Add("echo id must use the sanctioned echo_ prefix: " + id);
                if (!seenEchoes.Add(id))
                {
                    result.Errors.Add("duplicate echo id: " + id);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(dto.title)) result.Errors.Add(id + " has no title");
                if (string.IsNullOrWhiteSpace(dto.bodyText)) result.Errors.Add(id + " has no bodyText");
                if (double.IsNaN(dto.weight) || double.IsInfinity(dto.weight) || dto.weight <= 0d)
                    result.Errors.Add(id + " must have a finite positive weight");
                if (dto.minDay < 0) result.Errors.Add(id + " has a negative minDay");
                if (dto.conditions == null)
                    result.Errors.Add(id + " must contain conditions");

                var definition = new EchoDefinition
                {
                    Id = id,
                    Title = dto.title?.Trim() ?? string.Empty,
                    BodyText = dto.bodyText ?? string.Empty,
                    Weight = dto.weight,
                    MinDay = dto.minDay,
                    Conditions = new EchoConditions
                    {
                        MinDay = dto.conditions?.MinDay ?? dto.minDay,
                        RequiredFlagId = dto.conditions?.RequiredFlagId?.Trim() ?? string.Empty
                    }
                };

                if (definition.Conditions.MinDay < 0)
                    result.Errors.Add(id + " has a negative conditions.MinDay");
                if (definition.Conditions.MinDay != definition.MinDay)
                    result.Errors.Add(id + " has mismatched minDay and conditions.MinDay");

                ParseChoices(dto.choices, definition, result);
                result.Echoes.Add(definition);
            }

            if (result.Errors.Count > 0)
            {
                result.Echoes.Clear();
                return result;
            }

            result.Echoes.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            instrumentation?.RecordDefinitionsRegistered(
                FileName, nameof(EchoSystem) + ".Catalog", result.Echoes.Count);
            return result;
        }

        private static void ParseChoices(
            List<ChoiceDto>? choices,
            EchoDefinition definition,
            EchoCatalogLoadResult result)
        {
            if (choices == null || choices.Count == 0)
            {
                result.Errors.Add(definition.Id + " must contain at least one choice");
                return;
            }

            var seenChoices = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < choices.Count; i++)
            {
                var dto = choices[i];
                if (dto == null)
                {
                    result.Errors.Add(definition.Id + $" choice[{i}] is null");
                    continue;
                }

                string choiceId = dto.choiceId?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(choiceId))
                {
                    result.Errors.Add(definition.Id + $" choice[{i}] has no choiceId");
                    continue;
                }
                if (!seenChoices.Add(choiceId))
                {
                    result.Errors.Add("duplicate choice id in " + definition.Id + ": " + choiceId);
                    continue;
                }

                var choice = new EchoChoiceDefinition
                {
                    ChoiceId = choiceId,
                    Text = dto.text?.Trim() ?? string.Empty,
                    MoraleDelta = dto.moraleDelta
                };
                if (string.IsNullOrWhiteSpace(choice.Text))
                    result.Errors.Add(definition.Id + "/" + choiceId + " has no text");

                if (dto.effects == null &&
                    dto.delayedConsequence == null &&
                    Math.Abs(dto.moraleDelta) <= double.Epsilon)
                {
                    result.Errors.Add(definition.Id + "/" + choiceId + " must contain an effects array");
                }
                else if (dto.effects != null)
                {
                    for (int effectIndex = 0; effectIndex < dto.effects.Count; effectIndex++)
                    {
                        var effectDto = dto.effects[effectIndex];
                        if (effectDto == null)
                        {
                            result.Errors.Add(definition.Id + "/" + choiceId + $" effect[{effectIndex}] is null");
                            continue;
                        }
                        var effect = ToEffect(effectDto);
                        if (effectDto.delayedConsequence != null)
                        {
                            choice.DelayedConsequence ??= ToDelayedConsequence(
                                effectDto.delayedConsequence, definition.Id, choiceId, result);
                            continue;
                        }
                        if (!effect.HasRuntimeEffect)
                            result.Errors.Add(definition.Id + "/" + choiceId + $" effect[{effectIndex}] has no supported payload");
                        choice.Effects.Add(effect);
                    }
                }

                if (dto.delayedConsequence != null && choice.DelayedConsequence == null)
                    choice.DelayedConsequence = ToDelayedConsequence(dto.delayedConsequence, definition.Id, choiceId, result);

                if (!choice.IsExecutable)
                    result.Errors.Add(definition.Id + "/" + choiceId + " has no real effect");
                definition.Choices.Add(choice);
            }
        }

        private static EchoEffectDefinition ToEffect(EffectDto dto)
        {
            return new EchoEffectDefinition
            {
                ItemId = dto.itemId?.Trim() ?? string.Empty,
                ItemAmount = dto.itemAmount,
                TargetNeed = dto.targetNeed?.Trim() ?? string.Empty,
                NeedDelta = dto.needDelta,
                SetWorldFlag = dto.setWorldFlag?.Trim() ?? string.Empty,
                WorldFlagValue = dto.worldFlagValue
            };
        }

        private static EchoDelayedConsequence ToDelayedConsequence(
            DelayedDto dto,
            string echoId,
            string choiceId,
            EchoCatalogLoadResult result)
        {
            var delayed = new EchoDelayedConsequence
            {
                DelayHours = dto.delayHours,
                Title = dto.title?.Trim() ?? string.Empty,
                Description = dto.description?.Trim() ?? string.Empty
            };
            if (double.IsNaN(delayed.DelayHours) || double.IsInfinity(delayed.DelayHours) || delayed.DelayHours < 0d)
                result.Errors.Add(echoId + "/" + choiceId + " has an invalid delayed consequence delayHours");
            if (dto.effects == null)
            {
                result.Errors.Add(echoId + "/" + choiceId + " delayedConsequence must contain effects");
                return delayed;
            }

            for (int i = 0; i < dto.effects.Count; i++)
            {
                var effectDto = dto.effects[i];
                if (effectDto == null)
                {
                    result.Errors.Add(echoId + "/" + choiceId + $" delayed effect[{i}] is null");
                    continue;
                }
                var effect = ToEffect(effectDto);
                if (!effect.HasRuntimeEffect)
                    result.Errors.Add(echoId + "/" + choiceId + $" delayed effect[{i}] has no supported payload");
                delayed.Effects.Add(effect);
            }
            return delayed;
        }
    }
}
