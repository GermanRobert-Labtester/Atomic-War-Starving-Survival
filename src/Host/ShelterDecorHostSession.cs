using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host bridge for the shelter decor authority. It owns no
    /// duplicate decor or morale state: item modifiers come from the already
    /// loaded ItemCatalog, placements live in ShelterDecorSystem, room
    /// occupancy comes from ShelterAssignmentSystem, and morale still belongs
    /// to NeedsSystem.
    /// </summary>
    public sealed class ShelterDecorHostSession : HostSessionBase
    {
        public const string MemorialWallRoomId = "room_memorial_wall";

        private readonly ShelterAssignmentSystem _assignments;
        private readonly NeedsSystem _needs;
        private readonly InventoryHostSession _inventory;

        public ShelterDecorSystem System { get; }
        public ShelterAssignmentSystem Assignments => _assignments;
        public NeedsSystem Needs => _needs;
        public ItemCatalog InventoryCatalog => _inventory.Catalog;
        public InventoryContainer Inventory => _inventory.Inventory;
        public string LastEvent { get; private set; } = string.Empty;
        public int CatalogModifierCount { get; private set; }
        public int LastMoraleRecipientCount { get; private set; }
        public float LastMoraleGranted { get; private set; }
        /// <summary>
        /// Current campaign day supplied by Main at setup and daily advance.
        /// It is command context only, not a second persistence authority.
        /// </summary>
        public int CurrentDay { get; private set; }

        public ShelterDecorHostSession(
            ShelterDecorSystem system,
            ShelterAssignmentSystem assignments,
            NeedsSystem needs,
            InventoryHostSession inventory)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            _assignments = assignments ?? throw new ArgumentNullException(nameof(assignments));
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));

            System.OnStateChanged += OnSystemStateChanged;
            _inventory.StateChanged += OnInventoryStateChanged;
        }

        /// <summary>
        /// Registers every authoritative item_decor_* entry from the live item
        /// catalog. No JSON is parsed a second time and modifiers never enter a
        /// save payload.
        /// </summary>
        public int LoadCatalogModifiers()
        {
            int registered = 0;
            foreach (string id in _inventory.Catalog.Ids)
            {
                if (!id.StartsWith("item_decor_", StringComparison.Ordinal))
                    continue;
                var definition = _inventory.Catalog.Get(id);
                if (definition == null) continue;
                System.RegisterItemModifier(new ShelterDecorItemModifier
                {
                    ItemId = definition.id,
                    LocalizedMoraleDelta = definition.decorLocalizedMoraleDelta,
                    Category = CategoryFor(definition.id)
                });
                registered++;
            }

            CatalogModifierCount = registered;
            LastEvent = registered == 0
                ? "No shelter decor items were registered from the item catalog."
                : $"{registered} shelter decor items registered from items.json.";
            RequestPresentationRefresh();
            return registered;
        }

        public IReadOnlyList<ShelterRoom> Rooms => _assignments.Rooms;

        public void SetCurrentDay(int day)
        {
            CurrentDay = Math.Max(0, day);
        }

        public void SetPanelMessage(string message)
        {
            LastEvent = message ?? string.Empty;
            RequestPresentationRefresh();
        }

        public string DisplayNameForRoom(string roomId)
        {
            if (string.Equals(roomId, MemorialWallRoomId, StringComparison.Ordinal))
                return "Memorial Wall";
            for (int i = 0; i < _assignments.Rooms.Count; i++)
            {
                var room = _assignments.Rooms[i];
                if (string.Equals(room.RoomId, roomId, StringComparison.Ordinal))
                    return room.DisplayName;
            }
            return roomId ?? string.Empty;
        }

        public List<ItemDefinition> ListAvailableDecor()
        {
            var result = new List<ItemDefinition>();
            foreach (var pair in System.ItemModifiers)
            {
                var definition = _inventory.Catalog.Get(pair.Key);
                if (definition != null)
                    result.Add(definition);
            }
            result.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.Ordinal));
            return result;
        }

        /// <summary>
        /// Mounts a real decor item. The item is consumed from inventory only
        /// after all placement validation succeeds; replacement must be a
        /// separate remove action so an occupied slot can never silently lose
        /// its item.
        /// </summary>
        public bool TryMount(string roomId, string slotId, string itemId, int day, out string reason)
        {
            reason = string.Empty;
            if (!IsMountableRoom(roomId))
            {
                reason = "Choose an existing shelter room.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(slotId))
            {
                reason = "Name the wall, peg, or shelf slot before mounting an item.";
                return false;
            }
            if (System.GetSlot(roomId, slotId) != null)
            {
                reason = "That slot is occupied. Return its item to storage before mounting another.";
                return false;
            }
            if (System.GetItemModifier(itemId) == null)
            {
                reason = "That item is not registered as shelter decor.";
                return false;
            }
            var definition = _inventory.Catalog.Get(itemId);
            if (definition == null || _inventory.Inventory.CountById(itemId) < 1)
            {
                reason = "The selected decor item is not in Holdfast storage.";
                return false;
            }
            if (!_inventory.Inventory.TryConsume(itemId, 1))
            {
                reason = "Storage could not release the selected item.";
                return false;
            }
            if (!System.Assign(roomId, slotId.Trim(), itemId, day))
            {
                _inventory.Inventory.TryProduce(itemId, 1, definition);
                reason = "The decor registry rejected that placement; the item was returned to storage.";
                return false;
            }

            LastEvent = $"Mounted {definition.displayName} at {DisplayNameForRoom(roomId)} / {slotId.Trim()}.";
            reason = LastEvent;
            return true;
        }

        /// <summary>
        /// Returns a player-mounted item to storage. Memorial plaques are
        /// records generated by MemorialSystem and stay on the wall.
        /// </summary>
        public bool TryRemoveMount(string roomId, string slotId, out string reason)
        {
            reason = string.Empty;
            var placement = System.GetSlot(roomId, slotId);
            if (placement == null)
            {
                reason = "There is no mounted item at that slot.";
                return false;
            }
            if (placement.IsMemorialPlaque)
            {
                reason = "Memorial plaques are ledger records and cannot be removed from this panel.";
                return false;
            }
            var definition = _inventory.Catalog.Get(placement.ItemId);
            if (definition == null || !_inventory.Inventory.CanAdd(definition, 1))
            {
                reason = "Storage has no safe capacity to receive that item.";
                return false;
            }
            if (!_inventory.Inventory.Add(definition, 1) || !System.Remove(roomId, slotId))
            {
                reason = "The item could not be returned to storage.";
                return false;
            }

            LastEvent = $"Returned {definition.displayName} to Holdfast storage.";
            reason = LastEvent;
            return true;
        }

        /// <summary>
        /// One-way MemorialSystem bridge. A newly committed ledger entry is
        /// represented as an idempotent plaque at the dedicated wall; no fake
        /// inventory item is minted or consumed.
        /// </summary>
        public bool TryMountMemorialPlaque(MemorialEntry entry, out string reason)
        {
            reason = string.Empty;
            if (entry == null || string.IsNullOrEmpty(entry.SurvivorId))
            {
                reason = "Memorial entry has no survivor id.";
                return false;
            }
            string slotId = "plaque_" + entry.SurvivorId;
            var existing = System.GetSlot(MemorialWallRoomId, slotId);
            if (existing != null && existing.IsMemorialPlaque
                && string.Equals(existing.MemorialSurvivorId, entry.SurvivorId, StringComparison.Ordinal))
            {
                reason = "The memorial wall already carries this survivor's plaque.";
                return true;
            }
            var placement = System.ResolvePlaqueSlot(
                entry.SurvivorId,
                entry.HeirloomItemId,
                MemorialWallRoomId,
                slotId,
                entry.Day);
            if (placement == null)
            {
                reason = "The catalog has no registered memorial plaque item.";
                return false;
            }
            if (!System.Assign(
                    placement.RoomId,
                    placement.SlotId,
                    placement.ItemId,
                    placement.DayInstalled,
                    placement.IsMemorialPlaque,
                    placement.MemorialSurvivorId,
                    placement.PlaqueSourceHeirloomId))
            {
                reason = "The memorial plaque could not be registered.";
                return false;
            }

            LastEvent = $"Memorial plaque mounted for {entry.SurvivorId}.";
            reason = LastEvent;
            return true;
        }

        /// <summary>
        /// Applies additive decor morale through the single existing needs
        /// authority. Only alive survivors with an active room assignment are
        /// recipients; the memorial wall has no assignment and thus grants no
        /// passive morale by itself.
        /// </summary>
        public int ApplyDailyMorale(int day)
        {
            SetCurrentDay(day);
            LastMoraleRecipientCount = 0;
            LastMoraleGranted = 0f;
            var assignments = _assignments.GetAssignments();
            for (int i = 0; i < assignments.Count; i++)
            {
                var assignment = assignments[i];
                if (assignment == null || assignment.Status != ShelterAssignmentStatus.Active)
                    continue;
                float delta = System.GetRoomMoraleDelta(assignment.RoomId);
                if (Math.Abs(delta) < 0.0001f) continue;
                var survivor = _needs.Get(assignment.SurvivorId);
                if (survivor == null || !survivor.IsAliveState) continue;
                _needs.Modify(assignment.SurvivorId, NeedKind.Morale, delta);
                LastMoraleRecipientCount++;
                LastMoraleGranted += delta;
            }

            if (LastMoraleRecipientCount > 0)
            {
                LastEvent = $"Room decor granted {LastMoraleGranted:F1} morale across {LastMoraleRecipientCount} assigned survivor(s).";
                RequestPresentationRefresh();
            }
            return LastMoraleRecipientCount;
        }

        protected override void UnsubscribeSystemEvents()
        {
            System.OnStateChanged -= OnSystemStateChanged;
            _inventory.StateChanged -= OnInventoryStateChanged;
        }

        private bool IsMountableRoom(string roomId)
        {
            if (string.Equals(roomId, MemorialWallRoomId, StringComparison.Ordinal))
                return false;
            for (int i = 0; i < _assignments.Rooms.Count; i++)
                if (string.Equals(_assignments.Rooms[i].RoomId, roomId, StringComparison.Ordinal))
                    return true;
            return false;
        }

        private static string CategoryFor(string itemId)
        {
            if (itemId.IndexOf("plaque", StringComparison.Ordinal) >= 0) return "memorial plaque";
            if (itemId.IndexOf("poster", StringComparison.Ordinal) >= 0) return "poster";
            if (itemId.IndexOf("drawing", StringComparison.Ordinal) >= 0) return "drawing";
            return "keepsake";
        }

        private void OnSystemStateChanged() => RaiseStateChanged();
        private void OnInventoryStateChanged() => RequestPresentationRefresh();
    }
}
