using System.Dynamic;

namespace Crynsole
{
    class garbage
    {
        public static implicit operator int(garbage g) => 0;
    }
    class trash { public static implicit operator garbage(trash g) => g; }

    class A
    {

        public static A operator +(A other) { throw new NotImplementedException(); }
    }
    class B : A { }


    internal class Program
    {

        //static void eh<T>() where T : A
        //{
        //    int x = 0;
        //    {
        //        int x = 100;
        //    }

        //    A bob = new A() + new A(); //cry
        //    checked
        //    {
        //        int z = int.MaxValue + 1;
        //    }

        //    var a = 2 - "a";
        //    int b = /*(garbage)(*/new trash();
        //    a += 1;
        //    int b = a.iowjrief;
        //    5.ToString();
        //    ExpandoObject a2 = new ExpandoObject();
        //    a = "Nikita".ToString();

        //    int c = a.Length;
        //    int x = () => 3;
        //}
        static void Main(string[] args)
        {
            bool a = false;
            bool b = true;
            bool c = a & b == b ^ a;
        }
    }
}