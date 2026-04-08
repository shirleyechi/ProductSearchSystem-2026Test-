using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PrudouctSearch : MonoBehaviour
{

    public InputField ProductID;
    public InputField ProductName;
    public InputField ProductType;
    public InputField ProductSize;
    public InputField ProductWorkmanship;

    //空字串查詢
    private bool EnableNullInput;
    public List<Product> ProductList = new List<Product>();   // 資料儲存
    //Hiddle List
    public List<GameObject> FilterList;
    public GameObject SearchGrid;
    public GameObject ClearButton;

    string dataName = "Config/ProductData";
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
    void Start()
    {
        LoadProducts();
    }
    #region load data
    [System.Serializable]
    public class ProductWrapper
    {
        public Product[] products;
    }
    void LoadProducts()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(dataName);
        if (null == textAsset)
        {
            Debug.LogError("找不到 txt 檔案！");
            return;
        }

        string json = textAsset.text;           // 整個檔案內容
        Debug.Log("檔案內容：\n" + json);

        // json解析
        ProductWrapper wrapper = JsonUtility.FromJson<ProductWrapper>(json);
        if (wrapper != null && wrapper.products != null)
        {
            ProductList = new List<Product>(wrapper.products);

            Debug.Log("成功讀取" + ProductList.Count + "筆商品資料！");

            // 印出檢查
            foreach (Product p in ProductList)
            {
                Debug.Log(string.Format("編號: {0} | 名稱: {1} | 類型: {2} 尺寸: {3} | 工藝品質: {4}", p.ProductID, p.ProductName, p.ProductType, p.ProductSize, p.ProductWorkmanship));
            }
        }
        else
        {
            Debug.LogError("JSON 解析失敗！請檢查 txt 格式是否正確。");
        }
    }
    #endregion

    #region filterProducts

    #endregion

    #region searchProducts

    #endregion


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
        if (null != ClearButton) ClearButton.SetActive(false);
    }
    #endregion
}
