# Plan 57 — The Shop Window: Screenshots, Statements, and a Store Page the Build Can Prove

> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window*
> **Depends on:** 54 (the slice: the thing you show), 50A/56A (game vs marketing assets are
> separate), 25 (localization completion), 37 (input/accessibility facts), 46 (metrics), 48
> (release artifacts), `docs/AI_DISCLOSURE.md` (already a draft with placeholders).
>
> **Theme:** there is no store-facing material in the repository. No screenshots directory, no press
> kit, no credits, no accessibility statement, no feature matrix — and `docs/AI_DISCLOSURE.md` is a
> literal **draft "to fill in the bracketed placeholders before submission"** for Steam's AI-content
> questionnaire. The 30 images that do exist are QA goldens, and the 62 pretty "screens" in
> `assets/ui/Screens/` are unreferenced design mockups for consoles that don't exist. A launch
> prepared from this position invents claims that the build can't support — which is Wave 1's fake
> consoles, at a different scale.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | No store-facing structure exists | `ls -d screenshots docs/press docs/store CREDITS*` → **none**; project name is set (`project.godot:9`) and `LICENSE` exists |
| 2 | The AI disclosure is unfinished by design | `docs/AI_DISCLOSURE.md` header: *"Draft for Steam's AI-content questionnaire. **Fill in the bracketed placeholders before submission.** Delete sections that don't apply."* — still a template; `docs/HUMAN_AUTHORSHIP.md` exists alongside |
| 3 | The only rendered images are QA artifacts | `snapshots/*.png` → **30** at 1280×800 (fixture goldens, deliberately not marketing art); `snapshot-capture/` working copies |
| 4 | The pretty pictures are mockups of systems that don't exist | 62 `assets/ui/Screens/*.png` + 60 `assets/ui/HtmlBundles/*.html` with **0 code references**, several named for Wave 1's unbacked consoles (`subterranean_mining_geological_excavation_terminal`, `radio_intercept_morse_decryption_array`, `air_filtration_carbon_scrubber`, `expedition_return_decontamination_terminal`) |
| 5 | Provenance is already partly machine-readable | 50A's `asset_registry.json` carries `source` and AI-flag per asset (its step 9) — the disclosure statement can be **generated**, not remembered |
| 6 | Accessibility facts are becoming real | 37A/B/C (input map gate, keyboard-only navigation, controller, rebinding, text scale, reduce-motion, captions) + 25's locale layer — a store accessibility statement is writable **only** once these exist, and must not predate them |
| 7 | Localization status is measurable | Wave 3's 25C step 12 asks for exactly this: supported locales, string counts, untranslated remainder — currently no `docs/l10n/` status page exists |
| 8 | The slice supplies the truthful imagery | Wave 8's 54A/54C: a frozen scenario, a bootable demo preset, and a scorecard — the honest source of screenshots is a captured slice session |
| 9 | Metrics answer the questions a page asks | 46A targets (day-30 survival, first-crisis window), 46B funnel discovery rates, 54C completion bars — "dozens of systems" is a claim; "the average first storm is survived on day X" is a fact |
| 10 | Store technical plumbing exists but unproven | `export_presets.cfg` (Linux + Windows with `application/company_name`/`product_name`), `embed_pck=false`, `include_filter="*.json"`, Wave 3's 26B export+boot gate — nothing today asserts the shipped artifact's data or boot health on the store branch |
| 11 | Achievements/wishlist-grade systems | 34A (achievement catalog from state), 34C (legacy ledger), 38C (commitments) — the store-facing "features" list maps 1:1 onto these plans; an unshipped one must not appear on the page |
| 12 | Content volume is real and countable | 138 catalogs / 5,563 authored ids / 225 items / 118 broadcasts / 129 survivors / 30 heirlooms / 272 codex catalogs — and Wave 7's metrics are the honest way to phrase "how much game" |

---

## Task 57A — Generate store assets from a real session, and never from a mockup

**Goal:** a press/store kit whose images, claims, and provenance are produced by the build itself.

**Files:** new `scripts/store/capture_store_assets.py` (headless capture), new
`store/screenshots/`, `store/press/`, `store/capsule/` (gitignored binaries or LFS per 56B),
`assets/ui/Screens` (design mockups stay out of the game tree — 56A step 4),
`export_presets.cfg`, 54A slice scenario, 50A `asset_registry.json`,
`scripts/ci/generate-provenance-report.py`, `docs/AI_DISCLOSURE.md`, `store/README.md`.

### Substeps

1. **Capture from the slice, not from a staging scene**: run the frozen 7-day scenario headlessly and
   screenshot the real states (day 1 orient, ration decision, dispatch with route preview, storm,
   death + memorial, policy choice, day-7 deadline resolution) — six to eight images that are all
   literally in the shipped build.
2. **Standardise the capture matrix**: resolutions (1920×1080 native, 1280×800, plus the store's
   required ratios), locale variants where 25 ships them, and text-scale variants so the type is
   legible at store thumbnail size (the reverse of a golden: check the small, not the large).
3. **Never use a mockup, a concept render, or a Stitch export as a screenshot** — write that as a
   rule in `store/README.md`; Wave 1's 30 fake consoles came from exactly this gap between picture
   and capability.
4. **Generate the provenance/AI statement** from `asset_registry.json`'s source fields plus
   `docs/HUMAN_AUTHORSHIP.md`, replacing `AI_DISCLOSURE.md`'s bracketed placeholders with computed
   lists (assets AI-generated, AI-assisted, human-only), reviewed once and regenerated on drift.
5. **Capsule/banner art**: sourced from the manifest's *game* art families only (56B's split of
   game vs marketing assets), authored or commissioned as part of this task — the only new-art work
   Wave 9 sanctions, and only because 50A proved the game's own 114 MB is unmapped elsewhere.
6. **Store copy from metrics, with dates**: every claim cites the 46A sweep or 54C scorecard that
   supports it and the build sha; features list = shipped plan list (34A/34C/38C/42/44 statuses), so
   the page can't run ahead of the code.
7. **Content-completeness statement**: generated from `--content-utilization-selftest` (families,
   counts, `EFFECT_PRODUCED`/`SELECTED` per 45A) — the honest version of "hundreds of items".
8. **Demo policy decision, documented**: either ship the slice preset as a demo (recommended — it
   already exists and is gated) or state why not; a demo build's save isolation is 55B/39C's slot
   rules, not an afterthought.
9. **Verify the marketing artifact chain end-to-end**: export both presets through the staging
   script (26B), boot headless, load a corpus save (55B), capture, and record the sha in the kit
   manifest — so every image has a build behind it.
10. **Retention/versioning**: the kit carries the build sha + date; a store-page update requires the
    kit regenerated, and a stale kit fails the release checklist (48B step 6).
11. **Rights check**: fonts (Barlow Condensed, Share Tech Mono) license confirmation for commercial
    distribution, music/SFX provenance per 52C step 10, and the `LICENSE` file's scope stated.
12. **Tests**: capture script determinism (same scenario ⇒ same shot list), provenance generator
    consistency (an asset whose source field changed appears in the diff), a claim checker that fails
    a claim with no metric citation (a docs gate, 29B style).
13. **Docs**: `store/README.md` (rules, matrix, refresh procedure) and a `store/CHECKLIST.md` used by
    the release gate.

**DoD:** every image and sentence on the store page traces to a build, a metric, or a generated
report.

---

## Task 57B — Accessibility, localization, and input statements that survive audit

**Goal:** publish what the game actually supports — measured, gated, and dated — instead of a
marketing paragraph.

**Files:** new `docs/accessibility/STATEMENT.md` (generated), 37A/B/C gates, `docs/ui/SNAPSHOT_COVERAGE.md`,
`docs/l10n/STATUS.md` (25C step 12), `docs/ui/KEYBOARD.md` (37B step 12),
`docs/qa/LONG_SESSION_CHECKLIST.md` (55C), `ashfall-ui-access` / `ashfall-input-map-audit` outputs,
`store/CHECKLIST.md`, `scripts/ci/generate-support-statements.py`.

### Substeps

1. **Enumerate support claims to test**: remappable input, controller, keyboard-only completion,
   text scaling, colour-independent status, captions, reduce-motion, pausable simulation, no
   reflex/timing gate, save-anytime, screen-reader-adjacent labels (37C step 8's list).
2. **Back each claim with a gate**, not a sentence: 37A's input-map gate, 37B's mouseless day test,
   54A step 9's keyboard-only slice assertion, 37C's scale-overflow probe — and where no gate exists,
   the claim is **absent from the statement** (the honest default).
3. **Localize the statement itself**: the page describes locale support from 25C's `docs/l10n/STATUS.md`
   (locales, string counts, untranslated remainder, coverage %) — regenerated, never typed.
4. **Publish a known-limitations section** (a credibility asset): what *isn't* supported, with the
   plan number that will change it.
5. **Accessibility review by a second tool** per the repo's cross-tool QA rule — reviewer gets the
   claims + gates, not the reasoning.
6. **Requirements matrix**: minimum / recommended specs, measured from 26C's budget + 55C's long-session
   numbers on the low-end target, not guessed.
7. **Age/ratings posture**: derive from content facts (violence, substance use, themes, no romantic
   content per 44B's guardrails) and record the questionnaire answers as data under `store/`, so a
   re-submission doesn't rely on someone's memory.
8. **Verify the shipped settings surface matches the statement**: what the sliders exist for (buses,
   scale, rebinding, reduce-motion, captions) is generated from `UserSettings` fields (37C/52B) —
   otherwise the page describes a settings menu the build doesn't have.
9. **Support and recovery**: the doctor (56C step 4), safe-mode reset (37C step 10), the triage kit
   (48C step 7) and the save-recovery promise (39C) all get documented as user-facing support paths.
10. **Add a statement-freshness gate**: the doc generator fails if any claim's backing gate was
    removed or never ran on this build (29B's claims-gate, reused).
11. **Tests**: generator determinism, claim-without-gate rejection, and a fixture that removes a gate
    and proves the statement drops the claim.
12. **Docs**: `docs/accessibility/STATEMENT.md` + `store/CHECKLIST.md` entries; link from
    `docs/CURRENT_AUTHORITY.md`.
13. **Run the checklist** + the accessibility audits named in step 2.

**DoD:** the accessibility/localization page is a report the build regenerates, and every claim in it
has a gate.

---

## Task 57C — Launch operations: version cadence, demo freeze, patch policy, and the feedback path

**Goal:** what happens after the page goes live — a policy that matches the engineering already
built (48A/B/C, 55B, 46), rather than a wish.

**Files:** `docs/release/PROCESS.md`, `docs/release/VERSIONING.md` (48A),
`docs/release/HOTFIX.md` (48C), `store/CHECKLIST.md`, `docs/roadmap/RAILS.md` (53C),
`docs/balance/DECISIONS.md` (46A), 54C's scorecard, 39A's release gate, `CHANGELOG.md`,
`docs/telemetry/PRIVACY.md` (46B), new `docs/release/SUPPORT.md`.

### Substeps

1. **Freeze and date the demo**: the slice scenario hash + build sha are recorded; a demo change
   requires a re-cut, not a silent update (57A step 8, 54A step 3).
2. **Define the cadence honestly**: how often a release happens, what each tier contains (patch =
   fixes only; minor = content/reachability on 45A's ladder; major = any save/data/mod-contract
   break), and what triggers an emergency hotfix (48C).
3. **Save-compatibility promise in public words**: which versions a save will load across, derived
   from 48A's policy and 55B's corpus — the sentence a support forum needs first.
4. **Mod-support posture**: publish 47A's contract, state what's supported (data packs, overlays,
   tags) and what never will be (code, assemblies), and freeze the promise with the fixture-pack CI
   suite (47C) as evidence.
5. **Feedback path**: what a player sends with a bug (the triage kit, 48C step 7), where issues go,
   what an anonymised report contains, and the explicit privacy stance (46B step 1, 57B step 9).
6. **Content-update pipeline for post-launch**: new families run the same intake (53C) and
   acceptance ladder (45A), ship as data packs where possible (47B) — so updates don't require a
   binary rebuild, which is also the emergency lever (48C step 5).
7. **Known-issues register** generated from the audit/wave indexes and open gates, published with
   each release (29C's wave ledger, 48B step 9's honest patch notes).
8. **Post-launch metrics review**: 46C's ritual, extended to real (opt-in) player data — the first
   time the synthetic funnel and human funnel can be compared.
9. **Balance-patch discipline**: any tuning change ships with its sweep + ADR (46A step 6) and its
   slice scorecard delta (54C step 6) — the antidote to "they nerfed something, no idea what".
10. **Support escalation ladder**: crash → triage kit → day record → seed replay reproduction →
    hotfix decision (48C), each step an existing tool, none invented here.
11. **Anniversary/telemetry-driven roadmap**: what gets built next comes from 54C's drop points and
    53B's `NEXT` band, so post-launch scope is evidence-led rather than feature-led — the closing
    gesture of this whole audit series.
12. **Tests**: the release checklist script fails on a missing statement, unversioned demo, or
    unattributed balance patch.
13. **Docs**: `docs/release/SUPPORT.md` (public-facing summary of all of the above), linked from the
    store kit and `CURRENT_AUTHORITY.md`.
14. **Run the checklist** + `release-gate.sh` + a dry-run of the full launch process.

**DoD:** launch is a repeatable, gated procedure — page, demo, patches, support, and updates all
generated from what the build can prove.

---

## Cross-Task Dependencies

```
54A/54C (slice, scorecard) ──► 57A steps 1,6,9        50A/56B (manifest, asset split) ──► 57A steps 4,5
25C/37A-C (l10n, input, a11y) ──► 57B step 2         46A/46B (metrics, telemetry) ──► 57A step 6, 57C step 8
48A/48B/48C (versions, release, hotfix) ──► 57C 1–4   47A/47C (mod contract) ──► 57C step 4
55B (save corpus) ──► 57C step 3                       53C (intake) ──► 57C step 6
```

**Execution order:** 54A → 54C → 57A → 57B → 57C. Nothing in this plan can precede the slice: an
artifact-free store page is where the fake consoles go to become public statements.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/store/capture_store_assets.py --check          # deterministic shot list
7. python3 scripts/ci/generate-provenance-report.py --check      # no bracketed placeholders left
8. python3 scripts/ci/generate-support-statements.py --check      # claims ↔ gates
9. bash scripts/ci/export-smoke-boot.sh (both presets) + demo slice preset boot
10. LFS/attributes: bash scripts/ci/lfs-health-check.sh (marketing + kit assets)
11. bash scripts/ci/release-gate.sh && bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Docs/Kit | Gates | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 57A | 1 capture script | store kit | 2 | 4–7 | Medium (art: capsules) | LOW — but **high reputational risk if a mockup ever becomes a screenshot** |
| 57B | 1 generator | statements | 1 | 4–6 | Medium | LOW |
| 57C | 0 | process docs | 1 checklist | 2–4 | Medium | LOW |

**Guardrails:** no marketing claim without a gate or a metric; no screenshot that isn't a captured
build; no publishing an accessibility line the settings menu can't back; no demo drift from the
frozen scenario; no AI-disclosure placeholders at submission; no new-art commission outside
capsules/banners; and no roadmap promise that 53C's intake hasn't accepted — Waves 1–8 found the same
bug at every altitude; a store page is where it stops being internal.
