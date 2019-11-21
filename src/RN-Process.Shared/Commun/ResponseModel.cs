using System.Collections;
using System.Collections.Generic;

namespace RN_Process.Shared.Commun
{
    public class ResponseModel<T>
    {
        public T Entity;
        public Hashtable Errors;
        public bool IsAuthenicated;
        public int PageSize;
        public long TotalPages;
        public long TotalRows;

        public ResponseModel()
        {
            ReturnMessage = new List<string>();
            ReturnStatus = true;
            Errors = new Hashtable();
            TotalPages = 0;
            TotalPages = 0;
            PageSize = 0;
            IsAuthenicated = false;
        }

        public string Token { get; set; }
        public bool ReturnStatus { get; set; }
        public List<string> ReturnMessage { get; set; }
    }
}