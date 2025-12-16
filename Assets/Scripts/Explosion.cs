using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPrefab;

    // Kallas från UI-knappen
    public void TriggerExplosion()
    {
        // Skapa en ny instans
        ParticleSystem fx = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity);
        fx.Play();
        Debug.Log("Explosion triggered!");

        // Städa upp efter att den är klar
        Destroy(fx.gameObject, fx.main.duration + fx.main.startLifetime.constantMax);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerExplosion();
        }
    }
}
