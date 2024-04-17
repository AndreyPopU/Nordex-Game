using UnityEngine;

public class StartPuzzle2Script : MonoBehaviour
{
    public GameObject puzzle2;
    public GameObject player; 

    public void OnButtonClick()
    {
        if (puzzle2 != null)
        {
            puzzle2.SetActive(true);

            // Freeze player position
            if (player != null)
            {
                Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    // Freeze position
                    playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                }
            }
        }

        Destroy(gameObject);
    }
}