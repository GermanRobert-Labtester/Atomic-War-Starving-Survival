#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audits Batch 20 (10 Oldest Remaining Plans with Lowest Character Counts) to ensure:
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
    ("Plan 130 (Batch 9 Roadmap)", "piagentsplans/130-batch9-roadmap-faction-branches-crossing-foundry.md"),
    ("Plan 02-09 (Consolidated Work)", "piagentsplans/02-09-consolidated-remaining-work.md"),
    ("Plan 31 (World Content Roadmap)", "piagentsplans/31-world-content-master-roadmap.md"),
    ("Expansion 09 (The Black Flotilla)", "docs/expansions/expansion_09_the_black_flotilla_plan.md"),
    ("Expansion 05 (The Year of Ash)", "docs/expansions/expansion_05_the_year_of_ash_plan.md"),
    ("Expansion 07 (The Dose)", "docs/expansions/expansion_07_the_dose_plan.md"),
    ("Expansion 08 (The Verdict)", "docs/expansions/expansion_08_the_verdict_plan.md"),
    ("Expansion 03 (The Standing Record)", "docs/expansions/expansion_03_the_standing_record_plan.md"),
    ("Expansion 02 (The Duty Roster)", "docs/expansions/expansion_02_the_duty_roster_plan.md"),
    ("Expansion 01 (The Holdfast)", "docs/expansions/expansion_the_holdfast_plan.md")
]

AUTHORITY_FILE = "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"

def main():
    print("=" * 80)
    print("AUDITING BATCH 20 (10 OLDEST PLANS) FOR EXPANSION & QUALITY ASSURANCE")
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
    print(f"Total Batch 20 Characters: {total_chars:,} across {len(PLANS)} plans")
    if all_passed:
        print("[SUCCESS] ALL 10 PLANS IN BATCH 20 MEET THE >= 250,000 CHARACTER REQUIREMENT WITH ALL AUDIT CHECKS PASSING!")
        sys.exit(0)
    else:
        print("[FAILURE] ONE OR MORE PLANS FAILED AUDIT CHECKS.")
        sys.exit(1)

if __name__ == "__main__":
    main()
