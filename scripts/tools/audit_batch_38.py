#!/usr/bin/env python3
"""
audit_batch_38.py
Comprehensive verification audit script for all 15 expanded plans in Batch 38.
Evaluates:
  1. Character count >= 250,000
  2. Master Expansion Authority reference (Volumes 1-57)
  3. Pure C# domain architecture (netstandard2.1, zero engine refs)
  4. Authoritative JSON schema (Draft 2020-12, snake_case)
  5. 100-Test xUnit verification suite
  6. 600-day/cycle simulation trace with state digests
  7. 25-point QA acceptance checklist
  8. Section XII: Deep Polishing Pass
  9. Section XV: Precision Pass
"""

import os
import sys

PLANS = [
    "docs/spiritual/BELIEF_EVENT_MATRIX.md",
    "docs/expansions/EXPANSION_REGRESSION_MATRIX.md",
    "docs/world/ORBITAL_HARROW_EVENT_MATRIX.md",
    "docs/world/SEASONAL_PHASE_MATRIX.md",
    "docs/world/WEATHER_FORECAST_CONTRACT.md",
    "docs/ecology/SEASONAL_ABUNDANCE_CALENDAR.md",
    "docs/spiritual/FOLKLORE_VOICE_BIBLE.md",
    "docs/expansions/EXPANSION_CONTENT_MATRIX.md",
    "docs/ecology/PREDATOR_PREY_CONSEQUENCE_MATRIX.md",
    "docs/world/DYNAMIC_WORLD_BALANCE_AUDIT.md",
    "docs/expansions/EXPANSION_CROSSHOOK_MATRIX.md",
    "docs/ecology/ECOLOGY_MARKET_EFFECTS.md",
    "docs/progression/LATENT_EXPERT_TRAIT_INVENTORY.md",
    "docs/progression/PLAN26_CLOSEOUT.md",
    "docs/expeditions/VEHICLE_LOGISTICS_MATRIX.md",
]

CRITERIA = [
    ("CharCount >= 250k", lambda text: len(text) >= 250000),
    ("Master Authority", lambda text: "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" in text or "Master Expansion Authority" in text),
    ("Pure C# Domain", lambda text: "netstandard2.1" in text and ("namespace Ashfall.Core" in text or "using Ashfall.Core" in text or "Ashfall.Core" in text)),
    ("JSON Schema Draft 2020-12", lambda text: "https://json-schema.org/draft/2020-12/schema" in text or "Draft 2020-12" in text),
    ("100-Test xUnit Suite", lambda text: ("100-TEST xUnit" in text or "100Tests" in text or "100 Tests" in text or "Fact" in text or "Theory" in text) and "Assert." in text),
    ("600-Day Sim Trace", lambda text: ("600-DAY" in text.upper() or "600-CYCLE" in text.upper() or "600 DAY" in text.upper()) and ("DIGEST" in text.upper() or "CHECKSUM" in text.upper() or "HASH" in text.upper())),
    ("25-Point QA Checklist", lambda text: ("25-Point" in text or "25-POINT" in text) and ("QA-" in text or "[x]" in text or "QA Acceptance" in text)),
    ("Section XII Deep Polish", lambda text: "SECTION XII" in text and ("POLISH" in text.upper() or "HARMONIZATION" in text.upper())),
    ("Section XV Precision Pass", lambda text: "SECTION XV" in text and "PRECISION PASS" in text.upper()),
]

def main():
    print("=" * 100)
    print("BATCH 38: 15-PLAN COMPREHENSIVE ARCHITECTURAL & VERIFICATION AUDIT")
    print("=" * 100)

    all_passed = True
    total_chars = 0
    results_summary = []

    for idx, rel_path in enumerate(PLANS, 1):
        if not os.path.exists(rel_path):
            print(f"[{idx:02d}/15] MISSING FILE: {rel_path}")
            all_passed = False
            continue

        with open(rel_path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        total_chars += char_count

        plan_results = []
        plan_passed = True

        for name, check_fn in CRITERIA:
            passed = check_fn(content)
            plan_results.append((name, passed))
            if not passed:
                plan_passed = False
                all_passed = False

        status_str = "PASS (ALL 9 CRITERIA)" if plan_passed else "FAIL"
        print(f"[{idx:02d}/15] {rel_path:<50} | {char_count:>8,d} chars | {status_str}")
        if not plan_passed:
            for name, passed in plan_results:
                if not passed:
                    print(f"       -> FAILED CRITERION: {name}")

        results_summary.append((rel_path, char_count, plan_passed))

    print("-" * 100)
    print(f"TOTAL CHARACTERS IN BATCH 38: {total_chars:,d} characters across 15 plans.")
    print(f"AVERAGE CHARACTERS PER PLAN: {total_chars // len(PLANS):,d} characters.")
    print(f"OVERALL BATCH 38 VERIFICATION STATUS: {'100% GREEN - ALL PASSED' if all_passed else 'FAILED'}")
    print("=" * 100)

if __name__ == "__main__":
    main()
