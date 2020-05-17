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
        //Gets Volume
        v = GetComponent<Volume>();
        //Gets Vignette
        v.profile.TryGet(out vg);
        
    }

    public void Update()
    {
        //Set vignette to VGI
        vg.intensity.value = vgI;
    }




}
