using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SbsSW.SwiPlCs;


namespace Proyecto3Lenguajes
{
    public partial class Form1 : Form
    {
        DataTable ss = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Environment.SetEnvironmentVariable("SWI_HOME_DIR", @"C:\\Program Files (x86)\\swipl");
            Environment.SetEnvironmentVariable("Path", @"C:\\Program Files (x86)\\swipl\\bin");
            string[] p = { "-q", "-f", @"prueba.pl" };
            // Connect to Prolog Engine
            PlEngine.Initialize(p);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlQuery consulta = new PlQuery("familia(X)");
            foreach (PlQueryVariables x in consulta.SolutionVariables)
                listBox1.Items.Add(x["X"].ToString());


            
            ss.Columns.Add();
            ss.Columns.Add();

            ss.Rows.Add("", "X");
            ss.Rows.Add("X", "X");


            dataGridView1.DataSource = ss;

            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Green;
            dataGridView1.Rows[1].DefaultCellStyle.BackColor = Color.Green;
            dataGridView1.Rows[2].DefaultCellStyle.BackColor = Color.Red;

            if (DBNull.Value.Equals(ss.Rows[0][1]))
            {
                Console.WriteLine("el campo es nulo");
            }
            else
            {
                String d = (String)ss.Rows[0][0];
                Console.WriteLine(d);
            }

        }
    }
}
