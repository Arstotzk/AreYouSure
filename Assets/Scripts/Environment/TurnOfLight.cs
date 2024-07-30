using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnOfLight : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Light> lights;
    public Material lightOn;
    public Material lightOff;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnOff() 
    {
        foreach (var light in lights)
        {
            light.enabled = false;
            var renderer = light.GetComponentInParent<Renderer>();
            var materials = new List<Material>();
            renderer.GetMaterials(materials);
            var materialIndex = materials.FindIndex(m => m.name.Contains(lightOn.name));
            if (materialIndex == -1)
                continue;
            materials[materialIndex] = lightOff;
            renderer.SetMaterials(materials);
        }
    }
    public void TurnOn()
    {
        foreach (var light in lights)
        {
            light.enabled = true;
            var renderer = light.GetComponentInParent<Renderer>();
            var materials = new List<Material>();
            renderer.GetMaterials(materials);
            var materialIndex = materials.FindIndex(m => m.name.Contains(lightOff.name));
            if (materialIndex == -1)
                continue;
            materials[materialIndex] = lightOn;
            renderer.SetMaterials(materials);
        }
    }
}
