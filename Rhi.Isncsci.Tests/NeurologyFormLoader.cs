/* Copyright 2014 Rick Hansen Institute
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Rhi.Isncsci;

namespace Rhi.Isncsci.Tests
{
    public class NeurologyFormLoader
    {
        private static readonly string[] LevelNames = new[]
                                                          {
                                                              "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "T1", "T2", "T3"
                                                              , "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12",
                                                              "L1", "L2", "L3", "L4", "L5", "S1", "S2", "S3", "S4_5"
                                                          };

        private readonly static string[] KeyMuscles = new[] { "C5", "C6", "C7", "C8", "T1", "L2", "L3", "L4", "L5", "S1" };

        private const int NormalMotorValue = 5;
        private const int NormalSensoryValue = 2;
        private static readonly Regex NonSciImpairmentRegex = new Regex(".+[!]");
        private static readonly Regex NonSciImpairmentFlagsRegex = new Regex("[\\*!]");
        private static readonly Regex NtRegex = new Regex("^nt$", RegexOptions.IgnoreCase);
        private static readonly Regex ImpairmentNotDueToSciRegex = new Regex(@"\!$");

        public static NeurologyForm LoadNeurologyFormFrom(XDocument xmlDocument)
        {
            var formXml = xmlDocument.Root.Element("NeurologyForm");

            var neurologyForm = new NeurologyForm()
            {
                AnalContraction = GetBinaryObservationFrom(formXml.Element("AnalContraction")),
                AnalSensation = GetBinaryObservationFrom(formXml.Element("AnalSensation"))
            };

            neurologyForm.SetRightLowestNonKeyMuscleWithMotorFunction(formXml.Element("RightLowestNonKeyMuscleWithMotorFunction").Value);
            neurologyForm.SetLeftLowestNonKeyMuscleWithMotorFunction(formXml.Element("LeftLowestNonKeyMuscleWithMotorFunction").Value);

            foreach (var dermatomeElement in formXml.Descendants("Dermatome"))
                AddValuesFromDermatomeToForm(dermatomeElement, neurologyForm);

            return neurologyForm;
        }


        public static NeurologyFormTotals LoadNeurologyFormTotalsFrom(XDocument xmlDocument)
        {
            var totalsXml = xmlDocument.Root.Element("NeurologyFormTotals");

            var neurologyFormTotals = new NeurologyFormTotals
            {
                LeftPrickContainsNt = bool.Parse(totalsXml.Element("LeftPrickContainsNt").Value),
                LeftPrickTotal = GetIntValueFrom(totalsXml.Element("LeftPrickTotal").Value),
                LeftPrickTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("LeftPrickTotal").Value),
                LeftTouchContainsNt = bool.Parse(totalsXml.Element("LeftTouchContainsNt").Value),
                LeftTouchTotal = GetIntValueFrom(totalsXml.Element("LeftTouchTotal").Value),
                LeftTouchTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("LeftTouchTotal").Value),
                LeftLowerMotorContainsNt = bool.Parse(totalsXml.Element("LeftLowerMotorContainsNt").Value),
                LeftLowerMotorTotal = GetIntValueFrom(totalsXml.Element("LeftLowerMotorTotal").Value),
                LeftLowerMotorTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("LeftLowerMotorTotal").Value),
                LeftUpperMotorContainsNt = bool.Parse(totalsXml.Element("LeftUpperMotorContainsNt").Value),
                LeftUpperMotorTotal = GetIntValueFrom(totalsXml.Element("LeftUpperMotorTotal").Value),
                LeftUpperMotorTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("LeftUpperMotorTotal").Value),
                LowerMotorTotal = GetIntValueFrom(totalsXml.Element("LowerMotorTotal").Value),
                PrickTotal = GetIntValueFrom(totalsXml.Element("PrickTotal").Value),
                RightLowerMotorContainsNt = bool.Parse(totalsXml.Element("RightLowerMotorContainsNt").Value),
                RightLowerMotorTotal = GetIntValueFrom(totalsXml.Element("RightLowerMotorTotal").Value),
                RightLowerMotorTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("RightLowerMotorTotal").Value),
                RightUpperMotorContainsNt = bool.Parse(totalsXml.Element("RightUpperMotorContainsNt").Value),
                RightUpperMotorTotal = GetIntValueFrom(totalsXml.Element("RightUpperMotorTotal").Value),
                RightUpperMotorTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("RightUpperMotorTotal").Value),
                RightPrickContainsNt = bool.Parse(totalsXml.Element("RightPrickContainsNt").Value),
                RightPrickTotal = GetIntValueFrom(totalsXml.Element("RightPrickTotal").Value),
                RightPrickTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("RightPrickTotal").Value),
                RightTouchContainsNt = bool.Parse(totalsXml.Element("RightTouchContainsNt").Value),
                RightTouchTotal = GetIntValueFrom(totalsXml.Element("RightTouchTotal").Value),
                RightTouchTotalHasImpairmentNotDueToSci = DoesTotalHaveImpairmentNotDueToSci(totalsXml.Element("RightTouchTotal").Value),
                TouchTotal = GetIntValueFrom(totalsXml.Element("TouchTotal").Value),
                UpperMotorTotal = GetIntValueFrom(totalsXml.Element("UpperMotorTotal").Value),
            };

            var levelsDictionary = GetLevelsDictionary();

            foreach (var value in totalsXml.Element("RightSensory").Value.Split(',').Select(v => v.Trim()))
                neurologyFormTotals.AddRightSensoryValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("LeftSensory").Value.Split(',').Select(v => v.Trim()))
                neurologyFormTotals.AddLeftSensoryValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("RightMotor").Value.Split(',').Select(v => v.Trim()))
                neurologyFormTotals.AddRightMotorValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("LeftMotor").Value.Split(',').Select(v => v.Trim()))
                neurologyFormTotals.AddLeftMotorValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("NeurologicalLevelOfInjury").Value.Split(',').Select(v => v.Trim()))
                neurologyFormTotals.AddNeurologicalLevelOfInjury(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("RightSensoryZpp").Value.Split(',').Select(v => v.Trim()))
                if (!string.IsNullOrEmpty(value))
                    neurologyFormTotals.AddRightSensoryZppValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("LeftSensoryZpp").Value.Split(',').Select(v => v.Trim()))
                if (!string.IsNullOrEmpty(value))
                    neurologyFormTotals.AddLeftSensoryZppValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("RightMotorZpp").Value.Split(',').Select(v => v.Trim()))
                if (!string.IsNullOrEmpty(value))
                    neurologyFormTotals.AddRightMotorZppValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("LeftMotorZpp").Value.Split(',').Select(v => v.Trim()))
                if (!string.IsNullOrEmpty(value))
                    neurologyFormTotals.AddLeftMotorZppValue(levelsDictionary[value.ToUpper()]);

            foreach (var value in totalsXml.Element("AsiaImpairmentScale").Value.Split(',').Select(v => v.Trim()))
                neurologyFormTotals.AddAsiaImpairmentScaleValue(value.ToUpper());

            return neurologyFormTotals;
        }

        private static BinaryObservation GetBinaryObservationFrom(XElement element)
        {
            return (BinaryObservation)Enum.Parse(typeof(BinaryObservation), element.Value);
        }

        private static int GetIntValueFrom(string value)
        {
            int returnValue;
            int.TryParse(ImpairmentNotDueToSciRegex.Replace(value, string.Empty), out returnValue);

            return returnValue;
        }

        private static bool DoesTotalHaveImpairmentNotDueToSci(string value)
        {
            return ImpairmentNotDueToSciRegex.IsMatch(value);
        }

        private static void AddValuesFromDermatomeToForm(XElement dermatomeElement, NeurologyForm neurologyForm)
        {
            // Right Touch
            var rightTouchName = dermatomeElement.Element("RightTouch").Value;
            var rightPrickName = dermatomeElement.Element("RightPrick").Value;
            var leftTouchName = dermatomeElement.Element("LeftTouch").Value;
            var leftPrickName = dermatomeElement.Element("LeftPrick").Value;

            var rightMotorElement = dermatomeElement.Element("RightMotor");
            var rightMotorName = rightMotorElement == null ? "0" : rightMotorElement.Value;

            var leftMotorElement = dermatomeElement.Element("LeftMotor");
            var leftMotorName = leftMotorElement == null ? "0" : leftMotorElement.Value;

            neurologyForm.UpdateLevelAt(dermatomeElement.Attribute("name").Value,
                    rightTouchName, leftTouchName,
                    rightPrickName, leftPrickName,
                    rightMotorName, leftMotorName);
        }

        private static Dictionary<string, NeurologyFormLevel> GetLevelsDictionary()
        {
            var C1 = new NeurologyFormLevel
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

            var levels = new Dictionary<string, NeurologyFormLevel>();

            var previousLevel = C1;

            for (var i = 0; i < LevelNames.Length; i++)
            {
                var name = LevelNames[i];

                var currentLevel = i == 0 ? C1
                    : new NeurologyFormLevel
                    {
                        IsKeyMuscle = KeyMuscles.Contains(name),
                        IsLowerMuscle = (i >= 20 && i <= 24),
                        Name = name,
                        Ordinal = i + 1,
                        Previous = previousLevel
                    };

                previousLevel.Next = currentLevel;
                previousLevel = currentLevel;
                levels.Add(name, currentLevel);
            }

            return levels;
        }
    }
}