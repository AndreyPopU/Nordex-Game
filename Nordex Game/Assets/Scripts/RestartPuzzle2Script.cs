using UnityEngine;

public class RestartPuzzle2Script : MonoBehaviour
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
            CubeControllerScript[] cubeControllers = parentGameObject.GetComponentsInChildren<CubeControllerScript>();

            foreach (CubeControllerScript cubeController in cubeControllers)
            {
                cubeController.ResetCube();
            }

            ScaleObjectSmoothlyXScript[] scaleObjectsX = parentGameObject.GetComponentsInChildren<ScaleObjectSmoothlyXScript>();

            foreach (ScaleObjectSmoothlyXScript scaleObjectX in scaleObjectsX)
            {
                scaleObjectX.StopAllCoroutines();
                scaleObjectX.ResetToOriginalScale(); 
                scaleObjectX.StartCoroutine(scaleObjectX.ScaleAfterDelay());
            }

            ScaleObjectSmoothlyYScript[] scaleObjectsY = parentGameObject.GetComponentsInChildren<ScaleObjectSmoothlyYScript>();

            foreach (ScaleObjectSmoothlyYScript scaleObjectY in scaleObjectsY)
            {
                scaleObjectY.StopAllCoroutines();
                scaleObjectY.ResetToOriginalScale();
                scaleObjectY.StartCoroutine(scaleObjectY.ScaleAfterDelay());
            }
        }
    }
}