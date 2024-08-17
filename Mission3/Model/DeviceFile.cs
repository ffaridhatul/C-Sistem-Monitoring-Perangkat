using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission3.Model
{
    public class DeviceFile : IDataSource<Device>
    {
        private string filePath = "Device.txt";

        public List<Device> Load()
        {
            // 1. Baca string JSON yang disimpan dalam file “Device.txt” di folder yang sama.
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                // 2. Kembalikan string ke objek List<Device> menggunakan metode kelas JsonConvert.
                var devices = JsonConvert.DeserializeObject<List<Device>>(json);

                // 3. Jika hasil yang dikembalikan bukan null, List dikembalikan. Jika null, objek List<Device> kosong dibuat dan dikembalikan.
                return devices ?? new List<Device>();
            }
            return new List<Device>();
        }

        public void Save(List<Device> list)
        {
            // 1. Konversikan list yang diterima sebagai parameter menjadi string JSON menggunakan metode kelas JsonConvert.
            string json = JsonConvert.SerializeObject(list);

            // 2. Tulis string menggunakan file “Device.txt” di folder yang sama.
            File.WriteAllText(filePath, json);
        }

        public Task SaveAsync(List<Device> list)
        {
            // Kode untuk memanggil metode Save() yang ditulis di atas secara asinkron menggunakan metode Task.Factory.StartNew().
            return Task.Factory.StartNew(() => Save(list));
        }
    }
}
