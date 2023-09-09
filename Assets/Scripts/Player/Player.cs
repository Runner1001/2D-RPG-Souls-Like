using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    [SerializeField] Vector2[] _attackMovement;

    [Header("Move Setup")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpVelocity = 3f;

    [Header("Dash Info")]
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    [SerializeField] float _dashTimer;
    float _dashCooldown;
    public float DashDirection { get; private set; }

    [Header("Check Collisions")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckDistance;
    [SerializeField] Transform _wallCheck;
    [SerializeField] float _wallCheckDistance;
    [SerializeField] LayerMask _groundLayer;

    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }

    public float MoveSpeed => _moveSpeed;
    public float JumpVelocity => _jumpVelocity;
    public float DashSpeed => _dashSpeed;
    public float DashDuration => _dashDuration;
    public Vector2[] AttackMovement => _attackMovement;
    public int FacingDir { get; private set; } = 1;
    public bool IsBusy {  get; private set; }

    bool isFacingRight = true;

    void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");

        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
    }

    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);
    }

    void Update()
    {
        StateMachine.CurrentState.Update();

        CheckForDashInput();
    }

    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;

        yield return new WaitForSeconds(seconds);

        IsBusy = false;
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        RB.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetZeroVelocity() => RB.velocity = Vector2.zero;
    #endregion

    #region Flip
    private void Flip()
    {
        FacingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController(float x)
    {
        if (x > 0 && !isFacingRight)
            Flip();
        else if (x < 0 && isFacingRight)
            Flip();

    }
    #endregion

    #region Collision
    public bool IsGrounded() => Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDir, _wallCheckDistance, _groundLayer);

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        _dashCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashCooldown < 0)
        {
            DashDirection = Input.GetAxisRaw("Horizontal");

            if (DashDirection == 0)
                DashDirection = FacingDir;

            StateMachine.ChangeState(DashState);

            _dashCooldown = _dashTimer;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector3(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));
    }
    #endregion
}
