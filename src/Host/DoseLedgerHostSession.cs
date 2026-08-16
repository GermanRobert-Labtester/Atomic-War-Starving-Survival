using System;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE DOSE — thin Godot-host session for the four dose registers.
    /// Wraps DoseLedgerSystem, SickListSystem, CohortSystem and VoluntaryRegisterSystem.
    /// No gameplay rules here — everything delegates to Ashfall.Core, following the
    /// ExpansionHostSession pattern. Persistence via DoseLedgerSaveStore.
    /// </summary>
    public sealed class DoseLedgerHostSession
    {
        public const int DemoSeed = 1401;

        public DoseLedgerSystem Ledger { get; }
        public SickListSystem SickList { get; }
        public CohortSystem Cohort { get; }
        public VoluntaryRegisterSystem Voluntary { get; }
        public DoseRegistersCatalog Registers { get; }
        public DoseContentCatalog Content { get; }

        private readonly SeededRng _rng;

        /// <summary>Raised when any register changes (coalesced save dirty flag).</summary>
        public event Action StateChanged;

        public DoseLedgerHostSession(
            DoseLedgerSystem ledger = null,
            SickListSystem sickList = null,
            CohortSystem cohort = null,
            VoluntaryRegisterSystem voluntary = null,
            DoseRegistersCatalog registers = null,
            DoseContentCatalog content = null)
        {
            Ledger = ledger ?? new DoseLedgerSystem();
            SickList = sickList ?? new SickListSystem();
            Cohort = cohort ?? new CohortSystem();
            Voluntary = voluntary ?? new VoluntaryRegisterSystem();
            Registers = registers ?? new DoseRegistersCatalog();
            Content = content ?? new DoseContentCatalog();
            _rng = new SeededRng(DemoSeed);

            // Persistence: any register mutation marks the save dirty.
            Ledger.OnStateChanged += _ => StateChanged?.Invoke();
            SickList.OnStateChanged += _ => StateChanged?.Invoke();
            Cohort.OnStateChanged += _ => StateChanged?.Invoke();
            Voluntary.OnStateChanged += _ => StateChanged?.Invoke();
        }

        public static DoseLedgerHostSession Create(string dataDir, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            var registers = new DoseRegistersCatalog();
            var content = new DoseContentCatalog();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                registers = DoseRegistersCatalogLoader.Load(dataDir, fileIO, serializer);
                content = DoseContentCatalogLoader.Load(dataDir, fileIO, serializer);
            }
            return new DoseLedgerHostSession(registers: registers, content: content);
        }

        // ── Cross-host save ──────────────────────────────────────────

        public DoseLedgerSave CaptureSave(int simDay) =>
            DoseLedgerSaveCodec.Capture(simDay, Ledger, SickList, Cohort, Voluntary);

        public void RestoreSave(DoseLedgerSave save) =>
            DoseLedgerSaveCodec.Restore(save, Ledger, SickList, Cohort, Voluntary);

        // ── Demo actions (drive the registers through real core APIs) ──

        /// <summary>Assign the well-known demo survivors dosimeter tags so readings can be booked.</summary>
        public void SealDemoSurvivors()
        {
            Ledger.AssignDosimeter("survivor_gunner_mikhail", "tag_1", 40f);
            Ledger.AssignDosimeter("elena_vasquez", "tag_2", 15f);
            Ledger.SetShieldingFactor("survivor_gunner_mikhail", 0.6f);
        }

        /// <summary>Book a nominal reading against the veteran; returns the band label.</summary>
        public string ScribeReading(float nominalMsv, bool highEnergy)
        {
            var rng = new CoreSeededRng(_rng.Next(0, int.MaxValue));
            var outcome = Ledger.BookReading(
                "survivor_gunner_mikhail", 40, nominalMsv, "demo_scan",
                highEnergy, antiRadBefore: false, antiRadAfter: false, rng);
            return $"Booked {nominalMsv} mSv → band {outcome} (cumulative {Ledger.GetCumulative("survivor_gunner_mikhail"):F1}).";
        }

        /// <summary>Name the veteran into a Sick List band.</summary>
        public string DiagnoseDemo(int band)
        {
            bool ok = SickList.Diagnose("survivor_gunner_mikhail", band, 40);
            return ok ? $"Diagnosed veteran as band {band}." : "Diagnosis failed.";
        }

        /// <summary>Book a Cohort child (guess), then correct the baseline.</summary>
        public string BookDemoChild()
        {
            bool booked = Cohort.BookChild("sv_cohort_demo", new[] { "survivor_gunner_mikhail" }, "low", 120, "told a kind number");
            if (!booked) return "Child already booked or invalid guess.";
            bool corrected = Cohort.CorrectBaseline("sv_cohort_demo", "medium");
            return corrected ? "Cohort child booked (guess: low) and corrected to medium." : "Child booked.";
        }

        /// <summary>Sign a volunteer, complete it, and bank the dose.</summary>
        public string SignDemoVolunteer()
        {
            bool signed = Voluntary.Volunteer("elena_vasquez", "vented reactor corridor", 44, "I walked the corridor before.");
            if (!signed) return "Volunteer registration failed.";
            bool done = Voluntary.CompleteVolunteer("elena_vasquez", "vented reactor corridor", 180f, 45);
            return done ? "Elena volunteered, completed the corridor, banked 180 mSv." : "Volunteer task open.";
        }

        // ── Status lines ─────────────────────────────────────────────

        /// <summary>Register the Dose quest lines into a QuestlineSystem (engine-agnostic
        /// graph). The host owns the QuestlineSystem instance; this only ingests content.</summary>
        public int RegisterContentQuests(QuestlineSystem questSystem)
        {
            if (questSystem == null || Content == null || Content.quests == null) return 0;
            int count = 0;
            foreach (var q in Content.quests)
            {
                if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
                questSystem.RegisterQuestline(q);
                count++;
            }
            return count;
        }

        /// <summary>One-line summary of what the Expansion 07 content bundle adds.</summary>
        public string ContentStatusLine()
        {
            return
                $"Dose content: {Content?.locations?.Count ?? 0} rooms, " +
                $"{Content?.items?.Count ?? 0} items, " +
                $"{Content?.quests?.Count ?? 0} quest lines";
        }

        public string LedgerLine()
        {
            var sb = new StringBuilder();
            sb.Append("Dose Ledger: ").Append(Ledger.Entries.Count).Append(" tagged · ").
                Append(Ledger.State.readingsSinceLastCalibration).Append(" since calibration").
                Append(Ledger.State.calibrationOverdue ? " · OVERDUE" : "");
            for (int i = 0; i < Ledger.Entries.Count; i++)
            {
                var e = Ledger.Entries[i];
                if (e == null) continue;
                int band = DoseLedgerSystem.BandFor(e.cumulativeMsv);
                sb.Append("\n  ").Append(e.survivorId).Append(": ").Append(e.cumulativeMsv.ToString("F1")).
                    Append(" mSv [band ").Append(band).Append("]");
            }
            return sb.ToString();
        }

        public string DoseStatusLine()
        {
            return
                $"Dose: {LedgerLine()}\n" +
                $"Sick: {SickList.Bands.Count} named\n" +
                $"Cohort: {Cohort.Children.Count} booked\n" +
                $"Voluntary: {Voluntary.Entries.Count} signed";
        }
    }

    /// <summary>A11: ISeededRng adapter delegates to the core SeededRng
    /// (deterministic xorshift64) — no System.Random in decision paths.</summary>
    internal sealed class CoreSeededRng : ISeededRng
    {
        private readonly SeededRng _rng;
        public int Seed { get; }
        public CoreSeededRng(int seed) { Seed = seed; _rng = new SeededRng(seed); }
        public int Next(int min, int max) => _rng.Next(min, max);
        public float NextFloat() => _rng.NextFloat();
        public double NextDouble() => _rng.NextDouble();
    }
}