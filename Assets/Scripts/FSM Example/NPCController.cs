using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCController : MonoBehaviour
{
    private enum State
    {
        Sleep,
        Idle,
        Walking,
        WalkingToWork,
        Working,
        GoingToBar,
        drinking,
        WalkingHome
    }

    [SerializeField] private State currentState = State.Sleep;
    public float productsMade = 0;

    [SerializeField] Transform bedTransform;
    [SerializeField] Transform workTransform;
    [SerializeField] Transform barTransform;

    bool deactiveBool = true;

    NPCUI UIM;

    bool startedWorking;
    bool startedConsuming;

    //local references
    NavMeshAgent agent;
    Animator animator;

    //external references
    TimeController timeController;

    private void Awake()
    {
        agent = FindAnyObjectByType<NavMeshAgent>();
        timeController = FindAnyObjectByType<TimeController>();
        animator = FindAnyObjectByType<Animator>();
        UIM = FindAnyObjectByType<NPCUI>();
    }

    void Update()
    {
        switch (timeController.hours)
        {
            case 7:
                currentState = State.WalkingToWork;
                break;
            case 19:
                currentState = State.GoingToBar;
                break;
            case 23:
                currentState = State.WalkingHome;
                break;
        }

        switch (currentState)
        {
            case State.Sleep:
                transform.position = bedTransform.position;
                transform.rotation = bedTransform.rotation;
                break;
            case State.Idle:
                transform.rotation = Quaternion.identity;
                break;
            case State.Walking:
                break;
            case State.WalkingToWork:
                agent.destination = workTransform.position;
                if (Vector3.Distance(transform.position, workTransform.position) < 2)
                    {
                    currentState = State.Working;
                }
                break;
            case State.Working:
                if(!startedWorking)
                {
                    startedWorking = true;
                    InvokeRepeating("GoodsProduction", 2, 2);
                    UIM.ActiveWorkshop();
                }
                break;
            case State.GoingToBar:
                if (deactiveBool == true)
                {
                    UIM.InActiveWorkshop();
                    deactiveBool = false;
                }
                startedWorking = false;
                CancelInvoke();
                agent.destination = barTransform.position;
                if (Vector3.Distance(transform.position, barTransform.position) < 2)
                {
                    currentState = State.drinking;
                }
                break;
            case State.drinking:
                deactiveBool = true;
                if (!startedConsuming)
                {
                    startedConsuming = true;
                    InvokeRepeating("GoodsConsumed", 2, 4);
                }
                break;
            case State.WalkingHome:
                startedConsuming = false;
                CancelInvoke();
                agent.destination = bedTransform.position;
                if (Vector3.Distance(transform.position, bedTransform.position) < 2)
                    {
                        currentState = State.Sleep;
                    }
                break;
        }

        switch(currentState)
        {
            case State.WalkingToWork:
            case State.WalkingHome:
            case State.GoingToBar:
                animator.SetFloat("Movement", 1);
                break;
            default:
                animator.SetFloat("Movement", 0);
                break;
        }
    }

    void GoodsProduction()
    {
        UIM.GoodsMade();
    }

    void GoodsConsumed()
    {
        UIM.GoodLost();
    }
}
