//! Port of `tools/gotools/pkg/orchestrator/orchestrator.go` — CLI surface only.
//!
//! `agent-core` is a resident HTTP task-orchestrator (`NewAgentCoreServer` +
//! `Start`, blocking on `http.ListenAndServe`); no CI gate, pre-commit hook, or
//! scoped test invokes it, and a faithful port would require an async HTTP
//! server dependency the workspace deliberately does not carry. Rather than
//! pretend to serve, the Rust CLI prints an explicit unsupported notice and
//! exits 2, the same convention Stage 2 uses for `index --watch/--serve`.
//!
//! The Go implementation (task records, `/agent/task`, `/agent/tasks`,
//! `/healthz`) lives in `tools/gotools/pkg/orchestrator/orchestrator.go` and
//! must be used until a later stage ports it.

/// Exact text the CLI emits for `agent-core`.
pub const UNSUPPORTED: &str = "[ashfall-dev] agent-core is not supported in the Rust port yet (Stage 3); use tools/gotools for the resident agent orchestrator.";

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn unsupported_notice_names_gotools_and_stage() {
        assert!(UNSUPPORTED.contains("not supported in the Rust port yet (Stage 3)"));
        assert!(UNSUPPORTED.contains("tools/gotools"));
    }
}