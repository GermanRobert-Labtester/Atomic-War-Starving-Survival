using System.Collections.Generic;
using Godot;
using Ashfall.Core.Verdict;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

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
            CustomMinimumSize = new Vector2(CoreTheme.PanelMaxWidth, 400);

            // Apply standard panel 9-slice
            var tex = AshfallUiHelpers.TryLoadTexture("res://Assets/UI/Textures/panel_bg_9slice.png");
            if (tex != null)
            {
                var sb = new StyleBoxTexture
                {
                    Texture = tex,
                    TextureMarginLeft = 16,
                    TextureMarginTop = 16,
                    TextureMarginRight = 16,
                    TextureMarginBottom = 16
                };
                AddThemeStyleboxOverride("panel", sb);
            }

            var rootVbox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            AddChild(rootVbox);

            // ── Title ──
            rootVbox.AddChild(AshfallUiHelpers.MakeTitle("THE MACHINE'S REGISTER", CoreTheme.FontSizeH3));

            // ── Phase strip ──
            _lblPhase = new Label { Text = "phase: dormant" };
            _lblPhase.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeBody);
            _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
            rootVbox.AddChild(_lblPhase);

            // ── Readout ──
            _lblReadout = new Label
            {
                Text = "[shelter instruments] — standby cycle.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _lblReadout.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
            _lblReadout.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
            rootVbox.AddChild(_lblReadout);

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Log scroll ──
            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 190)
            };
            rootVbox.AddChild(scroll);

            _logList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
            scroll.AddChild(_logList);

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Figures ──
            rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("FIGURES OF THE RECORD"));

            _npcList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
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

            // Phase-colored strip — uses Theme tokens for each phase.
            switch (_verdict.Reckoning.Phase)
            {
                case ReckoningPhase.Dormant:
                    _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
                    break;
                case ReckoningPhase.Knowing:
                    _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
                    break;
                case ReckoningPhase.Culpable:
                    _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
                    break;
                case ReckoningPhase.Counted:
                    _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
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
                row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
                if (!e.read)
                    row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
                else
                    row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
                _logList.AddChild(row);
            }

            if (shown == 0)
            {
                var empty = new Label
                {
                    Text = "The log is quiet. The meter reads its own current.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                empty.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
                empty.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
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
                none.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
                none.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
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
                row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
                row.AddThemeColorOverride("font_color",
                    _verdict.Npcs.State.spokenNpcIds.Contains(npc.id)
                        ? AshfallUiHelpers.ToColor(CoreTheme.Muted)
                        : AshfallUiHelpers.ToColor(CoreTheme.Pale));

                if (!_verdict.Npcs.State.spokenNpcIds.Contains(npc.id))
                {
                    var btn = AshfallUiHelpers.MakeButton("hear", () =>
                    {
                        if (_verdict.Npcs.Speak(npc.id))
                        {
                            RefreshView();
                            EmitSignal("NpcSpoken", npc.id);
                        }
                    });
                    var h = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
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
