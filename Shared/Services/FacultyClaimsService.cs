using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class FacultyClaimsService
    {
        private readonly UserManager<APIUser> _userManager;
        private readonly IConfiguration _configuration;
        public List<string> facultyNumbers = new List<string>();
        public List<Claim> facultiesClaims = new List<Claim>();

        public FacultyClaimsService(UserManager<APIUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public void LoadFaculties(ClaimsPrincipal user)
        {
            facultiesClaims = user.Claims.Where(c => c.Type == "Faculty").ToList();

            if (facultiesClaims.Count > 0)
            {
                foreach (var claim in facultiesClaims)
                {
                    facultyNumbers.Add(claim.Value.Split("-")[0]);
                }
            }

        }
    }

    }
