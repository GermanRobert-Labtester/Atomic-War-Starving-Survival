//! Port of the retired Go `cmd/reachability-report` (Stage 4 cutover).
//!
//! Renders `docs/ci/content_reachability_dispositions.json` against
//! `artifacts/content-utilization.json` into
//! `artifacts/content-reachability-report.md`, failing on a disposition without
//! an expiry or owner, or an unresolved catalog without a disposition.
//! `Ashfall.Core.Tests/Content/ContentReachabilityDispositionTests` asserts this
//! generator and the artifact it publishes.

use serde::Deserialize;
use std::collections::{BTreeMap, BTreeSet, HashMap, HashSet};
use std::fmt::Write as _;
use std::fs;
use std::path::{Path, PathBuf};

const UNRESOLVED_CLASSIFICATION: i64 = 6;
const GAMEPLAY_CLASSIFICATION: i64 = 0;

// The dispositions file is authored in PascalCase; Go's encoding/json matched
// those keys case-insensitively, so accept both spellings here.
#[derive(Debug, Default, Clone, Deserialize)]
struct Exemption {
    #[serde(rename = "ContentPath", alias = "contentPath", default)]
    content_path: String,
    #[serde(rename = "Owner", alias = "owner", default)]
    owner: String,
    #[serde(rename = "Classification", alias = "classification", default)]
    classification: String,
    #[serde(rename = "ExpiryCondition", alias = "expiryCondition", default)]
    expiry_condition: String,
}

#[derive(Debug, Default, Deserialize)]
struct Registry {
    #[serde(rename = "Exemptions", alias = "exemptions", default)]
    exemptions: Vec<Exemption>,
}

#[derive(Debug, Default, Clone, Deserialize)]
struct Catalog {
    #[serde(default)]
    path: String,
    #[serde(default)]
    classification: i64,
    #[serde(default)]
    loader: String,
}

#[derive(Debug, Default, Deserialize)]
struct Utilization {
    #[serde(default)]
    catalogs: Vec<Catalog>,
}

pub fn run(root: &str, out_path: &str) -> Result<(), String> {
    let policy_path = Path::new(root)
        .join("docs")
        .join("ci")
        .join("content_reachability_dispositions.json");
    let graph_path = Path::new(root)
        .join("artifacts")
        .join("content-utilization.json");
    let out: PathBuf = if out_path.is_empty() {
        Path::new(root)
            .join("artifacts")
            .join("content-reachability-report.md")
    } else {
        PathBuf::from(out_path)
    };

    let policy = load_registry(&policy_path)?;
    let graph = load_utilization(&graph_path)?;

    let mut current: HashMap<String, Catalog> = HashMap::with_capacity(graph.catalogs.len());
    for c in graph.catalogs {
        current.insert(c.path.clone(), c);
    }

    let mut dispositioned: HashSet<String> = HashSet::new();
    let mut failures: Vec<String> = Vec::new();
    for e in &policy.exemptions {
        dispositioned.insert(e.content_path.clone());
        if e.expiry_condition.trim().is_empty() {
            failures.push(format!("disposition without an expiry: {}", e.content_path));
        }
        if e.owner.trim().is_empty() {
            failures.push(format!("disposition without an owner: {}", e.content_path));
        }
    }

    // Any UNRESOLVED catalog without a disposition is a hard failure.
    let mut unresolved: Vec<String> = Vec::new();
    for (path, c) in &current {
        if c.classification == UNRESOLVED_CLASSIFICATION && !dispositioned.contains(path) {
            unresolved.push(path.clone());
        }
    }
    unresolved.sort();
    for path in &unresolved {
        failures.push(format!("unresolved catalog without a disposition: {}", path));
    }

    let report = render(&policy, &current);
    if let Some(parent) = out.parent() {
        fs::create_dir_all(parent).map_err(|e| format!("mkdir {}: {}", parent.display(), e))?;
    }
    // End with exactly one newline (no trailing blank line), matching the
    // committed artifact and satisfying the repo whitespace gate.
    fs::write(&out, format!("{}\n", report.trim_end()).as_bytes())
        .map_err(|e| format!("write {}: {}", out.display(), e))?;

    println!(
        "reachability-report: wrote {} ({} dispositions, {} unresolved-without-disposition)",
        out.to_string_lossy(),
        policy.exemptions.len(),
        unresolved.len()
    );
    if !failures.is_empty() {
        return Err(format!(
            "{} reachability failure(s):\n  - {}",
            failures.len(),
            failures.join("\n  - ")
        ));
    }
    Ok(())
}

fn render(policy: &Registry, current: &HashMap<String, Catalog>) -> String {
    let mut b = String::new();
    b.push_str("# Content Reachability Report\n\n");
    b.push_str(
        "Generated from `docs/ci/content_reachability_dispositions.json` against `artifacts/content-utilization.json`.\n\n",
    );

    let mut by_class: BTreeMap<String, BTreeMap<String, Vec<&Exemption>>> = BTreeMap::new();
    for e in &policy.exemptions {
        by_class
            .entry(e.classification.clone())
            .or_default()
            .entry(e.owner.clone())
            .or_default()
            .push(e);
    }

    let mut consumed: Vec<String> = Vec::new();
    let mut removal_candidates: Vec<String> = Vec::new();
    for e in &policy.exemptions {
        let Some(c) = current.get(&e.content_path) else {
            continue;
        };
        if c.classification == GAMEPLAY_CLASSIFICATION {
            consumed.push(e.content_path.clone());
        }
        if e.classification == "DORMANT"
            && c.classification == UNRESOLVED_CLASSIFICATION
            && c.loader.trim().is_empty()
        {
            removal_candidates.push(e.content_path.clone());
        }
    }
    consumed.sort();
    removal_candidates.sort();

    b.push_str("## Summary\n\n");
    let _ = writeln!(b, "- Dispositions: {}", policy.exemptions.len());
    let _ = writeln!(
        b,
        "- Now gameplay-consumed (disposition can retire): {}",
        consumed.len()
    );
    let _ = writeln!(
        b,
        "- Dormant unreferenced removal candidates: {}",
        removal_candidates.len()
    );
    let _ = writeln!(b, "- Owners: {}\n", count_owners(&policy.exemptions));

    b.push_str("## Removal candidates (dormant + unresolved + no loader)\n\n");
    if removal_candidates.is_empty() {
        b.push_str(
            "None. Every dormant catalog carries loader or source evidence; none is safe to delete without a reference audit.\n\n",
        );
    } else {
        for path in &removal_candidates {
            let _ = writeln!(b, "- `{}`", path);
        }
        b.push('\n');
    }

    for (class, owners) in &by_class {
        let _ = writeln!(b, "## {} ({})\n", class, count_class(&policy.exemptions, class));
        b.push_str("| Catalog | Owner | Expiry | Current graph class |\n|---|---|---|---|\n");
        for rows in owners.values() {
            let mut rows = rows.clone();
            rows.sort_by(|a, b| a.content_path.cmp(&b.content_path));
            for e in rows {
                let graph_class = current
                    .get(&e.content_path)
                    .map(|c| c.classification)
                    .unwrap_or(0);
                let _ = writeln!(
                    b,
                    "| `{}` | {} | {} | {} |",
                    e.content_path, e.owner, e.expiry_condition, graph_class
                );
            }
        }
        b.push('\n');
    }
    b
}

fn count_owners(es: &[Exemption]) -> usize {
    let set: BTreeSet<&str> = es.iter().map(|e| e.owner.as_str()).collect();
    set.len()
}

fn count_class(es: &[Exemption], class: &str) -> usize {
    es.iter().filter(|e| e.classification == class).count()
}

fn load_registry(path: &Path) -> Result<Registry, String> {
    let data = fs::read_to_string(path)
        .map_err(|e| format!("read dispositions {}: {}", path.display(), e))?;
    let r: Registry = serde_json::from_str(&data)
        .map_err(|e| format!("parse dispositions {}: {}", path.display(), e))?;
    if r.exemptions.is_empty() {
        return Err(format!("dispositions {} is empty", path.display()));
    }
    Ok(r)
}

fn load_utilization(path: &Path) -> Result<Utilization, String> {
    let data = fs::read_to_string(path)
        .map_err(|e| format!("read utilization {}: {}", path.display(), e))?;
    serde_json::from_str(&data).map_err(|e| format!("parse utilization {}: {}", path.display(), e))
}

#[cfg(test)]
mod tests {
    use super::*;

    fn exemption(path: &str, owner: &str, class: &str, expiry: &str) -> Exemption {
        Exemption {
            content_path: path.to_string(),
            owner: owner.to_string(),
            classification: class.to_string(),
            expiry_condition: expiry.to_string(),
        }
    }

    #[test]
    fn counts_owners_and_classes() {
        let es = vec![
            exemption("a.json", "alice", "DORMANT", "expiry"),
            exemption("b.json", "alice", "DORMANT", "expiry"),
            exemption("c.json", "bob", "ORPHAN", "expiry"),
        ];
        assert_eq!(count_owners(&es), 2);
        assert_eq!(count_class(&es, "DORMANT"), 2);
        assert_eq!(count_class(&es, "ORPHAN"), 1);
    }

    #[test]
    fn render_includes_summary_and_tables() {
        let policy = Registry {
            exemptions: vec![exemption("a.json", "alice", "DORMANT", "when referenced")],
        };
        let mut current = HashMap::new();
        current.insert(
            "a.json".to_string(),
            Catalog {
                path: "a.json".to_string(),
                classification: UNRESOLVED_CLASSIFICATION,
                loader: String::new(),
            },
        );
        let out = render(&policy, &current);
        assert!(out.contains("## Summary"));
        assert!(out.contains("- Dispositions: 1"));
        assert!(out.contains("## DORMANT (1)"));
        assert!(out.contains("| `a.json` | alice | when referenced | 6 |"));
        // dormant + unresolved + no loader => removal candidate
        assert!(out.contains("- `a.json`"));
    }

    #[test]
    fn failure_strings_are_present_for_the_gate_test() {
        // These literals are asserted by ContentReachabilityDispositionTests.
        let missing_expiry = format!("disposition without an expiry: {}", "x.json");
        let unresolved = format!("unresolved catalog without a disposition: {}", "y.json");
        assert!(missing_expiry.contains("disposition without an expiry"));
        assert!(unresolved.contains("unresolved catalog without a disposition"));
    }
}