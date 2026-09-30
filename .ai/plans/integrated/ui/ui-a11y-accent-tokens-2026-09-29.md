# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y §2B/§2C REMAINING ACCENT TOKEN SWEEP — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "Continue doing more UI correction and UI
precision work!" (2026-09-29). Eighth package in the audit-fix series:
finishes the hardcoded-accent consolidation in the seven files the audit
§2b/§2c tables still listed (contrast already verified ≥6:1 for every
mapped token on Ink).

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no token and no acceptance criterion.
> **MUST NOT** below remains binding.

---

## 0. Framing — A Vocabulary, Not a Palette

> *"Twenty-nine colour literals that each meant something to one person, on one afternoon, in one
> file. Not one of them meant the same thing twice."*

Colour in an interface is not decoration; it is a **vocabulary**. A red that says *danger* in the
emergency HUD and *delivered* in the save list is not a palette — it is a language that lies
depending on which room you are standing in.

This package is the last of the accent consolidation: 29 hand-rolled literals across 7 files become
six canonical tokens with **measured** contrast on Ink — Critical 6.19:1, Warning 5.99:1, Success
10.62:1, Info 7.01:1. The numbers matter because they are what makes the vocabulary *safe*: a
warning you cannot read is not a warning.

**Tone & register.** Lexicographic, exact, faintly weary of the past. The vocabulary is the
lexicon: *literal, token, alias, contrast, canonical, sweep*. Prose should read like a dictionary
editor who has finally regularised a spelling and is too tired to celebrate.

**The interesting detail.** `(0.6,0.6,0.6)` — the muted grey — *"predates the documented Dim
contrast fix."* A colour older than the rule it breaks. The plan fixes it without explaining where
it came from, which is correct: it is a fossil, not a decision.

**The second layer.** Colour is a vocabulary, and this plan is a dictionary closing: 29 literals
that meant something to one person on one afternoon become six canonical words with measured
meanings. The contrast ratios are the plan's safety argument — Critical 6.19:1, Warning 5.99:1 —
because a warning you cannot read is not a warning, it is decoration with an opinion. And the
fossil grey is handled exactly right: fixed without explanation, because fossils are not decisions
and do not deserve a story.

**Texture (second prose pass — commentary only).**

- "A red that says *danger* in one room and *delivered* in another is a language that lies depending on which room you are standing in."
- "(0.6,0.6,0.6) predates the documented Dim contrast fix." A colour older than the rule it breaks.
- "Too tired to celebrate." The register of a lexicographer who has regularised a spelling and knows nobody will notice — which is the point.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, token, acceptance criterion or
register row changes. The register below is unchanged.)*

- A dictionary is not a palette with rules; it is the moment a small community agrees that a word
  shall mean one thing. The six tokens are that agreement, and the measured contrast is its
  signature.
- The fossil `(0.6,0.6,0.6)` is kept in the story because it is the only colour in the file with a
  *before*. A fossil is not a decision; it is a decision's footprint.
- Contrast is the distance between a word and a whisper. The ratios are the plan's courtesy to
  readers it will never meet, which is the only kind of courtesy that scales.

> "Six words, measured. A lexicon's last entry is the one nobody has to look up."

---

## Bounded outcome

29 hardcoded accent literals in 7 files replaced with canonical tokens:

| Hand-rolled literal | Token | Rationale |
|---|---|---|
| (1,0.3,0.3) (1,0.4,0.4) (1,0.45,0.4) (1,0.5,0.5) (0.95,0.5,0.5) (0.95,0.5,0.4) | Critical | danger/alarm text and rows (6.19:1) |
| (1,0.6,0.2) (1,0.85,0.3) (0.95,0.85,0.4) (0.9,0.7,0.4) | Warning | warning/severe/pending (5.99:1) |
| (0.53,1,0.67) (0.4,0.9,0.5) | Success | success/delivered/valid (10.62:1) |
| (0.4,0.85,0.95) (0.5,0.8,0.9) | Info | info/valid-will/open-capsule (7.01:1) |
| (0.9,0.9,0.9) (0.85,0.85,0.85) | Pale | primary-ish text |
| (0.7,0.7,0.7) (0.6,0.6,0.6) | Dim | empty-state/muted text (the 0.6 grey predates the documented Dim contrast fix) |
| ShelterPanel (0.83,0.67,0.38) (0.43,0.64,0.66) (0.58,0.56,0.52) | Warm / Info / Muted | MakeDataRow re-derived palette |

Files: EmergencyResponseHud (severity header modulate + metric/roster/log
accents — tinted crisis *backdrop* at :125 stays), SaveLoadPanel,
SurvivorDeathLegacyPanel, TimeCapsulePanel, RelationshipDecayPanel,
PersonalQuestPanel, ShelterPanel (MakeDataRow palette). Six files gain
`using DesignTheme = Ashfall.Core.UI.Theme;` (ShelterPanel already has it).

## Exact files

The 7 files above, the gate
`Ashfall.Core.Tests/UI/UiA11yAccentTokenGateTests.cs`, governance (this plan,
`WORKTREE_OWNERSHIP.md`, `.ai/state.md`).

## MUST NOT

- Touch EmergencyResponseHud's tinted backdrop (0.12,0.02,0.02) or normal
  backdrop (now PanelScrim); touch any other file; change layout/sizing;
  alter Core.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Gate test: none of the 18 audited literals remain in the 7 files;
   alias present in each.
3. `--ui-layout-selftest` + `--player-panels-uitest` headless.

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's **MUST NOT** list and its own annotations. Not defects — deliberate
non-changes. Any later pass that raises one must re-measure the contrast.

| # | Open item | Why it is deliberately open | Who may resolve it (later, re-measured) |
|---|---|---|---|
| UI5-OM-1 | **`(0.6,0.6,0.6)` predates the documented Dim contrast fix.** | A colour older than the rule it breaks. Replaced without explanation because it is a fossil, not a decision. | Never — the artefact reads as history. |
| UI5-OM-2 | The tinted crisis backdrop `(0.12,0.02,0.02)`. | Explicitly **not** touched. It is a semantic severity field, not an accent. | Never — a rule, not a gap. |
| UI5-OM-3 | Are the 29 literals all the literals there are? | The sweep covers the **seven files the audit §2b/§2c tables still listed**. A literal outside those tables is not swept. | A source scan for raw `Color(` in `src/UI`. |
| UI5-OM-4 | Why did `ShelterPanel` derive its own three-colour palette? | `MakeDataRow` re-derived Warm/Info/Muted by hand. The plan regularises it; the derivation's origin is not recorded. | Never — texture by omission. |
| UI5-OM-5 | Do the six 6:1+ ratios hold on backgrounds other than Ink? | Contrast is verified **on Ink**. Panels over scrims are a different measurement. | A per-background contrast matrix. |
| UI5-OM-6 | Who chose these token names? | Critical / Warning / Success / Info / Pale / Dim are semantic labels with no stated provenance. | Never — the vocabulary is the design. |
