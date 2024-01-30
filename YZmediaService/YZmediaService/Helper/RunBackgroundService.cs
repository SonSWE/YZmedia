using CommonLib;

namespace YZmediaService.Helper
{
    public class RunBackgroundService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(CacheMemoryData, cancellationToken); //Load memory: 5 min

            return Task.CompletedTask;
        }

        public Task CacheMemoryData()
        {
            while (true)
            {
                try
                {
                    MemoryData.LoadMemory();
                }
                catch (Exception ex)
                {
                    Logger.log.Error(ex.ToString());
                }
                Thread.Sleep(5 * 60 * 1000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
