using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float health, knockTime;
    public bool beingDamaged = false;
    public bool isDead;
    public LayerMask layerMask;
    Rigidbody rb;

    [SerializeField] Slider healthSlider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = startingHealth;
        if (healthSlider) healthSlider.value = health;
        //layerMask = LayerMask.NameToLayer("Environment");
    }
    public void takeDamage(float damageAmount, Vector3 knockDirection = new Vector3(), bool knockback = false)
    {
        Debug.Log("damage function");
        if (!beingDamaged)
        {
            Debug.Log("takeThatDamage");
            health -= damageAmount;
            beingDamaged = true;
            if (healthSlider) healthSlider.value = health;
            if (health <= 0) Death();
            Invoke("Recover", 0.5f);
            if (knockback)
            {
                StartCoroutine(KnockBack(transform.position, knockDirection));
            }
        }
        
    }

    public virtual void Death()
    {
        Debug.Log("Death");
        isDead = true;
    }
    void Recover()
    {
        beingDamaged = false;
    }
    IEnumerator KnockBack(Vector3 a, Vector3 b)
    {
        float current = 0;
        float wallCheck = transform.localScale.x * 0.6f;
        Ray ray = new Ray();
        ray.origin = a;
        ray.direction = b;
        RaycastHit hit;
        while(current/knockTime < 1)
        {
            ray.origin = transform.position;
            ray.direction = b - transform.position;
            transform.position = Vector3.Lerp(a, b, current / knockTime);
            if (Physics.Raycast(ray, out hit, wallCheck, layerMask))
            {
                Debug.Log(hit.transform.name);
                b = hit.point + -ray.direction;
                yield return null;
            }
            
            Debug.DrawRay(ray.origin, ray.direction);
            current += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
