using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BowAttack : MonoBehaviour
{
    [HideInInspector]
    public int selectedQuiver;
    [Header("Ammunition Settings")]
    public float baseArrowRechargeTime;
    public Quiver[] quivers;
    public ParticleSystem[] shotParticles;

    [Header("Firing Mechanisms")]
    public Transform spawnPoint;
    public bool canShoot;
    public float coolTime;
    private float lastShot;

    [Header("UI Settings")]
    public TMP_Text ammoText;
    public Image ammoImage;
    public Color[] imageColors;

    //script components
    private Animator anim;
    private MousePosition mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        mousePosition = GetComponent<MousePosition>();
        FillAllQuivers();
        UpdateArrowUI();
        InvokeRepeating("RefillArrows", 1, baseArrowRechargeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (lastShot > coolTime && canShoot)
            {
                if (quivers[selectedQuiver].arrowCount <= 0) return;

                anim.Play("Shoot");
                shotParticles[selectedQuiver].Play();
                var targetPos = mousePosition.GetPosition(Camera.main) - transform.position;
                targetPos.y = transform.position.y - 0.5f;
                Instantiate(quivers[selectedQuiver].arrow, spawnPoint.position, Quaternion.LookRotation(targetPos));
                lastShot = 0;
                quivers[selectedQuiver].arrowCount--;
                UpdateArrowUI();
            }
        }
        lastShot += Time.deltaTime;
    }
    void RefillArrows()
    {
        if (quivers[0].arrowCount < quivers[0].maxArrows) quivers[0].arrowCount++;
        UpdateArrowUI();
    }
    void FillAllQuivers()
    {
        foreach (Quiver q in quivers)
        {
            q.arrowCount = q.maxArrows;
        }
    }
    public void UpdateArrowUI()
    {
        if (ammoText == null) return;
        ammoText.text = quivers[selectedQuiver].arrowCount.ToString();
        ammoImage.color = imageColors[selectedQuiver];
    }
}
