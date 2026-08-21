using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for SurvivorRelationsSystem.
    /// Manages dweller-to-dweller affinity, trust, resentment, interpersonal conflicts, and mediation.
    /// </summary>
    public sealed class SurvivorRelationsHostSession
    {
        public SurvivorRelationsSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public SurvivorRelationsHostSession(SurvivorRelationsSystem system)
        {
            System = system ?? new SurvivorRelationsSystem(new SeededRng(1986), new GodotLog());

            System.OnConflictStarted += conflict =>
            {
                LastEvent = $"[Relations] CONFLICT: {conflict.dwellerA} and {conflict.dwellerB} clashed over {conflict.cause}!";
                StateChanged?.Invoke();
            };

            System.OnConflictResolved += entry =>
            {
                LastEvent = $"[Relations] Conflict {entry.conflictId} resolved by {entry.mediatorId}: {entry.outcome}";
                StateChanged?.Invoke();
            };
        }

        public void ModifyAffinity(string dwellerA, string dwellerB, float delta)
        {
            System.ModifyAffinity(dwellerA, dwellerB, delta);
            StateChanged?.Invoke();
        }

        public ConflictEntry? TryTriggerConflict()
        {
            var conflict = System.TryTriggerConflict();
            if (conflict != null)
            {
                LastEvent = $"Conflict erupted between {conflict.dwellerA} and {conflict.dwellerB}: {conflict.cause}";
                StateChanged?.Invoke();
            }
            return conflict;
        }

        public ActionResult Mediate(string conflictId, string mediatorId, MediationStyle style)
        {
            var res = System.Mediate(conflictId, mediatorId, style);
            if (res.IsSuccess)
            {
                LastEvent = $"Mediation completed for conflict {conflictId} by {mediatorId} ({style})";
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            StateChanged?.Invoke();
        }
    }
}
