using UnityEngine;
using UnityEngine.UI;

public class OrbFillBar : MonoBehaviour
{
    public Slider Slider;

    public void SetPowerUpBar(int orbCount)
    {
        Slider.value = orbCount;
    }

}
