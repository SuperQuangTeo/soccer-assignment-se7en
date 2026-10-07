using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button resetButton;

    private void Awake()
    {
        resetButton.onClick.AddListener(ResetScene);
    }
    public void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void OnDestroy()
    {
        resetButton.onClick.RemoveListener(ResetScene);
    }
}
