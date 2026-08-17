using System;
using UnityEngine;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{ 
    [SerializeField]
    private RectTransform _blue, _red;

    public RectTransform SpawnItem;

    [SerializeField]
    private Vector3 _spawnItemSize;

    public Button button;

    public string TurnString;

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
        if ( GameManager.instance.TurnNumber == 0)
        {
            TurnString = "Blue";
            SpawnItem = Instantiate(_blue, transform.position, Quaternion.identity);
        }
        else  
        {
            TurnString = "Red";
            SpawnItem = Instantiate(_red, transform.position, Quaternion.identity);
        }
        //Create a red or blue object UI
        _gameManagment.TurnNumber++;
        if (_gameManagment.TurnNumber > 1)
            _gameManagment.TurnNumber = 0;
        SpawnItem.transform.SetParent(gameObject.transform);
        SpawnItem.transform.localScale = _spawnItemSize;
        //Set cell data
        _gameManagment.GameButtonsReader = this;
        _gameManagment.ChechDatas(this);
        button.enabled = false;
    }
}