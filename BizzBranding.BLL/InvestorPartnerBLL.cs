using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class InvestorPartnerBLL
    {
        InvestorPartnerDAL Objdal = new InvestorPartnerDAL();
        public int AddEditInvestorPartnering(InvestorPartneringModel objmodel)
        {
            try
            {
                return Objdal.AddEditInvestorPartnering(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<InvestorPartneringModel> GetAllInvestorPartnerList(int skip, int take)
        {
            try
            {
                return Objdal.GetAllInvestorPartnerList(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<InvestorPartneringModel> GetAllInvestorPartnerList()
        {
            try
            {
                return Objdal.GetAllInvestorPartnerList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<InvestorPartneringModel> GetAllInvestorPartnerList(int skip, int take, int cid)
        {
            try
            {
                return Objdal.GetAllInvestorPartnerList(skip, take, cid);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public InvestorPartneringModel GetInvestorPartnerDetailsById(int id)
        {
            try
            {
                return Objdal.GetInvestorPartnerDetailsById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<InvestorPartneringModel> GetInvestorPartnerDetailsByUserId(int id)
        {
            try
            {
                return Objdal.GetInvestorPartnerDetailsByUserId(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        

        public bool ChangeInvestorPartnerStatus(int id)
        {
            try
            {
                return Objdal.ChangeInvestorPartnerStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool ChangeInvestorPartnerApprovalStatus(int id)
        {
            try
            {
                return Objdal.ChangeInvestorPartnerApprovalStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public int Remove(int id)
        {
            try
            {
                return Objdal.Remove(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCount()
        {
            try
            {
                return Objdal.GetPageCount();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCountByUserId(int id)
        {
            try
            {
                return Objdal.GetPageCountByUserId(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
