using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Expansion VI — Gossip & Scapegoat. When things go wrong, the bunker looks
    /// for someone to blame. The Outcast (high radiation) or the Defector (former
    /// Cultist) gets targeted. The bunker demands exile. Defend them and lose
    /// authority, or exile them and lose a survivor.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class GossipAndScapegoatSystem
    {
        public const float ScapegoatTriggerThreshold = 0.3f; // 30% bad event frequency
        public const float AuthorityCost_Defend = 15f;
        public const float MoraleBoost_Exile = 10f;
        public const string Event_SpontaneousMurder = "event_spontaneous_murder";
        public const string Affliction_Stigmatized = "affliction_stigmatized";

        public event Action<string> OnScapegoatTargeted;
        public event Action<string> OnScapegoatDefended;
        public event Action<string> OnScapegoatExiled;
        public event Action<string> OnMurderPlotted;
        public event Action<string> OnMoralChronicleEntry;

        private readonly System.Random _rng;
        private string _currentScapegoatId;
        private readonly List<ScapegoatEvent> _eventLog = new List<ScapegoatEvent>();
        private int _scapegoatsExiled;

        public string CurrentScapegoatId => _currentScapegoatId;
        public int ScapegoatsExiled => _scapegoatsExiled;
        public IReadOnlyList<ScapegoatEvent> EventLog => _eventLog;

        public GossipAndScapegoatSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(3333);
        }

        /// <summary>
        /// Roll for scapegoat targeting after a bad event (raid, sickness, crop failure).
        /// Evaluates InterpersonalAffinity and Trait_Stigmatized.
        /// </summary>
        public string RollForScapegoat(IReadOnlyList<Survivor> survivors,
            Dictionary<string, float> radiationLevels, int badEventCount)
        {
            if (survivors == null || survivors.Count < 3) return null;
            if (badEventCount < 2) return null; // Need at least 2 bad events

            // Find candidates: high radiation or has stigmatized trait
            var candidates = new List<string>();
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (sv.HasTrait("trait_stigmatized") || sv.HasTrait("the_defector"))
                    candidates.Add(sv.Id);
                else if (radiationLevels != null && radiationLevels.TryGetValue(sv.Id, out var rad)
                         && rad > 50f)
                    candidates.Add(sv.Id);
            }

            if (candidates.Count == 0) return null;
            if (_rng.NextDouble() > ScapegoatTriggerThreshold) return null;

            string target = candidates[_rng.Next(candidates.Count)];
            _currentScapegoatId = target;
            OnScapegoatTargeted?.Invoke(target);
            return target;
        }

        /// <summary>Defend the scapegoat. Costs authority.</summary>
        public bool DefendScapegoat(string defenderId, Action<string, float> modifyAuthority)
        {
            if (string.IsNullOrEmpty(_currentScapegoatId)) return false;

            modifyAuthority?.Invoke(defenderId, -AuthorityCost_Defend);
            OnScapegoatDefended?.Invoke(_currentScapegoatId);

            // The General/ Sheriff may plot murder
            if (_rng.NextDouble() < 0.30f)
            {
                OnMurderPlotted?.Invoke(_currentScapegoatId);
                OnMoralChronicleEntry?.Invoke(
                    "The gossip was silenced. But the General draws lines on the map. " +
                    "The scapegoat's name is underlined.");
            }

            _eventLog.Add(new ScapegoatEvent
            {
                ScapegoatId = _currentScapegoatId,
                Action = "defended",
                DefenderId = defenderId
            });

            _currentScapegoatId = null;
            return true;
        }

        /// <summary>Exile the scapegoat. Opens the hatch and pushes them out.</summary>
        public bool ExileScapegoat(IReadOnlyList<Survivor> survivors, Action<string, float> modifyMorale)
        {
            if (string.IsNullOrEmpty(_currentScapegoatId)) return false;

            // Mark as exiled (survivor is removed from the bunker)
            _scapegoatsExiled++;

            // Morale boost — the bunker feels "safe"
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv != null && sv.IsAlive && sv.Id != _currentScapegoatId)
                        modifyMorale?.Invoke(sv.Id, MoraleBoost_Exile);
                }
            }

            OnScapegoatExiled?.Invoke(_currentScapegoatId);
            OnMoralChronicleEntry?.Invoke(
                "The cough started on Tuesday. By Thursday, we decided it was his fault. " +
                "We opened the door. He didn't fight back.");

            _eventLog.Add(new ScapegoatEvent
            {
                ScapegoatId = _currentScapegoatId,
                Action = "exiled"
            });

            _currentScapegoatId = null;
            return true;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public GossipSave CaptureState()
        {
            var log = new ScapegoatEventSave[_eventLog.Count];
            for (int i = 0; i < _eventLog.Count; i++)
                log[i] = new ScapegoatEventSave
                {
                    ScapegoatId = _eventLog[i].ScapegoatId,
                    Action = _eventLog[i].Action,
                    DefenderId = _eventLog[i].DefenderId
                };
            return new GossipSave
            {
                CurrentScapegoatId = _currentScapegoatId,
                ScapegoatsExiled = _scapegoatsExiled,
                EventLog = log
            };
        }

        public void RestoreState(GossipSave save)
        {
            _currentScapegoatId = null;
            _scapegoatsExiled = 0;
            _eventLog.Clear();
            if (save == null) return;
            _currentScapegoatId = save.CurrentScapegoatId;
            _scapegoatsExiled = save.ScapegoatsExiled;
            if (save.EventLog != null)
                for (int i = 0; i < save.EventLog.Length; i++)
                    if (save.EventLog[i] != null)
                        _eventLog.Add(new ScapegoatEvent
                        {
                            ScapegoatId = save.EventLog[i].ScapegoatId,
                            Action = save.EventLog[i].Action,
                            DefenderId = save.EventLog[i].DefenderId
                        });
        }
    }

    public class ScapegoatEvent
    {
        public string ScapegoatId;
        public string Action;
        public string DefenderId;
    }

    [Serializable]
    public class GossipSave
    {
        public string CurrentScapegoatId;
        public int ScapegoatsExiled;
        public ScapegoatEventSave[] EventLog;
    }

    [Serializable]
    public class ScapegoatEventSave
    {
        public string ScapegoatId;
        public string Action;
        public string DefenderId;
    }
}
