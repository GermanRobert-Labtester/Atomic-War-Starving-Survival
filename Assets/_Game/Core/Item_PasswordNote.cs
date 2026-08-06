using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PasswordNoteState
    {
        public string item_id = "item_password_note";
        public List<string> codes = new List<string>();
        public List<string> location_hints = new List<string>();
    }

    public sealed class Item_PasswordNote
    {
        private PasswordNoteState _state;

        public event Action<string, string, string> OnNoteFound;    // (survivor_id, code, location_hint)
        public event Action<string> OnCodeAccepted;                // code
        public event Action<string> OnCodeRejected;                // code

        public string ItemId => _state.item_id;

        public Item_PasswordNote()
        {
            _state = new PasswordNoteState();
        }

        /// <summary>
        /// Records a newly found password note, adding it to the journal.
        /// </summary>
        public void FindNote(string survivor_id, string code, string location_hint)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Item_PasswordNote] survivor_id is null or empty.");
                return;
            }

            if (string.IsNullOrEmpty(code))
            {
                Debug.LogError("[Item_PasswordNote] code is null or empty.");
                return;
            }

            // Avoid duplicate entries
            for (int i = 0; i < _state.codes.Count; i++)
            {
                if (_state.codes[i] == code)
                {
                    Debug.Log($"[Item_PasswordNote] Code '{code}' already in journal.");
                    return;
                }
            }

            string hint = location_hint ?? "";
            _state.codes.Add(code);
            _state.location_hints.Add(hint);

            OnNoteFound?.Invoke(survivor_id, code, hint);
            Debug.Log($"[Item_PasswordNote] Survivor '{survivor_id}' found code '{code}' " +
                      $"(hint: '{hint}').");
        }

        /// <summary>
        /// Attempts to enter a code at a keypad. Returns true if the code matches.
        /// </summary>
        public bool TryEnterCode(string code, string required_code)
        {
            if (string.IsNullOrEmpty(code))
            {
                Debug.LogWarning("[Item_PasswordNote] Entered code is null or empty.");
                OnCodeRejected?.Invoke(code ?? "");
                return false;
            }

            if (string.IsNullOrEmpty(required_code))
            {
                Debug.LogError("[Item_PasswordNote] required_code is null or empty.");
                OnCodeRejected?.Invoke(code);
                return false;
            }

            if (code == required_code)
            {
                OnCodeAccepted?.Invoke(code);
                Debug.Log($"[Item_PasswordNote] Code '{code}' accepted.");
                return true;
            }

            OnCodeRejected?.Invoke(code);
            Debug.Log($"[Item_PasswordNote] Code '{code}' rejected.");
            return false;
        }

        /// <summary>
        /// Returns all found codes as a list of (code, location_hint) tuples.
        /// </summary>
        public List<(string code, string location_hint)> GetAllFoundCodes()
        {
            var result = new List<(string code, string location_hint)>(_state.codes.Count);

            for (int i = 0; i < _state.codes.Count; i++)
            {
                string hint = (i < _state.location_hints.Count) ? _state.location_hints[i] : "";
                result.Add((_state.codes[i], hint));
            }

            return result;
        }

        public PasswordNoteState CaptureState()
        {
            return new PasswordNoteState
            {
                item_id = _state.item_id,
                codes = new List<string>(_state.codes),
                location_hints = new List<string>(_state.location_hints)
            };
        }

        public void RestoreState(PasswordNoteState saved)
        {
            _state = saved ?? new PasswordNoteState();
        }
    }
}
