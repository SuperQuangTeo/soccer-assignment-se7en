using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBallDetector : MonoBehaviour
{
    [SerializeField] private Button kickButton;
    [SerializeField] private Button autoKickButton;
    [SerializeField] private float range = 2f;

    [SerializeField] private Goal[] goals;
    [SerializeField] private Transform[] balls;

    private void Awake()
    {
        kickButton.onClick.AddListener(KickBall);
        autoKickButton.onClick.AddListener(AutoKickBall);
    }

    private void Update()
    {
        CheckBallInRange();
    }

    private void CheckBallInRange()
    {
        bool isInRange = false;

        foreach (Transform ball in balls)
        {
            float distance = Vector3.Distance(transform.position, ball.position);

            if (distance <= range)
            {
                isInRange = true;
                break;
            }
        }

        kickButton.gameObject.SetActive(isInRange);
    }

    private void KickBall()
    {
        Transform nearestBall = GetNearestBall();
        if (nearestBall == null) return;

        BallController ballController = nearestBall.GetComponent<BallController>();

        if (ballController == null) return;

        if (ballController.Iskicking) return;

        Goal NearestGoal = GetNearestGoal(nearestBall);

        ballController.KickBallToGoal(NearestGoal.transform);
    }

    private void AutoKickBall()
    {
        Transform farthestBall = GetFarthestBall();
        if (farthestBall == null) return;

        BallController ballController = farthestBall.GetComponent<BallController>();

        if (ballController == null) return;

        if (ballController.Iskicking) return;

        Goal nearestGoal = GetNearestGoal(farthestBall);
        Debug.Log(nearestGoal);

        ballController.KickBallToGoal(nearestGoal.transform);
    }

    private Goal GetNearestGoal(Transform ball)
    {
        Goal nearestGoal = null;

        float nearestDistance = Mathf.Infinity;
        foreach (Goal goal in goals)
        {
            if (goal == null) continue;

            float distance = 0f;

            distance = Vector3.Distance(ball.position, goal.transform.position);
            if (distance < nearestDistance)
            {
                nearestGoal = goal;
                nearestDistance = distance;
            }
        }
        return nearestGoal;
    }

    private Transform GetFarthestBall()
    {
        float farthestDistance = -Mathf.Infinity;
        Transform farthestBall = null;
        foreach (Transform ball in balls)
        {
            if (ball == null) continue;
            BallController ballController = ball.GetComponent<BallController>();

            if (ballController == null) continue;

            if (ballController.Iskicking) continue;

            if (ballController.HasReachedGoal) continue;

            float distance = Vector3.Distance(transform.position, ball.transform.position);
            if (distance > farthestDistance)
            {
                farthestDistance = distance;
                farthestBall = ball;
            }
        }
        return farthestBall;
    }

    private Transform GetNearestBall()
    {
        float nearestDistance = Mathf.Infinity;
        Transform nearBall = null;
        foreach (Transform ball in balls)
        {
            if (ball == null) continue;
            BallController ballController = ball.GetComponent<BallController>();

            if (ballController == null) continue;

            if (ballController.Iskicking) continue;

            if (ballController.HasReachedGoal) continue;
            float distance = Vector3.Distance(transform.position, ball.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearBall = ball;
            }
        }
        return nearBall;
    }

    private void OnDestroy()
    {
        kickButton.onClick.RemoveListener(KickBall);
        autoKickButton.onClick.RemoveListener(AutoKickBall);
    }
}
