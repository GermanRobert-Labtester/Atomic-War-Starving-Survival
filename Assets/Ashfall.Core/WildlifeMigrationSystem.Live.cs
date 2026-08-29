using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    public partial class WildlifeMigrationSystem
    {
        // ── Tunables ────────────────────────────────────────────────────
        /// <summary>Daily chance a starving pack (starvation &gt; HungerDriveThreshold) moves to an adjacent sector.</summary>
        public const float MigrationChancePerDay = 0.25f;
        /// <summary>Starvation level above which a pack feels the hunger drive to move.</summary>
        public const float HungerDriveThreshold = 0.5f;
        /// <summary>Moving to new ground relieves hunger: the pack found something to eat on the way.</summary>
        public const float MigrationStarvationRelief = 0.35f;
        /// <summary>Starvation above which the pack loses members daily.</summary>
        public const float StarvationLossThreshold = 0.7f;
        /// <summary>Daily chance a starving pack turns rabid (checked while above the loss threshold).</summary>
        public const float RabiesChancePerDay = 0.03f;
        /// <summary>Days a well-fed pack needs between births.</summary>
        public const int BreathingRoomDaysForBirth = 3;

        /// <summary>Sector adjacency for auto-migration, keyed by sector id. Set at seed time.</summary>
        private readonly Dictionary<string, List<string>> _sectorNeighbors =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);

        /// <summary>
        /// Provide the sector graph packs migrate along. Only links between
        /// known sectors are kept; packs in unlinked sectors stay put.
        /// </summary>
        public void SetSectorAdjacency(IEnumerable<(string sectorId, List<string> neighbors)> links)
        {
            if (links == null) return;
            foreach (var (sectorId, neighbors) in links)
            {
                if (string.IsNullOrEmpty(sectorId) || neighbors == null || neighbors.Count == 0) continue;
                _sectorNeighbors[sectorId] = new List<string>(neighbors);
            }
        }

        public bool TryGetNeighbors(string sectorId, out List<string> neighbors)
        {
            return _sectorNeighbors.TryGetValue(sectorId, out neighbors!);
        }

        public WildlifePackRecord? TryGetPack(string packId)
        {
            if (string.IsNullOrEmpty(packId)) return null;
            return _state.packs.Find(p => string.Equals(p.packId, packId, StringComparison.Ordinal));
        }

        /// <summary>
        /// Pack pressure in a sector: total population of packs currently
        /// holding it. Trapping, encounter weighting, and scarcity read this.
        /// </summary>
        public int GetSectorPackPopulation(string sectorId)
        {
            int total = 0;
            foreach (var p in _state.packs)
            {
                if (p != null && p.population > 0
                    && string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal))
                {
                    total += p.population;
                }
            }
            return total;
        }

        /// <summary>Live population over seeded population, 1.0 when never seeded. Drives scarcity.</summary>
        public float GetGlobalPopulationRatio()
        {
            int live = 0, seeded = 0;
            foreach (var p in _state.packs)
            {
                if (p == null) continue;
                live += p.population;
                seeded += p.seededPopulation > 0 ? p.seededPopulation : p.population;
            }
            return seeded > 0 ? (float)live / seeded : 1f;
        }

        /// <summary>
        /// Live-world daily tick: hunger, movement along the sector graph,
        /// population loss and recovery, rabies. All rolls come from the
        /// caller's per-day fork so the trajectory depends on the day, not on
        /// call counts. The legacy <see cref="TickDay(int)"/> delegates here.
        /// </summary>
        public void TickDay(int day, ISeededRng? dayRng = null)
        {
            var rng = dayRng ?? _rng;
            _state.lastMigrationDay = day;
            foreach (var pack in _state.packs)
            {
                if (pack == null) continue;

                // Hunger grows on held ground.
                pack.starvationLevel = Math.Min(1f, pack.starvationLevel + 0.05f);

                // The hunger drive: starving packs move to adjacent ground.
                if (rng != null && pack.starvationLevel > HungerDriveThreshold
                    && pack.population > 0
                    && _sectorNeighbors.TryGetValue(pack.currentSectorId, out var neighbors)
                    && neighbors.Count > 0
                    && rng.NextDouble() < MigrationChancePerDay)
                {
                    string target = neighbors[rng.Next(0, neighbors.Count)];
                    pack.currentSectorId = target;
                    pack.starvationLevel = Math.Max(0f, pack.starvationLevel - MigrationStarvationRelief);
                    OnPackMigrated?.Invoke(pack);
                }

                if (pack.starvationLevel > StarvationLossThreshold)
                {
                    // Starving packs thin out and can turn rabid.
                    if (pack.population > 0) pack.population--;
                    if (!pack.isRabid && rng != null && rng.NextDouble() < RabiesChancePerDay)
                    {
                        pack.isRabid = true;
                        pack.lastThreatFiredDay = day;
                    }
                    // Desperation reads as aggression (existing rule).
                    pack.aggressionScore = Math.Min(1f, pack.aggressionScore + 0.1f);
                }
                else if (pack.starvationLevel < 0.3f && pack.population > 0
                         && day - pack.lastThreatFiredDay >= BreathingRoomDaysForBirth)
                {
                    // Fed ground lets a pack recover toward twice its seed size.
                    int ceiling = (pack.seededPopulation > 0 ? pack.seededPopulation : pack.population) * 2;
                    if (pack.population < ceiling)
                    {
                        pack.population++;
                        pack.aggressionScore = Math.Max(0f, pack.aggressionScore - 0.05f);
                    }
                }
            }
        }
    }
}
