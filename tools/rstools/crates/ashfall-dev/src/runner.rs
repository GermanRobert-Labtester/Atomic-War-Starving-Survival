//! Port of `tools/gotools/pkg/runner/runner.go`.
//!
//! JSON parity contract (consumed by `scripts/ci/run-gates.py`): `Task.timeout`
//! is nanoseconds; `TaskResult` exposes `id`, `exit_code`, `duration` (ns),
//! `duration_seconds`, `output`, `peak_rss_kb`, optional `error`, `timed_out`.
//!
//! Unix-only: peak RSS comes from `wait4(2)`'s `rusage`, exactly as the Go
//! original reads `ProcessState.SysUsage().Maxrss`.

use serde::{Deserialize, Serialize};
use std::io::Read;
use std::process::{Command, Stdio};
use std::sync::atomic::{AtomicUsize, Ordering};
use std::sync::{Arc, Mutex};
use std::time::{Duration, Instant};

const DEFAULT_TIMEOUT_NS: i64 = 180 * 1_000_000_000;
const TIMEOUT_EXIT_CODE: i32 = 124;

#[derive(Debug, Clone, Deserialize, Serialize)]
pub struct Task {
    pub id: String,
    pub command: String,
    #[serde(default)]
    pub args: Vec<String>,
    #[serde(default)]
    pub working_dir: String,
    /// nanoseconds (Go `time.Duration` JSON encoding)
    #[serde(default)]
    pub timeout: i64,
}

#[derive(Debug, Clone, Serialize)]
pub struct TaskResult {
    pub id: String,
    pub exit_code: i32,
    pub duration: i64,
    pub duration_seconds: f64,
    pub output: String,
    pub peak_rss_kb: i64,
    #[serde(skip_serializing_if = "String::is_empty")]
    pub error: String,
    pub timed_out: bool,
}

impl TaskResult {
    fn failure(id: &str, error: String) -> Self {
        TaskResult {
            id: id.to_string(),
            exit_code: 1,
            duration: 0,
            duration_seconds: 0.0,
            output: String::new(),
            peak_rss_kb: 0,
            error,
            timed_out: false,
        }
    }
}

#[cfg(unix)]
fn reap_with_timeout(
    pid: libc::pid_t,
    timeout: Duration,
    start: Instant,
) -> (i32, i64, bool) {
    let mut status: libc::c_int = 0;
    let mut rusage: libc::rusage = unsafe { std::mem::zeroed() };

    let mut timed_out = false;
    let mut reaped = false;

    loop {
        let r = unsafe { libc::wait4(pid, &mut status, libc::WNOHANG, &mut rusage) };
        if r == pid {
            reaped = true;
            break;
        }
        if r < 0 {
            // No child (already reaped) or hard error.
            break;
        }
        if start.elapsed() >= timeout {
            timed_out = true;
            unsafe {
                libc::kill(pid, libc::SIGKILL);
            }
            let _ = unsafe { libc::wait4(pid, &mut status, 0, &mut rusage) };
            reaped = true;
            break;
        }
        std::thread::sleep(Duration::from_millis(5));
    }

    if !reaped {
        return (1, 0, timed_out);
    }

    let peak_rss_kb = rusage.ru_maxrss as i64;

    if timed_out {
        return (TIMEOUT_EXIT_CODE, peak_rss_kb, true);
    }

    let exit_code = if libc::WIFEXITED(status) {
        libc::WEXITSTATUS(status)
    } else if libc::WIFSIGNALED(status) {
        // Go's ExitCode() returns -1 when killed by a signal.
        -1
    } else {
        1
    };
    (exit_code, peak_rss_kb, false)
}

pub fn execute_task(task: &Task) -> TaskResult {
    let timeout_ns = if task.timeout <= 0 {
        DEFAULT_TIMEOUT_NS
    } else {
        task.timeout
    };
    let timeout = Duration::from_nanos(timeout_ns as u64);

    let mut cmd = Command::new(&task.command);
    cmd.args(&task.args);
    if !task.working_dir.is_empty() {
        cmd.current_dir(&task.working_dir);
    }
    cmd.stdin(Stdio::null());
    cmd.stdout(Stdio::piped());
    cmd.stderr(Stdio::piped());

    let start = Instant::now();
    let mut child = match cmd.spawn() {
        Ok(c) => c,
        Err(e) => return TaskResult::failure(&task.id, e.to_string()),
    };

    let stdout = child.stdout.take();
    let stderr = child.stderr.take();
    let out_handle = stdout.map(|mut s| {
        std::thread::spawn(move || {
            let mut buf = Vec::new();
            let _ = s.read_to_end(&mut buf);
            buf
        })
    });
    let err_handle = stderr.map(|mut s| {
        std::thread::spawn(move || {
            let mut buf = Vec::new();
            let _ = s.read_to_end(&mut buf);
            buf
        })
    });

    let pid = child.id() as libc::pid_t;

    #[cfg(unix)]
    let (exit_code, peak_rss_kb, timed_out) = reap_with_timeout(pid, timeout, start);

    #[cfg(not(unix))]
    let (exit_code, peak_rss_kb, timed_out) = {
        // Fallback for non-unix hosts (no rusage available).
        loop {
            match child.try_wait() {
                Ok(Some(status)) => break (status.code().unwrap_or(-1), 0, false),
                Ok(None) => {
                    if start.elapsed() >= timeout {
                        let _ = child.kill();
                        let _ = child.wait();
                        break (TIMEOUT_EXIT_CODE, 0, true);
                    }
                    std::thread::sleep(Duration::from_millis(5));
                }
                Err(_) => break (1, 0, false),
            }
        }
    };

    let mut output = String::new();
    if let Some(h) = out_handle {
        if let Ok(buf) = h.join() {
            output.push_str(&String::from_utf8_lossy(&buf));
        }
    }
    if let Some(h) = err_handle {
        if let Ok(buf) = h.join() {
            output.push_str(&String::from_utf8_lossy(&buf));
        }
    }

    let duration = start.elapsed();
    let error = if timed_out {
        format!("task exceeded timeout of {:?}", timeout)
    } else {
        String::new()
    };

    TaskResult {
        id: task.id.clone(),
        exit_code,
        duration: duration.as_nanos() as i64,
        duration_seconds: duration.as_secs_f64(),
        output,
        peak_rss_kb,
        error,
        timed_out,
    }
}

/// Port of `TaskRunnerPool.RunTasks` (errgroup with `SetLimit`).
pub fn run_tasks(tasks: &[Task], concurrency: usize) -> Vec<TaskResult> {
    let n = tasks.len();
    if n == 0 {
        return Vec::new();
    }
    let workers = if concurrency == 0 { 4 } else { concurrency }.min(n);

    let shared = Arc::new(tasks.to_vec());
    let next = Arc::new(AtomicUsize::new(0));
    let slots: Arc<Mutex<Vec<Option<TaskResult>>>> = Arc::new(Mutex::new((0..n).map(|_| None).collect()));

    let mut handles = Vec::with_capacity(workers);
    for _ in 0..workers {
        let shared = Arc::clone(&shared);
        let next = Arc::clone(&next);
        let slots = Arc::clone(&slots);
        handles.push(std::thread::spawn(move || loop {
            let i = next.fetch_add(1, Ordering::SeqCst);
            if i >= shared.len() {
                break;
            }
            let result = execute_task(&shared[i]);
            slots.lock().unwrap()[i] = Some(result);
        }));
    }
    for h in handles {
        let _ = h.join();
    }

    let guard = slots.lock().unwrap();
    guard.iter().map(|slot| slot.clone().unwrap()).collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn captures_output_and_exit_code() {
        let r = execute_task(&Task {
            id: "ok".into(),
            command: "bash".into(),
            args: vec!["-c".into(), "echo hello; exit 0".into()],
            working_dir: String::new(),
            timeout: 5_000_000_000,
        });
        assert_eq!(r.exit_code, 0);
        assert!(r.output.contains("hello"));
        assert!(!r.timed_out);
        assert!(r.error.is_empty());
    }

    #[test]
    fn nonzero_exit_is_reported() {
        let r = execute_task(&Task {
            id: "fail".into(),
            command: "bash".into(),
            args: vec!["-c".into(), "exit 7".into()],
            working_dir: String::new(),
            timeout: 5_000_000_000,
        });
        assert_eq!(r.exit_code, 7);
    }

    #[test]
    fn timeout_kills_and_marks_124() {
        let r = execute_task(&Task {
            id: "slow".into(),
            command: "bash".into(),
            args: vec!["-c".into(), "sleep 30".into()],
            working_dir: String::new(),
            timeout: 200_000_000, // 200ms
        });
        assert_eq!(r.exit_code, 124);
        assert!(r.timed_out);
        assert!(r.error.contains("timeout"));
        assert!(r.duration_seconds < 5.0);
    }

    #[test]
    fn missing_command_reports_error() {
        let r = execute_task(&Task {
            id: "missing".into(),
            command: "definitely-not-a-real-binary-xyz".into(),
            args: vec![],
            working_dir: String::new(),
            timeout: 1_000_000_000,
        });
        assert_eq!(r.exit_code, 1);
        assert!(!r.error.is_empty());
    }

    #[test]
    fn pool_preserves_input_order_and_runs_all() {
        let tasks: Vec<Task> = (0..5)
            .map(|i| Task {
                id: format!("t{}", i),
                command: "bash".into(),
                args: vec!["-c".into(), format!("echo {}", i)],
                working_dir: String::new(),
                timeout: 5_000_000_000,
            })
            .collect();
        let results = run_tasks(&tasks, 4);
        assert_eq!(results.len(), 5);
        for (i, r) in results.iter().enumerate() {
            assert_eq!(r.id, format!("t{}", i));
            assert_eq!(r.exit_code, 0);
        }
    }

    #[test]
    fn peak_rss_is_recorded_on_unix() {
        let r = execute_task(&Task {
            id: "rss".into(),
            command: "bash".into(),
            args: vec!["-c".into(), "echo ok".into()],
            working_dir: String::new(),
            timeout: 5_000_000_000,
        });
        assert!(r.peak_rss_kb > 0);
    }

    #[test]
    fn result_json_shape_matches_gates_consumer() {
        let r = execute_task(&Task {
            id: "shape".into(),
            command: "true".into(),
            args: vec![],
            working_dir: String::new(),
            timeout: 5_000_000_000,
        });
        let v = serde_json::to_value(&r).unwrap();
        assert_eq!(v["id"], "shape");
        assert_eq!(v["exit_code"], 0);
        assert!(v.get("duration_seconds").is_some());
        assert!(v.get("output").is_some());
        assert_eq!(v["timed_out"], false);
        // error omitted when empty (Go `omitempty`)
        assert!(v.get("error").is_none());
    }
}