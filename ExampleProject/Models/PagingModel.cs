using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Models
{
    public class PagingModel<T>
    {
        public PagingModel(string filter, int count, int currentPage, List<T> element)
        {
            Filter = filter;
            Count = count;
            CurrentPage = currentPage;
            Elements = element;
        }

        public string Filter { get; set; }

        public List<T> Elements { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int Count { get; set; }

        public int PageSize { get; set; } = 10;

        public int TotalPages => (Count + PageSize - 1) / PageSize;

        public bool EnablePrevious => CurrentPage > 1;

        public bool EnableNext => CurrentPage < TotalPages;
    }
}
