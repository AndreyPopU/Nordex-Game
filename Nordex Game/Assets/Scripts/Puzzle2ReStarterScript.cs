using UnityEngine;

public class Puzzle2ReStarterScript : MonoBehaviour
{
    public GameObject puzzle2;
    public GameObject parentGameObject;

    public void OnButtonClick()
    {
        if (puzzle2 != null)
        {
            puzzle2.SetActive(false);
            puzzle2.SetActive(true);
        }

        if (parentGameObject != null)
        {
            Puzzle2FormsControllerScript[] cubeControllers = parentGameObject.GetComponentsInChildren<Puzzle2FormsControllerScript>();

            foreach (Puzzle2FormsControllerScript cubeController in cubeControllers)
            {
                cubeController.ResetCube();
            }

            Puzzle2ScaleObjectSmoothlyXScript[] scaleObjectsX = parentGameObject.GetComponentsInChildren<Puzzle2ScaleObjectSmoothlyXScript>();

            foreach (Puzzle2ScaleObjectSmoothlyXScript scaleObjectX in scaleObjectsX)
            {
                scaleObjectX.StopAllCoroutines();
                scaleObjectX.ResetToOriginalScale(); 
                scaleObjectX.StartCoroutine(scaleObjectX.ScaleAfterDelay());
            }

            Puzzle2ScaleObjectSmoothlyYScript[] scaleObjectsY = parentGameObject.GetComponentsInChildren<Puzzle2ScaleObjectSmoothlyYScript>();

            foreach (Puzzle2ScaleObjectSmoothlyYScript scaleObjectY in scaleObjectsY)
            {
                scaleObjectY.StopAllCoroutines();
                scaleObjectY.ResetToOriginalScale();
                scaleObjectY.StartCoroutine(scaleObjectY.ScaleAfterDelay());
            }
        }
    }
}