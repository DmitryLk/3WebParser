using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebParser.App
{
    public class SpaceObjectNameRequestDTO
    {
        [Required(ErrorMessage = "Space object name {0} is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Space object name lenght should be between 1 and 30")]
        public string SpaceObjectName { get; set; }
    }
}
