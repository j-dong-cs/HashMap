using System;
using HashMap;

namespace TestMap
{
    class TestMap
    {
        static void Main(string[] args)
        {
            PrintIntro();
            HashMap<int, string> testMap = new HashMap<int, string>();
            TestAdd(testMap);
            TestContainsKey(testMap);
            TestKeySet(testMap);
            TestTryGetValue(testMap);
            TestRemove(testMap);
            TestRehash(testMap);
            TestKeySet(testMap);
            TestClear(testMap);
            TestKeySet(testMap);
        }

        private static void PrintIntro()
        {
            Console.WriteLine("HashMap Test Program #1:");
            Console.WriteLine("The exact order of your map may differ from the expected output, but you should have the same");
            Console.WriteLine("overall collection of key/value pairs.");
            Console.WriteLine();
        }

        private static void TestAdd(HashMap<int, string> map)
        {
            Console.WriteLine("Test Add(TKey): ");
            foreach (int n in new int[] { 42, 29, 42, 17, 112, -9, 17, 82, 53 })
            {
                map.Add(n, "boo" + n);
                Console.Write("after adding {0,3}, map = {1}, count = {2}", n, map, map.Count);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void TestContainsKey(HashMap<int, string> map)
        {
            Console.WriteLine("Test ContainsKey(TKey): ");
            foreach (int n in new int[] { 42, 29, 42, 17, 112, -9, 17, 82, 53, 55, 182, -91, 888, 72, 33, 999, -17 })
            {
                Console.Write("{0} ContainsKey({1,3})? {2}", map, n, map.ContainsKey(n));
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void TestKeySet(HashMap<int, string> map)
        {
            Console.WriteLine("Test Keys:");
            Console.Write("Keys = {");
            foreach (int i in map.Keys)
            {
                Console.Write(" {0}", i);
            }
            Console.WriteLine(" }");
        }

        private static void TestTryGetValue(HashMap<int, string> map)
        {
            Console.WriteLine("Test TryGetValue(TKey, TValue): ");
            foreach (int n in new int[] { 42, 29, 42, 17, 112, -9, 17, 82, 53, 55, 182, -91, 888, 72, 33, 999, -17 })
            {
                Console.Write("{0} TryGetValue({1,3}) -> {3}) ? {2}", map, n, map.TryGetValue(n, out string value), value);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void TestRemove(HashMap<int, string> map)
        {
            Console.WriteLine("Test Remove(TKey):");
            foreach (int n in new int[] { 55, 42, 182, 17, -91, 82 })
            {
                map.Remove(n);
                Console.WriteLine("after removing {0,3}, map = {1}, size = {2}, ContainsKey({3})? {4}", n, map, map.Count, n, map.ContainsKey(n));
            }
            Console.WriteLine();
            // test Add after Remove
            int newValue = 22;
            map.Add(newValue, "foo");
            Console.WriteLine("after adding {0,3}, map = {1}, size = {2}, ContainsKey({3})? {4}", newValue, map, map.Count, newValue, map.ContainsKey(newValue));
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void TestRehash(HashMap<int, string> map)
        {
            Console.WriteLine("Test Add with many elements:");
            foreach (int n in new int[] { 33, 56, 22, 19, 41, 104, -2, 97, 97, 22, 984, -797, 66, 33, 90210, 44444 })
            {
                map.Add(n, "foo"+  n);
                Console.WriteLine("after adding {0,5}, map = {1}, size = {2}, ContainsKey({3})? {4}", n, map, map.Count, n, map.ContainsKey(n));
            }
            Console.WriteLine();
        }

        private static void TestClear(HashMap<int, string> map)
        {
            Console.WriteLine("Test Clear:");
            map.Clear();
            Console.WriteLine("after clear #1, map = {0}, size = {1}, IsEmpty = {2}", map, map.Count, map.IsEmpty());
            map.Clear();
            Console.WriteLine("after clear #2, map = {0}, size = {1}, IsEmpty = {2}", map, map.Count, map.IsEmpty());
        }
    }
}
