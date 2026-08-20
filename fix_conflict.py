content = """# ASHFALL primary CI gate — Unity 6000.5.5f1 (AUDIT-001 / AUDIT-006)
# Pins editor version explicitly so package resolution matches ProjectVersion.txt.
name: ASHFALL CI

on:
  push:
    branches: [main, master]
  pull_request:
    branches: [main, master]

permissions:
  contents: read
  checks: write

env:
  UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
  UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
  UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
  UNITY_VERSION: 6000.5.5f1

jobs:
  validate:
    name: Data Validation Gate
    runs-on: ubuntu-latest
    steps:
      - name: Checkout Repository
        uses: actions/checkout@v4

      - name: Validate StreamingAssets JSON syntax
        shell: bash
        run: |
          python3 - <<'PY'
          import json
          import pathlib
          import sys

          root = pathlib.Path("Assets/StreamingAssets")
          errors = []

          if not root.exists():
              print(f"WARN: {root} missing — skipping JSON syntax scan")
              sys.exit(0)

          files = sorted(root.rglob("*.json"))
          for path in files:
              try:
                  json.loads(path.read_text(encoding="utf-8"))
              except Exception as exc:
                  errors.append(f"{path}: {exc}")

          if errors:
              print("JSON validation FAILED:")
              print("\\n".join(errors))
              sys.exit(1)

          print(f"OK: validated {len(files)} JSON file(s) under {root}")
          PY

      - name: Assert ProjectVersion is 6000.5.5f1
        shell: bash
        run: |
          grep -q '^m_EditorVersion: 6000\.5\.5f1$' ProjectSettings/ProjectVersion.txt
          echo "ProjectVersion pin OK: 6000.5.5f1"

"""
with open(".github/workflows/ci.yml", "w") as f:
    f.write(content)
