using UnityEngine;

public class Enemy4Exmp : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] private int currentWaypoint;
    [SerializeField] private Transform[] waypoints; // this is an array of the waypoints postion

    [SerializeField] bool loop; // this is a true or false statement deciding if the ai will loop through its waypoints or it will stop once it reaches its destination

    private void Update()
    {
        Move(); // this runs a siliar movement function as the following ai with a few differences

        transform.LookAt(waypoints[currentWaypoint]); // this is similar to the following enemy but instead of looking at a player its looking at the waypoint it is going too
    }

    void Move()
    {
        /* this if statement compared to the following ai checks if the distance the enemy is to the waypoint is greater than the stopping distance
        it will continue on, otherwise it will switch the waypoint to the next in the array and if its reached the last of its waypoints it will check the loop
        bool to see if its true or false. if its true then the ai enemy will head to the first wapoint in its array and if its false it will destroy the game object.*/
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) >= stoppingDistance)
        {
            // pos moving towards = (1 -               -1)                        change magnitude to only be 1
            Vector3 direction = (transform.position - waypoints[currentWaypoint].position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, movementSpeed * Time.deltaTime);
        }
        else // this else runs if the ai has detected that its reached its waypoint
        {
            Debug.Log("Enemy Has Reached Waypoint");
            int tempWaypointCount = currentWaypoint;
            tempWaypointCount++;
            if (currentWaypoint == waypoints.Length - 1) // the ai checks if its reached its last waypoint
            {
                // if true it will check the "loop" bool
                if (loop) // if "loop" is equal to true then it will repeat the patrol
                {
                    currentWaypoint = 0;
                }
                else // otherwise it will destroy the game object
                {
                    Debug.Log("Kill enemy");
                    Destroy(gameObject);
                }
                //currentWaypoint = 0;
            }
            else // if it has not reached its last waypoint it will set its new direction and movement to the next of the waypoints
            {
                tempWaypointCount = Mathf.Clamp(tempWaypointCount, 0, waypoints.Length - 1);
                currentWaypoint = tempWaypointCount;
            }
        }
    }
}
