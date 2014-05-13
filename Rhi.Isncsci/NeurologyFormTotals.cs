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
using System.Collections.Generic;
using System.Linq;

namespace Rhi.Isncsci
{
    public class NeurologyFormTotals
    {
        public int RightUpperMotorTotal;
        public bool RightUpperMotorTotalHasImpairmentNotDueToSci;
        public bool RightUpperMotorContainsNt;
        public int LeftUpperMotorTotal;
        public bool LeftUpperMotorTotalHasImpairmentNotDueToSci;
        public bool LeftUpperMotorContainsNt;
        public int RightLowerMotorTotal;
        public bool RightLowerMotorTotalHasImpairmentNotDueToSci;
        public bool RightLowerMotorContainsNt;
        public int LeftLowerMotorTotal;
        public bool LeftLowerMotorTotalHasImpairmentNotDueToSci;
        public bool LeftLowerMotorContainsNt;
        public int RightTouchTotal;
        public bool RightTouchTotalHasImpairmentNotDueToSci;
        public bool RightTouchContainsNt;
        public int LeftTouchTotal;
        public bool LeftTouchTotalHasImpairmentNotDueToSci;
        public bool LeftTouchContainsNt;
        public int RightPrickTotal;
        public bool RightPrickTotalHasImpairmentNotDueToSci;
        public bool RightPrickContainsNt;
        public int LeftPrickTotal;
        public bool LeftPrickTotalHasImpairmentNotDueToSci;
        public bool LeftPrickContainsNt;

        public int UpperMotorTotal;
        public int LowerMotorTotal;
        public int TouchTotal;
        public int PrickTotal;

        #region Right Sensory
        private readonly List<NeurologyFormLevel> _rightSensoryValues = new List<NeurologyFormLevel>();
        public bool RightSensoryHasOnlySoftValues = true;

        public bool RightSensoryIsEmpty()
        {
            return !_rightSensoryValues.Any();
        }

        public void AddRightSensoryValue(NeurologyFormLevel level)
        {
            if (_rightSensoryValues.Any(l => l.Name.ToUpper() == level.Name))
                return;

            _rightSensoryValues.Add(level);
        }

        public List<NeurologyFormLevel> GetRightSensoryValues()
        {
            return new List<NeurologyFormLevel>(_rightSensoryValues);
        }

        public bool RightSensoryContains(string levelName)
        {
            return _rightSensoryValues.Any(l => l.Name.ToUpper() == levelName.ToUpper());
        }
        #endregion

        #region Left Sensory
        private readonly List<NeurologyFormLevel> _leftSensoryValues = new List<NeurologyFormLevel>();
        public bool LeftSensoryHasOnlySoftValues = true;

        public bool LeftSensoryIsEmpty()
        {
            return !_leftSensoryValues.Any();
        }

        public void AddLeftSensoryValue(NeurologyFormLevel level)
        {
            if (_leftSensoryValues.Any(l => l.Name.ToUpper() == level.Name))
                return;

            _leftSensoryValues.Add(level);
        }

        public List<NeurologyFormLevel> GetLeftSensoryValues()
        {
            return new List<NeurologyFormLevel>(_leftSensoryValues);
        }

        public bool LeftSensoryContains(string levelName)
        {
            return _leftSensoryValues.Any(l => l.Name.ToUpper() == levelName.ToUpper());
        }
        #endregion

        #region Right Motor
        private readonly List<NeurologyFormLevel> _rightMotorValues = new List<NeurologyFormLevel>();
        public bool RightMotorHasOnlySoftValues = true;
        public bool HasRightCollins = false;
        public NeurologyFormLevel MostRostralRightMotor { get; protected set; }
        public NeurologyFormLevel MostCaudalRightMotor { get; protected set; }

        public bool RightMotorIsEmpty()
        {
            return !_rightMotorValues.Any();
        }

        public void AddRightMotorValue(NeurologyFormLevel level)
        {
            if (_rightMotorValues.Any(l => l.Name.ToUpper() == level.Name))
                return;

            if (MostRostralRightMotor == null || level.Ordinal < MostRostralRightMotor.Ordinal)
                MostRostralRightMotor = level;

            if (MostCaudalRightMotor == null || level.Ordinal > MostCaudalRightMotor.Ordinal)
                MostCaudalRightMotor = level;

            _rightMotorValues.Add(level);
        }

        public List<NeurologyFormLevel> GetRightMotorValues()
        {
            return new List<NeurologyFormLevel>(_rightMotorValues);
        }

        public bool RightMotorContains(string levelName)
        {
            return _rightMotorValues.Any(l => l.Name.ToUpper() == levelName.ToUpper());
        }
        #endregion

        #region Left Motor
        private readonly List<NeurologyFormLevel> _leftMotorValues = new List<NeurologyFormLevel>();
        public bool LeftMotorHasOnlySoftValues = true;
        public bool HasLeftCollins = false;

        public bool LeftMotorIsEmpty()
        {
            return !_leftMotorValues.Any();
        }

        public void AddLeftMotorValue(NeurologyFormLevel level)
        {
            if (_leftMotorValues.Any(l => l.Name.ToUpper() == level.Name))
                return;

            if (MostRostralLeftMotor == null || level.Ordinal < MostRostralLeftMotor.Ordinal)
                MostRostralLeftMotor = level;

            if (MostCaudalLeftMotor == null || level.Ordinal > MostCaudalLeftMotor.Ordinal)
                MostCaudalLeftMotor = level;

            _leftMotorValues.Add(level);
        }

        public List<NeurologyFormLevel> GetLeftMotorValues()
        {
            return new List<NeurologyFormLevel>(_leftMotorValues);
        }

        public bool LeftMotorContains(string levelName)
        {
            return _leftMotorValues.Any(l => l.Name.ToUpper() == levelName.ToUpper());
        }
        #endregion

        #region Neurological Level of Injury
        private readonly List<NeurologyFormLevel> _neurologicalLevelsOfInjury = new List<NeurologyFormLevel>();
        public bool NeurologicalLevelOfInjuryHasOnlySoftValues = true;

        public void AddNeurologicalLevelOfInjury(NeurologyFormLevel level)
        {
            if (_neurologicalLevelsOfInjury.Any(l => l.Name.ToLower() == level.Name.ToLower()))
                return;

            if (MostRostralNeurologicalLevelOfInjury == null || level.Ordinal < MostRostralNeurologicalLevelOfInjury.Ordinal)
                MostRostralNeurologicalLevelOfInjury = level;

            if (MostCaudalNeurologicalLevelOfInjury == null || level.Ordinal > MostCaudalNeurologicalLevelOfInjury.Ordinal)
                MostCaudalNeurologicalLevelOfInjury = level;

            _neurologicalLevelsOfInjury.Add(level);
        }

        public List<NeurologyFormLevel> GetNeurologicalLevelsOfInjury()
        {
            return new List<NeurologyFormLevel>(_neurologicalLevelsOfInjury);
        }
        #endregion
        
        #region Right Sensory ZPP
        private readonly List<NeurologyFormLevel> _rightSensoryZppValues = new List<NeurologyFormLevel>();
        public bool RightSensoryZppHasOnlySoftValues = true;

        public bool RightSensoryZppIsEmpty()
        {
            return !_rightSensoryZppValues.Any();
        }

        public void AddRightSensoryZppValue(NeurologyFormLevel level)
        {
            if (_rightSensoryZppValues.Any(l => l.Name.ToUpper() == level.Name)
                || "S4_5".Equals(level.Name))
                return;

            _rightSensoryZppValues.Add(level);
        }

        public List<NeurologyFormLevel> GetRightSensoryZppValues()
        {
            return new List<NeurologyFormLevel>(_rightSensoryZppValues);
        }
        #endregion

        #region Left Sensory ZPP
        private readonly List<NeurologyFormLevel> _leftSensoryZppValues = new List<NeurologyFormLevel>();
        public bool LeftSensoryZppHasOnlySoftValues = true;
        public bool LeftSensoryZppIsEmpty()
        {
            return !_leftSensoryZppValues.Any();
        }

        public void AddLeftSensoryZppValue(NeurologyFormLevel level)
        {
            if (_leftSensoryZppValues.Any(l => l.Name.ToUpper() == level.Name)
                || "S4_5".Equals(level.Name))
                return;

            _leftSensoryZppValues.Add(level);
        }

        public List<NeurologyFormLevel> GetLeftSensoryZppValues()
        {
            return new List<NeurologyFormLevel>(_leftSensoryZppValues);
        }
        #endregion

        #region Right Motor ZPP
        private readonly List<NeurologyFormLevel> _rightMotorZppValues = new List<NeurologyFormLevel>();
        public bool RightMotorZppHasOnlySoftValues = true;

        public bool RightMotorZppIsEmpty()
        {
            return !_rightMotorZppValues.Any();
        }

        public void AddRightMotorZppValue(NeurologyFormLevel level)
        {
            if (_rightMotorZppValues.Any(l => l.Name.ToUpper() == level.Name)
                || "S4_5".Equals(level.Name))
                return;

            _rightMotorZppValues.Add(level);
        }

        public List<NeurologyFormLevel> GetRightMotorZppValues()
        {
            return new List<NeurologyFormLevel>(_rightMotorZppValues);
        }
        #endregion

        #region Left Motor ZPP
        private readonly List<NeurologyFormLevel> _leftMotorZppValues = new List<NeurologyFormLevel>();
        public bool LeftMotorZppHasOnlySoftValues = true;

        public bool LeftMotorZppIsEmpty()
        {
            return !_leftMotorZppValues.Any();
        }

        public void AddLeftMotorZppValue(NeurologyFormLevel level)
        {
            if (_leftMotorZppValues.Any(l => l.Name.ToUpper() == level.Name)
                || "S4_5".Equals(level.Name))
                return;

            _leftMotorZppValues.Add(level);
        }

        public List<NeurologyFormLevel> GetLeftMotorZppValues()
        {
            return new List<NeurologyFormLevel>(_leftMotorZppValues);
        }
        #endregion

        #region Asia Impairment Scale
        //public string Complete;
        //public bool IsSensoryIncomplete;
        //public bool IsMotorIncomplete;
        private readonly List<string> _asiaImpairmentScaleValues = new List<string>();

        public void AddAsiaImpairmentScaleValue(string value)
        {
            if (string.IsNullOrEmpty(value) || _asiaImpairmentScaleValues.Contains(value.ToUpper()))
                return;

            _asiaImpairmentScaleValues.Add(value.ToUpper());
        }

        public string GetAsiaImpairmentScaleValues()
        {
            return string.Join(",", _asiaImpairmentScaleValues.OrderBy(v => v));
        }
        #endregion


        public NeurologyFormLevel MostRostralLeftMotor { get; protected set; }
        public NeurologyFormLevel MostCaudalLeftMotor { get; protected set; }

        public NeurologyFormLevel MostRostralNeurologicalLevelOfInjury { get; protected set; }
        public NeurologyFormLevel MostCaudalNeurologicalLevelOfInjury { get; protected set; }

        public NeurologyFormLevel MostRostralRightLevelWithMotorFunction { get; set; }
        public NeurologyFormLevel MostCaudalRightLevelWithMotorFunction { get; set; }
        public NeurologyFormLevel MostRostralLeftLevelWithMotorFunction { get; set; }
        public NeurologyFormLevel MostCaudalLeftLevelWithMotorFunction { get; set; }
    }
}