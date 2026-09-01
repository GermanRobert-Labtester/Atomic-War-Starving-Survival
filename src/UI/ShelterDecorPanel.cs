using System;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Live room-interior panel for Plan 12C. Every action routes through
    /// ShelterDecorHostSession: mounting consumes a real inventory item,
    /// removal returns it to storage, and memorial plaques are read-only
    /// projections of the memorial ledger.
    /// </summary>
    public partial class ShelterDecorPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail _statusRail = null!;
        private OptionButton _roomPicker = null!;
        private LineEdit _slotInput = null!;
        private VBoxContainer _placements = null!;
        private VBoxContainer _storage = null!;
        private Label _roomSummary = null!;
        private Label _selectionSummary = null!;
        private Label _eventLine = null!;
        private string _selectedItemId = string.Empty;
        private ShelterDecorHostSession? _host;

        /// <summary>Rendered count exposed to the headless self-test.</summary>
        public int RenderedPlacementCount { get; private set; }
        public bool IsBound => _host != null;

        /// <summary>
        /// Opens the panel through the same lightweight surface used by the
        /// player route and the visual snapshot harness.
        /// </summary>
        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Bind(ShelterDecorHostSession session)
        {
            if (ReferenceEquals(_host, session)) return;
            Unbind();
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
                _host.PresentationRefreshRequested += RefreshView;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_host == null) return;
            _host.StateChanged -= RefreshView;
            _host.PresentationRefreshRequested -= RefreshView;
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Shelter Interior // Memorial Wall", minWidth: 1160, minHeight: 700);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("mounted", "Mounted Pieces", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("rooms", "Decorated Rooms", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("morale", "Daily Room Morale", "+0.0", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("plaques", "Memorial Plaques", "0", AshfallMetricCard.Criticality.Normal, minWidth: 145);

            var root = AshfallUiHelpers.MakeVBox(12);
            root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            root.SizeFlagsVertical = SizeFlags.ExpandFill;

            var mountPanel = AshfallUiHelpers.MakePanel();
            var mountStack = AshfallUiHelpers.MakeVBox(8);
            mountPanel.AddChild(mountStack);
            mountStack.AddChild(AshfallUiHelpers.MakeSectionHeader("Mount from Holdfast storage"));

            var mountRow = AshfallUiHelpers.MakeHBox(8);
            mountStack.AddChild(mountRow);
            mountRow.AddChild(AshfallUiHelpers.MakeLabel("ROOM"));
            _roomPicker = new OptionButton { CustomMinimumSize = new Vector2(245, 36) };
            _roomPicker.ItemSelected += _ => RefreshView();
            mountRow.AddChild(_roomPicker);
            mountRow.AddChild(AshfallUiHelpers.MakeLabel("SLOT"));
            _slotInput = new LineEdit
            {
                PlaceholderText = "north_wall / shelf_1 / entry_hook",
                CustomMinimumSize = new Vector2(260, 36),
                TooltipText = "A named wall, shelf, peg, or surface inside the selected room."
            };
            if (AshfallUiHelpers.FontShareTechMono != null)
                _slotInput.AddThemeFontOverride("font", AshfallUiHelpers.FontShareTechMono);
            mountRow.AddChild(_slotInput);
            var mountButton = AshfallUiHelpers.MakeButton("MOUNT SELECTED", MountSelected);
            mountButton.CustomMinimumSize = new Vector2(165, 36);
            mountRow.AddChild(mountButton);
            _selectionSummary = AshfallUiHelpers.MakeMetadata("Select an item from storage below. Mounting removes one real item from storage.");
            mountStack.AddChild(_selectionSummary);
            root.AddChild(mountPanel);

            var columns = AshfallUiHelpers.MakeHBox(12);
            columns.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            columns.SizeFlagsVertical = SizeFlags.ExpandFill;
            root.AddChild(columns);

            var installedPanel = AshfallUiHelpers.MakePanel();
            installedPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            installedPanel.SizeFlagsStretchRatio = 1.15f;
            var installedStack = AshfallUiHelpers.MakeVBox(8);
            installedPanel.AddChild(installedStack);
            installedStack.AddChild(AshfallUiHelpers.MakeSectionHeader("Installed in selected room"));
            _roomSummary = AshfallUiHelpers.MakeMetadata("No room selected.");
            installedStack.AddChild(_roomSummary);
            var placementScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _placements = AshfallUiHelpers.MakeVBox(8);
            _placements.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            placementScroll.AddChild(_placements);
            installedStack.AddChild(placementScroll);
            columns.AddChild(installedPanel);

            var storagePanel = AshfallUiHelpers.MakePanel();
            storagePanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            storagePanel.SizeFlagsStretchRatio = 1f;
            var storageStack = AshfallUiHelpers.MakeVBox(8);
            storagePanel.AddChild(storageStack);
            storageStack.AddChild(AshfallUiHelpers.MakeSectionHeader("Decor available in storage"));
            storageStack.AddChild(AshfallUiHelpers.MakeMetadata("Choose an item, then name a free slot. Zero-count entries remain visible so the catalog is legible."));
            var storageScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _storage = AshfallUiHelpers.MakeVBox(7);
            _storage.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            storageScroll.AddChild(_storage);
            storageStack.AddChild(storageScroll);
            columns.AddChild(storagePanel);

            _eventLine = AshfallUiHelpers.MakeInfo("The wall is quiet. Nothing has been mounted in this session.");
            root.AddChild(_eventLine);
            _shell.SetContent(root);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (_shell == null || _statusRail == null) return;
            if (_host == null)
            {
                _statusRail.Set("mounted", "UNBOUND", AshfallMetricCard.Criticality.Caution);
                _statusRail.Set("rooms", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("morale", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("plaques", "—", AshfallMetricCard.Criticality.Normal);
                if (_eventLine != null) _eventLine.Text = "Shelter decor is waiting for the campaign session.";
                return;
            }

            RebuildRoomPicker();
            string roomId = SelectedRoomId();
            var placements = _host.System.ListRoomPlacements(roomId);
            RenderedPlacementCount = placements.Count;
            int decoratedRooms = 0;
            int plaques = 0;
            float cumulativeMorale = 0f;
            foreach (var room in _host.Rooms)
            {
                float delta = _host.System.GetRoomMoraleDelta(room.RoomId);
                if (_host.System.ListRoomPlacements(room.RoomId).Count > 0) decoratedRooms++;
                cumulativeMorale += delta * ActiveOccupantCount(room.RoomId);
            }
            foreach (var placement in _host.System.State.Placements)
                if (placement != null && placement.IsMemorialPlaque) plaques++;

            _statusRail.Set("mounted", _host.System.State.Placements.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("rooms", decoratedRooms.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("morale", $"+{cumulativeMorale:F1}", cumulativeMorale > 0f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("plaques", plaques.ToString(), plaques > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            if (_roomSummary != null)
            {
                float roomDelta = _host.System.GetRoomMoraleDelta(roomId);
                int occupants = ActiveOccupantCount(roomId);
                _roomSummary.Text = string.Equals(roomId, ShelterDecorHostSession.MemorialWallRoomId, StringComparison.Ordinal)
                    ? "Ledger-backed plaques are permanent records. They do not consume storage and have no assigned occupants."
                    : $"{_host.DisplayNameForRoom(roomId)} · {occupants} active assigned occupant(s) · +{roomDelta:F1} morale per occupant at daily needs tick.";
            }
            if (_selectionSummary != null)
            {
                if (string.IsNullOrEmpty(_selectedItemId))
                    _selectionSummary.Text = "Select an item from storage. Mounting removes one real item; removing a player-mounted item returns it.";
                else
                {
                    var selected = _host.InventoryCatalog.Get(_selectedItemId);
                    var modifier = _host.System.GetItemModifier(_selectedItemId);
                    int held = _host.Inventory.CountById(_selectedItemId);
                    _selectionSummary.Text = selected == null || modifier == null
                        ? "The selected item is no longer available."
                        : $"SELECTED · {selected.displayName} · {held} in storage · +{modifier.LocalizedMoraleDelta:F1} morale / assigned occupant / day.";
                }
            }
            if (_eventLine != null) _eventLine.Text = _host.LastEvent;

            RebuildPlacements(roomId, placements);
            RebuildStorage();
        }

        /// <summary>
        /// Selects a room by canonical id. Used by the headless UI gate and by
        /// future room-hotspot callers; it changes only panel selection.
        /// </summary>
        public bool SelectRoom(string roomId)
        {
            if (_roomPicker == null || string.IsNullOrEmpty(roomId)) return false;
            for (int i = 0; i < _roomPicker.ItemCount; i++)
            {
                if (!string.Equals(_roomPicker.GetItemMetadata(i).AsString(), roomId, StringComparison.Ordinal))
                    continue;
                _roomPicker.Select(i);
                RefreshView();
                return true;
            }
            return false;
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        private void MountSelected()
        {
            if (_host == null) return;
            if (string.IsNullOrEmpty(_selectedItemId))
            {
                _host.SetPanelMessage("Choose a decor item from storage before mounting.");
                RefreshView();
                return;
            }
            _host.TryMount(SelectedRoomId(), _slotInput?.Text ?? string.Empty, _selectedItemId, _host.CurrentDay, out _);
            RefreshView();
        }

        private void RebuildRoomPicker()
        {
            if (_host == null || _roomPicker == null) return;
            string current = SelectedRoomId();
            if (string.IsNullOrEmpty(current))
            {
                // First open should lead with a lived-in room rather than an
                // arbitrary empty corridor. Players can still choose every
                // room or the wall from the selector.
                foreach (var room in _host.Rooms)
                {
                    if (_host.System.ListRoomPlacements(room.RoomId).Count <= 0) continue;
                    current = room.RoomId;
                    break;
                }
                if (string.IsNullOrEmpty(current)
                    && _host.System.ListRoomPlacements(ShelterDecorHostSession.MemorialWallRoomId).Count > 0)
                {
                    current = ShelterDecorHostSession.MemorialWallRoomId;
                }
            }
            _roomPicker.Clear();
            foreach (var room in _host.Rooms)
            {
                _roomPicker.AddItem(room.DisplayName);
                _roomPicker.SetItemMetadata(_roomPicker.ItemCount - 1, room.RoomId);
            }
            _roomPicker.AddItem("Memorial Wall");
            _roomPicker.SetItemMetadata(_roomPicker.ItemCount - 1, ShelterDecorHostSession.MemorialWallRoomId);
            for (int i = 0; i < _roomPicker.ItemCount; i++)
            {
                if (string.Equals(_roomPicker.GetItemMetadata(i).AsString(), current, StringComparison.Ordinal))
                {
                    _roomPicker.Select(i);
                    return;
                }
            }
            if (_roomPicker.ItemCount > 0) _roomPicker.Select(0);
        }

        private string SelectedRoomId()
        {
            if (_roomPicker == null || _roomPicker.Selected < 0 || _roomPicker.Selected >= _roomPicker.ItemCount)
                return string.Empty;
            return _roomPicker.GetItemMetadata(_roomPicker.Selected).AsString();
        }

        private void RebuildPlacements(string roomId, System.Collections.Generic.List<ShelterDecorPlacement> placements)
        {
            ClearChildren(_placements);
            if (_host == null || placements.Count == 0)
            {
                _placements.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No decor is mounted here yet.",
                    "BARE SURFACE",
                    string.Equals(roomId, ShelterDecorHostSession.MemorialWallRoomId, StringComparison.Ordinal)
                        ? "Memorial entries place their plaques here automatically."
                        : "Choose a storage item, name a slot, and mount it."));
                return;
            }
            foreach (var placement in placements)
            {
                var card = AshfallUiHelpers.MakePanel();
                var stack = AshfallUiHelpers.MakeVBox(5);
                card.AddChild(stack);
                var definition = _host.InventoryCatalog.Get(placement.ItemId);
                var modifier = _host.System.GetItemModifier(placement.ItemId);
                stack.AddChild(AshfallUiHelpers.MakeLabel(
                    $"{placement.SlotId.ToUpperInvariant()}  //  {(definition?.displayName ?? placement.ItemId).ToUpperInvariant()}",
                    18, bold: true));
                stack.AddChild(AshfallUiHelpers.MakeMetadata($"+{modifier?.LocalizedMoraleDelta ?? 0f:F1} morale per assigned occupant / day · mounted day {placement.DayInstalled}"));
                if (placement.IsMemorialPlaque)
                {
                    stack.AddChild(AshfallUiHelpers.MakeInfo($"Memorial record · {placement.MemorialSurvivorId} · heirloom: {placement.PlaqueSourceHeirloomId}"));
                }
                else
                {
                    string slot = placement.SlotId;
                    var remove = AshfallUiHelpers.MakeButton("RETURN TO STORAGE", () =>
                    {
                        _host.TryRemoveMount(roomId, slot, out _);
                        RefreshView();
                    });
                    remove.CustomMinimumSize = new Vector2(180, 30);
                    stack.AddChild(remove);
                }
                _placements.AddChild(card);
            }
        }

        private void RebuildStorage()
        {
            ClearChildren(_storage);
            if (_host == null) return;
            var options = _host.ListAvailableDecor();
            if (options.Count == 0)
            {
                _storage.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "The item catalog did not register any item_decor_* entries.",
                    "NO DECOR AUTHORITY"));
                return;
            }
            foreach (var definition in options)
            {
                string itemId = definition.id;
                var modifier = _host.System.GetItemModifier(itemId);
                int count = _host.Inventory.CountById(itemId);
                var choose = AshfallUiHelpers.MakeButton(
                    $"{definition.displayName.ToUpperInvariant()}  ·  {count} HELD  ·  +{modifier?.LocalizedMoraleDelta ?? 0f:F1}",
                    () =>
                    {
                        _selectedItemId = itemId;
                        RefreshView();
                    });
                choose.TooltipText = definition.description;
                choose.Disabled = count <= 0;
                choose.Modulate = string.Equals(itemId, _selectedItemId, StringComparison.Ordinal)
                    ? AshfallUiHelpers.ColorHighlight
                    : Colors.White;
                _storage.AddChild(choose);
            }
        }

        private int ActiveOccupantCount(string roomId)
        {
            if (_host == null) return 0;
            int count = 0;
            foreach (var assignment in _host.Assignments.GetAssignmentsForRoom(roomId))
            {
                if (assignment.Status == ShelterAssignmentStatus.Active
                    && _host.Needs.Get(assignment.SurvivorId)?.IsAliveState == true)
                    count++;
            }
            return count;
        }

        private static void ClearChildren(Node container)
        {
            if (container == null) return;
            foreach (Node child in container.GetChildren())
            {
                container.RemoveChild(child);
                child.QueueFree();
            }
        }
    }
}
