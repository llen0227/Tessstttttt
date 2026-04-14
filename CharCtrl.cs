using System.Collections;
using UnityEngine;

public class CharCtrl : MonoBehaviour 
{
    public GameObject bloodEffect;
    public Weapon weapon;
    public int Hp;

    // Use this for initialization
    void Start()
    {
      //  weapon.isMine = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //  if(collision.collider.gameObject)
        /* if (collision.gameObject.tag == "aaa")
           {

           }
           if (collision.collider.gameObject.tag=="aaa")
           {

           }*/
        if (collision.gameObject.tag == "Weapon"
            && !collision.gameObject.GetComponent<Weapon>().isMine)
        {
            Damage(collision.contacts[0].point, weapon.power);

        }
    }

    public void Damage(Vector3 pos, int damage)
    {
        StartCoroutine(this.CreateBloodEffect(pos, damage));
    }

    IEnumerator CreateBloodEffect(Vector3 pos, int damage)
    {

        //Debug.Log(11);
        Instantiate(bloodEffect, pos, Quaternion.identity);

        Hp -= damage;

        yield return null;

    }
}

