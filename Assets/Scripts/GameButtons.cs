using System;
using UnityEngine;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{ 
    [SerializeField]
    private RectTransform Blue, Red;

    public RectTransform SpawnItem;

    [SerializeField]
    private Vector3 _spawnItemSize;

    public Button button;

    public string Turn;

    public int Row, Column;

    private GameManager _gameManagment;

    private void Awake()
    {
        button = GetComponent<Button>();
       
    }

    public void OnClickMe()
    {
        //Change turn and instantiate UI Item
        //0 = Blue
        //1 = Red
        if ( GameManager.instance.Turn == 0)
        {
            Turn = "Blue";
            SpawnItem = Instantiate(Blue, transform.position, Quaternion.identity);
        }
        else  
        {
            Turn = "Red";
            SpawnItem = Instantiate(Red, transform.position, Quaternion.identity);
        }
        _gameManagment.Turn++;
        if (_gameManagment.Turn > 1)
            _gameManagment.Turn = 0;
        SpawnItem.transform.SetParent(gameObject.transform);
        SpawnItem.transform.localScale = _spawnItemSize;
        //Set cell data
        _gameManagment.GameButtonsReader = this;
        _gameManagment.ChechDatas(this);
        button.enabled = false;
    }
}