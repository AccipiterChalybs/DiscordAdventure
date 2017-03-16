using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolour : MonoBehaviour {

    public MeshRenderer[] meshesToRecoulour;

    public Material baseMaterial;
    private Material currentMaterial;

    public ParticleSystemRenderer psr;

    public void Start()
    {
        currentMaterial = baseMaterial;
    }

    public void GenerateMaterial(Color rgb)
    {
        Material output = new Material(baseMaterial);
        output.color = rgb;
        output.SetColor("_EmissionColor", rgb);
        SetMaterial(output);
    }

    public Material GetMaterial() { return currentMaterial;  }

    public void SetMaterial(Material newMaterial)
    {
        currentMaterial = newMaterial;
        foreach (MeshRenderer r in meshesToRecoulour)
        {
            r.material = currentMaterial;
        }
        if (psr)
        {
            psr.material = currentMaterial;
        }
    }
}
