using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class EncounterChoiceResolverTests
    {
        [Fact]
        public void Resolve_AddsHistoryEntry()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            var result = r.Resolve(new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_stranded_trader",
                ChoiceId = "trade",
                Day = 5,
                PredictedOutcome = "traded",
                TriggerCombat = false,
                LootSummary = "+3 clean_water"
            });
            Assert.True(result.Succeeded);
            Assert.Single(r.History);
        }

        [Fact]
        public void Resolve_IsIdempotent_NoDuplicate()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            var req = new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_a",
                ChoiceId = "fight",
                Day = 3,
                PredictedOutcome = "combat"
            };
            Assert.True(r.Resolve(req).Succeeded);
            var second = r.Resolve(req);
            Assert.False(second.Succeeded);
            Assert.Equal("already_resolved", second.ReasonCode);
            Assert.Single(r.History);
        }

        [Fact]
        public void Resolve_TriggerCombat_FlagsResolution()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            var result = r.Resolve(new EncounterChoiceRequest
            {
                ExpeditionId = "exp_2",
                EncounterId = "enc_bandit_ambush",
                ChoiceId = "fight",
                Day = 4,
                TriggerCombat = true,
                PredictedOutcome = "combat_started"
            });
            Assert.True(result.Succeeded);
            Assert.True(result.Resolution.TriggeredCombat);
        }

        [Fact]
        public void Resolve_RequiresAllIds()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            Assert.False(r.Resolve(new EncounterChoiceRequest { ExpeditionId = "", EncounterId = "x", ChoiceId = "y" }).Succeeded);
            Assert.False(r.Resolve(new EncounterChoiceRequest { ExpeditionId = "x", EncounterId = "", ChoiceId = "y" }).Succeeded);
            Assert.False(r.Resolve(new EncounterChoiceRequest { ExpeditionId = "x", EncounterId = "y", ChoiceId = "" }).Succeeded);
        }

        [Fact]
        public void IsResolved_TrueAfterFirstResolve()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            Assert.False(r.IsResolved("exp_1", "enc_a"));
            r.Resolve(new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_a",
                ChoiceId = "flee",
                Day = 1
            });
            Assert.True(r.IsResolved("exp_1", "enc_a"));
        }

        [Fact]
        public void IsResolved_ScopedByExpedition()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            r.Resolve(new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_a",
                ChoiceId = "trade",
                Day = 1
            });
            Assert.False(r.IsResolved("exp_2", "enc_a"));
        }

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            r.Resolve(new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_a",
                ChoiceId = "trade",
                Day = 1
            });
            var save = r.CaptureState();
            var fresh = new EncounterChoiceResolver(new EncounterChoiceState());
            fresh.RestoreState(save);
            Assert.Single(fresh.History);
        }

        [Fact]
        public void Events_FireOnResolve()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            EncounterResolution? captured = null;
            r.OnResolved += res => captured = res;
            r.Resolve(new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_a",
                ChoiceId = "trade",
                Day = 1
            });
            Assert.NotNull(captured);
            Assert.Equal("trade", captured.ChoiceId);
        }

        [Fact]
        public void DuplicateResolution_DoesNotFireEvent()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            int fired = 0;
            r.OnResolved += _ => fired++;
            var req = new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_a",
                ChoiceId = "trade",
                Day = 1
            };
            r.Resolve(req);
            r.Resolve(req);
            Assert.Equal(1, fired);
        }

        [Fact]
        public void LootSummary_PreservedOnResolution()
        {
            var r = new EncounterChoiceResolver(new EncounterChoiceState());
            var result = r.Resolve(new EncounterChoiceRequest
            {
                ExpeditionId = "exp_1",
                EncounterId = "enc_a",
                ChoiceId = "search",
                Day = 2,
                LootSummary = "+1 bandage, +2 canned_food"
            });
            Assert.Equal("+1 bandage, +2 canned_food", result.Resolution.LootSummary);
        }
    }
}
