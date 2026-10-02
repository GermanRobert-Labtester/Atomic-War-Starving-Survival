//! Port of `tools/gotools/pkg/monitor/{policy,report,git,size,compile}.go`.
//!
//! `monitor-size` gates tracked-blob growth between a base ref and HEAD plus
//! any newly added oversize Markdown; `monitor-compile` intersects the
//! MSBuild-evaluated `Compile` item sets of the policy's production/test
//! projects (plus any auto-discovered standalone `.csproj`) against git-tracked
//! `.cs` files and classifies every tracked path. Both are read-only.
//!
//! Parity notes:
//! * Violations carry a Go `float64` `confidence`; it is emitted through
//!   [`crate::jsonout::GoFloat`] so `1.0` renders as `1`, as `encoding/json`
//!   does.
//! * `Report.metrics` is a concrete serializable struct per monitor so field
//!   order matches the Go structs (a `serde_json::Value` intermediary would
//!   re-sort keys).
//! * Go iterates the HEAD blob map when emitting new-Markdown violations, so
//!   its order is non-deterministic; this port iterates sorted paths. git tree
//!   and `ls-files` order (used for the compile classification) are reproduced
//!   as git emits them.
//! * `git` is invoked with `Command::current_dir(root)`; `dotnet msbuild
//!   <abs> -getItem:Compile -nologo` is the MSBuild authority, never a
//!   re-implementation of glob semantics.

use crate::gotime;
use crate::jsonout::{self, GoFloat};
use serde::{Deserialize, Serialize};
use std::collections::{HashMap, HashSet};
use std::path::{Component, Path, PathBuf};

/// Schema version stamped on every monitor report.
pub const REPORT_SCHEMA_VERSION: &str = "1.0";

pub const SEVERITY_ERROR: &str = "error";
pub const SEVERITY_WARNING: &str = "warning";
pub const SEVERITY_INFO: &str = "info";

// ---------------------------------------------------------------------------
// Policy
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Default, Deserialize)]
pub struct SizePolicy {
    #[serde(default)]
    pub new_markdown_max_bytes: i64,
    #[serde(default)]
    pub pr_growth_fail_bytes: i64,
    #[serde(default)]
    pub pr_growth_warn_bytes: i64,
    #[serde(default)]
    pub markdown_allowlist: Vec<String>,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct KnownCompilePath {
    #[serde(default)]
    pub path: String,
    #[serde(default)]
    pub classification: String,
    #[serde(default)]
    pub severity: String,
    #[serde(default)]
    pub reason: String,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct CompilePolicy {
    #[serde(default)]
    pub production_projects: Vec<String>,
    #[serde(default)]
    pub test_projects: Vec<String>,
    #[serde(default)]
    pub test_root_prefix: String,
    #[serde(default)]
    pub known_paths: Vec<KnownCompilePath>,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct Policy {
    /// Documented schema version; deserialized but not otherwise read.
    #[allow(dead_code)]
    #[serde(default)]
    pub schema_version: String,
    #[serde(default)]
    pub size: SizePolicy,
    #[serde(default)]
    pub compile: CompilePolicy,
}

impl CompilePolicy {
    pub fn known_path(&self, path: &str) -> Option<&KnownCompilePath> {
        self.known_paths.iter().find(|k| k.path == path)
    }
}

/// Port of `LoadPolicy` — a missing or malformed policy fails closed.
pub fn load_policy(path: &str) -> Result<Policy, String> {
    let data = std::fs::read(path).map_err(|e| format!("read monitoring policy {}: {}", path, e))?;
    let p: Policy =
        serde_json::from_slice(&data).map_err(|e| format!("parse monitoring policy {}: {}", path, e))?;
    if p.size.new_markdown_max_bytes <= 0 || p.size.pr_growth_fail_bytes <= 0 {
        return Err(format!(
            "monitoring policy {}: size thresholds must be positive",
            path
        ));
    }
    if p.compile.production_projects.is_empty() || p.compile.test_projects.is_empty() {
        return Err(format!(
            "monitoring policy {}: compile.production_projects and compile.test_projects must be non-empty",
            path
        ));
    }
    Ok(p)
}

// ---------------------------------------------------------------------------
// Report
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Serialize)]
pub struct Violation {
    pub severity: String,
    pub confidence: GoFloat,
    pub category: String,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub path: Option<String>,
    pub message: String,
}

impl Violation {
    fn new(severity: &str, confidence: f64, category: &str, path: Option<&str>, message: String) -> Violation {
        Violation {
            severity: severity.to_string(),
            confidence: GoFloat(confidence),
            category: category.to_string(),
            path: path.map(|p| p.to_string()),
            message,
        }
    }
}

#[derive(Debug, Clone, Serialize)]
pub struct Report<M: Serialize> {
    pub schema_version: String,
    pub monitor: String,
    pub commit_sha: String,
    pub passed: bool,
    pub metrics: M,
    pub violations: Vec<Violation>,
}

impl<M: Serialize> Report<M> {
    /// Port of `NewReport` — `passed` is the absence of any `error` violation.
    pub fn new(monitor_name: &str, commit_sha: String, metrics: M, violations: Vec<Violation>) -> Self {
        let passed = !violations.iter().any(|v| v.severity == SEVERITY_ERROR);
        Report {
            schema_version: REPORT_SCHEMA_VERSION.to_string(),
            monitor: monitor_name.to_string(),
            commit_sha,
            passed,
            metrics,
            violations,
        }
    }

    /// Port of `WriteJSON` — indented JSON, parents created, evidence kept even
    /// on a policy violation.
    pub fn write_json(&self, out_path: &str) -> Result<(), String> {
        let data = jsonout::to_pretty(self);
        if let Some(dir) = Path::new(out_path).parent() {
            if !dir.as_os_str().is_empty() && dir != Path::new(".") {
                std::fs::create_dir_all(dir).map_err(|e| e.to_string())?;
            }
        }
        std::fs::write(out_path, data).map_err(|e| e.to_string())
    }

    /// Port of `HasError`.
    pub fn has_error(&self) -> bool {
        !self.passed
    }
}

fn is_zero_i64(v: &i64) -> bool {
    *v == 0
}

#[derive(Debug, Clone, Serialize)]
pub struct SizeMetrics {
    pub head_commit: String,
    pub base_ref: String,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub base_commit: Option<String>,
    pub head_tracked_files: i64,
    #[serde(skip_serializing_if = "is_zero_i64")]
    pub base_tracked_files: i64,
    pub head_total_bytes: i64,
    #[serde(skip_serializing_if = "is_zero_i64")]
    pub base_total_bytes: i64,
    #[serde(skip_serializing_if = "is_zero_i64")]
    pub growth_bytes: i64,
    pub new_markdown_count: i64,
    pub new_markdown_over_limit_count: i64,
    pub base_resolved: bool,
}

#[derive(Debug, Clone, Serialize)]
pub struct CompileMetrics {
    pub tracked_cs_files: i64,
    pub production_count: i64,
    pub test_count: i64,
    pub standalone_tool_count: i64,
    pub accepted_debt_count: i64,
    pub new_unclassified_count: i64,
    pub missing_working_tree_count: i64,
    // No `omitempty` upstream: a nil slice marshals as `null`, not omitted.
    pub projects_evaluated: Option<Vec<String>>,
    pub standalone_projects_discovered: Option<Vec<String>>,
}

// ---------------------------------------------------------------------------
// git plumbing
// ---------------------------------------------------------------------------

fn run_git(dir: &str, args: &[&str]) -> Result<String, String> {
    let out = std::process::Command::new("git")
        .args(args)
        .current_dir(dir)
        .output()
        .map_err(|e| format!("git {}: {}", args.join(" "), e))?;
    if !out.status.success() {
        let code = out.status.code().unwrap_or(-1);
        return Err(format!(
            "git {}: exit status {}: {}",
            args.join(" "),
            code,
            String::from_utf8_lossy(&out.stderr).trim()
        ));
    }
    Ok(String::from_utf8_lossy(&out.stdout).trim().to_string())
}

/// Port of `ResolveCommit`.
pub fn resolve_commit(root: &str, reference: &str) -> Result<String, String> {
    if reference.trim().is_empty() {
        return Err("empty git ref".to_string());
    }
    let spec = format!("{}^{{commit}}", reference);
    let sha = run_git(root, &["rev-parse", "--verify", &spec])
        .map_err(|e| format!("resolve ref {:?}: {}", reference, e))?;
    Ok(sha)
}

/// Port of `TreeEntry`.
#[derive(Debug, Clone)]
pub struct TreeEntry {
    pub path: String,
    pub size: i64,
}

/// Port of `ListTreeBlobs` (`git ls-tree -r -z -l`).
pub fn list_tree_blobs(root: &str, reference: &str) -> Result<Vec<TreeEntry>, String> {
    let out = run_git(root, &["ls-tree", "-r", "-z", "-l", reference])
        .map_err(|e| format!("list tree at {:?}: {}", reference, e))?;

    let mut entries = Vec::new();
    for rec in out.split('\0') {
        if rec.is_empty() {
            continue;
        }
        let Some(tab) = rec.find('\t') else { continue };
        let meta = &rec[..tab];
        let path = &rec[tab + 1..];
        let fields: Vec<&str> = meta.split_whitespace().collect();
        if fields.len() < 4 {
            continue;
        }
        if fields[1] != "blob" {
            continue;
        }
        let Ok(size) = fields[3].parse::<i64>() else {
            continue;
        };
        entries.push(TreeEntry {
            path: path.to_string(),
            size,
        });
    }
    Ok(entries)
}

/// Port of `ListTrackedFiles`.
pub fn list_tracked_files(root: &str, pathspec: &str) -> Result<Vec<String>, String> {
    let out = std::process::Command::new("git")
        .args(["ls-files", "-z", "--", pathspec])
        .current_dir(root)
        .output()
        .map_err(|e| format!("git ls-files -- {}: {}", pathspec, e))?;
    if !out.status.success() {
        return Err(format!(
            "git ls-files -- {}: exit status {}: {}",
            pathspec,
            out.status.code().unwrap_or(-1),
            String::from_utf8_lossy(&out.stderr).trim()
        ));
    }
    let mut files = Vec::new();
    for p in String::from_utf8_lossy(&out.stdout).split('\0') {
        if p.is_empty() {
            continue;
        }
        files.push(p.replace('\\', "/"));
    }
    Ok(files)
}

/// Port of `HeadCommit`.
pub fn head_commit(root: &str) -> Result<String, String> {
    resolve_commit(root, "HEAD")
}

// ---------------------------------------------------------------------------
// Size monitor
// ---------------------------------------------------------------------------

#[derive(Debug, Clone)]
pub struct SizeOptions {
    pub root: String,
    pub base_ref: String,
    pub policy_path: String,
}

fn is_markdown_path(path: &str) -> bool {
    let name = path.rsplit('/').next().unwrap_or(path);
    let ext = match name.rfind('.') {
        Some(i) => name[i..].to_ascii_lowercase(),
        None => String::new(),
    };
    ext == ".md" || ext == ".markdown"
}

fn blob_map(entries: &[TreeEntry]) -> (HashMap<String, i64>, i64) {
    let mut m = HashMap::with_capacity(entries.len());
    let mut total = 0i64;
    for e in entries {
        m.insert(e.path.clone(), e.size);
        total += e.size;
    }
    (m, total)
}

fn short_sha(sha: &str) -> &str {
    if sha.len() > 10 {
        &sha[..10]
    } else {
        sha
    }
}

/// Port of `RunSizeMonitor`.
pub fn run_size_monitor(opts: &SizeOptions) -> Result<Report<SizeMetrics>, String> {
    let policy = load_policy(&opts.policy_path)?;

    let head_sha = head_commit(&opts.root).map_err(|e| format!("resolve HEAD: {}", e))?;
    let head_entries =
        list_tree_blobs(&opts.root, &head_sha).map_err(|e| format!("list HEAD tree: {}", e))?;
    let (head_map, head_total) = blob_map(&head_entries);

    let mut metrics = SizeMetrics {
        head_commit: head_sha.clone(),
        base_ref: opts.base_ref.clone(),
        base_commit: None,
        head_tracked_files: head_map.len() as i64,
        base_tracked_files: 0,
        head_total_bytes: head_total,
        base_total_bytes: 0,
        growth_bytes: 0,
        new_markdown_count: 0,
        new_markdown_over_limit_count: 0,
        base_resolved: false,
    };

    let mut violations: Vec<Violation> = Vec::new();

    let base_sha = match resolve_commit(&opts.root, &opts.base_ref) {
        Ok(s) => s,
        Err(base_err) => {
            violations.push(Violation::new(
                SEVERITY_ERROR,
                1.0,
                "base_ref_unresolved",
                None,
                format!(
                    "base ref {:?} could not be resolved to a commit ({}); size-growth comparison requires a valid --base and fails closed when it is missing",
                    opts.base_ref, base_err
                ),
            ));
            return Ok(Report::new(
                "monitor-size",
                head_sha,
                metrics,
                violations,
            ));
        }
    };

    metrics.base_resolved = true;
    metrics.base_commit = Some(base_sha.clone());

    let base_entries =
        list_tree_blobs(&opts.root, &base_sha).map_err(|e| format!("list base tree at {}: {}", base_sha, e))?;
    let (base_map, base_total) = blob_map(&base_entries);
    metrics.base_tracked_files = base_map.len() as i64;
    metrics.base_total_bytes = base_total;

    let growth = head_total - base_total;
    metrics.growth_bytes = growth;

    if growth > policy.size.pr_growth_fail_bytes {
        violations.push(Violation::new(
            SEVERITY_ERROR,
            1.0,
            "growth_exceeds_fail_threshold",
            None,
            format!(
                "tracked-blob growth {} bytes exceeds fail threshold {} bytes (base {} -> HEAD {})",
                growth,
                policy.size.pr_growth_fail_bytes,
                short_sha(&base_sha),
                short_sha(&head_sha)
            ),
        ));
    } else if growth > policy.size.pr_growth_warn_bytes {
        violations.push(Violation::new(
            SEVERITY_WARNING,
            1.0,
            "growth_exceeds_warn_threshold",
            None,
            format!(
                "tracked-blob growth {} bytes exceeds warn threshold {} bytes (base {} -> HEAD {})",
                growth,
                policy.size.pr_growth_warn_bytes,
                short_sha(&base_sha),
                short_sha(&head_sha)
            ),
        ));
    }

    // New Markdown files only (sorted for deterministic output; Go map order).
    let mut paths: Vec<&String> = head_map.keys().collect();
    paths.sort();
    for path in paths {
        if !is_markdown_path(path) {
            continue;
        }
        if base_map.contains_key(path) {
            continue;
        }
        let size = head_map[path];
        metrics.new_markdown_count += 1;
        if size <= policy.size.new_markdown_max_bytes {
            continue;
        }
        if policy.size.markdown_allowlist.iter().any(|a| a == path) {
            violations.push(Violation::new(
                SEVERITY_INFO,
                1.0,
                "new_markdown_allowlisted",
                Some(path),
                format!(
                    "new markdown file is {} bytes (over {} byte cap) but is explicitly allowlisted in policy",
                    size, policy.size.new_markdown_max_bytes
                ),
            ));
            continue;
        }
        metrics.new_markdown_over_limit_count += 1;
        violations.push(Violation::new(
            SEVERITY_ERROR,
            1.0,
            "new_markdown_oversize",
            Some(path),
            format!(
                "new markdown file is {} bytes, exceeding the {} byte cap for newly added Markdown; add an explicit allowlist entry in docs/ci/MONITORING_POLICY.json if this is intentional",
                size, policy.size.new_markdown_max_bytes
            ),
        ));
    }

    Ok(Report::new("monitor-size", head_sha, metrics, violations))
}

// ---------------------------------------------------------------------------
// Compile monitor
// ---------------------------------------------------------------------------

#[derive(Debug, Clone)]
pub struct CompileOptions {
    pub root: String,
    pub policy_path: String,
}

#[derive(Debug, Default, Deserialize)]
struct MsbuildCompileItem {
    #[serde(rename = "FullPath", default)]
    full_path: String,
}

#[derive(Debug, Default, Deserialize)]
struct MsbuildItems {
    #[serde(rename = "Compile", default)]
    compile: Vec<MsbuildCompileItem>,
}

#[derive(Debug, Default, Deserialize)]
struct MsbuildGetItemResult {
    #[serde(rename = "Items", default)]
    items: MsbuildItems,
}

/// `filepath.Abs`-style path cleaning.
fn go_abs(p: &str) -> PathBuf {
    let joined = if Path::new(p).is_absolute() {
        PathBuf::from(p)
    } else {
        std::env::current_dir()
            .unwrap_or_else(|_| PathBuf::from("."))
            .join(p)
    };
    let mut out = PathBuf::new();
    for component in joined.components() {
        match component {
            Component::CurDir => {}
            Component::ParentDir => {
                if !out.pop() && !out.has_root() {
                    out.push("..");
                }
            }
            other => out.push(other.as_os_str()),
        }
    }
    out
}

/// `filepath.Rel(root, full)` with forward slashes; `None` if outside root.
fn go_rel(root: &str, full: &str) -> Option<String> {
    let abs_root = go_abs(root);
    let abs_full = go_abs(full);
    let rel = abs_full.strip_prefix(&abs_root).ok()?;
    Some(rel.to_string_lossy().replace('\\', "/"))
}

/// Port of `GetCompileItems` — the MSBuild-evaluated compile set.
pub fn get_compile_items(root: &str, csproj_rel_path: &str) -> Result<Vec<String>, String> {
    let abs_proj = Path::new(root).join(csproj_rel_path);
    if !abs_proj.exists() {
        return Err(format!(
            "project {} not found at {}: no such file or directory",
            csproj_rel_path,
            abs_proj.to_string_lossy()
        ));
    }

    let out = std::process::Command::new("dotnet")
        .args([
            "msbuild",
            &abs_proj.to_string_lossy(),
            "-getItem:Compile",
            "-nologo",
        ])
        .current_dir(root)
        .output()
        .map_err(|e| format!("dotnet msbuild {} -getItem:Compile: {}", csproj_rel_path, e))?;
    if !out.status.success() {
        return Err(format!(
            "dotnet msbuild {} -getItem:Compile: exit status {}: {}",
            csproj_rel_path,
            out.status.code().unwrap_or(-1),
            String::from_utf8_lossy(&out.stderr).trim()
        ));
    }

    let result: MsbuildGetItemResult = serde_json::from_slice(&out.stdout)
        .map_err(|e| format!("parse msbuild -getItem:Compile JSON for {}: {}", csproj_rel_path, e))?;

    let mut items = Vec::new();
    for item in result.items.compile {
        if item.full_path.is_empty() {
            continue;
        }
        let Some(rel) = go_rel(root, &item.full_path) else {
            continue;
        };
        if rel.starts_with("../") {
            continue;
        }
        items.push(rel);
    }
    Ok(items)
}

/// Port of `discoverStandaloneProjects` (`filepath.WalkDir`, skipping
/// `bin`/`obj`/`.git`/`node_modules`/`.godot`, sorted).
fn discover_standalone_projects(
    root: &str,
    exclude: &HashSet<String>,
) -> Result<Vec<String>, String> {
    let mut found: Vec<String> = Vec::new();
    let walk_root = Path::new(root);
    if !walk_root.exists() {
        return Ok(found);
    }
    let entries = crate::gowalk::walk_go(walk_root, |name| {
        let n = name.to_string_lossy();
        !matches!(n.as_ref(), "bin" | "obj" | ".git" | "node_modules" | ".godot")
    });
    let abs_root = go_abs(root);
    for e in entries {
        if e.is_dir {
            continue;
        }
        let name = e
            .path
            .file_name()
            .map(|n| n.to_string_lossy().into_owned())
            .unwrap_or_default();
        if !name.ends_with(".csproj") {
            continue;
        }
        let rel = match e.path.strip_prefix(&abs_root) {
            Ok(r) => r.to_string_lossy().replace('\\', "/"),
            Err(_) => continue,
        };
        if exclude.contains(&rel) {
            continue;
        }
        found.push(rel);
    }
    found.sort();
    Ok(found)
}

/// Port of `RunCompileMonitor`.
pub fn run_compile_monitor(opts: &CompileOptions) -> Result<Report<CompileMetrics>, String> {
    let policy = load_policy(&opts.policy_path)?;
    let head_sha = head_commit(&opts.root).map_err(|e| format!("resolve HEAD: {}", e))?;

    let mut evaluated: Vec<String> = Vec::new();

    let mut prod_set: HashSet<String> = HashSet::new();
    for proj in &policy.compile.production_projects {
        let items = get_compile_items(&opts.root, proj)
            .map_err(|e| format!("evaluate production project {}: {}", proj, e))?;
        evaluated.push(proj.clone());
        prod_set.extend(items);
    }

    let mut test_set: HashSet<String> = HashSet::new();
    for proj in &policy.compile.test_projects {
        let items = get_compile_items(&opts.root, proj)
            .map_err(|e| format!("evaluate test project {}: {}", proj, e))?;
        evaluated.push(proj.clone());
        test_set.extend(items);
    }

    let mut exclude: HashSet<String> = HashSet::new();
    for p in policy
        .compile
        .production_projects
        .iter()
        .chain(policy.compile.test_projects.iter())
    {
        exclude.insert(p.replace('\\', "/"));
    }

    let standalone_projects = discover_standalone_projects(&opts.root, &exclude)?;

    let mut standalone_compiled: HashMap<String, Vec<String>> = HashMap::new();
    for proj in &standalone_projects {
        let items = get_compile_items(&opts.root, proj)
            .map_err(|e| format!("evaluate standalone project {}: {}", proj, e))?;
        for it in items {
            standalone_compiled.entry(it).or_default().push(proj.clone());
        }
    }

    let tracked_cs = list_tracked_files(&opts.root, "*.cs")
        .map_err(|e| format!("list tracked .cs files: {}", e))?;

    let mut metrics = CompileMetrics {
        tracked_cs_files: tracked_cs.len() as i64,
        production_count: 0,
        test_count: 0,
        standalone_tool_count: 0,
        accepted_debt_count: 0,
        new_unclassified_count: 0,
        missing_working_tree_count: 0,
        projects_evaluated: if evaluated.is_empty() {
            None
        } else {
            Some(evaluated.clone())
        },
        standalone_projects_discovered: if standalone_projects.is_empty() {
            None
        } else {
            Some(standalone_projects.clone())
        },
    };

    let mut violations: Vec<Violation> = Vec::new();
    let mut seen_known: HashSet<String> = HashSet::new();

    for path in &tracked_cs {
        if !Path::new(&opts.root).join(path).exists() {
            metrics.missing_working_tree_count += 1;
            violations.push(Violation::new(
                SEVERITY_ERROR,
                1.0,
                "tracked_source_missing_from_worktree",
                Some(path),
                "tracked C# source is missing from the working tree while still present in the index; the evaluated compile set cannot classify it reliably. Restore or commit the deletion, then rerun on a coherent checkout".to_string(),
            ));
            continue;
        }
        let in_prod = prod_set.contains(path);
        let in_test = test_set.contains(path);

        if in_prod && in_test {
            metrics.production_count += 1;
            violations.push(Violation::new(
                SEVERITY_INFO,
                0.8,
                "shared_production_and_test",
                Some(path),
                "compiled by both a production project and a test project Compile item; verify this is intentional".to_string(),
            ));
        } else if in_prod {
            metrics.production_count += 1;
        } else if in_test {
            if path.starts_with(&policy.compile.test_root_prefix) {
                metrics.test_count += 1;
                continue;
            }
            if let Some(known) = policy.compile.known_path(path) {
                seen_known.insert(path.clone());
                metrics.accepted_debt_count += 1;
                violations.push(Violation::new(
                    &known.severity,
                    1.0,
                    &known.classification,
                    Some(path),
                    known.reason.clone(),
                ));
                continue;
            }
            metrics.new_unclassified_count += 1;
            violations.push(Violation::new(
                SEVERITY_ERROR,
                1.0,
                "new_test_only_production_source",
                Some(path),
                format!(
                    "newly compiled only by a test project ({}) outside its normal test root ({}); this is production-like source with no production owner. Add an explicit docs/ci/MONITORING_POLICY.json compile.known_paths entry with a written reason if this is intentional accepted debt, or give it a production compile owner.",
                    policy.compile.test_projects.join(", "),
                    policy.compile.test_root_prefix
                ),
            ));
        } else if let Some(projs) = standalone_compiled.get(path) {
            metrics.standalone_tool_count += 1;
            violations.push(Violation::new(
                SEVERITY_INFO,
                1.0,
                "standalone_tool_compiled",
                Some(path),
                format!("compiled by standalone project(s): {}", projs.join(", ")),
            ));
        } else if let Some(known) = policy.compile.known_path(path) {
            seen_known.insert(path.clone());
            metrics.accepted_debt_count += 1;
            violations.push(Violation::new(
                &known.severity,
                1.0,
                &known.classification,
                Some(path),
                known.reason.clone(),
            ));
        } else {
            metrics.new_unclassified_count += 1;
            violations.push(Violation::new(
                SEVERITY_ERROR,
                1.0,
                "unclassified_uncompiled_source",
                Some(path),
                "tracked .cs file is not compiled by any production, test, or discovered standalone project, and has no docs/ci/MONITORING_POLICY.json compile.known_paths entry explaining why. Add a compile item or an explicit allowlist entry with a written reason.".to_string(),
            ));
        }
    }

    for k in &policy.compile.known_paths {
        if seen_known.contains(&k.path) {
            continue;
        }
        violations.push(Violation::new(
            SEVERITY_WARNING,
            0.9,
            "stale_policy_allowlist_entry",
            Some(&k.path),
            "docs/ci/MONITORING_POLICY.json compile.known_paths entry no longer matches a tracked file classified in this bucket (removed, renamed, or now compiled normally); remove or update the entry".to_string(),
        ));
    }

    Ok(Report::new("monitor-compile", head_sha, metrics, violations))
}

// ---------------------------------------------------------------------------
// Report writing shared by the CLI
// ---------------------------------------------------------------------------

/// Convenience: the RFC3339 stamp a report may embed (kept for parity with the
/// Go package's unused time helper surface).
#[allow(dead_code)]
pub fn now_rfc3339() -> String {
    gotime::now_utc_rfc3339()
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::fs;
    use std::process::Command;

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-monitor-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    fn git(dir: &Path, args: &[&str]) {
        let out = Command::new("git")
            .args(args)
            .current_dir(dir)
            .output()
            .unwrap();
        assert!(out.status.success(), "git {:?}: {}", args, String::from_utf8_lossy(&out.stderr));
    }

    fn init_repo(tag: &str) -> PathBuf {
        let root = tmpdir(tag).join("repo with spaces");
        fs::create_dir_all(&root).unwrap();
        git(&root, &["init", "-q"]);
        git(&root, &["config", "user.email", "t@example.invalid"]);
        git(&root, &["config", "user.name", "T"]);
        root
    }

    fn write(root: &Path, rel: &str, content: &str) {
        let full = root.join(rel);
        fs::create_dir_all(full.parent().unwrap()).unwrap();
        fs::write(full, content).unwrap();
    }

    fn commit_all(root: &Path, msg: &str) -> String {
        git(root, &["add", "-A"]);
        git(root, &["commit", "-q", "--allow-empty", "-m", msg]);
        let out = Command::new("git")
            .args(["rev-parse", "HEAD"])
            .current_dir(root)
            .output()
            .unwrap();
        String::from_utf8_lossy(&out.stdout).trim().to_string()
    }

    fn size_policy(
        root: &Path,
        markdown_max: i64,
        growth_fail: i64,
        growth_warn: i64,
        allowlist: &[&str],
    ) -> String {
        let allow: Vec<String> = allowlist.iter().map(|s| format!("{:?}", s)).collect();
        let body = format!(
            r#"{{"schema_version":"1.0","size":{{"new_markdown_max_bytes":{},"pr_growth_fail_bytes":{},"pr_growth_warn_bytes":{},"markdown_allowlist":[{}]}},"compile":{{"production_projects":["p.csproj"],"test_projects":["t.csproj"],"test_root_prefix":"Tests/"}}}}"#,
            markdown_max,
            growth_fail,
            growth_warn,
            allow.join(",")
        );
        let path = root.join("policy.json");
        fs::write(&path, body).unwrap();
        path.to_string_lossy().into_owned()
    }

    fn run_size(root: &Path, base: &str, policy: &str) -> Report<SizeMetrics> {
        run_size_monitor(&SizeOptions {
            root: root.to_string_lossy().into_owned(),
            base_ref: base.to_string(),
            policy_path: policy.to_string(),
        })
        .unwrap()
    }

    #[test]
    fn size_monitor_passes_within_thresholds() {
        let root = init_repo("size-pass");
        write(&root, "src/a.txt", "baseline content");
        let base = commit_all(&root, "base");
        write(&root, "src/b.txt", "small addition");
        commit_all(&root, "head");
        let policy = size_policy(&root, 1 << 20, 1 << 20, 1 << 10, &[]);

        let rep = run_size(&root, &base, &policy);
        assert!(rep.passed, "{:?}", rep.violations);
        assert!(rep.metrics.base_resolved);
        assert_eq!(rep.metrics.growth_bytes, "small addition".len() as i64);
        // Go emits an empty array, not null.
        assert!(jsonout::to_pretty(&rep).contains("\"violations\": []"));

        let _ = fs::remove_dir_all(root.parent().unwrap());
    }

    #[test]
    fn size_monitor_flags_oversize_new_markdown_unless_allowlisted() {
        let root = init_repo("size-md");
        write(&root, "src/a.txt", "baseline");
        let base = commit_all(&root, "base");
        let big = "x".repeat(2048);
        write(&root, "docs/NEW_BIG.md", &big);
        commit_all(&root, "head");

        let policy = size_policy(&root, 1024, 1 << 30, 1 << 20, &[]);
        let rep = run_size(&root, &base, &policy);
        assert!(!rep.passed);
        let v = rep
            .violations
            .iter()
            .find(|v| v.path.as_deref() == Some("docs/NEW_BIG.md"))
            .unwrap();
        assert_eq!(v.severity, SEVERITY_ERROR);
        assert_eq!(v.category, "new_markdown_oversize");

        let policy = size_policy(&root, 1024, 1 << 30, 1 << 20, &["docs/NEW_BIG.md"]);
        let rep = run_size(&root, &base, &policy);
        assert!(rep.passed, "{:?}", rep.violations);
        let v = rep
            .violations
            .iter()
            .find(|v| v.path.as_deref() == Some("docs/NEW_BIG.md"))
            .unwrap();
        assert_eq!(v.severity, SEVERITY_INFO);
        assert_eq!(v.category, "new_markdown_allowlisted");
        let _ = fs::remove_dir_all(root.parent().unwrap());
    }

    #[test]
    fn size_monitor_does_not_flag_preexisting_markdown() {
        let root = init_repo("size-existing");
        write(&root, "docs/ALREADY_HERE.md", &"y".repeat(2048));
        write(&root, "src/a.txt", "baseline");
        let base = commit_all(&root, "base");
        write(&root, "src/a.txt", "baseline modified slightly");
        commit_all(&root, "head");
        let policy = size_policy(&root, 1024, 1 << 30, 1 << 20, &[]);
        let rep = run_size(&root, &base, &policy);
        assert!(rep.passed, "{:?}", rep.violations);
        assert_eq!(rep.metrics.new_markdown_count, 0);
        let _ = fs::remove_dir_all(root.parent().unwrap());
    }

    #[test]
    fn size_monitor_fails_closed_on_unresolved_base() {
        let root = init_repo("size-base");
        write(&root, "src/a.txt", "baseline");
        commit_all(&root, "only");
        let policy = size_policy(&root, 1 << 20, 1 << 30, 1 << 20, &[]);
        let rep = run_size(&root, "no-such-ref-anywhere", &policy);
        assert!(!rep.passed);
        assert!(!rep.metrics.base_resolved);
        assert!(rep.metrics.head_total_bytes > 0);
        assert!(rep
            .violations
            .iter()
            .any(|v| v.category == "base_ref_unresolved" && v.severity == SEVERITY_ERROR));
        let _ = fs::remove_dir_all(root.parent().unwrap());
    }

    #[test]
    fn list_tree_blobs_uses_committed_size_and_tracked_files_filters() {
        let root = init_repo("git-plumbing");
        let pointer = "version https://git-lfs.github.com/spec/v1\noid sha256:deadbeef\nsize 999999999\n";
        write(&root, "pointer.bin", pointer);
        write(&root, "a.cs", "class A {}");
        write(&root, "sub/b.cs", "class B {}");
        write(&root, "readme.md", "# hi");
        let sha = commit_all(&root, "seed");
        // Simulate a hydrated working-tree file; the committed blob size wins.
        fs::write(root.join("pointer.bin"), vec![0u8; 5000]).unwrap();

        let entries = list_tree_blobs(&root.to_string_lossy(), &sha).unwrap();
        let found = entries.iter().find(|e| e.path == "pointer.bin").unwrap();
        assert_eq!(found.size, pointer.len() as i64);

        let files = list_tracked_files(&root.to_string_lossy(), "*.cs").unwrap();
        assert_eq!(files.len(), 2);
        assert!(files.contains(&"a.cs".to_string()));
        assert!(files.contains(&"sub/b.cs".to_string()));

        assert!(resolve_commit(&root.to_string_lossy(), "nope").is_err());
        assert!(resolve_commit(&root.to_string_lossy(), "").is_err());
        let _ = fs::remove_dir_all(root.parent().unwrap());
    }

    #[test]
    fn compile_monitor_classifies_buckets_and_fails_on_unknown_debt() {
        let root = init_repo("compile");
        write(&root, "Prod.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework><EnableDefaultCompileItems>false</EnableDefaultCompileItems></PropertyGroup><ItemGroup><Compile Include=\"prod/**/*.cs\" /></ItemGroup></Project>");
        write(&root, "prod/Widget.cs", "namespace P { public class Widget {} }");
        write(&root, "Tests/Tests.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup><ItemGroup><Compile Include=\"../legacy/Legacy.cs\" Link=\"Legacy.cs\" /></ItemGroup></Project>");
        write(&root, "Tests/WidgetTests.cs", "namespace T { public class WidgetTests {} }");
        write(&root, "legacy/Legacy.cs", "namespace L { public class Legacy {} }");
        write(&root, "stray/Stray.cs", "namespace S { public class Stray {} }");
        commit_all(&root, "fixture");
        let policy = root.join("policy.json");
        fs::write(
            &policy,
            r#"{"schema_version":"1.0","size":{"new_markdown_max_bytes":1048576,"pr_growth_fail_bytes":1073741824},"compile":{"production_projects":["Prod.csproj"],"test_projects":["Tests/Tests.csproj"],"test_root_prefix":"Tests/"}}"#,
        )
        .unwrap();

        match run_compile_monitor(&CompileOptions {
            root: root.to_string_lossy().into_owned(),
            policy_path: policy.to_string_lossy().into_owned(),
        }) {
            Ok(rep) => {
                assert!(!rep.passed);
                assert_eq!(rep.metrics.production_count, 1);
                assert_eq!(rep.metrics.test_count, 1);
                assert_eq!(rep.metrics.new_unclassified_count, 2);
                let stray = rep
                    .violations
                    .iter()
                    .find(|v| v.path.as_deref() == Some("stray/Stray.cs"))
                    .unwrap();
                assert_eq!(stray.category, "unclassified_uncompiled_source");
                let legacy = rep
                    .violations
                    .iter()
                    .find(|v| v.path.as_deref() == Some("legacy/Legacy.cs"))
                    .unwrap();
                assert_eq!(legacy.category, "new_test_only_production_source");
            }
            // If the environment has no working `dotnet`, the monitor must fail
            // loudly (never invent a verdict); tolerate that in unit tests only.
            Err(e) => assert!(e.contains("dotnet msbuild"), "{e}"),
        }
        let _ = fs::remove_dir_all(root.parent().unwrap());
    }

    #[test]
    fn compile_get_items_errors_on_missing_project() {
        let root = tmpdir("compile-missing");
        let err = get_compile_items(&root.to_string_lossy(), "DoesNotExist.csproj").unwrap_err();
        assert!(err.contains("not found"), "{err}");
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn policy_validation_fails_closed() {
        let root = tmpdir("policy");
        let bad = root.join("bad.json");
        fs::write(&bad, r#"{"schema_version":"1.0","size":{"new_markdown_max_bytes":0,"pr_growth_fail_bytes":1},"compile":{"production_projects":["p"],"test_projects":["t"]}}"#).unwrap();
        assert!(load_policy(&bad.to_string_lossy()).is_err());
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn go_float_renders_integral_confidence_like_go() {
        let rep = Report::new(
            "monitor-compile",
            "abc".to_string(),
            CompileMetrics {
                tracked_cs_files: 1,
                production_count: 1,
                test_count: 0,
                standalone_tool_count: 0,
                accepted_debt_count: 0,
                new_unclassified_count: 0,
                missing_working_tree_count: 0,
                projects_evaluated: None,
                standalone_projects_discovered: None,
            },
            vec![Violation::new(SEVERITY_INFO, 1.0, "x", Some("a.cs"), "m".into())],
        );
        let json = jsonout::to_pretty(&rep);
        assert!(json.contains("\"confidence\": 1,"), "{json}");
        assert!(json.contains("\"projects_evaluated\": null"), "{json}");
    }
}