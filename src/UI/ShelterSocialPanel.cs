// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Living Quarters Privacy Pressure & Communal Mess Hall Panel.
    /// Thin presentation layer over ShelterSocialDynamicsSystem.
    /// </summary>
    public partial class ShelterSocialPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        public bool IsBound => _social != null;
        private ShelterSocialDynamicsSystem? _social;
        private SurvivorRelationsSystem? _relations;
        private NeedsSystem? _needs;
        private MemorialSystem? _memorial;
        private SurvivorsHostSession? _survivors;
        private Ashfall.Core.Inventory.Inventory? _inventory;
        private int _currentDay;

        // Plan 211 is a thin read/command projection over the Core owner. The
        // panel never keeps a second message list or decides permissions.
        private InternalCommunicationHostSession? _internalCommunication;
        private string _internalCommunicationActorId = string.Empty;
        private Func<int>? _internalCommunicationCurrentDay;
        private VBoxContainer? _internalCommunicationContainer;
        private VBoxContainer? _internalCommunicationNoticesContainer;
        private Label? _internalCommunicationStatus;
        private Button? _postWaterAdvisoryButton;

        private Button _closeButton = null!;
        private VBoxContainer _roomListContainer = null!;
        private VBoxContainer _survivorListContainer = null!;
        private VBoxContainer _relationsContainer = null!;
        private VBoxContainer _disputeContainer = null!;
        private Button _mediateButton = null!;
        private Button _gatheringButton = null!;
        private VBoxContainer _memorialContainer = null!;
        private VBoxContainer _historyContainer = null!;
        private Label _statusLabel = null!;

        private string _selectedRoomId = "room_bunks_crowded";
        private string _selectedIncidentId = string.Empty;

        public void Bind(
            ShelterSocialDynamicsSystem social,
            SurvivorRelationsSystem? relations = null,
            NeedsSystem? needs = null,
            MemorialSystem? memorial = null,
            SurvivorsHostSession? survivors = null,
            Ashfall.Core.Inventory.Inventory? inventory = null,
            int currentDay = 0)
        {
            // Preserve the communications binding when the existing social
            // route rebinds its other owners. A full Unbind is reserved for
            // panel disposal/lifecycle teardown.
            UnbindSocialOnly();

            _social = social;
            _relations = relations;
            _needs = needs;
            _memorial = memorial;
            _survivors = survivors;
            _inventory = inventory;
            _currentDay = currentDay;

            _social.OnIncidentTriggered += OnIncidentTriggered;
            _social.OnIncidentMediated += OnIncidentMediated;
            _social.OnSocialStateChanged += RefreshView;

            RefreshView();
        }

        /// <summary>
        /// Bind the existing Plan 211 host session to this already-routed
        /// Shelter Social panel. Main supplies the current leader/roster actor;
        /// the host remains the authority for all identity checks.
        /// </summary>
        public void BindCommunications(
            InternalCommunicationHostSession? session,
            string actorId = "",
            Func<int>? currentDayProvider = null)
        {
            if (_internalCommunication != null)
                _internalCommunication.StateChanged -= RefreshView;

            _internalCommunication = session;
            _internalCommunicationActorId = actorId ?? string.Empty;
            _internalCommunicationCurrentDay = currentDayProvider;
            if (_internalCommunication != null)
                _internalCommunication.StateChanged += RefreshView;

            RefreshView();
        }

        public void Unbind()
        {
            UnbindSocialOnly();
            UnbindCommunications();
        }

        private void UnbindSocialOnly()
        {
            if (_social != null)
            {
                _social.OnIncidentTriggered -= OnIncidentTriggered;
                _social.OnIncidentMediated -= OnIncidentMediated;
                _social.OnSocialStateChanged -= RefreshView;
            }
            _social = null;
            _relations = null;
            _needs = null;
            _memorial = null;
            _survivors = null;
            _inventory = null;
        }

        private void UnbindCommunications()
        {
            if (_internalCommunication != null)
                _internalCommunication.StateChanged -= RefreshView;
            _internalCommunication = null;
            _internalCommunicationActorId = string.Empty;
            _internalCommunicationCurrentDay = null;
        }

        public override void _Ready()
        {
            Visible = false;
            var binder = new SceneBinder(this, typeof(ShelterSocialPanel));
            binder.Require<Button>("CloseButton");
            binder.Require<VBoxContainer>("RoomListContainer");
            binder.Require<VBoxContainer>("SurvivorListContainer");
            binder.Require<VBoxContainer>("RelationsContainer");
            binder.Require<VBoxContainer>("DisputeContainer");
            binder.Require<Button>("MediateButton");
            binder.Require<Button>("GatheringButton");
            binder.Require<VBoxContainer>("MemorialContainer");
            binder.Require<VBoxContainer>("HistoryContainer");
            binder.Require<Label>("StatusLabel");

            _closeButton = binder.Get<Button>("CloseButton");
            _roomListContainer = binder.Get<VBoxContainer>("RoomListContainer");
            _survivorListContainer = binder.Get<VBoxContainer>("SurvivorListContainer");
            _relationsContainer = binder.Get<VBoxContainer>("RelationsContainer");
            _disputeContainer = binder.Get<VBoxContainer>("DisputeContainer");
            _mediateButton = binder.Get<Button>("MediateButton");
            _gatheringButton = binder.Get<Button>("GatheringButton");
            _memorialContainer = binder.Get<VBoxContainer>("MemorialContainer");
            _historyContainer = binder.Get<VBoxContainer>("HistoryContainer");
            _statusLabel = binder.Get<Label>("StatusLabel");
            EnsureInternalCommunicationWidgets();

            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            _mediateButton.Pressed += OnMediatePressed;
            _gatheringButton.Pressed += OnGatheringPressed;

            RefreshView();
        }

        public override void _ExitTree() => Unbind();

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_roomListContainer == null || _survivorListContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_roomListContainer);
            AshfallUiHelpers.EmptyChildren(_survivorListContainer);
            AshfallUiHelpers.EmptyChildren(_relationsContainer);
            AshfallUiHelpers.EmptyChildren(_disputeContainer);
            AshfallUiHelpers.EmptyChildren(_memorialContainer);
            AshfallUiHelpers.EmptyChildren(_historyContainer);
            RefreshInternalCommunicationView();

            if (_social == null)
            {
                _statusLabel.Text = "No shelter social session bound.";
                _mediateButton.Disabled = true;
                _gatheringButton.Disabled = true;
                return;
            }

            var state = _social.State;

            // 1. Rooms
            var rooms = new (string id, string name, string desc)[]
            {
                ("room_bunks_crowded", "Crowded Bunks", "High density (+12% fatigue/day)"),
                ("room_quarters_private", "Private Quarters", "Restful sanctuary (-25% fatigue/day)"),
                ("room_common_mess_hall", "Communal Mess Hall", "Gathering & shared meals")
            };

            foreach (var r in rooms)
            {
                int occupants = state.privacyProfiles.Values.Count(p => p.AssignedRoomId == r.id);
                var btn = new Button
                {
                    Text = $"[{r.name}] Occupancy: {occupants} // {r.desc}",
                    Alignment = HorizontalAlignment.Left
                };
                string capturedRoom = r.id;
                if (capturedRoom == _selectedRoomId)
                {
                    btn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                }
                btn.Pressed += () =>
                {
                    _selectedRoomId = capturedRoom;
                    RefreshView();
                };
                _roomListContainer.AddChild(btn);
            }

            // 2. Survivors & Privacy
            var dwellerIds = _survivors?.RosterState?.Select(d => d.Id).ToList()
                             ?? state.privacyProfiles.Keys.ToList();
            if (dwellerIds.Count == 0)
            {
                _survivorListContainer.AddChild(AshfallUiHelpers.MakeMetadata("No survivors registered in shelter."));
            }
            else
            {
                foreach (var sId in dwellerIds)
                {
                    var profile = _social.GetOrCreatePrivacyProfile(sId);
                    float fatiguePct = profile.PrivacyFatiguePermille / 10f;
                    string roomName = rooms.FirstOrDefault(r => r.id == profile.AssignedRoomId).name ?? (!string.IsNullOrEmpty(profile.AssignedRoomId) ? profile.AssignedRoomId : "Unassigned");

                    var row = new HBoxContainer();
                    var label = new Label
                    {
                        Text = $"{sId} | Room: {roomName} | Privacy Fatigue: {fatiguePct:F0}%",
                        SizeFlagsHorizontal = SizeFlags.ExpandFill
                    };
                    if (fatiguePct >= 75f)
                    {
                        label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
                        label.Text += " [FATIGUE WARNING]";
                    }
                    row.AddChild(label);

                    var assignBtn = new Button
                    {
                        Text = $"ASSIGN {_selectedRoomId.Replace("room_", "").ToUpperInvariant()}"
                    };
                    string capSurvivor = sId;
                    assignBtn.Pressed += () =>
                    {
                        _social.RegisterSurvivorRoom(capSurvivor, _selectedRoomId);
                        _statusLabel.Text = $"Assigned {capSurvivor} to {_selectedRoomId}.";
                        RefreshView();
                    };
                    row.AddChild(assignBtn);
                    _survivorListContainer.AddChild(row);
                }
            }

            // 3. Relationships
            if (_relations != null)
            {
                var rels = _relations.State.relationships;
                if (rels.Count == 0)
                {
                    _relationsContainer.AddChild(AshfallUiHelpers.MakeMetadata("No established survivor relationships."));
                }
                else
                {
                    foreach (var rel in rels.Take(8))
                    {
                        _relationsContainer.AddChild(new Label
                        {
                            Text = $"{rel.dwellerA} <-> {rel.dwellerB}: Affinity {rel.affinity:F0} | Trust {rel.trust:F0} | Resentment {rel.resentment:F0}"
                        });
                    }
                }
            }
            else
            {
                _relationsContainer.AddChild(AshfallUiHelpers.MakeMetadata("Relations system offline."));
            }

            // 4. Disputes
            var unresolved = state.recentIncidents.Where(i => !i.Resolved).ToList();
            if (unresolved.Count == 0)
            {
                _disputeContainer.AddChild(AshfallUiHelpers.MakeMetadata("No active interpersonal disputes in shelter."));
                _mediateButton.Disabled = true;
            }
            else
            {
                foreach (var inc in unresolved)
                {
                    _social.Catalog.TryGetValue(inc.EventId, out var eventDef);
                    string title = eventDef != null ? eventDef.DisplayName : inc.EventId;
                    var btn = new Button
                    {
                        Text = $"[DISPUTE] {title} in {inc.RoomId} (Day {inc.Day})",
                        Alignment = HorizontalAlignment.Left
                    };
                    string capturedInc = inc.IncidentId;
                    if (capturedInc == _selectedIncidentId)
                    {
                        btn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    }
                    btn.Pressed += () =>
                    {
                        _selectedIncidentId = capturedInc;
                        RefreshView();
                    };
                    _disputeContainer.AddChild(btn);
                }
                _mediateButton.Disabled = string.IsNullOrEmpty(_selectedIncidentId);
                _mediateButton.Text = "MEDIATE SELECTED DISPUTE";
            }

            // 5. Communal Gathering Button
            _gatheringButton.Disabled = dwellerIds.Count < 2;
            _gatheringButton.Text = $"HOST COMMUNAL GATHERING ({dwellerIds.Count} Attendees)";

            // 6. Memorial
            if (_memorial != null)
            {
                var fallen = _memorial.Entries;
                if (fallen.Count == 0)
                {
                    _memorialContainer.AddChild(AshfallUiHelpers.MakeMetadata("No fallen survivors inscribed on memorial wall."));
                }
                else
                {
                    foreach (var f in fallen)
                    {
                        _memorialContainer.AddChild(new Label
                        {
                            Text = $"† {f.SurvivorId} — Fallen Day {f.Day} ({f.Cause})"
                        });
                    }
                }
            }
            else
            {
                _memorialContainer.AddChild(AshfallUiHelpers.MakeMetadata("Memorial system offline."));
            }

            // 7. History
            var history = state.recentIncidents.TakeLast(6).Reverse().ToList();
            if (history.Count == 0)
            {
                _historyContainer.AddChild(AshfallUiHelpers.MakeMetadata("No recent social incidents logged."));
            }
            else
            {
                foreach (var h in history)
                {
                    string status = h.Resolved ? (h.IsMediated ? "Mediated" : "Resolved") : "Pending";
                    _historyContainer.AddChild(new Label
                    {
                        Text = $"Day {h.Day}: {h.EventId} in {h.RoomId} [{status}]"
                    });
                }
            }
        }

        private void EnsureInternalCommunicationWidgets()
        {
            if (_internalCommunicationContainer != null) return;

            _internalCommunicationContainer = new VBoxContainer
            {
                Name = "InternalCommunicationContainer",
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _internalCommunicationContainer.SetAnchorsPreset(LayoutPreset.BottomWide);
            _internalCommunicationContainer.OffsetLeft = 24;
            _internalCommunicationContainer.OffsetRight = -24;
            _internalCommunicationContainer.OffsetTop = -360;
            _internalCommunicationContainer.OffsetBottom = -20;
            AddChild(_internalCommunicationContainer);

            _internalCommunicationContainer.AddChild(new Label
            {
                Text = "INTERNAL SHELTER COMMUNICATIONS",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            });

            _internalCommunicationStatus = new Label
            {
                Text = "Communication session offline.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _internalCommunicationContainer.AddChild(_internalCommunicationStatus);

            _postWaterAdvisoryButton = new Button { Text = "POST WATER ADVISORY" };
            _postWaterAdvisoryButton.Pressed += OnPostWaterAdvisoryPressed;
            _internalCommunicationContainer.AddChild(_postWaterAdvisoryButton);

            _internalCommunicationNoticesContainer = new VBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _internalCommunicationContainer.AddChild(_internalCommunicationNoticesContainer);
        }

        private void RefreshInternalCommunicationView()
        {
            if (_internalCommunicationContainer == null) return;
            if (_internalCommunicationNoticesContainer != null)
                AshfallUiHelpers.EmptyChildren(_internalCommunicationNoticesContainer);

            if (_internalCommunication == null)
            {
                if (_internalCommunicationStatus != null)
                    _internalCommunicationStatus.Text = "Communication session offline.";
                if (_postWaterAdvisoryButton != null)
                {
                    _postWaterAdvisoryButton.Disabled = true;
                    _postWaterAdvisoryButton.Text = "COMMUNICATIONS OFFLINE";
                }
                return;
            }

            bool canAuthor = _internalCommunication.CanAuthor(_internalCommunicationActorId);
            if (_postWaterAdvisoryButton != null)
            {
                _postWaterAdvisoryButton.Disabled = !canAuthor;
                _postWaterAdvisoryButton.Text = canAuthor
                    ? "POST WATER ADVISORY"
                    : "WATER ADVISORY — LEADER ONLY";
            }

            if (_internalCommunicationStatus != null)
            {
                _internalCommunicationStatus.Text = _internalCommunication.CatalogReady
                    ? (canAuthor
                        ? "Current actor may author public notices. Public notices are visible to the shelter; private mail stays in its recipient inbox."
                        : "Read and acknowledge public notices. Only the current shelter leader may author a notice.")
                    : $"Communication catalog unavailable: {_internalCommunication.CatalogLoadError ?? "invalid authored catalog"}";
            }

            if (!_internalCommunication.CatalogReady)
                return;

            var notices = _internalCommunication.PublicNotices;
            if (notices.Count == 0)
            {
                _internalCommunicationNoticesContainer?.AddChild(
                    AshfallUiHelpers.MakeMetadata("No active public shelter notices."));
                return;
            }

            foreach (var notice in notices.Take(20))
            {
                var card = new VBoxContainer();
                card.AddThemeConstantOverride("separation", 2);
                card.AddChild(new Label
                {
                    Text = $"[{notice.Priority.ToString().ToUpperInvariant()}] {notice.Subject} — day {notice.PostedDay}",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                });
                card.AddChild(new Label
                {
                    Text = notice.Content,
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                });

                string state = $"{(notice.IsRead ? "READ" : "UNREAD")} / {(notice.IsAcknowledged ? "ACKNOWLEDGED" : "UNACKNOWLEDGED")}";
                card.AddChild(new Label { Text = state });

                var actions = new HBoxContainer();
                string actor = _internalCommunicationActorId;
                if (!notice.IsRead)
                {
                    var readButton = AshfallUiHelpers.MakeButton("MARK READ", () =>
                    {
                        var result = _internalCommunication.MarkRead(notice.MessageId, actor);
                        if (_internalCommunicationStatus != null)
                            _internalCommunicationStatus.Text = result.Accepted ? result.Detail : result.ReasonKey;
                    });
                    readButton.Disabled = !_internalCommunication.IsKnownSurvivor(actor);
                    actions.AddChild(readButton);
                }

                if (!notice.IsAcknowledged)
                {
                    var ackButton = AshfallUiHelpers.MakeButton("ACKNOWLEDGE", () =>
                    {
                        var result = _internalCommunication.Acknowledge(notice.MessageId, actor);
                        if (_internalCommunicationStatus != null)
                            _internalCommunicationStatus.Text = result.Accepted ? result.Detail : result.ReasonKey;
                    });
                    ackButton.Disabled = !_internalCommunication.IsKnownSurvivor(actor);
                    actions.AddChild(ackButton);
                }

                card.AddChild(actions);
                _internalCommunicationNoticesContainer?.AddChild(card);
            }
        }

        private void OnPostWaterAdvisoryPressed()
        {
            if (_internalCommunication == null) return;
            int currentDay = _internalCommunicationCurrentDay?.Invoke() ?? _currentDay;
            var result = _internalCommunication.PostWaterAdvisory(_internalCommunicationActorId, currentDay);
            if (_internalCommunicationStatus != null)
            {
                _internalCommunicationStatus.Text = result.Accepted
                    ? $"Posted {result.MessageId}: {result.Detail}"
                    : $"Refused: {result.ReasonKey} — {result.Detail}";
            }
            RefreshView();
        }

        private void OnMediatePressed()
        {
            if (_social == null || string.IsNullOrEmpty(_selectedIncidentId)) return;
            var dwellerIds = _survivors?.RosterState?.Select(d => d.Id).ToList()
                             ?? _social.State.privacyProfiles.Keys.ToList();
            string mediator = dwellerIds.FirstOrDefault() ?? "mediator_chief";

            var result = _social.TryMediateIncident(_selectedIncidentId, mediator);
            _statusLabel.Text = result.IsSuccess ? $"Mediation successful by {mediator}." : $"Mediation failed: {result.FailureCode}";
            _selectedIncidentId = string.Empty;
            RefreshView();
        }

        private void OnGatheringPressed()
        {
            if (_social == null) return;
            var dwellerIds = _survivors?.RosterState?.Select(d => d.Id).ToList()
                             ?? _social.State.privacyProfiles.Keys.ToList();
            var result = _social.TriggerCommunalGathering("room_common_mess_hall", dwellerIds, _currentDay);
            _statusLabel.Text = result.IsSuccess ? "Communal gathering conducted in Mess Hall. Morale and cohesion bolstered." : $"Gathering blocked: {result.FailureCode}";
            RefreshView();
        }

        private void OnIncidentTriggered(SocialIncidentRecord inc) => RefreshView();
        private void OnIncidentMediated(SocialIncidentRecord inc) => RefreshView();
    }
}
