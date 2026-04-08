using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PrudouctSearch : MonoBehaviour
{

    public InputField ProductID;
    public InputField ProductName;
    public InputField ProductType;
    public InputField ProductSize;
    public InputField ProductWorkmanship;

    public GameObject GridItem;
    public Transform GridItemTransform;
    //空字串查詢
    private bool EnableNullInput;
    public List<Product> ProductList = new List<Product>();   // 資料儲存
    //Hiddle List
    public List<GameObject> FilterList;
    public GameObject SearchGrid;
    public GameObject ClearButton;

    string dataName = "Config/ProductData";
    bool increaseOrder = true;

    private void OnEnable()
    {
        OnCloseClear();
        EnableNullInput = true;
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
  
    #region searchProducts

    public void InitGrid()
    {
        DestroyAllChildren();

        List<Product> list = AfterProductSearch();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = UnityEngine.Object.Instantiate(GridItem, GridItemTransform);
            go.SetActive(true);
            ProductItem _item = go.GetComponent<ProductItem>();
            _item.InitData(list[i]);
        }

        // 強制捲動到最頂
        ScrollRect scrollRect = GridItemTransform.parent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;   // 1 = 最頂部，0 = 最底部
        }
    }

    public List<Product> AfterProductSearch()
    {
        // 使用 LINQ 過濾：只有「有填寫」的欄位才會加入條件
        var query = ProductList.Where(p =>
        {
            // 以下條件：如果欄位沒填（空字串），則自動通過 (true)
            // 如果有填寫，則必須完全符合才通過

            bool matchID = string.IsNullOrEmpty(ProductID.text) || p.ProductID.ToString() == ProductID.text;

            bool matchName = string.IsNullOrEmpty(ProductName.text) ||
                             p.ProductName.IndexOf(ProductName.text, System.StringComparison.OrdinalIgnoreCase) >= 0; // 模糊搜尋（不分大小寫）

            bool matchType = string.IsNullOrEmpty(ProductType.text) ||
                             p.ProductType.IndexOf(ProductType.text, System.StringComparison.OrdinalIgnoreCase) >= 0;

            bool matchSize = string.IsNullOrEmpty(ProductSize.text) || p.ProductSize.ToString() == ProductSize.text;

            bool matchWork = string.IsNullOrEmpty(ProductWorkmanship.text) ||
                             p.ProductWorkmanship.IndexOf(ProductWorkmanship.text, System.StringComparison.OrdinalIgnoreCase) >= 0;

            // 全部條件都要成立（AND）
            return matchID && matchName && matchType && matchSize && matchWork;
        });
        List<Product> list = increaseOrder ? query.ToList() : query.Reverse().ToList();
        return list;
    }

    public void DestroyAllChildren()
    {
        foreach (Transform child in GridItemTransform)
        {
            Destroy(child.gameObject);
        }
    }
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

        InitGrid();
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

    public void OrderChange(Transform ts)
    {
        increaseOrder = !increaseOrder;
        for(int i = 0; i < ts.childCount; i++)
        {
            ts.GetChild(0).gameObject.SetActive(increaseOrder);
            ts.GetChild(1).gameObject.SetActive(!increaseOrder);
        }
        OnSearch();
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
