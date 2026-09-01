// SPDX-License-Identifier: MIT
// Task #132 — Test double for the domain component contract.
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Survivors
{
    /// <summary>
    /// Minimal in-memory <see cref="ISurvivorComponentStore"/> standing in for a real
    /// domain (needs, radiation, medical...) so referential integrity and the Leave
    /// transaction can be tested before any domain is migrated.
    /// </summary>
    internal sealed class FakeSurvivorComponentStore : ISurvivorComponentStore
    {
        private readonly HashSet<SurvivorId> _owners = new HashSet<SurvivorId>();

        public FakeSurvivorComponentStore(
            string componentName,
            SurvivorComponentCardinality cardinality = SurvivorComponentCardinality.ZeroOrOne,
            bool retainsHistoryAfterDeath = false)
        {
            ComponentName = componentName;
            Cardinality = cardinality;
            RetainsHistoryAfterDeath = retainsHistoryAfterDeath;
        }

        public string ComponentName { get; }
        public SurvivorComponentCardinality Cardinality { get; }
        public bool RetainsHistoryAfterDeath { get; }

        /// <summary>How many times <see cref="Release"/> was called, for atomicity assertions.</summary>
        public int ReleaseCallCount { get; private set; }

        public IEnumerable<SurvivorId> OwnerIds
        {
            get
            {
                var ordered = new List<SurvivorId>(_owners);
                ordered.Sort();
                return ordered;
            }
        }

        public bool Contains(SurvivorId owner) => _owners.Contains(owner);

        public bool Release(SurvivorId owner)
        {
            ReleaseCallCount++;
            return _owners.Remove(owner);
        }

        /// <summary>Attach a record, bypassing any integrity check — that is the point.</summary>
        public void Attach(SurvivorId owner) => _owners.Add(owner);

        /// <summary>Attach a record for an id that may not be a survivor at all.</summary>
        public void Attach(string rawId) => _owners.Add(new SurvivorId(rawId));

        public int Count => _owners.Count;
    }
}
