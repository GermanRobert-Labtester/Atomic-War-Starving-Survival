using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.Audio;
using Ashfall.Core.UI;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// Godot 4.7+ UI Control for the Year of Ash Radio Intercept Terminal (142.850 MHz / 88.400 MHz).
    /// Thin presentation only: displays loaded Year of Ash radio bulletins and emergency carrier waves.
    /// Zero simulation rules.
    /// </summary>
    public partial class RadioBroadcastTerminal : PanelContainer
    {
        private readonly List<YearOfAshRadioEntry> _broadcasts = new List<YearOfAshRadioEntry>();
        private readonly HashSet<string> _playedAudioBroadcasts = new HashSet<string>(StringComparer.Ordinal);
        private VBoxContainer _logContainer;
        private Label _lblHeader;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.BottomLeft);
            CustomMinimumSize = new Vector2(420, 260);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            _lblHeader = new Label
            {
                Text = "SECTOR 4 EMERGENCY RADIO RECEIVER [142.850 MHz]",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _lblHeader.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(_lblHeader);

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(0, 200),
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            rootVbox.AddChild(scroll);

            _logContainer = new VBoxContainer();
            _logContainer.AddThemeConstantOverride("separation", 8);
            scroll.AddChild(_logContainer);
        }

        public void LoadBroadcasts(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loaded = YearOfAshCatalogLoader.LoadRadioBroadcasts(dataDir, fileIO, json);

            _broadcasts.Clear();
            _playedAudioBroadcasts.Clear();
            _broadcasts.AddRange(loaded);
            RefreshView(180);
        }

        public void RefreshView(int currentDay)
        {
            if (_logContainer == null) return;

            foreach (Node child in _logContainer.GetChildren())
            {
                child.QueueFree();
            }

            int visibleCount = 0;
            string? firstNewVoiceCue = null;
            foreach (var b in _broadcasts)
            {
                if (currentDay < b.dayTrigger) continue;

                // The terminal can reveal several old messages after a large day
                // jump. Emit only one newly-unlocked VO per refresh so speech
                // remains intelligible; unplayed messages remain eligible on the
                // next terminal refresh.
                if (firstNewVoiceCue == null &&
                    !string.IsNullOrWhiteSpace(b.audio_cue) &&
                    _playedAudioBroadcasts.Add(b.id))
                    firstNewVoiceCue = b.audio_cue;

                var panel = new PanelContainer();
                var vbox = new VBoxContainer();
                panel.AddChild(vbox);

                var header = new Label
                {
                    Text = $"[{b.frequency}] {b.source} {(b.isEmergency ? "● EMERGENCY" : "○ CIVIL")}"
                };
                header.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                vbox.AddChild(header);

                var body = new Label
                {
                    Text = b.message,
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                body.AddThemeFontSizeOverride("font_size", 10);
                vbox.AddChild(body);

                _logContainer.AddChild(panel);
                visibleCount++;
            }

            if (visibleCount == 0)
            {
                var emptyLbl = new Label
                {
                    Text = "No active carrier wave detected on current frequency band."
                };
                emptyLbl.AddThemeFontSizeOverride("font_size", 10);
                _logContainer.AddChild(emptyLbl);
            }

            if (firstNewVoiceCue != null)
                AudioManager.Instance?.PlayVoiceOverCue(firstNewVoiceCue);
        }
    }
}
