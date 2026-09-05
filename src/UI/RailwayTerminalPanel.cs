using System;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class RailwayTerminalPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        private AshfallDashboardShell _shell = null!;
        private RailwaySystem? _system;
        public bool IsBound => _system != null;

        public void Bind(RailwaySystem system) { _system = system; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("TRANSIT CORRIDOR // RAILWAY NETWORK", minWidth: 1000, minHeight: 650);
            AddChild(_shell);
            _shell.SetContent(new Label { Text = "Railway routing terminal..." });
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }
        public void RefreshView() {}
    }
}
