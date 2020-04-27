using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPVolumeControl : MonoBehaviour
{

    private Volume v;
    
    public Vignette vg;
    public static float vgI = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        v = GetComponent<Volume>();
        v.profile.TryGet(out vg);

        vg.intensity.value = vgI;
    }

    public void Update()
    {
        vg.intensity.value = vgI;
    }




}
