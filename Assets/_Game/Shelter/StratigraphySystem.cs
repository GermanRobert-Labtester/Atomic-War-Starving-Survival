using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Expansion III — The Bunker's Bones. Every excavation digs through time.
    /// Three geological/historical layers with distinct hazards, loot, and anomalies.
    /// Integrates with ExcavationSystem (digging triggers stratum events) and
    /// StructuralIntegritySystem (deep digs compromise structure).
    /// Save/load safe. Plain C#.
    /// </summary>
    public class StratigraphySystem
    {
        // ── Layer definitions ─────────────────────────────────────────
        public enum Stratum
        {
            Layer1_1962Paranoia,   // 0–10m: asbestos, lead pipes, civil defense
            Layer2_1984Expansion,  // 10–20m: military overflow, methane, cheaper materials
            Layer3_Bedrock         // 20m+: raw earth, aquifer, the sealed blast door
        }

        // ── Hazard ids ────────────────────────────────────────────────
        public const string Hazard_Asbestos = "hazard_asbestos";
        public const string Hazard_Methane = "hazard_methane";
        public const string Hazard_FlammableGas = "hazard_flammable_gas";
        public const string Hazard_SinkholeCollapse = "hazard_sinkhole_collapse";

        // ── Affliction ids (cross-module references) ──────────────────
        public const string Affliction_Mesothelioma = "affliction_mesothelioma";
        public const string Affliction_TheBends = "affliction_the_bends";

        // ── Discovery item ids ────────────────────────────────────────
        public const string Item_CivilDefenseFilmReel = "civil_defense_film_reel";
        public const string Item_IodinePillsExpired = "iodine_pills_expired_10_of_10";
        public const string Item_PalletMre1984 = "pallet_mre_1984";
        public const string Item_ServerRackLog = "server_rack_log_entry";
        public const string Item_GeigerCounterDead = "geiger_counter_dead";

        // ── Layer thresholds (depth in meters) ────────────────────────
        public const float Layer1_MaxDepth = 10f;
        public const float Layer2_MaxDepth = 20f;

        // ── Hazard chances per excavation event ───────────────────────
        public const float AsbestosExposureChance = 0.35f;
        public const float MethanePocketChance = 0.25f;
        public const float FlammableGasChance = 0.15f;
        public const float SinkholeChance = 0.10f;
        public const float WaterTableBreachChance = 0.20f;

        // ── Structural integrity cost per deep dig ────────────────────
        public const float Layer2_IntegrityDamage = 3f;
        public const float Layer3_IntegrityDamage = 8f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, Stratum> OnHazardTriggered;       // (hazardId, stratum)
        public event Action<string, Stratum> OnDiscoveryMade;         // (itemId, stratum)
        public event Action<string> OnAnomalyRevealed;                // (anomalyId)
        public event Action<Stratum> OnStratumEntered;                // first dig into layer

        private readonly System.Random _rng;
        private readonly HashSet<Stratum> _enteredStrata = new HashSet<Stratum>();
        private readonly HashSet<string> _discoveredAnomalies = new HashSet<string>();
        private readonly HashSet<string> _discoveredItems = new HashSet<string>();
        private float _currentDepth;
        private bool _waterTableBreached;

        public float CurrentDepth => _currentDepth;
        public Stratum CurrentStratum => GetStratumAtDepth(_currentDepth);
        public bool IsWaterTableBreached => _waterTableBreached;
        public IReadOnlyCollection<string> DiscoveredAnomalies => _discoveredAnomalies;

        public StratigraphySystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(2024);
        }

        /// <summary>
        /// Called by ExcavationSystem when rubble is cleared. Advances depth
        /// and rolls for layer-specific hazards and discoveries.
        /// </summary>
        public ExcavationResult OnExcavation(float rubbleCleared, bool hasRespirator,
            bool hasHazmatSuit, bool hasHammerOrCrowbar, float structuralIntegrity)
        {
            var result = new ExcavationResult();
            if (rubbleCleared <= 0f) return result;

            float depthGain = rubbleCleared * 0.5f; // ~0.5m per rubble unit
            float previousDepth = _currentDepth;
            _currentDepth += depthGain;

            var prevStratum = GetStratumAtDepth(previousDepth);
            var newStratum = GetStratumAtDepth(_currentDepth);

            // Entering a new layer for the first time
            if (newStratum != prevStratum && _enteredStrata.Add(newStratum))
            {
                OnStratumEntered?.Invoke(newStratum);
                result.EnteredNewStratum = true;
                result.NewStratum = newStratum;
            }

            // Roll hazards based on current stratum
            RollHazards(result, newStratum, hasRespirator, hasHazmatSuit, hasHammerOrCrowbar);

            // Roll discoveries
            RollDiscoveries(result, newStratum);

            // Structural damage from deep digging
            result.StructuralDamage = ComputeStructuralDamage(newStratum);

            return result;
        }

        /// <summary>Reveal a named anomaly (called by specific event triggers).</summary>
        public bool RevealAnomaly(string anomalyId)
        {
            if (string.IsNullOrEmpty(anomalyId)) return false;
            if (!_discoveredAnomalies.Add(anomalyId)) return false;
            OnAnomalyRevealed?.Invoke(anomalyId);
            return true;
        }

        // ── Layer logic ───────────────────────────────────────────────

        private void RollHazards(ExcavationResult result, Stratum stratum,
            bool hasRespirator, bool hasHazmatSuit, bool hasHammerOrCrowbar)
        {
            switch (stratum)
            {
                case Stratum.Layer1_1962Paranoia:
                    // Asbestos exposure — respirator + hazmat required
                    if (_rng.NextDouble() < AsbestosExposureChance)
                    {
                        if (!hasRespirator || !hasHazmatSuit)
                        {
                            result.Hazards.Add(Hazard_Asbestos);
                            result.AfflictionIds.Add(Affliction_Mesothelioma);
                            OnHazardTriggered?.Invoke(Hazard_Asbestos, stratum);
                        }
                        else
                        {
                            result.HazardsDodged.Add(Hazard_Asbestos);
                        }
                    }
                    break;

                case Stratum.Layer2_1984Expansion:
                    // Methane pocket — spark from tools ignites it
                    if (_rng.NextDouble() < MethanePocketChance)
                    {
                        result.Hazards.Add(Hazard_Methane);
                        OnHazardTriggered?.Invoke(Hazard_Methane, stratum);
                        if (hasHammerOrCrowbar && _rng.NextDouble() < FlammableGasChance)
                        {
                            result.Hazards.Add(Hazard_FlammableGas);
                            result.ExplosionTriggered = true;
                            OnHazardTriggered?.Invoke(Hazard_FlammableGas, stratum);
                        }
                    }
                    break;

                case Stratum.Layer3_Bedrock:
                    // Sinkhole collapse
                    if (_rng.NextDouble() < SinkholeChance)
                    {
                        result.Hazards.Add(Hazard_SinkholeCollapse);
                        OnHazardTriggered?.Invoke(Hazard_SinkholeCollapse, stratum);
                    }
                    // Water table breach
                    if (!_waterTableBreached && _rng.NextDouble() < WaterTableBreachChance)
                    {
                        _waterTableBreached = true;
                        result.WaterTableBreached = true;
                    }
                    break;
            }
        }

        private void RollDiscoveries(ExcavationResult result, Stratum stratum)
        {
            switch (stratum)
            {
                case Stratum.Layer1_1962Paranoia:
                    if (_rng.NextDouble() < 0.20f && _discoveredItems.Add(Item_CivilDefenseFilmReel))
                    {
                        result.Discoveries.Add(Item_CivilDefenseFilmReel);
                        OnDiscoveryMade?.Invoke(Item_CivilDefenseFilmReel, stratum);
                    }
                    // The anomaly: suicide note + expired iodine (one-time)
                    if (_currentDepth >= 8f && _discoveredItems.Add(Item_IodinePillsExpired))
                    {
                        result.Discoveries.Add(Item_IodinePillsExpired);
                        result.AnomalyId = "anomaly_1962_suicide_note";
                        _discoveredAnomalies.Add(result.AnomalyId);
                        OnDiscoveryMade?.Invoke(Item_IodinePillsExpired, stratum);
                        OnAnomalyRevealed?.Invoke(result.AnomalyId);
                    }
                    break;

                case Stratum.Layer2_1984Expansion:
                    if (_rng.NextDouble() < 0.25f && _discoveredItems.Add(Item_PalletMre1984))
                    {
                        result.Discoveries.Add(Item_PalletMre1984);
                        OnDiscoveryMade?.Invoke(Item_PalletMre1984, stratum);
                    }
                    // Server rack anomaly (one-time)
                    if (_currentDepth >= 15f && _discoveredItems.Add(Item_ServerRackLog))
                    {
                        result.Discoveries.Add(Item_ServerRackLog);
                        result.AnomalyId = "anomaly_1984_server_rack";
                        _discoveredAnomalies.Add(result.AnomalyId);
                        OnDiscoveryMade?.Invoke(Item_ServerRackLog, stratum);
                        OnAnomalyRevealed?.Invoke(result.AnomalyId);
                    }
                    break;

                case Stratum.Layer3_Bedrock:
                    // Sealed blast door anomaly (one-time)
                    if (_currentDepth >= 22f && _discoveredItems.Add(Item_GeigerCounterDead))
                    {
                        result.Discoveries.Add(Item_GeigerCounterDead);
                        result.AnomalyId = "anomaly_sealed_blast_door";
                        _discoveredAnomalies.Add(result.AnomalyId);
                        OnDiscoveryMade?.Invoke(Item_GeigerCounterDead, stratum);
                        OnAnomalyRevealed?.Invoke(result.AnomalyId);
                    }
                    break;
            }
        }

        private float ComputeStructuralDamage(Stratum stratum)
        {
            return stratum switch
            {
                Stratum.Layer1_1962Paranoia => 0f,
                Stratum.Layer2_1984Expansion => Layer2_IntegrityDamage,
                Stratum.Layer3_Bedrock => Layer3_IntegrityDamage,
                _ => 0f
            };
        }

        public static Stratum GetStratumAtDepth(float depth)
        {
            if (depth < Layer1_MaxDepth) return Stratum.Layer1_1962Paranoia;
            if (depth < Layer2_MaxDepth) return Stratum.Layer2_1984Expansion;
            return Stratum.Layer3_Bedrock;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public StratigraphySave CaptureState()
        {
            var entered = new Stratum[_enteredStrata.Count];
            _enteredStrata.CopyTo(entered);
            var anomalies = new string[_discoveredAnomalies.Count];
            _discoveredAnomalies.CopyTo(anomalies);
            var items = new string[_discoveredItems.Count];
            _discoveredItems.CopyTo(items);
            return new StratigraphySave
            {
                CurrentDepth = _currentDepth,
                WaterTableBreached = _waterTableBreached,
                EnteredStrata = entered,
                DiscoveredAnomalies = anomalies,
                DiscoveredItems = items
            };
        }

        public void RestoreState(StratigraphySave save)
        {
            _enteredStrata.Clear();
            _discoveredAnomalies.Clear();
            _discoveredItems.Clear();
            _currentDepth = 0f;
            _waterTableBreached = false;
            if (save == null) return;
            _currentDepth = save.CurrentDepth;
            _waterTableBreached = save.WaterTableBreached;
            if (save.EnteredStrata != null)
                for (int i = 0; i < save.EnteredStrata.Length; i++)
                    _enteredStrata.Add(save.EnteredStrata[i]);
            if (save.DiscoveredAnomalies != null)
                for (int i = 0; i < save.DiscoveredAnomalies.Length; i++)
                    if (!string.IsNullOrEmpty(save.DiscoveredAnomalies[i]))
                        _discoveredAnomalies.Add(save.DiscoveredAnomalies[i]);
            if (save.DiscoveredItems != null)
                for (int i = 0; i < save.DiscoveredItems.Length; i++)
                    if (!string.IsNullOrEmpty(save.DiscoveredItems[i]))
                        _discoveredItems.Add(save.DiscoveredItems[i]);
        }
    }

    /// <summary>Result of one excavation tick through the stratigraphy.</summary>
    public class ExcavationResult
    {
        public bool EnteredNewStratum;
        public StratigraphySystem.Stratum NewStratum;
        public List<string> Hazards = new List<string>();
        public List<string> HazardsDodged = new List<string>();
        public List<string> AfflictionIds = new List<string>();
        public List<string> Discoveries = new List<string>();
        public string AnomalyId;
        public float StructuralDamage;
        public bool ExplosionTriggered;
        public bool WaterTableBreached;
    }

    [Serializable]
    public class StratigraphySave
    {
        public float CurrentDepth;
        public bool WaterTableBreached;
        public StratigraphySystem.Stratum[] EnteredStrata;
        public string[] DiscoveredAnomalies;
        public string[] DiscoveredItems;
    }
}
