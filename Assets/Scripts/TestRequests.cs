using UnityEngine;

public class TestRequests : MonoBehaviour
{
    private InputSystem_Actions inputActions;


    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Test.RequestA.performed += ctx => RequestA();
        inputActions.Test.RequestB.performed += ctx => RequestB();
        inputActions.Test.RequestC.performed += ctx => RequestC();
        inputActions.Test.RequestD.performed += ctx => RequestD();
        inputActions.Test.RequestE.performed += ctx => RequestE();

    }


    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Test.RequestA.performed -= ctx => RequestA();
        inputActions.Test.RequestB.performed -= ctx => RequestB();
        inputActions.Test.RequestC.performed -= ctx => RequestC();
        inputActions.Test.RequestD.performed -= ctx => RequestD();
        inputActions.Test.RequestE.performed -= ctx => RequestE();
    }


    void RequestA()
    {
        ElevatorManager.Instance.RequestElevator(1, 1);
    }


    void RequestB()
    {
        ElevatorManager.Instance.RequestElevator(5, -1);
    }


    void RequestC()
    {
        ElevatorManager.Instance.RequestElevator(3, 1);
    }
    void RequestD()
    {
        ElevatorManager.Instance.RequestElevator(2, -1);
    }


    void RequestE()
    {
        ElevatorManager.Instance.RequestElevator(4, 1);
    }
}