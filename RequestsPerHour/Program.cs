using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RequestsPerHour
{
	class Program
	{
		static void Main()
		{
			var hash = new Dictionary<Int64, Dictionary<String, Int64>>();
			var path = new Regex(@":27183(\/[\/\S]+)\s");
			var time = new Regex(@"\s(\d\d:\d\d:\d\d,\d\d\d)\s");
			//boxId=re.compile(r'boxId\=([\da-f\-]+)[\@\&$\s]')
			//ip=re.compile(r'\s([\d{1,3}\.]{3}\d{1,3})\s')
			var log = new StreamReader(@"../../actions_2012-08-06");
			var paths = new SortedSet<String>();
			var i = 0.0;
			while (!log.EndOfStream)
			{
				Console.Write("\r{0,2:f}%", i*100 / 6327965);
				i++;
				var line = log.ReadLine();
				if (line == null) continue;
				var pm = path.Match(line);
				if (!pm.Success) continue;
				var pathing = pm.Value.Replace(":27183", "");
				paths.Add(pathing);
				var tm = time.Match(line);
				if (!tm.Success) continue;
				var tim = tm.Value.Replace(',', '.').Trim();
				var timing = Convert.ToDateTime(tim).Hour;
				if (hash.ContainsKey(timing))
				{
					if (hash[timing].ContainsKey(pathing))
					{
						hash[timing][pathing]++;
					}
					else
					{
						hash[timing][pathing] = 1;
					}
				}
				else
				{
					hash[timing] = new Dictionary<String, Int64>();
					hash[timing][pathing] = 1;
				}
			}

			var fil = new StreamWriter(@"../../Results/byFullPath.html");
			fil.WriteLine(paths.Count);
			fil.Write("<table border=1 style='border-collapse: collapse;'><tr><th>Path</th>");
			for (var t = 0; t < 24;t++ )
			{
				fil.Write("<th>{0}</th>", t);
			}
			fil.Write("</tr>");
			foreach (var v in paths)
			{
				fil.Write("<tr>");
				fil.Write("<th>{0}</th>",v);
				for (var k = 0; k < 24; k++)
				{
					if (hash[k].ContainsKey(v))
					{
						fil.Write("<td>{0}</td>",hash[k][v]);
					}
					else
					{
						fil.Write("<td></td>");
					}
				}
				fil.WriteLine("</tr>");
			}
			fil.WriteLine("</table>");
			fil.Close();

		}
	}
}
