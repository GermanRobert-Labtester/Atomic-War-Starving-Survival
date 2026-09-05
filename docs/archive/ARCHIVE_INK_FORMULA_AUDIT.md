# Archive Ink Formula Audit

> **Formula & Decay Semantics:** Mathematical relationship between initial legibility, elapsed campaign days, fade rate, and archival longevity.

---

## 1. Runtime Formula

The effective legibility of a transcribed document over campaign time $t$ (days since transcription) is:

$$\text{Legibility}(t) = \max\left(0, L_0 - F \times t\right)$$

Where:
- $L_0$ = Initial `legibility_score` $[0.30, 1.00]$
- $F$ = `fade_rate_per_day` $[0.0005, 0.0200]$
- $t$ = Elapsed campaign days $[0, \infty)$

---

## 2. Terminal Longevity Boundary

`archival_longevity_days` ($T_{\text{max}}$) represents the structural degradation threshold of the binder and substrate:
- When $t \ge T_{\text{max}}$, the paper or substrate itself experiences irreversible physical brittleness/flaking.
- Documents are considered fully illegible when either $\text{Legibility}(t) = 0$ or $t \ge T_{\text{max}}$.
- The readable threshold for research and codex unlocking is typically $\text{Legibility} \ge 0.20$.
