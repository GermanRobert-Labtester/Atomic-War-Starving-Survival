using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Standing Record Panel (Expansion 03).
    /// Interactive exploration console for 14 authoritative ground layouts,
    /// room hierarchies, site stencils, and 38 memory strata mutations.
    ///
    /// Presentation only — queries LocationLayoutSystem for authoritative state.
    /// </summary>
    public partial class StandingRecordPanel : Control
    {
        public event Action? OnClose;

        private LocationLayoutSystem? _layoutSystem;
        private ItemList _locationsList = null!;
        private VBoxContainer _roomDetailsContainer = null!;
        private Label _statusLabel = null!;
        private readonly List<string> _parentLocationIds = new List<string>();
        private string _selectedParentId = LocationLayoutSystem.LocKilometre19;

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

        public void Bind(LocationLayoutSystem? layoutSystem)
        {
            _layoutSystem = layoutSystem;
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
                Color = new Color(0.04f, 0.05f, 0.06f, 0.95f)
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

            // ── Header ──
            var headerCard = AshfallUiHelpers.MakeCardFrame(
                "THE STANDING RECORD // EXP 03 GROUND LAYOUTS & STRATA",
                "Fourteen architectural ground layouts, room hierarchies, site stencils, and thirty-eight memory strata mutations across the Ashfall wasteland."
            );
            mainVBox.AddChild(headerCard);

            // ── Body Columns ──
            var hsplit = new HBoxContainer();
            hsplit.SizeFlagsVertical = SizeFlags.ExpandFill;
            hsplit.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            mainVBox.AddChild(hsplit);

            // Left Column: Location Selection List
            var leftCard = AshfallUiHelpers.MakePanel();
            leftCard.CustomMinimumSize = new Vector2(360, 0);
            leftCard.SizeFlagsVertical = SizeFlags.ExpandFill;
            hsplit.AddChild(leftCard);

            var leftMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            leftCard.AddChild(leftMargin);

            var leftBox = new VBoxContainer();
            leftBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            leftMargin.AddChild(leftBox);

            leftBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVEYED GROUND SITES (14)"));

            _locationsList = new ItemList
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SelectMode = ItemList.SelectModeEnum.Single
            };
            _locationsList.ItemSelected += OnLocationSelected;
            leftBox.AddChild(_locationsList);

            // Right Column: Room Hierarchy & Inspector
            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            hsplit.AddChild(scroll);

            var contentBox = new VBoxContainer();
            contentBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            _roomDetailsContainer = new VBoxContainer();
            _roomDetailsContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_roomDetailsContainer);

            // ── Bottom Action Bar ──
            var bottomBar = new HBoxContainer();
            bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            mainVBox.AddChild(bottomBar);

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
            btnClose.CustomMinimumSize = new Vector2(240, 44);
            bottomBar.AddChild(btnClose);

            _statusLabel = AshfallUiHelpers.MakeMono("Select a surveyed ground layout to inspect room access and strata.");
            _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomBar.AddChild(_statusLabel);
        }

        private void OnLocationSelected(long index)
        {
            if (index >= 0 && index < _parentLocationIds.Count)
            {
                _selectedParentId = _parentLocationIds[(int)index];
                _layoutSystem?.ArriveAtParent(_selectedParentId);
                RefreshRoomDetails();
            }
        }

        public void RefreshView()
        {
            _parentLocationIds.Clear();
            _locationsList.Clear();

            if (_layoutSystem == null)
            {
                _statusLabel.Text = "Standing Record system unavailable.";
                return;
            }

            var layouts = _layoutSystem.Layouts;
            if (layouts == null || layouts.Count == 0)
            {
                // Fallback canonical defaults if not loaded via directory
                _parentLocationIds.Add(LocationLayoutSystem.LocKilometre19);
                _parentLocationIds.Add(LocationLayoutSystem.LocTransitHq);
                _locationsList.AddItem("Kilometre 19 Cut (loc_cut_kilometre_19)");
                _locationsList.AddItem("Transit Authority HQ (loc_transit_authority_hq)");
            }
            else
            {
                for (int i = 0; i < layouts.Count; i++)
                {
                    var l = layouts[i];
                    _parentLocationIds.Add(l.parentLocationId);
                    string name = string.IsNullOrEmpty(l.displayName) ? l.parentLocationId : l.displayName;
                    _locationsList.AddItem($"{name} ({l.RoomCount} Rooms)");
                }
            }

            if (_parentLocationIds.Count > 0)
            {
                if (string.IsNullOrEmpty(_selectedParentId) || !_parentLocationIds.Contains(_selectedParentId))
                    _selectedParentId = _parentLocationIds[0];

                int idx = _parentLocationIds.IndexOf(_selectedParentId);
                if (idx >= 0) _locationsList.Select(idx);
                _layoutSystem.Unlock();
                _layoutSystem.ArriveAtParent(_selectedParentId);
            }

            RefreshRoomDetails();
        }

        private void RefreshRoomDetails()
        {
            ClearContainer(_roomDetailsContainer);
            if (_layoutSystem == null || string.IsNullOrEmpty(_selectedParentId)) return;

            var layout = _layoutSystem.GetLayout(_selectedParentId);
            if (layout == null)
            {
                var card = AshfallUiHelpers.MakePanel();
                var cardMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
                card.AddChild(cardMargin);

                var vbox = new VBoxContainer();
                cardMargin.AddChild(vbox);
                vbox.AddChild(AshfallUiHelpers.MakeSectionHeader($"LAYOUT: {_selectedParentId}"));
                vbox.AddChild(AshfallUiHelpers.MakeDataRow("Survey Status", "Architectural schematic loaded into active memory", AshfallUiHelpers.ToColor(CoreTheme.Warm)));
                _roomDetailsContainer.AddChild(card);
                return;
            }

            // Header card for layout
            var header = AshfallUiHelpers.MakePanel();
            var hMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            header.AddChild(hMargin);

            var hBox = new VBoxContainer();
            hBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingXs);
            hMargin.AddChild(hBox);

            hBox.AddChild(AshfallUiHelpers.MakeSectionHeader(layout.displayName.ToUpperInvariant()));
            hBox.AddChild(AshfallUiHelpers.MakeDataRow("Parent Location", layout.parentLocationId, AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            hBox.AddChild(AshfallUiHelpers.MakeDataRow("Room Count", $"{layout.RoomCount} Hierarchical Chambers", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            hBox.AddChild(AshfallUiHelpers.MakeDataRow("Air / Rad Rating", "Hazard Class A · Ambient Radiation Active", AshfallUiHelpers.ToColor(CoreTheme.Hot)));
            _roomDetailsContainer.AddChild(header);

            _roomDetailsContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ARCHITECTURAL CHAMBERS & ACCESS HIERARCHY"));

            if (layout.rooms != null)
            {
                for (int i = 0; i < layout.rooms.Length; i++)
                {
                    var r = layout.rooms[i];
                    if (r == null) continue;
                    bool canEnter = _layoutSystem.CanEnter(_selectedParentId, r.id);
                    bool isInspected = _layoutSystem.HasInspected(_selectedParentId, r.id);

                    var roomCard = AshfallUiHelpers.MakePanel();
                    var roomMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
                    roomCard.AddChild(roomMargin);

                    var rBox = new VBoxContainer();
                    rBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
                    roomMargin.AddChild(rBox);

                    var top = new HBoxContainer();
                    top.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
                    rBox.AddChild(top);

                    var lblName = AshfallUiHelpers.MakeSectionHeader($"{i + 1}. {r.displayName} ({r.id})");
                    lblName.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(canEnter ? CoreTheme.Hot : CoreTheme.Pale));
                    lblName.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    top.AddChild(lblName);

                    string badgeText = isInspected ? "[INSPECTED]" : (canEnter ? "[ACCESSIBLE]" : "[LOCKED]");
                    var badgeColor = isInspected ? CoreTheme.Pale : (canEnter ? CoreTheme.Hot : CoreTheme.Dim);
                    var badge = AshfallUiHelpers.MakeSmall(badgeText);
                    badge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(badgeColor));
                    top.AddChild(badge);

                    var lblDesc = AshfallUiHelpers.MakeBody(r.description ?? string.Empty);
                    lblDesc.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
                    rBox.AddChild(lblDesc);

                    rBox.AddChild(AshfallUiHelpers.MakeDataRow("Access Rule", r.unlockRule ?? "Open", AshfallUiHelpers.ToColor(CoreTheme.Pale)));

                    var actionsRow = new HBoxContainer();
                    actionsRow.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
                    rBox.AddChild(actionsRow);

                    if (canEnter)
                    {
                        var btnEnter = AshfallUiHelpers.MakeButton("ENTER CHAMBER", () =>
                        {
                            _layoutSystem.EnterRoom(r.id);
                            _statusLabel.Text = $"Entered chamber {r.displayName}.";
                            RefreshRoomDetails();
                        });
                        btnEnter.CustomMinimumSize = new Vector2(160, 32);
                        actionsRow.AddChild(btnEnter);

                        if (!isInspected)
                        {
                            var btnInspect = AshfallUiHelpers.MakeButton("INSPECT CHAMBER & UNLOCK NEIGHBOURS", () =>
                            {
                                _layoutSystem.InspectRoom(r.id);
                                _statusLabel.Text = $"Inspected {r.displayName}. Adjacent chambers unlocked.";
                                RefreshRoomDetails();
                            });
                            btnInspect.CustomMinimumSize = new Vector2(280, 32);
                            actionsRow.AddChild(btnInspect);
                        }
                    }
                    else
                    {
                        var lblLocked = AshfallUiHelpers.MakeMono($"Locked: Requires {r.unlockRule}");
                        lblLocked.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
                        actionsRow.AddChild(lblLocked);
                    }

                    _roomDetailsContainer.AddChild(roomCard);
                }
            }
        }

        private static void ClearContainer(VBoxContainer container)
        {
            AshfallUiHelpers.EmptyChildren(container);
        }

        public override void _ExitTree()
        {
            if (_locationsList != null)
            {
                _locationsList.ItemSelected -= OnLocationSelected;
            }
            base._ExitTree();
        }
    }
}
