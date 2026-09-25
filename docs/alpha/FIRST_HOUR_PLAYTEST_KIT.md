# ASHFALL — First-Hour Alpha Playtest Kit

Goal: one real first-hour session, played to prove **keyboard-only** control, motion
comfort, and the 7-stage onboarding flow — then turn the session into a funnel report and
at most three fixes.

## 1. Launch

| Mode | Command | Notes |
|---|---|---|
| Editor run (fastest feedback) | `godot --path .` | uses the checkout data directly |
| Exported alpha | `builds/linux/run-ashfall.sh` | PCK-only also works: just `./ashfall.x86_64` |
| Keyboard-only leg | same, but put the mouse aside before pressing Start | see §3 |

Before starting a clean capture (optional): delete the sink file for a fresh session —
`rm -f "$HOME/.local/share/godot/app_userdata/ASHFALL- Atomic War - Starving Survival/play_metrics.jsonl"`
The game recreates it on the next event.

## 2. The first hour (what to check)

| # | Stage | What to do | Pass looks like |
|---|---|---|---|
| 1 | Water | Follow the hint; start one treatment batch | hint points at the right panel; batch visibly starts |
| 2 | Power | Throw one shelter breaker | grid state changes; hint advances |
| 3 | Food | Consume one ration from inventory | ration count drops; roster fed |
| 4 | Duty | Assign one survivor a shift | duty roster accepts it; work begins |
| 5 | Dose | Open the dose ledger / radiation detail | dose value readable; hint advances |
| 6 | Research | Start one available node | node queued; days remain visible |
| 7 | Expedition | Dispatch one expedition | dispatch confirmed; party leaves the hatch |

Also watch for (report anything odd):

* **Motion comfort**: panes fade+rise in ~0.14 s and fade out ~0.10 s; button hover lift / press
  kick are subtle. Toggle **Settings → ReducedMotion** — all motion must stop immediately
  (that setting is the single authority for both UI and audio stingers).
* **Keyboard-only leg (§3)**: no mouse clicks from Start to the end of stage 7.
* **Truthfulness**: no empty panes, no buttons that do nothing, no stale numbers after a day
  advance while the pane stays open.
* **Weather lesson**: on a severe-weather day, open weather — the storm-prep lesson should
  appear once, not every time.

## 3. Keyboard-only leg

`Tab` / `Shift+Tab` and arrows move focus, `Enter` / `Space` activate, `Esc` closes. On every
opened panel the first `Tab` must land **inside** the panel (initial focus is granted on open).
If focus escapes behind a panel or a control cannot be reached, record the panel name and the
control — that is a defect against the focusability gate (currently 561 interactive controls,
0 unreachable in the automated audit).

## 4. Capture the funnel

```bash
cd "<repo root>"
python3 scripts/tools/first_hour_funnel.py --discover --out docs/telemetry/FIRST_HOUR_FUNNEL.md
```

The sink (`user://play_metrics.jsonl`) records every session automatically; `--discover` finds
it. The report shows each session's 7 live steps, the canonical 13-step funnel, max day and top
actions.

## 5. Triage (≤3 fixes)

Copy this block into a new note and fill it from the report + your notes:

```
Session date:
Reached stage: __/7   Max day: __
First drop-off:
   step:            (from the funnel table)
   what happened:   (hint missing / panel confusing / control unreachable / other)
Fix 1 (system owner):
Fix 2 (system owner):
Fix 3 (system owner):
Notes (comfort / tone / keyboard):
```

Rules: a fix must extend an existing owner (Core system, host session, panel) — no new
authority. If a fix needs a save-schema or determinism decision, record the blocker instead of
improvising.

## 6. Automations already in place

* `scripts/tools/first_hour_funnel.py` — funnel report from live JSONL (this kit, §4).
* `scripts/tools/ashfall-package-verify.sh` — package gates before handing the build out.
* `docs/ui/KEYBOARD_FIRST_HOUR_WALKTHROUGH.md` — automated focusability/clickability evidence.
* `docs/telemetry/FIRST_HOUR_FUNNEL.md` — latest generated report.
