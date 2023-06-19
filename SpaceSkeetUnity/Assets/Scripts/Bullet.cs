using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //[SerializeField] int lifeTimeInMiliseconds = 1500;
    [SerializeField] float lifeTime = 1.5f;

    private async void OnEnable()
    {
        //await Task.Run(() => Disable());
        //await Task.Delay(lifeTimeInMiliseconds);
        //Disable();
        StartCoroutine(Disable());
    }

    /*void Disable()
    {
        gameObject.SetActive(false);
    }*/

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}