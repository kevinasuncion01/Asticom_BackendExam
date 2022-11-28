using Asticom_BackendExam.Context;
using Asticom_BackendExam.Helper;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asticom_BackendExam_UnitTest
{
    public class BaseTests
    {
        protected AsticomContext BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AsticomContext>()
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var dbContext = new AsticomContext(options);

            return dbContext;
        }

        protected IMapper BuildMap()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfile());
            });

            return config.CreateMapper();
        }
    }
}
