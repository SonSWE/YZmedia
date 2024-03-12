using CommonLib;
using DataAccess;
using ObjectInfo;

namespace YZmediaService.Helper
{
    public class RunBackgroundService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(CacheMemoryData, cancellationToken); //Load memory: 5 min

            Task.Run(Auto_Train_Image, cancellationToken);

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

        public Task Auto_Train_Image()
        {
            while (true)
            {
                try
                {
                    List<YZ_Post_Info> listPostUntrained = YZ_Post_DA.GetInstance().GetListPostUntrained();

                    string url = Config_Info.FlaskApiAddress;

                    foreach (var post in listPostUntrained)
                    {

                    }


                    //lấy danh sách bài viết status là đang xử lý dữ liệu

                    //lấy top vài trăm file có status là chưa train theo bài viết lên gọi api train xong thì cập nhập trạng thái file

                    //xong thì cập nhật trạng thái bài viết

                }
                catch (Exception ex)
                {
                    Logger.log.Error(ex.ToString());
                }
                Thread.Sleep(5000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
