using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Fire();
    void DismissFire();
    void Damage();
    void EnableLockSprite(bool enabled);
    GameObject GameObject {get;}
}
