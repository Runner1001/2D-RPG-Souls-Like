using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_player.IsWallDetected())
            _stateMachine.ChangeState(_player.WallSlideState);

        if (_player.IsGrounded())
            _stateMachine.ChangeState(_player.IdleState);

        if (_horizontal != 0)
            _player.SetVelocity(_player.MoveSpeed * .8f * _horizontal, _rb.velocity.y);
    }
}
