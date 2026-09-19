# ASHFALL — Support Triage Kit

> **Authority:** Plan 48 / C2[21] Release Craft
> **Status:** ACTIVE
> **Last Updated:** 2026-09-19

---

## Purpose

This document helps triage player-reported issues: classify them, determine the
correct response route, and decide whether a hotfix is warranted.

---

## Classification Flow

```
Player reports issue
        │
        ▼
Is data (saves) corrupted or lost?
        │
    YES │                  NO
        ▼                  ▼
QUARANTINE      Can it be reproduced on the current release tag?
(see HOTFIX.md §Emergency)
                    │
                YES │           NO
                    ▼           ▼
            Is it a crash   Needs more info
            or game-breaking
            blocker?
                    │
                YES │       NO
                    ▼       ▼
            Hotfix route  Next regular release
            (see HOTFIX.md)
```

---

## Severity Levels

| Level | Description | Target Response | Route |
|---|---|---|---|
| **S1 — Critical** | Save corruption, data loss, crash on launch | Hotfix | `hotfix/*` immediately |
| **S2 — High** | Crash mid-game, major feature broken | Hotfix or fast-track PR | Evaluate in 24h |
| **S3 — Medium** | Feature partially broken, workaround exists | Regular PR | Next sprint |
| **S4 — Low** | Visual glitch, minor UX issue | Regular PR | Backlog |
| **S5 — Enhancement** | New feature request | Plan/backlog | Standard planning |

---

## Save Issues Triage

If a player reports a broken save:

1. Ask for the save file version: `version` field in `holdfast_save.json`
2. Check which game version they are on
3. Look up the codec versions in `artifacts/golden_saves/historical/`
4. If the save version is from a supported range → migration bug (S1/S2)
5. If the save version is from an unsupported/ancient version → document, inform player

### Supported save versions (as of v1.1.0)

| Codec | Current | Minimum supported |
|---|---|---|
| `holdfast` | v5 | v4 (migration: v4→v5 proven in DeepCoastHeadlessDemo) |
| `year_of_ash` | v5 | v4 |
| `dose_ledger` | v2 | v1 |
| `expansion_hub` | v6 | v5 |
| `expansion_quest` | v1 | v1 |
| `weight_of_choices` | v2 | v1 |

---

## Diagnostic Commands

```bash
# Verify version sources agree
python3 scripts/ci/version-gate.py

# Data integrity check
godot --headless --path . -- --data-integrity-selftest

# Save support window
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SaveSupportWindowTests --nologo

# Fast CI gates
bash scripts/ci/verify-fast.sh
```

---

## Escalation

| Situation | Action |
|---|---|
| Save corruption confirmed | Open `[QUARANTINE]` issue, see HOTFIX.md §Emergency |
| Save corruption suspected | Collect save files, run data integrity tests before acting |
| Crash reproducible | Open issue with reproduction steps, classify severity |
| Cannot reproduce | Request additional info (OS, GPU, save file) |

---

## Related Documents

- [HOTFIX.md](HOTFIX.md) — hotfix classification and iron rule
- [VERSIONING.md](VERSIONING.md) — version policy and support window
- [POSTMORTEM_TEMPLATE.md](POSTMORTEM_TEMPLATE.md) — post-incident template
- `docs/testing/FIXTURE_POLICY.md` §7 — historical corpus
