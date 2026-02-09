using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

using Malwine.Extensions;

namespace Malwine.Selectors;

[AttributeUsage(AttributeTargets.Method)]
public class OverloadSelectorAttribute : ActionMethodSelectorAttribute
{
  public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
  {
    var request = routeContext.HttpContext.Request;
    var parameters = action.Parameters.Where(parameter =>
      parameter.BindingInfo!.BindingSource == BindingSource.Query
    );

    return Enumerable.Zip(request.Query, parameters)
                     .All((query, parameter) => query.Key == parameter.Name);
  }
}