using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProfiGroup.DomainModel;

namespace TestProfiGroup.DataAccess
{
    public class NotesRepository : INotesRepository
    {
        private readonly string connectionString;

        public NotesRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Guid Create(Note note, bool useDateChanged = false)
        {
            note.Id = Guid.NewGuid();
            if (!useDateChanged)
            {
                note.DateChanged = DateTime.Now;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "insertNote";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_id", note.Id);
                    command.Parameters.AddWithValue("_title", note.Title);
                    command.Parameters.AddWithValue("_content", note.Content ?? "");
                    command.Parameters.AddWithValue("_dateChanged", note.DateChanged);
                    command.ExecuteNonQuery();
                }
            }

            return note.Id;
        }

        public void Update(Note note, bool useDateChanged = false)
        {
            if (!useDateChanged)
            {
                note.DateChanged = DateTime.Now;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "updateNote";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_id", note.Id);
                    command.Parameters.AddWithValue("_title", note.Title);
                    command.Parameters.AddWithValue("_content", note.Content ?? "");
                    command.Parameters.AddWithValue("_dateChanged", note.DateChanged);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Guid id)
        {
            var dateChanged = DateTime.Now;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "deleteNote";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_id", id);
                    command.Parameters.AddWithValue("_dateChanged", dateChanged);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Note Read(Guid id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "readNote";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_id", id);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var result = new Note();
                        result.Id = (Guid)reader[0];
                        result.Title = (string)reader[1];
                        result.Content = (string)reader[2];
                        result.DateChanged = (DateTime)reader[3];

                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public IList<Note> ReadAll()
        {
            var result = new List<Note>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "readAllNotes";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        var item = new Note();
                        item.Id = (Guid)reader[0];
                        item.Title = (string)reader[1];
                        item.Content = (string)reader[2];
                        item.DateChanged = (DateTime)reader[3];

                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public double ReadDatabaseSize()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "readDatabaseSize";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();

                    reader.Read();
                    return (double)((decimal)reader[0]);
                }
            }
        }
    }
}
