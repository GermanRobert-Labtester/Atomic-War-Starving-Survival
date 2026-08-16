using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Duty Roster panel.
    /// Manages live survivor shift assignments across canonical Holdfast roles.
    /// Directly connects to DutyRosterHostSession and SurvivorsHostSession.
    /// </summary>
    public partial class DutyRosterPanel : Control
    {
        public event Action? OnClose;
        public event Action? OnAssignmentChanged;

        private DutyRosterHostSession? _rosterHost;
        private SurvivorsHostSession? _survivorsHost;
        private VBoxContainer _rolesContainer = null!;

        private readonly (string RoleId, string Title, string Description)[] _shiftRoles =
        {
            (DutyRosterSystem.RoleIntakeSleeper, "INTAKE FILTRATION", "Maintains HEPA filtration stack; reduces daily filter wear by 50%."),
            (DutyRosterSystem.RoleNightWatch, "NIGHT WATCH", "Perimeter sensor vigilance and early fallout storm warnings."),
            (DutyRosterSystem.RoleMess, "MESS & RATIONS", "Ration efficiency and kitchen stores preservation."),
            (DutyRosterSystem.RoleHatchOpener, "HATCH DEFENSE", "Airlock decontamination and security protocols."),
            (DutyRosterSystem.RoleExpedition, "SCAVENGING SORTIE", "Sortie equipment maintenance and wasteland readiness.")
        };

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "[UNASSIGNED]";
            return id switch
            {
                "survivor_sarah_chen" => "Dr. Sarah Chen",
                "survivor_mikhail_volkov" => "Gunner Mikhail",
                "survivor_elena_vasquez" => "Elena Vasquez",
                _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
            };
        }

        public void Bind(DutyRosterHostSession rosterHost, SurvivorsHostSession survivorsHost)
        {
            _rosterHost = rosterHost;
            _survivorsHost = survivorsHost;
            EnsureOccupantsEnrolled();
            RefreshView();
        }

        private void EnsureOccupantsEnrolled()
        {
            if (_rosterHost == null || _survivorsHost == null) return;

            for (int i = 0; i < _survivorsHost.RosterState.Count; i++)
            {
                var s = _survivorsHost.RosterState[i];
                if (s == null) continue;
                _rosterHost.Roster.WriteName(
                    s.Id,
                    FormatSurvivorName(s.Id),
                    "Resident",
                    DutyRosterSystem.ScriptPencil,
                    1,
                    true
                );
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            rootBox.CustomMinimumSize = new Vector2(720, 0);
            center.AddChild(rootBox);

            var header = AshfallUiHelpers.MakeTitle("DUTY ROSTER // WORK-SHIFT SCHEDULE", Ashfall.Core.UI.Theme.FontSizeH1);
            header.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(header);

            var subHeader = AshfallUiHelpers.MakeMetadata("Assign shelter residents to active shifts. Intake filtration reduces air degradation by 50%.");
            subHeader.HorizontalAlignment = HorizontalAlignment.Center;
            subHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(subHeader);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _rolesContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_rolesContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO DASHBOARD [Esc]", () => OnClose?.Invoke(), true);
            btnClose.CustomMinimumSize = new Vector2(240, 42);
            btnClose.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
            rootBox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("Press [Esc] to return");
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(hint);
        }

        public void RefreshView()
        {
            if (_rolesContainer == null || _rosterHost == null || _survivorsHost == null) return;

            while (_rolesContainer.GetChildCount() > 0)
            {
                var child = _rolesContainer.GetChild(0);
                _rolesContainer.RemoveChild(child);
                child.QueueFree();
            }

            var aliveSurvivors = new List<(string Id, string Name)>();
            for (int i = 0; i < _survivorsHost.RosterState.Count; i++)
            {
                var s = _survivorsHost.RosterState[i];
                if (s != null && s.IsAliveState)
                {
                    aliveSurvivors.Add((s.Id, FormatSurvivorName(s.Id)));
                }
            }

            foreach (var (roleId, title, description) in _shiftRoles)
            {
                var roleCard = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                var headerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                var lblRole = AshfallUiHelpers.MakeSectionHeader(title);
                lblRole.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                headerRow.AddChild(lblRole);

                string currentAssigneeId = _rosterHost.Roster.GetAssignment(roleId);
                string currentAssigneeName = "[UNASSIGNED]";
                if (!string.IsNullOrEmpty(currentAssigneeId))
                {
                    var found = aliveSurvivors.Find(s => s.Id == currentAssigneeId);
                    currentAssigneeName = found.Name ?? FormatSurvivorName(currentAssigneeId);
                }

                var lblAssignee = AshfallUiHelpers.MakeMono($"ASSIGNED: {currentAssigneeName}");
                lblAssignee.AddThemeColorOverride(
                    "font_color",
                    string.IsNullOrEmpty(currentAssigneeId)
                        ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)
                        : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)
                );
                headerRow.AddChild(lblAssignee);

                // Assign/Cycle Button
                string currentIdCapture = currentAssigneeId;
                string roleIdCapture = roleId;

                var btnCycle = AshfallUiHelpers.MakeButton("CYCLE SHIFT", () =>
                {
                    if (aliveSurvivors.Count == 0) return;
                    int currentIndex = aliveSurvivors.FindIndex(s => s.Id == currentIdCapture);
                    int nextIndex = (currentIndex + 1) % (aliveSurvivors.Count + 1);
                    if (nextIndex >= aliveSurvivors.Count)
                    {
                        _rosterHost.Roster.Assign(roleIdCapture, null!);
                    }
                    else
                    {
                        _rosterHost.Roster.Assign(roleIdCapture, aliveSurvivors[nextIndex].Id);
                    }
                    OnAssignmentChanged?.Invoke();
                    RefreshView();
                });
                btnCycle.CustomMinimumSize = new Vector2(130, 32);
                headerRow.AddChild(btnCycle);

                var btnClear = AshfallUiHelpers.MakeButton("CLEAR", () =>
                {
                    _rosterHost.Roster.Assign(roleIdCapture, null!);
                    OnAssignmentChanged?.Invoke();
                    RefreshView();
                });
                btnClear.CustomMinimumSize = new Vector2(80, 32);
                headerRow.AddChild(btnClear);

                roleCard.AddChild(headerRow);

                var lblDesc = AshfallUiHelpers.MakeBody(description);
                lblDesc.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                roleCard.AddChild(lblDesc);

                var panel = AshfallUiHelpers.MakePanel();
                panel.AddChild(roleCard);
                _rolesContainer.AddChild(panel);
            }
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
