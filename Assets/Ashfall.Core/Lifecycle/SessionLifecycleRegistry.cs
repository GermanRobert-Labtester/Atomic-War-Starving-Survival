using System;
using System.Collections.Generic;
using Ashfall.Core.Save;

namespace Ashfall.Core.Lifecycle
{
    /// <summary>
    /// Lifecycle phases for ASHFALL sessions and host participants.
    /// </summary>
    public enum SessionLifecyclePhase
    {
        Construct,
        Bind,
        Restore,
        Activate,
        Unbind,
        Dispose,
        Reset,
        DeletePersistence
    }

    /// <summary>
    /// Contract for a session or subsystem registered with the lifecycle coordinator.
    /// A participant owns either a canonical save section or a lifecycle group. A
    /// group is an in-memory lifecycle boundary; it is never itself a campaign
    /// section or an on-disk file.
    /// </summary>
    public interface ISessionParticipant
    {
        string ParticipantId { get; }
        IReadOnlyList<string> DependsOn { get; }
        string? SaveSectionKey { get; }
        string? LifecycleGroup { get; }
        IReadOnlyList<string> OwnedSaveSectionKeys { get; }

        void OnReset();
        void OnDispose();
    }

    /// <summary>
    /// Lightweight delegate-backed participant for host sessions and panels.
    /// Save-section aliases are normalized at this lifecycle boundary so the
    /// participant metadata cannot drift from the current save registry.
    /// </summary>
    public sealed class DelegateSessionParticipant : ISessionParticipant
    {
        public string ParticipantId { get; }
        public IReadOnlyList<string> DependsOn { get; }
        public string? SaveSectionKey { get; }
        public string? LifecycleGroup { get; }
        public IReadOnlyList<string> OwnedSaveSectionKeys { get; }

        private readonly Action? _onReset;
        private readonly Action? _onDispose;

        public DelegateSessionParticipant(
            string participantId,
            IReadOnlyList<string>? dependsOn = null,
            string? saveSectionKey = null,
            Action? onReset = null,
            Action? onDispose = null,
            string? lifecycleGroup = null,
            IReadOnlyList<string>? ownedSaveSectionKeys = null)
        {
            ParticipantId = participantId ?? throw new ArgumentNullException(nameof(participantId));
            DependsOn = dependsOn ?? Array.Empty<string>();
            SaveSectionKey = SaveSectionRegistry.CanonicalizeSectionKey(saveSectionKey);
            LifecycleGroup = string.IsNullOrWhiteSpace(lifecycleGroup) ? null : lifecycleGroup;
            OwnedSaveSectionKeys = BuildOwnedSaveSectionKeys(ownedSaveSectionKeys);
            _onReset = onReset;
            _onDispose = onDispose;
        }

        private IReadOnlyList<string> BuildOwnedSaveSectionKeys(IReadOnlyList<string>? requested)
        {
            if (requested == null)
            {
                if (SaveSectionKey != null)
                    requested = new[] { SaveSectionKey };
                else if (LifecycleGroup != null)
                    requested = SaveSectionRegistry.SectionKeysForLifecycleGroup(LifecycleGroup);
                else
                    requested = Array.Empty<string>();
            }

            var result = new List<string>(requested.Count);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var key in requested)
            {
                string? canonical = SaveSectionRegistry.CanonicalizeSectionKey(key);
                if (string.IsNullOrWhiteSpace(canonical) || !seen.Add(canonical))
                    continue;
                result.Add(canonical);
            }

            return result.AsReadOnly();
        }

        public void OnReset() => _onReset?.Invoke();
        public void OnDispose() => _onDispose?.Invoke();
    }

    /// <summary>
    /// Central typed registry managing session construction, dependencies,
    /// safe-order resets, and disposals without reflection.
    /// </summary>
    public interface ISessionLifecycleRegistry
    {
        IReadOnlyList<ISessionParticipant> Participants { get; }
        void Register(ISessionParticipant participant);
        bool Unregister(string participantId);
        void ResetAll();
        void DisposeAll();
        IReadOnlyList<string> GetTopologicalOrder();
        IReadOnlyList<string> GetReverseTopologicalOrder();
        IReadOnlyList<string> ValidatePolicies();
    }

    public sealed class SessionLifecycleRegistry : ISessionLifecycleRegistry
    {
        private readonly Dictionary<string, ISessionParticipant> _participants =
            new Dictionary<string, ISessionParticipant>(StringComparer.Ordinal);
        private readonly List<string> _insertionOrder = new List<string>();

        public IReadOnlyList<ISessionParticipant> Participants
        {
            get
            {
                var list = new List<ISessionParticipant>(_participants.Count);
                foreach (var id in _insertionOrder)
                {
                    if (_participants.TryGetValue(id, out var p))
                        list.Add(p);
                }
                return list;
            }
        }

        public void Register(ISessionParticipant participant)
        {
            if (participant == null) throw new ArgumentNullException(nameof(participant));
            if (string.IsNullOrWhiteSpace(participant.ParticipantId))
                throw new ArgumentException("Participant ID cannot be empty.", nameof(participant));

            if (!_participants.ContainsKey(participant.ParticipantId))
            {
                _insertionOrder.Add(participant.ParticipantId);
            }
            _participants[participant.ParticipantId] = participant;
        }

        public bool Unregister(string participantId)
        {
            if (string.IsNullOrWhiteSpace(participantId)) return false;
            _insertionOrder.Remove(participantId);
            return _participants.Remove(participantId);
        }

        public void ResetAll()
        {
            var order = GetReverseTopologicalOrder();
            foreach (var id in order)
            {
                if (_participants.TryGetValue(id, out var p))
                {
                    p.OnReset();
                }
            }
        }

        public void DisposeAll()
        {
            var order = GetReverseTopologicalOrder();
            foreach (var id in order)
            {
                if (_participants.TryGetValue(id, out var p))
                {
                    p.OnDispose();
                }
            }
        }

        public IReadOnlyList<string> GetTopologicalOrder()
        {
            var result = new List<string>(_participants.Count);
            var visited = new HashSet<string>(StringComparer.Ordinal);
            var visiting = new HashSet<string>(StringComparer.Ordinal);

            void Visit(string id)
            {
                if (visited.Contains(id)) return;
                if (visiting.Contains(id)) return; // cycle fallback: break cycle

                visiting.Add(id);
                if (_participants.TryGetValue(id, out var p))
                {
                    foreach (var dep in p.DependsOn)
                    {
                        if (_participants.ContainsKey(dep))
                            Visit(dep);
                    }
                }
                visiting.Remove(id);
                visited.Add(id);
                result.Add(id);
            }

            foreach (var id in _insertionOrder)
            {
                Visit(id);
            }

            return result;
        }

        public IReadOnlyList<string> GetReverseTopologicalOrder()
        {
            var topo = new List<string>(GetTopologicalOrder());
            topo.Reverse();
            return topo;
        }

        public IReadOnlyList<string> ValidatePolicies()
        {
            var errors = new List<string>();
            foreach (var kv in _participants)
            {
                var p = kv.Value;
                foreach (var dep in p.DependsOn)
                {
                    if (!_participants.ContainsKey(dep))
                    {
                        errors.Add($"Participant '{p.ParticipantId}' depends on unregistered participant '{dep}'.");
                    }
                }
            }
            return errors;
        }
    }
}
