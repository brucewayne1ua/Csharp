using System;
using System.Data.SQLite;

namespace course
{
    public class Patient : Database
    {
        public void AddPatient(string fullName, string birthDate, string address, string phone)
        {
            Open();

            string sql = @"
                INSERT INTO Patients (FullName, BirthDate, Address, Phone)
                VALUES (@name, @birthDate, @address, @phone)";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", fullName);
            command.Parameters.AddWithValue("@birthDate", birthDate);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);
            command.ExecuteNonQuery();

            Close();

            Console.WriteLine("Пациент добавлен.");
        }

        public void ShowAllPatients()
        {
            Open();

            string sql = "SELECT * FROM Patients";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("В базе нет пациентов.");
                Close();
                return;
            }

            Console.WriteLine("\nID | ФИО | Дата рождения | Адрес | Телефон");
            Console.WriteLine("================================================");

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["PatientID"]} | {reader["FullName"]} | {reader["BirthDate"]} | {reader["Address"]} | {reader["Phone"]}");
            }

            Close();
        }

        public bool HasPatients()
        {
            Open();

            string sql = "SELECT 1 FROM Patients LIMIT 1";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            bool exists = command.ExecuteScalar() != null;

            Close();

            return exists;
        }

        public bool PatientExists(int id)
        {
            Open();

            string sql = "SELECT 1 FROM Patients WHERE PatientID = @id LIMIT 1";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            bool exists = command.ExecuteScalar() != null;

            Close();

            return exists;
        }
    }
}