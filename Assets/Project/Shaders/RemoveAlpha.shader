Shader "Omiya Games/Alpha"
{
	SubShader {
	    Pass {
	        ZTest Always Cull Off ZWrite Off
	        ColorMask A
	        Color (0,0,0,0)
	    }
	}
}
