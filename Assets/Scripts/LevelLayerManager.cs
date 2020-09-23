using UnityEngine;

public class LevelLayerManager : MonoBehaviour {

    public int scenarioLayer=9;
    public string scenarioTag="Wall";

	void Awake () {
		foreach(Transform child in transform)
        {
            child.tag = scenarioTag;
            child.gameObject.layer = scenarioLayer;
        }
	}
}
