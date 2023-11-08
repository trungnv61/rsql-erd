﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ToolGetDataERD
{
    [Serializable]
    public class connect
    {
        public string servername;

        public string Servername
        {
            get { return servername; }
            set { servername = value;}
        }

        public string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string passwd;

        public string Passwd
        {
            get { return passwd; }
            set { passwd = value; }
        }

        public string database;

        public string Database
        {
            get { return database; }
            set { database = value; }
        }

        public connect(string _serverName, string _userName, string _password, string _database)
        {
            this.servername = _serverName;
            this.username = _userName;
            this.passwd = _password;
            this.database = _database;
        }

        public void ConnectData(string fileName)
        {
            if (File.Exists("connectdb.dba"))
                File.Delete("connectdb.dba");
            FileStream fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this);
            fs.Close();
        }
    }
}
