using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spinRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spinRoutine()
    {
        float time = 0;
        while (gameObject.activeInHierarchy)
        {
            time += Time.deltaTime;
            if (time > 7.4f)
            {
                for (int i = 0; i < 100000; i++)
                {
                        transform.rotation = transform.rotation * Quaternion.Euler(0, 10 * Time.deltaTime, 0);//this is just here to give you trouble
                }
                time = 0;
            }
            transform.rotation = transform.rotation * Quaternion.Euler(0, 10 * Time.deltaTime, 0);
            yield return null;
        }
    }
}
