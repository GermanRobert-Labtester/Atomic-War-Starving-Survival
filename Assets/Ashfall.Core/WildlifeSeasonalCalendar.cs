using System;
using System.Collections.Generic;
using Ashfall.Core.World;

namespace Ashfall.Core
{
    /// <summary>
    /// Ecological role a seeded species plays across the annual cycle.
    /// Archetypes are assigned per species id (never per pack), so a species
    /// always behaves as one biological population pattern.
    /// </summary>
    public enum MigrationArchetype
    {
        /// <summary>Year-round holder; hunger tracks forage and winter cold, not season windows.</summary>
        Resident,
        /// <summary>Large grazing herd; winter range collapse, thaw recovery, rut movement late in the year.</summary>
        HerdGrazer,
        /// <summary>Burrowing prey that booms on grain and forage, then seeks warm ground in the freeze.</summary>
        BurrowSwarm,
        /// <summary>Omnivorous sounder; peaks on the mast fall in The Turning.</summary>
        Sounder,
        /// <summary>Passage birds; movement windows in thaw and the turning, thin in deep cold.</summary>
        PassageFlock,
        /// <summary>Water-bound runner; the fish run is a thaw-to-bloom population boom in the waterway pair.</summary>
        CoastalRunner,
        /// <summary>Warm-damp insect bloom; near-absent in hard cold, crop pressure in the bloom window.</summary>
        SwarmBlight
    }

    /// <summary>
    /// Plan 28 — deterministic seasonal ecology calendar.
    ///
    /// Pure functions only: the Plan 19 weather-season windows
    /// (<see cref="SeasonProfileDef"/> from weather_seasons.json) remain the
    /// sole season authority, and this calendar never holds campaign state.
    /// Archetypes modulate the existing hunger/birth/movement rules inside
    /// <see cref="WildlifeMigrationSystem.TickDay(int, ISeededRng?)"/> — no new
    /// population simulator, no wall-clock, no RNG of its own.
    /// </summary>
    public static class WildlifeSeasonalCalendar
    {
        // ── Season window ids (weather_seasons.json is the authority) ───
        public const string SeasonAshfall = "window_ashfall";
        public const string SeasonDeepFreeze = "window_deep_freeze";
        public const string SeasonThaw = "window_thaw";
        public const string SeasonBlackBloom = "window_black_bloom";
        public const string SeasonHighCold = "window_high_cold";
        public const string SeasonTheTurning = "window_the_turning";

        /// <summary>Hunger factor bounds. 1.0 keeps the authored +0.05/day cadence.</summary>
        public const float HungerFactorMin = 0.6f;
        public const float HungerFactorMax = 1.5f;

        /// <summary>Abundance (presence) factor bounds feeding trapping density and notices.</summary>
        public const float AbundanceFactorMin = 0.2f;
        public const float AbundanceFactorMax = 1.5f;

        /// <summary>
        /// Species → archetype table for the seeded fauna. Species absent from
        /// the table read as <see cref="MigrationArchetype.Resident"/>.
        /// </summary>
        public static MigrationArchetype ArchetypeOf(string speciesId) => speciesId switch
        {
            "species_rad_dog" => MigrationArchetype.Resident,
            "species_wolf" => MigrationArchetype.Resident,
            "species_dust_lynx" => MigrationArchetype.Resident,
            "species_feral_goat" => MigrationArchetype.HerdGrazer,
            "species_blight_rat" => MigrationArchetype.BurrowSwarm,
            "species_ash_boar" => MigrationArchetype.Sounder,
            "species_iron_crow" => MigrationArchetype.PassageFlock,
            "species_ash_gull" => MigrationArchetype.PassageFlock,
            "species_cotton_hare" => MigrationArchetype.BurrowSwarm,
            "species_gray_heron" => MigrationArchetype.CoastalRunner,
            "species_mirror_carp" => MigrationArchetype.CoastalRunner,
            "species_ghost_moth" => MigrationArchetype.SwarmBlight,
            _ => MigrationArchetype.Resident
        };

        /// <summary>
        /// Season window for a campaign day from the Plan 19 profile; mirrors
        /// <see cref="WeatherSystem.GetSeasonForDay"/> (last window whose
        /// startDay ≤ day). Pure and save-neutral.
        /// </summary>
        public static SeasonWindowDef SeasonWindowForDay(SeasonProfileDef? profile, int day)
        {
            if (profile?.seasons == null || profile.seasons.Count == 0) return null!;
            SeasonWindowDef? current = null;
            for (int i = 0; i < profile.seasons.Count; i++)
            {
                var s = profile.seasons[i];
                if (s != null && s.startDay <= day && (current == null || s.startDay >= current.startDay))
                    current = s;
            }
            return current!;
        }

        /// <summary>Multiplier on the daily starvation growth for a pack of this archetype in this window.</summary>
        public static float HungerFactor(SeasonWindowDef? season, MigrationArchetype archetype)
        {
            float f = (season?.id, archetype) switch
            {
                // Resident predators: winter is lean, the thaw feeds.
                (SeasonDeepFreeze, MigrationArchetype.Resident) => 1.15f,
                (SeasonHighCold, MigrationArchetype.Resident) => 1.15f,
                (SeasonThaw, MigrationArchetype.Resident) => 0.9f,

                // Grazing herds: winter range is thin, thaw country is rich.
                (SeasonDeepFreeze, MigrationArchetype.HerdGrazer) => 1.3f,
                (SeasonHighCold, MigrationArchetype.HerdGrazer) => 1.1f,
                (SeasonThaw, MigrationArchetype.HerdGrazer) => 0.75f,
                (SeasonBlackBloom, MigrationArchetype.HerdGrazer) => 0.8f,

                // Burrowers ride grain and warmth; hard cold drives them under.
                (SeasonDeepFreeze, MigrationArchetype.BurrowSwarm) => 1.2f,
                (SeasonThaw, MigrationArchetype.BurrowSwarm) => 0.8f,
                (SeasonBlackBloom, MigrationArchetype.BurrowSwarm) => 0.9f,
                (SeasonHighCold, MigrationArchetype.BurrowSwarm) => 1.1f,
                (SeasonAshfall, MigrationArchetype.BurrowSwarm) => 1.05f,

                // Sounders peak on the mast fall in The Turning.
                (SeasonTheTurning, MigrationArchetype.Sounder) => 0.7f,
                (SeasonDeepFreeze, MigrationArchetype.Sounder) => 1.2f,
                (SeasonThaw, MigrationArchetype.Sounder) => 0.85f,
                (SeasonHighCold, MigrationArchetype.Sounder) => 1.05f,

                // Passage birds: hard winters empty the sky.
                (SeasonDeepFreeze, MigrationArchetype.PassageFlock) => 1.25f,
                (SeasonThaw, MigrationArchetype.PassageFlock) => 0.8f,
                (SeasonHighCold, MigrationArchetype.PassageFlock) => 1.15f,

                // The fish run: thaw and bloom fill the water, ice starves it.
                (SeasonDeepFreeze, MigrationArchetype.CoastalRunner) => 1.3f,
                (SeasonThaw, MigrationArchetype.CoastalRunner) => 0.7f,
                (SeasonBlackBloom, MigrationArchetype.CoastalRunner) => 0.8f,
                (SeasonHighCold, MigrationArchetype.CoastalRunner) => 1.1f,
                (SeasonAshfall, MigrationArchetype.CoastalRunner) => 1.2f,

                // The moth bloom: warm damp country, gone by first hard cold.
                (SeasonBlackBloom, MigrationArchetype.SwarmBlight) => 0.5f,
                (SeasonThaw, MigrationArchetype.SwarmBlight) => 0.9f,
                (SeasonDeepFreeze, MigrationArchetype.SwarmBlight) => 1.5f,
                (SeasonHighCold, MigrationArchetype.SwarmBlight) => 1.3f,
                (SeasonAshfall, MigrationArchetype.SwarmBlight) => 1.3f,

                _ => 1f
            };
            return Math.Clamp(f, HungerFactorMin, HungerFactorMax);
        }

        /// <summary>
        /// How present ( huntable, trappable, visible ) this archetype is in
        /// the given season window. Feeds the trapping density composition and
        /// the coarse map/radio projections — abundance, not population math.
        /// </summary>
        public static float AbundanceFactor(SeasonWindowDef? season, MigrationArchetype archetype)
        {
            float f = (season?.id, archetype) switch
            {
                (SeasonDeepFreeze, MigrationArchetype.HerdGrazer) => 0.6f,
                (SeasonThaw, MigrationArchetype.HerdGrazer) => 1.2f,
                (SeasonBlackBloom, MigrationArchetype.HerdGrazer) => 1.25f,
                (SeasonHighCold, MigrationArchetype.HerdGrazer) => 0.9f,
                (SeasonTheTurning, MigrationArchetype.HerdGrazer) => 1.1f,

                (SeasonDeepFreeze, MigrationArchetype.BurrowSwarm) => 0.8f,
                (SeasonThaw, MigrationArchetype.BurrowSwarm) => 1.3f,
                (SeasonBlackBloom, MigrationArchetype.BurrowSwarm) => 1.3f,
                (SeasonHighCold, MigrationArchetype.BurrowSwarm) => 0.7f,

                (SeasonDeepFreeze, MigrationArchetype.Sounder) => 0.9f,
                (SeasonThaw, MigrationArchetype.Sounder) => 1.0f,
                (SeasonBlackBloom, MigrationArchetype.Sounder) => 1.1f,
                (SeasonHighCold, MigrationArchetype.Sounder) => 0.9f,
                (SeasonTheTurning, MigrationArchetype.Sounder) => 1.4f,

                (SeasonDeepFreeze, MigrationArchetype.PassageFlock) => 0.4f,
                (SeasonThaw, MigrationArchetype.PassageFlock) => 1.3f,
                (SeasonBlackBloom, MigrationArchetype.PassageFlock) => 1.0f,
                (SeasonHighCold, MigrationArchetype.PassageFlock) => 0.6f,
                (SeasonTheTurning, MigrationArchetype.PassageFlock) => 1.25f,

                // The fish run: water wakes in the thaw, runs through the bloom.
                (SeasonDeepFreeze, MigrationArchetype.CoastalRunner) => 0.2f,
                (SeasonThaw, MigrationArchetype.CoastalRunner) => 1.5f,
                (SeasonBlackBloom, MigrationArchetype.CoastalRunner) => 1.4f,
                (SeasonHighCold, MigrationArchetype.CoastalRunner) => 0.6f,
                (SeasonAshfall, MigrationArchetype.CoastalRunner) => 0.8f,
                (SeasonTheTurning, MigrationArchetype.CoastalRunner) => 0.8f,

                // The moth bloom: one warm damp window, then nothing.
                (SeasonBlackBloom, MigrationArchetype.SwarmBlight) => 1.5f,
                (SeasonThaw, MigrationArchetype.SwarmBlight) => 0.9f,
                (SeasonDeepFreeze, MigrationArchetype.SwarmBlight) => 0.1f,
                (SeasonHighCold, MigrationArchetype.SwarmBlight) => 0.4f,
                (SeasonAshfall, MigrationArchetype.SwarmBlight) => 0.6f,
                (SeasonTheTurning, MigrationArchetype.SwarmBlight) => 0.5f,

                _ => 1f
            };
            return Math.Clamp(f, AbundanceFactorMin, AbundanceFactorMax);
        }

        /// <summary>
        /// Mean abundance factor of the packs currently holding
        /// <paramref name="sectorId"/>; 1.0 when no pack or no season stands
        /// there. Deterministic: same day, same packs, same factor.
        /// </summary>
        public static float SectorAbundanceFactor(
            SeasonProfileDef? profile, int day, string sectorId,
            IEnumerable<WildlifePackRecord>? packs)
        {
            if (profile == null || packs == null || string.IsNullOrEmpty(sectorId)) return 1f;
            var season = profile.seasons is { Count: > 0 } ? SeasonWindowForDay(profile, day) : null;
            float sum = 0f;
            int n = 0;
            foreach (var p in packs)
            {
                if (p == null || !string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)) continue;
                sum += AbundanceFactor(season, ArchetypeOf(p.speciesId));
                n++;
            }
            return n == 0 ? 1f : Math.Clamp(sum / n, AbundanceFactorMin, AbundanceFactorMax);
        }

        /// <summary>
        /// Water-bound runners only move along water; everywhere else the
        /// hunger drive may pick any neighbor. A runner stranded on dry ground
        /// (legacy save) may cross it to reach water again.
        /// </summary>
        public static List<string> FilterNeighbors(
            MigrationArchetype archetype, string currentSectorId,
            List<string> neighbors, HashSet<string>? waterSectors)
        {
            if (neighbors == null || neighbors.Count == 0) return neighbors ?? new List<string>();
            if (archetype != MigrationArchetype.CoastalRunner || waterSectors == null || waterSectors.Count == 0)
                return neighbors;

            bool inWater = waterSectors.Contains(currentSectorId);
            var water = new List<string>();
            foreach (var n in neighbors)
                if (!string.IsNullOrEmpty(n) && waterSectors.Contains(n)) water.Add(n);

            if (inWater && water.Count > 0) return water;
            if (!inWater && water.Count > 0) return water; // head for the nearest water
            return neighbors; // stranded; any move is allowed
        }

        /// <summary>
        /// Plan 28 Phase 5 — observation map: a seeded species sighted in the
        /// wild unlocks its "reading the land" field-guide entry (Plan 20A).
        /// Species absent from the map have no field-guide teach link.
        /// </summary>
        public static string? FieldGuideEntryFor(string speciesId) => speciesId switch
        {
            "species_wolf" => "field_guide_scat_predator_marking",
            "species_feral_goat" => "field_guide_browsing_stripped_bark",
            "species_iron_crow" => "field_guide_birdsong_silence_omen",
            "species_ash_gull" => "field_guide_carrion_circling_watch",
            "species_ghost_moth" => "field_guide_termite_soil_drill",
            "species_ash_boar" => "field_guide_caribou_rut_track",
            _ => null
        };

        /// <summary>
        /// Coarse, player-plausible notice for a pack seen moving between
        /// sectors. Never exposes exact population; wording stays scout-radio
        /// plain. Returns null for archetypes with no observation value.
        /// </summary>
        public static string? MigrationNotice(
            MigrationArchetype archetype, string speciesId, string fromSector, string toSector, int day)
        {
            if (string.IsNullOrEmpty(speciesId)) return null;
            return archetype switch
            {
                MigrationArchetype.HerdGrazer =>
                    $"wildlife net: grazing herd sighted leaving {fromSector} for {toSector} (day {day})",
                MigrationArchetype.Sounder =>
                    $"wildlife net: boar sign heavy on the {fromSector}–{toSector} line (day {day})",
                MigrationArchetype.PassageFlock =>
                    $"wildlife net: passage birds moving {fromSector} toward {toSector} (day {day})",
                MigrationArchetype.CoastalRunner =>
                    $"wildlife net: fish running the water at {toSector} (day {day})",
                MigrationArchetype.SwarmBlight =>
                    $"wildlife net: insect front drifting from {fromSector} toward {toSector} (day {day})",
                MigrationArchetype.BurrowSwarm =>
                    $"wildlife net: burrower sign spreading out of {fromSector} (day {day})",
                _ => null
            };
        }
    }
}
