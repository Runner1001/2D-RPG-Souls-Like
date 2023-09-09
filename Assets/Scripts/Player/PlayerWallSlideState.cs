using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.ChangeState(_player.WallJumpState);
            return;
        }

        if (_horizontal != 0 && _player.FacingDir != _horizontal)
            _stateMachine.ChangeState(_player.IdleState);

        if(_vertical < 0)
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        else
            _rb.velocity = new Vector2(0, _rb.velocity.y * .7f);

        if (_player.IsGrounded())
            _stateMachine.ChangeState(_player.IdleState);
    }
}
