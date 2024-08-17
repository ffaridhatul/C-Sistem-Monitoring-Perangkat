using Mission3.Business;
using Mission3.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Mission3.View
{
    public partial class frmMain : Form
    {
        DeviceComManager deviceComManager;
        SensorMonitoringBiz sensorMonitoringBiz;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            sensorMonitoringBiz = SensorMonitoringBiz.GetInstance();
            sensorMonitoringBiz.SetDeviceFile(new DeviceFile());
            sensorMonitoringBiz.SetSensorDataFile(new SensorDataFile());
            
            sensorMonitoringBiz.LoadData();

            deviceComManager = new DeviceComManager(9999);
            deviceComManager.SensorDataArrived += SensorDataArrived;
            deviceComManager.Start();

            RefreshGrid();

            tmrSaveData.Start();
        }

        private void SensorDataArrived(SensorData sensorData)
        {
            this.Invoke(new Action(()=> { ProcessSensorData(sensorData); }));
        }

        //Metode dilakukan setiap kali informasi suhu/kelembaban (SensorData) dikirimkan dari peralatan
        private void ProcessSensorData(SensorData sensorData)
        {
            var deviceMap = sensorMonitoringBiz.GetDeviceMap();  // Dapatkan objek Map peralatan yang dikelola

            // 1. Periksa apakah Device yang sesuai dengan DeviceId dari objek SensorData yang dikirimkan ada di objek Map (menggunakan metode ContainsKey()).
            if (deviceMap.ContainsKey(sensorData.DeviceId))
            {
                // 2. Jika ada, maka akan mencari objek Device dan memperbarui nilai suhu dan kelembapan saat ini dengan nilai yang dikirimkan dari peralatan.
                var device = deviceMap[sensorData.DeviceId];
                device.CurrentTemperature = sensorData.Temperature;
                device.CurrentHumidity = sensorData.Humidity;

                // 3. Tambahkan data transmisi perangkat ke List dengan memanggil metode AddSensorData() objek SensorMonitoringBiz.
                sensorMonitoringBiz.AddSensorData(sensorData);

                // 4. Panggil metode RefreshGrid() untuk memperbarui suhu/kelembaban saat ini.
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            dgvDevice.AutoGenerateColumns = false;
            dgvDevice.DataSource = sensorMonitoringBiz.GetDeviceList().ToList();
        }

        private void lblAddSensorDevice_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new frmDeviceAdd();

            if (form.ShowDialog() == DialogResult.OK)
                RefreshGrid();
        }

        private void tmrSaveData_Tick(object sender, EventArgs e)
        {
            sensorMonitoringBiz.SaveToDataSource();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            sensorMonitoringBiz.SaveToDataSource();
        }

        private void lblSensorDataHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new frmSensorDataList();
            form.ShowDialog();
        }
    }
}
