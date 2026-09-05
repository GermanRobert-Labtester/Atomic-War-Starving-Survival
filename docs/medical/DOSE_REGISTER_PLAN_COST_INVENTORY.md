# DOSE REGISTER PLAN COST INVENTORY

> Plan 90 / Task 90H. What the `cost` field can actually hold, what items exist,
> and what a follow-on (unblocking) plan may safely reference.

---

## 90H.1 — Cost grammar (verified)

`DosePlanDef.cost` is a **freeform string** (`DoseRegistersCatalog.cs:26`). It
is deserialized, round-tripped through saves of nothing (plans themselves are
not saved), and rendered by **nothing**. Zero lines of code read `cost`.

Therefore the "grammar" is pure convention set by the shipped file:

| Value | In shipped data | Status |
|---|---|---|
| `"morphine"` | `plan_morphine_tray` | **Dead reference** — no `morphine` item in `items.json` |
| `"time"` | `plan_comfort_rounds` | Sentinel, in active use |
| `"none"` | `plan_nothing` | Sentinel, in active use |

- **Quantity:** unsupported. One string, one token. A plan cannot express
  `["clean_water", "dried_rations"]` and no numeric quantity field exists.
- **Sentinels:** `time` and `none` are the established literals; nothing
  enforces them, so any follow-on plan should reuse exactly these.

## 90H.2 — Candidate items verified in `items.json`

Real IDs only (exact-ID grep, line numbers from `items.json`). Every ID below
resolves today; none is a forward reference.

| Role in a future care plan | Resolving item IDs |
|---|---|
| Clean water / fluids | `clean_water` (:186), `clean_water_jug` (:2023), `water_purification_tablets` (:2783) |
| Rations / feeding | `dried_rations` (:2103), `military_rations` (:2423) |
| Generic medical supplies | `medical_kit` (:386), `bandage` (:306), `sterilised_bandage` (:4085), `medical_scissors` (:4065) |
| Symptom control / analgesia | `painkillers` (:6361), `item_medical_saline_salt` (:4993) |
| Anti-rad / decorporation (existing abstraction) | `anti_rad` (:86) — description explicitly frames it as post-exposure dose deduction already implemented by the radiation runtime (`BookReading` `antiRadBefore/antiRadAfter` mechanics, `DoseLedgerSystem.cs:139-143`) |
| Iodine (thyroid) | `iodine_pills` (:66), `iodine_tablets` (:2223) — items exist, but see guardrail §3 |
| Fuel (transport) | `fuel` (:246), `fuel_1l` (:2163), `diesel_fuel` (:2085), `fuel_canister` (:3143) |
| Diagnostics | `calibration_kit` (:426) |

**No `morphine` item exists.** The closest shipped analgesia item is
`painkillers`.

## 90H.3 — Conditional-plan feasibility verdicts

| Proposed plan | Runtime support? | Verdict |
|---|---|---|
| Iodine prophylaxis | Items exist, but **no radioactive-iodine exposure model, no applicability predicate on plans, no timing window** anywhere in Core. A universal KI plan would violate guardrail 1.2. | **REJECT** (documented substitution below) |
| Generic chelation | No item, no radionuclide model. However `anti_rad` **is** an existing, runtime-honored decorporation abstraction (it halves/pre-scales booked dose in `BookReading`). | **REJECT `plan_chelation` as new data** — the existing `anti_rad` mechanics already own this role |
| Decontamination | No external-contamination state is modeled (`DoseEntry` has no contamination field). | **REJECT** |
| Controlled isolation | Nothing models contagiousness of irradiated patients; guardrail 1.4 forbids implying it. | **REJECT** |
| Medical transfer | No facility-transfer runtime of any kind exists in Core or host. | **REJECT** — dead data |
| Observation / rest / fluids / supportive care | All are *ledger inscriptions* under the current model (plans are strings; nothing is consumed), which is exactly what the register honestly is. Costs remain display vocabulary. | **SAFE** as data, in the follow-on pass |

## 90H.4 — Recommended final 8-plan slate (for the unblocking follow-up)

First three preserved verbatim. Five additions, costs drawn only from §90H.2
plus the two established sentinels:

| # | ID | Label | Cost | Note intent |
|---|---|---|---|---|
| 1 | `plan_morphine_tray` | Morphine tray | `painkillers` *(corrected from dead `morphine` — or left as-is if the misspelled-by-absence reference is deemed period-flavor; decide in follow-up)* | preserved |
| 2 | `plan_comfort_rounds` | Comfort rounds | `time` | preserved |
| 3 | `plan_nothing` | Nothing | `none` | preserved |
| 4 | `plan_observation` | Observation | `time` | remove from exposure duty; repeat the reading after rest |
| 5 | `plan_bed_rest` | Bed rest | `time` | off duty, warm, fed, available for symptom checks |
| 6 | `plan_fluids` | Fluids & rations | `clean_water` | reserve clean fluids while watching for worsening |
| 7 | `plan_supportive_care` | Supportive care | `medical_kit` | scarce supplies for symptom control and repeated examination |
| 8 | `plan_symptom Relief` → **`plan_painkillers`** | Pain control | `painkillers` | scheduled analgesia short of the morphine tray |

No KI, no chelation, no decontamination, no isolation, no transfer — per §90H.3.

> **Note:** this slate is a **design artifact for the follow-up plan only**.
> It was **not** applied to `dose_registers.json` in Plan 90 (mode: BLOCKED —
> see closeout). Applying it requires the Core changes listed in the closeout.
