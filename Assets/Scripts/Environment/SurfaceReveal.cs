using UnityEngine;

[ExecuteInEditMode]
public class SurfaceReveal : MonoBehaviour
{
    [SerializeField] private Material mat;
    
    private Light src;
    private PlayerSelfLight playerLight;

    private void Awake()
    {
        playerLight = GameObject.FindWithTag("Player").GetComponent<PlayerSelfLight>();
        src = playerLight ? playerLight.LightSrc : null;
	}

    private void Update()
    {
        if (src)
        {
            mat.SetVector("_SpotPos", src.transform.position);
            mat.SetVector("_SpotDir", -src.transform.forward);
            mat.SetFloat("_SpotAngle", src.spotAngle);
            mat.SetColor("_Color", src.color);
            mat.SetInteger("_IsHeadLightOn", playerLight.IsOn ? 1 : 0);
        }
    }
}
