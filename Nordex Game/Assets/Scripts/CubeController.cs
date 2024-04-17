using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // Reference to the particle system prefab
    private bool isDragging = false;
    private bool canDrag = true; // Variable to track whether the cube can be dragged
    private Vector3 initialPosition; // Store initial position before dragging
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>(); // List to store instantiated particle systems

    private void Start()
    {
        initialPosition = transform.position; // Store the initial position
    }

    private void Update()
    {
        // Check if the cube can be dragged
        if (!canDrag)
        {
            // If not, wait for a short delay before allowing dragging again
            StartCoroutine(EnableDraggingAfterDelay(0.1f)); // Adjust the delay time as needed
        }
    }

    private System.Collections.IEnumerator EnableDraggingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDrag = true; // Allow dragging again after the delay
    }

    private void OnMouseDown()
    {
        if (canDrag)
        {
            isDragging = true;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition();
            newPosition.z = transform.position.z; // Keep the z-position unchanged

            // Check if the cube can cross particle systems of different colors
            bool canCrossParticleSystems = true;
            if (gameObject.CompareTag("Red"))
            {
                canCrossParticleSystems = !IsOverlappingWithParticleSystems("YellowCubeParticles") &&
                                          !IsOverlappingWithParticleSystems("BlueCubeParticles");
            }
            else if (gameObject.CompareTag("Yellow"))
            {
                canCrossParticleSystems = !IsOverlappingWithParticleSystems("RedCubeParticles") &&
                                          !IsOverlappingWithParticleSystems("BlueCubeParticles");
            }
            else if (gameObject.CompareTag("Blue"))
            {
                canCrossParticleSystems = !IsOverlappingWithParticleSystems("RedCubeParticles") &&
                                          !IsOverlappingWithParticleSystems("YellowCubeParticles");
            }

            if (canCrossParticleSystems)
            {
                // Instantiate particles at the current position
                ParticleSystem particleSystemInstance = Instantiate(particleSystemPrefab, newPosition, Quaternion.identity);
                particleSystems.Add(particleSystemInstance);

                transform.position = newPosition;

                // Check for overlapping blockers while dragging
                if (IsOverlappingWithBlocker())
                {
                    // Return to initial position if overlapping with a blocker
                    transform.position = initialPosition;
                    isDragging = false; // Reset dragging state
                    ClearParticleSystems(); // Clear the particle systems
                    return;
                }
            }
            else
            {
                // Cube cannot cross particle systems of different colors, return to initial position
                transform.position = initialPosition;
                isDragging = false; // Reset dragging state
                ClearParticleSystems(); // Clear the particle systems
                return;
            }
        }
        else if (Input.GetMouseButtonDown(0)) // Check if mouse button is pressed
        {
            // Check if the cube is in its initial position before allowing dragging
            if (transform.position == initialPosition)
            {
                isDragging = true; // Allow dragging if the cube is in its initial position
            }
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        Debug.Log("Mouse released. Checking for overlaps...");

        // Check for overlapping cubes and snap if possible
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(gameObject.tag);
        foreach (GameObject cube in cubes)
        {
            if (cube != gameObject && IsOverlapping(cube))
            {
                // Snap to the overlapping cube's position
                transform.position = cube.transform.position;
                // Don't clear particle systems when snapping to another cube
                return;
            }
        }

        // If no overlapping cube found, return to initial position
        transform.position = initialPosition;
        ClearParticleSystems(); // Clear the particle systems
    }

    private void ClearParticleSystems()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            Destroy(ps.gameObject); // Destroy each particle system instance
        }
        particleSystems.Clear(); // Clear the list
    }

    private Vector3 GetMouseWorldPosition()
    {
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
                Debug.LogError("No collider hit by raycast.");
                return transform.position; // Return current position if no collider is hit
            }
        }
        else
        {
            Debug.LogError("First person camera not found.");
            return Vector3.zero;
        }
    }

    private bool IsOverlapping(GameObject otherCube)
    {
        Bounds bounds1 = GetComponent<Renderer>().bounds;
        Bounds bounds2 = otherCube.GetComponent<Renderer>().bounds;
        return bounds1.Intersects(bounds2);
    }

    private bool IsOverlappingWithParticleSystems(string particleSystemTag)
    {
        GameObject[] particleSystems = GameObject.FindGameObjectsWithTag(particleSystemTag);
        foreach (GameObject ps in particleSystems)
        {
            Bounds bounds1 = GetComponent<Renderer>().bounds;
            Bounds bounds2 = ps.GetComponent<Renderer>().bounds;
            if (bounds1.Intersects(bounds2))
            {
                Debug.Log("Overlap detected with particle system: " + ps.name);
                return true; // Cube is overlapping with a particle system of the specified color
            }
        }
        return false; // Cube is not overlapping with any particle system of the specified color
    }

    private bool IsOverlappingWithBlocker()
    {
        Debug.Log("Checking overlap with blockers...");
        GameObject[] blockers = GameObject.FindGameObjectsWithTag("Blocker");
        foreach (GameObject blocker in blockers)
        {
            Bounds bounds1 = GetComponent<Renderer>().bounds;
            Bounds bounds2 = blocker.GetComponent<Renderer>().bounds;
            if (bounds1.Intersects(bounds2))
            {
                Debug.Log("Overlap detected with blocker: " + blocker.name);
                return true; // Cube is overlapping with a blocker
            }
        }
        Debug.Log("No overlap with blockers.");
        return false; // Cube is not overlapping with any blocker
    }
}
