using System.Collections.Generic;
using Godot;
using Ashfall.Core.Verdict;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — shelter machine surface.
    /// Diegetic panel presenting the machine log, the Reckoning phase strip
    /// (phase-colored), the shelter readout, evidence counter, and the
    /// available Verdict figures (flag-gated, one-shot spoken). Thin
    /// presentation only; zero simulation logic.
    /// </summary>
    public partial class VerdictPanel : PanelContainer
    {
        private VerdictHostSession _verdict;
        private Label _lblPhase;
        private Label _lblReadout;
        private VBoxContainer _logList;
        private VBoxContainer _npcList;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(420, 400);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "THE MACHINE'S REGISTER",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", 14);
            rootVbox.AddChild(title);

            _lblPhase = new Label { Text = "phase: dormant" };
            _lblPhase.AddThemeFontSizeOverride("font_size", 13);
            rootVbox.AddChild(_lblPhase);

            _lblReadout = new Label
            {
                Text = "[shelter instruments] — standby cycle.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _lblReadout.AddThemeFontSizeOverride("font_size", 11);
            rootVbox.AddChild(_lblReadout);

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 190)
            };
            rootVbox.AddChild(scroll);

            _logList = new VBoxContainer();
            scroll.AddChild(_logList);

            // Figures of the machine's human record.
            var npcTitle = new Label
            {
                Text = "FIGURES OF THE RECORD",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            npcTitle.AddThemeFontSizeOverride("font_size", 12);
            rootVbox.AddChild(npcTitle);

            _npcList = new VBoxContainer();
            rootVbox.AddChild(_npcList);
        }

        public void Bind(VerdictHostSession verdict)
        {
            _verdict = verdict;
            if (verdict != null) verdict.StateChanged += RefreshView;
        }

        public void RefreshView()
        {
            if (_verdict == null || _logList == null) return;

            RefreshPhaseStrip();
            RefreshLog();
            RefreshNpcs();
        }

        private void RefreshPhaseStrip()
        {
            var state = _verdict.Reckoning.State;
            string phaseName = _verdict.Reckoning.Phase.ToString().ToLowerInvariant();
            string callState = state.callResolved ? " · call RESOLVED" : "";
            _lblPhase.Text = $"phase: {phaseName} · evidence {_verdict.Evidence.Count} · " +
                             $"logs read {_verdict.MachineLog.ReadCount()}/{_verdict.MachineLog.Entries.Count}{callState}";

            // Phase-colored strip (procedural, restrained hues).
            switch (_verdict.Reckoning.Phase)
            {
                case ReckoningPhase.Dormant:
                    _lblPhase.AddThemeColorOverride("font_color", new Color(0.45f, 0.47f, 0.5f));
                    break;
                case ReckoningPhase.Knowing:
                    _lblPhase.AddThemeColorOverride("font_color", new Color(0.53f, 0.56f, 0.42f));
                    break;
                case ReckoningPhase.Culpable:
                    _lblPhase.AddThemeColorOverride("font_color", new Color(0.62f, 0.5f, 0.26f));
                    break;
                case ReckoningPhase.Counted:
                    _lblPhase.AddThemeColorOverride("font_color", new Color(0.68f, 0.36f, 0.3f));
                    break;
            }

            _lblReadout.Text = VerdictReadout.LineFor(
                state, _verdict.Evidence.Count, _verdict.MachineLog.ReadCount());
        }

        private void RefreshLog()
        {
            foreach (Node child in _logList.GetChildren())
                child.QueueFree();

            var entries = _verdict.MachineLog.Entries;
            int shown = 0;
            for (int i = entries.Count - 1; i >= 0 && shown < 12; i--, shown++)
            {
                var e = entries[i];
                string flag = e.read ? "read" : "unread";
                string icon = e.kind switch
                {
                    "maintenance" => "⚙",
                    "anomaly" => "◆",
                    "count" => "∑",
                    _ => "·"
                };
                var row = new Label
                {
                    Text = $"{icon} [D{e.day}] {e.facilityId} · {e.kind} · {flag}\n   {e.bodyShort}",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                row.AddThemeFontSizeOverride("font_size", 10);
                if (!e.read)
                    row.AddThemeColorOverride("font_color", new Color(0.8f, 0.78f, 0.7f));
                _logList.AddChild(row);
            }

            if (shown == 0)
            {
                var empty = new Label
                {
                    Text = "The log is quiet. The meter reads its own current.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                empty.AddThemeFontSizeOverride("font_size", 10);
                _logList.AddChild(empty);
            }
        }

        private void RefreshNpcs()
        {
            foreach (Node child in _npcList.GetChildren())
                child.QueueFree();

            var available = _verdict.AvailableNpcs();
            if (available.Count == 0)
            {
                var none = new Label
                {
                    Text = "No figures have stepped forward yet. The record waits.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                none.AddThemeFontSizeOverride("font_size", 10);
                _npcList.AddChild(none);
                return;
            }

            for (int i = 0; i < available.Count; i++)
            {
                var npc = available[i];
                string kindIcon = npc.kind switch
                {
                    "tape_echo" => "▤",
                    "paper_ghost" => "✉",
                    "living" => "◉",
                    "readings" => "▥",
                    _ => "·"
                };
                string shown = _verdict.Npcs.State.spokenNpcIds.Contains(npc.id) ? "(spoken)" : "";
                var row = new Label
                {
                    Text = $"{kindIcon} {npc.name} — {npc.role} {shown}",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                row.AddThemeFontSizeOverride("font_size", 10);

                if (!_verdict.Npcs.State.spokenNpcIds.Contains(npc.id))
                {
                    var btn = new Button
                    {
                        Text = "hear",
                        CustomMinimumSize = new Vector2(0, 22)
                    };
                    string captured = npc.id;
                    btn.Pressed += () =>
                    {
                        if (_verdict.Npcs.Speak(captured))
                        {
                            RefreshView();
                            EmitSignal("NpcSpoken", captured);
                        }
                    };
                    var h = new HBoxContainer();
                    h.AddChild(row);
                    h.AddChild(btn);
                    _npcList.AddChild(h);
                }
                else
                {
                    _npcList.AddChild(row);
                }
            }
        }

        [Signal]
        public delegate void NpcSpokenEventHandler(string npcId);
    }
}
