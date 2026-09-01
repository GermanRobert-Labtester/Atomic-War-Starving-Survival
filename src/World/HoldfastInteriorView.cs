using AtomicWar.GodotApp.UI;
using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp.World
{
    /// <summary>
    /// Configuration data for a room in the Holdfast shelter interior layout.
    /// </summary>
    public class InteriorRoomDefinition
    {
        public string RoomId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public Vector2 BasePosition { get; set; } = Vector2.Zero;
        public Vector2 HotspotPosition { get; set; } = Vector2.Zero;
        public string StatusSummary { get; set; } = string.Empty;
        public int Capacity { get; set; } = 4;

        public InteriorRoomDefinition() { }

        public InteriorRoomDefinition(
            string roomId,
            string displayName,
            Vector2 basePosition,
            Vector2 hotspotPosition,
            string statusSummary = "",
            int capacity = 4)
        {
            RoomId = roomId;
            DisplayName = displayName;
            BasePosition = basePosition;
            HotspotPosition = hotspotPosition;
            StatusSummary = statusSummary;
            Capacity = capacity;
        }
    }

    /// <summary>
    /// ASHFALL — 2D Holdfast Shelter Interior View.
    /// Fully data-driven survivor placement and room hotspot management.
    /// Receives live room layouts, positions, and assignments from <see cref="ShelterAssignmentHostSession"/>
    /// and <see cref="DutyRosterHostSession"/>, with fallback handling for unknown rooms.
    /// </summary>
    public partial class HoldfastInteriorView : Node2D
    {
        [Signal]
        public delegate void RoomSelectedEventHandler(string roomId);

        public const string DefaultFallbackRoomId = "room_bunks";
        public static readonly Vector2 DefaultFallbackPosition = new Vector2(510, 290);
        public static readonly Vector2 DefaultFallbackHotspot = new Vector2(510, 240);

        private SurvivorsHostSession? _survivors;
        private DutyRosterHostSession? _dutyRoster;
        private ShelterAssignmentHostSession? _shelterAssignments;
        private Func<string, string>? _customAssignmentResolver;
        private Ashfall.Core.Shelter.ShelterRoomIdentityCatalog? _roomIdentities;
        private Ashfall.Core.Shelter.ShelterMachineTellCatalog? _machineTellCatalog;

        private readonly Dictionary<string, InteriorRoomDefinition> _roomDefinitions = new(StringComparer.Ordinal);
        private readonly List<SurvivorActorView> _survivorActors = new();
        private readonly Dictionary<string, RoomHotspotView> _roomHotspots = new(StringComparer.Ordinal);
        private CanvasModulate? _lightingModulate;

        public IReadOnlyDictionary<string, InteriorRoomDefinition> RoomDefinitions => _roomDefinitions;
        public IReadOnlyList<SurvivorActorView> SurvivorActors => _survivorActors;

        public HoldfastInteriorView()
        {
            if (!HasNode("SurvivorActors"))
                AddChild(new Node2D { Name = "SurvivorActors" });
            if (!HasNode("RoomHotspots"))
                AddChild(new Node2D { Name = "RoomHotspots" });

            _lightingModulate = new CanvasModulate
            {
                Name = "InteriorLighting",
                Color = new Color(0.95f, 0.95f, 0.95f, 1.0f)
            };
            AddChild(_lightingModulate);

            InitializeDefaultRooms();
        }

        private void InitializeDefaultRooms()
        {
            _roomDefinitions.Clear();
            var defaults = new[]
            {
                new InteriorRoomDefinition("room_storage_bay", "Storage Bay", new Vector2(160, 290), new Vector2(160, 240), "Tool & Supply Depot", 4),
                new InteriorRoomDefinition("room_bunker_corridor", "Central Corridor", new Vector2(340, 290), new Vector2(340, 240), "Access Concourse", 0),
                new InteriorRoomDefinition("room_bunks", "Bunk Living", new Vector2(510, 290), new Vector2(510, 240), "Living Quarters: Warmth 100%", 6),
                new InteriorRoomDefinition("room_filtration", "Filtration Stack", new Vector2(680, 290), new Vector2(680, 240), "Filtration Stack: Active · Attenuation: 99%", 2),
                new InteriorRoomDefinition("room_airlock", "Airlock Hatch", new Vector2(860, 290), new Vector2(860, 240), "Airlock: Sealed · Outer Decon Ready", 2),
                new InteriorRoomDefinition("room_kitchen", "Galley Kitchen", new Vector2(420, 290), new Vector2(420, 240), "Ration Prep Operational", 2),
                new InteriorRoomDefinition("room_clinic", "Medical Ward", new Vector2(595, 290), new Vector2(595, 240), "Triage Bay Ready", 2),
                new InteriorRoomDefinition("room_workshop", "Workshop", new Vector2(250, 290), new Vector2(250, 240), "Fabrication Bench Ready", 2)
            };

            foreach (var r in defaults)
            {
                _roomDefinitions[r.RoomId] = r;
            }
        }

        public void Initialize(
            SurvivorsHostSession survivors,
            DutyRosterHostSession? dutyRoster = null,
            ShelterAssignmentHostSession? shelterAssignments = null)
        {
            _survivors = survivors;
            _dutyRoster = dutyRoster;
            _shelterAssignments = shelterAssignments;

            SyncRoomsFromLiveState();
            ClearExistingSurvivors();
            PopulateRoomHotspots();
            PopulateSurvivors();
            UpdateSurvivorPositions();
        }

        public void Bind(
            SurvivorsHostSession survivors,
            DutyRosterHostSession? dutyRoster = null,
            ShelterAssignmentHostSession? shelterAssignments = null)
        {
            Initialize(survivors, dutyRoster, shelterAssignments);
        }

        public void SetCustomAssignmentResolver(Func<string, string>? resolver)
        {
            _customAssignmentResolver = resolver;
            UpdateSurvivorPositions();
        }

        /// <summary>
        /// Plan 29 Task 29A: bind the room identity overlay (data-driven,
        /// HoldfastFlavorCatalog pattern — missing catalog or unknown room falls
        /// back to the neutral status line, never blocks the view). Tooltip-only
        /// surfacing: lore never buries the live status line (§29A.6).
        /// </summary>
        public void SetRoomIdentityCatalog(Ashfall.Core.Shelter.ShelterRoomIdentityCatalog? catalog)
        {
            _roomIdentities = catalog;
            UpdateSurvivorPositions();
        }

        /// <summary>
        /// Plan 29 Task 29B: bind the machine identity catalog so room tooltips
        /// can surface machine names for rooms that host machines.
        /// </summary>
        public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog)
        {
            _machineTellCatalog = catalog;
        }

        public void ConfigureRooms(IEnumerable<InteriorRoomDefinition> rooms)
        {
            if (rooms == null) return;
            foreach (var room in rooms)
            {
                if (room != null && !string.IsNullOrEmpty(room.RoomId))
                {
                    _roomDefinitions[room.RoomId] = room;
                }
            }
            PopulateRoomHotspots();
            UpdateSurvivorPositions();
        }

        public void SetRoomPosition(string roomId, Vector2 basePosition, Vector2? hotspotPosition = null)
        {
            if (string.IsNullOrEmpty(roomId)) return;

            if (_roomDefinitions.TryGetValue(roomId, out var def))
            {
                def.BasePosition = basePosition;
                if (hotspotPosition.HasValue)
                    def.HotspotPosition = hotspotPosition.Value;
            }
            else
            {
                _roomDefinitions[roomId] = new InteriorRoomDefinition(
                    roomId,
                    FormatRoomDisplayName(roomId),
                    basePosition,
                    hotspotPosition ?? (basePosition + new Vector2(0, -50)));
            }

            PopulateRoomHotspots();
            UpdateSurvivorPositions();
        }

        public void SetRoomPositions(IDictionary<string, Vector2> positions)
        {
            if (positions == null) return;
            foreach (var kvp in positions)
            {
                SetRoomPosition(kvp.Key, kvp.Value);
            }
        }

        public void SetLightingPhase(string phase)
        {
            if (_lightingModulate == null) return;

            _lightingModulate.Color = phase.ToLowerInvariant() switch
            {
                "morning" => new Color(1.0f, 0.96f, 0.88f, 1.0f),
                "midday" => new Color(1.0f, 1.0f, 1.0f, 1.0f),
                "evening" => new Color(0.85f, 0.82f, 0.88f, 1.0f),
                "night" => new Color(0.55f, 0.58f, 0.70f, 1.0f),
                "emergency" => new Color(0.95f, 0.40f, 0.35f, 1.0f),
                _ => new Color(0.95f, 0.95f, 0.95f, 1.0f)
            };
        }

        private void SyncRoomsFromLiveState()
        {
            if (_shelterAssignments?.System != null)
            {
                foreach (var r in _shelterAssignments.System.Rooms)
                {
                    if (r == null || string.IsNullOrEmpty(r.RoomId)) continue;
                    if (_roomDefinitions.TryGetValue(r.RoomId, out var existing))
                    {
                        existing.DisplayName = r.DisplayName;
                        existing.Capacity = r.Capacity;
                    }
                    else
                    {
                        // Generate fallback slot for new dynamic room
                        _roomDefinitions[r.RoomId] = new InteriorRoomDefinition(
                            r.RoomId,
                            r.DisplayName,
                            DefaultFallbackPosition,
                            DefaultFallbackHotspot,
                            $"{r.DisplayName} Active",
                            r.Capacity);
                    }
                }
            }
        }

        public void UpdateSurvivorPositions()
        {
            if (_survivors == null)
                return;

            var roomCounts = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var key in _roomDefinitions.Keys)
            {
                roomCounts[key] = 0;
            }

            foreach (var actor in _survivorActors)
            {
                UpdateSurvivorActor(actor);
                string assignedRoom = GetAssignedRoomForSurvivor(actor.SurvivorId);

                if (!roomCounts.ContainsKey(assignedRoom))
                {
                    roomCounts[assignedRoom] = 0;
                }

                int occupantIndex = roomCounts[assignedRoom]++;
                int xOffset = occupantIndex * 35;
                Vector2 basePos = GetRoomBasePosition(assignedRoom);
                actor.Position = basePos + new Vector2(xOffset, 25);
            }

            // Update hotspot occupant counters and live status
            foreach (var kvp in _roomHotspots)
            {
                int count = roomCounts.TryGetValue(kvp.Key, out int c) ? c : 0;
                string status = GetRoomStatusSummary(kvp.Key);
                kvp.Value.SetRoomInfo(GetRoomDisplayName(kvp.Key), status, count);
            }
        }

        public string GetAssignedRoomForSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return DefaultFallbackRoomId;

            // 1. Custom assignment override
            if (_customAssignmentResolver != null)
            {
                string customRoom = _customAssignmentResolver(survivorId);
                if (!string.IsNullOrEmpty(customRoom))
                    return customRoom;
            }

            // 2. Authoritative live ShelterAssignmentSystem state
            if (_shelterAssignments?.System != null)
            {
                var assignment = _shelterAssignments.System.GetAssignmentForSurvivor(survivorId);
                if (assignment != null && !string.IsNullOrEmpty(assignment.RoomId))
                {
                    return assignment.RoomId;
                }
            }

            // 3. Authoritative live DutyRosterSystem role mapping
            if (_dutyRoster?.Roster != null)
            {
                var role = _dutyRoster.Roster.GetRoleOf(survivorId);
                if (!string.IsNullOrEmpty(role))
                {
                    if (role == DutyRosterIds.RoleIntakeSleeper)
                        return "room_filtration";
                    if (role == DutyRosterIds.RoleNightWatch || role == DutyRosterIds.RoleHatchOpener)
                        return "room_airlock";
                    if (role == DutyRosterIds.RoleMess)
                        return "room_kitchen";
                    if (role == DutyRosterIds.RoleExpedition)
                        return "room_storage_bay";
                }
            }

            // 4. Default fallback room
            return DefaultFallbackRoomId;
        }

        public Vector2 GetRoomBasePosition(string roomId)
        {
            if (!string.IsNullOrEmpty(roomId) && _roomDefinitions.TryGetValue(roomId, out var def))
            {
                return def.BasePosition;
            }
            return DefaultFallbackPosition;
        }

        public string GetRoomDisplayName(string roomId)
        {
            if (!string.IsNullOrEmpty(roomId) && _roomDefinitions.TryGetValue(roomId, out var def) && !string.IsNullOrEmpty(def.DisplayName))
            {
                return def.DisplayName;
            }
            return FormatRoomDisplayName(roomId);
        }

        public string GetRoomStatusSummary(string roomId)
        {
            string status;
            if (!string.IsNullOrEmpty(roomId) && _roomDefinitions.TryGetValue(roomId, out var def) && !string.IsNullOrEmpty(def.StatusSummary))
            {
                status = def.StatusSummary;
            }
            else
            {
                status = "Shelter Area";
            }
            return AppendRoomIdentity(status, roomId);
        }

        /// <summary>Append the identity overlay (former use, one-line history, ambient fixtures) to a room's tooltip status.</summary>
        private string AppendRoomIdentity(string status, string roomId)
        {
            var identity = _roomIdentities?.GetRoomIdentity(roomId);
            if (identity == null) return status;
            var sb = new System.Text.StringBuilder(status);
            if (!string.IsNullOrEmpty(identity.former_use))
                sb.Append("\nFormerly: ").Append(identity.former_use);
            if (!string.IsNullOrEmpty(identity.one_line_history))
                sb.Append('\n').Append(identity.one_line_history);

            // Fixture details are ambient inspection texture (Plan 29 §29A.10-29A.12):
            // a short visible pool, capped so lore never buries the status line, and
            // never presented as clickable actions because there is no fixture action.
            var fixtures = _roomIdentities!.GetFixturesForRoom(roomId);
            int shown = 0;
            for (int i = 0; i < fixtures.Count && shown < 3; i++)
            {
                if (!fixtures[i].art_visible || string.IsNullOrWhiteSpace(fixtures[i].detail)) continue;
                sb.Append(shown == 0 ? "\nNotable: " : "; ").Append(fixtures[i].detail);
                shown++;
            }

            // Plan 29 Task 29B: machine identities for rooms that host machines.
            if (_machineTellCatalog != null && _machineTellCatalog.MachineCount > 0)
            {
                var machines = new System.Collections.Generic.List<string>();
                for (int i = 0; i < _machineTellCatalog.MachineCount; i++)
                {
                    var m = _machineTellCatalog.Machines[i];
                    if (string.Equals(m.room_id, roomId, StringComparison.Ordinal))
                        machines.Add(m.display_name);
                }
                if (machines.Count > 0)
                {
                    sb.Append("\nMachines: ").Append(string.Join(", ", machines));
                }
            }
            return sb.ToString();
        }

        private static string FormatRoomDisplayName(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return "Shelter Area";
            string name = roomId.StartsWith("room_") ? roomId.Substring(5) : roomId;
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.Replace('_', ' '));
        }

        private void ClearExistingSurvivors()
        {
            var survivorActorsNode = GetNode<Node2D>("SurvivorActors");
            foreach (var actor in _survivorActors)
            {
                if (actor == null || !GodotObject.IsInstanceValid(actor))
                    continue;

                if (actor.GetParent() == survivorActorsNode)
                    survivorActorsNode.RemoveChild(actor);
                actor.QueueFree();
            }
            _survivorActors.Clear();
        }

        private void PopulateSurvivors()
        {
            if (_survivors == null)
                return;

            var survivorActorsNode = GetNode<Node2D>("SurvivorActors");

            int maxSurvivors = Math.Min(_survivors.RosterState.Count, 12);
            for (int i = 0; i < maxSurvivors; i++)
            {
                var survivorState = _survivors.RosterState[i];
                if (survivorState == null || string.IsNullOrEmpty(survivorState.Id) || !survivorState.IsAliveState)
                    continue;

                var actor = new SurvivorActorView();
                actor.SurvivorId = survivorState.Id;
                actor.Label.Text = FormatSurvivorName(survivorState.Id);
                survivorActorsNode.AddChild(actor);
                _survivorActors.Add(actor);
            }
        }

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
        }

        private void PopulateRoomHotspots()
        {
            var roomHotspotsNode = GetNode<Node2D>("RoomHotspots");
            AshfallUiHelpers.EmptyChildren(roomHotspotsNode);
            _roomHotspots.Clear();

            foreach (var def in _roomDefinitions.Values)
            {
                var hotspot = new RoomHotspotView();
                hotspot.RoomId = def.RoomId;
                hotspot.Position = def.HotspotPosition;
                hotspot.SetRoomInfo(def.DisplayName, GetRoomStatusSummary(def.RoomId), 0);
                hotspot.Connect(RoomHotspotView.SignalName.Clicked, Callable.From<string>(OnRoomClicked));
                roomHotspotsNode.AddChild(hotspot);
                _roomHotspots[def.RoomId] = hotspot;
            }
        }

        private void UpdateSurvivorActor(SurvivorActorView actor)
        {
            if (_survivors == null || string.IsNullOrEmpty(actor.SurvivorId))
                return;

            var survivorState = _survivors.Find(actor.SurvivorId);
            if (survivorState == null || !survivorState.IsAliveState)
            {
                actor.Visible = false;
                return;
            }

            var rad = _survivors.RadStateFor(actor.SurvivorId);
            actor.Visible = true;
            actor.UpdateFromSurvivor(survivorState, rad);
        }

        private void OnRoomClicked(string roomId)
        {
            EmitSignal(SignalName.RoomSelected, roomId);
            GD.Print($"[Ashfall Godot][World] Room clicked: {roomId}");
        }

        public override void _ExitTree()
        {
            ClearExistingSurvivors();
            base._ExitTree();
        }
    }
}
