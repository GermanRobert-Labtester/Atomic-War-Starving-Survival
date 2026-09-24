// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class ChildDevelopmentHostSession : HostSessionBase
    {
        public ChildDevelopmentSystem System { get; }

        public ChildDevelopmentHostSession(ChildDevelopmentSystem? system = null)
        {
            System = system ?? new ChildDevelopmentSystem();
            System.OnStageChanged += (_, _) => RaiseStateChanged();
            System.OnMilestoneAchieved += _ => RaiseStateChanged();
        }

        public static ChildDevelopmentHostSession Create(string dataDir, ChildDevelopmentState? restoredState = null)
        {
            var system = new ChildDevelopmentSystem(restoredState ?? new ChildDevelopmentState());
            string catalogPath = Path.Combine(dataDir, "development_traits.json");
            if (File.Exists(catalogPath))
            {
                system.LoadCatalog(File.ReadAllText(catalogPath));
            }
            return new ChildDevelopmentHostSession(system);
        }

        public ChildProfile RegisterChild(
            string childId,
            string name,
            int birthDay,
            IEnumerable<string>? parentIds = null,
            string caregiverId = "")
        {
            var profile = System.RegisterChild(childId, name, birthDay, parentIds, caregiverId);
            RaiseStateChanged();
            return profile;
        }

        public void TickDay(int currentDay)
        {
            System.TickDay(currentDay);
            RaiseStateChanged();
        }

        public bool RecordEducation(string childId, float amount = 5f)
        {
            bool ok = System.RecordEducation(childId, amount);
            if (ok) RaiseStateChanged();
            return ok;
        }

        public bool AssignCaregiver(string childId, string caregiverId)
        {
            bool ok = System.AssignCaregiver(childId, caregiverId);
            if (ok) RaiseStateChanged();
            return ok;
        }

        public float GetChoreWorkCapacity(string childId) => System.GetChoreWorkCapacity(childId);

        public ChildProfile? GetChild(string childId) => System.GetChild(childId);

        public ChildDevelopmentCensus GetCensus() => System.GetCensus();
    }
}
