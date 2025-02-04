using UnityEngine;

public class Enemy1Exmp : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance; // stopping distance is used to prevent the enemy from pushing the player too much

    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        Move(); // this function called in update "move" will run as the enemy looks at the player.

        transform.LookAt(targetTransform.position); // Its target transform is set as the player and in the update function it will allways look at the player object
    }

    void Move()
    {
        /* this if statement checks if the enemy's postion in comparisons to the player is greater 
           than the stopping distance to prevent the enemy from moving too close to the player.*/
        if (Vector3.Distance(transform.position, targetTransform.position) >= stoppingDistance)
        {
            // pos moving towards = (1 -               -1)                        change magnitude to only be 1
            Vector3 direction = (transform.position - targetTransform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, movementSpeed * Time.deltaTime);
            // this calculation will move the enemy object towards the direction its facing at the speed set in the value "movementSpeed"
        }
        else
        {
            Debug.Log("Player Killed");
        }
    }
}
