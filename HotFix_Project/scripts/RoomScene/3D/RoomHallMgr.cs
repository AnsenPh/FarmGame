using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace HotFix_Project
{
    class RoomHallMgr : BaseUIMgr
    {
        GameObject m_Floor;
        List<GameObject> m_FloorCell;
        public override void InitGameObjParam()
        {
            InitFloor();
        }
        //添加监听事件
        public override void AddDataListener()
        {

        }
        //移除监听事件
        public override void RemoveDataListener()
        {

        }


        void InitFloor()
        {
            m_Floor = m_GameObj.transform.Find("Floor").gameObject;
            m_FloorCell = new List<GameObject>();

            float CellLength = 0.2f;

    
            GameObject FirstCell = m_Floor.transform.Find("Cell").gameObject;
            m_FloorCell.Add(FirstCell);
            float StartX = FirstCell.transform.localPosition.x;
            float StartY = FirstCell.transform.localPosition.y;
            float StartZ = FirstCell.transform.localPosition.z;
            for (int row = 0; row < RoomSceneConst.HallMaxRow; row++) 
            {
                for (int col = 0; col < RoomSceneConst.HallMaxCol; col++)
                {
                    if (col == 0 && row == 0)
                    {
                        continue;
                    }

                    float CurrentX = StartX - CellLength * col;
                    float CurrentZ = StartZ - CellLength * row;
                    GameObject TempCell = GameObject.Instantiate(FirstCell, m_Floor.transform);
                    TempCell.transform.localPosition = new Vector3(CurrentX, StartY, CurrentZ);
                    m_FloorCell.Add(TempCell);
                }

            }
        }
    }
}
