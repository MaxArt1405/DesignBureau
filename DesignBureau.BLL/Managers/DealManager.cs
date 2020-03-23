using DesignBureau.DAL.Services;
using DesignBureau.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignBureau.BLL.Managers
{
    public class DealManager
    {
        public Deal MakeDeal(Deal deal)
        {
            if (deal != null)
            {
                var dal = new BaseDalService<Deal>();
                return dal.Add(deal);
            }
            return deal;
        }
        public void DropDeal(Deal deal)
        {
            if(deal != null)
            {
                var dal = new BaseDalService<Deal>();
                dal.Delete(deal);
            }
        }
    }
}
