using UnityEngine;

public class StateController : MonoBehaviour
{
    public enum State
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

    [SerializeField] State CurrentState;

    public float x;

    private void Start()
    {
        InvokeRepeating("IncrementInt", 0, 3);
    }

    void IncrementInt()
    {
        x++;
    }

    private void Update()
    {
        /*if (x == 3)
        {
            print("X is equal to 3");
        }
        else if (x == 4)
        {
            print("X is equal to 4");
        }
        else
        {
            print("X is not equal to 3 or 4");
        }

        switch (CurrentState)
        {
            case State.Idle:
                print("Idle");
                break;
            case State.Walking:
                print("Walking");
                break;
            case State.FollowingPlayer:
                print("FollowingPlayer");
                break;
            default:
                print("default");
                break;
        }
        */
    }
}
