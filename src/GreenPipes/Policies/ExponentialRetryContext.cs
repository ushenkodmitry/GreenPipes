// Copyright 2012-2018 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace GreenPipes.Policies
{
    using System;
    using System.Threading;


    public class ExponentialRetryContext<TContext> :
        BaseRetryContext<TContext>,
        RetryContext<TContext>
        where TContext : class, PipeContext
    {
        readonly ExponentialRetryPolicy _policy;

        public ExponentialRetryContext(ExponentialRetryPolicy policy, TContext context, Exception exception, int retryCount, CancellationToken cancellationToken)
            : base(context, exception, retryCount, cancellationToken)
        {
            _policy = policy;
        }

        public override TimeSpan? Delay => _policy.Intervals[RetryCount];

        bool RetryContext<TContext>.CanRetry(Exception exception, out RetryContext<TContext> retryContext)
        {
            retryContext = new ExponentialRetryContext<TContext>(_policy, Context, Exception, RetryCount + 1, CancellationToken);

            return RetryAttempt < _policy.Intervals.Length && _policy.IsHandled(exception);
        }
    }
}