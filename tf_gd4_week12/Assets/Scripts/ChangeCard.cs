using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCard : MonoBehaviour
{
    public GameObject card;
    public TextMeshProUGUI _name, _effect;
    public Image _backgroundImage;
    public Image _potionImage;
    
    public void ChangeCardDetails(PotionData data)
    {
        card.SetActive(!card.active);
        _effect.text = data.effect;
        _name.text = data.name;
        _backgroundImage.color = data.backgroundColor;
        _potionImage.sprite = data.sprite;
    }

    public void Test()
    {
        Debug.Log($"clicked");
    }
}
