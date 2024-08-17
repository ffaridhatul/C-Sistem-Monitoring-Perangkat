using Mission3.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mission3.View
{
    public partial class frmSensorDataList : Form
    {
        private SensorMonitoringBiz sensorMonitoringBuz;

        public frmSensorDataList()
        {
            InitializeComponent();
        }

        private void frmSensorDataList_Load(object sender, EventArgs e)
        {
            sensorMonitoringBuz = SensorMonitoringBiz.GetInstance();
            dgvDevice.AutoGenerateColumns = false;
        }

        private void btnShowInsideTempRange_Click(object sender, EventArgs e)
        {
            // Menampilkan riwayat dengan suhu antara numFromTemp.Value dan numToTemp.Value
            var filteredData = sensorMonitoringBuz.GetSensorData(
                d => d.Temperature >= (double)numFromTemp.Value && d.Temperature <= (double)numToTemp.Value
            );
            dgvDevice.DataSource = filteredData.ToList();
        }

        private void btnShowOutsideTempRange_Click(object sender, EventArgs e)
        {
            // Menampilkan riwayat dengan suhu di luar numFromTemp.Value dan numToTemp.Value
            var filteredData = sensorMonitoringBuz.GetSensorData(
                d => d.Temperature < (double)numFromTemp.Value || d.Temperature > (double)numToTemp.Value
            );
            dgvDevice.DataSource = filteredData.ToList();
        }

        private void btnShowInsideHumidityRange_Click(object sender, EventArgs e)
        {
            // Menampilkan riwayat dengan kelembapan antara numFromHumidity.Value dan numToHumidity.Value
            var filteredData = sensorMonitoringBuz.GetSensorData(
                d => d.Humidity >= (double)numFromHumidity.Value && d.Humidity <= (double)numToHumidity.Value
            );
            dgvDevice.DataSource = filteredData.ToList();
        }

        private void btnShowOutsideHumidityRange_Click(object sender, EventArgs e)
        {
            // Menampilkan riwayat dengan kelembapan di luar numFromHumidity.Value dan numToHumidity.Value
            var filteredData = sensorMonitoringBuz.GetSensorData(
                d => d.Humidity < (double)numFromHumidity.Value || d.Humidity > (double)numToHumidity.Value
            );
            dgvDevice.DataSource = filteredData.ToList();
        }
    }
}
