using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Basic_SQLtest.Properties;
using System.Data;
using System.Windows.Forms;

namespace Basic_SQLtest
{
    class Database
    {
        public Database()
        { }

        public void GetData(string sqlCommand, List<Produit> l_produit)
        {
            SqlConnection sqlConnection1 = new SqlConnection(Settings.Default.connection);
            SqlCommand cmd = new SqlCommand(sqlCommand, sqlConnection1);
            SqlDataReader reader = null;
            sqlConnection1.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Produit produit = new Produit();
                    produit.Id = reader["id"].ToString();
                    produit.Nom = reader["nom"].ToString();
                    produit.Num_serie = reader["num_serie"].ToString();
                    l_produit.Add(produit);
                }
            }
             //close the connection
           if (sqlConnection1 != null)
           {
            sqlConnection1.Close();
           }
        }

        public bool deleteSN(int id, string num_serie)
        {
            bool state = true;
            int lineNumber = 0;

            SqlConnection sqlConnection1 = new SqlConnection(Settings.Default.connection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM SN WHERE SN.id_nom_produit=@id AND SN.num_serie=@num_serie";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = id;
            cmd.Parameters.Add("@num_serie", SqlDbType.NVarChar);
            cmd.Parameters["@num_serie"].Value = num_serie;

        
            try
            {
                sqlConnection1.Open();

                lineNumber = cmd.ExecuteNonQuery();
                if (lineNumber < 1)
                {
                    state = false;
                }
            }
            catch (Exception ex)
            {
                state = false;
                //Log.Database.Error("Error : " + ex.ToString());
            }
            finally
            {
                //close the connection
                if (sqlConnection1 != null)
                {
                    sqlConnection1.Close();
                }
            }
          return state;
        }

        public bool updateProduit(string nom, string num_serie, List<Produit> l_produit)
        {
            bool state = false;
            SqlConnection sqlConnection3 = new SqlConnection(Settings.Default.connection);
            SqlCommand cmd = new SqlCommand();
            int lineNumber = 0;
            int tempId = getProductId(nom, num_serie);
       
            cmd.CommandText = "UPDATE SN SET num_serie=@num_serie WHERE id_nom_produit = @tempId";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection3;
            cmd.Parameters.Add("@id_nom_produit", SqlDbType.Int);
            cmd.Parameters["@id_nom_produit"].Value = tempId;
            cmd.Parameters.Add("@num_serie", SqlDbType.VarChar);
            cmd.Parameters["@num_serie"].Value = num_serie;

         
            try
            {
                sqlConnection3.Open();

                lineNumber = cmd.ExecuteNonQuery();
                if (lineNumber < 1)
                {
                    state = false;
                }
            }
            catch (Exception ex)
            {
                state = false;
               // Log.Database.Error("Erreur : " + ex.ToString());
            }
            finally
            {
                //close the connection
                if (sqlConnection3 != null)
                {
                    sqlConnection3.Close();
                }
            }
            return state;
        }


        public bool insertSN(string nom, string num_serie, List<Produit> l_produit)
        {
            SqlConnection sqlConnection2 = new SqlConnection(Settings.Default.connection);
            SqlCommand cmd = new SqlCommand();
            int lineNumber = 0;
            int tempId = getProductId(nom, num_serie);
            bool state = true;

            // 1st insert applied on SN
            cmd.CommandText = "INSERT INTO SN (id_nom_produit, num_serie) VALUES(@id_nom_produit, @num_serie)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection2;
            cmd.Parameters.Add("@id_nom_produit", SqlDbType.Int);
            cmd.Parameters["@id_nom_produit"].Value = tempId;
            cmd.Parameters.Add("@num_serie", SqlDbType.NVarChar);
            cmd.Parameters["@num_serie"].Value = num_serie;

            try
            {
                sqlConnection2.Open();
                lineNumber = cmd.ExecuteNonQuery();
                if (lineNumber < 1)
                {
                    state = false;
                }
            }
            catch (Exception ex)
            {
                state = false;
            }
            finally
            {
                //close the connection
                if (sqlConnection2 != null)
                {
                    sqlConnection2.Close();
                }
            }
            // 2nd insert applied on Produit table
            cmd.CommandText = "INSERT INTO Produit (id, nom) VALUES (LAST_INSERT_ID(), @nom)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection2;
            cmd.Parameters.Add("@nom", SqlDbType.NVarChar);
            cmd.Parameters["@nom"].Value = nom;

            try
            {
                sqlConnection2.Open();
                lineNumber = cmd.ExecuteNonQuery();
                if (lineNumber < 1)
                {
                    state = false;
                }
            }
            catch (Exception ex)
            {
                state = false;
            }
            finally
            {
                //close the connection
                if (sqlConnection2 != null)
                {
                    sqlConnection2.Close();
                }
            }
            return state;
        }

      public int getProductId(string name, string num_serie)
        {
            SqlConnection sqlConnection1 = new SqlConnection(Settings.Default.connection);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader = null;
            int id = 0;
                       
            cmd.CommandText = "SELECT * FROM Produit LEFT JOIN SN ON Produit.id = SN.id_nom_produit WHERE Produit.nom = @nom";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@nom", SqlDbType.NVarChar);
            cmd.Parameters["@nom"].Value = name;

            try
            {
                sqlConnection1.Open();

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = Int32.Parse(reader["ID"].ToString());
                        if (string.Equals(reader["NUM_SERIE"].ToString(), num_serie))
                          {
                             id = 0;
                             break;
                          }
                     }
                }
            }
            catch (Exception ex)
            {
                /// Log.Database.Error("Erreur : " + ex.ToString());
                Console.Write("Failed at line 241 catch");
            }
            finally
            {
                //close the reader
                if (reader != null)
                {
                    reader.Close();
                }

                //close the connection
                if (sqlConnection1 != null)
                {
                    sqlConnection1.Close();
                }
            }
            return id;
        }
    }   
}
