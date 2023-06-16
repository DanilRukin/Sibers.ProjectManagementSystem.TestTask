using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Sibers.ProjectManagementSystem.Presentation.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sibers.ProjectManagementSystem.Application;

namespace Sibers.ProjectManagementSystem.UnitTests.AutoMapper
{
    public class ProfileTests
    {
        [Fact]
        public void WhenProfilesAreConfigured_ItShouldNotThrowException()
        {
            // Arrange
            var config = new MapperConfiguration(configuration =>
            {
                configuration.EnableEnumMappingValidation();

                configuration.AddMaps(typeof(Marker).Assembly, typeof(ApplicationModule).Assembly);
            });

            // Assert
            config.AssertConfigurationIsValid();
        }
    }
}
