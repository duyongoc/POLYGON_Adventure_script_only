using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBasicRanger : MonoBehaviour
{


    public void DoAction(ActorBase owner, DataAttackBase attack, Transform attackPoint, Action cbFinish)
    {
        var ranger = attack as DataAttackRanger;
        var lookDir = Quaternion.LookRotation(owner.CurrentEnemy.Position - transform.position);
        var angleStep = -(ranger.angle * (ranger.totalBullet / 2));

        for (var i = 0; i < ranger.totalBullet; i++)
        {
            var rotation = Quaternion.Euler(lookDir.eulerAngles.x, lookDir.eulerAngles.y + angleStep, lookDir.eulerAngles.z);
            var prefab = ranger.prefabShoot.SpawnToGarbage(attackPoint.position, rotation);
            prefab.GetComponent<BulletBase>().Init(owner, ranger);

            // print("shoot: i " + i + " angleStep: " + angleStep);
            angleStep += ranger.angle;
        }
    }


}
