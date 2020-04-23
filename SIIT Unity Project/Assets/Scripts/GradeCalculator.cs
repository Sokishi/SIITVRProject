using System;
using UnityEngine;

[CreateAssetMenu(menuName="GradeCalculator")]
public class GradeCalculator: ScriptableObject
{
    public int timeForMaxGrade = 2;
    public int secondsPerGradePenalty = 20;

    public enum Grade
    {
        A, // 20
        B, // 21 / 19
        C, // 22 / 18
        D, // 23 / 17
        E, // 24 / 16
        F // 25 / 15
    }

    public Grade CalculateGrade(int time)
    {
        var timeDifference = Math.Abs(timeForMaxGrade - time);
        if (timeDifference == 0) 
            return (Grade) timeDifference;
            
            
        var gradeLastIndex = Enum.GetNames(typeof(Grade)).Length - 1; 
        var multiplesOfDifference = timeDifference / secondsPerGradePenalty;
        var gradeOffset = multiplesOfDifference;
        if (gradeOffset > gradeLastIndex)
            return (Grade) gradeLastIndex;
            
        return (Grade) gradeOffset;
    }
}