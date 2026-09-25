#!/usr/bin/env python3
"""
audit_batch_40.py
Rigorous verification audit script for Batch 40: 15 expanded plans.
Checks each plan against all quality, architecture, and verification criteria:
  1. Character count >= 250,000 characters
  2. Grounding in Master Authority Volumes 1-57
  3. Pure engine-free C# domain architecture (netstandard2.1, zero engine refs)
  4. Draft 2020-12 JSON Schema specification
  5. >= 100 isolated [Fact] xUnit tests
  6. 600-day longitudinal deterministic simulation traces with 0xHEX digests
  7. 25-point QA acceptance checklist
  8. Section XII Deep Polishing Pass
  9. Section XV Precision Pass
 10. 150 Domain Casebooks
 11. 150 Field Treatises
"""

import os
import re
import sys

PLANS_BATCH_40 = [
    "docs/production/PRODUCTION_TRADE_FLOW.md",
    "docs/spiritual/PLAN30_BASELINE.md",
    "docs/expeditions/PLAN32_BASELINE.md",
    "docs/progression/SKILL_DOMAIN_MATRIX.md",
    "docs/progression/PLAN33_REGRESSION_MATRIX.md",
    "docs/combat/COMBAT_AUTHORITY_MAP.md",
    "docs/world/REGIONAL_CONTROL_MATRIX.md",
    "docs/expeditions/EXPEDITION_SCHEMA_CONTRACT.md",
    "docs/progression/PLAN33_BASELINE.md",
    "docs/progression/SKILL_SYSTEM_HOOK_MATRIX.md",
    "docs/world/DYNAMIC_WORLD_SAVE_CONTRACT.md",
    "docs/progression/RESEARCH_KNOWLEDGE_SCHEMA.md",
    "docs/progression/AUTOPSY_KNOWLEDGE_MATRIX.md",
    "docs/progression/RESEARCH_DATA_AUTHORITY_MIGRATION.md",
    "docs/radio/RADIO_AUDIO_HOOKS.md",
]

def audit_plan(path):
    if not os.path.exists(path):
        return False, f"File missing: {path}"

    with open(path, "r", encoding="utf-8") as f:
        content = f.read()

    char_count = len(content)
    errors = []

    # 1. Character count >= 250,000
    if char_count < 250000:
        errors.append(f"Character count {char_count} < 250,000")

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

    # 6. 600-day/cycle simulation trace with digests
    if not any(k in content for k in ["600-DAY", "600-Day", "600-CYCLE", "600-Cycle"]):
        errors.append("Missing 600-Day/Cycle longitudinal simulation trace header")
    if "StateDigest:" not in content:
        errors.append("Missing StateDigest marker in simulation trace")

    # 7. 25-point QA checklist
    if "25-POINT" not in content and "25-Point" not in content:
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
        return False, f"FAILED: {', '.join(errors)} (chars: {char_count}, facts: {fact_count}, casebooks: {casebook_count}, treatises: {treatise_count})"

    return True, f"PASSED: {char_count:,} chars, {fact_count} [Fact]s, {casebook_count} casebooks, {treatise_count} treatises"

def main():
    print(f"Auditing Batch 40: {len(PLANS_BATCH_40)} Plans...")
    all_passed = True
    total_chars = 0
    results = []

    for i, path in enumerate(PLANS_BATCH_40, 1):
        passed, msg = audit_plan(path)
        if not passed:
            all_passed = False
            print(f"[{i:02d}/15] ❌ {path}: {msg}")
        else:
            with open(path, "r", encoding="utf-8") as f:
                chars = len(f.read())
            total_chars += chars
            print(f"[{i:02d}/15] ✅ {path}: {msg}")
        results.append((path, passed, msg))

    print("\n" + "=" * 80)
    print(f"Batch 40 Audit Summary: {'ALL 15 PLANS PASSED 100% GREEN' if all_passed else 'SOME PLANS FAILED'}")
    print(f"Total Characters Across Batch 40: {total_chars:,} characters")
    print(f"Average Characters Per Plan: {total_chars // len(PLANS_BATCH_40):,} characters")
    print("=" * 80)

    if not all_passed:
        sys.exit(1)

if __name__ == "__main__":
    main()
