//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MenuManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Ingredient
    {
        public Ingredient()
        {
            this.MenuItems = new HashSet<MenuItem>();
        }

        [Required]
        public int ID { get; set; }

        [Required,
         StringLength(20, ErrorMessage = Helpers.TOO_LONG)]
        public string Name { get; set; }

        [Required,
         Display(Name="Stock")]
        public Nullable<int> Amount { get; set; }

        [Required,
         Display(Name = "Unit"),
         StringLength(20, ErrorMessage = Helpers.TOO_LONG)]
        public string Amount_Unit { get; set; }

        [Display(Name="Low Threshold")]
        public Nullable<int> Amount_Low { get; set; }

        [Display(Name="Date Restocked"),
         DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date_Restocked { get; set; }

        [Display(Name = "Shelf Life (Days)")]
        public Nullable<int> Shelf_Life { get; set; }

        [Display(Name = "Age (Days)")]
        public Nullable<int> Age
        {
            get
            {
                if (Shelf_Life == null)
                    return null;
                return (DateTime.Now - (DateTime)Date_Restocked).Days;
            }
        }

        [Required,
         Display(Name="Store ID")]
        public int Store_ID { get; set; }
    
        public virtual Store Store { get; set; }

        [Display(Name="Menu Items")]
        public virtual ICollection<MenuItem> MenuItems { get; set; }

        public ICollection<Menu> Menus
        {
            get
            {
                List<Menu> list = new List<Menu>();
                foreach (MenuItem mi in MenuItems)
                {
                    if (!list.Contains(mi.Menu))
                        list.Add(mi.Menu);
                }
                return list;
            }
        }

        public string IsLowYesNo(bool blankIfNo = false)
        {
            if (Amount <= Amount_Low)
                return "Yes";
            else
                return (blankIfNo ? string.Empty : "No");
        }

        public string IsOldYesNo(bool blankIfNo = false)
        {
            if (Age != null && Age >= Shelf_Life)
                return "Yes";
            else
                return (blankIfNo ? string.Empty : "No");
        }
    }
}