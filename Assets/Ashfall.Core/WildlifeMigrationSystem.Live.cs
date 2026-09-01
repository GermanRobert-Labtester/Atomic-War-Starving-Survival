using System;
using System.Collections.Generic;
using Ashfall.Core.World;

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

        /// <summary>Plan 28 Phase 3: war-blocked sectors. Packs will not move INTO these; a pack already inside may still flee to unblocked ground.</summary>
        private readonly HashSet<string> _blockedSectors = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>Waterway sectors (Plan 28): water-bound runners migrate only along these.</summary>
        private readonly HashSet<string> _waterSectors = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>Plan 19 season authority bound at host seed time; null keeps legacy season-neutral behavior.</summary>
        private SeasonProfileDef? _seasonProfile;

        /// <summary>
        /// Bind the Plan 19 season profile (weather_seasons.json). Optional:
        /// without a bound profile every pack reads as season-neutral and the
        /// tick behaves exactly as before Plan 28.
        /// </summary>
        public void BindSeasonProfile(World.SeasonProfileDef? profile) => _seasonProfile = profile;

        /// <summary>
        /// Mark waterway sectors so water-bound runners (fish, piscivore birds)
        /// migrate along the waterway pair instead of crossing dry land.
        /// </summary>
        public void SetWaterSectors(IEnumerable<string>? sectorIds)
        {
            if (sectorIds == null) return;
            _waterSectors.Clear();
            foreach (var s in sectorIds)
                if (!string.IsNullOrEmpty(s)) _waterSectors.Add(s);
        }

        public bool IsWaterSector(string sectorId) =>
            !string.IsNullOrEmpty(sectorId) && _waterSectors.Contains(sectorId);

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

        // ── Plan 28 Phase 3: war-blocked corridors & harvest pressure ──

        /// <summary>
        /// Mark a sector as blocked (faction war, fortified ground). Blocked
        /// sectors are removed from movement targets; packs already inside may
        /// still flee to unblocked neighbors. Projection state: recomputed
        /// from the host's live faction dominance each day, never persisted.
        /// </summary>
        public void SetSectorBlocked(string sectorId, bool blocked)
        {
            if (string.IsNullOrEmpty(sectorId)) return;
            if (blocked) _blockedSectors.Add(sectorId);
            else _blockedSectors.Remove(sectorId);
        }

        public bool IsSectorBlocked(string sectorId) =>
            !string.IsNullOrEmpty(sectorId) && _blockedSectors.Contains(sectorId);

        /// <summary>Clear all blockages (re-projected fresh each day by the host).</summary>
        public void ClearSectorBlockages() => _blockedSectors.Clear();

        /// <summary>
        /// Plan 28 Phase 3 — harvest pressure: each trapped animal thins the
        /// largest population pack holding <paramref name="sectorId"/> (ties
        /// broken by pack id, ordinal). Bounded at zero by the pack record;
        /// recovery remains the existing birth rule. No RNG: deterministic.
        /// Returns the population actually removed.
        /// </summary>
        public int ApplyHarvestPressure(string sectorId, int amount)
        {
            if (string.IsNullOrEmpty(sectorId) || amount <= 0) return 0;
            int removed = 0;
            for (int i = 0; i < amount; i++)
            {
                WildlifePackRecord? best = null;
                foreach (var p in _state.packs)
                {
                    if (p == null || p.population <= 0
                        || !string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)) continue;
                    if (p.population <= 1) continue; // a remnant pair always survives
                    if (best == null
                        || p.population > best.population
                        || (p.population == best.population && string.CompareOrdinal(p.packId, best.packId) < 0))
                    {
                        best = p;
                    }
                }
                if (best == null) break;
                best.population--;
                removed++;
            }
            return removed;
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

            // Plan 28: one season window per day, resolved once. No bound
            // profile (headless tools, legacy hosts) keeps every factor at 1.0.
            var season = _seasonProfile != null && _seasonProfile.seasons is { Count: > 0 }
                ? WildlifeSeasonalCalendar.SeasonWindowForDay(_seasonProfile, day)
                : null;

            foreach (var pack in _state.packs)
            {
                if (pack == null) continue;

                // Hunger grows on held ground, paced by the seasonal calendar.
                float hungerFactor = season != null
                    ? WildlifeSeasonalCalendar.HungerFactor(
                        season, WildlifeSeasonalCalendar.ArchetypeOf(pack.speciesId))
                    : 1f;
                pack.starvationLevel = Math.Min(1f, pack.starvationLevel + 0.05f * hungerFactor);

                // The hunger drive: starving packs move to adjacent ground,
                // paced by the Plan 28 seasonal calendar. War-blocked sectors
                // (Plan 28 Phase 3) are never entered; a pack already inside
                // may still flee to unblocked ground.
                if (rng != null && pack.starvationLevel > HungerDriveThreshold
                    && pack.population > 0
                    && _sectorNeighbors.TryGetValue(pack.currentSectorId, out var neighbors)
                    && neighbors.Count > 0
                    && rng.NextDouble() < MigrationChancePerDay)
                {
                    var archetype = WildlifeSeasonalCalendar.ArchetypeOf(pack.speciesId);
                    var candidates = WildlifeSeasonalCalendar.FilterNeighbors(
                        archetype, pack.currentSectorId, neighbors, _waterSectors);
                    if (_blockedSectors.Count > 0)
                    {
                        var passable = new List<string>();
                        foreach (var n in candidates)
                            if (!_blockedSectors.Contains(n)) passable.Add(n);
                        // Blocked ground is never entered; a pack already
                        // inside may still flee to the last open neighbor.
                        // Fully enclosed packs stay put (siege: hunger grows).
                        candidates = passable;
                    }
                    if (candidates.Count > 0)
                    {
                        string target = candidates[rng.Next(0, candidates.Count)];
                        pack.currentSectorId = target;
                        pack.starvationLevel = Math.Max(0f, pack.starvationLevel - MigrationStarvationRelief);
                        OnPackMigrated?.Invoke(pack);
                    }
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
