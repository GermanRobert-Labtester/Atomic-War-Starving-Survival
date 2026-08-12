using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public enum BunkerRoomStatus
    {
        Operational,
        Damaged,
        Critical,
        Offline
    }

    [Serializable]
    public class BunkerRoomData
    {
        public string RoomId;
        public string RoomCode = "R-01";
        public string RoomName = "GENERATOR BAY";
        public BunkerRoomStatus Status = BunkerRoomStatus.Operational;
        public float IntegrityPercent = 100f;
    }

    public class BunkerFloorMapMiniature : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private List<BunkerRoomData> _rooms = new List<BunkerRoomData>();

        private VisualElement _root;
        private VisualElement _gridContainer;
        private VisualElement _tooltipBox;
        private Label _tooltipText;

        public event Action<BunkerRoomData> OnRoomClicked;
        public event Action<List<BunkerRoomData>> OnStateChanged;

        public IReadOnlyList<BunkerRoomData> Rooms => _rooms;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("bunker-map-root") 
                      ?? _document.rootVisualElement.Q("bunker_map_root");
                Bind();
                InitializeDefaultRoomsIfEmpty();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _gridContainer = _root.Q<VisualElement>("bunker_grid_container");
            _tooltipBox = _root.Q<VisualElement>("bunker_tooltip_box");
            _tooltipText = _root.Q<Label>("bunker_tooltip_text");
        }

        private void InitializeDefaultRoomsIfEmpty()
        {
            if (_rooms.Count == 0)
            {
                string[] defaultNames = new string[]
                {
                    "GENERATOR", "WATER PURIFIER", "HYDROPONICS", "MED BAY",
                    "ARMORY", "COMMUNICATIONS", "AIR SCRUBBER", "QUARTERS",
                    "WORKBENCH", "STORAGE A", "STORAGE B", "HATCH ENTRY"
                };

                for (int i = 0; i < 12; i++)
                {
                    _rooms.Add(new BunkerRoomData
                    {
                        RoomId = $"room_{i + 1:D2}",
                        RoomCode = $"R-{i + 1:D2}",
                        RoomName = defaultNames[i],
                        Status = (BunkerRoomStatus)(i % 4),
                        IntegrityPercent = 100f - (i * 7f)
                    });
                }
            }
        }

        public void SetRooms(List<BunkerRoomData> rooms)
        {
            _rooms = rooms ?? new List<BunkerRoomData>();
            RefreshUI();
            OnStateChanged?.Invoke(_rooms);
        }

        public void UpdateRoomStatus(string roomId, BunkerRoomStatus status, float integrity)
        {
            var room = _rooms.Find(r => r.RoomId == roomId || r.RoomCode == roomId);
            if (room != null)
            {
                room.Status = status;
                room.IntegrityPercent = Mathf.Clamp(integrity, 0f, 100f);
                RefreshUI();
                OnStateChanged?.Invoke(_rooms);
            }
        }

        private void RefreshUI()
        {
            if (_gridContainer == null) return;

            _gridContainer.Clear();

            foreach (var room in _rooms)
            {
                VisualElement cell = new VisualElement();
                cell.AddToClassList("bunker-room-cell");

                switch (room.Status)
                {
                    case BunkerRoomStatus.Operational: cell.AddToClassList("status-operational"); break;
                    case BunkerRoomStatus.Damaged: cell.AddToClassList("status-damaged"); break;
                    case BunkerRoomStatus.Critical: cell.AddToClassList("status-critical"); break;
                    case BunkerRoomStatus.Offline: cell.AddToClassList("status-offline"); break;
                }

                Label codeLabel = new Label(room.RoomCode);
                codeLabel.AddToClassList("room-cell-code");
                cell.Add(codeLabel);

                var rData = room;
                cell.RegisterCallback<PointerEnterEvent>(evt => ShowTooltip(rData));
                cell.RegisterCallback<PointerLeaveEvent>(evt => HideTooltip());
                cell.RegisterCallback<ClickEvent>(evt => OnRoomClicked?.Invoke(rData));

                _gridContainer.Add(cell);
            }
        }

        private void ShowTooltip(BunkerRoomData room)
        {
            if (_tooltipBox == null || _tooltipText == null) return;
            _tooltipText.text = $"{room.RoomName} [{room.RoomCode}] // {room.Status.ToString().ToUpper()} ({Mathf.RoundToInt(room.IntegrityPercent)}%)";
            _tooltipBox.RemoveFromClassList("hidden");
        }

        private void HideTooltip()
        {
            _tooltipBox?.AddToClassList("hidden");
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
