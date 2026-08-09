using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AndroidState
    {
        public string npcId = "npc_android";
        public bool isRevealed = false;
        public bool isDestroyed = false;
        // Track known androids by id
        public List<string> androidIds = new List<string>();
        public List<bool> revealedFlags = new List<bool>();
        public List<bool> destroyedFlags = new List<bool>();
    }

    /// <summary>
    /// Android Defector — a synthetic being that looks and acts human.
    /// Requires no sleep or food. If shot during a raid, it bleeds white
    /// synthetic fluid, revealing its true nature. The crew then mutinies
    /// to destroy the "Machine."
    /// Prompt #794: NPC_Android
    /// </summary>
    public class NPC_Android
    {
        // -- Events --
        public event Action<string> OnAndroidJoined;     // androidId
        public event Action<string> OnAndroidRevealed;   // androidId
        public event Action<string> OnMutinyTriggered;   // androidId

        // -- State --
        private readonly Dictionary<string, bool> _revealedAndroids = new Dictionary<string, bool>();
        private readonly Dictionary<string, bool> _destroyedAndroids = new Dictionary<string, bool>();

        // -- Public API --

        /// <summary>
        /// An android joins the bunker disguised as a human survivor.
        /// It requires no sleep or food.
        /// </summary>
        public void JoinBunker(string androidId)
        {
            if (string.IsNullOrEmpty(androidId)) return;
            if (!_revealedAndroids.ContainsKey(androidId))
            {
                _revealedAndroids[androidId] = false;
                _destroyedAndroids[androidId] = false;
            }
            OnAndroidJoined?.Invoke(androidId);
        }

        /// <summary>
        /// Checks if the android was shot during a raid. If so, it bleeds
        /// white synthetic fluid, revealing its non-human identity to the crew.
        /// </summary>
        public void CheckShot(string androidId, bool wasShotInRaid)
        {
            if (string.IsNullOrEmpty(androidId)) return;
            if (!wasShotInRaid) return;
            if (!_revealedAndroids.ContainsKey(androidId))
            {
                Debug.LogWarning($"[NPC_Android] Unknown android '{androidId}'.");
                return;
            }
            _revealedAndroids[androidId] = true;
            OnAndroidRevealed?.Invoke(androidId);
        }

        /// <summary>
        /// The crew turns on the revealed android, attempting to destroy it.
        /// </summary>
        public void TriggerMutiny(string androidId)
        {
            if (string.IsNullOrEmpty(androidId)) return;
            if (!_revealedAndroids.ContainsKey(androidId))
            {
                Debug.LogWarning($"[NPC_Android] Cannot trigger mutiny for unknown android '{androidId}'.");
                return;
            }
            if (!_revealedAndroids[androidId])
            {
                Debug.LogWarning($"[NPC_Android] Android '{androidId}' has not been revealed yet.");
                return;
            }
            _destroyedAndroids[androidId] = true;
            OnMutinyTriggered?.Invoke(androidId);
        }

        /// <summary>Returns true if the given android's identity has been revealed.</summary>
        public bool IsRevealed(string androidId)
        {
            if (string.IsNullOrEmpty(androidId)) return false;
            return _revealedAndroids.TryGetValue(androidId, out var revealed) && revealed;
        }

        /// <summary>Returns true if the given android has been destroyed by the crew.</summary>
        public bool IsDestroyed(string androidId)
        {
            if (string.IsNullOrEmpty(androidId)) return false;
            return _destroyedAndroids.TryGetValue(androidId, out var destroyed) && destroyed;
        }

        // -- Save / Load --

        public AndroidState CaptureState()
        {
            var state = new AndroidState
            {
                npcId = "npc_android",
                androidIds = new List<string>(),
                revealedFlags = new List<bool>(),
                destroyedFlags = new List<bool>()
            };
            foreach (var kvp in _revealedAndroids)
            {
                state.androidIds.Add(kvp.Key);
                state.revealedFlags.Add(kvp.Value);
                state.destroyedFlags.Add(
                    _destroyedAndroids.TryGetValue(kvp.Key, out var d) ? d : false);
            }
            // Set aggregate flags from first android for convenience
            if (state.androidIds.Count > 0)
            {
                state.isRevealed = state.revealedFlags[0];
                state.isDestroyed = state.destroyedFlags[0];
            }
            return state;
        }

        public void RestoreState(AndroidState saved)
        {
            _revealedAndroids.Clear();
            _destroyedAndroids.Clear();
            // A save written with an explicit null list (older build, hand-edited file)
            // deserializes these as null; under FailFastRestore one NPE aborts the load.
            if (saved?.androidIds == null) return;
            int count = saved.androidIds.Count;
            for (int i = 0; i < count; i++)
            {
                string id = saved.androidIds[i];
                if (string.IsNullOrEmpty(id)) continue;
                _revealedAndroids[id] =
                    saved.revealedFlags != null && i < saved.revealedFlags.Count && saved.revealedFlags[i];
                _destroyedAndroids[id] =
                    saved.destroyedFlags != null && i < saved.destroyedFlags.Count && saved.destroyedFlags[i];
            }
        }
    }
}
