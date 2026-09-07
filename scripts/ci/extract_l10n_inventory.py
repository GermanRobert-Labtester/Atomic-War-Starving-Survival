#!/usr/bin/env python3
"""Create the deterministic Wave-1 localization inventory artifact."""

from __future__ import annotations

import json
import re
from collections import Counter
from pathlib import Path


ROOT = Path(__file__).resolve().parents[2]
UI_ROOT = ROOT / "src" / "UI"
OUT = ROOT / "artifacts" / "l10n-inventory.json"

STRING_LITERAL = re.compile(r'\b(?:Text|TooltipText)\s*=\s*"([^"]+)"')
KEY_LOOKUP = re.compile(
    r'(?:AshfallLocalization\.)?(?:Tr|TrFormat|T|F)\(\s*"([^"]+)"'
)


def main() -> int:
    records: list[dict[str, object]] = []
    by_panel: Counter[str] = Counter()

    for path in sorted(UI_ROOT.glob("*.cs"), key=lambda item: item.as_posix()):
        lines = path.read_text(encoding="utf-8").splitlines()
        panel = path.stem
        for line_no, line in enumerate(lines, 1):
            for match in STRING_LITERAL.finditer(line):
                records.append(
                    {
                        "file": path.relative_to(ROOT).as_posix(),
                        "line": line_no,
                        "panel": panel,
                        "literal": match.group(1),
                        "classification": "hardcoded_ui_literal",
                        "key": None,
                        "exemption": "inventory_only_until_panel_wave",
                    }
                )
                by_panel[panel] += 1
            for match in KEY_LOOKUP.finditer(line):
                records.append(
                    {
                        "file": path.relative_to(ROOT).as_posix(),
                        "line": line_no,
                        "panel": panel,
                        "literal": None,
                        "classification": "localized_lookup",
                        "key": match.group(1),
                        "exemption": None,
                    }
                )

    records.sort(key=lambda item: (str(item["file"]), int(item["line"]), str(item["classification"])))
    payload = {
        "schema_version": 1,
        "generated_by": "scripts/ci/extract_l10n_inventory.py",
        "pilot_panels": ["ResearchPanel", "OnboardingHintPanel"],
        "ui_file_count": len(list(UI_ROOT.glob("*.cs"))),
        "record_count": len(records),
        "hardcoded_literal_count": sum(1 for item in records if item["classification"] == "hardcoded_ui_literal"),
        "localized_lookup_count": sum(1 for item in records if item["classification"] == "localized_lookup"),
        "literal_counts_by_panel": dict(sorted(by_panel.items())),
        "records": records,
    }
    OUT.parent.mkdir(parents=True, exist_ok=True)
    OUT.write_text(json.dumps(payload, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    print(
        f"L10N inventory written: {len(records)} records, "
        f"{payload['hardcoded_literal_count']} literals, "
        f"{payload['localized_lookup_count']} lookups"
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
