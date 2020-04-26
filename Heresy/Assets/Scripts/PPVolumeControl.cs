using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPVolumeControl : MonoBehaviour
{

    private Volume v;
    private Bloom b;
    public Vignette vg;
    // Start is called before the first frame update
    void Start()
    {
        v = GetComponent<Volume>();
        v.profile.TryGet(out b);
        v.profile.TryGet(out vg);
    }

   

}
