#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audits Batch 17 (Plans 109 to 118) for completeness, character count thresholds,
authority file citations, Section XII Deep Polishing Passes, Section XV Precision Passes,
xUnit test coverage, 600-day simulation traces, and 25-point checklists.
"""

import os
import sys
import re

AUTHORITY_PATH = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"

PLANS = [
    ("Plan 109", "piagentsplans/109-moral-choice-echo-quests-expansion.md"),
    ("Plan 110", "piagentsplans/110-moral-choice-gossip-expansion.md"),
    ("Plan 111", "piagentsplans/111-phantom-triggers-expansion.md"),
    ("Plan 112", "piagentsplans/112-disease-catalog-expansion.md"),
    ("Plan 113", "piagentsplans/113-verdict-questlines-expansion.md"),
    ("Plan 114", "piagentsplans/114-year-of-ash-questlines-expansion.md"),
    ("Plan 115", "piagentsplans/115-crossing-encounters-expansion.md"),
    ("Plan 116", "piagentsplans/116-deep-lore-locations-expansion.md"),
    ("Plan 117", "piagentsplans/117-holdfast-quests-expansion.md"),
    ("Plan 118", "piagentsplans/118-standing-record-quests-expansion.md"),
]

def audit():
    print("=" * 80)
    print("AUDITING BATCH 17 (PLANS 109 - 118)")
    print("=" * 80)

    all_passed = True

    for name, path in PLANS:
        if not os.path.exists(path):
            print(f"FAILED: {name} ({path}) does not exist!")
            all_passed = False
            continue

        with open(path, "r", encoding="utf-8") as f:
            content = f.read()

        chars = len(content)
        has_authority = AUTHORITY_PATH in content
        has_sec_xii = "SECTION XII: DEEP POLISHING PASS" in content
        has_sec_xv = "SECTION XV: PRECISION PASS" in content
        has_sec_v = "SECTION V: 600-DAY DETERMINISTIC REPLAY SIMULATION TRACE" in content

        test_count = len(re.findall(r"\[Fact\]|\[Theory\]", content))
        chk_count = len(re.findall(r"- \[x\] \*\*\d{2}\.", content))

        passed = (
            chars >= 250000 and
            has_authority and
            has_sec_xii and
            has_sec_xv and
            has_sec_v and
            test_count >= 100 and
            chk_count >= 25
        )

        status_str = "PASS" if passed else "FAIL"
        print(f"[{status_str}] {name}: {chars:,} chars | Authority: {has_authority} | Sec XII: {has_sec_xii} | Sec XV: {has_sec_xv} | Tests: {test_count} | QA Checklist: {chk_count}")

        if not passed:
            all_passed = False

    print("=" * 80)
    if all_passed:
        print("ALL 10 PLANS FULLY CERTIFIED & PASSED AUDIT (>= 250,000 CHARACTERS EACH)!")
    else:
        print("AUDIT FAILED: Some criteria were not satisfied.")
    print("=" * 80)

    return 0 if all_passed else 1

if __name__ == "__main__":
    sys.exit(audit())
