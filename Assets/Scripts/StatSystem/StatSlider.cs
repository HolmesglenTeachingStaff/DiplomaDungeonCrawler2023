using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSlider : MonoBehaviour
{
    enum StatType { health, armour }
    [SerializeField] StatType statType;
    [SerializeField] Stats stats;

    public bool isWorldSpace;
    private Camera cam;
    private Slider slider;
    private Image image;
    private bool isImage;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            isImage = true;
        }
        else
        {
            isImage = false;
            slider = GetComponent<Slider>();
        }

        UpdateSlider();
        cam = Camera.main;
    }
    private void Update()
    {
        if (!isWorldSpace) return;
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
    public void UpdateSlider()
    {
        if (statType == StatType.health)
        {
            if (isImage)
            {
                image.fillAmount = stats.currentHealth / stats.maxHealth;
            }
            else
            {
                slider.maxValue = stats.maxHealth;
                slider.minValue = 0;
                slider.value = stats.currentHealth;
            }

        }
        else
        {
            if (isImage)
            {
                image.fillAmount = stats.currentArmour / stats.maxArmour;
            }
            else
            {
                slider.maxValue = stats.maxArmour;
                slider.minValue = 0;
                slider.value = stats.currentArmour;
            }
        }
    }
}

