// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Contract for the engine-agnostic feedback messaging service.
    /// Manages message resolution, parameter safety, deduplication, and routing.
    /// </summary>
    public interface IFeedbackService
    {
        FeedbackMessageCatalog Catalog { get; }
        FeedbackDeduplicator Deduplicator { get; }

        event Action<ResolvedFeedbackMessage>? OnFeedbackEmitted;
        event Action<ResolvedFeedbackMessage>? OnDiagnosticEmitted;

        bool Emit(FeedbackEvent evt, float currentTimeSeconds = 0f);
        ResolvedFeedbackMessage Resolve(FeedbackEvent evt);
    }
}
