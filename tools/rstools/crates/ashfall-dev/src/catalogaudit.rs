//! Port of `tools/gotools/pkg/catalogaudit/catalogaudit.go`.
//!
//! Read-only catalog-hygiene audit behind `audit-catalogs` and the
//! `catalog_audit` CI gate: D01 reference integrity, D02 cross-domain duplicate
//! ids, D03 id naming, D04 schema_version drift, plus the policy-domain /
//! allowlist ratchets and the two non-failing advisories.
//!
//! Parity notes:
//! * [`glob_match`] reproduces Go `filepath.Match` semantics (`*`, `?`,
//!   `[class]` with `^` negation and ranges, `\` escapes). Policy globs are
//!   matched against bare filenames, so the separator never participates.
//! * `regex` (RE2-compatible) replaces Go's `regexp` for the two policy
//!   regexes; both patterns are plain snake_case/id-suffix expressions.
//! * Go iterates `map[string]interface{}` documents in random order, but every
//!   ordered output is sorted before emission (`sortFindings`, `sort.Strings`);
//!   this port iterates `serde_json::Map` in its stable (sorted) order instead.
//! * Serde field order mirrors the Go struct field order, and `omitempty`
//!   fields are `Option`s so `null`-vs-`[]`-vs-omitted matches `encoding/json`.

use crate::jsonout;
use regex::Regex;
use serde::{Deserialize, Serialize};
use std::collections::{BTreeMap, BTreeSet, HashMap, HashSet};
use std::path::{Path, PathBuf};

// ---------------------------------------------------------------------------
// Policy
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Default, Deserialize)]
pub struct DomainRule {
    pub domain: String,
    #[serde(default)]
    pub globs: Vec<String>,
    /// Authored documentation only; deserialized but never read by the check.
    #[allow(dead_code)]
    #[serde(default)]
    pub note: String,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct ReferenceRule {
    pub source_glob: String,
    pub field: String,
    pub target_domain: String,
    /// Authored documentation only; deserialized but never read by the check.
    #[allow(dead_code)]
    #[serde(default)]
    pub note: String,
    #[serde(default)]
    pub allowlist: Vec<String>,
    #[serde(default)]
    pub allowlist_reasons: HashMap<String, String>,
    #[serde(default)]
    pub container: String,
}

#[derive(Debug, Clone, Default, Deserialize, Serialize)]
pub struct ExcludedRule {
    pub source_glob: String,
    pub field: String,
    pub reason: String,
}

#[derive(Debug, Clone, Default, Deserialize)]
pub struct Policy {
    #[serde(default)]
    pub schema_version: String,
    /// Authored documentation only; deserialized but never read by the check.
    #[allow(dead_code)]
    #[serde(default)]
    pub description: String,
    #[serde(default)]
    pub data_dir: String,
    #[serde(default)]
    pub id_naming_regex: String,
    #[serde(default)]
    pub unit_suffix_advisory_regex: String,
    #[serde(default)]
    pub default_domain: String,
    #[serde(default)]
    pub id_domains: Vec<DomainRule>,
    #[serde(default)]
    pub duplicate_id_allowlist: Vec<String>,
    #[serde(default)]
    pub reference_rules: Vec<ReferenceRule>,
    #[serde(default)]
    pub reference_rules_excluded: Vec<ExcludedRule>,
    #[serde(default)]
    pub advisory_allowlist: Vec<String>,
    #[serde(default)]
    pub mirror_unresolved_max: i64,
    #[serde(default)]
    pub schema_version_default: i64,
    #[serde(default)]
    pub schema_version_expectations: HashMap<String, i64>,
}

// ---------------------------------------------------------------------------
// Findings / baseline / report
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Serialize)]
pub struct Finding {
    pub check: String,
    pub key: String,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub file: Option<String>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub detail: Option<String>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub source: Option<String>,
}

impl Finding {
    fn new(check: &str, key: impl Into<String>) -> Finding {
        Finding {
            check: check.to_string(),
            key: key.into(),
            file: None,
            detail: None,
            source: None,
        }
    }
    fn with_file(mut self, file: &str) -> Finding {
        self.file = Some(file.to_string());
        self
    }
    fn with_detail(mut self, detail: impl Into<String>) -> Finding {
        self.detail = Some(detail.into());
        self
    }
}

#[derive(Debug, Clone, Default, Deserialize, Serialize)]
pub struct Baseline {
    #[serde(default)]
    pub schema_version: String,
    #[serde(default)]
    pub accepted: BTreeMap<String, Vec<String>>,
}

#[derive(Debug, Clone, Serialize)]
pub struct MirrorAdvisory {
    pub mirror_catalogs: i64,
    pub mirror_unresolved: i64,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub by_catalog: Option<BTreeMap<String, i64>>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub sample: Option<Vec<String>>,
}

#[derive(Debug, Clone, Serialize)]
pub struct Report {
    pub files_scanned: i64,
    pub ids_indexed: i64,
    pub findings: Vec<Finding>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub accepted: Option<Vec<Finding>>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub advisories: Option<Vec<Finding>>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub stale_baseline: Option<Vec<String>>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub parse_errors: Option<Vec<Finding>>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub reference_rules_excluded: Option<Vec<ExcludedRule>>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub mirror_resolution: Option<MirrorAdvisory>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub reference_rule_counts: Option<BTreeMap<String, i64>>,
    pub checked: BTreeMap<String, i64>,
}

impl Report {
    pub fn has_stale(&self) -> bool {
        self.stale_baseline
            .as_ref()
            .map(|v| !v.is_empty())
            .unwrap_or(false)
    }
    pub fn to_json_pretty(&self) -> String {
        jsonout::to_pretty(self)
    }
}

// ---------------------------------------------------------------------------
// Loading
// ---------------------------------------------------------------------------

/// Port of `LoadPolicy` (read, apply defaults, validate).
pub fn load_policy(path: &str) -> Result<Policy, String> {
    let raw = std::fs::read(path).map_err(|e| format!("read policy {}: {}", path, e))?;
    let mut p: Policy =
        serde_json::from_slice(&raw).map_err(|e| format!("parse policy {}: {}", path, e))?;
    if p.id_naming_regex.trim().is_empty() {
        return Err(format!("policy {}: id_naming_regex is required", path));
    }
    if p.default_domain.is_empty() {
        p.default_domain = "unclassified".to_string();
    }
    if p.data_dir.is_empty() {
        p.data_dir = "Assets/StreamingAssets/Data".to_string();
    }
    if p.schema_version_default == 0 {
        p.schema_version_default = 1;
    }
    if p.unit_suffix_advisory_regex.trim().is_empty() {
        p.unit_suffix_advisory_regex = r"_[0-9]+[a-z]+_of_[0-9]+[a-z]+$".to_string();
    }
    validate_policy(&p).map_err(|e| format!("policy {}: {}", path, e))?;
    Ok(p)
}

fn contains_string(items: &[String], target: &str) -> bool {
    items.iter().any(|i| i == target)
}

fn check_sorted_unique(name: &str, items: &[String]) -> Result<(), String> {
    let mut seen: HashSet<&str> = HashSet::new();
    let mut prev: Option<&str> = None;
    for item in items {
        if seen.contains(item.as_str()) {
            return Err(format!("{} contains duplicate {:?}", name, item));
        }
        if let Some(p) = prev {
            if item.as_str() < p {
                return Err(format!("{} must be sorted (found {:?} before {:?})", name, p, item));
            }
        }
        seen.insert(item.as_str());
        prev = Some(item.as_str());
    }
    Ok(())
}

/// Port of `validatePolicy` — fails closed rather than silently passing.
pub fn validate_policy(p: &Policy) -> Result<(), String> {
    if p.schema_version.trim().is_empty() {
        return Err("schema_version is required".to_string());
    }
    if p.id_domains.is_empty() {
        return Err("at least one id_domains entry is required".to_string());
    }
    if let Err(e) = Regex::new(&p.id_naming_regex) {
        return Err(format!("id_naming_regex {:?}: {}", p.id_naming_regex, e));
    }
    check_sorted_unique("duplicate_id_allowlist", &p.duplicate_id_allowlist)?;
    check_sorted_unique("advisory_allowlist", &p.advisory_allowlist)?;
    if p.mirror_unresolved_max < 0 {
        return Err("mirror_unresolved_max must be >= 0 (0 disables the ratchet)".to_string());
    }
    let mut declared: HashSet<&str> = HashSet::new();
    for d in &p.id_domains {
        if d.domain.trim().is_empty() {
            return Err("id_domains entry has an empty domain".to_string());
        }
        declared.insert(d.domain.as_str());
    }
    let mut enforced: HashSet<String> = HashSet::new();
    for r in &p.reference_rules {
        if r.source_glob.trim().is_empty() {
            return Err(format!(
                "reference rule for field {:?} has an empty source_glob",
                r.field
            ));
        }
        if r.field.trim().is_empty() {
            return Err(format!(
                "reference rule for source_glob {:?} has an empty field",
                r.source_glob
            ));
        }
        if !declared.contains(r.target_domain.as_str()) {
            return Err(format!(
                "reference rule {}.{} targets undeclared domain {:?}",
                r.source_glob, r.field, r.target_domain
            ));
        }
        check_sorted_unique(
            &format!("reference_rules.allowlist for {}.{}", r.source_glob, r.field),
            &r.allowlist,
        )?;
        for key in r.allowlist_reasons.keys() {
            if !contains_string(&r.allowlist, key) {
                return Err(format!(
                    "reference rule {}.{} has allowlist_reasons for {:?} which is not in the allowlist",
                    r.source_glob, r.field, key
                ));
            }
        }
        enforced.insert(format!("{}|{}", r.source_glob, r.field));
    }
    for r in &p.reference_rules_excluded {
        if r.source_glob.trim().is_empty() || r.field.trim().is_empty() || r.reason.trim().is_empty()
        {
            return Err(
                "reference_rules_excluded entry needs source_glob, field, and reason".to_string(),
            );
        }
        if enforced.contains(&format!("{}|{}", r.source_glob, r.field)) {
            return Err(format!(
                "field {}.{} is both enforced and excluded",
                r.source_glob, r.field
            ));
        }
    }
    Ok(())
}

/// Port of `NewBaseline`.
pub fn new_baseline() -> Baseline {
    Baseline {
        schema_version: "1.0.0".to_string(),
        accepted: BTreeMap::new(),
    }
}

/// Port of `LoadBaseline` (a missing file yields an empty baseline).
pub fn load_baseline(path: &str) -> Result<Baseline, String> {
    let mut b = new_baseline();
    let raw = match std::fs::read(path) {
        Ok(r) => r,
        Err(e) if e.kind() == std::io::ErrorKind::NotFound => return Ok(b),
        Err(e) => return Err(format!("read baseline {}: {}", path, e)),
    };
    b = serde_json::from_slice(&raw).map_err(|e| format!("parse baseline {}: {}", path, e))?;
    if b.schema_version.is_empty() {
        b.schema_version = "1.0.0".to_string();
    }
    Ok(b)
}

/// Port of `WriteBaseline`.
pub fn write_baseline(path: &str, mut b: Baseline) -> Result<(), String> {
    if b.schema_version.is_empty() {
        b.schema_version = "1.0.0".to_string();
    }
    for keys in b.accepted.values_mut() {
        keys.sort();
    }
    let mut raw = jsonout::to_pretty(&b);
    raw.push('\n');
    if let Some(dir) = Path::new(path).parent() {
        std::fs::create_dir_all(dir).map_err(|e| e.to_string())?;
    }
    std::fs::write(path, raw).map_err(|e| e.to_string())
}

/// Port of `BaselineFromReport`.
pub fn baseline_from_report(rep: &Report) -> Baseline {
    let mut b = new_baseline();
    let accepted = rep.accepted.clone().unwrap_or_default();
    for f in accepted.iter().chain(rep.findings.iter()) {
        if f.source.as_deref() == Some("policy") {
            continue;
        }
        b.accepted.entry(f.check.clone()).or_default().push(f.key.clone());
    }
    b
}

// ---------------------------------------------------------------------------
// filepath.Match
// ---------------------------------------------------------------------------

/// Go's `filepath.Match` (Unix separator `/`). Bad patterns return false, as
/// `globMatch` does when `Match` errors.
pub fn glob_match(pattern: &str, name: &str) -> bool {
    match_go(pattern.as_bytes(), name.as_bytes())
}

fn match_go(p: &[u8], n: &[u8]) -> bool {
    if p.is_empty() {
        return n.is_empty();
    }
    match p[0] {
        b'*' => {
            let mut i = 1;
            while i < p.len() && p[i] == b'*' {
                i += 1;
            }
            let rest = &p[i..];
            if rest.is_empty() {
                return !n.contains(&b'/');
            }
            for k in 0..=n.len() {
                if k > 0 && n[k - 1] == b'/' {
                    break;
                }
                if match_go(rest, &n[k..]) {
                    return true;
                }
            }
            false
        }
        b'?' => !n.is_empty() && n[0] != b'/' && match_go(&p[1..], &n[1..]),
        b'[' => {
            let (ok, consumed) = match_class(p, n);
            if !ok {
                return false;
            }
            match_go(&p[consumed..], &n[1..])
        }
        b'\\' => {
            if p.len() < 2 {
                return false; // ErrBadPattern in Go
            }
            !n.is_empty() && n[0] != b'\\' && n[0] == p[1] && match_go(&p[2..], &n[1..])
        }
        c => !n.is_empty() && n[0] != b'/' && n[0] == c && match_go(&p[1..], &n[1..]),
    }
}

/// Returns `(matched_one_char, pattern_bytes_consumed_up_to_and_including_']')`.
fn match_class(p: &[u8], n: &[u8]) -> (bool, usize) {
    if n.is_empty() || n[0] == b'/' {
        return (false, p.len());
    }
    let c = n[0];
    let mut i = 1; // skip '['
    let negated = i < p.len() && (p[i] == b'^' || p[i] == b'!');
    if negated {
        i += 1;
    }
    let mut matched = false;
    let mut first = true;
    loop {
        if i >= p.len() {
            return (false, p.len()); // ErrBadPattern
        }
        if p[i] == b']' && !first {
            return ((matched != negated), i + 1);
        }
        first = false;
        let lo = match read_class_char(p, &mut i) {
            Some(c) => c,
            None => return (false, p.len()),
        };
        let mut hi = lo;
        if i + 1 < p.len() && p[i] == b'-' && p[i + 1] != b']' {
            i += 1;
            match read_class_char(p, &mut i) {
                Some(c) => hi = c,
                None => return (false, p.len()),
            }
        }
        if lo <= c && c <= hi {
            matched = true;
        }
    }
}

/// Read one literal (or `\`-escaped) class byte.
fn read_class_char(p: &[u8], i: &mut usize) -> Option<u8> {
    if *i >= p.len() {
        return None;
    }
    let c = p[*i];
    *i += 1;
    if c == b'\\' {
        if *i >= p.len() {
            return None;
        }
        let e = p[*i];
        *i += 1;
        Some(e)
    } else {
        Some(c)
    }
}

// ---------------------------------------------------------------------------
// JSON document walking
// ---------------------------------------------------------------------------

fn walk_json<F: FnMut(&str, &serde_json::Value)>(v: &serde_json::Value, f: &mut F) {
    match v {
        serde_json::Value::Object(m) => {
            for (k, child) in m {
                f(k, child);
                walk_json(child, f);
            }
        }
        serde_json::Value::Array(a) => {
            for child in a {
                walk_json(child, f);
            }
        }
        _ => {}
    }
}

fn walk_string_values<F: FnMut(&str)>(v: &serde_json::Value, field: &str, f: &mut F) {
    match v {
        serde_json::Value::Object(m) => {
            for (k, child) in m {
                if k == field {
                    match child {
                        serde_json::Value::String(s) => f(s),
                        serde_json::Value::Array(a) => {
                            for item in a {
                                if let serde_json::Value::String(s) = item {
                                    f(s);
                                }
                            }
                        }
                        _ => {}
                    }
                }
                walk_string_values(child, field, f);
            }
        }
        serde_json::Value::Array(a) => {
            for child in a {
                walk_string_values(child, field, f);
            }
        }
        _ => {}
    }
}

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

struct IdRef {
    id: String,
}

struct CatalogFile {
    name: String,
    domain: String,
    ids: Vec<IdRef>,
    schema_version: Option<i64>,
}

fn domain_for(p: &Policy, name: &str) -> String {
    for rule in &p.id_domains {
        for g in &rule.globs {
            if glob_match(g, name) {
                return rule.domain.clone();
            }
        }
    }
    p.default_domain.clone()
}

fn sort_findings(fs: &mut [Finding]) {
    fs.sort_by(|a, b| {
        a.check
            .cmp(&b.check)
            .then_with(|| a.key.cmp(&b.key))
    });
}

fn resolve_data_dir(root: &str, p: &Policy) -> PathBuf {
    if Path::new(&p.data_dir).is_absolute() {
        PathBuf::from(&p.data_dir)
    } else {
        Path::new(root).join(&p.data_dir)
    }
}

/// Sorted `.json` entry names directly under `dataDir` (Go `os.ReadDir` order).
fn json_entries(data_dir: &Path) -> Result<Vec<String>, String> {
    let rd = std::fs::read_dir(data_dir)
        .map_err(|e| format!("read data dir {}: {}", data_dir.to_string_lossy(), e))?;
    let mut names: Vec<String> = Vec::new();
    for entry in rd.flatten() {
        let name = entry.file_name().to_string_lossy().into_owned();
        let is_dir = entry.file_type().map(|t| t.is_dir()).unwrap_or(false);
        if is_dir || !name.to_ascii_lowercase().ends_with(".json") {
            continue;
        }
        names.push(name);
    }
    names.sort();
    Ok(names)
}

/// `json.Unmarshal(raw, &doc)` into an untyped document (nil on parse failure).
fn parse_doc(path: &Path) -> Option<serde_json::Value> {
    let raw = std::fs::read(path).ok()?;
    serde_json::from_slice::<serde_json::Value>(&raw).ok()
}

fn itoa(n: i64) -> String {
    n.to_string()
}

fn read_catalogs(data_dir: &Path, p: &Policy) -> Result<(Vec<CatalogFile>, Vec<Finding>), String> {
    let names = json_entries(data_dir)?;
    let mut files = Vec::with_capacity(names.len());
    let mut parse_errors: Vec<Finding> = Vec::new();
    for name in &names {
        let path = data_dir.join(name);
        let mut cf = CatalogFile {
            name: name.clone(),
            domain: domain_for(p, name),
            ids: Vec::new(),
            schema_version: None,
        };
        let raw = match std::fs::read(&path) {
            Ok(r) => r,
            Err(e) => {
                parse_errors.push(
                    Finding::new("parse_error", name.clone())
                        .with_file(name)
                        .with_detail(e.to_string()),
                );
                continue;
            }
        };
        let doc: serde_json::Value = match serde_json::from_slice(&raw) {
            Ok(d) => d,
            Err(e) => {
                parse_errors.push(
                    Finding::new("parse_error", name.clone())
                        .with_file(name)
                        .with_detail(e.to_string()),
                );
                continue;
            }
        };
        walk_json(&doc, &mut |key, value| match key {
            "id" => {
                if let serde_json::Value::String(s) = value {
                    if !s.is_empty() {
                        cf.ids.push(IdRef { id: s.clone() });
                    }
                }
            }
            "schema_version" => {
                if let Some(n) = value.as_f64() {
                    if n == (n as i64) as f64 {
                        cf.schema_version = Some(n as i64);
                    }
                }
            }
            _ => {}
        });
        files.push(cf);
    }
    Ok((files, parse_errors))
}

// ---------------------------------------------------------------------------
// Run
// ---------------------------------------------------------------------------

/// Port of `Run`.
pub fn run(root: &str, p: &Policy, baseline: &Baseline) -> Result<Report, String> {
    let re = Regex::new(&p.id_naming_regex)
        .map_err(|e| format!("invalid id_naming_regex {:?}: {}", p.id_naming_regex, e))?;

    let data_dir = resolve_data_dir(root, p);
    let names = json_entries(&data_dir)?;

    let (files, parse_errors) = {
        let (f, pe) = read_catalogs(&data_dir, p)?;
        (f, pe)
    };
    let files_scanned = names.len() as i64;
    let ids_indexed: i64 = files.iter().map(|f| f.ids.len() as i64).sum();

    let mut report = Report {
        files_scanned,
        ids_indexed,
        findings: Vec::new(),
        accepted: None,
        advisories: None,
        stale_baseline: None,
        parse_errors: None,
        reference_rules_excluded: None,
        mirror_resolution: None,
        reference_rule_counts: None,
        checked: BTreeMap::new(),
    };
    if !parse_errors.is_empty() {
        report.parse_errors = Some(parse_errors);
    }

    // Reference-target id sets per domain rule.
    let mut target_ids: HashMap<String, HashSet<String>> = HashMap::new();
    for rule in &p.id_domains {
        let mut set: HashSet<String> = HashSet::new();
        for f in &files {
            if rule.globs.iter().any(|g| glob_match(g, &f.name)) {
                for r in &f.ids {
                    set.insert(r.id.clone());
                }
            }
        }
        target_ids.insert(rule.domain.clone(), set);
    }

    // Assigned-domain id sets (mirror excluded) for the mirror advisory.
    let mut assigned_ids: HashMap<String, HashSet<String>> = HashMap::new();
    for f in &files {
        if f.domain == "mirror" {
            continue;
        }
        let set = assigned_ids.entry(f.domain.clone()).or_default();
        for r in &f.ids {
            set.insert(r.id.clone());
        }
    }

    let mut findings: Vec<Finding> = Vec::new();

    // H03 — a domain whose globs match no catalog is dead policy.
    for rule in &p.id_domains {
        let matched = files.iter().any(|f| rule.globs.iter().any(|g| glob_match(g, &f.name)));
        if !matched {
            findings.push(
                Finding::new("policy_domain", format!("policy:domain-{}:dead-glob", rule.domain))
                    .with_detail(format!("id_domains entry {:?} matches no catalog", rule.domain)),
            );
        }
    }

    // D01 — reference integrity.
    let empty_set = HashSet::new();
    let mut rule_counts: BTreeMap<String, i64> = BTreeMap::new();
    let mut stale_baseline: Vec<String> = Vec::new();
    for rule in &p.reference_rules {
        let targets = target_ids.get(&rule.target_domain).unwrap_or(&empty_set);
        if targets.is_empty() {
            findings.push(
                Finding::new(
                    "reference_integrity",
                    format!("policy:{}:target-domain-{}", rule.field, rule.target_domain),
                )
                .with_detail(format!(
                    "target domain {:?} has no ids; reference rule cannot be evaluated",
                    rule.target_domain
                )),
            );
            continue;
        }
        let allowed: HashSet<&str> = rule.allowlist.iter().map(|s| s.as_str()).collect();
        let mut seen_allow: HashSet<String> = HashSet::new();
        let mut values_checked: i64 = 0;
        let mut matched: i64 = 0;
        for f in &files {
            if !glob_match(&rule.source_glob, &f.name) {
                continue;
            }
            matched += 1;
            *report.checked.entry("reference_integrity".to_string()).or_insert(0) += 1;
            let path = data_dir.join(&f.name);
            let doc = match parse_doc(&path) {
                Some(d) => d,
                None => continue,
            };
            let roots: Vec<&serde_json::Value> = if !rule.container.is_empty() {
                let arr = doc
                    .as_object()
                    .and_then(|m| m.get(&rule.container))
                    .and_then(|v| v.as_array());
                match arr {
                    Some(a) => {
                        // Borrowing the array's elements directly.
                        a.iter().collect()
                    }
                    None => {
                        findings.push(
                            Finding::new(
                                "reference_integrity",
                                format!("policy:{}:container-{}", rule.field, rule.container),
                            )
                            .with_file(&f.name)
                            .with_detail(format!(
                                "container {:?} not found in {}; reference rule cannot be evaluated",
                                rule.container, f.name
                            )),
                        );
                        continue;
                    }
                }
            } else {
                vec![&doc]
            };
            for root_v in roots {
                walk_string_values(root_v, &rule.field, &mut |value: &str| {
                    values_checked += 1;
                    if allowed.contains(value) {
                        seen_allow.insert(value.to_string());
                    }
                    if !targets.contains(value) && !allowed.contains(value) {
                        findings.push(
                            Finding::new(
                                "reference_integrity",
                                format!("{}:{}:{}", f.name, rule.field, value),
                            )
                            .with_file(&f.name)
                            .with_detail(format!(
                                "{:?}={:?} does not resolve in domain {:?}",
                                rule.field, value, rule.target_domain
                            )),
                        );
                    }
                });
            }
        }
        rule_counts.insert(format!("{}.{}", rule.source_glob, rule.field), values_checked);
        // I01 — matched a file but found no values is dead policy.
        if matched > 0 && values_checked == 0 {
            findings.push(
                Finding::new("reference_integrity", format!("policy:{}:no-values", rule.field))
                    .with_detail(format!(
                        "field {:?} appears nowhere in the matched catalog(s); reference rule cannot be evaluated",
                        rule.field
                    )),
            );
        }
        // H02 — a stale allowlist entry must be pruned.
        for a in &rule.allowlist {
            if !seen_allow.contains(a) {
                stale_baseline.push(format!(
                    "reference_allowlist:{}:{}:{}",
                    rule.source_glob, rule.field, a
                ));
            }
        }
        // F01 — a rule whose source glob matches no catalog is dead policy.
        if matched == 0 {
            findings.push(
                Finding::new(
                    "reference_integrity",
                    format!("policy:{}:source-glob-{}", rule.field, rule.source_glob),
                )
                .with_detail(format!(
                    "source_glob {:?} matched no catalog; reference rule cannot be evaluated",
                    rule.source_glob
                )),
            );
        }
    }
    if !rule_counts.is_empty() {
        report.reference_rule_counts = Some(rule_counts);
    }

    // D02 — cross-domain duplicate ids.
    let dup_allow: HashSet<&str> = p.duplicate_id_allowlist.iter().map(|s| s.as_str()).collect();
    let mut domains_by_id: HashMap<String, BTreeSet<String>> = HashMap::new();
    for f in &files {
        if f.domain == "mirror" {
            continue;
        }
        for r in &f.ids {
            domains_by_id
                .entry(r.id.clone())
                .or_default()
                .insert(f.domain.clone());
        }
    }
    let mut accepted: Vec<Finding> = Vec::new();
    for (id, domains) in &domains_by_id {
        *report.checked.entry("duplicate_ids".to_string()).or_insert(0) += 1;
        if domains.len() < 2 {
            continue;
        }
        let ds: Vec<String> = domains.iter().cloned().collect();
        let detail = format!("id defined in multiple id-domains: {}", ds.join(", "));
        if dup_allow.contains(id.as_str()) {
            accepted.push(
                Finding::new("duplicate_ids", id.clone())
                    .with_detail(detail)
                    .tap_source("policy"),
            );
            continue;
        }
        findings.push(Finding::new("duplicate_ids", id.clone()).with_detail(detail));
    }
    for id in &p.duplicate_id_allowlist {
        if domains_by_id.get(id).map(|d| d.len()).unwrap_or(0) < 2 {
            stale_baseline.push(format!("duplicate_id_allowlist:{}", id));
        }
    }

    // D03 — id naming convention.
    for f in &files {
        for r in &f.ids {
            *report.checked.entry("id_naming".to_string()).or_insert(0) += 1;
            if re.is_match(&r.id) {
                continue;
            }
            findings.push(
                Finding::new("id_naming", format!("{}:{}", f.name, r.id))
                    .with_file(&f.name)
                    .with_detail(format!("id {:?} does not match {}", r.id, p.id_naming_regex)),
            );
        }
    }

    // D04 — schema_version drift.
    for f in &files {
        let Some(sv) = f.schema_version else { continue };
        *report
            .checked
            .entry("schema_version_drift".to_string())
            .or_insert(0) += 1;
        let expected = p
            .schema_version_expectations
            .get(&f.name)
            .copied()
            .unwrap_or(p.schema_version_default);
        if sv != expected {
            findings.push(
                Finding::new("schema_version_drift", format!("{}:{}", f.name, itoa(sv)))
                    .with_file(&f.name)
                    .with_detail(format!(
                        "schema_version {} does not match expected {}",
                        sv, expected
                    )),
            );
        }
    }

    // E05 — echo deliberately-excluded reference contracts.
    if !p.reference_rules_excluded.is_empty() {
        report.reference_rules_excluded = Some(p.reference_rules_excluded.clone());
    }

    // E06 — unit-suffix advisory census (non-failing).
    let mut advisories: Vec<Finding> = Vec::new();
    if let Ok(unit_re) = Regex::new(&p.unit_suffix_advisory_regex) {
        let mut seen: HashSet<String> = HashSet::new();
        for f in &files {
            for r in &f.ids {
                if seen.contains(&r.id) || !unit_re.is_match(&r.id) {
                    continue;
                }
                seen.insert(r.id.clone());
                advisories.push(
                    Finding::new("id_unit_suffix", r.id.clone())
                        .with_file(&f.name)
                        .with_detail("id carries a redundant unit suffix (advisory, non-failing)"),
                );
            }
        }
        sort_findings(&mut advisories);
    }
    if !advisories.is_empty() {
        report.advisories = Some(advisories);
    }

    // E07 — mirror-resolution advisory.
    let mirror_catalogs = files.iter().filter(|f| f.domain == "mirror").count() as i64;
    let mut mirror_ids: HashSet<String> = HashSet::new();
    for f in &files {
        if f.domain == "mirror" {
            for r in &f.ids {
                mirror_ids.insert(r.id.clone());
            }
        }
    }
    if mirror_catalogs > 0 {
        let mut unresolved: Vec<String> = Vec::new();
        for id in &mirror_ids {
            let found = assigned_ids
                .iter()
                .any(|(domain, set)| domain != "mirror" && set.contains(id));
            if !found {
                unresolved.push(id.clone());
            }
        }
        unresolved.sort();
        let sample: Vec<String> = unresolved.iter().take(10).cloned().collect();
        let unresolved_set: HashSet<&str> = unresolved.iter().map(|s| s.as_str()).collect();
        let mut by_catalog: BTreeMap<String, i64> = BTreeMap::new();
        for f in &files {
            if f.domain != "mirror" {
                continue;
            }
            for r in &f.ids {
                if unresolved_set.contains(r.id.as_str()) {
                    *by_catalog.entry(f.name.clone()).or_insert(0) += 1;
                }
            }
        }
        report.mirror_resolution = Some(MirrorAdvisory {
            mirror_catalogs,
            mirror_unresolved: unresolved.len() as i64,
            by_catalog: if by_catalog.is_empty() {
                None
            } else {
                Some(by_catalog)
            },
            sample: if sample.is_empty() { None } else { Some(sample) },
        });

        // G03 — optional count ratchet.
        if p.mirror_unresolved_max > 0 && (unresolved.len() as i64) > p.mirror_unresolved_max {
            findings.push(
                Finding::new(
                    "mirror_resolution_ratchet",
                    format!(
                        "mirror_resolution:{}>{}",
                        unresolved.len(),
                        p.mirror_unresolved_max
                    ),
                )
                .with_detail(format!(
                    "{} mirror ids lack a primary author, over the policy ceiling {}",
                    unresolved.len(),
                    p.mirror_unresolved_max
                )),
            );
        }
    }

    // Classify against the baseline.
    let mut accepted_set: HashMap<String, HashSet<String>> = HashMap::new();
    for (check, keys) in &baseline.accepted {
        accepted_set.insert(check.clone(), keys.iter().cloned().collect());
    }
    let mut seen_keys: HashMap<String, HashSet<String>> = HashMap::new();
    for f in findings {
        if accepted_set
            .get(&f.check)
            .map(|s| s.contains(&f.key))
            .unwrap_or(false)
        {
            let mut f = f;
            f.source = Some("baseline".to_string());
            accepted.push(f);
            continue;
        }
        if let Some(set) = seen_keys.get_mut(&f.check) {
            set.insert(f.key.clone());
        } else {
            let mut set = HashSet::new();
            set.insert(f.key.clone());
            seen_keys.insert(f.check.clone(), set);
        }
        report.findings.push(f);
    }
    for f in &accepted {
        seen_keys
            .entry(f.check.clone())
            .or_default()
            .insert(f.key.clone());
    }
    for (check, keys) in &baseline.accepted {
        for k in keys {
            if !seen_keys.get(check).map(|s| s.contains(k)).unwrap_or(false) {
                stale_baseline.push(format!("{}:{}", check, k));
            }
        }
    }
    stale_baseline.sort();
    if !stale_baseline.is_empty() {
        report.stale_baseline = Some(stale_baseline);
    }
    sort_findings(&mut report.findings);
    if !accepted.is_empty() {
        sort_findings(&mut accepted);
        report.accepted = Some(accepted);
    }

    Ok(report)
}

/// Small helper so a finding can be tagged with `source` inline.
trait TapSource {
    fn tap_source(self, source: &str) -> Self;
}
impl TapSource for Finding {
    fn tap_source(mut self, source: &str) -> Self {
        self.source = Some(source.to_string());
        self
    }
}

/// Port of `DomainIDs` (`--dump-ids`).
pub fn domain_ids(root: &str, p: &Policy, domain: &str) -> Result<Vec<String>, String> {
    let mut globs: Vec<String> = Vec::new();
    for rule in &p.id_domains {
        if rule.domain == domain {
            globs.extend(rule.globs.iter().cloned());
        }
    }
    if globs.is_empty() {
        return Err(format!("unknown id domain {:?}", domain));
    }
    let data_dir = resolve_data_dir(root, p);
    let names = json_entries(&data_dir)?;
    let mut set: HashSet<String> = HashSet::new();
    for name in &names {
        if !globs.iter().any(|g| glob_match(g, name)) {
            continue;
        }
        let Some(doc) = parse_doc(&data_dir.join(name)) else {
            continue;
        };
        walk_json(&doc, &mut |key, value| {
            if key == "id" {
                if let serde_json::Value::String(s) = value {
                    if !s.is_empty() {
                        set.insert(s.clone());
                    }
                }
            }
        });
    }
    let mut ids: Vec<String> = set.into_iter().collect();
    ids.sort();
    Ok(ids)
}

/// Port of `DuplicateEntry`.
#[derive(Debug, Clone, Serialize)]
pub struct DuplicateEntry {
    pub id: String,
    pub domains: Vec<String>,
}

/// Port of `DuplicateIDs` (`--dump-duplicates`).
pub fn duplicate_ids(root: &str, p: &Policy) -> Result<Vec<DuplicateEntry>, String> {
    let data_dir = resolve_data_dir(root, p);
    let names = json_entries(&data_dir)?;
    let mut domains_by_id: HashMap<String, BTreeSet<String>> = HashMap::new();
    for name in &names {
        let domain = domain_for(p, name);
        if domain == "mirror" {
            continue;
        }
        let Some(doc) = parse_doc(&data_dir.join(name)) else {
            continue;
        };
        walk_json(&doc, &mut |key, value| {
            if key != "id" {
                return;
            }
            if let serde_json::Value::String(s) = value {
                if s.is_empty() {
                    return;
                }
                domains_by_id
                    .entry(s.clone())
                    .or_default()
                    .insert(domain.clone());
            }
        });
    }
    let mut out: Vec<DuplicateEntry> = Vec::new();
    for (id, domains) in &domains_by_id {
        if domains.len() < 2 {
            continue;
        }
        out.push(DuplicateEntry {
            id: id.clone(),
            domains: domains.iter().cloned().collect(),
        });
    }
    out.sort_by(|a, b| a.id.cmp(&b.id));
    Ok(out)
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::fs;

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-catalogaudit-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    fn write(root: &Path, rel: &str, body: &str) {
        let full = root.join(rel);
        fs::create_dir_all(full.parent().unwrap()).unwrap();
        fs::write(full, body).unwrap();
    }

    fn test_policy() -> Policy {
        Policy {
            schema_version: "1.0.0".to_string(),
            data_dir: "data".to_string(),
            id_naming_regex: "^[a-z0-9]+(_[a-z0-9]+)*$".to_string(),
            default_domain: "unclassified".to_string(),
            id_domains: vec![
                DomainRule {
                    domain: "item".into(),
                    globs: vec!["items.json".into(), "*_items.json".into()],
                    note: String::new(),
                },
                DomainRule {
                    domain: "scavenging_table".into(),
                    globs: vec!["scavenging_tables.json".into()],
                    note: String::new(),
                },
                DomainRule {
                    domain: "location".into(),
                    globs: vec!["*location*.json".into(), "expeditions.json".into()],
                    note: String::new(),
                },
            ],
            reference_rules: vec![
                ReferenceRule {
                    source_glob: "expeditions.json".into(),
                    field: "lootCategories".into(),
                    target_domain: "item".into(),
                    ..Default::default()
                },
                ReferenceRule {
                    source_glob: "expeditions.json".into(),
                    field: "scavenging_table_id".into(),
                    target_domain: "scavenging_table".into(),
                    ..Default::default()
                },
            ],
            schema_version_default: 1,
            unit_suffix_advisory_regex: r"_[0-9]+[a-z]+_of_[0-9]+[a-z]+$".into(),
            schema_version_expectations: HashMap::from([("items.json".to_string(), 2)]),
            ..Default::default()
        }
    }

    fn seed_valid(root: &Path) {
        write(
            root,
            "data/items.json",
            r#"{"schema_version":2,"items":[{"id":"bandage"},{"id":"dried_rations"}]}"#,
        );
        write(
            root,
            "data/scavenging_tables.json",
            r#"{"schema_version":1,"tables":[{"id":"salvage_common"}]}"#,
        );
        write(
            root,
            "data/expeditions.json",
            r#"{"schema_version":1,"expeditions":[{"id":"loc_a","lootCategories":["bandage"],"scavenging_table_id":"salvage_common"}]}"#,
        );
    }

    fn has(rep: &Report, check: &str, key: &str) -> bool {
        rep.findings.iter().any(|f| f.check == check && f.key == key)
    }

    #[test]
    fn glob_match_matches_go_filepath_match() {
        assert!(glob_match("items.json", "items.json"));
        assert!(!glob_match("items.json", "other.json"));
        assert!(glob_match("*", "anything.json"));
        assert!(glob_match("*.json", "a.json"));
        assert!(!glob_match("*.json", "a.txt"));
        assert!(glob_match("*location*.json", "shelter_location.json"));
        assert!(!glob_match("*location*.json", "expeditions.json"));
        assert!(glob_match("a?c", "abc"));
        assert!(!glob_match("a?c", "ac"));
        assert!(glob_match("[a-c]x", "bx"));
        assert!(glob_match("[^a-c]x", "dx"));
        assert!(glob_match(r"a\*b", "a*b"));
    }

    #[test]
    fn clean_corpus_has_no_findings() {
        let root = tmpdir("clean");
        seed_valid(&root);
        let rep = run(&root.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        assert!(rep.findings.is_empty(), "{:?}", rep.findings);
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn dangling_loot_category_is_a_finding() {
        let root = tmpdir("dangling");
        seed_valid(&root);
        write(
            &root,
            "data/expeditions.json",
            r#"{"schema_version":1,"expeditions":[{"id":"loc_a","lootCategories":["bandages"],"scavenging_table_id":"salvage_common"}]}"#,
        );
        let rep = run(&root.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        assert!(has(&rep, "reference_integrity", "expeditions.json:lootCategories:bandages"));
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn duplicate_ids_and_allowlist() {
        let root = tmpdir("dupes");
        seed_valid(&root);
        write(
            &root,
            "data/shelter_location.json",
            r#"{"schema_version":1,"locations":[{"id":"bandage"}]}"#,
        );
        let rep = run(&root.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        assert!(has(&rep, "duplicate_ids", "bandage"));

        let mut p = test_policy();
        p.duplicate_id_allowlist = vec!["bandage".to_string()];
        let rep = run(&root.to_string_lossy(), &p, &new_baseline()).unwrap();
        assert!(!has(&rep, "duplicate_ids", "bandage"));
        let accepted = rep.accepted.unwrap();
        assert_eq!(accepted[0].source.as_deref(), Some("policy"));
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn naming_and_schema_version_drift() {
        let root = tmpdir("naming");
        seed_valid(&root);
        write(
            &root,
            "data/verdict_items.json",
            r#"{"schema_version":1,"items":[{"id":"Bad-ID"}]}"#,
        );
        let rep = run(&root.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        assert!(has(&rep, "id_naming", "verdict_items.json:Bad-ID"));

        let root2 = tmpdir("drift");
        seed_valid(&root2);
        write(&root2, "data/items.json", r#"{"schema_version":1,"items":[{"id":"bandage"}]}"#);
        let rep = run(&root2.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        assert!(has(&rep, "schema_version_drift", "items.json:1"));
        let _ = fs::remove_dir_all(&root);
        let _ = fs::remove_dir_all(&root2);
    }

    #[test]
    fn baseline_suppresses_and_round_trips() {
        let root = tmpdir("baseline");
        seed_valid(&root);
        write(
            &root,
            "data/expeditions.json",
            r#"{"schema_version":1,"expeditions":[{"id":"loc_a","lootCategories":["bandages"],"scavenging_table_id":"salvage_common"}]}"#,
        );
        let first = run(&root.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        assert!(!first.findings.is_empty());
        let baseline = baseline_from_report(&first);
        let second = run(&root.to_string_lossy(), &test_policy(), &baseline).unwrap();
        assert!(second.findings.is_empty(), "{:?}", second.findings);
        assert!(!second.has_stale());
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn stale_baseline_reported_and_malformed_catalog_is_a_parse_error() {
        let root = tmpdir("stale");
        seed_valid(&root);
        let mut b = new_baseline();
        b.accepted
            .insert("id_naming".to_string(), vec!["gone.json:ghost".to_string()]);
        let rep = run(&root.to_string_lossy(), &test_policy(), &b).unwrap();
        assert_eq!(
            rep.stale_baseline.as_deref(),
            Some(&["id_naming:gone.json:ghost".to_string()][..])
        );

        let root2 = tmpdir("broken");
        seed_valid(&root2);
        write(&root2, "data/broken.json", r#"{"schema_version":1,"items":[}"#);
        let rep = run(&root2.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        let pe = rep.parse_errors.unwrap();
        assert_eq!(pe.len(), 1);
        assert_eq!(pe[0].file.as_deref(), Some("broken.json"));
        let _ = fs::remove_dir_all(&root);
        let _ = fs::remove_dir_all(&root2);
    }

    #[test]
    fn baseline_from_report_excludes_policy_allowlist() {
        let root = tmpdir("pol");
        seed_valid(&root);
        write(
            &root,
            "data/shelter_location.json",
            r#"{"schema_version":1,"locations":[{"id":"bandage"}]}"#,
        );
        let mut p = test_policy();
        p.duplicate_id_allowlist = vec!["bandage".to_string()];
        let rep = run(&root.to_string_lossy(), &p, &new_baseline()).unwrap();
        let b = baseline_from_report(&rep);
        assert!(!b.accepted.contains_key("duplicate_ids"));
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn unit_suffix_advisory_and_mirror_resolution() {
        let root = tmpdir("adv");
        seed_valid(&root);
        write(
            &root,
            "data/verdict_items.json",
            r#"{"schema_version":1,"items":[{"id":"copper_wire_10m_of_10m"},{"id":"plain_item"}]}"#,
        );
        let rep = run(&root.to_string_lossy(), &test_policy(), &new_baseline()).unwrap();
        assert!(rep.findings.is_empty(), "advisory must not become a finding");
        assert!(rep
            .advisories
            .as_ref()
            .unwrap()
            .iter()
            .any(|a| a.check == "id_unit_suffix" && a.key == "copper_wire_10m_of_10m"));

        let root2 = tmpdir("mirror");
        seed_valid(&root2);
        let mut p = test_policy();
        p.id_domains.push(DomainRule {
            domain: "mirror".into(),
            globs: vec!["asset_registry.json".into()],
            note: String::new(),
        });
        write(
            &root2,
            "data/asset_registry.json",
            r#"{"schema_version":1,"assets":[{"id":"bandage"},{"id":"asset_only_id"}]}"#,
        );
        let rep = run(&root2.to_string_lossy(), &p, &new_baseline()).unwrap();
        let m = rep.mirror_resolution.unwrap();
        assert_eq!(m.mirror_unresolved, 1);
        assert!(rep.findings.is_empty());
        let _ = fs::remove_dir_all(&root);
        let _ = fs::remove_dir_all(&root2);
    }

    #[test]
    fn container_scoping_and_missing_container_fail_closed() {
        let root = tmpdir("container");
        seed_valid(&root);
        let mut p = test_policy();
        p.id_domains.push(DomainRule {
            domain: "canonical_item".into(),
            globs: vec!["items.json".into()],
            note: String::new(),
        });
        p.reference_rules = vec![ReferenceRule {
            source_glob: "combat_catalog.json".into(),
            field: "id".into(),
            target_domain: "canonical_item".into(),
            container: "ammo".into(),
            ..Default::default()
        }];
        write(
            &root,
            "data/combat_catalog.json",
            r#"{"schema_version":1,"ammo":[{"id":"bandage"},{"id":"ammo_missing"}],"weapons":[{"id":"weapon_missing"}]}"#,
        );
        let rep = run(&root.to_string_lossy(), &p, &new_baseline()).unwrap();
        assert!(has(&rep, "reference_integrity", "combat_catalog.json:id:ammo_missing"));
        assert!(!has(&rep, "reference_integrity", "combat_catalog.json:id:weapon_missing"));

        let root2 = tmpdir("nocontainer");
        seed_valid(&root2);
        write(
            &root2,
            "data/combat_catalog.json",
            r#"{"schema_version":1,"weapons":[{"id":"bandage"}]}"#,
        );
        let rep = run(&root2.to_string_lossy(), &p, &new_baseline()).unwrap();
        assert!(has(&rep, "reference_integrity", "policy:id:container-ammo"));
        let _ = fs::remove_dir_all(&root);
        let _ = fs::remove_dir_all(&root2);
    }

    #[test]
    fn dead_policy_domain_and_source_glob_and_no_values() {
        let root = tmpdir("dead");
        seed_valid(&root);
        let mut p = test_policy();
        p.id_domains.push(DomainRule {
            domain: "ghost_domain".into(),
            globs: vec!["ghost_*.json".into()],
            note: String::new(),
        });
        let rep = run(&root.to_string_lossy(), &p, &new_baseline()).unwrap();
        assert!(has(&rep, "policy_domain", "policy:domain-ghost_domain:dead-glob"));

        let mut p2 = test_policy();
        p2.reference_rules = vec![ReferenceRule {
            source_glob: "does_not_exist.json".into(),
            field: "lootCategories".into(),
            target_domain: "item".into(),
            ..Default::default()
        }];
        let rep = run(&root.to_string_lossy(), &p2, &new_baseline()).unwrap();
        assert!(has(
            &rep,
            "reference_integrity",
            "policy:lootCategories:source-glob-does_not_exist.json"
        ));
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn validate_policy_rejects_bad_configs() {
        let mut p = test_policy();
        p.id_naming_regex = "^[a-z".to_string();
        assert!(validate_policy(&p).is_err());

        let mut p = test_policy();
        p.reference_rules = vec![ReferenceRule {
            source_glob: "x.json".into(),
            field: "id".into(),
            target_domain: "typo_domain".into(),
            ..Default::default()
        }];
        assert!(validate_policy(&p).is_err());

        let mut p = test_policy();
        p.duplicate_id_allowlist = vec!["b_id".into(), "a_id".into()];
        assert!(validate_policy(&p).is_err());

        let mut p = test_policy();
        p.duplicate_id_allowlist = vec!["a_id".into(), "a_id".into()];
        assert!(validate_policy(&p).is_err());

        let mut p = test_policy();
        p.reference_rules = vec![ReferenceRule {
            source_glob: "x.json".into(),
            field: "id".into(),
            target_domain: "item".into(),
            allowlist: vec!["a".into()],
            allowlist_reasons: HashMap::from([("b".to_string(), "why".to_string())]),
            ..Default::default()
        }];
        assert!(validate_policy(&p).is_err());

        let mut p = test_policy();
        p.reference_rules = vec![ReferenceRule {
            source_glob: "x.json".into(),
            field: "id".into(),
            target_domain: "item".into(),
            ..Default::default()
        }];
        p.reference_rules_excluded = vec![ExcludedRule {
            source_glob: "x.json".into(),
            field: "id".into(),
            reason: "contradiction".into(),
        }];
        assert!(validate_policy(&p).is_err());
    }

    #[test]
    fn domain_ids_and_duplicate_ids_helpers() {
        let root = tmpdir("helpers");
        seed_valid(&root);
        let ids = domain_ids(&root.to_string_lossy(), &test_policy(), "item").unwrap();
        assert_eq!(ids, vec!["bandage", "dried_rations"]);
        assert!(domain_ids(&root.to_string_lossy(), &test_policy(), "nope").is_err());

        write(
            &root,
            "data/shelter_location.json",
            r#"{"schema_version":1,"locations":[{"id":"bandage"}]}"#,
        );
        let dupes = duplicate_ids(&root.to_string_lossy(), &test_policy()).unwrap();
        assert_eq!(dupes.len(), 1);
        assert_eq!(dupes[0].id, "bandage");
        assert_eq!(dupes[0].domains, vec!["item", "location"]);
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn report_json_omits_optional_collections() {
        let json = jsonout::to_pretty(&Report {
            files_scanned: 0,
            ids_indexed: 0,
            findings: Vec::new(),
            accepted: None,
            advisories: None,
            stale_baseline: None,
            parse_errors: None,
            reference_rules_excluded: None,
            mirror_resolution: None,
            reference_rule_counts: None,
            checked: BTreeMap::new(),
        });
        assert!(json.contains("\"findings\": []"), "{json}");
        assert!(!json.contains("\"accepted\""), "{json}");
        assert!(json.contains("\"checked\": {}"), "{json}");
    }
}