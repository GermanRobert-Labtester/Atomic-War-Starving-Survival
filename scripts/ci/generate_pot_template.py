#!/usr/bin/env python3
"""Generate the gettext POT template deterministically from strings.csv."""

from __future__ import annotations

import argparse
import csv
import json
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
CSV_PATH = ROOT / "assets/l10n/strings.csv"
POT_PATH = ROOT / "assets/l10n/template.pot"
POT_DATE = "2026-10-02 00:00+0000"


def generate() -> str:
    with CSV_PATH.open(encoding="utf-8-sig", newline="") as handle:
        rows = list(csv.DictReader(handle))
    entries = [row for row in rows if row.get("key") and row.get("en")]
    lines = [
        "# ASHFALL gettext translation template",
        'msgid ""',
        'msgstr ""',
        '"Project-Id-Version: Ashfall\\n"',
        f'"POT-Creation-Date: {POT_DATE}\\n"',
        '"MIME-Version: 1.0\\n"',
        '"Content-Type: text/plain; charset=UTF-8\\n"',
        '"Content-Transfer-Encoding: 8bit\\n"',
        f'"X-Source-Row-Count: {len(entries)}\\n"',
        f'"X-Generator: scripts/ci/generate_pot_template.py\\n"',
        "",
    ]
    for row in entries:
        source = row.get("source", "").strip()
        if source:
            lines.append("#: " + " ".join(source.split(";")))
        lines.append("msgid " + json.dumps(row["key"], ensure_ascii=False))
        lines.append("msgstr " + json.dumps(row["en"], ensure_ascii=False))
        lines.append("")
    return "\n".join(lines)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true", help="fail if template.pot is stale")
    args = parser.parse_args()
    expected = generate()
    if args.check:
        actual = POT_PATH.read_text(encoding="utf-8") if POT_PATH.exists() else ""
        if actual != expected:
            print(f"POT template is stale: run {Path(__file__).relative_to(ROOT)}", file=sys.stderr)
            return 1
        print(f"POT_TEMPLATE_DRIFT PASS — {expected.count(chr(10) + 'msgstr ') - 1} entries")
        return 0
    POT_PATH.write_text(expected, encoding="utf-8", newline="\n")
    print(f"POT template generated: {POT_PATH.relative_to(ROOT)}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
