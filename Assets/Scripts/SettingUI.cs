using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameObject SettingGO;


    private void Start()
    {
        if(SettingGO != null && SettingGO.activeSelf)
            closeSettingUI();
    }
    public void openSettingUI()
    {
        if (SettingGO != null)
        {
            SettingGO.SetActive(true);
        }
    }

    public void closeSettingUI()
    {
        if (SettingGO != null)
        {
            SettingGO.SetActive(false);
        }
    }

}//SettingUI
