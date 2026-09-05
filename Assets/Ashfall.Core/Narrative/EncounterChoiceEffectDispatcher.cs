using System;
using Ashfall.Core.Flags;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// F5 / Section 11 &amp; 12: Generic post-resolution consequence dispatcher.
    /// Pure, engine-agnostic coordinator that applies resolved choice side-effects
    /// (such as world flags) to canonical host authorities after successful
    /// resolution. Never mutates state on failed resolutions.
    /// </summary>
    public static class EncounterChoiceEffectDispatcher
    {
        public enum EffectStatus
        {
            NotApplicable,
            Applied,
            AlreadyKnown,
            SkippedNoAuthority
        }

        public readonly struct FlagApplicationResult
        {
            public readonly string FlagId;
            public readonly EffectStatus Status;

            public FlagApplicationResult(string flagId, EffectStatus status)
            {
                FlagId = flagId ?? string.Empty;
                Status = status;
            }

            public bool IsApplied => Status == EffectStatus.Applied;
            public bool IsSuccess => Status == EffectStatus.Applied || Status == EffectStatus.AlreadyKnown;
        }

        /// <summary>
        /// Applies the SetWorldFlagId from a successful resolution to the provided flag authority.
        /// Idempotent: setting an already-set flag returns Status.AlreadyKnown without side-effects.
        /// </summary>
        public static FlagApplicationResult ApplyWorldFlag(
            NarrativeEncounterResolutionResult? resolution,
            IFlagLedger? flags)
        {
            if (resolution == null || string.IsNullOrWhiteSpace(resolution.SetWorldFlagId))
            {
                return new FlagApplicationResult(resolution?.SetWorldFlagId ?? string.Empty, EffectStatus.NotApplicable);
            }

            string flagId = resolution.SetWorldFlagId.Trim();
            if (flags == null)
            {
                return new FlagApplicationResult(flagId, EffectStatus.SkippedNoAuthority);
            }

            bool alreadySet = flags.IsSet(flagId);
            flags.Set(flagId, NarrativeEncounterSystem.SystemId, resolution.ResolutionId, resolution.Day);

            return new FlagApplicationResult(
                flagId,
                alreadySet ? EffectStatus.AlreadyKnown : EffectStatus.Applied);
        }

        /// <summary>
        /// Overload for direct flag application with custom provenance.
        /// </summary>
        public static FlagApplicationResult ApplyWorldFlag(
            string? flagId,
            IFlagLedger? flags,
            string originSystem = NarrativeEncounterSystem.SystemId,
            string resolutionId = "",
            int day = 0)
        {
            if (string.IsNullOrWhiteSpace(flagId))
            {
                return new FlagApplicationResult(string.Empty, EffectStatus.NotApplicable);
            }

            string normalized = flagId.Trim();
            if (flags == null)
            {
                return new FlagApplicationResult(normalized, EffectStatus.SkippedNoAuthority);
            }

            bool alreadySet = flags.IsSet(normalized);
            flags.Set(normalized, originSystem, resolutionId, day);

            return new FlagApplicationResult(
                normalized,
                alreadySet ? EffectStatus.AlreadyKnown : EffectStatus.Applied);
        }
    }
}
