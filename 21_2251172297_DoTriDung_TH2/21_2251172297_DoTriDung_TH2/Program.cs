using System.Collections.Concurrent;

class Program
{
    static int N = 200;
    static int a = 4;
    static int b = 2;
    static ConcurrentQueue<int> A = new ConcurrentQueue<int>();
    static SemaphoreSlim semaphoreWrite = new SemaphoreSlim(1,1);
    static SemaphoreSlim semaphoreRead = new SemaphoreSlim(1, 1);
    static Random rnd = new Random();
    static void Main(string[] args)
    {
        List<Thread> threads = new List<Thread>();
        for(int i = 0; i < a; i++)
        {
            int threadIndex = i;
            Thread t = new Thread(() => SinhDuLieu(threadIndex));
            threads.Add(t);
            t.Start();
        }
        Thread.Sleep(20000);
        Environment.Exit(0);

    }
    static void SinhDuLieu(int d)
    {
        while (true)
        {
            Thread.Sleep(rnd.Next(200,1000));
            int value = rnd.Next(1,1000);
            semaphoreWrite.Wait();
            try
            {
                if(A.Count < N)
                {
                    A.Enqueue(value);
                    Console.WriteLine($"P{d}: {value} - {DateTime.Now:HH:mm}");
                }
            }
            finally
            {
                semaphoreWrite.Release();
            }
        }
    }
    static void XuLyDuLieu(int d)
    {
        while (true)
        {
            Thread.Sleep(rnd.Next(300, 10000));
            semaphoreRead.Wait();
            try
            {
                if(A.TryDequeue(out int value))
                {
                    int result = Xuly(value);
                    Console.WriteLine($"C{d}:{value} - Ket qua xu ly: {result} - {DateTime.Now:HH:mm}");
                }
            }
            finally
            {
                semaphoreRead.Release();
            }
        }
    }
    static int Xuly(int x)
    {
        return x * x;
    }
}