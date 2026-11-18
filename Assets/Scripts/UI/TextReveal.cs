using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TMP_Text))]
public class TextReveal : MonoBehaviour
{
    [Header("Material con el shader Custom/TextReveal")]
    [SerializeField] private Material revealMaterial;

    [Header("Ajustes de la revelación")]
    [SerializeField] private float range = 10f;
    [Range(0f, 1f)][SerializeField] private float edgeSoftness = 0.25f;
    [Range(0f, 1f)][SerializeField] private float lightTint = 0f;

    private TMP_Text text;
    private Material mat;
    private Light src;
    private PlayerSelfLight playerLight;

    private static readonly int SpotPos = Shader.PropertyToID("_SpotPos");
    private static readonly int SpotDir = Shader.PropertyToID("_SpotDir");
    private static readonly int SpotAngle = Shader.PropertyToID("_SpotAngle");
    private static readonly int SpotRange = Shader.PropertyToID("_SpotRange");
    private static readonly int Softness = Shader.PropertyToID("_Softness");
    private static readonly int LightColor = Shader.PropertyToID("_LightColor");
    private static readonly int TintStrength = Shader.PropertyToID("_TintStrength");
    private static readonly int IsHeadLightOn = Shader.PropertyToID("_IsHeadLightOn");

    private void OnEnable()
    {
        text = GetComponent<TMP_Text>();

        var player = GameObject.FindWithTag("Player");
        playerLight = player ? player.GetComponent<PlayerSelfLight>() : null;
        src = playerLight ? playerLight.LightSrc : null;

        if (!revealMaterial || !text) return;

        // TMP crea un material propio por fuente; hay que asignarle el nuestro
        // conservando el atlas de la fuente.
        Texture atlas = text.font ? text.font.atlasTexture : null;

        if (Application.isPlaying)
        {
            mat = new Material(revealMaterial);
            if (atlas) mat.SetTexture("_MainTex", atlas);
            text.fontSharedMaterial = mat;
        }
        else
        {
            mat = revealMaterial;
            if (atlas) mat.SetTexture("_MainTex", atlas);
            text.fontSharedMaterial = mat;
        }
    }

    private void OnDestroy()
    {
        if (Application.isPlaying && mat) Destroy(mat);
    }

    private void Update()
    {
        if (!AllVarsSet()) return;

        mat.SetVector(SpotPos, src.transform.position);
        mat.SetVector(SpotDir, -src.transform.forward);
        mat.SetFloat(SpotAngle, src.spotAngle);
        mat.SetFloat(SpotRange, range);
        mat.SetFloat(Softness, edgeSoftness);
        mat.SetColor(LightColor, src.color);
        mat.SetFloat(TintStrength, lightTint);
        mat.SetFloat(IsHeadLightOn, playerLight.IsOn ? 1f : 0f);
    }

    private bool AllVarsSet()
    {
        return src && mat && playerLight;
    }
}