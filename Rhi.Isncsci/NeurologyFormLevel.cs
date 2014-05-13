/* Copyright © 2014 Rick Hansen Institute
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the terms of the Apache License,
 * Version 2.0 (the "License");
 * you may not use this software except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES
 * OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 * 
 * This software, developed by the Rick Hansen Institute (RHI) in collaboration with the International Spinal Cord Society (ISCoS) as well as industry
 * experts http://www.isncscialgorithm.com/Home/Team/, provides a tool which utilizes the raw test scores determined by performing the exam to electronically
 * score and classify a spinal cord injury using the American Spinal Injury Association's (ASIA) International Standards for Neurological Classification of
 * Spinal Cord Injury Revised 2011 scoring rules. This is being provided without liability on the part of RHI and no representation or warranty with respect
 * to the content is being made by ASIA or ISCoS.  Any modifications to the software may invalidate the algorithm.
 * 
 * If you would like  to modify it in any  way, we ask that you include the below citation on any modification and that you share a copy of your modification
 * with us at isncscialgorithm@rickhanseninstitute.org . Thank you for your cooperation.
 * 
 * Citation to be included in any use (or modification): “(Modified by [X] from)  ISNCSCI Algorithm. (V. 1.0, 2014). http://www.isncscialgorithm.com The Rick Hansen Institute, Vancouver, British Columbia.” 
 * 
 * Recommended citation for publications:
 * ISNCSCI Algorithm. (V. 1.0, 2014). http://www.isncscialgorithm.com  The Rick Hansen Institute, Vancouver, British Columbia. (accessed DD/MM/YYYY)
*/
namespace Rhi.Isncsci
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