# Feedback Event Boundary Specification

## 1. Boundary Architecture

The feedback event boundary separates authoritative gameplay systems from presentation and UI rendering:

```
[ Domain Authorities ] ──(State / Outcome)──> [ Host Adapters / Main ]
                                                        │
                                                        ▼
                                             FeedbackEvent (DTO)
                                                        │
                                                        ▼
                                              [ FeedbackService ]
                                            (Catalog / Deduplicator)
                                                        │
                                                        ▼
                                             ResolvedFeedbackMessage
                                                        │
                                                        ▼
                                                [ FeedbackPanel ]
                                            (Godot Toast / Overlay)
```

---

## 2. FeedbackEvent DTO

Resides in `Assets/Ashfall.Core/Feedback/FeedbackEvent.cs`:

```csharp
namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Engine-agnostic data transfer object emitted when an authoritative
    /// gameplay event produces player-facing transient feedback.
    /// </summary>
    public sealed class FeedbackEvent
    {
        public string Key { get; }
        public object[] Arguments { get; }
        public string? Category { get; }
        public string? SourceSystem { get; }
        public string? DedupeKey { get; }
        public FeedbackSeverity? SeverityOverride { get; }
        public string? PresentationContext { get; }

        public FeedbackEvent(
            string key,
            object[]? arguments = null,
            string? category = null,
            string? sourceSystem = null,
            string? dedupeKey = null,
            FeedbackSeverity? severityOverride = null,
            string? presentationContext = null)
        {
            Key = key ?? string.Empty;
            Arguments = arguments ?? Array.Empty<object>();
            Category = category;
            SourceSystem = sourceSystem;
            DedupeKey = dedupeKey ?? key;
            SeverityOverride = severityOverride;
            PresentationContext = presentationContext;
        }
    }
}
```

---

## 3. ResolvedFeedbackMessage

Represents the rendered, formatted message ready for presentation:

```csharp
namespace Ashfall.Core.Feedback
{
    public sealed class ResolvedFeedbackMessage
    {
        public string Key { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public FeedbackSeverity Severity { get; init; } = FeedbackSeverity.Info;
        public string FormattedText { get; init; } = string.Empty;
        public float DisplayDurationSeconds { get; init; } = 3.0f;
        public string? DedupeKey { get; init; }
        public string? SourceSystem { get; init; }
        public long TimestampTick { get; init; }
    }
}
```

---

## 4. Invariants & Rules

1. **Unidirectional Flow:** Events flow from domain systems to the presentation queue. Feedback events cannot call domain APIs or trigger gameplay state changes.
2. **No Serialized Queue:** Transient feedback messages are ephemeral. Neither `FeedbackEvent` nor `ResolvedFeedbackMessage` is persisted in campaign saves.
3. **Safe Fallback:** If a key is missing from the catalog, `FeedbackService` produces a safe generic message (`"Status: {key}"` or category default) and logs a warning; it never throws an exception or crashes the UI.
4. **Parameter Guard:** Missing arguments are replaced with empty values or placeholders; extra arguments are safely ignored.
