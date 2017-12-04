using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat.Droid
{
    
    public class IndividualRoomList : IIndividualRoomInfo
    {
        private string _RSNo, _CSID, _Rooms, _Style, _Colour, _Hardware, _CounterTop;
        private int _deliveryPhoto, _installationPhoto, _partsCount;

        public string RSNo
        {
            get { return _RSNo; }
            set { this._RSNo = value; }
        }
        public string CSID
        {
            get { return _CSID; }
            set { this._CSID = value; }
        }
        public string Rooms
        {
            get { return this._Rooms; }
            set { this._Rooms = value; }
        }

        public string Style
        {
            get { return this._Style; }
            set { this._Style = value; }
        }

        public string Colour
        {
            get { return this._Colour; }
            set { this._Colour = value; }
        }

        public string Hardware
        {
            get { return this._Hardware; }
            set { this._Hardware = value; }
        }

        public string CounterTop
        {
            get { return this._CounterTop; }
            set { this._CounterTop = value; }
        }

        public int PartsCount
        {
            get { return this._partsCount; }
            set { this._partsCount = value; }
        }

        public int deliveryPhoto
        {
            get { return this._deliveryPhoto; }
            set { this._deliveryPhoto = value; }
        }

        public int installationPhoto
        {
            get { return this._installationPhoto; }
            set { this._installationPhoto = value; }
        }
    }
}