using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [SerializeField] bool canAttack;

    public void CreateClone(Transform clone)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(clone, cloneDuration, canAttack);
    }
}
