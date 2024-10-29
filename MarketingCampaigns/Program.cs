using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketingCampaigns
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Xml;

    class Program
    {
        static void Main()
        {
            // Строка подключения к базе данных
            string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";

            // Завдання 1: Розрахувати середній бюджет на одного клієнта для кожної кампанії
            CalculateAverageBudgetPerClient(connectionString);

            // Завдання 2: Зберегти дані таблиці у XML-файл
            SaveDataToXml(connectionString, "MarketingCampaignsData.xml");
        }

        static void CalculateAverageBudgetPerClient(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT 
                CampaignName,
                Budget,
                ClientCount,
                CASE WHEN ClientCount > 0 THEN Budget / ClientCount ELSE 0 END AS AvgBudgetPerClient
                FROM dbo.MarketingCampaigns";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Середній бюджет на одного клієнта для кожної кампанії:");
                        Console.WriteLine("---------------------------------------------");

                        while (reader.Read())
                        {
                            string campaignName = reader["CampaignName"].ToString();
                            decimal budget = (decimal)reader["Budget"];
                            int clientCount = (int)reader["ClientCount"];
                            decimal avgBudgetPerClient = (decimal)reader["AvgBudgetPerClient"];

                            Console.WriteLine($"Кампанія: {campaignName}, Бюджет: {budget}, Кількість клієнтів: {clientCount}, Середній бюджет на одного клієнта: {avgBudgetPerClient}");
                        }
                    }
                }
            }
        }

        static void SaveDataToXml(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT CampaignName, StartDate, Budget, ClientCount FROM MarketingCampaigns";
                string file = "C:\\Users\\Ксеня\\source\\repos\\MarketingXML.xml";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                
                    dataTable.WriteXml(file);

                    Console.WriteLine($"Дані таблиці збережено у файл: {file}");
                }
            }
        }
    }

}
