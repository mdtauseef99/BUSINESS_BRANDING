using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
    public class SearcbyKeywordBLL
    {
        SearcbyKeywordDAL objsearchdal = new SearcbyKeywordDAL();

        public SearchwordsModel Search(string txtval)
        {
            try
            {
                return objsearchdal.Search(txtval);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SearchwordsModel SearchByAccessPlan(string txt)
        {
            try
            {
                return objsearchdal.SearchByAccessPlan(txt);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SearchwordsModel SearchbyIndustry(int IndId)
        {
            try
            {
                return objsearchdal.SearchbyIndustry(IndId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string IndustrySearch(int IndId)
        {
            try
            {
                return objsearchdal.IndustrySearch(IndId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CitySearch(int CityId)
        {
            try
            {
                return objsearchdal.CitySearch(CityId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SearchwordsModel SearchCity(int CityId)
        {
            try
            {
                return objsearchdal.SearchCity(CityId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SearchwordsModel SearchMemberByProduct(string txtval)
        {
            try
            {
                return objsearchdal.SearchMemberByProduct(txtval);
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
