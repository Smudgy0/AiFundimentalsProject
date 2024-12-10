using UnityEngine;

public class BattleScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;


    [SerializeField] public Transform targetPosition;
    [SerializeField] private Transform targetTransform;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.LookAt(targetTransform.position);

        if(tag == "Team1")
        {
            if(targetTransform.tag == "Team2")
            {
                Vector3 distance = targetTransform.position - transform.position;
            }
        }

        if (tag == "Team2")
        {
            if (targetTransform.tag == "Team1")
            {
                Vector3 distance = targetTransform.position - transform.position;
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) >= stoppingDistance)
        {
            // pos moving towards = (1 -               -1)                        change magnitude to only be 1
            Vector3 direction = (transform.position - targetTransform.position).normalized;
            direction *= movementSpeed;
            rb.linearVelocity = direction;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            Debug.Log("Player Killed");
        }
    }
}
