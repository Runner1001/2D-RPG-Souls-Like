using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotKeyController : MonoBehaviour
{
    [SerializeField] TMP_Text hotKeyText;
    [SerializeField] SpriteRenderer spriteRenderer;

    KeyCode hotKey;
    Transform enemy;
    BlackholeSkillController blackhole;


    private void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            blackhole.AddEnemyToList(enemy);

            hotKeyText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }

    public void SetupHotKey(KeyCode hotKey, Transform enemy, BlackholeSkillController blackhole)
    {
        this.hotKey = hotKey;
        this.enemy = enemy;
        this.blackhole = blackhole;

        hotKeyText.SetText(hotKey.ToString());
    }
}
