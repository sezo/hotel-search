namespace HotelSearch.Domain.Queries;

public record HotelSearchQuery(double Longitude, double Latitude, int? Page = 1, int? PageSize = 10);