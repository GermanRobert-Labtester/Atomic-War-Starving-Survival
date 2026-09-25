#!/usr/bin/env python3
"""
Comprehensive Audit Script for Batch 41 (15 Plans)
Verifies:
1. File exists
2. Character count >= 250,000
3. Master Expansion Authority reference (Volumes 1–57)
4. Pure engine-free C# (netstandard2.1, zero Godot/UnityEngine using directives)
5. Draft 2020-12 JSON schema declaration
6. >= 100 [Fact] tests
7. 600-Day/Cycle longitudinal simulation trace with state digests
8. 25-Point QA Acceptance Checklist
9. Section XII Deep Polishing Pass
10. Section XV Precision Pass
11. >= 150 Domain Casebooks
12. >= 150 Field Treatises
"""

import os
import re
import sys

PLANS = [
    "docs/ui/EXPERT_WORKFLOW_AUDIT.md",
    "docs/spiritual/BELIEF_MOVEMENTS_SPECIFICATION.md",
    "docs/radio/SIGNAL_INTELLIGENCE_HANDOFF.md",
    "docs/expeditions/EXPEDITION_STAT_DERIVATION.md",
    "docs/ecology/ECOLOGY_BALANCE_AUDIT.md",
    "docs/ui/INFORMATION_HIERARCHY_AUDIT.md",
    "docs/radio/RADIO_SAVE_MIGRATION.md",
    "docs/progression/LATENT_EXPERT_AWAKENING_MATRIX.md",
    "docs/ecology/PLAN28_COMPLETION_REPORT.md",
    "docs/production/PRODUCTION_REGRESSION_MATRIX.md",
    "docs/radio/BROADCAST_STATE_PROVENANCE.md",
    "docs/progression/MANUAL_KNOWLEDGE_MATRIX.md",
    "docs/ecology/ECOLOGY_REGRESSION_MATRIX.md",
    "docs/ecology/RAD_TAINT_FOOD_SAFETY_MATRIX.md",
    "docs/production/APICULTURE_PRODUCT_MATRIX.md"
]

def audit():
    print(f"Auditing Batch 41 ({len(PLANS)} plans)...")
    total_chars = 0
    all_passed = True

    for i, plan in enumerate(PLANS, 1):
        if not os.path.exists(plan):
            print(f"[{i:02d}] FAIL: {plan} does not exist!")
            all_passed = False
            continue

        with open(plan, "r", encoding="utf-8") as f:
            content = f.read()

        length = len(content)
        total_chars += length
        errors = []

        # 1. Character count >= 250,000
        if length < 250000:
            errors.append(f"Character count {length:,} < 250,000")

        # 2. Master Authority grounding
        if "Volumes 1–57" not in content and "Volumes 1-57" not in content:
            errors.append("Missing Master Expansion Authority reference (Volumes 1–57)")

        # 3. Pure engine-free C#
        if "netstandard2.1" not in content:
            errors.append("Missing netstandard2.1 C# domain architecture marker")
        if "using Godot;" in content or "using UnityEngine;" in content:
            errors.append("Violates Core engine-free rule (contains engine using directive)")

        # 4. Draft 2020-12 JSON Schema
        if "https://json-schema.org/draft/2020-12/schema" not in content:
            errors.append("Missing Draft 2020-12 JSON schema declaration")

        # 5. >= 100 [Fact] tests
        fact_count = content.count("[Fact]")
        if fact_count < 100:
            errors.append(f"xUnit [Fact] test count {fact_count} < 100")

        # 6. 600-day/cycle simulation trace
        if not any(k in content for k in ["600-DAY", "600-Day", "600-CYCLE", "600-Cycle"]):
            errors.append("Missing 600-Day/Cycle longitudinal simulation trace header")
        if not any(k in content for k in ["Digest:", "Digest :", "StateDigest", "Checksum:", "0x"]):
            errors.append("Missing state digest marker in simulation trace")

        # 7. 25-point QA checklist
        if "25-POINT" not in content and "25-Point" not in content and "25-point" not in content:
            errors.append("Missing 25-Point QA Acceptance Checklist header")

        # 8. Section XII Deep Polishing Pass
        if "SECTION XII: DEEP POLISHING PASS" not in content:
            errors.append("Missing SECTION XII: DEEP POLISHING PASS")

        # 9. Section XV Precision Pass
        if "SECTION XV: PRECISION PASS" not in content:
            errors.append("Missing SECTION XV: PRECISION PASS")

        # 10. 150 Domain Casebooks
        casebook_count = len(re.findall(r"### Casebook [A-Z0-9_\-]+:\s*", content))
        if casebook_count < 150:
            errors.append(f"Casebook count {casebook_count} < 150")

        # 11. 150 Field Treatises
        treatise_count = len(re.findall(r"### Treatise [A-Z0-9_\-]+:\s*", content))
        if treatise_count < 150:
            errors.append(f"Treatise count {treatise_count} < 150")

        if errors:
            print(f"[{i:02d}] FAIL: {plan} ({length:,} chars) - Errors: {errors}")
            all_passed = False
        else:
            print(f"[{i:02d}] PASS: {plan} - {length:,} chars | {fact_count} [Fact] | {casebook_count} Casebooks | {treatise_count} Treatises")

    print("-" * 80)
    print(f"Total Batch 41 Characters: {total_chars:,}")
    print(f"Average Plan Length: {total_chars // len(PLANS):,} characters")
    if all_passed:
        print("ALL 15 PLANS IN BATCH 41 FULLY AUDITED AND 100% GREEN!")
    else:
        print("AUDIT FAILED: Some plans did not meet criteria.")
        sys.exit(1)

if __name__ == "__main__":
    audit()
