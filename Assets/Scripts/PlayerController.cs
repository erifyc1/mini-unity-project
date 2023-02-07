using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 50;
    [SerializeField] Vector3 startPosition;

    private float fbAxis, rlAxis = 0;

    private Rigidbody rb;

    void Start()
    {
        transform.position = startPosition;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float adjustedSpeed = Time.fixedDeltaTime * movementSpeed * 1000;
        rb.velocity = adjustedSpeed * rlAxis * transform.right + adjustedSpeed * fbAxis * transform.forward + transform.up * (-9.8f * Time.fixedDeltaTime + rb.velocity.y);
        rb.angularVelocity = Vector3.zero;
        //Debug.Log(rb.velocity);
    }

    public void UpdateRightLeft(InputAction.CallbackContext context)
    {
        rlAxis = context.ReadValue<float>();
    }
    public void UpdateForwardBack(InputAction.CallbackContext context)
    {
        fbAxis = context.ReadValue<float>();
    }
}
