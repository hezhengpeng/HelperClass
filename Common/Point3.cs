using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadSys.Common
{
    /// <summary>
    /// 三角形的点位
    /// </summary>
    public struct Point3
    {
        public double x;
        public double y;
        public double z;

        public Point3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}