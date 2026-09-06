# Plan 169 — Procedural Narrative Closeout

`QuestRuntimeCoordinator` provides one player-facing read model for static, expansion, dynamic, and procedural instances while leaving mature domain quest authorities in place. `ProceduralNarrativeSystem` selects structured templates from an immutable snapshot, keeps eligibility RNG-free, binds canonical actor/location IDs deterministically, applies cooldown and concurrency limits, rejects protected or unsatisfiable drafts, and preserves merge provenance.

The template catalog is `Assets/StreamingAssets/Data/quest_templates.json`. Generated state stores template IDs, localization keys, bindings, objective modules, deadlines, rewards, failure consequences, generation seed, and parent/child provenance. `ProceduralNarrativeSaveState` persists narrative metadata and quest runtime together.

Focused verification: `Plan169ProceduralNarrativeTests` passed 6/6. The catalog validator accepts forward follow-up references and rejects unknown references after collecting the complete catalog ID set.

Remaining integration work includes domain objective adapters, event-context collection from espionage/fluid incidents, player-facing quest UI, and typed consequence/reward routing.
