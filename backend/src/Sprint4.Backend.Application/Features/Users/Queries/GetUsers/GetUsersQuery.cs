using MediatR;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery : IRequest<IEnumerable<User>>;
}