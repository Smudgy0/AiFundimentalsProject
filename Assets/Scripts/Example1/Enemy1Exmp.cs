using UnityEngine;

public class Enemy1Exmp : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        if(Vector3.Distance(transform.position, targetTransform.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, movementSpeed);
        }

        transform.LookAt(targetTransform.position);
    }
}
