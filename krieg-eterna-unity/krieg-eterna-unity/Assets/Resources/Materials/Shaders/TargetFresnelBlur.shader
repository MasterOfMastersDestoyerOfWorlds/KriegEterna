Shader "Shader Graphs/TargetFresnelBlur"
{
    Properties
    {
        [ToggleUI]_Flash("Flash", Float) = 1
        [NoScaleOffset]_Texture2D("Texture2D", 2D) = "white" {}
        [ToggleUI]_TransparentFlash("TransparentFlash", Float) = 0
        [ToggleUI]_Transparent("Transparent", Float) = 0
        _FresnelIntensity("FresnelIntensity", Float) = 2
        _FrenelEffect("FrenelEffect", Float) = 1.54
        _RectWidth("RectWidth", Float) = 0.7
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" 
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Flash;
        float4 _Texture2D_TexelSize;
        float _TransparentFlash;
        float _Transparent;
        float _FresnelIntensity;
        float _FrenelEffect;
        float _RectWidth;
        CBUFFER_END
        
        
        // Object and Global properties
        TEXTURE2D(_Texture2D);
        SAMPLER(sampler_Texture2D);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_ReplaceColor_float(float3 In, float3 From, float3 To, float Range, out float3 Out, float Fuzziness)
        {
            float Distance = distance(From, In);
            Out = lerp(To, In, saturate((Distance - Range) / max(Fuzziness, 1e-5f)));
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_RoundedRectangle_float(float2 UV, float Width, float Height, float Radius, out float Out)
        {
            Radius = max(min(min(abs(Radius * 2), abs(Width)), abs(Height)), 1e-5);
            float2 uv = abs(UV * 2 - 1) - float2(Width, Height) + Radius;
            float d = length(max(0, uv)) / Radius;
        #if defined(SHADER_STAGE_RAY_TRACING)
            Out = saturate((1 - d) * 1e7);
        #else
            float fwd = max(fwidth(d), 1e-5);
            Out = saturate((1 - d) / fwd);
        #endif
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }

        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }

        void Unity_Horizontal_Blur_float(float2 UV, float map,  float BlurStrength, out float Out)
        {
            
            float sum = 0;
            
            float res = _Texture2D_TexelSize.xy;
            int samples = 2 * BlurStrength + 1;

            for (float x = 0; x < samples; x++)
            {
                float offset = x - BlurStrength;
                sum += map.x + offset * res;
            }
            Out = sum / samples;
        }
        
        void Unity_Blend_Darken_float(float Base, float Blend, out float Out, float Opacity)
        {
            Out = min(Blend, Base);
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float3 _ReplaceColor_a6400e3ced1e4dd69af8789e13cc07b8_Out_4_Vector3;
            Unity_ReplaceColor_float(float3 (0, 0, 0), IsGammaSpace() ? float3(1, 1, 1) : SRGBToLinear(float3(1, 1, 1)), IsGammaSpace() ? float3(0.8679245, 0.7990731, 0) : SRGBToLinear(float3(0.8679245, 0.7990731, 0)), 1.86, _ReplaceColor_a6400e3ced1e4dd69af8789e13cc07b8_Out_4_Vector3, 4.89);
            float _Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean = _Transparent;
            float _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float = _RectWidth;
            float _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float;
            Unity_Multiply_float_float(_Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, 1.19, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float);
            float _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float, 0.06, _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float);
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float;
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors = float (1);
            Unity_InvertColors_float(_RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float, _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors, _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float);
            float2 _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.5, 0.49), 90, _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2);
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[0];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_G_2_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[1];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_B_3_Float = 0;
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_A_4_Float = 0;
            float _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float;
            Unity_Multiply_float_float(_Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float, 8.57, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float);
            float _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float;
            Unity_Blend_Darken_float(_InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float, _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, 1);
            float2 _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (2.27, -0.61), 1, _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2);
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[0];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_G_2_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[1];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_B_3_Float = 0;
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_A_4_Float = 0;
            float _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float;
            Unity_Multiply_float_float(_Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float, 5.53, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float);
            float _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float, _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, 1);
            float2 _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.51, 0.51), 180, _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2);
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[0];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[1];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_B_3_Float = 0;
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_A_4_Float = 0;
            float _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float, 5.53, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float);
            float _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float, _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, 1);
            float _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float, 8.57, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float);
            float _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float, _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, 1);
            float _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, 1, 1, 0.06, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float);
            float _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float, _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 1.76);
            float _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float;
            Unity_Multiply_float_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 0.47, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float);
            float _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float);
            float _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float;
            Unity_Sine_float(_Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float);
            float _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float;
            Unity_Lerp_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float);
            float _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float;
            Unity_Branch_float(_Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean, 0, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float, _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float);
            float _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            Unity_Clamp_float(_Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float, 0, 1, _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float);
            
            float _Blur_Out_3_Float;
            Unity_Horizontal_Blur_float(IN.uv0.xy, _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float, 5, _Blur_Out_3_Float);
            surface.BaseColor = _ReplaceColor_a6400e3ced1e4dd69af8789e13cc07b8_Out_4_Vector3;

            surface.Alpha = _Blur_Out_3_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                // Declaring the variable containing the normal vector for each
                // vertex.
                half3 normal        : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                half3 normal        : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Use the TransformObjectToWorldNormal function to transform the
                // normals from object to world space. This function is from the
                // SpaceTransforms.hlsl file, which is referenced in Core.hlsl.
                OUT.normal = TransformObjectToWorldNormal(IN.normal);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = 0;
                // IN.normal is a 3D vector. Each vector component has the range
                // -1..1. To show all vector elements as color, including the
                // negative values, compress each value into the range 0..1.
                color.rgb = IN.normal * 0.5 + 0.5;
                return 0;
            }
            ENDHLSL
        }
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 normalWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Flash;
        float4 _Texture2D_TexelSize;
        float _TransparentFlash;
        float _Transparent;
        float _FresnelIntensity;
        float _FrenelEffect;
        float _RectWidth;
        CBUFFER_END
        
        
        // Object and Global properties
        TEXTURE2D(_Texture2D);
        SAMPLER(sampler_Texture2D);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_RoundedRectangle_float(float2 UV, float Width, float Height, float Radius, out float Out)
        {
            Radius = max(min(min(abs(Radius * 2), abs(Width)), abs(Height)), 1e-5);
            float2 uv = abs(UV * 2 - 1) - float2(Width, Height) + Radius;
            float d = length(max(0, uv)) / Radius;
        #if defined(SHADER_STAGE_RAY_TRACING)
            Out = saturate((1 - d) * 1e7);
        #else
            float fwd = max(fwidth(d), 1e-5);
            Out = saturate((1 - d) / fwd);
        #endif
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Blend_Darken_float(float Base, float Blend, out float Out, float Opacity)
        {
            Out = min(Blend, Base);
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean = _Transparent;
            float _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float = _RectWidth;
            float _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float;
            Unity_Multiply_float_float(_Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, 1.19, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float);
            float _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float, 0.06, _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float);
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float;
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors = float (1);
            Unity_InvertColors_float(_RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float, _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors, _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float);
            float2 _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.5, 0.49), 90, _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2);
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[0];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_G_2_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[1];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_B_3_Float = 0;
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_A_4_Float = 0;
            float _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float;
            Unity_Multiply_float_float(_Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float, 8.57, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float);
            float _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float;
            Unity_Blend_Darken_float(_InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float, _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, 1);
            float2 _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (2.27, -0.61), 1, _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2);
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[0];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_G_2_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[1];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_B_3_Float = 0;
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_A_4_Float = 0;
            float _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float;
            Unity_Multiply_float_float(_Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float, 5.53, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float);
            float _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float, _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, 1);
            float2 _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.51, 0.51), 180, _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2);
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[0];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[1];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_B_3_Float = 0;
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_A_4_Float = 0;
            float _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float, 5.53, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float);
            float _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float, _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, 1);
            float _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float, 8.57, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float);
            float _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float, _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, 1);
            float _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, 1, 1, 0.06, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float);
            float _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float, _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 1.76);
            float _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float;
            Unity_Multiply_float_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 0.47, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float);
            float _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float);
            float _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float;
            Unity_Sine_float(_Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float);
            float _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float;
            Unity_Lerp_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float);
            float _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float;
            Unity_Branch_float(_Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean, 0, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float, _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float);
            float _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            Unity_Clamp_float(_Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float, 0, 1, _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float);
            surface.Alpha = _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 normalWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Flash;
        float4 _Texture2D_TexelSize;
        float _TransparentFlash;
        float _Transparent;
        float _FresnelIntensity;
        float _FrenelEffect;
        float _RectWidth;
        CBUFFER_END
        
        
        // Object and Global properties
        TEXTURE2D(_Texture2D);
        SAMPLER(sampler_Texture2D);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_RoundedRectangle_float(float2 UV, float Width, float Height, float Radius, out float Out)
        {
            Radius = max(min(min(abs(Radius * 2), abs(Width)), abs(Height)), 1e-5);
            float2 uv = abs(UV * 2 - 1) - float2(Width, Height) + Radius;
            float d = length(max(0, uv)) / Radius;
        #if defined(SHADER_STAGE_RAY_TRACING)
            Out = saturate((1 - d) * 1e7);
        #else
            float fwd = max(fwidth(d), 1e-5);
            Out = saturate((1 - d) / fwd);
        #endif
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Blend_Darken_float(float Base, float Blend, out float Out, float Opacity)
        {
            Out = min(Blend, Base);
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean = _Transparent;
            float _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float = _RectWidth;
            float _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float;
            Unity_Multiply_float_float(_Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, 1.19, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float);
            float _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float, 0.06, _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float);
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float;
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors = float (1);
            Unity_InvertColors_float(_RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float, _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors, _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float);
            float2 _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.5, 0.49), 90, _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2);
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[0];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_G_2_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[1];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_B_3_Float = 0;
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_A_4_Float = 0;
            float _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float;
            Unity_Multiply_float_float(_Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float, 8.57, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float);
            float _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float;
            Unity_Blend_Darken_float(_InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float, _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, 1);
            float2 _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (2.27, -0.61), 1, _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2);
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[0];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_G_2_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[1];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_B_3_Float = 0;
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_A_4_Float = 0;
            float _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float;
            Unity_Multiply_float_float(_Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float, 5.53, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float);
            float _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float, _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, 1);
            float2 _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.51, 0.51), 180, _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2);
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[0];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[1];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_B_3_Float = 0;
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_A_4_Float = 0;
            float _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float, 5.53, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float);
            float _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float, _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, 1);
            float _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float, 8.57, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float);
            float _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float, _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, 1);
            float _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, 1, 1, 0.06, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float);
            float _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float, _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 1.76);
            float _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float;
            Unity_Multiply_float_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 0.47, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float);
            float _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float);
            float _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float;
            Unity_Sine_float(_Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float);
            float _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float;
            Unity_Lerp_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float);
            float _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float;
            Unity_Branch_float(_Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean, 0, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float, _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float);
            float _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            Unity_Clamp_float(_Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float, 0, 1, _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float);
            surface.Alpha = _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Flash;
        float4 _Texture2D_TexelSize;
        float _TransparentFlash;
        float _Transparent;
        float _FresnelIntensity;
        float _FrenelEffect;
        float _RectWidth;
        CBUFFER_END
        
        
        // Object and Global properties
        TEXTURE2D(_Texture2D);
        SAMPLER(sampler_Texture2D);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_RoundedRectangle_float(float2 UV, float Width, float Height, float Radius, out float Out)
        {
            Radius = max(min(min(abs(Radius * 2), abs(Width)), abs(Height)), 1e-5);
            float2 uv = abs(UV * 2 - 1) - float2(Width, Height) + Radius;
            float d = length(max(0, uv)) / Radius;
        #if defined(SHADER_STAGE_RAY_TRACING)
            Out = saturate((1 - d) * 1e7);
        #else
            float fwd = max(fwidth(d), 1e-5);
            Out = saturate((1 - d) / fwd);
        #endif
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Blend_Darken_float(float Base, float Blend, out float Out, float Opacity)
        {
            Out = min(Blend, Base);
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean = _Transparent;
            float _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float = _RectWidth;
            float _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float;
            Unity_Multiply_float_float(_Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, 1.19, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float);
            float _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float, 0.06, _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float);
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float;
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors = float (1);
            Unity_InvertColors_float(_RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float, _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors, _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float);
            float2 _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.5, 0.49), 90, _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2);
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[0];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_G_2_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[1];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_B_3_Float = 0;
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_A_4_Float = 0;
            float _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float;
            Unity_Multiply_float_float(_Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float, 8.57, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float);
            float _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float;
            Unity_Blend_Darken_float(_InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float, _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, 1);
            float2 _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (2.27, -0.61), 1, _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2);
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[0];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_G_2_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[1];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_B_3_Float = 0;
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_A_4_Float = 0;
            float _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float;
            Unity_Multiply_float_float(_Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float, 5.53, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float);
            float _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float, _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, 1);
            float2 _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.51, 0.51), 180, _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2);
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[0];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[1];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_B_3_Float = 0;
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_A_4_Float = 0;
            float _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float, 5.53, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float);
            float _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float, _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, 1);
            float _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float, 8.57, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float);
            float _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float, _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, 1);
            float _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, 1, 1, 0.06, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float);
            float _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float, _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 1.76);
            float _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float;
            Unity_Multiply_float_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 0.47, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float);
            float _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float);
            float _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float;
            Unity_Sine_float(_Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float);
            float _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float;
            Unity_Lerp_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float);
            float _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float;
            Unity_Branch_float(_Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean, 0, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float, _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float);
            float _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            Unity_Clamp_float(_Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float, 0, 1, _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float);
            surface.Alpha = _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Flash;
        float4 _Texture2D_TexelSize;
        float _TransparentFlash;
        float _Transparent;
        float _FresnelIntensity;
        float _FrenelEffect;
        float _RectWidth;
        CBUFFER_END
        
        
        // Object and Global properties
        TEXTURE2D(_Texture2D);
        SAMPLER(sampler_Texture2D);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_RoundedRectangle_float(float2 UV, float Width, float Height, float Radius, out float Out)
        {
            Radius = max(min(min(abs(Radius * 2), abs(Width)), abs(Height)), 1e-5);
            float2 uv = abs(UV * 2 - 1) - float2(Width, Height) + Radius;
            float d = length(max(0, uv)) / Radius;
        #if defined(SHADER_STAGE_RAY_TRACING)
            Out = saturate((1 - d) * 1e7);
        #else
            float fwd = max(fwidth(d), 1e-5);
            Out = saturate((1 - d) / fwd);
        #endif
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Blend_Darken_float(float Base, float Blend, out float Out, float Opacity)
        {
            Out = min(Blend, Base);
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean = _Transparent;
            float _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float = _RectWidth;
            float _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float;
            Unity_Multiply_float_float(_Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, 1.19, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float);
            float _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, _Property_cd1a6ac35be84847aea88b6a93bfa601_Out_0_Float, _Multiply_7e384c4dc57349a5a7c621fbd65db0be_Out_2_Float, 0.06, _RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float);
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float;
            float _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors = float (1);
            Unity_InvertColors_float(_RoundedRectangle_7720ff2709294f139027462c7e83cd51_Out_4_Float, _InvertColors_5c16825217a6472d98b1db76c13374a0_InvertColors, _InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float);
            float2 _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.5, 0.49), 90, _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2);
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[0];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_G_2_Float = _Rotate_5c4f87d760214342b528b87d9172fc74_Out_3_Vector2[1];
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_B_3_Float = 0;
            float _Split_a067209b7c11439faf0ddc7a8e4f6712_A_4_Float = 0;
            float _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float;
            Unity_Multiply_float_float(_Split_a067209b7c11439faf0ddc7a8e4f6712_R_1_Float, 8.57, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float);
            float _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float;
            Unity_Blend_Darken_float(_InvertColors_5c16825217a6472d98b1db76c13374a0_Out_1_Float, _Multiply_a7b25c86e47e4639a3e24158673af27d_Out_2_Float, _Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, 1);
            float2 _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (2.27, -0.61), 1, _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2);
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[0];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_G_2_Float = _Rotate_afbd673fa687458fa85049fddc1a35ba_Out_3_Vector2[1];
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_B_3_Float = 0;
            float _Split_a16ac62c71ef4f55b8f0811f2595fd5b_A_4_Float = 0;
            float _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float;
            Unity_Multiply_float_float(_Split_a16ac62c71ef4f55b8f0811f2595fd5b_R_1_Float, 5.53, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float);
            float _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_5647cb46c986412fb37d0454b0f1da83_Out_2_Float, _Multiply_22df4e90341d4fdf91069ea093877580_Out_2_Float, _Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, 1);
            float2 _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2;
            Unity_Rotate_Degrees_float(IN.uv0.xy, float2 (0.51, 0.51), 180, _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2);
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[0];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float = _Rotate_6a7b1a0967ec487490345c54b81d5281_Out_3_Vector2[1];
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_B_3_Float = 0;
            float _Split_4c2e2511b3d44181890e7ed7cf9b44ad_A_4_Float = 0;
            float _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_R_1_Float, 5.53, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float);
            float _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_bc0661aa8f244743a2699d4b76021ac7_Out_2_Float, _Multiply_f4ad3f0953d9438a99c6ab286663a14f_Out_2_Float, _Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, 1);
            float _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float;
            Unity_Multiply_float_float(_Split_4c2e2511b3d44181890e7ed7cf9b44ad_G_2_Float, 8.57, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float);
            float _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_a79d966c38f345bba3675876f743736b_Out_2_Float, _Multiply_3cf86986a55342bfbe50143e094b48d1_Out_2_Float, _Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, 1);
            float _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float;
            Unity_RoundedRectangle_float(IN.uv0.xy, 1, 1, 0.06, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float);
            float _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float;
            Unity_Blend_Darken_float(_Blend_9d20247fff00497c9494ff4e319b0450_Out_2_Float, _RoundedRectangle_10b6d34198e946f5a62f37a18b0395a8_Out_4_Float, _Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 1.76);
            float _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float;
            Unity_Multiply_float_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, 0.47, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float);
            float _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float);
            float _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float;
            Unity_Sine_float(_Multiply_e0cc2a9df6364af6a7d768c70a7a49df_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float);
            float _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float;
            Unity_Lerp_float(_Blend_473fb538154c418ca10b7227e9e54794_Out_2_Float, _Multiply_93067073441e4253b1d42c30ddd475a5_Out_2_Float, _Sine_cd43538e20c649db8681e342b5f6aef0_Out_1_Float, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float);
            float _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float;
            Unity_Branch_float(_Property_ee4142272fcc4af0a0549b0a56e51a68_Out_0_Boolean, 0, _Lerp_d2ca49ca53a644eba35c204e0a2ca025_Out_3_Float, _Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float);
            float _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            Unity_Clamp_float(_Branch_7da3d1ad693b400f8e6621b6de6e86e0_Out_3_Float, 0, 1, _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float);
            surface.Alpha = _Clamp_b8edac10ee934f9182e5f3bc9a353909_Out_3_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        
    }
SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }

        Pass
        {
                    Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        Tags { }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                // Declaring the variable containing the normal vector for each
                // vertex.
                half3 normal        : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                half3 normal        : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Use the TransformObjectToWorldNormal function to transform the
                // normals from object to world space. This function is from the
                // SpaceTransforms.hlsl file, which is referenced in Core.hlsl.
                OUT.normal = TransformObjectToWorldNormal(IN.normal);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = 0;
                // IN.normal is a 3D vector. Each vector component has the range
                // -1..1. To show all vector elements as color, including the
                // negative values, compress each value into the range 0..1.
                color.rgb = IN.normal * 0.5 + 0.5;
                return color;
            }
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}