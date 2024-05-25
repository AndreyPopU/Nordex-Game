using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadePanel : MonoBehaviour
{
    public static FadePanel instance;
    public CanvasGroup group;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(transform.parent.gameObject);

        group = GetComponent<CanvasGroup>();
    }

    public void LoadScene(string scene, Vector3 spawnPos)
    {
        StartCoroutine(LoadSceneAsync(scene, spawnPos));
    }

    private IEnumerator LoadSceneAsync(string scene, Vector3 spawnPos)
    {
        YieldInstruction wait = new WaitForFixedUpdate();

        Player.instance.rb.isKinematic = true;

        // Fade out
        while (group.alpha < 1)
        {
            group.alpha += .1f;
            yield return wait;
        }

        // Start loading scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        // If fully faded out && scene is ready to be activated
        while (group.alpha < 1 || !operation.isDone)
        {
            yield return null;
        }

        // Setup player
        Player.instance.transform.position = spawnPos;
        Player.instance.rb.isKinematic = false;

        while (group.alpha > 0)
        {
            group.alpha -= .1f;
            yield return wait;
        }
    }
}
