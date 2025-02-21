using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject[] children;
    
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (GameObject child in children)
        {
            Vector2 newMousePos = new Vector2(mousePos.x, mousePos.y);

            PolygonCollider2D collider = child.GetComponent<PolygonCollider2D>();
            if (collider.bounds.Contains(newMousePos))
            {
                Debug.Log($"point in collider {child.name}");
                Debug.DrawRay(new Vector3(newMousePos.x, newMousePos.y, -1f), Vector3.back * 3, Color.blue);
            }
            
        }
    }
}
