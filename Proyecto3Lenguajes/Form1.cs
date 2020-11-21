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

        //List<List<int>> listaTemporal = new List<List<int>>();
        List<List<List<int>>> listaGlobal = new List<List<List<int>>>();





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
            int nSize = int.Parse(sizeN.Text);
            for (int iMatriz = 0; iMatriz < nSize; iMatriz++)
            {
                //AQUI EN ESTE FOR SE OBTIENEN TODOS LOS PARES ORDENADOS DE LA MATRIZ.
                for (int jMatriz = 0; jMatriz < nSize; jMatriz++)
                {
                    String resultado = funcionConsulta(iMatriz.ToString(),jMatriz.ToString());

                    //Console.WriteLine(resultado);
                    
                    if (resultado != "" && resultado != "[]")
                    {
                        string nuevo = resultado.Remove(0, 1);
                        string final = nuevo.Remove(nuevo.Length - 1);

                        //Console.WriteLine("El retorno final fue --->" + final);

                        List<List<int>> listaTemporal = new List<List<int>>();

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
                                    listaTemporal.Add(subListaAAgregar);
                                }
                            }
                        }

                        if (!listaGlobal.Contains(listaTemporal))
                        {
                            Console.WriteLine("entra al contains");
                            listaGlobal.Add(listaTemporal);
                        }
                        
                        

                        /*
                        for (int i2 = 0; i2 < listaGlobal.Count; i2++)
                        {
                            Console.WriteLine("Este es el grupo -> " + i2);
                            for (int j2 = 0; j2 < listaGlobal[i2].Count; j2++)
                            {
                                Console.WriteLine("");
                                for (int q2 = 0; q2 < listaGlobal[i2][j2].Count; q2++)
                                {
                                    Console.Write(listaGlobal[i2][j2][q2]);
                                }
                            }
                        }
                        */

                        //Console.WriteLine("");
                    }
                    else if (resultado == "[]")
                    {
                        Console.WriteLine("entro");
                        List<List<int>> listaTemporal = new List<List<int>>();
                        List<int> subListaAAgregar = new List<int>();

                        subListaAAgregar.Add(iMatriz);
                        subListaAAgregar.Add(jMatriz);

                        listaTemporal.Add(subListaAAgregar);

                        if (!listaGlobal.Contains(listaTemporal))
                        {
                            Console.WriteLine("entra al contains2");
                            listaGlobal.Add(listaTemporal);
                        }

                    }
                    else
                    {
                        //Console.WriteLine("no existe el punto");
                    }
                }
            
            }

            /*
            for (int i2 = 0; i2 < listaGlobal.Count; i2++)
            {
                Console.WriteLine("Este es el grupo -> " + i2);
                for (int j2 = 0; j2 < listaGlobal[i2].Count; j2++)
                {
                    Console.WriteLine("");
                    for (int q2 = 0; q2 < listaGlobal[i2][j2].Count; q2++)
                    {
                        Console.Write(listaGlobal[i2][j2][q2]);
                    }
                }
            }
            */

            /*
            for (int q = 0; q < listaGlobal.Count; q++)
            {
                for (int j = 0; j < listaGlobal.Count; j++)
                {
                    if (listaGlobal[q].SequenceEqual(listaGlobal[j]))
                    {
                        Console.WriteLine("entroooo");
                        listaGlobal.RemoveAt(j);
                    }
                }
            }
            
            */






        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < listaGlobal.Count; i++)
            {
                int r = random.Next(0, 256);
                int g = random.Next(0, 256);
                int b = random.Next(0, 256);

                for (int j = 0; j < listaGlobal[i].Count; j++)
                {
                    dataGridView1.Rows[listaGlobal[i][j][1]].Cells[listaGlobal[i][j][0]].Style.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                }
                    
            }

            //MessageBox.Show("Este grupo contenia: " + listaGlobal[0].Count + " celdas, se han pintado de rojo", "Informacion adicional", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private String funcionConsulta(string x, string y)
        {
            string consulta = "final([" + x + "," + y + "],[" + x + "," + y + "],X).";

            //Console.WriteLine(consulta);

            PlQuery query = new PlQuery(consulta); // se hace la consulta
            String resultado = "";

            foreach (PlQueryVariables z in query.SolutionVariables)
            {
                resultado = z["X"].ToString();
            }
            query.NextSolution();
            query.Dispose();

            return resultado;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine(listaGlobal.Count);
            Console.WriteLine(listaGlobal[0].Count);
            Console.WriteLine(listaGlobal[0].Count);

        }
    }
}
