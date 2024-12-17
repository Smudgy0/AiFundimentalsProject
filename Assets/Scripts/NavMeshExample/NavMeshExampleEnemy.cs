using UnityEngine;
using UnityEngine.AI;

public class NavMeshExampleEnemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool detected;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detected)
        {
            agent.destination = target.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detected = false;
        }
    }
}
