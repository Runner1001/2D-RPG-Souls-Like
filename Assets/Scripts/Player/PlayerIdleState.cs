using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_horizontal == _player.FacingDir && _player.IsWallDetected())
            return;

        if (_horizontal != 0 && !_player.IsBusy)
            _stateMachine.ChangeState(_player.MoveState);
    }
}
