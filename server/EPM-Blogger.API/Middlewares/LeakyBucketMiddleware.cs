namespace EPM_Blogger.API.Middlewares
{
    public class LeakyBucketMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly Queue<DateTime> _requestQueue = new();
        private static readonly object _lock = new();

        private readonly int _capacity = 10;         
        private readonly TimeSpan _leakInterval = TimeSpan.FromSeconds(10); // process 1 request per second
        private DateTime _lastLeakTime = DateTime.UtcNow;

        public LeakyBucketMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            lock (_lock)
            {
                Leak(); // process queue at fixed rate

                if (_requestQueue.Count < _capacity)
                {
                    // add new request timestamp to bucket
                    _requestQueue.Enqueue(DateTime.UtcNow);
                }
                else
                {
                    context.Response.StatusCode = 429;
                    context.Response.WriteAsync("Too Many Requests, Try again later");
                    return;
                }
            }

            await _next(context);
        }
        private void Leak()
        {
            var now = DateTime.UtcNow;

            // process requests at fixed interval
            while (_requestQueue.Count > 0 && now - _lastLeakTime >= _leakInterval)
            {
                _requestQueue.Dequeue();  // leak one request
                _lastLeakTime = _lastLeakTime.Add(_leakInterval);
            }
        }
    }
    public static class LeakyBucketMiddlewareExtension
    {
        public static IApplicationBuilder UseLeakyBucketMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LeakyBucketMiddleware>();
        }
    }
}
