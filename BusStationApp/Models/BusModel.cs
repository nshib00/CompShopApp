using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusStationApp.Models
{
    class BusModel
    {
        private string _regNumber;
        private string _brand;
        private string _model;

        public string RegNumber {
            get { return _regNumber; }
            set { _regNumber = value; } 
        }
        public string Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }
    }
}
