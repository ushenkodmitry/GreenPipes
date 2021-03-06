﻿// Copyright 2012-2018 Chris Patterson
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
namespace GreenPipes.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Filters;


    public class ContextFilterPipeSpecification<TContext> :
        IPipeSpecification<TContext>
        where TContext : class, PipeContext
    {
        readonly Func<TContext, Task<bool>> _filter;

        public ContextFilterPipeSpecification(Func<TContext, Task<bool>> filter)
        {
            _filter = filter;
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new ContextFilter<TContext>(_filter));
        }

        public IEnumerable<ValidationResult> Validate()
        {
            if (_filter == null)
                yield return this.Failure("Filter", "must not be null");
        }
    }
}