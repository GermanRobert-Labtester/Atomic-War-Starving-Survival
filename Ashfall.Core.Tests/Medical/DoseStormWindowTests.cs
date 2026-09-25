// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W2 — Dose storm window (focused suite; run alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W2-DOSE-STORM-WINDOW (cases AV.2).
//
// What is pinned here: the pure day→multiplier provider, the authored catalog
// rows, and the receiver contract (the ledger still owns the roll, the bands,
// and the anti-rad timing). The host seam is exercised through the runtime
// selftests; its contract is that it conditions exactly once, at one site.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DoseStormWindowTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static List<YearOfAshEventEntry> LoadEvents()
        {
            return YearOfAshCatalogLoader.LoadEvents(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        // ── Provider contract (AV.2 cases 1–4) ───────────────────────────────

        [Fact]
        public void ClearDay_IsIdentityMultiplier()
        {
            // Day 1 is outside the authored 180–360 window: neutral, never reduced.
            var provider = new FalloutWindowProvider(LoadEvents());
            Assert.Equal(FalloutWindowProvider.NeutralMultiplier, provider.MultiplierFor(1), 4);
            Assert.False(provider.IsInWindow(1));
            Assert.Null(provider.WindowFor(1));
        }

        [Fact]
        public void AuthoredWindow_RaisesMultiplierAboveNeutral()
        {
            // Day 300 carries the radioactive-flood row (the strongest authored band).
            var provider = new FalloutWindowProvider(LoadEvents());
            var window = provider.WindowFor(300);

            Assert.NotNull(window);
            Assert.True(window!.Multiplier > 1f);
            Assert.Equal("radioactive_flood", window.HazardType);
            Assert.Equal(300, window.Day);
            Assert.NotEmpty(window.EventId);
        }

        [Fact]
        public void EmptyProvider_IsNeutralEverywhere()
        {
            // Missing calendar ⇒ neutral on every day (fail-closed, never a discount).
            var empty = FalloutWindowProvider.Empty;
            for (int d = 180; d <= 360; d += 17)
            {
                Assert.Equal(1f, empty.MultiplierFor(d), 4);
                Assert.False(empty.IsInWindow(d));
            }
        }

        [Fact]
        public void WindowRadius_CoversAuthoredDayAndNeighbours()
        {
            var events = new List<YearOfAshEventEntry>
            {
                new YearOfAshEventEntry { id = "e_test", day = 200, hazardType = "radioactive_flood", exposureMultiplier = 1.8f }
            };
            var provider = new FalloutWindowProvider(events, radiusDays: 1);

            Assert.Equal(1.8f, provider.MultiplierFor(200), 4);
            Assert.Equal(1.8f, provider.MultiplierFor(199), 4);
            Assert.Equal(1.8f, provider.MultiplierFor(201), 4);
            Assert.Equal(1f, provider.MultiplierFor(202), 4);
        }

        [Fact]
        public void StrongerDayWins_WhenWindowsOverlap()
        {
            var events = new List<YearOfAshEventEntry>
            {
                new YearOfAshEventEntry { id = "weak", day = 300, hazardType = "h", exposureMultiplier = 1.2f },
                new YearOfAshEventEntry { id = "strong", day = 301, hazardType = "h2", exposureMultiplier = 1.9f }
            };
            var provider = new FalloutWindowProvider(events, radiusDays: 1);
            Assert.Equal(1.9f, provider.MultiplierFor(300), 4);
            Assert.Equal(1.9f, provider.MultiplierFor(301), 4);
        }

        [Fact]
        public void InvalidMultipliers_AreIgnored()
        {
            // NaN / zero / negative rows never create a window (no inverted pressure).
            var events = new List<YearOfAshEventEntry>
            {
                new YearOfAshEventEntry { id = "nan", day = 210, exposureMultiplier = float.NaN },
                new YearOfAshEventEntry { id = "zero", day = 211, exposureMultiplier = 0f },
                new YearOfAshEventEntry { id = "neg", day = 212, exposureMultiplier = -2f }
            };
            var provider = new FalloutWindowProvider(events);
            foreach (var e in events) Assert.Equal(1f, provider.MultiplierFor(e.day), 4);
        }

        [Fact]
        public void AuthoredMultipliers_AreClamped()
        {
            var events = new List<YearOfAshEventEntry>
            {
                new YearOfAshEventEntry { id = "runaway", day = 250, exposureMultiplier = 99f }
            };
            var provider = new FalloutWindowProvider(events);
            Assert.True(provider.MultiplierFor(250) <= 8f);
        }

        [Fact]
        public void Provider_IsDeterministic_TwoPass()
        {
            var a = new FalloutWindowProvider(LoadEvents());
            var b = new FalloutWindowProvider(LoadEvents());
            Assert.Equal(a.WindowDays(), b.WindowDays());
            for (int d = 180; d <= 360; d += 7)
                Assert.Equal(a.MultiplierFor(d), b.MultiplierFor(d), 5);
        }

        [Fact]
        public void WindowDays_AreAscending()
        {
            var provider = new FalloutWindowProvider(LoadEvents());
            var days = provider.WindowDays();
            Assert.NotEmpty(days);
            for (int i = 1; i < days.Count; i++)
                Assert.True(days[i] > days[i - 1], "window days must be strictly ascending");
        }

        // ── Authored data (AV.2: data-first slice) ───────────────────────────

        [Fact]
        public void Catalog_AnnotatesRadiologicalRows_AndLeavesOthersNeutral()
        {
            var events = LoadEvents();
            Assert.NotEmpty(events);

            int annotated = events.Count(e => e.exposureMultiplier > 1f);
            Assert.True(annotated >= 5, $"expected several authored exposure rows, found {annotated}");

            // Every non-annotated row stays neutral, so untouched content keeps its
            // previous behavior exactly.
            foreach (var e in events.Where(e => e.exposureMultiplier <= 1f))
                Assert.Equal(1f, e.exposureMultiplier, 4);
        }

        [Fact]
        public void Catalog_WindowStaysInsideCanonRange()
        {
            // The authored storm vocabulary is the 180–360 canon; no row may drift out.
            foreach (var e in LoadEvents())
                Assert.InRange(e.day, 180, 360);
        }

        // ── Receiver contract: the ledger still owns the roll (AA.2) ─────────

        [Fact]
        public void Ledger_StillOwnsBands_AndCumulative_UnderConditioning()
        {
            // Conditioning happens upstream: identical nominal + same day + different
            // window multiplier must still flow through BookReading's own rules, and
            // the ledger's band edges are untouched.
            var clear = new DoseLedgerSystem();
            var storm = new DoseLedgerSystem();

            clear.AssignDosimeter("s_a", "tag_1");
            storm.AssignDosimeter("s_b", "tag_1");

            var provider = new FalloutWindowProvider(LoadEvents());
            float nominal = 0.5f;
            var window = provider.WindowFor(300);
            Assert.NotNull(window);

            var rngClear = new Ashfall.Core.SeededRng(77);
            var rngStorm = new Ashfall.Core.SeededRng(77);
            clear.BookReading("s_a", 1, nominal, "walk", false, false, false, rngClear);
            storm.BookReading("s_b", 300, nominal * window!.Multiplier, "walk", false, false, false, rngStorm);

            Assert.True(storm.GetCumulative("s_b") > clear.GetCumulative("s_a"));
            // Band edges are the ledger's authority and were not modified by W2.
            Assert.Equal(DoseLedgerSystem.BandGreen, DoseLedgerSystem.BandFor(nominal));
        }
    }
}
