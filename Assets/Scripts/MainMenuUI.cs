using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string gameSceneName = "Game"; // döp till er spelscen
    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        Invoke("ChangeScene", 3f); // liten fördröjning för att hinna spela en ljud eller animation
        sound.Play();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
