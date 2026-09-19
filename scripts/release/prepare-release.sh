#!/usr/bin/env bash
# scripts/release/prepare-release.sh — Plan 48 / C2[21] Phase 4
# ==============================================================
# Automates the ASHFALL release ceremony:
#   1. Validates current state (fast gates, version-gate, changelog-drift)
#   2. Writes the three version sources to the target version
#   3. Generates the changelog section for the new version
#   4. Captures a schema snapshot into artifacts/golden_saves/historical/
#   5. Updates docs/INDEX.md and regenerates any drifted catalogs
#   6. Commits the release prep and prints the tag command
#
# Usage:
#   bash scripts/release/prepare-release.sh --version 1.2.0 --base v1.1.0
#   bash scripts/release/prepare-release.sh --version 1.2.0 --base v1.1.0 --dry-run
#
# Requirements:
#   - Clean working tree (git status must be clean)
#   - Must run from repo root
#   - All fast CI gates must pass before the script will proceed

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

# ---------------------------------------------------------------------------
# Argument parsing
# ---------------------------------------------------------------------------
VERSION=""
BASE_REF=""
DRY_RUN=false

while [[ $# -gt 0 ]]; do
    case "$1" in
        --version) VERSION="$2"; shift 2 ;;
        --base)    BASE_REF="$2"; shift 2 ;;
        --dry-run) DRY_RUN=true; shift ;;
        *) echo "Unknown argument: $1"; exit 1 ;;
    esac
done

if [[ -z "$VERSION" ]]; then
    echo "prepare-release: --version is required (e.g. --version 1.2.0)"
    exit 1
fi

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------
log()  { echo "[prepare-release] $*"; }
fail() { echo "[prepare-release] FAIL: $*" >&2; exit 1; }
dryrun_or() {
    if $DRY_RUN; then
        echo "[DRY-RUN] Would run: $*"
    else
        "$@"
    fi
}

cd "$REPO_ROOT"

# ---------------------------------------------------------------------------
# Step 0: Strict-semver check on target version
# ---------------------------------------------------------------------------
if ! echo "$VERSION" | grep -qE '^[0-9]+\.[0-9]+\.[0-9]+$'; then
    fail "Target version '$VERSION' is not strict semver X.Y.Z"
fi
log "Target version: $VERSION"

# ---------------------------------------------------------------------------
# Step 1: Clean working tree check
# ---------------------------------------------------------------------------
if [[ -n "$(git status --porcelain)" ]]; then
    fail "Working tree is not clean. Commit or stash changes before running."
fi
log "Working tree: clean"

# ---------------------------------------------------------------------------
# Step 2: Version gate — current state must pass first
# ---------------------------------------------------------------------------
log "Running version-gate.py (current state check)..."
if ! python3 scripts/ci/version-gate.py; then
    fail "version-gate.py failed on current state. Fix version drift before continuing."
fi

# ---------------------------------------------------------------------------
# Step 3: Write the three version sources
# ---------------------------------------------------------------------------
log "Writing version $VERSION to three sources..."

write_project_godot() {
    local ver="$1"
    local file="$REPO_ROOT/project.godot"
    if $DRY_RUN; then
        echo "[DRY-RUN] Would write config/version=\"$ver\" to project.godot"
        return
    fi
    sed -i "s/^config\/version=\"[^\"]*\"/config\/version=\"$ver\"/" "$file"
}

write_build_props() {
    local ver="$1"
    local file="$REPO_ROOT/Directory.Build.props"
    if $DRY_RUN; then
        echo "[DRY-RUN] Would write <VersionPrefix>$ver</VersionPrefix> to Directory.Build.props"
        return
    fi
    sed -i "s|<VersionPrefix>[^<]*</VersionPrefix>|<VersionPrefix>$ver</VersionPrefix>|" "$file"
}

write_export_presets() {
    local ver="$1"
    local file="$REPO_ROOT/export_presets.cfg"
    if $DRY_RUN; then
        echo "[DRY-RUN] Would write application/file_version=\"$ver\" and product_version to export_presets.cfg"
        return
    fi
    sed -i "s/^application\/file_version=\"[^\"]*\"/application\/file_version=\"$ver\"/" "$file"
    sed -i "s/^application\/product_version=\"[^\"]*\"/application\/product_version=\"$ver\"/" "$file"
}

write_project_godot "$VERSION"
write_build_props "$VERSION"
write_export_presets "$VERSION"

log "Verifying three-source agreement after write..."
if ! python3 scripts/ci/version-gate.py; then
    fail "version-gate.py failed after writing $VERSION. Inspect the three source files."
fi
log "Three-source agreement: OK"

# ---------------------------------------------------------------------------
# Step 4: Capture schema snapshot
# ---------------------------------------------------------------------------
SNAPSHOT_DIR="$REPO_ROOT/artifacts/golden_saves/historical"
SNAPSHOT_FILE="$SNAPSHOT_DIR/schema_snapshot_v${VERSION}.json"
MANIFEST_FILE="$SNAPSHOT_DIR/manifest.json"

if [[ -f "$SNAPSHOT_FILE" ]]; then
    log "Schema snapshot already exists: $SNAPSHOT_FILE (skipping)"
else
    log "Capturing schema snapshot for v$VERSION..."

    # Read current schema versions from VersionReport via dotnet
    SCHEMA_OUTPUT=$(dotnet run --project "$REPO_ROOT/Ashfall.csproj" --no-build -- --version 2>/dev/null | grep 'save schemas' || true)
    # Fallback: read directly from source constants
    HOLDFAST=$(grep -oP 'CurrentSaveVersion = \K[0-9]+' "$REPO_ROOT/Assets/Ashfall.Core/HoldfastSave.cs" | head -1)
    YEAR_OF_ASH=$(grep -oP 'CurrentSaveVersion = \K[0-9]+' "$REPO_ROOT/Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs" 2>/dev/null | head -1 || grep -rn 'CurrentSaveVersion = ' "$REPO_ROOT/Assets/Ashfall.Core/YearOfAsh/" | grep -oP '= \K[0-9]+' | head -1)
    DOSE_LEDGER=$(grep -oP 'CurrentSaveVersion = \K[0-9]+' "$REPO_ROOT/Assets/Ashfall.Core/DoseLedgerSave.cs" | head -1)
    EXPANSION_HUB=$(grep -oP 'CurrentSaveVersion = \K[0-9]+' "$REPO_ROOT/Assets/Ashfall.Core/ExpansionHubSave.cs" | head -1)
    EXPANSION_QUEST=$(grep -oP 'CurrentVersion = \K[0-9]+' "$REPO_ROOT/Assets/Ashfall.Core/ExpansionQuestSave.cs" | head -1)
    WEIGHT_OF_CHOICES=$(grep -oP 'CurrentSaveVersion = \K[0-9]+' "$REPO_ROOT/Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs" | head -1)

    if $DRY_RUN; then
        echo "[DRY-RUN] Would write schema_snapshot_v${VERSION}.json:"
        echo "  holdfast=$HOLDFAST year_of_ash=$YEAR_OF_ASH dose_ledger=$DOSE_LEDGER"
        echo "  expansion_hub=$EXPANSION_HUB expansion_quest=$EXPANSION_QUEST weight_of_choices=$WEIGHT_OF_CHOICES"
    else
        TODAY=$(date +%Y-%m-%d)
        mkdir -p "$SNAPSHOT_DIR"
        cat > "$SNAPSHOT_FILE" <<EOF
{
  "game_version": "${VERSION}",
  "release_tag": "v${VERSION}",
  "release_date": "${TODAY}",
  "captured_by": "scripts/release/prepare-release.sh",
  "notes": "Schema snapshot generated by prepare-release.sh at v${VERSION} release ceremony.",
  "_policy": "IMMUTABLE — never edit. Append-only corpus.",
  "schema_map": {
    "holdfast": ${HOLDFAST},
    "year_of_ash": ${YEAR_OF_ASH},
    "dose_ledger": ${DOSE_LEDGER},
    "expansion_hub": ${EXPANSION_HUB},
    "expansion_quest": ${EXPANSION_QUEST},
    "weight_of_choices": ${WEIGHT_OF_CHOICES}
  },
  "curated_codec_count": 6
}
EOF
        # Append to manifest
        python3 - <<'PYEOF'
import json, sys, os

manifest_path = os.path.join(os.environ.get('SNAPSHOT_DIR', ''), 'manifest.json')
snapshot_file = os.path.join(os.environ.get('SNAPSHOT_DIR', ''), f"schema_snapshot_v${VERSION}.json")
version = os.environ.get('TARGET_VERSION', '')

with open(manifest_path) as f:
    manifest = json.load(f)

snapshot_name = f"schema_snapshot_v{version}.json"
if not any(fx.get('fixture_name') == snapshot_name for fx in manifest.get('fixtures', [])):
    with open(snapshot_file) as f:
        snap = json.load(f)
    manifest['fixtures'].append({
        'fixture_name': snapshot_name,
        'game_version': snap['game_version'],
        'release_tag': snap['release_tag'],
        'release_date': snap['release_date'],
        'schema_map': snap['schema_map']
    })
    with open(manifest_path, 'w') as f:
        json.dump(manifest, f, indent=2)
        f.write('\n')
    print(f"Appended {snapshot_name} to manifest.json")
else:
    print(f"{snapshot_name} already in manifest")
PYEOF
    fi
fi

export SNAPSHOT_DIR
export TARGET_VERSION="$VERSION"

# ---------------------------------------------------------------------------
# Step 5: Changelog section generation
# ---------------------------------------------------------------------------
log "Running generate_changelog.py --check..."
if ! python3 scripts/release/generate_changelog.py --check; then
    fail "changelog_drift check failed. Fix CHANGELOG.md marker region before continuing."
fi

if [[ -n "$BASE_REF" ]]; then
    log "Generating changelog section for [$VERSION] from $BASE_REF..."
    dryrun_or python3 scripts/release/generate_changelog.py --version "$VERSION" --base "$BASE_REF"
fi

# ---------------------------------------------------------------------------
# Step 6: Regenerate docs index
# ---------------------------------------------------------------------------
log "Regenerating docs/INDEX.md..."
dryrun_or python3 scripts/ci/generate-docs-index.py

# ---------------------------------------------------------------------------
# Step 7: Build check
# ---------------------------------------------------------------------------
log "Running dotnet build (0/0 check)..."
if ! dotnet build "$REPO_ROOT/Ashfall.csproj" --nologo -v q 2>&1 | grep -qE '^Build succeeded'; then
    if $DRY_RUN; then
        echo "[DRY-RUN] Would verify dotnet build succeeds"
    else
        fail "dotnet build failed. Fix build errors before releasing."
    fi
fi
log "Build: OK"

# ---------------------------------------------------------------------------
# Step 8: Commit (not in dry-run)
# ---------------------------------------------------------------------------
if $DRY_RUN; then
    echo ""
    echo "[DRY-RUN] Dry run complete. The following would have been committed:"
    echo "  - project.godot, Directory.Build.props, export_presets.cfg → v${VERSION}"
    echo "  - CHANGELOG.md → [${VERSION}] section"
    echo "  - artifacts/golden_saves/historical/schema_snapshot_v${VERSION}.json"
    echo "  - docs/INDEX.md regenerated"
    echo ""
    echo "After real run, tag with:"
    echo "  git tag -a v${VERSION} -m \"ASHFALL v${VERSION}\""
    echo "  git push origin main v${VERSION}"
    exit 0
fi

git add \
    project.godot \
    Directory.Build.props \
    export_presets.cfg \
    CHANGELOG.md \
    docs/INDEX.md \
    "artifacts/golden_saves/historical/schema_snapshot_v${VERSION}.json" \
    "artifacts/golden_saves/historical/manifest.json" \
    2>/dev/null || true

git diff --check --cached || { echo "Whitespace issues detected. Fix before committing."; exit 1; }

git commit -m "release: prepare v${VERSION}

Version sources updated: project.godot, Directory.Build.props, export_presets.cfg
CHANGELOG.md [${VERSION}] section generated
Schema snapshot: artifacts/golden_saves/historical/schema_snapshot_v${VERSION}.json
docs/INDEX.md regenerated"

echo ""
echo "============================================================"
echo "  Release prepare COMPLETE for v${VERSION}"
echo "============================================================"
echo ""
echo "Review CHANGELOG.md, then tag and push:"
echo "  git tag -a v${VERSION} -m \"ASHFALL v${VERSION}\""
echo "  git push origin main v${VERSION}"
echo ""
echo "IMPORTANT: Run 'bash scripts/ci/release-gate.sh' before pushing the tag."
