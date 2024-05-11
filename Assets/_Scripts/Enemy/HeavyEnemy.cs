using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy
{

    private int _numberOfHits;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {



    }



    /*
     OnTriggerEnter(Collider other): Calls the TakeDamage()
     when the enemy has a collision with the player's prjectile.
     Also calls HeavyEnemyHealth() that will in turn call the ChangeColor() to
     change the enemy's color when it has taken its 1st hit.
    */

    //TBC if player projectile is trigger _ TODO (Estelle)
    //TBC player projectile tage name _ TODO (Estelle)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            _numberOfHits++;
            TakeDamage(1);
            HeavyEnemyHealth();
        }
    }

    // Tracks _numberOfHits and calls the heavy enemy's ChangeColor().
    private void HeavyEnemyHealth()
    {
        if (_numberOfHits == 1)
        { ChangeColor(); }

    }

    // Uses _numberOfHits change the animations in animator.
    private void ChangeColor()
    { anim.SetInteger("NumberOfHits", _numberOfHits); }

}


