using Models.Entities_POCO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI_Repos
{
    public class GenreRepo
    {
        private string _connectionString = @"Data Source=STEVEBSTORM\MSSQLSERVER01;Initial Catalog=CyberSecuEF_Game;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private Genre ToEntity(SqlDataReader reader)
        {
            return new Genre
            {
                Id = (int)reader["Id"],
                Nom = reader["Nom"].ToString()
            };
        }
        public void CreateGenre(Genre g)
        {
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string sql = "INSERT INTO Genre (Nom) " +
                        "VALUES(@nom)";

                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("nom", g.Nom);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public IEnumerable<Genre> GetAll()
        {
            List<Genre> toReturn = new List<Genre>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT * FROM Genres";
                    cmd.CommandText = sql;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            toReturn.Add(ToEntity(reader));
                        }
                    }
                    conn.Close();
                }
            }
            return toReturn;
        }
        public Genre GetById(int id)
        {
            Genre toReturn = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT * FROM Genres";
                    cmd.CommandText = sql;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            toReturn = ToEntity(reader);
                        }
                    }
                    conn.Close();
                }
            }
            return toReturn;
        }
    }
}
