using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float boostModifier = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int maxJumps = 2;

    private Rigidbody2D rb;
    private Vector2 inputDirection = Vector2.zero;
    private int numJumps = 0;
    private bool isGrounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 1f;
    }

    void OnMoving(InputValue value)
    {
        Vector2 movementDir = value.Get<Vector2>();
        inputDirection = movementDir;
    }

    void OnJump(InputValue value)
    {
        Debug.Log("Jump action triggered");
        // Check if grounded or if the vertical speed is nearly zero
        if (isGrounded || Mathf.Abs(rb.velocity.y) < 0.1f || numJumps < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset Y velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply jump force
            numJumps++;
            isGrounded = false; // Player is airborne after jump
            Debug.Log("Jumped. Current numJumps: " + numJumps);
        }
        else
        {
            Debug.Log("Max jumps reached or not grounded.");
        }
    }
    /*void OnDrop(InputValue value)
    {
        Debug.Log("Drop action triggered");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        isGrounded = false;
    }*/

    private void Update()
    {
        rb.AddForce(inputDirection * boostModifier);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Player is grounded
            numJumps = 0; // Reset jump count on landing
            Debug.Log("Landed on ground. numJumps reset to 0.");
        }
        Debug.Log("Collision detected with: " + collision.gameObject.name);
    }
}