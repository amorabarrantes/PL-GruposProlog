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

        List<String> listaComprobacion = new List<string>();

        Boolean banderaClick = false;



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
            ss.Clear();
            ss.Columns.Clear();
            ss.Rows.Clear();
            dataGridView1.CellClick -= clickDataTable;


            int nSize = int.Parse(sizeN.Text);
            if(nSize <= 10)
            {
                for (int i = 0; i < nSize; i++)
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
                dataGridView1.ClearSelection();


                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.Width = 48;
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 48;
                }
            }
            else
            {
                MessageBox.Show("No se permite mayor a 10x10 la matriz", "Precaucion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        /*
         
        Funcion que genera de manera aleatoria una combinacion de x en la matriz. 
         
        */
        private void button2_Click(object sender, EventArgs e)
        {
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
            if(banderaClick == true)
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
            else
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value.ToString().Contains("x"))
                {
                    string resultado = funcionConsulta(e.ColumnIndex.ToString(), e.RowIndex.ToString());
                    Console.WriteLine(resultado);

                    if (resultado != "" && resultado != "[]")
                    {
                        string nuevo = resultado.Remove(0, 1);
                        string final = nuevo.Remove(nuevo.Length - 1);
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

                        Random random = new Random();
                        int r = random.Next(0, 256);
                        int g = random.Next(0, 256);
                        int b = random.Next(0, 256);
                        for (int i3 = 0; i3 < listaTemporal.Count; i3++)
                        {
                            dataGridView1.Rows[listaTemporal[i3][1]].Cells[listaTemporal[i3][0]].Style.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                        }


                        MessageBox.Show("El grupo seleccionado consta de: " + listaTemporal.Count + " elementos.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listaTemporal.Clear();
                    }
                    else if (resultado == "[]")
                    {


                        List<List<int>> listaTemporal = new List<List<int>>();
                        List<int> subListaAAgregar = new List<int>();

                        subListaAAgregar.Add(e.ColumnIndex);
                        subListaAAgregar.Add(e.RowIndex);

                        listaTemporal.Add(subListaAAgregar);

                        Random random = new Random();
                        int r = random.Next(0, 256);
                        int g = random.Next(0, 256);
                        int b = random.Next(0, 256);
                        for (int i3 = 0; i3 < listaTemporal.Count; i3++)
                        {
                            dataGridView1.Rows[listaTemporal[i3][1]].Cells[listaTemporal[i3][0]].Style.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                        }

                        MessageBox.Show("El grupo seleccionado consta de: " + listaTemporal.Count + " elementos.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listaTemporal.Clear();

                    }


                }
                else
                {
                    MessageBox.Show("Esa celda no tiene un grupo", "Precaucion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
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
            listaComprobacion.Clear();
            listaGlobal.Clear();
            int nSize = int.Parse(sizeN.Text);

            //AQUI EN ESTE FOR SE OBTIENEN TODOS LOS PARES ORDENADOS DE LA MATRIZ.
            for (int iMatriz = 0; iMatriz < nSize; iMatriz++)
            {
                for (int jMatriz = 0; jMatriz < nSize; jMatriz++)
                {
                    String resultado = funcionConsulta(iMatriz.ToString(),jMatriz.ToString());
                    if (!listaComprobacion.Contains(resultado))
                    {
                        
                        if (resultado != "" && resultado != "[]")
                        {
                            //Console.WriteLine(resultado);
                            listaComprobacion.Add(resultado);
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
                            listaGlobal.Add(listaTemporal);

                        }
                        else if (resultado == "[]")
                        {
                            string agregacion = "[[" + iMatriz.ToString() + "," + jMatriz.ToString()+"]]";
                            //Console.WriteLine(agregacion);
                            listaComprobacion.Add(agregacion);

                            List<List<int>> listaTemporal = new List<List<int>>();
                            List<int> subListaAAgregar = new List<int>();

                            subListaAAgregar.Add(iMatriz);
                            subListaAAgregar.Add(jMatriz);

                            listaTemporal.Add(subListaAAgregar);
                            listaGlobal.Add(listaTemporal);

                        }
                        else
                        {
                            //Console.WriteLine("no existe el punto");
                        }
                    }
                    
                }
                    
            }
            
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
            dataGridView1.ClearSelection();

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
            List<int> sizeList = new List<int>();

            for (int i = 0;i<listaGlobal.Count; i++)
            {
                int numeroPorAgregar = listaGlobal[i].Count;
                sizeList.Add(numeroPorAgregar);
            }

            sizeList.Sort();

            int variable = listaGlobal.Count;
            string prueba = "";

            var g = sizeList.GroupBy(i => i);
            foreach (var grp in g)
            {
                int valor1 = grp.Key;
                int valor2 = grp.Count();
                prueba = prueba + "\nDe tamaño: " + valor1 + ", existen: " + valor2 + " grupos,";
            }
            prueba.Remove(prueba.Length - 1);

            MessageBox.Show("Los grupos presentes en la matriz son -> "+ variable + ",\nY sus tamaños respectivamente son: " + prueba, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            if (banderaClick == true)
            {
                banderaClick = false;
            }
            else
            {
                banderaClick = true;
            }
        }

    }
}
