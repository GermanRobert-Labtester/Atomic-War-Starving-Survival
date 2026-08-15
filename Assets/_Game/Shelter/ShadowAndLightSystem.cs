using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Expansion VI — Shadow & Light. Power is scarce. You cannot light the whole bunker.
    /// Survivors in unlit rooms suffer Claustrophobia and accelerated Morale decay.
    /// Performing surgery in the dark increases botch chance by 60%.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ShadowAndLightSystem
    {
        public const float UnlitMoraleDecayPerHour = 2f;
        public const float UnlitFatigueMultiplier = 1.3f;
        public const float DarkSurgeryBotchChanceIncrease = 0.60f;
        public const float DarkSurgeryBaseBotchChance = 0.05f;
        public const string Affliction_Claustrophobia = "affliction_claustrophobia";

        public event Action<string> OnRoomLit;
        public event Action<string> OnRoomUnlit;
        public event Action<string> OnClaustrophobiaTriggered;
        public event Action<string> OnSurgeryBotch;

        private readonly Dictionary<string, bool> _roomLitState = new Dictionary<string, bool>();
        private readonly HashSet<string> _claustrophobiaVictims = new HashSet<string>();
        private readonly System.Random _rng;

        public ShadowAndLightSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(2222);
        }

        /// <summary>Set a room's lighting state.</summary>
        public void SetRoomLit(string roomId, bool lit)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            _roomLitState[roomId] = lit;
            if (lit) OnRoomLit?.Invoke(roomId);
            else OnRoomUnlit?.Invoke(roomId);
        }

        /// <summary>Check if a room is lit.</summary>
        public bool IsRoomLit(string roomId)
        {
            return _roomLitState.TryGetValue(roomId, out var lit) && lit;
        }

        /// <summary>
        /// Per-tick update. Survivors in unlit rooms suffer morale decay
        /// and may develop Claustrophobia.
        /// </summary>
        public void Tick(float gameHours, Dictionary<string, string> survivorRoomAssignments,
            Action<string, float> modifyMorale, Action<string, float> modifyFatigue,
            System.Random rng = null)
        {
            if (survivorRoomAssignments == null) return;
            rng = rng ?? _rng;

            foreach (var kv in survivorRoomAssignments)
            {
                string survivorId = kv.Key;
                string roomId = kv.Value;
                if (string.IsNullOrEmpty(roomId)) continue;

                if (!IsRoomLit(roomId))
                {
                    // Morale decay
                    modifyMorale?.Invoke(survivorId, -UnlitMoraleDecayPerHour * gameHours);

                    // Fatigue multiplier (indirect: survivors tire faster in the dark)
                    modifyFatigue?.Invoke(survivorId, 1f * gameHours * (UnlitFatigueMultiplier - 1f));

                    // Claustrophobia roll
                    if (!_claustrophobiaVictims.Contains(survivorId)
                        && rng.NextDouble() < 0.02f * gameHours)
                    {
                        _claustrophobiaVictims.Add(survivorId);
                        OnClaustrophobiaTriggered?.Invoke(survivorId);
                    }
                }
            }
        }

        /// <summary>
        /// Check surgery botch chance. Dark rooms multiply the base chance.
        /// </summary>
        public float GetSurgeryBotchChance(string roomId, float baseSkill)
        {
            float baseChance = DarkSurgeryBaseBotchChance * (1f - Mathf.Clamp01(baseSkill));
            if (!IsRoomLit(roomId))
                baseChance += DarkSurgeryBotchChanceIncrease;
            return Mathf.Clamp01(baseChance);
        }

        /// <summary>Roll for surgery botch. Returns true if botched.</summary>
        public bool RollSurgeryBotch(string roomId, float baseSkill, string surgeonId)
        {
            float chance = GetSurgeryBotchChance(roomId, baseSkill);
            if (_rng.NextDouble() < chance)
            {
                OnSurgeryBotch?.Invoke(surgeonId);
                return true;
            }
            return false;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public ShadowLightSave CaptureState()
        {
            var entries = new RoomLightSave[_roomLitState.Count];
            int i = 0;
            foreach (var kv in _roomLitState)
                entries[i++] = new RoomLightSave { RoomId = kv.Key, IsLit = kv.Value };
            var victims = new string[_claustrophobiaVictims.Count];
            _claustrophobiaVictims.CopyTo(victims);
            return new ShadowLightSave { RoomLights = entries, ClaustrophobiaVictims = victims };
        }

        public void RestoreState(ShadowLightSave save)
        {
            _roomLitState.Clear();
            _claustrophobiaVictims.Clear();
            if (save == null) return;
            if (save.RoomLights != null)
                for (int i = 0; i < save.RoomLights.Length; i++)
                    if (save.RoomLights[i] != null)
                        _roomLitState[save.RoomLights[i].RoomId] = save.RoomLights[i].IsLit;
            if (save.ClaustrophobiaVictims != null)
                for (int i = 0; i < save.ClaustrophobiaVictims.Length; i++)
                    if (!string.IsNullOrEmpty(save.ClaustrophobiaVictims[i]))
                        _claustrophobiaVictims.Add(save.ClaustrophobiaVictims[i]);
        }
    }

    [Serializable]
    public class ShadowLightSave
    {
        public RoomLightSave[] RoomLights;
        public string[] ClaustrophobiaVictims;
    }

    [Serializable]
    public class RoomLightSave
    {
        public string RoomId;
        public bool IsLit;
    }
}
