//! Port of `tools/gotools/pkg/config/validate.go`.
//!
//! JSON-Schema validation for quest / save / config files, used by the
//! `validate-config <schema.json> <file.[json|yaml]>` subcommand.
//!
//! Contract reproduced: the argument arity check and usage line, the `OK` stdout
//! line, and the exit codes (2 = bad invocation, 1 = validation failure,
//! 0 = valid). Failure output is one `- <instance-pointer>: <message>` line per
//! leaf error, matching `PrettyValidationError`/`collectErrors`.
//!
//! Documented limitation — message *wording*. The Go original uses
//! `santhosh-tekuri/jsonschema/v6`; this port uses the `jsonschema` crate.
//! Both flatten to leaf errors addressed by the same JSON pointer, but the
//! human-readable text of a failure differs because the two libraries word
//! `enum`/`additionalProperties`/`type` violations differently. Exit codes and
//! the line structure are identical, so an exit-code-driven caller cannot tell
//! the difference; a caller diffing stderr text on an *invalid* document can.
//! The valid path (`OK`) is byte-identical.
//!
//! Draft handling matches Go: the draft named by `$schema` is used, defaulting
//! to 2020-12 when absent. `$ref`s are resolved from disk (`file:` URIs) with the
//! schema file's own URL as the base URI, as the Go compiler does when handed an
//! absolute path.

use serde::{Deserialize, Serialize};
use std::fmt;
use std::fs;

/// Error returned by [`validate_file`] / [`validate_bytes`].
#[derive(Debug, Clone)]
pub struct ValidateError {
    /// Go's `err.Error()`: the wrapped message, or `validate <path>: <err>`.
    pub message: String,
    /// Pretty per-leaf lines when the failure is a schema violation.
    pub lines: Option<Vec<String>>,
}

impl ValidateError {
    fn msg(message: impl Into<String>) -> Self {
        Self {
            message: message.into(),
            lines: None,
        }
    }
}

impl fmt::Display for ValidateError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        f.write_str(&self.message)
    }
}

impl std::error::Error for ValidateError {}

/// Port of `PrettyValidationError`.
pub fn pretty_validation_error(err: &ValidateError) -> Vec<String> {
    match &err.lines {
        Some(lines) if !lines.is_empty() => lines.clone(),
        _ => vec![err.message.clone()],
    }
}

/// Percent-encode a filesystem path for use in a `file:` URI.
fn file_url(path: &str) -> String {
    let mut out = String::with_capacity(path.len() + 8);
    out.push_str("file://");
    for byte in path.bytes() {
        match byte {
            b'A'..=b'Z' | b'a'..=b'z' | b'0'..=b'9' | b'-' | b'.' | b'_' | b'~' | b'/' | b':' => {
                out.push(byte as char)
            }
            other => out.push_str(&format!("%{:02X}", other)),
        }
    }
    out
}

/// Compile the schema file, mirroring `jsonschema.NewCompiler().Compile(absSchema)`.
fn compile_validator(
    schema_path: &str,
) -> Result<jsonschema::Validator, ValidateError> {
    let raw = fs::read(schema_path)
        .map_err(|e| ValidateError::msg(format!("compile schema: {}", e)))?;
    let schema: serde_json::Value = serde_json::from_slice(&raw)
        .map_err(|e| ValidateError::msg(format!("compile schema: {}", e)))?;

    let mut options = jsonschema::options();
    if let Ok(abs) = fs::canonicalize(schema_path) {
        // Absolute base URI so relative `$ref`s resolve against the schema
        // directory, like Go compiling from `filepath.Abs`.
        options = options.with_base_uri(file_url(&abs.to_string_lossy()));
    }
    options
        .build(&schema)
        .map_err(|e| ValidateError::msg(format!("compile schema: {}", e)))
}

/// Build the prettified leaf-error lines for a failed validation.
fn validation_lines(
    instance_path: &str,
    validator: &jsonschema::Validator,
    instance: &serde_json::Value,
    root: &jsonschema::ValidationError<'_>,
) -> ValidateError {
    let mut lines: Vec<String> = Vec::new();
    for error in validator.iter_errors(instance) {
        let path = error.instance_path().as_str();
        // Go: `loc := "/" + strings.Join(ve.InstanceLocation, "/")`.
        let loc = if path.is_empty() { "/" } else { path };
        lines.push(format!("- {}: {}", loc, error));
    }
    if lines.is_empty() {
        lines.push(format!("- /: {}", root));
    }
    ValidateError {
        message: format!("validate {}: {}", instance_path, root),
        lines: Some(lines),
    }
}

/// Port of `ValidateFile`.
pub fn validate_file(schema_path: &str, instance_path: &str) -> Result<(), ValidateError> {
    let validator = compile_validator(schema_path)?;

    let data = fs::read(instance_path)
        .map_err(|e| ValidateError::msg(format!("read instance: {}", e)))?;

    // YAML first: YAML is a superset of JSON, so both formats load here.
    let instance: serde_json::Value = serde_yaml::from_slice(&data)
        .map_err(|e| ValidateError::msg(format!("parse YAML/JSON: {}", e)))?;

    match validator.validate(&instance) {
        Ok(()) => Ok(()),
        Err(root) => Err(validation_lines(
            instance_path,
            &validator,
            &instance,
            &root,
        )),
    }
}

/// Port of `ValidateBytes`.
#[allow(dead_code)]
pub fn validate_bytes(schema_path: &str, data: &[u8]) -> Result<(), ValidateError> {
    let validator = compile_validator(schema_path)?;
    let instance: serde_json::Value = serde_yaml::from_slice(data)
        .map_err(|e| ValidateError::msg(format!("parse YAML/JSON: {}", e)))?;
    match validator.validate(&instance) {
        Ok(()) => Ok(()),
        Err(root) => Err(ValidateError::msg(root.to_string())),
    }
}

// ---------------------------------------------------------------------------
// Typed game structs with two-step validation (Go: Quest / Objective / Reward /
// Save). Only exercised by the package's own tests upstream; the CLI never
// calls them, so they are allowed to be unreachable here too.
// ---------------------------------------------------------------------------

#[derive(Debug, Clone, Serialize, Deserialize)]
#[allow(dead_code)]
pub struct Quest {
    pub id: String,
    pub title: String,
    #[serde(default, skip_serializing_if = "Option::is_none")]
    pub description: Option<String>,
    pub objectives: Vec<Objective>,
    #[serde(default, skip_serializing_if = "Option::is_none")]
    pub rewards: Option<Reward>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[allow(dead_code)]
pub struct Objective {
    pub id: String,
    #[serde(rename = "type")]
    pub kind: String,
    #[serde(default, skip_serializing_if = "is_zero")]
    pub count: i64,
    #[serde(default, skip_serializing_if = "Option::is_none")]
    pub target: Option<String>,
}

/// `omitempty` for the Go `int` fields below; reached only through the derived
/// `Serialize` impls, which the CLI never calls.
#[allow(dead_code)]
fn is_zero(v: &i64) -> bool {
    *v == 0
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[allow(dead_code)]
pub struct Reward {
    #[serde(default, skip_serializing_if = "is_zero")]
    pub xp: i64,
    #[serde(default, skip_serializing_if = "Option::is_none")]
    pub items: Option<Vec<String>>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[allow(dead_code)]
pub struct Save {
    pub version: i64,
    pub player_id: String,
    pub quests: Vec<SaveQuest>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[allow(dead_code)]
pub struct SaveQuest {
    pub quest_id: String,
    pub state: String,
}

/// Port of `LoadQuest`.
#[allow(dead_code)]
pub fn load_quest(schema_path: &str, path: &str) -> Result<Quest, ValidateError> {
    validate_file(schema_path, path)?;
    let data = fs::read(path).map_err(|e| ValidateError::msg(e.to_string()))?;
    serde_yaml::from_slice(&data).map_err(|e| ValidateError::msg(format!("unmarshal quest: {}", e)))
}

/// Port of `LoadSave`.
#[allow(dead_code)]
pub fn load_save(schema_path: &str, path: &str) -> Result<Save, ValidateError> {
    validate_file(schema_path, path)?;
    let data = fs::read(path).map_err(|e| ValidateError::msg(e.to_string()))?;
    serde_yaml::from_slice(&data).map_err(|e| ValidateError::msg(format!("unmarshal save: {}", e)))
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::path::{Path, PathBuf};

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-config-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    /// Locate the repository's `schemas/` directory from the crate manifest dir
    /// (`tools/rstools/crates/ashfall-dev` -> repository root is three up).
    fn schemas_dir() -> PathBuf {
        let mut p = PathBuf::from(env!("CARGO_MANIFEST_DIR"));
        p.pop(); // crates
        p.pop(); // rstools
        p.pop(); // tools
        p.push("schemas");
        p
    }

    fn write(dir: &Path, name: &str, body: &str) -> String {
        let p = dir.join(name);
        fs::write(&p, body).unwrap();
        p.to_string_lossy().into_owned()
    }

    #[test]
    fn valid_yaml_quest_passes_and_loads() {
        let schema = schemas_dir().join("quest.schema.json");
        if !schema.exists() {
            eprintln!("skipping: {} not present", schema.display());
            return;
        }
        let dir = tmpdir("quest-ok");
        let f = write(
            &dir,
            "quest_valid.yaml",
            r#"
id: rescue_survivor_01
title: "Rescue Dr. Vance"
description: "Locate and extract Dr. Vance from the collapsed shelter."
objectives:
  - id: reach_shelter
    type: reach
    target: shelter_collapsed_04
  - id: talk_doctor
    type: talk
    target: npc_vance
rewards:
  xp: 150
  items:
    - medkit_military
"#,
        );

        assert!(validate_file(&schema.to_string_lossy(), &f).is_ok());

        let q = load_quest(&schema.to_string_lossy(), &f).unwrap();
        assert_eq!(q.id, "rescue_survivor_01");
        assert_eq!(q.objectives.len(), 2);
        assert_eq!(q.rewards.as_ref().unwrap().xp, 150);
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn invalid_quest_fails_with_pointer_addressed_lines() {
        let schema = schemas_dir().join("quest.schema.json");
        if !schema.exists() {
            eprintln!("skipping: {} not present", schema.display());
            return;
        }
        let dir = tmpdir("quest-bad");
        let f = write(
            &dir,
            "quest_invalid.yaml",
            r#"
id: invalid_quest
title: "Invalid Quest"
objectives:
  - id: obj_1
    type: dance
rewards:
  gold: 100
"#,
        );

        let err = validate_file(&schema.to_string_lossy(), &f).unwrap_err();
        let lines = pretty_validation_error(&err);
        assert!(!lines.is_empty());
        assert!(lines.iter().all(|l| l.starts_with("- /")), "lines: {lines:?}");
        // The offending enum value must be located by JSON pointer.
        assert!(
            lines.iter().any(|l| l.contains("/objectives/0/type")),
            "lines: {lines:?}"
        );
        assert!(err.message.starts_with("validate "));
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn valid_json_save_passes_and_loads() {
        let schema = schemas_dir().join("save.schema.json");
        if !schema.exists() {
            eprintln!("skipping: {} not present", schema.display());
            return;
        }
        let dir = tmpdir("save-ok");
        let f = write(
            &dir,
            "save_slot_01.json",
            r#"{
  "version": 1,
  "player_id": "survivor_42",
  "quests": [
    {
      "quest_id": "rescue_survivor_01",
      "state": "active"
    }
  ]
}"#,
        );

        assert!(validate_file(&schema.to_string_lossy(), &f).is_ok());
        let s = load_save(&schema.to_string_lossy(), &f).unwrap();
        assert_eq!(s.version, 1);
        assert_eq!(s.player_id, "survivor_42");
        assert_eq!(s.quests.len(), 1);
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn invalid_save_state_is_rejected() {
        let schema = schemas_dir().join("save.schema.json");
        if !schema.exists() {
            eprintln!("skipping: {} not present", schema.display());
            return;
        }
        let dir = tmpdir("save-bad");
        let f = write(
            &dir,
            "save_bad.json",
            r#"{"version": 1, "player_id": "p", "quests": [{"quest_id": "q", "state": "bogus"}]}"#,
        );
        assert!(validate_file(&schema.to_string_lossy(), &f).is_err());
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn missing_instance_reports_read_instance() {
        let schema = schemas_dir().join("save.schema.json");
        if !schema.exists() {
            eprintln!("skipping: {} not present", schema.display());
            return;
        }
        let err = validate_file(
            &schema.to_string_lossy(),
            "/nonexistent/ashfall-instance.json",
        )
        .unwrap_err();
        assert!(err.message.starts_with("read instance: "), "{}", err.message);
        assert_eq!(pretty_validation_error(&err), vec![err.message.clone()]);
    }

    #[test]
    fn missing_schema_reports_compile_schema() {
        let err = validate_file("/nonexistent/schema.json", "/nonexistent/x.json").unwrap_err();
        assert!(err.message.starts_with("compile schema: "), "{}", err.message);
    }

    #[test]
    fn base_uri_encoding_leaves_a_valid_uri_for_spaced_paths() {
        assert_eq!(
            file_url("/home/a b/schemas/q.json"),
            "file:///home/a%20b/schemas/q.json"
        );
    }
}