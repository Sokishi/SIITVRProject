using UnityEngine;

namespace NonVR
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float rotationSpeed = 10f;

        [SerializeField] private float zoomedFieldOfView = 50f;
        [SerializeField] private float zoomFieldOfViewTime = 0.25f;

        private CharacterController characterController;
        private Camera playerCamera;
        private float originalFieldOfView;

        private void Awake()
        {
            playerCamera = GetComponentInChildren<Camera>();
            characterController = GetComponent<CharacterController>();
            originalFieldOfView = playerCamera.fieldOfView;
        }

        private void Update()
        {
            var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movement = transform.TransformDirection(movement);
            movement *= moveSpeed;

            characterController.SimpleMove(movement * Time.deltaTime);
            
            var horizontalRotation = rotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
            var verticalRotation = rotationSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            transform.Rotate(0, horizontalRotation, 0);        
            playerCamera.transform.Rotate(-verticalRotation, 0, 0);

            if (Input.GetMouseButtonDown(1))
            {
                var original = playerCamera.fieldOfView;
                LeanTween.value(playerCamera.gameObject, original, zoomedFieldOfView, zoomFieldOfViewTime)
                    .setOnUpdate(value => { playerCamera.fieldOfView = value; }).setEaseInOutQuad();
            }

            if (Input.GetMouseButtonUp(1))
            {
                var original = playerCamera.fieldOfView;
                LeanTween.value(playerCamera.gameObject, original, originalFieldOfView, zoomFieldOfViewTime)
                    .setOnUpdate(value => { playerCamera.fieldOfView = value; });            
            }
            
        }
    }
}
