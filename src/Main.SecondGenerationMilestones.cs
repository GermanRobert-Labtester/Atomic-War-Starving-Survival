// SPDX-License-Identifier: MIT
// PLAN-GENERATIONAL-MILESTONE-TRUTH-160 — second-generation milestone host
// wiring. Evaluation records into ChildDevelopmentState via its owner.

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SecondGenerationMilestoneHostSession? _secondGenerationMilestones;
        public SecondGenerationMilestoneHostSession? SecondGenerationMilestones => _secondGenerationMilestones;

        public void SetupSecondGenerationMilestones()
        {
            if (_secondGenerationMilestones != null) return;
            EnsureChildDevelopment();
            _secondGenerationMilestones = new SecondGenerationMilestoneHostSession(_childDevelopment!.System);
        }

        public void TickSecondGenerationMilestones(int day)
        {
            _secondGenerationMilestones?.TickAll(day);
        }

        public void ResetSecondGenerationMilestones() { _secondGenerationMilestones = null; }
    }
}
