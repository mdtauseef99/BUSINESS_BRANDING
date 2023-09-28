using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class CountryDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<CountryModel> GetAllCountry()
        {
            try
            {
                return objdb.Countries.Where(x => x.IsActive == true).Select(x => new CountryModel
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                    //CountryCode = x.CountryCode,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CountryModel> GetAllCountry(int skip, int take, int cid)
        {
            try
            {
                return objdb.Countries.Where(x => x.CountryId == cid).Select(x => new CountryModel
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                    //CountryCode = x.CountryCode,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CountryId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CountryModel> GetAllCountry(int skip, int take)
        {
            try
            {
                return objdb.Countries.Where(x => x.CountryId != null).Select(x => new CountryModel
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                    //CountryCode = x.CountryCode,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CountryId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCountry(CountryModel objmodel)
        {
            try
            {

                if (objmodel.CountryId == 0)
                {
                    Country objcountry = new Country
                    {
                        CountryName = objmodel.CountryName,
                        //CountryCode = objmodel.CountryCode,
                        CountryId = objmodel.CountryId,
                        //CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.Countries.Add(objcountry);
                    objdb.SaveChanges();
                    return objcountry.CountryId;
                }
                else
                {
                    var objcountry = objdb.Countries.Find(objmodel.CountryId);
                    objcountry.CountryName = objmodel.CountryName;
                    objcountry.CountryId = objmodel.CountryId;
                    //objcountry.CountryCode = objmodel.CountryCode;
                    objcountry.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.CountryId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CountryModel GetCountryById(int id)
        {
            try
            {
                return objdb.Countries.Where(x => x.CountryId == id).Select(x => new CountryModel
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                    //CountryCode = x.CountryCode,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).SingleOrDefault();
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
                return objdb.Countries.Where(x => x.CountryId != null)
                            .Select(x => x.CountryId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CountryModel> GetCountryByParent(int id)
        {
            try
            {
                return objdb.Countries.Where(x => x.CountryId == id).Select(x => new CountryModel
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                var obj = objdb.Countries.Find(id);
                if (obj != null && obj.IsActive == true)
                {
                    obj.IsActive = false;
                    objdb.SaveChanges();
                    return false;
                }
                else if (obj != null)
                {
                    obj.IsActive = true;
                    objdb.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }



    }
}
