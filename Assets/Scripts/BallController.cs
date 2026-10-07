using System;
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

    public static Action OnReachedGoal;

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

        BoxCollider goalCol = goal.GetComponent<BoxCollider>();
        Bounds bounds = goalCol.bounds;

        Vector3 startPoint = transform.position;
        Vector3 goalPoint = goal.position;

        goalPoint.z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);

        float distance = Vector3.Distance(startPoint, goalPoint);
        Vector3 direction = (goalPoint - transform.position).normalized;
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

        Goal goalController = goal.GetComponent<Goal>();

        if (goalController != null)
        {
            goalController.PlayConfetti();
        }

        OnReachedGoal?.Invoke();
    }
}
