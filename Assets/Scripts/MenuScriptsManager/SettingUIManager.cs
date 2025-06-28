using UnityEngine;

public class SettingUIManager : MonoBehaviour
{
    public GameObject SettingUI;

    public void openSetting()
    {
        SettingUI.SetActive(true);
    }

    public void closeSetting()
    {
        SettingUI.SetActive(false);
    }


}
