using System;

namespace course
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            db.CreateDatabase();

            Doctor doctor_db = new Doctor();
            Patient patient_db = new Patient();
            Appointment appointment_db = new Appointment();
            Visit visit_db = new Visit();

            while (true)
            {
                Console.WriteLine("\n===== МЕНЮ =====");
                Console.WriteLine("1. Добавить врача");
                Console.WriteLine("2. Показать врачей");
                Console.WriteLine("3. Удалить врача");
                Console.WriteLine("4. Редактировать врача");
                Console.WriteLine("5. Найти врача");
                Console.WriteLine("6. Сортировка врачей");
                Console.WriteLine("7. Добавить пациента");
                Console.WriteLine("8. Показать пациентов");
                Console.WriteLine("9. Записать пациента на приём");
                Console.WriteLine("10. Показать записи на приём");
                Console.WriteLine("11. Добавить карточку посещения");
                Console.WriteLine("12. Показать карточки посещений");
                Console.WriteLine("0. Выход");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("ФИО: ");
                        string name = Console.ReadLine();

                        Console.Write("Специализация: ");
                        string spec = Console.ReadLine();

                        Console.Write("Телефон: ");
                        string phone = Console.ReadLine();

                        doctor_db.AddDoctor(name, spec, phone);
                        break;

                    case "2":
                        doctor_db.ShowAllDoctors();
                        break;

                    case "3":
                        if (!doctor_db.HasDoctors())
                        {
                            Console.WriteLine("В базе нет врачей.");
                            break;
                        }

                        doctor_db.ShowAllDoctors();

                        int deleteId = ReadInt("Введите ID врача для удаления: ");

                        if (!doctor_db.DoctorExists(deleteId))
                        {
                            Console.WriteLine("Врач не найден.");
                            break;
                        }

                        doctor_db.DeleteDoctor(deleteId);
                        break;

                    case "4":
                        if (!doctor_db.HasDoctors())
                        {
                            Console.WriteLine("В базе нет врачей.");
                            break;
                        }

                        doctor_db.ShowAllDoctors();

                        int editId = ReadInt("Введите ID врача для редактирования: ");

                        if (!doctor_db.DoctorExists(editId))
                        {
                            Console.WriteLine("Врач не найден.");
                            break;
                        }

                        doctor_db.EditDoctor(editId);
                        break;

                    case "5":
                        if (!doctor_db.HasDoctors())
                        {
                            Console.WriteLine("В базе нет врачей.");
                            break;
                        }

                        Console.Write("Введите ФИО, специализацию или телефон: ");
                        string search = Console.ReadLine();

                        doctor_db.FindDoctor(search);
                        break;

                    case "6":
                        if (!doctor_db.HasDoctors())
                        {
                            Console.WriteLine("В базе нет врачей.");
                            break;
                        }

                        Console.WriteLine("\nСортировать по:");
                        Console.WriteLine("1. ФИО");
                        Console.WriteLine("2. Специализации");
                        Console.WriteLine("3. Телефону");

                        int sortType = ReadInt("Ваш выбор: ");

                        if (sortType < 1 || sortType > 3)
                        {
                            Console.WriteLine("Неверный выбор.");
                            break;
                        }

                        doctor_db.ShowSortedDoctors(sortType);
                        break;

                    case "7":
                        Console.Write("ФИО пациента: ");
                        string patientName = Console.ReadLine();

                        Console.Write("Дата рождения: ");
                        string birthDate = Console.ReadLine();

                        Console.Write("Адрес: ");
                        string address = Console.ReadLine();

                        Console.Write("Телефон: ");
                        string patientPhone = Console.ReadLine();

                        patient_db.AddPatient(patientName, birthDate, address, patientPhone);
                        break;

                    case "8":
                        patient_db.ShowAllPatients();
                        break;

                    case "9":
                        if (!patient_db.HasPatients())
                        {
                            Console.WriteLine("В базе нет пациентов.");
                            break;
                        }

                        if (!doctor_db.HasDoctors())
                        {
                            Console.WriteLine("В базе нет врачей.");
                            break;
                        }

                        patient_db.ShowAllPatients();
                        int patientId = ReadInt("Введите ID пациента: ");

                        if (!patient_db.PatientExists(patientId))
                        {
                            Console.WriteLine("Пациент не найден.");
                            break;
                        }

                        doctor_db.ShowAllDoctors();
                        int doctorId = ReadInt("Введите ID врача: ");

                        if (!doctor_db.DoctorExists(doctorId))
                        {
                            Console.WriteLine("Врач не найден.");
                            break;
                        }

                        Console.Write("Введите дату и время приёма, например 2026-06-15 10:30: ");
                        string dateTime = Console.ReadLine();

                        appointment_db.AddAppointment(patientId, doctorId, dateTime);
                        break;

                    case "10":
                        appointment_db.ShowAllAppointments();
                        break;

                    case "11":
                        if (!appointment_db.HasAppointments())
                        {
                            Console.WriteLine("Активных записей на приём нет.");
                            break;
                        }

                        appointment_db.ShowAllAppointments();

                        int appointmentId = ReadInt("Введите ID записи: ");

                        var appointment = appointment_db.GetAppointment(appointmentId);

                        if (appointment == null)
                        {
                            Console.WriteLine("Запись не найдена.");
                            break;
                        }

                        Console.Write("Диагноз: ");
                        string diagnosis = Console.ReadLine();

                        string visitDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                        visit_db.AddVisit(
                            appointment.Value.PatientId,
                            appointment.Value.DoctorId,
                            visitDate,
                            diagnosis);

                        appointment_db.CompleteAppointment(appointmentId);

                        Console.WriteLine("Приём завершён.");
                        break;

                    case "12":
                        visit_db.ShowAllVisits();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        public static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Введите число!");
            }
        }
    }
}