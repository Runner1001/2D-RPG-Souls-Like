using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    [SerializeField] SwordType swordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] float bounceSpeed = 20;
    [SerializeField] int bounceAmount;
    [SerializeField] float bounceGravity;

    [Header("Pierce Info")]
    [SerializeField] int pierceAmount;
    [SerializeField] float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] float hitCooldown = 0.35f;
    [SerializeField] float maxTravelDistance = 7f;
    [SerializeField] float spinDuration = 2f;
    [SerializeField] float spinGravity = 1f;

    [Header("Skill Info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 lauchForce;
    [SerializeField] float swordGravity;
    [SerializeField] float freezeTimeDuration = 0.7f;
    [SerializeField] float returnSpeed = 12f;

    [Header("Aim Dots")]
    [SerializeField] int numberOfDots;
    [SerializeField] float spaceBetweenDots;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] Transform dotsParent;

    Vector2 finalDirection;
    GameObject[] allDots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
        SetupGravity();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDirection = new Vector2(AimDirection().normalized.x * lauchForce.x, AimDirection().normalized.y * lauchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < allDots.Length; i++)
            {
                allDots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        var swordSkillController = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
            swordSkillController.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            swordSkillController.SetupPierce(pierceAmount);
        else if( swordType == SwordType.Spin)
            swordSkillController.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        swordSkillController.SetupSword(finalDirection, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    private void GenerateDots()
    {
        allDots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            allDots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            allDots[i].SetActive(false);
        }
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < allDots.Length; i++)
        {
            allDots[i].SetActive(isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * lauchForce.x,
            AimDirection().normalized.y * lauchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
}
