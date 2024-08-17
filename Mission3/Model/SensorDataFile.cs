using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission3.Model
{
    public class SensorDataFile : IDataSource<SensorData>
    {
        private string filePath = "SensorRecord.txt";

        public List<SensorData> Load()
        {
            // 1. Baca string JSON yang disimpan dalam file “SensorRecord.txt” di folder yang sama.
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                // 2. Kembalikan string ke objek List<SensorData> menggunakan metode kelas JsonConvert.
                var sensorDataList = JsonConvert.DeserializeObject<List<SensorData>>(json);

                // 3. Jika hasil yang dikembalikan bukan null, List dikembalikan. Jika null, objek List<SensorData> kosong dibuat dan dikembalikan.
                return sensorDataList ?? new List<SensorData>();
            }
            return new List<SensorData>();
        }

        public void Save(List<SensorData> list)
        {
            // 1. Konversikan list yang diterima sebagai parameter menjadi string JSON menggunakan metode kelas JsonConvert.
            string json = JsonConvert.SerializeObject(list);

            // 2. Tulis string menggunakan file “SensorRecord.txt” di folder yang sama.
            File.WriteAllText(filePath, json);
        }

        public Task SaveAsync(List<SensorData> list)
        {
            // 1. Kode untuk memanggil metode Save() yang ditulis di atas secara asinkron menggunakan metode Task.Factory.StartNew().
            return Task.Factory.StartNew(() => Save(list));
        }
    }
}
