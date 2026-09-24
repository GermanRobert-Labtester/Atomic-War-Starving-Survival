// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 30 — The Press
// Subsystem    : Broadsheet Press Ledger (stateful owner over the signed pure
//                PublicBroadsheetPressEngine)
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Print
{
    /// <summary>One archived press run: the shelter's written record of what it published.</summary>
    [Serializable]
    public sealed class PressedPublication
    {
        public string PublicationId { get; set; } = string.Empty;
        /// <summary>(int)PublicationKind — stored as an int so the archive survives enum reordering.</summary>
        public int Kind { get; set; }
        public string Headline { get; set; } = string.Empty;
        public int CopiesPrinted { get; set; }
        public int AudienceReachPermille { get; set; }
        public int MoraleStabilizationPermille { get; set; }
        public int Day { get; set; } = 1;

        public PublicationKind KindValue => (PublicationKind)Kind;

        public PressedPublication Clone() => new PressedPublication
        {
            PublicationId = PublicationId,
            Kind = Kind,
            Headline = Headline,
            CopiesPrinted = CopiesPrinted,
            AudienceReachPermille = AudienceReachPermille,
            MoraleStabilizationPermille = MoraleStabilizationPermille,
            Day = Day
        };
    }

    /// <summary>
    /// Persisted press state: the type tray consumables and the bound archive of
    /// what the shelter printed. This is the press room's own layer only; rumor
    /// facts stay with <c>RumorSystem</c> and morale stays with the survivors'
    /// canonical needs authority.
    /// </summary>
    [Serializable]
    public sealed class BroadsheetPressState
    {
        public int SchemaVersion { get; set; } = 1;
        public TypeTrayState TypeTray { get; set; } = new TypeTrayState();
        public List<PressedPublication> Publications { get; set; } = new List<PressedPublication>();
        public int CumulativeCopiesPrinted { get; set; }
        public int CumulativeReachPermille { get; set; }
        public int CumulativeMoraleStabilizedPermille { get; set; }
        public int DebunkPamphletsPrinted { get; set; }

        public BroadsheetPressState Clone() => new BroadsheetPressState
        {
            SchemaVersion = SchemaVersion,
            TypeTray = TypeTray?.Clone() ?? new TypeTrayState(),
            Publications = Publications != null
                ? Publications.ConvertAll(p => p?.Clone() ?? new PressedPublication())
                : new List<PressedPublication>(),
            CumulativeCopiesPrinted = CumulativeCopiesPrinted,
            CumulativeReachPermille = CumulativeReachPermille,
            CumulativeMoraleStabilizedPermille = CumulativeMoraleStabilizedPermille,
            DebunkPamphletsPrinted = DebunkPamphletsPrinted
        };
    }

    /// <summary>Bounded read model of the press ledger.</summary>
    public struct BroadsheetPressCensus
    {
        public int PublicationCount { get; }
        public int CumulativeCopiesPrinted { get; }
        public int AverageReachPermille { get; }
        public int TypePiecesAvailable { get; }
        public int TypeWearPermille { get; }
        public int InkReservoirPermille { get; }
        public int PaperStockPermille { get; }
        public int DebunkPamphletsPrinted { get; }
        /// <summary>True once worn type is degrading print quality.</summary>
        public bool IsTypeDegraded { get; }

        public BroadsheetPressCensus(
            int publicationCount, int cumulativeCopiesPrinted, int averageReachPermille,
            int typePiecesAvailable, int typeWearPermille, int inkReservoirPermille,
            int paperStockPermille, int debunkPamphletsPrinted, bool isTypeDegraded)
        {
            PublicationCount = publicationCount;
            CumulativeCopiesPrinted = cumulativeCopiesPrinted;
            AverageReachPermille = averageReachPermille;
            TypePiecesAvailable = typePiecesAvailable;
            TypeWearPermille = typeWearPermille;
            InkReservoirPermille = inkReservoirPermille;
            PaperStockPermille = paperStockPermille;
            DebunkPamphletsPrinted = debunkPamphletsPrinted;
            IsTypeDegraded = isTypeDegraded;
        }
    }

    /// <summary>
    /// Owns the mutable press-room state and delegates every physical and editorial
    /// calculation to the signed pure <see cref="PublicBroadsheetPressEngine"/>.
    /// It never owns rumor facts, shelter morale, or the archive's ink catalog.
    /// </summary>
    public sealed class BroadsheetPressLedger
    {
        /// <summary>Upper bound on the archived press archive, so a long campaign cannot grow without limit.</summary>
        public const int MaxArchivedPublications = 200;

        private readonly BroadsheetPressState _state;
        private int _sequence;

        public BroadsheetPressLedger(BroadsheetPressState? state = null)
        {
            _state = state ?? new BroadsheetPressState();
            if (_state.TypeTray == null) _state.TypeTray = new TypeTrayState();
            if (_state.Publications == null) _state.Publications = new List<PressedPublication>();
            _sequence = _state.Publications.Count;
        }

        public TypeTrayState Tray => _state.TypeTray;
        public IReadOnlyList<PressedPublication> Publications => _state.Publications;
        public int PublicationCount => _state.Publications.Count;

        public BroadsheetPressState CaptureState() => _state.Clone();

        public void RestoreState(BroadsheetPressState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
                throw new InvalidOperationException(
                    $"broadsheet press schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.TypeTray = saved.TypeTray?.Clone() ?? new TypeTrayState();
            _state.Publications = saved.Publications != null
                ? saved.Publications.ConvertAll(p => p?.Clone() ?? new PressedPublication())
                : new List<PressedPublication>();
            _state.CumulativeCopiesPrinted = Math.Max(0, saved.CumulativeCopiesPrinted);
            _state.CumulativeReachPermille = Math.Max(0, saved.CumulativeReachPermille);
            _state.CumulativeMoraleStabilizedPermille = Math.Max(0, saved.CumulativeMoraleStabilizedPermille);
            _state.DebunkPamphletsPrinted = Math.Max(0, saved.DebunkPamphletsPrinted);
            _sequence = _state.Publications.Count;
        }

        /// <summary>
        /// Executes one publication run and archives it. Blocked runs (resource
        /// shortage) consume nothing and are not archived, so the archive only
        /// holds publications the shelter actually produced.
        /// </summary>
        /// <param name="publicationId">Stable id; when blank a deterministic day/sequence id is used.</param>
        /// <param name="kind">Publication kind.</param>
        /// <param name="targetCopies">Requested copies (clamped by the engine to 1..5000).</param>
        /// <param name="compositorSkillPermille">Compositor skill (0..1000) from the canonical skill owner.</param>
        /// <param name="shelterPopulation">Shelter population used for the reach calculation.</param>
        /// <param name="headline">Headline recorded in the press archive.</param>
        /// <param name="day">Campaign day the run belongs to.</param>
        public PrintRunResult ExecuteRun(
            string? publicationId,
            PublicationKind kind,
            int targetCopies,
            int compositorSkillPermille,
            int shelterPopulation,
            string? headline = null,
            int day = 1)
        {
            var result = PublicBroadsheetPressEngine.ExecutePrintRun(
                _state.TypeTray, kind, targetCopies, compositorSkillPermille, shelterPopulation);

            if (result.BlockedByShortage || result.CopiesPrinted <= 0) return result;

            string id = string.IsNullOrWhiteSpace(publicationId)
                ? $"pub_d{day}_{_sequence:D4}"
                : publicationId!.Trim();

            _state.Publications.Add(new PressedPublication
            {
                PublicationId = id,
                Kind = (int)kind,
                Headline = headline ?? string.Empty,
                CopiesPrinted = result.CopiesPrinted,
                AudienceReachPermille = result.AudienceReachPermille,
                MoraleStabilizationPermille = result.MoraleStabilizationPermille,
                Day = day
            });
            // Bound the archive by dropping the oldest entry, never by rewriting history counters.
            if (_state.Publications.Count > MaxArchivedPublications)
                _state.Publications.RemoveRange(0, _state.Publications.Count - MaxArchivedPublications);

            _state.CumulativeCopiesPrinted += result.CopiesPrinted;
            _state.CumulativeReachPermille += result.AudienceReachPermille;
            _state.CumulativeMoraleStabilizedPermille += result.MoraleStabilizationPermille;
            if (kind == PublicationKind.Pamphlet) _state.DebunkPamphletsPrinted++;
            _sequence++;
            return result;
        }

        public PressedPublication? FindPublication(string publicationId) =>
            string.IsNullOrWhiteSpace(publicationId)
                ? null
                : _state.Publications.Find(p => string.Equals(p.PublicationId, publicationId, StringComparison.Ordinal));

        /// <summary>
        /// Replaces worn type with fresh foundry stock. Delegates to the engine and
        /// records no publication: resetting is maintenance, not a print run.
        /// </summary>
        public void RestoreTypeTray(int freshTypePiecesAdded) =>
            PublicBroadsheetPressEngine.RestoreTypeTray(_state.TypeTray, freshTypePiecesAdded);

        /// <summary>
        /// Restocks press consumables (ink, paper, type) from shelter stores. This
        /// is the press's own consumable pool, not a second inventory ledger.
        /// </summary>
        public void RestockConsumables(int inkPermille, int paperPermille, int freshTypePieces)
        {
            _state.TypeTray.InkReservoirPermille = Math.Clamp(_state.TypeTray.InkReservoirPermille + Math.Max(0, inkPermille), 0, 1000);
            _state.TypeTray.PaperStockPermille = Math.Clamp(_state.TypeTray.PaperStockPermille + Math.Max(0, paperPermille), 0, 1000);
            if (freshTypePieces > 0) RestoreTypeTray(freshTypePieces);
        }

        /// <summary>
        /// Returns the credibility correction a pamphlet run would apply to a rumor.
        /// Pure calculation: the host routes the result through the canonical
        /// <c>RumorSystem</c> record; this ledger never mutates rumor state.
        /// </summary>
        public int CalculateDebunkCorrection(int rumorStrengthPermille, int pamphletAudienceReachPermille, int evidenceQualityPermille) =>
            PublicBroadsheetPressEngine.CalculateRumorDebunkingCorrection(
                rumorStrengthPermille, pamphletAudienceReachPermille, evidenceQualityPermille);

        public BroadsheetPressCensus GetCensus()
        {
            int averageReach = _state.Publications.Count == 0
                ? 0
                : _state.CumulativeReachPermille / _state.Publications.Count;

            return new BroadsheetPressCensus(
                _state.Publications.Count,
                _state.CumulativeCopiesPrinted,
                averageReach,
                _state.TypeTray.TypePiecesAvailable,
                _state.TypeTray.TypeWearPermille,
                _state.TypeTray.InkReservoirPermille,
                _state.TypeTray.PaperStockPermille,
                _state.DebunkPamphletsPrinted,
                _state.TypeTray.TypeWearPermille >= PublicBroadsheetPressEngine.TypeWearDegradationThreshold);
        }

        public void Clear()
        {
            _state.TypeTray = new TypeTrayState();
            _state.Publications.Clear();
            _state.CumulativeCopiesPrinted = 0;
            _state.CumulativeReachPermille = 0;
            _state.CumulativeMoraleStabilizedPermille = 0;
            _state.DebunkPamphletsPrinted = 0;
            _sequence = 0;
        }
    }
}
