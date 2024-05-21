using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public IEnumerator FadeOut()
    {
        // Wait for scene activation
        yield return new WaitForSeconds(1);

        Player.instance.rb.isKinematic = false;

        YieldInstruction wait = new WaitForFixedUpdate();

        while (group.alpha > 0)
        {
            group.alpha -= .1f;
            yield return wait;
        }

    }
}
