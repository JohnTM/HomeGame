using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour {

    public List<Light> lights = new List<Light>();
    List<float> lightBaseValues = new List<float>();
    public Light sunLight;

    public Material tvScreen;
    public Color tvScreenBaseEmission;
    public float tvBrightnessMult = 2f;

	// Use this for initialization
	void Start () {
        lightBaseValues.Clear();
        for (int i = 0; i < lights.Count; i++)
        {
            lightBaseValues.Add(lights[i].intensity);
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].intensity = lightBaseValues[i] - sunLight.intensity;
        }
        tvScreen.SetColor("_EmissionColor", tvScreenBaseEmission * (tvBrightnessMult - (sunLight.intensity * 2)));
    }
}
