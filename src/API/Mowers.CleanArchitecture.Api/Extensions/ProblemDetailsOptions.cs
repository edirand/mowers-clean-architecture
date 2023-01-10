using Hellang.Middleware.ProblemDetails;
using Mowers.CleanArchitecture.Application.Exceptions;

namespace Mowers.CleanArchitecture.Api.Extensions;

/// <summary>
/// Options to configure problem details.
/// </summary>
public static class ProblemDetailsOptions
{
    /// <summary>
    /// Configures problem details.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
    /// <returns>The configured instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection ConfigureProblemDetails(this IServiceCollection services)
    {
        return services
            .AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (context, exception) => false;
                options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);

                options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });
    }
}