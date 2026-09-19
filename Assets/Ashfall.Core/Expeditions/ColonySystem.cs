// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Expeditions
{
    public enum ColonyType
    {
        Outpost = 0,
        Settlement = 1,
        Fortress = 2,
        TradingPost = 3,
        FarmingCommune = 4
    }

    public enum SupplyLineStatus
    {
        Active = 0,
        Disrupted = 1,
        Blocked = 2
    }

    [Serializable]
    public sealed class ColonyOutpost
    {
        public string ColonyId { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ColonyType Type { get; set; } = ColonyType.Outpost;
        public int EstablishedDay { get; set; } = 1;
        public float DefenseRating { get; set; } = 50f;
        public float MoraleRating { get; set; } = 70f;
        public List<string> PopulationIds { get; set; } = new List<string>();
        public float StoredSupplies { get; set; } = 50f;
        public bool IsActive { get; set; } = true;
    }

    [Serializable]
    public sealed class SupplyLine
    {
        public string LineId { get; set; } = string.Empty;
        public string OriginId { get; set; } = string.Empty;
        public string DestinationId { get; set; } = string.Empty;
        public SupplyLineStatus Status { get; set; } = SupplyLineStatus.Active;
        public float CargoCapacity { get; set; } = 100f;
        public float DailyFlow { get; set; } = 10f;
        public int LastSupplyDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class ColonyState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<ColonyOutpost> Colonies { get; set; } = new List<ColonyOutpost>();
        public List<SupplyLine> SupplyLines { get; set; } = new List<SupplyLine>();
    }

    /// <summary>
    /// Plan 160 — Expedition Colony & Outpost System.
    /// Manages forward operating bases and permanent settlements at expedition destinations,
    /// garrison assignments, resource supply lines, defense ratings, and local colony morale.
    /// </summary>
    public sealed class ColonySystem
    {
        private readonly ColonyState _state;

        public event Action<ColonyOutpost>? OnColonyEstablished;
        public event Action<SupplyLine, SupplyLineStatus>? OnSupplyLineStatusChanged;
        public event Action<ColonyOutpost, float>? OnColonySuppliesUpdated;

        public int TotalColonyCount => _state.Colonies.Count;
        public int ActiveSupplyLineCount => _state.SupplyLines.Count(s => s.Status == SupplyLineStatus.Active);

        public ColonySystem(ColonyState? state = null)
        {
            _state = state ?? new ColonyState();
        }

        public ColonyOutpost EstablishColony(
            string locationId,
            string name,
            ColonyType type,
            IEnumerable<string>? initialGarrison,
            float initialSupplies = 50f,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(locationId)) throw new ArgumentNullException(nameof(locationId));

            float baseDefense = type switch
            {
                ColonyType.Fortress => 80f,
                ColonyType.Outpost => 50f,
                ColonyType.TradingPost => 40f,
                ColonyType.FarmingCommune => 30f,
                _ => 45f
            };

            var colony = new ColonyOutpost
            {
                ColonyId = $"col_{_state.NextSequence++}",
                LocationId = locationId.Trim(),
                Name = string.IsNullOrWhiteSpace(name) ? $"Outpost {locationId}" : name.Trim(),
                Type = type,
                EstablishedDay = Math.Max(1, currentDay),
                DefenseRating = baseDefense,
                MoraleRating = 70f,
                PopulationIds = initialGarrison != null ? new List<string>(initialGarrison) : new List<string>(),
                StoredSupplies = Math.Max(0f, initialSupplies),
                IsActive = true
            };

            _state.Colonies.Add(colony);
            OnColonyEstablished?.Invoke(colony);
            return colony;
        }

        public SupplyLine EstablishSupplyLine(
            string originId,
            string destinationId,
            float cargoCapacity = 100f,
            float dailyFlow = 10f,
            int currentDay = 1)
        {
            var line = new SupplyLine
            {
                LineId = $"sl_{_state.NextSequence++}",
                OriginId = originId ?? "shelter",
                DestinationId = destinationId ?? string.Empty,
                Status = SupplyLineStatus.Active,
                CargoCapacity = Math.Max(10f, cargoCapacity),
                DailyFlow = Math.Max(1f, dailyFlow),
                LastSupplyDay = currentDay
            };

            _state.SupplyLines.Add(line);
            return line;
        }

        public bool SetSupplyLineStatus(string lineId, SupplyLineStatus status)
        {
            var line = _state.SupplyLines.FirstOrDefault(s => string.Equals(s.LineId, lineId, StringComparison.OrdinalIgnoreCase));
            if (line == null) return false;

            line.Status = status;
            OnSupplyLineStatusChanged?.Invoke(line, status);
            return true;
        }

        public bool AssignSurvivorToColony(string colonyId, string survivorId)
        {
            var colony = GetColony(colonyId);
            if (colony == null || string.IsNullOrWhiteSpace(survivorId)) return false;

            if (!colony.PopulationIds.Contains(survivorId))
            {
                colony.PopulationIds.Add(survivorId);
            }
            return true;
        }

        public float TransferSupplies(string colonyId, float amount)
        {
            var colony = GetColony(colonyId);
            if (colony == null) return 0f;

            colony.StoredSupplies = Math.Max(0f, colony.StoredSupplies + amount);
            OnColonySuppliesUpdated?.Invoke(colony, colony.StoredSupplies);
            return colony.StoredSupplies;
        }

        public void TickDay(int currentDay)
        {
            // 1. Process Supply Lines
            foreach (var line in _state.SupplyLines)
            {
                if (line.Status == SupplyLineStatus.Active)
                {
                    var destColony = GetColony(line.DestinationId);
                    if (destColony != null && destColony.IsActive)
                    {
                        destColony.StoredSupplies += line.DailyFlow;
                        line.LastSupplyDay = currentDay;
                        OnColonySuppliesUpdated?.Invoke(destColony, destColony.StoredSupplies);
                    }
                }
            }

            // 2. Consume supplies and update colony morale
            foreach (var colony in _state.Colonies)
            {
                if (!colony.IsActive) continue;

                float consumption = Math.Max(1, colony.PopulationIds.Count) * 1.5f;
                colony.StoredSupplies = Math.Max(0f, colony.StoredSupplies - consumption);

                if (colony.StoredSupplies <= 0f)
                {
                    // Starvation / supply shortage
                    colony.MoraleRating = Math.Clamp(colony.MoraleRating - 10f, 0f, 100f);
                }
                else if (colony.MoraleRating < 80f)
                {
                    colony.MoraleRating = Math.Clamp(colony.MoraleRating + 1f, 0f, 100f);
                }
            }
        }

        public IReadOnlyList<ColonyOutpost> GetColonies() => _state.Colonies;

        public ColonyOutpost? GetColony(string colonyId)
        {
            return _state.Colonies.FirstOrDefault(c => string.Equals(c.ColonyId, colonyId, StringComparison.OrdinalIgnoreCase));
        }

        public ColonyState CaptureState()
        {
            var state = new ColonyState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Colonies = new List<ColonyOutpost>(_state.Colonies.Count),
                SupplyLines = new List<SupplyLine>(_state.SupplyLines.Count)
            };

            foreach (var c in _state.Colonies)
            {
                state.Colonies.Add(new ColonyOutpost
                {
                    ColonyId = c.ColonyId,
                    LocationId = c.LocationId,
                    Name = c.Name,
                    Type = c.Type,
                    EstablishedDay = c.EstablishedDay,
                    DefenseRating = c.DefenseRating,
                    MoraleRating = c.MoraleRating,
                    PopulationIds = new List<string>(c.PopulationIds),
                    StoredSupplies = c.StoredSupplies,
                    IsActive = c.IsActive
                });
            }

            foreach (var s in _state.SupplyLines)
            {
                state.SupplyLines.Add(new SupplyLine
                {
                    LineId = s.LineId,
                    OriginId = s.OriginId,
                    DestinationId = s.DestinationId,
                    Status = s.Status,
                    CargoCapacity = s.CargoCapacity,
                    DailyFlow = s.DailyFlow,
                    LastSupplyDay = s.LastSupplyDay
                });
            }

            return state;
        }

        public void RestoreState(ColonyState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Colonies.Clear();
            _state.SupplyLines.Clear();

            if (state.Colonies != null)
            {
                foreach (var c in state.Colonies)
                {
                    _state.Colonies.Add(new ColonyOutpost
                    {
                        ColonyId = c.ColonyId,
                        LocationId = c.LocationId,
                        Name = c.Name,
                        Type = c.Type,
                        EstablishedDay = c.EstablishedDay,
                        DefenseRating = c.DefenseRating,
                        MoraleRating = c.MoraleRating,
                        PopulationIds = new List<string>(c.PopulationIds ?? Enumerable.Empty<string>()),
                        StoredSupplies = c.StoredSupplies,
                        IsActive = c.IsActive
                    });
                }
            }

            if (state.SupplyLines != null)
            {
                foreach (var s in state.SupplyLines)
                {
                    _state.SupplyLines.Add(new SupplyLine
                    {
                        LineId = s.LineId,
                        OriginId = s.OriginId,
                        DestinationId = s.DestinationId,
                        Status = s.Status,
                        CargoCapacity = s.CargoCapacity,
                        DailyFlow = s.DailyFlow,
                        LastSupplyDay = s.LastSupplyDay
                    });
                }
            }
        }
    }
}
