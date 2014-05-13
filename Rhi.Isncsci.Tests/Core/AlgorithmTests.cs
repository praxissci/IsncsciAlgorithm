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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhi.Isncsci;

namespace Rhi.Isncsci.Tests
{
    [TestClass]
    public class AlgorithmTests
    {
        private const string PathToTests = @"C:\lalo\TeamFoundationService\IsncsciAlgorithm\master\Rhi.Isncsci.Tests\Resources\IsncsciTestCases\";
        private const string PathToResources = @"C:\lalo\TeamFoundationService\IsncsciAlgorithm\master\Rhi.Isncsci.Tests\Resources\";
        private const string NotDeterminable = "UTD";

        /// <summary>
        /// Loads all the xml files in our Test Cases folder and applies the calculations on them.  It will then compare the results to the expected values in the xml file.
        /// </summary>
        [TestMethod]
        public void RunAllTests()
        {
            var filePaths = new List<string>();
            filePaths.AddRange(Directory.GetFiles(PathToTests + @"Fixes\", "*.xml"));
            filePaths.AddRange(Directory.GetFiles(PathToTests + @"Instep\", "*.xml"));
            filePaths.AddRange(Directory.GetFiles(PathToTests + @"OriginalCases\", "*.xml"));
            filePaths.AddRange(Directory.GetFiles(PathToTests + @"RevisionsToTheLogic\", "*.xml"));
            filePaths.AddRange(Directory.GetFiles(PathToTests + @"NtCases\", "*.xml"));
            filePaths.AddRange(Directory.GetFiles(PathToTests + @"AsiaSpecialArticle\", "*.xml"));

            foreach (var filePath in filePaths)
            {
                System.Console.WriteLine(filePath);

                var xmlDocument = XDocument.Load(filePath);
                TestForm(NeurologyFormLoader.LoadNeurologyFormFrom(xmlDocument), NeurologyFormLoader.LoadNeurologyFormTotalsFrom(xmlDocument));
            }
        }
        [TestMethod]
        public void RunSingleTest()
        {
            const string filePath = PathToTests + @"NtCases/ISNCSCI NT Case 2.xml";
            System.Console.WriteLine(filePath);

            var xmlDocument = XDocument.Load(filePath);
            TestForm(NeurologyFormLoader.LoadNeurologyFormFrom(xmlDocument), NeurologyFormLoader.LoadNeurologyFormTotalsFrom(xmlDocument));
        }

        private static void TestForm(NeurologyForm neurologyForm, NeurologyFormTotals expected)
        {
            // Arrange - Act
            var totals = Algorithm.GetTotalsFor(neurologyForm);

            // Assert
            Assert.AreEqual(expected.RightTouchTotal, totals.RightTouchTotal, "Right touch total");
            Assert.AreEqual(expected.RightTouchContainsNt, totals.RightTouchContainsNt, "Right touch total contains NT");
            Assert.AreEqual(expected.RightTouchTotalHasImpairmentNotDueToSci, totals.RightTouchTotalHasImpairmentNotDueToSci, "Right touch total has impairment not due to SCI");
            Assert.AreEqual(expected.LeftTouchTotal, totals.LeftTouchTotal, "Left touch total");
            Assert.AreEqual(expected.LeftTouchContainsNt, totals.LeftTouchContainsNt, "Left touch total contains NT");
            Assert.AreEqual(expected.LeftTouchTotalHasImpairmentNotDueToSci, totals.LeftTouchTotalHasImpairmentNotDueToSci, "Left touch total has impairment not due to SCI");
            Assert.AreEqual(expected.RightPrickTotal, totals.RightPrickTotal, "Right prick total");
            Assert.AreEqual(expected.RightPrickContainsNt, totals.RightPrickContainsNt, "Right prick total contains NT");
            Assert.AreEqual(expected.RightPrickTotalHasImpairmentNotDueToSci, totals.RightPrickTotalHasImpairmentNotDueToSci, "Right prick total has impairment not due to SCI");
            Assert.AreEqual(expected.LeftPrickTotal, totals.LeftPrickTotal, "Left prick total");
            Assert.AreEqual(expected.LeftPrickContainsNt, totals.LeftPrickContainsNt, "Left prick total contains NT");
            Assert.AreEqual(expected.LeftPrickTotalHasImpairmentNotDueToSci, totals.LeftPrickTotalHasImpairmentNotDueToSci, "Left prick total has impairment not due to SCI");
            Assert.AreEqual(expected.TouchTotal, totals.TouchTotal, "Touch total");
            Assert.AreEqual(expected.PrickTotal, totals.PrickTotal, "Prick");
            Assert.AreEqual(expected.RightUpperMotorTotal, totals.RightUpperMotorTotal, "Right upper motor total");
            Assert.AreEqual(expected.RightUpperMotorTotalHasImpairmentNotDueToSci, totals.RightUpperMotorTotalHasImpairmentNotDueToSci, "Right upper motor total has impairment not due to SCI");
            Assert.AreEqual(expected.RightUpperMotorContainsNt, totals.RightUpperMotorContainsNt, "Right upper motor total contains NT");
            Assert.AreEqual(expected.LeftUpperMotorTotal, totals.LeftUpperMotorTotal, "Left upper motor total");
            Assert.AreEqual(expected.LeftUpperMotorTotalHasImpairmentNotDueToSci, totals.LeftUpperMotorTotalHasImpairmentNotDueToSci, "Left upper motor total has impairment not due to SCI");
            Assert.AreEqual(expected.LeftUpperMotorContainsNt, totals.LeftUpperMotorContainsNt, "Left upper motor total contains NT");
            Assert.AreEqual(expected.RightLowerMotorTotal, totals.RightLowerMotorTotal, "Right lower motor total");
            Assert.AreEqual(expected.RightLowerMotorTotalHasImpairmentNotDueToSci, totals.RightLowerMotorTotalHasImpairmentNotDueToSci, "Right lower motor total has impairment not due to SCI");
            Assert.AreEqual(expected.RightLowerMotorContainsNt, totals.RightLowerMotorContainsNt, "Right lower motor total contains NT");
            Assert.AreEqual(expected.LeftLowerMotorTotal, totals.LeftLowerMotorTotal, "Left lower motor total");
            Assert.AreEqual(expected.LeftLowerMotorTotalHasImpairmentNotDueToSci, totals.LeftLowerMotorTotalHasImpairmentNotDueToSci, "Left lower motor total has impairment not due to SCI");
            Assert.AreEqual(expected.LeftLowerMotorContainsNt, totals.LeftLowerMotorContainsNt, "Left lower motor total contains NT");
            Assert.AreEqual(expected.UpperMotorTotal, totals.UpperMotorTotal, "Upper motor total");
            Assert.AreEqual(expected.LowerMotorTotal, totals.LowerMotorTotal, "Lower motor total");

            Assert.AreEqual(GetValueStringFrom(expected.GetRightSensoryValues()), GetValueStringFrom(totals.GetRightSensoryValues()), "Right Sensory");
            Assert.AreEqual(GetValueStringFrom(expected.GetLeftSensoryValues()), GetValueStringFrom(totals.GetLeftSensoryValues()), "Left Sensory");
            Assert.AreEqual(GetValueStringFrom(expected.GetRightMotorValues()), GetValueStringFrom(totals.GetRightMotorValues()), "Right Motor");
            Assert.AreEqual(GetValueStringFrom(expected.GetLeftMotorValues()), GetValueStringFrom(totals.GetLeftMotorValues()), "Left Motor");
            Assert.AreEqual(GetValueStringFrom(expected.GetNeurologicalLevelsOfInjury()), GetValueStringFrom(totals.GetNeurologicalLevelsOfInjury()), "Neurological Level of Injury");

            Assert.AreEqual(expected.GetAsiaImpairmentScaleValues(), totals.GetAsiaImpairmentScaleValues(), "Asia impairment scale");
            Assert.AreEqual(GetValueStringFrom(expected.GetRightSensoryZppValues()), GetValueStringFrom(totals.GetRightSensoryZppValues()), "Right sensory ZPP");
            Assert.AreEqual(GetValueStringFrom(expected.GetLeftSensoryZppValues()), GetValueStringFrom(totals.GetLeftSensoryZppValues()), "Left sensory ZPP");
            Assert.AreEqual(GetValueStringFrom(expected.GetRightMotorZppValues()), GetValueStringFrom(totals.GetRightMotorZppValues()), "Right Motor ZPP");
            Assert.AreEqual(GetValueStringFrom(expected.GetLeftMotorZppValues()), GetValueStringFrom(totals.GetLeftMotorZppValues()), "Left Motor ZPP");
        }

        [TestMethod]
        public void CanGetTotalsSummaryForNtCase2()
        {
            // Arrange
            const string filePath = PathToTests + @"NtCases/ISNCSCI NT Case 2.xml";
            var xmlDocument = XDocument.Load(filePath);

            // Act 
            var summary = Algorithm.GetTotalsSummaryFor(NeurologyFormLoader.LoadNeurologyFormFrom(xmlDocument));

            // Assert
            Assert.AreEqual("B,C,D", summary.AsiaImpairmentScale);
            Assert.AreEqual("I", summary.Completeness);
            Assert.AreEqual(NotDeterminable, summary.LeftLowerMotorTotal);
            Assert.AreEqual("C5", summary.LeftMotor);
            Assert.AreEqual(NotDeterminable, summary.LeftMotorTotal);
            Assert.AreEqual("NA", summary.LeftMotorZpp);
            Assert.AreEqual(NotDeterminable, summary.LeftPrickTotal);
            Assert.AreEqual("C5", summary.LeftSensory);
            Assert.AreEqual("NA", summary.LeftSensoryZpp);
            Assert.AreEqual(NotDeterminable, summary.LeftTouchTotal);
            Assert.AreEqual("6", summary.LeftUpperMotorTotal);
            Assert.AreEqual(NotDeterminable, summary.LowerMotorTotal);
            Assert.AreEqual("C5", summary.NeurologicalLevelOfInjury);
            Assert.AreEqual(NotDeterminable, summary.PrickTotal);
            Assert.AreEqual(NotDeterminable, summary.RightLowerMotorTotal);
            Assert.AreEqual("C5", summary.RightMotor);
            Assert.AreEqual(NotDeterminable, summary.RightMotorTotal);
            Assert.AreEqual("NA", summary.RightMotorZpp);
            Assert.AreEqual(NotDeterminable, summary.RightPrickTotal);
            Assert.AreEqual("C5", summary.RightSensory);
            Assert.AreEqual("NA", summary.RightSensoryZpp);
            Assert.AreEqual(NotDeterminable, summary.RightTouchTotal);
            Assert.AreEqual("6", summary.RightUpperMotorTotal);
            Assert.AreEqual(NotDeterminable, summary.TouchTotal);
            Assert.AreEqual("12", summary.UpperMotorTotal);
        }

        [TestMethod]
        public void CanGetTotalsSummaryForNtCase4()
        {
            // Arrange
            const string filePath = PathToTests + @"NtCases/ISNCSCI NT Case 4.xml";
            var xmlDocument = XDocument.Load(filePath);

            // Act 
            var summary = Algorithm.GetTotalsSummaryFor(NeurologyFormLoader.LoadNeurologyFormFrom(xmlDocument));

            // Assert
            Assert.AreEqual("A,D", summary.AsiaImpairmentScale);
            Assert.AreEqual("C,I", summary.Completeness);
            Assert.AreEqual("16", summary.LeftLowerMotorTotal);
            Assert.AreEqual("C5", summary.LeftMotor);
            Assert.AreEqual("26", summary.LeftMotorTotal);
            Assert.AreEqual("NA,S1", summary.LeftMotorZpp);
            Assert.AreEqual(NotDeterminable, summary.LeftPrickTotal);
            Assert.AreEqual("C4", summary.LeftSensory);
            Assert.AreEqual("NA,S2-S3", summary.LeftSensoryZpp);
            Assert.AreEqual(NotDeterminable, summary.LeftTouchTotal);
            Assert.AreEqual("10", summary.LeftUpperMotorTotal);
            Assert.AreEqual("32", summary.LowerMotorTotal);
            Assert.AreEqual("C4", summary.NeurologicalLevelOfInjury);
            Assert.AreEqual(NotDeterminable, summary.PrickTotal);
            Assert.AreEqual("16", summary.RightLowerMotorTotal);
            Assert.AreEqual("C5", summary.RightMotor);
            Assert.AreEqual("26", summary.RightMotorTotal);
            Assert.AreEqual("NA,S1", summary.RightMotorZpp);
            Assert.AreEqual(NotDeterminable, summary.RightPrickTotal);
            Assert.AreEqual("C4", summary.RightSensory);
            Assert.AreEqual("NA,S2-S3", summary.RightSensoryZpp);
            Assert.AreEqual(NotDeterminable, summary.RightTouchTotal);
            Assert.AreEqual("10", summary.RightUpperMotorTotal);
            Assert.AreEqual(NotDeterminable, summary.TouchTotal);
            Assert.AreEqual("20", summary.UpperMotorTotal);
        }

        private static string GetValueStringFrom(IList<NeurologyFormLevel> levels)
        {
            return string.Join(",", levels.OrderBy(l => l.Ordinal).Select(v => v.Name));
        }
    }
}