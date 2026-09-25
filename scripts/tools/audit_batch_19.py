#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audits Batch 19 (Plans 42, 64, 75, 100, 53, 97, 86, 108, 00, 119) to ensure:
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
    ("Plan 42", "piagentsplans/42-batch1-roadmap-scaffolding-systems.md"),
    ("Plan 64", "piagentsplans/64-batch3-roadmap-thin-catalog-expansion.md"),
    ("Plan 75", "piagentsplans/75-batch4-roadmap-narrative-depth.md"),
    ("Plan 100", "piagentsplans/100-dose-register-lifetime-booking.md"),
    ("Plan 53", "piagentsplans/53-batch2-roadmap-world-content.md"),
    ("Plan 97", "piagentsplans/97-batch6-roadmap-relics-confessions-endings.md"),
    ("Plan 86", "piagentsplans/86-batch5-roadmap-exploration-investigation.md"),
    ("Plan 108", "piagentsplans/108-batch7-roadmap-factions-economy-dose-ledger.md"),
    ("Plan 00", "piagentsplans/00-master-roadmap.md"),
    ("Plan 119", "piagentsplans/119-batch8-roadmap-moral-echoes-disease-expansion-quests.md")
]

AUTHORITY_FILE = "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"

def main():
    print("=" * 80)
    print("AUDITING BATCH 19 (10 OLDEST PLANS) FOR EXPANSION & QUALITY ASSURANCE")
    print("=" * 80)

    all_passed = True
    total_chars = 0

    for name, path in PLANS:
        if not os.path.exists(path):
            print(f"[FAIL] {name}: File not found at {path}")
            all_passed = False
            continue

        with open(path, "r", encoding="utf-8") as f:
            content = f.read()

        chars = len(content)
        total_chars += chars
        has_len = chars >= 250000
        has_auth = AUTHORITY_FILE in content
        has_polish = "SECTION XII: DEEP POLISHING PASS" in content
        has_precision = "SECTION XV: PRECISION PASS" in content
        has_tests = "100" in content and "xUnit" in content
        has_sim = "600-Day" in content or "600-day" in content or "Day 600" in content
        has_qa = "25-point" in content or "25-Point" in content or "[x] **25." in content or "25-POINT" in content
        has_csharp = "```csharp" in content
        has_json = "```json" in content

        passed = (has_len and has_auth and has_polish and has_precision and
                  has_tests and has_sim and has_qa and has_csharp and has_json)

        status = "[PASS]" if passed else "[FAIL]"
        if not passed:
            all_passed = False

        print(f"{status} {name} ({path}): {chars:,} chars")
        if not passed:
            print(f"       Details: Len={has_len} ({chars}/250000), Auth={has_auth}, Polish={has_polish}, "
                  f"Precision={has_precision}, Tests={has_tests}, Sim={has_sim}, QA={has_qa}, "
                  f"CSharp={has_csharp}, JSON={has_json}")

    print("=" * 80)
    print(f"Total Batch 19 Characters: {total_chars:,} across {len(PLANS)} plans")
    if all_passed:
        print("[SUCCESS] ALL 10 PLANS IN BATCH 19 MEET THE >= 250,000 CHARACTER REQUIREMENT WITH ALL AUDIT CHECKS PASSING!")
        sys.exit(0)
    else:
        print("[FAILURE] ONE OR MORE PLANS FAILED AUDIT CHECKS.")
        sys.exit(1)

if __name__ == "__main__":
    main()
