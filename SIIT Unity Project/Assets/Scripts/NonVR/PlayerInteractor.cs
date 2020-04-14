using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NonVR
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private float pickupRange = 1f;
        [SerializeField] private TMP_Text pickupText = null;
        [SerializeField] private Transform holdingTransform = null;
        private Transform playerCameraTransform;
        private IInteractable hoveringOverInteractable;
        private IInteractable holdingItem;

        private void Start()
        {
            playerCameraTransform = GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            var cameraPosition = playerCameraTransform.position;
            var cameraForward = playerCameraTransform.forward;
            var target = cameraPosition + cameraForward * pickupRange;

            var ray = new Ray(cameraPosition, cameraForward);
            if (Physics.Raycast(ray, out var hit, pickupRange))
            {
                hoveringOverInteractable = hit.transform.GetComponent<IInteractable>();

                if (hoveringOverInteractable == null)
                {
                    HitNothing(cameraPosition, target);
                    return;
                }

                Debug.DrawLine(cameraPosition, target, Color.green);
                print("Pickup text: " + pickupText);
                pickupText.gameObject.SetActive(true);
                pickupText.text = "PICK UP: " + hit.transform.name;
                print("Pickup status: " + pickupText.isActiveAndEnabled);
            }
            else
            {
                HitNothing(cameraPosition, target);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Clicked();
            }
        }

        private void Clicked()
        {
            if (holdingItem != null)
            {
                Drop(holdingItem);
                return;
            }

            // else, pickup if hovering over item

            if (hoveringOverInteractable == null) return;
            Pickup(hoveringOverInteractable);
        }

        private void Drop(IInteractable interactable)
        {
            holdingItem = null;
            interactable.Drop();
        }

        private void Pickup(IInteractable interactable)
        {
            if (holdingTransform == null) return;
            interactable.PickUp(holdingTransform);
            holdingItem = interactable;
        }

        private void HitNothing(Vector3 cameraPosition, Vector3 target)
        {
            hoveringOverInteractable = null;
            pickupText.gameObject.SetActive(false);
            Debug.DrawLine(cameraPosition, target, Color.red);
        }
    }
}