using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IChatMessageService
    {
        IResult GetByDate(DateTime from, DateTime to);
        IResult GetByDate(DateTime date);
        IResult GetTodaysMessages();
        IResult Add(ChatMessage chatMessage);
    }
}
