using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelf
{
    public class GetOrderBatchService : TimerServiceBase, IGetOrderBatchService
    {
        private ILog _logger = LogManager.GetLogger(typeof(GetOrderBatchService));

        public GetOrderBatchService(double interval) : base(interval)
        {

        }
        protected override void DoExecute()
        {
            _logger.Info("记录日志.....执行代码");
        }
    }
}
