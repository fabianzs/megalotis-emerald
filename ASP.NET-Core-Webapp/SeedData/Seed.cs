using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Entities;
namespace ASP.NET_Core_Webapp.SeedData
{
    public class Seed
    {
        public string Json { get; set; }
        public string Json2 { get; set; }
        public DataSet DataSet { get; set; }
        public DataSet SeedDataSet { get; set; }
        public DataTable DataTable { get; set; }
        public DataTable LibraryTable { get; set; }
        public DataTable Users { get; set; }
        public DataTable Archetypes { get; set; }
        public DataTable Pitches { get; set; }
        public StreamReader StreamReader { get; set; }
        public Seed()
        {
            this.StreamReader = new StreamReader(@"C:\Users\laszl\Documents\Programozás\greenfox\megem_project\megalotis-emerald\ASP.NET-Core-Webapp\SeedData\SeedData.json");
            Json2 = StreamReader.ReadToEnd();
            Json = @"{
          'Table1': [
            {
              'id': 0,
              'item': 'item 0'
            },
            {
              'id': 1,
              'item': 'item 1'
            }
          ]
        }";
            this.DataSet = JsonConvert.DeserializeObject<DataSet>(Json);
            DataTable = this.DataSet.Tables["Table1"];

            this.SeedDataSet = JsonConvert.DeserializeObject<DataSet>(Json2);
            LibraryTable = this.SeedDataSet.Tables["library"];
            Users = this.SeedDataSet.Tables["users"];
            Archetypes = this.SeedDataSet.Tables["archetypes"];
            Pitches = this.SeedDataSet.Tables["pitches"];


        }

        public void writeRowCount()
        {
            Console.WriteLine(DataTable.Rows.Count);
            // 2
        }

        public void writeAllRows()
        {
            foreach (DataRow row in DataTable.Rows)
            {
                Console.WriteLine(row["id"] + " - " + row["item"]);
            }
            // 0 - item 0
            // 1 - item 1
            foreach (DataRow row in LibraryTable.Rows)
            {
                Console.WriteLine(row[0] + " " + row[1] + " " + row[2] +" "+row[3]);
            }
            Console.WriteLine(Json2);
        }

        public void FillDataBase()
        {

        }
    }
}
