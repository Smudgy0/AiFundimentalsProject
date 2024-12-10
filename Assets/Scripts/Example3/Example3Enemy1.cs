using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Example3Enemy : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float arrivalDistance;

    [SerializeField] Vector3 randomPosition;

    [SerializeField] Transform floorSize;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GetRandomPosition();
    }

    private void Update()
    {
        transform.LookAt(randomPosition);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, randomPosition) >= arrivalDistance)
        {
            // pos moving towards = (1 -               -1)                        change magnitude to only be 1
            Vector3 direction = (randomPosition - transform.position).normalized;
            direction *= movementSpeed;
            rb.linearVelocity = direction;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            GetRandomPosition();
        }
    }

    void GetRandomPosition()
    {
        float xPosition = Random.Range(-floorSize.localScale.x / 2, floorSize.localScale.x / 2);
        float zPosition = Random.Range(-floorSize.localScale.z / 2, floorSize.localScale.z / 2);
        randomPosition = floorSize.position + new Vector3(xPosition, transform.position.y, zPosition);
    }
}
