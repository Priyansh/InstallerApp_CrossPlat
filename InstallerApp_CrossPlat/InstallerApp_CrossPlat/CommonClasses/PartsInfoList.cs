using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat.Droid
{
    public class PartsInfoList : IPartsInfo
    {
        private string _CabinetName, _LFinish, _RFinish;
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
    }
}