using LMSystem.Models.ViewModels;
using LMSystem.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LMSystem.Services.ReportingServices
{
    public class OverDueBookService : IOverDueBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OverDueBookService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IList<OverDueBookViewModel> OverDueBooks(string FromDate,String ToDate, string Bookid)
        {
            if (Bookid is not null && Bookid!="Select Book")
            {
                var overDueBook = (from i in _unitOfWork.IssueBookRepository.GetAll()
                                     join m in _unitOfWork.MemberRepository.GetAll()
                                     on i.Memberid equals m.id
                                     join b in _unitOfWork.BookRepository.GetAll()
                                     on i.Bookid equals b.id

                                     where !i.IsInActive && !m.IsInActive && !b.IsInActive && DateTime.Parse(FromDate) <=i.IssueDate 
                                       && DateTime.Parse(ToDate)>=i.IssueDate  && i.Bookid == Bookid && DateTime.Now>i.DueDate
                                     select new OverDueBookViewModel
                                     {
                                         IssueDate = i.IssueDate.ToString("yyyy-MM-dd"),
                                         DueDate = i.DueDate.ToString("yyyy-MM-dd"),
                                         BookInfo = b.Title,
                                         MemberInfo = m.Name,

                                     }).ToList();   
                              return overDueBook;

            }
            else
            {
                var overDueBook = (from i in _unitOfWork.IssueBookRepository.GetAll()
                                   join m in _unitOfWork.MemberRepository.GetAll()
                                   on i.Memberid equals m.id
                                   join b in _unitOfWork.BookRepository.GetAll()
                                   on i.Bookid equals b.id

                                   where !i.IsInActive && !m.IsInActive && !b.IsInActive && DateTime.Parse(FromDate) <= i.IssueDate
                                      && DateTime.Parse(ToDate) >= i.IssueDate  && DateTime.Now > i.DueDate

                                   select new OverDueBookViewModel
                                   {
                                       IssueDate = i.IssueDate.ToString("yyyy-MM-dd"),
                                       DueDate = i.DueDate.ToString("yyyy-MM-dd"),
                                       BookInfo = b.Title,
                                       MemberInfo = m.Name,

                                   }).ToList();
                return overDueBook;


            }

        }
    }
}
