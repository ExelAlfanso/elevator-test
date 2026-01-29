using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum ElevatorState
{
    Idle,
    MovingUp,
    MovingDown,
    DoorsOpen,
    Arrived,
    DoorsOpening,
    DoorsClosing
}


[Serializable]
public class FloorDoor
{
    public Transform leftDoor;
    public Transform rightDoor;

}
public class Elevator : MonoBehaviour
{
    [Header("Floor Doors")]
    public List<FloorDoor> floorDoors = new List<FloorDoor>();
    [Header("Queues")]
    public List<int> upQueue = new List<int>();
    public List<int> downQueue = new List<int>();

    [Header("Elevator Settings")]
    public ElevatorState currentState = ElevatorState.Idle;
    public Transform elevatorCabin;

    public int currentFloor = 1;
    public int? currentTargetFloor = null;

    public float moveDurationPerFloor = 2f;
    public float holdOpenedDoorTime = 2f;

    Tween moveTween;
    Tween doorHoldTween;

    void Update()
    {
        FSM();
    }
    #region Main Methods
    void FSM()
    {
        switch (currentState)
        {
            case ElevatorState.Idle:
                IdleState();
                break;

            case ElevatorState.Arrived:
                ArrivedState();
                break;

            case ElevatorState.DoorsOpen:
                DoorsOpenState();
                break;
        }
    }

    public void Request(int floor)
    {
        if (floor == currentFloor)
            return;

        if (floor > currentFloor)
        {
            upQueue.Add(floor);
            upQueue.Sort();
        }
        else
        {
            downQueue.Add(floor);
            downQueue.Sort();
            downQueue.Reverse();
        }

        if (currentState == ElevatorState.Idle)
            NextTarget();
    }


    void NextTarget()
    {
        if (upQueue.Count == 0 && downQueue.Count == 0)
        {
            currentState = ElevatorState.Idle;
            return;
        }

        if (upQueue.Count > 0)
        {
            currentTargetFloor = upQueue[0];
            upQueue.RemoveAt(0);
            Move(currentTargetFloor.Value, true);
            return;
        }

        if (downQueue.Count > 0)
        {
            currentTargetFloor = downQueue[0];
            downQueue.RemoveAt(0);
            Move(currentTargetFloor.Value, false);
        }
    }


    void IdleState()
    {
        NextTarget();
    }

    void ArrivedState()
    {
        OpenDoors().Play().OnComplete(() =>
        {
            currentState = ElevatorState.DoorsOpen;
        });
    }

    void DoorsOpenState()
    {
        if (doorHoldTween != null && doorHoldTween.IsActive())
            return;

        doorHoldTween = DOVirtual.DelayedCall(holdOpenedDoorTime, () =>
        {
            CloseDoors().Play().OnComplete(() =>
            {
                currentTargetFloor = null;
                NextTarget();
            });
        });
    }


    void Move(int targetFloor, bool goingUp)
    {
        currentState = goingUp ? ElevatorState.MovingUp : ElevatorState.MovingDown;

        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();

        float targetY = (targetFloor - 1) * ElevatorManager.Instance.floorHeight;
        float distanceFloors = Mathf.Abs(currentFloor - targetFloor);
        float duration = distanceFloors * moveDurationPerFloor;

        moveTween = elevatorCabin.DOMoveY(targetY, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                currentFloor = targetFloor;
                currentState = ElevatorState.Arrived;
            });
    }
    #endregion

    #region Door Animations
    public Sequence OpenDoors()
    {
        if (currentFloor - 1 < 0 || currentFloor - 1 >= floorDoors.Count)
        {
            Debug.LogWarning($"Floor {currentFloor} does not have door configuration!");
            return DOTween.Sequence();
        }

        FloorDoor doorPair = floorDoors[currentFloor - 1];
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => currentState = ElevatorState.DoorsOpening);
        seq.Join(doorPair.leftDoor.DOLocalMoveX(1.7f, 1.5f));
        seq.Join(doorPair.rightDoor.DOLocalMoveX(-1.7f, 1.5f));
        return seq;
    }

    public Sequence CloseDoors()
    {
        if (currentFloor - 1 < 0 || currentFloor - 1 >= floorDoors.Count)
        {
            Debug.LogWarning($"Floor {currentFloor} does not have door configuration!");
            return DOTween.Sequence();
        }

        FloorDoor doorPair = floorDoors[currentFloor - 1];
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => currentState = ElevatorState.DoorsClosing);
        seq.Join(doorPair.leftDoor.DOLocalMoveX(0.23f, 1.5f));
        seq.Join(doorPair.rightDoor.DOLocalMoveX(-0.78f, 1.5f));
        return seq;
    }

    #endregion

    #region Debug Methods

    [ContextMenu("Play Open Doors Animation")]
    public void PlayOpenDoorsAnimation()
    {
        OpenDoors().Play();
    }
    [ContextMenu("Play Close Doors Animation")]
    public void PlayCloseDoorsAnimation()
    {
        CloseDoors().Play();
    }

    #endregion


    #region Utility
    public int QueueSize()
    {
        return upQueue.Count + downQueue.Count;
    }

    #endregion
}

