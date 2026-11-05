using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Desktop.HaTang;

/// <summary>
/// Tạo form qua DI. Mỗi form có một scope riêng ⇒ một DbContext riêng, được giải phóng khi form đóng.
/// (DbContext không an toàn khi dùng lâu dài/đa luồng, nên không dùng chung một DbContext cho cả ứng dụng.)
/// </summary>
public interface IFormFactory
{
    T Tao<T>() where T : Form;
}

public class FormFactory : IFormFactory
{
    private readonly IServiceProvider _root;
    public FormFactory(IServiceProvider root) => _root = root;

    public T Tao<T>() where T : Form
    {
        var scope = _root.CreateScope();
        try
        {
            var form = scope.ServiceProvider.GetRequiredService<T>();
            form.Disposed += (_, _) => scope.Dispose();
            return form;
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
}
