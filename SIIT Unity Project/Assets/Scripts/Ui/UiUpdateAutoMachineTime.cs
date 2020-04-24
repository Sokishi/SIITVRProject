using TMPro;
using UnityEngine;
using Utilities;

namespace Ui
{
    public class UiUpdateAutoMachineTime : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private FloatVariable variable;

        private const string autoMachineTimePrefix = "AutoMachine Time: ";

        private void Update()
        {
            if (tmpText == null || variable == null) return;
            tmpText.text = autoMachineTimePrefix + variable.Value.ToString("F2");
        }
    }
}
