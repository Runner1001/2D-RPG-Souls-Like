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

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.WallSlideState);

        if (player.IsGrounded())
            stateMachine.ChangeState(player.IdleState);

        if (horizontal != 0)
            player.SetVelocity(player.MoveSpeed * .8f * horizontal, rb.velocity.y);
    }
}
