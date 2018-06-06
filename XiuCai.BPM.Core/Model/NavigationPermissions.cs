using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace XiuCai.BPM.Core.Model
{
    public class NavigationPermissions
    {
        [Key]
        public int KeyId{get;set;}
        public int NavId{get;set;}
        public int PermissionID{get;set;}
    }
}
