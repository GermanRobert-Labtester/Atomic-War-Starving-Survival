using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EventBus dispatch semantics + the editor-only slow-event profiler:
    /// snapshot caching must survive mid-dispatch mutation, and any event
    /// taking longer than the 2ms budget logs a warning.
    /// </summary>
    [TestFixture]
    public class EventBusTests
    {
        private struct PingEvent { public int Value; }
        private struct OtherEvent { public int Value; }

        [SetUp]
        public void SetUp() => EventBus.Clear();

        [TearDown]
        public void TearDown() => EventBus.Clear();

        [Test]
        public void Raise_DeliversToAllSubscribers_ByType()
        {
            int a = 0, b = 0, other = 0;
            EventBus.Subscribe<PingEvent>(e => a += e.Value);
            EventBus.Subscribe<PingEvent>(e => b += e.Value);
            EventBus.Subscribe<OtherEvent>(e => other += e.Value);

            EventBus.Raise(new PingEvent { Value = 5 });

            Assert.That(a, Is.EqualTo(5));
            Assert.That(b, Is.EqualTo(5));
            Assert.That(other, Is.EqualTo(0), "different event type must not receive the raise");
            Assert.That(EventBus.SubscriberCount<PingEvent>(), Is.EqualTo(2));
        }

        [Test]
        public void Unsubscribe_StopsDelivery()
        {
            int hits = 0;
            System.Action<PingEvent> handler = e => hits++;
            EventBus.Subscribe(handler);
            EventBus.Raise(new PingEvent());
            EventBus.Unsubscribe(handler);
            EventBus.Raise(new PingEvent());

            Assert.That(hits, Is.EqualTo(1));
        }

        [Test]
        public void Raise_HandlerUnsubscribesItself_MidDispatch_DoesNotThrowOrSkipOthers()
        {
            int secondHits = 0;
            System.Action<PingEvent> selfRemoving = null;
            selfRemoving = e => EventBus.Unsubscribe(selfRemoving);
            EventBus.Subscribe(selfRemoving);
            EventBus.Subscribe<PingEvent>(e => secondHits++);

            Assert.DoesNotThrow(() => EventBus.Raise(new PingEvent()));
            Assert.That(secondHits, Is.EqualTo(1));

            // Second raise: self-remover is gone, other handler still fires.
            EventBus.Raise(new PingEvent());
            Assert.That(secondHits, Is.EqualTo(2));
        }

        [Test]
        public void Raise_HandlerSubscribesNewHandler_MidDispatch_NewHandlerFiresNextRaiseOnly()
        {
            int lateHits = 0;
            System.Action<PingEvent> late = e => lateHits++;
            EventBus.Subscribe<PingEvent>(e => EventBus.Subscribe(late));

            EventBus.Raise(new PingEvent());
            Assert.That(lateHits, Is.EqualTo(0), "mid-dispatch subscribe must not fire in the same raise");

            EventBus.Raise(new PingEvent());
            Assert.That(lateHits, Is.EqualTo(1));
        }

        [Test]
        public void Raise_RepeatedSteadyState_DeliversEveryTime()
        {
            int hits = 0;
            EventBus.Subscribe<PingEvent>(e => hits++);

            // Hammer the cached-snapshot path (the hot loop during time-skips).
            for (int i = 0; i < 1000; i++)
                EventBus.Raise(new PingEvent());

            Assert.That(hits, Is.EqualTo(1000));
        }

#if UNITY_EDITOR
        [Test]
        public void Raise_SlowerThanBudget_LogsWarning()
        {
            EventBus.Subscribe<PingEvent>(e => Thread.Sleep(5));

            LogAssert.Expect(LogType.Warning, new Regex(@"\[EventBus\] Slow event: PingEvent took .*ms"));

            EventBus.Raise(new PingEvent());
        }

        [Test]
        public void Raise_FastHandler_LogsNoWarning()
        {
            EventBus.Subscribe<PingEvent>(e => { });

            // No LogAssert.Expect: any warning here fails the test.
            EventBus.Raise(new PingEvent());
        }
#endif
    }
}
