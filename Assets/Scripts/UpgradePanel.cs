using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public Button close;
    public GameObject Upgrades;
    void Start()
    {
        close.onClick.AddListener(CloseTabel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CloseTabel(){
        Upgrades.SetActive(false);
        Time.timeScale = 1f;
    }
}
