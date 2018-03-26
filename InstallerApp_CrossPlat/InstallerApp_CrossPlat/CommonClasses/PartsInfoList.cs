using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat.Droid
{
    public class PartsInfoList : IPartsInfo
    {
        private string _CabinetName, _LFinish, _RFinish;
        private int _PartType, _LabelNo, _CSID;
        public string CabinetName
        {
            get { return this._CabinetName; }
            set { this._CabinetName = value; }
        }
        public string LFinish
        {
            get { return this._LFinish; }
            set { this._LFinish = value; }
        }
        public string RFinish
        {
            get { return this._RFinish; }
            set { this._RFinish = value; }
        }
        public int PartType
        {
            get { return this._PartType; }
            set { this._PartType = value; }
        }

        public int LabelNo
        {
            get { return this._LabelNo; }
            set { this._LabelNo = value; }
        }

        public int CSID
        {
            get { return this._CSID; }
            set { this._CSID = value; }
        }

        public int OrderPartsStatus { get; set; }
    }
}