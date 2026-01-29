using UnityEngine;

public class ElevatorButton : InteractableObject
{
    [SerializeField] public int floorNumber = 1;
    public bool goingUp = true;
    public override void Interacted()
    {
        ElevatorManager.Instance.RequestElevator(floorNumber, goingUp ? 1 : -1);
    }

}
