using System.Data;
using System.Data.SQLite;
using System.IO;

namespace course
{
    public class Database
    {
        protected SQLiteConnection connection;
        private string db_file_path = "hospital.db";

        public Database()
        {
            connection = new SQLiteConnection($"Data Source={db_file_path};Version=3;");
        }

        public void Open()
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        public void Close()
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        public void CreateDatabase()
        {
            if (!File.Exists(db_file_path))
                SQLiteConnection.CreateFile(db_file_path);

            Open();

            string sql = @"
            CREATE TABLE IF NOT EXISTS Doctors (
                DoctorID INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                Specialization TEXT NOT NULL,
                Phone TEXT
            );

            CREATE TABLE IF NOT EXISTS Patients (
                PatientID INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                BirthDate TEXT,
                Address TEXT,
                Phone TEXT
            );

            CREATE TABLE IF NOT EXISTS Appointments (
                AppointmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                PatientID INTEGER NOT NULL,
                DoctorID INTEGER NOT NULL,
                AppointmentDateTime TEXT NOT NULL,
                Status TEXT DEFAULT 'Записаний',
                FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
                FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
            );

            CREATE TABLE IF NOT EXISTS Visits (
                VisitID INTEGER PRIMARY KEY AUTOINCREMENT,
                PatientID INTEGER NOT NULL,
                DoctorID INTEGER NOT NULL,
                VisitDateTime TEXT NOT NULL,
                Diagnosis TEXT NOT NULL,
                FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
                FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
            );";

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();

            Close();
        }
    }
}