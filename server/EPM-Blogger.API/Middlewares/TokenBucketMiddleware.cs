namespace EPM_Blogger.API.Middlewares
{
    public class TokenBucketMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Dictionary<string, TokenBucket> _buckets = new();
        private readonly int _capacity = 5;        // max 5 requests
        private readonly double _refillRate = 5.0 / 60.0; // ~0.083 tokens per second

        public TokenBucketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string key = context.Connection.RemoteIpAddress?.ToString() ?? "global";
            if (!_buckets.ContainsKey(key))
            {
                _buckets[key] = new TokenBucket(_capacity, _refillRate);
            }

            if (_buckets[key].GrantAccess())
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too Many Requests.Please wait");
            }
        }
    }

    internal class TokenBucket
    {
        private readonly int _capacity;
        private readonly double _refillRate; // tokens per second
        private double _tokens;
        private DateTime _lastRefill;

        public TokenBucket(int capacity, double refillRate)
        {
            _capacity = capacity;
            _refillRate = refillRate;
            _tokens = capacity; // start full
            _lastRefill = DateTime.UtcNow;
        }

        public bool GrantAccess()
        {
            Refill();
            if (_tokens >= 1)
            {
                _tokens -= 1;
                return true;
            }
            return false;
        }


        // This refills based on the time difference
        private void Refill()
        {
            var now = DateTime.UtcNow;
            var seconds = (now - _lastRefill).TotalSeconds;
            var refillTokens = seconds * _refillRate;
            _tokens = Math.Min(_capacity, _tokens + refillTokens);
            _lastRefill = now;
        }
    }
}
