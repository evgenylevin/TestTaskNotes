using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestProfiGroup.Web.Code
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer container;

        public WindsorControllerFactory()
        {
            container = ContainerFactory.Current();
        }

        protected override IController GetControllerInstance(
            RequestContext requestContext,
            Type controllerType)
        {
            return (IController)container.Resolve(controllerType);
        }
    }
}