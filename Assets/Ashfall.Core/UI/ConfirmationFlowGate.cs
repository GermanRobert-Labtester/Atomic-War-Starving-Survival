// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Pure Core gate enforcing the confirmation contract (Section 16 / FEEDBACK_CONFIRMATION_CONTRACT):
    /// 1. Action executes at most once upon Confirm.
    /// 2. Cancel executes zero domain mutations.
    /// 3. Double-clicks/rapid submissions are blocked once resolved (single-submission lock).
    /// 4. Re-validation predicate can abort execution if state changed while dialog was open.
    /// </summary>
    public sealed class ConfirmationFlowGate
    {
        private readonly Action _onConfirm;
        private readonly Action? _onCancel;
        private readonly Func<bool>? _isValidityPredicate;
        private bool _resolved;

        public bool IsPending => !_resolved;

        public ConfirmationFlowGate(Action onConfirm, Action? onCancel = null, Func<bool>? isValidityPredicate = null)
        {
            _onConfirm = onConfirm ?? throw new ArgumentNullException(nameof(onConfirm));
            _onCancel = onCancel;
            _isValidityPredicate = isValidityPredicate;
        }

        /// <summary>
        /// Attempts to execute the confirmed action. Returns true if executed; false if already resolved or invalid.
        /// </summary>
        public bool Confirm()
        {
            if (_resolved) return false;
            _resolved = true;

            if (_isValidityPredicate != null && !_isValidityPredicate())
            {
                return false;
            }

            _onConfirm();
            return true;
        }

        /// <summary>
        /// Cancels the confirmation flow. Resolves the gate and invokes onCancel callback without domain mutation.
        /// </summary>
        public void Cancel()
        {
            if (_resolved) return;
            _resolved = true;

            _onCancel?.Invoke();
        }
    }
}
