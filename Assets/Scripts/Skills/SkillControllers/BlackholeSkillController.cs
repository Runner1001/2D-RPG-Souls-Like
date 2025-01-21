using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] GameObject hotKeyPrefab;
    [SerializeField] List<KeyCode> keyCodeList;

    int amountOfAttacks = 4;
    float maxSize;
    float growSpeed;
    float shrinkSpeed;
    float cloneAttackTimer;
    float blackholeTimer;
    float cloneAttackCooldown = 0.3f;

    bool canGrow = true;
    bool canShrink;
    bool cloneAttackReleased;
    bool canCreateHotKeys = true;
    bool playerCanDisappear = true;

    List<Transform> targets = new List<Transform>();
    List<GameObject> createdHotKeys = new List<GameObject>();

    public bool PlayerCanExitState { get; private set; }

    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if(targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1f, -1f), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0f)
                Destroy(gameObject);
        }
    }

    public void SetupBlackhole(float maxSize, float growSpeed, float shrinkSpeed, int amountOfAttacks, float cloneAttackCooldown, float blackholeDuration)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.blackholeTimer = blackholeDuration;
    }

    private void ReleaseCloneAttack()
    {
        if(targets.Count <= 0)
            return;

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.Instance.Player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset = Random.Range(0, 100) > 50 ? 2 : -2;

            SkillManager.Instance.Clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0f, 0f));

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", 0.5f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        PlayerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKeys()
    {
        if (createdHotKeys.Count <= 0)
            return;

        foreach (var key in createdHotKeys)
        {
            Destroy(key);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.FreezeTime(true);

            CreateHotKey(enemy);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        { 
            enemy.FreezeTime(false); 
        }
    }

    private void CreateHotKey(Enemy enemy)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.Log("Not enough keycode in the list");
            return;
        }

        if (!canCreateHotKeys)
            return;

        var newHotKey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        createdHotKeys.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackholeHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackholeHotKeyController>();
        newHotKeyScript.SetupHotKey(choosenKey, enemy.transform, this);
    }

    public void AddEnemyToList(Transform enemy)
    {
        targets.Add(enemy);
    }
}
