#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""
Tasks 5–8 — generate docs/collectibles/COLLECTIBLES_UTILIZATION_MATRIX.md
(one row per authored collectible, machine-derived from the data authority;
no hardcoded ID lists). Supports --check for CI.
"""
import json
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
DATA = ROOT / "Assets" / "StreamingAssets" / "Data"
OUT = ROOT / "docs" / "collectibles" / "COLLECTIBLES_UTILIZATION_MATRIX.md"

EFFECT_CONSUMER = {
    "knowledge": "ResearchSystem.UnlockManual",
    "location_clue": "WastelandMapSystem.DiscoverSurvey",
    "journal_unlock": "JournalSystem.TryDiscoverKnowledge",
    "faction_info": "JournalSystem.TryDiscoverKnowledge",
    "morale": "NeedsSystem.Modify",
    "none": "discovery-only (+ acquisition map for vinyl)",
    "vinyl": "VinylMoraleSystem.AcquireRecord",
}


def main() -> int:
    coll = json.loads((DATA / "collectibles.json").read_text())["collectibles"]
    items = {i["id"]: i for i in json.loads((DATA / "items.json").read_text())["items"]}
    tables = json.loads((DATA / "scavenging_tables.json").read_text())["tables"]

    sources: dict[str, list[str]] = {}
    for t in tables:
        for e in t.get("entries", []):
            if e.get("item_id", "").startswith("item_collectible_"):
                sources.setdefault(e["item_id"], []).append(t["id"])

    lines = [
        "# ASHFALL Collectibles Utilization Matrix",
        "",
        "**Generated** by `scripts/ci/generate-collectibles-matrix.py` — machine-derived from",
        "`collectibles.json`, `items.json`, and `scavenging_tables.json`. Regenerate; never hand-edit.",
        "",
        "| Collectible | Category | Rarity | Weight | Trade Value | Effect | Target | Sources (n) | Unique? | Consumer | Status |",
        "|---|---|---|---:|---:|---|---|---|---|---|---|",
    ]

    broken = 0
    for c in sorted(coll, key=lambda x: x["item_id"]):
        item = items.get(c["item_id"])
        srcs = sources.get(c["item_id"], [])
        consumer = EFFECT_CONSUMER.get(c["effect_type"], "—")
        if item is None:
            status, w, v = "❌ item orphan", "—", "—"
        elif not srcs:
            status, w, v = "❌ no acquisition source", item.get("weight", "—"), item.get("tradeValue", "—")
        else:
            status, w, v = "✅ live", item.get("weight", "—"), item.get("tradeValue", "—")
        if status != "✅ live":
            broken += 1
        lines.append(
            f"| {c['item_id']} | {c['category']} | {c['rarity']} | {w} | {v} "
            f"| {c['effect_type']} | {c['effect_target'] or '—'} "
            f"| {len(srcs)} | {'✓' if c.get('unique') else ''} "
            f"| {EFFECT_CONSUMER.get(c['effect_type'], '—')} | {status} |"
        )

    lines += [
        "",
        f"**Rows:** {len(coll)}/40 · **Broken:** {broken} (must be 0 for the permanent gate)",
        "",
        "Acquisition sources are runtime table identities from `scavenging_tables.json` (§9.4).",
        "Effect-target FKs (research/map/journal) are enforced by `CollectibleCatalogIntegrityValidator`",
        "inside `--data-integrity-selftest` (Wave B); the utilization layer (§9.18) proves sources",
        "reach the runtime and consumers stay live.",
    ]
    OUT.parent.mkdir(parents=True, exist_ok=True)
    content = "\n".join(lines) + "\n"
    if "--check" in sys.argv:
        if OUT.exists() and OUT.read_text() == content:
            print(f"OK: {OUT} is in sync ({len(coll)} rows).")
            return 0
        print(f"FAIL: {OUT} is out of date. Run python3 scripts/ci/generate-collectibles-matrix.py")
        return 1
    OUT.write_text(content)
    print(f"Wrote {OUT} ({len(coll)} rows).")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
