using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
    const float LIFETIME = 5;
    public GameObject explo;
    float startTime = 0;

    // Use this for initialization
    void Start() {
        startTime = Time.fixedTime;
    }

    // Update is called once per frame
    void Update() {
        if(Time.fixedTime > startTime + LIFETIME)
            Destroy(gameObject);
    }


    void OnCollisionEnter(Collision col) {

        if(col.gameObject.tag == "Bounds") {
            Physics.IgnoreCollision(col.collider, this.GetComponent<Collider>());
            Destroy(gameObject);
            return;
        }

        GameObject.Instantiate(explo, col.contacts[0].point, Quaternion.identity);
        Debug.Log("Bullet hit:" + col.gameObject.name + "               Child of" + col.gameObject.transform.parent.gameObject.name);
        //if (col.gameObject.transform.parent.gameObject.name == "Destroyable") {
        col.gameObject.transform.parent.gameObject.SendMessage("DecreaseHealth", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
        //}

    }


}
