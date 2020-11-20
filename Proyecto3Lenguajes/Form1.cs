using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
        List<List<int>> listaGlobal = new List<List<int>>();





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
                    
                    if (DBNull.Value.Equals(ss.Rows[j][i]) || ss.Rows[j][i] == " ")
                    {
                        
                    }
                    else
                    {
                        //String d = (String)ss.Rows[i][j];
                        String conexion = "punto(" + i.ToString() + "," + j.ToString() + ").";
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

            string docPath2 = "C:\\Users\\ExtremeTech\\Documents\\Prolog";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath2, "baseDeDatos.pl"), true))
            {
                outputFile.WriteLine(conexion);
            }
        }

        private void guardarBaseDeDatosDinamico()
        {
            File.Delete ("C:\\Users\\ExtremeTech\\Desktop\\TEC\\4to Semestre\\Lenguajes\\ParadigmaLogico\\Proyecto3\\Proyecto3Lenguajes\\Proyecto3Lenguajes\\bin\\Debug\\baseDeDatos.pl");
            File.Delete("C:\\Users\\ExtremeTech\\Documents\\Prolog\\baseDeDatos.pl");
            string docPath = "C:\\Users\\ExtremeTech\\Desktop\\TEC\\4to Semestre\\Lenguajes\\ParadigmaLogico\\Proyecto3\\Proyecto3Lenguajes\\Proyecto3Lenguajes\\bin\\Debug";
            
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "baseDeDatos.pl"),true))
            {
                outputFile.WriteLine(":- dynamic punto/2.");
            }

            string docPath2 = "C:\\Users\\ExtremeTech\\Documents\\Prolog";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath2, "baseDeDatos.pl"), true))
            {
                outputFile.WriteLine(":- dynamic punto/2.");
            }
        }

        private void consultarBoton_Click(object sender, EventArgs e)
        {
            listaGlobal.Clear();
            String resultado = "";

            PlQuery query = new PlQuery("final([1,0],[1,0],X)."); // se hace la consulta

            foreach (PlQueryVariables z in query.SolutionVariables)
            {
                resultado = z["X"].ToString();
            }
            query.NextSolution();
            query.Dispose();

            Console.WriteLine("El retorno  fue ---> " + resultado);
            if(resultado != "")
            {
                string nuevo = resultado.Remove(0, 1);
                string final = nuevo.Remove(nuevo.Length - 1);

                Console.WriteLine("El retorno final fue --->" + final);

                for (int i = 0; i < final.Length; i++)
                {
                    if (final[i].Equals(','))
                    {
                        if (char.IsDigit(final[i - 1]) && char.IsDigit(final[i + 1]))
                        {
                            List<int> subListaAAgregar = new List<int>();
                            int x = (int)Char.GetNumericValue(final[i - 1]);
                            int y = (int)Char.GetNumericValue(final[i + 1]);
                            subListaAAgregar.Add(x);
                            subListaAAgregar.Add(y);
                            listaGlobal.Add(subListaAAgregar);
                        }
                    }
                }

                for (int i2 = 0; i2 < listaGlobal.Count; i2++)
                {
                    Console.WriteLine("");
                    for (int j2 = 0; j2 < listaGlobal[i2].Count; j2++)
                    {
                        Console.Write(listaGlobal[i2][j2]);
                    }
                }
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("no existe ese punto");
            }
            




        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            
            Con estas lineas se crea las sublistas para la lista de listas.
            
            */
            List<List<int>> ListaDeListas = new List<List<int>>();
            List<int> sublista1 = new List<int>();
            List<int> sublista2 = new List<int>();
            List<int> sublista3 = new List<int>();
            List<int> sublista4 = new List<int>();

            sublista1.Add(1);
            sublista1.Add(2);
            sublista1.Add(3);
            sublista1.Add(4);

            sublista2.Add(1);
            sublista2.Add(2);
            sublista2.Add(3);
            sublista2.Add(4);

            sublista3.Add(1);
            sublista3.Add(2);
            sublista3.Add(3);
            sublista3.Add(4);

            sublista4.Add(1);
            sublista4.Add(2);
            sublista4.Add(3);
            sublista4.Add(4);

            ListaDeListas.Add(sublista1);
            ListaDeListas.Add(sublista2);
            ListaDeListas.Add(sublista3);
            ListaDeListas.Add(sublista4);



            /*
            
            Con estas lineas, se remueven las sublistas que tengan datos repetidos.

            */


            for (int i = 0; i<ListaDeListas.Count; i++)
            {
                for (int j = 0; j < ListaDeListas.Count; j++)
                {
                    if (ListaDeListas[i].SequenceEqual(ListaDeListas[j]))
                    {
                        Console.WriteLine("listas iguales, procedo a eliminar 1");
                        ListaDeListas.RemoveAt(j);
                    }
                }
            }


            /*
             
            Con estas lineas se imprime las sublistas. 
             
            */
            for (int i = 0; i < ListaDeListas.Count; i++)
            {
                for (int j = 0; j < ListaDeListas[i].Count; j++)
                {
                    Console.WriteLine(ListaDeListas[i][j]);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listaGlobal.Count; i++)
            {
                dataGridView1.Rows[listaGlobal[i][1]].Cells[listaGlobal[i][0]].Style.BackColor = Color.Red;
            }

            MessageBox.Show("Este grupo contenia: " + listaGlobal.Count + " celdas, se han pintado de rojo", "Informacion adicional", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
