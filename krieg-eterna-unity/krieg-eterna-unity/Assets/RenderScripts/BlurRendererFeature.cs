using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

public class BlurRendererFeature : ScriptableRendererFeature
{

    // References to our pass and its settings.
    BlurRenderPass pass;

    [SerializeField]
    public Shader blurShader;
    [SerializeField]
    public Shader compositeShader;

    private Material blurMaterial;
    private Material compositeMaterial;

    bool useDynamicTexture = false;

    // Gets called every time serialization happens.
    // Gets called when you enable/disable the renderer feature.
    // Gets called when you change a property in the inspector of the renderer feature.
    public override void Create()
    {
        if (blurMaterial == null)
        {
            blurMaterial = CoreUtils.CreateEngineMaterial(blurShader);
            
            compositeMaterial = CoreUtils.CreateEngineMaterial(compositeShader);
            useDynamicTexture = true;
        }
        pass = new BlurRenderPass(blurMaterial, compositeMaterial);
    }



    // This prevents attempted destruction of a manually-assigned material later

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }

    protected override void Dispose(bool disposing)
    {
        if (useDynamicTexture)
        {
            // Added this line to match convention for cleaning up materials
            // ... But only for a dynamically-generated material
            CoreUtils.Destroy(blurMaterial);
            CoreUtils.Destroy(compositeMaterial);
        }
        pass.Dispose();
    }
}