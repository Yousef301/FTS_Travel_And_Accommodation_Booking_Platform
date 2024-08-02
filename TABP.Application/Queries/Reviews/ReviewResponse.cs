namespace TABP.Application.Queries.Reviews;

public class ReviewResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Comment { get; set; }
    public double Rate { get; set; }
}