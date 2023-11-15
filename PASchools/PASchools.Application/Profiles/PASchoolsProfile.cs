using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PASchools.Application.DTOs;
using PASchools.Domain.Entities;
using PASchools.Google.Connector.Models.Requests;

namespace PASchools.Application.Profiles
{
    public class PASchoolsProfile : Profile
    {
        public PASchoolsProfile()
        {
            CreateMap<School, SchoolDTO>()
         .ReverseMap();
            CreateMap<Address, AddressDTO>()
         .ReverseMap();
        }

    }
}
