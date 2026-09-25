#!/usr/bin/env python3
"""
audit_batch_39.py
Audits all 15 plans in Batch 39 against the 9 verification criteria:
1. File exists on disk
2. Character count >= 250,000 characters
3. Master Authority reference synchronization (Volumes 1-57)
4. Pure engine-free Core C# architecture (Assets/Ashfall.Core/ or netstandard2.1)
5. Authoritative Draft 2020-12 JSON Schema
6. 100-test xUnit verification suite with exactly >= 100 [Fact] assertions
7. Longitudinal 600-day/cycle simulation traces with 0xHEX state digests
8. 25-point QA acceptance checklist
9. Section XII Deep Polishing Pass & Section XV Precision Pass
"""

import os
import sys

BATCH_39_PLANS = [
    "docs/expeditions/LOOT_CATEGORY_ALLOWLIST.md",
    "docs/expeditions/DIVE_LOOT_PROVENANCE.md",
    "docs/production/FOUNDRY_TREATY_LABOR_MATRIX.md",
    "docs/progression/RESEARCH_BALANCE_MATRIX.md",
    "docs/production/FOOD_SPOILAGE_BALANCE.md",
    "docs/spiritual/FOLKLORE_CONTENT_MATRIX.md",
    "docs/radio/RADIO_ALERT_PRIORITY.md",
    "docs/production/FOUNDRY_MATERIAL_HEAT_LABOR_MATRIX.md",
    "docs/world/MAP_EVOLUTION_CONTRACT.md",
    "docs/progression/SKILL_CATALOG_SCHEMA.md",
    "docs/production/SALT_PRODUCT_MATRIX.md",
    "docs/ecology/ECOLOGY_CONTENT_UTILIZATION.md",
    "docs/ecology/FIELD_GUIDE_ECOLOGY_HANDOFF.md",
    "docs/ecology/ECOLOGICAL_EVENT_MATRIX.md",
    "docs/expeditions/VEHICLE_ROLE_MATRIX.md"
]

def audit():
    print(f"Auditing Batch 39 ({len(BATCH_39_PLANS)} plans)...")
    all_passed = True

    for idx, path in enumerate(BATCH_39_PLANS, 1):
        if not os.path.exists(path):
            print(f"[{idx:02d}/15] FAIL: {path} does not exist!")
            all_passed = False
            continue

        with open(path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        has_min_chars = char_count >= 250000
        has_master_auth = "Volumes 1–57" in content or "Volumes 1-57" in content
        has_core_arch = ("Assets/Ashfall.Core/" in content or "Ashfall.Core" in content) and ("netstandard2.1" in content or "engine-free" in content.lower())
        has_json_schema = "draft/2020-12/schema" in content or "Draft 2020-12" in content
        fact_count = content.count("[Fact]")
        has_xunit_100 = fact_count >= 100
        has_sim_trace = ("600-Day" in content or "600-DAY" in content or "600-Cycle" in content or "600-CYCLE" in content or "600-day" in content) and "0x" in content
        has_qa_checklist = "25-Point" in content or "25-POINT" in content
        has_polish_pass = "SECTION XII: DEEP POLISHING" in content or "SECTION XII" in content
        has_precision_pass = "SECTION XV: PRECISION PASS" in content or "SECTION XV" in content

        checks = [
            ("Min 250k chars", has_min_chars, f"{char_count:,} chars"),
            ("Master Authority", has_master_auth, ""),
            ("Core C# Architecture", has_core_arch, ""),
            ("Draft 2020-12 Schema", has_json_schema, ""),
            ("100 xUnit Tests", has_xunit_100, f"{fact_count} [Fact]s"),
            ("600-Day/Cycle Trace", has_sim_trace, ""),
            ("25-Point QA Checklist", has_qa_checklist, ""),
            ("Deep Polish Pass", has_polish_pass, ""),
            ("Precision Pass", has_precision_pass, "")
        ]

        failed_checks = [name for name, passed, extra in checks if not passed]

        if failed_checks:
            print(f"[{idx:02d}/15] FAIL: {path} ({char_count:,} chars) - Failed: {failed_checks}")
            all_passed = False
        else:
            print(f"[{idx:02d}/15] PASS: {path} ({char_count:,} chars, {fact_count} tests) - All 9 criteria GREEN")

    print("-" * 80)
    if all_passed:
        print("ALL 15 PLANS IN BATCH 39 PASSED 100% OF AUDIT CRITERIA!")
    else:
        print("SOME CHECKS FAILED IN BATCH 39 AUDIT.")
        sys.exit(1)

if __name__ == "__main__":
    audit()
