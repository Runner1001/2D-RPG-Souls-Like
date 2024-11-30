using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{

    [Header("Bounce Info")]
    float bounceSpeed;
    bool isBouncing;
    int bounceAmount;
    List<Transform> enemyTarget = new List<Transform>();
    private int targetIndex;

    [Header("Pierce Info")]
    float pierceAmount;

    [Header("Spin Info")]
    float maxTravelDistance;
    float spinDuration;
    float spinTimer;
    bool wasStopped;
    bool isSpinning;

    Animator anim;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    Player player;
    bool canRotate = true;
    bool isReturning;
    float returnSpeed;

    float hitTimer;
    float hitCooldown;

    float spinDirection;
    float freezeTimeDuration;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 2f)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
            return;

        if (other.GetComponent<Enemy>() != null)
        {
            var enemy = other.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetForBounce(other);

        StuckInto(other);
    }

    public void SetupSword(Vector2 direction, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)
    {
        this.player = player;
        rb.velocity = direction;
        rb.gravityScale = gravityScale;
        this.freezeTimeDuration = freezeTimeDuration;
        this.returnSpeed = returnSpeed;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7f);
    }

    public void SetupBounce(bool isBouncing, int amountOfBounce, float bounceSpeed)
    {
        this.isBouncing = isBouncing;
        this.bounceAmount = amountOfBounce;
        this.bounceSpeed = bounceSpeed;
    }

    public void SetupPierce(float pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    var colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var collider in colliders)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                            SwordSkillDamage(collider.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
    }

    private void SetupTargetForBounce(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var collider in colliders)
                {
                    if (collider.GetComponent<Enemy>() != null)
                        enemyTarget.Add(collider.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D other)
    {
        if(pierceAmount > 0 && other.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        circleCollider.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
