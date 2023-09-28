using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;
namespace BizzBranding.BLL
{
    public class AdministratorBLL
    {
        AdministratorDAL objdal = new AdministratorDAL();

        public List<AdministratorModel> GetAllAdministrators()
        {
            try
            {
                return objdal.GetAllAdministrators();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //public List<InstructorModel> GetAllInstructorsWithImages()
        //{
        //    try
        //    {
        //        return objdal.GetAllInstructorsWithImages();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public AdministratorModel GetAdministratorById(int id)
        {
            try
            {
                return objdal.GetAdministratorById(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AdministratorModel AdminLogin(AdministratorModel objmodel)
        {
            try
            {
                return objdal.AdminLogin(objmodel);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddUpdateAdministrator(AdministratorModel objmodel)
        {
            try
            {
                return objdal.AddUpdateAdministrator(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<AdministratorModel> GetAllAdministrators(int skip, int take)
        {
            try
            {
                return objdal.GetAllAdministrators(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int GetPageCount()
        {
            try
            {
                return objdal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                return objdal.ChangeStatus(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
