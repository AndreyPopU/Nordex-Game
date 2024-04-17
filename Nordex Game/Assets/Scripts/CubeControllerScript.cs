using System.Collections.Generic;
using UnityEngine;

public class CubeControllerScript : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;

    private bool isDragging = false;
    private bool canDrag = true;

    private Vector3 initialPosition;

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private void Start()
    {
        // Store the initial position of the cube
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (!canDrag)
        {
            // Coroutine used to enable dragging after a delay
            StartCoroutine(EnableDraggingAfterDelay(0.1f));
        }
    }

    // Coroutine to enable dragging after a delay
    private System.Collections.IEnumerator EnableDraggingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDrag = true;
    }

    private void OnMouseDown()
    {
        // Check if dragging is allowed
        if (canDrag)
        {
            isDragging = true;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            // Get the new position of the cube based on mouse position
            Vector3 newPosition = GetMouseWorldPosition();
            newPosition.z = transform.position.z;

            // Check if the cube can cross other particle systems or cubes based on its tag
            bool canCrossParticleSystemsOrOtherCubes = true;
            if (gameObject.CompareTag("Red"))
            {
                // Check overlapping with yellow and blue and green cubes and their particle systems
                canCrossParticleSystemsOrOtherCubes = !IsOverlappingWithParticleSystemsOrOtherCubes("YellowCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("BlueCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Yellow") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Blue") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("GreenCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Green");
            }
            else if (gameObject.CompareTag("Yellow"))
            {
                // Check overlapping with red and blue and green cubes and their particle systems
                canCrossParticleSystemsOrOtherCubes = !IsOverlappingWithParticleSystemsOrOtherCubes("RedCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("BlueCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Red") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Blue") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("GreenCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Green");
            }
            else if (gameObject.CompareTag("Blue"))
            {
                // Check overlapping with red and yellow and green cubes and their particle systems
                canCrossParticleSystemsOrOtherCubes = !IsOverlappingWithParticleSystemsOrOtherCubes("RedCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("YellowCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Red") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Yellow") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("GreenCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Green");
            }
            else if (gameObject.CompareTag("Green"))
            {
                // Check overlapping with red and blue and yellow cubes and their particle systems
                canCrossParticleSystemsOrOtherCubes = !IsOverlappingWithParticleSystemsOrOtherCubes("RedCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("BlueCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Red") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Blue") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("YellowCubeParticles") &&
                                          !IsOverlappingWithParticleSystemsOrOtherCubes("Yellow");
            }

            // If cube can cross, instantiate a particle system at the new position
            if (canCrossParticleSystemsOrOtherCubes)
            {
                ParticleSystem particleSystemInstance = Instantiate(particleSystemPrefab, newPosition, Quaternion.identity);
                particleSystems.Add(particleSystemInstance);

                // Move the cube to the new position
                transform.position = newPosition;

                // If overlapping with a blocker, reset cube position and clear particle systems
                if (IsOverlappingWithBlocker())
                {
                    transform.position = initialPosition;
                    isDragging = false;
                    ClearParticleSystems();
                    return;
                }
            }
            else
            {
                // If cube cannot cross, reset cube position and clear particle systems
                transform.position = initialPosition;
                isDragging = false;
                ClearParticleSystems();
                return;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // If mouse is clicked and cube is at initial position, start dragging
            if (transform.position == initialPosition)
            {
                isDragging = true;
            }
        }
    }

    private void OnMouseUp()
    {
        // Dragging ends
        isDragging = false;

        // Find all cubes with the same tag
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(gameObject.tag);
        foreach (GameObject cube in cubes)
        {
            // If overlapping with another cube, snap to its position
            if (cube != gameObject && IsOverlapping(cube))
            {
                transform.position = cube.transform.position;

                return;
            }
        }

        // If not overlapping with any other cube, snap back to initial position and clear particle systems
        transform.position = initialPosition;
        ClearParticleSystems();
    }

    private void ClearParticleSystems()
    {
        // Destroy all particle systems
        foreach (ParticleSystem ps in particleSystems)
        {
            Destroy(ps.gameObject);
        }
        particleSystems.Clear();
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in the world
        Vector3 mousePosition = Input.mousePosition;
        Camera firstPersonCamera = GameObject.FindGameObjectWithTag("FirstPersonCamera").GetComponent<Camera>();
        if (firstPersonCamera != null)
        {
            Ray ray = firstPersonCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }
            else
            {
                return transform.position;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }

    private bool IsOverlapping(GameObject otherCube)
    {
        // Check if this cube is overlapping with another cube
        Bounds bounds1 = GetComponent<Renderer>().bounds;
        Bounds bounds2 = otherCube.GetComponent<Renderer>().bounds;
        return bounds1.Intersects(bounds2);
    }

    private bool IsOverlappingWithParticleSystemsOrOtherCubes(string particleSystemTag)
    {
        // Check if this cube is overlapping with any particle systems or cubes with a specific tag
        GameObject[] particleSystems = GameObject.FindGameObjectsWithTag(particleSystemTag);
        foreach (GameObject ps in particleSystems)
        {
            Bounds bounds1 = GetComponent<Renderer>().bounds;
            Bounds bounds2 = ps.GetComponent<Renderer>().bounds;
            if (bounds1.Intersects(bounds2))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsOverlappingWithBlocker()
    {
        // Check if this cube is overlapping with any blockers
        GameObject[] blockers = GameObject.FindGameObjectsWithTag("Blocker");
        foreach (GameObject blocker in blockers)
        {
            Bounds bounds1 = GetComponent<Renderer>().bounds;
            Bounds bounds2 = blocker.GetComponent<Renderer>().bounds;
            if (bounds1.Intersects(bounds2))
            {
                return true;
            }
        }
        return false;
    }

    // Method for other CS
    public void ResetCube()
    {
        // Reset cube to initial position
        transform.position = initialPosition;

        // Clear particle systems
        ClearParticleSystems();
    }
}