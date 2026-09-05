# Epilogue Chronicle Builder Selection Model Audit

**Document ID:** `docs/endgame/EPILOGUE_BUILDER_SELECTION_AUDIT.md`
**Inspected Source:** `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`
**Classification:** Model A — Linear Default Sequence / Presentation Catalog

---

## 1. Runtime Selection Model Analysis

The user request required evaluating `EpilogueChronicleBuilder.cs` against three architectural models:

1. **Model A — Linear Default Sequence**: Every `default_slides` entry forms the canonical presentation sequence, deterministically sorted by `Order`.
2. **Model B — Conditional Slide Pool**: The builder filters slides using runtime campaign flags/predicates.
3. **Model C — Ending-Linked Composition**: Ending/epilogue records specify slide IDs or slide groups.

### Evidence from `EpilogueChronicleBuilder.cs`:
```csharp
public EpilogueChronicle Build(EpilogueChronicleInput input)
{
    if (input == null) throw new ArgumentNullException(nameof(input));
    var chronicle = new EpilogueChronicle
    {
        EndingKey = input.EndingKey ?? "unknown",
        GeneratedDay = input.Day,
        BuildSeed = input.BuildSeed,
        Title = TitleFor(input.EndingKey ?? "unknown"),
        Metrics = new List<EpilogueMetric>(input.Metrics ?? new List<EpilogueMetric>()),
        Slides = new List<EpilogueSlide>(input.Slides ?? new List<EpilogueSlide>()),
        FateCards = new List<SurvivorFateCard>(input.FateCards ?? new List<SurvivorFateCard>())
    };
    // Stable ordering: by slide index, then by survivor id.
    chronicle.Slides.Sort((a, b) => a.Order.CompareTo(b.Order));
    ...
    return chronicle;
}
```

### Architectural Finding:
- `EpilogueChronicleBuilder` performs **no internal filtering or condition evaluation**. It is a deterministic compositor and sorter.
- `epilogue_chronicle.json` defines `"default_slides"`, which represents the full 20-slide default sequence.
- Outcome-specific text and narrative resolution belong to Plan 89's `muster_epilogues.json` (`EndingDefinition.prose`), while `epilogue_chronicle.json` defines the visual pacing cards.
- Therefore, the runtime operates under **Model A (Linear Default Sequence / Presentation Catalog)**.

---

## 2. Pacing & Breadth Implications

Because all 20 slides participate in the default presentation catalog:
1. Every slide subject must be universally applicable across all ending paths (e.g. catastrophe, shelter, resources, losses, survivors, factions, radio, relics, choices, future).
2. No slide title may presuppose a specific faction victory (e.g. not "The Garrison Prevails", but "The Factions").
3. Slide 15 (`The Resolution`) serves as the dedicated visual presentation card for whichever authoritative Plan 89 ending outcome was earned.
