using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Transform doorObject;
    public bool locked = false;

    enum State { closed,opening,open}
    State state = State.closed;
    public float openTime=0.5f;
    float currentTimer = 0;

    public float openDeltaY = 2;


    Vector3 startingPos;
    Vector3 targetPos;

    public Material normalMaterial;
    public Material lockedMaterial;

    public Renderer doorRenderer;

    void OnTriggerEnter(Collider col)
    {
        if(state== State.open)
        {
            return;
        }
        if (col.CompareTag("Player"))
        {
            if (!locked)
            {
                Open();
            }
            else
            {
                if (GameLogic.instance.keys > 0)
                {
                    GameLogic.instance.useKey();
                    doorRenderer.material = normalMaterial;
                    Open();
                    
                }
                else
                {
                    ConsoleText.getInstance().ShowMessage("Keycard Required");
                }
            }
        }
    }

    void Open()
    {
        state = State.opening;
    }

	// Use this for initialization
	void Start () {
        startingPos = doorObject.position;
        targetPos = doorObject.position - Vector3.up * openDeltaY;

        if (locked)
        {
            doorRenderer.material = lockedMaterial;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.opening)
        {
            currentTimer += Time.deltaTime;
            doorObject.transform.position = Vector3.Lerp(startingPos, targetPos, currentTimer / openTime);
            if (currentTimer >= openTime)
            {
                doorObject.transform.position = targetPos;
                state = State.open;
            }
        }
	}
}
