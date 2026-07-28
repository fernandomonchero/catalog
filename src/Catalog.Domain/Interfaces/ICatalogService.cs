using Catalog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Interfaces
{
    public interface ICatalogService
    {
        Task Import(List<Product> products);
    }
}