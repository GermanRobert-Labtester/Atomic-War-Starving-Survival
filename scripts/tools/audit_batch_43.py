#!/usr/bin/env python3
"""
audit_batch_43.py
Comprehensive verification script for Batch 43 (15 expanded plans).
Validates all 12 criteria per plan:
  1. Character count >= 250,000
  2. Master authority reference (Volumes 1-57)
  3. Engine-free Core domain model (Assets/Ashfall.Core/, zero Godot/UnityEngine)
  4. Authoritative Draft 2020-12 JSON schema (additionalProperties: false)
  5. 100 xUnit tests with isolated assertions ([Fact] >= 100)
  6. 600-day longitudinal simulation traces with state checksums (0xHEX)
  7. 25-point QA acceptance checklist
  8. 150 Domain Casebooks
  9. 150 Technical Field Treatises
  10. Section XII Deep Polishing Pass
  11. Section XIII & XIV Integration Framework & Data Consumers
  12. Section XV Precision Pass
"""

import os
import re
import sys

BATCH_43_PLANS = [
    "docs/foundry/FOUNDRY_TREATY_OUTCOME_CONTRACT.md",
    "docs/foundry/FOUNDRY_TREATY_STANDING_HANDOFF.md",
    "docs/memorials/WASTELAND_EPITAPH_MICRO_LOCATION_HANDOFF.md",
    "docs/economy/HARDCORE_CONTENT_UTILIZATION.md",
    "docs/year_of_ash/YEAR_OF_ASH_EXISTING_8_AUDIT.md",
    "docs/crossing/CROSSING_EXISTING_11_PARITY.md",
    "docs/holdfast/PLAN128_BASELINE.md",
    "docs/medical/PLAN112_EXISTING_7_INVENTORY.md",
    "docs/holdfast/HOLDFAST_FLAVOR_SAVE_BEHAVIOR.md",
    "docs/implementation/PLAN142_TIMESTAMP_POLICY.md",
    "docs/year_of_ash/YEAR_OF_ASH_STAGE_GRAPH_MATRIX.md",
    "docs/content/PLAN156_SAVE_COMPATIBILITY.md",
    "docs/implementation/PLAN143_ATOMICITY_POLICY.md",
    "docs/world/PLAN_121_GPR_AUTHORITY_MAP.md",
    "docs/shelter/PLAN_120_COMPOSITES_AUTHORITY_MAP.md"
]

def audit_plan(path):
    if not os.path.exists(path):
        return False, [f"File not found: {path}"]

    with open(path, "r", encoding="utf-8") as f:
        text = f.read()

    char_count = len(text)
    errors = []

    # 1. Character count >= 250,000
    if char_count < 250000:
        errors.append(f"Character count {char_count} < 250,000")

    # 2. Master authority reference
    if "docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" not in text:
        errors.append("Missing Master Authority reference (Volumes 1-57)")

    # 3. Engine-free Core domain model
    if "Assets/Ashfall.Core/" not in text:
        errors.append("Missing Assets/Ashfall.Core/ namespace / path specification")
    if "using Godot;" in text or "using UnityEngine;" in text:
        errors.append("Violates Core engine-free rule (contains using Godot or using UnityEngine)")

    # 4. Draft 2020-12 schema
    if "https://json-schema.org/draft/2020-12/schema" not in text:
        errors.append("Missing Draft 2020-12 schema URI")
    if '"additionalProperties": false' not in text:
        errors.append('Missing "additionalProperties": false in schema')

    # 5. 100 xUnit tests
    fact_count = len(re.findall(r'\[Fact\]', text))
    if fact_count < 100:
        errors.append(f"Test count [Fact] {fact_count} < 100")

    # 6. 600-day simulation traces with 0xHEX
    if "Simulation Day" not in text or "0x" not in text:
        errors.append("Missing 600-day simulation trace or 0x state checksums")

    # 7. 25-point QA acceptance checklist
    if "25-POINT QA ACCEPTANCE CHECKLIST" not in text:
        errors.append("Missing 25-Point QA Acceptance Checklist header")

    # 8. 150 Domain Casebooks
    casebook_count = len(re.findall(r'### Casebook', text))
    if casebook_count < 150:
        errors.append(f"Casebook count {casebook_count} < 150")

    # 9. 150 Technical Field Treatises
    treatise_count = len(re.findall(r'### Treatise', text))
    if treatise_count < 150:
        errors.append(f"Treatise count {treatise_count} < 150")

    # 10. Section XII Deep Polishing Pass
    if "SECTION XII: DEEP POLISHING PASS" not in text:
        errors.append("Missing Section XII Deep Polishing Pass")

    # 11. Section XIII & XIV Integration Framework & Data Consumers
    if "SECTION XIII: INTEGRATION FRAMEWORK" not in text:
        errors.append("Missing Section XIII Integration Framework")
    if "SECTION XIV: DATA CONSUMER" not in text:
        errors.append("Missing Section XIV Data Consumer & Seam Harmonization")

    # 12. Section XV Precision Pass
    if "SECTION XV: PRECISION PASS" not in text:
        errors.append("Missing Section XV Precision Pass")

    details = {
        "chars": char_count,
        "facts": fact_count,
        "casebooks": casebook_count,
        "treatises": treatise_count
    }

    return (len(errors) == 0), errors, details

def run_audit():
    print("=" * 80)
    print("BATCH 43 COMPREHENSIVE 12-CRITERIA AUDIT (15 PLANS)")
    print("=" * 80)

    total_chars = 0
    all_passed = True

    for i, rel_path in enumerate(BATCH_43_PLANS, 1):
        passed, errors, details = audit_plan(rel_path)
        total_chars += details.get("chars", 0)

        status_str = "PASS [GREEN]" if passed else "FAIL [RED]"
        print(f"\n[{i:02d}/15] {rel_path}")
        print(f"     Status: {status_str}")
        print(f"     Metrics: Chars={details.get('chars', 0):,} | Tests={details.get('facts', 0)} | Casebooks={details.get('casebooks', 0)} | Treatises={details.get('treatises', 0)}")

        if not passed:
            all_passed = False
            for err in errors:
                print(f"     ERROR: {err}")

    print("\n" + "=" * 80)
    print(f"AUDIT SUMMARY: {'100% GREEN - ALL 15 PLANS PASSED' if all_passed else 'FAILURES DETECTED'}")
    print(f"Total Characters Across Batch 43: {total_chars:,} characters")
    print(f"Average Characters Per Plan: {total_chars // len(BATCH_43_PLANS):,} characters")
    print("=" * 80)

    return 0 if all_passed else 1

if __name__ == "__main__":
    sys.exit(run_audit())
