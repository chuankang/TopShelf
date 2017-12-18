using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TopShelf
{
    public abstract class TimerServiceBase : IAppService
    {
        private readonly System.Timers.Timer _timer;
        protected readonly ILog Logger = LogManager.GetLogger(typeof(TimerServiceBase));

        protected TimerServiceBase(double interval)
        {
            _timer = new Timer(interval) { AutoReset = true };
            _timer.Elapsed += _timer_Elapsed;
        }

        protected abstract void DoExecute();
        //protected abstract Task DoExecute();//异步
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Stop();
            try
            {
                DoExecute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                Start();
            }
        }


        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
