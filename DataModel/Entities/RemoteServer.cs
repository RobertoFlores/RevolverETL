using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Interfaces;

namespace DataModel.Entities
{
    [Serializable()]
    public class RemoteServer 
    {
        public RemoteServer(string raddress, string user, string password, RemoteConnection dbconn)
        {
            RemoteAdress = raddress;
            User = user;
            Password = password;
            RemoteConnection = dbconn;
        }

        private string _Name { get; set; }
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }
        public string RemoteAdress { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public RemoteConnection RemoteConnection { get; set; }

        public void Enable()
        {
            //TBA
        }

        public void Disable()
        {
            //TBA
        }


    }
}
