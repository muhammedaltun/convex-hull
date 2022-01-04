using System;                               // To run in the terminal, write 'dotnet run'
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace ConvexHull
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the coordinates of points:");
            Console.WriteLine("Example for 3 points: (1.3,2.5)(3.7,5.9)(78.12,23.58)");
            
            String str = Console.ReadLine();
            String[] tupl = new string[]{};
            String[] seps = { "(", ")" };
            String[] strList = str.Split(seps,StringSplitOptions.RemoveEmptyEntries);
            
            List<double[]> pointList = new List<double[]>{};
            double d0,d1;
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            foreach(String s in strList)
            {
                tupl = s.Split(",",StringSplitOptions.RemoveEmptyEntries);
                d0 = double.Parse(tupl[0],NumberStyles.Any,ci);
                d1 = double.Parse(tupl[1],NumberStyles.Any,ci);
                pointList.Add(new double[2] {d0,d1});
            }
            
            
            Console.WriteLine("Vertices of the convex hull are:");
            Console.WriteLine(string.Join(", ", findHull(pointList).ConvertAll(x => '(' + x[0].ToString().Replace(',','.') + ',' + x[1].ToString().Replace(',','.') + ')')));

            Console.Write($"{Environment.NewLine}Press any key to exit...");
            Console.ReadKey(true);
            
        }
        static double distance(double[] point1,double[] point2) 
        {
            return Math.Sqrt(Math.Pow((point1[0]-point2[0]),2) + Math.Pow((point1[1]-point2[1]),2));
        }
        static int cross(double[] point1,double[] point2,double[] point3)    
        // finds the sign of cross product: (point1,point2) x (point1,point3)
        {
            double[] vector0 = {point2[0]-point1[0],point2[1]-point1[1]};
            double[] vector1 = {point3[0]-point1[0],point3[1]-point1[1]};
            double determinant = vector0[0]*vector1[1] - vector0[1]*vector1[0];
            if(determinant == 0) {return 0;}          // the points are linear
            else if(determinant > 0) {return 1;}      // point3 is on the right side of (point1,point2)
            else {return -1;}                         // point3 is on the left side of (point1,point2)
        }

        static List<double[]> findHull(List<double[]> points)
        {
            if (points.Count < 3) {return points;}
            double[] first = points[0];
            foreach(double[] p in points)
            {
                if (p[0] < first[0]) {first = p;}
                else if (p[0] == first[0] && p[1] > first[1]) {first = p;}
            }
            List<double[]> hull = new List<double[]> {first};

            double[] nextOne = new double[] {0d,0d};
            while (hull.Count == 1 || (hull.Count > 1 && hull[^1] != hull[0]))
            {
                List<double[]> candidates = new List<double[]>{};
                foreach (double[] p in points)
                { 
                    if (p != hull[^1] && points.All(r => cross(hull[^1],p,r) >= 0))
                        {candidates.Add(p);}
                }
                double dist = -1;   
                foreach (double[] c in candidates)
                {
                    if (dist == -1) 
                    {
                        nextOne = c;
                        dist = distance(c,hull[^1]);
                    }
                    else if (dist < distance(c,hull[^1]))
                    {
                        nextOne = c;
                        dist = distance(c,hull[^1]);
                    }
                }        
                hull.Add(nextOne);
            }
            hull.RemoveAt(hull.Count - 1);
            return hull;
        }
    }
        
}
