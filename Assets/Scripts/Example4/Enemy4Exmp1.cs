using UnityEngine;

public class Enemy4Exmp : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] private int currentWaypoint;
    [SerializeField] private Transform[] waypoints;

    [SerializeField] bool loop;

    private void Update()
    {
        Move();

        transform.LookAt(waypoints[currentWaypoint]);
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) >= stoppingDistance)
        {
            // pos moving towards = (1 -               -1)                        change magnitude to only be 1
            Vector3 direction = (transform.position - waypoints[currentWaypoint].position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, movementSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Enemy Has Reached Waypoint");
            int tempWaypointCount = currentWaypoint;
            tempWaypointCount++;
            if (currentWaypoint == waypoints.Length - 1)
            {
                if (loop)
                {
                    currentWaypoint = 0;
                }
                else
                {
                    Debug.Log("Kill enemy");
                    Destroy(gameObject);
                }
                //currentWaypoint = 0;
            }
            else
            {
                tempWaypointCount = Mathf.Clamp(tempWaypointCount, 0, waypoints.Length - 1);
                currentWaypoint = tempWaypointCount;
            }
        }
    }
}
