using System;

namespace BehaveN.Tests
{
    public class StubReporter : Reporter
    {
        private int _undefinedSteps;
        private int _pendingSteps;
        private int _passedSteps;
        private int _failedSteps;
        private int _skippedSteps;

        [CoverageExclude]
        public override void ReportDescription(string description)
        {
        }

        public override void ReportUndefined(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            _undefinedSteps++;
        }

        public override void ReportPending(StepType stepType, string description)
        {
            _pendingSteps++;
        }

        public override void ReportPassed(StepType stepType, string description)
        {
            _passedSteps++;
        }

        public override void ReportFailed(StepType stepType, string description, Exception e)
        {
            _failedSteps++;
        }

        public override void ReportSkipped(StepType stepType, string description)
        {
            _skippedSteps++;
        }

        public override void ReportConvertibleObject(IConvertibleObject convertibleObject)
        {
        }

        public override void ReportEnd()
        {
        }

        public int UndefinedSteps { get { return _undefinedSteps; } }
        public int PendingSteps { get { return _pendingSteps; } }
        public int PassedSteps { get { return _passedSteps; } }
        public int FailedSteps { get { return _failedSteps; } }
        public int SkippedSteps { get { return _skippedSteps; } }
    }
}
