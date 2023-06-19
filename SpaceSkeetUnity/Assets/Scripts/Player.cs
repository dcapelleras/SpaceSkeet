using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    Camera cam;

    List<GameObject> marks = new List<GameObject>(); //can be special explosion effect
    [SerializeField] GameObject shootMark;

    [SerializeField] float shootCooldown = 0.3f;
    float canShoot = 0;
    bool reloading = false;
    [SerializeField] int shootSoundIndex = 0;

    [SerializeField] int maxAmmo = 12;
    [SerializeField] int currentAmmo = 12;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] int damage;

    [SerializeField] TMP_Text cargador;

    private void Awake()
    {
        cam = Camera.main;
        for (int i = 0; i < 20; i++)
        {
            GameObject newMark = Instantiate(shootMark);
            newMark.transform.SetParent(transform);
            newMark.SetActive(false);
            marks.Add(newMark);
            cargador.text = currentAmmo.ToString();
        }
    }

    private void Update()
    {
        if (!reloading)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time > canShoot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentAmmo <= 0)
                {
                    StartCoroutine(ReloadWeapon());
                    return;
                }
                RaycastHit hit;
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
                {
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
                cargador.text = currentAmmo.ToString();
            }
        }
    }

    IEnumerator ReloadWeapon()
    {
        reloading = true;
        //play reload animation
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        cargador.text = currentAmmo.ToString();
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