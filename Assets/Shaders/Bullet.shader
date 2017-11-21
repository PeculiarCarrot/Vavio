Shader "Bullet" {
 
Properties {
    _Color ("Color", Color) = (1,1,1)
}
 
SubShader {
    Color [_Color]
    Pass {
    	Ztest Always
    }
}
 
}