namespace UserApi.Services
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } // نشان‌دهنده موفقیت یا شکست
        public string Message { get; set; } // پیام مربوط به وضعیت
        public string? ErrorMessage { get; set; } // اضافه کردن این مقدار
        public T Data { get; set; } // داده‌ای که به درخواست بر می‌گردد (اگر موجود باشد)
    }
}
