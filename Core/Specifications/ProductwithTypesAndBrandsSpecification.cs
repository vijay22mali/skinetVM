
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductwithTypesAndBrandsSpecification: BaseSpecification<Product>
    {
        
        public ProductwithTypesAndBrandsSpecification(ProductSpecParams productParams)
            :base(x =>
                    (string.IsNullOrEmpty(productParams.Search)|| x.Name.ToLower().Contains(productParams.Search))&&
                    (!productParams.BrandId.HasValue||x.ProductBrandId==productParams.BrandId) &&
                    (!productParams.TypeId.HasValue||x.ProductTypeId==productParams.TypeId)
                )
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
            AddOrdrBy(x=>x.Name);
            ApplyPaging(productParams.PageSize*(productParams.PageIndex-1),productParams.PageSize);

            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                switch(productParams.Sort)
                {
                    case "priceASC":
                        AddOrdrBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        AddOrdrByDescending(p=>p.Price);
                        break;
                    default:
                        AddOrdrBy(n=>n.Name);
                        break;
                }
            }
        }

        public ProductwithTypesAndBrandsSpecification(int id)
            : base(x=>x.Id==id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }
    }
}