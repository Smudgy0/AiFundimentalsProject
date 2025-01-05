using UnityEngine;

public class TDBallon1Script : MonoBehaviour
{
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private int BalloonHP = 10;

        [SerializeField] private float stoppingDistance;

        [SerializeField] private int currentWaypoint;

        [SerializeField] bool loop;

    public Vector3 moveDelta;

        TDManager TDM;

        private void Awake()
        {
            TDM = FindAnyObjectByType<TDManager>();
        }
        private void Update()
        {
            Move();

            transform.LookAt(TDM.waypoints[currentWaypoint].transform);
        }

        void Move()
        {
            if (Vector3.Distance(transform.position, TDM.waypoints[currentWaypoint].transform.position) >= stoppingDistance)
            {
                // pos moving towards = (1 -               -1)                        change magnitude to only be 1
                Vector3 direction = (transform.position - TDM.waypoints[currentWaypoint].transform.position).normalized;
                moveDelta = direction;
                transform.position = Vector3.MoveTowards(transform.position, TDM.waypoints[currentWaypoint].transform.position, movementSpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Enemy Has Reached Waypoint");
                int tempWaypointCount = currentWaypoint;
                tempWaypointCount++;

                if (currentWaypoint == TDM.waypoints.Length - 1)
                {
                    Debug.Log("final waypoint reached");
                    TDM.ChangeHealth(1);
                    TDM.KillBloon();
                    Destroy(gameObject);
                }
                else
                {
                    tempWaypointCount = Mathf.Clamp(tempWaypointCount, 0, TDM.waypoints.Length - 1);
                    currentWaypoint = tempWaypointCount;
                }
            }
        }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Damageballon"))
        {
            Destroy(other.gameObject);
            BalloonHP = BalloonHP - 10;
            if(BalloonHP <= 0)
            {
                TDM.KillBloon();
                Destroy(gameObject);
            }
        }
    }
}
