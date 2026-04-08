using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductItem : MonoBehaviour
{
    public Text DataContent;
    int dataID;
    string dataName;
    string dataType;
    int dataSize;
    string dataQuality;

    public void InitData(Product data)
    {
        if (null == data)
            return;
        dataID = data.ProductID;
        dataName = data.ProductName;
        dataType = data.ProductType;
        dataSize = data.ProductSize;
        dataQuality = data.ProductWorkmanship;

        DataContent.text = string.Format("編號: {0} | 名稱: {1} | 類型: {2} 尺寸: {3} | 工藝品質: {4}", dataID, dataName, dataType, dataSize, dataQuality);
    }
}
