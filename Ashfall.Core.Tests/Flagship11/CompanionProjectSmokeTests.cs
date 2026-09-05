using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Flagship11;

/// <summary>
/// Proves the Flagship XI companion verification path (see
/// docs/plans/FLAGSHIP_XI_IMPLEMENTATION_LOG.md, Divergence D1): this project compiles
/// only Flagship11 tests against the real Core assembly and runs them with xunit.
/// Also pins the two base facts the milestone's math depends on.
/// </summary>
public class CompanionProjectSmokeTests
{
    [Fact]
    public void SeededRng_IsDeterministic()
    {
        var a = new SeededRng(12345);
        var b = new SeededRng(12345);

        Assert.Equal(a.Next(0, 1000), b.Next(0, 1000));
        Assert.Equal(a.NextDouble(), b.NextDouble());
    }

    [Fact]
    public void NeedsSystem_MoralePolarityIsInverted()
    {
        // SurvivorNeedsState.Morale is 0..100 where HIGHER = WORSE (NeedsSystem.cs:6-8).
        // Flagship XI morale-contagion math depends on this polarity; pin it here.
        var needs = new NeedsSystem();
        needs.Register(new SurvivorNeedsState { Id = "survivor_a" });

        var before = needs.Get("survivor_a")!.Morale;
        Assert.Equal(50f, before); // neutral

        needs.Modify("survivor_a", NeedKind.Morale, +10f);
        Assert.Equal(60f, needs.Get("survivor_a")!.Morale); // +delta == worse morale (more despair)

        needs.Modify("survivor_a", NeedKind.Morale, -25f);
        Assert.Equal(35f, needs.Get("survivor_a")!.Morale); // -delta == better morale (more hope)

        needs.Modify("survivor_a", NeedKind.Morale, +200f);
        Assert.Equal(100f, needs.Get("survivor_a")!.Morale); // clamped
    }
}
