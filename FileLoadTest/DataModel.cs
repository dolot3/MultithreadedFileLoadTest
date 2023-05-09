using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoadTest
{
    [Table("TestInfo")]
    public class DataModel
    {

        public DataModel() { }

        public DataModel(string data)
        {
            var split = data.Split('\t');

            this.ssn = split[0];
            this.name = split[1];
            this.address = split[2];
            this.city = split[3];
            this.state = split[4];
            this.zip = split[5];
            this.country = split[6];
            this.phone = split[7];
            this.email = split[8];
            this.company = split[9];
            this.profession = split[10];
            this.birthday = DateTime.Parse(split[11]);
            this.cc = split[12];
            this.amount = double.Parse(split[13].Trim('$', '"'));
            this.timestamp = int.Parse(split[14]);
            
        }

        [Key]
        public int id { get; set; }
        public string ssn { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public string city { get; set; }
        
        public string state { get; set; }

        public string zip { get; set; }

        public string country { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string company { get; set; }

        public string profession { get; set; }

        public DateTime birthday { get; set; }

        public string cc { get; set; }

        public double amount { get; set; }

        public int timestamp { get; set; }

    }
}
