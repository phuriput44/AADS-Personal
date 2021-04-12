using Demo.WindowsForms.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Net_GmapMarkerWithLabel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AADS.Views.City
{
    public partial class main : UserControl
    {
        public main()
        {
            InitializeComponent();
        }
        Dictionary<GMapMarker, City> cityMarker = new Dictionary<GMapMarker, City>();
        GMapMarker getmarker = null;
        public void setPoints(PointLatLng p)
        {
            txtPoints.Text = PositionConverter.ParsePointToString(p, "Signed Degree");
        }
        public void delMarker(GMapMarker marker)
        {
            GMapOverlay overlay = mainInstance.GetOverlay("markersP");
            cityMarker.Remove(marker);
            overlay.Markers.Remove(marker);
            map.Overlays.Add(overlay);
        }
        public void getMarker(GMapMarker marker)
        {
            getmarker = marker;
            if (cityMarker.ContainsKey(marker))
            {
                txtLabel.Text = cityMarker[marker].GetLabel();
                txtName.Text = cityMarker[marker].GetName();
                txtPoints.Text = PositionConverter.ParsePointToString(cityMarker[marker].GetPoint(), "Signed Degree");
            }
        }
        private void main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (getmarker != null)
            {
                cityMarker[getmarker].SetName(txtName.Text) ;
                cityMarker[getmarker].SetLabel(txtLabel.Text);
            }
        }
        private static MainForm mainInstance = MainForm.GetInstance();
        private GMapControl map = mainInstance.GetmainMap();
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPoints.Text))
            {
                MessageBox.Show("กรุณาเลือกตำแหน่งที่ต้องการ", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PointLatLng point = PositionConverter.ParsePointFromString(txtPoints.Text);
            
            
            Bitmap pic = (Bitmap)Image.FromFile("Images/City.png");
            var marker = new GmapMarkerWithLabel(point,txtLabel.Text.ToString(),pic,20);
            GMapOverlay overlay = mainInstance.GetOverlay("markersP");
            overlay.Markers.Add(marker);
            City city = new City(marker,txtName.Text,txtLabel.Text,point);
            cityMarker.Add(marker, city);            
            map.Overlays.Add(overlay);
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            delMarker(getmarker);
        }
    }
}
