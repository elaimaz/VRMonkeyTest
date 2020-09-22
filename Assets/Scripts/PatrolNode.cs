using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour {

    public PatrolNode nextNode;
    [HideInInspector] public PatrolNode previousNode;
	
    public enum NodeType { walkNode,lookNode};
    public NodeType nodeType = NodeType.walkNode;

    void Awake()
    {
        if (nextNode != null)
        {
            nextNode.previousNode = this;
        }
        Renderer thisRenderer = GetComponent<Renderer>();
        if (thisRenderer != null)
        {
            thisRenderer.enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        if (nextNode != null)
        {
            if (nodeType == NodeType.walkNode)
            {
                Gizmos.color = Color.white;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawLine(transform.position, nextNode.transform.position);
            Gizmos.DrawWireSphere(transform.position + 0.75f * (nextNode.transform.position - transform.position),0.5f);
        }
    }
}
