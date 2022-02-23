using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rhi.Isncsci.Core
{
	public class NeurologyForm
    {
        private static readonly string[] LevelNames = new[]
                                                          {
                                                              "C2", "C3", "C4", "C5", "C6", "C7", "C8", "T1", "T2", "T3"
                                                              , "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12",
                                                              "L1", "L2", "L3", "L4", "L5", "S1", "S2", "S3", "S4_5"
                                                          };

        private readonly static string[] KeyMuscles = new[] { "C5", "C6", "C7", "C8", "T1", "L2", "L3", "L4", "L5", "S1" };
        private readonly Dictionary<string, NeurologyFormLevel> _levels;
        private static readonly Regex NtRegex = new Regex(@"\bNT\b", RegexOptions.IgnoreCase);
        private static readonly Regex NonSciImpairmentRegex = new Regex(".+[!]");
        private static readonly Regex NonSciImpairmentFlagsRegex = new Regex("[\\*!]");

        private const int NormalMotorValue = 5;
        private const int NormalSensoryValue = 2;

        public NeurologyFormLevel C1 { get; protected set; }

        public BinaryObservation AnalContraction { get; set; }
        public BinaryObservation AnalSensation { get; set; }
        public NeurologyFormLevel RightLowestNonKeyMuscleWithMotorFunction { get; protected set; }
        public NeurologyFormLevel LeftLowestNonKeyMuscleWithMotorFunction { get; protected set; }

        public NeurologyForm()
        {
            C1 = new NeurologyFormLevel
            {
                Ordinal = 0,
                IsKeyMuscle = false,
                IsLowerMuscle = false,
                Name = "C1",
                Previous = null,
                RightMotorName = "5",
                RightMotorValue = 5,
                LeftMotorName = "5",
                LeftMotorValue = 5,
                RightTouchName = "2",
                RightTouchValue = 2,
                LeftTouchName = "2",
                LeftTouchValue = 2,
                RightPrickName = "2",
                RightPrickValue = 2,
                LeftPrickName = "2",
                LeftPrickValue = 2
            };

            _levels = new Dictionary<string, NeurologyFormLevel>();

            var previousLevel = C1;

            for (var i = 0; i < LevelNames.Length; i++)
            {
                var name = LevelNames[i];

                var currentLevel = new NeurologyFormLevel
                {
                    IsKeyMuscle = KeyMuscles.Contains(name),
                    IsLowerMuscle = (i >= 20 && i <= 24),
                    Name = name,
                    Ordinal = i + 1,
                    Previous = previousLevel
                };

                previousLevel.Next = currentLevel;
                previousLevel = currentLevel;
                _levels.Add(name, currentLevel);
            }
        }

        public void SetRightLowestNonKeyMuscleWithMotorFunction(string levelName)
        {
            if (string.IsNullOrEmpty(levelName))
                return;

            var key = levelName.ToUpper();

            if (!_levels.ContainsKey(key))
                return;

            if (RightLowestNonKeyMuscleWithMotorFunction != null)
                RightLowestNonKeyMuscleWithMotorFunction.HasOtherRightMotorFunction = false;

            RightLowestNonKeyMuscleWithMotorFunction = _levels[key];
            RightLowestNonKeyMuscleWithMotorFunction.HasOtherRightMotorFunction = true;
        }

        public void SetLeftLowestNonKeyMuscleWithMotorFunction(string levelName)
        {
            if (string.IsNullOrEmpty(levelName))
                return;

            var key = levelName.ToUpper();

            if (!_levels.ContainsKey(key))
                return;

            if (LeftLowestNonKeyMuscleWithMotorFunction != null)
                LeftLowestNonKeyMuscleWithMotorFunction.HasOtherLeftMotorFunction = false;

            LeftLowestNonKeyMuscleWithMotorFunction = _levels[key];
            LeftLowestNonKeyMuscleWithMotorFunction.HasOtherLeftMotorFunction = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Neurology level at the specified index.</returns>
        public NeurologyFormLevel GetLevelAt(int index)
        {
            return index >= 0 && index < _levels.Count ? _levels.ElementAt(index).Value : null;
        }

        /// <summary>
        /// Returns the neurology level based on a level name.  E.g. C2,C3,C4...S1,S2,S3,S4_5
        /// </summary>
        /// <param name="levelName">Lavel name being searched.</param>
        /// <returns>Neurology level that matches the specified level name.</returns>
        public NeurologyFormLevel GetLevelWithName(string levelName)
        {
            var key = levelName.ToUpper();

            return _levels.ContainsKey(key) ? _levels[key] : null;
        }

        /// <summary>
        /// Updates the values of the neurology level with the specified name.
        /// You can pass strings containing values between 0-2 for touch and prick and 0-5 for motor.
        /// You can also use the exclamation mark and asterisk at the end of the string to indicate impairment not due to a spinal cord injury.
        /// Finally, you can also pass NT to indicate that a value was not testable.
        /// </summary>
        /// <param name="levelName">Name of the respective neurology level.</param>
        /// <param name="rightTouchName">Right touch</param>
        /// <param name="leftTouchName">Left touch</param>
        /// <param name="rightPrickName">Right Prick</param>
        /// <param name="leftPrickName">Left prick</param>
        /// <param name="rightMotorName">Right motor</param>
        /// <param name="leftMotorName">Left motor</param>
        /// <returns></returns>
        public NeurologyForm UpdateLevelAt(string levelName, string rightTouchName, string leftTouchName,
            string rightPrickName, string leftPrickName, string rightMotorName, string leftMotorName)
        {
            var level = GetLevelWithName(levelName);

            if (level != null)
                UpdateLevel(level, rightTouchName, leftTouchName, rightPrickName, leftPrickName, rightMotorName, leftMotorName);

            return this;
        }

        private static int GetDermatomeValue(string text, int normalValue)
        {
            if (text.ToUpper() == "NT*")
                return normalValue;

            int value;
            int.TryParse(NonSciImpairmentFlagsRegex.Replace(text, string.Empty), out value);

            return value;
        }

        private static void UpdateLevel(NeurologyFormLevel level, string rightTouchName, string leftTouchName,
            string rightPrickName, string leftPrickName, string rightMotorName, string leftMotorName)
        {
            // Right Touch
            level.RightTouchName = rightTouchName;
            level.RightTouchValue = GetDermatomeValue(rightTouchName, NormalSensoryValue);
            level.RightTouchImpairmentNotDueToSci = NonSciImpairmentRegex.IsMatch(rightTouchName);

            // Left Touch
            level.LeftTouchName = leftTouchName;
            level.LeftTouchValue = GetDermatomeValue(leftTouchName, NormalSensoryValue);
            level.LeftTouchImpairmentNotDueToSci = NonSciImpairmentRegex.IsMatch(leftTouchName);

            // Right Prick
            level.RightPrickName = rightPrickName;
            level.RightPrickValue = GetDermatomeValue(rightPrickName, NormalSensoryValue);
            level.RightPrickImpairmentNotDueToSci = NonSciImpairmentRegex.IsMatch(rightPrickName);

            // Left Prick
            level.LeftPrickName = leftPrickName;
            level.LeftPrickValue = GetDermatomeValue(leftPrickName, NormalSensoryValue);
            level.LeftPrickImpairmentNotDueToSci = NonSciImpairmentRegex.IsMatch(leftPrickName);

            // Right Motor
            level.RightMotorName = rightMotorName;
            level.RightMotorValue = GetDermatomeValue(rightMotorName, NormalMotorValue);
            level.RightMotorImpairmentNotDueToSci = NonSciImpairmentRegex.IsMatch(rightMotorName);

            // Left Motor
            level.LeftMotorName = leftMotorName;
            level.LeftMotorValue = GetDermatomeValue(leftMotorName, NormalMotorValue);
            level.LeftMotorImpairmentNotDueToSci = NonSciImpairmentRegex.IsMatch(leftMotorName);

            if (!level.IsKeyMuscle)
            {
                if ((level.RightTouchValue == 2 || level.RightTouchImpairmentNotDueToSci)
                    && (level.RightPrickValue == 2 || level.RightPrickImpairmentNotDueToSci))
                {
                    level.RightMotorName = "5";
                    level.RightMotorValue = 5;
                }
                else
                {
                    level.RightMotorName = (NtRegex.IsMatch(level.RightTouchName) || level.RightTouchValue == 2)
                        && (NtRegex.IsMatch(level.RightPrickName) || level.RightPrickValue == 2) ? "NT" : "0";
                    level.RightMotorValue = 0;
                }

                if ((level.LeftTouchValue == 2 || level.LeftTouchImpairmentNotDueToSci)
                    && (level.LeftPrickValue == 2 || level.LeftPrickImpairmentNotDueToSci))
                {
                    level.LeftMotorName = "5";
                    level.LeftMotorValue = 5;
                }
                else
                {
                    level.LeftMotorName = (NtRegex.IsMatch(level.LeftTouchName) || level.LeftTouchValue == 2)
                        && (NtRegex.IsMatch(level.LeftPrickName) || level.LeftPrickValue == 2) ? "NT" : "0";
                    level.LeftMotorValue = 0;
                }
            }
        }
    }
}
