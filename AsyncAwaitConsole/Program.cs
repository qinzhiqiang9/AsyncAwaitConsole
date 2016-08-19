using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitConsole
{
    class Program
    {
        static SemaphoreSlim hore = new SemaphoreSlim(5);

        static void Main(string[] args)
        {
            //SemaphoreTest();
            //ThreadExceptionTest();
            TaskExceptionThrowTest();
        }

      


        #region Task capture exception example

        public static void TaskExceptionThrowTest()
        {
            try
            {
                var task = Task.Run(() => { GoTask(); });
                //task.Wait();  // 在调用了这句话之后，主线程才能捕获task里面的异常

                // 对于有返回值的Task, 我们接收了它的返回值就不需要再调用Wait方法了
                // GetName 里面的异常我们也可以捕获到
                var task2 = Task.Run(() => { return GetName(); });
                var name = task2.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception!");
            }
        }
        static void GoTask() { throw null; }
        static string GetName() { throw null; }

        #endregion

        #region  Get one conclusion, main thread can not capture the exception from sub thread

        public static void ThreadExceptionTest()
        {
            try
            {
                new Thread(Go).Start();
            }
            catch (Exception ex)
            {
                // 其它线程里面的异常，我们这里面是捕获不到的。
                Console.WriteLine("Exception!");
            }
        }
        static void Go() { throw null; }

        #endregion

        #region SemaphoreSlim Test

        static void SemaphoreTest()
        {
            for (int i = 0; i <= 10; i++)
            {
                new Thread(Enter).Start(i);
            }

        }

        static void Enter(object obj)
        {
            Console.WriteLine("The number {0} enter in...",obj);
            hore.Wait();
            Console.WriteLine("Start process for the number {0}...", obj);
            Thread.Sleep(5000);
            Console.WriteLine("Finished process for the number {0}...", obj);
            hore.Release();
        }

        #endregion



        static void TestThread()
        {
            new Thread(Method).Start();
        }

        static void TestTask()
        {
            Task.Factory.StartNew(Method);

            Task.Run(new Action(Method));
        }

        static void TestThreadPool()
        {
            ThreadPool.QueueUserWorkItem(Method1);
        }

        static void Method()
        {
            Console.WriteLine("You have called me");
        }


        static void Method1(object data)
        {
            Console.WriteLine("You have called me");
        }
    }
}
