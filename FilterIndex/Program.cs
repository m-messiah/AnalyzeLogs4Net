using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FilterIndex
{
	class Program
	{
		private readonly Regex arg; //It is a argument of request
		private const string LogFile = @"../../actions_2012-08-08";

		public Program()
		{
			arg = new Regex(@"BoxId\:\s([\da-f\-]+),\s");
		}

		private static void Main()
		{
			var p = new Program();
			p.Run();
		}
		private void Run()
		{
			var hash = new Dictionary<String, Int64>();
			var log = new StreamReader(LogFile);
			var i = 0.0;
			while (!log.EndOfStream)
			{
				Console.Write("\r{0,2:f}%", i * 100 / 486084);
				i++;
				var line = log.ReadLine();
				if (line == null) continue;
				var bm = arg.Matches(line);
				foreach (var box in from Match boxi in bm select boxi.Value.Replace("BoxId: ", "").Replace(",", "").Trim())
				{
					hash[box] = hash.ContainsKey(box) ? hash[box] + 1 : 1;
				}
			}
			var fil = new StreamWriter(@"../../Results/byBox.html");
			var boxes = hash.Keys.ToList();
			fil.WriteLine(boxes.Count);
			fil.WriteLine("<table border=1 style='border-collapse: collapse;'><tr><th>BoxId</th><th>Count</th></tr>");
			foreach (var v in boxes)
			{
				fil.Write("<tr>");
				fil.Write("<th>{0}</th>", v);
				if (hash.ContainsKey(v)) fil.Write("<td>{0}</td>", hash[v]);
				else fil.Write("<td></td>");
				fil.WriteLine("</tr>");
			}
			fil.WriteLine("</table>");
			fil.Close();
		}
	}
}
