using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using MicroActive.Security.AuthorizationServer.Data;
using MicroActive.Security.AuthorizationServer.Web.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using MicroActive.Security.AuthorizationServer.Data.Models;

namespace MicroActive.Security.AuthorizationServer.Web
{
	public class AutomapperConfig : Profile
	{
		//public static void AddAutoMapper(this IServiceCollection services)
		//{
		//	services.AddAutoMapper(DependencyContext.Default);
		//}

		//public static void AddAutoMapper(this IServiceCollection services, DependencyContext dependencyContext)
		//{
		//	services.AddAutoMapper(dependencyContext.RuntimeLibraries.SelectMany(lib => lib.GetDefaultAssemblyNames(dependencyContext).Select(Assembly.Load)));
		//}

		public AutomapperConfig()
		{
			RecognizePrefixes("AspNet");
			RecognizeDestinationPrefixes("AspNet");

			//ReverseMap just maps src => dest, as well as dest => src
			CreateMap<AspNetRoleClaim, RoleActionModel>().ReverseMap();
			CreateMap<AspNetRole, RoleModel>().AfterMap((src, dest) =>
			{
				dest.Actions = Mapper.Map<List<AspNetRoleClaim>, List<RoleActionModel>>(src.AspNetRoleClaims);
			}).ReverseMap().AfterMap((src, dest) =>
			{
				dest.AspNetRoleClaims = Mapper.Map<List<RoleActionModel>, List<AspNetRoleClaim>>(src.Actions);

			});
			CreateMap<AspNetUser, UserModel>().ReverseMap();
			CreateMap<AspNetRoleClaim, ActionModel>().ReverseMap();
			CreateMap<Client, ClientModel>().AfterMap((src, dest) =>
			{
				dest.ClientClaims = Mapper.Map<List<ClientClaim>, List<ActionModel>>(src.ClientClaims);
			}).ReverseMap().AfterMap((src, dest) =>
			{
				dest.ClientClaims = Mapper.Map<List<ActionModel>, List<ClientClaim>>(src.ClientClaims);

			});
			CreateMap<ApiScope, ScopeModel>().ReverseMap();

		}

			//.AfterMap((src, dest) => {
				 //dest.RoleActionId = src.Id;
			// });

			//config.AssertConfigurationIsValid();

				 //var mapper = config.CreateMapper();

				 //var subDest1 = mapper.Map<Source1, SubDest1>(new Source1 { SomeValue = "Value1" });
				 //var subDest2 = mapper.Map<Source2, SubDest2>(new Source2 { SomeOtherValue = "Value2" });

				 //subDest1.SomeValue.ShouldEqual("Value1");
				 //subDest2.SomeOtherValue.ShouldEqual("Value2");

	}
}