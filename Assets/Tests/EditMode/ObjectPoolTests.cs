using NUnit.Framework;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// GenericObjectPool semantics: reuse without re-allocation, reset-on-release,
    /// capacity ceiling, and the pre-warm path that keeps first-use GC-free.
    /// </summary>
    [TestFixture]
    public class ObjectPoolTests
    {
        private sealed class Widget
        {
            public int Value;
            public string Label;
        }

        private static void ResetWidget(Widget w)
        {
            w.Value = 0;
            w.Label = null;
        }

        [Test]
        public void AcquireRelease_ReusesSameInstance_WithoutNewAllocations()
        {
            var pool = new GenericObjectPool<Widget>(() => new Widget(), ResetWidget);

            var first = pool.Acquire();
            first.Value = 42;
            first.Label = "hot";
            pool.Release(first);

            Assert.That(pool.InstancesCreated, Is.EqualTo(1));
            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.PooledCount, Is.EqualTo(1));

            var second = pool.Acquire();
            Assert.That(second, Is.SameAs(first), "released instance must be recycled, not re-allocated");
            Assert.That(pool.InstancesCreated, Is.EqualTo(1), "reuse must not create new instances");
            Assert.That(second.Value, Is.EqualTo(0), "reset hook must scrub stale state");
            Assert.That(second.Label, Is.Null);
        }

        [Test]
        public void Prewarm_CreatesInstancesUpFront()
        {
            var pool = new GenericObjectPool<Widget>(() => new Widget(), initialCapacity: 8);

            Assert.That(pool.InstancesCreated, Is.EqualTo(8));
            Assert.That(pool.PooledCount, Is.EqualTo(8));

            for (int i = 0; i < 8; i++)
                pool.Acquire();

            Assert.That(pool.InstancesCreated, Is.EqualTo(8), "acquiring pre-warmed stock must not allocate");
            Assert.That(pool.ActiveCount, Is.EqualTo(8));
        }

        [Test]
        public void MaxCapacity_AcquireReturnsNull_WhenExhausted()
        {
            var pool = new GenericObjectPool<Widget>(() => new Widget(), maxCapacity: 2);

            var a = pool.Acquire();
            var b = pool.Acquire();
            Assert.That(a, Is.Not.Null);
            Assert.That(b, Is.Not.Null);
            Assert.That(pool.Acquire(), Is.Null, "pool at cap must refuse to grow");

            pool.Release(a);
            Assert.That(pool.Acquire(), Is.SameAs(a), "released stock is acquirable again");
        }

        [Test]
        public void Release_NullOrOverRelease_IsIgnored()
        {
            var pool = new GenericObjectPool<Widget>(() => new Widget(), ResetWidget);
            var w = pool.Acquire();

            pool.Release(w);
            pool.Release(w); // double release: ActiveCount must not go negative
            pool.Release(null);

            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.PooledCount, Is.EqualTo(1));
        }

        [Test]
        public void ReleaseAll_ReturnsEveryLiveInstance()
        {
            var pool = new GenericObjectPool<Widget>(() => new Widget(), ResetWidget);
            var live = new[] { pool.Acquire(), pool.Acquire(), pool.Acquire() };

            pool.ReleaseAll(live);

            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.PooledCount, Is.EqualTo(3));
            Assert.That(pool.InstancesCreated, Is.EqualTo(3));
        }

        [Test]
        public void Churn_KeepsInstanceCountFlat()
        {
            var pool = new GenericObjectPool<Widget>(() => new Widget(), ResetWidget);

            // Warm to steady state.
            for (int i = 0; i < 4; i++)
                pool.Release(pool.Acquire());
            int createdAtSteadyState = pool.InstancesCreated;

            // Heavy acquire/release churn: nothing new may be created.
            for (int i = 0; i < 1000; i++)
            {
                var w = pool.Acquire();
                w.Value = i;
                pool.Release(w);
            }

            Assert.That(pool.InstancesCreated, Is.EqualTo(createdAtSteadyState),
                "steady-state churn must be allocation-free");
        }
    }
}
