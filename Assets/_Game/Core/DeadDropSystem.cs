using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Dead Drops — Contactless Trade (Prompt #72). Map nodes can be DeadDrop
    /// sites. Player leaves items in a locker; 48h later a Faction replaces them
    /// with agreed-upon goods. Bypasses face-to-face ambush risk but has a 15%
    /// chance of theft by random scavengers. Save/load safe. Plain C#.
    /// </summary>
    public class DeadDropSystem
    {
        /// <summary>Hours until a dead drop is resolved.</summary>
        public const float DeadDropResolveHours = 48f;

        /// <summary>Chance (0..1) that the drop is stolen by random scavengers.</summary>
        public const float TheftChance = 0.15f;

        /// <summary>Morale hit when a dead drop is stolen.</summary>
        public const float TheftMoralePenalty = 8f;

        /// <summary>Reputation/trust bonus when a dead drop succeeds.</summary>
        public const float SuccessfulDropTrustBonus = 5f;

        /// <summary>Active dead drops.</summary>
        public class DeadDrop
        {
            public string DropId;
            public string NodeId;
            public string FactionId;
            public float HoursUntilResolve;
            /// <summary>Item ids deposited by the player (for theft notification).</summary>
            public List<string> DepositedItemIds = new List<string>();
            /// <summary>Item ids expected in return.</summary>
            public List<string> ExpectedReturnItemIds = new List<string>();
            public bool IsResolved;
            public bool WasStolen;
        }

        private readonly List<DeadDrop> _activeDrops = new List<DeadDrop>();
        private readonly HashSet<string> _deadDropNodeIds = new HashSet<string>();
        private readonly System.Random _rng;
        private int _dropSeq;
        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private Func<string, Survivor> _resolveCourier; // optional: map drop to courier

        // -- Events --
        public event Action<DeadDrop> OnDeadDropPlaced;
        public event Action<DeadDrop> OnDeadDropResolved;    // successful trade
        public event Action<DeadDrop> OnDeadDropStolen;
        public event Action OnStateChanged;

        public IReadOnlyList<DeadDrop> ActiveDrops => _activeDrops;
        public IReadOnlyCollection<string> DeadDropNodeIds => _deadDropNodeIds;

        public DeadDropSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(72);
        }

        /// <summary>Prompt #231 — Lost Route dead-drop progress for The Courier.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null,
            Func<string, Survivor> resolveCourier = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
            _resolveCourier = resolveCourier;
        }

        /// <summary>Mark a node as a dead-drop site.</summary>
        public void SetDeadDropNode(string nodeId, bool isDrop)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            if (isDrop) _deadDropNodeIds.Add(nodeId);
            else _deadDropNodeIds.Remove(nodeId);
        }

        public bool IsDeadDropNode(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId) && _deadDropNodeIds.Contains(nodeId);
        }

        /// <summary>
        /// Place a dead drop at a node. Items are deposited now; return expected
        /// in 48h. Returns the drop id or null if node is invalid.
        /// </summary>
        public DeadDrop PlaceDeadDrop(string nodeId, string factionId,
            List<string> depositedItemIds, List<string> expectedReturnItemIds)
        {
            if (string.IsNullOrEmpty(nodeId) || string.IsNullOrEmpty(factionId)) return null;
            if (!IsDeadDropNode(nodeId)) return null;

            var drop = new DeadDrop
            {
                DropId = $"dead_drop_{++_dropSeq}",
                NodeId = nodeId,
                FactionId = factionId,
                HoursUntilResolve = DeadDropResolveHours,
                DepositedItemIds = depositedItemIds ?? new List<string>(),
                ExpectedReturnItemIds = expectedReturnItemIds ?? new List<string>(),
                IsResolved = false,
                WasStolen = false
            };
            _activeDrops.Add(drop);
            OnDeadDropPlaced?.Invoke(drop);
            OnStateChanged?.Invoke();
            return drop;
        }

        /// <summary>
        /// Tick dead drops toward resolution. After 48h, roll for theft.
        /// </summary>
        public void Tick(float gameHours,
            Action<DeadDrop> onSuccess = null,
            Action<DeadDrop> onTheft = null)
        {
            if (gameHours <= 0f || _activeDrops.Count == 0) return;

            for (int i = _activeDrops.Count - 1; i >= 0; i--)
            {
                var drop = _activeDrops[i];
                if (drop.IsResolved) continue;

                drop.HoursUntilResolve -= gameHours;
                if (drop.HoursUntilResolve > 0f) continue;

                drop.IsResolved = true;

                // 15% theft chance.
                if (_rng.NextDouble() < TheftChance)
                {
                    drop.WasStolen = true;
                    OnDeadDropStolen?.Invoke(drop);
                    onTheft?.Invoke(drop);
                    NotifyCourierFailure(drop);
                }
                else
                {
                    OnDeadDropResolved?.Invoke(drop);
                    onSuccess?.Invoke(drop);
                    NotifyCourierSuccess(drop);
                }

                _activeDrops.RemoveAt(i);
            }

            if (_activeDrops.Count == 0)
                OnStateChanged?.Invoke();
        }

        /// <summary>Find a dead drop by id.</summary>
        public DeadDrop GetDrop(string dropId)
        {
            if (string.IsNullOrEmpty(dropId)) return null;
            for (int i = 0; i < _activeDrops.Count; i++)
                if (_activeDrops[i].DropId == dropId) return _activeDrops[i];
            return null;
        }

        private void NotifyCourierSuccess(DeadDrop drop)
        {
            if (_personalQuests == null) return;
            var courier = ResolveCourierForDrop(drop);
            if (courier != null)
                _personalQuests.RecordDeadDropSuccess(courier);
        }

        private void NotifyCourierFailure(DeadDrop drop)
        {
            if (_personalQuests == null) return;
            var courier = ResolveCourierForDrop(drop);
            if (courier != null)
                _personalQuests.RecordDeadDropFailure(courier);
        }

        private Survivor ResolveCourierForDrop(DeadDrop drop)
        {
            if (_resolveCourier != null && drop != null)
            {
                var c = _resolveCourier(drop.DropId);
                if (c != null) return c;
            }
            var list = _getSurvivors?.Invoke();
            if (list == null) return null;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s == null || !s.IsAlive) continue;
                if (string.Equals(s.ArchetypeId, PersonalQuestSystem.CourierId, StringComparison.Ordinal)
                    || string.Equals(s.Id, PersonalQuestSystem.CourierId, StringComparison.Ordinal))
                    return s;
            }
            return null;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public DeadDropSave CaptureState()
        {
            var drops = new DeadDropEntrySave[_activeDrops.Count];
            for (int i = 0; i < _activeDrops.Count; i++)
            {
                var d = _activeDrops[i];
                drops[i] = new DeadDropEntrySave
                {
                    DropId = d.DropId,
                    NodeId = d.NodeId,
                    FactionId = d.FactionId,
                    HoursUntilResolve = d.HoursUntilResolve,
                    DepositedItemIds = d.DepositedItemIds?.ToArray() ?? Array.Empty<string>(),
                    ExpectedReturnItemIds = d.ExpectedReturnItemIds?.ToArray() ?? Array.Empty<string>(),
                    IsResolved = d.IsResolved,
                    WasStolen = d.WasStolen
                };
            }
            var nodeIds = new string[_deadDropNodeIds.Count];
            _deadDropNodeIds.CopyTo(nodeIds);
            return new DeadDropSave
            {
                Drops = drops,
                DeadDropNodeIds = nodeIds,
                DropSeq = _dropSeq
            };
        }

        public void RestoreState(DeadDropSave save)
        {
            _activeDrops.Clear();
            _deadDropNodeIds.Clear();
            _dropSeq = 0;
            if (save == null) return;
            _dropSeq = save.DropSeq;
            if (save.DeadDropNodeIds != null)
                for (int i = 0; i < save.DeadDropNodeIds.Length; i++)
                    if (!string.IsNullOrEmpty(save.DeadDropNodeIds[i]))
                        _deadDropNodeIds.Add(save.DeadDropNodeIds[i]);
            if (save.Drops != null)
            {
                for (int i = 0; i < save.Drops.Length; i++)
                {
                    var d = save.Drops[i];
                    if (d == null) continue;
                    _activeDrops.Add(new DeadDrop
                    {
                        DropId = d.DropId,
                        NodeId = d.NodeId,
                        FactionId = d.FactionId,
                        HoursUntilResolve = d.HoursUntilResolve,
                        DepositedItemIds = d.DepositedItemIds != null
                            ? new List<string>(d.DepositedItemIds) : new List<string>(),
                        ExpectedReturnItemIds = d.ExpectedReturnItemIds != null
                            ? new List<string>(d.ExpectedReturnItemIds) : new List<string>(),
                        IsResolved = d.IsResolved,
                        WasStolen = d.WasStolen
                    });
                }
            }
        }
    }

    [Serializable]
    public class DeadDropSave
    {
        public DeadDropEntrySave[] Drops;
        public string[] DeadDropNodeIds;
        public int DropSeq;
    }

    [Serializable]
    public class DeadDropEntrySave
    {
        public string DropId;
        public string NodeId;
        public string FactionId;
        public float HoursUntilResolve;
        public string[] DepositedItemIds;
        public string[] ExpectedReturnItemIds;
        public bool IsResolved;
        public bool WasStolen;
    }
}
