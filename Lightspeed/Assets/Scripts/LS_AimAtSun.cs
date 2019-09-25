using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_AimAtSun : MonoBehaviour {
    public Transform target;
    public float speed = 10;

    void Start() {
        var sun = GameObject.Find("Sun");
        if(sun == null)
            Debug.LogError("404: Sun not found.");
        target = sun.transform;
    }


    void Update() {
        Vector3 targetDir = target.position - transform.position;
        float step = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
