using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

[Serializable, VolumeComponentMenuForRenderPipeline("Blur", typeof(UniversalRenderPipeline))]
public class BlurEffectComponent : VolumeComponent, IPostProcessComponent
{
    [Header("Blur Settings")]

    // Used for any potential down-sampling we will do in the pass.
    public IntParameter downsample = new IntParameter(1);

    // A variable that's specific to the use case of our pass.
    public IntParameter blurStrength = new IntParameter(5);

    bool IPostProcessComponent.IsActive()
    {
        return (blurStrength.value > 0.0f) && active;
    }

    bool IPostProcessComponent.IsTileCompatible()
    {
        return false;
    }

    // additional properties ...
}
