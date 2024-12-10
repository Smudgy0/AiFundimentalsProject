using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Example2Enemy : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] private Transform targetTransform;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.LookAt(targetTransform.position);
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
