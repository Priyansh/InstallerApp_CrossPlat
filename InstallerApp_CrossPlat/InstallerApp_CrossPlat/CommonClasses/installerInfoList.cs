using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat.Droid
{
    public class InstallerInfoList : IInstallerInfo
    {
        private string _Company, _Project, _Lot, _JobNum, _MasterNum, _InstallAssignDate, _InstallAssignPerson, _ShippedDone, _InstallerJobStart, _InstallerJobComplete;
        private int _CSID, _InstallerAssignID, _InstallerJobStatus;

        public string Company
        {
            get { return this._Company; }
            set { this._Company = value; }
        }
       
        public string Project
        {
            get { return this._Project; }
            set { this._Project = value; }
        }

        public int CSID
        {
            get { return _CSID; }
            set { _CSID = value; }
        }

        public string Lot
        {
            get { return this._Lot; }
            set { this._Lot = value; }
        }

        public string JobNum
        {
            get { return this._JobNum; }
            set { this._JobNum = value; }
        }

        public string MasterNum
        {
            get { return this._MasterNum; }
            set { this._MasterNum = value; }
        }

        public string InstallAssignDate
        {
            get { return this._InstallAssignDate; }
            set { this._InstallAssignDate = value; }
        }

        public string InstallAssignPerson
        {
            get { return this._InstallAssignPerson; }
            set { this._InstallAssignPerson = value; }
        }

        public string ShippedDone
        {
            get { return this._ShippedDone; }
            set { this._ShippedDone = value; }
        }

        public int InstallerAssignID
        {
            get { return this._InstallerAssignID; }
            set { this._InstallerAssignID = value; }
        }

        public int InstallerJobStatus
        {
            get { return _InstallerJobStatus; }
            set { _InstallerJobStatus = value; }
        }

        public string InstallerJobStart
        {
            get { return _InstallerJobStart; }
            set { _InstallerJobStart = value; }
        }

        public string InstallerJobComplete
        {
            get { return _InstallerJobComplete; }
            set { _InstallerJobComplete = value; }
        }
    }
}