using UnityEngine;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;
public class BumpPlatform : MonoBehaviour
{

    [SerializeField] float moveDistance;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 moveDirection;

    public bool performAction = true;

    public void CallMove()
    {
       
        if (performAction)
        {
            Debug.Log($"Collision Enter");
            StartCoroutine(Move());
            performAction = false;
        }
        
    }
    IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        float distanceMoved = 0f;

        while (distanceMoved < moveDistance)
        {
            distanceMoved += Time.deltaTime * moveSpeed;
            transform.position = startPos + moveDirection * distanceMoved;
            yield return null;

        }

        transform.position = startPos + moveDistance * moveDirection;
    }
}
