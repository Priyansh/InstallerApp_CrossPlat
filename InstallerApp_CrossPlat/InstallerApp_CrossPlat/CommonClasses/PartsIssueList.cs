using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat.Droid
{
    public class PartsIssueList : IPartsIssueInfo
    {
        private int _PartIssueListID;
        private string _PartDescription;
        
        public int PartIssueListID
        {
            get { return this._PartIssueListID; }
            set { this._PartIssueListID = value; }
        }
        public string PartDescription
        {
            get { return this._PartDescription; }
            set { this._PartDescription = value; }
        }
        public bool IsCbSelected {  get; set;  }
    }
}