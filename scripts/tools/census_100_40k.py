#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Census script to count markdown plans in the 100-40,000 character range
both in docs/ and across the entire repository.
"""

import os

def scan_markdown(root_dir):
    brackets = {
        "100_999": 0,
        "1k_5k": 0,
        "5k_10k": 0,
        "10k_20k": 0,
        "20k_40k": 0,
    }
    ge_250k = 0
    total_md = 0
    all_files = []

    for root, dirs, files in os.walk(root_dir):
        # Exclude git, cache, artifacts, venvs
        if ".git" in root or ".gemini" in root or "__pycache__" in root or "bin" in root or "obj" in root:
            continue
        for file in files:
            if file.endswith(".md"):
                total_md += 1
                full_path = os.path.join(root, file)
                try:
                    with open(full_path, "r", encoding="utf-8", errors="ignore") as f:
                        content = f.read()
                    char_len = len(content)
                    mtime = os.path.getmtime(full_path)
                    all_files.append((char_len, mtime, os.path.relpath(full_path, root_dir)))

                    if char_len >= 250000:
                        ge_250k += 1
                    elif 100 <= char_len < 1000:
                        brackets["100_999"] += 1
                    elif 1000 <= char_len < 5000:
                        brackets["1k_5k"] += 1
                    elif 5000 <= char_len < 10000:
                        brackets["5k_10k"] += 1
                    elif 10000 <= char_len < 20000:
                        brackets["10k_20k"] += 1
                    elif 20000 <= char_len <= 40000:
                        brackets["20k_40k"] += 1
                except Exception as e:
                    pass

    total_100_40k = sum(brackets.values())
    return {
        "total_md": total_md,
        "ge_250k": ge_250k,
        "total_100_40k": total_100_40k,
        "brackets": brackets,
        "all_files": all_files
    }

def main():
    print("=" * 80)
    print("ASHFALL REPOSITORY PLAN CENSUS (100–40,000 CHAR RANGE)")
    print("=" * 80)

    docs_data = scan_markdown("docs")
    repo_data = scan_markdown(".")

    print("\n--- [ docs/ Directory Scope ] ---")
    print(f"Total Markdown Files: {docs_data['total_md']:,}")
    print(f"Plans >= 250k Chars: {docs_data['ge_250k']:,}")
    print(f"Plans in 100–40k Range: {docs_data['total_100_40k']:,}")
    print("  Bracket Breakdown:")
    print(f"    - 100 – 999 chars:      {docs_data['brackets']['100_999']:,}")
    print(f"    - 1,000 – 4,999 chars:  {docs_data['brackets']['1k_5k']:,}")
    print(f"    - 5,000 – 9,999 chars:  {docs_data['brackets']['5k_10k']:,}")
    print(f"    - 10,000 – 19,999 chars:{docs_data['brackets']['10k_20k']:,}")
    print(f"    - 20,000 – 40,000 chars:{docs_data['brackets']['20k_40k']:,}")

    print("\n--- [ Entire Repository Scope ] ---")
    print(f"Total Markdown Files: {repo_data['total_md']:,}")
    print(f"Plans >= 250k Chars: {repo_data['ge_250k']:,}")
    print(f"Plans in 100–40k Range: {repo_data['total_100_40k']:,}")
    print("  Bracket Breakdown:")
    print(f"    - 100 – 999 chars:      {repo_data['brackets']['100_999']:,}")
    print(f"    - 1,000 – 4,999 chars:  {repo_data['brackets']['1k_5k']:,}")
    print(f"    - 5,000 – 9,999 chars:  {repo_data['brackets']['5k_10k']:,}")
    print(f"    - 10,000 – 19,999 chars:{repo_data['brackets']['10k_20k']:,}")
    print(f"    - 20,000 – 40,000 chars:{repo_data['brackets']['20k_40k']:,}")

    # Top 10 candidate files for the next batch (oldest with lowest character count in 100-40k range)
    # Filter files in docs/ that are not historical pre-foreman rules
    eligible_docs = [
        f for f in docs_data["all_files"]
        if 100 <= f[0] <= 40000 and "archive/agent-rules" not in f[2]
    ]
    # Sort by mtime ascending, then char_len ascending
    eligible_docs.sort(key=lambda x: (x[1], x[0]))

    print("\n--- [ Next 10 Smallest/Oldest Candidate Plans for Batch 36 ] ---")
    for i, (size, mtime, rel) in enumerate(eligible_docs[:10], 1):
        print(f"  {i:02d}. docs/{rel} ({size:,} chars)")

    print("=" * 80)

if __name__ == "__main__":
    main()
