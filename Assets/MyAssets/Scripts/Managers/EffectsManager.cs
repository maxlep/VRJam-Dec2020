using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;

    public GameObject dynamicAudioPlayer;
    public Transform effectsContainer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public GameObject SpawnParticlesAtPoint(GameObject particles, Vector3 position, Quaternion rotation, bool destroy = true, float lifeTime = 5f)
    {
        GameObject spawnedParticles = Instantiate(particles, position, rotation, effectsContainer);
        
        if (destroy)
            Destroy(spawnedParticles, lifeTime);
        else
        {
            //Instead of destory, just stop emission
            //To destroy, need to make sure stop action set to destroy on particleSystem
            ParticleHelper particleHelper = spawnedParticles.GetComponent<ParticleHelper>();
            particleHelper.DisableEmissions(lifeTime);
        }
            

        return spawnedParticles;
    }

    public void PlayClipAtPoint(AudioClip clip, Vector3 position, bool is3D = false, float volumeScale = 1f, float lifeTime = 5f)
    {
        GameObject dynamicPlayer = Instantiate(dynamicAudioPlayer, position, Quaternion.identity,
            effectsContainer);
        AudioSource dynamicSource = dynamicPlayer.GetComponent<AudioSource>();
        dynamicSource.clip = clip;
        dynamicSource.volume = volumeScale;
        if (is3D) dynamicSource.spatialBlend = 1f;
        dynamicSource.Play();
        Destroy(dynamicPlayer, lifeTime);
    }
}
