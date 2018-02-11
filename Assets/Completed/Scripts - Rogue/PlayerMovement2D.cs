using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public LayerMask GroundMask;
    public LayerMask IgnoreMask;

    private Rigidbody2D rig;

    [Range(0.5f, 15.0f)]
    public float fallMultiplier = 2.5f;

    [Range(0.5f, 15.0f)]
    public float jumpVelocity = 5.0f;

    [Range(0.1f, 2.0f)]
    public float walkMultiplier = 5.0f;

    [SerializeField]
    private Vector2 moveDirection;

    // Use this for initialization
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        TakeInput();

        ApplyMotion();
    }

    private void ApplyMotion()
    {
        //slam down jump
        if (rig.velocity.y < 0)
        {
            rig.AddForce(Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime, ForceMode2D.Impulse);
        }
        else if (rig.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rig.AddForce(Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (!IsGrounded())
        {
            //horizontal movement
            rig.AddForce(moveDirection * walkMultiplier, ForceMode2D.Impulse);
        }
    }

    private void TakeInput()
    {
        //Jump Input
        if (Input.GetButtonDown("Jump"))
        {
            rig.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        }

        moveDirection = new Vector2((Input.GetAxis("Horizontal")), 0);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapArea(transform.position, transform.position, GroundMask);
    }
}