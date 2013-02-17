using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web_Whg_Backgroundworker.ServiceReference1;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;

namespace Web_Whg_Backgroundworker
{
    class Program
    {
        const string CONSTRING = "Server=instance29437.db.xeround.com;Port=19153;Database=users;Uid=appharbor;Pwd=NNDKjRzh";
        const string url = "http://www.immobilienscout24.de/Suche/S-82/Wohnung-Miete/Bayern/Muenchen/-/2,00-3,00/-/EURO-450,00-850,00?enteredFrom=result_list";
        int oldCount = 0, newCount = 0;
        int newID=0, oldID=0;
        


        static void Main(string[] args)
        {
            bool exit = false;

         //   my_init();
            HashSet<int> currentIDs = new HashSet<int>();

            //connect
            MySqlConnection con = new MySqlConnection(CONSTRING);
            con.Open();


            //adapter
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            //SQL Abfrage erstellen
            string cmdText = "SELECT ID, ZEIT FROM Web_Whg_table ORDER BY ZEIT DESC";
            MySqlCommand cmd = new MySqlCommand(cmdText, con);

            //Datatable abrufen
            DataTable dt = new DataTable();
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            DataRowCollection dataRowC = dt.Rows;

            int j= dataRowC.Count;

            for (int i = 0; i < j; i++)
            {
                currentIDs.Add(Convert.ToInt32(dataRowC[i].ItemArray[0]));
                Console.WriteLine(Convert.ToInt32(dataRowC[i].ItemArray[0]));
            }


            while (!exit)
            {



                // Datenbank leeren

                // Service wird initialisiert
                ServiceClient client2 = new ServiceClient();

                // Abfrage nach allen Wohnungen im Web wird durchgeführt
                string[] result = client2.getWhgs(url);

                // Deklaration eines Arrays, in dem in der for schleife jedes einzelne Abfrageergebnis temporär gespeichert wird
                string[] sArray;

                int i = result.Length - 1;

                for (int k = i; k >= 0; k--)
                {
                    string s = result[k];
                    string[] sep = new string[1];
                    sep[0] = ";:;";
                    sArray = s.Split(sep, System.StringSplitOptions.None);

                    int ID = Convert.ToInt32(sArray[9]);

                    Console.WriteLine("Überprüfe " + ID);
                    if (!currentIDs.Contains(ID))
                    {
                        Console.WriteLine("Die ID " + ID + "war noch nicht vorhanden");
                        string cmdText2 = "INSERT INTO Web_Whg_table (Header, Picture, Miete, Zimmer, Flaeche, detail0, detail1, detail2, detail3, ID, lage) VALUES ('" + sArray[0] + "', '" + sArray[1] + "', '" + sArray[2] + "', '" + sArray[3] + "', '" + sArray[4] + "', '" + sArray[5] + "', '" + sArray[6] + "', '" + sArray[7] + "', '" + sArray[8] + "', " + Convert.ToInt32(sArray[9]) + ", '" + sArray[10] + "');";
                        MySqlCommand cmd2 = new MySqlCommand(cmdText2, con);
                        try
                        {
                            cmd2.ExecuteNonQuery();
                            currentIDs.Add(ID);
                        } catch (MySqlException){
                            Console.WriteLine("SQL-Exception");
                        }

                    }
                    string cmdText3 = "UPDATE Web_Whg_table SET Header='Crawler' WHERE ID=1;";
                    MySqlCommand cmd3 = new MySqlCommand(cmdText3, con);
                    try
                    {
                        cmd3.ExecuteNonQuery();
                        currentIDs.Add(ID);
                    }
                    catch (MySqlException)
                    {
                        Console.WriteLine("SQL-Exception");
                    }

                    

                }
                Thread.Sleep(20000);


            }




            Console.Read();
            //ServiceClient client2 = new ServiceClient();

            //int i= dt.Rows.Count;


            //client2.getWhgs(url);
            //System.Collections.ObjectModel.ObservableCollection<string> sColl = e.Result;
            //string[] sArray;

            //foreach ()
            //{
            //    // sArray = s.Split(';');
            //    string[] sep = new string[1];
            //    sep[0] = ";:;";
            //    sArray = s.Split(sep, System.StringSplitOptions.None);
            //    //1    2        3       4       5        6        7        8        9     10    11
            //    //     'INSERT INTO Web_Whg_table (Header, Picture, Miete, Zimmer, Flaeche, detail0, detail1, detail2, detail3, ID, lage) 
            //    //   VALUES (sArray[0], sArray[1], sArray[2], sArray[3], sArray[4], sArray[5], sArray[6], sArray[7], sArray[8], sArray[9], sArray[10])'

            //    //   myListBox.Items.Add(new ItemViewModel() { LineOne = sArray[0], WhgPicture = sArray[1], LineTwo = sArray[2], LineThree = sArray[4], LineFour = sArray[3], Detail1 = sArray[5], Detail2 = sArray[6], Detail3 = sArray[7], Detail4 = sArray[8], link = "http://www.immobilienscout24.de" + sArray[9], Lage = sArray[10] });

            //    // MessageBox.Show(sArray[10]);
            //    // myListBox.SelectionChanged += new SelectionChangedEventHandler(myListBox_SelectionChanged);



          //  }
        }

        static public void my_init()
        {


            MySqlConnection con = new MySqlConnection(CONSTRING);
            con.Open();

            //adapter
            MySqlDataAdapter adapter = new MySqlDataAdapter();



            // Datenbank leeren

            // Service wird initialisiert
            ServiceClient client2 = new ServiceClient();

            // Abfrage nach allen Wohnungen im Web wird durchgeführt
            string[] result = client2.getWhgs(url);

            // Deklaration eines Arrays, in dem in der for schleife jedes einzelne Abfrageergebnis temporär gespeichert wird
            string[] sArray;

            int i = result.Length -1;

            for ( int j = i ; j>=0 ; j--)
            {
                string s = result[j];
                string[] sep = new string[1];
                sep[0] = ";:;";
                sArray = s.Split(sep, System.StringSplitOptions.None);

                string cmdText = "INSERT INTO Web_Whg_table (Header, Picture, Miete, Zimmer, Flaeche, detail0, detail1, detail2, detail3, ID, lage) VALUES ('" + sArray[0] + "', '" + sArray[1] + "', '" + sArray[2] + "', '" + sArray[3] + "', '" + sArray[4] + "', '" + sArray[5] + "', '" + sArray[6] + "', '" + sArray[7] + "', '" + sArray[8] + "', " + Convert.ToInt32(sArray[9]) + ", '" + sArray[10] + "');";
                MySqlCommand cmd = new MySqlCommand(cmdText, con);

                cmd.ExecuteNonQuery();



            }
        }


    }
}
