using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternAttack : MonoBehaviour
{
    public enum States { Attacking, Returning, Following}
    public StatSystem.DamageType damageType;
    public LayerMask layersToDamage;
    public LayerMask obsticleLayers;
    private TrailRenderer trail;
    public float minDamage, maxDamage;
    public States lanternState;
    public ParticleSystem[] lanternParticles;

    public LanternFollowBehaviour follow;

    public float attackSpeed, returnSpeed;
    private float followSpeed;

    private Vector3 target;

    private int burstCounter = 0;

    public float smallDamageRadius;
    public float largeDamageRadius;

    public Transform player;
    private Animator anim;
    private MousePosition mousePos;
    //Ray settings

    RaycastHit hit;
    public float range;
    public float timeBetweenShots;

    private float lastShot;
    private float attackTime;

    public float targetBuffer;
    private Collider damageCollider;

    //Lantern Lerp settings
    private float journeyLength;
    private float startTime;
    public AnimationCurve movementCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f); // Customize the curve in the Inspector
     private Quaternion initialRotation;

    public Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        lanternState = States.Following;
        anim = player.GetComponentInChildren<Animator>();
        mousePos = GetComponent<MousePosition>();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.emitting = false;
        lastShot = Time.time;
        followSpeed = follow.followSpeed;
        initialRotation = transform.rotation; 
       
    }

    // Update is called once per frame
    void Update()
    {
        //check if fully returned
        if(lanternState == States.Returning)
        {
            if(Vector3.Distance(transform.position, follow.followTarget.position)< 0.5f)
            {
                lanternState = States.Following;
                follow.followSpeed = followSpeed;
                
            }

              else
            {
                // Rotate the lantern while returning
                float rotationSpeed = 90f; // Adjust the rotation speed as needed
                transform.rotation *= Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime);
            }
        }

        if (Input.GetButtonDown("Fire2") && lanternState == States.Following)
        {
            if (anim.GetCurrentAnimatorStateInfo(3).IsName("Empty"))
            {
                anim.Play("Shoot");
                AimLantern();
                burstCounter = 0;
                StartCoroutine(LaunchLantern(target));
            }
            
        }

         
        
    }
    void AimLantern()
    {
        //find direction
        Vector3 direction = mousePos.GetPosition(Camera.main) - player.transform.position;
        direction.y = player.transform.position.y;
        direction.Normalize();
        //check raycast
        if(Physics.Raycast(player.position, direction, out hit, range, obsticleLayers))
        {
            target = hit.point;
        }
        else
        {
            target = player.transform.position + direction * range;
        }
        target.y = transform.position.y;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (lanternState != States.Attacking) return;
        Debug.Log("hit");
        if(other.tag == "Enemy")
        {
            Debug.Log("EnemyHit");
            if(Time.time - lastShot > attackTime)
            {
                if(burstCounter < 2)
                {
                    Debug.Log("passed timer check");
                    DamageBurst(smallDamageRadius);
                    burstCounter++;
                }
                
            }
        }

    }

    void DamageBurst(float radius)
    {
        pickParticle(radius);
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, radius, layersToDamage);
        if (possibleEnemies.Length == 0)
        {
            Debug.Log("no Enemies found");
            return;
        }
        foreach (Collider hit in possibleEnemies)
        {
            Stats stats = hit.GetComponent<Stats>();
            if(stats != null)
            {
                //measure the distance from the centre of the explosion to the target
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                //convert the distance / radius to get a percentage damage
                float ratio = distance / radius;
                ratio = Mathf.Clamp01(ratio);

                //determine damage based of distance ratio
                float damage = Mathf.Lerp(maxDamage, minDamage, ratio);
                //deal the damage
                StatSystem.DealDamage(stats, damageType, damage, true, transform.position);
            }
        }
        //lanternParticles[0].Play();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target, 0.5f);
        Gizmos.DrawLine(transform.position, target);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, smallDamageRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, largeDamageRadius);
    }

    IEnumerator LaunchLantern(Vector3 target)
    {
        yield return new WaitForSeconds(0.2f);
        trail.emitting = true;
        lanternState = States.Attacking;
        follow.enabled = false;
        // Calculate the length of the journey based on the positions
        journeyLength = Vector3.Distance(transform.position, target);
        float currentJourneyLength = journeyLength;
        startTime = Time.time; // Store the start time
        float timer = 0f;

        
        float distanceCovered = (timer) * attackSpeed; // Calculate how far we've gone
        float fractionOfJourney = distanceCovered / journeyLength; // Calculate the journey progress

        while (currentJourneyLength > targetBuffer)
        {
            // Ensure we stay within the curve range [0, 1]
            fractionOfJourney = Mathf.Clamp01(fractionOfJourney);

            // Use the curve to determine the eased progress
            float easedProgress = movementCurve.Evaluate(fractionOfJourney);

            // Lerp the object's position
            transform.position = Vector3.Lerp(transform.position, target, easedProgress);

            timer += Time.deltaTime;
            distanceCovered = (timer) * attackSpeed;
            fractionOfJourney = distanceCovered / journeyLength; // Calculate the journey progress
            currentJourneyLength = Vector3.Distance(transform.position, target);
  targetRotation = transform.rotation; 
            yield return new WaitForEndOfFrame();
        }

        //enabled = false;
        DamageBurst(largeDamageRadius);
        lanternState = States.Returning;
        yield return new WaitForSeconds(0.5f);
        trail.emitting = false;
        follow.followSpeed = returnSpeed;
        follow.enabled = true;

        

        yield return null;
    }
    void pickParticle(float radius)
    {
        int index = (int)damageType;

        ParticleSystem particle = lanternParticles[index];

        if(radius == smallDamageRadius)
        {
            particle.gameObject.transform.localScale = new Vector3(6, 6, 6);
        }
        else
        {
            particle.gameObject.transform.localScale = new Vector3(12, 12, 12);
        }
        particle.transform.position = transform.position;
        particle.Play();
    }
}
