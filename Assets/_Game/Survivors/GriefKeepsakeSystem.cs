using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    public class GriefKeepsakeSystem
    {
        public event Action<Survivor, Survivor, string> OnKeepsakeCreated; // hoarder, deceased, itemId
        public event Action<Survivor, string> OnKeepsakeScrapped; // survivor, itemId

        public void OnSurvivorDied(
            Survivor deceased,
            IReadOnlyList<Survivor> survivors,
            InterpersonalAffinity affinityMatrix,
            string highestValueItemId)
        {
            if (deceased == null || survivors == null || string.IsNullOrEmpty(highestValueItemId)) return;

            Survivor bestBond = null;
            float maxAffinity = 0f;

            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive || s.Id == deceased.Id) continue;

                float aff = affinityMatrix != null ? affinityMatrix.Get(s.Id, deceased.Id) : 0f;
                if (aff > maxAffinity)
                {
                    maxAffinity = aff;
                    bestBond = s;
                }
            }

            // Fall back to first living survivor if no positive affinity bond exists
            if (bestBond == null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    if (survivors[i] != null && survivors[i].IsAlive && survivors[i].Id != deceased.Id)
                    {
                        bestBond = survivors[i];
                        break;
                    }
                }
            }

            if (bestBond != null)
            {
                if (bestBond.KeepsakeItemIds == null)
                {
                    bestBond.KeepsakeItemIds = new List<string>();
                }
                bestBond.KeepsakeItemIds.Add(highestValueItemId);
                OnKeepsakeCreated?.Invoke(bestBond, deceased, highestValueItemId);
            }
        }

        public bool IsKeepsake(Survivor sv, string itemId)
        {
            if (sv == null || sv.KeepsakeItemIds == null || string.IsNullOrEmpty(itemId)) return false;
            return sv.KeepsakeItemIds.Contains(itemId);
        }

        public void ForceScrapKeepsake(
            Survivor sv,
            string itemId,
            MentalBreakSystem mentalBreak,
            System.Random rng)
        {
            if (sv == null || sv.KeepsakeItemIds == null || !sv.KeepsakeItemIds.Contains(itemId)) return;

            sv.KeepsakeItemIds.Remove(itemId);

            if (sv.Needs != null)
            {
                sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - 40f, 0f, 100f);
            }

            if (mentalBreak != null && rng != null)
            {
                mentalBreak.TryRollForBreak(sv, rng);
            }

            OnKeepsakeScrapped?.Invoke(sv, itemId);
        }
    }
}
