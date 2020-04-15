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
            copiedObject = Instantiate(copyPrefab);
            copiedObject.SetActive(false);
        }

        private void Start()
        {
            GameEventSystem.Instance.onAssemblyComplete += AssemblyCompleted;
            GameEventSystem.Instance.StartAssemblyLoop();
        }

        private void OnDestroy()
        {
            GameEventSystem.Instance.onAssemblyComplete -= AssemblyCompleted;
        }

        private void AssemblyCompleted()
        {
            if (currentLoop < numberOfLoops - 1)
            {
                currentLoop++;
                // Record Time
                // TODO: Fix this, currently it generates junk objects
                copiedObject.SetActive(true);
                GameEventSystem.Instance.StartAssemblyLoop();
            }
            else
            {
                GameEventSystem.Instance.StopAssemblyLoop();
            }
        }
    }
}