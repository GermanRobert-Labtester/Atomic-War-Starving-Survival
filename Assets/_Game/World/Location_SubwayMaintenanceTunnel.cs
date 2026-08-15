using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion IV — The Subterranean Dark. The Biome_Subterranean_Dark introduces
    /// flooded utility tunnels, subway maintenance catacombs, and the people who
    /// fled down there on Day 0 and never came back up.
    ///
    /// NPC_TheLightless: 200 civilians who fled into the Tessarat subway maintenance
    /// tunnels. Survived on broken water mains and rats. Severe Vitamin D deficiency,
    /// rickets, cataracts. Communicate in clicks and whispers. Terrified of light.
    /// </summary>
    public class Location_SubwayMaintenanceTunnel
    {
        public const string LocationId = "location_subway_maintenance_tunnel";
        public const string DisplayName = "The Subway Maintenance Tunnels";
        public const int TravelHours = 3;
        public const int DangerLevel = 8;
        public const float BaseRads = 5f; // Low rad, high biological hazard

        // ── Required gear ─────────────────────────────────────────────
        public const string RequiredGear_Crowbar = "crowbar";

        // ── Hazard constants ──────────────────────────────────────────
        public const string Hazard_Drowning = "hazard_drowning";
        public const string Hazard_ZoonoticFlu = "hazard_zoonotic_flu";
        public const string Hazard_TrenchFoot = "hazard_trench_foot";
        public const float DrowningDepthWaist = 0.8f; // meters
        public const float ZoonoticFluChance = 0.30f;
        public const float TrenchFootChance = 0.25f;

        // ── Lightless behavior ────────────────────────────────────────
        public const string FactionId_Lightless = "npc_the_lightless";
        public const float LightlessTradeTrust = 0.5f; // Moderate trust
        public const string LightlessTrade_WaterFilter = "water_filter_cartridge";
        public const string LightlessTrade_CandleTallow = "candle_tallow";
        public const string LightlessTrade_Herbs = "herbs";
        public const float FlareWhitePanicChance = 1.0f; // 100% panic on white flare

        // ── Unique loot ───────────────────────────────────────────────
        public const string Loot_GeneratorAlternator = "generator_alternator";
        public const string Loot_CopperWire10m = "copper_wire_10m_of_10m";
        public const string Loot_MilitarySandstone = "military_grade_sandstone";
        public const string Item_FlareWhite = "flare_white";

        // ── The Iron Worm shrine ──────────────────────────────────────
        public const string ShrineItemId = "scrap_metal";
        public const int ShrineScrapCount = 15; // Scrap stacked on tracks
        public const float ShrineDesecrationMoraleHit = 20f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnLightlessEncounter;
        public event Action<string> OnLightlessTrade;
        public event Action<string> OnLightlessPanic;
        public event Action<string> OnShrineDesecrated;
        public event Action<string> OnLightlessHunt;
        public event Action<string> OnDrowningExposure;
        public event Action<string> OnZoonoticInfection;
        public event Action<string> OnTrenchFootInfection;

        private readonly System.Random _rng;
        private readonly HashSet<string> _searchedAreas = new HashSet<string>();
        private bool _alternatorRecovered;
        private int _copperWireRecovered;
        private bool _sandstoneRecovered;
        private bool _shrineDesecrated;
        private bool _lightlessAlerted;
        private float _lightlessTrust;
        private int _tradesCompleted;

        public bool IsAlternatorRecovered => _alternatorRecovered;
        public int CopperWireRecovered => _copperWireRecovered;
        public bool IsShrineDesecrated => _shrineDesecrated;
        public bool IsLightlessAlerted => _lightlessAlerted;
        public float LightlessTrust => _lightlessTrust;
        public int TradesCompleted => _tradesCompleted;

        public Location_SubwayMaintenanceTunnel(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(3333);
            _lightlessTrust = LightlessTradeTrust;
        }

        // ── Tunnel Entry ──────────────────────────────────────────────

        /// <summary>
        /// Enter the tunnels. Requires crowbar to breach the grating.
        /// Waist-deep black water with biological hazards.
        /// </summary>
        public TunnelEntryResult EnterTunnels(string survivorId, bool hasCrowbar)
        {
            if (!hasCrowbar) return new TunnelEntryResult { Success = false, MissingItem = RequiredGear_Crowbar };

            var result = new TunnelEntryResult { Success = true };

            // Drowning risk in waist-deep water
            if (_rng.NextDouble() < 0.15f)
            {
                result.DrowningRisk = true;
                OnDrowningExposure?.Invoke(survivorId);
            }

            // Zoonotic flu from water
            if (_rng.NextDouble() < ZoonoticFluChance)
            {
                result.ZoonoticExposure = true;
                OnZoonoticInfection?.Invoke(survivorId);
            }

            // Trench foot
            if (_rng.NextDouble() < TrenchFootChance)
            {
                result.TrenchFootExposure = true;
                OnTrenchFootInfection?.Invoke(survivorId);
            }

            return result;
        }

        // ── Lightless Encounter ───────────────────────────────────────

        /// <summary>
        /// Encounter the Lightless. They are terrified of light.
        /// If you use a flashlight or flare, they swarm in panic.
        /// Otherwise, cautious trade is possible.
        /// </summary>
        public LightlessEncounterResult EncounterLightless(
            string survivorId,
            bool hasLightSource,
            bool hasFlareWhite)
        {
            OnLightlessEncounter?.Invoke(survivorId);

            // White flare: guaranteed panic swarm
            if (hasFlareWhite)
            {
                _lightlessAlerted = true;
                _lightlessTrust = 0f;
                OnLightlessPanic?.Invoke(survivorId);
                return new LightlessEncounterResult
                {
                    Panicked = true,
                    Message = "The flare ignites. The tunnels erupt in screaming. " +
                        "Two hundred blind, starving humans swarm in sensory overload. " +
                        "They are not attacking. They are drowning in light."
                };
            }

            // Flashlight: high panic chance
            if (hasLightSource && _rng.NextDouble() < 0.70f)
            {
                _lightlessAlerted = true;
                _lightlessTrust *= 0.3f;
                OnLightlessPanic?.Invoke(survivorId);
                return new LightlessEncounterResult
                {
                    Panicked = true,
                    Message = "The beam cuts the dark. The Lightless scatter, " +
                        "crashing into walls, into each other. The sound is " +
                        "not screaming. It is worse than screaming."
                };
            }

            // No light: cautious trade possible
            return new LightlessEncounterResult
            {
                Panicked = false,
                CanTrade = _lightlessTrust > 0.2f,
                Message = "Shapes in the dark. Clicks. Whispers. A hand reaches out, " +
                    "offering a water filter cartridge. It wants candle tallow in return. " +
                    "It wants the dark to smell like something other than copper."
            };
        }

        // ── Lightless Trade ───────────────────────────────────────────

        /// <summary>
        /// Trade with the Lightless. They want candle_tallow and herbs.
        /// They offer pristine water_filter cartridges from deep municipal reserves.
        /// </summary>
        public LightlessTradeResult TradeWithLightless(
            string survivorId,
            string offeredItemId,
            int offeredAmount)
        {
            if (_lightlessTrust < 0.2f || _lightlessAlerted)
                return new LightlessTradeResult { Success = false, TrustTooLow = true };

            string receivedItemId = null;
            int receivedAmount = 0;

            if (offeredItemId == LightlessTrade_CandleTallow || offeredItemId == LightlessTrade_Herbs)
            {
                receivedItemId = LightlessTrade_WaterFilter;
                receivedAmount = offeredAmount; // 1:1 trade
                _lightlessTrust = Mathf.Min(1f, _lightlessTrust + 0.1f);
                _tradesCompleted++;
                OnLightlessTrade?.Invoke(survivorId);
            }

            return new LightlessTradeResult
            {
                Success = receivedItemId != null,
                ReceivedItemId = receivedItemId,
                ReceivedAmount = receivedAmount
            };
        }

        // ── Pump Room Scavenge ────────────────────────────────────────

        /// <summary>
        /// Search the municipal backup generator room. Contains alternator,
        /// copper wire, and military-grade sandstone.
        /// </summary>
        public List<string> SearchPumpRoom(string survivorId)
        {
            var loot = new List<string>();

            if (!_alternatorRecovered && _rng.NextDouble() < 0.50f)
            {
                _alternatorRecovered = true;
                loot.Add(Loot_GeneratorAlternator);
            }

            if (_copperWireRecovered < 10 && _rng.NextDouble() < 0.70f)
            {
                int yield = _rng.Next(2, 5);
                _copperWireRecovered += yield;
                for (int i = 0; i < yield; i++)
                    loot.Add(Loot_CopperWire10m);
            }

            if (!_sandstoneRecovered && _rng.NextDouble() < 0.30f)
            {
                _sandstoneRecovered = true;
                loot.Add(Loot_MilitarySandstone);
            }

            return loot;
        }

        // ── The Iron Worm Shrine ──────────────────────────────────────

        /// <summary>
        /// Take the scrap metal from the shrine. The Lightless will hunt you.
        /// You must use Action_CoverTracks to escape.
        /// </summary>
        public ShrineActionResult DesecrateShrine(string survivorId)
        {
            if (_shrineDesecrated) return new ShrineActionResult { AlreadyDesecrated = true };

            _shrineDesecrated = true;
            _lightlessAlerted = true;
            _lightlessTrust = 0f;

            int scrapYield = ShrineScrapCount;
            OnShrineDesecrated?.Invoke(survivorId);
            OnLightlessHunt?.Invoke(survivorId);

            return new ShrineActionResult
            {
                Success = true,
                ScrapYield = scrapYield,
                LightlessHunting = true,
                MoraleHit = ShrineDesecrationMoraleHit,
                Message = "The metal clatters as you take it. The clicks stop. " +
                    "Then, from deep in the tunnel, a new sound. Not clicks. " +
                    "Footsteps. Hundreds of them. Coming for you."
            };
        }

        // ── Save / Load ───────────────────────────────────────────────

        public SubwayTunnelSave CaptureState()
        {
            var areas = new string[_searchedAreas.Count];
            _searchedAreas.CopyTo(areas);
            return new SubwayTunnelSave
            {
                AlternatorRecovered = _alternatorRecovered,
                CopperWireRecovered = _copperWireRecovered,
                SandstoneRecovered = _sandstoneRecovered,
                ShrineDesecrated = _shrineDesecrated,
                LightlessAlerted = _lightlessAlerted,
                LightlessTrust = _lightlessTrust,
                TradesCompleted = _tradesCompleted,
                SearchedAreas = areas
            };
        }

        public void RestoreState(SubwayTunnelSave save)
        {
            _searchedAreas.Clear();
            _alternatorRecovered = false;
            _copperWireRecovered = 0;
            _sandstoneRecovered = false;
            _shrineDesecrated = false;
            _lightlessAlerted = false;
            _lightlessTrust = LightlessTradeTrust;
            _tradesCompleted = 0;
            if (save == null) return;
            _alternatorRecovered = save.AlternatorRecovered;
            _copperWireRecovered = save.CopperWireRecovered;
            _sandstoneRecovered = save.SandstoneRecovered;
            _shrineDesecrated = save.ShrineDesecrated;
            _lightlessAlerted = save.LightlessAlerted;
            _lightlessTrust = save.LightlessTrust;
            _tradesCompleted = save.TradesCompleted;
            if (save.SearchedAreas != null)
                for (int i = 0; i < save.SearchedAreas.Length; i++)
                    if (!string.IsNullOrEmpty(save.SearchedAreas[i]))
                        _searchedAreas.Add(save.SearchedAreas[i]);
        }
    }

    [Serializable]
    public class TunnelEntryResult
    {
        public bool Success;
        public string MissingItem;
        public bool DrowningRisk;
        public bool ZoonoticExposure;
        public bool TrenchFootExposure;
    }

    [Serializable]
    public class LightlessEncounterResult
    {
        public bool Panicked;
        public bool CanTrade;
        public string Message;
    }

    [Serializable]
    public class LightlessTradeResult
    {
        public bool Success;
        public bool TrustTooLow;
        public string ReceivedItemId;
        public int ReceivedAmount;
    }

    [Serializable]
    public class ShrineActionResult
    {
        public bool Success;
        public bool AlreadyDesecrated;
        public int ScrapYield;
        public bool LightlessHunting;
        public float MoraleHit;
        public string Message;
    }

    [Serializable]
    public class SubwayTunnelSave
    {
        public bool AlternatorRecovered;
        public int CopperWireRecovered;
        public bool SandstoneRecovered;
        public bool ShrineDesecrated;
        public bool LightlessAlerted;
        public float LightlessTrust;
        public int TradesCompleted;
        public string[] SearchedAreas;
    }
}
