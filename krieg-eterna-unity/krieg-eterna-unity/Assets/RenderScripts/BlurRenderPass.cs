using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class BlurRenderPass : ScriptableRenderPass
{
    const string profilerTag = "Blur Pass";
 
 
    RTHandle colorBuffer;
    RTHandle temporaryBuffer;
 
    Material mat; 
    Material compositeMat;
 
    static readonly int blurStrengthProperty = Shader.PropertyToID("_BlurStrength");
 
    public BlurRenderPass(Material material, Material compositeMaterial)
    {  
        renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
 
        // Now that this is verified within the Renderer Feature, it's already "trusted" here
        mat = material;
        compositeMat = compositeMaterial;

        
    }
 
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
 
        VolumeStack stack = VolumeManager.instance.stack;
        BlurEffectComponent blurEffect = stack.GetComponent<BlurEffectComponent>();
        descriptor.width /= blurEffect.downsample.value;
        descriptor.height /= blurEffect.downsample.value;
 
        descriptor.depthBufferBits = 0; // Color and depth cannot be combined in RTHandles
 
        // Enable these if your pass requires access to the CameraDepthTexture or the CameraNormalsTexture
        // ConfigureInput(ScriptableRenderPassInput.Depth);
        // ConfigureInput(ScriptableRenderPassInput.Normal);
 
        colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
 
 
        RenderingUtils.ReAllocateIfNeeded(ref temporaryBuffer, Vector2.one / blurEffect.downsample.value, descriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_TempBuffer");
    }
 
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // A reasonably common and simple safety net
        if(mat == null)
        {
            mat = CoreUtils.CreateEngineMaterial("Hidden/Blur");
        }
        if(temporaryBuffer == null)
        {
            temporaryBuffer = RTHandles.Alloc("_TempBuffer", name: "_TempBuffer");
        }
        if(colorBuffer == null)
        {
            colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
        }
 
        // NOTE: Do NOT mix ProfilingScope with named CommandBuffers i.e. CommandBufferPool.Get("name").
        // Currently there's an issue which results in mismatched markers.
        VolumeStack stack = VolumeManager.instance.stack;
        BlurEffectComponent blurEffect = stack.GetComponent<BlurEffectComponent>();
        CommandBuffer cmd = CommandBufferPool.Get();
        using(new ProfilingScope(cmd, new ProfilingSampler(profilerTag)))
        {
            
            mat.SetInt(blurStrengthProperty, blurEffect.blurStrength.value);
            Blitter.BlitCameraTexture(cmd, colorBuffer, temporaryBuffer, mat, 0); // shader pass 0
            Blitter.BlitCameraTexture(cmd, temporaryBuffer, colorBuffer, mat, 1); // shader pass 1
        }
        cmd.SetGlobalTexture("_BlurTexture", colorBuffer);
 
        //Blitter.BlitCameraTexture(cmd, colorBuffer, colorBuffer, compositeMat, 0);
        // Execute the command buffer and release it
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
 
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if(cmd == null)
        {
            throw new System.ArgumentNullException("cmd");
        }
 
        // Mentioned in the "Upgrade Guide" but pretty much only seen in "official" examples
        // in "DepthNormalOnlyPass"
        // https://github.com/Unity-Technologies/Graphics/blob/9ff23b60470c39020d8d474547bc0e01dde1d9e1/Packages/com.unity.render-pipelines.universal/Runtime/Passes/DepthNormalOnlyPass.cs
        colorBuffer = null;
    }
 
    public void Dispose()
    {
        // This seems vitally important, so why isn't it more prominently stated how it's intended to be used?
        temporaryBuffer?.Release();
    }
}