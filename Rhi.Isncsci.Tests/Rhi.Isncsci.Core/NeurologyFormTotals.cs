using System.Collections.Generic;
using System.Linq;

namespace Rhi.Isncsci.Core
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
