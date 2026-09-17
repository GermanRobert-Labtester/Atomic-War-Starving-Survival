# D1 — Flagship Integration Plan [2]: The Shop Window as Build-Proven Product Evidence

> **Canonical filename:** `D1_planintegration[2].md`<br>
> **Previous file:** `D1_planintegration.md`<br>
> **Next file:** `D1_planintegration[3].md`<br>
> **Then:** `D1_planintegration[4].md`<br>
> **Naming rule:** keep each incrementing sequence number immediately before `.md`, inside square brackets, so every filename remains simple to copy/paste and sorts as one D1 integration-plan family.
>
> **Plan class:** Flagship integration / store-readiness / product-evidence / launch-operations plan<br>
> **Primary source:** Plan 57 — *The Shop Window: Screenshots, Statements, and a Store Page the Build Can Prove*<br>
> **Source wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window*<br>
> **Primary upstream dependency:** Plan 54 / D1 slice work, because the frozen seven-day slice is the canonical playable thing the public-facing material must depict.<br>
> **Integration intent:** expand 57A → 57B → 57C into an implementation-grade program that turns the actual build, its metrics, its accessibility/localization gates, its provenance data, its demo artifact, and its release processes into a truthful store/press/support surface.<br>
> **Source-preserving rule:** repository-state assertions below are inherited from the supplied Plan 57 unless explicitly marked **Integration-derived**. New tasks after 57C are follow-on work packages derived from the source's own requirements; they do not pretend to be previously verified repository findings.

---

## 0. Executive Directive

Plan 57 identifies a launch-readiness failure that is easy to underestimate because it lives outside ordinary runtime code. The repository can become technically sophisticated while the public-facing layer remains disconnected from reality. The source states that no store-facing structure exists, the AI disclosure remains a placeholder template, the only current rendered screenshots are QA artifacts, and dozens of attractive mockups represent systems that are not actually wired into the game. That means a rushed store launch would invite one of the most damaging classes of production error: publishing statements, imagery, capability claims, accessibility promises, or feature descriptions that the shipped build cannot prove.

This plan converts the store page from a manually authored marketing document into a **derived product artifact**. Screenshots must come from a real playable slice. Feature statements must trace to shipped system status. Accessibility and localization claims must be generated from gates and measurable status. AI/provenance disclosures must be generated from machine-readable registry data and reviewed for correctness. Demo and full-build artifacts must share release provenance. Launch operations, save compatibility, feedback handling, hotfix policy, known issues, and post-launch metrics must be tied into the engineering authority already established elsewhere in ASHFALL.

D1[2] therefore has six principal outcomes:

1. **A canonical store artifact chain.** A frozen scenario/build produces screenshots, a manifest, provenance records, feature evidence, support statements, and store-copy inputs.
2. **A claim-to-evidence contract.** Every substantive public claim resolves to a gate, metric, content-count report, runtime capability, legal/provenance record, or explicitly labelled design statement.
3. **Generated accessibility/localization truth.** The support statement says only what the currently tested build supports and automatically drops stale or unproven claims.
4. **A demo that cannot silently drift.** Demo scenario hash, build SHA, save root, settings contract, screenshots, and release scorecard are bound together.
5. **A repeatable launch and patch process.** Store page, demo, save promise, known issues, support escalation, hotfix rules, and patch notes become one operational system.
6. **A standing anti-misrepresentation gate.** Mockups cannot leak into screenshots, placeholder disclosures cannot ship, unsupported features cannot appear in feature lists, and stale statements fail the release process.

The governing principle is:

> **If the build, its data, its gates, or its release evidence cannot demonstrate a statement, the store page does not get to make that statement.**

This is not merely a documentation quality rule. It is product-integrity infrastructure.

---

## 1. Source Baseline and Integration Problem

### 1.1 Fixed source facts

The supplied Plan 57 establishes the following current-state evidence and D1[2] must treat it as the baseline rather than replacing it with assumptions:

- No canonical `store/`, `docs/store/`, `docs/press/`, `screenshots/`, or credits structure currently exists in the cited repository state.
- `docs/AI_DISCLOSURE.md` is explicitly a Steam questionnaire draft with bracketed placeholders still requiring completion.
- Existing PNG screenshots are QA golden/snapshot artifacts, not marketing captures.
- `assets/ui/Screens/` and `assets/ui/HtmlBundles/` contain attractive design mockups, including mockups for consoles that are not backed by runtime systems; they cannot be represented as gameplay screenshots.
- Asset provenance is already expected to become partly machine-readable through `asset_registry.json`, including source and AI-related metadata.
- Accessibility and input support are only publishable once the corresponding gates from the accessibility/input plans are real.
- Localization support is measurable and should be summarized from a generated status artifact rather than manually asserted.
- The frozen seven-day slice from Plan 54 is the correct source of truthful gameplay imagery.
- Balance/funnel/slice metrics provide defensible factual statements, while broad marketing superlatives do not.
- Linux and Windows export plumbing exists but store-branch build/boot integrity still depends on export-smoke verification.
- Achievements, legacy systems, commitments, and other feature-list candidates may only appear when the associated implementation plans are actually shipped.
- Content volume is countable, and that count should be generated from authoritative catalogs/utilization reports rather than copied into prose and forgotten.

### 1.2 Failure modes this plan is designed to prevent

D1[2] must explicitly defend against the following production failures:

1. A concept UI is accidentally uploaded as a store screenshot and implies a nonexistent mechanic.
2. A screenshot is captured from a developer-only staging scene whose state cannot occur in the shipping build.
3. Store copy claims controller, remapping, captions, text scaling, or keyboard-only support before the corresponding release gate passes.
4. A locale is listed publicly after translation coverage regresses or required strings become untranslated.
5. An AI disclosure is submitted with placeholders, stale counts, or incomplete provenance because someone completed it from memory.
6. A content-volume claim remains unchanged after catalogs are removed, merged, or marked unreachable.
7. A demo continues shipping a scenario version different from the screenshots or public description.
8. The full build and demo are exported from different commits while appearing under the same public release label.
9. A save-compatibility promise is broader than the tested save corpus actually supports.
10. Patch notes say a feature was added even though the corresponding acceptance/reachability gates never passed.
11. A store page remains stale across releases because there is no freshness policy for screenshots, claims, support statements, or technical requirements.
12. A support article promises recovery actions that no longer exist in the current settings or command surface.
13. A hotfix changes tuning without a balance sweep, ADR/decision record, or slice scorecard comparison.
14. A known issue is hidden because the launch checklist has no generated open-gate/audit register.
15. Marketing and runtime asset ownership become entangled, increasing build size or causing store-only art to enter gameplay packages unintentionally.
16. Rights/license documentation for fonts, SFX, music, icons, or AI-assisted assets cannot be reconstructed at submission time.
17. A first-party screenshot is correct at full resolution but illegible at store-card thumbnail size.
18. Accessibility text is technically true in a narrow selftest but misleading for an exported build or full slice.
19. An old screenshot survives a major UI redesign and depicts controls no longer available.
20. Multiple team/tools edit store copy by hand, producing conflicting statements about the same feature.

### 1.3 Integration definition of done

The whole D1[2] plan is complete only when all of these are true simultaneously:

- one command or documented pipeline can build the approved release candidate, boot it, run the canonical slice, capture the approved shot matrix, and emit a store-kit manifest;
- every screenshot is associated with scenario id/version/hash, build SHA, capture recipe, locale, resolution, and source artifact;
- `AI_DISCLOSURE.md` contains no unresolved placeholders and is reproducible from provenance data plus reviewed human-authorship policy;
- accessibility/localization/input claims are generated and linked to passing gates for the same build revision;
- feature claims resolve to an approved capability manifest rather than a free-form marketing list;
- content-volume claims resolve to generated reachable/selected/effect-producing counts;
- demo and full-game build provenance is traceable and the demo uses an isolated save root;
- a store-kit freshness gate can detect stale screenshots/statements after relevant runtime changes;
- release policy, support path, save-compatibility promise, mod-support posture, hotfix rules, and known-issues publication are represented as version-controlled artifacts;
- a dry-run launch can be executed without undocumented tribal knowledge;
- release gating fails on unresolved placeholders, unsupported public claims, provenance gaps, mockup leakage, stale demo metadata, or missing support artifacts.

---

## 2. Product-Evidence Architecture

### 2.1 Canonical artifact flow

```text
Authoritative game + data + asset registry
          │
          ├──────────────► capability/feature manifest
          │                     │
          │                     └──► store copy claim inputs
          │
          ├──────────────► provenance generator
          │                     ├──► AI disclosure
          │                     ├──► rights inventory
          │                     └──► press-kit provenance report
          │
          ├──────────────► accessibility/input/l10n gates
          │                     └──► generated support statement
          │
          └──────────────► frozen seven-day slice
                                │
                                ├──► export release candidate
                                ├──► export demo candidate
                                ├──► boot verification
                                ├──► canonical screenshot capture
                                └──► slice scorecard / metrics

All generated evidence + build ids
          │
          ▼
store/kit_manifest.json
          │
          ├──► screenshots
          ├──► capsules/banners metadata
          ├──► store-copy evidence map
          ├──► accessibility statement
          ├──► localization status
          ├──► provenance/AI disclosure
          ├──► technical requirements
          ├──► demo manifest
          ├──► known issues
          └──► support/release docs

store/kit_manifest.json + release gate
          │
          ▼
PUBLICATION ALLOWED / BLOCKED
```

### 2.2 Core contract: `StoreKitManifest`

**Integration-derived recommendation:** create one machine-readable manifest that binds every public-facing artifact to the exact build and evidence that produced it. Suggested logical fields:

```text
StoreKitManifest
  schemaVersion
  productId
  publicVersion
  gitSha
  buildTimestampUtc
  sourceBranch
  scenario:
    id
    version
    contentHash
    difficultyPreset
  builds:
    windowsArtifactDigest
    linuxArtifactDigest
    demoArtifactDigest
    exportPresetIds[]
    bootVerified
  screenshots[]:
    id
    filename
    sceneOrBeatId
    locale
    resolution
    textScale
    captureRecipeVersion
    imageDigest
    sourceBuildDigest
  claims[]:
    claimId
    publicTextKey
    evidenceKind
    evidenceId
    evidenceRevision
    status
  provenance:
    reportDigest
    aiDisclosureDigest
    unresolvedCount
  accessibility:
    statementDigest
    gateRunId
    supportedClaims[]
  localization:
    statusDigest
    locales[]
    coverageByLocale
  content:
    utilizationReportDigest
    countsByFamily
  rights:
    inventoryDigest
    blockingIssues
  knownIssues:
    reportDigest
    blockingIssues
  generatedAtUtc
```

The exact implementation language may differ, but the conceptual ownership should remain singular. A screenshot folder without this manifest is incomplete. A manifest whose build digest no longer exists is stale. A store statement whose evidence id cannot be resolved is invalid.

### 2.3 Claim taxonomy

Every public statement should be classified before publication:

| Claim class | Example shape | Required evidence |
|---|---|---|
| Runtime capability | “Full keyboard navigation” | passing release gate + exported-build test |
| Content quantity | “Over 100 broadcasts” | generated catalog/utilization count |
| Accessibility | “Reduce motion available” | setting exists + behavior gate |
| Localization | “Latvian UI available” | locale shipped + coverage threshold |
| Input support | “Controller supported” | mapped actions + end-to-end slice test |
| Performance | minimum/recommended spec | measured build + long-session/perf data |
| Save support | “Saves from X remain compatible” | save corpus + versioning policy |
| Mod support | “Data-pack mods supported” | published contract + fixture CI |
| AI/provenance | disclosure text | asset registry + human review |
| Design description | “Survival management after nuclear collapse” | game design/pillar authority; must not imply unbuilt mechanic |
| Metric-derived | “First-week survival…” | release scorecard/sweep for cited build |
| Roadmap/future | “Planned…” | must be explicitly labelled future and accepted by roadmap policy; preferably excluded from launch copy |

The default status for an unclassified statement is **blocked**.

### 2.4 Authority hierarchy

When two sources disagree, use this order:

1. Shipping build behavior and release-gate evidence.
2. Machine-readable authoritative manifests/data used by the build.
3. Generated reports tied to the same SHA.
4. Versioned design/release policy.
5. Reviewed store copy.
6. Free-form notes, mockups, pitch documents, old screenshots, and unpublished concepts.

A lower authority may never override a higher authority merely because its wording is more attractive.

---

## 3. Program Sequencing and Critical Path

### 3.1 Mandatory source order

The source defines `54A → 54C → 57A → 57B → 57C` as the critical order. D1[2] preserves that dependency. The seven-day slice must already be frozen and scorecard-capable before store material can truthfully derive from it.

### 3.2 Expanded implementation phases

**Phase 0 — Store-readiness reconnaissance**

- confirm current store/press/provenance/accessibility/l10n/release artifacts;
- establish an authority map and forbidden-input list;
- confirm the current seven-day slice id/hash and build pipeline;
- enumerate current placeholder text and incomplete disclosures;
- inventory mockup directories and explicitly mark them non-marketing/non-gameplay;
- record baseline build SHA and export preset identities.

**Phase 1 — Evidence contracts**

- implement/define the store kit manifest;
- implement the claim registry/evidence references;
- define screenshot recipe metadata;
- define provenance report inputs/outputs;
- define accessibility/localization statement schema;
- define demo/version binding.

**Phase 2 — 57A capture/provenance pipeline**

- export verified candidates;
- capture the real slice;
- generate screenshot metadata;
- generate provenance/AI disclosure;
- generate content counts and feature evidence;
- establish capsule/banner asset provenance and rights checks.

**Phase 3 — 57B truthful support statements**

- enumerate claims;
- resolve each claim to gates;
- generate accessibility/input/localization statement;
- derive technical requirements from measured performance;
- emit known limitations;
- add freshness enforcement.

**Phase 4 — 57C launch operations**

- bind demo/version/save promises;
- codify patch/hotfix cadence;
- publish feedback/support paths;
- publish mod posture and known issues;
- make release checklists executable;
- perform complete dry run.

**Phase 5 — Integration-derived hardening**

- store claim compiler;
- screenshot drift detection;
- rights/provenance closure;
- publication manifest sealing;
- store-platform submission matrix;
- demo/full-build parity gate;
- customer-support reproducibility pack;
- post-launch evidence review.

### 3.3 Stop-the-line conditions

Any one of the following blocks publication until resolved or explicitly waived under a written product decision:

- screenshot source cannot be traced to a boot-verified shipping/demo artifact;
- screenshot is identified as mockup/concept/staging-only;
- AI disclosure contains brackets/placeholders/TODOs;
- a public accessibility or input claim has no passing release gate;
- a locale is claimed but does not meet the defined coverage bar;
- a feature appears in public copy but is absent from the capability manifest;
- a numeric content claim lacks a generated report for the current SHA;
- demo scenario hash differs from the approved slice hash without a re-cut decision;
- full and demo builds advertised under one release have irreconcilable source revisions;
- rights inventory contains an unresolved commercial-distribution blocker;
- minimum/recommended specs are unmeasured guesses;
- save compatibility exceeds what the save corpus verifies;
- known release blocker is omitted from the public known-issues policy when policy requires disclosure;
- store kit is older than a configured set of relevant source changes and has not been regenerated;
- release checklist cannot identify who approved the kit and which evidence revision they reviewed.


---

## 4. Task 57A — Generate Store Assets From a Real Session, Never From a Mockup

### 4.1 Mission

57A turns marketing media into a reproducible build output. The implementation must make it easier to capture a truthful screenshot than an untraceable one. It must also create enough metadata that a future reviewer can answer, without relying on memory: “Which build produced this image? Which scenario state is this? Was the image altered? Which locale and text scale were used? Was this shot ever possible in the shipping build?”

The task is not complete when a folder contains attractive PNGs. It is complete when the repository contains a deterministic capture recipe, a build-bound manifest, a provenance report, a rights checklist, store-copy evidence references, and release gates that can reject stale or untraceable media.

### 4.2 Primary files and artifacts

Source-named files:

- new `scripts/store/capture_store_assets.py`;
- new `store/screenshots/`;
- new `store/press/`;
- new `store/capsule/`;
- `export_presets.cfg`;
- the 54A seven-day slice scenario;
- 50A `asset_registry.json`;
- `scripts/ci/generate-provenance-report.py`;
- `docs/AI_DISCLOSURE.md`;
- `store/README.md`;
- `store/CHECKLIST.md`.

**Integration-derived supporting artifacts:**

- `store/store_kit_manifest.json`;
- `store/capture_recipes.json`;
- `store/claims.json`;
- `store/rights_inventory.json`;
- `store/feature_matrix.json`;
- `store/technical_requirements.json`;
- `store/demo_manifest.json`;
- `store/generated/CONTENT_FACTS.md`;
- `store/generated/PROVENANCE_REPORT.md`;
- `store/generated/BUILD_EVIDENCE.md`;
- `scripts/store/validate_store_kit.py`;
- `scripts/store/check_mockup_leakage.py`;
- `scripts/store/check_claim_evidence.py`;
- `scripts/store/check_screenshot_freshness.py`;
- `Ashfall.Core.Tests/StoreKitContractTests.cs` where C# contract validation is preferable.

These names are recommendations, not source facts. If equivalent existing paths are found during implementation, extend them rather than creating parallel authorities.

### 4.3 57A preflight — establish what is and is not a valid source

1. Enumerate every image directory that could plausibly be mistaken for store material.
2. Classify each directory as:
   - `RUNTIME_CAPTURE_ALLOWED`;
   - `MARKETING_ART_SOURCE_ALLOWED`;
   - `QA_ONLY`;
   - `MOCKUP_ONLY`;
   - `WORKING_COPY_ONLY`;
   - `UNKNOWN_BLOCKED`.
3. Explicitly classify `snapshots/` and `snapshot-capture/` as QA, not store screenshots.
4. Explicitly classify `assets/ui/Screens/` and `assets/ui/HtmlBundles/` as mockup/design sources that cannot be called gameplay captures.
5. Add a machine-readable denylist consumed by the store-kit validator.
6. Search the repository for old README/docs references that describe mockups as implemented features and log them as claim debt.
7. Confirm where the frozen seven-day slice data lives and how the exact scenario id/version/hash is emitted.
8. Confirm the release and demo export preset identifiers and the staging/export scripts they pass through.
9. Confirm how the build exposes a deterministic or scripted path to each intended screenshot beat.
10. Capture the baseline SHA and generate a preflight report before editing anything.

**Preflight DoD:** every candidate media source has a classification and the capture pipeline has exactly one approved gameplay source: an exported, boot-verified build running the approved slice.

### 4.4 Canonical screenshot shot list

The source calls for six to eight real states. Expand that into a stable semantic shot list whose ids remain constant even if presentation improves:

| Shot id | Required state | Product purpose | Required proof |
|---|---|---|---|
| `store_day1_orientation` | day 1 orient state | establish shelter/UI/world tone | slice beat + build id |
| `store_ration_decision` | ration allocation decision | show core management tension | decision surface is live and selectable |
| `store_dispatch_route` | expedition dispatch + route preview | show planning/exploration | valid route/destination from runtime |
| `store_storm_pressure` | active weather threat | show systemic hazard/presentation | weather system actually affecting state |
| `store_death_memorial` | death + memorial/funeral | show human consequence | death/memorial pipeline reached naturally/scripted by slice |
| `store_policy_grievance` | policy choice with grievance | show social consequence | policy state + downstream grievance evidence |
| `store_day7_deadline` | deadline/resolution | show week arc | canonical day-7 beat |
| `store_shelter_overview` | representative shelter state | broad overview | no dev-only panels, no staged fake widgets |

Optional screenshots may be added, but release cannot delete required shots without a versioned shot-list decision.

### 4.5 Capture recipe contract

Each shot must be defined as a recipe rather than “pause at a nice moment.” A recipe should include:

- recipe id/version;
- scenario id/version/hash;
- start seed or deterministic save checkpoint;
- expected game day/time;
- expected beat id;
- required visible panels;
- panels/overlays that must be hidden;
- locale;
- text scale;
- resolution and aspect ratio;
- safe-area requirements;
- camera/viewport state if applicable;
- input sequence or checkpoint load method;
- acceptable timing window;
- forbidden debug/dev UI markers;
- expected runtime assertions before capture;
- file output name;
- post-capture validation steps.

Where exact pixel determinism is not practical because of animation or effects, semantic determinism still must be enforced: the same recipe must reach the same gameplay state and required UI composition.

### 4.6 Capture tool implementation

Implement `scripts/store/capture_store_assets.py` as a thin orchestrator over existing game/export interfaces rather than a second gameplay harness.

Recommended modes:

```text
--list-recipes
--recipe <id>
--all
--build <artifact>
--locale <locale>
--resolution <WxH>
--verify-only
--manifest-only
--check
```

Required behaviors:

1. refuse to run against an unverified artifact unless an explicit developer-only override is used;
2. read scenario/build identity from the artifact rather than trusting a CLI label;
3. launch the exported build in the supported automation/headless/capture mode;
4. wait for explicit beat/state readiness, not fixed arbitrary sleeps;
5. capture only after required runtime assertions pass;
6. compute the image digest;
7. write recipe/build/scenario metadata into `store_kit_manifest.json`;
8. fail if debug overlays, missing fonts, placeholder localization, or known developer-only markers are detected;
9. retain logs sufficient to reproduce the capture;
10. never copy from a mockup directory as a fallback.

If the engine cannot perform a desired screenshot fully headlessly, use deterministic runtime orchestration to reach the state and a controlled windowed capture stage. The integrity rule matters more than whether the final screenshot API is literally headless.

### 4.7 Resolution, aspect-ratio, locale, and scale matrix

The source explicitly requires 1920×1080, 1280×800, store-required ratios, locale variants where shipped, and text-scale variants. Operationalize that as two layers:

**Primary public master captures**
- 1920×1080 or the selected native marketing master;
- default shipping locale;
- default text scale;
- no platform overlays;
- lossless source retained.

**Verification variants**
- 1280×800 to align with existing QA viewport expectations;
- each supported store-critical aspect ratio;
- longest-string locale;
- one non-Latin or diacritic-heavy locale if shipped;
- maximum supported text scale;
- minimum supported viewport.

Verification variants do not all need to be published. Their purpose is to prove that the published framing is not hiding clipping/overflow and that the claimed UI remains representative.

### 4.8 Thumbnail-legibility gate

A store screenshot is often consumed at card size, not fullscreen. Add a verification pass that:

1. generates standardized downscaled previews;
2. checks that critical text/UI does not become meaningless visual noise;
3. confirms important decision states remain visually distinguishable;
4. flags screens dominated by tiny text;
5. rejects compositions requiring a reviewer to zoom just to identify the mechanic;
6. stores the thumbnail previews as review artifacts, not necessarily public assets.

This can initially be a human-approved checklist with reproducible generated thumbnails, later upgraded with automated geometry/text-size heuristics.

### 4.9 Runtime authenticity assertions

Before each capture, validate the state being depicted. Examples:

- ration screenshot: selected ration values exist in real inventory and can be committed;
- expedition screenshot: destination and route are runtime-resolved, not placeholder labels;
- storm screenshot: active weather state is registered by the weather system and relevant modifiers are nonzero if expected;
- memorial screenshot: a valid survivor death record exists and the memorial UI resolves to it;
- policy screenshot: selected policy id exists, can be applied, and the grievance shown belongs to actual downstream state;
- day-7 screenshot: scenario day and resolution beat match the approved scenario.

A screenshot that visually resembles the state but fails its state assertion must not be captured.

### 4.10 Mockup-leakage gate

Implement a strict check against accidental use of non-runtime visual sources.

The validator should:

1. hash known QA/mockup/concept images where practical;
2. reject exact copied assets found under store screenshot destinations;
3. inspect store manifests for source paths containing forbidden directories;
4. scan captions/metadata for prohibited terms such as `mockup`, `stitch`, `concept`, `fixture`, `snapshot` unless explicitly classified as a marketing-art source;
5. reject screenshot entries without a source build digest;
6. reject screenshots whose source recipe does not exist;
7. reject screenshots whose source build cannot be matched to an export artifact manifest;
8. emit a human-readable failure explaining which provenance edge is missing.

Do not attempt brittle “AI image detection.” Provenance should be established through controlled generation and source tracking.

### 4.11 Store-kit build provenance

For every generated kit, write:

- public version;
- git SHA;
- dirty-tree status (must be false for publication);
- export preset id;
- build artifact digest;
- build date;
- scenario id/version/hash;
- capture recipe version;
- capture script version;
- locale pack revision;
- asset registry revision;
- support statement revision;
- provenance report revision;
- scorecard revision;
- approver/review record id.

If any of these move after capture, the freshness checker determines whether regeneration is mandatory.

### 4.12 Provenance and AI disclosure generator

The source explicitly calls for generated disclosure from `asset_registry.json` and `docs/HUMAN_AUTHORSHIP.md`.

Implementation stages:

1. Define source categories clearly:
   - human-authored;
   - AI-generated;
   - AI-assisted;
   - third-party licensed;
   - commissioned;
   - procedural/runtime-generated if relevant;
   - unknown/unclassified.
2. Require every store-visible and shipped asset family to resolve to one category or fail the provenance report.
3. Generate counts by category and asset family.
4. Generate an exception list for assets whose registry fields are incomplete.
5. Generate a reviewable disclosure draft from structured data.
6. Preserve human-review-only narrative where the platform questionnaire requires contextual explanation.
7. Record the data revision and report digest in the store kit.
8. Scan final `docs/AI_DISCLOSURE.md` for bracket placeholders, `TODO`, `TBD`, example text, or template instructions.
9. Fail the release checklist if unresolved template language remains.
10. Re-run on any asset-registry change that touches shipped/marketing assets.

The generator should never automatically make legal claims beyond what the structured evidence establishes. The human-authorship statement remains an authored policy input, but the asset counts and lists should be computed.

### 4.13 Rights and commercial-distribution inventory

The source specifically calls out Barlow Condensed, Share Tech Mono, music/SFX provenance, and license scope. Expand this into a rights inventory with the following fields per dependency/asset family:

- name;
- version/source;
- copyright owner or origin;
- license identifier;
- license file path;
- commercial use permitted;
- modification permitted;
- attribution required;
- redistribution conditions;
- AI/tool-specific contractual note if applicable;
- store/press use permitted;
- proof/document reference;
- reviewer;
- status: `CLEAR`, `ATTRIBUTION_REQUIRED`, `REVIEW_REQUIRED`, `BLOCKED`.

Required substeps:

1. confirm font licenses and bundle required notices;
2. confirm music and SFX source/rights;
3. confirm icons/stock/third-party packages;
4. confirm commissioned marketing art rights;
5. confirm generated/AI-assisted asset usage is consistent with source/tool terms available to the project owner;
6. generate a credits/attribution appendix where required;
7. verify press-kit redistribution rights separately where the kit republishes assets;
8. block publication on `BLOCKED` or unresolved critical entries.

This is an evidence/organization task, not a substitute for legal advice.

### 4.14 Capsule and banner art boundary

The source permits new art here only for capsules/banners and requires it to derive from game art families. Enforce a clean boundary:

- runtime screenshots remain untouched gameplay captures except ordinary crop/format conversion where allowed and disclosed internally;
- capsule/banner art may be composed/illustrated as marketing art but must be labeled as such;
- marketing art cannot be passed off as gameplay;
- marketing-only source files remain outside runtime asset deployment;
- all capsule/banner inputs are registered in the provenance/rights inventory;
- generated final assets include source-project references and export dimensions;
- store LFS/binary policy applies without polluting shipped package size.

### 4.15 Store-copy evidence registry

Create `store/claims.json` (or equivalent) as the only approved source for substantive factual store claims.

Suggested record:

```json
{
  "claimId": "feature_keyboard_navigation",
  "publicTextKey": "store.feature.keyboard_navigation",
  "type": "runtime_capability",
  "evidence": [
    "gate:keyboard_only_slice",
    "gate:input_map_integrity"
  ],
  "minimumStatus": "pass",
  "introducedVersion": "…",
  "status": "eligible"
}
```

Rules:

1. no raw marketing sentence embeds mutable numbers directly if they can be generated;
2. feature claims must point to shipped capability ids;
3. metric claims must point to dated scorecards/sweeps;
4. content-count claims must point to generated utilization reports;
5. accessibility claims are preferably imported from the support-statement generator rather than duplicated;
6. unsupported/future capabilities are `ineligible`;
7. claim eligibility is recalculated per release;
8. store copy generation/checking fails if an ineligible claim is used.

### 4.16 Content-completeness facts

Use the source-required content-utilization selftest to produce facts that distinguish:

- authored;
- loaded;
- reachable;
- selected;
- effect-producing;
- deliberately codex-only or non-gameplay content.

Do not market raw authored ids as playable content if many are intentionally non-gameplay or unreachable. Prefer phrasing generated from categories whose semantics are unambiguous.

Example internal fact table:

| Family | Authored | Loaded | Reachable | Selected | Effect-producing | Publicly claimable? |
|---|---:|---:|---:|---:|---:|---|
| Items | generated | generated | generated | n/a | generated | yes, according to chosen claim rule |
| Broadcasts | generated | generated | generated | generated | n/a | yes if surfaced |
| Codex | generated | generated | generated | generated | n/a | describe as lore/codex, not active mechanics |
| Quests | generated | generated | generated | generated | generated | claim reachable/effect-producing subset |

### 4.17 Demo policy and isolation

The source recommends shipping the frozen slice as a demo unless there is a documented reason not to.

Required integration points:

1. demo boots directly into the approved scenario entry flow;
2. demo uses the same runtime code/data contracts as full release where possible;
3. demo save root is isolated from full-game saves;
4. demo cannot overwrite full-game slots;
5. demo scenario id/version/hash is embedded and queryable;
6. demo build has its own artifact digest;
7. demo screenshots and public demo copy must reference the demo manifest;
8. a demo content/update decision requires regeneration of any affected screenshot or statement;
9. upgrade-to-full-game behavior is documented if save transfer is supported; otherwise explicitly state no transfer;
10. demo build passes export boot, integrity, accessibility, and support-statement gates relevant to its surface.

### 4.18 End-to-end marketing artifact chain

Implement one dry-run command or documented pipeline equivalent to:

```text
clean checkout
  → build/test/integrity gates
  → export Linux + Windows + demo
  → boot each candidate
  → verify scenario metadata
  → run canonical slice state checkpoints
  → capture shot matrix
  → generate provenance report
  → generate content facts
  → generate claim eligibility
  → generate support statements
  → generate technical requirements
  → generate/store kit manifest
  → validate rights
  → validate freshness
  → package press/store kit
  → run release gate
```

A partial pipeline is acceptable during implementation, but the final DoD requires the chain to be reproducible from documented commands.

### 4.19 57A tests

Minimum test families:

**Contract tests**
- manifest schema validation;
- screenshot recipe ids unique;
- all required shot ids present;
- each screenshot entry has source build/scenario/digest;
- each claim has supported evidence type.

**Negative tests**
- mockup copied into screenshot directory fails;
- screenshot without build digest fails;
- altered asset-registry source flag changes provenance report;
- AI disclosure placeholder fails;
- dirty-tree publication build fails;
- stale scenario hash fails demo/store binding;
- missing required font/license record fails rights check according to severity.

**Determinism tests**
- same shot recipe produces same semantic state sequence;
- shot list is stable and ordered;
- manifest generation is deterministic given same inputs;
- content-facts generation stable for unchanged catalogs.

**Integration tests**
- exported candidate boots;
- screenshot capture can reach each required state;
- generated kit references exact export artifact digests;
- release checker consumes the kit successfully.

### 4.20 57A completion evidence

57A closes only when the close-out contains:

- store kit manifest path + digest;
- list of final required screenshots;
- scenario id/version/hash;
- Linux/Windows/demo artifact digests;
- provenance report summary and unresolved count = 0 for blocking fields;
- AI disclosure placeholder scan = clean;
- rights inventory blocking count = 0;
- claim registry validation = pass;
- content-facts generation = pass;
- mockup-leakage gate = pass;
- capture freshness check = pass;
- reviewer sign-off that marketing art and gameplay screenshots are correctly distinguished.

**57A DoD:** every gameplay image and factual sentence intended for the store can be traced to a verified build, generated evidence, or explicitly versioned policy input.


---

## 5. Task 57B — Accessibility, Localization, and Input Statements That Survive Audit

### 5.1 Mission

57B converts accessibility/localization/input support from prose into a generated report over the release candidate. The public statement must be deliberately narrower than internal aspiration. If a capability is not backed by a passing gate for the current build, it does not appear as supported.

The key architectural rule is **absence-by-default**: an unsupported or unverified claim is omitted or moved into the known-limitations section. This prevents documentation from running ahead of implementation.

### 5.2 Support-claim registry

Create one registry of public support claims. Suggested fields:

```text
SupportClaim
  id
  category
  titleKey
  descriptionKey
  backingGates[]
  backingSettings[]
  backingRuntimeCapabilities[]
  requiredPlatforms[]
  requiredLocales[]
  statusPolicy
  knownLimitationKey
  introducedVersion
  lastVerifiedBuild
```

Suggested initial categories:

- input;
- keyboard;
- mouse;
- controller;
- remapping;
- text scale;
- captions;
- reduce motion;
- color-independent signaling;
- pause behavior;
- timing/reflex requirements;
- save behavior;
- screen-reader-adjacent semantic labels;
- localization;
- platform support;
- recovery/reset.

The registry is not itself proof. It tells the generator which proof must exist.

### 5.3 Gate-to-claim mapping

For every claim:

1. identify the exact CI/selftest/integration gate that demonstrates it;
2. establish whether the gate executes on source only or on an exported build;
3. prefer exported-build evidence for public claims;
4. record the gate result id and source revision;
5. reject claims where the backing gate was skipped;
6. reject claims where the gate exists but did not run on the current release candidate;
7. reject claims whose backing setting was removed/renamed;
8. reject claims with platform-specific gaps when the statement does not disclose the platform scope.

Examples:

| Claim | Minimum evidence |
|---|---|
| keyboard-only playable | canonical slice complete with no pointer input |
| controller support | input map + controller navigation + canonical slice critical-path completion |
| remapping | settings persistence + conflict handling + exported-build exercise |
| text scaling | setting exists + max-scale overflow probe |
| captions | caption setting + all required audio cue classes represented |
| reduce motion | setting exists + motion-producing surfaces respect it |
| color-independent status | semantic/icon/text alternate exists for coded statuses |
| save anytime | save policy + state round-trip test at supported points |
| pausable simulation | simulation pause gate across relevant gameplay state |
| localization | locale pack shipped + coverage/placeholder thresholds |

### 5.4 Generated `docs/accessibility/STATEMENT.md`

The generated statement should contain:

1. build/version/date;
2. supported input methods;
3. remapping behavior;
4. keyboard-only status;
5. controller status;
6. text scaling;
7. caption/audio-support behavior;
8. reduce-motion behavior;
9. color/shape/text redundancy;
10. pause/timing requirements;
11. save behavior;
12. language/localization support;
13. known limitations;
14. recovery/reset/support paths;
15. evidence revision references.

The public-facing prose can be human-readable, but the supported/unsupported statuses must be computed.

### 5.5 Localization status generator

Produce `docs/l10n/STATUS.md` and a machine-readable companion.

Per locale record:

- locale code/name;
- shipped yes/no;
- total translatable strings;
- translated strings;
- untranslated strings;
- placeholder/fallback strings;
- format-token errors;
- overflow test status;
- font coverage status;
- last verified SHA;
- public eligibility.

Set explicit thresholds. For example, a release locale may require:
- 100% critical UI strings;
- no placeholder/TODO strings;
- no missing font glyphs in required corpus;
- no formatting-token mismatch;
- no blocker overflow in required screens.

Do not silently call a locale “supported” merely because its folder exists.

### 5.6 Long-string and diacritic verification

The screenshot/support plan should include representative worst-case strings.

Required probes:

1. longest menu label;
2. longest policy/quest title;
3. longest body-text paragraph;
4. longest button/action label;
5. diacritic-rich names;
6. number/date formats;
7. pluralization variants;
8. keyboard shortcut interpolation;
9. dynamic item names + quantities;
10. accessibility statement itself in each published language if localized.

The generator should distinguish translation completeness from UI fitness; both matter.

### 5.7 Settings-surface parity

Public claims should be cross-checked against the actual `UserSettings` model and rendered settings UI.

For each claimed user-facing control:

1. field exists;
2. default exists;
3. range/enumeration exists;
4. persistence works;
5. reset-to-default works;
6. UI control binds to the correct field;
7. runtime subscriber actually reacts;
8. setting is reachable without developer tooling;
9. setting label/help text is localized;
10. exported build exposes it.

A claim such as “reduce motion” is invalid if the checkbox exists but relevant animations do not subscribe to it.

### 5.8 Known-limitations section

Known limitations are a deliberate output, not an embarrassment to hide.

Each entry should include:

- limitation id;
- short user-facing description;
- affected platform/input/locale;
- severity;
- workaround if safe and real;
- planned task/plan reference if one exists;
- first affected version;
- last reviewed build.

Avoid promising dates unless release planning has explicitly accepted them.

### 5.9 Technical requirements matrix

The source requires minimum/recommended specs to derive from measured performance, not guesses.

Create a test matrix covering:

- CPU class;
- memory;
- integrated/discrete GPU where relevant;
- storage size + headroom;
- OS versions;
- display resolution;
- long-session memory behavior;
- load/boot times;
- canonical slice frame-time/CPU budget if applicable.

Record:
- tested machine profile;
- build SHA;
- scenario;
- measurement method;
- p50/p95 where meaningful;
- observed worst state;
- pass/fail against proposed minimum/recommended tier.

A public spec line must point to this matrix. If insufficient hardware coverage exists, state the tested configuration rather than inventing a broad minimum.

### 5.10 Age/rating questionnaire evidence pack

The source asks for rating posture derived from content facts. Treat this as structured preparation:

- violence categories actually depicted;
- injury/death presentation;
- substance/alcohol/drug references if any;
- strong language if any;
- gambling if any;
- sexual/romantic content if any;
- fear/horror themes;
- discrimination/ethical themes where questionnaires ask;
- user-generated content/modding scope;
- online interaction status;
- in-app purchases/DLC posture if applicable.

Store the answers with:
- source content ids;
- reviewer;
- build version;
- questionnaire/platform;
- submission date.

This avoids re-answering from memory across platforms.

### 5.11 Recovery and support claims

Public support docs may reference only tested recovery paths:

- safe-mode/reset operation;
- settings reset;
- save recovery rules;
- triage kit generation;
- log location;
- crash reproduction path;
- bug-report package;
- privacy/redaction behavior.

Each support instruction should be executable by a normal user on a shipping build. Developer-only CLI switches belong in internal support escalation docs unless explicitly exposed.

### 5.12 Statement-freshness gate

The generator must fail when:

- a backing gate id disappears;
- a backing gate did not run;
- a backing setting disappears;
- a locale drops below threshold;
- a public claim references an old build;
- a technical-requirement statement references stale benchmark data after significant performance changes;
- known limitations changed but the generated statement was not refreshed.

Relevant source changes should mark the statement stale. Candidate triggers:
- input maps;
- `UserSettings`;
- localization catalogs;
- UI scenes;
- caption/audio buses;
- motion/animation systems;
- save policy;
- export/platform settings.

### 5.13 Cross-tool/reviewer audit

The source asks for a second-tool review. Formalize the review packet:

Reviewer receives:
- generated statement;
- claim registry;
- gate results;
- settings field list;
- known limitations;
- current build id.

Reviewer does **not** receive:
- aspirational design notes;
- implementation reasoning that could bias them toward “intended” behavior.

Review questions:
1. Can each support claim be found in the build?
2. Does each claim describe the observed behavior accurately?
3. Are any unsupported capabilities implied?
4. Are limitations material enough that omission would mislead?
5. Does platform scope match reality?
6. Are localization claims precise?
7. Are support/recovery instructions executable?

### 5.14 57B tests

Minimum tests:

- generator deterministic for unchanged evidence;
- unsupported claim omitted;
- removed gate causes claim rejection;
- skipped gate causes claim rejection;
- removed settings field causes claim rejection;
- locale below threshold removed from supported list;
- known limitation appears when configured;
- max text scale test result propagates correctly;
- controller claim cannot pass on input-map-only evidence if end-to-end gate is required;
- statement references current build SHA;
- no stale generated output accepted.

### 5.15 57B completion evidence

Close-out must include:

- generated accessibility statement;
- generated localization status;
- claim-to-gate matrix;
- settings parity report;
- technical requirements matrix;
- known limitations;
- rating-questionnaire evidence pack;
- recovery/support path verification;
- second-review result;
- freshness gate result.

**57B DoD:** the public support statement is a reproducible report over the current build, and every positive claim has current evidence.

---

## 6. Task 57C — Launch Operations: Version Cadence, Demo Freeze, Patch Policy, and Feedback Path

### 6.1 Mission

57C converts launch from an event into an operating system. The source already points to release/version/hotfix policy, save corpus, roadmap intake, balance decisions, scorecards, telemetry privacy, and support documentation. D1[2] integrates them into one repeatable launch lifecycle.

### 6.2 Public version and artifact identity

For each release define one immutable release record:

```text
ReleaseRecord
  publicVersion
  gitSha
  sourceBranch
  releaseDate
  fullBuilds[]
  demoBuild
  sliceScenarioId
  sliceScenarioHash
  storeKitManifestDigest
  accessibilityStatementDigest
  localizationStatusDigest
  provenanceReportDigest
  knownIssuesDigest
  saveCompatibilityPolicyVersion
  modContractVersion
  releaseGateRunId
```

No public version is “released” internally until this record is complete.

### 6.3 Demo freeze procedure

1. select approved slice version/hash;
2. select release candidate SHA;
3. export demo;
4. boot-verify demo;
5. run demo integrity gates;
6. verify isolated save root;
7. verify settings surface;
8. verify input/accessibility subset;
9. capture/generate demo-specific evidence;
10. compute artifact digest;
11. write demo manifest;
12. freeze the record;
13. require a re-cut for any scenario/build change.

A demo update without a new manifest is forbidden.

### 6.4 Release cadence policy

Define release tiers precisely:

**Patch**
- bug fixes;
- presentation corrections;
- data corrections compatible with existing contracts;
- no intentional save-contract break;
- no mod-contract break.

**Minor**
- new content;
- new reachable systems within compatible save/data contracts;
- tuning changes with sweeps;
- optional new data-pack families;
- compatible feature additions.

**Major**
- save/data/mod-contract break;
- incompatible world-state migration;
- major system replacement;
- significant public capability contract change.

Tie each tier to:
- required tests;
- required playtest scope;
- store-kit regeneration scope;
- changelog content;
- save corpus expectations;
- demo re-cut policy.

### 6.5 Emergency hotfix triggers

Define objective triggers, for example:

- crash on boot or load;
- save corruption/data loss;
- release-blocking progression lock;
- severe privacy issue;
- severe accessibility regression blocking basic operation;
- artifact missing required catalogs;
- demo/full-build mismatch causing user data collision;
- major exploit only if it undermines intended survival/economy and cannot wait.

Hotfix flow:
1. reproduce;
2. isolate;
3. choose minimal change;
4. run targeted tests;
5. run fast release gates;
6. run save corpus;
7. run canonical slice determinism;
8. regenerate affected statements/store artifacts;
9. update known issues/changelog;
10. sign release record.

### 6.6 Save compatibility promise

The public promise must be derived from tested policy.

Document:
- oldest supported save version;
- forward/backward expectations;
- migration behavior;
- what happens on incompatible save;
- backup behavior;
- demo/full-game transfer behavior;
- modded-save support posture;
- recovery instructions.

Release gate should compare the promise against the save corpus and migration tests. If the corpus no longer proves the advertised range, narrow the promise or restore compatibility.

### 6.7 Mod-support posture

Publish only what 47A/47C or equivalent contract actually supports.

Required distinctions:
- supported data packs;
- overlays;
- tags;
- load order;
- conflict behavior;
- version constraints;
- unsupported executable code/assemblies if that remains policy;
- save implications;
- support boundary for modded sessions;
- fixture-pack CI evidence.

The public statement must not imply a general plugin architecture when only data contracts are supported.

### 6.8 Feedback path

Define one user-facing issue intake path and one internal escalation path.

User-facing bug packet should request:
- public version;
- platform;
- concise reproduction steps;
- optional triage package;
- save/seed only where appropriate;
- screenshot if relevant;
- consent/privacy note.

Internal triage enriches with:
- build SHA;
- gate state;
- scenario/day;
- log excerpts;
- save schema version;
- content pack list;
- reproduction status;
- severity;
- ownership;
- hotfix eligibility.

Never require users to submit personal data irrelevant to reproduction.

### 6.9 Triage kit contract

The support/triage package should be generated with predictable contents:

- build/version;
- platform;
- non-sensitive settings subset;
- loaded catalog versions;
- save schema version;
- scenario/seed if applicable;
- recent error log;
- recent game events if privacy-safe;
- mod/data-pack list;
- integrity check result.

Redact:
- username/home path where possible;
- unrelated filesystem data;
- network identifiers;
- personal free-text not required;
- telemetry identifiers beyond support need.

### 6.10 Known-issues register

Generate from:
- open release blockers;
- accepted waivers;
- high-severity audit findings;
- known platform limitations;
- current accessibility limitations;
- save migration caveats;
- demo limitations.

Each entry:
- id;
- affected version/platform;
- concise symptom;
- workaround;
- severity;
- status;
- source issue/plan;
- first known version;
- fixed version when resolved.

The public known-issues artifact can be a curated subset, but curation must be explicit rather than omission by accident.

### 6.11 Balance patch discipline

For any tuning change:

1. capture reason;
2. run required sweep;
3. record before/after metrics;
4. record slice scorecard delta;
5. test relevant human/synthetic funnel if material;
6. write decision entry;
7. update patch note;
8. verify no store claim becomes stale.

A balance patch should be reproducible as a policy decision, not an unexplained number change.

### 6.12 Post-launch telemetry review

Only opt-in/private telemetry permitted by the existing privacy posture should be used.

Review cadence should compare:
- synthetic funnel;
- human playtest funnel;
- opt-in live funnel;
- crash/support issue classes;
- slice scorecard;
- save failure rate;
- input/accessibility issue volume;
- localization issue volume.

Do not treat live telemetry as automatic truth. Changes require interpretation and a recorded decision.

### 6.13 Content-update pipeline

New content should pass:
- roadmap intake;
- schema/data validation;
- utilization/reachability;
- acceptance ladder;
- save compatibility;
- mod/data-pack contract if affected;
- slice relevance check;
- store claim freshness;
- release gate.

Where data packs can safely carry updates without binary rebuild, use that path only if versioning/signature/load-order/recovery policies are already established.

### 6.14 Support escalation ladder

Recommended ladder:

1. FAQ/known issue;
2. settings/reset/recovery action;
3. triage kit review;
4. save/day/seed reproduction;
5. deterministic replay where available;
6. targeted selftest;
7. source-level debugging;
8. hotfix decision;
9. release candidate;
10. post-fix verification against original reproduction.

Every escalation should preserve the original issue id and evidence chain.

### 6.15 Launch dry run

Before real publication, perform a complete rehearsal from clean checkout.

Dry run must produce:
- candidate builds;
- demo;
- screenshot kit;
- provenance report;
- AI disclosure;
- accessibility/l10n statement;
- feature matrix;
- technical requirements;
- known issues;
- support page;
- changelog;
- release record;
- final release gate result.

The dry run should be treated as a failure-finding exercise. Any undocumented manual step becomes a task.

### 6.16 57C tests

- missing public support statement blocks release;
- missing demo manifest blocks demo publication;
- mismatched scenario hash blocks release;
- unattributed balance patch blocks release;
- save promise outside corpus support blocks release;
- unresolved hotfix waiver blocks release;
- known blocker missing required disposition blocks release;
- release record missing artifact digest fails;
- feedback/triage doc references nonexistent command fails docs check;
- release checklist can run from a clean checkout.

### 6.17 57C completion evidence

- versioning policy approved;
- demo frozen and manifested;
- save promise generated/verified;
- mod posture published;
- support path executable;
- triage kit tested;
- known issues generated;
- patch/hotfix policy dry-run tested;
- full launch rehearsal complete;
- release record complete;
- release gate green.

**57C DoD:** page, demo, patches, saves, support, feedback, and updates are all governed by one versioned release-evidence chain.


---

## 7. Integration-Derived Follow-On Tasks

The source ends at 57C. Operating 57A–57C introduces new integration surfaces that deserve explicit ownership. The following tasks are **Integration-derived**. They should be scheduled only after the source-critical path is functioning; they are not permission to delay 57A–57C behind infrastructure overengineering.

### Task 57D — Store Claim Compiler and Feature Eligibility Gate

**Goal:** prevent manually written public feature lists from outrunning runtime reality.

#### Substeps

1. Inventory every candidate store feature statement currently present in docs, pitch text, README material, mockups, and future store copy.
2. Normalize each into a stable claim id.
3. Classify each claim as runtime capability, content quantity, accessibility, localization, performance, save, mod, provenance, or design-description.
4. Map runtime claims to plan/system ids and gate ids.
5. Add a feature-status state machine: `PLANNED`, `IMPLEMENTED_UNVERIFIED`, `VERIFIED_INTERNAL`, `PUBLIC_ELIGIBLE`, `DEPRECATED`.
6. Generate the public feature matrix only from `PUBLIC_ELIGIBLE`.
7. Fail if store copy references `PLANNED` or `IMPLEMENTED_UNVERIFIED`.
8. Add deprecation handling so removed features disappear from generated copy and stale screenshots are flagged.
9. Add a docs linter that detects raw claims not present in the registry.
10. Store build SHA and gate run id for every eligibility decision.
11. Add fixtures for unsupported fake-console features and prove they cannot become public-eligible without real evidence.
12. Generate a diff per release: added public claims, removed claims, changed evidence.
13. Feed the diff into patch/store review.
14. Add a manual review checkpoint only for wording, never for eligibility.

**DoD:** public feature eligibility is computed, not remembered.

### Task 57E — Screenshot Freshness and Visual Drift Control

**Goal:** know when a once-truthful screenshot has become stale after UI/gameplay changes.

#### Substeps

1. Record source paths/areas that each shot depends on.
2. Track scenario hash, UI scene revisions, localization revision, relevant settings schema, and asset families.
3. Define freshness triggers by shot recipe.
4. If an affected dependency changes, mark the shot `STALE_PENDING_RECAPTURE`.
5. Do not require recapture for unrelated changes; keep the dependency mapping narrow enough to avoid permanent churn.
6. Generate current and previous capture contact sheets for review.
7. Add semantic assertions to catch controls/panels removed from the new build.
8. Detect old key prompts or outdated action labels.
9. Re-run thumbnail legibility after typography/layout changes.
10. Preserve old store kits by release version for provenance, but only one kit is `CURRENT`.
11. Add a release check that no published screenshot is stale.
12. Add a changelog hook stating when the public screenshot set changed.

**DoD:** a screenshot can become stale mechanically and cannot remain current by accident.

### Task 57F — Rights, Credits, and Provenance Closure Gate

**Goal:** move asset/license due diligence from submission-week archaeology into continuous validation.

#### Substeps

1. Make provenance metadata mandatory for every newly introduced shipped or marketing asset.
2. Require license/rights references for third-party assets.
3. Require commission/source records for commissioned work.
4. Require AI/source classification where applicable.
5. Generate credits from the rights inventory where attribution is required.
6. Validate that required license texts are included in distribution artifacts.
7. Validate that marketing/press redistribution does not violate source terms.
8. Add expiration/review fields only where contracts actually need them.
9. Block release on unknown source for store-visible assets.
10. Add migration entries for legacy assets lacking metadata.
11. Keep human review for ambiguous cases.
12. Record reviewer and evidence date without inventing legal certainty.

**DoD:** no release depends on reconstructing where an asset came from after the fact.

### Task 57G — Publication Manifest Sealing

**Goal:** create one immutable record of exactly what public material belongs to a release.

#### Substeps

1. Finalize `store_kit_manifest.json`.
2. Hash all public kit files.
3. Hash release/demo artifacts.
4. Record scenario and scorecard identity.
5. Record generated support/provenance reports.
6. Record public feature claim ids.
7. Record known-issues revision.
8. Seal the manifest after approval.
9. Refuse silent mutation; any change generates a new manifest revision.
10. Archive sealed manifests by public version.
11. Add verification command that recomputes hashes.
12. Make support able to identify a public screenshot/statement from manifest revision.

**DoD:** every published release has a cryptographically checkable public-evidence inventory.

### Task 57H — Demo/Full-Build Parity Matrix

**Goal:** make explicit where the demo intentionally differs from the full game and prove everything else remains shared.

#### Substeps

1. Enumerate binaries/assemblies/scripts shared by demo and full build.
2. Enumerate catalogs shared.
3. Enumerate intentionally excluded content.
4. Enumerate save-root differences.
5. Enumerate scenario-start differences.
6. Enumerate settings/input/accessibility differences; target zero unless justified.
7. Enumerate telemetry/privacy differences.
8. Verify shared code paths with build metadata.
9. Verify no demo-only tuning leak reaches the full build.
10. Verify no full-game save is writable by demo.
11. Verify screenshots describing shared mechanics remain representative.
12. Publish internal parity report each release.

**DoD:** demo differences are explicit, minimal, and tested.

### Task 57I — Store Platform Submission Matrix

**Goal:** remove platform-specific submission knowledge from memory and encode it as a checklist/data matrix.

#### Substeps

1. Enumerate every target storefront actually intended for release.
2. For each, record required image classes/dimensions, copy fields, rating forms, accessibility fields, AI/provenance disclosures, supported OS metadata, demo handling, and build packaging requirements.
3. Keep platform rules version/date stamped because requirements can change.
4. Separate invariant internal evidence from platform-specific wording.
5. Generate a missing-assets report per target platform.
6. Map every upload field to a store-kit artifact or reviewed manual field.
7. Mark fields requiring human/legal/business judgment.
8. Prevent one storefront's wording from becoming the canonical truth source.
9. Add submission dry-run checklist.
10. Archive final submitted metadata with release record.

**DoD:** storefront differences are adapters over one product-evidence core, not separate truth systems.

### Task 57J — Customer-Support Reproduction Pack

**Goal:** make first-line support able to convert user reports into deterministic engineering evidence.

#### Substeps

1. Define supported triage package schema.
2. Add one-click or documented generation path where appropriate.
3. Redact local paths and unrelated personal data.
4. Include build, save schema, catalog versions, mods/data packs, relevant settings, and integrity result.
5. Include recent error/event traces within privacy rules.
6. Provide checksum and package version.
7. Build an internal viewer or documented extraction flow.
8. Map common report classes to diagnostics.
9. Add corrupted-package handling.
10. Add regression fixtures from resolved real issues.
11. Verify support docs use the same commands as the shipping build.
12. Feed reproducible issues into hotfix/release policy.

**DoD:** support reports arrive with enough structured evidence to reproduce rather than speculate.

### Task 57K — Public Evidence Regression Suite

**Goal:** test the boundary between internal changes and public truth.

#### Substeps

1. Add fixtures for stale screenshots.
2. Add fixture for missing claim gate.
3. Add fixture for removed setting.
4. Add fixture for dropped locale coverage.
5. Add fixture for unresolved AI-disclosure placeholder.
6. Add fixture for unknown asset provenance.
7. Add fixture for demo scenario drift.
8. Add fixture for build SHA mismatch.
9. Add fixture for missing rights notice.
10. Add fixture for save promise wider than corpus.
11. Add fixture for technical requirements older than configured freshness window.
12. Add fixture for known blocker omitted from required release disposition.
13. Run fast subset on PRs touching public-evidence dependencies.
14. Run full suite during release/nightly.

**DoD:** the failure modes of the shop window are represented by executable negative tests.

### Task 57L — Post-Launch Evidence Review and Roadmap Feedback

**Goal:** convert real player evidence into controlled product decisions without turning telemetry into automatic design.

#### Substeps

1. Establish release review cadence.
2. Compare synthetic, playtest, opt-in live, support, and crash evidence.
3. Identify discrepancies between what store copy leads players to expect and what they actually discover.
4. Record misleading-but-technically-true claims as product bugs.
5. Track screenshot states players cannot reproduce as capture/legibility defects.
6. Track accessibility claims that produce disproportionate support issues.
7. Track locale-specific comprehension/support issues.
8. Feed accepted findings into `DECISIONS.md`.
9. Feed larger work through roadmap intake/rails.
10. Regenerate store/support statements when evidence changes public truth.
11. Preserve rejected findings with reasons.
12. Compare changes across releases using the frozen/ versioned slice and release records.

**DoD:** post-launch evidence modifies roadmap and public statements through the same decision discipline as engineering changes.

---

## 8. Cross-Task Dependency Graph

```text
Plan 54 / D1 slice
   │
   ├── frozen scenario + scorecard ───────────────┐
   │                                              │
   ▼                                              ▼
57A capture/provenance ──────────────► 57D claim compiler
   │                                      │
   ├────────► 57E screenshot freshness    │
   ├────────► 57F rights closure          │
   ├────────► 57G manifest sealing ◄──────┘
   │
   ▼
57B support statements
   │
   ├────────► l10n/accessibility/input truth
   ├────────► technical requirements
   └────────► known limitations
   │
   ▼
57C launch operations
   │
   ├────────► 57H demo/full parity
   ├────────► 57I storefront adapters
   ├────────► 57J support reproduction
   ├────────► 57K evidence regression suite
   └────────► 57L post-launch evidence review
```

### 8.1 Required upstream dependencies

- 54A/54C: frozen slice and scorecard;
- 50A/56A/56B: asset registry and game-vs-marketing boundary;
- 25C: localization status inputs;
- 37A/B/C: accessibility/input gates;
- 46A/B/C: metrics, telemetry, review ritual;
- 48A/B/C: version/release/hotfix process;
- 55B: save corpus;
- 47A/47C: mod contract and fixture tests;
- 53C: roadmap intake;
- 39A/39C: release/save gates;
- 52C where audio provenance/right checks depend on it.

### 8.2 Parallelism rules

Safe parallel work after the base contracts are set:
- capture recipe authoring and rights inventory;
- claim registry and localization status generator;
- support docs and known-issues schema;
- platform submission matrix and press-kit structure.

Unsafe parallelism:
- final screenshots before slice/build freeze;
- final accessibility statement before gate inventory;
- final AI disclosure before provenance migration;
- final store copy before feature eligibility;
- final minimum specs before measured performance data;
- demo publication before save isolation and parity checks.

---

## 9. Detailed Verification Strategy

### 9.1 Build and core integrity

Run at minimum:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Expected release posture: zero build errors, project-defined warning policy satisfied, integrity selftests green.

### 9.2 Store pipeline

```bash
python3 scripts/store/capture_store_assets.py --check
python3 scripts/store/validate_store_kit.py --check
python3 scripts/store/check_mockup_leakage.py
python3 scripts/store/check_claim_evidence.py
python3 scripts/store/check_screenshot_freshness.py
python3 scripts/ci/generate-provenance-report.py --check
python3 scripts/ci/generate-support-statements.py --check
```

Paths are Integration-derived where not already source-defined; adapt to actual project tooling instead of duplicating equivalent commands.

### 9.3 Export and demo verification

- export Windows;
- export Linux;
- export demo;
- compute digests;
- boot each;
- confirm data catalogs load;
- confirm scenario id/hash where applicable;
- confirm demo save isolation;
- run canonical slice path;
- verify settings accessibility/input claims;
- compare demo/full parity report.

### 9.4 Publication negative tests

A release candidate must be proven to fail correctly when seeded with:
- a mockup screenshot;
- a stale screenshot manifest;
- an unsupported claim;
- a missing locale threshold;
- unresolved AI template text;
- unknown asset source;
- mismatched build SHA;
- stale demo hash;
- missing required license attribution;
- out-of-range save promise;
- skipped accessibility gate;
- unmeasured technical spec claim.

A gate that never sees a failing fixture is not yet trusted.

---

## 10. Risk Register

| Risk | Likelihood | Impact | Mitigation | Release blocker? |
|---|---|---|---|---|
| mockup presented as gameplay | medium before controls | critical reputational | denylist + provenance + capture manifest | yes |
| AI disclosure incomplete | medium | high/platform rejection | generated report + placeholder scan + review | yes |
| unsupported accessibility claim | medium | high | claim-to-gate generation | yes |
| stale locale claim | medium | medium/high | coverage gate + statement freshness | yes if publicly claimed |
| screenshots stale after UI change | high without freshness | medium/high | recipe dependency mapping | yes for affected shots |
| build/demo mismatch | medium | high | release record + parity gate | yes |
| save promise too broad | low/medium | high | corpus-derived promise | yes |
| rights record incomplete | medium for legacy assets | critical | migration inventory + blocker state | yes where material |
| technical specs guessed | medium | medium | measured matrix | yes before publishing specs |
| excessive infrastructure delays launch | medium | medium | source critical path first; derived tasks staged | no, manage scope |
| generated statement wording awkward | medium | low | human wording review after eligibility | no |
| generator becomes second authority | medium | high | generators consume authoritative data only | yes if divergence |
| telemetry overinterpreted | medium | medium | decision review + multiple evidence sources | no |
| support package leaks personal data | low/medium | high | explicit redaction tests | yes |
| store platform requirements change | high over time | medium | dated adapter matrix | depends |

### 10.1 Highest-priority risks

P0:
- public misrepresentation;
- unverified AI/provenance disclosure;
- rights blockers;
- build/demo identity mismatch;
- data-loss/save-promise mismatch;
- privacy leak in support/telemetry.

P1:
- stale screenshots;
- unsupported accessibility/input/localization claim;
- incorrect technical requirements;
- missing known-issue disposition.

P2:
- suboptimal screenshot composition;
- wording quality;
- press-kit convenience files.

---

## 11. Acceptance Matrix

| Area | Required artifact | Required proof | Failure action |
|---|---|---|---|
| screenshots | shot set + recipes | exact build/scenario + authenticity assertions | block |
| capsule/banner | marketing art + provenance | rights/source inventory | block if unresolved |
| AI disclosure | final statement | generated registry report + human review | block |
| feature list | claim registry output | gate/capability evidence | omit/block |
| accessibility | generated statement | current gates | omit unsupported; block misleading |
| localization | status report | coverage/font/format/overflow | drop unsupported locale claim |
| technical specs | requirements matrix | measured profiles | do not publish guess |
| demo | demo manifest | boot + hash + isolation | block demo |
| save support | promise | save corpus/migration tests | narrow promise or fix |
| mod support | posture | contract + fixture CI | narrow promise or fix |
| known issues | generated register | release audit | block if required disposition missing |
| support | support doc + triage kit | executable shipping flow | fix docs/tool |
| release | ReleaseRecord | final gate | block |
| post-launch | review record | evidence comparison + decisions | roadmap intake |

---

## 12. Execution Backlog — Recommended Order

### Stage A — Must happen first

1. verify slice version/hash;
2. inventory store/press/mockup/QA media;
3. define store kit manifest;
4. define claim registry;
5. define provenance source categories;
6. define support claim registry;
7. define demo manifest;
8. define release record.

### Stage B — Build the truthful artifact path

9. implement capture recipes;
10. implement capture runner;
11. implement runtime authenticity checks;
12. implement mockup leakage gate;
13. implement provenance generator;
14. migrate rights/source metadata;
15. implement content-facts generator;
16. implement feature eligibility;
17. create capsule/banner art boundary.

### Stage C — Support statements

18. connect accessibility/input gates;
19. generate localization status;
20. add settings parity check;
21. add known limitations;
22. generate technical requirements;
23. prepare rating evidence;
24. review support/recovery instructions;
25. add statement freshness.

### Stage D — Launch operations

26. freeze demo;
27. verify save isolation;
28. define patch/minor/major cadence;
29. verify save promise;
30. verify mod posture;
31. create feedback/triage path;
32. generate known issues;
33. integrate balance decision flow;
34. create complete release record.

### Stage E — Hardening

35. screenshot freshness mapping;
36. manifest sealing;
37. demo/full parity matrix;
38. storefront submission matrix;
39. support reproduction pack;
40. public evidence negative-test suite;
41. post-launch review ritual.

### Stage F — Dry run and close-out

42. clean checkout;
43. full tests;
44. exports;
45. boot verification;
46. canonical slice run;
47. screenshot capture;
48. generated reports;
49. rights review;
50. store kit validation;
51. release gate;
52. launch rehearsal;
53. close-out report;
54. archive release manifest.

---

## 13. Guardrails

1. No gameplay screenshot from a mockup, design export, QA golden, or staging-only state.
2. No factual store claim without registered evidence.
3. No mutable numeric content claim maintained solely by hand.
4. No accessibility claim because a setting merely exists; runtime behavior must be verified.
5. No locale claim because translation files merely exist.
6. No AI disclosure completed from memory when provenance data can generate it.
7. No unresolved placeholders in platform submission text.
8. No marketing-only assets silently entering runtime packages.
9. No runtime screenshots silently beautified into states the player cannot reproduce.
10. No demo writing to full-game save roots.
11. No demo drift without manifest/version change.
12. No release under a public version whose artifacts come from conflicting source revisions without explicit, justified metadata.
13. No save promise broader than tested migration/corpus coverage.
14. No general “mod support” claim when only constrained data-pack contracts are supported.
15. No technical minimum/recommended specifications without measured evidence.
16. No support procedure that requires developer-only knowledge without saying so.
17. No triage package containing unnecessary personal data.
18. No hotfix that bypasses save/slice/release integrity checks because it is “small.”
19. No balance tuning change without sweep/decision evidence.
20. No roadmap promise on the store page simply because a concept exists.
21. No release exception without an owner, reason, scope, and expiry/review point.
22. No generated report treated as authoritative if its inputs are stale or non-authoritative.
23. No deletion of historical sealed store kits; preserve them for provenance.
24. No public known-issues process that silently edits history; resolve entries by version.
25. No store pipeline that becomes so elaborate it blocks fixing the actual game; the source critical path remains 57A → 57B → 57C.

---

## 14. Final Verification Checklist

```text
SOURCE / SLICE
[ ] Approved seven-day slice id/version/hash recorded
[ ] Slice scorecard available for target build
[ ] Demo/full candidate SHA recorded

BUILD
[ ] Core build passes
[ ] Core tests pass
[ ] Game build passes
[ ] Data-integrity selftest passes
[ ] Bridge selftest passes
[ ] Windows export boots
[ ] Linux export boots
[ ] Demo export boots

SCREENSHOTS
[ ] Required shot ids complete
[ ] Each shot comes from exported verified build
[ ] Each shot has recipe + source build digest
[ ] Runtime authenticity assertion passed
[ ] No debug/dev-only UI
[ ] No QA golden/mockup/concept source
[ ] Thumbnail legibility reviewed
[ ] Locale/text-scale verification variants checked
[ ] Screenshot freshness = current

PROVENANCE / RIGHTS
[ ] Asset registry source classifications complete for public/shipped scope
[ ] Provenance report generated
[ ] AI disclosure generated/reviewed
[ ] No TODO/TBD/bracket placeholders
[ ] Font licenses checked
[ ] Music/SFX provenance checked
[ ] Marketing art provenance checked
[ ] Required attributions generated
[ ] Blocking rights issues = 0

CLAIMS / CONTENT
[ ] Feature matrix generated
[ ] Every public feature claim eligible
[ ] Content counts generated from authoritative utilization data
[ ] No unreachable/non-gameplay content misrepresented as active gameplay
[ ] Metric-derived claims cite current evidence
[ ] No future roadmap feature presented as shipped

ACCESSIBILITY / INPUT / L10N
[ ] Support claim registry resolved
[ ] Backing gates ran on current candidate
[ ] Keyboard-only status verified
[ ] Controller status verified if claimed
[ ] Remapping verified if claimed
[ ] Text scale/overflow verified
[ ] Captions verified if claimed
[ ] Reduce motion verified if claimed
[ ] Settings surface parity passes
[ ] Localization status generated
[ ] Claimed locales meet threshold
[ ] Known limitations generated
[ ] Cross-review completed

TECHNICAL / RATING
[ ] Minimum/recommended specs derived from measurements
[ ] Long-session/performance evidence current
[ ] Rating questionnaire evidence pack reviewed
[ ] Platform-specific fields mapped to evidence

DEMO / SAVE / MOD
[ ] Demo manifest sealed
[ ] Demo scenario hash matches approved slice
[ ] Demo save root isolated
[ ] Demo/full parity report passes
[ ] Save compatibility promise matches corpus
[ ] Mod support statement matches contract + fixtures

SUPPORT / RELEASE
[ ] Support doc executable on shipping build
[ ] Triage kit generated and redaction-tested
[ ] Known-issues register current
[ ] Feedback path documented
[ ] Patch/hotfix policy current
[ ] Balance changes have sweep + decision
[ ] Changelog current
[ ] Store kit manifest sealed
[ ] ReleaseRecord complete
[ ] Store checklist passes
[ ] release-gate.sh passes
[ ] verify-fast.sh passes
[ ] Full launch dry run completed
```

---

## 15. Final Definition of Done

D1[2] is complete when ASHFALL's public-facing surface is no longer a manually curated approximation of the game.

A reviewer must be able to start from the public version and trace backward:

```text
public version
  → sealed release record
  → store kit manifest
  → screenshot recipe / claim / statement
  → build artifact + scenario + gate/metric/provenance evidence
  → authoritative runtime/data source
```

And the reverse direction must also work:

```text
runtime/data change
  → affected evidence identified
  → screenshot/claim/statement marked stale
  → regeneration/review
  → new sealed store kit
  → publication
```

The finished system should make false confidence expensive and truthful publication routine. A visually impressive mockup remains welcome as design material, but it cannot masquerade as the game. A planned accessibility feature remains valuable work, but it cannot appear as supported until the build demonstrates it. An AI/provenance statement remains reviewable prose, but its underlying asset facts cannot depend on memory. A demo remains a product artifact, but it cannot silently fork its saves, scenario, or source revision. A patch remains small, but it cannot bypass the release evidence chain.

The source plan's final guardrail is therefore elevated into the standing product rule for this integration family:

> **The shop window is part of the executable product contract. Every image, feature statement, support promise, and disclosure must be able to survive the same question: “Show the evidence from this build.”**

---

## 16. Filename Continuation

Use this exact sequence for subsequent D1 flagship integration plans:

```text
D1_planintegration.md
D1_planintegration[2].md
D1_planintegration[3].md
D1_planintegration[4].md
D1_planintegration[5].md
D1_planintegration[6].md
...
```

Always place the bracketed sequence number immediately before `.md`.
