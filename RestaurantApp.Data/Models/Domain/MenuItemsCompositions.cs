using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Data.Models.Domain
{
    public class MenuItemsCompositions
    {
        public int MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }

        public int CompositionId { get; set; }

        public virtual Composition Composition { get; set; }
    }
}
