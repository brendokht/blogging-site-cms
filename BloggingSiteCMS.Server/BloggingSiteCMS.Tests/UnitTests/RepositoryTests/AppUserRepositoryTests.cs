using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BloggingSiteCMS.DAL.Domain;

using Microsoft.AspNetCore.Identity;

using Xunit;

namespace BloggingSiteCMS.Tests.UnitTests.RepositoryTests
{
    public class AppUserRepositoryTests
    {
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepositoryTests()
        {

        }
        /*
         * Rather than using the custom-made repository, we will use the 
         * generic UserManager to do the CRUD and other necessary 
         * operations onto the users 
         */
        [Fact]
        public void CreateUserTest()
        {

        }
    }
}