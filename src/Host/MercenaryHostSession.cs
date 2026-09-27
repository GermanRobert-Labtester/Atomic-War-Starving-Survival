// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Mercenary System — host session binding the Core system to
    /// the campaign lifecycle. Manages mercenary contracts and deployments.
    /// (Coordination note 2026-09-27: the draft-local duplicate
    /// `MercenarySaveStore` class was removed — the canonical committed store
    /// `MercenarySaveStore.cs` (section `mercenary_bounties`) is the enrolled
    /// authority; `MercenarySystemState` did not exist anywhere in Core — the
    /// real type is `Ashfall.Core.Economy.MercenaryState`. Repair applied by
    /// the rumor/memorial seal session to unblock the shared host build;
    /// session design otherwise untouched.)
    /// </summary>
    public sealed class MercenaryHostSession : HostSessionBase
    {
        private readonly MercenarySystem _system;

        public MercenarySystem System => _system;
        public MercenaryState State => _system.State;

        public MercenaryHostSession(MercenarySystem? system = null)
        {
            // Coordination repair 2026-09-27 (rumor/memorial seal session): the
            // draft used a guessed no-arg ctor; the live Core ctor is
            // (rng, inventory) and loads bounty_board.json with a built-in
            // fallback template when the file is absent.
            _system = system ?? new MercenarySystem(
                new SeededRng(1234), new Ashfall.Core.Inventory.Inventory());
        }

        public void Tick(int day)
        {
            _system.TickDay(day);
            RaiseStateChanged();
        }

        public MercenaryState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(MercenaryState state)
        {
            _system.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _system.RestoreState(new MercenaryState());
            RaiseStateChanged();
        }
    }
}
