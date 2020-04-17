using System;
using UnityEngine;

namespace NonVR
{
    public class CarAssemblerLoop : MonoBehaviour
    {
        [SerializeField] private int numberOfLoops = 1;
        [SerializeField] private GameObject copyPrefab;

        private int currentLoop = 0;
        private GameObject copiedObject;

        private void Awake()
        {
            CopyAndHideRootObject(copyPrefab);
        }

        private void OnEnable()
        {
            GameEventSystem.Instance.onAssemblyComplete += AssemblyCompleted;
        }

        private void OnDisable()
        {
            GameEventSystem.Instance.onAssemblyComplete -= AssemblyCompleted;
        }


        private void Start()
        {
            GameEventSystem.Instance.StartAssemblyLoop();
            GameEventSystem.Instance.UpdateLoopUi(currentLoop, numberOfLoops);
        }

        private void CopyAndHideRootObject(GameObject objToCopy)
        {
            copiedObject = Instantiate(objToCopy, transform);
            copiedObject.SetActive(false);
        } 

        private void AssemblyCompleted()
        {
            if (currentLoop < numberOfLoops - 1)
            {
                currentLoop++;
                copiedObject.SetActive(true);
                GameEventSystem.Instance.StartAssemblyLoop();
                CopyAndHideRootObject(copiedObject);
                GameEventSystem.Instance.UpdateLoopUi(currentLoop, numberOfLoops);
            }
            else
            {
                currentLoop++;
                GameEventSystem.Instance.UpdateLoopUi(currentLoop, numberOfLoops);
                GameEventSystem.Instance.StopAssemblyLoop();
            }
        }
    }
}