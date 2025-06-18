using Prometheus;

namespace API.Middlewares
{
    public class PrometheusMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Counter _requestCounter;
        private readonly Histogram _requestDuration;
        private readonly Gauge _activeRequests;
        private readonly Counter _errorCounter;

        public PrometheusMiddleware(RequestDelegate next)
        {
            _next = next;

            _requestCounter = Metrics
                .CreateCounter("usermanagement_http_requests_total",
                "Total HTTP requests", ["method", "endpoint", "status_code"]);

            _requestDuration = Metrics
                .CreateHistogram("usermanagement_http_request_duration_seconds",
                "HTTP request duration", ["method", "endpoint"]);

            _activeRequests = Metrics
                .CreateGauge("usermanagement_http_requests_active",
                "Active HTTP requests");

            _errorCounter = Metrics
                .CreateCounter("usermanagement_http_errors_total",
                "Total HTTP errors", ["method", "endpoint", "error_type"]);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var endpoint = GetNormalizedPath(context.Request.Path.Value);

            _activeRequests.Inc();

            using var timer = _requestDuration.WithLabels(method, endpoint).NewTimer();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _errorCounter.WithLabels(method, endpoint, ex.GetType().Name).Inc();
                throw;
            }
            finally
            {
                var statusCode = context.Response.StatusCode.ToString();
                _requestCounter.WithLabels(method, endpoint, statusCode).Inc();
                _activeRequests.Dec();
            }
        }

        private static string GetNormalizedPath(string? path)
        {
            if (string.IsNullOrEmpty(path)) return "unknown";

            var normalizedPath = path.ToLowerInvariant();

            normalizedPath = System.Text.RegularExpressions.Regex.Replace(
                normalizedPath, @"/\d+", "/{id}");

            return normalizedPath;
        }
    }
}
