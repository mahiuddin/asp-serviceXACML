using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace serviceXACML
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = ReadFile();
            InsertRecords(list);
            Console.Read();
        }
        public static List<InputData> ReadFile()
        {
            var list = new List<InputData>();

            string localFile = @"File.txt";
           // string sourceFile = @"\\192.168.1.109\Shared\File.txt";
          //  if (File.Exists(sourceFile))
            {
            //    File.Copy(sourceFile, localFile, true);
                //File.Delete(sourceFile);//File.Create(sourceFile);
                foreach (string line in File.ReadAllLines(localFile))
                {
                    var cols = line.Split('|');
                    var input_data = new InputData();
                    input_data.PatientId = cols[0];
                    input_data.Temperature = cols[1];
                    input_data.InputTime = Convert.ToDateTime(cols[2]);
                    input_data.DeviceId = cols[3];
                    list.Add(input_data);
                    Console.WriteLine(line);
                }
            }

            // File.Delete(localFile);
            return list;
        }

        public static void InsertRecords(List<InputData> list)
        {

            SqliteConnection conn = new SqliteConnection("Data Source=hrms.db;");
            int recordsAffected = 0;
            foreach (var record in list)
            {
                try
                {

                    var insertCmd = conn.CreateCommand();
                    insertCmd.CommandText = "INSERT INTO InputDataTable (PatientId , Temperature, InputTime, DeviceId) VALUES (@PatientId , @Temperature, @InputTime, @DeviceId)";
                    insertCmd.Parameters.AddWithValue("@PatientId", record.PatientId);
                    insertCmd.Parameters.AddWithValue("@Temperature", record.Temperature);
                    insertCmd.Parameters.AddWithValue("@InputTime", record.InputTime);
                    insertCmd.Parameters.AddWithValue("@DeviceId", record.DeviceId);
                    
                    conn.Open();
                    recordsAffected += insertCmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Occured : " + e.Message + "\t" + e.GetType());
                }
            }
            Console.WriteLine("{0} Records Inserted Successfully", recordsAffected);
        }
    }
    public class InputData
    {
        public string PatientId { get; set; }
        public string Temperature { get; set; }
        public DateTime InputTime { get; set; }
        public string DeviceId { get; set; }
    }
}
