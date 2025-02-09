using System.Linq.Expressions;
using Hangfire;
using Shared.Application.Services;
using Shared.Common.Enums;

namespace Shared.Common.Services;

public class HangfireService(IBackgroundJobClient backgroundJobClient) : IHangfireService
{
    public string AddEnque(Expression<Action> methodCall)
    {
        return backgroundJobClient.Enqueue(methodCall);
    }
    
    public string AddEnque<T>(Expression<Action<T>> methodCall)
    {
        return backgroundJobClient.Enqueue(methodCall);
    }
    
    public string AddContinuations(Expression<Action> methodCall, string jobid)
    {
        return backgroundJobClient.ContinueJobWith(jobid, methodCall);
    }
    
    public string AddContinuations<T>(Expression<Action<T>> methodCall, string jobid)
    {
        return backgroundJobClient.ContinueJobWith(jobid, methodCall);
    }

    public string AddSchedule(Expression<Action> methodCall, double time)
    {
        throw new NotImplementedException();
    }

    public string AddSchedule<T>(Expression<Action<T>> methodCall, RecuringTime recuringTime, double time)
    {
        throw new NotImplementedException();
    }
}