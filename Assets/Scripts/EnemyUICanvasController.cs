using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUICanvasController : MonoBehaviour
{
    public Slider healthSlider;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }

}
