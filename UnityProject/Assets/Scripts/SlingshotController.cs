using UnityEngine;

public class SlingshotController : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    public Transform launchPoint;
    public float forceMultiplier = 10f;

    private Rigidbody currentProjectile;
    private Vector3 dragStartPoint;
    private bool isDragging;

    void Update()
    {
        // When mouse button pressed, instantiate projectile and start drag
        if (Input.GetMouseButtonDown(0))
        {
            currentProjectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
            dragStartPoint = GetMouseWorldPosition();
            isDragging = true;
        }

        // While dragging, update pull vector and launch on release
        if (isDragging)
        {
            Vector3 currentPoint = GetMouseWorldPosition();
            Vector3 pullVector = dragStartPoint - currentPoint;

            if (Input.GetMouseButtonUp(0))
            {
                Launch(pullVector);
                isDragging = false;
            }
        }
    }

    // Convert mouse screen position to world space on plane at launch height
    Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, launchPoint.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return launchPoint.position;
    }

    // Apply force to the projectile and clear current projectile
    void Launch(Vector3 pullVector)
    {
        currentProjectile.isKinematic = false;
        currentProjectile.AddForce(pullVector * forceMultiplier, ForceMode.Impulse);
        currentProjectile = null;
    }
}
