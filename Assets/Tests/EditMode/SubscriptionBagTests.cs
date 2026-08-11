using System;
using NUnit.Framework;
using UnityEngine.TestTools;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class SubscriptionBagTests
    {
        private sealed class Publisher
        {
            public event Action OnFired;
            public int ListenerCount => OnFired?.GetInvocationList().Length ?? 0;
            public void Fire() => OnFired?.Invoke();
        }

        [Test]
        public void DisposeAll_Unsubscribes_TrackedHandler()
        {
            var pub = new Publisher();
            int fireCount = 0;
            Action handler = () => fireCount++;
            pub.OnFired += handler;

            var bag = new SubscriptionBag();
            bag.Track(() => pub.OnFired -= handler);

            pub.Fire();
            Assert.AreEqual(1, fireCount);

            bag.DisposeAll();
            pub.Fire();

            Assert.AreEqual(1, fireCount, "Handler should not fire after DisposeAll unsubscribes it.");
            Assert.AreEqual(0, pub.ListenerCount);
        }

        [Test]
        public void DisposeAll_Unsubscribes_AllTrackedHandlers_AcrossMultiplePublishers()
        {
            var pubA = new Publisher();
            var pubB = new Publisher();
            var bag = new SubscriptionBag();

            Action handlerA = () => { };
            Action handlerB = () => { };
            pubA.OnFired += handlerA;
            pubB.OnFired += handlerB;
            bag.Track(() => pubA.OnFired -= handlerA);
            bag.Track(() => pubB.OnFired -= handlerB);

            Assert.AreEqual(2, bag.Count);

            bag.DisposeAll();

            Assert.AreEqual(0, pubA.ListenerCount);
            Assert.AreEqual(0, pubB.ListenerCount);
            Assert.AreEqual(0, bag.Count, "Bag should clear its tracked list after disposal.");
        }

        [Test]
        public void DisposeAll_IsIdempotent_SecondCallDoesNothing()
        {
            var pub = new Publisher();
            Action handler = () => { };
            pub.OnFired += handler;

            var bag = new SubscriptionBag();
            bag.Track(() => pub.OnFired -= handler);

            bag.DisposeAll();
            Assert.DoesNotThrow(() => bag.DisposeAll());
            Assert.AreEqual(0, bag.Count);
        }

        [Test]
        public void DisposeAll_ContinuesPastThrowingUnsubscriber()
        {
            var pub = new Publisher();
            Action handler = () => { };
            pub.OnFired += handler;

            var bag = new SubscriptionBag();
            bag.Track(() => throw new InvalidOperationException("boom"));
            bag.Track(() => pub.OnFired -= handler);

            LogAssert.Expect(UnityEngine.LogType.Error,
                new System.Text.RegularExpressions.Regex(@"\[SubscriptionBag\] Unsubscribe #0 threw"));

            bag.DisposeAll();

            Assert.AreEqual(0, pub.ListenerCount, "A throwing unsubscriber must not block later ones from running.");
        }

        [Test]
        public void Track_IgnoresNullAction()
        {
            var bag = new SubscriptionBag();
            bag.Track(null);
            Assert.AreEqual(0, bag.Count);
            Assert.DoesNotThrow(() => bag.DisposeAll());
        }
    }
}
