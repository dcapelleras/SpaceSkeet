using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(LineRenderer))]
public class Player : MonoBehaviour
{
    Camera cam;

    List<GameObject> marks = new List<GameObject>(); //can be special explosion effect
    [SerializeField] GameObject shootMark;
    //[SerializeField] Animator armAnimator;

    [SerializeField] float shootCooldown = 0.3f;
    float canShoot = 0;
    bool reloading = false;
    [SerializeField] int shootSoundIndex = 0;

    [SerializeField] int maxAmmo = 12;
    [SerializeField] int currentAmmo = 12;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] int damage;

    [SerializeField] TMP_Text cargador;

    LineRenderer lineRenderer;
    [SerializeField] Material rayMaterial;

    private void Awake()
    {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.material= rayMaterial;
        for (int i = 0; i < 20; i++)
        {
            GameObject newMark = Instantiate(shootMark);
            //newMark.transform.SetParent(transform);
            newMark.SetActive(false);
            marks.Add(newMark);
            cargador.text = "Ammo: " + currentAmmo.ToString();
        }
    }

    private void Update()
    {
        if (!reloading)
        {
            Shoot();
        }
        UpdateRotation();
    }

    void UpdateRotation()
    {
        RaycastHit hitRotation;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitRotation))
        {
            Vector3 dir = hitRotation.point - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = rot;
        }
    }

    void Shoot()
    {
        if (GameManager.instance.gamePaused)
        {
            return;
        }
        if (Time.time > canShoot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentAmmo <= 0)
                {
                    StartCoroutine(ReloadWeapon());
                    return;
                }
                //armAnimator.SetTrigger("Shoot");
                RaycastHit hit;
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    Vector3[] rayPositions = new Vector3[2] {new Vector3(0,0,0), hit.point};
                    lineRenderer.SetPositions(rayPositions);
                    StartCoroutine(ShowRayShot());
                    if (hit.transform.TryGetComponent(out Enemy enemy))
                    {
                        GameObject mark = GetMark();
                        mark.transform.position = hit.point;
                        mark.transform.rotation = Quaternion.LookRotation(hit.normal);
                        //AudioManager.instance.ShootSound(shootSoundIndex);
                        enemy.GetDamaged(damage);
                    }
                }
                canShoot = Time.time + shootCooldown;
                currentAmmo--;
                cargador.text = "Ammo: " + currentAmmo.ToString();
            }
        }
    }

    IEnumerator ShowRayShot()
    {
        yield return new WaitForSeconds(shootCooldown);
        Vector3[] rayPositions = new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
        lineRenderer.SetPositions(rayPositions);
    }

    IEnumerator ReloadWeapon()
    {
        reloading = true;
        //armAnimator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        cargador.text = "Ammo: " + currentAmmo.ToString();
        reloading = false;
    }

    GameObject GetMark()
    {
        foreach (GameObject mark in marks)
        {
            if (!mark.activeInHierarchy)
            {
                mark.SetActive(true);
                return mark;
            }
        }
        GameObject firstMark = marks[0];
        marks.RemoveAt(0);
        marks.Add(firstMark);
        return firstMark;
    }
}