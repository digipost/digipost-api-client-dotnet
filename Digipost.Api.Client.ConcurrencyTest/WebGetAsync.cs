using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.ConcurrencyTest
{
    internal class WebGetAsync
    {
        private readonly int _defaultConnectionLimit;
        private readonly int _numberOfRequests;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly object _syncLock = new object();
        private int _failedCalls;
        private int _itemsLeft;
        private int _successfulCalls;

        public WebGetAsync(int numberOfRequests, int defaultConnectionLimit)
        {
            _defaultConnectionLimit = defaultConnectionLimit;
            _itemsLeft = numberOfRequests;
            _numberOfRequests = numberOfRequests;
        }

        private void DisplayTestResults()
        {
            _stopwatch.Stop();

            Console.WriteLine("Success:" + _successfulCalls + " , Failed:" + _failedCalls + ", Duration:" +
                              _stopwatch.ElapsedMilliseconds + " Avg:" +
                              (_stopwatch.Elapsed.Seconds == 0
                                  ? _successfulCalls
                                  : _successfulCalls/_stopwatch.Elapsed.Seconds) + " req/sec");
        }

        public void TestParallel()
        {
            ServicePointManager.DefaultConnectionLimit = _defaultConnectionLimit;
            var httpClient = new HttpClient();

            for (var i = 0; i < _numberOfRequests; i++)
            {
                Task.Run(() => { ProcessUrlAsync(httpClient); });
            }
        }

        public void TestAsync()
        {
            ServicePointManager.DefaultConnectionLimit = _defaultConnectionLimit;
            var httpClient = new HttpClient();

            for (var i = 0; i < _numberOfRequests; i++)
            {
                ProcessUrlAsync(httpClient);
            }
        }

        private async void ProcessUrlAsync(HttpClient httpClient)
        {
            HttpResponseMessage httpResponse = null;

            try
            {
                //string URL = "http://vg.no";
                const string url = "http://10.16.0.125:3000/";
                //Console.WriteLine("AsyncGet to " + URL);
                var getTask = httpClient.GetAsync(url);
                httpResponse = await getTask.ConfigureAwait(false);

                Interlocked.Increment(ref _successfulCalls);
                //Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failedCalls);
                //Console.WriteLine("Failed");
            }
            finally
            {
                if (httpResponse != null) httpResponse.Dispose();
            }

            lock (_syncLock)
            {
                _itemsLeft--;
                if (_itemsLeft == 0)
                {
                    DisplayTestResults();
                }
            }
        }
    }
}