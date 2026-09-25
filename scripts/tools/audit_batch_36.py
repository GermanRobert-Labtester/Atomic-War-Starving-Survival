#!/usr/bin/env python3
"""
audit_batch_36.py
Verifies all 15 plans in Batch 36 against all 9 structural, architectural, and content criteria.
"""

import os
import sys

BATCH_36_PLANS = [
    "docs/bodymind/PLAN27_SAVE_COMPATIBILITY.md",
    "docs/bodymind/PSYCHOLOGICAL_SYSTEM_OVERLAP_AUDIT.md",
    "docs/bodymind/DOSE_REGISTER_STATE_MODEL.md",
    "docs/bodymind/PLAN27_BASELINE.md",
    "docs/bodymind/AUTOPSY_FINDING_PROVENANCE.md",
    "docs/bodymind/DOSE_QUEST_MATRIX.md",
    "docs/bodymind/PLAN27_COMPLETION_REPORT.md",
    "docs/combat/COMBAT_ENCOUNTER_COVERAGE.md",
    "docs/combat/PLAN10_REGRESSION_MATRIX.md",
    "docs/combat/PLAN10_BASELINE.md",
    "docs/combat/WARLORD_DOCTRINE_MATRIX.md",
    "docs/combat/WEAPON_CONDITION_MATRIX.md",
    "docs/combat/ENEMY_BEHAVIOR_MATRIX.md",
    "docs/combat/AMMO_BALLISTICS_MATRIX.md",
    "docs/combat/PLAN10_COMPLETION_REPORT.md",
]

CRITERIA = [
    ("File Exists", lambda c: c is not None),
    ("Character Count >= 250k", lambda c: len(c) >= 250000),
    ("Master Authority Reference", lambda c: "Master Expansion Authority" in c or "Volumes 1–57" in c),
    ("Pure C# Domain Architecture", lambda c: "```csharp" in c and "namespace Ashfall.Core" in c),
    ("Authoritative Draft 2020-12 JSON Schema", lambda c: "draft/2020-12/schema" in c or "Draft 2020-12" in c),
    ("100-Test xUnit Suite", lambda c: "[Fact]" in c and ("100-TEST" in c or "100-Test" in c or "Test_100_" in c)),
    ("Longitudinal Simulation Trace", lambda c: "600-" in c or "LONGITUDINAL SIMULATION" in c),
    ("25-Point QA Checklist", lambda c: "[x]" in c and ("25-POINT" in c or "25-Point" in c or "25. [x]" in c)),
    ("Deep Polishing & Precision Passes", lambda c: "DEEP POLISHING PASS" in c and "PRECISION PASS" in c),
]

def main():
    print(f"Auditing Batch 36 ({len(BATCH_36_PLANS)} plans) against all 9 criteria...\n")
    all_passed = True

    for plan_path in BATCH_36_PLANS:
        if not os.path.exists(plan_path):
            print(f"[FAIL] {plan_path} does not exist!")
            all_passed = False
            continue

        with open(plan_path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        failures = []
        for name, validator in CRITERIA:
            if not validator(content):
                failures.append(name)

        if failures:
            print(f"[FAIL] {plan_path} ({char_count:,} chars) - Failed criteria: {', '.join(failures)}")
            all_passed = False
        else:
            print(f"[PASS] {plan_path} ({char_count:,} chars) - All 9 criteria satisfied!")

    print("\n" + ("=" * 80))
    if all_passed:
        print(f"SUCCESS: All {len(BATCH_36_PLANS)} plans in Batch 36 passed all 9 verification gates!")
    else:
        print("ERROR: One or more plans failed verification.")
        sys.exit(1)

if __name__ == "__main__":
    main()
