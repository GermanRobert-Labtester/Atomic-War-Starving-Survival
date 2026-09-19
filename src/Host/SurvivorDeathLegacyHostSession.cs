// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : SurvivorDeathLegacyHostSession
// Core System  : Ashfall.Core.Survivors.SurvivorDeathLegacySystem
// Host Caller  : Main.SurvivorDeathLegacy
// Purpose      : Plan 206 — Survivor death records, last wills, estate inheritance, and disputes
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class SurvivorDeathLegacyHostSession
    {
        private readonly SurvivorDeathLegacySystem _system;

        public SurvivorDeathLegacySystem System => _system;

        public event Action? StateChanged;

        public int DeathCount => _system.DeathCount;
        public int WillCount => _system.WillCount;
        public int ActiveDisputeCount => _system.ActiveDisputeCount;
        public IReadOnlyList<DeathRecord> DeathRecords => _system.DeathRecords;
        public IReadOnlyList<LastWill> Wills => _system.Wills;
        public IReadOnlyList<InheritedItem> InheritedItems => _system.InheritedItems;
        public IReadOnlyList<InheritanceDispute> Disputes => _system.Disputes;

        public SurvivorDeathLegacyHostSession(SurvivorDeathLegacySystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            _system.OnDeathRecorded += _ => StateChanged?.Invoke();
            _system.OnWillCreated += _ => StateChanged?.Invoke();
            _system.OnInheritanceDistributed += (_, _) => StateChanged?.Invoke();
            _system.OnDisputeRaised += _ => StateChanged?.Invoke();
            _system.OnDisputeResolved += _ => StateChanged?.Invoke();
        }

        public LastWill CreateWill(
            string survivorId,
            IEnumerable<BeneficiaryEntry>? beneficiaries = null,
            IEnumerable<SpecialBequest>? specialBequests = null,
            string residuaryBeneficiary = "",
            IEnumerable<string>? witnesses = null,
            int currentDay = 1)
        {
            var will = _system.CreateWill(survivorId, beneficiaries, specialBequests, residuaryBeneficiary, witnesses, currentDay);
            StateChanged?.Invoke();
            return will;
        }

        public DeathRecord RecordDeath(
            string survivorId,
            string survivorName,
            DeathCause cause,
            int deathDay,
            string location = "",
            string lastWords = "",
            IEnumerable<string>? witnesses = null,
            string circumstances = "")
        {
            var rec = _system.RecordDeath(survivorId, survivorName, cause, deathDay, location, lastWords, witnesses, circumstances);
            StateChanged?.Invoke();
            return rec;
        }

        public IReadOnlyList<InheritedItem> DistributeInheritance(string deathRecordId, IEnumerable<string> itemIds, string fallbackBeneficiary = "commons")
        {
            var items = _system.DistributeInheritance(deathRecordId, itemIds, fallbackBeneficiary);
            StateChanged?.Invoke();
            return items;
        }

        public InheritanceDispute RaiseDispute(string willId, string disputantId, string reason)
        {
            var disp = _system.RaiseDispute(willId, disputantId, reason);
            StateChanged?.Invoke();
            return disp;
        }

        public bool ResolveDispute(string disputeId, DisputeResolution resolution)
        {
            bool res = _system.ResolveDispute(disputeId, resolution);
            if (res)
                StateChanged?.Invoke();
            return res;
        }

        public LastWill? GetActiveWill(string survivorId)
        {
            return _system.GetActiveWill(survivorId);
        }

        public DeathRecord? GetDeathRecord(string survivorId)
        {
            return _system.GetDeathRecord(survivorId);
        }

        public SurvivorDeathLegacyState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(SurvivorDeathLegacyState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
