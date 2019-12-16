//using System;
//using System.Collections.Generic;
//using NUnit.Framework;
//using Tools.Extensions;
//using Tools.Tests;

//namespace Tools
//{
//    public class Grid
//    {
//        public Cell[,] Cells { get; set; }

//        public Grid(int rows, int columns)
//        {
//            Cells = new Cell[rows, columns];

//            for (int x = 0; x < rows; x++)
//            {
//                for (int y = 0; y < columns; y++)
//                {
//                    var cell = new Cell();
//                    cell.X = x;
//                    cell.Y = y;
//                    cell.IsWall = false;

//                    Cells[x, y] = cell;
//                }
//            }
//        }
//    }

//    public class Cell
//    {
//        public int X { get; set; }

//        public int Y { get; set; }

//        public bool IsWall { get; set; }
//    }
//}

//namespace Tools.Tests
//{
//    public class Tests
//    {
//        [SetUp]
//        public void Setup()
//        {
//        }

//        [Test]
//        public void Test1()
//        {
//            var grid = new Grid(9, 9);
//            grid.Cells[3, 4].IsWall = true;
//            grid.Cells[4, 4].IsWall = true;
//            grid.Cells[5, 4].IsWall = true;
//            Console.WriteLine(grid.Draw());
//        }

////        [Test]
////        public void Draw2_Test1()
////        {
////            var grid = new Grid(1, 1);

////            var str = grid.Draw2();
////            Console.WriteLine(str);

////            Assert.That(str, Is.EqualTo(@"╔═══╗
////║   ║
////╚═══╝"));
////        }

////        [Test]
////        public void Draw2_Test2()
////        {
////            var grid = new Grid(1, 2);

////            var str = grid.Draw2();
////            Console.WriteLine(str);

////            Assert.That(str, Is.EqualTo(@"╔═══╦═══╗
////║   ║   ║
////╚═══╩═══╝"));
////        }

////        [Test]
////        public void Draw2_Test3()
////        {
////            var grid = new Grid(2, 2);

////            var str = grid.Draw2();
////            Console.WriteLine(str);

////            Assert.That(str, Is.EqualTo(@"╔═══╦═══╗
////║   ║   ║
////╠═══╬═══╣
////║   ║   ║
////╚═══╩═══╝"));
////        }

////        [Test]
////        public void Draw2_Test4()
////        {
////            var grid = new Grid(3, 3);

////            var str = grid.Draw2();
////            Console.WriteLine(str);

////            Assert.That(str, Is.EqualTo(@"╔═══╦═══╦═══╗
////║   ║   ║   ║
////╠═══╬═══╬═══╣
////║   ║   ║   ║
////╠═══╬═══╬═══╣
////║   ║   ║   ║
////╚═══╩═══╩═══╝"));
////        }
//    }
//}

//namespace Tools.Extensions
//{
//    public static class Extensions
//    {
//        public static string Draw(this Grid grid)
//        {
//            var xL = grid.Cells.GetLength(0);
//            var yL = grid.Cells.GetLength(1);

//            var str = "";

//            for (int x = 0; x < xL; x++)
//            {
//                for (int y = 0; y < yL; y++)
//                {
//                    str += grid.Cells[x, y].IsWall ? "w" : "·";
//                }

//                str += $"\r\n";
//            }

//            return str;
//        }

//        //public static string Draw2(this Grid grid)
//        //{
//        //    const string s = "═║╔╗╚╝╣╦╩╬╠";

//        //    var xL = grid.Cells.GetLength(0);
//        //    var yL = grid.Cells.GetLength(1);

//        //    var str = $"";

//        //    for (int x = 0; x < xL; x++)
//        //    {
//        //        for (int y = 0; y < yL; y++)
//        //        {
//        //            str += grid.Cells[x, y].IsWall ? "w" : "·";
//        //        }

//        //        str += $"\r\n";
//        //    }

//        //    return str;
//        //}
//    }
//}