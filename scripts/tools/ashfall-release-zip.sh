#!/bin/bash
# ASHFALL — Linux alpha distribution zip.
#
# Packages builds/linux into dist/ashfall-alpha-linux-x86_64-YYYYMMDD.zip with a
# SHA256 manifest, and writes the zip checksum beside it. Requires the three
# real artifacts: binary, PCK, and the data_* assembly folder.
set -u
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT" || exit 1
OUT="$ROOT/builds/linux"
DIST="$ROOT/dist"
DATE="$(date +%Y%m%d)"
NAME="ashfall-alpha-linux-x86_64-$DATE"
ZIP="$DIST/$NAME.zip"

for artifact in ashfall.x86_64 ashfall.pck data_Ashfall_linuxbsd_x86_64; do
  if [ ! -e "$OUT/$artifact" ]; then
    echo "[release] ABORT: missing $OUT/$artifact (run ashfall-package-verify.sh first)"
    exit 1
  fi
done

mkdir -p "$DIST"
STAGE="$(mktemp -d)/$NAME"
mkdir -p "$STAGE"
cp "$OUT/ashfall.x86_64" "$OUT/ashfall.pck" "$STAGE/"
cp -r "$OUT/data_Ashfall_linuxbsd_x86_64" "$STAGE/"
[ -f "$OUT/RELEASE_STAMP.txt" ] && cp "$OUT/RELEASE_STAMP.txt" "$STAGE/"
[ -f "$OUT/run-ashfall.sh" ] && cp "$OUT/run-ashfall.sh" "$STAGE/"

cat > "$STAGE/HOW-TO-RUN.txt" <<'EOF'
ASHFALL alpha (Linux x86_64)

Run:      ./ashfall.x86_64            (self-contained: data loads from ashfall.pck)
Optional: ./run-ashfall.sh            (same, but reads the loose data folder when present
                                       — delete that folder before refreshing it)
Do not separate the three parts: ashfall.x86_64, ashfall.pck and data_Ashfall_linuxbsd_x86_64/
must stay in the same directory or the game will report missing .NET assemblies.

Data authority: the shipped PCK contains all data catalogs; the game verifies them at boot
(data-integrity self-test). Logs/telemetry: ~/.local/share/godot/app_userdata/
EOF

( cd "$(dirname "$STAGE")" && sha256sum "$NAME/ashfall.x86_64" "$NAME/ashfall.pck" > "$NAME/SHA256SUMS.txt" )
( cd "$(dirname "$STAGE")" && zip -qr "$ZIP" "$NAME" )
sha256sum "$ZIP" > "$ZIP.sha256"
rm -rf "$(dirname "$STAGE")"

echo "[release] wrote $ZIP ($(stat -c%s "$ZIP") bytes)"
echo "[release] checksum: $(cat "$ZIP.sha256")"

# ── Windows zip (when the Windows export exists) ────────────────────
WIN_OUT="$ROOT/builds/windows"
if [ -f "$WIN_OUT/ashfall.exe" ] && [ -f "$WIN_OUT/ashfall.pck" ] && [ -d "$WIN_OUT/data_Ashfall_windows_x86_64" ]; then
  WIN_NAME="ashfall-alpha-windows-x86_64-$DATE"
  WIN_ZIP="$DIST/$WIN_NAME.zip"
  WIN_STAGE="$(mktemp -d)/$WIN_NAME"
  mkdir -p "$WIN_STAGE"
  cp "$WIN_OUT/ashfall.exe" "$WIN_OUT/ashfall.pck" "$WIN_STAGE/"
  cp -r "$WIN_OUT/data_Ashfall_windows_x86_64" "$WIN_STAGE/"
  cat > "$WIN_STAGE/HOW-TO-RUN.txt" <<'EOF'
ASHFALL alpha (Windows x86_64)

Run: ashfall.exe
Keep ashfall.exe, ashfall.pck and data_Ashfall_windows_x86_64/ together —
the game will report missing .NET assemblies if they are separated.
EOF
  ( cd "$(dirname "$WIN_STAGE")" && sha256sum "$WIN_NAME/ashfall.exe" "$WIN_NAME/ashfall.pck" > "$WIN_NAME/SHA256SUMS.txt" )
  ( cd "$(dirname "$WIN_STAGE")" && zip -qr "$WIN_ZIP" "$WIN_NAME" )
  sha256sum "$WIN_ZIP" > "$WIN_ZIP.sha256"
  rm -rf "$(dirname "$WIN_STAGE")"
  echo "[release] wrote $WIN_ZIP ($(stat -c%s "$WIN_ZIP") bytes)"
  echo "[release] checksum: $(cat "$WIN_ZIP.sha256")"
else
  echo "[release] windows zip skipped (no windows export in $WIN_OUT)"
fi
