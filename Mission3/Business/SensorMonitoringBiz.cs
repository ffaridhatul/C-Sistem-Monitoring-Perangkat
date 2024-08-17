using Mission3.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mission3.Business
{
    public class SensorMonitoringBiz
    {
        private static SensorMonitoringBiz sensorMonitoringBiz;
        private List<Device> deviceList;
        private Dictionary<int, Device> deviceMap;
        private List<SensorData> sensorDataList;
        private IDataSource<SensorData> sensorDataFile;
        private IDataSource<Device> deviceFile;

        // 1. Tambahkan konstruktor SensorMonitoringBiz() {} } pribadi untuk mencegah orang luar membuat objek dengan kelas ini.
        private SensorMonitoringBiz() { }

        // 2. Menambahkan anggota private static SensorMonitoringBiz sensorMonitoringBiz;
        // (Sudah ditambahkan di atas)

        // 3. Tambahkan metode public static SensorMonitoringBiz GetInstance()
        //    Jika sensorMonitoringBiz bernilai null, buat objek SensorMonitoringBiz baru dan tetapkan ke sensorMonitoringBiz
        public static SensorMonitoringBiz GetInstance()
        {
            if (sensorMonitoringBiz == null)
            {
                sensorMonitoringBiz = new SensorMonitoringBiz();
            }
            return sensorMonitoringBiz;
        }

        public void SetSensorDataFile(IDataSource<SensorData> dataSource)
        {
            // Copy nilai dataSource yang diterima sebagai parameter ke variabel anggota sensorDataFile.
            sensorDataFile = dataSource;
        }

        public void SetDeviceFile(IDataSource<Device> dataSource)
        {
            // Copy nilai dataSource yang diterima sebagai parameter ke variabel anggota deviceFile.
            deviceFile = dataSource;
        }

        public void LoadData()
        {
            // 1. Jika variabel anggota sensorDataFile bukan null, maka metode sensorDataFile.Load() dipanggil
            //    untuk mendapatkan objek List dari file dan hasilnya disimpan di sensorDataList.
            if (sensorDataFile != null)
            {
                sensorDataList = sensorDataFile.Load();
            }

            // 2. Jika variabel anggota deviceFile bukan null, maka metode deviceFile.Load() dipanggil untuk mendapatkan objek List dari file 
            //    dan hasilnya disimpan di deviceList.
            if (deviceFile != null)
            {
                deviceList = deviceFile.Load();
                deviceMap = PutToDeviceMap(deviceList);
            }
        }

        public Dictionary<int, Device> PutToDeviceMap(List<Device> deviceList)
        {
            // 1. var deviceMap = new Dictionary<int, Device>();
            var deviceMap = new Dictionary<int, Device>();

            // 2. Di loop foreach, masukkan Device di parameter DeviceList ke dalam DeviceMap
            //    Saat ini, Device.Id di objek Device dengan nilai int digunakan sebagai kunci Dictionary.
            foreach (var device in deviceList)
            {
                deviceMap[device.DeviceId] = device;
            }

            // 3. Mengembalikan deviceMap.
            return deviceMap;
        }

        public List<SensorData> GetSensorData(Func<SensorData, Boolean> filter)
        {
            // 1. var list = new List<SensorData>();
            var list = new List<SensorData>();

            // 2. Dalam loop foreach, metode yang ditunjuk oleh filter variabel delegasi yang diterima sebagai parameter dipanggil,
            //    dan hanya jika hasilnya true, metode tersebut dimasukkan dalam list yang dibuat di atas.
            foreach (var data in sensorDataList)
            {
                if (filter(data))
                {
                    list.Add(data);
                }
            }

            // 3. Mengembalikan list yang berisi objek SensorData yang memenuhi ketentuan.
            return list;
        }

        public void AddDevice(Device device)
        {
            // 1. Menambahkan nilai parameter device ke DeviceList.
            deviceList.Add(device);

            // 2. Tambahkan device ke koleksi deviceMap dengan device.DeviceId sebagai kunci.
            deviceMap[device.DeviceId] = device;

            // 3. Gunakan metode deviceFile.Save() untuk menyimpan objek List<Device> yang ditunjuk oleh deviceList ke sebuah file.
            deviceFile.Save(deviceList);
        }

        public void AddSensorData(SensorData sensorData)
        {
            // Tambahkan sensorData ke dalam sensorDataList.
            sensorDataList.Add(sensorData);
        }

        public List<Device> GetDeviceList()
        {
            // Mengembalikan koleksi List<Device> deviceList.
            return deviceList;
        }

        public Dictionary<int, Device> GetDeviceMap()
        {
            // Mengembalikan koleksi Dictionary deviceMap.
            return deviceMap;
        }

        public async void SaveToDataSource()
        {
            // 1. Panggil metode sensorDataFile.SaveAsync() untuk menyimpan sensorDataList secara asinkron.
            await sensorDataFile.SaveAsync(sensorDataList);

            // 2. Panggil metode deviceFile.SaveAsync() untuk menyimpan deviceList secara asinkron.
            await deviceFile.SaveAsync(deviceList);
        }
    }
}
