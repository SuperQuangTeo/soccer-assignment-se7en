using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private Rigidbody ballRb;
    private bool isKicking;
    private bool hasReachedGoal;

    public bool HasReachedGoal => hasReachedGoal;
    public bool Iskicking => isKicking;

    private void Awake()
    {
        ballRb = GetComponent<Rigidbody>();
    }

    public void KickBallToGoal(Transform goal)
    {
        if (isKicking) return;

        StartCoroutine(BallFlyToGoal(goal));
    }

    private IEnumerator BallFlyToGoal(Transform goal)
    {
        isKicking = true;
        ballRb.isKinematic = true;
        Vector3 startPoint = transform.position;
        Vector3 goalPoint = goal.position;

        float distance = Vector3.Distance(startPoint, goalPoint);
        Vector3 direction = (goal.position - transform.position).normalized;

        float duration = distance / speed;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            Vector3 newPosition = transform.position + direction * speed * Time.fixedDeltaTime;

            ballRb.MovePosition(newPosition);

            yield return new WaitForFixedUpdate();
        }
        ballRb.MovePosition(goalPoint);
        hasReachedGoal = true;
        isKicking = false;
    }
}
