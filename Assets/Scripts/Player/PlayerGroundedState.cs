using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.PrimaryAttackState);

        if (!player.IsGrounded())
            stateMachine.ChangeState(player.AirState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded())
            stateMachine.ChangeState(player.JumpState);
    }
}
