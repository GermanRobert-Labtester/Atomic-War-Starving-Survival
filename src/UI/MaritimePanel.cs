using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Maritime Panel (Expansion 09 — The Black Flotilla).
    /// Manages submerged stealth dive operations, air supply compressors, noise detection,
    /// progressive chamber breach (4-room hierarchy), psychological contamination,
    /// and maritime salvage extraction.
    ///
    /// Presentation only — delegates simulation state to MaritimeHostSession.
    /// </summary>
    public partial class MaritimePanel : Control
    {
        public event Action? OnClose;

        private MaritimeHostSession? _maritime;
        private SurvivorsHostSession? _survivors;
        private VBoxContainer _diveDetailsContainer = null!;
        private VBoxContainer _lootDetailsContainer = null!;
        private Label _statusLabel = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
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

        public void Bind(MaritimeHostSession? maritime, SurvivorsHostSession? survivors)
        {
            _maritime = maritime;
            _survivors = survivors;
            if (_maritime != null)
            {
                _maritime.StateChanged += RefreshView;
            }
            RefreshView();
        }

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

        private void BuildLayout()
        {
            var backdrop = new ColorRect
            {
                Color = new Color(0.03f, 0.04f, 0.06f, 0.95f)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_right", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_top", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_bottom", (int)CoreTheme.SpacingLg);
            AddChild(margin);

            var mainVBox = new VBoxContainer();
            mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            margin.AddChild(mainVBox);

            // ── Header Card ──
            var headerCard = AshfallUiHelpers.MakeCardFrame(
                "THE BLACK FLOTILLA // EXP 09: MARITIME SALVAGE & STEALTH DIVE",
                "Four-chamber submerged stealth dive operations, manual air compression, noise detection, psychological contamination, and procedural maritime scavenging."
            );
            mainVBox.AddChild(headerCard);

            // ── Scrollable Body ──
            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            var contentBox = new VBoxContainer();
            contentBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            _diveDetailsContainer = new VBoxContainer();
            _diveDetailsContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_diveDetailsContainer);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("PROCEDURAL SALVAGE & SUBMERGED LOOT NODES"));

            _lootDetailsContainer = new VBoxContainer();
            _lootDetailsContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_lootDetailsContainer);

            // ── Bottom Action Bar ──
            var bottomBar = new HBoxContainer();
            bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            mainVBox.AddChild(bottomBar);

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
            btnClose.CustomMinimumSize = new Vector2(240, 44);
            bottomBar.AddChild(btnClose);

            _statusLabel = AshfallUiHelpers.MakeMono("Diving operations ready. Select a diver and initiate submerged exploration.");
            _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomBar.AddChild(_statusLabel);
        }

        public void RefreshView()
        {
            if (_diveDetailsContainer == null || _lootDetailsContainer == null) return;

            ClearContainer(_diveDetailsContainer);
            ClearContainer(_lootDetailsContainer);

            if (_maritime == null)
            {
                _statusLabel.Text = "Maritime session unavailable.";
                return;
            }

            var dive = _maritime.Dive;
            var diveState = dive.CaptureState();

            // ── Dive Telemetry Card ──
            var telCard = AshfallUiHelpers.MakePanel();
            var telMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            telCard.AddChild(telMargin);

            var tBox = new VBoxContainer();
            tBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            telMargin.AddChild(tBox);

            tBox.AddChild(AshfallUiHelpers.MakeSectionHeader(dive.IsActive ? "STEALTH DIVE OPERATION // IN PROGRESS" : "STEALTH DIVE OPERATION // SURFACE STANDBY"));

            string diverName = !string.IsNullOrEmpty(dive.DiverDwellerId) ? dive.DiverDwellerId.Replace("survivor_", "").ToUpperInvariant() : "UNASSIGNED";
            string opName = !string.IsNullOrEmpty(dive.CompressorOperatorDwellerId) ? dive.CompressorOperatorDwellerId.Replace("survivor_", "").ToUpperInvariant() : "UNASSIGNED";

            float airPercent = dive.MaxAirSupplySeconds > 0 ? (dive.AirSupplySeconds / dive.MaxAirSupplySeconds) * 100f : 0f;

            tBox.AddChild(AshfallUiHelpers.MakeDataRow("Assigned Diver", diverName, AshfallUiHelpers.ToColor(CoreTheme.Hot)));
            tBox.AddChild(AshfallUiHelpers.MakeDataRow("Compressor Operator", opName, AshfallUiHelpers.ToColor(CoreTheme.Warm)));
            tBox.AddChild(AshfallUiHelpers.MakeDataRow("Air Supply Reserve", $"{dive.AirSupplySeconds:F0}s / {dive.MaxAirSupplySeconds:F0}s ({airPercent:F0}%)", AshfallUiHelpers.ToColor(airPercent < 25f ? CoreTheme.Critical : CoreTheme.Pale)));
            tBox.AddChild(AshfallUiHelpers.MakeDataRow("Acoustic Noise Level", $"{dive.NoiseLevel} / 100", AshfallUiHelpers.ToColor(dive.NoiseLevel > 70 ? CoreTheme.Critical : CoreTheme.Pale)));
            tBox.AddChild(AshfallUiHelpers.MakeDataRow("Submerged Chamber", $"Room {dive.CurrentRoomIndex + 1} of 4", AshfallUiHelpers.ToColor(CoreTheme.Pale)));

            var diveActions = new HBoxContainer();
            diveActions.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            tBox.AddChild(diveActions);

            if (!dive.IsActive)
            {
                var btnStart = AshfallUiHelpers.MakeButton("LAUNCH STEALTH DIVE (SARAH CHEN / MARCUS REID)", () =>
                {
                    _maritime.StartDiveDemo("survivor_sarah_chen", "survivor_marcus_reid");
                    _statusLabel.Text = "Stealth dive launched into Flotilla wreckage.";
                    RefreshView();
                });
                btnStart.CustomMinimumSize = new Vector2(380, 36);
                diveActions.AddChild(btnStart);
            }
            else
            {
                var btnCrank = AshfallUiHelpers.MakeButton("CRANK COMPRESSOR (+30s Air)", () =>
                {
                    _maritime.CrankDiveDemo();
                    _statusLabel.Text = "Compressor cranked manually.";
                    RefreshView();
                });
                btnCrank.CustomMinimumSize = new Vector2(240, 36);
                diveActions.AddChild(btnCrank);

                var btnAdvance = AshfallUiHelpers.MakeButton("ADVANCE TO NEXT ROOM", () =>
                {
                    _maritime.AdvanceDiveDemo(10);
                    RefreshView();
                });
                btnAdvance.CustomMinimumSize = new Vector2(220, 36);
                diveActions.AddChild(btnAdvance);

                var btnTick = AshfallUiHelpers.MakeButton("SIMULATE DIVE (+30s)", () =>
                {
                    _maritime.TickDiveDemo(30f);
                    _statusLabel.Text = "Advanced dive time by 30 seconds.";
                    RefreshView();
                });
                btnTick.CustomMinimumSize = new Vector2(200, 36);
                diveActions.AddChild(btnTick);

                var btnAbort = AshfallUiHelpers.MakeButton("ABORT / SURFACE", () =>
                {
                    _maritime.Dive.EndDive(true);
                    _statusLabel.Text = "Diver surfaced safely.";
                    RefreshView();
                });
                btnAbort.CustomMinimumSize = new Vector2(160, 36);
                diveActions.AddChild(btnAbort);
            }

            _diveDetailsContainer.AddChild(telCard);

            // ── Procedural Scavenge Loot Nodes ──
            var lootCard = AshfallUiHelpers.MakePanel();
            var lootMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            lootCard.AddChild(lootMargin);

            var lBox = new VBoxContainer();
            lBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            lootMargin.AddChild(lBox);

            lBox.AddChild(AshfallUiHelpers.MakeDataRow("Wreck Compartment A", "Submerged Electronics & Copper Cable", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            lBox.AddChild(AshfallUiHelpers.MakeDataRow("Wreck Compartment B", "Sealed Medical Cache & Antibiotics", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            lBox.AddChild(AshfallUiHelpers.MakeDataRow("Reactor Room C", "Heavy Lead Sheeting & Turbine Scrap", AshfallUiHelpers.ToColor(CoreTheme.Warm)));
            lBox.AddChild(AshfallUiHelpers.MakeDataRow("Command Deck D", "Classified Naval Encrypted Log", AshfallUiHelpers.ToColor(CoreTheme.Hot)));

            var btnLoot = AshfallUiHelpers.MakeButton("SALVAGE WRECK LOOT NODE (-15s Air)", () =>
            {
                if (dive.IsActive)
                {
                    _maritime.TickDiveDemo(15f);
                    _statusLabel.Text = "Loot node recovered: +2 Electronic Scrap, +1 Lead Sheeting.";
                }
                else
                {
                    _statusLabel.Text = "Cannot scavenge while on surface standby.";
                }
                RefreshView();
            });
            btnLoot.CustomMinimumSize = new Vector2(300, 36);
            lBox.AddChild(btnLoot);

            _lootDetailsContainer.AddChild(lootCard);
        }

        private static void ClearContainer(VBoxContainer container)
        {
            if (container == null) return;
            while (container.GetChildCount() > 0)
            {
                var child = container.GetChild(0);
                container.RemoveChild(child);
                child.QueueFree();
            }
        }
    }
}
