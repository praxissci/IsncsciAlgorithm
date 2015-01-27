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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Rhi.Isncsci
{
    /// <summary>
    /// Contains the methods which perform the ISNCSCI calculations.
    /// </summary>
    public sealed class Algorithm
    {
        private static readonly Regex NtRegex = new Regex(@"\bNT\b", RegexOptions.IgnoreCase);
        private const string NotDeterminable = "UTD";
        private const string NotApplicable = "NA";
        private const string Intact = "INT";

        /// <summary>
        /// Produces a summarized version of a NeurologyFormTotals object which can be used to be displayed in an interface or be stored in a database.
        /// The values in the summary return will have ranges instead of lists of values.
        /// </summary>
        /// <param name="neurologyForm">Neurology form that has been populated with the values to be used in the algorithm calculations.</param>
        /// <returns>
        /// Summarized version of the totals which presents the results as ranges, rather than lists containing every possible value for every field.
        /// </returns>
        public static NeurologyFormTotalsSummary GetTotalsSummaryFor(NeurologyForm neurologyForm)
        {
            return GetTotalsSummaryFor(GetTotalsFor(neurologyForm));
        }

        /// <summary>
        /// Produces a summarized version of a NeurologyFormTotals object which can be used to be displayed in an interface or be stored in a database.
        /// The values in the summary return will have ranges instead of lists of values.
        /// </summary>
        /// <param name="totals">The form totals object to be used to generate the summary.</param>
        /// <returns>
        /// Summarized version of the totals where the enumerations get replaced by ranges.
        /// </returns>
        public static NeurologyFormTotalsSummary GetTotalsSummaryFor(NeurologyFormTotals totals)
        {
            var ais = totals.GetAsiaImpairmentScaleValues();
            var isAsiaA = ais.Contains("A");
            var couldBeOtherThanAsiaA = !isAsiaA || ais.Length > 1;

            var summary = new NeurologyFormTotalsSummary
                {
                    AsiaImpairmentScale = ais,
                    Completeness = isAsiaA ? (couldBeOtherThanAsiaA ? "C,I" : "C") : "I",
                    LeftLowerMotorTotal = GetSummaryStringFor(totals.LeftLowerMotorTotal, totals.LeftLowerMotorTotalHasImpairmentNotDueToSci, totals.LeftLowerMotorContainsNt),
                    LeftMotor = GetLevelsRange(totals.GetLeftMotorValues(), false),
                    LeftMotorZpp = isAsiaA ? GetLevelsRange(totals.GetLeftMotorZppValues(), couldBeOtherThanAsiaA) : NotApplicable,
                    LeftMotorTotal = GetSummaryStringFor(totals.LeftUpperMotorTotal + totals.LeftLowerMotorTotal,
                                                            totals.LeftUpperMotorTotalHasImpairmentNotDueToSci || totals.LeftLowerMotorTotalHasImpairmentNotDueToSci,
                                                            totals.LeftUpperMotorContainsNt || totals.LeftLowerMotorContainsNt),
                    LeftPrickTotal = GetSummaryStringFor(totals.LeftPrickTotal, totals.LeftPrickTotalHasImpairmentNotDueToSci, totals.LeftPrickContainsNt),
                    LeftSensory = GetLevelsRange(totals.GetLeftSensoryValues(), false),
                    LeftSensoryZpp = isAsiaA ? GetLevelsRange(totals.GetLeftSensoryZppValues(), couldBeOtherThanAsiaA) : NotApplicable,
                    LeftTouchTotal = GetSummaryStringFor(totals.LeftTouchTotal, totals.LeftTouchTotalHasImpairmentNotDueToSci, totals.LeftTouchContainsNt),
                    LeftUpperMotorTotal = GetSummaryStringFor(totals.LeftUpperMotorTotal, totals.LeftUpperMotorTotalHasImpairmentNotDueToSci, totals.LeftUpperMotorContainsNt),
                    LowerMotorTotal = GetSummaryStringFor(totals.LowerMotorTotal,
                                                        totals.RightLowerMotorTotalHasImpairmentNotDueToSci || totals.LeftLowerMotorTotalHasImpairmentNotDueToSci,
                                                        totals.RightLowerMotorContainsNt || totals.LeftLowerMotorContainsNt),
                    NeurologicalLevelOfInjury = GetLevelsRange(totals.GetNeurologicalLevelsOfInjury(), false),
                    PrickTotal = GetSummaryStringFor(totals.RightPrickTotal + totals.LeftPrickTotal,
                                                        totals.RightPrickTotalHasImpairmentNotDueToSci || totals.LeftPrickTotalHasImpairmentNotDueToSci,
                                                        totals.RightPrickContainsNt || totals.LeftPrickContainsNt),
                    RightLowerMotorTotal = GetSummaryStringFor(totals.RightLowerMotorTotal, totals.RightLowerMotorTotalHasImpairmentNotDueToSci, totals.RightLowerMotorContainsNt),
                    RightMotor = GetLevelsRange(totals.GetRightMotorValues(), false),
                    RightMotorZpp = isAsiaA ? GetLevelsRange(totals.GetRightMotorZppValues(), couldBeOtherThanAsiaA) : NotApplicable,
                    RightMotorTotal = GetSummaryStringFor(totals.RightUpperMotorTotal + totals.RightLowerMotorTotal,
                                                            totals.RightUpperMotorTotalHasImpairmentNotDueToSci || totals.RightLowerMotorTotalHasImpairmentNotDueToSci,
                                                            totals.RightUpperMotorContainsNt || totals.RightLowerMotorContainsNt),
                    RightPrickTotal = GetSummaryStringFor(totals.RightPrickTotal, totals.RightPrickTotalHasImpairmentNotDueToSci, totals.RightPrickContainsNt),
                    RightSensory = GetLevelsRange(totals.GetRightSensoryValues(), false),
                    RightSensoryZpp = isAsiaA ? GetLevelsRange(totals.GetRightSensoryZppValues(), couldBeOtherThanAsiaA) : NotApplicable,
                    RightTouchTotal = GetSummaryStringFor(totals.RightTouchTotal, totals.RightTouchTotalHasImpairmentNotDueToSci, totals.RightTouchContainsNt),
                    RightUpperMotorTotal = GetSummaryStringFor(totals.RightUpperMotorTotal, totals.RightUpperMotorTotalHasImpairmentNotDueToSci, totals.RightUpperMotorContainsNt),
                    TouchTotal = GetSummaryStringFor(totals.RightTouchTotal + totals.LeftTouchTotal,
                                                        totals.RightTouchTotalHasImpairmentNotDueToSci || totals.LeftTouchTotalHasImpairmentNotDueToSci,
                                                        totals.RightTouchContainsNt || totals.LeftTouchContainsNt),
                    UpperMotorTotal = GetSummaryStringFor(totals.UpperMotorTotal,
                                                            totals.RightUpperMotorTotalHasImpairmentNotDueToSci || totals.LeftUpperMotorTotalHasImpairmentNotDueToSci,
                                                            totals.RightUpperMotorContainsNt || totals.LeftUpperMotorContainsNt)
                };

            return summary;
        }

        /// <summary>
        /// Returns the results produced by the ISNCSCI Algorithm ir a raw values format.
        /// </summary>
        /// <param name="neurologyForm">Neurology form that has been populated with the values to be used in the algorithm calculations.</param>
        /// <returns>
        /// Totals in raw values format.  The results contain lists with every prossible value for each field.
        /// You can use the resulting object to obtained a summarized version, which uses ranges, by passing the result to the method GetTotalsSummaryFor
        /// </returns>
        public static NeurologyFormTotals GetTotalsFor(NeurologyForm neurologyForm)
        {
            var totals = new NeurologyFormTotals();

            UpdateTotalsWithLevelAt(neurologyForm, totals, neurologyForm.GetLevelWithName("C2"), false, false);

            totals.UpperMotorTotal = totals.RightUpperMotorTotal + totals.LeftUpperMotorTotal;
            totals.LowerMotorTotal = totals.RightLowerMotorTotal + totals.LeftLowerMotorTotal;
            totals.TouchTotal = totals.RightTouchTotal + totals.LeftTouchTotal;
            totals.PrickTotal = totals.RightPrickTotal + totals.LeftPrickTotal;

            var s4_5 = neurologyForm.GetLevelWithName("S4_5");

            var c1 = neurologyForm.GetLevelWithName("C2").Previous;

            if (totals.RightSensoryZppHasOnlySoftValues)
                totals.AddRightSensoryZppValue(c1);

            if (totals.LeftSensoryZppHasOnlySoftValues)
                totals.AddLeftSensoryZppValue(c1);

            if (totals.RightMotorZppHasOnlySoftValues)
                totals.AddRightMotorZppValue(c1);

            if (totals.LeftMotorZppHasOnlySoftValues)
                totals.AddLeftMotorZppValue(c1);

            if (totals.MostRostralRightLevelWithMotorFunction == null)
                totals.MostRostralRightLevelWithMotorFunction = c1;

            if (totals.MostRostralLeftLevelWithMotorFunction == null)
                totals.MostRostralLeftLevelWithMotorFunction = c1;

            if (totals.MostCaudalRightLevelWithMotorFunction == null)
                totals.MostCaudalRightLevelWithMotorFunction = c1;

            if (totals.MostCaudalLeftLevelWithMotorFunction == null)
                totals.MostCaudalLeftLevelWithMotorFunction = c1;

            // [ASIA learning center 2012-11-14] Sensory incomplete: Sacral sparing of sensory function
            var isSensoryIncomplete = neurologyForm.AnalSensation == BinaryObservation.Yes || neurologyForm.AnalSensation == BinaryObservation.NT
                || !"0".Equals(s4_5.RightTouchName) || !"0".Equals(s4_5.LeftTouchName)
                || !"0".Equals(s4_5.RightPrickName) || !"0".Equals(s4_5.LeftPrickName);

            var couldNotHaveMotorFunctionMoreThan3LevelsBelowMotorLevel = CouldNotHaveMotorFunctionMoreThan3LevelsBelowMotorLevel(neurologyForm, totals);

            var couldNotBeMotorIncomplete = (neurologyForm.AnalContraction == BinaryObservation.No || neurologyForm.AnalContraction == BinaryObservation.NT)
                && isSensoryIncomplete && couldNotHaveMotorFunctionMoreThan3LevelsBelowMotorLevel;

            if ((neurologyForm.AnalContraction == BinaryObservation.No || neurologyForm.AnalContraction == BinaryObservation.NT)
                && (neurologyForm.AnalSensation == BinaryObservation.No || neurologyForm.AnalSensation == BinaryObservation.NT)
                && s4_5.RightTouchValue == 0 && !s4_5.RightTouchImpairmentNotDueToSci
                && s4_5.RightPrickValue == 0 && !s4_5.RightPrickImpairmentNotDueToSci
                && s4_5.LeftTouchValue == 0 && !s4_5.LeftTouchImpairmentNotDueToSci
                && s4_5.LeftPrickValue == 0 && !s4_5.LeftPrickImpairmentNotDueToSci)
                totals.AddAsiaImpairmentScaleValue("A");

            if (couldNotBeMotorIncomplete && totals.MostRostralNeurologicalLevelOfInjury.Name != "S4_5")
                totals.AddAsiaImpairmentScaleValue("B");


            //// Not an ASIA E only
            //// AND not an ASIA A only
            if (totals.MostRostralNeurologicalLevelOfInjury.Name != "S4_5"
                && (isSensoryIncomplete || neurologyForm.AnalContraction == BinaryObservation.Yes || neurologyForm.AnalContraction == BinaryObservation.NT))
            {
                bool couldBeAsiaC;
                bool couldBeAsiaD;
                CouldBeAsiaCorD(neurologyForm, totals, out couldBeAsiaC, out couldBeAsiaD);

                if (couldBeAsiaC)
                    totals.AddAsiaImpairmentScaleValue("C");

                if (couldBeAsiaD)
                    totals.AddAsiaImpairmentScaleValue("D");
            }

            if (totals.RightSensoryContains("S4_5") && totals.LeftSensoryContains("S4_5")
                && totals.RightMotorContains("S4_5") && totals.LeftMotorContains("S4_5"))
                totals.AddAsiaImpairmentScaleValue("E");

            return totals;
        }

        /// <summary>
        /// Formats a total value depending on the specified flags.
        /// </summary>
        /// <param name="total">Raw total value.</param>
        /// <param name="hasImpairmentNotDueToSci">Flag indicating if any value used in the calculation of this total presents impairment not due to a spinal cord injury.</param>
        /// <param name="containsNt">Flag indicating if any value used in the calculation of this total is set to Not Testable.</param>
        /// <returns>
        /// The value, followed by an exclamation mark if the hasImpairmentNotDueToSci is set to true
        /// or
        /// UTD (Unable to determine) if the containsNt flag is set to true.
        /// </returns>
        private static string GetSummaryStringFor(int total, bool hasImpairmentNotDueToSci, bool containsNt)
        {
            if (containsNt)
                return NotDeterminable;

            return string.Format("{0}{1}", total, hasImpairmentNotDueToSci ? "!" : string.Empty);
        }

        /// <summary>
        /// Returns a range string based on the specified list.
        /// </summary>
        /// <param name="levels">Neurological levels returned in a calculation.</param>
        /// <param name="addNa">Used in the Zone of Partial Preservation to indicate that one of the possible Asia Impairment Scale values is &quot;A&quot;</param>
        /// <returns>A representation of the list as a range string.  E.g. NA,C4-C6,T1,T2</returns>
        private static string GetLevelsRange(List<NeurologyFormLevel> levels, bool addNa)
        {
            if (levels.Count == 0)
                return addNa ? NotApplicable : string.Empty;

            levels.Sort(CompareNeurologyFormTotalsLevelByOrdinal);
            NeurologyFormLevel previous = null;
            var text = string.Empty;
            var isRange = false;

            foreach (var level in levels)
            {
                if (previous == null || previous.Ordinal < level.Ordinal)
                {
                    var name = level.Ordinal == 28 ? Intact : level.Name;

                    if (previous == null)
                    {
                        text = name;
                    }
                    else if (level.Ordinal == previous.Ordinal + 1)
                    {
                        isRange = true;
                    }
                    else
                    {
                        text += (isRange ? "-" + previous.Name + "," : ",") + name;
                        isRange = false;
                    }

                    previous = level;
                }
            }

            if (isRange)
                text += "-" + (previous.Ordinal == 28 ? Intact : previous.Name);

            return addNa ? string.IsNullOrEmpty(text) ? NotApplicable : string.Format("{0},{1}", NotApplicable, text) : text;
        }

        /// <summary>
        /// Compares two neurology levels based on property &quot;Ordinal&quot;.
        /// </summary>
        /// <param name="a">Neurology form level A</param>
        /// <param name="b">Neurology form level B</param>
        /// <returns>
        /// Returns
        /// &ndash; 1 if a is more caudal (the ordinal of a is greater than the one of b)
        /// &ndash; 0 if both ordinal values are the same
        /// &ndash; -1 if b is more caudal (the ordinal of b is greater than the one of a).
        /// </returns>
        private static int CompareNeurologyFormTotalsLevelByOrdinal(NeurologyFormLevel a,
                                                                    NeurologyFormLevel b)
        {
            // Both are null so they are equal
            // OR
            // a is null and b is not so b is greater
            if (a == null)
                return b == null ? 0 : -1;

            // b is null and a is not so a is greater
            if (b == null)
                return 1;

            return a.Ordinal > b.Ordinal ? 1 : a.Ordinal == b.Ordinal ? 0 : -1;
        }

        /// <summary>
        /// Recursive method which iterates through the values in a nuerology form while it updates the totals generating the results produced by the algorithm.
        /// </summary>
        /// <param name="neurologyForm">Form being evaluated.</param>
        /// <param name="totals">Brand new totals object where the results are to be stored.</param>
        /// <param name="level">Current neurology level being evaluated</param>
        /// <param name="nextNonKeyMuscleShouldBeRightMotor">Flag used to evaluate the Kathy Collins condition on the right motor results.</param>
        /// <param name="nextNonKeyMuscleShouldBeLeftMotor">Flag used to evaluate the Kathy Collins condition on the left motor results.</param>
        private static void UpdateTotalsWithLevelAt(NeurologyForm neurologyForm, NeurologyFormTotals totals, NeurologyFormLevel level, bool nextNonKeyMuscleShouldBeRightMotor, bool nextNonKeyMuscleShouldBeLeftMotor)
        {
            totals.RightTouchTotal += level.RightTouchValue;
            totals.LeftTouchTotal += level.LeftTouchValue;
            totals.RightPrickTotal += level.RightPrickValue;
            totals.LeftPrickTotal += level.LeftPrickValue;

            if (level.IsKeyMuscle)
            {
                if (level.IsLowerMuscle)
                {
                    if (level.RightMotorImpairmentNotDueToSci && !totals.RightLowerMotorTotalHasImpairmentNotDueToSci)
                        totals.RightLowerMotorTotalHasImpairmentNotDueToSci = true;

                    if (level.LeftMotorImpairmentNotDueToSci && !totals.LeftLowerMotorTotalHasImpairmentNotDueToSci)
                        totals.LeftLowerMotorTotalHasImpairmentNotDueToSci = true;

                    if (NtRegex.IsMatch(level.RightMotorName) && !level.RightMotorImpairmentNotDueToSci)
                        totals.RightLowerMotorContainsNt = true;

                    if (NtRegex.IsMatch(level.LeftMotorName) && !level.LeftMotorImpairmentNotDueToSci)
                        totals.LeftLowerMotorContainsNt = true;
                }
                else
                {
                    if (level.RightMotorImpairmentNotDueToSci && !totals.RightUpperMotorTotalHasImpairmentNotDueToSci)
                        totals.RightUpperMotorTotalHasImpairmentNotDueToSci = true;

                    if (level.LeftMotorImpairmentNotDueToSci && !totals.LeftUpperMotorTotalHasImpairmentNotDueToSci)
                        totals.LeftUpperMotorTotalHasImpairmentNotDueToSci = true;

                    if (NtRegex.IsMatch(level.RightMotorName) && !level.RightMotorImpairmentNotDueToSci)
                        totals.RightUpperMotorContainsNt = true;

                    if (NtRegex.IsMatch(level.LeftMotorName) && !level.LeftMotorImpairmentNotDueToSci)
                        totals.LeftUpperMotorContainsNt = true;
                }
            }
            else
            {
                if (nextNonKeyMuscleShouldBeRightMotor)
                {
                    nextNonKeyMuscleShouldBeRightMotor = false;
                    totals.AddRightMotorValue(level.Previous);

                    if (!totals.RightSensoryHasOnlySoftValues)
                        totals.RightMotorHasOnlySoftValues = false;
                }

                if (nextNonKeyMuscleShouldBeLeftMotor)
                {
                    nextNonKeyMuscleShouldBeLeftMotor = false;
                    totals.AddLeftMotorValue(level.Previous);

                    if (!totals.LeftSensoryHasOnlySoftValues)
                        totals.LeftMotorHasOnlySoftValues = false;
                }
            }

            if (level.RightTouchImpairmentNotDueToSci && !totals.RightTouchTotalHasImpairmentNotDueToSci)
                totals.RightTouchTotalHasImpairmentNotDueToSci = true;

            if (level.LeftTouchImpairmentNotDueToSci && !totals.LeftTouchTotalHasImpairmentNotDueToSci)
                totals.LeftTouchTotalHasImpairmentNotDueToSci = true;

            if (level.RightPrickImpairmentNotDueToSci && !totals.RightPrickTotalHasImpairmentNotDueToSci)
                totals.RightPrickTotalHasImpairmentNotDueToSci = true;

            if (level.LeftPrickImpairmentNotDueToSci && !totals.LeftPrickTotalHasImpairmentNotDueToSci)
                totals.LeftPrickTotalHasImpairmentNotDueToSci = true;

            // Check if a column contains any NT value so we can properly format the total presented to the user
            if (NtRegex.IsMatch(level.RightTouchName) && !level.RightTouchImpairmentNotDueToSci && !totals.RightTouchContainsNt)
                totals.RightTouchContainsNt = true;

            if (NtRegex.IsMatch(level.LeftTouchName) && !level.LeftTouchImpairmentNotDueToSci && !totals.LeftTouchContainsNt)
                totals.LeftTouchContainsNt = true;

            if (NtRegex.IsMatch(level.RightPrickName) && !level.RightPrickImpairmentNotDueToSci && !totals.RightPrickContainsNt)
                totals.RightPrickContainsNt = true;

            if (NtRegex.IsMatch(level.LeftPrickName) && !level.LeftPrickImpairmentNotDueToSci && !totals.LeftPrickContainsNt)
                totals.LeftPrickContainsNt = true;




            if (totals.RightSensoryHasOnlySoftValues &&
                ((level.RightTouchValue != 2 && !level.RightTouchImpairmentNotDueToSci) ||
                 (level.RightPrickValue != 2 && !level.RightPrickImpairmentNotDueToSci)))
            {
                totals.AddRightSensoryValue(level.Previous);

                if ("S4_5".Equals(level.Name)
                    && (level.RightTouchValue == 2 || NtRegex.IsMatch(level.RightTouchName))
                    && (level.RightPrickValue == 2 || NtRegex.IsMatch(level.RightPrickName)))
                {
                    totals.AddRightSensoryValue(level);

                    if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues)
                        totals.AddNeurologicalLevelOfInjury(level);
                }

                if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues)
                    totals.AddNeurologicalLevelOfInjury(level.Previous);

                if ((level.RightTouchValue != 2 && !NtRegex.IsMatch(level.RightTouchName))
                    || (level.RightPrickValue != 2 && !NtRegex.IsMatch(level.RightPrickName)))
                {
                    totals.RightSensoryHasOnlySoftValues = false;
                    totals.NeurologicalLevelOfInjuryHasOnlySoftValues = false;
                }

                if (level.IsKeyMuscle)
                {
                    nextNonKeyMuscleShouldBeRightMotor = true;
                    totals.HasRightCollins = true;
                }
            }

            if (totals.LeftSensoryHasOnlySoftValues &&
                ((level.LeftTouchValue != 2 && !level.LeftTouchImpairmentNotDueToSci) ||
                 (level.LeftPrickValue != 2 && !level.LeftPrickImpairmentNotDueToSci)))
            {
                totals.AddLeftSensoryValue(level.Previous);

                if ("S4_5".Equals(level.Name)
                    && (level.LeftTouchValue == 2 || NtRegex.IsMatch(level.LeftTouchName))
                    && (level.LeftPrickValue == 2 || NtRegex.IsMatch(level.LeftPrickName)))
                {
                    totals.AddLeftSensoryValue(level);

                    if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues)
                        totals.AddNeurologicalLevelOfInjury(level);
                }

                if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues)
                    totals.AddNeurologicalLevelOfInjury(level.Previous);

                if ((level.LeftTouchValue != 2 && !NtRegex.IsMatch(level.LeftTouchName))
                    || (level.LeftPrickValue != 2 && !NtRegex.IsMatch(level.LeftPrickName)))
                {
                    totals.LeftSensoryHasOnlySoftValues = false;
                    totals.NeurologicalLevelOfInjuryHasOnlySoftValues = false;
                }

                if (level.IsKeyMuscle)
                {
                    nextNonKeyMuscleShouldBeLeftMotor = true;
                    totals.HasLeftCollins = true;
                }
            }

            if (totals.RightMotorHasOnlySoftValues && level.RightMotorValue != 5 && !level.RightMotorImpairmentNotDueToSci)
            {
                if (level.IsKeyMuscle && (level.RightMotorValue >= 3 || NtRegex.IsMatch(level.RightMotorName)))
                {
                    totals.AddRightMotorValue(level);

                    // Check if left won't make the NLI have to be the previous level.
                    // Let the logic for left motor handle the SNL instead
                    if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues
                        && (level.LeftMotorValue > 2 || level.LeftMotorImpairmentNotDueToSci || NtRegex.IsMatch(level.LeftMotorName)))
                    {
                        totals.AddNeurologicalLevelOfInjury(level);

                        if (!NtRegex.IsMatch(level.RightMotorName))
                            totals.NeurologicalLevelOfInjuryHasOnlySoftValues = false;
                    }
                }

                if (level.RightMotorValue < 3 || NtRegex.IsMatch(level.RightMotorName))
                {
                    totals.AddRightMotorValue(level.Previous);

                    if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues)
                    {
                        totals.AddNeurologicalLevelOfInjury(level.Previous);

                        if (!NtRegex.IsMatch(level.RightMotorName))
                            totals.NeurologicalLevelOfInjuryHasOnlySoftValues = false;
                    }
                }

                if (!NtRegex.IsMatch(level.RightMotorName))
                    totals.RightMotorHasOnlySoftValues = false;
            }

            if (totals.LeftMotorHasOnlySoftValues && level.LeftMotorValue != 5 && !level.LeftMotorImpairmentNotDueToSci)
            {
                if (level.IsKeyMuscle && (level.LeftMotorValue >= 3 || NtRegex.IsMatch(level.LeftMotorName)))
                {
                    totals.AddLeftMotorValue(level);

                    if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues)
                        totals.AddNeurologicalLevelOfInjury(level);
                }

                if (level.LeftMotorValue < 3 || NtRegex.IsMatch(level.LeftMotorName))
                {
                    totals.AddLeftMotorValue(level.Previous);

                    if (totals.NeurologicalLevelOfInjuryHasOnlySoftValues)
                        totals.AddNeurologicalLevelOfInjury(level.Previous);
                }

                if (!NtRegex.IsMatch(level.LeftMotorName))
                {
                    totals.LeftMotorHasOnlySoftValues = false;
                    totals.NeurologicalLevelOfInjuryHasOnlySoftValues = false;
                }
            }

            /* -- RECURSIVE CALL --------------- */
            if (level.Next != null)
                UpdateTotalsWithLevelAt(neurologyForm, totals, level.Next,
                    nextNonKeyMuscleShouldBeRightMotor && totals.RightMotorHasOnlySoftValues,
                    nextNonKeyMuscleShouldBeLeftMotor && totals.LeftMotorHasOnlySoftValues);

            #region This happens when there are INTACT values -------------------------------------------
            if ("S4_5".Equals(level.Name))
            {
                if (totals.RightSensoryHasOnlySoftValues && totals.LeftSensoryHasOnlySoftValues
                && totals.RightMotorHasOnlySoftValues && totals.LeftMotorHasOnlySoftValues)
                    totals.AddNeurologicalLevelOfInjury(level);

                if (totals.RightSensoryHasOnlySoftValues)
                {
                    totals.AddRightSensoryValue(level);
                    totals.RightSensoryHasOnlySoftValues = false;
                }

                if (totals.LeftSensoryHasOnlySoftValues)
                {
                    totals.AddLeftSensoryValue(level);
                    totals.LeftSensoryHasOnlySoftValues = false;
                }

                if (totals.RightMotorHasOnlySoftValues)
                {
                    totals.AddRightMotorValue(level);
                    totals.RightMotorHasOnlySoftValues = false;
                }

                if (totals.LeftMotorHasOnlySoftValues)
                {
                    totals.AddLeftMotorValue(level);
                    totals.LeftMotorHasOnlySoftValues = false;
                }
            }
            #endregion

            if (totals.RightSensoryZppHasOnlySoftValues && (!"0".Equals(level.RightTouchName) || !"0".Equals(level.RightPrickName)))
            {
                if ((level.RightTouchValue > 0 || level.RightTouchImpairmentNotDueToSci)
                    || (level.RightPrickValue > 0 || level.RightPrickImpairmentNotDueToSci))
                    totals.RightSensoryZppHasOnlySoftValues = false;

                totals.AddRightSensoryZppValue(level);
            }

            if (totals.LeftSensoryZppHasOnlySoftValues && (!"0".Equals(level.LeftTouchName) || !"0".Equals(level.LeftPrickName)))
            {
                if ((level.LeftTouchValue > 0 || level.LeftTouchImpairmentNotDueToSci)
                    || (level.LeftPrickValue > 0 || level.LeftPrickImpairmentNotDueToSci))
                    totals.LeftSensoryZppHasOnlySoftValues = false;

                totals.AddLeftSensoryZppValue(level);
            }

            if (totals.RightMotorZppHasOnlySoftValues
                && (level.HasOtherRightMotorFunction
                    || (!"0".Equals(level.RightMotorName) && (level.IsKeyMuscle || totals.RightMotorContains(level.Name)))))
            {
                if ((level.RightMotorImpairmentNotDueToSci || level.HasOtherRightMotorFunction || !NtRegex.IsMatch(level.RightMotorName)) &&
                    (level.IsKeyMuscle
                    || level.Ordinal < 4
                    || (level.Ordinal > 25 && !totals.RightUpperMotorContainsNt && !totals.RightLowerMotorContainsNt && !totals.HasRightCollins)
                    || (level.Ordinal > 8 && level.Ordinal < 21 && !totals.RightUpperMotorContainsNt)))
                    totals.RightMotorZppHasOnlySoftValues = false;

                totals.AddRightMotorZppValue(level);
            }

            if (totals.LeftMotorZppHasOnlySoftValues
                && (level.HasOtherLeftMotorFunction
                    || (!"0".Equals(level.LeftMotorName) && (level.IsKeyMuscle || totals.LeftMotorContains(level.Name)))))
            {
                if ((level.LeftMotorImpairmentNotDueToSci || level.HasOtherLeftMotorFunction || !NtRegex.IsMatch(level.LeftMotorName)) &&
                    (level.IsKeyMuscle
                    || level.Ordinal < 4
                    || (level.Ordinal > 25 && !totals.LeftUpperMotorContainsNt && !totals.LeftLowerMotorContainsNt && !totals.HasLeftCollins)
                    || (level.Ordinal > 8 && level.Ordinal < 21 && !totals.LeftUpperMotorContainsNt)))
                    totals.LeftMotorZppHasOnlySoftValues = false;

                totals.AddLeftMotorZppValue(level);
            }


            // Update most Rostral levels with motor function
            if ((level.IsKeyMuscle || level.HasOtherRightMotorFunction)
                && totals.MostRostralRightLevelWithMotorFunction == null
                && (level.RightMotorImpairmentNotDueToSci || level.HasOtherRightMotorFunction || (level.RightMotorValue != 0 && level.IsKeyMuscle)))
                totals.MostRostralRightLevelWithMotorFunction = level;

            if ((level.IsKeyMuscle || level.HasOtherLeftMotorFunction)
                && totals.MostRostralLeftLevelWithMotorFunction == null
                && (level.LeftMotorImpairmentNotDueToSci || level.HasOtherLeftMotorFunction || (level.LeftMotorValue != 0 && level.IsKeyMuscle)))
                totals.MostRostralLeftLevelWithMotorFunction = level;

            // Update most Caudal levels with motor function
            if ((level.IsKeyMuscle || level.HasOtherRightMotorFunction)
                && totals.MostCaudalRightLevelWithMotorFunction == null
                && (!"0".Equals(level.RightMotorName) || level.HasOtherRightMotorFunction))
                totals.MostCaudalRightLevelWithMotorFunction = level;

            if ((level.IsKeyMuscle || level.HasOtherLeftMotorFunction)
                && totals.MostCaudalLeftLevelWithMotorFunction == null
                && (!"0".Equals(level.LeftMotorName) || level.HasOtherLeftMotorFunction))
                totals.MostCaudalLeftLevelWithMotorFunction = level;


            if (!level.IsKeyMuscle)
                return;

            if (level.IsLowerMuscle)
            {
                totals.RightLowerMotorTotal += level.RightMotorValue;
                totals.LeftLowerMotorTotal += level.LeftMotorValue;
            }
            else
            {
                totals.RightUpperMotorTotal += level.RightMotorValue;
                totals.LeftUpperMotorTotal += level.LeftMotorValue;
            }
        }

        /// <summary>
        /// Evaluates the specified form and totals to determine if any of the different
        /// return conditions could produce a case where there could be motor function more
        /// than 3 levels below the injury level.
        /// </summary>
        /// <param name="neurologyForm">Form that was used to produce the totals.</param>
        /// <param name="totals">Totals retunred by the algorithm.</param>
        /// <returns>Flag indicating if any combination in the totals could have a case with motor function more than 3 levels below the injury level.</returns>
        private static bool CouldNotHaveMotorFunctionMoreThan3LevelsBelowMotorLevel(NeurologyForm neurologyForm, NeurologyFormTotals totals)
        {
            NeurologyFormLevel mostRostralRightLevelWithMotor = null;
            NeurologyFormLevel mostRostralLeftLevelWithMotor = null;

            var currentLevel = neurologyForm.GetLevelWithName("S1");

            while ((mostRostralRightLevelWithMotor == null || mostRostralLeftLevelWithMotor == null)
                && currentLevel != null // Could happen if SNL is C1
                && currentLevel.Ordinal >= totals.MostCaudalNeurologicalLevelOfInjury.Ordinal)
            {
                if (mostRostralRightLevelWithMotor == null && 
                    (currentLevel.RightMotorImpairmentNotDueToSci || currentLevel.HasOtherRightMotorFunction || (currentLevel.RightMotorValue != 0 && currentLevel.IsKeyMuscle)))
                    mostRostralRightLevelWithMotor = currentLevel;

                if (mostRostralLeftLevelWithMotor == null &&
                    (currentLevel.LeftMotorImpairmentNotDueToSci || currentLevel.HasOtherLeftMotorFunction || (currentLevel.LeftMotorValue != 0 && currentLevel.IsKeyMuscle)))
                    mostRostralLeftLevelWithMotor = currentLevel;

                currentLevel = currentLevel.Previous;
            }

            return (mostRostralRightLevelWithMotor == null || mostRostralRightLevelWithMotor.Ordinal - totals.MostCaudalRightMotor.Ordinal <= 3)
                && (mostRostralLeftLevelWithMotor == null || mostRostralLeftLevelWithMotor.Ordinal - totals.MostCaudalLeftMotor.Ordinal <= 3);
        }

        /// <summary>
        /// Evaluates the specified form and totals to determine if any of the different
        /// return conditions could produce a case where the Asia Impairment Scale is C o D.
        /// </summary>
        /// <param name="neurologyForm">Form that was used to produce the totals.</param>
        /// <param name="totals">Totals retunred by the algorithm.</param>
        /// <param name="couldBeAsiaC">out variable use as flag indicating if this case is a possible ASIA C.</param>
        /// <param name="couldBeAsiaD">out variable use as flag indicating if this case is a possible ASIA D.</param>
        private static void CouldBeAsiaCorD(NeurologyForm neurologyForm, NeurologyFormTotals totals, out bool couldBeAsiaC, out bool couldBeAsiaD)
        {
            couldBeAsiaC = false;
            couldBeAsiaD = false;
            NeurologyFormLevel rightMotor = null;
            NeurologyFormLevel leftMotor = null;

            // Check if the patient could be motor incoplete via sphincter contraction
            // Otherwise we need to check for motor function more than 3 levels below motor level.
            var couldHaveAnalContraction = neurologyForm.AnalContraction == BinaryObservation.Yes || neurologyForm.AnalContraction == BinaryObservation.NT;
            //var couldNotHaveAnalContraction = neurologyForm.AnalContraction == BinaryObservation.No || neurologyForm.AnalContraction == BinaryObservation.NT;

            var nliEnum = totals.GetNeurologicalLevelsOfInjury().GetEnumerator();

            while (nliEnum.MoveNext() && (!couldBeAsiaC || !couldBeAsiaD))
            {
                //var otherMotorIsMoreThanThreeLevelsBelowNli = (neurologyForm.)

                // RIGHT MOTOR
                // If not motor incomplete already, find the right motor level that correspond to this particular neurological level
                if (!couldHaveAnalContraction && (rightMotor == null || rightMotor.Ordinal < nliEnum.Current.Ordinal))
                {
                    var rightMotorValues = totals.GetRightMotorValues().GetEnumerator();
                    rightMotor = null;

                    while (rightMotor == null && rightMotorValues.MoveNext())
                    {
                        if (rightMotorValues.Current.Ordinal >= nliEnum.Current.Ordinal)
                            rightMotor = rightMotorValues.Current;
                    }

                    rightMotorValues.Dispose();
                }

                // LEFT MOTOR
                // If not motor incomplete already, find the left motor level that correspond to this particular neurological level
                if (!couldHaveAnalContraction && (leftMotor == null || leftMotor.Ordinal < nliEnum.Current.Ordinal))
                {
                    var leftMotorValues = totals.GetLeftMotorValues().GetEnumerator();
                    leftMotor = null;

                    while (leftMotor == null && leftMotorValues.MoveNext())
                    {
                        if (leftMotorValues.Current.Ordinal >= nliEnum.Current.Ordinal)
                            leftMotor = leftMotorValues.Current;
                    }

                    leftMotorValues.Dispose();
                }

                //if (couldNotHaveAnalContraction
                //    && (rightMotor == null || totals.MostCaudalRightLevelWithMotorFunction.Ordinal - rightMotor.Ordinal <= 3)
                //    && (leftMotor == null || totals.MostCaudalLeftLevelWithMotorFunction.Ordinal - leftMotor.Ordinal <= 3))
                //    couldBeAsiaB = true;

                // If the test is not motor incomplete at this level, do not continue to count motor levels, move to the next nli available.
                if (!couldHaveAnalContraction
                    && totals.MostCaudalRightLevelWithMotorFunction.Ordinal - rightMotor.Ordinal <= 3
                    && totals.MostCaudalLeftLevelWithMotorFunction.Ordinal - leftMotor.Ordinal <= 3)
                    continue;

                // When motor incomplete and the nli is S1 or more caudal, it is automatically D since there are no myotomes to count from.
                // We can break the loop.
                if (nliEnum.Current.Ordinal > 24) // Greater than L5 (24)
                {
                    couldBeAsiaD = true;
                    break;
                }

                // If motor incomplete, count the motor levels with muscle grade greater than two
                var levelsWithMuscleGradeGreaterThanTwo = 0;
                var levelsWithMuscleGradeLessThanThree = 0;
                var eligibleLevelCount = 0;
                var currentLevel = nliEnum.Current.Next;

                while (currentLevel != null)
                {
                    if (currentLevel.IsKeyMuscle)
                    {
                        eligibleLevelCount += 2;

                        if (currentLevel.RightMotorValue > 2 || currentLevel.RightMotorImpairmentNotDueToSci || NtRegex.IsMatch(currentLevel.RightMotorName))
                            levelsWithMuscleGradeGreaterThanTwo++;

                        if ((currentLevel.RightMotorValue < 3 || NtRegex.IsMatch(currentLevel.RightMotorName)) && !currentLevel.RightMotorImpairmentNotDueToSci)
                            levelsWithMuscleGradeLessThanThree++;

                        if (currentLevel.LeftMotorValue > 2 || currentLevel.LeftMotorImpairmentNotDueToSci || NtRegex.IsMatch(currentLevel.LeftMotorName))
                            levelsWithMuscleGradeGreaterThanTwo++;

                        if ((currentLevel.LeftMotorValue < 3 || NtRegex.IsMatch(currentLevel.LeftMotorName)) && !currentLevel.LeftMotorImpairmentNotDueToSci)
                            levelsWithMuscleGradeLessThanThree++;
                    }

                    currentLevel = currentLevel.Next;
                }

                // If not more than half the myotomes contain values less to 3, this is an Asia C
                if (levelsWithMuscleGradeLessThanThree > eligibleLevelCount / 2)
                    couldBeAsiaC = true;

                // If at least half the myotomes below the current NLI containing values greater or equal to 3, hooray! it is ASIA D.
                if (levelsWithMuscleGradeGreaterThanTwo >= eligibleLevelCount / 2)
                    couldBeAsiaD = true;
            }

            nliEnum.Dispose();
        }
    }
}