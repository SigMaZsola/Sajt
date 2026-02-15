using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField] private Image _healthBarSprite;
    public int playerIndex = 1;

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
     _healthBarSprite.fillAmount = currentHealth / maxHealth;
    }
}
