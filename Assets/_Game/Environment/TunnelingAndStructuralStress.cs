using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Expansion II — Tunneling &amp; Structural Stress. Expanding the bunker
    /// downward requires shoring materials and pneumatic jacks. Every new room
    /// below Level 3 adds Overburden Stress to the ceiling. If stress exceeds
    /// the MaterialShieldingSO threshold, a cave-in buries scavengers and
    /// destroys modules.
    ///
    /// Subterranean Gas Hazards:
    ///   Digging risks hitting pressurised pockets of Methane (Hazard_Methane)
    ///   or Hydrogen Sulfide. Without proper ventilation, gas accumulates in
    ///   the lowest room and can trigger an explosion from any ignition source.
    ///
    /// Save/load safe. Plain C#. No MonoBehaviour.
    /// </summary>
    [Serializable]
    public class TunnelingAndStructuralStressSave
    {
        public string systemId = "tunneling_stress";
        public int deepestExcavatedLevel = 2;
        public float overburdenStress;
        public float methaneConcentration;
        public float h2sConcentration;
        public int shoringTimberInstalled;
        public int pneumaticJacksInstalled;
        public List<TunnelingRoomRecord> excavatedRooms = new List<TunnelingRoomRecord>();
        public bool caveInActive;
        public string caveInRoomId;
        public float caveInHoursRemaining;
    }

    [Serializable]
    public class TunnelingRoomRecord
    {
        public string roomId;
        public int level;
        public float stressContribution;
        public bool hasShoring;
        public bool hasPneumaticJack;
        public float excavationHoursRemaining;
    }

    public struct TunnelingCaveInEvent
    {
        public string RoomId;
        public int Level;
        public float OverburdenStress;
        public float MaterialThreshold;
    }

    public struct TunnelingGasPocketEvent
    {
        public string RoomId;
        public string GasType; // "methane" or "h2s"
        public float Concentration;
    }

    public struct TunnelingExcavationEvent
    {
        public string RoomId;
        public int Level;
        public float StressAdded;
        public int ShoringRequired;
    }

    public class TunnelingAndStructuralStress
    {
        /// <summary>Deepest level that does not generate overburden stress.</summary>
        public const int SafeDepthLevel = 3;

        /// <summary>Stress added per room below the safe depth level.</summary>
        public const float StressPerDeepRoom = 15f;

        /// <summary>Stress reduction per shoring timber installed.</summary>
        public const float StressReliefPerShoring = 10f;

        /// <summary>Stress reduction per pneumatic jack installed.</summary>
        public const float StressReliefPerJack = 12f;

        /// <summary>Material shielding threshold — cave-in triggers above this.</summary>
        public const float DefaultMaterialThreshold = 100f;

        /// <summary>Hours to excavate a new room with proper tools.</summary>
        public const float BaseExcavationHours = 16f;

        /// <summary>Excavation time multiplier without pneumatic jack.</summary>
        public const float NoJackTimeMultiplier = 3f;

        /// <summary>Chance per excavation hour to hit a gas pocket.</summary>
        public const float GasPocketChancePerHour = 0.04f;

        /// <summary>Methane generated per gas pocket hit (ppm).</summary>
        public const float MethanePerPocket = 2000f;

        /// <summary>H2S generated per gas pocket hit (ppm).</summary>
        public const float H2SPerPocket = 800f;

        /// <summary>Methane explosion threshold (ppm).</summary>
        public const float MethaneExplosionThreshold = 5000f;

        /// <summary>H2S lethal threshold (ppm).</summary>
        public const float H2SLethalThreshold = 1000f;

        /// <summary>Natural gas dissipation per hour with ventilation.</summary>
        public const float GasDissipationPerHour = 100f;

        /// <summary>Hours a cave-in takes to excavate and rebuild.</summary>
        public const float CaveInRebuildHours = 240f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<TunnelingCaveInEvent> OnCaveIn;
        public event Action<TunnelingGasPocketEvent> OnGasPocketHit;
        public event Action<TunnelingExcavationEvent> OnExcavationStarted;
        public event Action<string> OnExcavationComplete;
        public event Action<float> OnStressChanged;
        public event Action OnMethaneExplosion;

        // ── State ─────────────────────────────────────────────────────
        private int _deepestExcavatedLevel = 2;
        private float _overburdenStress;
        private float _methaneConcentration;
        private float _h2sConcentration;
        private int _shoringTimberInstalled;
        private int _pneumaticJacksInstalled;
        private readonly List<TunnelingRoomRecord> _excavatedRooms = new List<TunnelingRoomRecord>();
        private bool _caveInActive;
        private string _caveInRoomId;
        private float _caveInHoursRemaining;
        private float _materialThreshold = DefaultMaterialThreshold;

        public int DeepestExcavatedLevel => _deepestExcavatedLevel;
        public float OverburdenStress => _overburdenStress;
        public float MethaneConcentration => _methaneConcentration;
        public float H2SConcentration => _h2sConcentration;
        public int ShoringTimberInstalled => _shoringTimberInstalled;
        public int PneumaticJacksInstalled => _pneumaticJacksInstalled;
        public bool IsCaveInActive => _caveInActive;
        public string CaveInRoomId => _caveInRoomId;
        public float MaterialThreshold => _materialThreshold;
        public IReadOnlyList<TunnelingRoomRecord> ExcavatedRooms => _excavatedRooms;

        /// <summary>
        /// Stress as a fraction of the material threshold (0..1+).
        /// UI uses this for the structural stress overlay red-zone warning.
        /// </summary>
        public float StressFraction => _materialThreshold > 0f
            ? _overburdenStress / _materialThreshold
            : 0f;

        public void SetMaterialThreshold(float threshold)
        {
            _materialThreshold = Mathf.Max(1f, threshold);
        }

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Called every game-hour. Advances excavation timers, gas
        /// dissipation, cave-in countdown, and stress checks.
        /// </summary>
        public void Tick(float gameHours, System.Random rng = null)
        {
            if (gameHours <= 0f) return;

            // Phase 1: Advance active excavations
            for (int i = 0; i < _excavatedRooms.Count; i++)
            {
                var room = _excavatedRooms[i];
                if (room.excavationHoursRemaining <= 0f) continue;

                room.excavationHoursRemaining -= gameHours;

                // Gas pocket rolls during excavation
                if (rng != null && rng.NextDouble() < GasPocketChancePerHour * gameHours)
                {
                    HitGasPocket(room, rng);
                }

                if (room.excavationHoursRemaining <= 0f)
                {
                    room.excavationHoursRemaining = 0f;
                    _excavatedRooms[i] = room;
                    FinalizeExcavation(room);
                }
                else
                {
                    _excavatedRooms[i] = room;
                }
            }

            // Phase 2: Gas dissipation (with ventilation)
            _methaneConcentration = Mathf.Max(0f, _methaneConcentration - GasDissipationPerHour * gameHours);
            _h2sConcentration = Mathf.Max(0f, _h2sConcentration - GasDissipationPerHour * gameHours * 0.5f);

            // Phase 3: Methane explosion check
            if (_methaneConcentration >= MethaneExplosionThreshold)
            {
                OnMethaneExplosion?.Invoke();
                _methaneConcentration = 0f; // explosion vents the gas
            }

            // Phase 4: Cave-in countdown
            if (_caveInActive)
            {
                _caveInHoursRemaining -= gameHours;
                if (_caveInHoursRemaining <= 0f)
                {
                    _caveInActive = false;
                    _caveInRoomId = null;
                }
            }

            // Phase 5: Stress recalculation
            RecalculateStress();
            OnStressChanged?.Invoke(_overburdenStress);

            // Phase 6: Cave-in check
            if (_overburdenStress > _materialThreshold && !_caveInActive)
            {
                TriggerCaveIn();
            }
        }

        private void HitGasPocket(TunnelingRoomRecord room, System.Random rng)
        {
            bool isMethane = rng.NextDouble() < 0.6;
            if (isMethane)
            {
                _methaneConcentration += MethanePerPocket;
                OnGasPocketHit?.Invoke(new TunnelingGasPocketEvent
                {
                    RoomId = room.roomId,
                    GasType = "methane",
                    Concentration = _methaneConcentration
                });
            }
            else
            {
                _h2sConcentration += H2SPerPocket;
                OnGasPocketHit?.Invoke(new TunnelingGasPocketEvent
                {
                    RoomId = room.roomId,
                    GasType = "h2s",
                    Concentration = _h2sConcentration
                });
            }
        }

        private void FinalizeExcavation(TunnelingRoomRecord room)
        {
            if (room.level > _deepestExcavatedLevel)
                _deepestExcavatedLevel = room.level;

            RecalculateStress();
            OnExcavationComplete?.Invoke(room.roomId);
        }

        private void RecalculateStress()
        {
            float stress = 0f;
            for (int i = 0; i < _excavatedRooms.Count; i++)
            {
                var room = _excavatedRooms[i];
                if (room.level <= SafeDepthLevel) continue;

                // Stress accrues proportionally while excavation is underway so the HUD
                // fraction reflects active deep digging, not just completed rooms.
                float totalHours = BaseExcavationHours * (room.hasPneumaticJack ? 1f : NoJackTimeMultiplier);
                float completion = room.excavationHoursRemaining <= 0f
                    ? 1f
                    : Mathf.Clamp01(1f - (room.excavationHoursRemaining / totalHours));
                if (completion <= 0f) continue;

                float roomStress = StressPerDeepRoom * (room.level - SafeDepthLevel) * completion;
                if (room.hasShoring) roomStress -= StressReliefPerShoring;
                if (room.hasPneumaticJack) roomStress -= StressReliefPerJack;
                room.stressContribution = Mathf.Max(0f, roomStress);
                _excavatedRooms[i] = room;
                stress += room.stressContribution;
            }
            _overburdenStress = stress;
        }

        private void TriggerCaveIn()
        {
            // Find the weakest deep room
            string weakestRoom = null;
            float maxStress = 0f;
            for (int i = 0; i < _excavatedRooms.Count; i++)
            {
                var room = _excavatedRooms[i];
                if (room.level <= SafeDepthLevel) continue;
                if (room.excavationHoursRemaining > 0f) continue;
                if (room.stressContribution > maxStress)
                {
                    maxStress = room.stressContribution;
                    weakestRoom = room.roomId;
                }
            }

            _caveInActive = true;
            _caveInRoomId = weakestRoom ?? "sub_level_4";
            _caveInHoursRemaining = CaveInRebuildHours;

            OnCaveIn?.Invoke(new TunnelingCaveInEvent
            {
                RoomId = _caveInRoomId,
                Level = _deepestExcavatedLevel,
                OverburdenStress = _overburdenStress,
                MaterialThreshold = _materialThreshold
            });
        }

        // ── Actions ───────────────────────────────────────────────────

        /// <summary>
        /// Begin excavating a new room at the specified depth level.
        /// Requires shoring timber and optionally a pneumatic jack.
        /// Returns the excavation record for tracking.
        /// </summary>
        public TunnelingRoomRecord BeginExcavation(string roomId, int level, bool hasPneumaticJack)
        {
            // Check for duplicate
            for (int i = 0; i < _excavatedRooms.Count; i++)
                if (_excavatedRooms[i].roomId == roomId) return _excavatedRooms[i];

            float hours = BaseExcavationHours;
            if (!hasPneumaticJack) hours *= NoJackTimeMultiplier;

            var record = new TunnelingRoomRecord
            {
                roomId = roomId,
                level = level,
                excavationHoursRemaining = hours,
                hasPneumaticJack = hasPneumaticJack
            };

            _excavatedRooms.Add(record);

            OnExcavationStarted?.Invoke(new TunnelingExcavationEvent
            {
                RoomId = roomId,
                Level = level,
                StressAdded = StressPerDeepRoom * Mathf.Max(0, level - SafeDepthLevel),
                ShoringRequired = level > SafeDepthLevel ? 1 : 0
            });

            return record;
        }

        /// <summary>
        /// Install shoring timber in a completed room. Reduces stress.
        /// </summary>
        public bool InstallShoring(string roomId)
        {
            for (int i = 0; i < _excavatedRooms.Count; i++)
            {
                var room = _excavatedRooms[i];
                if (room.roomId == roomId && !room.hasShoring && room.excavationHoursRemaining <= 0f)
                {
                    room.hasShoring = true;
                    _excavatedRooms[i] = room;
                    _shoringTimberInstalled++;
                    RecalculateStress();
                    OnStressChanged?.Invoke(_overburdenStress);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Install a pneumatic jack in a completed room. Major stress reduction.
        /// </summary>
        public bool InstallPneumaticJack(string roomId)
        {
            for (int i = 0; i < _excavatedRooms.Count; i++)
            {
                var room = _excavatedRooms[i];
                if (room.roomId == roomId && !room.hasPneumaticJack && room.excavationHoursRemaining <= 0f)
                {
                    room.hasPneumaticJack = true;
                    _excavatedRooms[i] = room;
                    _pneumaticJacksInstalled++;
                    RecalculateStress();
                    OnStressChanged?.Invoke(_overburdenStress);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if H2S concentration is at lethal levels.
        /// </summary>
        public bool IsH2SLethal => _h2sConcentration >= H2SLethalThreshold;

        /// <summary>
        /// Check if methane is at explosion risk.
        /// </summary>
        public bool IsMethaneExplosionRisk => _methaneConcentration >= MethaneExplosionThreshold * 0.8f;

        // ── Save / Load ────────────────────────────────────────────────
        public TunnelingAndStructuralStressSave CaptureState()
        {
            var save = new TunnelingAndStructuralStressSave
            {
                deepestExcavatedLevel = _deepestExcavatedLevel,
                overburdenStress = _overburdenStress,
                methaneConcentration = _methaneConcentration,
                h2sConcentration = _h2sConcentration,
                shoringTimberInstalled = _shoringTimberInstalled,
                pneumaticJacksInstalled = _pneumaticJacksInstalled,
                caveInActive = _caveInActive,
                caveInRoomId = _caveInRoomId,
                caveInHoursRemaining = _caveInHoursRemaining
            };

            for (int i = 0; i < _excavatedRooms.Count; i++)
            {
                var r = _excavatedRooms[i];
                save.excavatedRooms.Add(new TunnelingRoomRecord
                {
                    roomId = r.roomId,
                    level = r.level,
                    stressContribution = r.stressContribution,
                    hasShoring = r.hasShoring,
                    hasPneumaticJack = r.hasPneumaticJack,
                    excavationHoursRemaining = r.excavationHoursRemaining
                });
            }

            return save;
        }

        public void RestoreState(TunnelingAndStructuralStressSave save)
        {
            _excavatedRooms.Clear();
            _caveInActive = false;
            _caveInRoomId = null;
            _caveInHoursRemaining = 0f;
            _methaneConcentration = 0f;
            _h2sConcentration = 0f;
            _overburdenStress = 0f;
            _shoringTimberInstalled = 0;
            _pneumaticJacksInstalled = 0;
            _deepestExcavatedLevel = 2;

            if (save == null) return;

            _deepestExcavatedLevel = save.deepestExcavatedLevel;
            _methaneConcentration = save.methaneConcentration;
            _h2sConcentration = save.h2sConcentration;
            _shoringTimberInstalled = save.shoringTimberInstalled;
            _pneumaticJacksInstalled = save.pneumaticJacksInstalled;
            _caveInActive = save.caveInActive;
            _caveInRoomId = save.caveInRoomId;
            _caveInHoursRemaining = save.caveInHoursRemaining;

            for (int i = 0; i < save.excavatedRooms.Count; i++)
            {
                if (save.excavatedRooms[i] != null)
                    _excavatedRooms.Add(save.excavatedRooms[i]);
            }

            RecalculateStress();
        }
    }
}
