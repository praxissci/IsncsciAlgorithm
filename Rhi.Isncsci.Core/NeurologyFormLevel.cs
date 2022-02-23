namespace Rhi.Isncsci.Core
{
	public class NeurologyFormLevel
    {
        public virtual bool IsKeyMuscle { get; set; }
        public virtual bool IsLowerMuscle { get; set; }
        public virtual bool LeftTouchImpairmentNotDueToSci { get; set; }
        public virtual string LeftTouchName { get; set; }
        public virtual int LeftTouchValue { get; set; }
        public virtual bool LeftPrickImpairmentNotDueToSci { get; set; }
        public virtual string LeftPrickName { get; set; }
        public virtual int LeftPrickValue { get; set; }
        public virtual bool LeftMotorImpairmentNotDueToSci { get; set; }
        public virtual bool HasOtherLeftMotorFunction { get; set; }
        public virtual string LeftMotorName { get; set; }
        public virtual int LeftMotorValue { get; set; }
        public virtual string Name { get; set; }
        public virtual NeurologyFormLevel Next { get; set; }
        //public virtual NeurologyTest NeurologyTest { get; set; }
        public virtual int Ordinal { get; set; }
        public virtual NeurologyFormLevel Previous { get; set; }
        public virtual bool RightTouchImpairmentNotDueToSci { get; set; }
        public virtual string RightTouchName { get; set; }
        public virtual int RightTouchValue { get; set; }
        public virtual bool RightPrickImpairmentNotDueToSci { get; set; }
        public virtual string RightPrickName { get; set; }
        public virtual int RightPrickValue { get; set; }
        public virtual bool RightMotorImpairmentNotDueToSci { get; set; }
        public bool HasOtherRightMotorFunction { get; set; }
        public virtual string RightMotorName { get; set; }
        public virtual int RightMotorValue { get; set; }

        public virtual NeurologyFormLevel SetValues(int ordinal, bool isKeyMuscle, bool isLowerMuscle,
            string rightTouchName, int rightTouchValue, bool rightTouchImpairmentNotDueToSci,
            string leftTouchName, int leftTouchValue, bool leftTouchImpairmentNotDueToSci,
            string rightPrickName, int rightPrickValue, bool rightPrickImpairmentNotDueToSci,
            string leftPrickName, int leftPrickValue, bool leftPrickImpairmentNotDueToSci,
            string rightMotorName, int rightMotorValue, bool rightMotorImpairmentNotDueToSci,
            string leftMotorName, int leftMotorValue, bool leftMotorImpairmentNotDueToSci)
        {
            IsKeyMuscle = isKeyMuscle;
            IsLowerMuscle = isLowerMuscle;
            Ordinal = ordinal;

            RightTouchName = rightPrickName;
            RightTouchValue = rightPrickValue;
            RightTouchImpairmentNotDueToSci = rightTouchImpairmentNotDueToSci;

            LeftTouchName = leftTouchName;
            LeftTouchValue = leftTouchValue;
            LeftTouchImpairmentNotDueToSci = leftTouchImpairmentNotDueToSci;

            RightPrickName = rightPrickName;
            RightPrickValue = rightPrickValue;
            RightPrickImpairmentNotDueToSci = rightPrickImpairmentNotDueToSci;

            LeftPrickName = leftPrickName;
            LeftPrickValue = leftPrickValue;
            LeftPrickImpairmentNotDueToSci = leftPrickImpairmentNotDueToSci;

            RightMotorName = rightMotorName;
            RightMotorValue = rightMotorValue;
            RightMotorImpairmentNotDueToSci = rightMotorImpairmentNotDueToSci;

            LeftMotorName = leftMotorName;
            LeftMotorValue = leftMotorValue;
            LeftMotorImpairmentNotDueToSci = leftMotorImpairmentNotDueToSci;

            return this;
        }
    }
}
