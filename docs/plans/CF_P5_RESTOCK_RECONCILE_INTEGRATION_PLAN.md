# CF-P5-RESTOCK-RECONCILE — Merchant Restock Priority Ledger Reconciliation & Ratification

**Package:** `CF-P5-RESTOCK-RECONCILE` (completion-first program "Plan 03", roster entry 02)
**Anchor:** `DEC-05` (SIGNED, `docs/governance/DECISION_REGISTER.md`), Wave 9 Part 2 C1 (SEALED), `FOLLOWUPS-210-213-THINSEAMS` row in `INTEGRATION_PLANS.md`
**Plan type:** Ledger-truth reconciliation + foreman ratification line. **Zero production code, zero data, zero test changes.**
**Status of underlying feature:** LIVE and GREEN since 2026-09-17 (Wave 9 Part 2 C1, Option C, user-authorized).
**Plan authored:** 2026-09-19, by the ashfall-plan role, from current source re-verified at HEAD `65357b8a`.
**Entry gate (from program):** a foreman ratification line only (wording-only signature that the existing SIGNED decision + live implementation are accepted as the final state). Ledger-text edits are integrator-owned (`claim-wave11-part2-execution-2026-09-18` claims `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, `docs/governance/DECISION_REGISTER.md`).

---

# 1. Objective

Bring the three governance ledgers that touch merchant restock priority back into
agreement with the already-signed decision (`DEC-05`) and the already-live
implementation, and record one foreman ratification line so the question can never
be re-opened as if it were still pending.

Concretely, the bounded outcome is:

1. `INTEGRATION_PLANS.md` no longer describes merchant restock priority as
   "Still deferred with authority question … a priority ordering needs a signed
   design" — in **both** places where that stale claim currently appears (the
   `RESCUE-SIGNAL-RUNTIME` status paragraph and the `FOLLOWUPS-210-213-THINSEAMS`
   package row).
2. `docs/governance/DECISION_REGISTER.md` row `DEC-05` keeps its `SIGNED`
   verdict (unchanged) but its evidence field is corrected from the stale
   "(14/14 PASS)" count to the true, re-verified "(6/6 PASS)" count, with a
   dated drift note.
3. `docs/plans/wave9_part2/C1_DECISION.md` — the memo `DEC-05` cites as its
   source — has its blank signature block back-filled with the DEC-05
   ratification, so the memo, the register, and the queue ledger all agree.
4. `KNOWN_DEBT.md` is **confirmed, by grep, to carry no restock-priority row**
   and is therefore intentionally **not edited** — the reconcile records that
   confirmation rather than inventing a debt row that was never tracked.
5. A foreman ratification line (wording-only) is recorded where the reconcile
   lands, ratifying DEC-05's wording across the three ledgers:
   *priority-weighted tier restock with deterministic ordering; trade ledger
   updates canonically; Option C (evaluation/display order only) is the final
   state.*

**Non-goals (explicit):** no restock behavior change; no new priority feature;
no quantity scaling, slot caps, or scarcity-driven selection; no production
binding of the `PriorityScorer` seam; no JSON schema change; no new tests; no
full-suite run; no edits to historical corpus documents.

---

# 2. Current Reality

Everything in this section was re-verified by direct reading/grep/test execution
at HEAD `65357b8a` on 2026-09-19 during plan authoring. Line numbers are current
as of that commit.

## 2.1 The live implementation (production, engine-free Core)

**File:** `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs` (netstandard2.1
Core; no Godot/UnityEngine references — verified by namespace imports:
`System`, `System.Collections.Generic`, `Ashfall.Core`, `Ashfall.Core.Inventory`,
`Ashfall.Core.Shelter`).

### 2.1.1 The priority scorer seam (line 136)

```csharp
/// <summary>
/// Wave 9 Part 2 C1 (Option C) — optional priority scorer for stock items
/// (e.g. category scarcity index, active shocks, or trade pressure).
/// When unset, items evaluate using their authored price multiplier and ID ordinal.
/// </summary>
public Func<string, int>? PriorityScorer
{
    get => _priorityScorer;
    set => _priorityScorer = value;
}
```

Verified: **no production host code sets `PriorityScorer`.** A repo-wide grep
for `PriorityScorer` returns exactly one hit in `src/`'s scope — none; the only
assignments are in `Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs`
(lines 59 and 134). Production therefore runs with `_priorityScorer == null`,
and the ordering reduces to authored `price_multiplier_bp` descending with the
ordinal item-ID tie-break. The seam is a tested extension point, not a live
binding — that is exactly what Option C signed.

### 2.1.2 The pure scoring function (line 283)

```csharp
/// <summary>
/// Wave 9 Part 2 C1 (Option C) — pure deterministic priority score calculation.
/// Higher score = higher priority. Tie-break is deterministic by ordinal item ID.
/// </summary>
public static int ComputeItemPriorityScore(CaravanStockItem item, MerchantCaravanDef def, Func<string, int>? customScorer = null)
{
    if (item == null) return 0;
    int score = item.price_multiplier_bp;
    if (customScorer != null && !string.IsNullOrEmpty(item.item_id))
    {
        score += customScorer(item.item_id);
    }
    return score;
}
```

Properties proven by test and reading:

- **Pure and total:** no instance state read, no RNG, no clock, no catalog
  lookup. `def` is accepted for signature stability but not consulted in the
  current formula.
- **Null-safe:** null item → 0; null/empty `item_id` → scorer not invoked.
- **Additive composition:** score = authored `price_multiplier_bp` + optional
  custom scorer delta. The authored multiplier is the base weight, so authored
  manifest intent dominates unless a scorer explicitly overrides.
- **Overflow-safe in practice:** scores are `int` basis-point-scale values
  (authored multipliers are 8500–12500 in shipped content; test scorers use
  ≤5000 deltas). No clamping is applied because Option C never feeds the score
  back into quantities or prices.

### 2.1.3 The deterministic ordering (lines 299–316)

```csharp
/// <summary>
/// Wave 9 Part 2 C1 (Option C) — returns the caravan's stock sorted deterministically
/// by priority score descending, then item ID ascending (stable tie-break).
/// All authored items and quantities are preserved exactly (Option C: order only).
/// </summary>
public IReadOnlyList<CaravanStockItem> GetPrioritizedStock(MerchantCaravanDef def)
{
    if (def?.stock == null || def.stock.Count == 0)
        return Array.Empty<CaravanStockItem>();

    var list = new List<CaravanStockItem>(def.stock);
    list.Sort((a, b) =>
    {
        int scoreA = ComputeItemPriorityScore(a, def, _priorityScorer);
        int scoreB = ComputeItemPriorityScore(b, def, _priorityScorer);
        int cmp = scoreB.CompareTo(scoreA); // descending priority
        if (cmp != 0) return cmp;
        return string.Compare(a.item_id, b.item_id, StringComparison.Ordinal); // ascending tie-break
    });
    return list;
}
```

Properties:

- **Non-mutating:** copies `def.stock` into a new list before sorting; the
  authored manifest order is never disturbed. (`List<T>.Sort` is an unstable
  introspective sort in general, but the comparator is a *total order* — score
  descending, then ordinal ID ascending with no possible second tie — so the
  output is fully determined regardless of sort stability.)
- **Deterministic tie-break:** `StringComparison.Ordinal` — culture-invariant,
  machine-invariant, exactly the determinism contract AGENTS.md requires.
- **Order-only semantics (Option C):** every authored item and quantity is
  preserved; the sort influences insertion/display order, never inclusion.

### 2.1.4 The restock consumption (lines 316–330)

```csharp
private void RestockCaravan(MerchantCaravanDef def, CaravanRuntimeState cState)
{
    cState.remainingStock.Clear();
    var stockItems = GetPrioritizedStock(def);
    foreach (var item in stockItems)
    {
        // Plan 147: per-arrival day gate — high-tier stock only enters
        // the manifest from its gate day (real campaign state). Stock is
        // then pinned for the whole stay; no reroll by reopening.
        int qty = item.quantity;
        if (item.available_from_day > 0 && _state.currentDay < item.available_from_day)
            qty = 0;
        cState.remainingStock[item.item_id] = qty;
    }
}
```

`RestockCaravan` has exactly **two call sites**, both verified by grep at HEAD:

- **Line 273** — inside `EnsureCaravanState`: first-time registration of a
  caravan seeds its runtime state through the same prioritized restock path.
- **Line 532** — inside `TickDay`, strictly on the arrival edge
  (`shouldBePresent && !cState.isAtAirlock`). Presence is schedule-driven:
  `shouldBePresent = (day % def.schedule_period_days) < def.stay_duration_days`.

This is the stay-pinned invariant: restock is evaluated **once per arrival**;
for the remainder of the stay, `remainingStock` mutates only through executed
trades. Reopening a panel, re-entering the airlock UI, or re-querying
`GetPrioritizedStock` cannot reroll, restore, or recompute pinned stock
(proven by test 5, §18).

### 2.1.5 The Plan 147 day gate

`CaravanStockItem.available_from_day` (default 0 = always available) is the
per-arrival gate. A gated item enters `remainingStock` with quantity **0** (not
omitted) until the arrival day reaches the gate. The only shipped producer of
non-zero gates is the contraband broker (see §2.3); the four legacy caravans
and `merchant_caravans.json` carry no gate fields.

### 2.1.6 UI consumption (Option C's visible half)

`src/UI/ShelterBarterPanel.cs:680`:

```csharp
var stockItems = _barterSystem != null ? _barterSystem.GetPrioritizedStock(caravan) : (IReadOnlyList<CaravanStockItem>)caravan.stock;
```

The panel iterates the prioritized list when bound to the live system, with a
byte-identical fallback to authored order when unbound (harness/specimen path).
The panel owns no gameplay rule: it reads `cState.remainingStock` for
availability, clamps its own request counters to availability, and renders
unit price from the same `price_multiplier_bp` the Core uses (`:697`). Display
order is the only thing Option C changed for the player.

## 2.2 Host wiring (Godot side, `src/`)

**File:** `src/Main.Plans147.cs` (partial of `Main`).

- `:198` — `private ShelterBarterSystem? _shelterBarter;`
- `:233` — `EnsureShelterBarter()` lazily constructs the system:
  - `:240` — RNG: `_campaignDay.Rng.Fork("shelter_barter")` (campaign-forked
    seeded stream) with a `new SeededRng(147)` fallback when no campaign day
    coordinator exists (harness path).
  - `:244-249` — constructor receives the campaign inventory authority
    (`SetupInventory()` first; never a fabricated empty inventory — comment
    cites INV-16.3), `thermalSystem: null` (airlock-freeze gating stays a
    Plan-54 refinement; the broker route does not depend on it), `GodotLog`,
    and the canonical item catalog lookup.
  - `:251-253` — registers the contraband broker built from the contraband
    activation map **before** state restore (registration-precedes-restore
    ordering).
  - `:256-259` — `ShelterBarterSaveStore.TryLoad()` → `RestoreState(saved)`.
  - `:261-273` — `OnCaravanArrived` / `OnCaravanDeparted` journal projections
    (`shelter_barter_arrival_{id}` / `shelter_barter_departure_{id}`).
- `:305-311` — `SaveShelterBarter()` →
  `CaptureSection("shelter_barter", ShelterBarterSaveStore.TryCapturePersisted(_shelterBarter.CaptureState()))`.
- `:314-317` — `TickShelterBarterDay(int day)` → `_shelterBarter?.TickDay(day)`.

**Day-pipeline position:** `src/Main.CampaignOwners.cs:1310` calls
`_m.TickShelterBarterDay(day)` immediately after the contraband stash tick
(`:1307`) and before plastic pyrolysis (`:1313`) — a fixed, deterministic
position in the daily owner sequence.

**Persistence:** `src/Host/ShelterBarterSaveStore.cs` — section name
`shelter_barter` (`:28`), file `shelter_barter_save.json` (`:27`), thin facade
over `SaveStoreHub.FromCodec` with the `SchemaVersionedEnvelope<ShelterBarterSaveState>`
adapter (`{ SchemaVersion, State, Checksum }`). No pre-envelope legacy format
ever existed for this section (documented in the store header).

## 2.3 The contraband broker (Plan 147's gate producer)

`Assets/Ashfall.Core/Narrative/ContrabandBrokerCaravan.cs` (engine-free Core,
`Ashfall.Core.Narrative`):

- `CaravanId = "caravan_contraband_broker"`, display name "The Quiet Counter",
  faction `faction_wasteland_outlaws`, period 10 days, stay 2 days,
  `barter_tolerance_bp = 10000`, `counterfeit_risk_bp = 0`,
  `use_canonical_item_values = true`.
- `ScarcityPremiumBp = 12500` (1.25x over canonical `tradeValue`): every broker
  stock line is priced at the same premium, so **every broker item has an equal
  authored priority base** — broker ordering is therefore decided by the
  ordinal tie-break unless a `PriorityScorer` is attached. Buy→sell
  round-trips strictly lose value (no arbitrage; the item table stays the
  value authority).
- `Build(...)` (line ~55) maps each reviewed `ContrabandStashActivation` to one
  stock line: `available_from_day = Math.Max(0, activation.minDay)` — the stash
  route and the barter route share one gate authority (the reviewed activation
  map), exactly as the type header documents.
- Stock lines are pre-sorted by `entryId` ordinal at build time — deterministic
  input order into the priority sort.

## 2.4 The data authority mirror

`Assets/StreamingAssets/Data/merchant_caravans.json` exists (schema_version 1)
and mirrors the four code-registered default caravans — `caravan_scrap_salvagers`,
`caravan_permafrost_traders`, `caravan_medic_syndicate`,
`caravan_black_market_munitions` — with the same stock IDs, quantities, and
multipliers, and **no `available_from_day` fields** (grep-verified).

Verified nuance the plan must record honestly: `ShelterBarterSystem.LoadCatalog`
(`:162`) can parse this file, and `CatalogPath = "merchant_caravans.json"` is
declared (`:82`), but **no host code currently calls `LoadCatalog`** — the host
path uses `RegisterDefaultCaravans()` (constructor) plus the code-built broker.
The JSON is kept non-orphan by explicit scanner mapping
(`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:525` and `:1191`:
`["merchant_caravans.json"] = new[] { "ShelterBarterSystem" }`). This is a
pre-existing, accepted mirror arrangement; the reconcile does **not** touch it
(any change here would be a data-authority decision, not a ledger fix).

## 2.5 Test evidence

`Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs` — **exactly 6
`[Fact]` methods, born with exactly 6** (git: the file was introduced in commit
`8f72ee62` "feat: integrate follow-up ASHFALL waves" with 150 lines and 6
facts; it has never had any other count).

Re-run during plan authoring, 2026-09-19:

```
$ bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs
[ashfall-tests] target=...Plan147RestockPriorityTests.cs classes=1 timeout=180s
Passed!  - Failed: 0, Passed: 6, Skipped: 0, Total: 6, Duration: 14 ms
```

Full per-case enumeration is in §18.

## 2.6 The three-ledger disagreement (the defect this package fixes)

| # | Document | Location | Current text says | Truth at HEAD |
|---|---|---|---|---|
| L1 | `INTEGRATION_PLANS.md` | line 213 (`RESCUE-SIGNAL-RUNTIME` status paragraph) | "merchant-restock priority deferred pending a signed design" | **STALE** — design signed 2026-09-17 (DEC-05), implemented same wave |
| L2 | `INTEGRATION_PLANS.md` | line 218 (`FOLLOWUPS-210-213-THINSEAMS` package row, acceptance cell) | "**Still deferred with authority question:** merchant restock \"priority\" (restock already day-gated per Plan 147; a priority ordering needs a signed design)." | **STALE** — same |
| L3 | `docs/governance/DECISION_REGISTER.md` | line 19, row `DEC-05`, evidence field | "`Plan147RestockPriorityTests.cs` (14/14 PASS), `ShelterBarterSystem.cs`" | Verdict `SIGNED` **correct**; count **STALE** — the file has 6 facts and passes 6/6; it has never had 14 |
| L4 | `docs/plans/wave9_part2/C1_DECISION.md` | "Foreman Signature Gate" section | `- **Chosen Option:** [PENDING FOREMAN DECISION]` / `- **Signer:** [User / Foreman]` / `- **Date:** [YYYY-MM-DD]` | **STALE-BY-OMISSION** — the memo DEC-05 cites as its source was never back-filled after the 2026-09-17 user authorization (recorded in `WAVE9_PART2_CLOSEOUT.md`: "User Authorized C1 Option C") |
| L5 | `KNOWN_DEBT.md` | — | (no restock-priority row exists) | **CORRECT AS-IS** — grep for `restock`, `priority`, `caravan`, `barter`, `merchant`, `147` finds only unrelated rows (DEBT-194 "priority order" phrasing; DEBT-PLAN32 "Traveling caravans"). No row is owed: the Wave 9 Part 2 C1 seal was recorded as closed, so there was never an open debt to retire |

Supporting documents that are **already truthful** and need no edit:
`docs/plans/wave9_part2/WAVE9_PART2_CLOSEOUT.md` (C1 "SEALED …
`Plan147RestockPriorityTests` 6/6 PASS"),
`docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` (roster row 2 states the
implementation is live and 6/6 was re-run), and the two Seal-steps program
documents that reference the sealed scorer.

---

# 3. Required Delta

Four text edits and one ratification line. Nothing else.

## D1 — `INTEGRATION_PLANS.md` line 213 (status paragraph parenthetical)

**Current (exact bytes at HEAD):**

```
merchant-restock priority deferred pending a signed design
```

**Required:**

```
merchant-restock priority sealed 2026-09-17 by DEC-05 (Option C — deterministic display-order priority, live in ShelterBarterSystem)
```

**Reason:** the parenthetical sits inside the `RESCUE-SIGNAL-RUNTIME` batch
status line that summarizes `FOLLOWUPS-210-213-THINSEAMS`; leaving it would
keep the top-level batch summary lying even after the row below is fixed.

## D2 — `INTEGRATION_PLANS.md` line 218 (package row acceptance cell)

**Current (exact bytes at HEAD):**

```
**Still deferred with authority question:** merchant restock "priority" (restock already day-gated per Plan 147; a priority ordering needs a signed design).
```

**Required:**

```
**Sealed (DEC-05, signed 2026-09-17):** merchant restock priority is live — `ShelterBarterSystem.ComputeItemPriorityScore` (pure: authored price_multiplier_bp + optional PriorityScorer seam) consumed by `GetPrioritizedStock` on the arrival restock (`:319`) and by `ShelterBarterPanel` (`:680`); Option C = ordering only, quantities/inclusion unchanged; `Plan147RestockPriorityTests` 6/6 PASS (re-verified 2026-09-19).
```

**Reason:** replaces the stale "needs a signed design" claim with the signed
decision id, the live binding evidence, and the current test count — the same
shape the sibling "Follow-up truth (2026-09-17)" sentence in the same cell
already uses for the black-market item.

## D3 — `docs/governance/DECISION_REGISTER.md` line 19 (DEC-05 evidence field)

**Current:**

```
`Plan147RestockPriorityTests.cs` (14/14 PASS), `ShelterBarterSystem.cs`
```

**Required:**

```
`Plan147RestockPriorityTests.cs` (6/6 PASS; count corrected from 14/14 on 2026-09-19 — the file has contained exactly 6 facts since introduction in 8f72ee62; verdict unchanged), `ShelterBarterSystem.cs`
```

**Reason:** the register's invariant is that every row is terminal *and
truthful*. The verdict is right; the evidence count is not. The drift note
follows the register's existing convention of dating corrections (cf. DEC-08,
DEC-20 rows) and preserves the audit trail instead of silently rewriting
history. Note the correction is "14→6", and it is a **correction of a
consolidation-time miscount**, not a post-hoc test reduction — see §4, E7.

## D4 — `docs/plans/wave9_part2/C1_DECISION.md` (signature block back-fill)

**Current:**

```
- **Chosen Option:** [PENDING FOREMAN DECISION]
- **Signer:** [User / Foreman]
- **Date:** [YYYY-MM-DD]
```

**Required:**

```
- **Chosen Option:** Option C — Evaluation/Display Order Only (ratified as `DEC-05`, SIGNED 2026-09-17; wording ratified verbatim by the CF-P5 reconcile, 2026-09-19)
- **Signer:** User / Foreman (Wave 9 Part 2 authorization; recorded in `docs/governance/DECISION_REGISTER.md` row `DEC-05` and `docs/plans/wave9_part2/WAVE9_PART2_CLOSEOUT.md`)
- **Date:** 2026-09-17 (decision); ratification re-confirmed 2026-09-19 (CF-P5)
```

The three standing conditions already listed in the memo (arrival-edge restock
only; stock pinning / same-day no-reroll absolute; no new RNG streams or save
sections) are kept verbatim — they are all honored by the shipped
implementation and remain the rule for any future modification.

**Reason:** DEC-05's "Memo / Source" column points at this file; a signed
register row whose source memo still says `[PENDING FOREMAN DECISION]` is an
internal contradiction the next governance audit (DEC-08's periodic cadence)
would flag.

## D5 — `KNOWN_DEBT.md`: **NO EDIT** (recorded confirmation)

Grep evidence (2026-09-19): no row mentions restock priority, Plan 147 barter
priority, or the C1 package. The correct reconciliation action is to record in
this plan and in the handoff that the check was performed and no row is owed.
Inventing a RETIRED row would fabricate debt history that never existed.

## D6 — Foreman ratification line (entry gate F2, wording-only)

Proposed verbatim wording (foreman may adjust wording, not substance):

> Ratified 2026-09-19 (CF-P5-RESTOCK-RECONCILE): `DEC-05` "Merchant Restock
> Priority Formula" stands as signed — priority-weighted, deterministic
> display-order restock (Option C) is the final state; the live implementation
> in `ShelterBarterSystem` and the 6/6 `Plan147RestockPriorityTests` evidence
> are accepted; no further design signature is owed.

The line is recorded in the D2 replacement text (by reference to DEC-05) and in
the D4 back-fill (by date), so register, memo, and queue ledger carry the same
ratification facts.

---

# 4. Evidence

Every claim above is backed by one of the following, gathered 2026-09-19 at
HEAD `65357b8a` unless noted:

| # | Evidence | Method | Result |
|---|---|---|---|
| E1 | `ShelterBarterSystem.cs` live implementation | Full file read | `PriorityScorer` at `:136`; `ComputeItemPriorityScore` at `:283`; `GetPrioritizedStock` at `:299` with sort body `:305–315`; `RestockCaravan` at `:316` consuming the sort at `:319` |
| E2 | Restock call sites | `grep -n "RestockCaravan"` | Exactly two production call sites: `:273` (EnsureCaravanState seeding) and `:532` (TickDay arrival edge); plus the definition at `:316` |
| E3 | No production PriorityScorer binding | Repo-wide `grep -rn "PriorityScorer"` | Only the property declaration (`ShelterBarterSystem.cs:136`) and test assignments (`Plan147RestockPriorityTests.cs:59`, `:134`). Zero hits in `src/` |
| E4 | Test suite green | `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs` | `Passed: 6, Failed: 0, Skipped: 0` (14 ms), within the 180 s policy cap |
| E5 | Test count has always been 6 | `git show 8f72ee62:…Plan147RestockPriorityTests.cs \| grep -c "\[Fact\]"` | `6`. The file was introduced in `8f72ee62` with 150 lines and 6 facts |
| E6 | "14/14" origin | `git log -S "14/14 PASS" -- docs/governance/DECISION_REGISTER.md` | The string was introduced in commit `8f72ee62` — the **same commit** that introduced the 6-fact test file. The "14/14" was therefore never true of this file; it is a consolidation-time miscount (Wave 10 Part 2 Task E1 consolidated 19 decision memos into the register), not evidence of removed tests |
| E7 | Stale queue-ledger text | `sed -n '213p;218p' INTEGRATION_PLANS.md` | Both stale strings extracted byte-exactly (quoted in §3 D1/D2) |
| E8 | Register row location | `grep -n "14/14 PASS" docs/governance/DECISION_REGISTER.md` | Line 19, row `DEC-05`, verdict `SIGNED`, execution package "Wave 9 Part 2 C1 (`SEALED`)" |
| E9 | Memo signature block | Read `docs/plans/wave9_part2/C1_DECISION.md` | `[PENDING FOREMAN DECISION]` block still blank; memo's own conditions 1–3 match the shipped behavior |
| E10 | KNOWN_DEBT absence | `grep -in "restock\|priority\|caravan\|barter\|merchant\|147" KNOWN_DEBT.md` | No restock-priority row; only unrelated "priority"/"caravan" phrasings in DEBT-194 and DEBT-PLAN32 |
| E11 | Host wiring | Read `src/Main.Plans147.cs:190–320`, `src/Host/ShelterBarterSaveStore.cs`, `grep TickShelterBarterDay src/` | RNG fork `"shelter_barter"` at `:240`; broker registration before restore (`:251–259`); save section `shelter_barter`; daily tick at `Main.CampaignOwners.cs:1310` |
| E12 | Panel consumption | Read `src/UI/ShelterBarterPanel.cs:655–714` | `GetPrioritizedStock(caravan)` at `:680` with authored-order fallback; unit price from same bp multiplier at `:697` |
| E13 | Broker day-gate producer | Read `Assets/Ashfall.Core/Narrative/ContrabandBrokerCaravan.cs` | `available_from_day = Math.Max(0, activation.minDay)` at `:55`; flat 12500 bp premium on all broker stock |
| E14 | Data mirror status | `head -40 …/merchant_caravans.json`; `grep caravan_id/available_from_day` on the file; `grep merchant_caravans …ContentUtilizationScanner.cs` | 4 caravans mirroring code defaults; zero `available_from_day` fields; scanner maps the file to `ShelterBarterSystem` (`:525`, `:1191`); `LoadCatalog` (`:162`) has **no production caller** (grep over `src/`) |
| E15 | Prior audit agreement | Read `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` row 2; `docs/plans/wave9_part2/WAVE9_PART2_CLOSEOUT.md` §2.1 | Both already state live + 6/6 + SEALED; consistent with this plan |
| E16 | Claim ownership of ledgers | Read `WORKTREE_OWNERSHIP.md` rows | `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, `docs/governance/DECISION_REGISTER.md` are exact paths of ACTIVE `claim-wave11-part2-execution-2026-09-18` (Integrator); also listed in several DONE wave claims |
| E17 | Closeout's own count | `WAVE9_PART2_CLOSEOUT.md` (dated 2026-09-17) | Already says "6/6 PASS" — the closeout written one day after execution disagrees with the register's "14/14", corroborating the miscount finding |
| E18 | Authority-map origin | `docs/foreman/PLANS_210_213_FLAGSHIP_ECONOMY_AUTHORITY_MAP.md:126-127` | The original requirement ("query category indices for restock priority — stock refresh cadence stays with the caravan owners") is historical context for what DEC-05 scoped down to Option C; the map is a historical record and is not edited |

**Contradictions found while verifying (refinements to the original briefing):**

1. The briefing/program implied the stale queue text lives only in the
   FOLLOWUPS row; it actually appears **twice** (§2.6 L1 and L2). Both are in
   scope.
2. The "14/14 → 6/6" change is not post-hoc count drift (tests were never
   removed); it is a same-commit miscount (E5/E6). The replacement text worded
   in D3 records this accurately so a future auditor does not go hunting for 8
   "deleted" tests.
3. `KNOWN_DEBT.md` genuinely has no row; the reconcile's correct action there
   is a recorded no-op, not an edit (D5).
4. The memo's blank signature block (L4) is a third real staleness the
   briefing only alluded to ("rides F1"); it is promoted here to a first-class
   delta (D4) because the register cites the memo as its source.

---

# 5. Existing Extension Seams

All of these exist today and none require modification for this package; they
are enumerated to prove the reconcile needs no new architecture and to document
where any *future* priority work must attach (rather than forking).

| Seam | Location | Status today | Rule for future use |
|---|---|---|---|
| `PriorityScorer` property | `ShelterBarterSystem.cs:136` | Unset in production; exercised by tests | A production binder (e.g. a scarcity-driven scorer sourced from `MarketSystem` category indices) is the **only** sanctioned way to make priority dynamic; it requires a new signed decision because DEC-05 scoped Option C as display-order with authored bases |
| `ComputeItemPriorityScore(item, def, customScorer)` | `:283` | Static, pure, public | Reusable by any future projection without instantiating the system |
| `GetPrioritizedStock(def)` | `:299` | Live: restock insertion order + panel display | Read-only; safe to call at any time (no state mutation) |
| `available_from_day` gate | `CaravanStockItem` field | Live via broker (activation `minDay`) | Authored/day-gated content flows through JSON→activation map→broker build; no second gate authority |
| `LoadCatalog(jsonContent)` | `:162` | Parse-capable, uncalled in production | The `merchant_caravans.json` mirror can become load-bearing later via this seam without touching the system core |
| `use_canonical_item_values` + `_itemLookup` | def field + ctor param | Live on the broker | Keeps broker pricing on the canonical item `tradeValue`; no second pricing authority |
| Save seam | `CaptureState`/`RestoreState` + `ShelterBarterSaveStore` | Live, versioned envelope | Any future persistent priority state must ride this section — but Option C persists **no** priority state, and this package keeps it that way |
| Events | `OnCaravanArrived` / `OnCaravanDeparted` / `OnBarterStateChanged` | Live → journal + panel refresh | Presentation-only consumers; no gameplay decisions hang off them |

Collision check conclusion: there is no partial or parallel restock-priority
implementation anywhere else in the repo (grep for `ComputeItemPriorityScore`,
`GetPrioritizedStock`, `restock priority` across `Assets/`, `src/`, `docs/`,
`C-integration-plans/` confirms a single implementation plus governance
references). Extension is therefore not just preferred — there is nothing to
duplicate.

---

# 6. Proposed Architecture

No new runtime architecture is proposed. The "architecture" of this package is
the **ledger-of-truth topology** the project already operates, and the delta is
to make the restock-priority entries consistent within it:

```
Historical requirement          Decision source memo           Standing verdict register        Live queue ledger              Debt ledger
(authority map §2.3 item 6) →   wave9_part2/C1_DECISION.md  →  DECISION_REGISTER.md DEC-05  →   INTEGRATION_PLANS.md rows      KNOWN_DEBT.md
"restock priority" scoped       Options A/B/C/D analysis;      SIGNED 2026-09-17;             (a) batch status line           (no row —
by Wave 9 Part 2 C1 to          signature block ← D4           evidence ← D3                   (b) FOLLOWUPS row ← D1/D2       confirmed, D5)
Option C                        back-fill                                                     both → "Sealed (DEC-05)"
```

Reconciliation rules this package applies (and which any future ledger
reconcile should reuse):

1. **The register is the verdict authority.** Its verdict fields are never
   edited by a reconcile; only demonstrably false *evidence* fields are
   corrected, always with a dated drift note, never silently.
2. **The queue ledger (`INTEGRATION_PLANS.md`) reflects the register.** When a
   row's prose contradicts a terminal register verdict, the row loses; the
   edit points at the decision id and the live evidence.
3. **The debt ledger is not a trash can for history.** Absence of a row, when
   verified, is recorded as a confirmation, not "fixed" by inventing a row.
4. **Source memos get back-filled, not rewritten.** The memo's analysis stays
   byte-identical; only the pending signature block is completed with the
   recorded authorization facts (date, signer role, register pointer).
5. **Historical corpus and audit snapshots are immutable.** Documents that
   describe the past as it was known then (`PLANS_210_213_FLAGSHIP_ECONOMY_AUTHORITY_MAP`,
   `UNBLOCKED_PLANS_AUDIT_2026-09-19.md`, Seal-steps program files, corpus
   `C-integration-plans/*`) are never retro-edited; they are cited.

---

# 7. Ownership Matrix

| Concern | Authoritative owner | This package's relationship |
|---|---|---|
| Restock priority arithmetic | `ShelterBarterSystem` (Core) | Unchanged; evidence cited |
| Stock inclusion/quantities (authored manifest) | `MerchantCaravanDef.stock` (code defaults + broker build; JSON mirror) | Unchanged |
| Day-gate authority | Contraband activation map → `ContrabandBrokerCaravan.Build` | Unchanged |
| Per-stay pinned stock | `CaravanRuntimeState.remainingStock` (Core state) | Unchanged |
| Barter persistence | `ShelterBarterSaveState` + `ShelterBarterSaveStore` (section `shelter_barter`) | Unchanged |
| Display order on the trade panel | `ShelterBarterPanel` (read-only consumer) | Unchanged |
| Decision verdict (SIGNED) | `DECISION_REGISTER.md` DEC-05 / foreman | Verdict untouched; evidence field corrected (D3, integrator-applied) |
| Queue-ledger prose | `INTEGRATION_PLANS.md` (integrator-owned under `claim-wave11-part2-execution-2026-09-18`) | Two string replacements (D1, D2), owner-routed |
| Debt ledger | `KNOWN_DEBT.md` (same claim) | Verified no-op (D5) |
| Source memo signature | `docs/plans/wave9_part2/C1_DECISION.md` | Back-fill (D4) — see §20 for the ownership note and routing |
| Focused test contract | `Plan147RestockPriorityTests.cs` | Re-verified; not edited |
| Ratification | Foreman (user) | One wording-only line (D6) |

Ambiguous-ownership flag (recorded, not left vague): `docs/plans/wave9_part2/`
is not listed in any ACTIVE claim's exact paths (the wave 9 part 2 claims are
DONE/handed off). The memo back-fill therefore routes through the integrator
together with the register edit, as one commit, so the two never disagree
in-tree.

---

# 8. Data Flow

Documented as-is (the reconcile changes none of it):

**Arrival/restock flow (daily):**

```
CampaignDayCoordinator (day owners, fixed order)
  └─ Main.CampaignOwners.cs:1310  TickShelterBarterDay(day)
       └─ ShelterBarterSystem.TickDay(day)                       [ShelterBarterSystem.cs ~:520]
            ├─ _state.currentDay = day
            ├─ per catalog caravan (Dictionary ordinal enumeration):
            │    shouldBePresent = (day % period) < stay
            │    arrival edge (false→true):
            │        isAtAirlock = true; daysPresent = 1
            │        RestockCaravan(def, cState)                 [:532]
            │            └─ GetPrioritizedStock(def)             [:319]
            │                 └─ ComputeItemPriorityScore ×N     [:283]  (bp + optional seam)
            │            └─ per item: qty = authored quantity,
            │               or 0 when available_from_day gate unmet
            │            └─ cState.remainingStock[item_id] = qty  (pinned for stay)
            │        OnCaravanArrived → journal line (host)
            │    departure edge (true→false): isAtAirlock=false; OnCaravanDeparted → journal
            └─ OnBarterStateChanged → panel refresh (host)
```

**Trade flow (player-initiated):** `ExecuteTrade` validates caravan known → at
airlock → airlock accessible (thermal gate inert in current host) → request
non-empty → `remainingStock` sufficiency → valuation with basis-point tolerance
→ counterfeit roll (sole RNG consumer, campaign-forked stream) →
`InventoryBill` pre-commit validation → atomic commit → deduct
`remainingStock` → counters/events. Priority order plays no role in trade
legality or pricing — Option C is order-only.

**Render flow:** `OpenShelterBarterPanel` → `Bind(barter, inventory, journal,
itemLookup)` → `RefreshMerchantStockTable` → `GetPrioritizedStock(caravan)`
(`ShelterBarterPanel.cs:680`) → per-row availability from pinned
`remainingStock`, unit price from `price_multiplier_bp`.

**Save flow:** `SaveShelterBarter` (Main.Plans147.cs:305) → `CaptureState`
(JSON-clone) → `ShelterBarterSaveStore.TryCapturePersisted` →
`SchemaVersionedEnvelope` bytes into the campaign aggregate section
`shelter_barter`. Restore runs at `EnsureShelterBarter` after all caravan
registrations, so restore never triggers a restock by itself (EnsureCaravanState
only restocks caravans missing from state; restored caravans are present).

---

# 9. State Model

Persisted (`ShelterBarterSaveState`, JSON envelope):

| Field | Type | Meaning |
|---|---|---|
| `currentDay` | int | last ticked campaign day (drives day-gate evaluation on next arrival) |
| `completedTradesCount` | int | lifetime trade counter |
| `counterfeitsDetectedCount` | int | lifetime counterfeit blocks (appraisal ≥ 2) |
| `caravans` | Dictionary<string, CaravanRuntimeState> (ordinal) | per-caravan `isAtAirlock`, `daysPresent`, `remainingStock` (pinned quantities) |

Derived, **never persisted** (and correctly so):

- Priority scores and stock ordering — pure functions of authored manifest +
  optional scorer. Persisting them would create a second truth that could
  disagree with the manifest after a content update.
- Catalog content — re-registered at boot from code defaults + broker build
  (the broker definition is explicitly "never serialized", per the save-store
  header comment).
- `PriorityScorer` binding — a delegate; not serializable, host-scoped,
  currently unset.

This package adds, removes, and reshapes **zero** state.

---

# 10. API/Contracts

Contracts the reconcile relies on and must not disturb:

1. `ComputeItemPriorityScore(CaravanStockItem, MerchantCaravanDef, Func<string,int>?) → int`
   — static, pure, deterministic; null item → 0; scorer skipped for null/empty
   `item_id`; score = `price_multiplier_bp` + scorer delta.
2. `GetPrioritizedStock(MerchantCaravanDef) → IReadOnlyList<CaravanStockItem>`
   — non-mutating copy; total order (score desc, item_id ordinal asc); empty/
   null stock → empty array; preserves every authored entry and quantity.
3. `PriorityScorer { get; set; }` — optional instance seam; unset ⇒ authored
   multipliers only; assignment takes effect on the next `GetPrioritizedStock`
   call (including the next arrival's restock), never retroactively on pinned
   stock.
4. `RestockCaravan` invariants — clears then repopulates `remainingStock`;
   gated items enter as 0 (key present); runs only at registration-seeding and
   the arrival edge.
5. Pinning invariant — between arrival edge and departure, `remainingStock`
   changes only via successful `ExecuteTrade` deduction.
6. Save contract — `CaptureState`/`RestoreState` deep-clone through the JSON
   serializer; unknown/restored caravans reconcile through `EnsureCaravanState`
   on the next tick.

---

# 11. Data Changes

None. No JSON file is added, edited, renamed, or re-keyed. For completeness:
`merchant_caravans.json` (the data-authority mirror of the four default
caravans) stays exactly as shipped; the reconcile deliberately does not "fix"
the LoadCatalog/mirror arrangement because that is a data-authority design
question outside this package's ledger-truth scope (§5 records the seam for
whoever owns that future decision).

---

# 12. Save/Load

No save-path change is needed, and the plan proves it rather than asserting it:

- The package touches four Markdown files only; the C# surface (including
  `ShelterBarterSaveState`, the store, and the envelope) is untouched, so the
  save format cannot change.
- The live save path is already correct for Option C: pinned
  `remainingStock` is persisted; ordering is recomputed deterministically
  after restore from the same authored inputs, so a save→load cycle reproduces
  byte-identical stock and identical display order without any persisted
  priority data.
- Registration-precedes-restore ordering in `EnsureShelterBarter` guarantees
  restored caravans are not reseeded (no double restock), and gated quantities
  restore as-saved (a gate that opened mid-save stays open; one that hadn't
  opened stays at its pinned 0 until the next arrival — matching the
  "evaluated once per restock" contract).
- No migration, no envelope version bump, no legacy fallback branch.

---

# 13. Determinism

Current guarantees (all preserved by doing nothing to code):

- The priority pipeline contains **zero RNG**: `ComputeItemPriorityScore` and
  the sort are pure; the comparator's tie-break is `StringComparison.Ordinal`.
- The system's only RNG consumer is the counterfeit check in `ExecuteTrade`,
  fed by the campaign-forked `ISeededRng` stream `"shelter_barter"`
  (`Main.Plans147.cs:240`) — replay-stable under the campaign seed.
- `Dictionary` instances use `StringComparer.Ordinal`; enumeration order of
  `_catalog` affects only event emission order, never state content.
- No `System.Random`, no wall-clock seeding, no hash-order dependence anywhere
  in the barter path (grep-verifiable).
- Stay-pinning means UI timing (when a panel is opened) cannot influence
  stock — a determinism property as much as a gameplay one.
- Because the reconcile edits no code, determinism evidence remains the
  existing suite (notably tests 1, 3, 5, 6 in §18) plus the standing
  determinism gates; no new determinism test is owed.

---

# 14. System/Event Wiring

No wiring change. As-is inventory, for the record:

- `OnCaravanArrived` / `OnCaravanDeparted` → journal raw entries
  (`shelter_barter_arrival_{id}` / `shelter_barter_departure_{id}`), wired in
  `EnsureShelterBarter` (`Main.Plans147.cs:261–273`).
- `OnBarterStateChanged` → panel/session refresh consumers.
- Daily tick routed through the campaign owner pipeline at
  `Main.CampaignOwners.cs:1310` (fixed position, after contraband stash,
  before plastic pyrolysis).
- Panel route `shelter_barter` registered in `PanelRegistry.ConfigureActions`
  (`Main.PlayerSurfaces.cs:576`), opened through `Main.GameFlow.cs:642` /
  `Main.ExpandedShelterSystems.cs:583`.
- CLI/selftest surface: `--contraband-stash-selftest` exercises the system
  through `ContrabandStashSelfTest.cs`.

---

# 15. Godot Integration

No Godot-side change. The existing integration is thin and correct: the panel
reads Core state and calls Core queries; it owns no rules (availability clamp
on its own request counters is presentation hygiene, not authority). The
`GetPrioritizedStock` call site (`ShelterBarterPanel.cs:680`) already carries
the harness-safe fallback. No new runtime session is required to verify this
package (a headless Godot check is unnecessary because no runtime path is
touched; TEST_POLICY's 15 FPS rule is therefore not engaged). If the foreman
wants a runtime smoke anyway, the sanctioned read-only check is the existing
`--contraband-stash-selftest`, which already boots the broker path.

---

# 16. Narrative/Content Integration

No content change. The broker ("The Quiet Counter") and the four guild
caravans keep their names, descriptions, factions, and restrained tone. The
reconcile changes only governance prose about the feature, never diegetic
text. Tone rules (restrained, human, fictional; no real-world references) are
trivially preserved.

---

# 17. Failure Modes

## 17.1 If the reconcile is executed incorrectly

| # | Failure | Class | Consequence | Mitigation in this plan |
|---|---|---|---|---|
| F1 | Editing `INTEGRATION_PLANS.md`/`DECISION_REGISTER.md` directly as a builder while `claim-wave11-part2-execution-2026-09-18` is ACTIVE | HIGH (Rule 6 violation) | Races the integrator, invalidates both packages, corrupts the claim map | All ledger edits are owner-routed (§7, §20); the builder's only write is this plan |
| F2 | "Fixing" the register by changing the verdict instead of the evidence count | HIGH | Re-opens a signed decision without authority; contradicts Wave 9 Part 2 authorization | D3 text touches only the evidence field; the verdict column diff must be empty (§24 gate) |
| F3 | Correcting 14/14 to a fresh miscount (e.g. copying "14/14" from a combined battery, or writing 6/6 without re-running) | MEDIUM | The same staleness recurs at the next audit | P0 re-runs the focused suite and greps the `\|Fact\|` count at the claiming HEAD; the drift note cites the commit (`8f72ee62`) so the number is auditable |
| F4 | Fixing the FOLLOWUPS row but missing the status-line parenthetical (L1) | MEDIUM | The batch summary keeps contradicting the row it summarizes — a subtler inconsistency than before | §2.6 enumerates both sites; §24 requires a grep-closure proof over the whole file |
| F5 | Retro-editing historical documents (authority map, audits, Seal-steps, corpus) so they "look consistent" | MEDIUM | Destroys audit history; violates the corpus immutability rule | §6 rule 5; §22 lists them as no-touch |
| F6 | Inventing a `KNOWN_DEBT.md` RETIRED row to "show work" | LOW–MEDIUM | Fabricates debt history; misleads future sweeps | D5 is an explicit recorded no-op with grep evidence |
| F7 | Reconcile wording claims behavior that does not exist (e.g. "scarcity-driven restock live") | HIGH | The ledger becomes false in the *opposite* direction; a future builder "discovers" missing features | D2 wording is mechanically derived from the verified call graph (order-only; seam unset in production) |
| F8 | Back-filling the memo with a different date/signer than the register | LOW | Register↔memo contradiction replaces register↔queue contradiction | D4 cites DEC-05's own routed date (2026-09-17) and the closeout's recorded authorization |
| F9 | Hard-coding today's line numbers into the ledger text | LOW | Line-number rot re-stales the rows | D2 cites symbols (`ComputeItemPriorityScore`, `GetPrioritizedStock`) with line numbers only as parenthetical hints; the durable reference is the symbol name |
| F10 | Re-verification (P0) finds the binding regressed (tests red, call sites gone) | HIGH | Proceeding would ratify a lie | P0 gate: stop; the package converts to a repair package and returns to the foreman (§23) |

## 17.2 If the ledgers are NOT reconciled (the cost of doing nothing)

| # | Failure | Consequence |
|---|---|---|
| N1 | A future agent reads "a priority ordering needs a signed design" and implements a second priority system (Option A/B style) | Duplicate authority against Rule 5; balance damage (slot caps/quantity scaling DEC-05 deliberately rejected); wasted build |
| N2 | A future foreman re-routes DEC-05 for signature | Re-litigates a settled decision; burns a signature slot; signals that SIGNED rows are unstable |
| N3 | The "14/14" count is pattern-matched by a future auditor against a 6-fact file | False "missing tests" alarm; possible wrongful quarantine/re-enable churn under TEST_POLICY |
| N4 | The memo's blank signature block is found by the DEC-08 periodic governance audit | The register's SIGNED row is cited against its own source; governance cadence produces a defect row that this package could have pre-empted |
| N5 | `KNOWN_DEBT.md` absence is rediscovered repeatedly | Every future restock-adjacent sweep re-pays the grep cost this package pays once and records |

---

# 18. Test Strategy

Policy compliance: TEST_POLICY selection rules — run the smallest directly
affected target; no new test is created (no new public contract, state, or
behavior); no full-suite run; the targeted runner's 180 s cap applies.

**Primary (and only required) verification:**

```
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs
```

Expected: 6/6 PASS. Re-run in this planning session 2026-09-19: **6/6 PASS**
(E4).

**Per-case enumeration (what the existing suite already proves):**

| # | Test | Proves |
|---|---|---|
| 1 | `ComputePriorityScore_IsPureAndDeterministic` | Score equals authored `price_multiplier_bp` (11500) with no scorer; repeated calls identical (purity/determinism) |
| 2 | `ComputePriorityScore_WithCustomScorer_AppliesModifier` | Seam composes additively: 10000 + 2500 = 12500 for the matched ID, 0 delta otherwise |
| 3 | `GetPrioritizedStock_OrdersByPriorityDescending_AndTieBreaksByItemIdOrdinal` | Total order: 12000 bp first; the two 10000 bp entries tie-break to `cloth` before `item_fuel` by ordinal comparison; 9000 bp last; all 4 entries present |
| 4 | `RestockCaravan_PreservesAllQuantities_OptionCInvariant` | Option C invariant: after arrival, `remainingStock` equals authored quantities exactly (10/20/30/15) — ordering changed nothing about inclusion or amount |
| 5 | `ReopeningSameDay_DoesNotRerollOrRecomputePinnedStock` | Pinning: a trade deduction persists; re-invoking `GetPrioritizedStock` same-day neither restores nor recomputes pinned stock |
| 6 | `NextArrival_ReevaluatesPriorityWithUpdatedScorer` | Seam lifecycle: scorer attached mid-campaign changes the *next arrival's* evaluation (water 9000+5000=14000 takes first place) while pinned quantities stay authored (30) — arrival-edge-only re-evaluation |

**Optional adjacent regression (only if the foreman wants belt-and-braces; not
required for a docs-only package):** `ShelterBarterSystemPlan54Tests.cs` (6/6 at
the Wave 9 Part 2 closeout), `ContrabandBarterRouteTests.cs` (11/11),
`ShelterBarterPanelRouteTests.cs` (3/3). These were green at the C1 closeout
and are not re-run by default under the focused-testing policy.

**Post-edit consistency gates (docs-side):**

- `python3 scripts/ci/generate-docs-index.py --check` after the integrator
  regenerates the index (the register and INTEGRATION_PLANS are indexed
  documents; the reconcile must not leave the generated index stale).
- Grep-closure sweep (§24) proving no stale string survives anywhere.

---

# 19. Dependency-Ordered Phases

**P0 — Re-verify (read-only; builder or cheap sweep agent).**
Re-run the focused suite; re-grep the binding (`:136`, `:283`, `:319`, `:273`,
`:532`, `ShelterBarterPanel.cs:680`); re-extract the four stale strings at the
claiming HEAD; re-check `KNOWN_DEBT.md` and claim ownership in
`WORKTREE_OWNERSHIP.md`.
*Gate:* every evidence row in §4 reproduces. If P0 fails (binding regressed,
ledgers already fixed, or claim map changed), **stop** — report the delta to
the foreman; do not improvise (Rule 10).

**P1 — Ratify (foreman; wording-only).**
Obtain the D6 ratification line (or foreman-adjusted equivalent).
*Gate:* the line exists and names DEC-05, Option C, and the 6/6 evidence.

**P2 — Reconcile (integrator, under the active claim; one commit).**
Apply D1, D2, D3, D4 exactly; confirm D5 (no KNOWN_DEBT edit); record the
ratification line in the commit message or the D4 block.
*Gate:* `git diff` shows exactly four files (the plan file aside): two strings
in `INTEGRATION_PLANS.md`, one evidence field in `DECISION_REGISTER.md`, one
signature block in `C1_DECISION.md`; the DEC-05 verdict column is untouched;
no production/test/data file appears in the diff.

**P3 — Consistency verification (integrator).**
Run the §24 grep-closure sweep; regenerate and `--check` the docs index;
confirm the register invariant (every row terminal) still holds; confirm the
docs-index gate passes.
*Gate:* zero stale matches; gates green.

**P4 — Handoff & ledger closeout (integrator).**
Mark `CF-P5-RESTOCK-RECONCILE` DONE in `INTEGRATION_PLANS.md`'s active queue
section (per the standing queue convention) with the P0–P3 evidence; hand off
per `AI_AGENT_WORKFLOW.md` (outcome, files, contract, commands/results,
limitations, shared paths intentionally untouched).
*Gate:* handoff contains the exact commands and outputs; the package row names
this plan file.

Dependencies are strictly sequential: P1 blocks P2 (no ratification, no
ledger edit), P2 blocks P3 (nothing to check before the edit exists), P3
blocks P4.

---

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `docs/plans/CF_P5_RESTOCK_RECONCILE_INTEGRATION_PLAN.md` | CREATE (this plan) | Package planning authority | none |
| `INTEGRATION_PLANS.md` | MODIFY — two strings (line 213 parenthetical; line 218 acceptance cell), per D1/D2 | Stale "deferred/authority question" text contradicts DEC-05 + live code | low — integrator-owned path; exact-byte replacements specified |
| `docs/governance/DECISION_REGISTER.md` | MODIFY — one evidence field (line 19, DEC-05), per D3 | 14/14 count never true (born 6/6 in `8f72ee62`); dated correction preserves audit trail | low — verdict column untouched |
| `docs/plans/wave9_part2/C1_DECISION.md` | MODIFY — signature block only, per D4 | Memo is DEC-05's cited source; blank block is an internal contradiction | low — analysis body untouched; routed with the register edit as one commit (§7 ownership note) |
| `KNOWN_DEBT.md` | **NO CHANGE** | Grep-verified: no restock row exists; none is owed (D5) | none |
| `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs` | NO CHANGE | Implementation already live and correct (E1–E3) | — |
| `src/UI/ShelterBarterPanel.cs` | NO CHANGE | Already consumes `GetPrioritizedStock` (E12) | — |
| `src/Main.Plans147.cs`, `src/Host/ShelterBarterSaveStore.cs`, `src/Main.CampaignOwners.cs` | NO CHANGE | Host/save/tick wiring already correct (E11) | — |
| `Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs` | NO CHANGE | 6/6 green; no new contract to test | — |
| `Assets/StreamingAssets/Data/merchant_caravans.json` | NO CHANGE | Mirror arrangement pre-existing and scanner-tracked; out of scope (§11) | — |
| `docs/plans/wave9_part2/WAVE9_PART2_CLOSEOUT.md`, `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`, `docs/foreman/PLANS_210_213_FLAGSHIP_ECONOMY_AUTHORITY_MAP.md`, `Seal-steps/*`, `C-integration-plans/*` | NO CHANGE | Already truthful or immutable history (§6 rule 5) | — |

---

# 21. Risks

| Risk | Likelihood | Impact | Handling |
|---|---|---|---|
| Integrator claim conflict (another wave package edits the same ledger lines first) | low–medium | merge/review churn | P0 re-extracts exact strings at the claiming HEAD; if the lines moved or vanished, adapt the edit or stop |
| Ratification wording bikeshedding | low | schedule only | D6 supplies verbatim default wording; foreman may adjust words, not substance |
| Line-number citation rot in the new ledger text | medium | cosmetic re-staleness | D2/D3 lead with symbol names; line numbers are parenthetical (F9) |
| Audit discovers a *fourth* stale mention after P3 | low | residual inconsistency | P3's grep patterns (§24) cover synonyms: `restock`, `priority ordering`, `authority question`, `14/14`, `PENDING FOREMAN DECISION` |
| P0 finds the implementation regressed since 2026-09-19 | very low | package invalid | §23 conversion path to a repair package |

---

# 22. Out of Scope

- Any restock behavior change: selection caps (DEC-05's rejected Option A),
  quantity scaling (rejected Option B), scarcity-driven scoring in production
  (the unset `PriorityScorer` seam stays unset).
- Any new feature, panel, RNG stream, save section, or save format change.
- Loading `merchant_caravans.json` at runtime / re-pointing the data mirror.
- Editing historical/audit/corpus documents (§6 rule 5) — including the
  Seal-steps program files that describe this very package.
- Creating a `KNOWN_DEBT.md` row (D5 is a recorded no-op).
- New tests, test aggregation, quarantine changes, or any full-suite/soak run.
- Re-deciding or amending DEC-05's verdict or scope.
- Touching the sibling "Follow-up truth" black-market sentence in the same
  INTEGRATION_PLANS cell (already truthful).

---

# 23. Rollback Strategy

- The entire delta is one integrator commit touching four Markdown files.
  Rollback = `git revert` of that commit. No code, data, save, or player-facing
  state exists to roll back; no migration or downgrade path is needed.
- The ratification line is wording-only; retracting it is itself just a ledger
  edit (and would require foreman instruction, since it re-opens DEC-05's
  settled status — an escalation, not a builder action).
- **Contingency path (P0 failure):** if re-verification shows the binding has
  regressed (tests red, call sites removed, seam repurposed), the reconcile
  must not proceed. The package converts to a repair package: freeze the
  ledger edits, restore the "authority question" framing only if the foreman
  formally re-opens DEC-05, and hand the code regression to the economy owner
  with the P0 evidence. Under no circumstance does the reconcile "fix" the
  ledgers to match a regression by downgrading DEC-05 on its own authority.

---

# 24. Definition of Done

All of the following must be observably true:

1. P0 evidence pack reproduced at the claiming HEAD (focused 6/6; call-site
   greps; four stale strings extracted byte-exactly; KNOWN_DEBT and claim-map
   checks logged).
2. The foreman ratification line (D6) is recorded verbatim in the P2 commit.
3. `git diff` for P2 shows exactly: `INTEGRATION_PLANS.md` (D1 + D2),
   `DECISION_REGISTER.md` (D3), `C1_DECISION.md` (D4). DEC-05's verdict column
   diff is empty. No `.cs`, `.json`, or test file is in the diff.
4. Grep-closure sweep returns zero matches for the stale patterns repo-wide
   (excluding immutable history and this plan's own quotations):
   - `Still deferred with authority question` (in the restock context)
   - `merchant-restock priority deferred`
   - `14/14 PASS` (in the DEC-05 context)
   - `PENDING FOREMAN DECISION` (in `wave9_part2/C1_DECISION.md`)
5. `KNOWN_DEBT.md` is byte-identical before/after (D5), and the handoff
   records the grep that justifies it.
6. `python3 scripts/ci/generate-docs-index.py --check` passes after the
   integrator regenerates the index; the register invariant (zero unsigned
   rows without condition) still holds.
7. Focused suite re-run after P2 (paranoia pass, docs-only change):
   `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs`
   still 6/6.
8. `INTEGRATION_PLANS.md`'s active-queue row for `CF-P5-RESTOCK-RECONCILE` is
   marked DONE with the P0–P3 evidence, and the handoff follows
   `AI_AGENT_WORKFLOW.md`.

---

# 25. Implementation Handoff

**Bounded outcome restated:** reconcile four governance texts to the signed,
live truth of merchant restock priority; add one ratification line; change no
code, data, or tests. **Non-goals:** §22. **Current evidence:** §4 (E1–E18).
**Exact edits:** §3 (D1–D6). **Ownership:** ledger paths belong to
`claim-wave11-part2-execution-2026-09-18`; this plan's author claims only this
file.

## MUST PRESERVE

- `DEC-05`'s `SIGNED` verdict and its scope (Option C: order-only; arrival-edge
  restock; stay-pinned stock; no new RNG streams or save sections).
- `ShelterBarterSystem.ComputeItemPriorityScore` purity and the
  `PriorityScorer` seam exactly as shipped (`:136`, `:283`, `:299`, `:319`).
- The stay-pinned / no-reroll invariants proven by tests 4–6.
- `KNOWN_DEBT.md` byte-for-byte (no row exists; none is invented).
- Historical/audit/corpus documents byte-for-byte (authority map, audits,
  Seal-steps programs, `C-integration-plans/*`).
- Unrelated dirty worktree files (the branch currently carries unrelated
  untracked items under `Seal-steps/` and `docs/plans/` — none are this
  package's business).

## MUST ADD

- D1/D2 replacement text in `INTEGRATION_PLANS.md` (both stale sites).
- D3 corrected evidence field with the dated 14→6 correction note in
  `DECISION_REGISTER.md`.
- D4 back-filled signature block in `docs/plans/wave9_part2/C1_DECISION.md`.
- The D6 foreman ratification line, recorded where the reconcile lands.
- P0 evidence pack and P3 grep-closure proof in the handoff.

## MUST NOT DO

- Do not edit any `.cs`, `.json`, or test file — this package reconciles
  truth; it adds no behavior.
- Do not change DEC-05's verdict, re-open the design question, or implement
  Options A/B.
- Do not bind `PriorityScorer` in production "while nearby", and do not wire
  `LoadCatalog` — both are future decisions with their own gates.
- Do not edit ledgers as a builder outside the owning claim (Rule 6); route
  through the integrator or obtain a user-authorized transfer.
- Do not invent a KNOWN_DEBT row, do not retro-edit history, do not run the
  full suite, do not hard-code line numbers as durable references.

## VERIFY WITH

- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs`
  (expect 6/6; re-verified 2026-09-19 during planning).
- `grep -n "RestockCaravan\|GetPrioritizedStock\|ComputeItemPriorityScore\|PriorityScorer" Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs`
  (expect `:136`, `:273`, `:283`, `:299`, `:319`, `:532`).
- `grep -rn "PriorityScorer" src/` (expect zero hits — seam unset in production).
- `grep -n "14/14 PASS\|Still deferred with authority question\|merchant-restock priority deferred\|PENDING FOREMAN DECISION" INTEGRATION_PLANS.md docs/governance/DECISION_REGISTER.md docs/plans/wave9_part2/C1_DECISION.md`
  (before P2: the four stale sites; after P2: zero matches).
- `grep -in "restock\|Plan 147\|Plan147" KNOWN_DEBT.md` (expect no restock row;
  D5 confirmation).
- `python3 scripts/ci/generate-docs-index.py --check` (after integrator regen).

## FIRST SAFE IMPLEMENTATION STEP

**P0 (read-only, one command + one grep):**

```
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs
grep -n "RestockCaravan" Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs
```

If both reproduce §4's evidence (6/6; call sites at `:273` and `:532`), proceed
to P1 (request the foreman ratification line). If either fails, stop and report
the delta — the reconcile premise has changed and the package returns to the
foreman rather than improvising (Rule 10).
