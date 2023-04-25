using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1,1000,ErrorMessage ="Range must be beteen 1 to 1000")]
        public int Count { get; set; }
        public string ApplcationUserId { get; set; }
        [ForeignKey(nameof (ApplcationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
