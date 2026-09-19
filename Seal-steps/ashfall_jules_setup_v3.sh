#!/usr/bin/env bash
# ASHFALL — Google Jules environment setup / snapshot script (v3)
# Audited against repository main on 2026-09-19.
#
# Canonical runtime:
#   Godot 4.7.1 .NET/C#
#   Ashfall host: net8.0
#   Ashfall.Core: net8.0
#   Ashfall.Core.Tests: net9.0
#
# Unity is retired and MUST NOT be installed or invoked.
#
# Jules clones the repository into /app before this script runs.
# Jules may deliberately set core.hooksPath to disable repository hooks.
# Therefore:
#   * keep Jules' hooks policy untouched;
#   * configure Git LFS with --skip-repo;
#   * do NOT invoke ./setup-repo.sh because it installs repository hooks.
#
# Snapshot policy:
#   * hydrate Git LFS;
#   * install deterministic .NET + Godot toolchains;
#   * restore/build the active projects;
#   * warm the Godot .godot import cache;
#   * run a small canonical snapshot verification by default;
#   * keep the expensive 50-gate + full xUnit verification opt-in.
#
# Optional switches:
#   ASHFALL_SETUP_EXPORT_TEMPLATES=1
#       Cache Godot export templates too. Off by default because they are large.
#
#   ASHFALL_SETUP_SNAPSHOT_VERIFY=0
#       Skip the lightweight post-warm verification.
#
#   ASHFALL_SETUP_FULL_VERIFY=1
#       Run the current CI fast-tier gate suite plus the full Core xUnit gate.
#
# Example Jules Initial Setup:
#   bash ./jules_setup_v3.sh
#
# After a successful "Run and Snapshot", future Jules tasks reuse the prepared
# environment snapshot.

set -Eeuo pipefail

export DEBIAN_FRONTEND=noninteractive
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export NUGET_XMLDOC_MODE=skip

APP_DIR="${APP_DIR:-/app}"
GODOT_VERSION="${GODOT_VERSION:-4.7.1}"
DOTNET_INSTALL_DIR="${DOTNET_INSTALL_DIR:-$HOME/.dotnet}"
LOCAL_BIN="${LOCAL_BIN:-$HOME/.local/bin}"
GODOT_INSTALL_DIR="${GODOT_INSTALL_DIR:-$HOME/.local/share/godot-${GODOT_VERSION}-mono}"
GODOT_TEMPLATE_DIR="${GODOT_TEMPLATE_DIR:-$HOME/.local/share/godot/export_templates/${GODOT_VERSION}.stable.mono}"
CACHE_DIR="${XDG_CACHE_HOME:-$HOME/.cache}/ashfall-jules-setup"

ASHFALL_SETUP_EXPORT_TEMPLATES="${ASHFALL_SETUP_EXPORT_TEMPLATES:-0}"
ASHFALL_SETUP_SNAPSHOT_VERIFY="${ASHFALL_SETUP_SNAPSHOT_VERIFY:-1}"
ASHFALL_SETUP_FULL_VERIFY="${ASHFALL_SETUP_FULL_VERIFY:-0}"

log()  { printf '\n[ashfall-jules] %s\n' "$*"; }
warn() { printf '\n[ashfall-jules] WARN: %s\n' "$*" >&2; }
die()  { printf '\n[ashfall-jules] ERROR: %s\n' "$*" >&2; exit 1; }

trap 'printf "\n[ashfall-jules] ERROR at line %s: %s\n" "$LINENO" "$BASH_COMMAND" >&2' ERR

[[ -d "$APP_DIR/.git" ]] || die "Expected Jules to clone the repository at $APP_DIR first."
cd "$APP_DIR"

mkdir -p "$LOCAL_BIN" "$CACHE_DIR" "$DOTNET_INSTALL_DIR"
export PATH="$LOCAL_BIN:$DOTNET_INSTALL_DIR:$PATH"
export DOTNET_ROOT="$DOTNET_INSTALL_DIR"

if [[ "${EUID:-$(id -u)}" -eq 0 ]]; then
  SUDO=()
  CAN_APT=1
elif command -v sudo >/dev/null 2>&1; then
  SUDO=(sudo)
  CAN_APT=1
else
  SUDO=()
  CAN_APT=0
fi

persist_profile_block() {
  local marker_begin="# >>> ASHFALL Jules environment >>>"
  local marker_end="# <<< ASHFALL Jules environment <<<"
  local profile="$HOME/.profile"
  touch "$profile"

  if grep -Fq "$marker_begin" "$profile" 2>/dev/null; then
    return 0
  fi

  {
    printf '\n%s\n' "$marker_begin"
    printf 'export DOTNET_ROOT="%s"\n' "$DOTNET_INSTALL_DIR"
    printf 'export PATH="%s:%s:$PATH"\n' "$LOCAL_BIN" "$DOTNET_INSTALL_DIR"
    printf '%s\n' "$marker_end"
  } >> "$profile"
}

install_os_prereqs() {
  command -v apt-get >/dev/null 2>&1 || {
    warn "apt-get is unavailable; assuming Jules image already contains native prerequisites."
    return 0
  }

  [[ "$CAN_APT" == "1" ]] || {
    warn "No root/sudo access; skipping apt prerequisites."
    return 0
  }

  local packages=(
    ca-certificates
    curl
    unzip
    git
    git-lfs
    jq
    python3
    file
    rsync
    coreutils
    libfontconfig1
    libfreetype6
    libx11-6
    libxcursor1
    libxinerama1
    libxrandr2
    libxi6
    libgl1
    libglu1-mesa
    libdbus-1-3
    libwayland-client0
    libxkbcommon0
    libpulse0
    zlib1g
  )

  if apt-cache show libasound2t64 >/dev/null 2>&1; then
    packages+=(libasound2t64)
  else
    packages+=(libasound2)
  fi

  local missing=()
  local package
  for package in "${packages[@]}"; do
    if ! dpkg-query -W -f='${Status}' "$package" 2>/dev/null | grep -q 'install ok installed'; then
      missing+=("$package")
    fi
  done

  if [[ ${#missing[@]} -eq 0 ]]; then
    log "OS prerequisites already installed"
    return 0
  fi

  log "Installing missing OS prerequisites: ${missing[*]}"
  "${SUDO[@]}" apt-get update -y
  "${SUDO[@]}" apt-get install -y --no-install-recommends "${missing[@]}"
}

ensure_dotnet_installer() {
  local installer="$CACHE_DIR/dotnet-install.sh"

  if [[ ! -s "$installer" ]]; then
    log "Downloading Microsoft's dotnet-install.sh"
    curl -fsSL --retry 4 --retry-delay 2 \
      https://dot.net/v1/dotnet-install.sh \
      -o "$installer"
    chmod +x "$installer"
  fi

  printf '%s\n' "$installer"
}

local_dotnet_has_major() {
  local major="$1"
  [[ -x "$DOTNET_INSTALL_DIR/dotnet" ]] || return 1
  "$DOTNET_INSTALL_DIR/dotnet" --list-sdks 2>/dev/null | grep -Eq "^${major}\."
}

install_dotnet() {
  # Match the repository and CI surface rather than assuming one SDK:
  #   Ashfall host/Core -> net8.0
  #   Core tests        -> net9.0
  #
  # global.json currently starts at 8.0.100 with latestMajor roll-forward.
  # Keeping both SDK families in the snapshot makes the environment robust to
  # project-specific selection and mirrors GitHub Actions more closely.
  local installer
  installer="$(ensure_dotnet_installer)"

  if ! local_dotnet_has_major 8; then
    log "Installing .NET 8 SDK into $DOTNET_INSTALL_DIR"
    "$installer" \
      --channel 8.0 \
      --quality GA \
      --install-dir "$DOTNET_INSTALL_DIR" \
      --no-path
  else
    log ".NET 8 SDK already available in Jules-local toolchain"
  fi

  if ! local_dotnet_has_major 9; then
    log "Installing .NET 9 SDK into $DOTNET_INSTALL_DIR"
    "$installer" \
      --channel 9.0 \
      --quality GA \
      --install-dir "$DOTNET_INSTALL_DIR" \
      --no-path
  else
    log ".NET 9 SDK already available in Jules-local toolchain"
  fi

  ln -sfn "$DOTNET_INSTALL_DIR/dotnet" "$LOCAL_BIN/dotnet"
  export DOTNET_ROOT="$DOTNET_INSTALL_DIR"
  export PATH="$LOCAL_BIN:$DOTNET_INSTALL_DIR:$PATH"

  command -v dotnet >/dev/null 2>&1 || die "dotnet is not available after installation."
}

install_godot() {
  local base="Godot_v${GODOT_VERSION}-stable_mono_linux_x86_64"
  local zip="$CACHE_DIR/${base}.zip"
  local url="https://github.com/godotengine/godot-builds/releases/download/${GODOT_VERSION}-stable/${base}.zip"

  # Prefer the deterministic Jules-local installation if it is already valid.
  if [[ -x "$LOCAL_BIN/godot" ]]; then
    local existing
    existing="$("$LOCAL_BIN/godot" --version 2>/dev/null || true)"
    if [[ "$existing" == "${GODOT_VERSION}"* && "$existing" == *mono* ]]; then
      log "Godot $existing already available"
      export PATH="$LOCAL_BIN:$PATH"
      return 0
    fi
  fi

  log "Installing Godot ${GODOT_VERSION} .NET/Mono"

  if [[ ! -s "$zip" ]]; then
    curl -fL --retry 4 --retry-delay 2 "$url" -o "$zip"
  fi

  unzip -tq "$zip" >/dev/null || die "Downloaded Godot archive failed integrity check: $zip"

  rm -rf "$GODOT_INSTALL_DIR"
  mkdir -p "$GODOT_INSTALL_DIR"
  unzip -q "$zip" -d "$GODOT_INSTALL_DIR"

  local godot_bin
  godot_bin="$(
    find "$GODOT_INSTALL_DIR" \
      -type f \
      -name 'Godot_v*-stable_mono_linux.x86_64' \
      -print \
      -quit
  )"

  [[ -n "$godot_bin" ]] || die "Godot executable not found after extraction."
  chmod +x "$godot_bin"

  # Wrapper instead of copying the executable: the Mono distribution contains
  # companion files beside the real Godot binary.
  cat > "$LOCAL_BIN/godot" <<EOF
#!/usr/bin/env bash
exec "$godot_bin" "\$@"
EOF
  chmod +x "$LOCAL_BIN/godot"
  ln -sfn "$LOCAL_BIN/godot" "$LOCAL_BIN/godot4"

  export PATH="$LOCAL_BIN:$PATH"

  local installed_version
  installed_version="$(godot --version 2>/dev/null || true)"
  [[ "$installed_version" == "${GODOT_VERSION}"* && "$installed_version" == *mono* ]] \
    || die "Installed Godot version is unexpected: '$installed_version'"
}

install_godot_export_templates() {
  [[ "$ASHFALL_SETUP_EXPORT_TEMPLATES" == "1" ]] || {
    log "Skipping optional Godot export templates"
    return 0
  }

  if [[ -d "$GODOT_TEMPLATE_DIR" ]] &&
     find "$GODOT_TEMPLATE_DIR" -mindepth 1 -print -quit | grep -q .; then
    log "Godot ${GODOT_VERSION} export templates already installed"
    return 0
  fi

  log "Installing Godot ${GODOT_VERSION} .NET export templates"

  local tpz="$CACHE_DIR/Godot_v${GODOT_VERSION}-stable_mono_export_templates.tpz"
  local url="https://github.com/godotengine/godot-builds/releases/download/${GODOT_VERSION}-stable/Godot_v${GODOT_VERSION}-stable_mono_export_templates.tpz"
  local tmp="$CACHE_DIR/export-templates-${GODOT_VERSION}"

  if [[ ! -s "$tpz" ]]; then
    curl -fL --retry 4 --retry-delay 2 "$url" -o "$tpz"
  fi

  unzip -tq "$tpz" >/dev/null || die "Downloaded export-template archive failed integrity check: $tpz"

  rm -rf "$tmp"
  mkdir -p "$tmp" "$GODOT_TEMPLATE_DIR"
  unzip -q "$tpz" -d "$tmp"

  if [[ -d "$tmp/templates" ]]; then
    cp -a "$tmp/templates/." "$GODOT_TEMPLATE_DIR/"
  else
    cp -a "$tmp/." "$GODOT_TEMPLATE_DIR/"
  fi
}

bootstrap_repo_for_jules() {
  log "Configuring repository for Jules"

  # The repository intentionally contains both:
  #   Assets/  -> Core source + authored StreamingAssets data
  #   assets/  -> Godot-native runtime assets
  # Their case distinction is semantically significant.
  git config --local core.ignorecase false

  local hooks_path
  hooks_path="$(git config --get core.hooksPath 2>/dev/null || true)"
  if [[ -n "$hooks_path" ]]; then
    log "Existing Git hook policy detected: core.hooksPath=$hooks_path"
  fi

  # Git LFS officially supports combining --local with --skip-repo.
  # --skip-repo installs LFS filters without trying to create hooks under
  # Jules' intentionally disabled hook path.
  git lfs install --local --skip-repo

  # Jules clones before this script runs, so hydrate any pointer files that
  # arrived in the checkout.
  log "Hydrating Git LFS objects"
  git lfs pull
  git lfs checkout

  # Do not call ./setup-repo.sh here; it intentionally installs repository
  # hooks for developer machines.
  if [[ -x scripts/ci/lfs-health-check.sh || -f scripts/ci/lfs-health-check.sh ]]; then
    log "Validating Git LFS materialization"
    bash scripts/ci/lfs-health-check.sh
  fi

  if [[ -d Assets/_Game ]]; then
    warn "Legacy Assets/_Game exists on this branch. Snapshot will not install or invoke Unity."
  fi
}

validate_repo_contracts() {
  [[ -f project.godot ]] || die "project.godot is missing."
  [[ -f Ashfall.csproj ]] || die "Ashfall.csproj is missing."
  [[ -f Ashfall.Core/Ashfall.Core.csproj ]] || die "Ashfall.Core/Ashfall.Core.csproj is missing."
  [[ -f Ashfall.Core.Tests/Ashfall.Core.Tests.csproj ]] || die "Ashfall.Core.Tests project is missing."
  [[ -f scripts/ci/run-godot-bounded.sh ]] || die "Bounded Godot runner is missing."
  [[ -f scripts/ci/run-gates.py ]] || die "Canonical gate runner is missing."
  [[ -f docs/ci/CI_GATE_MANIFEST.json ]] || die "CI gate manifest is missing."

  # Fail early if the manifest/quarantine/dependency graph itself is invalid.
  log "Validating canonical CI gate manifest"
  python3 scripts/ci/run-gates.py --tier fast --check-only
}

restore_build_and_warm() {
  log "Toolchain versions"
  git --version
  git lfs version
  dotnet --version
  dotnet --list-sdks
  godot --version

  log "Restoring active .NET projects"
  dotnet restore Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo
  dotnet restore Ashfall.csproj --nologo

  # Build both active compilation surfaces so the snapshot contains a warmed
  # NuGet cache and current Godot C# assembly. This also catches snapshot-time
  # source/toolchain incompatibility immediately.
  log "Building Core test assembly"
  dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
    --nologo \
    --no-restore

  log "Building Godot host"
  dotnet build Ashfall.csproj \
    --nologo \
    --no-restore

  # Use the repository's own bounded launcher. Raw `godot --import` can hang a
  # snapshot indefinitely on a pathological cold filesystem scan.
  log "Warming Godot resource/import cache"
  bash scripts/ci/run-godot-bounded.sh \
    --path "$APP_DIR" \
    --import
}

run_snapshot_verify() {
  [[ "$ASHFALL_SETUP_SNAPSHOT_VERIFY" == "1" ]] || {
    log "Skipping lightweight snapshot verification"
    return 0
  }

  log "Running lightweight canonical snapshot verification"

  # The gate runner adds build_godot_host as a dependency and verifies the
  # expected PASS tokens, preventing stale-assembly false positives.
  python3 scripts/ci/run-gates.py \
    --gate data_integrity,asset_registry \
    --no-fail-fast \
    --report-json build/reports/jules-snapshot-gates.json \
    --fail-artifact build/reports/JULES_SNAPSHOT_FAILURE_REPORT.md
}

run_full_verify() {
  [[ "$ASHFALL_SETUP_FULL_VERIFY" == "1" ]] || {
    log "Skipping optional full verification"
    return 0
  }

  log "Running full current CI fast-tier verification"
  python3 scripts/ci/run-gates.py \
    --tier fast \
    --no-fail-fast \
    --report-json build/reports/jules-fast-gates.json \
    --fail-artifact build/reports/JULES_FAST_GATE_FAILURE_REPORT.md

  log "Running full Core xUnit regression gate"
  python3 scripts/ci/run-gates.py \
    --gate test_core_suite \
    --report-json build/reports/jules-core-tests.json \
    --fail-artifact build/reports/JULES_CORE_TEST_FAILURE_REPORT.md
}

print_summary() {
  local head
  head="$(git rev-parse --short=12 HEAD 2>/dev/null || printf 'unknown')"

  log "Setup complete — ready for Jules Run and Snapshot"
  printf '%s\n' \
    "Repo:              $APP_DIR" \
    "HEAD:              $head" \
    "dotnet:            $(command -v dotnet)" \
    "godot:             $(command -v godot)" \
    "Godot version:     $(godot --version 2>/dev/null || true)" \
    "Git hooks:         Jules policy intentionally preserved" \
    "LFS:               hydrated and checked" \
    "Godot cache:       warmed with bounded importer" \
    "Snapshot verify:   $ASHFALL_SETUP_SNAPSHOT_VERIFY" \
    "Full verify:       $ASHFALL_SETUP_FULL_VERIFY" \
    "Export templates:  $ASHFALL_SETUP_EXPORT_TEMPLATES" \
    "Unity:             not installed or invoked"
}

main() {
  log "ASHFALL Jules environment setup v3"

  install_os_prereqs
  install_dotnet
  install_godot
  install_godot_export_templates
  persist_profile_block

  bootstrap_repo_for_jules
  validate_repo_contracts
  restore_build_and_warm
  run_snapshot_verify
  run_full_verify
  print_summary
}

main "$@"
