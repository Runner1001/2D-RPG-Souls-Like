using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    [SerializeField] Vector2[] attackMovement;
    [SerializeField] float counterAttackDuration;


    [Header("Move Setup")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpVelocity = 3f;
    [SerializeField] float swordReturnImpact;

    [Header("Dash Info")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
 
    public float DashDirection { get; private set; }    

    public SkillManager Skill {  get; private set; }
    public GameObject Sword { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackholeState BlackholeState { get; private set; }

    public float MoveSpeed => moveSpeed;
    public float JumpVelocity => jumpVelocity;
    public float DashSpeed => dashSpeed;
    public float DashDuration => dashDuration;
    public Vector2[] AttackMovement => attackMovement;
    public float CounterAttackDuration => counterAttackDuration;
    public float SwordReturnImpact => swordReturnImpact;
    
    public bool IsBusy {  get; private set; }



    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");

        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
        CounterAttackState = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");

        AimSwordState = new PlayerAimSwordState(this, StateMachine, "AimSword");
        CatchSwordState = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        BlackholeState = new PlayerBlackholeState(this, StateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();

        Skill = SkillManager.Instance;

        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();

        CheckForDashInput();
    }

    public void AssignNewSword(GameObject newSword)
    {
        Sword = newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(CatchSwordState);
        Destroy(Sword);
    }

    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;

        yield return new WaitForSeconds(seconds);

        IsBusy = false;
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();





    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.Dash.CanUseSkill())
        {
            DashDirection = Input.GetAxisRaw("Horizontal");

            if (DashDirection == 0)
                DashDirection = FacingDir;

            StateMachine.ChangeState(DashState);
        }
    }
}
