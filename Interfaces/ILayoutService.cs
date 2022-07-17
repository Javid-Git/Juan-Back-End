using Juan.ViewModels.BasketViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Interfaces
{
    public interface ILayoutService
    {
        Task<List<BasketVM>> GetBasket();
        Task<IDictionary<string, string>> GetSetting();
        
    }
}
