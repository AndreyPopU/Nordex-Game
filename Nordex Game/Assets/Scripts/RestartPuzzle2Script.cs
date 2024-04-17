using UnityEngine;

public class RestartPuzzle2Script : MonoBehaviour
{
    public GameObject puzzle2; 
    public GameObject parentGameObject; 

    public void OnButtonClick()
    {
        if (puzzle2 != null)
        {
            puzzle2.SetActive(false); // Deactivate the puzzle
            puzzle2.SetActive(true); // Reactivate the puzzle to reset it
        }

        if (parentGameObject != null)
        {
            // Find all CubeControllerScripts in children and reset them
            CubeControllerScript[] cubeControllers = parentGameObject.GetComponentsInChildren<CubeControllerScript>();
            foreach (CubeControllerScript cubeController in cubeControllers)
            {
                cubeController.ResetCube(); // Reset each cube
            }
        }
    }
}