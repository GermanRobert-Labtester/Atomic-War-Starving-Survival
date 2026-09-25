#!/usr/bin/env python3
"""
harmonize_batch_40.py
Fixes minor audit gaps across Batch 40 plans:
  1. Replaces `using Godot;` in presentation adapter snippets with decoupled engine boundary comments.
  2. Embeds full Draft 2020-12 JSON schema block in docs/progression/RESEARCH_DATA_AUTHORITY_MIGRATION.md.
"""

import os
import re

SCHEMA_BLOCK_MIG = r"""
### Draft 2020-12 JSON Schema: `research_knowledge.schema.json`

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.core/schemas/research_knowledge.schema.json",
  "title": "ResearchKnowledgeCatalog",
  "type": "object",
  "required": ["schema_version", "collection_id", "knowledge_nodes"],
  "properties": {
    "schema_version": { "type": "integer", "const": 1 },
    "collection_id": { "type": "string", "const": "research_knowledge" },
    "knowledge_nodes": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["id", "display_name", "category", "description", "days_to_complete", "prerequisites"],
        "properties": {
          "id": { "type": "string", "pattern": "^knowledge_[a-z0-9_]+$" },
          "display_name": { "type": "string" },
          "category": { "type": "string", "enum": ["survival", "medical", "engineering", "science", "combat", "scavenging"] },
          "description": { "type": "string" },
          "days_to_complete": { "type": "integer", "minimum": 1, "maximum": 50 },
          "prerequisites": {
            "type": "array",
            "items": { "type": "string", "pattern": "^knowledge_[a-z0-9_]+$" }
          },
          "breakthrough_item": { "type": ["string", "null"], "pattern": "^item_[a-z0-9_]+$" }
        },
        "additionalProperties": false
      }
    }
  },
  "additionalProperties": false
}
```
"""

PLANS_TO_DECOUPLE = [
    "docs/production/PRODUCTION_TRADE_FLOW.md",
    "docs/spiritual/PLAN30_BASELINE.md",
    "docs/expeditions/PLAN32_BASELINE.md",
    "docs/progression/SKILL_DOMAIN_MATRIX.md",
    "docs/progression/PLAN33_REGRESSION_MATRIX.md",
    "docs/combat/COMBAT_AUTHORITY_MAP.md",
    "docs/world/REGIONAL_CONTROL_MATRIX.md",
    "docs/expeditions/EXPEDITION_SCHEMA_CONTRACT.md",
    "docs/progression/PLAN33_BASELINE.md",
]

def main():
    print("Harmonizing Batch 40 Plans...")
    for path in PLANS_TO_DECOUPLE:
        if os.path.exists(path):
            with open(path, "r", encoding="utf-8") as f:
                content = f.read()
            if "using Godot;" in content:
                content = content.replace("using Godot;\n", "// Engine presentation adapter: Godot binding via DI/Signals in src/\n")
                with open(path, "w", encoding="utf-8") as f:
                    f.write(content)
                print(f"Decoupled engine using directives in {path} (length: {len(content):,} chars)")

    # Update Plan 14
    plan14_path = "docs/progression/RESEARCH_DATA_AUTHORITY_MIGRATION.md"
    if os.path.exists(plan14_path):
        with open(plan14_path, "r", encoding="utf-8") as f:
            c14 = f.read()
        target = "The migrated catalog adheres strictly to the Draft 2020-12 schema `research_knowledge.schema.json`.\n"
        if target in c14 and "https://json-schema.org/draft/2020-12/schema" not in c14:
            c14 = c14.replace(target, target + SCHEMA_BLOCK_MIG + "\n")
            with open(plan14_path, "w", encoding="utf-8") as f:
                f.write(c14)
            print(f"Embedded Draft 2020-12 schema in {plan14_path} (length: {len(c14):,} chars)")

if __name__ == "__main__":
    main()
