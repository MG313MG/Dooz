using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public int RowsandCols;
    public RectTransform ShowWindow;
    public RectTransform[] HideWindows;
    private GameManager _gameManagment;

    private void Awake()
    {
        _gameManagment = FindObjectOfType<GameManager>();
    }

    public void OnClickMe()
    {
        _gameManagment.UIButtonsReader = this;
        if (RowsandCols != 0)
            _gameManagment.SetGameBoardData();
    }
}