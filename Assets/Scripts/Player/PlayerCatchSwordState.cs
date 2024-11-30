using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    Transform sword;

    public PlayerCatchSwordState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.Sword.transform;

        if (player.transform.position.x > sword.position.x && player.FacingDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.FacingDir == -1)
            player.Flip();

        rb.velocity = new Vector2(player.SwordReturnImpact * -player.FacingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
