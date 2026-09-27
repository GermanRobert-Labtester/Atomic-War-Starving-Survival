// SPDX-License-Identifier: MIT
// ============================================================================
// Main partial : Main.Letters
// Core State   : Ashfall.Core.Narrative.LetterDeliverySystem (Plan 212 letter route)
// Purpose      : discovered letters — explicit delivery decision, recipient
//                match, canonical Needs morale exactly once per delivery,
//                truthful privacy-safe readout. Distinct authority from the
//                capsule envelope (plan's own split); rides the narrative
//                setup, save participant, and reset lifecycle.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private LetterDeliveryHostSession? _letters;
        private bool _lettersDirty;

        public LetterDeliveryHostSession? Letters => _letters;

        private void SetupLetters()
        {
            if (_letters != null) return;

            var saved = LetterDeliverySaveStore.TryLoad();
            _letters = LetterDeliveryHostSession.Create(_dataDir);
            if (saved != null) _letters.RestoreState(saved);
            _letters.StateChanged += () => _lettersDirty = true;
        }

        /// <summary>
        /// Canonical morale path: delivery of an authored letter to its
        /// addressed survivor applies one bounded morale delta through the
        /// Needs owner, exactly once (the Core owner refuses re-delivery).
        /// </summary>
        public string DeliverLetterToSurvivor(string letterId, string recipientSurvivorId, int? day = null)
        {
            SetupLetters();
            if (_letters == null) return "Letters not ready.";
            int effectiveDay = day ?? _simDay;

            if (!_letters.Address(letterId, recipientSurvivorId, effectiveDay))
                return _letters.LastEvent;

            bool delivered = _letters.Deliver(letterId, effectiveDay);
            if (delivered)
            {
                _lettersDirty = true;
                // Canonical owner: the survivor's morale, attributed and bounded.
                // Identity validated through the canonical grammar first.
                if (Ashfall.Core.Survivors.SurvivorId.TryParse(recipientSurvivorId, out _, out _)
                    && _survivors?.Needs.Get(recipientSurvivorId) != null)
                {
                    _survivors!.Needs.Modify(recipientSurvivorId,
                        Ashfall.Core.Survivors.NeedKind.Morale, 6.0f);
                }
            }
            return _letters.LastEvent;
        }

        public string WithholdLetter(string letterId)
        {
            SetupLetters();
            if (_letters == null) return "Letters not ready.";
            if (!_letters.Withhold(letterId, _simDay, "withheld by the overseer")) return _letters.LastEvent;
            _lettersDirty = true;
            return _letters.LastEvent;
        }

        public string DiscoverLetter(string letterId)
        {
            SetupLetters();
            if (_letters == null) return "Letters not ready.";
            if (_letters.Discover(letterId, _simDay) != null) _lettersDirty = true;
            return _letters.LastEvent;
        }

        /// <summary>Privacy-safe player readout (Plan 212 §3).</summary>
        public string GetLettersReadout()
        {
            if (_letters == null) return "Letters not ready.";
            var visible = _letters.VisibleLetters();
            var sb = new System.Text.StringBuilder();
            sb.Append($"Letters: {visible.Count} tracked / {_letters.AuthoredLetterIds().Count} authored");
            foreach (var (rec, body) in visible)
            {
                sb.Append($"\n  • {rec.letterId} [{rec.state}] day {rec.foundDay}");
                sb.Append($"\n      \"{(rec.resolutionNotes.Length > 60 ? rec.resolutionNotes.Substring(0, 57) + "..." : rec.resolutionNotes)}\"");
                sb.Append($"\n      body: {(rec.state == LetterDeliveryState.Delivered ? "revealed" : "sealed until delivery")}");
            }
            return sb.ToString();
        }

        public void SaveLetters()
        {
            if (_letters == null) return;
            var state = _letters.CaptureState();
            LetterDeliverySaveStore.TrySave(state);
            if (CaptureSection(LetterDeliverySaveStore.SectionName, LetterDeliverySaveStore.TryCapturePersisted(state)))
                _lettersDirty = false;
        }

        public void FlushLettersIfDirty()
        {
            if (_lettersDirty) SaveLetters();
        }

        public void ResetLetters()
        {
            _letters = null;
            _lettersDirty = false;
        }
    }
}
