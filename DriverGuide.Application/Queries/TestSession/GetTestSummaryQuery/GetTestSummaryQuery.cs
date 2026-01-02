using DriverGuide.Domain.DTOs;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetTestSummaryQuery : IRequest<TestSummaryDto?>
{
    public required string TestSessionId { get; set; }
}
