#!/usr/bin/env python3
"""
audit_batch_44.py
Audits all 15 plans of Batch 44 against the 12 mandatory criteria.
"""

import os
import re
import sys

PLANS = [
    "docs/shelter/ROOM_SKILL_RESEARCH_DEPENDENCY_MATRIX.md",
    "docs/economy/DEBT_DUE_TIME_CONTRACT.md",
    "docs/archive/ARCHIVE_INK_FORMULA_AUDIT.md",
    "docs/remediation/tickets/WEATHERGATE_INTEGRATION_REQUARANTINE.md",
    "docs/SHELTER_ACOUSTIC_AUTHORITY_MAP.md",
    "docs/content/STARTING_COHORT_BALANCE_SIMULATION.md",
    "docs/content/plan121/INDEPENDENT_BRANCH_SELECTION_BALANCE.md",
    "docs/content/plan121/INDEPENDENT_BRANCH_AUTHORITY_MAP.md",
    "docs/crossing/CROSSING_ITEM_ECONOMY_AUDIT.md",
    "docs/implementation/PLAN142_AUTHOR_IDENTITY_MAP.md",
    "docs/moral_choice/MORAL_FLAG_READ_PATH_MATRIX.md",
    "docs/crossing/CROSSING_ITEM_SCHEMA_CONTRACT.md",
    "docs/year_of_ash/YEAR_OF_ASH_CONSEQUENCE_MATRIX.md",
    "docs/moral_choice/MORAL_FLAG_SEMANTIC_MATRIX.md",
    "docs/holdfast/HOLDFAST_FLAVOR_BASELINE_MATRIX.md"
]

CRITERIA = [
    ("CharCount >= 250,000", lambda content: len(content) >= 250000),
    ("Master Authority Reference", lambda content: "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" in content),
    ("Engine-Free Core Target", lambda content: "Assets/Ashfall.Core/" in content and (".NET Standard 2.1" in content or "netstandard2.1" in content)),
    ("Zero Engine References", lambda content: "using Godot;" not in content and "using UnityEngine;" not in content),
    ("Authoritative Data Schema", lambda content: "draft/2020-12/schema" in content and "additionalProperties" in content),
    (">=100 xUnit Tests", lambda content: len(re.findall(r"\[Fact\]", content)) >= 100),
    ("600-Day Simulation Traces", lambda content: ("Day 001" in content or "[DAY 001]" in content) and ("Day 600" in content or "[DAY 600]" in content) and ("0x" in content)),
    ("25-Point QA Checklist", lambda content: "25-POINT QA ACCEPTANCE CHECKLIST" in content),
    ("Section XII Deep Polish", lambda content: "SECTION XII: DEEP POLISHING PASS" in content),
    ("Section XIII Integration Framework", lambda content: "SECTION XIII: INTEGRATION FRAMEWORK" in content),
    ("Section XIV Data Consumer & Seams", lambda content: "SECTION XIV: DATA CONSUMER" in content),
    ("Section XV Precision Pass", lambda content: "SECTION XV: PRECISION PASS" in content)
]

def main():
    workspace = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
    print("=" * 80)
    print("BATCH 44 AUDIT — 15 PLANS VERIFICATION")
    print("=" * 80)

    all_passed = True
    total_chars = 0

    for idx, rel_path in enumerate(PLANS, start=1):
        full_path = os.path.join(workspace, rel_path)
        if not os.path.exists(full_path):
            print(f"[{idx:02d}/15] FAIL — File missing: {rel_path}")
            all_passed = False
            continue

        with open(full_path, "r", encoding="utf-8") as f:
            content = f.read()

        c_len = len(content)
        total_chars += c_len

        failed_criteria = []
        for name, checker in CRITERIA:
            if not checker(content):
                failed_criteria.append(name)

        cb = len(re.findall(r"(?:Casebook|CASE-|Archival Docket|Archival Ledger)", content))
        tr = len(re.findall(r"(?:Treatise|Field Directive|Operative Directive|Commercial Treatise|Procedure|PROTOCOL)", content))

        status = "PASS" if not failed_criteria else "FAIL"
        print(f"[{idx:02d}/15] {status} | {c_len:,} chars | Casebooks/Ledgers: {cb} | Treatises: {tr} | {rel_path}")
        if failed_criteria:
            all_passed = False
            for fc in failed_criteria:
                print(f"       -> Missing: {fc}")

    print("=" * 80)
    print(f"BATCH 44 TOTAL CHARACTERS: {total_chars:,} (Average: {total_chars // len(PLANS):,} chars/plan)")
    print(f"ALL 15 PLANS GREEN: {all_passed}")
    print("=" * 80)

    if not all_passed:
        sys.exit(1)

if __name__ == "__main__":
    main()
