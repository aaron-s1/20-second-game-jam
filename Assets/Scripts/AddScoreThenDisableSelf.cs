using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreThenDisableSelf : MonoBehaviour
{
    Renderer rend;

    void Awake() =>
        rend = GetComponent<Renderer>();


    void OnMouseDown()
    {
        if (!GameManager.Instance.gameHasStarted)
            return;
        if (GameManager.Instance.gameHasEnded)
            return;

        GameManager.Instance.IncrementScore();
        gameObject.SetActive(false);
    }


    void OnDisable() {
        if (gameObject.scene.isLoaded)
            GameManager.Instance.SpawnNewMooMoo(); 
    }
}
