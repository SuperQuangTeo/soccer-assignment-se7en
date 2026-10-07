using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject confettiPrefab;

    public void PlayConfetti()
    {
        if (confettiPrefab == null) return;

        GameObject confetti = Instantiate(confettiPrefab, transform.position, Quaternion.identity);

        Destroy(confetti, 3f);
    }
}
