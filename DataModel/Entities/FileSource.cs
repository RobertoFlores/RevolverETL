using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Interfaces;

namespace DataModel.Entities
{
    [Serializable()]
    public class FileSource
    {
        public FileSource(string path, FileType ftype)
        {
            PathFile = path;
            FileType = ftype;
        }
        private string _Name { get; set; }
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }
        public string PathFile { get; set; }
        public FileType FileType { get; set; }
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
