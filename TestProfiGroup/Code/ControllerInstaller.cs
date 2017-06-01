using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TestProfiGroup.Web.Controllers;

namespace TestProfiGroup.Web.Code
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<NotesController>()
                    .ImplementedBy<NotesController>().LifeStyle.PerWebRequest);
        }
    }
}