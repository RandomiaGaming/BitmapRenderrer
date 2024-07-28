using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LickMyNuts
{
	public class Program
	{
		public static void Main(string[] args)
		{

		}

		public static double Warp(double a, double b, double c, double d, double t)
		{
			double clamp = (t - a) / (b - a);
			double range = (d - c);
			return c + (clamp * range);
		}
	}
}
