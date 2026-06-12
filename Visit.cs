using System;
using System.Data.SQLite;

namespace course
{
    public class Visit : Database
    {
        public void AddVisit(int patientId, int doctorId, string visitDateTime, string diagnosis)
        {
            Open();

            string sql = @"
                INSERT INTO Visits
                (PatientID, DoctorID, VisitDateTime, Diagnosis)
                VALUES
                (@patientId, @doctorId, @visitDateTime, @diagnosis)";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@patientId", patientId);
            command.Parameters.AddWithValue("@doctorId", doctorId);
            command.Parameters.AddWithValue("@visitDateTime", visitDateTime);
            command.Parameters.AddWithValue("@diagnosis", diagnosis);
            command.ExecuteNonQuery();

            Close();

            Console.WriteLine("Карточка посещения добавлена.");
        }

        public void ShowAllVisits()
        {
            Open();

            string sql = @"
                SELECT
                    Visits.VisitID,
                    Patients.FullName AS PatientName,
                    Doctors.FullName AS DoctorName,
                    Doctors.Specialization,
                    Visits.VisitDateTime,
                    Visits.Diagnosis
                FROM Visits
                JOIN Patients ON Visits.PatientID = Patients.PatientID
                JOIN Doctors ON Visits.DoctorID = Doctors.DoctorID";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Карточек посещений нет.");
                Close();
                return;
            }

            Console.WriteLine("\nID | Пациент | Врач | Специализация | Дата посещения | Диагноз");
            Console.WriteLine("================================================================");

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["VisitID"]} | {reader["PatientName"]} | {reader["DoctorName"]} | {reader["Specialization"]} | {reader["VisitDateTime"]} | {reader["Diagnosis"]}");
            }

            Close();
        }
    }
}