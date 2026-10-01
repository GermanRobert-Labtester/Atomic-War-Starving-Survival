// SPDX-License-Identifier: MIT
// ============================================================================
// Main partial : Main.Prologue
// Core Authority: Ashfall.Core.Narrative.PrologueSequence (prologue_sequence.json)
// Purpose       : Surface the authored diegetic opening beats through the
//                 existing Opening Protocol day-goal seam. No new modal, no
//                 save section: the current day selects the beat, so a reload
//                 reproduces the same line.
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private PrologueSequence? _prologue;
        private bool _prologueLoaded;

        public PrologueSequence? Prologue => EnsurePrologue();

        private PrologueSequence? EnsurePrologue()
        {
            if (_prologueLoaded) return _prologue;
            _prologueLoaded = true;
            try
            {
                string path = Path.Combine(
                    string.IsNullOrWhiteSpace(_dataDir) ? "." : _dataDir,
                    "prologue_sequence.json");
                if (File.Exists(path))
                {
                    _prologue = PrologueSequence.LoadFromJson(File.ReadAllText(path));
                    if (_prologue == null)
                        GD.PrintErr("[Prologue] prologue_sequence.json failed validation; using live day goals only.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Prologue] load warning: {ex.Message}");
                _prologue = null;
            }
            return _prologue;
        }

        /// <summary>
        /// The authored prologue beat for a campaign day, or false once the
        /// prologue has run out (later days use the live slice/briefing goal).
        /// </summary>
        internal bool TryGetPrologueGoal(int day, out string title, out string body)
        {
            title = string.Empty;
            body = string.Empty;
            var prologue = EnsurePrologue();
            if (prologue == null || !prologue.TryGetBeatForDay(day, out var beat) || beat == null)
                return false;
            title = beat.title;
            body = beat.body;
            return true;
        }
    }
}
