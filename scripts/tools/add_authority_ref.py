#!/usr/bin/env python3
import glob

auth_target = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"
authority_line = f"> **Master Expansion Authority File:** `{auth_target}`\n"

for p in range(87, 95):
    files = glob.glob(f"piagentsplans/{p}-*.md")
    if not files:
        continue
    filepath = files[0]
    with open(filepath, "r", encoding="utf-8") as f:
        content = f.read()

    if auth_target not in content:
        # insert after the first line (title)
        lines = content.splitlines(True)
        if len(lines) > 1:
            lines.insert(2, authority_line)
            new_content = "".join(lines)
            with open(filepath, "w", encoding="utf-8") as f:
                f.write(new_content)
            print(f"Added authority link to {filepath}")
        else:
            print(f"File too short: {filepath}")
    else:
        print(f"Authority already present in {filepath}")
