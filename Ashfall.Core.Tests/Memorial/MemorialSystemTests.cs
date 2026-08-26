using System;
using System.Collections.Generic;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests.Memorial
{
    public class MemorialSystemTests
    {
        [Fact]
        public void Memorialize_AddsEntry()
        {
            var sys = new MemorialSystem(new MemorialState());
            var e = sys.Memorialize(new MemorialInput
            {
                SurvivorId = "elena_vasquez",
                Cause = "radiation",
                Day = 40,
                BirthDay = 1,
                FinalWishResolved = true,
                Epitaph = "She walked into the grey.",
                HeirloomItemId = "wedding_ring",
                HeirloomRecipientId = "marcus_olejnik",
                MoraleDelta = -8f
            });
            Assert.Single(sys.Entries);
            Assert.Equal("elena_vasquez", e.SurvivorId);
            Assert.Equal(39, e.SurvivedDays);
        }

        [Fact]
        public void Memorialize_IsIdempotent_NoDuplicate()
        {
            var sys = new MemorialSystem(new MemorialState());
            sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
            sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
            Assert.Single(sys.Entries);
        }

        [Fact]
        public void Memorialize_DifferentSurvivors_BothAdded()
        {
            var sys = new MemorialSystem(new MemorialState());
            sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "radiation", Day = 5, BirthDay = 1 });
            sys.Memorialize(new MemorialInput { SurvivorId = "s2", Cause = "combat", Day = 6, BirthDay = 1 });
            Assert.Equal(2, sys.Entries.Count);
        }

        [Fact]
        public void Memorialize_DefaultsCauseWhenMissing()
        {
            var sys = new MemorialSystem(new MemorialState());
            var e = sys.Memorialize(new MemorialInput { SurvivorId = "s1", Day = 5, BirthDay = 1 });
            Assert.Equal("unspecified", e.Cause);
        }

        [Fact]
        public void Memorialize_BlankCauseIsReplaced()
        {
            var sys = new MemorialSystem(new MemorialState());
            var e = sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "", Day = 5, BirthDay = 1 });
            Assert.Equal("unspecified", e.Cause);
        }

        [Fact]
        public void Memorialize_RequiresSurvivorId()
        {
            var sys = new MemorialSystem(new MemorialState());
            Assert.Throws<ArgumentException>(() =>
                sys.Memorialize(new MemorialInput { SurvivorId = "", Day = 5, BirthDay = 1 }));
        }

        [Fact]
        public void Events_FireOnMemorialize()
        {
            var sys = new MemorialSystem(new MemorialState());
            MemorialEntry? captured = null;
            sys.OnMemorialized += e => captured = e;
            sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
            Assert.NotNull(captured);
            Assert.Equal("s1", captured.SurvivorId);
        }

        [Fact]
        public void Idempotency_DoesNotFireEventTwice()
        {
            var sys = new MemorialSystem(new MemorialState());
            int fired = 0;
            sys.OnMemorialized += _ => fired++;
            sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
            sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "combat", Day = 5, BirthDay = 1 });
            Assert.Equal(1, fired);
        }

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var sys = new MemorialSystem(new MemorialState());
            sys.Memorialize(new MemorialInput { SurvivorId = "s1", Cause = "radiation", Day = 5, BirthDay = 1 });
            sys.Memorialize(new MemorialInput { SurvivorId = "s2", Cause = "combat", Day = 6, BirthDay = 1 });
            var save = sys.CaptureState();
            var fresh = new MemorialSystem(new MemorialState());
            fresh.RestoreState(save);
            Assert.Equal(2, fresh.Entries.Count);
        }

        [Fact]
        public void HeirloomTransfer_AtomicInEntry()
        {
            var sys = new MemorialSystem(new MemorialState());
            var e = sys.Memorialize(new MemorialInput
            {
                SurvivorId = "s1",
                Cause = "radiation",
                Day = 5,
                BirthDay = 1,
                HeirloomItemId = "wedding_ring",
                HeirloomRecipientId = "marcus_olejnik"
            });
            Assert.Equal("wedding_ring", e.HeirloomItemId);
            Assert.Equal("marcus_olejnik", e.HeirloomRecipientId);
        }
    }
}
