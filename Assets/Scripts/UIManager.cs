using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField]
    private GameObject tradePanel;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


    public void OpenPanel() {
        tradePanel.SetActive(true);
    }

    public void ClosePanel() {
        tradePanel.SetActive(false);
    }

}
