// SPDX-License-Identifier: MIT
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
    : HostSessionBase{
        public ExcavationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public ExcavationHostSession(ExcavationSystem system)
        {
            System = system ?? new ExcavationSystem(new SeededRng(1986), new GodotLog());

            System.OnExcavationChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public ActionResult AddSite(string siteId, string blueprintId, float requiredProgress = 100f, float risk = 0.2f)
        {
            var res = System.AddSite(siteId, blueprintId, requiredProgress, risk);
            LastEvent = res.IsSuccess
                ? $"Surveyed new excavation site: {siteId}"
                : $"Survey refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult AssignWorkers(string siteId, int workerCount)
        {
            var res = System.AssignWorkers(siteId, workerCount);
            LastEvent = res.IsSuccess
                ? $"Assigned {workerCount} workers to excavation site {siteId}"
                : $"Worker assignment refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult ApplyShoring(string siteId)
        {
            var res = System.ApplyShoring(siteId);
            LastEvent = res.IsSuccess
                ? $"Reinforced shoring on excavation site {siteId}"
                : $"Shoring refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public void TickDay()
        {
            System.TickDay();
            RaiseStateChanged();
        }

    }
}
