using System;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Personal Keepsake — tracks loss of a survivor's designated keepsake item
    /// and grief decay over time.
    /// </summary>
    public class PersonalKeepsakeSystem
    {
        public const float GriefDecayPerDay = 0.02f;

        public event Action<Survivor, string> OnKeepsakeLost;

        public void OnInventoryItemRemoved(Survivor survivor, string itemId)
        {
            if (survivor == null || string.IsNullOrEmpty(itemId)) return;
            if (survivor.HasLostKeepsake) return;
            if (string.IsNullOrEmpty(survivor.PersonalKeepsakeItemId)) return;
            if (!string.Equals(survivor.PersonalKeepsakeItemId, itemId,
                    StringComparison.Ordinal)) return;

            survivor.HasLostKeepsake = true;
            survivor.KeepsakeGriefLevel = 1f;
            OnKeepsakeLost?.Invoke(survivor, itemId);
        }

        public void TickGriefDecay(Survivor survivor, float gameHours)
        {
            if (survivor == null || !survivor.HasLostKeepsake) return;
            if (survivor.KeepsakeGriefLevel <= 0f) return;
            float decay = GriefDecayPerDay * (gameHours / 24f);
            survivor.KeepsakeGriefLevel = Math.Max(0f, survivor.KeepsakeGriefLevel - decay);
        }
    }
}
