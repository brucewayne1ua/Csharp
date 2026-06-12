using System;
using System.Data.SQLite;

namespace course
{
    public class Appointment : Database
    {
        public bool AppointmentExists(int doctorId, string dateTime)
        {
            Open();

            string sql = @"
                SELECT 1 
                FROM Appointments 
                WHERE DoctorID = @doctorId 
                  AND AppointmentDateTime = @dateTime
                  AND Status = 'Записаний'
                LIMIT 1";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@doctorId", doctorId);
            command.Parameters.AddWithValue("@dateTime", dateTime);

            bool exists = command.ExecuteScalar() != null;

            Close();

            return exists;
        }

        public void AddAppointment(int patientId, int doctorId, string dateTime)
        {
            if (AppointmentExists(doctorId, dateTime))
            {
                Console.WriteLine("Это время уже занято.");
                return;
            }

            Open();

            string sql = @"
                INSERT INTO Appointments
                (PatientID, DoctorID, AppointmentDateTime, Status)
                VALUES
                (@patientId, @doctorId, @dateTime, @status)";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@patientId", patientId);
            command.Parameters.AddWithValue("@doctorId", doctorId);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@status", "Записаний");
            command.ExecuteNonQuery();

            Close();

            Console.WriteLine("Пациент записан на приём.");
        }

        public void ShowAllAppointments()
        {
            Open();

            string sql = @"
                SELECT 
                    Appointments.AppointmentID,
                    Patients.FullName AS PatientName,
                    Doctors.FullName AS DoctorName,
                    Doctors.Specialization,
                    Appointments.AppointmentDateTime,
                    Appointments.Status
                FROM Appointments
                JOIN Patients ON Appointments.PatientID = Patients.PatientID
                JOIN Doctors ON Appointments.DoctorID = Doctors.DoctorID
                WHERE Appointments.Status = 'Записаний'";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Активных записей на приём нет.");
                Close();
                return;
            }

            Console.WriteLine("\nID | Пациент | Врач | Специализация | Дата и время | Статус");
            Console.WriteLine("================================================================");

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["AppointmentID"]} | {reader["PatientName"]} | {reader["DoctorName"]} | {reader["Specialization"]} | {reader["AppointmentDateTime"]} | {reader["Status"]}");
            }

            Close();
        }

        public (int PatientId, int DoctorId)? GetAppointment(int appointmentId)
        {
            Open();

            string sql = @"
                SELECT PatientID, DoctorID
                FROM Appointments
                WHERE AppointmentID = @id
                  AND Status = 'Записаний'";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", appointmentId);

            using SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                Close();
                return null;
            }

            int patientId = Convert.ToInt32(reader["PatientID"]);
            int doctorId = Convert.ToInt32(reader["DoctorID"]);

            Close();

            return (patientId, doctorId);
        }

        public bool HasAppointments()
        {
            Open();

            string sql = @"
        SELECT 1
        FROM Appointments
        WHERE Status = 'Записаний'
        LIMIT 1";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);

            bool exists = command.ExecuteScalar() != null;

            Close();

            return exists;
        }

        public void CompleteAppointment(int appointmentId)
        {
            Open();

            string sql = @"
                UPDATE Appointments
                SET Status = @status
                WHERE AppointmentID = @id";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@status", "Завершено");
            command.Parameters.AddWithValue("@id", appointmentId);
            command.ExecuteNonQuery();

            Close();
        }
    }
}