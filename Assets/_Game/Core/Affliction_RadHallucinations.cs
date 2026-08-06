using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RadHallucinationState
    {
        public string affliction_id = "affliction_rad_hallucinations";
        public int stage_threshold = 3;
        public int fake_loot_count = 3;
    }

    public sealed class Affliction_RadHallucinations
    {
        private RadHallucinationState _state;

        public event Action<string, string> OnFakeLootDisplayed;
        public event Action<string> OnWastedGrabAttempt;

        public string AfflictionId => _state.affliction_id;

        public Affliction_RadHallucinations()
        {
            _state = new RadHallucinationState();
        }

        public bool IsAtStage3(int current_rad_stage)
        {
            return current_rad_stage >= _state.stage_threshold;
        }

        public List<string> GenerateFakeLoot(string survivor_id, Random rng)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Affliction_RadHallucinations] survivor_id is null or empty.");
                return new List<string>();
            }

            if (rng == null)
            {
                Debug.LogError("[Affliction_RadHallucinations] rng is null.");
                return new List<string>();
            }

            var fake_item_ids = new List<string>();

            string[] fake_templates =
            {
                "fake_canned_food",
                "fake_clean_water",
                "fake_mre",
                "fake_purified_water",
                "fake_preserved_meat",
                "fake_dried_ration"
            };

            for (int i = 0; i < _state.fake_loot_count; i++)
            {
                int idx = rng.Next(0, fake_templates.Length);
                string fake_id = $"{fake_templates[idx]}_{i}";
                fake_item_ids.Add(fake_id);
                OnFakeLootDisplayed?.Invoke(survivor_id, fake_id);
            }

            Debug.Log($"[Affliction_RadHallucinations] Generated {fake_item_ids.Count} fake loot items for '{survivor_id}'.");
            return fake_item_ids;
        }

        public void TryPickUp(string survivor_id, string fake_item_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Affliction_RadHallucinations] survivor_id is null or empty.");
                return;
            }

            OnWastedGrabAttempt?.Invoke(survivor_id);
            Debug.Log($"[Affliction_RadHallucinations] Survivor '{survivor_id}' wasted time grasping at dust (fake item '{fake_item_id}').");
        }

        public RadHallucinationState CaptureState()
        {
            return new RadHallucinationState
            {
                affliction_id = _state.affliction_id,
                stage_threshold = _state.stage_threshold,
                fake_loot_count = _state.fake_loot_count
            };
        }

        public void RestoreState(RadHallucinationState saved)
        {
            _state = saved ?? new RadHallucinationState();
        }
    }
}
