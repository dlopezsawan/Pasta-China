using MobileTowerDefense;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject enemyCanvas;
    GameObject enemy;
    void Start()
    {
        enemy = transform.parent.gameObject;
        enemyCanvas = GameObject.Find("EnemyCanvas");
        transform.SetParent(enemyCanvas.transform);
    }

    private void LateUpdate()
    {
        transform.position = enemy.transform.position;
    }
}
