# Utility Curve Contract

> **Curve Specification:** Structure, sorting, interpolation, and boundary handling implemented in `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs` (`ResponseCurve`).

---

## 1. Mathematical Structure

- **Representation:** An array of `CurvePoint` instances: `{ "x": float, "y": float }`.
- **Pre-Processing / Sorting:** `ResponseCurve` sorts points by ascending `x`. Malformed JSON out-of-order points are sorted in-memory during construction.
- **Empty Curve Fallback:** `ResponseCurve.Identity` (`x=0, y=0; x=1, y=1`) returns the input `x` unchanged.
- **Single Point:** If only 1 point exists, `Evaluate(x)` returns `points[0].y` for all `x`.

---

## 2. Interpolation Formula

For `x` between `points[i-1]` and `points[i]`:
```text
span = b.x - a.x
if span <= 1e-6:
    return b.y
t = (x - a.x) / span
return a.y + (b.y - a.y) * t
```

---

## 3. Boundary & Clamping Behavior

- **Left-Clamp (`x <= points[0].x`):** Returns `points[0].y`.
- **Right-Clamp (`x >= points[last].x`):** Returns `points[last].y`.
- Extrapolation is explicitly forbidden: points outside the authored range clamp safely to the nearest boundary key.
