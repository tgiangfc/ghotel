using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKS.Models
{
    public class RoomModel111
    {
        public List<tblPhong> listPhongs { get; set; }
        public tblPhong phong { get; set; }
        public List<tblLoaiPhong> listLoaiPhongs { get; set; }
        public List<tblTang> listTangs { get; set; }
    }
}