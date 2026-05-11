namespace Persistence.Base.Models;

public record class RequestParameters(int PageNumber = 1, int PageSize = 10);