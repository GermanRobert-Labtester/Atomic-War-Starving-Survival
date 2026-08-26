using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;
using AtomicWar.Journal;

namespace AtomicWar.GodotApp
{
    public class JournalHostSession : HostSessionBase
    {
        public JournalSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        private Action<JournalEntry>? _onentryadded_handler;
        private Action<JournalEntry>? _onnotificationping_handler;

        public JournalHostSession(JournalSystem? system = null)
        {
            System = system ?? new JournalSystem();
            _onentryadded_handler = entry =>
            {
                LastEvent = $"[Journal] New entry: {entry.Text}";
                RaiseStateChanged();
            };
            _onnotificationping_handler = entry =>
            {
                LastEvent = $"[Journal] Notification: {entry.Text}";
                RaiseStateChanged();
            };
        }

        public override void Save()
        {
            if (!IsDirty) return;
            try
            {
                var save = System.CaptureState();
                JournalSaveStore.Save(save);
                IsDirty = false;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Journal] save failed: " + e.Message);
            }
        }

        public void RestoreSave(JournalSave state)
        {
            if (state == null) return;
            try
            {
                System.RestoreState(state);
                IsDirty = false;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Journal] restore failed: " + e.Message);
            }
        }
    }
}