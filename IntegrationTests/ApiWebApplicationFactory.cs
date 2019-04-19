using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<API.Startup>
    {

    }
}
