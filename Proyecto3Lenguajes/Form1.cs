﻿using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using SbsSW.SwiPlCs;


/*
    PlQuery consulta = new PlQuery("familia(X)");
    foreach (PlQueryVariables x in consulta.SolutionVariables)
    listBox1.Items.Add(x["X"].ToString());
*/

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


        /*
        
        Esta funcion crea la dataTable, que contendra la matriz con todas las
        x que se crean de manera aleatoria o con el usuario.
         
        */
        private void button1_Click(object sender, EventArgs e)
        { 
            ss.Columns.Clear();
            ss.Rows.Clear();

            int nSize = int.Parse(sizeN.Text);
            for(int i = 0; i<nSize; i++)
            {
                ss.Columns.Add();       
            }
            for (int j = 0; j < nSize; j++)
            {
                ss.Rows.Add();
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowDrop = false;
            dataGridView1.DataSource = ss;
            dataGridView1.CellClick += clickDataTable;
        }

        /*
         
        Funcion que genera de manera aleatoria una combinacion de x en la matriz. 
         
        */
        private void button2_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Green;
            //dataGridView1.Rows[1].DefaultCellStyle.BackColor = Color.Green;
            //dataGridView1.Rows[2].DefaultCellStyle.BackColor = Color.Red;
            int nSize = int.Parse(sizeN.Text);

            for (int i = 0; i < nSize; i++)
            {
                for (int j = 0; j < nSize; j++)
                {
                    ss.Rows[i][j] = " ";
                }
            }

            Random rnd = new Random();
            int numeroDePuntos = rnd.Next(0, nSize*nSize);

            for (int i = numeroDePuntos; i>0; i--)
            {
                int x = rnd.Next(0, nSize);
                int y = rnd.Next(0, nSize);
                ss.Rows[x][y] = "x";
            }
        }

        private void sizeN_TextChanged(object sender, EventArgs e)
        {

        }

        /*
         
        Funcion que detecta el click en la datatable.
    
        */
        private void clickDataTable(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value == "x")
            {
                cell.Value = " ";
            }
            else
            {
                cell.Value = "x";
            }
        }


        /*
        
        Las siguientes 3 funciones generan la base de datos que se utilizara en prolog para
        los diferentes grupos disponibles en la matriz.
         
        */
        private void button1_Click_1(object sender, EventArgs e)
        {
            int nSize = int.Parse(sizeN.Text);
            guardarBaseDeDatosDinamico();

            for (int i = 0; i < nSize; i++)
            {
                for (int j = 0; j < nSize; j++)
                {
                    if (DBNull.Value.Equals(ss.Rows[i][j]) || ss.Rows[i][j] == " ")
                    {
                    }
                    else
                    {
                        //String d = (String)ss.Rows[i][j];
                        String conexion = "conexion("+i.ToString()+ "," + j.ToString()+").";
                        guardarBaseDeDatos(conexion);
                    }
                }
            }
        }

        private void guardarBaseDeDatos(string conexion)
        {
            string docPath = "C:\\Users\\ExtremeTech\\Desktop\\TEC\\4to Semestre\\Lenguajes\\ParadigmaLogico\\Proyecto3\\Proyecto3Lenguajes\\Proyecto3Lenguajes\\bin\\Debug";

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "baseDeDatos.pl"),true))
            {
                outputFile.WriteLine(conexion);
            }
        }

        private void guardarBaseDeDatosDinamico()
        {
            File.Delete ("C:\\Users\\ExtremeTech\\Desktop\\TEC\\4to Semestre\\Lenguajes\\ParadigmaLogico\\Proyecto3\\Proyecto3Lenguajes\\Proyecto3Lenguajes\\bin\\Debug\\baseDeDatos.pl");

            string docPath = "C:\\Users\\ExtremeTech\\Desktop\\TEC\\4to Semestre\\Lenguajes\\ParadigmaLogico\\Proyecto3\\Proyecto3Lenguajes\\Proyecto3Lenguajes\\bin\\Debug";

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "baseDeDatos.pl"),true))
            {
                outputFile.WriteLine(":- dynamic conexion/2.");
            }
        }

        private void consultarBoton_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(resultado.NextSolution().ToString());
            PlQuery resultado = new PlQuery("comprobar(0,2).");
            if (resultado.NextSolution() == true)
                Console.WriteLine("fue true");
            else
                Console.WriteLine("fue false");
        }
    }
}
