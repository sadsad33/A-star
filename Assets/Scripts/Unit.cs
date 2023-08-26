using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Transform target;
    float speed = 2f;
    Vector3[] path;
    int targetIndex;
    float attackDistance = 3f;
    void Start() {
        target = GameObject.Find("Target").GetComponent<Transform>();
    }

    void Update() {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackDistance)
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        else
            Debug.Log("АјАн!!!");
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];
        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length)
                    yield break;
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
