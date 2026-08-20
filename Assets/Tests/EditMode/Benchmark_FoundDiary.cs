using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Narrative;
using Unity.PerformanceTesting;
using System.Diagnostics;
using System;

public class Benchmark_FoundDiary
{
    [Test, Performance]
    public void Benchmark_DiscoverDiary()
    {
        var evt = new Event_FoundDiary();
        evt.OnDiaryFound += (f, o) => { };

        Measure.Method(() =>
        {
            evt.DiscoverDiary("f", "o", true);
        })
        .WarmupCount(10)
        .MeasurementCount(100)
        .IterationsPerMeasurement(10000)
        .Run();
    }
}
