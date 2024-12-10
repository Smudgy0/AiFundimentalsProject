using UnityEngine;

public class Enemy1Exmp : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        Move();

        transform.LookAt(targetTransform.position);
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) >= stoppingDistance)
        {
            // pos moving towards = (1 -               -1)                        change magnitude to only be 1
            Vector3 direction = (transform.position - targetTransform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Player Killed");
        }
    }
}
