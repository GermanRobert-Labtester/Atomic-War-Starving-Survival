using System.Collections.Generic;
#pragma warning disable CS8618
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
        public event System.Action? OnClose;

        private VerdictHostSession _verdict;
        private Label _lblPhase;
        private Label _lblReadout;
        private VBoxContainer _logList;
        private VBoxContainer _npcList;
        private VBoxContainer _placeList;
        private VBoxContainer _radioList;

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            CustomMinimumSize = new Vector2(CoreTheme.PanelMaxWidth, 400);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());

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

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("PLACES & EVIDENCE"));

            _placeList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
            rootVbox.AddChild(_placeList);

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("TRANSMISSIONS"));

            var radioScroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 170)
            };
            rootVbox.AddChild(radioScroll);
            _radioList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
            radioScroll.AddChild(_radioList);

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
            btnClose.CustomMinimumSize = new Vector2(240, 36);
            rootVbox.AddChild(btnClose);
        }

        public void Bind(VerdictHostSession verdict)
        {
            _verdict = verdict;
            if (verdict != null) verdict.StateChanged += RefreshView;
        }

        public void RefreshView()
        {
            if (_verdict == null || _logList == null || _radioList == null) return;

            RefreshPhaseStrip();
            RefreshLog();
            RefreshNpcs();
            RefreshPlaces();
            RefreshRadio();
        }

        /// <summary>Thin read-only accessor for tests: the number of broadcast rows
        /// currently rendered in the TRANSMISSIONS section (broadcast labels carry a
        /// "[D{day}]" marker; the summary header label is excluded).</summary>
        public int RenderedRadioRowCount()
        {
            if (_radioList == null) return 0;
            int n = 0;
            foreach (Node child in _radioList.GetChildren())
                if (child is Label lbl && lbl.Text != null && lbl.Text.Contains("[D")) n++;
            return n;
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
            AshfallUiHelpers.EmptyChildren(_logList);

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
            AshfallUiHelpers.EmptyChildren(_npcList);

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

        /// <summary>Render the reachable Verdict places and evidence/story items.
        /// Thin presentation: lists what the machine's records point to (the four
        /// standing sites and the fifteen evidence/quest objects) as read-only rows.</summary>
        private void RefreshPlaces()
        {
            AshfallUiHelpers.EmptyChildren(_placeList);

            bool any = false;
            if (_verdict.Locations != null)
            {
                for (int i = 0; i < _verdict.Locations.Count; i++)
                {
                    var loc = _verdict.Locations[i];
                    if (loc == null || string.IsNullOrEmpty(loc.displayName)) continue;
                    any = true;
                    var row = new Label
                    {
                        Text = $"▨ {loc.displayName} ({loc.id}) · danger {loc.dangerLevel}\n   {Truncate(loc.description, 180)}",
                        AutowrapMode = TextServer.AutowrapMode.WordSmart
                    };
                    row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
                    row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
                    _placeList.AddChild(row);
                }
            }
            if (_verdict.Items != null)
            {
                for (int i = 0; i < _verdict.Items.Count; i++)
                {
                    var it = _verdict.Items[i];
                    if (it == null || string.IsNullOrEmpty(it.id)) continue;
                    any = true;
                    string kind = string.IsNullOrEmpty(it.category) ? "story_item" : it.category;
                    string icon = kind switch
                    {
                        "consumable" => "⊕",
                        "quest_item" => "◆",
                        _ => "◈"
                    };
                    var row = new Label
                    {
                        Text = $"{icon} {it.displayName} ({it.id}) · {kind}",
                        AutowrapMode = TextServer.AutowrapMode.WordSmart
                    };
                    row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
                    row.AddThemeColorOverride("font_color",
                        it.id.StartsWith("evidence_") ? AshfallUiHelpers.ToColor(CoreTheme.Warm)
                                                       : AshfallUiHelpers.ToColor(CoreTheme.Muted));
                    _placeList.AddChild(row);
                }
            }
            if (!any)
            {
                var empty = new Label
                {
                    Text = "The record names no places yet. Keep the machine reading.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                empty.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
                empty.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
                _placeList.AddChild(empty);
            }
        }

        /// <summary>Render the diegetic radio corpus (verdict_radio.json). Lists each
        /// broadcast with its dayTrigger and kind, marking fired vs pending. Drivers
        /// from the session's VerdictRadioSystem state. Thin presentation only.</summary>
        private void RefreshRadio()
        {
            AshfallUiHelpers.EmptyChildren(_radioList);
            if (_verdict.Radio == null)
            {
                _radioList.AddChild(AshfallUiHelpers.MakeSmall("The radio is silent.", true));
                return;
            }

            var radio = _verdict.Radio;
            var header = new Label
            {
                Text = $"{radio.FiredCount}/{radio.Corpus.Count} broadcasts received",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            header.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
            header.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
            _radioList.AddChild(header);

            bool any = false;
            if (radio.Corpus != null)
            {
                for (int i = 0; i < radio.Corpus.Count; i++)
                {
                    var r = radio.Corpus[i];
                    if (r == null || string.IsNullOrEmpty(r.id)) continue;
                    any = true;
                    bool isFired = radio.HasFired(r.id);
                    string kindIcon = r.kind switch
                    {
                        "carrier" => "◌",
                        "call" => "▸",
                        "maintenance" => "⚙",
                        "witness" => "✉",
                        "readings" => "▥",
                        _ => "·"
                    };
                    var row = new Label
                    {
                        Text = $"{kindIcon} {Truncate(r.message, 90)} \n   [D{r.dayTrigger}] {r.id} · {(isFired ? "RECEIVED" : "pending")}",
                        AutowrapMode = TextServer.AutowrapMode.WordSmart
                    };
                    row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
                    row.AddThemeColorOverride("font_color",
                        isFired ? AshfallUiHelpers.ToColor(CoreTheme.Pale)
                                : AshfallUiHelpers.ToColor(CoreTheme.Muted));
                    _radioList.AddChild(row);
                }
            }
            if (!any)
            {
                _radioList.AddChild(AshfallUiHelpers.MakeSmall("No broadcasts logged yet.", true));
            }
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= max) return s ?? string.Empty;
            return s.Substring(0, max) + "…";
        }

        [Signal]
        public delegate void NpcSpokenEventHandler(string npcId);

        public override void _ExitTree()
        {
            if (_verdict != null)
            {
                _verdict.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
