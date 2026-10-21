using UnityEngine;

[ExecuteInEditMode]
public class SurfaceReveal : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Light spotLight;

    private PlayerSelfLight playerLight;

    private void Awake()
    {
        playerLight = GameObject.FindWithTag("Player").GetComponent<PlayerSelfLight>();
    }

    private void Update()
    {
        if (spotLight)
        {
            mat.SetVector("_SpotPos", spotLight.transform.position);
            mat.SetVector("_SpotDir", -spotLight.transform.forward);
            mat.SetFloat("_SpotAngle", spotLight.spotAngle);
            if (playerLight)
                mat.SetInteger("_IsHeadLightOn", playerLight.IsOn ? 1 : 0);
        }
    }
}
