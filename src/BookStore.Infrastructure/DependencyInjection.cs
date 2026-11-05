using BookStore.Application.Common;
using BookStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Đăng ký DbContext dùng chung cho Web và Desktop.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<BookStoreDbContext>(o => o.UseSqlServer(connectionString,
            sql => sql.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null)));
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<BookStoreDbContext>());
        return services;
    }
}
