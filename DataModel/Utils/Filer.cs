using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NLog;

namespace DataModel.Utils
{
    class Filer
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public String filesName;
        public String filesExt;
        public byte codingKey = 127;

        public Filer(String filesName, String filesExt)
        {
            this.filesName = filesName;
            this.filesExt = filesExt;
        }

        public Filer(String filesName, String filesExt, byte codingKey)
        {
            this.filesName = filesName;
            this.filesExt = filesExt;
            this.codingKey = codingKey;
        }

        //calculate directory based on the current date
        protected String calcCompleteDir(String globalDir)
        {
            //get date, and parse it to obtain the directories tree
            DateTime dateNow = DateTime.Today;
            String month = Convert.ToString(dateNow.Month);
            if (month.Length < 2)
            {
                month = "0" + month;
            }
            String year = Convert.ToString(dateNow.Year);

            String dir = globalDir + "\\" + year + "\\" + month;

            return dir;
        }

        //calculate file name based current day
        protected String calcDayFileName()
        {
            DateTime dateNow = DateTime.Today;
            String day = Convert.ToString(dateNow.Day);
            if (day.Length < 2)
            {
                day = "0" + day;
            }
            return filesName + day + filesExt;
        }

        //creates directory if not exist
        protected bool checkDirExists(String dir)
        {
            if (!Directory.Exists(dir))
            {
                //directory inexistent, create
                Directory.CreateDirectory(dir);

                //inexistent
                return false;
            }
            //allready exists
            return true;
        }

        public bool appendToCurrentFile(String line, String globalDir)
        {
            try
            {
                //calculate directory
                String completeDir = calcCompleteDir(globalDir);
                //create directory if it doesn't exist
                checkDirExists(completeDir);

                //add the file name 
                String path = completeDir + "\\" + calcDayFileName();

                //save line to the correspondent file
                File.AppendAllText(path, line + "\n");
            }
            catch //pokemon exception handling
            {
                return false;
            }
            return true;
        }

        protected bool appendObjectToFile(Object obj, String path)
        {
            try
            {
                //open file for writing
                Stream fstream = File.Open(path, FileMode.Append);
                //serialize object
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fstream, obj);//here we serialize and write to file

                fstream.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool appendToCurrentFile(Object obj, String globalDir)
        {
            try
            {
                //calculate directory
                String completeDir = calcCompleteDir(globalDir);
                //create directory if it doesn't exist
                checkDirExists(completeDir);

                //add the file name 
                String path = completeDir + calcDayFileName();

                //save object to the correspondent file
                appendObjectToFile(obj, path);
            }
            catch //pokemon exception handling
            {
                return false;
            }
            return true;
        }

        //Encodes/Decodes a byte stream with XOR function with a given key
        private byte[] encodeXOR(Stream stream, byte key)
        {
            //from the begining
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            //put the stream content into the bytes array
            stream.Read(bytes, 0, bytes.Length);
            //modify (encode) the bytes
            for (int i = 0; i < bytes.Length; i++)
            {
                //XOR every byte		
                bytes[i] = (byte)(bytes[i] ^ key);
            }
            return bytes;
        }

        public bool saveToFile(Object obj, string dir)
        {
            //create directory if it doesn't exist
            checkDirExists(dir);
            string path = dir + "\\" + filesName + "." + filesExt;

            if (saveObjectToFile(obj, path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool saveObjectToFile(Object obj, string path)
        {
            //open file for writing
            Stream fstream = File.Open(path, FileMode.Create);
            try
            {
                Stream serialObj = new MemoryStream();
                //serialize object
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(serialObj, obj);

                //encode object with XOR function
                byte[] bytes = encodeXOR(serialObj, codingKey);

                //copy encoded serialized object to file
                fstream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                //Logger.output("#####ERROR:" + ex.Message);
                logger.Error("#####ERROR Saving object: {0}", ex.Message);
                return false;
            }
            finally
            {
                fstream.Close();
            }

            return true;
        }

        public ObjClass readClassObjectFromFile<ObjClass>(string path)
        {
            //verificar si existe archivo
            if (!File.Exists(path))
            {
                //Logger.output("No existe el archivo: " + path);
                logger.Error("#####ERROR File dosn't exist: {0}", path);
                return default(ObjClass);
            }

            ObjClass obj;

            //open file for reading
            Stream fstream = File.OpenRead(path);
            try
            {
                //decode stream with XOR function
                byte[] bytes = encodeXOR(fstream, codingKey);
                //get the bytes into a stream
                Stream decoded = new MemoryStream();
                decoded.Write(bytes, 0, bytes.Length);

                //deserialize object
                decoded.Position = 0;//from the begining
                BinaryFormatter formatter = new BinaryFormatter();
                obj = (ObjClass)formatter.Deserialize(decoded);
            }
            catch (Exception ex)
            {
                //Logger.output("#####ERROR:" + ex.Message);
                logger.Error("#####ERROR Reading File: {0}", ex.Message);
                return default(ObjClass);
            }
            finally
            {
                fstream.Close();
            }

            return obj;
        }

        public Object readObjectFromFile(string path)
        {
            //verificar si existe archivo
            if (!File.Exists(path))
            {
                //Logger.output("No existe el archivo: " + path);
                logger.Error("#####ERROR File Dosn't exist: {0}", path);
                return null;
            }

            Object obj;

            //open file for reading
            Stream fstream = File.OpenRead(path);
            try
            {
                //decode stream with XOR function
                byte[] bytes = encodeXOR(fstream, codingKey);
                //get the bytes into a stream
                Stream decoded = new MemoryStream();
                decoded.Write(bytes, 0, bytes.Length);

                //deserialize object
                decoded.Position = 0;//from the begining
                BinaryFormatter formatter = new BinaryFormatter();
                obj = (Object)formatter.Deserialize(decoded);
            }
            catch (Exception ex)
            {
                //Logger.output("#####ERROR:" + ex.Message);
                logger.Error("#####ERROR Decoding File: {0}", ex.Message);
                return null;
            }
            finally
            {
                fstream.Close();
            }

            return obj;
        }

    }
}
