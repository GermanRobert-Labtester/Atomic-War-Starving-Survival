using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion III — UXO Field System (The Dead Hand).
    /// Extends the existing UxoHazardSystem with a grid-based "Probing"
    /// mechanic for UXO-dense biomes (Biome_Rust, Biome_UXOField).
    ///
    /// The Probe Mechanic:
    ///   Survivors use item_mine_prod to physically test the soil.
    ///   Hitting a mine triggers a SkillCheck_Dexterity. Failure results
    ///   in Affliction_TraumaticAmputation or instant death.
    ///
    /// Wire-Cutting:
    ///   Encountering a tripwire requires item_wire_cutters and a
    ///   UtilityAI evaluation of the survivor's trait_tremors or
    ///   Affliction_Fatigue. Shaking hands snap the wire.
    ///
    /// Acoustic Signature:
    ///   Running, firing unsuppressed weapons, or using unshielded
    ///   heaters generates an AcousticSignature. High signatures
    ///   attract Loitering Munitions (slow, buzzing suicide drones).
    ///
    /// Save/load safe. Plain C#. No MonoBehaviour.
    /// Complements (does not replace) the existing UxoHazardSystem.
    /// </summary>
    [Serializable]
    public class UXOFieldSystemSave
    {
        public string systemId = "uxo_field";
        public List<UXOFieldNodeState> nodes = new List<UXOFieldNodeState>();
        public float globalAcousticSignature;
        public int totalProbesPerformed;
        public int totalMinesDetonated;
        public int totalTripwiresCut;
        public int totalMinesDisarmed;
    }

    [Serializable]
    public class UXOFieldNodeState
    {
        public string nodeId;
        public float mineDensity; // 0..1
        public bool probed;
        public bool cleared;
        public int minesRemaining;
        public int tripwiresRemaining;
        public float acousticSignature;
    }

    public struct UXOProbeResult
    {
        public bool MineFound;
        public bool Detonated;
        public string SurvivorId;
        public string NodeId;
        public float SkillCheckRoll;
    }

    public struct UXOWireCutResult
    {
        public bool Success;
        public bool WireSnapped;
        public string SurvivorId;
        public string NodeId;
    }

    public struct AcousticSignatureEvent
    {
        public string SurvivorId;
        public float OldSignature;
        public float NewSignature;
        public string Source; // "running", "weapon_fire", "heater", "decoy"
    }

    public struct LoiteringMunitionEvent
    {
        public string SurvivorId;
        public string NodeId;
        public float AcousticSignature;
        public float Threshold;
    }

    public class UXOFieldSystem
    {
        /// <summary>Base mine density for UXO-heavy nodes (0..1).</summary>
        public const float DefaultMineDensity = 0.4f;

        /// <summary>Dexterity skill check threshold to avoid detonation (0..1, roll above to pass).</summary>
        public const float ProbeSkillCheckThreshold = 0.35f;

        /// <summary>Chance of traumatic amputation on failed probe (vs instant death).</summary>
        public const float AmputationVsDeathChance = 0.7f;

        /// <summary>Wire-cut success base chance (before trait modifiers).</summary>
        public const float WireCutBaseChance = 0.75f;

        /// <summary>Wire-cut penalty for trait_tremors or high fatigue.</summary>
        public const float WireCutTremorsPenalty = 0.35f;

        /// <summary>Acoustic signature gained per hour of running.</summary>
        public const float RunningSignaturePerHour = 15f;

        /// <summary>Acoustic signature gained per unsuppressed weapon discharge.</summary>
        public const float WeaponFireSignature = 25f;

        /// <summary>Acoustic signature gained per hour of unshielded heater use.</summary>
        public const float HeaterSignaturePerHour = 5f;

        /// <summary>Acoustic signature threshold that attracts loitering munitions.</summary>
        public const float LoiteringMunitionThreshold = 60f;

        /// <summary>Chance per hour per 10 points above threshold to attract a munition.</summary>
        public const float MunitionAttractionChancePer10 = 0.12f;

        /// <summary>Natural acoustic signature decay per hour (quiet movement).</summary>
        public const float SignatureDecayPerHour = 3f;

        /// <summary>Acoustic decoy signature value (draws all attention).</summary>
        public const float DecoySignatureValue = 100f;

        /// <summary>Health damage from traumatic amputation.</summary>
        public const float AmputationHealthDamage = 50f;

        /// <summary>Health damage from loitering munition strike.</summary>
        public const float MunitionStrikeHealthDamage = 40f;

        /// <summary>trait_uxo_instinct probe time reduction multiplier.</summary>
        public const float UxoInstinctProbeReduction = 0.6f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<UXOProbeResult> OnProbeResult;
        public event Action<UXOWireCutResult> OnWireCutResult;
        public event Action<AcousticSignatureEvent> OnAcousticSignatureChanged;
        public event Action<LoiteringMunitionEvent> OnLoiteringMunitionAttracted;
        public event Action<string> OnNodeCleared;
        public event Action OnFieldStateChanged;

        // ── State ─────────────────────────────────────────────────────
        private readonly Dictionary<string, UXOFieldNodeState> _nodes = new Dictionary<string, UXOFieldNodeState>();
        private float _globalAcousticSignature;
        private int _totalProbesPerformed;
        private int _totalMinesDetonated;
        private int _totalTripwiresCut;
        private int _totalMinesDisarmed;

        // Host callbacks
        public Func<string, bool> SurvivorHasTrait; // survivorId, traitId → bool
        public Func<string, float> GetSurvivorFatigue; // survivorId → fatigue (0..100)
        public Action<string, string> InflictAffliction; // survivorId, afflictionId
        public Action<string, float> ApplyHealthDamage; // survivorId, damage
        public Func<string, bool> SurvivorHasItem; // itemId → bool
        public Action<string, int> ConsumeItem; // itemId, count

        public IReadOnlyDictionary<string, UXOFieldNodeState> Nodes => _nodes;
        public float GlobalAcousticSignature => _globalAcousticSignature;
        public int TotalProbesPerformed => _totalProbesPerformed;
        public int TotalMinesDetonated => _totalMinesDetonated;
        public int TotalMinesDisarmed => _totalMinesDisarmed;

        // ── Node Management ───────────────────────────────────────────

        /// <summary>Register a map node as a UXO field with mine density.</summary>
        public void RegisterUXONode(string nodeId, float mineDensity = -1f)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            if (_nodes.ContainsKey(nodeId)) return;

            float density = mineDensity < 0f ? DefaultMineDensity : Mathf.Clamp01(mineDensity);
            int mines = Mathf.CeilToInt(density * 10f); // abstract: 10 max mines per node
            int tripwires = Mathf.CeilToInt(density * 3f); // 3 max tripwires

            _nodes[nodeId] = new UXOFieldNodeState
            {
                nodeId = nodeId,
                mineDensity = density,
                probed = false,
                cleared = false,
                minesRemaining = mines,
                tripwiresRemaining = tripwires,
                acousticSignature = 0f
            };
        }

        /// <summary>Check if a node is a registered UXO field.</summary>
        public bool IsUXONode(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId) && _nodes.ContainsKey(nodeId);
        }

        /// <summary>Check if a UXO node has been fully cleared.</summary>
        public bool IsNodeCleared(string nodeId)
        {
            return _nodes.TryGetValue(nodeId, out var state) && state.cleared;
        }

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Called every game-hour. Advances acoustic signature decay
        /// and loitering munition attraction rolls.
        /// </summary>
        public void Tick(float gameHours, System.Random rng = null)
        {
            if (gameHours <= 0f) return;

            // Phase 1: Global acoustic signature decay
            float oldSig = _globalAcousticSignature;
            _globalAcousticSignature = Mathf.Max(0f,
                _globalAcousticSignature - SignatureDecayPerHour * gameHours);

            if (Mathf.Abs(_globalAcousticSignature - oldSig) > 0.01f)
            {
                OnAcousticSignatureChanged?.Invoke(new AcousticSignatureEvent
                {
                    OldSignature = oldSig,
                    NewSignature = _globalAcousticSignature,
                    Source = "decay"
                });
            }

            // Phase 2: Loitering munition attraction
            if (rng != null && _globalAcousticSignature >= LoiteringMunitionThreshold)
            {
                float excess = _globalAcousticSignature - LoiteringMunitionThreshold;
                float chance = MunitionAttractionChancePer10 * (excess / 10f) * gameHours;
                if ((float)rng.NextDouble() < chance)
                {
                    OnLoiteringMunitionAttracted?.Invoke(new LoiteringMunitionEvent
                    {
                        AcousticSignature = _globalAcousticSignature,
                        Threshold = LoiteringMunitionThreshold
                    });
                }
            }

            // Phase 3: Per-node signature decay
            foreach (var kv in _nodes)
            {
                var state = kv.Value;
                state.acousticSignature = Mathf.Max(0f,
                    state.acousticSignature - SignatureDecayPerHour * gameHours * 0.5f);
            }

            OnFieldStateChanged?.Invoke();
        }

        // ── Actions ───────────────────────────────────────────────────

        /// <summary>
        /// Probe the soil at a UXO node using item_mine_prod.
        /// Returns the probe result. A failed skill check detonates the mine.
        /// </summary>
        public UXOProbeResult Probe(string nodeId, string survivorId, System.Random rng)
        {
            if (rng == null) rng = new System.Random();
            var result = new UXOProbeResult { SurvivorId = survivorId, NodeId = nodeId };

            if (!_nodes.TryGetValue(nodeId, out var state)) return result;
            if (state.cleared || state.minesRemaining <= 0) return result;

            _totalProbesPerformed++;

            // Skill check: roll above threshold to pass
            float roll = (float)rng.NextDouble();

            // trait_uxo_instinct improves probe safety
            if (SurvivorHasTrait != null && SurvivorHasTrait("trait_uxo_instinct"))
                roll += (1f - ProbeSkillCheckThreshold) * 0.3f;

            result.SkillCheckRoll = roll;

            if (roll >= ProbeSkillCheckThreshold)
            {
                // Success: mine found and disarmed
                result.MineFound = true;
                result.Detonated = false;
                state.minesRemaining--;
                _totalMinesDisarmed++;

                if (state.minesRemaining <= 0 && state.tripwiresRemaining <= 0)
                {
                    state.cleared = true;
                    OnNodeCleared?.Invoke(nodeId);
                }
            }
            else
            {
                // Failure: mine detonates
                result.MineFound = true;
                result.Detonated = true;
                _totalMinesDetonated++;

                ApplyDetonationDamage(survivorId, rng);
            }

            OnProbeResult?.Invoke(result);
            OnFieldStateChanged?.Invoke();
            return result;
        }

        /// <summary>
        /// Cut a tripwire at a UXO node using item_wire_cutters.
        /// Survivors with trait_tremors or high fatigue risk snapping the wire.
        /// </summary>
        public UXOWireCutResult CutWire(string nodeId, string survivorId, System.Random rng)
        {
            if (rng == null) rng = new System.Random();
            var result = new UXOWireCutResult { SurvivorId = survivorId, NodeId = nodeId };

            if (!_nodes.TryGetValue(nodeId, out var state)) return result;
            if (state.cleared || state.tripwiresRemaining <= 0) return result;

            float chance = WireCutBaseChance;

            // trait_tremors penalty
            if (SurvivorHasTrait != null && SurvivorHasTrait("trait_tremors"))
                chance -= WireCutTremorsPenalty;

            // High fatigue penalty
            if (GetSurvivorFatigue != null)
            {
                float fatigue = GetSurvivorFatigue(survivorId);
                if (fatigue > 70f) chance -= 0.15f;
            }

            chance = Mathf.Clamp01(chance);

            if ((float)rng.NextDouble() < chance)
            {
                result.Success = true;
                state.tripwiresRemaining--;
                _totalTripwiresCut++;

                if (state.minesRemaining <= 0 && state.tripwiresRemaining <= 0)
                {
                    state.cleared = true;
                    OnNodeCleared?.Invoke(nodeId);
                }
            }
            else
            {
                result.WireSnapped = true;
                // Snapped wire triggers the mine
                _totalMinesDetonated++;
                ApplyDetonationDamage(survivorId, rng);
            }

            OnWireCutResult?.Invoke(result);
            OnFieldStateChanged?.Invoke();
            return result;
        }

        /// <summary>
        /// Add acoustic signature from an action (running, weapon fire, etc.).
        /// </summary>
        public void AddAcousticSignature(float amount, string survivorId = null, string source = "unknown")
        {
            float old = _globalAcousticSignature;
            _globalAcousticSignature = Mathf.Min(100f, _globalAcousticSignature + amount);

            OnAcousticSignatureChanged?.Invoke(new AcousticSignatureEvent
            {
                SurvivorId = survivorId,
                OldSignature = old,
                NewSignature = _globalAcousticSignature,
                Source = source
            });
        }

        /// <summary>
        /// Deploy an acoustic decoy to draw automated sentry fire.
        /// Sets signature to maximum to attract all attention.
        /// </summary>
        public void DeployAcousticDecoy(string nodeId)
        {
            if (_nodes.TryGetValue(nodeId, out var state))
                state.acousticSignature = DecoySignatureValue;
            AddAcousticSignature(DecoySignatureValue, source: "decoy");
        }

        private void ApplyDetonationDamage(string survivorId, System.Random rng)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            float roll = (float)rng.NextDouble();
            if (roll < AmputationVsDeathChance)
            {
                // Traumatic amputation
                ApplyHealthDamage?.Invoke(survivorId, AmputationHealthDamage);
                InflictAffliction?.Invoke(survivorId, "affliction_traumatic_amputation");
            }
            else
            {
                // Instant death
                ApplyHealthDamage?.Invoke(survivorId, 100f);
            }
        }

        // ── Save / Load ────────────────────────────────────────────────
        public UXOFieldSystemSave CaptureState()
        {
            var save = new UXOFieldSystemSave
            {
                globalAcousticSignature = _globalAcousticSignature,
                totalProbesPerformed = _totalProbesPerformed,
                totalMinesDetonated = _totalMinesDetonated,
                totalTripwiresCut = _totalTripwiresCut,
                totalMinesDisarmed = _totalMinesDisarmed
            };

            foreach (var kv in _nodes)
            {
                var s = kv.Value;
                save.nodes.Add(new UXOFieldNodeState
                {
                    nodeId = s.nodeId,
                    mineDensity = s.mineDensity,
                    probed = s.probed,
                    cleared = s.cleared,
                    minesRemaining = s.minesRemaining,
                    tripwiresRemaining = s.tripwiresRemaining,
                    acousticSignature = s.acousticSignature
                });
            }

            return save;
        }

        public void RestoreState(UXOFieldSystemSave save)
        {
            _nodes.Clear();
            _globalAcousticSignature = 0f;
            _totalProbesPerformed = 0;
            _totalMinesDetonated = 0;
            _totalTripwiresCut = 0;
            _totalMinesDisarmed = 0;

            if (save == null) return;

            _globalAcousticSignature = save.globalAcousticSignature;
            _totalProbesPerformed = save.totalProbesPerformed;
            _totalMinesDetonated = save.totalMinesDetonated;
            _totalTripwiresCut = save.totalTripwiresCut;
            _totalMinesDisarmed = save.totalMinesDisarmed;

            for (int i = 0; i < save.nodes.Count; i++)
            {
                if (save.nodes[i] != null)
                    _nodes[save.nodes[i].nodeId] = save.nodes[i];
            }
        }
    }
}
