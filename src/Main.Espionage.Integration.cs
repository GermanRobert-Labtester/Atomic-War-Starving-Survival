// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EspionageHostSession? _espionage166;
        private readonly HashSet<string> _espionageAgentsAway = new HashSet<string>(StringComparer.Ordinal);
        private bool _espionage166Dirty;
        private bool _espionageConsequenceBound;

        private void BindEspionageConsequenceRouting()
        {
            if (_espionage166 == null || _espionageConsequenceBound) return;

            var caravan = EnsureCaravanTrade();
            SetupPsyOps();
            SetupYearOfAsh();

            _espionage166.BindConsequenceConsumers(
                applySupply: intent => caravan.TryApplySupplyDisruption(
                    intent.targetFactionId,
                    intent.magnitude,
                    intent.startDay,
                    intent.expiryDay,
                    intent.incidentId),
                applyComms: intent =>
                {
                    if (_psyops?.System == null) return false;
                    int days = Math.Max(1, intent.expiryDay - intent.startDay);
                    return _psyops.System.StartJamming(
                        intent.targetFactionId,
                        intent.magnitude,
                        days,
                        intent.startDay);
                },
                applyDefense: intent =>
                {
                    if (_yearOfAsh?.FactionWar == null) return false;
                    return _yearOfAsh.FactionWar.TryApplyDefenseReadinessPressure(
                        intent.targetFactionId,
                        intent.magnitude,
                        intent.startDay,
                        intent.expiryDay,
                        intent.incidentId);
                });
            _espionageConsequenceBound = true;
        }

        private void SaveEspionage()
        {
            if (_espionage166 != null)
                CaptureSection(EspionageSaveStore.SectionName,
                    EspionageSaveStore.TryCapturePersisted(_espionage166.CaptureState()));
        }

        private void TickPlan167Espionage(int day)
        {
            SetupPlans166To169();
            BindEspionageConsequenceRouting();
            _espionage166?.AdvanceDay(day);
        }

        private sealed class Plan167EspionageDayOwner : Ashfall.Core.Campaign.IDayAdvanceOwner
        {
            private readonly Main _main;
            public Plan167EspionageDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPlan167Espionage(day);
                events.Add(new DayStateChangeEvent("espionage_ticked", "plan_167", null, null, day));
            }
        }

    }
}
