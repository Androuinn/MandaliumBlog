using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.Core.Models
{
    public class SystemSetting : BaseEntityWithId
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}