using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DeadLetterOfficeState
    {
        public string id = "encounter_dead_letter_office";
        public string displayName = "Dead Letter Office";
        public int letterCount = 12;
        public int supplyPackCount = 3;
        public string factionBoundLetterId = "letter_to_scavengers";
        public bool isLooted = false;
        public bool letterDelivered = false;
        public bool lettersRead = false;
    }

    /// <summary>
    /// Prompt #901: Encounter — The Dead Letter Office.
    /// An overturned postal van on the ring road, four kilometers from Tessarat.
    /// The driver died at the wheel, hands still gripping it. In the back:
    /// twelve undelivered letters, three supply packs, and one envelope
    /// addressed to a survivor now living at the Scavenger Camp.
    ///
    /// The survivor can:
    ///   - Read the letters (small morale hit — absorbing pre-war grief)
    ///   - Deliver the faction-bound letter (trust boost with scavenger_camp)
    ///   - Take the supply packs and leave the rest
    ///   - Burn the van (morale hit to whole party, but prevents others
    ///     from finding the supplies)
    ///
    /// No combat. No skill check. Just a quiet moment in the ash.
    /// </summary>
    public class Encounter_DeadLetterOffice
    {
        private static System.Random _fallbackRng;
        private static System.Random FallbackRng =>
            _fallbackRng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("encounter_deadletteroffice");

        private DeadLetterOfficeState _state = new DeadLetterOfficeState();

        /// <summary>Morale penalty per letter read (absorbs their grief).</summary>
        public const float MoraleHitPerLetter = 2f;

        /// <summary>Morale hit for burning the van (party-wide).</summary>
        public const float BurnMoraleHit = 10f;

        /// <summary>Trust boost for delivering the faction letter.</summary>
        public const float DeliveryTrustBoost = 8f;

        /// <summary>Faction that the bound letter is addressed to.</summary>
        public const string TargetFactionId = "scavenger_camp";

        /// <summary>Supply item id found in the packs.</summary>
        public const string SupplyItemId = "canned_food";

        /// <summary>Comfort item id — the unopened letter.</summary>
        public const string LetterItemId = "prewar_letter";

        // ── Events ──────────────────────────────────────────────────────
        public event Action<DeadLetterOfficeState> OnVanDiscovered;
        public event Action<DeadLetterOfficeState, int> OnLettersRead;       // state, count
        public event Action<DeadLetterOfficeState> OnLetterDelivered;        // faction trust applied
        public event Action<DeadLetterOfficeState, int> OnSuppliesTaken;     // state, packCount
        public event Action<DeadLetterOfficeState> OnVanBurned;

        public DeadLetterOfficeState State => _state;

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>Notifies that the postal van has been discovered.</summary>
        public void DiscoverVan()
        {
            if (_state.isLooted) return;
            OnVanDiscovered?.Invoke(_state);
        }

        /// <summary>
        /// Read some or all of the letters. Each one applies a small morale
        /// penalty as the survivor absorbs the grief of strangers.
        /// Returns the number actually read.
        /// </summary>
        public int ReadLetters(int count)
        {
            if (_state.isLooted) return 0;
            int toRead = Mathf.Min(count, _state.letterCount);
            _state.lettersRead = true;
            _state.letterCount -= toRead;
            OnLettersRead?.Invoke(_state, toRead);
            return toRead;
        }

        /// <summary>
        /// Read all remaining letters at once. Returns the total morale
        /// penalty that should be applied.
        /// </summary>
        public float ReadAllLetters()
        {
            int count = ReadLetters(_state.letterCount);
            return count * MoraleHitPerLetter;
        }

        /// <summary>
        /// Deliver the letter addressed to the scavenger camp.
        /// Returns true if a letter was available to deliver.
        /// </summary>
        public bool DeliverFactionLetter()
        {
            if (_state.isLooted || _state.letterDelivered) return false;
            _state.letterDelivered = true;
            OnLetterDelivered?.Invoke(_state);
            return true;
        }

        /// <summary>
        /// Take supply packs from the van. Returns the number taken.
        /// </summary>
        public int TakeSupplies(int packsToTake = -1)
        {
            if (_state.isLooted) return 0;
            int toTake = packsToTake < 0
                ? _state.supplyPackCount
                : Mathf.Min(packsToTake, _state.supplyPackCount);
            _state.supplyPackCount -= toTake;
            if (_state.supplyPackCount <= 0 && _state.letterCount <= 0)
                _state.isLooted = true;
            OnSuppliesTaken?.Invoke(_state, toTake);
            return toTake;
        }

        /// <summary>
        /// Mark the van as fully looted.
        /// </summary>
        public void MarkLooted()
        {
            _state.isLooted = true;
        }

        /// <summary>
        /// Burn the postal van. Applies a party-wide morale hit but
        /// prevents anyone else from finding the supplies.
        /// </summary>
        public void BurnVan()
        {
            if (_state.isLooted) return;
            _state.isLooted = true;
            OnVanBurned?.Invoke(_state);
        }

        // ── Lore — one-line letter fragments ────────────────────────────

        /// <summary>Random letter fragment for the UI/journal.</summary>
        public static string GetRandomLetterFragment(System.Random rng)
        {
            rng ??= FallbackRng;
            string[] fragments =
            {
                "\"…tell mother the tomatoes came in. All six of them. She'll know what that means.\"",
                "\"…they've stopped the trains. I'll try the ring road on Thursday. If you get this before I arrive, put the blue jug out.\"",
                "\"…the baby said her first word today. 'Light.' Just 'light.' I don't know why that breaks me.\"",
                "\"…I'm not coming home. I've enclosed the deed to the allotment. Don't sell it. Not yet.\"",
                "\"…the radio says the negotiations are going well. The radio has said that for three weeks. The shelling is closer.\"",
                "\"…happy birthday. You're seven today. I hope you're somewhere that still remembers what a birthday is.\"",
                "\"…they found water in the west field. Not much. But enough to argue over, which around here passes for hope.\"",
                "\"…I saw a bird yesterday. A real one. Grey, small, but alive. I stood there for ten minutes just watching it breathe.\"",
                "\"…the iodine ran out on Tuesday. Mr. Halper from the pharmacy said to boil the water twice. He didn't say what to do after that.\"",
                "\"…if you're reading this, the postal service outlasted the government. Small victories.\"",
            };
            return fragments[rng.Next(fragments.Length)];
        }

        // ── Save/Load ───────────────────────────────────────────────────

        public DeadLetterOfficeState CaptureState() => _state;

        public void RestoreState(DeadLetterOfficeState save)
        {
            if (save != null) _state = save;
        }
    }
}
