using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //components
    [SerializeField] ParticleSystem hitParticle;

    //Settings
    [SerializeField] StatSystem.DamageType damageType;
    [SerializeField] float damageAmount;
    [SerializeField] float arrowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += transform.forward * arrowSpeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StatSystem.DealDamage(collision.gameObject.GetComponent<Stats>(), damageType, damageAmount);
            CameraFeedback.instance.FramePause(0.25f);
        }
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}