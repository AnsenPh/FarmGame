using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFix_Project
{
    class CSVReader:Singleton<CSVReader>
    {
        private string[][] m_ItemDataArray;
        public void LoadItemData()
        {
            TextAsset ItemData = ABManager.LoadAssetFromAB("src/common/Data", "ItemData") as TextAsset;
            string[] lineArray = ItemData.text.Split("\r"[0]);

            //创建二维数组
            m_ItemDataArray = new string[lineArray.Length][];

            //把csv中的数据储存在二位数组中
            for (int i = 0; i < lineArray.Length; i++)
            {
                m_ItemDataArray[i] = lineArray[i].Split(',');
            }
        }

        public string GetItemData(int _Id, string _Name)
        {
            return GetItemDataByIdAndName(_Id , _Name , m_ItemDataArray);
        }

        string GetItemDataByIdAndName(int _Id, string _Name , string[][] _Src)
        {
            if (_Src.Length <= 0)
                return "";

            int nRow = _Src.Length;
            int nCol = _Src[0].Length;
            for (int i = 1; i < nRow; ++i)
            {
                string strId = string.Format("\n{0}", _Id);
                if (_Src[i][0] == strId)
                {
                    for (int j = 0; j < nCol; ++j)
                    {
                        if (_Src[0][j] == _Name)
                        {
                            return _Src[i][j];
                        }
                    }
                }
            }
            return "";
        }
    }
}
