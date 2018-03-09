using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class NotebookGPUViewModel
    {
        public List<Notebook> notebooksList;
        public SelectList GPUs;
        public string notebookGPU { get; set; }
    }
}
