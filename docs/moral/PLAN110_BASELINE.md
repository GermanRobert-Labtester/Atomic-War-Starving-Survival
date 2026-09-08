# Plan 110 — Baseline and Runtime Contract

## Repository findings

The live catalog is `Assets/StreamingAssets/Data/moral_choice_gossip.json`.
Its root contains `schema_version`, `description`, three gossip sections, and
`gossip_decay`.

The three sections are plain `List<string>` pools:

- `camp_chatter`
- `npc_greeting_shifts`
- `whisper_lines`

The canonical seven bands are:

`very_positive`, `positive`, `slightly_positive`, `neutral`,
`slightly_evil`, `evil`, and `very_evil`.

The repository initially had only six whisper-band fields. The missing
`whisper_lines.slightly_positive` field was a verified model/loader/runtime
gap: adding that one typed string list completes the existing seven-band
shape without introducing metadata, predicates, or a second runtime.

## Baseline counts

The original valid corpus contained 106 lines across 20 pools:

| Section | Band counts before expansion |
| --- | --- |
| `camp_chatter` | 10, 9, 5, 5, 5, 8, 10 |
| `npc_greeting_shifts` | 5, 4, 3, 4, 4, 4, 5 |
| `whisper_lines` | 6, 4, missing, 3, 3, 4, 5 |

The order in each existing pool was preserved. No legacy lines were replaced.

## Runtime contract

`MoralChoiceGossipCatalogLoader` maps the JSON arrays directly to the typed
catalog. `MoralChoiceGossipRuntime` selects the current effective moral band,
then calls the seeded RNG for a random index in the selected section.

- Repetition classification: **D0 — no repeat prevention**.
- RNG authority: injected `ISeededRng`.
- Runtime-owned gossip state: none.
- Saved line index/history: none.
- Save migration: not required.
- Selection bounds: current list count.
- Empty-pool fallback: empty string/list; final catalog makes all pools non-empty.

## Context contract

Context capability: **C2 — band-only selection**.

There is no external Plan 109/100 filter or interpolation token in the gossip
runtime. Therefore no new line claims that a particular echo outcome or faction
reaction occurred. Existing baseline references remain unchanged; all new
cross-plan-adjacent prose is thematic and safe for any campaign in its band.

Plan 109 and Plan 100 event-specific gossip remains deferred to their owning
contextual systems rather than being falsely marked as wired in this catalog.

## Baseline verification

- Moral-choice focused tests before expansion: **144 passed, 0 failed**.
- No Unity tooling was invoked.
