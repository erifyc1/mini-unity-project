using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    [SerializeField] float xSensitivity = 10;
    [Range(0.0f, 10.0f)]
    [SerializeField] float ySensitivity = 10;

    private Quaternion playerTargetRot;
    private Quaternion cameraTargetRot;

    private float maxVerticalAngle = 80f;
    private Transform playerTransform;
    private float slerpDelayFactor = 10f;

    private Vector2 deltaMouse;
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.identity;
        playerTransform = transform.parent;
        playerTargetRot = playerTransform.localRotation;
        cameraTargetRot = transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        UpdateLookDirection();
    }

    private void UpdateLookDirection()
    {
        playerTargetRot *= Quaternion.Euler(0f, deltaMouse.x * xSensitivity, 0f);
        cameraTargetRot *= Quaternion.Euler(-deltaMouse.y * ySensitivity, 0f, 0f);
        
        if (Quaternion.Angle(Quaternion.Euler(new Vector3(0,90,0)), cameraTargetRot) > maxVerticalAngle)
        {
            cameraTargetRot = ClampVerticalRotation(cameraTargetRot);
        }

        playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, playerTargetRot, slerpDelayFactor * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, cameraTargetRot, slerpDelayFactor * Time.deltaTime);

        //playerTransform.localRotation = playerTargetRot;
        //transform.localRotation = cameraTargetRot;
    }

    public void SetDeltaMouse(InputAction.CallbackContext context)
    {
        deltaMouse = context.ReadValue<Vector2>();
    }

    Quaternion ClampVerticalRotation (Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -maxVerticalAngle, maxVerticalAngle);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
