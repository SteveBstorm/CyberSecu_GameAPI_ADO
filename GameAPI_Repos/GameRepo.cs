using Models.Entities_POCO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAPI_Repos
{
    public class GameRepo
    {
        private string _connectionString = @"Data Source=STEVEBSTORM\MSSQLSERVER01;Initial Catalog=CyberSecuEF_Game;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private Jeu ToEntity(SqlDataReader reader)
        {
            return new Jeu
            {
                Id = (int)reader["Id"],
                Titre = reader["Titre"].ToString(),
                Description = (reader["Description"] is DBNull) ? null : reader["Description"].ToString(),
                AnneeSortie = (int)reader["AnneeSortie"],
                Note = (int)reader["Note"],
                GenreId = (int)reader["GenreId"]
            };
        }

        public void AddGame(Jeu jeu)
        {
            using(SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    string sql = "INSERT INTO Jeux (Titre, Description, AnneeSortie, Note, GenreId) " +
                        "VALUES(@titre, @desc, @annee, @note, @genreid";

                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("titre", jeu.Titre);
                    cmd.Parameters.AddWithValue("desc", jeu.Description);
                    cmd.Parameters.AddWithValue("annee", jeu.AnneeSortie);
                    cmd.Parameters.AddWithValue("note", jeu.Note);
                    cmd.Parameters.AddWithValue("genreid", jeu.GenreId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public IEnumerable<Jeu> GetAll()
        {
            List<Jeu> toReturn = new List<Jeu>();
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT * FROM Jeux";
                    cmd.CommandText = sql;
                    conn.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
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

        public IEnumerable<Jeu> GetByKeywork(string keyword)
        {
            List<Jeu> toReturn = new List<Jeu>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT * FROM Jeux WHERE Titre LIKE '%"+keyword+"%'";
                    cmd.CommandText = sql;

                    //cmd.Parameters.AddWithValue("keyword", keyword);
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

        public IEnumerable<Jeu> GetByGenre(int idGenre)
        {
            List<Jeu> toReturn = new List<Jeu>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT * FROM Jeux WHERE GenreId = @genreId";
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("genreId", idGenre);
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
    }
}
