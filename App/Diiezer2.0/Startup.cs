﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Diiezer2._0.Startup))]
namespace Diiezer2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
