using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour {

    public bool friend = false;
    public float damage = 10;
    public float speed = 6;
    public float lifetime = 2;
    float currentTime = 0;

    void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
        currentTime += Time.deltaTime;
        if (currentTime > lifetime)
        {
            DestroyBullet(false);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
  //      if (coll.gameObject.layer == 9)
    //    {
            DestroyBullet();
   //     }
    }

    void DestroyBullet(bool effect=true)
    {
        Destroy(gameObject);
    }

	void OnTriggerEnter(Collider col)
    {
        Character colCharacter = col.GetComponent<Character>();
        if(colCharacter!=null && colCharacter.friend != friend)
        {
            colCharacter.DealDamage(damage);
            DestroyBullet();
        }
    }
}
