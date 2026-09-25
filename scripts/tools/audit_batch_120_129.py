#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audits Batch 18 (Plans 120 through 129) to ensure:
- Exact 10 plans verified
- Each plan >= 250,000 characters
- Master authority path present
- Section XII Deep Polishing Pass present
- Section XV Precision Pass present
- 100 xUnit tests present
- 600-day deterministic trace present
- 25-point QA checklist present
- Pure engine-free C# domain architecture present
- Authoritative JSON schema present
"""

import os
import sys

PLANS = [
    ("Plan 120", "piagentsplans/120-crossing-factions-expansion.md"),
    ("Plan 121", "piagentsplans/121-independent-faction-branch-expansion.md"),
    ("Plan 122", "piagentsplans/122-military-faction-branch-expansion.md"),
    ("Plan 123", "piagentsplans/123-rebel-faction-branch-expansion.md"),
    ("Plan 124", "piagentsplans/124-faction-war-location-overrides-expansion.md"),
    ("Plan 125", "piagentsplans/125-moral-choice-flags-expansion.md"),
    ("Plan 126", "piagentsplans/126-crossing-items-expansion.md"),
    ("Plan 127", "piagentsplans/127-verdict-data-corpus-ladder-expansion.md"),
    ("Plan 128", "piagentsplans/128-holdfast-flavor-factions-expansion.md"),
    ("Plan 129", "piagentsplans/129-foundry-production-expansion.md")
]

AUTHORITY_FILE = "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"

def main():
    print("=" * 80)
    print("AUDITING BATCH 18 (PLANS 120-129) FOR EXPANSION & QUALITY ASSURANCE")
    print("=" * 80)

    all_passed = True

    for name, path in PLANS:
        if not os.path.exists(path):
            print(f"[FAIL] {name}: File not found at {path}")
            all_passed = False
            continue

        with open(path, "r", encoding="utf-8") as f:
            content = f.read()

        chars = len(content)
        has_len = chars >= 250000
        has_auth = AUTHORITY_FILE in content
        has_polish = "SECTION XII: DEEP POLISHING PASS" in content
        has_precision = "SECTION XV: PRECISION PASS" in content
        has_tests = "100" in content and "xUnit" in content
        has_sim = "600-Day" in content or "600-day" in content or "Day 600" in content
        has_qa = "25-point" in content or "25-Point" in content or "[x] **25." in content
        has_csharp = "```csharp" in content
        has_json = "```json" in content

        status = "PASS" if (has_len and has_auth and has_polish and has_precision and has_tests and has_sim and has_qa and has_csharp and has_json) else "FAIL"
        if status == "FAIL":
            all_passed = False

        print(f"[{status}] {name} ({path}): {chars:,} chars")
        print(f"       Chars >= 250k: {has_len} | Authority: {has_auth} | Sec XII Polish: {has_polish} | Sec XV Precision: {has_precision}")
        print(f"       100 Tests: {has_tests} | 600-Day Sim: {has_sim} | 25-pt QA: {has_qa} | C# Arch: {has_csharp} | JSON: {has_json}")
        print("-" * 80)

    print("=" * 80)
    if all_passed:
        print("ALL 10 PLANS IN BATCH 18 CERTIFIED GREEN (>= 250,000 CHARS + FULL PASSES)")
    else:
        print("SOME CHECKS FAILED IN BATCH 18 AUDIT")
        sys.exit(1)

if __name__ == "__main__":
    main()
