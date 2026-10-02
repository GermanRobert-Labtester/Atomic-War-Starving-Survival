//! ASHFALL development & CI tool suite — Rust port of `tools/gotools`.
//!
//! Stage 1 implements the test-tooling cluster: `select-tests`, `run-tasks`,
//! and `run-scoped-tests`. Stage 2 adds the validation cluster: `validate-json`,
//! `validate-config`, `parse-results`, `scan-saves`, `build-manifest`, and
//! `index`. Stage 3 adds the governance/monitor cluster: `check-plan`,
//! `sync-agents`, `audit-catalogs`, `releasepolicy`, `monitor-size`,
//! `monitor-compile`, `llm-proxy`, and `agent-core`. The two resident servers
//! (`llm-proxy`, `agent-core`) and the resident indexer exit 2 with an explicit
//! unsupported notice rather than pretending to serve; see the module headers.
//! Cutover + removal of `tools/gotools/` is Stage 4
//! (`.ai/plans/rust-port-gotools-2026-10-02.md`).

mod agentsync;
mod catalogaudit;
mod checkplan;
mod config;
mod gotime;
mod gowalk;
mod indexer;
mod jsonout;
mod manifest;
mod monitor;
mod orchestrator;
mod parser;
mod proxy;
mod reachability;
mod releasepolicy;
mod runner;
mod scanner;
mod scopedtest;
mod selector;
mod validator;

use std::collections::{BTreeMap, HashMap, HashSet};
use std::io::Read;
use std::path::{Path, PathBuf};

#[derive(Default)]
struct Flags {
    values: HashMap<String, String>,
    bools: HashSet<String>,
    rest: Vec<String>,
}

impl Flags {
    fn get(&self, name: &str) -> Option<&str> {
        self.values.get(name).map(|s| s.as_str())
    }
    fn or<'a>(&'a self, name: &str, default: &'a str) -> &'a str {
        self.get(name).unwrap_or(default)
    }
    fn is_set(&self, name: &str) -> bool {
        self.bools.contains(name)
    }
}

/// Parse leading flags the way Go's `flag` package does: `-x`, `--x`, `-x=v`,
/// `-x v`, stopping at the first non-flag argument.
fn parse_flags(args: &[String], value_flags: &[&str], bool_flags: &[&str]) -> Flags {
    let mut f = Flags::default();
    let mut i = 0;
    while i < args.len() {
        let a = &args[i];
        if !a.starts_with('-') || a == "-" {
            break;
        }
        let stripped = a.trim_start_matches('-');
        if stripped.is_empty() {
            break;
        }
        let (name, inline) = match stripped.split_once('=') {
            Some((n, v)) => (n.to_string(), Some(v.to_string())),
            None => (stripped.to_string(), None),
        };
        if bool_flags.contains(&name.as_str()) {
            f.bools.insert(name);
            i += 1;
        } else if value_flags.contains(&name.as_str()) {
            match inline {
                Some(v) => {
                    f.values.insert(name, v);
                    i += 1;
                }
                None => {
                    if i + 1 < args.len() {
                        f.values.insert(name, args[i + 1].clone());
                        i += 2;
                    } else {
                        i += 1;
                    }
                }
            }
        } else {
            i += 1;
        }
    }
    f.rest = args[i..].to_vec();
    f
}

fn print_usage() {
    println!(
        "ASHFALL Development & CI Tool Suite (Rust)\n\
         \n\
         Usage:\n  ashfall-dev <command> [options]\n\
         \n\
         Commands:\n  \
         index            Fast repository file indexer (supports --watch and --serve)\n  \
         select-tests     Changed-file / changed-test selector enforcing test hierarchy\n  \
         run-scoped-tests Runs targeted tests mapped to changed files (enforces no-full-test policy)\n  \
         check-plan       Verifies approved plan in .ai/plans/ for code changes\n  \
         parse-results    Test-result parser with taxonomy classification and markdown summary\n  \
         validate-json    Fast JSON schema & UTF-8 validator (sub-second runtime)\n  \
         validate-config  JSON Schema validator for quest, save, and config files (JSON/YAML)\n  \
         audit-catalogs   Read-only catalog-hygiene audit (reference integrity, duplicate ids,\n                   id naming, schema_version drift) with a ratcheting findings baseline\n  \
         scan-saves       Save-file & save-store contract scanner\n  \
         build-manifest   Asset manifest builder with image dimensions and SHA256 hashes\n  \
         run-tasks        Parallel subprocess / task runner with timeout and RSS metrics\n  \
         llm-proxy        Lightweight local LLM API proxy and router\n  \
         sync-agents      Synchronize and check drift across all 13 client agent rulebooks\n  \
         agent-core       Resident long-running AI agent orchestrator (< 20 MB RAM)\n  \
         monitor-size     Repository tracked-blob size/growth monitor (base vs HEAD)\n  \
         monitor-compile  Compile-set / test-only-production-source monitor (MSBuild-evaluated)\n  \
         releasepolicy    Release workflow / required-gate parity check\n\
         \n\
         Note: the resident modes (index --watch/--serve, llm-proxy, agent-core) are not\n\
         ported yet and exit 2; use tools/gotools for those long-running servers.\n\
         \n\
         Use \"ashfall-dev <command> -h\" for options on a specific command."
    );
}

fn main() {
    let argv: Vec<String> = std::env::args().collect();
    if argv.len() < 2 {
        print_usage();
        std::process::exit(1);
    }

    let cmd = argv[1].as_str();
    let args = &argv[2..];

    match cmd {
        "index" => run_index(args),
        "select-tests" => run_select_tests(args),
        "run-scoped-tests" => run_run_scoped_tests(args),
        "check-plan" => run_check_plan(args),
        "parse-results" => run_parse_results(args),
        "validate-json" => run_validate_json(args),
        "validate-config" => run_validate_config(args),
        "audit-catalogs" => run_audit_catalogs(args),
        "scan-saves" => run_scan_saves(args),
        "build-manifest" => run_build_manifest(args),
        "run-tasks" => run_tasks(args),
        "llm-proxy" => run_llm_proxy(args),
        "sync-agents" => run_sync_agents(args),
        "agent-core" => run_agent_core(args),
        "monitor-size" => run_monitor_size(args),
        "monitor-compile" => run_monitor_compile(args),
        "releasepolicy" => run_releasepolicy(args),
        "reachability-report" => run_reachability_report(args),
        "-h" | "--help" | "help" => print_usage(),
        _ => {
            eprintln!("Unknown command: {}\n", cmd);
            print_usage();
            std::process::exit(1);
        }
    }
}

fn run_reachability_report(args: &[String]) {
    let f = parse_flags(args, &["root", "out"], &[]);
    let root = f.or("root", ".");
    let out = f.or("out", "");
    if let Err(e) = reachability::run(root, out) {
        eprintln!("reachability-report: {}", e);
        std::process::exit(1);
    }
}

fn run_index(args: &[String]) {
    let f = parse_flags(
        args,
        &["root", "serve"],
        &["hash", "json", "list", "watch"],
    );
    let root = f.or("root", ".");
    let compute_hashes = f.is_set("hash");
    let include_file_list = f.is_set("list");

    if !f.or("serve", "").is_empty() || f.is_set("watch") {
        eprintln!("{}", indexer::RESIDENT_UNSUPPORTED);
        std::process::exit(2);
    }

    let report = match indexer::index_repository(root, compute_hashes, include_file_list) {
        Ok(r) => r,
        Err(e) => {
            eprintln!("Error indexing repository: {}", e);
            std::process::exit(1);
        }
    };

    if f.is_set("json") {
        println!("{}", report.to_json(true));
    } else {
        print!("{}", report.summary());
    }
}

fn run_parse_results(args: &[String]) {
    let f = parse_flags(args, &["exit-code"], &["json"]);
    let code: i32 = f.get("exit-code").and_then(|s| s.parse().ok()).unwrap_or(0);

    let mut input = String::new();
    if let Err(e) = std::io::stdin().read_to_string(&mut input) {
        eprintln!("Failed to read input from stdin: {}", e);
        std::process::exit(1);
    }

    let parsed = parser::parse_test_output(&input, code);
    if f.is_set("json") {
        println!("{}", parsed.to_json());
    } else {
        println!("{}", parsed.markdown_report());
    }
}

fn run_validate_json(args: &[String]) {
    let f = parse_flags(args, &["dir"], &["json"]);
    let data_dir = f.or("dir", "Assets/StreamingAssets/Data");

    let summary = match validator::validate_directory(data_dir) {
        Ok(s) => s,
        Err(e) => {
            eprintln!("Error validating directory {}: {}", data_dir, e);
            std::process::exit(1);
        }
    };

    if f.is_set("json") {
        println!("{}", summary.to_json_pretty());
        return;
    }

    println!("=== Fast JSON Schema Validator ===");
    println!(
        "Checked {} files in {} ms | Valid: {} | Violations: {}",
        summary.total_checked, summary.duration_ms, summary.valid_count, summary.invalid_count
    );
    if summary.invalid_count > 0 {
        for v in &summary.violations {
            println!("  ❌ [{}] {}: {}", v.rule, v.file_path, v.message);
        }
        std::process::exit(1);
    }
    println!("✅ All catalog files adhere strictly to schema policy (schema_version >= 1, root object {{}}).");
}

/// Go's `runValidateConfig` does no flag parsing: the first two positional
/// arguments are the schema and instance paths verbatim.
fn run_validate_config(args: &[String]) {
    if args.len() < 2 {
        eprintln!("Usage: ashfall-dev validate-config <schema.json> <file.[json|yaml]>");
        std::process::exit(2);
    }
    let schema_path = &args[0];
    let instance_path = &args[1];

    if let Err(err) = config::validate_file(schema_path, instance_path) {
        for line in config::pretty_validation_error(&err) {
            eprintln!("{}", line);
        }
        std::process::exit(1);
    }
    println!("OK");
}

fn run_scan_saves(args: &[String]) {
    let f = parse_flags(args, &["root", "save-dir"], &["json"]);
    let root = f.or("root", ".");
    let save_dir = f.or("save-dir", "saves");

    let stores = scanner::discover_save_stores(root);
    let files = scanner::scan_save_directory(&scanner::go_join(root, save_dir));

    let report = scanner::SaveAuditReport {
        store_count: stores.len(),
        file_count: files.len(),
        discovered_stores: stores,
        scanned_files: files,
    };

    if f.is_set("json") {
        println!("{}", report.to_json_pretty());
        return;
    }

    println!("=== Save-File & Store Scanner ===");
    println!(
        "Discovered {} SaveStore declarations in C# codebase.",
        report.store_count
    );
    for s in &report.discovered_stores {
        println!(
            "  - {:<32} (Section: {:<20}, Path: {})",
            s.class_name, s.section_name, s.save_path
        );
    }
    if report.file_count > 0 {
        println!("\nScanned {} active save file(s):", report.file_count);
        for file in &report.scanned_files {
            println!(
                "  - {} (Ver: {}, SHA: {:.8}, Status: {})",
                file.file_path,
                file.schema_version,
                file.computed_sha256,
                file.status
            );
        }
    }
}

fn run_build_manifest(args: &[String]) {
    let f = parse_flags(args, &["root", "out"], &[]);
    let root = f.or("root", ".");
    let out_path = f.or("out", "assets/ASSET_MANIFEST_GENERATED.json");

    let dirs = ["assets", "Assets"];
    let m = manifest::build_manifest(root, &dirs);

    let full_out = scanner::go_join(root, out_path);
    if let Some(parent) = std::path::Path::new(&full_out).parent() {
        if let Err(e) = std::fs::create_dir_all(parent) {
            eprintln!("Error saving manifest: {}", e);
            std::process::exit(1);
        }
    }
    if let Err(e) = m.save_json(&full_out) {
        eprintln!("Error saving manifest: {}", e);
        std::process::exit(1);
    }

    println!("=== Asset Manifest Builder ===");
    println!(
        "Indexed {} assets ({:.2} MB) -> Saved to {}",
        m.total_assets,
        m.total_bytes as f64 / (1024.0 * 1024.0),
        out_path
    );
}

fn run_select_tests(args: &[String]) {
    let f = parse_flags(args, &["root", "ref"], &["json"]);
    let root = f.or("root", ".");
    let compare_ref = f.or("ref", "");

    let changed = selector::get_changed_files(root, compare_ref);
    let plan = selector::select_tests_for_files(root, &changed);

    if f.is_set("json") {
        println!("{}", plan.to_json_pretty());
    } else {
        println!(
            "=== Changed-Test Selector (Found {} changed files) ===",
            plan.changed_files.len()
        );
        println!("{}", plan.explanation);
        if !plan.selected_tests.is_empty() {
            println!("\nRecommended Targeted Commands (Hierarchical):");
            for t in &plan.selected_tests {
                println!("  [{}] {}\n    -> Run: {}", t.tier, t.target_file, t.command);
            }
        }
    }
}

fn run_tasks(args: &[String]) {
    let f = parse_flags(args, &["j"], &["json"]);
    let concurrency: usize = f.get("j").and_then(|s| s.parse().ok()).unwrap_or(4);

    if f.is_set("json") {
        let mut input = String::new();
        if let Err(e) = std::io::stdin().read_to_string(&mut input) {
            eprintln!("Error reading task JSON: {}", e);
            std::process::exit(2);
        }
        let tasks: Vec<runner::Task> = match serde_json::from_str(&input) {
            Ok(t) => t,
            Err(e) => {
                eprintln!("Error reading task JSON: {}", e);
                std::process::exit(2);
            }
        };
        let results = runner::run_tasks(&tasks, concurrency);
        match serde_json::to_string(&results) {
            // Go's encoding/json escapes `<`, `>`, and `&`; task commands and
            // captured output routinely contain `&&`, so route through the
            // Go-compatible escaper to stay byte-identical.
            Ok(json) => println!("{}", jsonout::escape_html_in_json(&json)),
            Err(e) => {
                eprintln!("Error writing task results: {}", e);
                std::process::exit(2);
            }
        }
        if results.iter().any(|r| r.exit_code != 0) {
            std::process::exit(1);
        }
        return;
    }

    // Example default task when no sub-commands supplied (mirrors Go).
    let self_exe = std::env::args().next().unwrap_or_else(|| "ashfall-dev".to_string());
    let tasks = vec![
        runner::Task {
            id: "json_validation".into(),
            command: self_exe.clone(),
            args: vec!["validate-json".into()],
            working_dir: String::new(),
            timeout: 10_000_000_000,
        },
        runner::Task {
            id: "index_repo".into(),
            command: self_exe,
            args: vec!["index".into()],
            working_dir: String::new(),
            timeout: 15_000_000_000,
        },
    ];
    let results = runner::run_tasks(&tasks, concurrency);
    println!("=== Parallel Task Runner (Workers: {}) ===", concurrency);
    for r in &results {
        let status = if r.exit_code == 0 {
            "PASS".to_string()
        } else {
            format!("FAIL (code {})", r.exit_code)
        };
        println!(
            "  - [{}] Task: {:<18} Duration: {:<8} Peak RSS: {} KB",
            status,
            r.id,
            format!("{:?}", std::time::Duration::from_nanos(r.duration as u64)),
            r.peak_rss_kb
        );
    }
}

fn run_run_scoped_tests(args: &[String]) {
    let f = parse_flags(
        args,
        &["root", "ref", "user-override"],
        &["dry-run", "full"],
    );

    let opts = scopedtest::RunOptions {
        repo_root: f.or("root", ".").to_string(),
        files: f.rest.clone(),
        compare_ref: f.or("ref", "").to_string(),
        dry_run: f.is_set("dry-run"),
        run_full_tests: f.is_set("full"),
        user_override: f.or("user-override", "").to_string(),
    };

    match scopedtest::run_scoped_tests(&opts) {
        Ok(res) => {
            if res.total_failed > 0 {
                std::process::exit(1);
            }
        }
        Err(e) => {
            eprintln!("\n[run-scoped-tests] ERROR: {}\n", e);
            std::process::exit(2);
        }
    }
}

fn run_check_plan(args: &[String]) {
    let f = parse_flags(args, &["root", "ref"], &["staged"]);
    let root = f.or("root", ".");
    match checkplan::check_approved_plan(root, f.is_set("staged"), f.or("ref", ""), &f.rest) {
        Ok((true, msg)) => println!("[check-plan] OK: {}", msg),
        Ok((false, msg)) => {
            eprintln!("\n[check-plan] COMMIT/CI REJECTED:\n{}\n", msg);
            std::process::exit(1);
        }
        Err(e) => {
            eprintln!("[check-plan] ERROR: {}", e);
            std::process::exit(2);
        }
    }
}

fn run_sync_agents(args: &[String]) {
    let f = parse_flags(args, &["root"], &["check"]);
    let check_only = f.is_set("check");
    match agentsync::sync_agent_rulebooks(f.or("root", "."), check_only) {
        Ok((ok, updated)) => {
            if check_only {
                if !ok {
                    println!("❌ Drift detected across agent rulebooks.");
                    std::process::exit(1);
                }
                println!("OK: All 13 client rulebooks are in sync with AGENTS.md.");
            } else {
                for file in &updated {
                    println!("Updated {}", file);
                }
                println!("Wrote docs/agents/AGENTS_SYNC_REPORT.md");
            }
        }
        Err(e) => {
            eprintln!("Error synchronizing agent rulebooks: {}", e);
            std::process::exit(1);
        }
    }
}

/// `filepath.Abs` for a CLI root, erroring with Go's message shape.
fn abs_root(root_flag: &str) -> String {
    match std::path::absolute(root_flag) {
        Ok(p) => p.to_string_lossy().into_owned(),
        Err(e) => {
            eprintln!("[audit-catalogs] ERROR: resolve root: {}", e);
            std::process::exit(2);
        }
    }
}

/// Resolve a policy/baseline path against the root, as Go's CLI does.
fn resolve_against_root(root: &str, explicit: &str, default_rel: &[&str]) -> String {
    if explicit.is_empty() {
        let mut p = PathBuf::from(root);
        for part in default_rel {
            p.push(part);
        }
        p.to_string_lossy().into_owned()
    } else if Path::new(explicit).is_absolute() {
        explicit.to_string()
    } else {
        Path::new(root)
            .join(explicit)
            .to_string_lossy()
            .into_owned()
    }
}

fn opt_len<T>(v: &Option<Vec<T>>) -> usize {
    v.as_ref().map(|v| v.len()).unwrap_or(0)
}

fn run_audit_catalogs(args: &[String]) {
    let f = parse_flags(
        args,
        &[
            "root",
            "policy",
            "baseline",
            "report-json",
            "dump-ids",
            "advisory-check",
        ],
        &[
            "json",
            "check",
            "update-baseline",
            "strict-stale",
            "fail-on-advisory",
            "summary",
            "list-checks",
            "list-advisories",
            "dump-duplicates",
        ],
    );

    let root = abs_root(f.or("root", "."));
    let policy = resolve_against_root(&root, f.or("policy", ""), &["docs", "ci", "catalog_audit_policy.json"]);
    let baseline = resolve_against_root(&root, f.or("baseline", ""), &["docs", "ci", "catalog_audit_baseline.json"]);

    let pol = match catalogaudit::load_policy(&policy) {
        Ok(p) => p,
        Err(e) => {
            eprintln!("[audit-catalogs] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    if f.is_set("list-checks") {
        println!("audit-catalogs checks:");
        println!("  reference_integrity    declared reference fields resolve to a target id-domain (D01)");
        println!("  duplicate_ids          an id defined across two different id-domains (D02)");
        println!("  id_naming              ids conform to the canonical snake_case regex (D03)");
        println!("  schema_version_drift   a catalog's schema_version matches its expected value (D04)");
        println!("  mirror_resolution_ratchet  mirror ids without a primary author stay under the policy ceiling");
        println!("  policy_domain / reference_allowlist  dead-policy and stale-allowlist detection");
        println!("advisories (never fail): id_unit_suffix, mirror_resolution");
        println!("enforced reference rules:");
        for r in &pol.reference_rules {
            let mut scope = format!("{}.{}", r.source_glob, r.field);
            if !r.container.is_empty() {
                scope.push_str(&format!("[{}]", r.container));
            }
            let allow = if !r.allowlist.is_empty() {
                format!(" (allowlist: {})", r.allowlist.join(", "))
            } else {
                String::new()
            };
            println!("  {} -> {}{}", scope, r.target_domain, allow);
        }
        println!("excluded reference rules:");
        for r in &pol.reference_rules_excluded {
            println!("  {}.{} — {}", r.source_glob, r.field, r.reason);
        }
        return;
    }

    if !f.or("dump-ids", "").is_empty() {
        let domain = f.or("dump-ids", "");
        match catalogaudit::domain_ids(&root, &pol, domain) {
            Ok(ids) => {
                for id in ids {
                    println!("{}", id);
                }
            }
            Err(e) => {
                eprintln!("[audit-catalogs] ERROR: {}", e);
                std::process::exit(2);
            }
        }
        return;
    }

    if f.is_set("dump-duplicates") {
        match catalogaudit::duplicate_ids(&root, &pol) {
            Ok(dupes) => {
                for d in dupes {
                    println!("{}\t{}", d.id, d.domains.join(","));
                }
            }
            Err(e) => {
                eprintln!("[audit-catalogs] ERROR: {}", e);
                std::process::exit(2);
            }
        }
        return;
    }

    let base = match catalogaudit::load_baseline(&baseline) {
        Ok(b) => b,
        Err(e) => {
            eprintln!("[audit-catalogs] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    let report = match catalogaudit::run(&root, &pol, &base) {
        Ok(r) => r,
        Err(e) => {
            eprintln!("[audit-catalogs] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    if f.is_set("update-baseline") {
        if let Err(e) = catalogaudit::write_baseline(&baseline, catalogaudit::baseline_from_report(&report)) {
            eprintln!("[audit-catalogs] ERROR: write baseline: {}", e);
            std::process::exit(2);
        }
        println!(
            "CATALOG_AUDIT baseline updated: {} ({} findings)",
            baseline,
            opt_len(&report.accepted) + report.findings.len()
        );
        return;
    }

    if f.is_set("list-advisories") {
        let advisory_check = f.or("advisory-check", "");
        for a in report.advisories.as_deref().unwrap_or(&[]) {
            if !advisory_check.is_empty() && a.check != advisory_check {
                continue;
            }
            println!(
                "{}\t{}\t{}",
                a.check,
                a.key,
                a.detail.as_deref().unwrap_or("")
            );
        }
        return;
    }

    if f.is_set("json") {
        println!("{}", report.to_json_pretty());
    } else if !f.is_set("summary") {
        println!(
            "[audit-catalogs] scanned {} catalogs, indexed {} ids",
            report.files_scanned, report.ids_indexed
        );
        for finding in &report.findings {
            println!(
                "  ✗ [{}] {} — {}",
                finding.check,
                finding.key,
                finding.detail.as_deref().unwrap_or("")
            );
        }
        for a in report.advisories.as_deref().unwrap_or(&[]) {
            println!(
                "  · [{}] {} — {}",
                a.check,
                a.key,
                a.detail.as_deref().unwrap_or("")
            );
        }
        if let Some(m) = &report.mirror_resolution {
            println!(
                "[audit-catalogs] mirror resolution: {} mirror id(s) lack a primary-domain author (advisory; {} mirror catalog(s))",
                m.mirror_unresolved, m.mirror_catalogs
            );
        }
        let stale = report.stale_baseline.as_deref().unwrap_or(&[]);
        if !stale.is_empty() {
            println!(
                "[audit-catalogs] {} stale baseline entries (safe to prune):",
                stale.len()
            );
            for s in stale {
                println!("  · {}", s);
            }
        }
    }

    for f in report.parse_errors.as_deref().unwrap_or(&[]) {
        eprintln!(
            "[audit-catalogs] unreadable catalog {}: {}",
            f.file.as_deref().unwrap_or(""),
            f.detail.as_deref().unwrap_or("")
        );
    }

    let report_json = f.or("report-json", "");
    if !report_json.is_empty() {
        let mut data = report.to_json_pretty();
        data.push('\n');
        if let Err(e) = std::fs::write(report_json, data) {
            eprintln!("[audit-catalogs] ERROR: write report: {}", e);
            std::process::exit(2);
        }
    }

    if f.is_set("summary") {
        let (mirror_unresolved, mirror_by_catalog) = match &report.mirror_resolution {
            Some(m) => (
                m.mirror_unresolved,
                m.by_catalog.clone().unwrap_or_default(),
            ),
            None => (0, BTreeMap::new()),
        };
        let mut s: BTreeMap<String, serde_json::Value> = BTreeMap::new();
        s.insert("files_scanned".into(), serde_json::json!(report.files_scanned));
        s.insert("findings".into(), serde_json::json!(report.findings.len()));
        s.insert("acknowledged".into(), serde_json::json!(opt_len(&report.accepted)));
        s.insert("advisories".into(), serde_json::json!(opt_len(&report.advisories)));
        s.insert("parse_errors".into(), serde_json::json!(opt_len(&report.parse_errors)));
        s.insert("stale".into(), serde_json::json!(opt_len(&report.stale_baseline)));
        s.insert("mirror_unresolved".into(), serde_json::json!(mirror_unresolved));
        s.insert("mirror_by_catalog".into(), serde_json::json!(mirror_by_catalog));
        s.insert(
            "reference_rules".into(),
            serde_json::json!(report.reference_rule_counts.clone().unwrap_or_default()),
        );
        println!("{}", jsonout::to_compact(&s));
    }

    if f.is_set("check") {
        let stale_fail = f.is_set("strict-stale") && report.has_stale();
        let mut advisory_fail = false;
        if f.is_set("fail-on-advisory") {
            let allowed: HashSet<&str> = pol.advisory_allowlist.iter().map(|s| s.as_str()).collect();
            for a in report.advisories.as_deref().unwrap_or(&[]) {
                if !allowed.contains(a.key.as_str()) {
                    advisory_fail = true;
                    break;
                }
            }
        }
        if !report.findings.is_empty()
            || opt_len(&report.parse_errors) > 0
            || stale_fail
            || advisory_fail
        {
            println!(
                "CATALOG_AUDIT FAIL ({} new finding(s), {} unreadable catalog(s), {} stale acknowledgement(s), {} advisory(ies))",
                report.findings.len(),
                opt_len(&report.parse_errors),
                opt_len(&report.stale_baseline),
                opt_len(&report.advisories)
            );
            if !report.findings.is_empty() {
                println!(
                    "[audit-catalogs] after review, re-record with --update-baseline (baseline: {})",
                    baseline
                );
            }
            std::process::exit(1);
        }
        println!(
            "CATALOG_AUDIT PASS ({} catalog(s), {} acknowledged finding(s), {} advisory(ies))",
            report.files_scanned,
            opt_len(&report.accepted),
            opt_len(&report.advisories)
        );
        return;
    }
    if !report.findings.is_empty() || opt_len(&report.parse_errors) > 0 {
        std::process::exit(1);
    }
}

fn run_llm_proxy(args: &[String]) {
    let f = parse_flags(args, &["addr"], &[]);
    let _ = f.or("addr", "127.0.0.1:8088");
    eprintln!("{}", proxy::UNSUPPORTED);
    std::process::exit(2);
}

fn run_agent_core(args: &[String]) {
    let f = parse_flags(args, &["addr", "root"], &[]);
    let _ = f.or("addr", "127.0.0.1:8082");
    let _ = f.or("root", ".");
    eprintln!("{}", orchestrator::UNSUPPORTED);
    std::process::exit(2);
}

/// `resolveMonitorRootAndPolicy`: absolute root + default policy location.
fn resolve_monitor_root_and_policy(root_flag: &str, policy_flag: &str) -> Result<(String, String), String> {
    let abs_root = std::path::absolute(root_flag)
        .map_err(|e| format!("resolve --root {:?}: {}", root_flag, e))?
        .to_string_lossy()
        .into_owned();
    let policy = if policy_flag.is_empty() {
        Path::new(&abs_root)
            .join("docs")
            .join("ci")
            .join("MONITORING_POLICY.json")
            .to_string_lossy()
            .into_owned()
    } else if Path::new(policy_flag).is_absolute() {
        policy_flag.to_string()
    } else {
        Path::new(&abs_root)
            .join(policy_flag)
            .to_string_lossy()
            .into_owned()
    };
    Ok((abs_root, policy))
}

fn run_monitor_size(args: &[String]) {
    let f = parse_flags(args, &["root", "base", "out", "policy"], &[]);
    let base_ref = f.or("base", "").to_string();
    let out = f.or("out", "").to_string();
    if base_ref.trim().is_empty() {
        eprintln!("[monitor-size] ERROR: --base is required (growth cannot be computed without a base ref; this monitor fails closed rather than skipping the comparison)");
        std::process::exit(2);
    }
    if out.trim().is_empty() {
        eprintln!("[monitor-size] ERROR: --out is required");
        std::process::exit(2);
    }

    let (abs_root, policy) = match resolve_monitor_root_and_policy(f.or("root", "."), f.or("policy", "")) {
        Ok(v) => v,
        Err(e) => {
            eprintln!("[monitor-size] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    let report = match monitor::run_size_monitor(&monitor::SizeOptions {
        root: abs_root,
        base_ref,
        policy_path: policy,
    }) {
        Ok(r) => r,
        Err(e) => {
            eprintln!("[monitor-size] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    if let Err(e) = report.write_json(&out) {
        eprintln!("[monitor-size] ERROR writing report to {}: {}", out, e);
        std::process::exit(2);
    }
    println!("[monitor-size] Report written to {} (passed={})", out, report.passed);
    if report.has_error() {
        std::process::exit(1);
    }
}

fn run_monitor_compile(args: &[String]) {
    let f = parse_flags(args, &["root", "out", "policy"], &[]);
    let out = f.or("out", "").to_string();
    if out.trim().is_empty() {
        eprintln!("[monitor-compile] ERROR: --out is required");
        std::process::exit(2);
    }

    let (abs_root, policy) = match resolve_monitor_root_and_policy(f.or("root", "."), f.or("policy", "")) {
        Ok(v) => v,
        Err(e) => {
            eprintln!("[monitor-compile] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    let report = match monitor::run_compile_monitor(&monitor::CompileOptions {
        root: abs_root,
        policy_path: policy,
    }) {
        Ok(r) => r,
        Err(e) => {
            eprintln!("[monitor-compile] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    if let Err(e) = report.write_json(&out) {
        eprintln!("[monitor-compile] ERROR writing report to {}: {}", out, e);
        std::process::exit(2);
    }
    println!("[monitor-compile] Report written to {} (passed={})", out, report.passed);
    if report.has_error() {
        std::process::exit(1);
    }
}

fn run_releasepolicy(args: &[String]) {
    let f = parse_flags(
        args,
        &["root", "workflow", "script", "export-script", "manifest", "out"],
        &[],
    );
    let out = f.or("out", "").to_string();
    if out.is_empty() {
        eprintln!("[releasepolicy] ERROR: --out is required");
        std::process::exit(2);
    }

    let rep = match releasepolicy::run(
        f.or("root", "."),
        f.or("workflow", ".github/workflows/release.yml"),
        f.or("script", "scripts/ci/release-gate.sh"),
        f.or("export-script", "scripts/ci/export-build.sh"),
        f.or("manifest", "docs/ci/CI_GATE_MANIFEST.json"),
    ) {
        Ok(r) => r,
        Err(e) => {
            eprintln!("[releasepolicy] ERROR: {}", e);
            std::process::exit(2);
        }
    };

    if let Err(e) = rep.write_json(&out) {
        eprintln!("[releasepolicy] ERROR writing report to {}: {}", out, e);
        std::process::exit(2);
    }

    if !rep.pass {
        eprintln!(
            "[releasepolicy] FAIL — {} violation(s); see {}",
            rep.metrics.violation_count, out
        );
        for v in &rep.violations {
            eprintln!("  - [{}] {}: {}", v.kind, v.location, v.detail);
        }
        std::process::exit(1);
    }

    println!(
        "[releasepolicy] PASS — {} release_required gate(s) all reachable from the release workflow (report: {})",
        rep.metrics.required_gate_count, out
    );
}

#[cfg(test)]
mod tests {
    use super::*;

    fn v(args: &[&str]) -> Vec<String> {
        args.iter().map(|s| s.to_string()).collect()
    }

    #[test]
    fn parses_value_and_bool_flags() {
        let f = parse_flags(&v(&["-root", "/x", "--json", "-ref=HEAD"]), &["root", "ref"], &["json"]);
        assert_eq!(f.or("root", "."), "/x");
        assert_eq!(f.or("ref", ""), "HEAD");
        assert!(f.is_set("json"));
    }

    #[test]
    fn stops_at_first_positional() {
        let f = parse_flags(
            &v(&["-dry-run", "Ashfall.Core.Tests/A.cs", "-root", "/x"]),
            &["root"],
            &["dry-run"],
        );
        assert!(f.is_set("dry-run"));
        assert_eq!(f.rest, v(&["Ashfall.Core.Tests/A.cs", "-root", "/x"]));
        assert_eq!(f.or("root", "."), ".");
    }
}