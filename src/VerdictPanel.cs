using Godot;
using Ashfall.Core.Verdict;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — shelter machine surface.
    /// Diegetic panel presenting the machine log, the Reckoning phase strip,
    /// the shelter readout line, and the evidence counter. Thin presentation
    /// only: renders VerdictHostSession state; zero simulation logic.
    /// </summary>
    public partial class VerdictPanel : PanelContainer
    {
        private VerdictHostSession _verdict;
        private Label _lblPhase;
        private Label _lblReadout;
        private VBoxContainer _logList;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(400, 320);

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
                CustomMinimumSize = new Vector2(0, 200)
            };
            rootVbox.AddChild(scroll);

            _logList = new VBoxContainer();
            scroll.AddChild(_logList);
        }

        public void Bind(VerdictHostSession verdict)
        {
            _verdict = verdict;
            if (verdict != null) verdict.StateChanged += RefreshView;
        }

        public void RefreshView()
        {
            if (_verdict == null || _logList == null) return;

            foreach (Node child in _logList.GetChildren())
                child.QueueFree();

            string phaseName = _verdict.Reckoning.Phase.ToString().ToLowerInvariant();
            string callState = _verdict.Reckoning.State.callResolved ? " · call RESOLVED" : "";
            _lblPhase.Text = $"phase: {phaseName} · evidence {_verdict.Evidence.Count} · " +
                             $"logs read {_verdict.MachineLog.ReadCount()}/{_verdict.MachineLog.Entries.Count}{callState}";

            _lblReadout.Text = VerdictReadout.LineFor(
                _verdict.Reckoning.State, _verdict.Evidence.Count, _verdict.MachineLog.ReadCount());

            // Latest machine-log entries first (bottom-up presentation mirror).
            var entries = _verdict.MachineLog.Entries;
            int shown = 0;
            for (int i = entries.Count - 1; i >= 0 && shown < 12; i--, shown++)
            {
                var e = entries[i];
                string flag = e.read ? "read" : "unread";
                string corrupt = e.kind == "anomaly" ? " ◆" : "";
                var row = new Label
                {
                    Text = $"[D{e.day}] {e.facilityId} · {e.kind}{corrupt} · {flag}\n   {e.bodyShort}",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                row.AddThemeFontSizeOverride("font_size", 10);
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
    }
}
