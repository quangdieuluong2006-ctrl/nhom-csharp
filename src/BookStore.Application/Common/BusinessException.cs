namespace BookStore.Application.Common;

/// <summary>Lỗi vi phạm quy tắc nghiệp vụ; nội dung được hiển thị nguyên văn cho người dùng.</summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}
