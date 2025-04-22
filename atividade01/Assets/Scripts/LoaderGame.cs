using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoaderGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        Loader.Instance.LoadScene("Game"); // Certifique-se que "Game" está no Build Settings
    }
    
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit(); // Só funciona em build (não no editor)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
