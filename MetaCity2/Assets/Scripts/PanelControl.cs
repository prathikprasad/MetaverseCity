using UnityEngine;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour
{
    public GameObject Panel; // Reference to the panel GameObject
    public GameObject Sun;
    public Slider xRotationSlider; // Reference to the UI Slider component for x-axis rotation
    public Slider yRotationSlider; // Reference to the UI Slider component for y-axis rotation

    public bool useSlider;
    public bool followSun;

    // Update is called once per frame
    void Update()
    {
        if (useSlider)
        {
            // Map the slider values (0 to 1) to the rotation angles (0 to 360 degrees)
            float xRotationAngle = xRotationSlider.value * 360f;
            float yRotationAngle = yRotationSlider.value * 360f;

            // Set the rotation of the panel
            Panel.transform.rotation = Quaternion.Euler(xRotationAngle, 0f, yRotationAngle);
        }
        else if (followSun)
        {
            Panel.transform.rotation = Sun.transform.rotation;
            //Panel.transform.rotation = Quaternion.Euler(Mathf.Clamp(Sun.transform.rotation.x, 75, 275), 0f, 0f);
        }
    }
}
