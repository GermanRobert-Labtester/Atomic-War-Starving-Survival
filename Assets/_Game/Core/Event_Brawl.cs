using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BrawlState
    {
        public string eventId = "event_brawl";
        public int affinityThreshold = 0;
        public float brokenBoneChance = 0.3f;
        public float concussionChance = 0.2f;
        public string fighterAId;
        public string fighterBId;
        public bool brawlActive;
    }

    public class Event_Brawl
    {
        public event Action<string, string> OnBrawlStarted;
        public event Action<string, string> OnBrawlBrokenUp;
        public event Action<string, string> OnInjuryInflicted;

        private readonly BrawlState _state;
        private bool _brawlActive;

        public Event_Brawl()
        {
            _state = new BrawlState();
        }

        public Event_Brawl(BrawlState state)
        {
            _state = state ?? new BrawlState();
            _brawlActive = _state.brawlActive;
        }

        /// <summary>
        /// Checks all survivor pairs. If any two have affinity &lt; 0, a brawl starts.
        /// Returns the pair, or null if no brawl.
        /// </summary>
        public (string a, string b)? CheckForBrawl(
            List<(string id, float affinityWithOthers)> survivors,
            System.Random rng)
        {
            if (survivors == null || survivors.Count < 2) return null;

            for (int i = 0; i < survivors.Count; i++)
            {
                for (int j = i + 1; j < survivors.Count; j++)
                {
                    if (survivors[i].affinityWithOthers < _state.affinityThreshold
                        && survivors[j].affinityWithOthers < _state.affinityThreshold)
                    {
                        string a = survivors[i].id;
                        string b = survivors[j].id;
                        _brawlActive = true;
                        _state.fighterAId = a;
                        _state.fighterBId = b;
                        _state.brawlActive = true;
                        OnBrawlStarted?.Invoke(a, b);
                        return (a, b);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// An intervenor breaks up the brawl.
        /// </summary>
        public void BreakUp(string intervenorId, string fighterA, string fighterB)
        {
            if (!_brawlActive) return;

            _brawlActive = false;
            _state.brawlActive = false;
            _state.fighterAId = null;
            _state.fighterBId = null;

            // The intervenor stops whichever fighter is still going
            string stoppedId = (fighterA != intervenorId) ? fighterA : fighterB;
            OnBrawlBrokenUp?.Invoke(intervenorId, stoppedId);
        }

        /// <summary>
        /// Let the brawl play out without intervention.
        /// Roll for injuries: broken bones or concussions.
        /// </summary>
        public void LetItPlay(string fighterA, string fighterB, System.Random rng)
        {
            if (!_brawlActive) return;

            float rollA = (float)rng.NextDouble();
            float rollB = (float)rng.NextDouble();

            if (rollA < _state.brokenBoneChance)
            {
                OnInjuryInflicted?.Invoke(fighterB, "broken_bone");
            }
            else if (rollA < _state.brokenBoneChance + _state.concussionChance)
            {
                OnInjuryInflicted?.Invoke(fighterB, "concussion");
            }

            if (rollB < _state.brokenBoneChance)
            {
                OnInjuryInflicted?.Invoke(fighterA, "broken_bone");
            }
            else if (rollB < _state.brokenBoneChance + _state.concussionChance)
            {
                OnInjuryInflicted?.Invoke(fighterA, "concussion");
            }

            _brawlActive = false;
            _state.brawlActive = false;
            _state.fighterAId = null;
            _state.fighterBId = null;
        }

        public BrawlState CaptureState()
        {
            _state.brawlActive = _brawlActive;
            return _state;
        }

        public void RestoreState(BrawlState state)
        {
            if (state == null) return;
            _state.affinityThreshold = state.affinityThreshold;
            _state.brokenBoneChance = state.brokenBoneChance;
            _state.concussionChance = state.concussionChance;
            _brawlActive = state.brawlActive;
            _state.brawlActive = state.brawlActive;
            _state.fighterAId = state.fighterAId;
            _state.fighterBId = state.fighterBId;
        }
    }
}
