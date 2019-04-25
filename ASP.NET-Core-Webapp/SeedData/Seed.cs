using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Entities;
namespace ASP.NET_Core_Webapp.Entities
{
    public class Seed
    {
        public ApplicationContext ApplicationContext { get; set; }
        public string Json { get; set; }
        public string Json2 { get; set; }
        public DataSet DataSet { get; set; }
        public DataSet SeedDataSet { get; set; }
        public DataTable DataTable { get; set; }
        public DataTable Library { get; set; }
        public DataTable Users { get; set; }
        public DataTable Archetypes { get; set; }
        public DataTable Pitches { get; set; }
        public StreamReader StreamReader { get; set; }
        public ApplicationContext DataBase { get; set; }

        public Seed(ApplicationContext applicationContext)
        {
            this.DataBase = applicationContext;
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
            Library = this.SeedDataSet.Tables["library"];
            Users = this.SeedDataSet.Tables["users"];
            Archetypes = this.SeedDataSet.Tables["archetypes"];
            Pitches = this.SeedDataSet.Tables["pitches"];
        }

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
            Library = this.SeedDataSet.Tables["library"];
            Users = this.SeedDataSet.Tables["users"];
            Archetypes = this.SeedDataSet.Tables["archetypes"];
            Pitches = this.SeedDataSet.Tables["pitches"];
        }

        public void writeRowCount()
        {
            Console.WriteLine(DataTable.Rows.Count);
            // 2
        }

        //public void writeAllRows()
        //{
        //    foreach (DataRow row in DataTable.Rows)
        //    {
        //        Console.WriteLine(row["id"] + " - " + row["item"]);
        //    }
        //    // 0 - item 0
        //    // 1 - item 1
        //    foreach (DataRow row in Library.Rows)
        //    {
        //        Console.WriteLine(row[0] + " " + row[1] + " " + row[2] +" "+row[3]);
        //    }

        //    foreach (DataRow row in Users.Rows)
        //    {
        //        Console.WriteLine(row[0] + " " + row[1] + " " + row[2] + " " + row[3] + " " + row[4]);
        //    }
        //    Console.WriteLine(Json2);
        //}

        public void FillDatabase()
        {
            //levels is a datatable

            foreach (DataRow row in Library.Rows)
            {
                var listOfLevels = row.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

                Badge badge = new Badge() { Version = row[0].ToString(), Name = row[1].ToString(), Tag = row[2].ToString() };
                DataBase.Add(badge);
            }
            foreach (DataRow row in Users.Rows)
            {
                User user = new User() { Name = row[0].ToString(), Picture = row[2].ToString() };
                DataBase.Add(user);
            }
            foreach (DataRow row in Pitches.Rows)
            {
                Pitch pitch = new Pitch() { TimeStamp = DateTime.Parse(row[6].ToString()), PitchMessage = row[4].ToString(), PitchedLevel = int.Parse(row[3].ToString()) };
                DataBase.Add(pitch);
            }
            DataBase.SaveChanges();
        }
    }
}
