// GameBootstrap.Biomes.cs — boot/wire Biome_* expedition terrain modifiers.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct all Biome_* trackers. Host hooks are offline-safe logs;
        /// expedition hosts call Enter/Traverse/Scout when parties hit biome tiles.
        /// </summary>
        private void BootBiomes()
        {
            BiomeAshSwamp = new Biome_AshSwamp();
            BiomeGlassDesert = new Biome_GlassDesert();
            BiomeHighwayTunnel = new Biome_HighwayTunnel();
            BiomeSaltFlats = new Biome_SaltFlats();
            BiomeSkyscraperTops = new Biome_SkyscraperTops();
            BiomeSuburbs = new Biome_Suburbs();

            WireBiomes();
            Debug.Log("[GameBootstrap] Biomes ready (6 trackers).");
        }

        private void WireBiomes()
        {
            if (BiomeAshSwamp != null)
            {
                BiomeAshSwamp.OnMovementPenalized += id =>
                    Debug.Log($"[GameBootstrap] BIOME: ash swamp movement penalty — '{id}'");
                BiomeAshSwamp.OnParasiteContracted += id =>
                    Debug.Log($"[GameBootstrap] BIOME: ash swamp parasite — '{id}'");
                BiomeAshSwamp.OnStealthFailed += id =>
                    Debug.Log($"[GameBootstrap] BIOME: ash swamp stealth failed — '{id}'");
            }

            if (BiomeGlassDesert != null)
            {
                BiomeGlassDesert.OnSniperExposed += id =>
                    Debug.Log($"[GameBootstrap] BIOME: glass desert sniper exposed — '{id}'");
                BiomeGlassDesert.OnHeatStress += id =>
                    Debug.Log($"[GameBootstrap] BIOME: glass desert heat stress — '{id}'");
                BiomeGlassDesert.OnVitrifiedGlassFound += (id, n) =>
                    Debug.Log($"[GameBootstrap] BIOME: glass desert vitrified glass x{n} — '{id}'");
            }

            if (BiomeHighwayTunnel != null)
            {
                BiomeHighwayTunnel.OnDarknessPenalty += id =>
                    Debug.Log($"[GameBootstrap] BIOME: highway tunnel darkness — '{id}'");
                BiomeHighwayTunnel.OnGasSiphoned += (id, vehicle) =>
                    Debug.Log($"[GameBootstrap] BIOME: highway tunnel siphoned gas from '{vehicle}' — '{id}'");
                BiomeHighwayTunnel.OnMutantEncounter += (id, mutant) =>
                    Debug.Log($"[GameBootstrap] BIOME: highway tunnel mutant '{mutant}' — '{id}'");
            }

            if (BiomeSaltFlats != null)
            {
                BiomeSaltFlats.OnBiomeEntered += withVehicle =>
                    Debug.Log($"[GameBootstrap] BIOME: salt flats entered vehicle={withVehicle}");
                BiomeSaltFlats.OnThirstDrained += (amount, mult) =>
                    Debug.Log($"[GameBootstrap] BIOME: salt flats thirst -{amount:F1} (x{mult:F0})");
                BiomeSaltFlats.OnHeatExposure += temp =>
                    Debug.Log($"[GameBootstrap] BIOME: salt flats heat {temp:F0}°C");
                BiomeSaltFlats.OnVehicleBroken += () =>
                    Debug.Log("[GameBootstrap] BIOME: salt flats vehicle broken");
                BiomeSaltFlats.OnSaltHarvested += amount =>
                    Debug.Log($"[GameBootstrap] BIOME: salt flats harvested {amount:F1}");
            }

            if (BiomeSkyscraperTops != null)
            {
                BiomeSkyscraperTops.OnFallHazard += id =>
                    Debug.Log($"[GameBootstrap] BIOME: skyscraper tops fall hazard — '{id}'");
                BiomeSkyscraperTops.OnColdExposure += id =>
                    Debug.Log($"[GameBootstrap] BIOME: skyscraper tops cold — '{id}'");
                BiomeSkyscraperTops.OnWindTurbineScavenged += (id, part) =>
                    Debug.Log($"[GameBootstrap] BIOME: skyscraper turbine part '{part}' — '{id}'");
            }

            if (BiomeSuburbs != null)
            {
                BiomeSuburbs.OnAmbushTriggered += (id, house) =>
                    Debug.Log($"[GameBootstrap] BIOME: suburbs ambush at '{house}' — '{id}'");
                BiomeSuburbs.OnFogOfWarRevealed += tile =>
                    Debug.Log($"[GameBootstrap] BIOME: suburbs fog revealed '{tile}'");
                BiomeSuburbs.OnLootFound += (id, count) =>
                    Debug.Log($"[GameBootstrap] BIOME: suburbs loot x{count} — '{id}'");
            }
        }
    }
}
