using System.Collections.Generic;
using UnityEngine;

namespace NonVR
{
    public class CarAssembler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> parts;
        private int currentPartIndex;

        private void Awake()
        {
            currentPartIndex = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentPartIndex < parts.Count)
                {
                    parts[currentPartIndex].SetActive(true);
                    currentPartIndex++;

                }
            }
        }
    }
}
