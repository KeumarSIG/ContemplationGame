// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-871-OUT;n:type:ShaderForge.SFN_Time,id:5507,x:31857,y:32905,varname:node_5507,prsc:2;n:type:ShaderForge.SFN_Rotator,id:3485,x:32045,y:32729,varname:node_3485,prsc:2|UVIN-1037-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1037,x:31810,y:32582,varname:node_1037,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:871,x:32230,y:32859,varname:node_871,prsc:2|A-3485-UVOUT,B-5507-TSL;pass:END;sub:END;*/

Shader "Shader Forge/Material_Shader_1" {
    Properties {
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_7023 = _Time + _TimeEditor;
                float node_3485_ang = node_7023.g;
                float node_3485_spd = 1.0;
                float node_3485_cos = cos(node_3485_spd*node_3485_ang);
                float node_3485_sin = sin(node_3485_spd*node_3485_ang);
                float2 node_3485_piv = float2(0.5,0.5);
                float2 node_3485 = (mul(i.uv0-node_3485_piv,float2x2( node_3485_cos, -node_3485_sin, node_3485_sin, node_3485_cos))+node_3485_piv);
                float4 node_5507 = _Time + _TimeEditor;
                float3 emissive = float3((node_3485*node_5507.r),0.0);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
