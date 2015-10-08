using Microsoft.Azure.WebJobs;

namespace KeyRotationJob
{
    class Program
    {
        static void Main()
        {
            var host = new JobHost();
            host.Call(typeof(Functions).GetMethod("RegenerateKey"));
        }
    }
}
