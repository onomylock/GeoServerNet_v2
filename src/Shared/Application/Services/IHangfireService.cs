using System.Linq.Expressions;
using Shared.Common.Enums;

namespace Shared.Application.Services;

public interface IHangfireService
{
    string AddEnque(Expression<Action> methodCall);

    string AddEnque<T>(Expression<Action<T>> methodCall);

    string AddContinuations(Expression<Action> methodCall, string jobid);

    string AddContinuations<T>(Expression<Action<T>> methodCall, string jobid);

    string AddSchedule(Expression<Action> methodCall, double time);

    string AddSchedule<T>(Expression<Action<T>> methodCall, RecuringTime recuringTime, double time);
}