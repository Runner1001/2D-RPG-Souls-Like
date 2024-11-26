using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    public DashSkill Dash { get; private set; }
    public CloneSkill Clone { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Dash = GetComponent<DashSkill>();
        Clone = GetComponent<CloneSkill>();
    }
}
