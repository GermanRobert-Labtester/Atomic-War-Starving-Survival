using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HackingState
    {
        public string event_id = "ui_event_hacking";
        public string vault_id;
        public int max_tries = 4;
        public int tries_remaining = 4;
        public bool is_permanently_locked = false;
        public bool is_unlocked = false;
        public string correct_word;
        public List<string> word_pool = new List<string>();
        public List<string> revealed_duds = new List<string>();
    }

    public sealed class UIEvent_Hacking
    {
        private HackingState _state;

        public event Action<string> OnGuessResult;       // "correct", "partial", "wrong"
        public event Action OnVaultUnlocked;
        public event Action OnAlarmTriggered;
        public event Action OnVaultPermanentlyLocked;

        public string EventId => _state.event_id;
        public bool IsLocked => _state.is_permanently_locked;
        public int TriesRemaining => _state.tries_remaining;

        public UIEvent_Hacking()
        {
            _state = new HackingState();
        }

        /// <summary>
        /// Initializes the hacking puzzle. Returns the displayed word list and any
        /// revealed dud words (revealed duds scale with survivor intelligence).
        /// </summary>
        public (string[] words, string[] revealed_duds) StartHack(
            string vault_id,
            float survivor_intelligence,
            List<string> word_list)
        {
            if (string.IsNullOrEmpty(vault_id))
            {
                Debug.LogError("[UIEvent_Hacking] vault_id is null or empty.");
                return (Array.Empty<string>(), Array.Empty<string>());
            }

            if (word_list == null || word_list.Count < 2)
            {
                Debug.LogError("[UIEvent_Hacking] word_list must contain at least 2 words.");
                return (Array.Empty<string>(), Array.Empty<string>());
            }

            _state.vault_id = vault_id;
            _state.tries_remaining = _state.max_tries;
            _state.is_permanently_locked = false;
            _state.is_unlocked = false;
            _state.word_pool = new List<string>(word_list);

            // Pick a random correct word
            var rng = new System.Random(vault_id.GetHashCode());
            _state.correct_word = word_list[rng.Next(word_list.Count)];

            // High intelligence reveals some dud words (non-correct words)
            _state.revealed_duds.Clear();
            float intelligence_clamped = Mathf.Clamp01(survivor_intelligence);
            int duds_to_reveal = Mathf.FloorToInt(intelligence_clamped * 3f);

            int dud_count = 0;
            for (int i = 0; i < word_list.Count && dud_count < duds_to_reveal; i++)
            {
                if (word_list[i] != _state.correct_word)
                {
                    _state.revealed_duds.Add(word_list[i]);
                    dud_count++;
                }
            }

            Debug.Log($"[UIEvent_Hacking] Hack started on vault '{vault_id}'. " +
                      $"{dud_count} dud(s) revealed (intelligence={intelligence_clamped:F2}).");

            return (_state.word_pool.ToArray(), _state.revealed_duds.ToArray());
        }

        /// <summary>
        /// Submit a guess. Returns "correct", "partial", or "wrong".
        /// After max wrong guesses, triggers permanent lock and alarm.
        /// </summary>
        public string Guess(string word)
        {
            if (_state.is_permanently_locked)
            {
                Debug.LogWarning("[UIEvent_Hacking] Vault is permanently locked.");
                return "wrong";
            }

            if (_state.is_unlocked)
            {
                Debug.LogWarning("[UIEvent_Hacking] Vault is already unlocked.");
                return "correct";
            }

            if (string.IsNullOrEmpty(word))
            {
                Debug.LogError("[UIEvent_Hacking] Guess word is null or empty.");
                return "wrong";
            }

            string result;

            if (word == _state.correct_word)
            {
                result = "correct";
                _state.is_unlocked = true;
                OnGuessResult?.Invoke(result);
                OnVaultUnlocked?.Invoke();
                Debug.Log($"[UIEvent_Hacking] Vault '{_state.vault_id}' unlocked.");
            }
            else
            {
                // Count matching characters at same positions for partial feedback
                int matches = 0;
                int len = Mathf.Min(word.Length, _state.correct_word.Length);
                for (int i = 0; i < len; i++)
                {
                    if (word[i] == _state.correct_word[i])
                        matches++;
                }

                result = matches > 0 ? "partial" : "wrong";
                _state.tries_remaining--;

                OnGuessResult?.Invoke(result);

                if (_state.tries_remaining <= 0)
                {
                    _state.is_permanently_locked = true;
                    OnVaultPermanentlyLocked?.Invoke();
                    OnAlarmTriggered?.Invoke();
                    Debug.Log($"[UIEvent_Hacking] Vault '{_state.vault_id}' permanently locked — alarm triggered.");
                }
            }

            return result;
        }

        public HackingState CaptureState()
        {
            return new HackingState
            {
                event_id = _state.event_id,
                vault_id = _state.vault_id,
                max_tries = _state.max_tries,
                tries_remaining = _state.tries_remaining,
                is_permanently_locked = _state.is_permanently_locked,
                is_unlocked = _state.is_unlocked,
                correct_word = _state.correct_word,
                word_pool = new List<string>(_state.word_pool),
                revealed_duds = new List<string>(_state.revealed_duds)
            };
        }

        public void RestoreState(HackingState saved)
        {
            _state = saved ?? new HackingState();
        }
    }
}
