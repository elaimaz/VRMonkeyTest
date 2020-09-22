using UnityEngine;
using System.Collections;

public class EnemyEyeController : MonoBehaviour
{

    public float timeToChangeBack = 1.0f;
    enum EyeTypes { normal, exclamation, question, angry, damaged,off, turningOn };
    bool isAngry = false;
    public bool dead = false;
    bool setup = false;

    public ParticleSystem exclamationParticles;
    public ParticleSystem questionParticles;

    public void SetAngry(bool val)
    {
        isAngry = val;
    }

    public void Awake()
    {
        if (setup)
        {
            return;
        }

        if (setup == false)
        {
            SetEye(0);
        }
        setup = true;
    }

    void SetEye(int eyeNumber)
    {
        switch (eyeNumber)
        {
            case 1:
                exclamationParticles.Play();
                break;
            case 2:
                questionParticles.Play();
                break;
        }

        // Debug.Log("Set Eye " + eyeNumber);
        if (dead && eyeNumber != 6)
        {
            return;
        }

    }

    public void SetDamaged()
    {
        SetEye((int)EyeTypes.damaged);
        StopCoroutine("waitToReturnToNormal");
        StartCoroutine("waitToReturnToNormal");
    }

    public void SetDead()
    {
        SetEye((int)EyeTypes.damaged);
        dead = true;
        StopAllCoroutines();
    }
    public void SetExclamation()
    {
        //  characterMats[1] = exclamationMaterial;
        //  eyeRenderer.materials = characterMats;
        SetEye((int)EyeTypes.exclamation);
        StopCoroutine("waitToReturnToNormal");
        StartCoroutine("waitToReturnToNormal");
    }
    public void SetQuestion()
    {
        SetEye((int)EyeTypes.question);
        //  characterMats[1] = questionMaterial;
        // eyeRenderer.materials = characterMats;
        StopCoroutine("waitToReturnToNormal");
        StartCoroutine("waitToReturnToNormal");
    }
    public void SetNormal()
    {
        if (isAngry)
        {
            SetEye((int)EyeTypes.angry);
        }
        else
        {
            SetEye((int)EyeTypes.normal);
        }

        //    characterMats[1] = normalMaterial;
        //    eyeRenderer.materials = characterMats;
    }


    public void SetOff()
    {
        SetEye((int)EyeTypes.off);
    }

    public void SetTurningOn()
    {
        SetEye((int)EyeTypes.turningOn);
    }

    IEnumerator waitToReturnToNormal()
    {
        yield return new WaitForSeconds(timeToChangeBack);
        SetNormal();
    }
}
