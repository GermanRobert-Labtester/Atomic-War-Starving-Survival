using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Survivors
{
    public enum AgendaType
    {
        FactionLoyalty = 0,
        ResourceTheft = 1,
        Sabotage = 2,
        EscapePlan = 3,
        SecretProtection = 4
    }

    public enum AgendaResolution
    {
        Pending = 0,
        Betrayed = 1,
        Reconciled = 2,
        Exploited = 3,
        Expelled = 4
    }

    [Serializable]
    public sealed class SurvivorHiddenAgenda
    {
        public string AgendaId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public AgendaType Type { get; set; } = AgendaType.ResourceTheft;
        public string TargetFactionId { get; set; } = string.Empty;
        public string TargetSurvivorId { get; set; } = string.Empty;
        public float DiscoveryProgress { get; set; } = 0f; // 0 to 100
        public bool IsDiscovered { get; set; } = false;
        public bool IsConfronted { get; set; } = false;
        public bool IsResolved { get; set; } = false;
        public AgendaResolution Resolution { get; set; } = AgendaResolution.Pending;
        public int StartDay { get; set; } = 1;
        public string Notes { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class AgendaClue
    {
        public string ClueId { get; set; } = string.Empty;
        public string AgendaId { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public float ClueStrength { get; set; } = 15f;
    }

    [Serializable]
    public sealed class AgendaTemplateDefinition
    {
        [JsonPropertyName("template_id")]
        public string TemplateId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = "resource_theft";

        [JsonPropertyName("target_faction_id")]
        public string TargetFactionId { get; set; } = string.Empty;

        [JsonPropertyName("target_survivor_id")]
        public string TargetSurvivorId { get; set; } = string.Empty;

        [JsonPropertyName("base_evidence_threshold")]
        public float BaseEvidenceThreshold { get; set; } = 50f;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("clue_descriptions")]
        public List<string> ClueDescriptions { get; set; } = new List<string>();

        public AgendaType GetAgendaType() => Type.ToLowerInvariant() switch
        {
            "faction_loyalty" or "factionloyalty" => AgendaType.FactionLoyalty,
            "resource_theft" or "resourcetheft" => AgendaType.ResourceTheft,
            "sabotage" => AgendaType.Sabotage,
            "escape_plan" or "escapeplan" => AgendaType.EscapePlan,
            "secret_protection" or "secretprotection" => AgendaType.SecretProtection,
            _ => AgendaType.ResourceTheft
        };
    }

    [Serializable]
    public sealed class HiddenAgendaCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("templates")]
        public List<AgendaTemplateDefinition> Templates { get; set; } = new List<AgendaTemplateDefinition>();
    }

    [Serializable]
    public sealed class HiddenAgendaState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<SurvivorHiddenAgenda> Agendas { get; set; } = new List<SurvivorHiddenAgenda>();
        public List<AgendaClue> Clues { get; set; } = new List<AgendaClue>();
    }

    /// <summary>
    /// Plan 132 — Survivor Hidden Agendas & Betrayal Arc System.
    /// Manages persistent secret motivations (theft, sabotage, faction loyalty, escape plans),
    /// progressive clue discovery through investigation, confrontation thresholds, and branching resolutions.
    /// </summary>
    public sealed class HiddenAgendaSystem
    {
        private readonly HiddenAgendaState _state;
        private readonly Dictionary<string, AgendaTemplateDefinition> _templates = new Dictionary<string, AgendaTemplateDefinition>(StringComparer.OrdinalIgnoreCase);

        public event Action<SurvivorHiddenAgenda>? OnAgendaAssigned;
        public event Action<AgendaClue>? OnClueDiscovered;
        public event Action<SurvivorHiddenAgenda>? OnAgendaExposed;
        public event Action<SurvivorHiddenAgenda, AgendaResolution>? OnAgendaResolved;

        public Action<string /*survivorId*/, float /*moraleDelta*/>? MoraleDeltaApplier { get; set; }
        public Action<string /*factionId*/, float /*standingDelta*/>? FactionStandingApplier { get; set; }
        public Action<string /*survivorId*/, string /*confiscatedItem*/, int /*count*/>? ConfiscationApplier { get; set; }

        public int ActiveAgendaCount => _state.Agendas.Count(a => !a.IsResolved);
        public int TotalCluesCount => _state.Clues.Count;
        public IReadOnlyCollection<AgendaTemplateDefinition> Templates => _templates.Values;

        public HiddenAgendaSystem(HiddenAgendaState? state = null)
        {
            _state = state ?? new HiddenAgendaState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var data = JsonSerializer.Deserialize<HiddenAgendaCatalogData>(json, options);
            if (data != null)
            {
                LoadCatalog(data);
            }
        }

        public void LoadCatalog(HiddenAgendaCatalogData catalog)
        {
            if (catalog?.Templates == null) return;
            foreach (var t in catalog.Templates)
            {
                if (!string.IsNullOrWhiteSpace(t.TemplateId))
                {
                    _templates[t.TemplateId] = t;
                }
            }
        }

        public AgendaTemplateDefinition? GetTemplate(string templateId)
        {
            if (string.IsNullOrWhiteSpace(templateId)) return null;
            return _templates.TryGetValue(templateId, out var t) ? t : null;
        }

        public SurvivorHiddenAgenda AssignAgendaFromTemplate(
            string survivorId,
            string templateId,
            int startDay = 1,
            string? customNotes = null)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));
            if (string.IsNullOrWhiteSpace(templateId)) throw new ArgumentNullException(nameof(templateId));

            if (!_templates.TryGetValue(templateId, out var template))
            {
                throw new ArgumentException($"Template '{templateId}' not found in loaded catalog.", nameof(templateId));
            }

            var agenda = AssignAgenda(
                survivorId: survivorId,
                type: template.GetAgendaType(),
                targetFaction: template.TargetFactionId,
                targetSurvivor: template.TargetSurvivorId,
                startDay: startDay,
                notes: customNotes ?? template.Description
            );
            return agenda;
        }

        public SurvivorHiddenAgenda AssignAgenda(
            string survivorId,
            AgendaType type,
            string targetFaction = "",
            string targetSurvivor = "",
            int startDay = 1,
            string notes = "")
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));

            var agenda = new SurvivorHiddenAgenda
            {
                AgendaId = $"agd_{_state.NextSequence++}",
                SurvivorId = survivorId.Trim(),
                Type = type,
                TargetFactionId = targetFaction ?? string.Empty,
                TargetSurvivorId = targetSurvivor ?? string.Empty,
                DiscoveryProgress = 0f,
                IsDiscovered = false,
                IsConfronted = false,
                IsResolved = false,
                Resolution = AgendaResolution.Pending,
                StartDay = Math.Max(1, startDay),
                Notes = notes ?? string.Empty
            };

            _state.Agendas.Add(agenda);
            OnAgendaAssigned?.Invoke(agenda);
            return agenda;
        }

        public AgendaClue? InvestigateAgenda(string survivorId, float investigatorSkill = 20f, int currentDay = 1)
        {
            var agenda = _state.Agendas.FirstOrDefault(a => !a.IsResolved && string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            if (agenda == null) return null;

            float progressGain = Math.Clamp(investigatorSkill, 5f, 40f);
            agenda.DiscoveryProgress = Math.Clamp(agenda.DiscoveryProgress + progressGain, 0f, 100f);

            string description = $"Suspicious behavior noted regarding {agenda.Type} by {survivorId}.";
            var matchingTemplate = _templates.Values.FirstOrDefault(t => t.GetAgendaType() == agenda.Type);
            if (matchingTemplate != null && matchingTemplate.ClueDescriptions.Count > 0)
            {
                int existingCluesCount = _state.Clues.Count(c => c.AgendaId == agenda.AgendaId);
                int index = existingCluesCount % matchingTemplate.ClueDescriptions.Count;
                description = matchingTemplate.ClueDescriptions[index];
            }

            var clue = new AgendaClue
            {
                ClueId = $"clue_{_state.NextSequence++}",
                AgendaId = agenda.AgendaId,
                Day = currentDay,
                Description = description,
                ClueStrength = progressGain
            };
            _state.Clues.Add(clue);
            OnClueDiscovered?.Invoke(clue);

            if (agenda.DiscoveryProgress >= 60f && !agenda.IsDiscovered)
            {
                agenda.IsDiscovered = true;
                OnAgendaExposed?.Invoke(agenda);
            }

            return clue;
        }

        public bool ConfrontSurvivor(string agendaId, AgendaResolution resolution, int currentDay)
        {
            var agenda = _state.Agendas.FirstOrDefault(a => string.Equals(a.AgendaId, agendaId, StringComparison.OrdinalIgnoreCase));
            if (agenda == null || agenda.IsResolved) return false;

            // Must have sufficient evidence (>= 60% progress)
            if (agenda.DiscoveryProgress < 60f) return false;

            agenda.IsConfronted = true;
            agenda.IsResolved = true;
            agenda.Resolution = resolution;

            // Trigger delegate bridges
            switch (resolution)
            {
                case AgendaResolution.Reconciled:
                    MoraleDeltaApplier?.Invoke(agenda.SurvivorId, 10f);
                    break;
                case AgendaResolution.Expelled:
                    MoraleDeltaApplier?.Invoke(agenda.SurvivorId, -15f);
                    if (agenda.Type == AgendaType.FactionLoyalty && !string.IsNullOrWhiteSpace(agenda.TargetFactionId))
                    {
                        FactionStandingApplier?.Invoke(agenda.TargetFactionId, -10f);
                    }
                    break;
                case AgendaResolution.Exploited:
                    MoraleDeltaApplier?.Invoke(agenda.SurvivorId, -20f);
                    break;
                case AgendaResolution.Betrayed:
                    MoraleDeltaApplier?.Invoke(agenda.SurvivorId, -25f);
                    if (agenda.Type == AgendaType.FactionLoyalty && !string.IsNullOrWhiteSpace(agenda.TargetFactionId))
                    {
                        FactionStandingApplier?.Invoke(agenda.TargetFactionId, 15f);
                    }
                    break;
            }

            if (agenda.Type == AgendaType.ResourceTheft && (resolution == AgendaResolution.Reconciled || resolution == AgendaResolution.Expelled || resolution == AgendaResolution.Exploited))
            {
                ConfiscationApplier?.Invoke(agenda.SurvivorId, "scrap_supplies", 5);
            }

            OnAgendaResolved?.Invoke(agenda, resolution);
            return true;
        }

        public void TickDay(int currentDay)
        {
            foreach (var agenda in _state.Agendas)
            {
                if (agenda.IsResolved) continue;

                // Passive slip-up chance: if active for more than 10 days, slight discovery increase
                if (currentDay - agenda.StartDay > 10 && agenda.DiscoveryProgress < 40f)
                {
                    agenda.DiscoveryProgress += 1.0f;
                }
            }
        }

        public SurvivorHiddenAgenda? GetAgendaForSurvivor(string survivorId)
        {
            return _state.Agendas.FirstOrDefault(a => !a.IsResolved && string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<SurvivorHiddenAgenda> GetActiveAgendas()
        {
            return _state.Agendas.Where(a => !a.IsResolved).ToList();
        }

        public IReadOnlyList<SurvivorHiddenAgenda> GetAllAgendas()
        {
            return _state.Agendas.ToList();
        }

        public IReadOnlyList<AgendaClue> GetCluesForAgenda(string agendaId)
        {
            return _state.Clues.Where(c => string.Equals(c.AgendaId, agendaId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IReadOnlyList<AgendaClue> GetAllClues()
        {
            return _state.Clues.ToList();
        }

        public HiddenAgendaState CaptureState()
        {
            var state = new HiddenAgendaState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Agendas = new List<SurvivorHiddenAgenda>(_state.Agendas.Count),
                Clues = new List<AgendaClue>(_state.Clues.Count)
            };

            foreach (var a in _state.Agendas)
            {
                state.Agendas.Add(new SurvivorHiddenAgenda
                {
                    AgendaId = a.AgendaId,
                    SurvivorId = a.SurvivorId,
                    Type = a.Type,
                    TargetFactionId = a.TargetFactionId,
                    TargetSurvivorId = a.TargetSurvivorId,
                    DiscoveryProgress = a.DiscoveryProgress,
                    IsDiscovered = a.IsDiscovered,
                    IsConfronted = a.IsConfronted,
                    IsResolved = a.IsResolved,
                    Resolution = a.Resolution,
                    StartDay = a.StartDay,
                    Notes = a.Notes
                });
            }

            foreach (var c in _state.Clues)
            {
                state.Clues.Add(new AgendaClue
                {
                    ClueId = c.ClueId,
                    AgendaId = c.AgendaId,
                    Day = c.Day,
                    Description = c.Description,
                    ClueStrength = c.ClueStrength
                });
            }

            return state;
        }

        public void RestoreState(HiddenAgendaState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Agendas.Clear();
            _state.Clues.Clear();

            if (state.Agendas != null)
            {
                foreach (var a in state.Agendas)
                {
                    _state.Agendas.Add(new SurvivorHiddenAgenda
                    {
                        AgendaId = a.AgendaId,
                        SurvivorId = a.SurvivorId,
                        Type = a.Type,
                        TargetFactionId = a.TargetFactionId,
                        TargetSurvivorId = a.TargetSurvivorId,
                        DiscoveryProgress = a.DiscoveryProgress,
                        IsDiscovered = a.IsDiscovered,
                        IsConfronted = a.IsConfronted,
                        IsResolved = a.IsResolved,
                        Resolution = a.Resolution,
                        StartDay = a.StartDay,
                        Notes = a.Notes
                    });
                }
            }

            if (state.Clues != null)
            {
                foreach (var c in state.Clues)
                {
                    _state.Clues.Add(new AgendaClue
                    {
                        ClueId = c.ClueId,
                        AgendaId = c.AgendaId,
                        Day = c.Day,
                        Description = c.Description,
                        ClueStrength = c.ClueStrength
                    });
                }
            }
        }
    }
}
