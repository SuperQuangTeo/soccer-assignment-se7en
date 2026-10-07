using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider fieldBounds;
    private Rigidbody playerRb;
    private Vector3 movement;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RunAnimation();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 newPosition = playerRb.position + movement * moveSpeed * Time.fixedDeltaTime;

        Bounds bounds = fieldBounds.bounds;
        newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
        newPosition.z = Mathf.Clamp(newPosition.z, bounds.min.z, bounds.max.z);

        playerRb.MovePosition(newPosition);
    }

    private void Rotate()
    {
        if (movement == Vector3.zero) return;
        transform.forward = movement;
    }

    private void RunAnimation()
    {
        bool isMoving = movement != Vector3.zero;
        animator.SetBool("IsMoving", isMoving);
    }
}
