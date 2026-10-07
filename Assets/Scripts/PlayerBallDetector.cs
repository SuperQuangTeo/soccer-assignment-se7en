using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBallDetector : MonoBehaviour
{
    [SerializeField] private Button kickButton;
    [SerializeField] private Button autoKickButton;
    [SerializeField] private float range = 1.5f;

    [SerializeField] private Goal[] goals;
    [SerializeField] private Transform[] balls;
    [SerializeField] private CameraController cameraController;

    private void Awake()
    {
        kickButton.onClick.AddListener(KickBall);
        autoKickButton.onClick.AddListener(AutoKickBall);
    }

    private void Update()
    {
        CheckBallInRange();
    }

    private void OnEnable()
    {
        BallController.OnReachedGoal += FollowCameraPlayer;
    }

    private void OnDisable()
    {
        BallController.OnReachedGoal -= FollowCameraPlayer;

    }

    private void FollowCameraPlayer()
    {
        StartCoroutine(FollowCameraPlayerCoroutine());
    }

    private IEnumerator FollowCameraPlayerCoroutine()
    {
        yield return new WaitForSeconds(2);
        cameraController.FollowObject(gameObject.transform);
        SetButtonStatus(true);
    }

    private void SetButtonStatus(bool isActivate)
    {
        kickButton.interactable = isActivate;
        autoKickButton.interactable = isActivate;
    }

    private void CheckBallInRange()
    {
        bool isInRange = false;

        foreach (Transform ball in balls)
        {
            if (ball == null) continue;

            BallController ballController = ball.GetComponent<BallController>();

            if (ballController == null) continue;

            if (ballController.Iskicking) continue;

            if (ballController.HasReachedGoal) continue;
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
        SetButtonStatus(false);

        Transform nearestBall = GetNearestBall();
        if (nearestBall == null) return;

        BallController ballController = nearestBall.GetComponent<BallController>();

        if (ballController == null) return;

        if (ballController.Iskicking) return;

        Goal NearestGoal = GetNearestGoal(nearestBall);

        cameraController.FollowObject(nearestBall);

        ballController.KickBallToGoal(NearestGoal.transform);
    }

    private void AutoKickBall()
    {
        SetButtonStatus(false);

        Transform farthestBall = GetFarthestBall();
        if (farthestBall == null) return;

        BallController ballController = farthestBall.GetComponent<BallController>();

        if (ballController == null) return;

        if (ballController.Iskicking) return;

        Goal nearestGoal = GetNearestGoal(farthestBall);

        cameraController.FollowObject(farthestBall);

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
            if (distance < nearestDistance && distance <= range)
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
