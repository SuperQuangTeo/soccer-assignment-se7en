using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    private Rigidbody playerRigid;
    private Vector3 movement;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody>();
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
        Vector3 newPosition = playerRigid.position + movement * moveSpeed * Time.fixedDeltaTime;
        playerRigid.MovePosition(newPosition);
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
