using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenAuthor.Entities;
using AuthenAuthor.Requests;
using AutoMapper;

namespace AuthenAuthor.Profiles
{
    public class AuthProfiles : Profile
    {
        public AuthProfiles()
        {
            CreateMap<User, AddUser>().ReverseMap();

        }
    }
}