﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public partial record SupplierProductSearchModel : BaseSearchModel
{
    #region Properties

    public string PurchaseOrderNo { get; set; }

    #endregion
}
