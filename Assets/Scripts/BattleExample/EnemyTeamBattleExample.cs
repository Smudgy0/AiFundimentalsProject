using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTeamBattleExample : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] public Transform targetTransform;

    [SerializeField] public int HP;

    public GameObject targetUnit;

    Rigidbody rb;

    public bool Team1 = false;
    public bool Team2 = false;

    float distance;

    private void Awake()
    {
        if (tag == "Team1")
        {
            Team1 = true;
        }
        else
        {
            Team2 = true;
        }
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        FindObjects();
        transform.LookAt(targetUnit.transform);
    }

    private void FindObjects()
    {
        if (Team1 == true)
        {
            targetUnit = GameObject.FindWithTag("Team2");
        }
        if (Team2 == true)
        {
            targetUnit = GameObject.FindWithTag("Team1");
        }

        targetTransform = targetUnit.transform;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (targetTransform == null)
        {
            return;
        }
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

    private void OnTriggerEnter(Collider target)
    {
        if (this.tag == "Team1" && target.tag == "Team2")
        {
            HP = HP - 1;
            if (HP <= 0)
            {
                Destroy(target.gameObject);
            }
        }

        if (this.tag == "Team2" && target.tag == "Team1")
        {
            HP = HP - 1;
            if (HP <= 0)
            {
                Destroy(target.gameObject);
            }
        }
    }
}
