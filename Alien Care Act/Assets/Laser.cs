using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer line_renderer;
    private Light gun_light;
    private AudioSource gun_audio;
    private ParticleSystem gun_particles;

    // Start is called before the first frame update
    void Start()
    {
        line_renderer = GetComponent<LineRenderer>();
        gun_light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEffects(Vector3 end_position)
    {
        //gun_audio.Play();

        gun_light.enabled = true;

        //gun_particles.Stop();
        //gun_particles.Play();

        line_renderer.enabled = true;
        line_renderer.SetPosition(0, transform.position);
        line_renderer.SetPosition(1, end_position);

        StartCoroutine("DespawnEffects");
    }
    
    private IEnumerator DespawnEffects()
    {
        yield return new WaitForSeconds(0.02f);

        gun_light.enabled = false;
        line_renderer.enabled = false;
    }

}
