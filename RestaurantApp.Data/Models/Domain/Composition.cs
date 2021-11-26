using RestaurantApp.Data.Abstraction;
using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class Composition : TEntity<int>
    {
        [DisplayName("Ингридиенты")]
        public string Ingredient { get; set; }
        [DisplayName("Калорийность")]
        public int CalorificValue { get; set; }

        public virtual List<MenuItemsCompositions> MenuItemsCompositions { get; set; }
    }
}
