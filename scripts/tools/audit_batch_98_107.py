#!/usr/bin/env python3
import glob
import os
import re

def main():
    plan_nums = range(98, 108)
    plans = []
    for p in plan_nums:
        matches = glob.glob(f"piagentsplans/{p}-*.md")
        if matches:
            plans.append(matches[0])
        else:
            print(f"MISSING Plan {p}!")

    print(f"Auditing {len(plans)} plans in Batch 98–107:\n")

    all_ok = True
    results = []
    for path in sorted(plans):
        with open(path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        has_polish = "SECTION XII: DEEP POLISHING PASS" in content or "DEEP POLISHING PASS" in content
        has_precision = "SECTION XV: PRECISION PASS" in content or "PRECISION PASS" in content
        test_count = len(re.findall(r"\[Fact\]", content))
        has_authority = "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" in content
        has_sim = any(k in content for k in ["600-DAY", "600-Day", "Day 600", "Day 599", "Day 595"])
        has_checklist = any(k in content for k in ["25-POINT", "25-Point"])

        status = "PASS" if (char_count >= 250000 and has_polish and has_precision and test_count == 100 and has_authority and has_sim and has_checklist) else "FAIL"
        if status == "FAIL":
            all_ok = False

        results.append({
            "file": os.path.basename(path),
            "chars": char_count,
            "polish": has_polish,
            "precision": has_precision,
            "tests": test_count,
            "authority": has_authority,
            "sim": has_sim,
            "checklist": has_checklist,
            "status": status
        })

    for r in results:
        fname = r["file"]
        c = f"{r['chars']:,}"
        p = r["polish"]
        pr = r["precision"]
        t = r["tests"]
        a = r["authority"]
        s = r["sim"]
        q = r["checklist"]
        st = r["status"]
        print(f"{fname:<45} | Chars: {c:>10} | Polish: {p} | Prec: {pr} | Tests: {t:>3} | Auth: {a} | Sim: {s} | QA: {q} => {st}")

    print(f"\nALL 10 PLANS AUDIT PASSED: {all_ok}")
    return 0 if all_ok else 1

if __name__ == "__main__":
    exit(main())
