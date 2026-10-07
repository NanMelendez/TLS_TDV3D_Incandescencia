using UnityEngine;

[ExecuteInEditMode]
public class SurfaceReveal : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Light spotLight;

    private PlayerLightCtrl plc;

    private void Awake()
    {
        plc = GameObject.FindWithTag("Player").GetComponent<PlayerLightCtrl>();
    }

    private void Update()
    {
        if (spotLight)
        {
            mat.SetVector("_SpotPos", spotLight.transform.position);
            mat.SetVector("_SpotDir", -spotLight.transform.forward);
            mat.SetFloat("_SpotAngle", spotLight.spotAngle);
            if (plc)
                mat.SetInteger("_IsHeadLightOn", plc.IsOn ? 1 : 0);
        }
    }
}
