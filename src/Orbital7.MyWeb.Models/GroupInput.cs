using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public class GroupInput : WebObjectInputBase<Group>, IGroup
    {
        [RequiredGuid]
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public GroupInput()
            : base()
        {

        }

        public GroupInput(
            string webKey,
            Guid categoryId)
            : base(webKey)
        {
            this.CategoryId = categoryId;
        }

        public GroupInput(
            string webKey,
            Group group)
            : base(webKey, group)
        {
            this.CategoryId = group.Category.Id;
            UpdateProperties(group, this);
        }

        protected override void UpdateProperties(
            Group group)
        {
            UpdateProperties(this, group);
        }

        private static void UpdateProperties(
            IGroup source,
            IGroup destination)
        {
            destination.Name = source.Name;
        }
    }
}
