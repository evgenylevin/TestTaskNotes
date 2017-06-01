using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestProfiGroup.BusinessLogic;
using TestProfiGroup.DataAccess;

namespace TestProfiGroup.Web.Code
{
    public class ContainerFactory
    {
        private static IWindsorContainer container;
        private static readonly object SyncObject = new object();

        public static IWindsorContainer Current()
        {
            if (container == null)
            {
                lock (SyncObject)
                {
                    if (container == null)
                    {
                        container = new WindsorContainer();
                        container.Install(new DataAccessInstaller());
                        container.Install(new BusinessLogicInstaller());
                        container.Install(new ControllerInstaller());
                    }
                }
            }
            return container;
        }
    }
}