//! Port of `tools/gotools/pkg/releasepolicy/releasepolicy.go` (and its
//! `cmd/releasepolicy` wrapper, exposed here as the `releasepolicy` subcommand).
//!
//! Structural policy check: it parses `.github/workflows/release.yml`,
//! `scripts/ci/release-gate.sh`, `scripts/ci/export-build.sh`, and
//! `docs/ci/CI_GATE_MANIFEST.json` and reports where they disagree. It never
//! executes the release gate, the exporters, or any test.
//!
//! Parity notes:
//! * YAML is parsed with `serde_yaml` (Go used `goccy/go-yaml`); only the
//!   `jobs.*` subset this check reads is modelled, and unknown keys are ignored
//!   as in Go.
//! * `generated_at` is `time.RFC3339` (whole seconds) and `commit` is
//!   `git rev-parse HEAD`; both are wall-clock/VCS values normalised in parity
//!   diffs.

use crate::gotime;
use crate::jsonout;
use regex::Regex;
use serde::{Deserialize, Serialize};
use std::collections::{BTreeMap, HashMap, HashSet};

/// Version stamped on the report.
pub const SCHEMA_VERSION: &str = "1.0.0";

/// The exact, unmodified blocking gate invocation release.yml must run.
pub const CANONICAL_RELEASE_GATE_INVOCATION: &str = "bash scripts/ci/release-gate.sh";

pub const KIND_MISSING_REQUIRED_GATE: &str = "missing_required_gate";
pub const KIND_NONBLOCKING_STEP: &str = "nonblocking_step";
pub const KIND_WEAKENED_CRITICALITY: &str = "weakened_criticality";
pub const KIND_ORPHANED_RELEASE_ID: &str = "orphaned_release_id";
pub const KIND_EXPORT_ORPHAN: &str = "export_orphan";

// ---------------------------------------------------------------------------
// Manifest
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Default, Deserialize)]
pub struct ManifestGate {
    #[serde(default)]
    pub gate_id: String,
    #[serde(default)]
    pub command: String,
    #[serde(default)]
    pub classification: String,
    #[serde(default)]
    pub critical: Option<bool>,
    #[serde(default)]
    pub release_required: bool,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct Manifest {
    #[serde(default)]
    pub gates: Vec<ManifestGate>,
}

pub fn load_manifest(path: &str) -> Result<Manifest, String> {
    let data = std::fs::read(path).map_err(|e| format!("read manifest {}: {}", path, e))?;
    serde_json::from_slice(&data).map_err(|e| format!("parse manifest {}: {}", path, e))
}

// ---------------------------------------------------------------------------
// Workflow
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Default, Deserialize)]
pub struct WorkflowStep {
    /// Human-readable step label; parsed for fidelity but not read by the check.
    #[allow(dead_code)]
    #[serde(default)]
    pub name: String,
    #[serde(default)]
    pub uses: String,
    #[serde(default)]
    pub run: String,
    #[serde(default, rename = "if")]
    pub if_cond: String,
    #[serde(default, rename = "continue-on-error")]
    pub continue_on_error: Option<serde_yaml::Value>,
    #[serde(default, rename = "with")]
    pub with: HashMap<String, serde_yaml::Value>,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct WorkflowJob {
    #[serde(default)]
    pub needs: Option<serde_yaml::Value>,
    #[serde(default, rename = "if")]
    pub if_cond: String,
    #[serde(default, rename = "continue-on-error")]
    pub continue_on_error: Option<serde_yaml::Value>,
    #[serde(default)]
    pub steps: Vec<WorkflowStep>,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct Workflow {
    #[serde(default)]
    pub jobs: BTreeMap<String, WorkflowJob>,
}

pub fn load_workflow(path: &str) -> Result<Workflow, String> {
    let data = std::fs::read(path).map_err(|e| format!("read workflow {}: {}", path, e))?;
    serde_yaml::from_slice(&data).map_err(|e| format!("parse workflow {}: {}", path, e))
}

// ---------------------------------------------------------------------------
// Report
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Serialize)]
pub struct Violation {
    pub kind: String,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub gate_id: Option<String>,
    pub location: String,
    pub detail: String,
    pub remediation: String,
}

impl Violation {
    fn new(kind: &str, gate_id: Option<String>, location: String, detail: String, remediation: String) -> Violation {
        Violation {
            kind: kind.to_string(),
            gate_id,
            location,
            detail,
            remediation,
        }
    }
}

#[derive(Debug, Clone, Serialize)]
pub struct Evidence {
    pub description: String,
    pub detail: String,
}

#[derive(Debug, Clone, Default, Serialize)]
pub struct Metrics {
    pub required_gate_count: i64,
    pub fast_tier_gate_count: i64,
    pub explicit_full_gate_count: i64,
    pub workflow_executed_count: i64,
    pub violation_count: i64,
}

#[derive(Debug, Clone, Serialize)]
pub struct Report {
    pub schema_version: String,
    pub commit: String,
    pub generated_at: String,
    pub workflow_path: String,
    pub script_path: String,
    pub export_script_path: String,
    pub manifest_path: String,
    pub pass: bool,
    pub metrics: Metrics,
    pub violations: Vec<Violation>,
    pub evidence: Vec<Evidence>,
    pub remediation: Vec<String>,
}

impl Report {
    /// `MarshalIndent` + a trailing newline, then write (creating parents).
    pub fn write_json(&self, path: &str) -> Result<(), String> {
        let mut data = jsonout::to_pretty(self);
        data.push('\n');
        if let Some(dir) = std::path::Path::new(path).parent() {
            if !dir.as_os_str().is_empty() && dir != std::path::Path::new(".") {
                std::fs::create_dir_all(dir).map_err(|e| e.to_string())?;
            }
        }
        std::fs::write(path, data).map_err(|e| e.to_string())
    }
}

// ---------------------------------------------------------------------------
// Evaluate
// ---------------------------------------------------------------------------

fn to_set(ids: &[String]) -> HashSet<String> {
    ids.iter().cloned().collect()
}

fn strip_shell_comments(script: &str) -> String {
    script
        .lines()
        .filter(|line| !line.trim_start().starts_with('#'))
        .collect::<Vec<_>>()
        .join("\n")
}

fn parse_explicit_gate_ids(script: &str) -> Vec<String> {
    let re = Regex::new(r"--gate\s+((?:[A-Za-z0-9_]+\s*,\s*)*[A-Za-z0-9_]+)").unwrap();
    let mut seen: HashSet<String> = HashSet::new();
    let mut ids: Vec<String> = Vec::new();
    for caps in re.captures_iter(script) {
        let group = caps.get(1).map(|m| m.as_str()).unwrap_or("");
        for id in group.split(',') {
            let id = id.trim();
            if id.is_empty() || seen.contains(id) {
                continue;
            }
            seen.insert(id.to_string());
            ids.push(id.to_string());
        }
    }
    ids.sort();
    ids
}

fn canonical_command_line(run: &str) -> String {
    for raw in run.lines() {
        let line = raw.trim();
        if line.is_empty() || line.starts_with('#') {
            continue;
        }
        if line.starts_with("bash scripts/ci/release-gate.sh") {
            return line.to_string();
        }
    }
    String::new()
}

fn is_truthy(v: Option<&serde_yaml::Value>) -> bool {
    match v {
        Some(serde_yaml::Value::Bool(b)) => *b,
        Some(serde_yaml::Value::String(s)) => {
            let s = s.trim().to_ascii_lowercase();
            !s.is_empty() && s != "false"
        }
        _ => false,
    }
}

fn blocking_condition(if_cond: &str, continue_on_error: Option<&serde_yaml::Value>) -> Option<String> {
    if !if_cond.trim().is_empty() {
        return Some(format!(
            "has a conditional 'if: {}' that can skip the required step",
            if_cond.trim()
        ));
    }
    if is_truthy(continue_on_error) {
        return Some("sets continue-on-error and can mask a failed gate".to_string());
    }
    None
}

fn needs_list(v: Option<&serde_yaml::Value>) -> Vec<String> {
    match v {
        Some(serde_yaml::Value::String(s)) => vec![s.clone()],
        Some(serde_yaml::Value::Sequence(seq)) => seq
            .iter()
            .filter_map(|e| e.as_str().map(|s| s.to_string()))
            .collect(),
        _ => Vec::new(),
    }
}

fn step_blocking_issue(line: &str, if_cond: &str, continue_on_error: Option<&serde_yaml::Value>) -> String {
    if let Some(issue) = blocking_condition(if_cond, continue_on_error) {
        return issue;
    }
    if line.contains("--skip-full") {
        return "step invokes release-gate.sh with --skip-full, which drops the required full-tier gates in CI".to_string();
    }
    if line != CANONICAL_RELEASE_GATE_INVOCATION {
        return format!(
            "step invokes {:?} instead of the exact canonical command {:?}",
            line, CANONICAL_RELEASE_GATE_INVOCATION
        );
    }
    String::new()
}

fn export_job_surface(wf: &Workflow, export_job_name: &str) -> String {
    let job = wf.jobs.get(export_job_name).cloned().unwrap_or_default();
    let mut b = String::new();
    for s in &job.steps {
        b.push_str(&strip_shell_comments(&s.run));
        b.push('\n');
    }
    b
}

fn check_gate_job_step(wf: &Workflow, gate_job_name: &str) -> Vec<Violation> {
    let Some(job) = wf.jobs.get(gate_job_name) else {
        return vec![Violation::new(
            KIND_NONBLOCKING_STEP,
            None,
            format!("jobs.{}", gate_job_name),
            format!("workflow has no job named {:?}", gate_job_name),
            format!(
                "add a {:?} job that runs `{}` as a blocking step",
                gate_job_name, CANONICAL_RELEASE_GATE_INVOCATION
            ),
        )];
    };

    let mut out = Vec::new();
    if let Some(issue) = blocking_condition(&job.if_cond, job.continue_on_error.as_ref()) {
        out.push(Violation::new(
            KIND_NONBLOCKING_STEP,
            None,
            format!("jobs.{}", gate_job_name),
            format!("release gate job {}", issue),
            "run the release gate job unconditionally and without continue-on-error".to_string(),
        ));
    }
    let mut found = false;
    for (i, step) in job.steps.iter().enumerate() {
        let line = canonical_command_line(&step.run);
        if line.is_empty() {
            continue;
        }
        found = true;
        let issue = step_blocking_issue(&line, &step.if_cond, step.continue_on_error.as_ref());
        if !issue.is_empty() {
            out.push(Violation::new(
                KIND_NONBLOCKING_STEP,
                None,
                format!("jobs.{}.steps[{}]", gate_job_name, i),
                issue,
                format!(
                    "invoke exactly `{}` with no if:, continue-on-error, or extra flags",
                    CANONICAL_RELEASE_GATE_INVOCATION
                ),
            ));
        }
    }
    if !found {
        out.push(Violation::new(
            KIND_NONBLOCKING_STEP,
            None,
            format!("jobs.{}", gate_job_name),
            format!(
                "no step in job {:?} invokes `{}`",
                gate_job_name, CANONICAL_RELEASE_GATE_INVOCATION
            ),
            format!("add a run step invoking exactly `{}`", CANONICAL_RELEASE_GATE_INVOCATION),
        ));
    }
    out
}

fn find_step_index<F: Fn(&WorkflowStep) -> bool>(steps: &[WorkflowStep], pred: F) -> i64 {
    for (i, s) in steps.iter().enumerate() {
        if pred(s) {
            return i as i64;
        }
    }
    -1
}

fn find_upload_step(steps: &[WorkflowStep], path_substr: &str) -> (i64, bool) {
    for (i, s) in steps.iter().enumerate() {
        if !s.uses.starts_with("actions/upload-artifact") {
            continue;
        }
        let p = s.with.get("path").and_then(|v| v.as_str()).unwrap_or("");
        if p.contains(path_substr) {
            let mode = s
                .with
                .get("if-no-files-found")
                .and_then(|v| v.as_str())
                .unwrap_or("");
            return (i as i64, mode == "error");
        }
    }
    (-1, false)
}

fn check_upload_step(
    job_name: &str,
    artifact_path: &str,
    export_idx: i64,
    upload_idx: i64,
    strict: bool,
) -> Vec<Violation> {
    let mut out = Vec::new();
    if upload_idx < 0 {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}", job_name),
            format!("no upload-artifact step uploads {}", artifact_path),
            format!(
                "add an actions/upload-artifact step with path: {} and if-no-files-found: error",
                artifact_path
            ),
        ));
        return out;
    }
    if !strict {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}.steps[{}]", job_name, upload_idx),
            format!(
                "{} upload does not set if-no-files-found: error, so a missing artifact would only warn (false green)",
                artifact_path
            ),
            format!("set `if-no-files-found: error` on the {} upload-artifact step", artifact_path),
        ));
    }
    if export_idx >= 0 && upload_idx < export_idx {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}.steps[{}]", job_name, upload_idx),
            format!("{} is uploaded before the export step that produces it", artifact_path),
            format!(
                "move the {} upload step after the export step that produces it",
                artifact_path
            ),
        ));
    }
    out
}

fn check_export_job(wf: &Workflow, gate_job_name: &str, export_job_name: &str) -> Vec<Violation> {
    let Some(job) = wf.jobs.get(export_job_name) else {
        return vec![Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}", export_job_name),
            format!("workflow has no job named {:?}", export_job_name),
            format!(
                "add a {:?} job that needs {:?} and exports both Linux and Windows builds",
                export_job_name, gate_job_name
            ),
        )];
    };

    let mut out = Vec::new();
    if let Some(issue) = blocking_condition(&job.if_cond, job.continue_on_error.as_ref()) {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}", export_job_name),
            format!("export job {}", issue),
            "run the export job unconditionally after the release gate succeeds".to_string(),
        ));
    }

    if !needs_list(job.needs.as_ref()).iter().any(|n| n == gate_job_name) {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}.needs", export_job_name),
            format!(
                "job {:?} does not `needs: {}`; a failed release gate would not block the export",
                export_job_name, gate_job_name
            ),
            format!("add `needs: {}` to job {:?}", gate_job_name, export_job_name),
        ));
    }

    let linux_export_idx = find_step_index(&job.steps, |s| {
        strip_shell_comments(&s.run).contains("export-build.sh")
    });
    let win_export_idx = find_step_index(&job.steps, |s| {
        let run = strip_shell_comments(&s.run);
        run.contains("--export-release") && run.contains("Windows Desktop")
    });
    let (linux_upload_idx, linux_upload_strict) = find_upload_step(&job.steps, "builds/linux");
    let (win_upload_idx, win_upload_strict) = find_upload_step(&job.steps, "builds/windows");

    if linux_export_idx < 0 {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}", export_job_name),
            "no step invokes scripts/ci/export-build.sh (Linux export + packaged parity)".to_string(),
            "add a run step invoking `bash scripts/ci/export-build.sh`".to_string(),
        ));
    }
    if win_export_idx < 0 {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}", export_job_name),
            "no step exports the Windows Desktop build (--export-release \"Windows Desktop\")".to_string(),
            "add a run step invoking scripts/ci/run-godot-bounded.sh --export-release \"Windows Desktop\" builds/windows/ashfall.exe".to_string(),
        ));
    }
    if linux_export_idx >= 0 && win_export_idx >= 0 && win_export_idx < linux_export_idx {
        out.push(Violation::new(
            KIND_EXPORT_ORPHAN,
            None,
            format!("jobs.{}", export_job_name),
            "the Windows export step runs before the Linux export/parity step".to_string(),
            "move the Windows export step after the export-build.sh step".to_string(),
        ));
    }
    for idx in [linux_export_idx, win_export_idx, linux_upload_idx, win_upload_idx] {
        if idx < 0 {
            continue;
        }
        let step = &job.steps[idx as usize];
        if let Some(issue) = blocking_condition(&step.if_cond, step.continue_on_error.as_ref()) {
            out.push(Violation::new(
                KIND_EXPORT_ORPHAN,
                None,
                format!("jobs.{}.steps[{}]", export_job_name, idx),
                format!("export or artifact step {}", issue),
                "remove the conditional or continue-on-error from the export and artifact steps"
                    .to_string(),
            ));
        }
    }

    out.extend(check_upload_step(
        export_job_name,
        "builds/linux",
        linux_export_idx,
        linux_upload_idx,
        linux_upload_strict,
    ));
    out.extend(check_upload_step(
        export_job_name,
        "builds/windows",
        win_export_idx,
        win_upload_idx,
        win_upload_strict,
    ));

    out
}

fn collect_remediation(violations: &[Violation]) -> Vec<String> {
    let mut seen: HashSet<&str> = HashSet::new();
    let mut out: Vec<String> = Vec::new();
    for v in violations {
        if seen.insert(v.remediation.as_str()) {
            out.push(v.remediation.clone());
        }
    }
    out
}

/// Port of `Evaluate` — the pure check.
pub fn evaluate(
    m: &Manifest,
    wf: &Workflow,
    script: &str,
    export_script: &str,
    gate_job_name: &str,
    export_job_name: &str,
) -> Report {
    let mut rep = Report {
        schema_version: SCHEMA_VERSION.to_string(),
        commit: String::new(),
        generated_at: String::new(),
        workflow_path: String::new(),
        script_path: String::new(),
        export_script_path: String::new(),
        manifest_path: String::new(),
        pass: false,
        metrics: Metrics::default(),
        violations: Vec::new(),
        evidence: Vec::new(),
        remediation: Vec::new(),
    };

    let mut gate_by_id: HashMap<&str, &ManifestGate> = HashMap::new();
    let mut fast_ids: Vec<String> = Vec::new();
    let mut required_ids: Vec<String> = Vec::new();
    for g in &m.gates {
        gate_by_id.insert(g.gate_id.as_str(), g);
        if g.classification == "fast" {
            fast_ids.push(g.gate_id.clone());
        }
        if g.release_required {
            required_ids.push(g.gate_id.clone());
        }
    }
    fast_ids.sort();
    required_ids.sort();
    let fast_set = to_set(&fast_ids);

    for id in &required_ids {
        let g = gate_by_id.get(id.as_str()).unwrap();
        if g.critical != Some(true) {
            rep.violations.push(Violation::new(
                KIND_WEAKENED_CRITICALITY,
                Some(id.clone()),
                "docs/ci/CI_GATE_MANIFEST.json".to_string(),
                format!("gate {:?} is release_required but critical is not true", id),
                format!(
                    "set \"critical\": true for gate_id {:?} in docs/ci/CI_GATE_MANIFEST.json",
                    id
                ),
            ));
        }
    }

    let explicit_ids = parse_explicit_gate_ids(&strip_shell_comments(script));
    let explicit_set = to_set(&explicit_ids);
    for id in &explicit_ids {
        if !gate_by_id.contains_key(id.as_str()) {
            rep.violations.push(Violation::new(
                KIND_ORPHANED_RELEASE_ID,
                Some(id.clone()),
                "scripts/ci/release-gate.sh".to_string(),
                format!(
                    "release-gate.sh invokes gate id {:?}, which does not exist in docs/ci/CI_GATE_MANIFEST.json",
                    id
                ),
                format!(
                    "remove the stale gate id {:?} from the --gate list in scripts/ci/release-gate.sh, or restore it to the manifest",
                    id
                ),
            ));
        }
    }

    let fast_invocation_re =
        Regex::new(r"(?m)^\s*(?:if\s+)?bash\s+scripts/ci/verify-fast\.sh(?:\s|;|$)").unwrap();
    if !fast_invocation_re.is_match(&strip_shell_comments(script)) {
        rep.violations.push(Violation::new(
            KIND_MISSING_REQUIRED_GATE,
            Some("fast_tier".to_string()),
            "scripts/ci/release-gate.sh".to_string(),
            "release-gate.sh does not invoke scripts/ci/verify-fast.sh; the fast tier would not run on tag push".to_string(),
            "call `bash scripts/ci/verify-fast.sh` unconditionally from release-gate.sh before the full-tier gates".to_string(),
        ));
    }

    let export_surface = format!(
        "{}\n{}",
        export_job_surface(wf, export_job_name),
        strip_shell_comments(export_script)
    );
    let selftest_flag_re = Regex::new(r"--[a-zA-Z0-9][a-zA-Z0-9-]*-selftest\b").unwrap();
    let mut export_satisfied_ids: Vec<String> = Vec::new();
    let mut export_satisfied_set: HashSet<String> = HashSet::new();
    for g in &m.gates {
        let flag = selftest_flag_re
            .find(&g.command)
            .map(|m| m.as_str().to_string())
            .unwrap_or_default();
        if !flag.is_empty() && export_surface.contains(&flag) {
            export_satisfied_set.insert(g.gate_id.clone());
            export_satisfied_ids.push(g.gate_id.clone());
        }
    }
    export_satisfied_ids.sort();

    let mut workflow_executed = fast_set.clone();
    workflow_executed.extend(explicit_set.iter().cloned());
    workflow_executed.extend(export_satisfied_set.iter().cloned());
    for id in &required_ids {
        if !workflow_executed.contains(id) {
            rep.violations.push(Violation::new(
                KIND_MISSING_REQUIRED_GATE,
                Some(id.clone()),
                "scripts/ci/release-gate.sh".to_string(),
                format!(
                    "gate {:?} is release_required but is neither fast-tier, explicitly invoked by release-gate.sh, nor satisfied by the export job",
                    id
                ),
                format!(
                    "add {:?} to the run-gates.py --gate list in scripts/ci/release-gate.sh, or wire its selftest flag into the export job",
                    id
                ),
            ));
        }
    }

    rep.violations
        .extend(check_gate_job_step(wf, gate_job_name));
    rep.violations
        .extend(check_export_job(wf, gate_job_name, export_job_name));

    rep.metrics = Metrics {
        required_gate_count: required_ids.len() as i64,
        fast_tier_gate_count: fast_ids.len() as i64,
        explicit_full_gate_count: explicit_ids.len() as i64,
        workflow_executed_count: workflow_executed.len() as i64,
        violation_count: rep.violations.len() as i64,
    };
    rep.pass = rep.violations.is_empty();
    rep.evidence = vec![
        Evidence {
            description: "fast_tier_gate_ids".to_string(),
            detail: fast_ids.join(","),
        },
        Evidence {
            description: "release_required_gate_ids".to_string(),
            detail: required_ids.join(","),
        },
        Evidence {
            description: "explicit_full_tier_gate_ids".to_string(),
            detail: explicit_ids.join(","),
        },
        Evidence {
            description: "export_job_satisfied_gate_ids".to_string(),
            detail: export_satisfied_ids.join(","),
        },
    ];
    rep.remediation = collect_remediation(&rep.violations);
    rep
}

/// `git rev-parse HEAD` in `root`; `unknown` on any failure (as Go does).
fn commit_hash(root: &str) -> String {
    let dir = if std::path::Path::new(root).is_absolute() {
        std::path::PathBuf::from(root)
    } else {
        std::env::current_dir()
            .map(|c| c.join(root))
            .unwrap_or_else(|_| std::path::PathBuf::from(root))
    };
    let out = std::process::Command::new("git")
        .arg("rev-parse")
        .arg("HEAD")
        .current_dir(dir)
        .output();
    match out {
        Ok(o) if o.status.success() => String::from_utf8_lossy(&o.stdout).trim().to_string(),
        _ => "unknown".to_string(),
    }
}

/// Port of `Run` — load the four files and evaluate.
pub fn run(
    root: &str,
    workflow_rel: &str,
    script_rel: &str,
    export_script_rel: &str,
    manifest_rel: &str,
) -> Result<Report, String> {
    let m = load_manifest(&join(root, manifest_rel))?;
    let wf = load_workflow(&join(root, workflow_rel))?;
    let script_path = join(root, script_rel);
    let script = std::fs::read(&script_path)
        .map_err(|e| format!("read script {}: {}", script_path, e))?;
    // Best-effort: a missing export script is evaluated as empty content.
    let export_script = std::fs::read(join(root, export_script_rel)).unwrap_or_default();

    let mut rep = evaluate(
        &m,
        &wf,
        &String::from_utf8_lossy(&script),
        &String::from_utf8_lossy(&export_script),
        "release-gate",
        "export-release",
    );
    rep.workflow_path = workflow_rel.to_string();
    rep.script_path = script_rel.to_string();
    rep.export_script_path = export_script_rel.to_string();
    rep.manifest_path = manifest_rel.to_string();
    let (secs, _) = gotime::parts_from_system_time(std::time::SystemTime::now());
    rep.generated_at = gotime::format_rfc3339(secs, 0, 0);
    rep.commit = commit_hash(root);
    Ok(rep)
}

fn join(root: &str, rel: &str) -> String {
    std::path::Path::new(root)
        .join(rel)
        .to_string_lossy()
        .into_owned()
}

#[cfg(test)]
mod tests {
    use super::*;

    fn bool_ptr(b: bool) -> Option<bool> {
        Some(b)
    }

    fn good_manifest() -> Manifest {
        Manifest {
            gates: vec![
                ManifestGate {
                    gate_id: "whitespace_hygiene".into(),
                    classification: "fast".into(),
                    critical: bool_ptr(true),
                    command: "bash scripts/ci/no-whitespace-churn.sh".into(),
                    release_required: false,
                },
                ManifestGate {
                    gate_id: "export_parity".into(),
                    classification: "full".into(),
                    critical: bool_ptr(true),
                    command:
                        "bash scripts/ci/run-godot-bounded.sh --path . -- --export-parity-selftest"
                            .into(),
                    release_required: true,
                },
                ManifestGate {
                    gate_id: "test_core_suite".into(),
                    classification: "full".into(),
                    critical: bool_ptr(true),
                    command: "dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj".into(),
                    release_required: true,
                },
                ManifestGate {
                    gate_id: "save_support_window".into(),
                    classification: "full".into(),
                    critical: bool_ptr(true),
                    command: "dotnet test --filter SaveSupportWindowTests".into(),
                    release_required: true,
                },
            ],
        }
    }

    const GOOD_SCRIPT: &str = "#!/usr/bin/env bash\nset -euo pipefail\nbash scripts/ci/verify-fast.sh\npython3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window --report-json x.json\n";
    const GOOD_EXPORT_SCRIPT: &str = "#!/usr/bin/env bash\n\"${GODOT_RUNNER[@]}\" --path . -- --export-parity-selftest --parity-target \"$DIR/builds/linux\"\n";

    fn good_workflow() -> Workflow {
        let mut jobs = BTreeMap::new();
        jobs.insert(
            "release-gate".to_string(),
            WorkflowJob {
                steps: vec![
                    WorkflowStep {
                        name: "checkout".into(),
                        uses: "actions/checkout@v4".into(),
                        ..Default::default()
                    },
                    WorkflowStep {
                        name: "Run canonical release gate".into(),
                        run: "bash scripts/ci/release-gate.sh".into(),
                        ..Default::default()
                    },
                ],
                ..Default::default()
            },
        );
        let upload = |path: &str| WorkflowStep {
            name: "upload".into(),
            uses: "actions/upload-artifact@v4".into(),
            with: HashMap::from([
                ("path".to_string(), serde_yaml::Value::String(path.to_string())),
                (
                    "if-no-files-found".to_string(),
                    serde_yaml::Value::String("error".to_string()),
                ),
            ]),
            ..Default::default()
        };
        jobs.insert(
            "export-release".to_string(),
            WorkflowJob {
                needs: Some(serde_yaml::Value::String("release-gate".to_string())),
                steps: vec![
                    WorkflowStep {
                        name: "linux export".into(),
                        run: "bash scripts/ci/export-build.sh".into(),
                        ..Default::default()
                    },
                    WorkflowStep {
                        name: "windows export".into(),
                        run: "bash scripts/ci/run-godot-bounded.sh --path . --export-release \"Windows Desktop\" builds/windows/ashfall.exe".into(),
                        ..Default::default()
                    },
                    upload("builds/linux"),
                    upload("builds/windows"),
                ],
                ..Default::default()
            },
        );
        Workflow { jobs }
    }

    fn eval_good(wf: &Workflow, script: &str) -> Report {
        evaluate(
            &good_manifest(),
            wf,
            script,
            GOOD_EXPORT_SCRIPT,
            "release-gate",
            "export-release",
        )
    }

    fn has_kind(rep: &Report, kind: &str) -> bool {
        rep.violations.iter().any(|v| v.kind == kind)
    }

    fn by_kind_gate<'a>(rep: &'a Report, kind: &str, gate: &str) -> Option<&'a Violation> {
        rep.violations
            .iter()
            .find(|v| v.kind == kind && v.gate_id.as_deref() == Some(gate))
    }

    #[test]
    fn positive_case_has_no_violations() {
        let rep = eval_good(&good_workflow(), GOOD_SCRIPT);
        assert!(rep.pass, "{:?}", rep.violations);
        assert_eq!(rep.metrics.violation_count, 0);
        assert_eq!(rep.metrics.required_gate_count, 3);
    }

    #[test]
    fn export_job_satisfies_selftest_gate() {
        let rep = eval_good(&good_workflow(), GOOD_SCRIPT);
        assert!(by_kind_gate(&rep, KIND_MISSING_REQUIRED_GATE, "export_parity").is_none());
        let e = rep
            .evidence
            .iter()
            .find(|e| e.description == "export_job_satisfied_gate_ids")
            .unwrap();
        assert!(e.detail.contains("export_parity"), "{}", e.detail);
    }

    #[test]
    fn missing_required_gate_when_export_surface_lacks_flag() {
        let rep = evaluate(
            &good_manifest(),
            &good_workflow(),
            GOOD_SCRIPT,
            "",
            "release-gate",
            "export-release",
        );
        assert!(by_kind_gate(&rep, KIND_MISSING_REQUIRED_GATE, "export_parity").is_some());
    }

    #[test]
    fn explicit_gate_named_is_not_reported_missing() {
        let script = "#!/usr/bin/env bash\nbash scripts/ci/verify-fast.sh\npython3 scripts/ci/run-gates.py --gate test_core_suite --report-json x.json\n";
        let rep = evaluate(&good_manifest(), &good_workflow(), script, "", "release-gate", "export-release");
        assert!(!rep.pass);
        assert!(by_kind_gate(&rep, KIND_MISSING_REQUIRED_GATE, "save_support_window").is_some());
        assert!(by_kind_gate(&rep, KIND_MISSING_REQUIRED_GATE, "export_parity").is_some());
        assert!(by_kind_gate(&rep, KIND_MISSING_REQUIRED_GATE, "test_core_suite").is_none());
    }

    #[test]
    fn fast_tier_not_invoked_is_reported() {
        let script = "#!/usr/bin/env bash\npython3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window --report-json x.json\n";
        let rep = eval_good(&good_workflow(), script);
        assert!(by_kind_gate(&rep, KIND_MISSING_REQUIRED_GATE, "fast_tier").is_some());
    }

    #[test]
    fn nonblocking_gate_steps_are_reported() {
        let cases: Vec<Vec<WorkflowStep>> = vec![
            vec![WorkflowStep {
                run: "bash scripts/ci/release-gate.sh".into(),
                continue_on_error: Some(serde_yaml::Value::Bool(true)),
                ..Default::default()
            }],
            vec![WorkflowStep {
                run: "bash scripts/ci/release-gate.sh".into(),
                if_cond: "success()".into(),
                ..Default::default()
            }],
            vec![WorkflowStep {
                run: "bash scripts/ci/release-gate.sh --skip-full".into(),
                ..Default::default()
            }],
            vec![WorkflowStep {
                run: "echo hi".into(),
                ..Default::default()
            }],
        ];
        for steps in cases {
            let mut wf = good_workflow();
            let mut gate = wf.jobs.get("release-gate").cloned().unwrap();
            gate.steps = steps;
            wf.jobs.insert("release-gate".to_string(), gate);
            let rep = eval_good(&wf, GOOD_SCRIPT);
            assert!(has_kind(&rep, KIND_NONBLOCKING_STEP), "{:?}", rep.violations);
        }
    }

    #[test]
    fn commented_commands_do_not_prove_parity() {
        let script = "#!/usr/bin/env bash\n# bash scripts/ci/verify-fast.sh\n# python3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window\necho \"no gates run\"\n";
        let rep = evaluate(
            &good_manifest(),
            &good_workflow(),
            script,
            "# --export-parity-selftest",
            "release-gate",
            "export-release",
        );
        for id in ["fast_tier", "test_core_suite", "save_support_window", "export_parity"] {
            assert!(
                by_kind_gate(&rep, KIND_MISSING_REQUIRED_GATE, id).is_some(),
                "commented command satisfied {id}: {:?}",
                rep.violations
            );
        }
    }

    #[test]
    fn commented_but_canonical_gate_step_is_recognized() {
        let mut wf = good_workflow();
        let mut gate = wf.jobs.get("release-gate").cloned().unwrap();
        gate.steps = vec![WorkflowStep {
            name: "Run canonical release gate".into(),
            run: "# one canonical, blocking command\n\nbash scripts/ci/release-gate.sh\n".into(),
            ..Default::default()
        }];
        wf.jobs.insert("release-gate".to_string(), gate);
        let rep = eval_good(&wf, GOOD_SCRIPT);
        assert!(!has_kind(&rep, KIND_NONBLOCKING_STEP), "{:?}", rep.violations);
    }

    #[test]
    fn weakened_criticality_and_orphaned_release_id() {
        let mut m = good_manifest();
        for g in m.gates.iter_mut() {
            if g.gate_id == "export_parity" {
                g.critical = None;
            }
        }
        let rep = evaluate(&m, &good_workflow(), GOOD_SCRIPT, GOOD_EXPORT_SCRIPT, "release-gate", "export-release");
        assert!(by_kind_gate(&rep, KIND_WEAKENED_CRITICALITY, "export_parity").is_some());

        let script = "#!/usr/bin/env bash\nbash scripts/ci/verify-fast.sh\npython3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window,ghost_gate --report-json x.json\n";
        let rep = eval_good(&good_workflow(), script);
        assert!(by_kind_gate(&rep, KIND_ORPHANED_RELEASE_ID, "ghost_gate").is_some());
    }

    #[test]
    fn export_orphan_cases_and_no_false_positive() {
        let good_export_steps = good_workflow().jobs.get("export-release").cloned().unwrap().steps;
        let no_needs = WorkflowJob {
            steps: good_export_steps.clone(),
            ..Default::default()
        };
        let mut wf = good_workflow();
        wf.jobs.insert("export-release".to_string(), no_needs);
        assert!(has_kind(&eval_good(&wf, GOOD_SCRIPT), KIND_EXPORT_ORPHAN));

        // No strict if-no-files-found.
        let mut steps = good_export_steps.clone();
        steps[2].with.remove("if-no-files-found");
        let mut wf = good_workflow();
        wf.jobs.insert(
            "export-release".to_string(),
            WorkflowJob {
                needs: Some(serde_yaml::Value::String("release-gate".into())),
                steps,
                ..Default::default()
            },
        );
        assert!(has_kind(&eval_good(&wf, GOOD_SCRIPT), KIND_EXPORT_ORPHAN));

        // Windows export before Linux export.
        let mut wf = good_workflow();
        let mut steps = good_export_steps.clone();
        steps.swap(0, 1);
        wf.jobs.insert(
            "export-release".to_string(),
            WorkflowJob {
                needs: Some(serde_yaml::Value::String("release-gate".into())),
                steps,
                ..Default::default()
            },
        );
        assert!(has_kind(&eval_good(&wf, GOOD_SCRIPT), KIND_EXPORT_ORPHAN));

        // Upload before its export step.
        let mut wf = good_workflow();
        let mut steps = good_export_steps.clone();
        let upload = steps.pop().unwrap();
        steps.insert(0, upload);
        wf.jobs.insert(
            "export-release".to_string(),
            WorkflowJob {
                needs: Some(serde_yaml::Value::String("release-gate".into())),
                steps,
                ..Default::default()
            },
        );
        assert!(has_kind(&eval_good(&wf, GOOD_SCRIPT), KIND_EXPORT_ORPHAN));
    }

    #[test]
    fn parse_explicit_gate_ids_and_canonical_command_line() {
        let script = "python3 scripts/ci/run-gates.py --gate  a,b, c  --report-json out.json\nsome other line --gate d\n";
        assert_eq!(parse_explicit_gate_ids(script).join(","), "a,b,c,d");

        assert_eq!(
            canonical_command_line("bash scripts/ci/release-gate.sh"),
            "bash scripts/ci/release-gate.sh"
        );
        assert_eq!(
            canonical_command_line("# comment\n\nbash scripts/ci/release-gate.sh\n"),
            "bash scripts/ci/release-gate.sh"
        );
        assert_eq!(
            canonical_command_line("bash scripts/ci/release-gate.sh --skip-full"),
            "bash scripts/ci/release-gate.sh --skip-full"
        );
        assert_eq!(canonical_command_line("echo hi"), "");
    }
}