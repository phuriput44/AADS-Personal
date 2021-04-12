using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADS
{
    public enum RadarFeature
    {
        Marker, Line, Polygon, Track
    }
    public class TrackCommandArgs : RadarCommandArgs
    {
        public TrackData Track { get; set; }
    }
    public abstract class RadarCommandArgs
    {
        public DateTime Time { get; set; }
    }
    public class RadarCommand
    {
        public RadarFeature Feature { get; set; }
        public string Operation { get; set; }
        public object Args { get; set; }
    }
}
