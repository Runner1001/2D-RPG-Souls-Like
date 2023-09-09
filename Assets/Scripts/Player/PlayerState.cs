using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;
    protected Rigidbody2D _rb;
    protected float _horizontal;
    protected float _vertical;
    protected float _stateTimer;
    protected bool _triggerCalled;

    string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, string animBoolName)
    {
        _player = player;
        _stateMachine = playerStateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        _player.Anim.SetBool(_animBoolName, true);
        _rb = _player.RB;
        _triggerCalled = false;
    }

    public virtual void Update()
    {
        _stateTimer -= Time.deltaTime;
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _player.Anim.SetFloat("yVelocity", _rb.velocity.y);
    }

    public virtual void Exit()
    {
        _player.Anim.SetBool(_animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        _triggerCalled = true;
    }
}
