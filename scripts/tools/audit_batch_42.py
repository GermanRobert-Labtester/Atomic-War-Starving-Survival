import os
import sys
import re

PLANS = [
    # Part 1
    "docs/remediation/tickets/LOCALIZATION_STORE_PACKS.md",
    "docs/remediation/tickets/UTILITYAI_EXPANDED_CATALOG_REQUARANTINE.md",
    "docs/economy/HARDCORE_WEATHER_HANDOFF.md",
    # Part 2
    "docs/memorials/WASTELAND_EPITAPH_CONTENT_UTILIZATION.md",
    "docs/content/STARTING_COHORT_NARRATIVE_COMPATIBILITY.md",
    "docs/foundry/FOUNDRY_TREATY_SAVE_CONTRACT.md",
    # Part 3
    "docs/moral_choice/MORAL_FLAG_SCHEMA.md",
    "docs/year_of_ash/YEAR_OF_ASH_DAY_WINDOW_MATRIX.md",
    "docs/survivors/FINAL_WISH_RECIPE_HANDOFF.md",
    # Part 4
    "docs/survivors/FINAL_WISH_CONFESSION_HANDOFF.md",
    "docs/foundry/FOUNDRY_TREATY_ACCESS_HANDOFF.md",
    "docs/foundry/FOUNDRY_TREATY_RESOURCE_HANDOFF.md",
    # Part 5
    "docs/medical/PLAN112_AUTOPSY_INTEGRATION.md",
    "docs/radio/PLAN_119_UV_CORONA_AUTHORITY_MAP.md",
    "docs/MEDICAL_30_DAY_CAPACITY_REPORT.md"
]

def audit():
    print("=" * 80)
    print("BATCH 42 AUDIT: 15 EXPANDED PLANS QUALITY & ARCHITECTURE VERIFICATION")
    print("=" * 80)

    all_passed = True
    total_chars = 0

    for idx, rel_path in enumerate(PLANS, 1):
        if not os.path.exists(rel_path):
            print(f"[{idx:02d}/15] FAIL - File not found: {rel_path}")
            all_passed = False
            continue

        with open(rel_path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        total_chars += char_count

        # Criteria checks
        has_min_chars = char_count >= 250000
        has_master_ref = "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" in content
        has_core_target = "Assets/Ashfall.Core/" in content and "netstandard2.1" in content
        no_godot = ("using Godot;" not in content) and ("using UnityEngine;" not in content)
        has_schema = "https://json-schema.org/draft/2020-12/schema" in content and "additionalProperties" in content

        fact_count = len(re.findall(r'\[Fact\]', content))
        has_100_facts = fact_count >= 100

        has_600_days = ("Day 600" in content) or ("Day 599" in content)
        has_digests = bool(re.search(r'0x[0-9A-Fa-f]{8}', content))

        has_25_qa = "25-POINT QA ACCEPTANCE CHECKLIST" in content or "25-Point QA Acceptance Checklist" in content
        has_sec_xii = "SECTION XII: DEEP POLISHING PASS" in content
        has_sec_xv = "SECTION XV: PRECISION PASS" in content

        casebook_count = len(re.findall(r'### Casebook', content))
        treatise_count = len(re.findall(r'### Treatise', content))

        has_150_cb = casebook_count >= 150
        has_150_tr = treatise_count >= 150

        plan_ok = (
            has_min_chars and has_master_ref and has_core_target and no_godot and
            has_schema and has_100_facts and has_600_days and has_digests and
            has_25_qa and has_sec_xii and has_sec_xv and has_150_cb and has_150_tr
        )

        status_str = "PASS" if plan_ok else "FAIL"
        if not plan_ok:
            all_passed = False

        print(f"[{idx:02d}/15] {status_str} | {char_count:,} chars | {fact_count} [Fact]s | {casebook_count} CB | {treatise_count} TR | {rel_path}")

        if not plan_ok:
            print("   Failures:")
            if not has_min_chars: print(f"     - Under 250k characters ({char_count:,})")
            if not has_master_ref: print("     - Missing Master Authority Reference")
            if not has_core_target: print("     - Missing Assets/Ashfall.Core/ or netstandard2.1 target")
            if not no_godot: print("     - Found forbidden engine using directives")
            if not has_schema: print("     - Missing Draft 2020-12 schema with additionalProperties")
            if not has_100_facts: print(f"     - Insufficient [Fact] tests ({fact_count} < 100)")
            if not (has_600_days and has_digests): print("     - Missing 600-day simulation trace or hex digests")
            if not has_25_qa: print("     - Missing 25-point QA checklist")
            if not has_sec_xii: print("     - Missing Section XII Deep Polish")
            if not has_sec_xv: print("     - Missing Section XV Precision Pass")
            if not has_150_cb: print(f"     - Insufficient casebooks ({casebook_count} < 150)")
            if not has_150_tr: print(f"     - Insufficient treatises ({treatise_count} < 150)")

    print("-" * 80)
    print(f"Total Characters across Batch 42: {total_chars:,} characters (Average: {total_chars // 15:,} chars/plan)")
    if all_passed:
        print("OVERALL RESULT: ALL 15 PLANS IN BATCH 42 PASSED 100% GREEN!")
    else:
        print("OVERALL RESULT: SOME PLANS FAILED AUDIT.")
    print("=" * 80)
    return 0 if all_passed else 1

if __name__ == "__main__":
    sys.exit(audit())
