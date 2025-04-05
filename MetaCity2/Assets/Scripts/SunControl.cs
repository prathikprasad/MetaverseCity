using UnityEngine;
using UnityEngine.UI;

public class SunControl : MonoBehaviour
{
    public Slider timeSliderx; // Reference to the UI Slider component
    public Slider timeSliderz; // Reference to the UI Slider component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Map the slider value (0 to 1) to the rotation angle (0 to 360 degrees)
        float rotationAnglex = timeSliderx.value * 360f;
        float rotationAnglez = timeSliderz.value * 360f;
        changeTime(rotationAnglex, rotationAnglez);

        // Set the rotation of the sun

    }

    void changeTime(float rotationAnglex, float rotationAnglez)
    {
        
        transform.rotation = Quaternion.Euler(rotationAnglex, 0f , rotationAnglez);
    }
}
