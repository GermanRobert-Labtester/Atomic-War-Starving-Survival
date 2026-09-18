# ASHFALL Standing Product Proof: Seven-Day Slice (Task D1)

**Package:** Wave 10 Part 2 — Task D1\
**Plan Authority:** `Seal-steps/5573522_ASHFALL_WAVE10_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md` §7\
**Status:** **SEALED / VERIFIED-GREEN**\
**Date:** 2026-09-17\
**Integrator:** Antigravity\

---

## 1. Objective & Scope

The Seven-Day Slice is ASHFALL's standing product-level proof of end-to-end simulation cohesion. Rather than testing subsystems in isolation, this slice verifies that the core survival loop functions as a coherent, playable, and deterministic whole across seven continuous campaign days:
1. **Scripted Deterministic Half:** Automated headless execution over fixed seed `9001`, asserting day-by-day oracles, weather mutation, needs decay, map progression, and mid-run save/reload state preservation.
2. **Human Playthrough Half:** Interactive manual checklist covering UX friction, game feel, audio legibility, panel transitions, and edge cases.
3. **Standing Release Integration:** Codified in the release battery and selftest manifests.

---

## 2. Scripted Slice Contract & Execution Oracle

- **Command:** `godot --headless --path . -- --7-day-smoke-selftest`
- **Primary Harness:** `src/Host/SevenDayDeterministicSmokeTest.cs`
- **Fixed Seed:** `9001`
- **Duration:** 7 days (168 simulation hours)
- **Active Subsystems:** `WastelandMapSystem`, `WeatherSystem`, `NeedsSystem`, `SaveStore` architecture

### Verification Gates & Oracle Results (Run at HEAD: 2026-09-17)

| Gate | Target System | Contract / Expected Oracle | Observed Execution Result | Verdict |
|---|---|---|---|---|
| **Gate 1** | Simulation Lifecyle | Baseline 7-day run completes without exceptions or leaks | Clean execution; 0 uncaught exceptions | **PASS** |
| **Gate 2** | Weather PRNG Determinism | Weather roll count identical across two independent runs (`rc1 == rc2 > 0`) | Run 1: 29 rolls; Run 2: 29 rolls (29 == 29) | **PASS** |
| **Gate 3** | Weather Simulation Liveness | Simulation progresses through dynamic weather states | 25 weather transitions observed; final kind: `Clear` | **PASS** |
| **Gate 4** | Needs & Health Decay | 7 days without food/water produces critical hunger (≥90), critical thirst (≥90), and degraded health (<100) | Hunger: 100.0, Thirst: 100.0, Warmth: 16.0, Health: 19.1 | **PASS** |
| **Gate 5** | Map Discovery Events | Starting home node and far node discovered over campaign progression | `loc_smoke_home` and `loc_smoke_far` discovered | **PASS** |
| **Gate 6** | Map Lock / Completion | Locked nodes remain locked; completed nodes marked completed | `loc_smoke_locked` locked; `loc_smoke_far` completed | **PASS** |
| **Gate 7** | Mid-Day-4 Save/Reload Parity | State saved at day 4, reloaded in fresh session, continued to day 7 matches uninterrupted baseline | Resumed roll count: 29 (baseline: 29); final weather: `Clear` | **PASS** |
| **Gate 8** | Survivor Needs Round-Trip | Needs values round-trip through save/reload with delta < 0.01 | Hunger delta: 0.0000; Thirst delta: 0.0000; Health delta: 0.0000 | **PASS** |
| **Gate 9** | Map State Round-Trip | Discovered, locked, and completed node sets preserve exact membership across save/reload | 2 discovered, 1 locked, 1 completed preserved | **PASS** |
| **Gate 10** | Weather Round-Trip | Weather roll count and active weather kind preserve exact state across save/reload | Rolls: 29 == 29; Kind: `Clear` == `Clear` | **PASS** |

**Summary Result:** 10/10 gates passed cleanly in 1.1s.

---

## 3. Human Playthrough Half Integration

The human playthrough half is maintained and tracked in:
`docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`

### Core Journey Structure
- **Step 1–2:** Launch, main menu, clean new game bootstrap.
- **Step 3–7:** Core shelter operations (shelter overview, crew needs status, medical triage, inventory inspection, crafting execution).
- **Step 8–10:** External environment & legibility (weather station & radiation attenuation, radio tuner frequencies, daily journal entries).
- **Step 11–12:** Wasteland interaction (expedition dispatch/sorties, research atlas disciplines).
- **Step 13:** Advance Day transition to Day 2 and persistence verification.
- **Edge-Case Tables:** A (Rapid clicks / cancel), B (New game overwrite isolation), C (Multi-day save/reload), D (Map unlock integrity), E (Audio/settings corruption recovery).

---

## 4. Release Cadence Integration

The seven-day slice is integrated into:
1. `scripts/ci/agent-fast-verify.py` (Gate 24)
2. `docs/ci/SELFTEST_MANIFEST.json`
3. `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`
4. Release captain pre-flight gate.

Task D1 is **SEALED**.
