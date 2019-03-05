using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public class CategoryInput : WebObjectInputBase<Category>, ICategory
    {
        [Required]
        public string Name { get; set; }

        public CategoryInput()
            : base()
        {

        }

        public CategoryInput(
            string webKey)
            : base(webKey)
        {

        }

        public CategoryInput(
            string webKey,
            Category category)
            : base(webKey, category)
        {
            UpdateProperties(category, this);
        }

        protected override void UpdateProperties(
            Category category)
        {
            UpdateProperties(this, category);
        }

        private static void UpdateProperties(
            ICategory source,
            ICategory destination)
        {
            destination.Name = source.Name;
        }
    }
}
