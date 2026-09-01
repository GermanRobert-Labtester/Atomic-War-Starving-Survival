# Plan 40 — Authored Personality, Not Inferred: Wire the Survivor Identity Layer

> **Wave:** Continuity Wave 6 — *The People In It* (Plans 40–44)
> **Predecessors:** [W1](Wave1_Continuity_Audit_INDEX.md) story · [W2](Wave2_Continuity_Audit_INDEX.md)
> physics · [W3](Wave3_Continuity_Audit_INDEX.md) ship · [W4](Wave4_Continuity_Audit_INDEX.md) world ·
> [W5](Wave5_Continuity_Audit_INDEX.md) interface.
> **Depends on:** 36A (the port contract — this plan's whole job is plugging seams in), 24A (fitness
> verdict as the consumer of personality), 25A (keyed text so voice lines aren't hardcoded).
>
> **Theme:** someone wrote 72 survivor belief profiles, keepsakes, phantom backgrounds and
> professions into the data authority, and a Core catalog class with query methods for exactly that.
> Nothing loads it. Instead the host **guesses** each survivor's belief from traits with a method
> literally named `InferBeliefProfile` and commented "best-effort mapping". Ideological friction —
> the thing that should make a shelter feel like people who disagree — therefore runs on invented
> data, while the authored data sits unread.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The personality layer is authored | `expansion_survivor_fields.json` → 72 `items` with `belief_profile_id`, `keepsake`, `phantom_background`, `profession`; also `deep_lore_survivor_fields.json` (4), `antigravity_survivor_fields.json` (11, `manifesto_law_code` + stance), `expansion_item_tags.json` (67 `item_tags`) |
| 2 | The Core catalog for it exists, with queries | `Assets/Ashfall.Core/ExpansionEnrichmentCatalog.cs` — `AddSurvivorFields` (`:103`), `GetSurvivorsByBeliefProfile` (`:64`), `GetSurvivorsByPhantomBackground` (`:78`); header: *"belief profiles, keepsakes, phantom backgrounds, professions … and item tags"* |
| 3 | **The catalog has zero consumers** | `grep -rn "ExpansionEnrichment" src/ Assets/Ashfall.Core` → only its own file. No loader call, no host wiring, no test |
| 4 | **The host invents the same data instead** | `src/Main.SurvivorSocial.cs:36` comment *"Register beliefs from survivor catalog traits (best-effort mapping)"*, `:43` `string belief = InferBeliefProfile(def);`, `:60` `private static string InferBeliefProfile(SurvivorDefinition? def)` |
| 5 | …and the belief does feed a live chain once registered | `SurvivorSocialCoordinator.cs:158–162` → `Friction.RegisterBelief(...)`; `IdeologicalFrictionSystem.cs:59`; and friction **is** consequential: `OnAffinityChanged → Relations` (coordinator `:35–40` of that block) |
| 6 | The base survivor authority already has identity fields | `survivors.json` → 129 definitions with `profession`, `bio`, `traitIds`, `traits`, `baseTraits`, `latentExpertTrait`, `isChild`, `activeQuestlineId` — so `profession` exists in **two** places (base + enrichment) with the enrichment copy unread: a forked fact |
| 7 | Keepsake identity is string-parsed, not declared | `Shelter/ShelterDecorSystem.cs:231,262` — recognises keepsakes by matching the item-id shape `"item_personal_keepsake_{survivor}_{kind}"` — no field, no reference check |
| 8 | The heirloom registry is test-only | `Narrative/DwellerHeirloomCatalog.cs` (*"The 30 Survivor Personal Keepsakes & Heirloom Registry"*) → referenced only by `Ashfall.Core.Tests/DwellerHeirloomCatalogTests.cs`; **no** `src/` consumer |
| 9 | The eulogy engine is referenced by nothing at all | `Journal/ProceduralEulogyEngine.cs` (103 lines) → `grep -rn "ProceduralEulogyEngine"` in `src/`, `Assets/`, and tests: **zero hits**. Dead shipped code for the game's most emotional beat |
| 10 | Item tags are authored and unused | `expansion_item_tags.json` (67 `item_id → tags`) has no consumer, while `CraftingSystem.cs:336–337` hardcodes a medical item **id list** (`bandage`, `morphine`, `anti_rad`, `rad_away`, `antibiotics`, `iodine_pills`) — the exact thing tags were written to replace |
| 11 | The social coordinator itself is healthy | `SurvivorSocialCoordinator.cs:66–70` takes `NeedsSystem`, `SurvivorRelationsSystem`, `DutyRosterSystem`; `:113–119` leadership → `_needs.Modify(Morale)`; `:21–33` trauma bond → affinity + same-shift; `:41–46` ration conflict → morale; and it **is** displayed (`Main.SurvivorSocial.cs:57,122` → `SurvivorRelationsPanel.SetSocialReadModel`) — so this is a data-plumbing gap, not a broken system |

**Coordination:** parallel plans 132 (hidden agendas), 144 (autonomy), 147 (per-NPC memory), 148
(ideological friction events), 150 (romance/family), 154 (education) all propose **inner-life
content**. This plan supplies the identity plumbing they must sit on; running them first would
create six more bespoke `InferBeliefProfile`-style shims.

---

## Task 40A — Load the authored identity and delete the guesswork

**Goal:** every survivor's belief, profession, keepsake, and phantom background come from the
authority, are registered once at setup, and are provably not invented anywhere.

**Files:** new `Assets/Ashfall.Core/ExpansionEnrichmentCatalogLoader.cs`,
`ExpansionEnrichmentCatalog.cs`, `src/Main.SurvivorSocial.cs:36–58` (delete
`InferBeliefProfile`), `SurvivorSocialCoordinator.cs`, `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`,
survivor roster session, `docs/data/CATALOG_REGISTRY.md`, `Ashfall.Core.Tests/SurvivorIdentityTests.cs`.

### Substeps

1. **Resolve the `profession` fork first**: `survivors.json` has it, and
   `expansion_survivor_fields.json` re-declares it. Pick one authority (the base file), make the
   enrichment file *extend* rather than duplicate, and record the decision — a forked fact is a bug
   with a data file attached (Invariant 6).
2. **Add the loader** in Core with the established pattern (`SystemTextJsonSerializer`,
   `CatalogDiagnostics.Warn(path, shape, ex)` on parse failure, missing file → silent-empty *only if
   genuinely optional*, per H4's resolution).
3. **Register at setup**: in `SetupSurvivorSocial`, load the catalog and call
   `RegisterBelief(survivorId, belief_profile_id)` from data; delete `InferBeliefProfile` outright —
   a heuristic that shadows authored data will always win by silence.
4. **Define precedence for missing data**: if a survivor has no authored belief, the friction system
   should have a documented default (or that survivor should be excluded from friction), never a
   silent empty string that makes them inert (the current `beliefProfileId ?? string.Empty` path at
   `:162`).
5. **Validate coverage as a data rule**: every `belief_profile_id` referenced by a survivor resolves
   to a defined profile; every profile is used by ≥1 survivor (dead profiles get deleted) — add both
   to `CatalogIntegrityValidator`'s tiers so the id rules are mechanical.
6. **Wire the other three fields** to their destinations, one line each: `phantom_background` →
   `phantom_memory` surface (Plan 41), `keepsake` → heirloom/decor (Plan 41), `profession` → duty
   role suitability (24A) and apprentice/mentor pairing (`ApprenticeshipSystem`).
7. **Report the change of state** through 31's event vocabulary (`belief_registered`,
   `friction_ignited`) so the first briefing difference is visible proof this landed.
8. **Keep the save shape stable**: beliefs are config-like, not state — do not persist what the
   authority provides, and prove a save from before this change still loads (no new required field).
9. **Remove the double registration path**: `Main.SurvivorSocial.cs:51–54` restores social state
   after registering beliefs; confirm restore doesn't clobber authored beliefs (order-dependent
   overwrite is the classic bug in exactly this shape).
10. **Tests**: loader malformed-logs, per-survivor belief equals authored value, default handling,
    integrity tier catches an invented profile id, restore-doesn't-clobber, and a scan test asserting
    no `Infer*`/heuristic personality derivation exists in `src/`.
11. **Docs**: `docs/systems/SURVIVOR_IDENTITY.md` — the field table, precedence, and the
    "authored not inferred" rule; cite it from `AGENTS.md`'s identity/id rules.
12. **Run the checklist** + `--data-integrity-selftest`.

**DoD:** who believes what is read from a file, and no code can pretend otherwise.

---

## Task 40B — Item tags: replace hardcoded id lists with declared properties

**Goal:** the 67 authored `item_tags` become the mechanism every system currently fakes with an id
list — the second-most-common missing link in this codebase after unplugged ports.

**Files:** `expansion_item_tags.json`, `ExpansionEnrichmentCatalog.cs`,
`Assets/Ashfall.Core/Crafting/CraftingSystem.cs:336–337`, `Inventory/ItemDefinitions.cs`,
`ItemCatalogLoader.cs`, `TradeScreenPresenter` / `FactionStanceEngine`, `KitchenNutritionSystem`
(ingredient classes), `DecontaminationSystem`, `MedicalWardSystem`/`MedicalTreatmentCatalog`,
`PharmaLabSystem`, `ashfall-data-schema` / `CatalogIntegrityValidator`.

### Substeps

1. **Collect every id-list hack first** — start from `CraftingSystem.cs:336–337`
   (`id == "bandage" || "morphine" || "anti_rad" || "rad_away" || "antibiotics" || "iodine_pills"`),
   `SilentFoundryHostSession.cs:257` (hardcoded "known military faction IDs"),
   `ShelterDecorSystem.cs:231–262` (keepsake id-shape matching), `HoldfastTerminalPanel.cs:212`
   (food id list, Wave 2's 22A), and `AssetRegistry`'s item→icon maps. Publish the table; that table
   is the specification.
2. **Define the tag vocabulary in data** (one `tag_`/keyword list, snake_case, with a doc line per
   tag) so tags are enumerable, validated, and can't drift into free text.
3. **Load tags into the item catalog** as a first-class property (`tags` on `ItemDefinition`) so
   consumers ask `item.HasTag("medical_antidote")` instead of comparing ids.
4. **Replace each hack in its own commit**, with a test that the behaviour is unchanged *and* that
   adding a new item with the tag works without touching code (that second assertion is the point).
5. **Faction membership from data**: replace the hardcoded military-faction id list with the faction
   catalog's own alignment fields — same class of bug, different file.
6. **Keepsakes by reference, not by string shape**: `ShelterDecorSystem` should read the authored
   keepsake id from survivor identity (40A step 6) rather than parse `item_personal_keepsake_*`.
7. **Integrity tier**: every tag referenced by an item or a rule must be defined; every tag defined
   must be used by ≥1 item or ≥1 rule (unused tags are deleted, not tolerated).
8. **Mod-surface bonus** (deferred to the mod wave): tags are the natural extension point — a mod
   that adds `tag_food_preservable` should reach every consumer with zero code; add a test that
   proves it, using a fixture tag.
9. **Deprecate the id lists in `AGENTS.md` terms**: any doc that lists items by id for a behaviour
   now cites the tag instead, so agents stop writing new id checks (Wave 3's 29B).
10. **Tests**: tag resolution, per-consumer equivalence, integrity tiers, and a source-scan gate that
    fails a new literal id comparison where a tag exists (the mechanical end of this whole task).
11. **Run the checklist** + `--content-utilization-selftest` (the tags catalog should move from
    `exempt_no_source_evidence` to consumed).

**DoD:** behaviour keys off declared properties; the only remaining id lists are in test fixtures.

---

## Task 40C — Make identity observable: the player must be able to learn it

**Goal:** personality is only a system if the player can discover it. Beliefs, grudges, professions,
and keepsakes need in-fiction channels — and none of them may be a stat sheet.

**Files:** `SurvivorRelationsPanel.cs`, `src/UI/SurvivorDetailPanel.cs`,
`CaregivingPanel.cs`, `DutyRosterPanel.cs` (suitability hints), journal/codex
(`JournalSystem`, `docs/ui/JOURNAL_UI_PLAN.md`), 31's briefing entries, `ashfall-write`/`ashfall-narrative-check`
tone gates, 25A/25C text layer.

### Substeps

1. **Set the diegetic rule first**: the player learns personality through what survivors **say,
   refuse, notice, and mourn** — not through a numeric compatibility meter. Write that in
   `docs/systems/SURVIVOR_IDENTITY.md` and let every later step obey it.
2. **Relations read-model**: the panel already receives `BuildReadModel()`; verify the model carries
   *reasons* (friction source, bond origin, ration grievance) rather than only affinity values, and
   extend it if not.
3. **Duty suitability hints** (24A): a role shows "she was a fitter before" from `profession` —
   advisory, never blocking, and phrased in-world.
4. **Grievances surface where they act**: a ration conflict raises morale pressure (already wired)
   **and** a line in the briefing (31) and the journal, so the cause is findable.
5. **Belief conflict as a named event**: `friction_ignited` with the two survivor ids and the profile
   pair, so the player can trace a bad week back to seating two people together.
6. **Keepsakes in the world**: an item whose identity is "Vasquez's son's whistle" (from 40A/40B)
   appears in decor/memorial surfaces by name, not by id pattern.
7. **No omniscience**: the player shouldn't see every belief at t=0 — discovery through shared
   shifts, meals, crises, and caregiving (Plan 41/43's channels). Model knowledge state per pair,
   reusing 32C's knowledge ladder idea.
8. **Tone and variety guard**: line selection must avoid repetition fatigue (`ashfall-write`
   variation rules) and the cold/restrained register — no moralising captions about someone's belief.
9. **Accessibility**: belief/grudge conveyed by text and icon shape, never colour alone
   (`ashfall-ui-access`, Wave 5's 37B focus order).
10. **Tests**: read-model contains reasons; suitability hints are advisory only; friction events
    reach the briefing; a keepsake resolves to its owner's name; nothing in the UI reads a raw id
    string for meaning (assert no `StartsWith("item_personal_keepsake")` in UI code).
11. **Snapshots** for relations/survivor-detail panels with a populated cast.
12. **Run the checklist**.

**DoD:** a player who never opens a spreadsheet can still tell you who in their shelter would not
share a night shift with whom, and why.

---

## Cross-Task Dependencies

```
36A (port contract) ──► 40A (RegisterBelief is the pilot seam) ──► 40B (tags) ──► 40C (observable)
25A/25C (keyed text) ──► 40C steps 3–6 (no new inline prose)
31A (event kinds)     ──► 40A step 7, 40C step 5
24A (fitness verdict) ──► 40A step 6, 40C step 3
22A (single consume)  ──► 40B step 4 (the food id list goes away here)
   Plans 41–44 (memory, voice, governance, relations) all sit on 40A's identity layer
   parallel 132/144/147/148/150/154/159 should run AFTER 40A/40B, on this plumbing
```

**Execution order:** 36A → 40A → 40B → 40C → then Plans 41–44. Do not author new inner-life content
before 40A: each bespoke content pass invents its own derivation shim, and the next audit finds
six more `InferBeliefProfile`s.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors (+ new identity tiers)
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --content-utilization-selftest   # enrichment files: consumed
7. python3 scripts/ci/generate-port-contract.py --check          # (36A gate)
8. bash scripts/ci/triad-drift-gate.sh
9. ashfall-narrative-check on the observable layer               # tone + diegetic rule
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 40A | 1 new loader | 1 | 2 tiers | 0 | 10–13 | Low–Med | LOW (replaces a heuristic with data) |
| 40B | 2–3 | 4–6 | 1 | 0 | 12–16 | Medium | MEDIUM (equivalence tests per hack removed) |
| 40C | read-model | 1 | 0 | 4 | 8–12 | Medium | LOW |

**Guardrails:** no new survivor fields invented in code; no numeric compatibility meter in the UI;
no second source of `profession`/belief; no tag that only one consumer understands (vocabulary is
reviewed data); and when deleting `ProceduralEulogyEngine`/`DwellerHeirloomCatalog` inattention,
*use* them (Plan 41) rather than remove them — they are authored intent, not debris.
