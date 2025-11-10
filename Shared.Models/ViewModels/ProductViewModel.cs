using Shared.Models.Attributes;
using Shared.Models.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Shared.ViewModels
{
    public class ProductViewModel
    {
        [DisplayName("LaunchDate")]
        [FormControl(FormControlType.Datetime)]
        public DateTime LaunchDate { get; init; }
        [DisplayName("Product name")]
        [FormControl(FormControlType.Text)]
        public required string Name { get; init; }

        [DisplayName("Product description")]
        [FormControl(FormControlType.Textarea)]
        public required string Description { get; init; }

        [DisplayName("Product type")]
        [FormControl(FormControlType.DropDownSingleSelect)]
        public required ProductType Type { get; set; }

        [DisplayName("Product price")]
        [Range(0, 10)]
        [FormControl(FormControlType.Numeric)]
        public required decimal Price { get; set; }

        [DisplayName("Is Product available")]
        [FormControl(FormControlType.Checkbox)]
        public bool IsAvailable { get; set; }

        [DisplayName("Has subscription")]
        [FormControl(FormControlType.Checkbox)]
        public bool? HasSubscription { get; set; }
    }
}
