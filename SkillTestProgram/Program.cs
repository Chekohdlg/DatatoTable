using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SkillTestProgram
{
    class Program
    {

        static void Main(string[] args)
        {
            //Set the righ file Path here:
             var reader = new StreamReader(@"C:\Users\Sergi\Desktop\SkillTestProgram\SkillTestProgram\SkillTestProgram\FullData.csv");
            List<Data> dataSet = new List<Data>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                Data data = new Data
                {
                    Date = DateTime.Parse(values[0]),
                    Quantity = float.Parse(values[1])
                };
                dataSet.Add(data);
            }
            //Star working with the dataSet from here


            List<Data> result = new List<Data>();
            List<int> years = new List<int>();

            foreach (Data item in dataSet)
            {
                var day = result.FirstOrDefault(x => x.Date == item.Date);
                if (result.Any(x=> x.Date == item.Date))
                {
                
                    day.Quantity = day.Quantity + item.Quantity;

                    result.Remove(day);
                    result.Add(day);
                }
                else
                {
                    result.Add(item);
                }
               
                if(!years.Any(x=>x == item.Date.Year))
                {
                    years.Add(item.Date.Year);
                }
            }

            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("<!DOCTYPE html><html> ");
            sbTable.Append("<style>td {border: 1px solid black;} table{ text-align: center; }</style> ");
            sbTable.Append("<body>");
            sbTable.Append("<table>");
            foreach (int year in years.OrderBy(x=>x))
            {
                List<Data> resultbyyearlst = result.Where(x => x.Date.Year == year).ToList();
               string stryear=  resultbyyear(resultbyyearlst, out float totalyear, out int nummonths);

                sbTable.Append("<td>");
                sbTable.Append($"<table>");

                sbTable.Append("<tr>");
                sbTable.Append($"<td colspan=" + nummonths + ">");
                sbTable.Append($"{year}");
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append($"<td colspan=" + nummonths + ">");
                sbTable.Append($"{totalyear}");
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append(stryear);


                sbTable.Append($"</table>");
                sbTable.Append("</td>");
            }
            sbTable.Append("</table>");
            sbTable.Append("</body>");
            sbTable.Append("</html>");

            sbTable.ToString();


            string path = @"C:\Users\Sergi\Desktop\SkillTestProgram\SkillTestProgram\SkillTestProgram\FullData.html";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(sbTable.ToString());
                    
                }
            }
            else
            {
                File.Delete(path);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(sbTable.ToString());

                }
            }

        }

        public static string resultbyyear(List<Data> data, out float totalyear, out int nummonths)
        {
            totalyear = 0;
            List<int> months = new List<int>();

            float total = 0;
           
            foreach(Data item in data)
            {
                if(!months.Any(x=> x == item.Date.Month))
                {
                    months.Add(item.Date.Month);
                }
            }

            StringBuilder sum = new StringBuilder();
            sum.Append("<tr>");
         
            foreach (int month in months.OrderBy(x=>x))
            {
                sum.Append("<td>");
                string mes =  resultbyMonth(data.Where(x => x.Date.Month == month).ToList(), out float totalmonth, out int numdays);
               
                sum.Append($"<table>");

                sum.Append("<tr>");
                sum.Append($"<td colspan=" + numdays + ">");
                sum.Append($"{month}");
                sum.Append("</td>");
                sum.Append("</tr>");

                sum.Append("<tr>");
                sum.Append($"<td colspan="+ numdays + ">");
                sum.Append($"{totalmonth}");
                sum.Append("</td>");
                sum.Append("</tr>");


                sum.Append(mes);

                sum.Append($"</table>");
                sum.Append("</td>");

                totalyear = totalyear + totalmonth;

            }
            sum.Append("</tr>");


            nummonths = months.Count;
            return sum.ToString();
        }

        private static string resultbyMonth(List<Data> data, out float totalmonth, out int numdays)
        {
            totalmonth = 0;
            List<int> days = new List<int>();
            StringBuilder sbdays = new StringBuilder();
            StringBuilder sbQuantity = new StringBuilder();
            sbdays.Append("<tr>");
            sbQuantity.Append("<tr>");
            foreach (Data item in data.OrderBy(x=>x.Date.Day))
            {
                if (!days.Any(x => x == item.Date.Day))
                {
                    days.Add(item.Date.Day);
                    totalmonth = totalmonth + item.Quantity;
                    sbdays.Append("<td>");
                    sbdays.Append($"{item.Date.Day}");
                    sbdays.Append("</td>");


                    sbQuantity.Append("<td>");
                    sbQuantity.Append($"{item.Quantity}");
                    sbQuantity.Append("</td>");
                }
            }
            sbdays.Append("</tr>");
            sbQuantity.Append("</tr>");
            numdays = days.Count;
            return sbdays.Append(sbQuantity.ToString()).ToString();
        }
 
    }

}
