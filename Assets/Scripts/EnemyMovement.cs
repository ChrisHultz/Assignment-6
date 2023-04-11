using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent enemy;
    public GameObject player;

    // Start is called before the first frame update
    void Start() {

        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        enemy.SetDestination(player.transform.position);
    }
}
