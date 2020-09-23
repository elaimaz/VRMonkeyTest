using UnityEngine;

public class Character : MonoBehaviour {

    public enum States { idle,moving,hitStun,shocking,attacking}
    public States state = States.idle;
    public bool dead = false;
    public bool hitStun = false;
    public AudioSource audioSource;
    public bool friend = false;
    public Transform target;

    public float maxDrainEnergy = 10;
    public float energyLeft = 10;

    public bool visible = true;
    public GameObject bullet;

	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
    public void SetState(States newState)
    {
        state = newState;
    }

    public void EndKill()
    {

    }

    public void Fire()
    {
        GameObject fireEffect = GameObject.Instantiate(EffectsManager.getInstance().shootEffect);
        fireEffect.transform.position= transform.position + 0.3f * Vector3.up+0.2f*transform.forward;
        fireEffect.transform.forward = transform.forward;

        GameObject thisBullet= GameObject.Instantiate(bullet);
        if (friend)
        {
            ParticleSystem ps = thisBullet.GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule ma = ps.main;
            ma.startColor = Color.blue;
        }
        thisBullet.transform.position = transform.position + 0.3f * Vector3.up;
        thisBullet.transform.forward = transform.forward;
        thisBullet.GetComponent<DamageArea>().friend = friend;
    }

    public virtual void DealDamage(float val)
    {
        AIAgent agent;
        agent = GetComponent<AIAgent>();
        if (agent)
        {
            agent.OnDrainStart();
        }
    }
}
