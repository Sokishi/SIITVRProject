using System;
using echo17.Signaler.Core;
using Signals;
using TMPro;
using UnityEngine;

namespace Ui
{
   public class UiUpdateGrade : MonoBehaviour, ISubscriber
   {
      [SerializeField] private TMP_Text tmpText;

      private void Awake()
      {
         Signaler.Instance.Subscribe<AssemblySignals.AchievedGradeSignal>(this, UpdateGradeText);
      }

      private bool UpdateGradeText(AssemblySignals.AchievedGradeSignal signal)
      {
         tmpText.text = "Achieved Grade: " + signal.Grade;
         return true;
      }
   }
}
