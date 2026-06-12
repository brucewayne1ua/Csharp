using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace course
{
    public class Doctor : Database
    {
        public void AddDoctor(string fullName, string specialization, string phone)
        {
            Open();

            string sql = @"
                INSERT INTO Doctors (FullName, Specialization, Phone)
                VALUES (@name, @spec, @phone)";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", fullName);
            command.Parameters.AddWithValue("@spec", specialization);
            command.Parameters.AddWithValue("@phone", phone);
            command.ExecuteNonQuery();

            Close();

            Console.WriteLine("Врач добавлен.");
        }

        public void ShowAllDoctors()
        {
            if (!HasDoctors())
            {
                Console.WriteLine("В базе нет врачей.");
                return;
            }

            Open();

            string sql = "SELECT * FROM Doctors";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            Console.WriteLine("\nID | ФИО | Специализация | Телефон");
            Console.WriteLine("================================================");

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["DoctorID"]} | {reader["FullName"]} | {reader["Specialization"]} | {reader["Phone"]}");
            }

            Close();
        }

        public bool HasDoctors()
        {
            Open();

            string sql = "SELECT 1 FROM Doctors LIMIT 1";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            bool exists = command.ExecuteScalar() != null;

            Close();

            return exists;
        }

        public bool DoctorExists(int id)
        {
            Open();

            string sql = "SELECT 1 FROM Doctors WHERE DoctorID = @id LIMIT 1";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            bool exists = command.ExecuteScalar() != null;

            Close();

            return exists;
        }

        public void DeleteDoctor(int id)
        {
            Open();

            string sql = "DELETE FROM Doctors WHERE DoctorID = @id";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            Close();

            Console.WriteLine("Врач удалён.");
        }

        public void EditDoctor(int id)
        {
            Console.WriteLine("\nЧто изменить?");
            Console.WriteLine("1. ФИО");
            Console.WriteLine("2. Специализацию");
            Console.WriteLine("3. Телефон");

            string choice = Console.ReadLine();

            string fieldName;
            string message;

            if (choice == "1")
            {
                fieldName = "FullName";
                message = "Новое ФИО: ";
            }
            else if (choice == "2")
            {
                fieldName = "Specialization";
                message = "Новая специализация: ";
            }
            else if (choice == "3")
            {
                fieldName = "Phone";
                message = "Новый телефон: ";
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
                return;
            }

            Console.Write(message);
            string newValue = Console.ReadLine();

            UpdateDoctorField(id, fieldName, newValue);
        }

        public void UpdateDoctorField(int id, string fieldName, string newValue)
        {
            Open();

            string sql = $"UPDATE Doctors SET {fieldName} = @value WHERE DoctorID = @id";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@value", newValue);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            Close();

            Console.WriteLine("Данные врача обновлены.");
        }

        public void FindDoctor(string search)
        {
            Open();

            string sql = @"
                SELECT *
                FROM Doctors
                WHERE FullName LIKE @search
                   OR Specialization LIKE @search
                   OR Phone LIKE @search";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@search", "%" + search + "%");

            using SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Врачи не найдены.");
                Close();
                return;
            }

            Console.WriteLine("\nID | ФИО | Специализация | Телефон");
            Console.WriteLine("================================================");

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["DoctorID"]} | {reader["FullName"]} | {reader["Specialization"]} | {reader["Phone"]}");
            }

            Close();
        }

        public List<DoctorModel> GetDoctors()
        {
            List<DoctorModel> doctors = new List<DoctorModel>();

            Open();

            string sql = "SELECT * FROM Doctors";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                doctors.Add(new DoctorModel
                {
                    DoctorID = Convert.ToInt32(reader["DoctorID"]),
                    FullName = reader["FullName"].ToString(),
                    Specialization = reader["Specialization"].ToString(),
                    Phone = reader["Phone"].ToString()
                });
            }

            Close();

            return doctors;
        }

        public void BubbleSortDoctors(List<DoctorModel> doctors, int sortType)
        {
            for (int i = 0; i < doctors.Count - 1; i++)
            {
                for (int j = 0; j < doctors.Count - i - 1; j++)
                {
                    bool swap = false;

                    if (sortType == 1)
                    {
                        swap = string.Compare(doctors[j].FullName, doctors[j + 1].FullName, true) > 0;
                    }
                    else if (sortType == 2)
                    {
                        swap = string.Compare(doctors[j].Specialization, doctors[j + 1].Specialization, true) > 0;
                    }
                    else if (sortType == 3)
                    {
                        swap = string.Compare(doctors[j].Phone, doctors[j + 1].Phone, true) > 0;
                    }

                    if (swap)
                    {
                        DoctorModel temp = doctors[j];
                        doctors[j] = doctors[j + 1];
                        doctors[j + 1] = temp;
                    }
                }
            }
        }

        public void ShowSortedDoctors(int sortType)
        {
            List<DoctorModel> doctors = GetDoctors();

            BubbleSortDoctors(doctors, sortType);

            Console.WriteLine("\nID | ФИО | Специализация | Телефон");
            Console.WriteLine("================================================");

            foreach (DoctorModel doctor in doctors)
            {
                Console.WriteLine(
                    $"{doctor.DoctorID} | {doctor.FullName} | {doctor.Specialization} | {doctor.Phone}");
            }
        }
    }
}