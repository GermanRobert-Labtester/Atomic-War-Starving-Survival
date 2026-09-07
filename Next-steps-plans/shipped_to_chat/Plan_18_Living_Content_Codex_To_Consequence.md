# Plan 18 — Living Content: From Readable to Consequential

> **Wave:** Continuity Wave 1
> **Depends on:** 15A (a playable choice to hang content on) and 15C (the liveness gate).
> **Relationship to other plans:** this plan *replaces* generic content-expansion batches
> (`piagentsplans/104–129`, which add entries to already-live systems) for one wave. Adding
> entries to catalogs that produce no effect is how the numbers below got this large.
>
> **Theme:** ASHFALL has 411 catalogs and 4,808 authored definitions, and almost none of them
> change the simulation. The content pipeline measures *presence* (loader named, ids
> registered) and exempts the rest with a rationale string. This plan converts a bounded slice of
> authored content into state that changes, and changes the metric from "how much exists" to
> "how much bites".

---

## Evidence Inventory (from `artifacts/content-utilization.json`, generated `f4f06d2d`, 2026-08-30)

| Metric | Value | Meaning |
|---|---|---:|
| Total catalogs | **411** | — |
| Total definitions | **4,808** | — |
| Gameplay-consumed catalogs | **110** | 27% |
| Codex-only catalogs | **272** | 66% (all under the `narrative/` prefix exemption) |
| Catalogs with **zero** `consumerSystems` | **300** | covering **2,067 definitions** |
| Stage `DISCOVERED` only | **271** | found on disk, nothing else |
| Stage `QUERIED` | **133** | read, no effect |
| Stage `EFFECT_PRODUCED` | **4** | **four catalogs in the whole game change the simulation** |
| Evidence tier `RUNTIME` | **9** | 402 classifications are static grep, not observed at runtime |
| Gate verdict | `Actionable Priorities: 0 / 0 / 0 / 0 / 0` | the gate is **green** while the above is true |

### Authored content that is explicitly "no source evidence" (`exemptionId: exempt_no_source_evidence`) — 26 catalogs / 429 definitions

| Catalog | Defs | Loader named by the scanner | Real C# consumer |
|---|---:|---|---|
| `environmental_atmosphere_expansion.json` | 152 | `WeatherSystem` | **none** |
| `medical_texts.json` | 83 | `MedicalWardSystem` | **none** |
| `environmental_texts_expansion_05.json` | 36 | `NarrativeEncounterSystem` | **none** |
| `audio_logs_expansion_05.json` | 30 | `AudioConditionSystem` | **none** |
| `narrative_encounters_expansion.json` | 29 | `NarrativeEncounterSystem` | **none** |
| `journal_entries_expansion_05.json` | 28 | `JournalSystem` | **none** |
| `memorials_expansion_05.json` | 27 | `MemorialSystem` | **none** |
| `narrative_arc_events.json` | 15 | `NarrativeEncounterSystem` | **none** |
| `moral_choice_quest_stubs.json` | 10 | — | **none** |
| `trade_specialties.json`, `guilt_sources.json`, `cassette_sets.json`, `confession_secrets.json`, `damaged_map_zones.json`, `deep_lore_survivor_fields.json`, `antigravity_survivor_fields.json`, `final_wishes.json` … | 0 each (root-array shape) | assorted | **none / shape-mismatched** |

**Why the "0 defs" rows matter:** the scanner counts 0 definitions for these files, which means
their root JSON shape is not what the counter expects. Content is present in the authority and
**invisible to the gate**. Two failure modes in one bucket: unread *and* uncounted.

### Other verified facts

| Fact | Evidence |
|---|---|
| `echoes.json` (23 authored echoes with choices, conditions, minDay) has **no loader, no system, no UI** | `Assets/Ashfall.Core/Content/ContentExemption.cs` `exempt_echoes_future`: *"No loader or consumer exists yet"*, `ExpiryCondition = "When EchoSystem is implemented and wired"`; `ls Assets/Ashfall.Core \| grep -i echo` → nothing; stage `LOADED`(1) |
| 215 authored moral-choice quests are unreadable by a player | `moral_choice_quests.json` (65) + `moral_choice_quests_branching.json` (100) + `moral_choice_quests_expansion.json` (50), all stage `QUERIED`, consumer `MoralChoiceSystem`, whose resolver has **0 call sites** (`src/Main.MoralChoice.cs:91`) |
| `regional_supply` in `economy_goods.json` is a dead knob | `grep -rn "regionalSupply\|RegionalSupply" Assets/Ashfall.Core src/` → **0 hits** in any C# while the field exists in the data authority |
| `DATA_GAP_AUDIT.md` is partly stale — do not trust its orphan list blindly | it still lists `questline_master.json` as "ORPHAN — no C# loader", but `src/Main.Application.cs:392` constructs `QuestlineMasterCatalogLoader` and `Main.cs:42` holds `_questlineMaster`; 362 defs, stage QUERIED. Verify every row before acting |
| Narrative depth is exempted *by policy*, not by accident | `exempt_narrative_codex` rationale: *"flavor text served through the JournalCodex, not gameplay systems"* — a defensible call, but it means 272 catalogs are declared causally inert |

**Interpretation:** the game does not have a content shortage. It has a **consequence shortage**.
The right move is to link small, verified slices and then make the metric honest — not to author
more.

---

## Task 18A — Close one narrative chain end to end (the proof)

**Goal:** pick one authored content family and make it change survivor state, prove it at
runtime, and use what is learned to define the standard wiring pattern every future content
family must follow.

**Chain chosen:** `echoes.json` (23 authored echoes with `choices`, `conditions`, `minDay`) →
echo loader/system → journal + a real state effect (guilt/dose/morale/flag) → player surface →
gate. It is the smallest complete loop, it already has an `echo_` id prefix in the sanctioned
list, it is *explicitly* marked "when EchoSystem is implemented", and the dialog-graph lint skill
already expects `echo_` nodes.

**Files:** new `Assets/Ashfall.Core/Narrative/EchoSystem.cs` (+ `EchoCatalog.cs`),
`Assets/Ashfall.Core/Content/ContentExemption.cs`, `src/Main.Narrative.cs`,
`src/UI/JournalPanel.cs` / `EchoPanel` (reuse, do not add a console),
`SaveSectionRegistry`, `docs/narrative/`.

### Substeps

1. **Read the data first**: enumerate the actual field names in `echoes.json` — `id`, `title`,
   `body`, `choices[]`, `conditions[]`, `minDay`, `factionId` — and write the shape down before
   writing a DTO. Do not design the schema from the plan text.
2. **Author the entity** `EchoDefinition` in Core as a plain engine-agnostic DTO (snake_case JSON
   mapping, `schema_version` respected, no `JsonUtility`, no engine refs — Invariants 1 and 6).
3. **Author `EchoCatalogLoader`** next to the existing catalog loaders, reusing
   `SystemTextJsonSerializer` and the `CatalogDiagnostics.Warn(path, shape, ex)` pattern from
   `YearOfAshCatalogLoader`/`VerdictCatalogLoader` so a malformed file logs instead of vanishing
   (H4's fix is the template).
4. **Author `EchoSystem`** with: availability filter by `minDay` + condition predicates against
   the **flag ledger** (not a new condition language), a deterministic `ISeededRng` selection
   roll, `CaptureState/RestoreState` DTO, and one C# event per state change (`OnEchoSurfaced`,
   `OnEchoResolved`) so UI and audio can subscribe.
5. **Reuse the existing resolution idiom** — `ExpeditionEncounterBridge.ResolveChoice`,
   `DoorEncounterSystem.ResolveChoice`, `DutyRosterQuestRuntime.ResolveChoiceWithEffects`. Echo
   choices must land through the same effects applier, not a fourth implementation.
6. **Give each authored choice at least one real effect**: set/check a `flag_`, add a guilt record
   (consumed by `GuiltInsomniaSystem`), adjust morale through the existing channel, or reveal a
   map node. Reject any choice whose authored effect cannot be applied — fix the data or the
   applier, never log a no-op.
7. **Register persistence**: add the `echoes` section to `SaveSectionRegistry` and wire it into
   the campaign envelope; store goes through `SaveStoreHub` (`src/Host/SaveStoreHub.cs`) so the
   coverage gate requirement ("must delegate") is satisfied on day one. No hand-rolled envelope.
8. **Join the day loop**: implement `IDayAdvanceOwner` and register a new owner id (or attach to
   `narrative_quests_verdict` if the phase ordering already fits — prefer attaching), and emit
   `DayStateChangeEvent`s with the 17A vocabulary.
9. **Surface it in an existing screen**: echoes read as journal/codex entries with inline
   choices, reusing `JournalPanel`/`journal_detail` routes. Do **not** register a new
   `echo_console` panel — that is exactly the BUG-UI-002 failure mode.
10. **Play it**: subscribe `AudioEventBridge` to `OnEchoSurfaced` using an existing
    `radio_*`/`amb_*` cue (no new asset).
11. **Tests:** catalog load + malformed-log, availability windows (`minDay` boundaries), condition
    gating via flags, choice → effect applied exactly once, save round-trip with checksum,
    paired-seed determinism, and one integration test that a surfaced echo reaches the briefing
    feed.
12. **Remove the exemption** `exempt_echoes_future` from `ContentExemption.cs` — the expiry
    condition is now met. The gate must go from "deferred" to "consumed", not stay exempt.
13. **Update the narrative graph docs/lint** (`ashfall-dialog-graph-lint`) so `echo_` nodes are
    reachable from a real producer/consumer pair, then re-run
    `godot --headless --path . -- --data-integrity-selftest`.
14. **Write the pattern doc**: `docs/narrative/ECHO_WIRING_PATTERN.md` — the five artefacts a
    content family must ship (entity, loader, system, effect applier, section + owner) with links.
    Every future batch in `piagentsplans/` cites this file.
15. **Run the five-step verification checklist.**

**DoD:** 23 authored echoes become playable content that changes state, one exemption is
deleted, and the reusable pattern is documented.

---

## Task 18B — Retire the "no source evidence" bucket

**Goal:** convert 26 exempted catalogs / 429 definitions into either *wired*, *deleted*, or
*honestly quarantined*, and close the loophole that lets "the loader is named X" count as
evidence when no source mentions X.

**Files:** `Assets/Ashfall.Core/Content/ContentExemption.cs`,
`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`,
`src/Host/ContentUtilizationRuntimeCollector.cs`, the 26 catalog files,
`Ashfall.Core.Tests/ContentUtilization*Tests.cs`, `artifacts/content-utilization-baseline.json`.

### Substeps

1. **Tighten the definition of evidence** in the scanner: a catalog is `GAMEPLAY` only if the
   named consumer appears in *source* (declaration + construction or call), not in the scanner's
   own hardcoded lookup table. The table (`ContentUtilizationScanner.cs:229`, `:350`, `:516`,
   `:674`, `:788`) is self-attestation and must stop being sufficient evidence.
2. **Make `exempt_no_source_evidence` a failure, not a state**: after a grace baseline, any
   catalog landing in that bucket must fail the gate, with three legal exits — wire it, delete it,
   or file it with an owner, ticket, and *expiry condition* like echoes.json already has.
3. **Fix the "0 definitions" shape bug** first: files whose root is a bare array
   (`cassette_sets.json`, `guilt_sources.json`, `confession_secrets.json`,
   `damaged_map_zones.json`, `trade_specialties.json`, `final_wishes.json`,
   `deep_lore_survivor_fields.json`, `antigravity_survivor_fields.json`) are counted as empty and
   therefore invisible. The AGENTS.md note "bare-array root exempt" in
   `CatalogIntegrityValidatorTests` confirms the same blind spot exists in the integrity gate —
   fix both counters together.
4. **Wire the two biggest** (`environmental_atmosphere_expansion.json` 152,
   `medical_texts.json` 83) into systems that *already* consume the shape:
   atmosphere text → `WeatherSystem`/`StartingLevel` hazard flavour that appears in the briefing
   (17A) and the `amb_*`/weather cue choice (17C); medical texts → `MedicalWardSystem`
   procedures/autopsy readouts and the sick-list panel.
5. **Wire the encounter cluster** (`narrative_encounters_expansion.json` 29,
   `environmental_texts_expansion_05.json` 36, `narrative_arc_events.json` 15,
   `narrative_questlines.json` 8) into the same `NarrativeEncounter` path that the working
   `DoorEncounterSystem`/`FactionWarChainRunner` use — attach, don't rebuild.
6. **Wire the memory cluster** (`journal_entries_expansion_05.json` 28,
   `memorials_expansion_05.json` 27) into `JournalSystem` and `MemorialSystem`/`DeathQuality`
   (Plan 09 9C just landed `MemorialOutcome` + `IGriefSink`) so deaths read differently per
   survivor.
7. **Delete or restore `moral_choice_quest_stubs.json`** (10 defs, no loader at all): stubs with
   no consumer are the seed of the next false affordance. Either give them to the 15A resolver or
   remove them from the authority.
8. **Small-field sweep**: `trade_specialties.json`, `confession_secrets.json`,
   `final_wishes.json`, `damaged_map_zones.json`, `cassette_sets.json`,
   `*_survivor_fields.json` — each needs **one** consumer decision, recorded as a table in the PR:
   wired, or removed with a note in `docs/narrative/`.
9. **Add the runtime half**: only 9 classifications had `RUNTIME` evidence and `Stage Breakdown`
   shows `LOADED 3 / DESERIALIZED 0 / REGISTERED 0 / SELECTED 0`. Extend
   `ContentUtilizationRuntimeCollector` so the *actual* campaign boot (not a synthetic harness)
   records LOAD/DESERIALIZE/REGISTER/SELECT/EFFECT, and gate on SELECTED > baseline.
10. **Re-pin the baseline** `artifacts/content-utilization-baseline.json` with the new numbers and
    a monotonic rule: definitions-wired may only rise.
11. **Tests:** scanner unit tests for the new evidence rule (named-only consumer must not pass),
    root-array counting test, and one test per wired catalog family proving effect production.
12. **Re-run** `--content-utilization-selftest`, `--data-integrity-selftest`,
    `dotnet test`.
13. **Update `docs/data/DATA_GAP_AUDIT.md`** — mark stale rows resolved (it still calls
    `questline_master.json` an orphan) so no future agent re-audits from bad data.

**DoD:** `exempt_no_source_evidence` count = 0, and the same headline number (definitions that
produce an effect) is published before/after.

---

## Task 18C — Make data fields load-bearing: dead-knob sweep and gate

**Goal:** every authored field in a gameplay catalog either influences the simulation or stops
existing. Today `regional_supply` proves that "authored balance intent" can be silently inert,
which makes every balance sweep run against those knobs worthless.

**Files:** `Assets/StreamingAssets/Data/economy_goods.json`, `Assets/Ashfall.Core/Economy/MarketSystem.cs`,
`Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (+ `CatalogIntegrityCheckers.cs`),
`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`, new
`Ashfall.Core.Tests/DataFieldUtilizationTests.cs`.

### Substeps

1. **Prove the general case from the known one**: `regional_supply` (in
   `economy_goods.json`) has 0 C# references — write the failing test that asserts
   "every property present in a gameplay catalog appears in at least one Core/host source file",
   and let it enumerate the full offender list.
2. **Build the field-level inventory**: parse each gameplay catalog's root object keys +
   per-definition property keys; compare against C# DTO property names *and* against raw JSON
   string lookups (`GetRawText`/`JsonNode["key"]` styles), because a field can be read without a
   matching DTO property. Report both numbers to avoid false positives.
3. **Triage into four buckets**: `WIRE` (intent is clear, consumer exists nearby),
   `CONSUME-AS-TEXT` (surface in briefing/codex), `DELETE` (no intent), `POLICY` (schema
   metadata like `schema_version` — whitelisted).
4. **Wire the economy knobs first**: regional supply → `MarketSystem` price/band modifiers,
   reusing Plan 14A's `TradeEmbargoSystem` proposal only if it lands; otherwise implement the
   minimal `RegionalSupplyModel` inside `MarketSystem` (a price multiplier keyed by
   `region × good`), with the caravan and trade screens reading the same number.
5. **One knob, one test, one observable**: for each WIRE field, a test that flipping the value
   changes simulation output, plus a line in the UI or briefing where the player can see the
   effect. A knob nobody can observe is not load-bearing yet.
6. **Respect modding**: fields that are part of the public data contract get a note in the
   `ashfall-mod-contract` doc; deleting a published field is a breaking change and needs a
   `schema_version` bump (Invariant 6).
7. **Gate it**: add the check to `CatalogIntegrityValidator`'s tiers (it already validates
   ids/refs/ranges/uniqueness — field utilization is the missing sixth tier), wired into
   `--data-integrity-selftest` so CI enforces it rather than a one-off script.
8. **Baseline + ratchet**: store the current offender list as an explicit allow-file
   (`docs/data/DEAD_FIELDS_BASELINE.md`); the gate fails if the list grows, and each entry must
   name an owner and a plan/task.
9. **Clean the exemptions**: the new field gate must not be satisfiable by adding to the
   allow-file — cap the allow-file size at its landed count, same ratchet idiom as 15C.
10. **Rebalance what you wired**: after wiring supply knobs, run
    `ashfall-balance-sim`/`ashfall-equipment-balance` on the affected economy loop and record the
    before/after price curves — the point of wiring knobs is that balance work now has meaning.
11. **Docs**: `docs/data/CATALOG_REGISTRY.md` gains a "consumed fields" column per catalog.
12. **Run the checklist** and report the two numbers: dead fields found, dead fields remaining.

**DoD:** `regional_supply` demonstrably moves prices in-game; a CI tier fails any new authored
field with no consumer.

---

## Cross-Task Dependencies

```
15A (choice route) ──► 18A (echo chain reuses the same resolve/effect idiom)
                          │
17A (event vocabulary) ───┼──► 18A step 8 (echo emits DayStateChangeEvent)
                          │
                          └──► 18B (retire exemptions; needs 18A as the pattern)
                                   │
18C (field tier) ◄────────────────┘  independent, but only meaningful once 18B proves
                                       that "named a consumer" ≠ "consumed"
```

**Execution order:** 18A → 18B → 18C. 18A is deliberately small: it exists to define the
five-artefact pattern that 18B then applies 25 times. Do not start 18B before 18A lands — you
would be wiring to an undefined standard.

**Sequencing against expansion batches:** while any of 18A/18B is open, *pause* the
`piagentsplans` "expand catalog X from 7 to 15" style batches. New entries in a
`QUERIED`-stage catalog add dead text at the exact rate the gate calls acceptable.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --content-utilization-selftest   # definitions-wired must RISE
7. bash scripts/ci/triad-drift-gate.sh
8. bash scripts/ci/verify-fast.sh
9. narrative gates: ashfall-dialog-graph-lint + ashfall-narrative-continuity
```

---

## Estimated Effort & Impact

| Task | New Core | Host | Data | Tests | Player-visible payoff | Difficulty |
|---|---|---|---|---|---|---|
| 18A | 2 files | 2 | 1 (already authored) | 12–16 | 23 playable echoes that change state | Medium |
| 18B | 1 (scanner) | 1 | ~26 files | 8–12 | 429 authored definitions stop being invisible | Medium–High |
| 18C | 1 (validator tier) | 0 | 1–3 | 6–10 | Balance knobs actually move prices | Medium |

**Impact math:** 2,067 definitions currently sit in catalogs with zero consumers. This plan does
not try to wire all of them. It wires the top ~500, deletes the rest with a record, and changes
the gate so the number cannot silently grow again — which is what makes the *next* content wave
compound instead of pile up.

**Guardrails:** no new console panels, no new condition language, no new resolver implementation,
no new ids outside the sanctioned prefix list, no content authored before its consumer exists.
