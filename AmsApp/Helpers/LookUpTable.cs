using AmsApp.Dto;
using AmsApp.Data;
using AmsApp.Models;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace AmsApp.Helpers
{

    public static class LookUpTable
    {
        public static List<ListItem> GetCCDispositions(AMSContext context)
        {
            return context.CCDispositions.AsNoTracking().Where(p => p.Status == 1).OrderBy(o => o.Id)
                                                        .Select(c => new ListItem(c.Id,  c.Title)).ToList();
        }
        public static List<ListItem> GetGender(AMSContext context)
        {
            return context.SqlQuery<ListItem>("select Id, Title from Genders where Status =1");
        }
        public static List<ListItem> GetClinicBranchs(AMSContext context)
        {
            return context.SqlQuery<ListItem>("select Id, Name as Title from Branch where Status =1 and BranchType=1 Order BY Name");
        }
        public static List<ListItem> GetMainDiseases(AMSContext context)
        {
            return context.SqlQuery<ListItem>("select Id, Title from PmsDisease where Status =1 Order BY Title");
        }
        public static List<ListItemWithParent> GetSubDiseases(AMSContext context)
        {
            return context.SqlQuery<ListItemWithParent>("select Id, Title, Disease as ParentId from PmsSubDisease where Status =1 Order BY Title");
        }
        public static List<ListItem> GetCities(AMSContext context)
        {
            return context.SqlQuery<ListItem>("select Id, Name as Title from Cities where Status =1 Order BY Name");
        }
        public static List<ListItem> GetStates(AMSContext context)
        {
            return context.SqlQuery<ListItem>("select Id, Name as Title from States where Status =1 Order BY Name");
        }
        public static List<ListItem> GetCountries(AMSContext context)
        {
            return context.SqlQuery<ListItem>("select Id, Name as Title from Countries where Status =1 Order BY Name");
        }
    }
}
