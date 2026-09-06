// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Failure and status codes for moral choice quest resolution.
    /// </summary>
    public enum MoralResolveResultCode
    {
        Success = 0,
        UnknownChoice = 1,
        ChoiceNotAvailable = 2,
        AlreadyResolved = 3,
        UnknownOption = 4,
        RequirementMissing = 5
    }

    /// <summary>
    /// Immutable result of an attempted moral choice resolution.
    /// </summary>
    public sealed class MoralResolveResult
    {
        public bool IsSuccess { get; }
        public MoralResolveResultCode Code { get; }
        public MoralChoiceResolution? Resolution { get; }
        public string Message { get; }

        public MoralResolveResult(
            bool isSuccess,
            MoralResolveResultCode code,
            MoralChoiceResolution? resolution,
            string message)
        {
            IsSuccess = isSuccess;
            Code = code;
            Resolution = resolution;
            Message = message ?? string.Empty;
        }

        public static MoralResolveResult Succeeded(MoralChoiceResolution resolution) =>
            new MoralResolveResult(true, MoralResolveResultCode.Success, resolution, "Resolution succeeded.");

        public static MoralResolveResult Failed(MoralResolveResultCode code, string message, MoralChoiceResolution? existing = null) =>
            new MoralResolveResult(false, code, existing, message);

        public override string ToString() =>
            $"[MoralResolveResult: Success={IsSuccess} Code={Code} Message='{Message}']";
    }
}
