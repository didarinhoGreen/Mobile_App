using UnityEngine;


public class UserLight : MonoBehaviour
{

    [SerializeField] Color GlowColor = Color.red;
    [SerializeField] float MinIntensity = 0.5f;
    [SerializeField] float MaxIntensity = 3f;
    [SerializeField] float FlikkerSpeed = 2f;

    Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        material = GetComponent<Renderer>().material;
        material.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        float wave = (Mathf.Sin(Time.time * FlikkerSpeed) + 1f) / 2f;
        float intensity = Mathf.Lerp(MinIntensity, MaxIntensity, wave);
        material.SetColor("_EmissionColor", GlowColor * intensity);
    }
}
