using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace lab14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbDataAdapter adapter; //объект для связи между Dataset и источником
        int count = 0,rowD=0;
        DataSet dataset; //DataSet отвечает за отображение таблиц используемой базы данных на компьютере пользователя без непрерывной связи с базой данных.
        int paint = 0;
        string conn_param = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=bd.mdb";//загрузка исходной БД

        private void refresh()
        {
            OleDbConnection connection = new OleDbConnection(conn_param);//осуществляет связь с источником данных
            OleDbCommand command = connection.CreateCommand();//обрабатывает запрос к БД
            adapter = new OleDbDataAdapter("Select * from Владельцы", connection);//запрос из БД
            dataset = new DataSet();// инициализация
            adapter.Fill(dataset); // обновление данных из БД и заполнения DataSet
            dataGridView1.DataSource = dataset.Tables[0];// вывод в DatagriedView
        }

        private void Form1_Load(object sender, EventArgs e) //считывание
        {
            refresh();
            OleDbConnection connection = new OleDbConnection(conn_param);//осуществляет связь с источником данных
            OleDbCommand command = connection.CreateCommand();//обрабатывает запрос к БД
            command.CommandText = "select max(Код) from Владельцы;";
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader();//позволяет прочитать строчку запроса
            reader.Read();
            count = reader.GetInt32(0);
            reader.Close();
            connection.Close();

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) // изминение ячейки
        {
            if (paint == 0)
            {
                int row = e.RowIndex, column = e.ColumnIndex;
                updateTable(row, column); // обновление по строке и столбцу
            }
        }

        private void insertInto(int row)
        {
            
                OleDbConnection connection = new OleDbConnection(conn_param);
                connection.Open();
                OleDbCommand command = connection.CreateCommand();
                int rrow = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value.ToString());
                string fio = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string name = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string sur = dataGridView1.Rows[row].Cells[3].Value.ToString();
                string street = dataGridView1.Rows[row].Cells[4].Value.ToString();
                string house = dataGridView1.Rows[row].Cells[5].Value.ToString();
                string all = dataGridView1.Rows[row].Cells[6].Value.ToString();
                string phone = dataGridView1.Rows[row].Cells[7].Value.ToString();
                if (street == "") street = "NULL";
                if (house == "") house = "NULL";
                command.CommandText = "insert into Владельцы values(" + rrow + ",'" + fio + "','" + name + "','" + sur + "'," + street + "," + house + ",'" + all + "','" + phone + "');";
                command.ExecuteNonQuery();
                connection.Close();
            
        }

        private void updateTable(int row, int column)//обновление после действий
        {
            string col = "";
            switch (column)
            {
                case 1: col = "Фамилия"; break;
                case 2: col = "Имя"; break;
                case 3: col = "Отчество"; break;
                case 4: col = "Код_улицы"; break;
                case 5: col = "Номер_дома"; break;
                case 6: col = "Дробная_часть_номер"; break;
                case 7: col = "Телефон"; break;
            }
            string updateString = dataGridView1.Rows[row].Cells[column].Value.ToString();
            OleDbConnection connection = new OleDbConnection(conn_param);
            connection.Open();
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "update Владельцы set " + col + "='" + updateString + "' where Код = " + (row + 1) + ";";
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)//удаление мышкой
        {
            OleDbConnection connection = new OleDbConnection(conn_param);
            connection.Open();
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "delete from Владельцы where Код="+rowD+";";
            command.ExecuteNonQuery();
            connection.Close();
            refresh();
        }

        private void button1_Click(object sender, EventArgs e) // кнопка добавления и сохранения в БД
        {
            paint = 0;
            int row=dataGridView1.RowCount-2;
            insertInto(row);
        }



        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            rowD = e.RowIndex + 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] arr = new string[dataGridView1.RowCount-1];
            for (int i=1;i<dataGridView1.RowCount;i++)
            {
                arr[i - 1] = dataGridView1.Rows[i-1].Cells[7].Value.ToString();
            }
            Phones q = new Phones();
            q.getString(arr);
            q.Show();
        }

       

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            paint = 1;
        }
    }
}
