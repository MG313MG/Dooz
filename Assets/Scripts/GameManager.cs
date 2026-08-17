using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  static GameManager instance;
    public Animator[] anim;

    [SerializeField]
    private AudioSource _adusioSource;

    [Space(10)]
    [SerializeField]
    private AudioClip _buttonsClick, Win;

    [Space(10)]
    [SerializeField]
    private RectTransform startingMenu, selectModes, draw, blue_Win, red_Win;

    [Space(10)]
    [SerializeField]
    private RectTransform Game_3v3, Game4v4, Game5v5, GameRandom;

    [Space(10)]
    public UIButtons UIButtonsReader;
    public GameButtons GameButtonsReader;

    public GameButtons[] _GameButtons;

    [Space(10)]
    public int Turn;

    public  string[,] GameBoardData;

    private int _row;
    private int _col;
    private string _gameName;

    [SerializeField]
    private float _boardCells;
    [SerializeField]
    private float _cellsFilled;
    [SerializeField]
    private bool _isWinned;

    private void Awake()
    {
        _boardCells = 1;
    }
    private void FixedUpdate()
    {
        for(int i = 0; i < anim.Length; i++)
        {
            anim[i].SetFloat("Blue/Red", Turn);
        }
        if (_cellsFilled >= _boardCells)
        {
            draw.gameObject.SetActive(true);
            Debug.Log("Draw");
        }
    }

    //Play sound
    private void _playSound(AudioClip audio)
    {
        _adusioSource.PlayOneShot(audio);
    }

    //Game and resault

    public void CheckWinner()
    {
        //Check Rows
        for (int row = 0; row < _row; row++)
        {
            if (CheckLine(row, 0, 0, 1))
                return;
        }

        //Check columns
        for (int col = 0; col < _col; col++)
        {
            if (CheckLine(0, col, 1, 0))
                return;
        }

        //Check radios
        if (CheckLine(0, 0, 1, 1))
            return;

        if (CheckLine(0, _col - 1, 1, -1))
            return;
    }

    public void CheckDraw()
    {
        _boardCells = _row * _col;
        if (!_isWinned)
            _cellsFilled += 0.5f;
    }

    public void DestroyAllUISpawnedItems()
    {
        foreach (GameButtons gamebutton in _GameButtons)
        {
            if (gamebutton.SpawnItem != null)
                Destroy(gamebutton.SpawnItem.gameObject);
        }
    }

    private void AssignButtons()
    {
        if (_row == 3)
        {
            _gameName = "3v3";
            this._GameButtons = FindObjectsOfType<GameButtons>();
        }
        else if (_row == 4)
        {
            _gameName = "4v4";
            this._GameButtons = FindObjectsOfType<GameButtons>();
        }
        else
        {
            _gameName = "5v5";
            this._GameButtons = FindObjectsOfType<GameButtons>();
        }
    }
    public bool CheckLine(int startRow, int startCol, int rowStep, int colStep)
    {
        string firstPlayer = GameBoardData[startRow, startCol];

        if (firstPlayer == "" || firstPlayer == "Empty")
            return false;

        for (int i = 1; i < _row; i++)
        {
            int currentRow = startRow + i * rowStep;
            int currentCol = startCol + i * colStep;

            if (GameBoardData[currentRow, currentCol] != firstPlayer)
                return false;
        }
        AnnounceWinner(firstPlayer);
        return true;
    }

    void AnnounceWinner(string player)
    {
        Debug.Log("Check");
        if (player == "Blue")
        {
            blue_Win.gameObject.SetActive(true);
            _isWinned = true;
        }
        else if (player == "Red")
        {
            red_Win.gameObject.SetActive(true);
            _isWinned = true;
        }
    }

    public bool IsValidPosition(int row, int col)
    {
        return row >= _row && col >= _col;
    }

    //Set data an turn
    public void ChechDatas(GameButtons button)
    {
        //Set turn animations
        foreach (Animator animator in anim)
        {
            animator.SetFloat("Blue/Red", Turn);
        }
        //Play sound
        _playSound(_buttonsClick);
        //Set datas
        GameBoardData[button.Row, button.Column] = button.Turn;
        Debug.Log($"Row : {button.Row}, Column : {button.Column}, Turn : {button.Turn}");
        //Check who win or draw
        CheckWinner();
        CheckDraw();
    }

    public void SelectTurn()
    {
        int _rnd = Random.Range(0, 2);
        ChangeTurn(_rnd);
    }
    private void ChangeTurn(int _turn)
    {
        //0 = Blue
        //1 = Red
        if (_turn == 0)
        {
            Turn = 0;
        }
        else
        {
            Turn = 1;
        }
        print(Turn);
    }

    public void SetGameBoardData()
    {
        SelectTurn();
        this._row = UIButtonsReader.RowsandCols;
        this._col = UIButtonsReader.RowsandCols;
        GameBoardData = new string[_row, _col];
        for (int row = 0; row < _row; row++)
        {
            for (int col = 0; col < _col; col++)
            {
                GameManager.instance.GameBoardData[row, col] = "Empty";
            }
        }
        _boardCells = _row * _col;
    }

    public void ResetGameBoardData()
    {
        _isWinned = false;
        _cellsFilled = 0;
        for (int row = 0; row < _row; row++)
        {
            for (int col = 0; col < _col; col++)
            {
                GameBoardData[row, col] = "Empty";
            }
        }
        foreach (GameButtons gamebutton in _GameButtons)
        {
            gamebutton.button.enabled = true;
        }
        DestroyAllUISpawnedItems();
    }

    //UI
    public void ActiveandInactiveWindows()
    {

        RectTransform _showWindow = UIButtonsReader.ShowWindow;

        RectTransform[] _hideWindows = UIButtonsReader.HideWindows;

        _playSound(_buttonsClick);

        foreach (var window in _hideWindows)
        {
            if (window != null)
            {
                window.gameObject.SetActive(false);
            }
        }
        if (_showWindow != null)
            _showWindow.gameObject.SetActive(true);
        AssignButtons();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
