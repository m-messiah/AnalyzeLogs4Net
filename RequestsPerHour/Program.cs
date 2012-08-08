using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RequestsPerHour
{
	internal class Program
	{

		private readonly Regex path;
		private readonly Regex fullpath;
		private readonly Regex time;
		private readonly Regex arg; //It is a argument of request
		private readonly Regex ip;

		public Program()
		{
			path = new Regex(@":27183(\/[\/\w\d]+)[\?\s]");
			fullpath = new Regex(@":27183(\/[\/\S]+)\s");
			time = new Regex(@"\s(\d\d:\d\d:\d\d,\d\d\d)\s");
			arg = new Regex(@"boxId\=([\da-f\-]+)[\@\&$\s]");
			ip = new Regex(@"\s(\d{1,3}\.){3}\d{1,3}\s");
		}

		private static void Main(string[] args)
		{
			var p = new Program();
			p.Run();
		}
		private void Run()
		{
			ByIp();
			ByFullPath();
			ByPath();
			ByArg();
		}

		private void ByArg()
		{
			var hash = new Dictionary<Int64, Dictionary<String, Int64>>();
			var log = new StreamReader(@"../../actions_2012-08-06");
			var boxes = new SortedSet<String>();
			var i = 0.0;
			Console.WriteLine("\nByArg");
			while (!log.EndOfStream)
			{
				Console.Write("\r{0,2:f}%", i * 100 / 6327965);
				i++;
				var line = log.ReadLine();
				if (line == null) continue;
				var bm = arg.Match(line);
				if (!bm.Success) continue;
				var boxing = bm.Value.Replace("boxId=", "").Replace("@","").Replace("&","").Replace("$","").Trim();
				boxes.Add(boxing);
				var tm = time.Match(line);
				if (!tm.Success) continue;
				var tim = tm.Value.Replace(',', '.').Trim();
				var timing = Convert.ToDateTime(tim).Hour;
				if (hash.ContainsKey(timing))
				{
					if (hash[timing].ContainsKey(boxing))
					{
						hash[timing][boxing]++;
					}
					else
					{
						hash[timing][boxing] = 1;
					}
				}
				else
				{
					hash[timing] = new Dictionary<String, Int64>();
					hash[timing][boxing] = 1;
				}
			}

			var fil = new StreamWriter(@"../../Results/byArg.html");
			fil.WriteLine(boxes.Count);
			fil.Write("<table border=1 style='border-collapse: collapse;'><tr><th>BoxId</th>");
			for (var t = 0; t < 24; t++)
			{
				fil.Write("<th>{0}</th>", t);
			}
			fil.Write("</tr>");
			foreach (var v in boxes)
			{
				fil.Write("<tr>");
				fil.Write("<th>{0}</th>", v);
				for (var k = 0; k < 24; k++)
				{
					if (hash.ContainsKey(k) && hash[k].ContainsKey(v))
					{
						fil.Write("<td>{0}</td>", hash[k][v]);
						continue;
					}
					fil.Write("<td></td>");
				}
				fil.WriteLine("</tr>");
			}
			fil.WriteLine("</table>");
			fil.Close();
		}

		private void ByPath()
		{
			var hash = new Dictionary<Int64, Dictionary<String, Int64>>();
			var log = new StreamReader(@"../../actions_2012-08-06");
			var paths = new SortedSet<String>();
			var i = 0.0;
			Console.WriteLine("\nByPath");
			while (!log.EndOfStream)
			{
				Console.Write("\r{0,2:f}%", i * 100 / 6327965);
				i++;
				var line = log.ReadLine();
				if (line == null) continue;
				var pm = path.Match(line);
				if (!pm.Success) continue;
				var pathing = pm.Value.Replace(":27183", "").Replace("?","");
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

			var fil = new StreamWriter(@"../../Results/byPath.html");
			fil.WriteLine(paths.Count);
			fil.Write("<table border=1 style='border-collapse: collapse;'><tr><th>Path</th>");
			for (var t = 0; t < 24; t++)
			{
				fil.Write("<th>{0}</th>", t);
			}
			fil.Write("</tr>");
			foreach (var v in paths)
			{
				fil.Write("<tr>");
				fil.Write("<th>{0}</th>", v);
				for (var k = 0; k < 24; k++)
				{
					if (hash.ContainsKey(k) && hash[k].ContainsKey(v))
					{
						fil.Write("<td>{0}</td>", hash[k][v]);
						continue;
					}
					fil.Write("<td></td>");
				}
				fil.WriteLine("</tr>");
			}
			fil.WriteLine("</table>");
			fil.Close();
		}

		private void ByIp()
		{
			var hash = new Dictionary<Int64, Dictionary<String, Int64>>();
			var log = new StreamReader(@"../../actions_2012-08-06");
			var ips = new SortedSet<String>();
			var i = 0.0;
			Console.WriteLine("\nByIp");
			while (!log.EndOfStream)
			{
				Console.Write("\r{0,2:f}%", i * 100 / 6327965);
				i++;
				var line = log.ReadLine();
				if (line == null) continue;
				var im = ip.Match(line);
				if (!im.Success) continue;
				var iping = im.Value;
				ips.Add(iping);
				var tm = time.Match(line);
				if (!tm.Success) continue;
				var tim = tm.Value.Replace(',', '.').Trim();
				var timing = Convert.ToDateTime(tim).Hour;
				if (hash.ContainsKey(timing))
				{
					if (hash[timing].ContainsKey(iping))
					{
						hash[timing][iping]++;
					}
					else
					{
						hash[timing][iping] = 1;
					}
				}
				else
				{
					hash[timing] = new Dictionary<String, Int64>();
					hash[timing][iping] = 1;
				}
			}

			var fil = new StreamWriter(@"../../Results/byIp.html");
			fil.WriteLine(ips.Count);
			fil.Write("<table border=1 style='border-collapse: collapse;'><tr><th>Ip</th>");
			for (var t = 0; t < 24; t++)
			{
				fil.Write("<th>{0}</th>", t);
			}
			fil.Write("</tr>");
			foreach (var v in ips)
			{
				fil.Write("<tr>");
				fil.Write("<th>{0}</th>", v);
				for (var k = 0; k < 24; k++)
				{
					if (hash.ContainsKey(k) && hash[k].ContainsKey(v))
						{
							fil.Write("<td>{0}</td>", hash[k][v]);
							continue;
						}
					fil.Write("<td></td>");
				}
				fil.WriteLine("</tr>");
			}
			fil.WriteLine("</table>");
			fil.Close();
		}

		private void ByFullPath()
		{
			var hash = new Dictionary<Int64, Dictionary<String, Int64>>();
			var log = new StreamReader(@"../../actions_2012-08-06");
			var fullpaths = new SortedSet<String>();
			var i = 0.0;
			Console.WriteLine("\nByFullPath");
			while (!log.EndOfStream)
			{
				Console.Write("\r{0,2:f}%", i*100/6327965);
				i++;
				var line = log.ReadLine();
				if (line == null) continue;
				var pm = fullpath.Match(line);
				if (!pm.Success) continue;
				var pathing = pm.Value.Replace(":27183", "");
				fullpaths.Add(pathing);
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
			fil.WriteLine(fullpaths.Count);
			fil.Write("<table border=1 style='border-collapse: collapse;'><tr><th>Path</th>");
			for (var t = 0; t < 24; t++)
			{
				fil.Write("<th>{0}</th>", t);
			}
			fil.Write("</tr>");
			foreach (var v in fullpaths)
			{
				fil.Write("<tr>");
				fil.Write("<th>{0}</th>", v);
				for (var k = 0; k < 24; k++)
				{
					if (hash.ContainsKey(k) && hash[k].ContainsKey(v))
					{
						fil.Write("<td>{0}</td>", hash[k][v]);
						continue;
					}
					fil.Write("<td></td>");
				}
				fil.WriteLine("</tr>");
			}
			fil.WriteLine("</table>");
			fil.Close();
		}
	}
}