using UnityEngine;

public class Units : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] public Transform targetTransform;

    [SerializeField] public int HPMult;
    [SerializeField] public int DamageTaken;

    public GameObject targetUnit; // which enemy unit transform its targeting

    Rigidbody rb; // the rigidbody value of the unit
    Material MT; // the colored material which visually indicates the team its on

    public bool Team1 = false; // in the code the object's bool "Team1" will be true if the objects tag is "Team1"
    public bool Team2 = false; // in the code the object's bool "Team2" will be true if the objects tag is "Team2"

    float distance;

    bool takingDamage = false; // checks if the enemy is taking

    int HP;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        MT = GetComponent<Material>();

        int RandomHP = Random.Range(20,31); // a random number to decide the hp of the unit between 20 and 31 which will be times by the "HPMult" value of the object
        HP = RandomHP * HPMult; // the "HPMult" is 10 for infantry and 20 for tanks

        // checks the tag of the prefab object before spawning to desiginate its team
        if (tag == "Team1")
        {
            Team1 = true;
        }
        else
        {
            Team2 = true;
        }
    }

    private void Update()
    {
        FindObjects(); // this function is where the ai will find a enemy troops and follow it to start attacking
        transform.LookAt(targetUnit.transform); // the ai will look at the enemy it is attacking

        if (takingDamage == true) // if the "takingDamage" value is true the ai will lose HP and if it reaches 0 it will be destroyed
        {
            HP -= DamageTaken;
            if (HP <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void FindObjects()
    {
        if (Team1 == true)
        {
            targetUnit = GameObject.FindWithTag("Team2"); // after checking the ai is in team one it will search for an ai with the tag "Team2" and start attacking it
        }
        else if (Team2 == true)
        {
            targetUnit = GameObject.FindWithTag("Team1"); // after checking the ai is in team two it will search for an ai with the tag "Team1" and start attacking it
        }
        else
        {
            return;
        }
        targetTransform = targetUnit.transform; // it will use the transform to track the enemy ai.
    }

    private void FixedUpdate()
    {
        Move(); // to keep the movement of the ai's the same across framerate the movement function has been put into fixed update
    }

    void Move()
    {
        if (targetTransform == null) // when moving it will first check if it even has a target and if not it won't move until it finds one
        {
            return;
        }
        if (Vector3.Distance(transform.position, targetTransform.position) >= stoppingDistance) // if it has found a target it will move towards it and get close enough to do damage
        {
            // pos moving towards = (1 -               -1)                        change magnitude to only be 1
            Vector3 direction = (targetTransform.position - transform.position).normalized;
            direction *= movementSpeed;
            rb.linearVelocity = direction;
        }
        else // if it has gotten close enough to its target the ai will stop
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        if (this.tag == "Team1" && target.tag == "Team2") // if the ai collides with a ai of the opposing team it wil start losing HP.
        {
            takingDamage = true;
        }

        else if (this.tag == "Team2" && target.tag == "Team1") // if the ai collides with a ai of the opposing team it wil start losing HP.
        {
            takingDamage = true;
        }

        else // if its not in contact with an enemy ai it will not take any damage
        {
            takingDamage = false;
        }
    }
}
