using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{
    NavMeshAgent Agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.SetDestination(transform.position + Vector3.forward * 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
