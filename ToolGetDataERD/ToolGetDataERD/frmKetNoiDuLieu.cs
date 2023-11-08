using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolGetDataERD
{
    public partial class frmKetNoiDuLieu : DevExpress.XtraEditors.XtraForm
    {
        public frmKetNoiDuLieu()
        {
            InitializeComponent();
        }

        public static string _db;

        SqlConnection GetCon(string server, string username, string pass, string database)
        {
            return new SqlConnection("Data Source = "+server+"; Initial Catalog="+database+"; " +
                "User ID="+username+"; Password="+pass+";");
        }
        private void frmKetNoiDuLieu_Load(object sender, EventArgs e)
        {

        }

        private void btnKiemTra_Click(object sender, EventArgs e)
        {
            SqlConnection con = GetCon(txtServer.Text, txtUserName.Text, txtPasswd.Text, cboDatabase.Text);
            try
            {
                con.Open();
                MessageBox.Show("Kết nối thành công", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
            catch (Exception)
            {

                MessageBox.Show("Kết nối thất bại", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }

        }

     

        private void cboDatabase_MouseClick(object sender, MouseEventArgs e)
        {
            cboDatabase.Items.Clear();
            string conn = "server=" + txtServer.Text +
                "; User ID=" + txtUserName.Text + "; pwd=" + txtPasswd.Text + ";";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            string qr = "SELECT NAME FROM SYS.DATABASES";
            SqlCommand cmd = new SqlCommand(qr, con);
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cboDatabase.Items.Add(dr[0].ToString());
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dl = MessageBox.Show("Bạn có muốn thoát không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dl == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string svEncrypt = Encryptor.Encrypt(txtServer.Text, "fsfuoufsd8935@!", true);
            string usEncrypt = Encryptor.Encrypt(txtUserName.Text, "fsfuoufsd8935@!", true);
            string passEncrypt = Encryptor.Encrypt(txtPasswd.Text, "fsfuoufsd8935@!", true);
            string dbEncrypt = Encryptor.Encrypt(cboDatabase.Text, "fsfuoufsd8935@!", true);
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "Chọn nơi lưu trữ";
            sf.Filter = "Text file (*.dba)|*.dba|AllFiles(*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                connect cn = new connect(svEncrypt, usEncrypt, passEncrypt, dbEncrypt);
                cn.ConnectData(sf.FileName);
                MessageBox.Show("Lưu file thành công", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
        }

        public string GetConnectionString()
        {
            string server = txtServer.Text;
            string username = txtUserName.Text;
            string pass = txtPasswd.Text;
            string database = cboDatabase.Text;

            return $"data source={server};Initial Catalog={database};User ID={username};Password={pass};";
        }

        private void btnDocFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn tập tin";
            op.Filter = "Text file (*.dba)|*.dba|AllFiles(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(op.FileName, FileMode.Open, FileAccess.Read);
                connect con = (connect)bf.Deserialize(fs);
                string srv = Encryptor.Decrypt(con.servername, "fsfuoufsd8935@!", true);
                string us = Encryptor.Decrypt(con.username, "fsfuoufsd8935@!", true);
                string pa = Encryptor.Decrypt(con.passwd, "fsfuoufsd8935@!", true);
                string db = Encryptor.Decrypt(con.database, "fsfuoufsd8935@!", true);

                txtServer.Text = srv;
                txtUserName.Text = us;
                txtPasswd.Text = pa;
                cboDatabase.Text = db;

                string database = db;
                string server = srv;
                string user = us;

                string connectionString = GetConnectionString();
                frmMain mainForm = new frmMain(connectionString, database, server, user);
                mainForm.ShowDialog();
            }
            this.Close();
        }
    }
}