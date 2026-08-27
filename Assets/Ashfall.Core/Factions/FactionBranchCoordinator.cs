// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Factions
{
    public enum FactionBranchKind
    {
        None = 0,
        Military = 1,
        Rebel = 2,
        Independent = 3
    }

    /// <summary>
    /// UI-ready descriptor of a faction branch option with availability and consequences.
    /// </summary>
    public sealed class FactionBranchOption
    {
        public string BranchId { get; set; } = string.Empty;
        public FactionBranchKind FactionKind { get; set; }
        public string FactionId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string PonrFlag { get; set; } = string.Empty;
        public string PonrTrigger { get; set; } = string.Empty;
        public string EntryBandMin { get; set; } = string.Empty;
        public string EntryBandMax { get; set; } = string.Empty;
        public bool IsCommitted { get; set; }
        public bool IsPonrLocked { get; set; }
        public bool IsAvailable { get; set; }
        public string? LockoutReason { get; set; }
        public List<string> PossibleEndings { get; set; } = new List<string>();
        public string ConsequencesSummary { get; set; } = string.Empty;
    }

    /// <summary>
    /// UI-ready summary of a faction's standing and alignment metrics.
    /// </summary>
    public sealed class FactionStandingSummary
    {
        public string FactionId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int Standing { get; set; }
        public int Alignment { get; set; }
        public bool IsHostile { get; set; }
        public bool IsAllied { get; set; }
        public bool IsJoined { get; set; }
        public bool IsOpposed { get; set; }
    }

    /// <summary>
    /// Unified coordinator for "The Weight of Choices" faction progression layer.
    ///
    /// Manages the four constituent systems:
    /// - MilitaryBranchSystem (8 Military branches, faction alignment, PoNR)
    /// - RebelBranchSystem (8 Rebel branches, faction alignment, PoNR)
    /// - IndependentBranchSystem (8 Independent branches, cross-faction relations, PoNR)
    /// - PrpfStandingSystem (PRPF third-power standing, alignment, join/oppose)
    ///
    /// Invariants:
    /// 1. Base faction commitment is strictly mutually exclusive: committing to one faction
    ///    locks out the others.
    /// 2. PRPF standing/alignment is durable and shared across all playthrough paths.
    /// 3. Zero engine coupling, fully deterministic, save/load safe via WeightOfChoicesSaveCodec.
    /// </summary>
    public sealed class FactionBranchCoordinator
    {
        public const string SystemId = "faction_branch_coordinator";

        private readonly MilitaryBranchCatalog _militaryCatalog;
        private readonly RebelBranchCatalog _rebelCatalog;
        private readonly IndependentBranchCatalog _independentCatalog;
        private readonly IFlagLedger _flags;
        private readonly ILog _log;

        public MilitaryBranchSystem Military { get; }
        public RebelBranchSystem Rebel { get; }
        public IndependentBranchSystem Independent { get; }
        public PrpfStandingSystem Prpf { get; }

        public event Action<FactionBranchKind, string>? OnBranchCommitted;
        public event Action<string, int>? OnPonrLocked;
        public event Action<string>? OnEndingResolved;
        public event Action? OnStateChanged;

        public FactionBranchCoordinator(
            MilitaryBranchCatalog? militaryCatalog = null,
            RebelBranchCatalog? rebelCatalog = null,
            IndependentBranchCatalog? independentCatalog = null,
            IFlagLedger? flags = null,
            ILog? log = null,
            MilitaryBranchSystemState? militaryState = null,
            RebelBranchSystemState? rebelState = null,
            IndependentBranchSystemState? independentState = null,
            PrpfSystemState? prpfState = null)
        {
            _flags = flags ?? new InMemoryFlagLedger();
            _log = log ?? NullLog.Instance;

            _militaryCatalog = militaryCatalog ?? MilitaryBranchCatalog.Empty();
            _rebelCatalog = rebelCatalog ?? RebelBranchCatalog.Empty();
            _independentCatalog = independentCatalog ?? IndependentBranchCatalog.Empty();

            Military = new MilitaryBranchSystem(_militaryCatalog, _flags, militaryState, _log);
            Rebel = new RebelBranchSystem(_rebelCatalog, _flags, rebelState, _log);
            Independent = new IndependentBranchSystem(_independentCatalog, _flags, independentState, _log);
            Prpf = new PrpfStandingSystem(_flags, prpfState, _log);

            WireEvents();
        }

        private void WireEvents()
        {
            Military.OnBranchCommitted += branchId =>
            {
                OnBranchCommitted?.Invoke(FactionBranchKind.Military, branchId);
                OnStateChanged?.Invoke();
            };
            Military.OnPonrLocked += (branchId, day) =>
            {
                OnPonrLocked?.Invoke(branchId, day);
                OnStateChanged?.Invoke();
            };
            Military.OnEndingResolved += endingId =>
            {
                OnEndingResolved?.Invoke(endingId);
                OnStateChanged?.Invoke();
            };
            Military.OnAlignmentChanged += _ => OnStateChanged?.Invoke();

            Rebel.OnBranchCommitted += branchId =>
            {
                OnBranchCommitted?.Invoke(FactionBranchKind.Rebel, branchId);
                OnStateChanged?.Invoke();
            };
            Rebel.OnPonrLocked += (branchId, day) =>
            {
                OnPonrLocked?.Invoke(branchId, day);
                OnStateChanged?.Invoke();
            };
            Rebel.OnEndingResolved += endingId =>
            {
                OnEndingResolved?.Invoke(endingId);
                OnStateChanged?.Invoke();
            };
            Rebel.OnAlignmentChanged += _ => OnStateChanged?.Invoke();

            Independent.OnBranchCommitted += branchId =>
            {
                OnBranchCommitted?.Invoke(FactionBranchKind.Independent, branchId);
                OnStateChanged?.Invoke();
            };
            Independent.OnPonrLocked += (branchId, day) =>
            {
                OnPonrLocked?.Invoke(branchId, day);
                OnStateChanged?.Invoke();
            };
            Independent.OnEndingResolved += endingId =>
            {
                OnEndingResolved?.Invoke(endingId);
                OnStateChanged?.Invoke();
            };
            Independent.OnMilitaryStandingChanged += _ => OnStateChanged?.Invoke();
            Independent.OnRebelStandingChanged += _ => OnStateChanged?.Invoke();

            Prpf.OnStandingChanged += _ => OnStateChanged?.Invoke();
            Prpf.OnAlignmentChanged += _ => OnStateChanged?.Invoke();
            Prpf.OnJoined += () => OnStateChanged?.Invoke();
            Prpf.OnOpposed += () => OnStateChanged?.Invoke();
        }

        public FactionBranchKind ActiveFactionKind
        {
            get
            {
                if (Military.State.branch.committed) return FactionBranchKind.Military;
                if (Rebel.State.branch.committed) return FactionBranchKind.Rebel;
                if (Independent.State.branch.committed) return FactionBranchKind.Independent;
                return FactionBranchKind.None;
            }
        }

        public string? ActiveBranchId
        {
            get
            {
                return ActiveFactionKind switch
                {
                    FactionBranchKind.Military => Military.CommittedBranchId,
                    FactionBranchKind.Rebel => Rebel.CommittedBranchId,
                    FactionBranchKind.Independent => Independent.CommittedBranchId,
                    _ => null
                };
            }
        }

        public bool IsCommitted => ActiveFactionKind != FactionBranchKind.None;

        public bool IsPonrLocked
        {
            get
            {
                return ActiveFactionKind switch
                {
                    FactionBranchKind.Military => Military.IsPonrLocked,
                    FactionBranchKind.Rebel => Rebel.IsPonrLocked,
                    FactionBranchKind.Independent => Independent.IsPonrLocked,
                    _ => false
                };
            }
        }

        public string? ResolvedEndingId
        {
            get
            {
                return ActiveFactionKind switch
                {
                    FactionBranchKind.Military => Military.ResolvedEndingId,
                    FactionBranchKind.Rebel => Rebel.ResolvedEndingId,
                    FactionBranchKind.Independent => Independent.ResolvedEndingId,
                    _ => null
                };
            }
        }

        public int CurrentDay =>
            Math.Max(Military.CurrentDay, Math.Max(Rebel.CurrentDay, Independent.CurrentDay));

        public void AdvanceDay(int day)
        {
            if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
            Military.AdvanceDay(day);
            Rebel.AdvanceDay(day);
            Independent.AdvanceDay(day);
            OnStateChanged?.Invoke();
        }

        public bool CanCommit(string branchId, MoralChoiceSystem moralChoice, out string? reason)
        {
            if (string.IsNullOrEmpty(branchId))
            {
                reason = "missing_branch_id";
                return false;
            }
            if (moralChoice == null)
            {
                reason = "missing_moral_choice_system";
                return false;
            }

            var kind = DetectBranchKind(branchId);
            if (kind == FactionBranchKind.None)
            {
                reason = $"unknown_branch_id '{branchId}'";
                return false;
            }

            // Exclusivity check
            if (IsCommitted)
            {
                if (ActiveFactionKind != kind)
                {
                    reason = $"Already committed to {ActiveFactionKind} branch '{ActiveBranchId}'. Faction branches are mutually exclusive.";
                    return false;
                }
                if (string.Equals(ActiveBranchId, branchId, StringComparison.Ordinal))
                {
                    reason = null;
                    return true; // Already on this branch
                }
                reason = $"Already committed to branch '{ActiveBranchId}' in {ActiveFactionKind}.";
                return false;
            }

            var band = moralChoice.CurrentBand;

            if (kind == FactionBranchKind.Military)
            {
                var def = _militaryCatalog.GetById(branchId);
                if (def == null) { reason = "unknown_military_branch"; return false; }
                var min = ParseBand(def.entry_band_min);
                var max = ParseBand(def.entry_band_max);
                if (band < min || band > max)
                {
                    reason = $"Requires morality band between {min} and {max} (current: {band}).";
                    return false;
                }
            }
            else if (kind == FactionBranchKind.Rebel)
            {
                var def = _rebelCatalog.GetById(branchId);
                if (def == null) { reason = "unknown_rebel_branch"; return false; }
                var min = ParseBand(def.entry_band_min);
                var max = ParseBand(def.entry_band_max);
                if (band < min || band > max)
                {
                    reason = $"Requires morality band between {min} and {max} (current: {band}).";
                    return false;
                }
            }
            else if (kind == FactionBranchKind.Independent)
            {
                var def = _independentCatalog.GetById(branchId);
                if (def == null) { reason = "unknown_independent_branch"; return false; }
                var min = ParseBand(def.entry_band_min);
                var max = ParseBand(def.entry_band_max);
                if (band < min || band > max)
                {
                    reason = $"Requires morality band between {min} and {max} (current: {band}).";
                    return false;
                }
                if (def.requires_prpf_standing_min.HasValue && Prpf.Standing < def.requires_prpf_standing_min.Value)
                {
                    reason = $"Requires PRPF standing >= {def.requires_prpf_standing_min.Value} (current: {Prpf.Standing}).";
                    return false;
                }
                if (def.requires_hostile_to_military == true && !Independent.IsHostileToMilitary)
                {
                    reason = "Requires hostile standing with Military (standing <= -50).";
                    return false;
                }
                if (def.requires_hostile_to_rebel == true && !Independent.IsHostileToRebel)
                {
                    reason = "Requires hostile standing with Rebels (standing <= -50).";
                    return false;
                }
            }

            reason = null;
            return true;
        }

        public ActionResult CommitBranch(string branchId, MoralChoiceSystem moralChoice)
        {
            if (string.IsNullOrEmpty(branchId))
                return ActionResult.Failed("missing_branch_id", "branch.missing_id");
            if (moralChoice == null)
                return ActionResult.Failed("missing_moral_choice", "branch.missing_moral_choice");

            if (!CanCommit(branchId, moralChoice, out string? reason))
                return ActionResult.Blocked("commitment_blocked", reason ?? "branch.cannot_commit");

            var kind = DetectBranchKind(branchId);
            try
            {
                string committedId = kind switch
                {
                    FactionBranchKind.Military => Military.CommitBranch(branchId, moralChoice),
                    FactionBranchKind.Rebel => Rebel.CommitBranch(branchId, moralChoice),
                    FactionBranchKind.Independent => Independent.CommitBranch(branchId, moralChoice, Prpf),
                    _ => throw new InvalidOperationException($"Cannot commit to unknown faction kind for branch '{branchId}'.")
                };
                return ActionResult.Success($"branch.committed:{committedId}");
            }
            catch (Exception ex)
            {
                _log.Error($"FactionBranchCoordinator commit error: {ex.Message}");
                return ActionResult.Failed("commit_exception", ex.Message);
            }
        }

        public ActionResult LockPonr(int day)
        {
            if (!IsCommitted)
                return ActionResult.Blocked("not_committed", "branch.ponr_requires_commitment");
            if (IsPonrLocked)
                return ActionResult.Success("branch.ponr_already_locked");

            try
            {
                if (day > CurrentDay)
                    AdvanceDay(day);

                switch (ActiveFactionKind)
                {
                    case FactionBranchKind.Military:
                        Military.LockPointOfNoReturn();
                        break;
                    case FactionBranchKind.Rebel:
                        Rebel.LockPointOfNoReturn();
                        break;
                    case FactionBranchKind.Independent:
                        Independent.LockPointOfNoReturn();
                        break;
                }
                return ActionResult.Success("branch.ponr_locked");
            }
            catch (Exception ex)
            {
                return ActionResult.Failed("ponr_error", ex.Message);
            }
        }

        public ActionResult ResolveEnding(MoralChoiceSystem moralChoice)
        {
            if (!IsCommitted)
                return ActionResult.Blocked("not_committed", "branch.ending_requires_commitment");
            if (!IsPonrLocked)
                return ActionResult.Blocked("ponr_not_locked", "branch.ending_requires_ponr");

            try
            {
                string endingId = ActiveFactionKind switch
                {
                    FactionBranchKind.Military => Military.ResolveEnding(moralChoice),
                    FactionBranchKind.Rebel => Rebel.ResolveEnding(moralChoice),
                    FactionBranchKind.Independent => Independent.ResolveEnding(moralChoice),
                    _ => throw new InvalidOperationException("No active faction to resolve ending.")
                };
                return ActionResult.Success($"branch.ending_resolved:{endingId}");
            }
            catch (Exception ex)
            {
                return ActionResult.Failed("ending_error", ex.Message);
            }
        }

        public void ModifyStanding(string factionId, int delta)
        {
            if (string.Equals(factionId, PrpfIds.FactionId, StringComparison.Ordinal))
            {
                Prpf.ModifyStanding(delta);
            }
            else if (string.Equals(factionId, MilitaryBranchIds.FactionId, StringComparison.Ordinal))
            {
                Independent.ModifyMilitaryStanding(delta);
            }
            else if (string.Equals(factionId, RebelBranchIds.FactionId, StringComparison.Ordinal))
            {
                Independent.ModifyRebelStanding(delta);
            }
            OnStateChanged?.Invoke();
        }

        public void ShiftFactionAlignment(string factionId, int delta)
        {
            if (string.Equals(factionId, MilitaryBranchIds.FactionId, StringComparison.Ordinal))
            {
                Military.ShiftFactionAlignment(delta);
            }
            else if (string.Equals(factionId, RebelBranchIds.FactionId, StringComparison.Ordinal))
            {
                Rebel.ShiftFactionAlignment(delta);
            }
            else if (string.Equals(factionId, PrpfIds.FactionId, StringComparison.Ordinal))
            {
                Prpf.ShiftFactionAlignment(delta);
            }
            OnStateChanged?.Invoke();
        }

        public bool TryJoinPrpf(MoralChoiceSystem moralChoice)
        {
            bool ok = Prpf.TryJoin(moralChoice);
            if (ok) OnStateChanged?.Invoke();
            return ok;
        }

        public void OpposePrpf()
        {
            Prpf.Oppose();
            OnStateChanged?.Invoke();
        }

        public FactionBranchKind DetectBranchKind(string branchId)
        {
            if (string.IsNullOrEmpty(branchId)) return FactionBranchKind.None;
            if (_militaryCatalog.Contains(branchId) || branchId.StartsWith("branch_mil_", StringComparison.Ordinal))
                return FactionBranchKind.Military;
            if (_rebelCatalog.Contains(branchId) || branchId.StartsWith("branch_rebel_", StringComparison.Ordinal))
                return FactionBranchKind.Rebel;
            if (_independentCatalog.Contains(branchId) || branchId.StartsWith("branch_ind_", StringComparison.Ordinal))
                return FactionBranchKind.Independent;
            return FactionBranchKind.None;
        }

        public IReadOnlyList<FactionBranchOption> GetBranchOptions(MoralChoiceSystem? moralChoice)
        {
            var list = new List<FactionBranchOption>();

            // 1. Military branches
            foreach (var b in _militaryCatalog)
            {
                bool isComm = string.Equals(ActiveBranchId, b.id, StringComparison.Ordinal);
                string? reason = null;
                bool avail = moralChoice != null && CanCommit(b.id, moralChoice, out reason);
                if (moralChoice == null) reason = "no_moral_choice_data";

                var opt = new FactionBranchOption
                {
                    BranchId = b.id,
                    FactionKind = FactionBranchKind.Military,
                    FactionId = MilitaryBranchIds.FactionId,
                    DisplayName = b.display_name,
                    PonrFlag = b.ponr_flag,
                    PonrTrigger = b.ponr_trigger,
                    EntryBandMin = b.entry_band_min,
                    EntryBandMax = b.entry_band_max,
                    IsCommitted = isComm,
                    IsPonrLocked = isComm && IsPonrLocked,
                    IsAvailable = avail,
                    LockoutReason = avail ? null : reason,
                    ConsequencesSummary = "Aligns with Military Command. Locks out Rebel and Independent paths."
                };
                foreach (var e in b.endings)
                    opt.PossibleEndings.Add($"{e.display_name} ({e.band_min}..{e.band_max})");
                list.Add(opt);
            }

            // 2. Rebel branches
            foreach (var b in _rebelCatalog)
            {
                bool isComm = string.Equals(ActiveBranchId, b.id, StringComparison.Ordinal);
                string? reason = null;
                bool avail = moralChoice != null && CanCommit(b.id, moralChoice, out reason);
                if (moralChoice == null) reason = "no_moral_choice_data";

                var opt = new FactionBranchOption
                {
                    BranchId = b.id,
                    FactionKind = FactionBranchKind.Rebel,
                    FactionId = RebelBranchIds.FactionId,
                    DisplayName = b.display_name,
                    PonrFlag = b.ponr_flag,
                    PonrTrigger = b.ponr_trigger,
                    EntryBandMin = b.entry_band_min,
                    EntryBandMax = b.entry_band_max,
                    IsCommitted = isComm,
                    IsPonrLocked = isComm && IsPonrLocked,
                    IsAvailable = avail,
                    LockoutReason = avail ? null : reason,
                    ConsequencesSummary = "Aligns with the Wasteland Insurgency. Locks out Military and Independent paths."
                };
                foreach (var e in b.endings)
                    opt.PossibleEndings.Add($"{e.display_name} ({e.band_min}..{e.band_max})");
                list.Add(opt);
            }

            // 3. Independent branches
            foreach (var b in _independentCatalog)
            {
                bool isComm = string.Equals(ActiveBranchId, b.id, StringComparison.Ordinal);
                string? reason = null;
                bool avail = moralChoice != null && CanCommit(b.id, moralChoice, out reason);
                if (moralChoice == null) reason = "no_moral_choice_data";

                var opt = new FactionBranchOption
                {
                    BranchId = b.id,
                    FactionKind = FactionBranchKind.Independent,
                    FactionId = IndependentBranchIds.FactionId,
                    DisplayName = b.display_name,
                    PonrFlag = b.ponr_flag,
                    PonrTrigger = b.ponr_trigger,
                    EntryBandMin = b.entry_band_min,
                    EntryBandMax = b.entry_band_max,
                    IsCommitted = isComm,
                    IsPonrLocked = isComm && IsPonrLocked,
                    IsAvailable = avail,
                    LockoutReason = avail ? null : reason,
                    ConsequencesSummary = "Walks an unaligned wasteland path. Locks out Military and Rebel allegiance."
                };
                foreach (var e in b.endings)
                    opt.PossibleEndings.Add($"{e.display_name} ({e.band_min}..{e.band_max})");
                list.Add(opt);
            }

            return list;
        }

        public IReadOnlyList<FactionStandingSummary> GetFactionStandingSummaries()
        {
            return new List<FactionStandingSummary>
            {
                new FactionStandingSummary
                {
                    FactionId = MilitaryBranchIds.FactionId,
                    DisplayName = "Military Outposts",
                    Standing = Independent.MilitaryStanding,
                    Alignment = Military.MilitaryAlignment,
                    IsHostile = Independent.IsHostileToMilitary,
                    IsAllied = ActiveFactionKind == FactionBranchKind.Military,
                    IsJoined = ActiveFactionKind == FactionBranchKind.Military,
                    IsOpposed = ActiveFactionKind == FactionBranchKind.Rebel || Independent.IsHostileToMilitary
                },
                new FactionStandingSummary
                {
                    FactionId = RebelBranchIds.FactionId,
                    DisplayName = "Rebel Insurgency",
                    Standing = Independent.RebelStanding,
                    Alignment = Rebel.RebelAlignment,
                    IsHostile = Independent.IsHostileToRebel,
                    IsAllied = ActiveFactionKind == FactionBranchKind.Rebel,
                    IsJoined = ActiveFactionKind == FactionBranchKind.Rebel,
                    IsOpposed = ActiveFactionKind == FactionBranchKind.Military || Independent.IsHostileToRebel
                },
                new FactionStandingSummary
                {
                    FactionId = PrpfIds.FactionId,
                    DisplayName = "Peace Protection Forces (PRPF)",
                    Standing = Prpf.Standing,
                    Alignment = Prpf.Alignment,
                    IsHostile = Prpf.IsHostile,
                    IsAllied = Prpf.IsAllied,
                    IsJoined = Prpf.IsJoined,
                    IsOpposed = Prpf.IsOpposed
                }
            };
        }

        public WeightOfChoicesSave CaptureState()
        {
            return WeightOfChoicesSaveCodec.Capture(Military, Rebel, Independent, Prpf);
        }

        public void RestoreState(WeightOfChoicesSave save)
        {
            if (save == null) return;
            WeightOfChoicesSaveCodec.Restore(save, Military, Rebel, Independent, Prpf);
            OnStateChanged?.Invoke();
        }

        public static FactionBranchCoordinator LoadFromData(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json,
            IFlagLedger flags,
            ILog? log = null)
        {
            var milCatalog = MilitaryBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
            var rebCatalog = RebelBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
            var indCatalog = IndependentBranchCatalog.LoadAndRegister(dataDir, fileIO, json);

            return new FactionBranchCoordinator(
                milCatalog,
                rebCatalog,
                indCatalog,
                flags,
                log);
        }

        private static MoralPathBand ParseBand(string s)
        {
            if (string.IsNullOrEmpty(s)) return MoralPathBand.Neutral;
            return s.ToLowerInvariant() switch
            {
                "very_evil" => MoralPathBand.VeryEvil,
                "evil" => MoralPathBand.Evil,
                "slightly_evil" => MoralPathBand.SlightlyEvil,
                "neutral" => MoralPathBand.Neutral,
                "slightly_positive" => MoralPathBand.SlightlyPositive,
                "positive" => MoralPathBand.Positive,
                "very_positive" => MoralPathBand.VeryPositive,
                _ => MoralPathBand.Neutral
            };
        }
    }
}
