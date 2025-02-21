using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Potion Info", fileName = "New Potion info")]
public class PotionData: ScriptableObject
{
    public string name;
    public string effect;
    public Sprite sprite;
    public Color backgroundColor;


}
