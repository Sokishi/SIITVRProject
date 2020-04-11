using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NonVR
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private float pickupRange = 1f;
        [SerializeField] private TMP_Text pickupText;
        [SerializeField] private Transform holdingTransform;
        private Transform playerCameraTransform;
        private IInteractable hoveringOverInteractable;
        private IInteractable holdingItem;

        private void Start()
        {
            Cursor.visible = false;
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
                pickupText.gameObject.SetActive(true);
                pickupText.text = "PICK UP: " + hit.transform.name;
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
                print("Clicked drop");
                Drop(holdingItem);
                return;
            }

            // else, pickup if hovering over item

            if (hoveringOverInteractable == null)
            {
                print("Clicked with nothing hovering");
                return;
            }
            print("clicked with hovering");
            Pickup(hoveringOverInteractable);
        }

        private void Drop(IInteractable interactable)
        {
            print("DROPPING");
            holdingItem = null;
            var cameraPosition = playerCameraTransform.position;
            var cameraForward = playerCameraTransform.forward;
            var dropRange = pickupRange / 2;
            var target = cameraPosition + cameraForward * dropRange;
            var dropHeight = 0.5f;
            var dropPosition = new Vector3(target.x, target.y + dropHeight, target.z);
            interactable.Drop(dropPosition);
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