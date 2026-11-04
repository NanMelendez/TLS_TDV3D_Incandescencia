using UnityEngine;

[ExecuteInEditMode]
public class SurfaceReveal : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Texture2D diffuseMap;
    
    private Light src;
    private PlayerSelfLight playerLight;

    private void Awake()
    {
        playerLight = GameObject.FindWithTag("Player").GetComponent<PlayerSelfLight>();
        src = playerLight ? playerLight.LightSrc : null;

        if (AllVarsSet())
            mat.SetTexture("_MainTex", diffuseMap);
	}

    private void Update()
    {
        if (AllVarsSet())
        {
            mat.SetVector("_SpotPos", src.transform.position);
            mat.SetVector("_SpotDir", -src.transform.forward);
            mat.SetFloat("_SpotAngle", src.spotAngle);
            mat.SetColor("_Color", src.color);
            mat.SetFloat("_IsHeadLightOn", playerLight.IsOn ? 1 : 0);
        }
    }

    private bool AllVarsSet()
    {
        return src && mat;
    }
}
