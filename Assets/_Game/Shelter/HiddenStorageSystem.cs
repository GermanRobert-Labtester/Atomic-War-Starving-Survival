using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #121 — Hidden storage: FalseWall/FloorSafe, raid-proof, 2h retrieval.</summary>
    public class HiddenStorageSystem
    {
        public const string FalseWallItemId = "false_wall_kit";
        public const string FloorSafeItemId = "floor_safe_kit";
        public const float RetrieveHours = 2f;
        private readonly Dictionary<string, int> _hiddenStacks = new Dictionary<string, int>(); // itemId -> count
        public int HiddenItemCount { get { int t = 0; foreach (var kv in _hiddenStacks) t += kv.Value; return t; } }
        public bool HasHidden(string itemId) => _hiddenStacks.TryGetValue(itemId, out int c) && c > 0;
        public event Action OnHiddenStashChanged;

        public void HideItem(string itemId, int amount)
        {
            if (amount <= 0) return;
            _hiddenStacks.TryGetValue(itemId, out int cur);
            _hiddenStacks[itemId] = cur + amount;
            OnHiddenStashChanged?.Invoke();
        }
        public int RetrieveItem(string itemId, int amount)
        {
            if (!HasHidden(itemId) || amount <= 0) return 0;
            int taken = Math.Min(amount, _hiddenStacks[itemId]);
            _hiddenStacks[itemId] -= taken;
            if (_hiddenStacks[itemId] <= 0) _hiddenStacks.Remove(itemId);
            OnHiddenStashChanged?.Invoke();
            return taken;
        }
        public HiddenStorageSave CaptureState()
        {
            var keys = new string[_hiddenStacks.Count]; var vals = new int[_hiddenStacks.Count]; int i = 0;
            foreach (var kv in _hiddenStacks) { keys[i] = kv.Key; vals[i] = kv.Value; i++; }
            return new HiddenStorageSave { ItemIds = keys, Amounts = vals };
        }
        public void RestoreState(HiddenStorageSave save)
        {
            _hiddenStacks.Clear();
            if (save?.ItemIds == null) return;
            for (int i = 0; i < save.ItemIds.Length; i++)
                if (!string.IsNullOrEmpty(save.ItemIds[i]) && save.Amounts != null && i < save.Amounts.Length && save.Amounts[i] > 0)
                    _hiddenStacks[save.ItemIds[i]] = save.Amounts[i];
        }
    }
    [Serializable] public class HiddenStorageSave { public string[] ItemIds; public int[] Amounts; }
}
