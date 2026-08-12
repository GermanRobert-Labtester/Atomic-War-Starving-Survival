using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Expansion VII — The Architecture of Confinement. Sound travels through
    /// ventilation shafts and concrete walls. Every action generates a NoiseProfile.
    /// A surgeon operating without anesthesia broadcasts screams to the children's
    /// bedroom. Soundproofing protects minds but blocks hatch alarms.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class AcousticPropagationSystem
    {
        // ── Noise profile constants ───────────────────────────────────
        public const float SilentVolume = 0f;
        public const float WhisperVolume = 10f;
        public const float NormalVolume = 30f;
        public const float LoudVolume = 60f;
        public const float ScreamVolume = 90f;

        // ── Propagation constants ─────────────────────────────────────
        public const float VentBleedFraction = 0.40f;    // 40% of noise bleeds through vents
        public const float WallBleedFraction = 0.15f;    // 15% through concrete walls
        public const float SoundproofBlockFraction = 0.90f; // 90% blocked by soundproofing

        // ── Thresholds ────────────────────────────────────────────────
        public const float TraumaNoiseThreshold = 50f;   // Above this → child trauma
        public const float SleepDisruptionThreshold = 40f;
        public const float NightTerrorThreshold = 60f;

        // ── Module ids ────────────────────────────────────────────────
        public const string Module_Soundproofing = "soundproofing_panel";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, float> OnNoisePropagated;       // roomId, effectiveVolume
        public event Action<string> OnChildTraumatized;             // childId
        public event Action<string> OnSleepDisrupted;               // survivorId
        public event Action<string> OnNightTerrorTriggered;         // childId
        public event Action<string> OnHatchAlarmMuffled;            // roomId

        private readonly Dictionary<string, float> _roomSoundproofing = new Dictionary<string, float>();
        private readonly Dictionary<string, List<string>> _roomAdjacency = new Dictionary<string, List<string>>();
        private readonly System.Random _rng;

        public AcousticPropagationSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(5555);
        }

        // ── Room configuration ────────────────────────────────────────

        /// <summary>Set soundproofing level for a room (0..1, 1 = fully soundproof).</summary>
        public void SetSoundproofing(string roomId, float level)
        {
            _roomSoundproofing[roomId] = Mathf.Clamp01(level);
        }

        /// <summary>Define room adjacency (rooms connected by vents/walls).</summary>
        public void SetAdjacency(string roomId, List<string> adjacentRoomIds)
        {
            _roomAdjacency[roomId] = adjacentRoomIds;
        }

        /// <summary>Check if a room is soundproofed.</summary>
        public bool IsSoundproofed(string roomId)
        {
            return _roomSoundproofing.TryGetValue(roomId, out var level) && level > 0.5f;
        }

        // ── Noise emission ────────────────────────────────────────────

        /// <summary>
        /// Emit noise from a room. Propagates to adjacent rooms through
        /// vents and walls, reduced by soundproofing.
        /// </summary>
        public NoisePropagationResult EmitNoise(string sourceRoomId, float volume,
            string noiseType = null)
        {
            var result = new NoisePropagationResult
            {
                SourceRoomId = sourceRoomId,
                OriginalVolume = volume,
                NoiseType = noiseType
            };

            // Get soundproofing of source room
            float sourceBlock = _roomSoundproofing.TryGetValue(sourceRoomId, out var sb)
                ? sb : 0f;

            if (!_roomAdjacency.TryGetValue(sourceRoomId, out var adjacent))
                return result;

            for (int i = 0; i < adjacent.Count; i++)
            {
                string targetRoomId = adjacent[i];
                float targetBlock = _roomSoundproofing.TryGetValue(targetRoomId, out var tb)
                    ? tb : 0f;

                // Vent bleed (higher) vs wall bleed (lower)
                float bleedFraction = VentBleedFraction;
                float effectiveBlock = Mathf.Max(sourceBlock, targetBlock);

                // Soundproofing reduces bleed
                float effectiveVolume = volume * bleedFraction * (1f - effectiveBlock * SoundproofBlockFraction);

                if (effectiveVolume > SilentVolume)
                {
                    result.PropagatedRooms.Add(new NoisePropagation
                    {
                        RoomId = targetRoomId,
                        EffectiveVolume = effectiveVolume
                    });
                    OnNoisePropagated?.Invoke(targetRoomId, effectiveVolume);
                }
            }

            return result;
        }

        /// <summary>
        /// Process the consequences of noise in a room. Children may be
        /// traumatized, sleepers may be disrupted.
        /// </summary>
        public void ProcessNoiseConsequences(string roomId, float effectiveVolume,
            Dictionary<string, string> survivorRoomAssignments,
            IReadOnlyList<AtomicWar._Game.Survivors.Survivor> survivors,
            AtomicWar._Game.Survivors.ChildDevelopmentSystem childSystem,
            AtomicWar._Game.Survivors.NeedsSystem needsSystem)
        {
            if (survivors == null || survivorRoomAssignments == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!survivorRoomAssignments.TryGetValue(sv.Id, out var assignedRoom)) continue;
                if (assignedRoom != roomId) continue;

                // Child trauma from loud noise
                if (sv.IsChild && effectiveVolume >= TraumaNoiseThreshold)
                {
                    childSystem?.ModifyTrauma(sv, effectiveVolume * 0.1f);
                    OnChildTraumatized?.Invoke(sv.Id);

                    if (effectiveVolume >= NightTerrorThreshold)
                    {
                        OnNightTerrorTriggered?.Invoke(sv.Id);
                    }
                }

                // Sleep disruption
                if (sv.State == AtomicWar._Game.Survivors.SurvivorState.Resting
                    && effectiveVolume >= SleepDisruptionThreshold)
                {
                    needsSystem?.Modify(sv, AtomicWar._Game.Survivors.NeedKind.Fatigue, 5f);
                    OnSleepDisrupted?.Invoke(sv.Id);
                }
            }
        }

        /// <summary>
        /// Check if hatch alarms are muffled in a soundproofed room.
        /// Returns true if the alarm is blocked.
        /// </summary>
        public bool IsHatchAlarmMuffled(string roomId)
        {
            if (!IsSoundproofed(roomId)) return false;
            OnHatchAlarmMuffled?.Invoke(roomId);
            return true;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public AcousticSave CaptureState()
        {
            var entries = new RoomSoundproofingSave[_roomSoundproofing.Count];
            int i = 0;
            foreach (var kv in _roomSoundproofing)
                entries[i++] = new RoomSoundproofingSave { RoomId = kv.Key, Level = kv.Value };
            return new AcousticSave { RoomSoundproofing = entries };
        }

        public void RestoreState(AcousticSave save)
        {
            _roomSoundproofing.Clear();
            if (save?.RoomSoundproofing == null) return;
            for (int i = 0; i < save.RoomSoundproofing.Length; i++)
                if (save.RoomSoundproofing[i] != null)
                    _roomSoundproofing[save.RoomSoundproofing[i].RoomId] = save.RoomSoundproofing[i].Level;
        }
    }

    [Serializable]
    public class NoisePropagationResult
    {
        public string SourceRoomId;
        public float OriginalVolume;
        public string NoiseType;
        public List<NoisePropagation> PropagatedRooms = new List<NoisePropagation>();
    }

    [Serializable]
    public class NoisePropagation
    {
        public string RoomId;
        public float EffectiveVolume;
    }

    [Serializable]
    public class AcousticSave
    {
        public RoomSoundproofingSave[] RoomSoundproofing;
    }

    [Serializable]
    public class RoomSoundproofingSave
    {
        public string RoomId;
        public float Level;
    }
}
