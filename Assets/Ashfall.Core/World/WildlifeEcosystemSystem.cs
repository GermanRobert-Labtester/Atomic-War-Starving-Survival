// SPDX-License-Identifier: MIT
// ============================================================================
// System     : WildlifeEcosystemSystem (Plan 165 — Wasteland Wildlife Ecology)
// Authority  : WildlifeMigrationSystem remains THE population store (packs in
//      the `world` save section). This system is the upstream ecology layer:
//      it derives per-sector densities from packs, applies predation /
//      radiation pressure / seasonal moves THROUGH migration APIs, tracks
//      hunting/trapping pressure, local extinction, apex activity, taming
//      handoff, and bestiary knowledge. It never keeps a second population.
// Catalog     : wildlife_ecosystem.json (species aligned with the existing
//      species_* archetype ids, predator-prey edges, seasonal patterns).
// RNG         : injected per tick (wildlife.population / .migration / .apex /
//      .taming forks owned by the host).
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class FaunaSpeciesDef
    {
        public string id { get; set; } = string.Empty;              // species_*
        public string display_name { get; set; } = string.Empty;
        public float radiation_tolerance { get; set; } = 0.5f;      // 0..1 vs OutdoorRadModifier/250
        public string diet_type { get; set; } = "herbivore";        // herbivore | carnivore | scavenger
        public int apex_population_threshold { get; set; }          // 0 = not an apex species
        public bool tameable { get; set; }
        public float tameness_chance { get; set; } = 0.15f;
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class PredatorPreyEdgeDef
    {
        public string predator_species_id { get; set; } = string.Empty;
        public string prey_species_id { get; set; } = string.Empty;
        public float predation_pressure { get; set; } = 0.05f;      // fraction of prey removed per day
    }

    [Serializable]
    public sealed class SeasonalMoveDef
    {
        public string species_id { get; set; } = string.Empty;
        public string season_window_id { get; set; } = string.Empty;
        public float move_chance { get; set; } = 0.1f;              // per pack per day in window
    }

    [Serializable]
    public sealed class WildlifeEcosystemContainer
    {
        public int schema_version { get; set; } = 1;
        public List<FaunaSpeciesDef> species { get; set; } = new List<FaunaSpeciesDef>();
        public List<PredatorPreyEdgeDef> predator_prey { get; set; } = new List<PredatorPreyEdgeDef>();
        public List<SeasonalMoveDef> seasonal_moves { get; set; } = new List<SeasonalMoveDef>();
    }

    [Serializable]
    public sealed class PressureKeyState
    {
        public string sector_id { get; set; } = string.Empty;
        public string species_id { get; set; } = string.Empty;
        public int pressure { get; set; }
    }

    [Serializable]
    public sealed class ApexActivityState
    {
        public string species_id { get; set; } = string.Empty;
        public string sector_id { get; set; } = string.Empty;
        public int since_day { get; set; }
        public int until_day { get; set; }
        public bool spotted_reported { get; set; }
    }

    [Serializable]
    public sealed class DomesticAnimalState
    {
        public string animal_id { get; set; } = string.Empty;
        public string species_id { get; set; } = string.Empty;
        public int tamed_day { get; set; }
        public string caretaker_id { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class WildlifeObservation
    {
        public string species_id { get; set; } = string.Empty;
        public string sector_id { get; set; } = string.Empty;
        public int day { get; set; }
        public float confidence { get; set; } = 1f;
    }

    [Serializable]
    public sealed class WildlifeEcosystemState
    {
        public string system_id { get; set; } = "wildlife_ecosystem";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; }
        public List<PressureKeyState> pressures { get; set; } = new List<PressureKeyState>();
        public List<string> extinct_species_sectors { get; set; } = new List<string>(); // "sector|species"
        public List<ApexActivityState> apex_activities { get; set; } = new List<ApexActivityState>();
        public List<DomesticAnimalState> domestic_animals { get; set; } = new List<DomesticAnimalState>();
        public List<WildlifeObservation> observations { get; set; } = new List<WildlifeObservation>();
        public int domestic_counter;
    }

    public sealed class WildlifeEcosystemSystem
    {
        public const string SystemId = "wildlife_ecosystem";
        public const int ExtinctionThreshold = 2;      // remnant pair = locally extinct
        public const int RecolonizationPopulation = 4;
        public const int ApexDurationDays = 5;
        public const int ObservationLogCapacity = 200;
        public const int PressureDecayPerDay = 1;

        private readonly WildlifeEcosystemContainer _catalog = new WildlifeEcosystemContainer();
        private readonly Dictionary<string, FaunaSpeciesDef> _speciesById =
            new Dictionary<string, FaunaSpeciesDef>(StringComparer.Ordinal);
        private readonly WildlifeEcosystemState _state = new WildlifeEcosystemState();

        public event Action<string, string>? OnWildlifeObserved;             // species, sector
        public event Action<string, string>? OnLocalExtinction;              // species, sector
        public event Action<string, string>? OnApexPredatorSpotted;          // species, sector
        public event Action<DomesticAnimalState>? OnWildlifeTamed;
        public event Action<string, string, int>? OnWildlifePopulationShifted; // species, sector, delta

        public string SaveId => SystemId;
        public WildlifeEcosystemState State => _state;
        public WildlifeEcosystemContainer Catalog => _catalog;
        public IReadOnlyList<DomesticAnimalState> DomesticAnimals => _state.domestic_animals;
        public IReadOnlyList<ApexActivityState> ApexActivities => _state.apex_activities;
        public IReadOnlyList<WildlifeObservation> Observations => _state.observations;

        public void LoadCatalog(WildlifeEcosystemContainer catalog)
        {
            _catalog.species = new List<FaunaSpeciesDef>();
            _catalog.predator_prey = new List<PredatorPreyEdgeDef>();
            _catalog.seasonal_moves = new List<SeasonalMoveDef>();
            _speciesById.Clear();
            if (catalog == null) return;
            _catalog.species.AddRange(catalog.species ?? new List<FaunaSpeciesDef>());
            _catalog.predator_prey.AddRange(catalog.predator_prey ?? new List<PredatorPreyEdgeDef>());
            _catalog.seasonal_moves.AddRange(catalog.seasonal_moves ?? new List<SeasonalMoveDef>());
            _catalog.schema_version = catalog.schema_version;
            foreach (var s in _catalog.species)
                if (s != null && !string.IsNullOrEmpty(s.id)) _speciesById[s.id] = s;
            _catalog.species.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            _catalog.predator_prey.Sort((a, b) =>
                string.CompareOrdinal(a.predator_species_id + "|" + a.prey_species_id,
                    b.predator_species_id + "|" + b.prey_species_id));
        }

        public FaunaSpeciesDef? Species(string speciesId) =>
            _speciesById.TryGetValue(speciesId ?? string.Empty, out var s) ? s : null;

        // ------------------------------------------------------------------
        // Derived queries (single population source: migration packs)
        // ------------------------------------------------------------------

        public int SectorSpeciesPopulation(WildlifeMigrationSystem migration, string sectorId, string speciesId)
        {
            if (migration?.State?.packs == null) return 0;
            int total = 0;
            foreach (var p in migration.State.packs)
                if (p != null && string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)
                    && string.Equals(p.speciesId, speciesId, StringComparison.Ordinal))
                    total += p.population;
            return total;
        }

        public bool IsLocallyExtinct(string sectorId, string speciesId) =>
            _state.extinct_species_sectors.Contains(ExtinctionKey(sectorId, speciesId));

        /// <summary>Density multiplier for the trapping host (plan §8.3): one
        /// population source, so trap checks and the ecosystem can never
        /// diverge. Extinct species contribute nothing.</summary>
        public float SectorDensityMultiplier(WildlifeMigrationSystem migration, string sectorId)
        {
            if (migration?.State?.packs == null) return 0f;
            int population = 0;
            foreach (var p in migration.State.packs)
            {
                if (p == null || !string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)) continue;
                if (IsLocallyExtinct(sectorId, p.speciesId)) continue;
                population += p.population;
            }
            return Math.Clamp(population / 15f, 0f, 1.5f);
        }

        public static string ExtinctionKey(string sectorId, string speciesId) => sectorId + "|" + speciesId;

        // ------------------------------------------------------------------
        // Pressure (hunting / trapping feedback)
        // ------------------------------------------------------------------

        public void RecordHuntingPressure(string sectorId, string speciesId, int amount)
        {
            if (amount <= 0) return;
            var key = _state.pressures.Find(p =>
                p != null && string.Equals(p.sector_id, sectorId, StringComparison.Ordinal)
                && string.Equals(p.species_id, speciesId, StringComparison.Ordinal));
            if (key == null)
            {
                key = new PressureKeyState { sector_id = sectorId, species_id = speciesId };
                _state.pressures.Add(key);
                _state.pressures.Sort((a, b) => string.CompareOrdinal(
                    a.sector_id + "|" + a.species_id, b.sector_id + "|" + b.species_id));
            }
            key.pressure += amount;
        }

        public int PressureOn(string sectorId, string speciesId)
        {
            var key = _state.pressures.Find(p =>
                p != null && string.Equals(p.sector_id, sectorId, StringComparison.Ordinal)
                && string.Equals(p.species_id, speciesId, StringComparison.Ordinal));
            return key?.pressure ?? 0;
        }

        // ------------------------------------------------------------------
        // Observations / bestiary knowledge
        // ------------------------------------------------------------------

        public void RecordObservation(string speciesId, string sectorId, int day, float confidence = 1f)
        {
            _state.observations.Add(new WildlifeObservation
            {
                species_id = speciesId,
                sector_id = sectorId,
                day = day,
                confidence = Math.Clamp(confidence, 0f, 1f)
            });
            if (_state.observations.Count > ObservationLogCapacity)
                _state.observations.RemoveRange(0, _state.observations.Count - ObservationLogCapacity);
            OnWildlifeObserved?.Invoke(speciesId, sectorId);
        }

        public int ObservationCount(string speciesId)
        {
            int count = 0;
            foreach (var o in _state.observations)
                if (o != null && string.Equals(o.species_id, speciesId, StringComparison.Ordinal)) count++;
            return count;
        }

        /// <summary>Knowledge-gated bestiary level (plan §8.23): exact
        /// populations stay hidden until Fully Documented.</summary>
        public string KnowledgeLevel(string speciesId)
        {
            int count = ObservationCount(speciesId);
            if (count >= 6) return "documented";
            if (count >= 3) return "studied";
            if (count >= 1) return "observed";
            return "unknown";
        }

        // ------------------------------------------------------------------
        // Daily ecology tick (plan §8.10 order: predation → pressure decay →
        // seasonal moves → extinction/recolonization → apex)
        // ------------------------------------------------------------------

        public void TickDay(
            int day,
            WildlifeMigrationSystem migration,
            float outdoorRadModifier,
            string seasonWindowId,
            ISeededRng populationRng,
            ISeededRng migrationRng,
            ISeededRng apexRng)
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;
            if (migration?.State?.packs == null) return;

            float radPressure = Math.Clamp(outdoorRadModifier / 250f, 0f, 1.5f);

            // 1. Predation: thin prey through the ONE population authority,
            //    remnant pair preserved (extinction is a flagged state, not a
            //    wipe). Radiation thins intolerant species the same way.
            foreach (var edge in _catalog.predator_prey)
            {
                if (edge == null) continue;
                var sectors = SectorsOfSpecies(migration, edge.prey_species_id);
                foreach (var sector in sectors)
                {
                    int prey = SectorSpeciesPopulation(migration, sector, edge.prey_species_id);
                    int removable = Math.Max(0, prey - ExtinctionThreshold);
                    int remove = (int)Math.Round(removable * edge.predation_pressure);
                    if (remove <= 0) continue;
                    migration.ThinSpeciesInSector(edge.prey_species_id, sector, remove, ExtinctionThreshold);
                    OnWildlifePopulationShifted?.Invoke(edge.prey_species_id, sector, -remove);
                }
            }

            foreach (var species in _catalog.species)
            {
                if (species == null || species.radiation_tolerance >= radPressure) continue;
                foreach (var sector in SectorsOfSpecies(migration, species.id))
                {
                    // Over-tolerance radiation: deterministic 1/day attrition
                    // for species outside their tolerance band.
                    if (populationRng != null && populationRng.NextDouble() < 0.5)
                    {
                        migration.ThinSpeciesInSector(species.id, sector, 1, ExtinctionThreshold);
                        OnWildlifePopulationShifted?.Invoke(species.id, sector, -1);
                    }
                }
            }

            // 2. Pressure decay (recovery from hunting).
            for (int i = _state.pressures.Count - 1; i >= 0; i--)
            {
                var p = _state.pressures[i];
                if (p == null) continue;
                p.pressure -= PressureDecayPerDay;
                if (p.pressure <= 0) _state.pressures.RemoveAt(i);
            }

            // 3. Seasonal moves through the migration authority's own routing.
            if (migrationRng != null)
            {
                foreach (var move in _catalog.seasonal_moves)
                {
                    if (move == null || !string.Equals(move.season_window_id, seasonWindowId, StringComparison.Ordinal))
                        continue;
                    foreach (var pack in migration.State.packs)
                    {
                        if (pack == null || !string.Equals(pack.speciesId, move.species_id, StringComparison.Ordinal))
                            continue;
                        if (migrationRng.NextDouble() >= move.move_chance) continue;
                        if (!migration.TryGetNeighbors(pack.currentSectorId, out var neighbors)
                            || neighbors == null || neighbors.Count == 0) continue;
                        int index = migrationRng.Next(0, neighbors.Count);
                        migration.MigratePack(pack.packId, neighbors[index]);
                        break; // one seasonal move per species per day
                    }
                }
            }

            // 4. Extinction / recolonization flags over the pack populations.
            var flags = new HashSet<string>(_state.extinct_species_sectors, StringComparer.Ordinal);
            foreach (var sector in AllSectors(migration))
            {
                foreach (var species in _catalog.species)
                {
                    if (species == null) continue;
                    int pop = SectorSpeciesPopulation(migration, sector, species.id);
                    string key = ExtinctionKey(sector, species.id);
                    if (pop > 0 && pop <= ExtinctionThreshold && !flags.Contains(key))
                    {
                        flags.Add(key);
                        _state.extinct_species_sectors.Add(key);
                        OnLocalExtinction?.Invoke(species.id, sector);
                    }
                    else if (pop >= RecolonizationPopulation && flags.Contains(key))
                    {
                        flags.Remove(key);
                        _state.extinct_species_sectors.Remove(key);
                    }
                }
            }

            // 5. Apex activity: deterministic trigger over population thresholds.
            for (int i = _state.apex_activities.Count - 1; i >= 0; i--)
            {
                var a = _state.apex_activities[i];
                if (a != null && day > a.until_day) _state.apex_activities.RemoveAt(i);
            }
            if (apexRng != null)
            {
                foreach (var species in _catalog.species)
                {
                    if (species == null || species.apex_population_threshold <= 0) continue;
                    bool already = _state.apex_activities.Exists(a =>
                        a != null && string.Equals(a.species_id, species.id, StringComparison.Ordinal));
                    if (already) continue;
                    int total = GlobalPopulation(migration, species.id);
                    if (total < species.apex_population_threshold) continue;
                    if (apexRng.NextDouble() >= 0.1) continue;
                    string? sector = LargestSectorOf(migration, species.id);
                    if (sector == null) continue;
                    var activity = new ApexActivityState
                    {
                        species_id = species.id,
                        sector_id = sector,
                        since_day = day,
                        until_day = day + ApexDurationDays
                    };
                    _state.apex_activities.Add(activity);
                    OnApexPredatorSpotted?.Invoke(species.id, sector);
                }
            }
        }

        private List<string> SectorsOfSpecies(WildlifeMigrationSystem migration, string speciesId)
        {
            var sectors = new List<string>();
            foreach (var p in migration.State.packs)
            {
                if (p == null || !string.Equals(p.speciesId, speciesId, StringComparison.Ordinal)) continue;
                if (!sectors.Contains(p.currentSectorId)) sectors.Add(p.currentSectorId);
            }
            sectors.Sort(StringComparer.Ordinal);
            return sectors;
        }

        private List<string> AllSectors(WildlifeMigrationSystem migration)
        {
            var sectors = new List<string>();
            foreach (var p in migration.State.packs)
                if (p != null && !sectors.Contains(p.currentSectorId)) sectors.Add(p.currentSectorId);
            sectors.Sort(StringComparer.Ordinal);
            return sectors;
        }

        private int GlobalPopulation(WildlifeMigrationSystem migration, string speciesId)
        {
            int total = 0;
            foreach (var p in migration.State.packs)
                if (p != null && string.Equals(p.speciesId, speciesId, StringComparison.Ordinal))
                    total += p.population;
            return total;
        }

        private string? LargestSectorOf(WildlifeMigrationSystem migration, string speciesId)
        {
            string? best = null;
            int bestPop = 0;
            var counts = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var p in migration.State.packs)
            {
                if (p == null || !string.Equals(p.speciesId, speciesId, StringComparison.Ordinal)) continue;
                counts.TryGetValue(p.currentSectorId, out int c);
                counts[p.currentSectorId] = c + p.population;
            }
            foreach (var kv in counts)
            {
                if (kv.Value > bestPop || (kv.Value == bestPop && best != null && string.CompareOrdinal(kv.Key, best) < 0))
                {
                    bestPop = kv.Value;
                    best = kv.Key;
                }
            }
            return best;
        }

        // ------------------------------------------------------------------
        // Taming (plan §8.20-8.21: authority transfer OUT of the wild)
        // ------------------------------------------------------------------

        public bool CanTame(string speciesId) =>
            Species(speciesId) is { tameable: true };

        public DomesticAnimalState? TryTame(
            string speciesId, string sectorId, int day, string caretakerId,
            WildlifeMigrationSystem migration, ISeededRng tamingRng)
        {
            var species = Species(speciesId);
            if (species == null || !species.tameable || tamingRng == null) return null;
            if (migration == null
                || SectorSpeciesPopulation(migration, sectorId, speciesId) <= ExtinctionThreshold)
                return null;
            if (tamingRng.NextDouble() >= species.tameness_chance) return null;

            // One unit leaves the wild population through the one authority.
            migration.ThinSpeciesInSector(speciesId, sectorId, 1, ExtinctionThreshold);
            _state.domestic_counter++;
            var animal = new DomesticAnimalState
            {
                animal_id = $"domestic_{_state.domestic_counter}",
                species_id = speciesId,
                tamed_day = day,
                caretaker_id = caretakerId ?? string.Empty
            };
            _state.domestic_animals.Add(animal);
            OnWildlifeTamed?.Invoke(animal);
            return animal;
        }

        // ------------------------------------------------------------------
        // Save
        // ------------------------------------------------------------------

        public WildlifeEcosystemState CaptureState()
        {
            var copy = new WildlifeEcosystemState
            {
                last_tick_day = _state.last_tick_day,
                domestic_counter = _state.domestic_counter,
                extinct_species_sectors = new List<string>(_state.extinct_species_sectors),
                pressures = new List<PressureKeyState>(_state.pressures.Count),
                apex_activities = new List<ApexActivityState>(_state.apex_activities.Count),
                domestic_animals = new List<DomesticAnimalState>(_state.domestic_animals.Count),
                observations = new List<WildlifeObservation>(_state.observations.Count)
            };
            foreach (var p in _state.pressures)
                copy.pressures.Add(new PressureKeyState
                { sector_id = p.sector_id, species_id = p.species_id, pressure = p.pressure });
            foreach (var a in _state.apex_activities)
                copy.apex_activities.Add(new ApexActivityState
                {
                    species_id = a.species_id, sector_id = a.sector_id,
                    since_day = a.since_day, until_day = a.until_day, spotted_reported = a.spotted_reported
                });
            foreach (var d in _state.domestic_animals)
                copy.domestic_animals.Add(new DomesticAnimalState
                {
                    animal_id = d.animal_id, species_id = d.species_id,
                    tamed_day = d.tamed_day, caretaker_id = d.caretaker_id
                });
            foreach (var o in _state.observations)
                copy.observations.Add(new WildlifeObservation
                {
                    species_id = o.species_id, sector_id = o.sector_id,
                    day = o.day, confidence = o.confidence
                });
            return copy;
        }

        public void RestoreState(WildlifeEcosystemState? state)
        {
            if (state == null) return;
            _state.last_tick_day = state.last_tick_day;
            _state.domestic_counter = state.domestic_counter;
            _state.extinct_species_sectors = state.extinct_species_sectors != null
                ? new List<string>(state.extinct_species_sectors) : new List<string>();
            _state.pressures = new List<PressureKeyState>(state.pressures?.Count ?? 0);
            if (state.pressures != null)
                foreach (var p in state.pressures)
                    _state.pressures.Add(new PressureKeyState
                    { sector_id = p.sector_id, species_id = p.species_id, pressure = p.pressure });
            _state.apex_activities = new List<ApexActivityState>(state.apex_activities?.Count ?? 0);
            if (state.apex_activities != null)
                foreach (var a in state.apex_activities)
                    _state.apex_activities.Add(new ApexActivityState
                    {
                        species_id = a.species_id, sector_id = a.sector_id,
                        since_day = a.since_day, until_day = a.until_day, spotted_reported = a.spotted_reported
                    });
            _state.domestic_animals = new List<DomesticAnimalState>(state.domestic_animals?.Count ?? 0);
            if (state.domestic_animals != null)
                foreach (var d in state.domestic_animals)
                    _state.domestic_animals.Add(new DomesticAnimalState
                    {
                        animal_id = d.animal_id, species_id = d.species_id,
                        tamed_day = d.tamed_day, caretaker_id = d.caretaker_id
                    });
            _state.observations = new List<WildlifeObservation>(state.observations?.Count ?? 0);
            if (state.observations != null)
                foreach (var o in state.observations)
                    _state.observations.Add(new WildlifeObservation
                    {
                        species_id = o.species_id, sector_id = o.sector_id,
                        day = o.day, confidence = o.confidence
                    });
        }
    }

    public static class WildlifeEcosystemCatalogLoader
    {
        public const string DefaultFileName = "wildlife_ecosystem.json";

        public static WildlifeEcosystemContainer Load(
            string dataDir, IFileIO? files = null, IJsonSerializer? json = null)
        {
            files ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();
            var path = System.IO.Path.Combine(dataDir ?? string.Empty, DefaultFileName);
            if (!files.FileExists(path)) return new WildlifeEcosystemContainer();
            try
            {
                var text = files.ReadAllText(path);
                return json.Deserialize<WildlifeEcosystemContainer>(text) ?? new WildlifeEcosystemContainer();
            }
            catch (Exception)
            {
                return new WildlifeEcosystemContainer();
            }
        }

        public static List<string> Validate(WildlifeEcosystemContainer catalog)
        {
            var diags = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var s in catalog.species)
            {
                if (s == null) continue;
                if (string.IsNullOrEmpty(s.id) || !s.id.StartsWith("species_", StringComparison.Ordinal))
                    diags.Add($"{s.id}: id must use the species_ prefix");
                else if (!seen.Add(s.id))
                    diags.Add($"{s.id}: duplicate species id");
                if (s.radiation_tolerance < 0f || s.radiation_tolerance > 1f)
                    diags.Add($"{s.id}: radiation_tolerance must be within [0,1]");
                if (s.tameness_chance < 0f || s.tameness_chance > 1f)
                    diags.Add($"{s.id}: tameness_chance must be within [0,1]");
            }
            foreach (var e in catalog.predator_prey)
            {
                if (e == null) continue;
                if (!seen.Contains(e.predator_species_id))
                    diags.Add($"edge {e.predator_species_id}->{e.prey_species_id}: predator not defined");
                if (!seen.Contains(e.prey_species_id))
                    diags.Add($"edge {e.predator_species_id}->{e.prey_species_id}: prey not defined");
                if (string.Equals(e.predator_species_id, e.prey_species_id, StringComparison.Ordinal))
                    diags.Add($"edge {e.predator_species_id}: self-predation is not supported");
                if (e.predation_pressure < 0f || e.predation_pressure > 0.5f)
                    diags.Add($"edge {e.predator_species_id}->{e.prey_species_id}: pressure must be within [0,0.5]");
            }
            foreach (var m in catalog.seasonal_moves)
            {
                if (m == null) continue;
                if (!seen.Contains(m.species_id))
                    diags.Add($"move {m.species_id}: species not defined");
                if (m.move_chance < 0f || m.move_chance > 1f)
                    diags.Add($"move {m.species_id}: move_chance must be within [0,1]");
            }
            diags.Sort(StringComparer.Ordinal);
            return diags;
        }
    }
}
