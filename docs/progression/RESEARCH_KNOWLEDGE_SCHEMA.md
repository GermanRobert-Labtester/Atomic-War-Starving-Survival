# Research Knowledge Schema

> **Document Status:** Authoritative Schema Specification
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. Schema Definition

`Assets/StreamingAssets/Data/research_knowledge.json` adheres to the following JSON schema:

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
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
        }
      }
    }
  }
}
```

---

## 2. Field Descriptions

- `id` (string, required): Unique snake_case identifier starting with `knowledge_`.
- `display_name` (string, required): UI display name for dashboard and research trees.
- `category` (string, required): Scientific discipline category.
- `description` (string, required): Player-facing descriptive lore and mechanical summary.
- `days_to_complete` (integer, required): Base days required for research under standard shelter allocation.
- `prerequisites` (array of strings, required): IDs of required prerequisite knowledge nodes.
- `breakthrough_item` (string, optional): ID of an item prototype awarded upon completion of research.
