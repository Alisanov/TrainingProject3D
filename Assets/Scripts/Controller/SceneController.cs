using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject[] _enemy;
    private float speedChange;

    [SerializeField] private int countEnemy;
    [SerializeField] private float ranMinX = -14f;
    [SerializeField] private float ranMaxX = 2.3f;
    [SerializeField] private float ranMinZ = -3f;
    [SerializeField] private float ranMaxZ = 13f;
       
    private void OnSpeedChanged(float value)
    {
        speedChange = value;
    }
    void Start()
    {
        _enemy = new GameObject[countEnemy];
        for(int i = 0; i < _enemy.Length; i++)
        {
            _enemy[i] = Instantiate(enemyPrefab) as GameObject;
            _enemy[i].transform.position = new Vector3(Random.Range(ranMinX, ranMaxX), 0.1f, Random.Range(ranMinZ, ranMaxZ));
            float angel = Random.Range(0, 360);
            _enemy[i].transform.Rotate(0, angel, 0);
            //Debug.Log("Scene: " + speedChange);
        }
    }
}
