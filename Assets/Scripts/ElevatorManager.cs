using System;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class FloorRequest
{
    public int floor;
    public int direction;

    public FloorRequest(int floor, int direction)
    {
        this.floor = floor;
        this.direction = direction;
    }
}

public class ElevatorManager : MonoBehaviour
{

    public static ElevatorManager Instance;
    public Elevator elevatorA;
    public Elevator elevatorB;

    public float floorHeight = 3.2f;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void RequestElevator(int floorNumber, int direction)
    {
        FloorRequest req = new FloorRequest(floorNumber, direction);
        Elevator chosenElevator = ChooseElevator(req);

        chosenElevator.Request(req.floor);
        Debug.Log($"Elevator requested to floor {floorNumber} going {(direction == 1 ? "up" : "down")} by {chosenElevator.name}");
    }
    public Elevator ChooseElevator(FloorRequest request)
    {

        float scoreA = CalculateScore(elevatorA, request);
        float scoreB = CalculateScore(elevatorB, request);

        return scoreA <= scoreB ? elevatorA : elevatorB;
    }

    float CalculateScore(Elevator elevator, FloorRequest request)
    {
        float score = Math.Abs(elevator.currentFloor - request.floor);
        float directionFactor = (elevator.currentFloor < request.floor) ? 1 : -1;
        return score + directionFactor;
    }

}
