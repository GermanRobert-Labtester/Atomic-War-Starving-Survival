using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ExcavationSystem.
    /// Manages underground rubble clearing, worker assignments, structural shoring, cave-in risk, and room discovery.
    /// </summary>
    public sealed class ExcavationHostSession
    {
        public ExcavationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public ExcavationHostSession(ExcavationSystem system)
        {
            System = system ?? new ExcavationSystem(new SeededRng(1986), new GodotLog());

            System.OnExcavationChanged += () =>
            {
                StateChanged?.Invoke();
            };
        }

        public ActionResult AddSite(string siteId, string blueprintId, float requiredProgress = 100f, float risk = 0.2f)
        {
            var res = System.AddSite(siteId, blueprintId, requiredProgress, risk);
            if (res.IsSuccess)
            {
                LastEvent = $"Surveyed new excavation site: {siteId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult AssignWorkers(string siteId, int workerCount)
        {
            var res = System.AssignWorkers(siteId, workerCount);
            if (res.IsSuccess)
            {
                LastEvent = $"Assigned {workerCount} workers to excavation site {siteId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult ApplyShoring(string siteId)
        {
            var res = System.ApplyShoring(siteId);
            if (res.IsSuccess)
            {
                LastEvent = $"Reinforced shoring on excavation site {siteId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay()
        {
            System.TickDay();
            StateChanged?.Invoke();
        }
    }
}
