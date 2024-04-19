using UnityEngine;

public class StartPuzzle2Script : Puzzle
{
    public GameObject puzzle2; 
    public GameObject player; 
    public GameObject restartButton;

    private void Start()
    {
        OnButtonClick();
    }

    public void OnButtonClick()
    {
        Focus(focusTransform);

        if ((puzzle2 != null) & (restartButton != null))
        {
            puzzle2.SetActive(true); // Activate the puzzle
            restartButton.SetActive(true); // Activate the restart button

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

        Destroy(gameObject); // Destroy this game object after starting the puzzle
    }
}