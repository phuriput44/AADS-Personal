using GMap.NET;
using GMap.NET.WindowsForms;
using Net_GmapMarkerWithLabel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADS.Views.City
{
    class City
    {
        public City(GMapMarker marker, string name, string label, PointLatLng point)
        {
            this.marker = marker;
            this.point = point;
            this.name = name;
            this.label = label;
        }
        public String GetName()
        {
            return name;
        }
        public String GetLabel()
        {
            return label;
        }
        public PointLatLng GetPoint()
        {
            return point;
        }
        public void SetName(String name)
        {
            this.name = name;
        }
        public void SetLabel(String label)
        {
            this.label = label;

        }
        

        private GMapMarker marker { get; set; }
        private String name { get; set; }
        private String label { get; set; }
        private PointLatLng point { get; set; }
        
    }
    
}
