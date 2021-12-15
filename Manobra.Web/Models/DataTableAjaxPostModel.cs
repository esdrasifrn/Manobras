using System.Collections.Generic;

namespace Manobra.Web.Models
{
    public class DataTableAjaxPostModel
    {
        public int draw { get; set; }
        public List<DataTableAjaxPostModelColumn> columns { get; set; }

        // paginação
        public int start { get; set; }
        public int length { get; set; }

        // busca e ordenação            
        public DataTableAjaxPostModelSearch search { get; set; }
        public List<DataTableAjaxPostModelOrder> order { get; set; }
    }

    public class DataTableAjaxPostModelColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public DataTableAjaxPostModelSearch search { get; set; }
    }

    public class DataTableAjaxPostModelSearch
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class DataTableAjaxPostModelOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}