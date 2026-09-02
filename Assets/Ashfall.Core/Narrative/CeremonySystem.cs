// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 200 — Wasteland Festivals & Ceremonies System
// Subsystem    : Communal Celebrations, Faction Truces & Strategic Morale
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public enum CeremonyPhase
    {
        Planned = 0,
        Preparing = 1,
        Ready = 2,
        Active = 3,
        Completed = 4,
        Failed = 5,
        Cancelled = 6
    }

    [Serializable]
    public sealed class CeremonyItemRequirement
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;
    }

    [Serializable]
    public sealed class CeremonyDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("preparation_days")]
        public int PreparationDays { get; set; } = 3;

        [JsonPropertyName("required_room_id")]
        public string RequiredRoomId { get; set; } = "room_common_mess_hall";

        [JsonPropertyName("min_population")]
        public int MinPopulation { get; set; } = 4;

        [JsonPropertyName("required_items")]
        public List<CeremonyItemRequirement> RequiredItems { get; set; } = new List<CeremonyItemRequirement>();

        [JsonPropertyName("morale_boost")]
        public float MoraleBoost { get; set; } = 25f;

        [JsonPropertyName("stress_relief")]
        public float StressRelief { get; set; } = 20f;

        [JsonPropertyName("truce_duration_days")]
        public int TruceDurationDays { get; set; }

        [JsonPropertyName("truce_eligible")]
        public bool TruceEligible { get; set; }

        [JsonPropertyName("disaster_pool")]
        public List<string> DisasterPool { get; set; } = new List<string>();

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ScheduledCeremonyState
    {
        public string CeremonyId { get; set; } = string.Empty;
        public CeremonyPhase Phase { get; set; } = CeremonyPhase.Planned;
        public int ScheduledDay { get; set; }
        public int PreparationDaysRemaining { get; set; }
        public Dictionary<string, int> CommittedItems { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public List<string> InvitedFactions { get; set; } = new List<string>();
        public List<string> AcceptedFactions { get; set; } = new List<string>();
        public int ActiveTruceDaysRemaining { get; set; }
        public string OccurredDisasterId { get; set; } = string.Empty;

        public ScheduledCeremonyState Clone()
        {
            var copy = new ScheduledCeremonyState
            {
                CeremonyId = CeremonyId,
                Phase = Phase,
                ScheduledDay = ScheduledDay,
                PreparationDaysRemaining = PreparationDaysRemaining,
                ActiveTruceDaysRemaining = ActiveTruceDaysRemaining,
                OccurredDisasterId = OccurredDisasterId,
                CommittedItems = new Dictionary<string, int>(CommittedItems, StringComparer.Ordinal),
                InvitedFactions = new List<string>(InvitedFactions),
                AcceptedFactions = new List<string>(AcceptedFactions)
            };
            return copy;
        }
    }

    [Serializable]
    public sealed class CeremonySaveState
    {
        public string SystemId { get; set; } = CeremonySystem.SystemId;
        public ScheduledCeremonyState? ActiveCeremony { get; set; }
        public List<string> CompletedCeremonyIds { get; set; } = new List<string>();
        public int TotalCeremoniesHeld { get; set; }
        public int TotalDisastersEncountered { get; set; }
    }

    /// <summary>
    /// Engine-agnostic communal ceremony and wasteland festival system.
    /// Orchestrates resource-heavy strategic celebrations, multi-day logistics preparation,
    /// faction invitations with temporary truce brokering, and deterministic disaster risk.
    /// </summary>
    public sealed class CeremonySystem
    {
        public const string SystemId = "ceremony_system";

        private readonly Dictionary<string, CeremonyDefinition> _ceremonyCatalog =
            new Dictionary<string, CeremonyDefinition>(StringComparer.Ordinal);

        private CeremonySaveState _state = new CeremonySaveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<CeremonyDefinition, ScheduledCeremonyState>? OnCeremonyScheduled;
        public event Action<float, float>? OnMoraleBoostRequested;        // boost, stressRelief
        public event Action<string, int>? OnTruceRequested;              // factionId, durationDays
        public event Action<string, string>? OnCeremonyDisaster;         // ceremonyId, disasterId
        public event Action<string>? OnCeremonyCompleted;
        public event Action? OnStateChanged;

        public CeremonySaveState State => _state;
        public IReadOnlyDictionary<string, CeremonyDefinition> CeremonyCatalog => _ceremonyCatalog;
        public ScheduledCeremonyState? ActiveCeremony => _state.ActiveCeremony;

        public CeremonySystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("ceremonies", out var ceremoniesEl) && ceremoniesEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in ceremoniesEl.EnumerateArray())
                {
                    var c = JsonSerializer.Deserialize<CeremonyDefinition>(el.GetRawText());
                    if (c != null && !string.IsNullOrEmpty(c.Id))
                    {
                        _ceremonyCatalog[c.Id] = c;
                    }
                }
            }
        }

        public bool ScheduleCeremony(string ceremonyId, int currentDay, int currentPopulation, out string error)
        {
            error = string.Empty;
            if (_state.ActiveCeremony != null && _state.ActiveCeremony.Phase < CeremonyPhase.Completed)
            {
                error = "Another ceremony is currently scheduled or active.";
                return false;
            }

            if (!_ceremonyCatalog.TryGetValue(ceremonyId, out var def))
            {
                error = $"Unknown ceremony definition: {ceremonyId}";
                return false;
            }

            if (currentPopulation < def.MinPopulation)
            {
                error = $"Shelter population too low ({currentPopulation}/{def.MinPopulation}).";
                return false;
            }

            var scheduled = new ScheduledCeremonyState
            {
                CeremonyId = ceremonyId,
                Phase = CeremonyPhase.Preparing,
                ScheduledDay = currentDay,
                PreparationDaysRemaining = def.PreparationDays
            };

            _state.ActiveCeremony = scheduled;
            OnCeremonyScheduled?.Invoke(def, scheduled);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool ContributeResource(string itemId, int quantity)
        {
            if (_state.ActiveCeremony == null || _state.ActiveCeremony.Phase != CeremonyPhase.Preparing)
                return false;

            if (string.IsNullOrEmpty(itemId) || quantity <= 0)
                return false;

            if (!_ceremonyCatalog.TryGetValue(_state.ActiveCeremony.CeremonyId, out var def))
                return false;

            var req = def.RequiredItems.Find(r => string.Equals(r.ItemId, itemId, StringComparison.OrdinalIgnoreCase));
            if (req == null) return false;

            _state.ActiveCeremony.CommittedItems.TryGetValue(itemId, out int current);
            _state.ActiveCeremony.CommittedItems[itemId] = current + quantity;

            CheckPreparationReadiness(def);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool InviteFaction(string factionId, int currentStanding)
        {
            if (_state.ActiveCeremony == null || string.IsNullOrEmpty(factionId))
                return false;

            if (_state.ActiveCeremony.InvitedFactions.Contains(factionId))
                return false;

            _state.ActiveCeremony.InvitedFactions.Add(factionId);

            // Factions with neutral or positive standing accept (standing >= -10)
            if (currentStanding >= -10)
            {
                _state.ActiveCeremony.AcceptedFactions.Add(factionId);

                if (_ceremonyCatalog.TryGetValue(_state.ActiveCeremony.CeremonyId, out var def) && def.TruceEligible && def.TruceDurationDays > 0)
                {
                    _state.ActiveCeremony.ActiveTruceDaysRemaining = def.TruceDurationDays;
                    OnTruceRequested?.Invoke(factionId, def.TruceDurationDays);
                }
            }

            OnStateChanged?.Invoke();
            return true;
        }

        private void CheckPreparationReadiness(CeremonyDefinition def)
        {
            if (_state.ActiveCeremony == null || _state.ActiveCeremony.Phase != CeremonyPhase.Preparing) return;

            bool allSatisfied = true;
            foreach (var req in def.RequiredItems)
            {
                _state.ActiveCeremony.CommittedItems.TryGetValue(req.ItemId, out int count);
                if (count < req.Quantity)
                {
                    allSatisfied = false;
                    break;
                }
            }

            if (allSatisfied && _state.ActiveCeremony.PreparationDaysRemaining <= 0)
            {
                _state.ActiveCeremony.Phase = CeremonyPhase.Ready;
            }
        }

        public void TickDay(int currentDay, out string outcomeSummary)
        {
            outcomeSummary = string.Empty;
            if (_state.ActiveCeremony == null) return;

            var active = _state.ActiveCeremony;
            if (!_ceremonyCatalog.TryGetValue(active.CeremonyId, out var def)) return;

            if (active.Phase == CeremonyPhase.Preparing)
            {
                active.PreparationDaysRemaining = Math.Max(0, active.PreparationDaysRemaining - 1);
                CheckPreparationReadiness(def);

                // If out of preparation days but materials missing, extends or flags
                if (active.PreparationDaysRemaining == 0 && active.Phase == CeremonyPhase.Preparing)
                {
                    outcomeSummary = $"Ceremony '{def.DisplayName}' preparation delayed: pending required materials.";
                }
            }
            else if (active.Phase == CeremonyPhase.Ready)
            {
                // Commence festival!
                active.Phase = CeremonyPhase.Active;

                // Deterministic disaster roll (15% base risk)
                if (def.DisasterPool != null && def.DisasterPool.Count > 0 && _rng.NextDouble() < 0.15)
                {
                    int disasterIdx = (int)(_rng.NextDouble() * def.DisasterPool.Count) % def.DisasterPool.Count;
                    string disasterId = def.DisasterPool[disasterIdx];
                    active.OccurredDisasterId = disasterId;
                    _state.TotalDisastersEncountered++;
                    OnCeremonyDisaster?.Invoke(active.CeremonyId, disasterId);
                    outcomeSummary = $"Ceremony incident during {def.DisplayName}: {disasterId}.";
                }
                else
                {
                    outcomeSummary = $"Ceremony '{def.DisplayName}' commenced successfully with high morale!";
                }

                // Apply morale boost and stress relief
                OnMoraleBoostRequested?.Invoke(def.MoraleBoost, def.StressRelief);
                active.Phase = CeremonyPhase.Completed;
                _state.TotalCeremoniesHeld++;
                _state.CompletedCeremonyIds.Add(active.CeremonyId);
                OnCeremonyCompleted?.Invoke(active.CeremonyId);
            }
            else if (active.Phase == CeremonyPhase.Completed && active.ActiveTruceDaysRemaining > 0)
            {
                active.ActiveTruceDaysRemaining--;
            }

            OnStateChanged?.Invoke();
        }

        // ── Save / Restore ──────────────────────────────────────────────────

        public CeremonySaveState CaptureState()
        {
            return new CeremonySaveState
            {
                SystemId = _state.SystemId,
                TotalCeremoniesHeld = _state.TotalCeremoniesHeld,
                TotalDisastersEncountered = _state.TotalDisastersEncountered,
                CompletedCeremonyIds = new List<string>(_state.CompletedCeremonyIds),
                ActiveCeremony = _state.ActiveCeremony?.Clone()
            };
        }

        public void RestoreState(CeremonySaveState? state)
        {
            if (state == null)
            {
                _state = new CeremonySaveState();
                return;
            }

            _state = new CeremonySaveState
            {
                SystemId = state.SystemId ?? SystemId,
                TotalCeremoniesHeld = state.TotalCeremoniesHeld,
                TotalDisastersEncountered = state.TotalDisastersEncountered,
                CompletedCeremonyIds = state.CompletedCeremonyIds != null ? new List<string>(state.CompletedCeremonyIds) : new List<string>(),
                ActiveCeremony = state.ActiveCeremony?.Clone()
            };

            OnStateChanged?.Invoke();
        }
    }
}
