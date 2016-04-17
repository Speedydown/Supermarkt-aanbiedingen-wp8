using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarktCore.Model
{
    public interface SupermarktItem
    {
        string URL { get; set; }
        string ImageURL { get; set; }
    }
}
