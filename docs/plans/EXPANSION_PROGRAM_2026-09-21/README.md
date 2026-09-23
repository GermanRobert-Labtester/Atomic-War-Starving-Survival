# ASHFALL Expansion & Integration Program — 2026-09-21

Six proposed plans produced from a read-only audit of HEAD `5be1a30a`
(2026-09-21). **None of these is a claim.** The foreman owns
`INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md`; no production file was
modified by the audit.

## The finding that organises the programme

The Core domain has outgrown its host wiring. A reachability analysis
(not grep-only; the algorithm is in `EVIDENCE.md`) finds **99 Core authority
files unreachable from the Godot host**, plus **5 fully dead authorities**.
The ledger records several of these as "integrated and sealed" because the
UNBLOCK waves delivered Core + tests without a host path. The mechanism that
allowed it: `SubsystemManifest` declares 18 subsystems while the host has 226
`Setup*` methods; nothing measured reachability; and plan numbers were reused
across corpora (`src/Main.Plans157.cs` wires Grain Milling, not the ledger's
Plan 157 `CommunicationsSystem`).

## The six plans

| # | Plan | Purpose | Primary evidence |
|---|---|---|---|
| 01 | [`PLAN-ORPHAN-SEAL-01.md`](PLAN-ORPHAN-SEAL-01.md) | Seal the 99 orphans + triage the 5 dead authorities in 8 bounded waves; define "integrated" | reachability audit |
| 02 | [`PLAN-INTEGRATION-KIT-02.md`](PLAN-INTEGRATION-KIT-02.md) | Build the scaffolding that prevents recurrence: reachability gate, complete manifest, `--integration-selftest`, scaffolds, ledger-truth checks | manifest = 18 vs 226 setup methods |
| 03 | [`PLAN-UNBLOCK-03.md`](PLAN-UNBLOCK-03.md) | Unblock the corpus: the register is terminal except DEC-11/13; execute signed-but-host-pending dispositions; census tranche machine; Plan 37/48/53 heads | DECISION_REGISTER 301 rows; §2.2 host-pending table |
| 04 | [`PLAN-VERTICAL-CULTURE-04.md`](PLAN-VERTICAL-CULTURE-04.md) | New mechanics: Almanac, Memory Room, Walls, Broadsheet, Voices/Cassettes, Post, Confessionals, Identity, Legacy | culture/voice/narrative orphan cohort |
| 05 | [`PLAN-VERTICAL-BODY-INDUSTRY-05.md`](PLAN-VERTICAL-BODY-INDUSTRY-05.md) | New mechanics: Clinic Continuum, Table & Water, Shelter Engine (Power Board), Market Depth, Polity & Distance | medical/economy/shelter cohort + signed DEC-21..44 |
| 06 | [`PLAN-LAUNCH-FACE-06.md`](PLAN-LAUNCH-FACE-06.md) | Input/focus/controller (Plan 37), release craft (Plan 48), E1/53, surface coverage, store truth, final gates | Plan 37/48 source docs + surface gates |

[`EVIDENCE.md`](EVIDENCE.md) holds the audit method, the full orphan inventory,
the collision evidence, and re-runnable commands.

## Sequencing

```
KIT (02) ──▶ ORPHAN (01) ──▶ CULTURE (04) ─┐
                          └▶ BODY/IND (05) ─┼─▶ FACE (06)
UNBLOCK (03) ──────────────────────────────┘
```

Plans 01–03 are gap-sealing. Plans 04–05 are the expansion/scaffolding waves
that make the sealed systems worth playing. Plan 06 is the player surface,
release, and truth closure.

## Rules these plans obey

- Core stays engine-free; host adapters own presentation/persistence.
- One authority per concern; every package cites the owner it extends.
- JSON data is authority; every new row needs a live consumer.
- Determinism via seeded streams; no `System.Random`/`GetHashCode`.
- Focused verification per `TEST_POLICY.md`; no full-suite defaults.
- No Unity restoration; no real-world wars/countries/art/copied text.
- Claims are filed by the foreman; a plan is not permission to edit.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).


## Wave 20 — claim-readiness closure

The finalisation pass left three apparent residual gaps. Re-verification found
only one real (three Wave 1 plans lack package IDs); the other two were
readiness-detector false negatives. Wave 20 closes all of it and installs the
auditor that prevents recurrence:
[`../EXPANSION_PROGRAM_WAVE20_2026-09-21/`](../EXPANSION_PROGRAM_WAVE20_2026-09-21/).
