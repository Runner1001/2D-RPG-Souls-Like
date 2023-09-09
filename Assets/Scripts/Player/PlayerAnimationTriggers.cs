using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    Player _player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        _player.AnimationTrigger();
    }
}
