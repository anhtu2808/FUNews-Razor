﻿using AutoMapper;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.Mapping
{
    public class AccountProfile : Profile
    {
        public AccountProfile() 
        {
            CreateMap<SystemAccount, SystemAccountResponse>();
        } 
    }
}
