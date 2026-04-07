using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrudouctSearch : MonoBehaviour {

    public InputField ProductID;
    public InputField ProductName;
    public InputField ProductType;
    public InputField ProductSize;
    public InputField ProductWorkmanship;

    //空字串查詢
    private bool EnableNullInput;
    //Hiddle List
    public List<GameObject> FilterList;
    public GameObject SearchGrid;
    public GameObject ClearButton;


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        OnCloseClear();
        EnableNullInput = false;
        if (null != SearchGrid) SearchGrid.SetActive(false);
    }
    // Use this for initialization
    void Start () {

        
            
	}
	
	

    #region Button
    public void OnSearch()
    {
        //不允許空字串查詢
        if (!EnableNullInput)
        {
            if (null != SearchGrid) SearchGrid.SetActive(false);
            return;
        }
            
        if (null != SearchGrid) SearchGrid.SetActive(true);
    }
    public void OnShowClear(Text input)
    {
        if (!string.IsNullOrEmpty(input.text))
            ClearButton.SetActive(true);
    }
    public void OnEnableNullInput(Toggle tg)
    {
        EnableNullInput = tg.isOn;
    }
    public void OnCloseClear()
    {
        ProductID.text = "";
        ProductName.text = "";
        ProductType.text = "";
        ProductSize.text = "";
        ProductWorkmanship.text = "";
        if(null!= ClearButton) ClearButton.SetActive(false);
    }
    #endregion
}
