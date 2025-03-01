using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ResetScene : MonoBehaviour
{
    [SerializeField] Transform[] interactableObjects;
    [SerializeField] GameObject speachBubble;
    Vector3[] positions;
    Vector3[] eulerAngles;

    GameObject player;
    PlayerController pc;
    Vector3 playerStartPos;
    int i = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        playerStartPos = player.transform.position;
        positions = new Vector3[interactableObjects.Length];
        eulerAngles = new Vector3[interactableObjects.Length];

        foreach (Transform t in interactableObjects)
        {
            positions[i] = t.position;
            eulerAngles[i] = t.eulerAngles;
            i++;
        }
    }

    // Update is called once per frame
    public void Restart()
    {
        player.transform.position = playerStartPos;

        

        for (int j = 0; j < interactableObjects.Length; j++)
        {
            Transform t = interactableObjects[j];
            t.position = positions[j];
            t.eulerAngles = eulerAngles[j];


            if (t.tag == "Move" ||  t.tag == "FallDown")
            {
                Spikes spikes = t.GetComponent<Spikes>();
                spikes.phase1 = true;
                spikes.startCoroutine = true;
            }
            else if (t.tag == "FallingPlatform")
            {
                FallingPlatform fp = t.GetComponent<FallingPlatform>();
                fp.startCoroutine = false;
            }
            else if (t.tag == "BumpPlatform")
            {
                BumpPlatform bp = t.GetComponent<BumpPlatform>();
                bp.performAction = true;
            }

        }

        StartCoroutine(ActivateSpeachBubble());

        

    }

    IEnumerator ActivateSpeachBubble()
    {
        speachBubble.SetActive(true);
        yield return new WaitForSeconds(3f);
        pc.startCoroutine = true;
        speachBubble.SetActive(false);
    }
}
