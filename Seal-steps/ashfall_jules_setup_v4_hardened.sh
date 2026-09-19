#!/usr/bin/env bash
# ASHFALL — Google Jules setup / snapshot bootstrap (v4)
# Repository-audited: 2026-09-19
#
# Purpose
# -------
# Prepare a Google Jules Ubuntu VM for ASHFALL's canonical Godot/.NET stack,
# warm expensive caches, and leave enough diagnostics to explain any failure.
#
# Canonical repository targets at audit time:
#   Godot host:        Godot 4.7.1 .NET / Ashfall.csproj / net8.0
#   Engine-free Core:  Ashfall.Core/Ashfall.Core.csproj / net8.0
#   Core tests:        Ashfall.Core.Tests/Ashfall.Core.Tests.csproj / net9.0
#   Authored data:     Assets/StreamingAssets/Data/
#   Godot assets:      assets/
#
# Unity is retired. This script never installs or invokes Unity.
#
# Jules notes
# -----------
# * Do NOT assume the repository lives at /app. Jules does not document a
#   stable clone path. The script discovers the current Git worktree.
# * Jules/host tooling may deliberately disable Git hooks through
#   core.hooksPath. Keep that policy intact.
# * Git LFS is configured with --skip-repo so filters are available without
#   writing hooks.
#
# Defaults are snapshot-oriented:
#   - hydrate LFS
#   - install/verify .NET 8 + 9
#   - install exact Godot 4.7.1 Mono
#   - restore/build both active compilation surfaces
#   - warm Godot imports
#   - run two high-signal host gates
#
# Optional environment variables:
#   ASHFALL_JULES_VERIFY=0
#       Skip the lightweight data-integrity + asset-registry verification.
#
#   ASHFALL_JULES_FULL_VERIFY=1
#       Run the repository's full current fast tier and full Core xUnit gate.
#
#   ASHFALL_JULES_EXPORT_TEMPLATES=1
#       Install ~1.2 GB Godot Mono export templates. Disabled by default.
#
#   ASHFALL_JULES_LFS_HEALTH=0
#       Skip the repository LFS health check after hydration.
#
#   ASHFALL_JULES_IMPORT_RETRY=0
#       Do not retry a failed Godot import after clearing the ignored .godot/
#       cache.
#
#   APP_DIR=/some/path
#       Optional explicit repository location. Normally unnecessary.
#
# Failure diagnostics:
#   build/reports/jules-setup/setup.log
#   build/reports/jules-setup/diagnostics.txt
#   build/reports/jules-setup/godot-import*.log
#
# Recommended Jules "Initial Setup":
#   bash ./scripts/jules/setup.sh
# or paste this script directly into the Initial Setup editor.

set -Eeuo pipefail
shopt -s inherit_errexit 2>/dev/null || true

export DEBIAN_FRONTEND=noninteractive
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export NUGET_XMLDOC_MODE=skip
export NUGET_CERT_REVOCATION_MODE=offline

GODOT_VERSION="${GODOT_VERSION:-4.7.1}"
LOCAL_BIN="${LOCAL_BIN:-$HOME/.local/bin}"
LOCAL_DOTNET="${LOCAL_DOTNET:-$HOME/.dotnet}"
GODOT_INSTALL_DIR="${GODOT_INSTALL_DIR:-$HOME/.local/share/godot-${GODOT_VERSION}-mono}"
GODOT_TEMPLATE_DIR="${GODOT_TEMPLATE_DIR:-$HOME/.local/share/godot/export_templates/${GODOT_VERSION}.stable.mono}"
CACHE_DIR="${XDG_CACHE_HOME:-$HOME/.cache}/ashfall-jules-setup"

ASHFALL_JULES_VERIFY="${ASHFALL_JULES_VERIFY:-1}"
ASHFALL_JULES_FULL_VERIFY="${ASHFALL_JULES_FULL_VERIFY:-0}"
ASHFALL_JULES_EXPORT_TEMPLATES="${ASHFALL_JULES_EXPORT_TEMPLATES:-0}"
ASHFALL_JULES_LFS_HEALTH="${ASHFALL_JULES_LFS_HEALTH:-1}"
ASHFALL_JULES_IMPORT_RETRY="${ASHFALL_JULES_IMPORT_RETRY:-1}"

# IMPORTANT: all status logging goes to stderr. This prevents helper functions
# used inside command substitutions from contaminating captured values.
log()  { printf '\n[ashfall-jules] %s\n' "$*" >&2; }
warn() { printf '\n[ashfall-jules] WARN: %s\n' "$*" >&2; }
die()  { printf '\n[ashfall-jules] ERROR: %s\n' "$*" >&2; exit 1; }

CURRENT_STAGE="startup"
REPO_ROOT=""
REPORT_DIR=""
SETUP_LOG=""
DIAG_FILE=""
DOTNET_MODE="unknown"
GODOT_BIN=""

set_stage() {
  CURRENT_STAGE="$1"
  log "STAGE: $CURRENT_STAGE"
}

retry() {
  local attempts="$1"
  shift
  local n=1
  local delay=2

  until "$@"; do
    local rc=$?
    if (( n >= attempts )); then
      return "$rc"
    fi
    warn "Attempt $n/$attempts failed (exit $rc): $*"
    sleep "$delay"
    n=$((n + 1))
    delay=$((delay * 2))
  done
}

discover_repo() {
  set_stage "discover repository"

  if [[ -n "${APP_DIR:-}" ]]; then
    [[ -d "$APP_DIR" ]] || die "APP_DIR does not exist: $APP_DIR"
    cd "$APP_DIR"
  fi

  REPO_ROOT="$(git rev-parse --show-toplevel 2>/dev/null || true)"

  if [[ -z "$REPO_ROOT" ]]; then
    # A setup editor may invoke the script one directory away from the worktree.
    # Check a few common, non-authoritative fallbacks without assuming /app.
    local candidate
    for candidate in "$PWD" "$PWD/repo" /app /workspace /workspace/repo; do
      if [[ -d "$candidate/.git" ]]; then
        REPO_ROOT="$candidate"
        break
      fi
    done
  fi

  [[ -n "$REPO_ROOT" && -d "$REPO_ROOT/.git" ]] \
    || die "Could not locate the Jules Git checkout. Run the setup from the cloned repository or set APP_DIR."

  cd "$REPO_ROOT"

  REPORT_DIR="$REPO_ROOT/build/reports/jules-setup"
  SETUP_LOG="$REPORT_DIR/setup.log"
  DIAG_FILE="$REPORT_DIR/diagnostics.txt"

  mkdir -p "$LOCAL_BIN" "$LOCAL_DOTNET" "$CACHE_DIR" "$REPORT_DIR"

  # From this point onward, preserve the complete console transcript.
  exec > >(tee -a "$SETUP_LOG") 2>&1

  log "Repository root: $REPO_ROOT"
  log "HEAD: $(git rev-parse --short=12 HEAD 2>/dev/null || printf unknown)"
}

diagnostics() {
  [[ -n "$REPORT_DIR" ]] || return 0

  {
    printf 'ASHFALL Jules setup diagnostics\n'
    printf '================================\n'
    printf 'stage: %s\n' "$CURRENT_STAGE"
    printf 'timestamp_utc: %s\n' "$(date -u +%Y-%m-%dT%H:%M:%SZ 2>/dev/null || true)"
    printf 'repo_root: %s\n' "$REPO_ROOT"
    printf 'pwd: %s\n' "$PWD"
    printf 'user: %s\n' "$(id 2>/dev/null || true)"
    printf 'uname: %s\n' "$(uname -a 2>/dev/null || true)"
    printf 'shell: %s\n' "${BASH_VERSION:-unknown}"
    printf '\n-- git --\n'
    git --version 2>&1 || true
    git status --short --branch 2>&1 | head -n 80 || true
    printf 'core.hooksPath=%s\n' "$(git config --get core.hooksPath 2>/dev/null || printf '<unset>')"
    printf 'core.ignorecase=%s\n' "$(git config --get core.ignorecase 2>/dev/null || printf '<unset>')"
    printf '\n-- disk/memory --\n'
    df -h "$REPO_ROOT" 2>&1 || true
    free -h 2>&1 || true
    printf '\n-- tool lookup --\n'
    command -v curl 2>&1 || true
    command -v git-lfs 2>&1 || true
    command -v dotnet 2>&1 || true
    command -v godot 2>&1 || true
    printf '\n-- dotnet --\n'
    dotnet --info 2>&1 || true
    dotnet --list-sdks 2>&1 || true
    dotnet --list-runtimes 2>&1 || true
    printf '\n-- godot --\n'
    godot --version 2>&1 || true
    if [[ -n "$GODOT_BIN" && -x "$GODOT_BIN" ]]; then
      printf '\n-- godot dynamic libraries --\n'
      ldd "$GODOT_BIN" 2>&1 || true
    fi
    printf '\n-- LFS --\n'
    git lfs version 2>&1 || true
    git lfs env 2>&1 | sed -n '1,80p' || true
  } > "$DIAG_FILE" 2>&1

  cat "$DIAG_FILE" >&2 || true
}

on_error() {
  local rc=$?
  local line="${BASH_LINENO[0]:-${LINENO:-unknown}}"
  local cmd="${BASH_COMMAND:-unknown}"

  set +e
  printf '\n[ashfall-jules] FATAL: stage="%s" exit=%s line=%s\n' "$CURRENT_STAGE" "$rc" "$line" >&2
  printf '[ashfall-jules] command: %s\n' "$cmd" >&2
  diagnostics
  printf '\n[ashfall-jules] Full log: %s\n' "${SETUP_LOG:-not-initialized}" >&2
  exit "$rc"
}

trap on_error ERR

detect_privilege() {
  if [[ "${EUID:-$(id -u)}" -eq 0 ]]; then
    SUDO=()
    CAN_APT=1
  elif command -v sudo >/dev/null 2>&1 && sudo -n true >/dev/null 2>&1; then
    SUDO=(sudo -n)
    CAN_APT=1
  else
    SUDO=()
    CAN_APT=0
  fi
}

apt_update() {
  [[ "$CAN_APT" == "1" ]] || return 1
  retry 3 "${SUDO[@]}" apt-get \
    -o Acquire::Retries=3 \
    -o Dpkg::Use-Pty=0 \
    update
}

apt_install() {
  [[ "$CAN_APT" == "1" ]] || return 1
  retry 3 "${SUDO[@]}" apt-get \
    -o Acquire::Retries=3 \
    -o Dpkg::Use-Pty=0 \
    install -y --no-install-recommends "$@"
}

install_base_prereqs() {
  set_stage "base OS prerequisites"
  detect_privilege

  if ! command -v apt-get >/dev/null 2>&1; then
    warn "apt-get is unavailable. Continuing with preinstalled tools."
    return 0
  fi

  if [[ "$CAN_APT" != "1" ]]; then
    warn "No passwordless root/sudo. Continuing with user-local fallbacks where possible."
    return 0
  fi

  apt_update

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
    xz-utils
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
  elif apt-cache show libasound2 >/dev/null 2>&1; then
    packages+=(libasound2)
  else
    warn "Neither libasound2t64 nor libasound2 is visible in APT; Godot headless may still work."
  fi

  apt_install "${packages[@]}"
}

has_sdk_major() {
  local major="$1"
  command -v dotnet >/dev/null 2>&1 || return 1
  dotnet --list-sdks 2>/dev/null | grep -Eq "^${major}\."
}

has_runtime_major() {
  local major="$1"
  command -v dotnet >/dev/null 2>&1 || return 1
  dotnet --list-runtimes 2>/dev/null | grep -Eq "Microsoft\.NETCore\.App ${major}\."
}

configure_system_dotnet() {
  # Do not force DOTNET_ROOT for distro packages. The system muxer knows its
  # own installation root; pointing DOTNET_ROOT at an empty ~/.dotnet can make
  # a perfectly valid system installation fail.
  unset DOTNET_ROOT || true

  # A prior failed local bootstrap may have left this symlink behind. Remove
  # only our own user-local shim so adding ~/.local/bin for Godot later cannot
  # unexpectedly switch a healthy system installation back to stale bytes.
  if [[ -L "$LOCAL_BIN/dotnet" ]]; then
    local target
    target="$(readlink -f "$LOCAL_BIN/dotnet" 2>/dev/null || true)"
    if [[ "$target" == "$LOCAL_DOTNET/dotnet" ]]; then
      rm -f "$LOCAL_BIN/dotnet"
      hash -r 2>/dev/null || true
    fi
  fi

  DOTNET_MODE="system"
}

ensure_dotnet_installer() {
  # Deliberately sets a global variable instead of returning the path through
  # stdout. v3 accidentally mixed a log line into a command substitution here.
  DOTNET_INSTALLER="$CACHE_DIR/dotnet-install.sh"

  if [[ ! -s "$DOTNET_INSTALLER" ]]; then
    log "Downloading dotnet-install.sh fallback"
    retry 3 curl -fsSL \
      --connect-timeout 20 \
      --max-time 180 \
      https://dot.net/v1/dotnet-install.sh \
      -o "$DOTNET_INSTALLER"
    chmod +x "$DOTNET_INSTALLER"
  fi
}

activate_local_dotnet() {
  ln -sfn "$LOCAL_DOTNET/dotnet" "$LOCAL_BIN/dotnet"
  export DOTNET_ROOT="$LOCAL_DOTNET"
  export PATH="$LOCAL_BIN:$LOCAL_DOTNET:$PATH"
  DOTNET_MODE="user-local"
}

install_dotnet_local_fallback() {
  set_stage ".NET local fallback"

  ensure_dotnet_installer

  log "Installing .NET 8 SDK into $LOCAL_DOTNET"
  "$DOTNET_INSTALLER" \
    --channel 8.0 \
    --quality GA \
    --install-dir "$LOCAL_DOTNET" \
    --no-path

  log "Installing .NET 9 SDK into $LOCAL_DOTNET"
  "$DOTNET_INSTALLER" \
    --channel 9.0 \
    --quality GA \
    --install-dir "$LOCAL_DOTNET" \
    --no-path

  activate_local_dotnet
}

install_dotnet() {
  set_stage ".NET SDKs"

  # If the image already has exactly what ASHFALL needs, use it.
  if has_sdk_major 8 && has_sdk_major 9 && has_runtime_major 8 && has_runtime_major 9; then
    configure_system_dotnet
    log "Existing .NET 8 + 9 SDK/runtime set is sufficient"
    return 0
  fi

  # Jules currently runs Ubuntu. Prefer distro/backports packages because they
  # are more reliable in restricted agent networks than bootstrap downloads.
  if command -v apt-get >/dev/null 2>&1 && [[ "$CAN_APT" == "1" ]]; then
    apt_update

    if ! has_sdk_major 8; then
      if apt-cache show dotnet-sdk-8.0 >/dev/null 2>&1; then
        log "Installing .NET 8 SDK from Ubuntu packages"
        apt_install dotnet-sdk-8.0
      fi
    fi

    if ! has_sdk_major 9; then
      # Ubuntu 24.04 exposes .NET 9 through the Ubuntu .NET backports feed.
      if apt-cache show dotnet-sdk-9.0 >/dev/null 2>&1; then
        log "Installing .NET 9 SDK from configured Ubuntu packages"
        apt_install dotnet-sdk-9.0
      else
        log ".NET 9 package not currently visible; trying Ubuntu .NET backports"
        if ! command -v add-apt-repository >/dev/null 2>&1; then
          apt_install software-properties-common || true
        fi

        if command -v add-apt-repository >/dev/null 2>&1; then
          if retry 2 "${SUDO[@]}" add-apt-repository -y ppa:dotnet/backports; then
            apt_update
            apt_install dotnet-sdk-9.0 || true
          else
            warn "Could not enable Ubuntu .NET backports."
          fi
        fi
      fi
    fi

    if has_sdk_major 8 && has_sdk_major 9 && has_runtime_major 8 && has_runtime_major 9; then
      configure_system_dotnet
      log "System .NET installation is complete"
      return 0
    fi
  fi

  warn "APT did not provide the complete .NET 8 + 9 set; using user-local fallback."
  install_dotnet_local_fallback

  has_sdk_major 8 || die ".NET 8 SDK is still missing after fallback installation."
  has_sdk_major 9 || die ".NET 9 SDK is still missing after fallback installation."
  has_runtime_major 8 || die ".NET 8 runtime is still missing after fallback installation."
  has_runtime_major 9 || die ".NET 9 runtime is still missing after fallback installation."
}

download_file() {
  local output="$1"
  shift
  local url

  for url in "$@"; do
    log "Download: $url"
    rm -f "$output.part"
    if retry 3 curl -fL \
      --connect-timeout 20 \
      --max-time 1800 \
      --retry-all-errors \
      "$url" \
      -o "$output.part"; then
      mv "$output.part" "$output"
      return 0
    fi
    warn "Download failed from: $url"
  done

  rm -f "$output.part"
  return 1
}

install_godot() {
  set_stage "Godot ${GODOT_VERSION} Mono"

  local base="Godot_v${GODOT_VERSION}-stable_mono_linux_x86_64"
  local zip="$CACHE_DIR/${base}.zip"
  local github_url="https://github.com/godotengine/godot-builds/releases/download/${GODOT_VERSION}-stable/${base}.zip"
  local mirror_url="https://downloads.sourceforge.net/project/godot-engine.mirror/${GODOT_VERSION}-stable/${base}.zip"

  # Accept an already-correct executable, regardless of whether it came from a
  # previous snapshot run or from the base VM.
  if command -v godot >/dev/null 2>&1; then
    local existing
    existing="$(godot --version 2>/dev/null || true)"
    if [[ "$existing" == "${GODOT_VERSION}"* && "$existing" == *mono* ]]; then
      GODOT_BIN="$(readlink -f "$(command -v godot)" 2>/dev/null || command -v godot)"
      log "Godot already available: $existing"
      return 0
    fi
  fi

  if [[ ! -s "$zip" ]] || ! unzip -tq "$zip" >/dev/null 2>&1; then
    rm -f "$zip"
    log "Downloading Godot ${GODOT_VERSION} Mono"
    download_file "$zip" "$github_url" "$mirror_url" \
      || die "Could not download Godot ${GODOT_VERSION} Mono from GitHub or the fallback mirror."
  fi

  unzip -tq "$zip" >/dev/null \
    || die "Godot archive failed ZIP integrity validation: $zip"

  rm -rf "$GODOT_INSTALL_DIR"
  mkdir -p "$GODOT_INSTALL_DIR"
  unzip -q "$zip" -d "$GODOT_INSTALL_DIR"

  GODOT_BIN="$(
    find "$GODOT_INSTALL_DIR" \
      -type f \
      -name 'Godot_v*-stable_mono_linux.x86_64' \
      -print \
      -quit
  )"

  [[ -n "$GODOT_BIN" && -f "$GODOT_BIN" ]] \
    || die "Could not find the Godot Mono executable after extraction."

  chmod +x "$GODOT_BIN"

  # Keep the real executable in its extracted Mono distribution so its
  # companion files remain beside it. The wrapper only delegates.
  cat > "$LOCAL_BIN/godot" <<EOF
#!/usr/bin/env bash
exec "$GODOT_BIN" "\$@"
EOF
  chmod +x "$LOCAL_BIN/godot"
  ln -sfn "$LOCAL_BIN/godot" "$LOCAL_BIN/godot4"
  export PATH="$LOCAL_BIN:$PATH"

  # If privileged, also expose Godot in a path that future non-login Jules
  # shells reliably inherit.
  if [[ "$CAN_APT" == "1" && -d /usr/local/bin ]]; then
    "${SUDO[@]}" ln -sfn "$LOCAL_BIN/godot" /usr/local/bin/godot || true
    "${SUDO[@]}" ln -sfn "$LOCAL_BIN/godot" /usr/local/bin/godot4 || true
  fi

  local missing_libs
  missing_libs="$(ldd "$GODOT_BIN" 2>/dev/null | awk '/not found/{print}' || true)"
  if [[ -n "$missing_libs" ]]; then
    printf '%s\n' "$missing_libs" >&2
    die "Godot still has missing native libraries. See diagnostics above."
  fi

  local version
  version="$(godot --version 2>/dev/null || true)"
  [[ "$version" == "${GODOT_VERSION}"* && "$version" == *mono* ]] \
    || die "Unexpected Godot executable/version after installation: '$version'"

  log "Installed Godot: $version"
}

install_export_templates() {
  [[ "$ASHFALL_JULES_EXPORT_TEMPLATES" == "1" ]] || {
    log "Export templates disabled for the snapshot"
    return 0
  }

  set_stage "Godot export templates"

  if [[ -d "$GODOT_TEMPLATE_DIR" ]] &&
     find "$GODOT_TEMPLATE_DIR" -mindepth 1 -print -quit | grep -q .; then
    log "Godot ${GODOT_VERSION} export templates already present"
    return 0
  fi

  local name="Godot_v${GODOT_VERSION}-stable_mono_export_templates.tpz"
  local tpz="$CACHE_DIR/$name"
  local github_url="https://github.com/godotengine/godot-builds/releases/download/${GODOT_VERSION}-stable/${name}"
  local mirror_url="https://downloads.sourceforge.net/project/godot-engine.mirror/${GODOT_VERSION}-stable/${name}"
  local tmp="$CACHE_DIR/export-templates-${GODOT_VERSION}"

  if [[ ! -s "$tpz" ]] || ! unzip -tq "$tpz" >/dev/null 2>&1; then
    rm -f "$tpz"
    download_file "$tpz" "$github_url" "$mirror_url" \
      || die "Could not download Godot Mono export templates."
  fi

  rm -rf "$tmp"
  mkdir -p "$tmp" "$GODOT_TEMPLATE_DIR"
  unzip -q "$tpz" -d "$tmp"

  if [[ -d "$tmp/templates" ]]; then
    cp -a "$tmp/templates/." "$GODOT_TEMPLATE_DIR/"
  else
    cp -a "$tmp/." "$GODOT_TEMPLATE_DIR/"
  fi
}

persist_user_path() {
  set_stage "persist user tool paths"

  local begin="# >>> ASHFALL Jules toolchain >>>"
  local end="# <<< ASHFALL Jules toolchain <<<"
  local file

  for file in "$HOME/.profile" "$HOME/.bashrc"; do
    touch "$file"
    if grep -Fq "$begin" "$file" 2>/dev/null; then
      continue
    fi

    {
      printf '\n%s\n' "$begin"
      printf 'export PATH="%s:$PATH"\n' "$LOCAL_BIN"
      if [[ "$DOTNET_MODE" == "user-local" ]]; then
        printf 'export DOTNET_ROOT="%s"\n' "$LOCAL_DOTNET"
        printf 'export PATH="%s:%s:$PATH"\n' "$LOCAL_BIN" "$LOCAL_DOTNET"
      fi
      printf '%s\n' "$end"
    } >> "$file"
  done
}

hydrate_lfs() {
  set_stage "Git LFS hydration"

  git config --local core.ignorecase false

  command -v git-lfs >/dev/null 2>&1 \
    || die "git-lfs is unavailable after prerequisite setup."

  # Preserve Jules' hook sandbox. Official Git LFS supports --local and
  # --skip-repo together specifically for hook-disabled environments.
  git lfs install --local --skip-repo

  log "Materializing LFS objects for the checked-out revision"
  retry 3 git lfs pull
  git lfs checkout

  if [[ "$ASHFALL_JULES_LFS_HEALTH" == "1" && -f scripts/ci/lfs-health-check.sh ]]; then
    log "Running repository LFS health check"
    bash scripts/ci/lfs-health-check.sh
  fi
}

validate_repository_shape() {
  set_stage "repository contract checks"

  local required=(
    project.godot
    Ashfall.csproj
    Ashfall.Core/Ashfall.Core.csproj
    Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    Directory.Packages.props
    global.json
    scripts/ci/run-godot-bounded.sh
    scripts/ci/run-gates.py
    docs/ci/CI_GATE_MANIFEST.json
    Assets/StreamingAssets/Data
    Assets/Ashfall.Core
    assets
  )

  local path
  for path in "${required[@]}"; do
    [[ -e "$path" ]] || die "Required repository path is missing: $path"
  done

  if [[ -d Assets/_Game ]]; then
    warn "Assets/_Game exists on this revision, but Unity remains forbidden by active AGENTS.md authority."
  fi

  # Validate JSON syntax without invoking the policy runner yet.
  python3 -m json.tool global.json >/dev/null
  python3 -m json.tool docs/ci/CI_GATE_MANIFEST.json >/dev/null
}

print_environment_summary() {
  set_stage "environment summary"

  if [[ -r /opt/environment_summary.sh ]]; then
    # Jules explicitly documents this helper. It is useful in the snapshot log
    # but not required for correctness.
    set +x
    . /opt/environment_summary.sh || true
  fi

  printf '\n--- ASHFALL required toolchain ---\n'
  git --version
  git lfs version
  dotnet --version
  dotnet --list-sdks
  dotnet --list-runtimes
  godot --version
}

restore_build() {
  set_stage "NuGet restore"

  retry 2 dotnet restore Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo
  retry 2 dotnet restore Ashfall.csproj --nologo

  set_stage "build Core test assembly"
  dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
    --nologo \
    --no-restore

  set_stage "build Godot host"
  dotnet build Ashfall.csproj \
    --nologo \
    --no-restore
}

run_godot_import_once() {
  local logfile="$1"
  bash scripts/ci/run-godot-bounded.sh \
    --path "$REPO_ROOT" \
    --import 2>&1 | tee "$logfile"
}

warm_godot_cache() {
  set_stage "Godot import/cache warmup"

  local first="$REPORT_DIR/godot-import.log"
  local second="$REPORT_DIR/godot-import-retry.log"

  if run_godot_import_once "$first"; then
    log "Godot import completed successfully"
    return 0
  fi

  if [[ "$ASHFALL_JULES_IMPORT_RETRY" != "1" ]]; then
    die "Godot import failed. Retry is disabled; see $first"
  fi

  warn "Initial Godot import failed. Clearing only the ignored .godot/ cache and retrying once."
  rm -rf "$REPO_ROOT/.godot"

  if run_godot_import_once "$second"; then
    log "Godot import succeeded on clean-cache retry"
    return 0
  fi

  die "Godot import failed twice. See $first and $second"
}

lightweight_verify() {
  [[ "$ASHFALL_JULES_VERIFY" == "1" ]] || {
    log "Lightweight snapshot verification disabled"
    return 0
  }

  set_stage "lightweight canonical verification"

  mkdir -p "$REPO_ROOT/build/reports"

  # These are high-signal and relatively bounded. The canonical gate runner
  # automatically adds the current-source Godot host build prerequisite,
  # preventing stale-assembly false positives.
  python3 scripts/ci/run-gates.py \
    --gate data_integrity,asset_registry \
    --no-fail-fast \
    --report-json build/reports/jules-snapshot-gates.json \
    --fail-artifact build/reports/JULES_SNAPSHOT_FAILURE_REPORT.md
}

full_verify() {
  [[ "$ASHFALL_JULES_FULL_VERIFY" == "1" ]] || {
    log "Full verification disabled for snapshot setup"
    return 0
  }

  set_stage "full fast-tier verification"

  python3 scripts/ci/run-gates.py \
    --tier fast \
    --no-fail-fast \
    --report-json build/reports/jules-fast-gates.json \
    --fail-artifact build/reports/JULES_FAST_GATE_FAILURE_REPORT.md

  set_stage "full Core xUnit regression"

  python3 scripts/ci/run-gates.py \
    --gate test_core_suite \
    --report-json build/reports/jules-core-tests.json \
    --fail-artifact build/reports/JULES_CORE_TEST_FAILURE_REPORT.md
}

final_sanity() {
  set_stage "final snapshot sanity"

  # Verify that future agents will resolve the intended programs.
  command -v dotnet >/dev/null 2>&1 || die "dotnet disappeared from PATH."
  command -v godot >/dev/null 2>&1 || die "godot disappeared from PATH."

  has_sdk_major 8 || die ".NET 8 SDK missing at final sanity check."
  has_sdk_major 9 || die ".NET 9 SDK missing at final sanity check."
  has_runtime_major 8 || die ".NET 8 runtime missing at final sanity check."
  has_runtime_major 9 || die ".NET 9 runtime missing at final sanity check."

  local gv
  gv="$(godot --version)"
  [[ "$gv" == "${GODOT_VERSION}"* && "$gv" == *mono* ]] \
    || die "Final Godot version check failed: $gv"

  diagnostics

  printf '\n============================================================\n'
  printf ' ASHFALL JULES SNAPSHOT SETUP: SUCCESS\n'
  printf '============================================================\n'
  printf 'Repo:              %s\n' "$REPO_ROOT"
  printf 'HEAD:              %s\n' "$(git rev-parse --short=12 HEAD)"
  printf 'dotnet mode:       %s\n' "$DOTNET_MODE"
  printf 'dotnet selected:   %s\n' "$(dotnet --version)"
  printf 'Godot:             %s\n' "$gv"
  printf 'LFS health check:  %s\n' "$ASHFALL_JULES_LFS_HEALTH"
  printf 'Snapshot verify:   %s\n' "$ASHFALL_JULES_VERIFY"
  printf 'Full verify:       %s\n' "$ASHFALL_JULES_FULL_VERIFY"
  printf 'Export templates:  %s\n' "$ASHFALL_JULES_EXPORT_TEMPLATES"
  printf 'Setup log:         %s\n' "$SETUP_LOG"
  printf 'Diagnostics:       %s\n' "$DIAG_FILE"
  printf 'Unity:             not installed or invoked\n'
}

main() {
  discover_repo
  install_base_prereqs
  install_dotnet
  install_godot
  install_export_templates
  persist_user_path
  hydrate_lfs
  validate_repository_shape
  print_environment_summary
  restore_build
  warm_godot_cache
  lightweight_verify
  full_verify
  final_sanity
}

main "$@"
