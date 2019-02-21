using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    static class Config
    {
        public static readonly string dataUTMPath = @"../../data_utm.xml";
        public static readonly string dataGeoPath = @"../../data_geo.xml";
        public static readonly string dataMapPath = @"../../data_map.xml";

        public static readonly int zoneUTM = 34;

        public static readonly double minLatitude = 45.2425;
        public static readonly double maxLatitude = 45.277031;
        public static readonly double minLongitude = 19.793909;
        public static readonly double maxLongitude = 19.894459;

        public static readonly string mapPath = @"../../novi_sad_map.jpg";
        public static readonly double mapHeight = 775;
        public static readonly double mapWidth = 1175;
    }
}
