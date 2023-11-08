using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolGetDataERD
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        public frmMain()
        {
            InitializeComponent();
            InitializeGridControl();
        }

        
        public frmMain(string connectionString, string database, string server, string user)
        {
            InitializeComponent();
            this._connectionString = connectionString;
            this._database = database;
            this._server = server;
            this._user = user;
            InitializeGridControl();
            lblDatabase.Text = _database;
            lblUserName.Text = _user;
            lblServer.Text = _server;
        }

        //private string connectionString = "data source=SHERWINNGUYEN\\SQLEXPRESS01;initial catalog=QUANLYKS;user id=sa;password=123456;";


        private string _initialConnectionString;
        private string _connectionString;
        private string _database;
        private string _server;
        private string _user;


        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadDataToGridControl();
        }

        private void InitializeGridControl()
        {
            // Thiết lập thuộc tính cho GridView
            gvDanhSach.OptionsBehavior.Editable = false;
        }

        private void LoadDataToGridControl()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 'sqlserver' dbms, t.TABLE_CATALOG, t.TABLE_SCHEMA, t.TABLE_NAME, c.COLUMN_NAME, c.ORDINAL_POSITION,
                        c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH, n.CONSTRAINT_TYPE, k2.TABLE_SCHEMA, k2.TABLE_NAME, k2.COLUMN_NAME
                        FROM INFORMATION_SCHEMA.TABLES t
                        LEFT JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_CATALOG=c.TABLE_CATALOG AND t.TABLE_SCHEMA=c.TABLE_SCHEMA AND t.TABLE_NAME=c.TABLE_NAME
                        LEFT JOIN (INFORMATION_SCHEMA.KEY_COLUMN_USAGE k JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS n ON k.CONSTRAINT_CATALOG=n.CONSTRAINT_CATALOG AND k.CONSTRAINT_SCHEMA=n.CONSTRAINT_SCHEMA AND k.CONSTRAINT_NAME=n.CONSTRAINT_NAME
                        LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r ON k.CONSTRAINT_CATALOG=r.CONSTRAINT_CATALOG AND k.CONSTRAINT_SCHEMA=r.CONSTRAINT_SCHEMA AND k.CONSTRAINT_NAME=r.CONSTRAINT_NAME)
                        ON c.TABLE_CATALOG=k.TABLE_CATALOG AND c.TABLE_SCHEMA=k.TABLE_SCHEMA AND c.TABLE_NAME=k.TABLE_NAME AND c.COLUMN_NAME=k.COLUMN_NAME
                        LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE k2 ON k.ORDINAL_POSITION=k2.ORDINAL_POSITION AND r.UNIQUE_CONSTRAINT_CATALOG=k2.CONSTRAINT_CATALOG AND r.UNIQUE_CONSTRAINT_SCHEMA=k2.CONSTRAINT_SCHEMA AND r.UNIQUE_CONSTRAINT_NAME=k2.CONSTRAINT_NAME
                        WHERE t.TABLE_TYPE='BASE TABLE';
                    ";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Gán dữ liệu cho GridControl
                        gcControl.DataSource = dataTable;
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCsv_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "(.csv)| *.csv";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                gcControl.ExportToCsv(sf.FileName);
                MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdateDB_Click(object sender, EventArgs e)
        {
            frmKetNoiDuLieu frm = new frmKetNoiDuLieu();
            frm.ShowDialog();
        }

        private void gvDanhSach_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle % 2 == 0)
            {
                e.Appearance.BackColor = Color.DeepPink;
                e.Appearance.BackColor = ColorTranslator.FromHtml("#f5e7e6");
            }
        }

        private void btnTxt_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "(.txt)| *.txt";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                gcControl.ExportToText(sf.FileName);
                MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}