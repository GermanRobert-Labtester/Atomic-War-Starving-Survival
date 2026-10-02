//! Port of `tools/gotools/pkg/proxy/proxy.go` — CLI surface only.
//!
//! `llm-proxy` is a resident HTTP reverse-proxy that blocks on
//! `http.ListenAndServe` until killed; no CI gate, pre-commit hook, or scoped
//! test invokes it, and a faithful port would require an async HTTP server
//! dependency the workspace deliberately does not carry. Rather than pretend to
//! serve, the Rust CLI prints an explicit unsupported notice and exits 2, the
//! same convention Stage 2 uses for `index --watch/--serve`.
//!
//! The Go implementation (routes, `/healthz`, single-host reverse proxy) lives
//! in `tools/gotools/pkg/proxy/proxy.go` and must be used for this server until
//! a later stage ports it.

/// Exact text the CLI emits for `llm-proxy`.
pub const UNSUPPORTED: &str = "[ashfall-dev] llm-proxy is not supported in the Rust port yet (Stage 3); use tools/gotools for the LLM proxy router.";

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn unsupported_notice_names_gotools_and_stage() {
        assert!(UNSUPPORTED.contains("not supported in the Rust port yet (Stage 3)"));
        assert!(UNSUPPORTED.contains("tools/gotools"));
    }
}