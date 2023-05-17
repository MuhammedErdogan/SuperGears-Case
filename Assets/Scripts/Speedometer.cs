using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public RectTransform needle;
    public GameObject numberPrefab;  // A Text UI prefab that will be instantiated for each number.
    public float maxSpeedKmh = 200f;  // max speed for your speedometer
    public int numberOfMarks = 10;  // The number of marks on your speedometer.

    private float maxAngle = 220f;  // the maximum rotation of the needle
    private float minAngle = -40f;  // the minimum rotation of the needle

    private void Start()
    {
        GenerateNumbers();
    }

    public void UpdateSpeedometer(float currentSpeedKmh)
    {
        float speedFraction = currentSpeedKmh / maxSpeedKmh;
        float angle = Mathf.Lerp(minAngle, maxAngle, speedFraction);
        needle.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void GenerateNumbers()
    {
        float range = maxAngle - minAngle;
        float increment = range / numberOfMarks;

        for (int i = 0; i <= numberOfMarks; i++)
        {
            GameObject numberObject = Instantiate(numberPrefab, transform);
            RectTransform rectTransform = numberObject.GetComponent<RectTransform>();

            float angle = minAngle + increment * i;
            rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

            var text = numberObject.GetComponent<TextMeshProUGUI>();
            text.text = ((maxSpeedKmh / numberOfMarks) * i).ToString();
        }
    }
}
