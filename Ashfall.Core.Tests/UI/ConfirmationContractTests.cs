// SPDX-License-Identifier: MIT
using System;
using Xunit;
using Ashfall.Core.UI;

namespace Ashfall.Core.Tests.UI
{
    public class ConfirmationContractTests
    {
        [Fact]
        public void Confirm_ExecutesActionExactlyOnce()
        {
            int executeCount = 0;
            var gate = new ConfirmationFlowGate(() => executeCount++);

            Assert.True(gate.IsPending);
            bool result = gate.Confirm();

            Assert.True(result);
            Assert.Equal(1, executeCount);
            Assert.False(gate.IsPending);
        }

        [Fact]
        public void Confirm_MultipleSubmissions_ExecutesOnlyFirstTime()
        {
            int executeCount = 0;
            var gate = new ConfirmationFlowGate(() => executeCount++);

            bool first = gate.Confirm();
            bool second = gate.Confirm();
            bool third = gate.Confirm();

            Assert.True(first);
            Assert.False(second);
            Assert.False(third);
            Assert.Equal(1, executeCount);
        }

        [Fact]
        public void Cancel_ExecutesCancelCallback_AndZeroDomainMutations()
        {
            int actionCount = 0;
            int cancelCount = 0;
            var gate = new ConfirmationFlowGate(
                onConfirm: () => actionCount++,
                onCancel: () => cancelCount++
            );

            Assert.True(gate.IsPending);
            gate.Cancel();

            Assert.Equal(0, actionCount);
            Assert.Equal(1, cancelCount);
            Assert.False(gate.IsPending);
        }

        [Fact]
        public void Cancel_ThenConfirm_DoesNotExecuteAction()
        {
            int actionCount = 0;
            var gate = new ConfirmationFlowGate(() => actionCount++);

            gate.Cancel();
            bool confirmResult = gate.Confirm();

            Assert.False(confirmResult);
            Assert.Equal(0, actionCount);
        }

        [Fact]
        public void Confirm_ThenCancel_DoesNotInvokeCancelCallback()
        {
            int actionCount = 0;
            int cancelCount = 0;
            var gate = new ConfirmationFlowGate(
                onConfirm: () => actionCount++,
                onCancel: () => cancelCount++
            );

            gate.Confirm();
            gate.Cancel();

            Assert.Equal(1, actionCount);
            Assert.Equal(0, cancelCount);
        }

        [Fact]
        public void Confirm_WhenValidityPredicateFails_AbortsExecution()
        {
            int actionCount = 0;
            bool stateValid = false;
            var gate = new ConfirmationFlowGate(
                onConfirm: () => actionCount++,
                isValidityPredicate: () => stateValid
            );

            bool result = gate.Confirm();

            Assert.False(result);
            Assert.Equal(0, actionCount);
            Assert.False(gate.IsPending);
        }

        [Fact]
        public void Confirm_WhenValidityPredicatePasses_ExecutesAction()
        {
            int actionCount = 0;
            bool stateValid = true;
            var gate = new ConfirmationFlowGate(
                onConfirm: () => actionCount++,
                isValidityPredicate: () => stateValid
            );

            bool result = gate.Confirm();

            Assert.True(result);
            Assert.Equal(1, actionCount);
            Assert.False(gate.IsPending);
        }

        [Fact]
        public void Constructor_NullConfirmAction_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfirmationFlowGate(null!));
        }
    }
}
