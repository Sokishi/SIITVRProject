using NonVR;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class GradeCalculatorTests
    {
        private GradeCalculator gradeCalculator;

        [SetUp]
        public void SetUp()
        {
            gradeCalculator = ScriptableObject.CreateInstance<GradeCalculator>();
            gradeCalculator.secondsPerGradePenalty = 2;
            gradeCalculator.timeForMaxGrade = 20;
        }

        [TearDown]
        public void TearDown()
        {
            gradeCalculator = null;
        }
        
        [Test]
        public void TestGradeA()
        {
           const int perfectTime = 20;
           const int underTime = 19;
           const int overTime = 21;
           
           var perfectGrade = gradeCalculator.CalculateGrade(perfectTime);
           var underGrade = gradeCalculator.CalculateGrade(underTime);
           var overGrade = gradeCalculator.CalculateGrade(overTime);
           
           Assert.AreEqual(GradeCalculator.Grade.A,perfectGrade);
           Assert.AreEqual(GradeCalculator.Grade.A,underGrade);
           Assert.AreEqual(GradeCalculator.Grade.A,overGrade);
        }
        
        [Test]
        public void TestGradeB()
        {
            const int underTime = 18;
            const int overTime = 22;
            const int underByOneMoreTime = 17;
            const int overByOneMoreTime = 23;
            
            var underGrade = gradeCalculator.CalculateGrade(underTime);
            var overGrade = gradeCalculator.CalculateGrade(overTime);
            var underByOneMoreGrade = gradeCalculator.CalculateGrade(underByOneMoreTime);
            var overByOneMoreGrade = gradeCalculator.CalculateGrade(overByOneMoreTime);
            
            Assert.AreEqual(GradeCalculator.Grade.B,underGrade);
            Assert.AreEqual(GradeCalculator.Grade.B,overGrade);
            Assert.AreEqual(GradeCalculator.Grade.B,underByOneMoreGrade);
            Assert.AreEqual(GradeCalculator.Grade.B,overByOneMoreGrade);
        }
        
        [Test]
        public void TestGradeC()
        {
            const int underTime = 16;
            const int overTime = 24;
            const int underByOneMoreTime = 15;
            const int overByOneMoreTime = 25;
            
            var underGrade = gradeCalculator.CalculateGrade(underTime);
            var overGrade = gradeCalculator.CalculateGrade(overTime);
            var underByOneMoreGrade = gradeCalculator.CalculateGrade(underByOneMoreTime);
            var overByOneMoreGrade = gradeCalculator.CalculateGrade(overByOneMoreTime);
            
            Assert.AreEqual(GradeCalculator.Grade.C,underGrade);
            Assert.AreEqual(GradeCalculator.Grade.C,overGrade);
            Assert.AreEqual(GradeCalculator.Grade.C,underByOneMoreGrade);
            Assert.AreEqual(GradeCalculator.Grade.C,overByOneMoreGrade);
        }
        
        [Test]
        public void TestGradeD()
        {
            const int underTime = 14;
            const int overTime = 26;
            const int underByOneMoreTime = 13;
            const int overByOneMoreTime = 27;
            
            var underGrade = gradeCalculator.CalculateGrade(underTime);
            var overGrade = gradeCalculator.CalculateGrade(overTime);
            var underByOneMoreGrade = gradeCalculator.CalculateGrade(underByOneMoreTime);
            var overByOneMoreGrade = gradeCalculator.CalculateGrade(overByOneMoreTime);
            
            Assert.AreEqual(GradeCalculator.Grade.D,underGrade);
            Assert.AreEqual(GradeCalculator.Grade.D,overGrade);
            Assert.AreEqual(GradeCalculator.Grade.D,underByOneMoreGrade);
            Assert.AreEqual(GradeCalculator.Grade.D,overByOneMoreGrade);
        }
        
        [Test]
        public void TestGradeE()
        {
            const int underTime = 12;
            const int overTime = 28;
            const int underByOneMoreTime = 11;
            const int overByOneMoreTime = 29;
            
            var underGrade = gradeCalculator.CalculateGrade(underTime);
            var overGrade = gradeCalculator.CalculateGrade(overTime);
            var underByOneMoreGrade = gradeCalculator.CalculateGrade(underByOneMoreTime);
            var overByOneMoreGrade = gradeCalculator.CalculateGrade(overByOneMoreTime);
            
            Assert.AreEqual(GradeCalculator.Grade.E,underGrade);
            Assert.AreEqual(GradeCalculator.Grade.E,overGrade);
            Assert.AreEqual(GradeCalculator.Grade.E,underByOneMoreGrade);
            Assert.AreEqual(GradeCalculator.Grade.E,overByOneMoreGrade);
        }
        
        [Test]
        public void TestGradeF()
        {
            const int underTime = 10;
            const int overTime = 30;
            const int underByOneMoreTime = 9;
            const int overByOneMoreTime = 31;
            
            var underGrade = gradeCalculator.CalculateGrade(underTime);
            var overGrade = gradeCalculator.CalculateGrade(overTime);
            var underByOneMoreGrade = gradeCalculator.CalculateGrade(underByOneMoreTime);
            var overByOneMoreGrade = gradeCalculator.CalculateGrade(overByOneMoreTime);
            
            Assert.AreEqual(GradeCalculator.Grade.F,underGrade);
            Assert.AreEqual(GradeCalculator.Grade.F,overGrade);
            Assert.AreEqual(GradeCalculator.Grade.F,underByOneMoreGrade);
            Assert.AreEqual(GradeCalculator.Grade.F,overByOneMoreGrade);
        }
       
        [Test]
        public void TestGradeExtremes()
        {
            const int underTime = 1;
            const int overTime = 200;
            
            var underGrade = gradeCalculator.CalculateGrade(underTime);
            var overGrade = gradeCalculator.CalculateGrade(overTime);
            
            Assert.AreEqual(GradeCalculator.Grade.F,underGrade);
            Assert.AreEqual(GradeCalculator.Grade.F,overGrade);
        }
    }
}
