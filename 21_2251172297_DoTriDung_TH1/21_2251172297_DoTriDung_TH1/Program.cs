using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int[] A;
    static int[] maxResults;
    static int threadCount = 4;

    static void Main(string[] args)
    {
        int N = 120;
        A = new int[N];
        maxResults = new int[threadCount];
        SinhMangNgauNhien(N);
        InMang();

        Console.WriteLine("\n--- Bat dau xu ly da luong ---\n");
        XuLyDaLuong(N);

        int ketQuaCuoi = TimMaxCuoiCung();
        Console.WriteLine($"\n==> Gia tri lon nhat trong mang: {ketQuaCuoi}");
    }


    static void SinhMangNgauNhien(int N)
    {
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            A[i] = rand.Next(1, 1000); 
        }
    }

    static void InMang()
    {
        Console.WriteLine("Mang A:");
        Console.WriteLine(string.Join(",", A));
    }

    static void XuLyDaLuong(int N)
    {
        int chunkSize = N / threadCount;
        List<Thread> threads = new List<Thread>();

        for (int i = 0; i < threadCount; i++)
        {
            int threadIndex = i;
            int start = threadIndex * chunkSize;
            int end = (threadIndex == threadCount - 1) ? N : start + chunkSize;

            Thread t = new Thread(() => XuLyDoan(threadIndex, start, end));
            threads.Add(t);
            t.Start();
        }

        
        foreach (var t in threads)
            t.Join();
    }

    static void XuLyDoan(int threadIndex, int start, int end)
    {
        int localMax = A[start];
        for (int j = start + 1; j < end; j++)
        {
            if (A[j] > localMax)
                localMax = A[j];
        }
        maxResults[threadIndex] = localMax;

        Console.WriteLine($"T{threadIndex + 1}: {localMax} - {DateTime.Now:HH:mm}");
        
    }

    static int TimMaxCuoiCung()
    {
        int max = maxResults[0];
        for (int i = 1; i < maxResults.Length; i++)
        {
            if (maxResults[i] > max)
                max = maxResults[i];
        }
        return max;
    }
}
