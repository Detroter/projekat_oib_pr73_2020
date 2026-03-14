using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autoservis.Model;
using Autoservis.Service.Interface;

namespace Autoservis.Service.Repository
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly string filePath = "C:/Users/HP/Documents/GitHub/projekat_oib_pr73_2020/Autoservis/DB/receipts.json";
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        public List<Receipt> GetAll()
        {
            if (!File.Exists(filePath))
            {
                return new List<Receipt>();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Receipt>>(json, options) ?? new List<Receipt>();
        }

        public void Add(Receipt receipt)
        {
            var receipts = GetAll();
            receipts.Add(receipt);
            
            string json = JsonSerializer.Serialize(receipts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}