using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine _stateMachine { get; private set; }

    public PlayerIdleState _idleState { get; private set;}
    public PlayerMoveState _moveState { get; private set;}

    void Awake()
    {
        _stateMachine = new PlayerStateMachine();

        _idleState = new PlayerIdleState(this, _stateMachine, "Idle");
        _moveState = new PlayerMoveState(this, _stateMachine, "Move");
    }

    void Start()
    {
        _stateMachine.Initialize(_idleState);
    }

    void Update()
    {
        _stateMachine.currentState.Update();
    }
}
