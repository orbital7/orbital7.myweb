using Orbital7.Extensions.Attributes;
using Orbital7.MyWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class _DialogAddGroupModel
    {
        [Required]
        public string WebKey { get; set; }

        [RequiredGuid]
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public static _DialogAddGroupModel Create(
            string webKey,
            Guid categoryId,
            string name)
        {
            return new _DialogAddGroupModel()
            {
                WebKey = webKey,
                CategoryId = categoryId,
                Name = name,
            };
        }

        public Group ToGroup()
        {
            return new Group(this.Name);
        }
    }
}
