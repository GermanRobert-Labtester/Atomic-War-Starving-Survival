---
name: ashfall-equipment-balance
description: Runs deterministic, scenario-based balance analysis for ASHFALL equipment condition, degradation, repair, protection, and replacement loops. Use for gas masks, hazmat gear, weapons, tools, or other degrading equipment; delegates general sweeps to ashfall-balance-sim.
---

# ASHFALL Equipment Degradation Analyst

## Role

Measure whether degradation creates meaningful decisions without creating a
softlock. Keep the scope to equipment condition and its direct resource loops.

## Workflow

1. Identify the actual condition systems, item definitions, repair recipes,
   protection effects, replacement sources, and tick/use events.
2. List data knobs and hardcoded rules with their owners. Never treat a host
   duplicate as authoritative.
3. Define deterministic scenarios: light, ordinary, heavy, failed-repair,
   no-replacement, and protection-critical use. State seed, duration, starting
   inventory, and assumptions.
4. Measure durability trajectory, protection loss, repair material demand,
   failure timing, replacement availability, and recovery opportunities.
5. Check invariants: no negative durability/resource quantities, no accidental
   permanent loss of required protection, and no strategy that dominates solely
   because another path is unreachable.
6. Report evidence-backed tuning proposals against JSON authority. Do not edit
   balance data from this skill; use `ashfall-balance-sim` for broader coupled
   economy/survival sweeps. Hand approved data changes to `ashfall-data-add` or
   `ashfall-data-schema`, and approved code changes to `ashfall-implement`.

## Rules

- Use `ISeededRng` and fixed seeds only.
- Distinguish intended scarcity from a softlock and label assumptions.
- Do not infer player behavior from one run; use stated scenarios and checkpoints.
- Read-only by default: do not edit Core, host code, JSON, tests, or scenes.
- Verification remains `dotnet` plus `godot --headless`.

## Output

Return the system map with file/line evidence, scenario manifest, metrics, failure thresholds,
softlock/degenerate-strategy findings, and proposed knob changes with evidence.
If a report is requested, use `docs/balance/EQUIPMENT_<scope>.md`.

## Quality gate

- Every conclusion includes seed, scenario, and checkpoint data.
- Proposed values are recommendations, not silent data edits.
- Any missing replacement or repair path is shown as a confirmed or unverified
  finding rather than guessed.
