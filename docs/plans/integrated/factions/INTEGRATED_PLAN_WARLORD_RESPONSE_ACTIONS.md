# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
> **Not committed** (per user directive). See §6 for the closeout evidence.

---

# PLAN-WARLORD-RESPONSE-ACTIONS — Idempotent Warlord Tribute Response Host Integration

> **STATUS: APPROVED BY USER**
> **Package:** `WARLORD-RESPONSE-ACTIONS`
> **Category:** factions / year-of-ash
> **Plan type:** host integration of an unhosted Core command surface. Closes a live double-settlement hole.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Warlords/WarlordResponseActions.cs` to the live
`WarlordDoctrineSystem` so tribute responses (Pay / Contest / Submit) are atomic
and idempotent. Today `src/Main.YearOfAsh.cs` calls
`YearOfAshHostSession.SettleWarlordTribute` directly with **no responded-tribute
guard**, so the PAY TRIBUTE button can settle the same week's ask repeatedly.

**Bounded outcome:** one response per canonical tribute id; the tribute currency
is consumed exactly once; the response ledger persists; superseded tribute ids
stop blocking new asks.

**Non-goals:** no new warlord doctrine authority, no second tribute ledger, no
new collector-voice text, no change to `SettleTribute`'s escalation math.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan | `grep -rn "WarlordResponseActions" src/` → no matches |
| Live owner | `WarlordDoctrineSystem` held by `src/YearOfAsh/YearOfAshHostSession.cs` (`Warlord` property, `BindWarlord`) |
| Live command path has no guard | `src/Main.YearOfAsh.cs:52` `PayWarlordTribute` → `inventory.RemoveItem` + `SettleWarlordTribute`, no responded check |
| Second entry point | `src/Main.YearOfAsh.cs:73` `RefuseWarlordTribute` → `SettleWarlordTribute(0, ...)`, no guard |
| Panel surface | `src/UI/FactionsPanel.cs:301` `PAY TRIBUTE`, `:304` `REFUSE THIS WEEK`; no Contest/Submit |
| Cadence producer | `WarlordDoctrineSystem.OnTributeDemanded(amount, itemId, day)`, `totalWeeksAsked` |
| Idempotence exists but unused | `WarlordResponseActions.IsResponded(tributeId)` + `Pay`/`Contest`/`Submit` + `CaptureState`/`RestoreState` |

Canonical tribute id derived from the live owner (no parallel ledger):
`tribute_week_{WarlordDoctrineSystemState.totalWeeksAsked}`.

## 3. Files

### New — Host
- `src/Host/WarlordResponseHostSession.cs` (+ `WarlordResponseSaveStore`)
- `src/Host/HostCli.WarlordResponse.cs`
- `src/Main.WarlordResponse.cs`

### Modified — Core
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `warlord_response` section
- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` — `warlord_response_ticked`
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--warlord-response-selftest`

### Modified — Host
- `src/YearOfAsh/YearOfAshHostSession.cs` (hold the response surface, expose it)
- `src/Main.YearOfAsh.cs` (route `PayWarlordTribute` / `RefuseWarlordTribute` through it)
- `src/UI/FactionsPanel.cs` (additive: CONTEST / SUBMIT buttons + responded state)
- `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `src/Main.CampaignOwners.cs` (phase-5 day owner: prune superseded tribute ids)
- `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`

### Modified — Tests
- `Ashfall.Core.Tests/Factions/PlanWarlordResponseHostIntegrationTests.cs` (new)
- `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` (section pin)
- `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` (1 row)

## 4. Acceptance

1. `--warlord-response-selftest` ≥ 10/10 headless: double-pay refusal, double-contest
   refusal, contest/submit marking, save/restore, superseded-id pruning,
   currency consumed exactly once, shortage path untouched, no-mutation-on-failure.
2. Focused xUnit suite green; the live `PayWarlordTribute` path settles at most once
   per tribute id even under repeated invocations in the same day.
3. `SettleTribute` remains the sole authority for ask escalation and reliability;
   this surface only guards which responses have been issued.
4. Adjacent gates green (same set as the railway package).

## 5. Deferred with named reasons

- No collector voice for Contest/Submit: `CollectorLine` handles paid/short/refused;
   new authored lines are content work, not wiring.
- No standing/consequence model for contesting: `WarlordDoctrineSystem` owns
   standing; a contest penalty would be new authority.

## 6. Closeout evidence (2026-09-26)

**Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
**Branch:** `integration/all-latest-2026-09-24` · **Not committed** (user directive).

| Item | Evidence |
| --- | --- |
| Headless probe | `--warlord-response-selftest` **11/11** (registered in the Core + host CLI registries, the selftest manifest, and the CLI command catalog) |
| Focused xUnit | `PlanWarlordResponseHostIntegrationTests` **8** |
| Host build | `dotnet build Ashfall.csproj` — 0 errors |
| Core test build | `dotnet build Ashfall.Core.Tests` — 0 errors |
| Save section pin | `ComprehensiveSaveStoreCorruptionAndMigrationTests` **308** sections |
| Adjacent gates green | `SaveSectionRegistryTests` 5/5, `PersistentFilenameRegistry`, `DayEventVocabulary`, `DayEventParitySourceGate`, `HostCliActionParityGate`, `MainTriadDriftGate.SetupWithoutSave`, `SaveStoreMatrixGate`, `PortContractGate` |
| Regenerated artifacts | selftest manifest **273** (271 headless) · CLI catalog **333 entries / 539 flags** · save-store matrix **310 stores** · port contract **307 seams** |

### Premise audit (Rule 7) — rejected candidates with current evidence

| Candidate | Why rejected |
| --- | --- |
| `DraisineRerailingSystem` | **Already fully hosted** by the Plan 130–133 lane (`src/Host/Plans130To133HostSessions.cs` `DraisineRerailingHostSession`, `src/Main.Plans130_133.cs`, `src/UI/Plans130To133Panel.cs`, `DraisineRerailingSaveStore`, and the `draisine_recovery` registry row). Integrating it would have created a duplicate host and a duplicate save section; the partial work was reverted in full. |
| `BallisticsSystem` | The live `TacticalCombatSystem` + `BallisticsWorkbenchSystem` own shot resolution. |
| `EncounterChoiceEffectDispatcher` | The live `ExpeditionHostSession` already applies `SetWorldFlagId` to the flag ledger. |
| `RadioTuner` | Would be a second tuning authority over `RadioHostSession.CurrentFrequency`. |
| `FoodTypeSystem` | `FoodPreservationSystem` is the signed spoilage truth and `food_preservation.json` already carries the per-item food-type mapping. |
| `RestockAllocationEngine` / `ProstheticConditionWearEngine` | F13 / F14 — decision-blocked (and concurrently in flight). |
| `GarmentLayeringThermalEngine` | Concurrent Plan 142 lane. |

### Deferred with named reasons

See §5 above; every deferral names the owner whose signature is required.
