using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GobangUI : MonoBehaviour
{
    public GameObject window_StartGame;
    public Toggle tog_Player1IsAI;
    public Toggle tog_Player2IsAI;
    public Dropdown dpd_Width;
    public Dropdown dpd_Height;
    public Dropdown dpd_AI1Level;
    public Dropdown dpd_AI2Level;
    public Button btn_StartGame;
    public Button btn_CloseWindow;

    public Text txt_Tips;

    public Button btn_OpenWindow;
    public Button btn_ManualAI;

    static readonly int[] Size = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
    static readonly int[] Level = new int[] { 1, 2, 3, 4, 5 };

    private void Awake()
    {
        btn_StartGame.onClick.AddListener(StartGame);
        btn_ManualAI.onClick.AddListener(ManualAI);
        btn_OpenWindow.onClick.AddListener(OpenWindow);
        btn_CloseWindow.onClick.AddListener(CloseWindow);

        List<string> sizeStr = new List<string>();

        foreach (var item in Size)
        {
            sizeStr.Add(item.ToString());
        }
        dpd_Width.ClearOptions();
        dpd_Width.AddOptions(sizeStr);
        dpd_Width.value = 5;
        dpd_Height.ClearOptions();
        dpd_Height.AddOptions(sizeStr);
        dpd_Height.value = 5;

        List<string> levelStr = new List<string>();

        foreach (var item in Level)
        {
            levelStr.Add(item.ToString());
        }
        dpd_AI1Level.ClearOptions();
        dpd_AI1Level.AddOptions(levelStr);
        dpd_AI1Level.value = 2;
        dpd_AI2Level.ClearOptions();
        dpd_AI2Level.AddOptions(levelStr);
        dpd_AI2Level.value = 2;

        tog_Player1IsAI.isOn = false;

        HideTips();
        CloseWindow();
    }

    private void StartGame()
    {
        GobangManager.Instance.StartGame(Size[dpd_Width.value], Size[dpd_Height.value],
            tog_Player1IsAI.isOn, tog_Player2IsAI.isOn,
            Level[dpd_AI1Level.value], Level[dpd_AI2Level.value]);
        CloseWindow();
    }

    private void ManualAI()
    {
        GobangManager.Instance.ManualAI();
    }

    private void OpenWindow()
    {
        window_StartGame.SetActive(true);
    }

    private void CloseWindow()
    {
        window_StartGame.SetActive(false);
    }

    public void ShowTips(string msg)
    {
        txt_Tips.gameObject.SetActive(true);
        txt_Tips.text = msg;
    }

    public void HideTips()
    {
        txt_Tips.gameObject.SetActive(false);
    }
}
